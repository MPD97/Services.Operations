using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Thesis.Services.Operations.Api.Events.External
{
    [Message("runs")]
    public class RunCreated : IEvent
    {
        public Guid RunId { get; }
        public string Status { get; }

        public RunCreated(Guid runId, string status)
        {
            RunId = runId;
            Status = status;
        }
    }
}