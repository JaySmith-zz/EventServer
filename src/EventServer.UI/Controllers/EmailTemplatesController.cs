using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using EventServer.Core;
using EventServer.Core.Services;

namespace EventServer.UI.Controllers
{
    [HandleError]
    public class EmailTemplatesController : AppController
    {
        public EmailTemplatesController(IRepository repository, ICurrentUserService currentUser) : base(repository, currentUser)
        {
        }

        protected override void HandleUnknownAction(string actionName)
        {
            Content("<p>Email template '{0}' not found</p>".F(actionName)).ExecuteResult(ControllerContext);
        }

        public ActionResult Index()
        {
            var templates = GetType()
                .GetMethods()
                .Where(x => x.ReturnType == typeof(ActionResult))
                .Select(x =>
                {
                    var routeValues = new RouteValueDictionary(ControllerContext.RouteData.Values);
                    routeValues["controller"] = "EmailTemplates";
                    routeValues["action"] = x.Name;
                    foreach (var key in Request.QueryString.AllKeys)
                        routeValues.Add(key, Request.QueryString[key]);
                    return GetUrl(routeValues);
                })
                .ToArray();

            return View(templates);
        }

        public ActionResult WelcomeNewUser(int id)
        {
            return View(GetUser(id, false));
        }

        public ActionResult UserRegistered(int id)
        {
            return View(GetUser(id, false));
        }

        public ActionResult SpeakerCreated(int id)
        {
            return View(GetUser(id, false));
        }

        public ActionResult TravelAssistanceRequested(int id)
        {
            return View(GetUser(id, false));
        }

        public ActionResult TravelAssistanceCancelled(int id)
        {
            return View(GetUser(id, false));
        }

        public ActionResult PresentationCreated(int id)
        {
            return View(GetSession(id, false));
        }

        public ActionResult PresentationChanged(int id)
        {
            return View(GetSession(id, false));
        }

        public ActionResult PresentationAccepted(int id)
        {
            return View(GetSession(id, false));
        }

        public ActionResult PresentationRejected(int id)
        {
            return View(GetSession(id, false));
        }

        public ActionResult CommentAdded(int id)
        {
            return View(GetSession(id, false));
        }
    }
}