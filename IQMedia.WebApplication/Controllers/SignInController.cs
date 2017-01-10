using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Model;
using IQMedia.Web.Common;
using IQMedia.Web.Logic.Base;
using IQMedia.WebApplication.Utility;
using System.Configuration;
using IQMedia.WebApplication.Config;
using IQMedia.Common.Util;
using System.Security.Authentication;
using System.Security.Principal;

namespace IQMedia.WebApplication.Controllers
{
    public class SignInController : Controller
    {
        //
        // GET: /SignIn/

        public ActionResult Index()
        {
            try
            {
                IQMedia.Shared.Utility.Log4NetLogger.Debug("Start");

                if (ConfigurationManager.AppSettings["IsMaintenance"] == "true")
                {
                    return RedirectToAction("Index", "UnderMaintenance");
                }
                else if (IQMedia.WebApplication.Utility.ActiveUserMgr.CheckAuthentication())
                {
                    var user = ActiveUserMgr.GetActiveUser();

                    if (ConfigurationManager.AppSettings["UseInfoAlert"] == "true")
                    {
                        return RedirectToAction("Index", "InfoAlert");
                    }
                    else
                    {
                        return RedirectToDefaultPage(user.DefaultPage);
                    }
                }
                else
                {
                    if (Authentication.IsAuthenticated)
                    {
                        Authentication.Logout();
                        HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                    }

                    Session.Timeout = 1;
                }
            }
            catch (Exception exception)
            {
                ViewBag.IsSuccess = false;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
            }

            return View();
        }

