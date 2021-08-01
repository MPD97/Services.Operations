using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Thesis.Services.Operations.Api.Events.External.Handlers
{
    public class RunCreatedHandler : IEventHandler<RunCreated>
    {
        public Task HandleAsync(RunCreated @event)
        {
            return Task.CompletedTask;
        }
    }
}