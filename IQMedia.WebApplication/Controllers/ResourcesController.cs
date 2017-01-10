using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQMedia.WebApplication.Controllers
{
    //[CheckAuthentication()]
    public class ResourcesController : Controller
    {
        //
        // GET: /Resources/

        public ActionResult Index()
        {
            ViewBag.CurrentMenu = "spnResources";
            return View();
        }

        /*public ActionResult Whitepapers()
        {
            return View();
        }*/

        public ActionResult ProductLiterature()
        {
            return View();
        }

        public ActionResult CaseStudies()
        {
            return View();
        }


    }
}
