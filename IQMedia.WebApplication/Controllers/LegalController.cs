using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQMedia.WebApplication.Controllers
{
    public class LegalController : Controller
    {
        //
        // GET: /Legal/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CopyrightAbusePolicy()
        {
            return View();
        }

        public ActionResult PrivacyStatement()
        {
            return View();
        }

        //public ActionResult SafeHarbor()
        //{
        //    return View();
        //}

        public ActionResult SecurityStatement()
        {
            return View();
        }

    }
}
