using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EventServer.Core;
using EventServer.Core.Domain;
using EventServer.Core.Services;
using ExpressionHelper=Microsoft.Web.Mvc.Internal.ExpressionHelper;

namespace EventServer.UI.Controllers
{
    public abstract class AppController : Controller
    {
        protected AppController(IRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUser = currentUserService;
        }

        protected readonly IRepository _repository;
        protected readonly ICurrentUserService _currentUser;

        protected ActionResult RedirectTo<TController>(Expression<Action<TController>> action) where TController : Controller
        {
            return RedirectToRoute(ExpressionHelper.GetRouteValuesFromExpression(action));
        }

        protected string GetUrl<TController>(Expression<Action<TController>> action) where TController : Controller
        {
            return GetUrl(ExpressionHelper.GetRouteValuesFromExpression(action));
        }

        protected string GetUrl(RouteValueDictionary routeValues)
        {
            return UrlHelper.GenerateUrl("", null, null, routeValues, RouteTable.Routes, ControllerContext.RequestContext, false);
        }

        protected UserProfile GetUser(int id, bool performOwnerCheck)
        {
            var user = _repository.Get<UserProfile>(id);

            if (user == null)
                throw new HttpException(404, "User {0} not found".F(id));

            if (performOwnerCheck && !_currentUser.Is(user) && !_currentUser.IsAdmin)
                throw new ApplicationException("You are not {0}".F(id));

            return user;
        }

        protected Presentation GetSession(int id, bool performOwnerCheck)
        {
            var session = _repository.Get<Presentation>(id);

            if (session == null)
                throw new HttpException(404, "Session {0} not found".F(id));

            if (performOwnerCheck && !_currentUser.Owns(session) && !_currentUser.IsAdmin)
                throw new ApplicationException("You do not own session {0}".F(id));

            return session;
        }
    }
}