using System;

namespace EventServer.Core.Domain
{
    public class SessionCreated : IDomainEvent
    {
        public int PresentationId { get; set; }
    }
}