using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Alachisoft.NCache.Web.Caching;
using System.Configuration;
using IQMedia.Common.Util;

namespace IQMedia.WebApplication
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            IQCommon.CommonFunctions.ConnString = ConfigurationManager.ConnectionStrings["IQMediaGroupConnectionString"].ConnectionString;

            try
            {
                Application["ActiveUsers"] = NCache.InitializeCache(ConfigurationManager.AppSettings["CacheActiveUsers"]);
            }
            catch (Exception ex)
            {
                Shared.Utility.Log4NetLogger.Error("Cache Initialization error", ex);                
            }
        }

        protected void Session_Start()
        { 
        
        }
        protected void Session_End()
        { 
        
        }       
    }
}