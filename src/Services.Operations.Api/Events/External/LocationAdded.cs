using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Thesis.Services.Operations.Api.Events.External
{
    [Message("localisations")]
    public class LocationAdded: IEvent
    {
        public Guid LocationId { get; }
        public Guid UserId { get; }  
        public DateTime CreatedAt { get; }
        public decimal Latitude { get; }
        public decimal Longitude { get; }
        public int  Accuracy { get; }

        public LocationAdded(Guid locationId, Guid userId, DateTime createdAt, decimal latitude, decimal longitude, int accuracy)
        {
            LocationId = locationId;
            UserId = userId;
            CreatedAt = createdAt;
            Latitude = latitude;
            Longitude = longitude;
            Accuracy = accuracy;
        }
    }
}