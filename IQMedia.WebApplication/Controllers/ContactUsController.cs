using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class ContactUsController : Controller
    {
        //
        // GET: /ContactUs/

        public ActionResult Index()
        {
            try
            {
                var ssoKey = ConfigurationManager.AppSettings["ZendeskSSOKey"];
                var email = ConfigurationManager.AppSettings["ZendeskEmail"];
                var name = ConfigurationManager.AppSettings["ZendeskName"];

                TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                int timestamp = (int)t.TotalSeconds;                

                var payload = new Dictionary<string, object>()
                {
                    { "iat", timestamp },
                    { "jti", System.Guid.NewGuid().ToString() },
                    {"email", email},
                    {"name", name}                    
                };

                string token = JWT.JsonWebToken.Encode(payload, ssoKey, JWT.JwtHashAlgorithm.HS256);
                string redirectUrl = ConfigurationManager.AppSettings["ZendeskRedirectURL"] + token;

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return View();
            }
        }

    }
}
