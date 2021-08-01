using System;
using System.Threading.Tasks;
using Thesis.Services.Operations.Api.DTO;
using Thesis.Services.Operations.Api.Types;

namespace Thesis.Services.Operations.Api.Services
{
    public interface IOperationsService
    {
        event EventHandler<OperationUpdatedEventArgs> OperationUpdated;
        Task<OperationDto> GetAsync(string id);

        Task<(bool updated, OperationDto operation)> TrySetAsync(string id, string userId, string name,
            OperationState state, string code = null, string reason = null, dynamic data = null);
    }
}