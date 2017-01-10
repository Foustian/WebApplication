using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

namespace IQMedia.WebApplication.Controllers
{
    public class ArticleController : Controller
    {
        //
        // GET: /NRTracking/
        [HttpGet]
        public ActionResult Index(string au)
        {
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();

                string articledetails = Shared.Utility.CommonFunctions.DecryptLicenseStringAes(au);
                string[] details = articledetails.Split('¶');
                if (details.Length > 3)
                {
                    var url = details[2];

                    Uri articleUrl = new Uri(details[2]);
                    NameValueCollection queryString = HttpUtility.ParseQueryString(articleUrl.Query);
                    queryString["u2"] = "1";

                    string[] tempURLParts = articleUrl.ToString().Split('?');
                    url = tempURLParts[0] + "?" + queryString.ToString();
                        
                    Response.Redirect(url);         
                }

                ViewBag.ErrorMessage = Config.ConfigSettings.Settings.NRTrackingInsertFailed;
                return View();
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                ViewBag.ErrorMessage = Config.ConfigSettings.Settings.NRTrackingExceptionMessage;
                return View();
            }
        }

    }
}
