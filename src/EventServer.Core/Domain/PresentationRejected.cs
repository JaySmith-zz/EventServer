using System;

namespace EventServer.Core.Domain
{
    public class PresentationRejected : IDomainEvent
    {
        public int PresentationId { get; set; }
    }
}