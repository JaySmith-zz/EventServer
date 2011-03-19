using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventServer.Core;
using EventServer.Core.Domain;
using EventServer.Core.Services;

namespace EventServer.UI.Controllers
{
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
    }
}
