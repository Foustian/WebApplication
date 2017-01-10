using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class ProductsController : Controller
    {
        //
        // GET: /Products/
        string currentProductPage = string.Empty;


        public ActionResult Index()
        {
            ViewBag.CurrentMenu = "spnProducts";
            ViewBag.CurrentPage = "Products";
            return View();
        }

        public ActionResult ProPrep()
        {
            ViewBag.CurrentMenu = "spnProducts";
            ViewBag.CurrentPage = "ProPrep";
            return View(currentProductPage);
            //return View();
        }

        public ActionResult cliQ()
        {
            ViewBag.CurrentMenu = "spnProducts";
            ViewBag.CurrentPage = "cliQ";
            return View();
            //return View();
        }

        public ActionResult OptimizedMediaCloud()
        {
            ViewBag.CurrentMenu = "spnProducts";
            ViewBag.CurrentPage = "OptimizedCloud";
            return View();
            //return View();
        }

        public ActionResult InlineMediaWorkspace()
        {
            ViewBag.CurrentMenu = "spnProducts";
            ViewBag.CurrentPage = "InlineWorkShop";
            return View();
            //return View();
        }

        public ActionResult myiQStatic()
        {
            ViewBag.CurrentMenu = "spnProducts";
            ViewBag.CurrentPage = "MyiQ";
            return View();
            //return View();
        }

        public ActionResult BroadcastTV()
        {
            ViewBag.CurrentMenu = "spnProducts";
            ViewBag.CurrentPage = "Products";
            return View();         
        }

        public ActionResult OnlineNews()
        {
            ViewBag.CurrentMenu = "spnProducts";
            ViewBag.CurrentPage = "Products";
            return View();
        }

        public ActionResult SocialMedia()
        {
            ViewBag.CurrentMenu = "spnProducts";
            ViewBag.CurrentPage = "Products";
            return View();
        }

        public ActionResult Twitter()
        {
            ViewBag.CurrentMenu = "spnProducts";
            ViewBag.CurrentPage = "Products";
            return View();
        }

        public ActionResult UserGeneratedContent()
        {
            ViewBag.CurrentMenu = "spnProducts";
            ViewBag.CurrentPage = "Products";
            return View();
        }

    }
}
