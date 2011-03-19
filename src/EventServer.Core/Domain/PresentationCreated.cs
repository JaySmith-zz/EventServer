using System;

namespace EventServer.Core.Domain
{
    public class PresentationCreated : IDomainEvent
    {
        public int PresentationId { get; set; }
    }
}