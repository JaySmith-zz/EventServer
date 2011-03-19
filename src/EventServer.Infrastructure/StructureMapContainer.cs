using System;
using System.Collections;
using System.Collections.Generic;
using EventServer.Core;
using EventServer.Core.Domain;
using EventServer.Core.Services;
using EventServer.Infrastructure.Repositories;
using EventServer.Infrastructure.Services;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.TypeRules;

namespace EventServer.Infrastructure
{
    public class StructureMapContainer : IInversionOfControlContainer
    {
        public StructureMapContainer()
        {
            _container = new Container(x =>
            {
                x.For<IRepository>().Use<XmlRepository>();
                x.For<ITwitterService>().Use<TweetSharpTwitterService>();
                x.For<ISyndicationService>().Use<SyndicationService>();

                x.ForSingletonOf<IMailGateway>()
                    .Use<MailGateway>()
                    .Ctor<string>()
                    .EqualToAppSetting("developerEmail");

                x.Scan(scanner =>
                {
                    scanner.Assembly("EventServer.Core");
                    scanner.Assembly("EventServer.Infrastructure");
                    scanner.Assembly("EventServer.UI");

                    scanner.WithDefaultConventions();
                    scanner.Convention<IHandlesConvention>();
                });
            });
        }

        private readonly Container _container;

        public T Resolve<T>()
        {
            return _container.GetInstance<T>();
        }

        public object Resolve(Type type)
        {
            return _container.GetInstance(type);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return _container.GetAllInstances<T>();
        }

        public IList ResolveAll(Type type)
        {
            return _container.GetAllInstances(type);
        }
    }

    public class IHandlesConvention : IRegistrationConvention
    {
        private static readonly Type _openIHandlesType = typeof(IHandles<>);

        public void Process(Type type, Registry registry)
        {
            if (!type.IsConcrete())
                return;

            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == _openIHandlesType)
                    registry.AddType(@interface, type);
            }
        }
    }
}