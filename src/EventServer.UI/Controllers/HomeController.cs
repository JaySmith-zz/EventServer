using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using EventServer.Core;
using EventServer.Core.Domain;
using EventServer.Core.Services;
using EventServer.Core.ViewModels;

namespace EventServer.UI.Controllers
{
    [HandleError]
    public class HomeController : AppController
    {
        private readonly ITwitterService _twitterService;

        public HomeController(IRepository repository, ICurrentUserService currentUser, ITwitterService twitterService) : base(repository, currentUser)
        {
            _twitterService = twitterService;
        }

        public ActionResult Index()
        {
            var homeIndexModel = new HomeIndexModel();

            homeIndexModel.Posts = _repository.Find<Post>().Published();
            
            homeIndexModel.Tweets = _twitterService.GetaRecentTweets().ToArray();
            
            homeIndexModel.Sponsors = _repository.Find<Sponsor>()
                .Where(s => s.IsActive == true)
                .Where(s => s.Level == SponsorshipLevel.Platinum)
                .OrderBy(x => x.Name)
                .ToArray();

            return View(homeIndexModel);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult HomeMessage()
        {
            return Content(_repository.GetSpecialPage(SpecialPage.HomeMessage).Content);
        }

        public ActionResult AboutMessage()
        {
            return Content(_repository.GetSpecialPage(SpecialPage.AboutMessage).Content);
        }

        public ActionResult Sidebar()
        {
            return Content(_repository.GetSpecialPage(SpecialPage.Sidebar).Content);
        }

        public ActionResult GetLogonWidget()
        {
            UserProfile user = null;

            if (_currentUser.IsSignedIn)
            {
                user = (UserProfile)Session["_currentUserProfile"];
                if (!_currentUser.Is(user))
                {
                    user = (UserProfile)(Session["_currentUserProfile"] = _repository.Find<UserProfile>().GetBy(_currentUser.Email));
                    if (user == null)
                        Response.Redirect(GetUrl<AccountController>(c => c.LogOff()));
                }
            }

            return View("LogOnUserControl", user);
        }

        public ActionResult GetRedirectMessages()
        {
            return Content(new JavaScriptSerializer().Serialize(_currentUser.GetAndFlushRedirectMessages()));
        }

        public ActionResult Test()
        {
            Ioc.Resolve<IEventAggregator>().Raise(new PresentationCreated {PresentationId = -1});
            return Content("Success");
        }
    }
}