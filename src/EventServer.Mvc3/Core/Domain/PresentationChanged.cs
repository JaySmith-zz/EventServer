using System;

namespace EventServer.Core.Domain
{
    public class PresentationChanged : IDomainEvent
    {
        public int PresentationId { get; set; }
    }
}