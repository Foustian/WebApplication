using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using IQMedia.Model;
using IQMedia.WebApplication.Utility;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class LogOutController : Controller
    {
        //
        // GET: /LogOut/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            //Session.Abandon();

            /*int _Service = 0;

            if (ConfigurationManager.AppSettings["ServicesBaseURL"] != null && int.TryParse(ConfigurationManager.AppSettings["ServicesBaseURL"], out _Service))
            {
           if (_Service == 1 || _Service == 2)
           {*/
            ActiveUserMgr.RemoveUserFromCacheBySessionID();
            IQMedia.Web.Common.Authentication.Logout();
            
            Session.Abandon();
            /*}
             }*/


            return RedirectToAction("Index", "SignIn");
        }

    }
}
