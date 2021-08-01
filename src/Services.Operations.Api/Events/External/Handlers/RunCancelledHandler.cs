using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Thesis.Services.Operations.Api.Events.External.Handlers
{
    public class RunCancelledHandler : IEventHandler<RunCancelled>
    {
        public Task HandleAsync(RunCancelled @event)
        {
            return Task.CompletedTask;
        }
    }
}