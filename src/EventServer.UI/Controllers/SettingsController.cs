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

            return View(settings);
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
    }
}
