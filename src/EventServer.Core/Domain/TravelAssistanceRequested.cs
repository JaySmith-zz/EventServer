using System;

namespace EventServer.Core.Domain
{
    public class TravelAssistanceRequested : IDomainEvent
    {
        public int UserId { get; set; }
    }

    public class TravelAssistanceCancelled : IDomainEvent
    {
        public int UserId { get; set; }
    }
}