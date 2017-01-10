using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class IndustriesController : Controller
    {
        //
        // GET: /Industries/

        public ActionResult Index()
        {
            ViewBag.CurrentMenu = "spnIndustries";
            ViewBag.CurrentPage = "Industries";
            return View();
        }

        public ActionResult PoliticalParties()
        {
            ViewBag.CurrentMenu = "spnIndustries";
            ViewBag.CurrentPage = "IndustriesPoliticalParties";
            return View();
        }

        public ActionResult CollegiateAthleticPrograms()
        {
            ViewBag.CurrentMenu = "spnIndustries";
            ViewBag.CurrentPage = "CollegiateAthleticPrograms";
            return View();
        }

        public ActionResult ProfessionalSports()
        {
            ViewBag.CurrentMenu = "spnIndustries";
            ViewBag.CurrentPage = "IndustriesProfessional";
            return View();
        }

        public ActionResult University()
        {
            ViewBag.CurrentMenu = "spnIndustries";
            ViewBag.CurrentPage = "IndustriesUniverties";
            return View();
        }

    }
}