        [HttpPost]        
        public ActionResult Index(CustomerModel customer)
        {
            System.Web.Configuration.SessionStateSection sessionStateSection = (System.Web.Configuration.SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");

            var cookieName = sessionStateSection.CookieName;
            var sessionCookieVal = Request.Cookies[cookieName].Value;

            if (!Session.IsNewSession)
            {
                Session.Timeout = 1;
                ViewBag.IsSuccess = false;
                ViewBag.Message = ConfigSettings.Settings.ErrorOccurred;
                ViewBag.Status = -3;

                var msg = "Session Cookie is not empty " + sessionCookieVal;

                IQMedia.Shared.Utility.Log4NetLogger.Fatal(msg);
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(new Exception(msg));

                return View();
            }
            else
            {
                ViewBag.Message = string.Empty;
                ViewBag.IsSuccess = true;
                ViewBag.Status = 0;

                if (ModelState.IsValid)
                {
                    try
                    {
                        customer.LoginID = customer.LoginID.Trim();
                        customer.Password = customer.Password.Trim();

                        CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                        bool isAuthenticated = customerLogic.CheckAuthentication(customer.LoginID, customer.Password, Convert.ToInt32(ConfigurationManager.AppSettings["MaxPasswordAttempts"]));

                        if (isAuthenticated)
                        {
                            CustomerModel customerValue = customerLogic.GetCustomerDetailsByLoginID(customer.LoginID);

                            if (customerValue != null && !string.IsNullOrWhiteSpace(customerValue.FirstName))
                            {
                                if (customerValue.AuthorizedVersion.HasValue && (customerValue.AuthorizedVersion == 0 || customerValue.AuthorizedVersion == 4))
                                {

                                    bool _IsLogin = false;

                                    var result = IQMedia.Web.Common.Authentication.Login(customer.LoginID, customer.Password);

                                    if (result == IQMedia.Web.Common.Authentication.LoginStatus.Success)
                                    {
                                        _IsLogin = true;
                                    }
                                    else
                                    {
                                        Shared.Utility.Log4NetLogger.Info("login failed for user :" + customer.LoginID.Trim());
                                    }

                                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["SessionDebug"]))
                                    {
                                        Shared.Utility.Log4NetLogger.Debug(" IP: " + HttpContext.Request.Headers["X-ClientIP"] + " - " + HttpContext.Request.UserHostAddress + " SessionID: " + Session.SessionID + " Url: " + HttpContext.Request.Url.ToString() + "Req Cookies: " + HttpContext.Request.Headers["Cookie"] + "Res Cookies" + HttpContext.Response.Headers["Set-Cookie"] + " SignIn");
                                    }

                                    if (_IsLogin)
                                    {
                                        List<CustomerRoleModel> customerRoles = customerLogic.GetCustomerRoles(customerValue.CustomerGUID);
                                        IQMedia.WebApplication.Utility.CommonFunctions.FillCustomerRoles(customerValue, customerRoles);

                                        ActiveUser sessionInformation = new ActiveUser();
                                        sessionInformation.ClientGUID = customerValue.ClientGUID;
                                        sessionInformation.ClientID = customerValue.ClientID;
                                        sessionInformation.ClientName = customerValue.ClientName;
                                        sessionInformation.ClientPlayerLogoImage = customerValue.ClientPlayerLogoImage;
                                        sessionInformation.CustomerKey = customerValue.CustomerKey;
                                        sessionInformation.Email = customerValue.Email;
                                        sessionInformation.LoginID = customerValue.LoginID.Trim();
                                        sessionInformation.FirstName = customerValue.FirstName;
                                        sessionInformation.LastName = customerValue.LastName;
                                        sessionInformation.IsClientPlayerLogoActive = customerValue.IsClientPlayerLogoActive;
                                        sessionInformation.IsLogIn = true;
                                        sessionInformation.IsUgcAutoClip = customerValue.IsUGCAutoClip;
                                        sessionInformation.MultiLogin = customerValue.MultiLogin;
                                        sessionInformation.CustomerGUID = customerValue.CustomerGUID;
                                        sessionInformation.AuthorizedVersion = customerValue.AuthorizedVersion;
                                        sessionInformation.Isv4Group = customerValue.Isv4Group;
                                        sessionInformation.DefaultPage = customerValue.DefaultPage;
                                        sessionInformation.TimeZone = customerValue.TimeZone;
                                        sessionInformation.dst = customerValue.dst;
                                        sessionInformation.gmt = customerValue.gmt;
                                        sessionInformation.MCID = customerValue.MCID;
                                        sessionInformation.MasterCustomerID = customerValue.MasterCustomerID == null ? customerValue.CustomerKey : customerValue.MasterCustomerID.Value;
                                        sessionInformation.MediaTypes = IQCommon.CommonFunctions.GetMediaTypes(customerValue.CustomerGUID);

                                        // Customer Roles Information

                                        sessionInformation.Isv4FeedsAccess = customerValue.Isv4FeedsAccess;
                                        sessionInformation.Isv4DiscoveryAccess = customerValue.Isv4DiscoveryAccess;
                                        sessionInformation.Isv4LibraryAccess = customerValue.Isv4LibraryAccess;
                                        sessionInformation.Isv4TimeshiftAccess = customerValue.Isv4TimeshiftAccess;
                                        sessionInformation.Isv4TAdsAccess = customerValue.Isv4TAdsAccess;
                                        sessionInformation.Isv5AdsAccess = customerValue.Isv5AdsAccess;
                                        sessionInformation.Isv5AnalyticsAccess = customerValue.Isv5AnalyticsAccess;
                                        sessionInformation.Isv4DashboardAccess = customerValue.Isv4DashboardAccess;
                                        sessionInformation.Isv4LibraryDashboardAccess = customerValue.Isv4LibraryDashboardAccess;
                                        sessionInformation.Isv4TimeshiftRadioAccess = customerValue.Isv4TimeshiftRadioAccess;
                                        sessionInformation.Isv4SetupAccess = customerValue.Isv4SetupAccess;
                                        sessionInformation.isv5LRAccess = customerValue.isv5LRAccess;
                                        sessionInformation.IsGlobalAdminAccess = customerValue.IsGlobalAdminAccess;
                                        sessionInformation.Isv4UGCAccess = customerValue.Isv4UGCAccess;
                                        sessionInformation.Isv4UGCDownload = customerValue.IsUGCDownload;
                                        sessionInformation.Isv4UGCUploadEdit = customerValue.IsUGCUploadEdit;
                                        sessionInformation.Isv4IQAgentAccess = customerValue.Isv4IQAgentAccess;
                                        sessionInformation.Isv4TV = customerValue.Isv4TV;
                                        sessionInformation.Isv4NM = customerValue.Isv4NM;
                                        sessionInformation.Isv4SM = customerValue.Isv4SM;
                                        sessionInformation.Isv4TW = customerValue.Isv4TW;
                                        sessionInformation.Isv4TM = customerValue.Isv4TM;
                                        sessionInformation.Isv4CustomImage = customerValue.Isv4CustomImage;
                                        sessionInformation.IsCompeteData = customerValue.IsCompeteData;
                                        sessionInformation.IsNielsenData = customerValue.IsNielsenData;
                                        sessionInformation.Isv4BLPM = customerValue.Isv4BLPM;
                                        sessionInformation.IsNewsRights = customerValue.IsNewsRights;
                                        sessionInformation.Isv4CustomSettings = customerValue.Isv4CustomSettings;
                                        sessionInformation.Isv4DiscoveryLiteAccess = customerValue.Isv4DiscoveryLiteAccess;
                                        sessionInformation.IsfliQAdmin = customerValue.IsfliQAdmin;
                                        sessionInformation.Isv4PQ = customerValue.Isv4PQ;
                                        sessionInformation.IsMediaRoomContributor = customerValue.IsMediaRoomContributor;
                                        sessionInformation.IsMediaRoomEditor = customerValue.IsMediaRoomEditor;
                                        sessionInformation.Isv4Google = customerValue.Isv4Google;
                                        sessionInformation.IsTimeshiftFacet = customerValue.IsTimeshiftFacet;
                                        sessionInformation.IsShareTV = customerValue.IsShareTV;
                                        sessionInformation.IsSMOther = customerValue.IsSMOther;
                                        sessionInformation.IsFB = customerValue.IsFB;
                                        sessionInformation.IsIG = customerValue.IsIG;
                                        sessionInformation.IsBL = customerValue.IsBL;
                                        sessionInformation.IsFO = customerValue.IsFO;
                                        sessionInformation.IsPR = customerValue.IsPR;
                                        sessionInformation.IsLN = customerValue.IsLN;
                                        sessionInformation.IsThirdPartyData = customerValue.IsThirdPartyData;
                                        sessionInformation.IsClientSpecificData = customerValue.IsClientSpecificData;
                                        sessionInformation.IsConnectAccess = customerValue.IsConnectAccess;
                                        sessionInformation.IsExternalRuleEditor = customerValue.IsExternalRuleEditor;

                                        IQMedia.WebApplication.Utility.ActiveUserMgr.AddUserIntoCache(sessionInformation);

                                        if (ConfigurationManager.AppSettings["UseInfoAlert"] == "true")
                                        {
                                            return RedirectToAction("Index", "InfoAlert");
                                        }
                                        else
                                        {
                                            return RedirectToDefaultPage(sessionInformation.DefaultPage);// RedirectToAction("Index", "Dashboard");
                                        }
                                    }
                                    /*}
                                    else
                                    {
                                        ViewBag.Message = ConfigSettings.Settings.AlreadyLoggedIn;// "You are already Logged In";
                                        return View();
                                    }*/

                                }
                                else
                                {
                                    Shared.Utility.Log4NetLogger.Info("User do not have access to website ,  user email :" + customer.LoginID.Trim());
                                    return RedirectToAction("Unauthorized", "Error");
                                }
                            }
                        }
                        else
                        {
                            Shared.Utility.Log4NetLogger.Info("username and password  do not match for :" + customer.LoginID.Trim());
                        }

                        ViewBag.Message = ConfigSettings.Settings.CredentialNotCorrect; //"Oops! the email and/or password is not correct.";
                        return View();
                    }
                    catch (AuthenticationException ex)
                    {
                        Shared.Utility.Log4NetLogger.Error(ex);
                        IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                        ViewBag.IsSuccess = false;
                        ViewBag.Message = ex.Message;
                        ViewBag.Status = -1;
                        return View();
                    }
                    catch (Exception ex)
                    {
                        Shared.Utility.Log4NetLogger.Fatal("An error occured while login", ex);
                        IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                        ViewBag.IsSuccess = false;
                        ViewBag.Message = ConfigSettings.Settings.ErrorOccurred;
                        ViewBag.Status = -3;
                        return View();
                    }
                    //return RedirectToAction("Index", "SignIn");


                }
                else
                {
                    return View();
                }
            }

        }


