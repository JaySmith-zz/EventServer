using System;

namespace EventServer.Core.Domain
{
    public class UserRegistered : IDomainEvent
    {
        public int UserId { get; set; }
        public bool RegisteredByAdmin { get; set; }
    }
}