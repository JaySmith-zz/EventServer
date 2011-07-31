namespace EventServer.Core.Domain
{
    public class SessionAccepted : IDomainEvent
    {
        public int SessionId { get; set; }
    }

    public class SessionChanged : IDomainEvent
    {
        public int SessionId { get; set; }
    }

    public class SessionRejected : IDomainEvent
    {
        public int SessionId { get; set; }
    }
}