        #region Custom Methods



        public ActionResult RedirectToDefaultPage(string p_DefaultPage)
        {
            if (Enum.IsDefined(typeof(IQMedia.Shared.Utility.CommonFunctions.DefaultPage), p_DefaultPage))
            {
                ActiveUser _SessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                switch ((IQMedia.Shared.Utility.CommonFunctions.DefaultPage)Enum.Parse(typeof(IQMedia.Shared.Utility.CommonFunctions.DefaultPage), p_DefaultPage))
                {
                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Dashboard:
                        if (_SessionInformation.Isv4DashboardAccess)
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Feeds:
                        if (_SessionInformation.Isv4FeedsAccess)
                        {
                            return RedirectToAction("Index", "Feeds");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Discovery:
                        if (_SessionInformation.Isv4DiscoveryAccess)
                        {
                            return RedirectToAction("Index", "Discovery");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }
                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4DiscoveryLite:
                        if (_SessionInformation.Isv4DiscoveryLiteAccess)
                        {
                            return RedirectToAction("Index", "DiscoveryLite");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }
                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Timeshift:
                        if (_SessionInformation.Isv4TimeshiftAccess)
                        {
                            return RedirectToAction("Index", "Timeshift");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4TAds:
                        if (_SessionInformation.Isv4TAdsAccess)
                        {
                            return RedirectToAction("Index", "TAds");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v5Analytics:
                        if (_SessionInformation.Isv5AnalyticsAccess)
                        {
                            return RedirectToAction("Index", "Analytics");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v5LR:
                        if (_SessionInformation.isv5LRAccess && _SessionInformation.ClientGUID.Equals(new Guid("7722A116-C3BC-40AE-8070-8C59EE9E3D2A")))
                        {
                            return RedirectToAction("Index", "ImagiQ");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Library:
                        if (_SessionInformation.Isv4LibraryAccess)
                        {
                            return RedirectToAction("Index", "Library");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Setup:
                        if (_SessionInformation.Isv4SetupAccess)
                        {
                            return RedirectToAction("Index", "Setup");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4GlobalAdmin:
                        if (_SessionInformation.ClientGUID.Equals(new Guid("7722a116-c3bc-40ae-8070-8c59ee9e3d2a")) && _SessionInformation.IsGlobalAdminAccess)
                        {
                            return RedirectToAction("Index", "GlobalAdmin");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    default:
                        return GetDefaultPageByAccessRights();

                }
            }
            else
            {
                return GetDefaultPageByAccessRights();
            }
        }

        public ActionResult GetDefaultPageByAccessRights()
        {
            ActiveUser _SessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
            if (_SessionInformation.Isv4DashboardAccess)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else if (_SessionInformation.Isv4FeedsAccess)
            {
                return RedirectToAction("Index", "Feeds");
            }
            else if (_SessionInformation.Isv4DiscoveryAccess)
            {
                return RedirectToAction("Index", "Discovery");
            }
            else if (_SessionInformation.Isv4DiscoveryLiteAccess)
            {
                return RedirectToAction("Index", "DiscoveryLite");
            }
            else if (_SessionInformation.Isv4TimeshiftAccess)
            {
                return RedirectToAction("Index", "Timeshift");
            }
            else if (_SessionInformation.Isv4TAdsAccess)
            {
                return RedirectToAction("Index", "TAds");
            }
            else if (_SessionInformation.Isv5AnalyticsAccess)
            {
                return RedirectToAction("Index", "Analytics");
            }
            else if (_SessionInformation.isv5LRAccess)
            {
                return RedirectToAction("Index", "ImagiQ");
            }
            else if (_SessionInformation.Isv4LibraryAccess)
            {
                return RedirectToAction("Index", "Library");
            }
            else if (_SessionInformation.Isv4SetupAccess)
            {
                return RedirectToAction("Index", "Setup");
            }
            else if (_SessionInformation.ClientGUID.Equals(new Guid("7722a116-c3bc-40ae-8070-8c59ee9e3d2a")) && _SessionInformation.IsGlobalAdminAccess)
            {
                return RedirectToAction("Index", "GlobalAdmin");
            }
            else if (_SessionInformation.MCID.HasValue && _SessionInformation.Isv4Group)
            {
                return RedirectToAction("Index", "Group");
            }
            else
            {
                return RedirectToAction("Unauthorized", "Error");
            }
        }

        #endregion

    }
}
