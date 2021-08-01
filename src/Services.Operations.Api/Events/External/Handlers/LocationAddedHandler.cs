using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Thesis.Services.Operations.Api.Events.External.Handlers
{
    public class LocationAddedHandler: IEventHandler<LocationAdded>
    {
        public Task HandleAsync(LocationAdded @event)
        {
            return Task.CompletedTask;
        }
    }
}