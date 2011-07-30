using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventServer.Controllers
{
    using EventServer.Core;
    using EventServer.Core.Domain;
    using EventServer.Core.Services;
    using EventServer.Core.ViewModels;

    public class HomeController : Controller
    {
        private readonly IRepository repository;
        private readonly ICurrentUserService currentUserService;
        private readonly ITwitterService twitterService;
        

        public HomeController(IRepository repository, ICurrentUserService currentUserService, ITwitterService twitterService)
        {
            this.repository = repository;
            this.currentUserService = currentUserService;
            this.twitterService = twitterService;
        }

        public ActionResult Index()
        {
            var model = new HomeIndexModel();

            model.Posts = repository.Find<Post>().Published();

            model.Tweets = twitterService.GetaRecentTweets().ToArray();

            model.Sponsors = repository.Find<Sponsor>()
                .Where(s => s.IsActive == true)
                .Where(s => s.Level == SponsorshipLevel.Platinum)
                .OrderBy(x => x.Name)
                .ToArray();

            return View(model);
        }

        public ActionResult HomeMessage()
        {
            return Content(repository.GetSpecialPage(SpecialPage.HomeMessage).Content);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
