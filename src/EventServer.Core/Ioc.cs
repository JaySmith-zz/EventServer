using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EventServer.Core
{
    public static class Ioc
    {
        public static void Initialize(IInversionOfControlContainer container)
        {
            _container = container;
        }

        private static IInversionOfControlContainer _container;

        public static T Resolve<T>()
        {
            return _container == null ? default(T) : _container.Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            return _container == null ? null : _container.Resolve(type);
        }

        public static IEnumerable<T> ResolveAll<T>()
        {
            return _container == null ? Enumerable.Empty<T>() : _container.ResolveAll<T>();
        }

        public static IList GetAllInstances(Type type)
        {
            return _container == null ? new ArrayList() : _container.ResolveAll(type);
        }
    }

    public interface IInversionOfControlContainer
    {
        T Resolve<T>();
        object Resolve(Type type);

        IEnumerable<T> ResolveAll<T>();
        IList ResolveAll(Type type);
    }
}