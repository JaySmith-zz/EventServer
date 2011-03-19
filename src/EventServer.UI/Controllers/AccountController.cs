using System;
using System.Web.Mvc;
using EventServer.Core;
using EventServer.Core.Domain;
using EventServer.Core.Services;
using EventServer.Core.ViewModels;

namespace EventServer.UI.Controllers
{
    [HandleError]
    public class AccountController : AppController
    {
        public AccountController(IRepository repository, ICurrentUserService currentUser, IFormsAuthenticationService formsAuthenticationService, IMembershipService membershipService) : base(repository, currentUser)
        {
            _formsService = formsAuthenticationService;
            _membershipService = membershipService;
        }

        private readonly IFormsAuthenticationService _formsService;
        private readonly IMembershipService _membershipService;

        public ActionResult LogOn(string returnUrl)
        {
            return View(new AccountLogOnModel {ReturnUrl = returnUrl});
        }

        [HttpPost]
        public ActionResult LogOn(AccountLogOnModel model)
        {
            if (ModelState.IsValid)
            {
                model.Email = model.Email.ToLower().Trim();

                if (_membershipService.ValidateUser(model.Email, model.Password))
                {
                    _formsService.SignIn(model.Email, model.RememberMe);
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);

                    return RedirectTo<HomeController>(c => c.Index());
                }

                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            _formsService.SignOut();
            return RedirectTo<HomeController>(c => c.Index());
        }

        public ActionResult Register()
        {
            return View(new AccountRegisterModel());
        }

        [HttpPost]
        public ActionResult Register(AccountRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                model.Email = model.Email.ToLower().Trim();

                var errorMessage = _membershipService.CreateUser(model.Email, model.Password);

                if (string.IsNullOrEmpty(errorMessage))
                {
                    _formsService.SignIn(model.Email, false);

                    var user = _repository.Find<UserProfile>().GetBy(model.Email);

                    user.CompleteRegistration(model.Name, false);
                    _repository.Save(user);

                    return RedirectTo<AccountController>(c => c.Show(user.Id, user.UrlName));
                }

                ModelState.AddModelError("", errorMessage);
            }

            return View(model);
        }

        /********************************************************************************/
        /********************************************************************************/

        [Authorize]
        public ActionResult Show(int id, string name)
        {
            return View(new AccountShowModel {User = GetUser(id, true)});
        }

        [Authorize, HttpPost]
        public ActionResult Show(AccountShowModel model)
        {
            return RedirectTo<AccountController>(c => c.Show(model.User.Id, model.User.UrlName));
        }

        [Authorize]
        public ActionResult ChangePassword(int id)
        {
            return View(new AccountChangePasswordModel {Email = GetUser(id, true).Email});
        }

        [Authorize, HttpPost]
        public ActionResult ChangePassword(AccountChangePasswordModel model)
        {
            if (string.IsNullOrEmpty(model.OldPassword))
            {
                if (!_currentUser.IsAdmin)
                    ModelState.AddModelError("OldPassword", "The Current password field is required.");
                else
                    model.OldPassword = "ADMIN";
            }

            if (ModelState.IsValid)
            {
                if (_membershipService.ChangePassword(model.Email, model.OldPassword, model.NewPassword))
                    return RedirectTo<AccountController>(c => c.ChangePasswordSuccess());

                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChangeName(int id)
        {
            var user = GetUser(id, true);

            var model = new AccountChangeNameModel
            {
                Id = id,
                Name = user.Name,
            };

            return View(model);
        }
        
        [Authorize, HttpPost]
        public ActionResult ChangeName(AccountChangeNameModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = GetUser(model.Id, true);
            user.UpdateName(model.Name.Trim());
            _repository.Save(user);

            return RedirectTo<AccountController>(c => c.Show(model.Id, user.UrlName));
        }

        [Authorize]
        public ActionResult ChangeEmail(int id)
        {
            var user = GetUser(id, true);

            var model = new AccountChangeEmailModel
            {
                Id = id,
                CurrentEmail = user.Email,
                NewEmail = user.Email,
            };

            return View(model);
        }

        [Authorize, HttpPost]
        public ActionResult ChangeEmail(AccountChangeEmailModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.NewEmail = model.NewEmail.Trim();

            var user = GetUser(model.Id, true);

            var foundUser = _repository.Find<UserProfile>().GetBy(model.NewEmail);
            if (foundUser != null && foundUser.Id != user.Id)
            {
                ModelState.AddModelError("NewEmail", "Invalid or taken email address.");
                return View(model);
            }

            if (foundUser == null)
            {
                user.UpdateEmail(model.NewEmail);
                _repository.Save(user);
            }

            return RedirectTo<AccountController>(c => c.Show(model.Id, user.UrlName));
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var user = GetUser(id, true);

            if (_currentUser.Is(user.Email))
                _currentUser.AddRedirectMessage("Cannot delete yourself");
            else
            {
                _membershipService.Delete(user.Email);
                _currentUser.AddRedirectMessage("{0} successfully deleted".F(user.Email));
            }

            return RedirectTo<AdminController>(c => c.Users());
        }
    }
}