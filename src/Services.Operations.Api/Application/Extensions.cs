using Convey;
using Convey.CQRS.Events;

namespace Thesis.Services.Operations.Api.Application
{
    public static class Extensions
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder)
            => builder
                .AddEventHandlers()
                .AddInMemoryEventDispatcher();
    }
}