using System;
using Thesis.Services.Operations.Api.DTO;

namespace Thesis.Services.Operations.Api.Services
{
    public class OperationUpdatedEventArgs : EventArgs
    {
        public OperationDto Operation { get; }

        public OperationUpdatedEventArgs(OperationDto operation)
        {
            Operation = operation;
        }
    }
}