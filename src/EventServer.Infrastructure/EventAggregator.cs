using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EventServer.Core;
using EventServer.Core.Domain;

namespace EventServer.Infrastructure
{
    public class EventAggregator : IEventAggregator
    {
        private static IList<Delegate> actions;

        public void Raise<T>(T domainEvent) where T : IDomainEvent
        {
            foreach (var handler in Ioc.ResolveAll<IHandles<T>>())
            {
                Action<T> action = e =>
                {
                    try
                    {
                        handler.Handle(e);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                };

                action.RunAsync(domainEvent, 60.Seconds());
            }

            foreach (var handler in actions.NullCheck().Where(x => x is Action<T>).Cast<Action<T>>())
            {
                Action<T> action = e =>
                {
                    try
                    {
                        handler(e);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                };

                action.RunAsync(domainEvent, 60.Seconds());
            }
        }

        public void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (actions == null)
                actions = new List<Delegate>();

            actions.Add(callback);
        }
    }
}