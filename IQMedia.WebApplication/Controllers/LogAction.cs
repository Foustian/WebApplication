using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Model;
using System.Web.Routing;
using IQMedia.WebApplication.Utility;
using System.Configuration;
using IQMedia.Web.Common;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.Shared.Utility;
using System.IO;

namespace IQMedia.WebApplication.Controllers
{
    public class LogAction : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ActiveUser sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
            IQLog_UserActionsModel iQLog_UserActionsModel = null;

            Log4NetLogger.Info("Requested URL :" + filterContext.HttpContext.Request.Url.AbsoluteUri);

            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            IQMedia.Shared.Utility.UserActionDetail userPage = IQMedia.Shared.Utility.UserActionList.UserActions.Where(a => a.Controller == controllerName).FirstOrDefault();
            if (userPage != null)
            {
                var userAction = userPage.ActionFriendlyName.Where(a => a.Key == actionName).Select(p => new { Key = p.Key, Value = p.Value }).FirstOrDefault();
                if (userAction != null)
                {
                    var parameters = string.Empty;
                    filterContext.HttpContext.Request.InputStream.Seek(0, SeekOrigin.Begin);

                    using (var sr = new StreamReader(filterContext.HttpContext.Request.InputStream))
                    {
                        parameters = sr.ReadToEnd();
                    }

                    if (filterContext.HttpContext.Request.QueryString.Count > 0)
                    {
                        parameters = parameters + (parameters != "" ? "&" : string.Empty) + string.Join("&", filterContext.HttpContext.Request.QueryString);  
                    }

                    if (userPage.IsCheckControllerParameters)
                    {
                        var controllerParams = filterContext.ActionDescriptor.GetParameters();
                        if (controllerParams.Count() > 0)
                        {
                            var values = string.Join("&", controllerParams.Select(s => s.ParameterName + "=" + (filterContext.Controller.ValueProvider.GetValue(s.ParameterName) != null ? filterContext.Controller.ValueProvider.GetValue(s.ParameterName).AttemptedValue : string.Empty)));
                            parameters = parameters + (parameters != "" ? "&" : string.Empty) + values;  
                        }
                    }

                    iQLog_UserActionsModel = new IQLog_UserActionsModel();
                    iQLog_UserActionsModel.PageName = userPage.PageName;
                    iQLog_UserActionsModel.ActionName = userAction.Value;
                    iQLog_UserActionsModel.IPAddress = filterContext.HttpContext.Request.Headers["X-ClientIP"] ?? filterContext.HttpContext.Request.UserHostAddress;
                    iQLog_UserActionsModel.RequestDateTime = filterContext.HttpContext.Timestamp;
                    iQLog_UserActionsModel.RequestParameters = parameters;
                    iQLog_UserActionsModel.CustomerID = sessionInformation != null ? (int?)sessionInformation.CustomerKey : null;
                    iQLog_UserActionsModel.SessionID = HttpContext.Current.Session.SessionID;

                    UtilityLogic.InsertActionLog(iQLog_UserActionsModel);
                }
            }
        }
    }
}