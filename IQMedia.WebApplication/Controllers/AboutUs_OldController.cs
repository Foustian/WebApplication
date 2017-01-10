using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.Model;

namespace IQMedia.WebApplication.Controllers
{
    //[CheckAuthentication()]
    public class AboutUs_OldController : Controller
    {
        //
        // GET: /AboutUs/


        public ActionResult Index()
        {
            ViewBag.CurrentMenu = "spnAboutUs";
            ViewBag.CurrentPage = "AboutUs";
            return View();
        }

        public ActionResult CopyrightPolicy()
        {
            ViewBag.CurrentMenu = "spnAboutUs";
            ViewBag.CurrentPage = "AboutUs";
            return View();
        }

        public ActionResult HowAreWeDifferent()
        {
            ViewBag.CurrentMenu = "spnAboutUs";
            ViewBag.CurrentPage = "AboutUs";
            return View();
        }

        public ActionResult ManagementTeam()
        {
            ViewBag.CurrentMenu = "spnAboutUs";
            ViewBag.CurrentPage = "AboutUs";
            return View();
        }

        public ActionResult News()
        {
            ViewBag.CurrentMenu = "spnAboutUs";
            ViewBag.CurrentPage = "AboutUs";

            IQNewsLogic iQNewsLogic = (IQNewsLogic)LogicFactory.GetLogic(LogicType.IQNews);
            List<IQNews_Model> lstIQNews_Model = iQNewsLogic.GetIQNews();           
            return View(lstIQNews_Model);
        }

        public ActionResult OEMPartnerships()
        {
            ViewBag.CurrentMenu = "spnAboutUs";
            ViewBag.CurrentPage = "AboutUs";
            return View();
        }

    }
}
