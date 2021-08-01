using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Thesis.Services.Operations.Api.Events.External
{
    [Message("runs")]
    public class RunPointCompleted: IEvent
    {
        public Guid PointId { get; }
        
        public RunPointCompleted( Guid pointId)
        {
            PointId = pointId;
        }
    }
}