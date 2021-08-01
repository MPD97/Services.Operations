using Convey.CQRS.Queries;
using Thesis.Services.Operations.Api.DTO;

namespace Thesis.Services.Operations.Api.Queries
{
    public class GetOperation : IQuery<OperationDto>
    {
        public string OperationId { get; set; }
    }
}