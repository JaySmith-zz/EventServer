using System;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using EventServer.Core;
using EventServer.Core.Domain;
using EventServer.Core.Services;
using EventServer.Core.ViewModels;

namespace EventServer.UI.Controllers
{
    [HandleError]
    public class SpeakersController : AppController
    {
        public SpeakersController(IRepository repository, ICurrentUserService currentUser) : base(repository, currentUser)
        {
        }

        public ActionResult Index()
        {
           var speakersWithSessions = _repository.Find<Presentation>()
              .Where(x => x.Status == PresentationStatus.Accepted)
              .Select(x => x.UserId)
              .Distinct()
              .OrderBy(x => x)
              .ToArray();

           var speakers = _repository.Find<UserProfile>()
                .Speakers()
                .Where(x => speakersWithSessions.Contains(x.Id))
                .OrderBy(x => x.Name)
                .ToArray();

            return View(new SpeakersIndexModel {Speakers = speakers});
        }

        public ActionResult SpeakerMenuWidget()
        {
            if (!_currentUser.IsSignedIn)
                return new EmptyResult();

            return View(_repository.Find<UserProfile>().GetBy(_currentUser.Email));
        }

        public ActionResult Show(int id, string name)
        {
            var user = GetUser(id, false);

            var sessions = _repository.Find<Presentation>()
                .Where(x => x.UserId == user.Id)
                .OrderBy(x => x.Status)
                .ThenBy(x => x.Title);

            var model = new SpeakersShowModel
                {
                    User = user,
                    CanAddSession = _currentUser.Is(user) || _currentUser.IsAdmin
                };

            if (_currentUser.Is(user) || _currentUser.IsAdmin)
                model.Sessions = sessions.ToArray();
            else
                model.Sessions = sessions.Where(x => x.Status == PresentationStatus.Accepted).ToArray();

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

        [Authorize]
        public ActionResult Edit(int id)
        {
            var user = GetUser(id, true);

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

        [Authorize, HttpPost, ValidateInput(false)]
        public ActionResult Edit(SpeakersEditModel model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            var user = GetUser(model.SpeakerId, true);
            user.UpdateSpeakerProfile(model.Bio, model.ImageUrl, model.BlogUrl, model.IsMvp, model.MvpProfileUrl, model.TravelAssistance);
            _repository.Save(user);

            return RedirectTo<SpeakersController>(c => c.Show(user.Id, user.UrlName));
        }
    }
}