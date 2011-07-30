using StructureMap;
namespace EventServer.Mvc3 {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        //scan.TheCallingAssembly();
                                        scan.Assembly("EventServer.Mvc3");
                                        scan.Assembly("EventServer.Core");
                                        //scan.Assembly("EventServer.Infrastructure");
                                        scan.WithDefaultConventions();
                                    });
            //                x.For<IExample>().Use<Example>();
                        });
            return ObjectFactory.Container;
        }
    }
}