namespace EventServer.UI.Controllers
{
    using System.Collections.Generic;

    using EventServer.Core.ViewModels;

    using System.Web.Mvc;

    using EventServer.Core;
    using EventServer.Core.Domain;
    using EventServer.Core.Services;

    public class SettingsController : AppController
    {
        public SettingsController(IRepository repository, ICurrentUserService currentUser)
            : base(repository, currentUser)
        {
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var settings = Settings.Instance;

            ViewData["AvailableThemes"] = GetAvailableThemesSelectListItems();
            ViewData["NumberOfDays"] = GetAvailableDays();

            return View(settings);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(AppSettings settings)
        {
            try
            {
                Settings.SaveSettings(settings);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Twitter()
        {
            var model = new TwitterSettingsViewModel();
            model.Id = Settings.Instance.TwitterId;
            model.FilterDate = Settings.Instance.TwitterFilterDate;
            model.DisplayCount = Settings.Instance.TwitterDisplayCount;

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Twitter(TwitterSettingsViewModel model)
        {
            Settings.Instance.TwitterId = model.Id;
            Settings.Instance.TwitterDisplayCount = model.DisplayCount;
            Settings.Instance.TwitterFilterDate = model.FilterDate;
            Settings.SaveSettings(Settings.Instance);

            return this.View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Email()
        {
            var model = new EmailSettingsViewModel { 
                    EmailEnabled = Settings.Instance.EmailEnabled,
                    EmailFromAddress = Settings.Instance.EmailFromAddress,
                    EmailHost = Settings.Instance.EmailHost,
                    EmailHostPort = Settings.Instance.EmailHostPort,
                    EnableSsl = Settings.Instance.EmailEnableSsl,
                    UserName = Settings.Instance.EmailUsername,
                    Password = Settings.Instance.EmailPassword,
                    SubjectPrefix = Settings.Instance.EmailSubjectPrefix
                };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Email(EmailSettingsViewModel model)
        {
            Settings.Instance.EmailEnabled = model.EmailEnabled;
            Settings.Instance.EmailFromAddress = model.EmailFromAddress;
            Settings.Instance.EmailHost = model.EmailHost;
            Settings.Instance.EmailHostPort = model.EmailHostPort;
            Settings.Instance.EmailEnableSsl = model.EnableSsl;
            Settings.Instance.EmailUsername = model.UserName;
            Settings.Instance.EmailPassword = model.Password;
            Settings.Instance.EmailSubjectPrefix = model.SubjectPrefix;

            Settings.SaveSettings(Settings.Instance);

            return this.View(model);
        }

        private List<SelectListItem> GetAvailableDays()
        {
            var availableDays = new List<SelectListItem>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new SelectListItem();

                var itemText = (i == 1) ? i + " Day" : i + " Days";

                item.Value = i.ToString();
                item.Text = itemText;
                availableDays.Add(item);
            }

            return availableDays;
        }

        private List<SelectListItem> GetAvailableThemesSelectListItems()
        {
            var availableThemesSelectListItems = new List<SelectListItem>();
            foreach (var availableTheme in Settings.Instance.AvailableThemes)
            {
                var item = new SelectListItem();
                item.Value = availableTheme;
                item.Text = availableTheme;
                availableThemesSelectListItems.Add(item);
            }

            return availableThemesSelectListItems;
        }

    }
}
