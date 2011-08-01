namespace EventServer.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using EventServer.Core;
    using EventServer.Core.Domain;
    using EventServer.Core.Services;
    using EventServer.Models;

    [HandleError]
    public class SpeakersController : Controller
    {
        private readonly IRepository repository;
        private readonly ICurrentUserService currentUser;

        public SpeakersController(IRepository repository, ICurrentUserService currentUser)
        {
            this.repository = repository;
            this.currentUser = currentUser;
        }

        public ActionResult Index()
        {
            var speakersWithSessions = this.repository.Find<Session>()
               .Where(x => x.Status == SessionStatus.Accepted)
               .Select(x => x.UserId)
               .Distinct()
               .OrderBy(x => x)
               .ToArray();

            var speakers = this.repository.Find<UserProfile>()
                 .Speakers()
                 .Where(x => speakersWithSessions.Contains(x.Id))
                 .OrderBy(x => x.Name)
                 .ToArray();

            return View(new SpeakersIndexModel { Speakers = speakers });
        }

        public ActionResult SpeakerMenuWidget()
        {
            if (!this.currentUser.IsSignedIn)
            {
                return new EmptyResult();
            }

            return View(this.repository.Find<UserProfile>().GetBy(this.currentUser.Email));
        }

        public ActionResult Show(int id, string name)
        {
            var user = repository.Get<UserProfile>(id);

            var sessions = this.repository.Find<Session>()
                .Where(x => x.UserId == user.Id)
                .OrderBy(x => x.Status)
                .ThenBy(x => x.Title);

            var model = new SpeakersShowModel
                {
                    User = user,
                    CanAddSession = this.currentUser.Is(user) || this.currentUser.IsAdmin
                };

            if (this.currentUser.Is(user) || this.currentUser.IsAdmin)
            {
                model.Sessions = sessions.ToArray();
            }
            else
            {
                model.Sessions = sessions.Where(x => x.Status == SessionStatus.Accepted).ToArray();
            }

            return View(model);
        }

        [Authorize]
        public ActionResult Create(int id)
        {
            return Edit(id);
        }

        [Authorize, HttpPost, ValidateInput(false)]
        public ActionResult Create(SpeakersEditModel model)
        {
            return Edit(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var user = repository.Get<UserProfile>(id);

            var model = new SpeakersEditModel
                {
                    SpeakerId = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                };

            if (user.SpeakerProfile != null)
            {
                model.Bio = user.SpeakerProfile.Biography;
                model.ImageUrl = user.SpeakerProfile.ImageUrl.ToUri();
                model.BlogUrl = user.SpeakerProfile.BlogUrl.ToUri();
                model.IsMvp = user.SpeakerProfile.IsMvp;
                model.MvpProfileUrl = user.SpeakerProfile.MvpProfileUrl.ToUri();
                model.TravelAssistance = user.SpeakerProfile.TravelAssistance;
            }

            return View("Edit", model);
        }

        [Authorize(Roles = "Admin"), HttpPost, ValidateInput(false)]
        public ActionResult Edit(SpeakersEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            var user = repository.Get<UserProfile>(model.SpeakerId);
            user.UpdateSpeakerProfile(model.Bio, model.ImageUrl, model.BlogUrl, model.IsMvp, model.MvpProfileUrl, model.TravelAssistance);
            this.repository.Save(user);

            return RedirectToAction("Show", new { id = user.Id, name = user.UrlName });
        }
    }
}