namespace EventServer.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using EventServer.Core;
    using EventServer.Core.Domain;
    using EventServer.Core.Services;
    using EventServer.Models;

    [HandleError]
    public class AdminController : Controller
    {
        private readonly IRepository repository;
        private readonly ICurrentUserService currentUser;
        private readonly IMembershipService membershipService;
        

        public AdminController(IRepository repository, ICurrentUserService userService, IMembershipService membershipService)
        {
            this.repository = repository;
            this.currentUser = userService;
            this.membershipService = membershipService;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Users()
        {
            return View(new AdminUsersModel { Users = GetUserModels(), NewUser = new RegisterModel() });
        }

        [Authorize(Roles = "Admin"), HttpPost]
        public ActionResult Users(AdminUsersModel model)
        {
            if (ModelState.IsValid)
            {
                var errorMessage = this.membershipService.CreateUser(model.NewUser.Email.ToLower(), model.NewUser.Password);

                if (string.IsNullOrEmpty(errorMessage))
                {
                    var user = this.repository.Find<UserProfile>().GetBy(model.NewUser.Email);
                    user.CompleteRegistration(model.NewUser.UserName, true);
                    this.repository.Save(user);

                    //return RedirectTo<AdminController>(c => c.Users());
                }

                ModelState.AddModelError("", errorMessage);
            }

            model.Users = GetUserModels();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Sessions()
        {
            var speakers = this.repository.Find<UserProfile>().ToDictionary(x => x.Id);

            var sessions = this.repository.Find<Session>()
                .Each(x => x.User = speakers.TryGet(x.UserId))
                .OrderBy(x => x.Category)
                .ThenBy(x => x.User != null ? x.User.Name : x.UserId.ToString())
                .ThenBy(x => x.Status)
                .ThenBy(x => x.Title)
                .ToArray();

            return View(new AdminSessionsModel { Sessions = sessions });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AlterAdmin(string email, bool isAdmin)
        {
            if (this.currentUser.Is(email))
            {
                return Json(new { Success = false, Message = "cannot add/remove yourself" });
            }

            if (isAdmin)
            {
                if (!this.membershipService.IsUserInRole(email, "Admin"))
                {
                    this.membershipService.AddUserToRole(email, "Admin");
                }

                return Json(new { Success = true, Message = email + " is now an administrator" });
            }

            if (this.membershipService.IsUserInRole(email, "Admin"))
            {
                this.membershipService.RemoveUserFromRole(email, "Admin");
            }

            return Json(new { Success = true, Message = email + " is no longer an administrator" });
        }

        public PartialViewResult AdminMenuWidget()
        {
            ViewBag.IsAdmin = currentUser.IsAdmin;

            return PartialView();
        }

        public ActionResult Cleanup()
        {
            this.repository.Find<Session>()
                .Where(x => x.Category.Value == ".NET")
                .Each(x =>
                {
                    x.Category = SessionCategory.Developers;
                    this.repository.Save(x);
                });

            return Content("All .NET categories changed to Developers");
        }

        private AdminUsersModel.UserModel[] GetUserModels()
        {
            return this.repository.Find<UserProfile>()
                .Select(user => new AdminUsersModel.UserModel
                    {
                        User = user,
                        IsAdmin = this.membershipService.IsUserInRole(user.Email, "Admin"),
                    })
                .OrderByDescending(x => x.IsAdmin)
                .ThenBy(x => x.User.Name)
                .ToArray();
        }
    }
}