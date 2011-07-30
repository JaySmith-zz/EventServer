namespace EventServer.Core.Domain
{
    using System;
    using System.Web.Mvc;

    [Serializable]
    public abstract class Entity
    {
        private int id;
        private IEventAggregator events;

        public int Id
        {
            get { return this.id != 0 ? this.id : (this.id = IdGenerator.NextId()); }
            set { this.id = value; }
        }
        
        protected void Raise<T>(T domainEvent) where T : IDomainEvent
        {
            if (this.events == null)
            {
                this.events = DependencyResolver.Current.GetService<IEventAggregator>();
            }

            if (this.events != null)
            {
                this.events.Raise(domainEvent);
            }
        }
    }
}