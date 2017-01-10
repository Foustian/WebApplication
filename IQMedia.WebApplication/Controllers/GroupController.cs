using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Model;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.WebApplication.Utility;
using IQMedia.WebApplication.Config;
using IQMedia.Web.Common;
using System.Configuration;
using IQMedia.Common.Util;
using System.IO;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class GroupController : Controller
    {
        //
        // GET: /Group/

        ActiveUser sessionInformation = null;
        public string PATH_Group_ClientListPartialView = "~/Views/Group/_ClientList.cshtml";

        public ActionResult Index()
        {
            try
            {
                List<ClientModel> lstClientModel = GetAllClientsByCustomer(null, true);
                ViewBag.IsSuccess = true;
                return View(lstClientModel);
            }
            catch (Exception exception)
            {
                ViewBag.IsSuccess = false;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return View();
            }
        }

        public JsonResult GetClients(string p_ClientName, bool p_IsAsc)
        {
            try
            {
                List<ClientModel> lstClientModel = GetAllClientsByCustomer(p_ClientName, p_IsAsc);
                return Json(new
                {
                    HTML = RenderPartialToString(PATH_Group_ClientListPartialView, lstClientModel),
                    isSuccess = true
                });

            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false
                });
            }
        }

        public List<ClientModel> GetAllClientsByCustomer(string p_ClientName, bool p_IsAsc)
        {
            sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
            ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
            return clientLogic.GetAllClientByCustomerAndMasterClient(sessionInformation.CustomerKey, sessionInformation.MCID.Value, p_ClientName, p_IsAsc);
        }

        [HttpPost]
        public ActionResult Index(Int64 p_ClientID)
        {
            try
            {
                ViewBag.Message = string.Empty;
                if (Authentication.IsAuthenticated)
                {
                    sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                    CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                    CustomerModel customerValue = customerLogic.CheckAuthenticationByClient(sessionInformation.MasterCustomerID, p_ClientID);
                    if (customerValue != null && !string.IsNullOrWhiteSpace(customerValue.FirstName))
                    {
                        if (customerValue.AuthorizedVersion.HasValue && (customerValue.AuthorizedVersion == 0 || customerValue.AuthorizedVersion == 4))
                        {
                            IQMedia.Web.Common.Authentication.LogoutWSA();
                            bool _IsLogin = false;

                            var result = IQMedia.Web.Common.Authentication.LoginWithHashPwd(customerValue.LoginID, customerValue.Password);

                            if (result == IQMedia.Web.Common.Authentication.LoginStatus.Success)
                            {
                                _IsLogin = true;
                            }
                            else
                            {
                                Shared.Utility.Log4NetLogger.Info("login failed for user :" + customerValue.LoginID);
                            }

                            if (_IsLogin)
                            {
                                List<CustomerRoleModel> customerRoles = customerLogic.GetCustomerRoles(customerValue.CustomerGUID);
                                IQMedia.WebApplication.Utility.CommonFunctions.FillCustomerRoles(customerValue, customerRoles);

                                sessionInformation = new ActiveUser();
                                sessionInformation.ClientGUID = customerValue.ClientGUID;
                                sessionInformation.ClientID = customerValue.ClientID;
                                sessionInformation.ClientName = customerValue.ClientName;
                                sessionInformation.ClientPlayerLogoImage = customerValue.ClientPlayerLogoImage;
                                sessionInformation.CustomerKey = customerValue.CustomerKey;
                                sessionInformation.Email = customerValue.Email;
                                sessionInformation.LoginID = customerValue.LoginID;
                                sessionInformation.FirstName = customerValue.FirstName;
                                sessionInformation.LastName = customerValue.LastName;
                                sessionInformation.IsClientPlayerLogoActive = customerValue.IsClientPlayerLogoActive;
                                sessionInformation.IsLogIn = true;
                                sessionInformation.IsUgcAutoClip = customerValue.IsUGCAutoClip;
                                sessionInformation.MultiLogin = customerValue.MultiLogin;
                                sessionInformation.CustomerGUID = customerValue.CustomerGUID;
                                sessionInformation.AuthorizedVersion = customerValue.AuthorizedVersion;
                                sessionInformation.DefaultPage = customerValue.DefaultPage;
                                sessionInformation.TimeZone = customerValue.TimeZone;
                                sessionInformation.dst = customerValue.dst;
                                sessionInformation.gmt = customerValue.gmt;
                                sessionInformation.MCID = customerValue.MCID.HasValue ? customerValue.MCID : (int?)null;
                                sessionInformation.Isv4Group = customerValue.Isv4Group;
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

                                return RedirectToDefaultPage(sessionInformation);// RedirectToAction("Index", "Dashboard");
                            }
                            else
                            {
                                ViewBag.Message = ConfigSettings.Settings.LoginFailed;
                            }

                        }
                        else if (customerValue.AuthorizedVersion.HasValue && customerValue.AuthorizedVersion.Value == 3)
                        {
                            Response.Redirect(ConfigurationManager.AppSettings["v3WebsiteUrl"]);
                        }
                        else
                        {
                            Shared.Utility.Log4NetLogger.Info("User do not have access to website ,  user email :" + customerValue.LoginID);
                            return RedirectToAction("Unauthorized", "Error");
                        }
                    }
                    else
                    {
                        Shared.Utility.Log4NetLogger.Info("username and password  do not match for :" + customerValue.LoginID);
                        ViewBag.Message = ConfigSettings.Settings.CredentialNotCorrect;
                    }
                }

                List<ClientModel> lstClientModel = GetAllClientsByCustomer(null, false);

                return View(lstClientModel);

            }
            catch (Exception _ex)
            {
                ViewBag.Message = ConfigSettings.Settings.ErrorOccurred;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                return View();
            }
        }

        public ActionResult RedirectToDefaultPage(ActiveUser _SessionInformation)
        {

            if (!string.IsNullOrEmpty(_SessionInformation.DefaultPage) && Enum.IsDefined(typeof(IQMedia.Shared.Utility.CommonFunctions.DefaultPage), _SessionInformation.DefaultPage))
            {
                switch ((IQMedia.Shared.Utility.CommonFunctions.DefaultPage)Enum.Parse(typeof(IQMedia.Shared.Utility.CommonFunctions.DefaultPage), _SessionInformation.DefaultPage))
                {
                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Dashboard:
                        if (_SessionInformation.Isv4DashboardAccess)
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights(_SessionInformation);
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Feeds:
                        if (_SessionInformation.Isv4FeedsAccess)
                        {
                            return RedirectToAction("Index", "Feeds");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights(_SessionInformation);
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Discovery:
                        if (_SessionInformation.Isv4DiscoveryAccess)
                        {
                            return RedirectToAction("Index", "Discovery");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights(_SessionInformation);
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Timeshift:
                        if (_SessionInformation.Isv4TimeshiftAccess)
                        {
                            return RedirectToAction("Index", "Timeshift");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights(_SessionInformation);
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4TAds:
                        if (_SessionInformation.Isv4TAdsAccess)
                        {
                            return RedirectToAction("Index", "TAds");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights(_SessionInformation);
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v5Analytics:
                        if (_SessionInformation.Isv5AnalyticsAccess)
                        {
                            return RedirectToAction("Index", "Analytics");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights(_SessionInformation);
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v5LR:
                        if (_SessionInformation.isv5LRAccess)
                        {
                            return RedirectToAction("Index", "ImagiQ");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights(_SessionInformation);
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Library:
                        if (_SessionInformation.Isv4LibraryAccess)
                        {
                            return RedirectToAction("Index", "Library");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights(_SessionInformation);
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Setup:
                        if (_SessionInformation.Isv4SetupAccess)
                        {
                            return RedirectToAction("Index", "Setup");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights(_SessionInformation);
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4GlobalAdmin:
                        if (sessionInformation.ClientGUID.Equals(new Guid("7722a116-c3bc-40ae-8070-8c59ee9e3d2a")) && _SessionInformation.IsGlobalAdminAccess)
                        {
                            return RedirectToAction("Index", "GlobalAdmin");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights(_SessionInformation);
                        }

                    default:
                        return GetDefaultPageByAccessRights(_SessionInformation);

                }
            }
            else
            {
                return GetDefaultPageByAccessRights(_SessionInformation);
            }
        }

        public ActionResult GetDefaultPageByAccessRights(ActiveUser _SessionInformation)
        {            
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
            else if (_SessionInformation.isv5LRAccess && _SessionInformation.ClientGUID.Equals(new Guid("7722A116-C3BC-40AE-8070-8C59EE9E3D2A")))
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
            else if (sessionInformation.ClientGUID.Equals(new Guid("7722a116-c3bc-40ae-8070-8c59ee9e3d2a")) && _SessionInformation.IsGlobalAdminAccess)
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

        #region Utility

        public string RenderPartialToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion

    }
}
