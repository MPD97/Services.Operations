using System.Threading.Tasks;
using Thesis.Services.Operations.Api.DTO;

namespace Thesis.Services.Operations.Api.Services
{
    public interface IHubService
    {
        Task PublishOperationPendingAsync(OperationDto operation);
        Task PublishOperationCompletedAsync(OperationDto operation);
        Task PublishOperationRejectedAsync(OperationDto operation);
    }
}