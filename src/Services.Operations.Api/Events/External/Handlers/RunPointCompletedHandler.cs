using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Thesis.Services.Operations.Api.Events.External.Handlers
{
    public class RunPointCompletedHandler : IEventHandler<RunPointCompleted>
    {
        public Task HandleAsync(RunPointCompleted @event)
        {
            return Task.CompletedTask;
        }
    }
}