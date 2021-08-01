using System.Text;
using Convey.MessageBrokers;
using Thesis.Services.Operations.Api.Types;

namespace Thesis.Services.Operations.Api.Handlers
{
    public static class Extensions
    {
        public static OperationState? GetSagaState(this IMessageProperties messageProperties)
        {
            const string sagaHeader = "Saga";
            if (messageProperties?.Headers is null || !messageProperties.Headers.TryGetValue(sagaHeader, out var saga))
            {
                return null;
            }

            return saga is byte[] sagaBytes
                ? Encoding.UTF8.GetString(sagaBytes).ToLowerInvariant() switch
                {
                    "pending" => OperationState.Pending,
                    "completed" => OperationState.Completed,
                    "rejected" => OperationState.Rejected,
                    _ => (OperationState?) null
                }
                : null;
        }
    }
}