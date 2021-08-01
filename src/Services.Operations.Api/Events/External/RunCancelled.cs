using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Thesis.Services.Operations.Api.Events.External
{
    [Message("runs")]
    public class RunCancelled : IEvent
    {
        public Guid RunId { get; }

        public RunCancelled(Guid runId)
        {
            RunId = runId;
        }
    }
}