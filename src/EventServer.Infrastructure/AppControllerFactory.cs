using System;
using System.Web.Mvc;
using System.Web.Routing;
using EventServer.Core;

namespace EventServer.Infrastructure
{
    public class AppControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                return controllerType == null ? base.GetControllerInstance(requestContext, controllerType) : (IController)Ioc.Resolve(controllerType);
            }
            catch
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }
        }
    }
}