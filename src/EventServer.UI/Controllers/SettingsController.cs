using System.Web.Mvc;
using EventServer.Core;
using EventServer.Core.Domain;
using EventServer.Core.Services;

namespace EventServer.UI.Controllers
{
    using System;
    using System.Collections.Generic;

    using EventServer.Core.ViewModels;

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
            var model = new SettingsTwitterViewModel();
            model.Id = Settings.Instance.TwitterId;
            model.FilterDate = Settings.Instance.TwitterFilterDate;
            model.DisplayCount = Settings.Instance.TwitterDisplayCount;

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Twitter(SettingsTwitterViewModel twitterSettings)
        {
            Settings.Instance.TwitterId = twitterSettings.Id;
            Settings.Instance.TwitterDisplayCount = twitterSettings.DisplayCount;
            Settings.Instance.TwitterFilterDate = twitterSettings.FilterDate;
            Settings.SaveSettings(Settings.Instance);

            return this.View(twitterSettings);
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
