using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EventServer.Core;
using EventServer.Core.Domain;
using EventServer.Core.Services;
using EventServer.Core.ViewModels;
using System.Web;

namespace EventServer.UI.Controllers
{
    [HandleError]
    public class SessionsController : AppController
    {
        public SessionsController(IRepository repository, ICurrentUserService currentUser) : base(repository, currentUser)
        {
        }

        public ActionResult Index(string tab)
        {
            if (!string.IsNullOrEmpty(tab) && new[] {"list", "tracks", "times"}.Contains(tab))
                Response.Cookies.Set(new HttpCookie("sessions-tab", tab) {Expires = DateTime.Today.AddYears(10)});
            else
            {
                var sessionsTabCookie = Request.Cookies["sessions-tab"];
                tab = sessionsTabCookie == null ? "list" : sessionsTabCookie.Value;
            }

            var actions = new Dictionary<string, string> {{"list", "GetRawList"}, {"tracks", "GetByTrack"}, {"times", "GetByTime"}};

            var model = new SessionsIndexModel();
            model.ContentAction = actions[tab];
            model.ListLinkCssClass = tab == "list" ? "you-are-here" : "";
            model.TracksLinkCssClass = tab == "tracks" ? "you-are-here" : "";
            model.TimesLinkCssClass = tab == "times" ? "you-are-here" : "";

            return View(model);
        }

        public ActionResult GetRawList()
        {
            var model = _repository.FindAcceptedPresentations().OrderBy(x => x.Title).ToArray();

            return View(model);
        }

        public ActionResult GetByTrack()
        {
            IEnumerable<Session> sessions = _repository
                .FindAcceptedPresentations()
                .OrderBy(x => x.Track)
                .ThenBy(x => x.Day)
                .ThenBy(x => x.Slot);
            
            if (!_currentUser.IsAdmin)
                sessions = sessions
                    .Where(x => !string.IsNullOrEmpty(x.Track))
                    .Where(x => x.Track != "None");

            return View(new SessionsGetByTrackModel(sessions, _currentUser.IsAdmin));
        }

        public ActionResult GetByTime()
        {
            IEnumerable<Session> presentations = _repository
                .FindAcceptedPresentations()
                .OrderBy(x => x.Day)
                .ThenBy(x => x.Slot)
                .ThenBy(x => x.Track);

            return View(new SessionsGetByTrackModel(presentations, _currentUser.IsAdmin));
        }

        public ActionResult Show(int id, string title)
        {
            var session = GetSession(id, false);

            session.User = _repository.Get<UserProfile>(session.UserId);

            return View(session);
        }

        [Authorize]
        public ActionResult Add()
        {
            return View(new SessionsAddEditModel {Id = -1});
        }

        [Authorize, HttpPost, ValidateInput(false)]
        public ActionResult Add(SessionsAddEditModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _repository.Find<UserProfile>().GetBy(_currentUser.Email);

            var session = new Session(user, model.Title, model.Description, model.Level, model.Category);

            _repository.Save(session);

            return RedirectTo<SessionsController>(c => c.Show(session.Id, session.Title.MakeUrlFriendly()));
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var session = GetSession(id, true);
            if (session.IsClosedForEdit())
            {
                _currentUser.AddRedirectMessage("Session {0} closed to editing".F(id));
                return RedirectTo<SessionsController>(c => c.Show(session.Id, session.Title.MakeUrlFriendly()));
            }

            var model = new SessionsAddEditModel
                {
                    Id = session.Id,
                    Title = session.Title,
                    Description = session.Description,
                    Level = session.Level,
                    Category = session.Category,
                    Track = session.Track,
                    TimeSlot = session.Slot,
                    Room = session.Room,
                    Day = session.Day
                };

            ViewData["TimeSlot"] = new SelectList(Core.Domain.Session.TimeSlots, "Key", "Value", model.TimeSlot);
            return View(model);
        }

        [Authorize, HttpPost, ValidateInput(false)]
        public ActionResult Edit(SessionsAddEditModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["TimeSlot"] = new SelectList(Core.Domain.Session.TimeSlots, "Key", "Value", model.TimeSlot);
                return View(model);
            }

            var session = GetSession(model.Id, true);
            if (session.IsClosedForEdit())
            {
                _currentUser.AddRedirectMessage("Session {0} closed to editing".F(model.Id));
                return RedirectTo<SessionsController>(c => c.Show(session.Id, session.Title.MakeUrlFriendly()));
            }

            session.Update(model.Title, model.Description, model.Level, model.Category, model.Track, model.TimeSlot, model.Room, model.Day);
            _repository.Save(session);

            return RedirectTo<SessionsController>(c => c.Show(session.Id, session.Title.MakeUrlFriendly()));
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var session = GetSession(id, true);
            if (session.IsClosedForEdit())
            {
                _currentUser.AddRedirectMessage("Session {0} closed to editing".F(id));
                return RedirectTo<SessionsController>(c => c.Show(session.Id, session.Title.MakeUrlFriendly()));
            }

            _repository.Delete(session);

            var user = _repository.Get<UserProfile>(session.UserId);

            return RedirectTo<SpeakersController>(c => c.Show(user.Id, user.UrlName));
        }

        [ValidateInput(false)]
        public ActionResult PostComment(int id, Comment comment)
        {
            var session = GetSession(id, true);

            session.AddComment(comment);
            _repository.Save(session);

            return View("DisplayTemplates/Comment", comment);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Accept(int id)
        {
            var session = GetSession(id, true);

            session.Accept();
            _repository.Save(session);

            return RedirectTo<SessionsController>(c => c.Show(session.Id, session.Title.MakeUrlFriendly()));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Reject(int id)
        {
            var session = GetSession(id, true);

            session.Reject();
            _repository.Save(session);

            return RedirectTo<SessionsController>(c => c.Show(session.Id, session.Title.MakeUrlFriendly()));
        }

        public ActionResult AdminBox(int id)
        {
            if (!Request.IsAuthenticated)
                return new EmptyResult();

            var session = _repository.Get<Session>(id);
            if (session == null)
                return new EmptyResult();

            var isAdmin = _currentUser.IsAdmin;

            if (!_currentUser.Owns(session) && !isAdmin)
                return new EmptyResult();

            var model = new SessionsAdminBoxModel
            {
                SessionId = session.Id,
                CanEdit = !session.IsClosedForEdit(),
                CanAcceptReject = isAdmin,
                User = _repository.Find<UserProfile>().GetBy(_currentUser.Email),
                Comments = session.Comments.ToArray()
            };

            return View(model);
        }
    }
}