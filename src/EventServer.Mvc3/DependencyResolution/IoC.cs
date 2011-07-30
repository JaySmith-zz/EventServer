using StructureMap;
namespace EventServer {
    using EventServer.Core;
    using EventServer.Core.Services;
    using EventServer.Infrastructure;
    using EventServer.Infrastructure.Repositories;
    using EventServer.Infrastructure.Services;

    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.For<IRepository>().Use<XmlRepository>();
                            x.For<ITwitterService>().Use<TweetSharpTwitterService>();
                            x.For<ISyndicationService>().Use<SyndicationService>();

                            x.ForSingletonOf<IMailGateway>()
                                .Use<MailGateway>()
                                .Ctor<string>()
                                .EqualToAppSetting("developerEmail");

                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });
                        });

            return ObjectFactory.Container;
        }
    }
}