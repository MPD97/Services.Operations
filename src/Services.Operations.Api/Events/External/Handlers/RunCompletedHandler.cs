using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Thesis.Services.Operations.Api.Events.External.Handlers
{
    public class RunCompletedHandler : IEventHandler<RunCompleted>
    {
        public Task HandleAsync(RunCompleted @event)
        {
            return Task.CompletedTask;
        }
    }
}