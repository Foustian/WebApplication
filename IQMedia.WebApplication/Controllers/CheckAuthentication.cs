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
    public class CheckAuthentication : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                ActiveUser sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                if (ConfigurationManager.AppSettings["IsMaintenance"] == "true")
                {
                    filterContext.Result = new RedirectResult("~/UnderMaintenance/Index");
                }
                else
                {
                    Int64 _CustomerID = 0;
                    string actionName = filterContext.ActionDescriptor.ActionName;
                    string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

                    IQLog_UserActionsModel iQLog_UserActionsModel = null;
                    IQMedia.Shared.Utility.UserActionDetail userPage = IQMedia.Shared.Utility.UserActionList.UserActions.Where(a => a.Controller == controllerName).FirstOrDefault();
                    if (userPage != null)
                    {
                        var userAction = userPage.ActionFriendlyName.Where(a => a.Key == actionName).Select(p => new { Key = p.Key, Value = p.Value }).FirstOrDefault();
                        if (userAction != null)
                        {
                            var parameters = string.Empty;
                            filterContext.HttpContext.Request.InputStream.Seek(0, SeekOrigin.Begin);

                            MemoryStream ms = new MemoryStream();
                            filterContext.HttpContext.Request.InputStream.CopyTo(ms);
                            using (var sr = new StreamReader(ms))
                            {
                                ms.Seek(0, SeekOrigin.Begin);
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
                        }
                    }

                    //var parameters = filterContext.ActionDescriptor.GetParameters();
                    //var values = string.Join("&", parameters.Select(s => s.ParameterName + "=" + filterContext.Controller.ValueProvider.GetValue(s.ParameterName).AttemptedValue));

                    bool IsValidSession = false;

                    if (IQMedia.Web.Common.Authentication.CurrentUser != null)
                    {
                        IsValidSession = IQMedia.WebApplication.Utility.ActiveUserMgr.IsUserInCacheBySessionID();
                    }

                    if (!IsValidSession || !Authentication.IsAuthenticated)
                    {
                        try
                        {
                            IQLog_UserActionsModel debugAction = new IQLog_UserActionsModel();

                            debugAction.IPAddress = filterContext.HttpContext.Request.Headers["X-ClientIP"] + " - " + filterContext.HttpContext.Request.UserHostAddress;
                            debugAction.PageName = "InvalidSession";
                            debugAction.ActionName = filterContext.HttpContext.Request.Url.ToString();
                            debugAction.RequestDateTime = filterContext.HttpContext.Timestamp;
                            debugAction.RequestParameters = filterContext.HttpContext.Request.Headers["Cookie"] + " Cache: " + IsValidSession.ToString() + " Authentication: " + Authentication.IsAuthenticated.ToString();
                            debugAction.SessionID = HttpContext.Current.Session.SessionID;

                            UtilityLogic.InsertActionLog(debugAction);
                        }
                        catch (Exception)
                        {


                        }

                        IQMedia.WebApplication.Utility.ActiveUserMgr.RemoveUserFromCacheBySessionID();

                        if (!filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            filterContext.Result = new RedirectResult("~/Home/Index");
                        }
                        else
                        {
                            var result = new JsonResult();
                            Authrorized authrorized = new Authrorized();

                            authrorized.isAuthorized = false;
                            authrorized.isSuccess = false;
                            authrorized.redirectURL = "../Home/Index";
                            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                            result.Data = authrorized;

                            filterContext.Result = result;
                        }
                        if (!IsValidSession)
                        {
                            IQMedia.Web.Common.Authentication.Logout();
                        }
                    }
                    else
                    {
                        _CustomerID = sessionInformation.CustomerKey;

                        if (!(sessionInformation.AuthorizedVersion == 0 || sessionInformation.AuthorizedVersion == 4))
                        {
                            IQMedia.WebApplication.Utility.ActiveUserMgr.RemoveUserFromCacheBySessionID();
                            IQMedia.Web.Common.Authentication.Logout();
                        }
                        else
                        {
                            IQMedia.WebApplication.Utility.ActiveUserMgr.UpdateUserIntoCache();

                            if (!CheckForAuthorization(controllerName))
                            {
                                filterContext.Result = new RedirectResult("~/Error/Unauthorized");
                            }
                        }
                    }

                    if (iQLog_UserActionsModel != null)
                    {
                        iQLog_UserActionsModel.SessionID = HttpContext.Current.Session.SessionID;
                        iQLog_UserActionsModel.CustomerID = _CustomerID;
                        UtilityLogic.InsertActionLog(iQLog_UserActionsModel);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            try
            {
                var cancel = filterContext.Canceled;
                var ex = filterContext.Exception;
                var exh = filterContext.ExceptionHandled;
                var r = filterContext.RouteData;

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["SessionDebug"]))
                {
                    Log4NetLogger.Debug(" IP: " + filterContext.HttpContext.Request.Headers["X-ClientIP"] + " - " + filterContext.HttpContext.Request.UserHostAddress + " SessionID: " + HttpContext.Current.Session.SessionID + " Url: " + filterContext.HttpContext.Request.Url.ToString() + "Req Cookies: " + filterContext.HttpContext.Request.Headers["Cookie"] + "Res Cookies" + filterContext.HttpContext.Response.Headers["Set-Cookie"]);
                }
            }
            catch (Exception ex)
            {

            }

        }

        private bool CheckForAuthorization(string controllerName)
        {
            if (!string.IsNullOrEmpty(controllerName))
            {
                ActiveUser session = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                if (controllerName.ToUpper() == "FEEDS" && !session.Isv4FeedsAccess)
                {
                    return false;
                }
                if (controllerName.ToUpper() == "DISCOVERY" && !session.Isv4DiscoveryAccess)
                {
                    return false;
                }
                if (controllerName.ToUpper() == "LIBRARY" && !session.Isv4LibraryAccess)
                {
                    return false;
                }
                if (controllerName.ToUpper() == "TIMESHIFT" && !session.Isv4TimeshiftAccess)
                {
                    return false;
                }
                if (controllerName.ToUpper() == "TADS" && !session.Isv4TAdsAccess)
                {
                    return false;
                }
                if (controllerName.ToUpper() == "ANALYTICS" && !session.Isv5AnalyticsAccess)
                {
                    return false;
                }
                if (controllerName.ToUpper() == "IMAGIQ" && !session.isv5LRAccess && !session.ClientGUID.Equals(new Guid("7722A116-C3BC-40AE-8070-8C59EE9E3D2A")))
                {
                    return false;
                }
                if (controllerName.ToUpper() == "DASHBOARD" && !(session.Isv4DashboardAccess || session.Isv4LibraryDashboardAccess))
                {
                    return false;
                }
                if (controllerName.ToUpper() == "SETUP" && !session.Isv4SetupAccess)
                {
                    return false;
                }
                if (controllerName.ToUpper() == "GLOBALADMIN" && !session.IsGlobalAdminAccess)
                {
                    return false;
                }
                if (controllerName.ToUpper() == "GROUP" && !session.Isv4Group)
                {
                    return false;
                }
                return true;
            }
            else
                return false;
        }

        public class Authrorized
        {
            public bool isSuccess { get; set; }
            public bool isAuthorized { get; set; }
            public string redirectURL { get; set; }
        }

    }
}
