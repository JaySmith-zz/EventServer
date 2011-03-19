using System.Linq;
using System.Web.Mvc;
using EventServer.Core;
using EventServer.Core.Domain;
using EventServer.Core.Services;
using EventServer.Core.ViewModels;

namespace EventServer.UI.Controllers
{
    [HandleError]
    public class AdminController : AppController
    {
        public AdminController(IRepository repository, ICurrentUserService currentUser, IMembershipService membershipService) : base(repository, currentUser)
        {
            _membershipService = membershipService;
        }

        private readonly IMembershipService _membershipService;

        [Authorize(Roles = "Admin")]
        public ActionResult Users()
        {
            return View(new AdminUsersModel {Users = GetUserModels(), NewUser = new AccountRegisterModel()});
        }

        [Authorize(Roles = "Admin"), HttpPost]
        public ActionResult Users(AdminUsersModel model)
        {
            if (ModelState.IsValid)
            {
                var errorMessage = _membershipService.CreateUser(model.NewUser.Email.ToLower(), model.NewUser.Password);

                if (string.IsNullOrEmpty(errorMessage))
                {
                    var user = _repository.Find<UserProfile>().GetBy(model.NewUser.Email);
                    user.CompleteRegistration(model.NewUser.Name, true);
                    _repository.Save(user);

                    return RedirectTo<AdminController>(c => c.Users());
                }

                ModelState.AddModelError("", errorMessage);
            }

            model.Users = GetUserModels();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Sessions()
        {
            var speakers = _repository.Find<UserProfile>().ToDictionary(x => x.Id);

            var sessions = _repository.Find<Presentation>()
                .Each(x => x.User = speakers.TryGet(x.UserId))
                .OrderBy(x => x.Category)
                .ThenBy(x => x.User != null ? x.User.Name : x.UserId.ToString())
                .ThenBy(x => x.Status)
                .ThenBy(x => x.Title)
                .ToArray();

            return View(new AdminSessionsModel {Sessions = sessions});
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AlterAdmin(string email, bool isAdmin)
        {
            if (_currentUser.Is(email))
                return Json(new {Success = false, Message = "cannot add/remove yourself"});

            if (isAdmin)
            {
                if (!_membershipService.IsUserInRole(email, "Admin"))
                    _membershipService.AddUserToRole(email, "Admin");

                return Json(new {Success = true, Message = email + " is now an administrator"});
            }

            if (_membershipService.IsUserInRole(email, "Admin"))
                _membershipService.RemoveUserFromRole(email, "Admin");

            return Json(new {Success = true, Message = email + " is no longer an administrator"});
        }

        public ActionResult AdminMenuWidget()
        {
            if (!_currentUser.IsAdmin)
                return new EmptyResult();

            return View();
        }

        public ActionResult Cleanup()
        {
            _repository.Find<Presentation>()
                .Where(x => x.Category.Value == ".NET")
                .Each(x =>
                {
                    x.Category = PresentationCategory.Developers;
                    _repository.Save(x);
                });

            return Content("All .NET categories changed to Developers");
        }

        private AdminUsersModel.UserModel[] GetUserModels()
        {
            return _repository.Find<UserProfile>()
                .Select(user => new AdminUsersModel.UserModel
                    {
                        User = user,
                        IsAdmin = _membershipService.IsUserInRole(user.Email, "Admin"),
                    })
                .OrderByDescending(x => x.IsAdmin)
                .ThenBy(x => x.User.Name)
                .ToArray();
        }
    }
}