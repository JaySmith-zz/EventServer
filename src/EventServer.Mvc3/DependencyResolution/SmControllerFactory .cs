namespace EventServer
{
    using System;
    using System.Web.Mvc;

    using StructureMap;

    public class SmControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new ArgumentNullException("controllerType");
            }

            return ObjectFactory.GetInstance(controllerType) as Controller;
            
            // return base.GetControllerInstance(requestContext, controllerType);
        }
    } 
}