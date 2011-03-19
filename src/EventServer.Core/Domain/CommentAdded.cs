using System;

namespace EventServer.Core.Domain
{
    public class CommentAdded : IDomainEvent
    {
        public int PresentationId { get; set; }
    }
}