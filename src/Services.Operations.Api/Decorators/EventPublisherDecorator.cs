using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.Types;
using Thesis.Services.Operations.Api.Handlers;
using Thesis.Services.Operations.Api.Infrastructure;
using Thesis.Services.Operations.Api.Services;
using Thesis.Services.Operations.Api.Types;

namespace Thesis.Services.Operations.Api.Decorators
{
    [Decorator]
    internal sealed class EventPublisherDecorator<TEvent> : IEventHandler<TEvent>
        where TEvent : class, IEvent
    {
        
        private readonly IEventHandler<TEvent> _handler;
        private readonly ICorrelationContextAccessor _contextAccessor;
        private readonly IMessagePropertiesAccessor _messagePropertiesAccessor;
        private readonly IOperationsService _operationsService;
        private readonly IHubService _hubService;

        public EventPublisherDecorator(IEventHandler<TEvent> handler, ICorrelationContextAccessor contextAccessor,
            IMessagePropertiesAccessor messagePropertiesAccessor,
            IOperationsService operationsService, IHubService hubService)
        {
            _handler = handler;
            _contextAccessor = contextAccessor;
            _messagePropertiesAccessor = messagePropertiesAccessor;
            _operationsService = operationsService;
            _hubService = hubService;
        }


        public async Task HandleAsync(TEvent @event)
        {
            await _handler.HandleAsync(@event);
            
            var messageProperties = _messagePropertiesAccessor.MessageProperties;
            var correlationId = messageProperties?.CorrelationId;
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                return;
            }

            var context = _contextAccessor.GetCorrelationContext();
            var name = string.IsNullOrWhiteSpace(context?.Name) ? @event.GetType().Name : context.Name;
            var userId = context?.User?.Id;
            var state = messageProperties.GetSagaState() ?? OperationState.Completed;
            var data = @event;
            
            
            var (updated, operation) = await _operationsService.TrySetAsync(correlationId, userId, name, state, data: data);
            if (!updated)
            {
                return;
            }

            switch (state)
            {
                case OperationState.Pending:
                    await _hubService.PublishOperationPendingAsync(operation);
                    break;
                case OperationState.Completed:
                    await _hubService.PublishOperationCompletedAsync(operation);
                    break;
                case OperationState.Rejected:
                    await _hubService.PublishOperationRejectedAsync(operation);
                    break;
                default:
                    throw new ArgumentException($"Invalid operation state: {state}", nameof(state));
            }
        }
    }
}