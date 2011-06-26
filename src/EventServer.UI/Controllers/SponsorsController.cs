using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using EventServer.Core;
using EventServer.Core.Domain;
using EventServer.Core.Services;
using EventServer.Core.ViewModels;

namespace EventServer.UI.Controllers
{
    public class SponsorsController : AppController
    {
        public SponsorsController(IRepository repository, ICurrentUserService currentUser) : base(repository, currentUser) { }

        public ActionResult SponsorMessage()
        {
            var page = _repository.GetSpecialPage(SpecialPage.SponsorInformation);
            return Content(page.Content);
        }

        //
        // GET: /Default1/
        public ActionResult Index()
        {
            var model = new SponsorIndexModel();

            var sponsors = _repository.Find<Sponsor>();

            model.PlatinumSponsors = sponsors
                .Where(x => x.Level == SponsorshipLevel.Platinum)
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToArray();

            model.GoldSponsors = sponsors
                .Where(x => x.Level == SponsorshipLevel.Gold)
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToArray();

            model.InactiveSponsors = sponsors
                .Where(x => x.IsActive == false)
                .OrderBy(x => x.Level)
                .OrderBy(x => x.Name)
                .ToArray();

            return View(model);
        }

        public ActionResult Sponsorship()
        {
            return View();
        }

        //
        // GET: /Default1/Details/5
        public ActionResult Activate(int id)
        {
            var sponsor = _repository.Get<Sponsor>(id);
            sponsor.IsActive = true;

            _repository.Save(sponsor);

            return RedirectToAction("Index");
        }

        public ActionResult Inactivate(int id)
        {
            var sponsor = _repository.Get<Sponsor>(id);
            sponsor.IsActive = false;

            _repository.Save(sponsor);

            return RedirectToAction("Index");
        }

        //
        // GET: /Default1/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var values = Enum.GetValues(typeof (SponsorshipLevel))
                .Cast<SponsorshipLevel>()
                .Select(e => new {ID = e, Name = e.ToString()});

            ViewData["Level"] = new SelectList(values, "Id", "Name", SponsorshipLevel.Gold);

            return View();
        }

        //
        // POST: /Default1/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateInput(false)]
        public ActionResult Create(Sponsor sponsor, string cancelButton)
        {
            try
            {
                if (String.IsNullOrEmpty(cancelButton))
                {
                    _repository.Save(sponsor);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Default1/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var sponsor = _repository.Get<Sponsor>(id);
            _repository.Delete(sponsor);

            return RedirectToAction("Index");
        }

        //
        // POST: /Default1/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var sponsor = _repository.Get<Sponsor>(id);
                _repository.Delete(sponsor);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        
        //
        // GET: /Default1/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var values = Enum.GetValues(typeof(SponsorshipLevel))
                .Cast<SponsorshipLevel>()
                .Select(e => new { ID = e, Name = e.ToString() });
            
            var sponsor = _repository.Get<Sponsor>(id);

            ViewData["Level"] = new SelectList(values, "Id", "Name", sponsor.Level);
            
            return View(sponsor);
        }

        //
        // POST: /Default1/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateInput(false)]
        public ActionResult Edit(Sponsor sponsor, string cancelButton)
        {
            try
            {
                if (String.IsNullOrEmpty(cancelButton))
                {
                    //var logoUrl = SaveLogo();
                    _repository.Save(sponsor);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult PastSponsors()
        {
            var model = new SponsorIndexModel();

            var platinumSponsors = new[]
                                       {
                                           new Sponsor()
                                               {
                                                   Description = "Microsoft",
                                                   IsActive = true,
                                                   LogoUri = @"http://i143.photobucket.com/albums/r140/capscdev/ms-logo_bL.png",
                                                   Name = "Microsoft",
                                                   Level = SponsorshipLevel.Platinum
                                               },
                                           new Sponsor()
                                               {
                                                   Description = "Improving Enterprises",
                                                   IsActive = true,
                                                   LogoUri = @"http://i143.photobucket.com/albums/r140/capscdev/improving.jpg",
                                                   Name = "Improving Enterprises",
                                                   Level = SponsorshipLevel.Platinum
                                               }
                                       };

            model.PlatinumSponsors = platinumSponsors;

            Sponsor[] goldSponsors = new[]
                                         {
                                             new Sponsor()
                                               {
                                                   Description = "Oracle",
                                                   IsActive = true,
                                                   LogoUri = @"http://www.oracleimg.com/us/assets/oralogo-small.gif",
                                                   Name = "Oracle",
                                                   Level = SponsorshipLevel.Platinum
                                               },
                                           new Sponsor()
                                               {
                                                   Description = "Telerik",
                                                   IsActive = true,
                                                   LogoUri = @"http://i143.photobucket.com/albums/r140/capscdev/telerikLogo-web-450x180px.png",
                                                   Name = "Telerik",
                                                   Level = SponsorshipLevel.Platinum
                                               }
                                         };
            model.GoldSponsors = goldSponsors;

                model.InactiveSponsors = new Sponsor[0];

            return View(model);
        }
        private string SaveLogo()
        {
            foreach (string inputTagName in Request.Files)
            {
                var folder = Settings.Instance.FileDataStorePath;

                HttpPostedFileBase file = Request.Files[inputTagName];

                if (file.ContentLength <= 0) continue;

                string filePath = Path.Combine(Settings.Instance.FileDataStorePath, Path.GetFileName(file.FileName));
                
                file.SaveAs(filePath);

                
            }

            return "";
        }

    }
}
