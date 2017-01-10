using System;
using System.Collections.Generic;
using System.Web.Mvc;
using IQMedia.Shared.Utility;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.Model;
using System.Configuration;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class ConnectController : Controller
    {
        //
        // GET: /Connect/

        public ActionResult Index()
        {
            try
            {
                ActiveUser sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                CustomerModel customer = customerLogic.GetCustomerWithRoleByCustomerID(sessionInformation.CustomerKey);

                string url = "https://connect.iqmcorp.com/api/user/login";
                string data = String.Format("key={0}&user_id={1}", ConfigurationManager.AppSettings["AnewstipAPIKey"], customer.AnewstipUserID);
                string response = CommonFunctions.DoHttpPostRequest(url, data, p_ContentType: "application/x-www-form-urlencoded", p_IgnoreResponseLength: true);
                
                Dictionary<string, string> dictResult = new Dictionary<string, string>();
                if (!String.IsNullOrEmpty(response))
                {
                    Newtonsoft.Json.Linq.JObject jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(response);

                    if (Convert.ToString(jsonData["code"]) == "1")
                        dictResult.Add("Token", Convert.ToString(jsonData["data"]["token"]));
                    else
                        Utility.CommonFunctions.WriteException(new Exception(String.Format("Received error code from Anewstip Login API call.  Url: {0}?{1} || Error Code: {2} || Error Message: {3}", url, data, jsonData["code"], jsonData["msg"])));
                }
                else
                    Utility.CommonFunctions.WriteException(new Exception(String.Format("No response received from Anewstip Login API call.  Url: {0}?{1}", url, data)));

                return View(dictResult);
            }
            catch (Exception ex)
            {
                Utility.CommonFunctions.WriteException(ex);
                return View();
            }
        }

    }
}
