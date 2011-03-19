using System;

namespace EventServer.Core.Domain
{
    public class SpeakerCreated : IDomainEvent
    {
        public int UserId { get; set; }
    }
}