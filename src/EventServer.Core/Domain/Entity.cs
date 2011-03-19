using System;

namespace EventServer.Core.Domain
{
    [Serializable]
    public abstract class Entity
    {
        private int _id;
        public int Id
        {
            set { _id = value; }
            get { return _id != 0 ? _id : (_id = IdGenerator.NextId()); }
        }

        private IEventAggregator _events;
        protected void Raise<T>(T domainEvent) where T : IDomainEvent
        {
            if (_events == null)
                _events = Ioc.Resolve<IEventAggregator>();

            if (_events != null)
                _events.Raise(domainEvent);
        }
    }
}