namespace EventServer.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using EventServer.Core;
    using EventServer.Core.Domain;
    using EventServer.Core.Services;
    using EventServer.Models;

    public class HomeController : Controller
    {
        private readonly IRepository repository;
        private readonly ITwitterService twitterService;
        
        public HomeController(IRepository repository, ICurrentUserService currentUserService, ITwitterService twitterService)
        {
            this.repository = repository;
            this.twitterService = twitterService;
        }

        public ActionResult Index()
        {
            var model = new HomeIndexModel();

            // model.Content = Content(repository.GetSpecialPage(SpecialPage.HomeMessage).Content);
            model.Posts = repository.Find<Post>().Published();

            model.Tweets = twitterService.GetaRecentTweets().ToArray();

            model.Sponsors = repository.Find<Sponsor>()
                .Where(s => s.IsActive == true)
                .Where(s => s.Level == SponsorshipLevel.Platinum)
                .OrderBy(x => x.Name)
                .ToArray();

            return View(model);
        }

        public ContentResult HomeMessage()
        {
            return Content(repository.GetSpecialPage(SpecialPage.HomeMessage).Content);
        }

        public ContentResult Sidebar()
        {
            return Content(repository.GetSpecialPage(SpecialPage.Sidebar).Content);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
