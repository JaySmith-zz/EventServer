using System.Web.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof(EventServer.App_Start.StructuremapMvc), "Start")]

namespace EventServer.App_Start 
{
    public static class StructuremapMvc 
    {
        public static void Start() 
        {
            var container = IoC.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }
    }
}