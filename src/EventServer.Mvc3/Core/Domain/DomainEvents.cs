using System;

namespace EventServer.Core.Domain
{
    public interface IEventAggregator
    {
        void Raise<T>(T domainEvent) where T : IDomainEvent;
        void Register<T>(Action<T> callback) where T : IDomainEvent;
    }

    public interface IDomainEvent
    {
    }

    public interface IHandles<T> where T : IDomainEvent
    {
        void Handle(T domainEvent);
    }
}