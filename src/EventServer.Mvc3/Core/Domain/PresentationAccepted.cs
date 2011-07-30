using System;

namespace EventServer.Core.Domain
{
    public class PresentationAccepted : IDomainEvent
    {
        public int PresentationId { get; set; }
    }
}