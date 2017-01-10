using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Model;
using System.Configuration;
using IQMedia.WebApplication.Models.TempData;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.Common.Util;
using IQMedia.WebApplication.Config;
using System.IO;
using IQMedia.Web.Common;
using IQMedia.WebApplication.Utility;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class SonyMediaController : Controller
    {
        //
        // GET: /SonyMedia/
        ActiveUser sessionInformation = null;
        KantorMediaTempData sonyMediaTempData = null;
        string PATH_SonyMediaPartialView = "~/Views/SonyMedia/_TVResults.cshtml";
        bool hasMoreResultPage = false;
        bool hasPreviousResultPage = false;
        string recordNumberDesc = string.Empty;
        public ActionResult Index()
        {
            try
            {
                ViewBag.IsSuccess = true;
                ViewBag.ErrorMessage = "";
                if (ConfigurationManager.AppSettings["IsMaintenance"] == "true")
                {
                    return RedirectToAction("Index", "UnderMaintenance");
                }
                else
                {
                    bool isUserAlreadyAuthenticated = ActiveUserMgr.CheckAuthentication();
                    if (isUserAlreadyAuthenticated)
                    {
                        sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                        if (sessionInformation.Isv4TV)
                        {
                            SetTempData(null);
                            sonyMediaTempData = GetTempData();
                            sonyMediaTempData.TotalResults = null;



                            ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                            //Add TV Country filter
                            //sonyMediaTempData.IQTVRegion = clientLogic.GetClientTVRegionSettings(sessionInformation.ClientGUID);
                            var IQTVRegionCountry = clientLogic.GetClientTVRegionSettings(sessionInformation.ClientGUID);
                            if (IQTVRegionCountry != null && IQTVRegionCountry.Count > 0)
                            {
                                sonyMediaTempData.IQTVRegion = (List<int>)IQTVRegionCountry[0];
                            }
                            if (IQTVRegionCountry != null && IQTVRegionCountry.Count > 1)
                            {
                                sonyMediaTempData.IQTVCountry = (List<int>)IQTVRegionCountry[1];
                            }

                            int manualClipDuration = clientLogic.GetClientManualClipDurationSettings(sessionInformation.ClientGUID);
                            sonyMediaTempData.ManualClipDuration = manualClipDuration;

                            SetTempData(sonyMediaTempData);

                            IQ_QVCDataLogic iQ_KantorDataLogic = (IQ_QVCDataLogic)LogicFactory.GetLogic(LogicType.KantorData);
                            return View(iQ_KantorDataLogic.GetQVCStations(Convert.ToInt16(Shared.Utility.CommonFunctions.KantarMediaTypes.Sony)));
                        }
                        else
                        {
                            return RedirectToAction("Unauthorized", "Error");
                        }
                    }
                    else
                    {
                        return RedirectToAction("SignIn", "SonyMedia");
                    }
                }
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                ViewBag.IsSuccess = false;
                return View();
            }
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            bool isUserAlreadyAuthenticated = ActiveUserMgr.CheckAuthentication();
            if (!isUserAlreadyAuthenticated)
                return View();
            else
                return RedirectToAction("Index", "SonyMedia");
        }

        [HttpPost]
        public ActionResult SignIn(CustomerModel customer)
        {
            ViewBag.Message = string.Empty;
            ViewBag.IsSuccess = true;
            if (ModelState.IsValid)
            {
                try
                {
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
                                    Shared.Utility.Log4NetLogger.Info("login failed for user :" + customer.LoginID);
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
                                    sessionInformation.LoginID = customerValue.LoginID;
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

                                    return RedirectToAction("Index", "SonyMedia");
                                }
                                /*}
                                else
                                {
                                    ViewBag.Message = ConfigSettings.Settings.AlreadyLoggedIn;// "You are already Logged In";
                                    return View();
                                }*/

                            }
                            else if (customerValue.AuthorizedVersion.HasValue && customerValue.AuthorizedVersion.Value == 3)
                            {
                                Response.Redirect(ConfigurationManager.AppSettings["v3WebsiteUrl"]);
                            }
                            else
                            {
                                Shared.Utility.Log4NetLogger.Info("User do not have access to website ,  user email :" + customer.LoginID);
                                return RedirectToAction("Unauthorized", "Error");
                            }
                        }
                        else
                        {
                            Shared.Utility.Log4NetLogger.Info("username and password  do not match for :" + customer.LoginID);
                        }
                    }
                    ViewBag.Message = ConfigSettings.Settings.CredentialNotCorrect; //"Oops! the email and/or password is not correct.";
                    return View();
                }
                catch (Exception ex)
                {
                    TempData.Keep("SonyMediaTempData");
                    Shared.Utility.Log4NetLogger.Fatal("An error occured while login", ex);
                    ViewBag.IsSuccess = false;
                    ViewBag.Message = ConfigSettings.Settings.ErrorOccurred; //"Oops! the email and/or password is not correct.";
                    return View();
                }
                //return RedirectToAction("Index", "Home");


            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public JsonResult SonyMediaSearchResults(string p_StationID, DateTime p_Date)
        {
            try
            {
                sonyMediaTempData = GetTempData();

                IQ_QVCDataLogic iQ_KantorDataLogic = (IQ_QVCDataLogic)LogicFactory.GetLogic(LogicType.KantorData);
                List<string> lstIQ_CC_KEy = iQ_KantorDataLogic.GetQVCIQ_CC_Keys(p_StationID, p_Date, Convert.ToInt16(Shared.Utility.CommonFunctions.KantarMediaTypes.Sony));
                sonyMediaTempData.IQ_CC_Key = lstIQ_CC_KEy;


                SetTempData(sonyMediaTempData);

                return SonySearchResults(0);
            }
            catch (Exception ex)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("SonyMediaTempData");
            }
            //return Json(new object());
        }

        [HttpPost]
        public JsonResult SonyMediaSearchResultsPaging(bool p_IsNext)
        {
            try
            {
                sonyMediaTempData = GetTempData();
                int PageNumber = 0;
                if (p_IsNext)
                {
                    if (sonyMediaTempData.HasMoreResultPage)
                    {
                        if (sonyMediaTempData.PageNumber != null)
                        {
                            PageNumber = sonyMediaTempData.PageNumber + 1;
                        }
                        else
                        {
                            PageNumber = 1;
                        }
                    }
                    else
                    {
                        // IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                        return Json(new
                        {
                            isSuccess = false
                        });
                    }
                }
                else
                {
                    if (sonyMediaTempData.HasPreviousResultPage)
                    {
                        if (sonyMediaTempData.PageNumber != null)
                        {
                            PageNumber = sonyMediaTempData.PageNumber - 1;
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            isSuccess = false
                        });
                    }

                }

                return SonySearchResults(PageNumber);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("SonyMediaTempData");
            }
        }

        [HttpPost]
        public JsonResult GetChart(string IQ_CC_KEY)
        {
            try
            {
                IQ_QVCDataLogic iQ_KantorDataLogic = (IQ_QVCDataLogic)LogicFactory.GetLogic(LogicType.KantorData);
                string audienceData = iQ_KantorDataLogic.GetQVCAudienceDataByIQ_CC_Key(IQ_CC_KEY, Convert.ToInt16(Shared.Utility.CommonFunctions.KantarMediaTypes.Sony));

                string jsonChartData = iQ_KantorDataLogic.KantorHighLineChart(audienceData, true);

                return Json(new
                {
                    lineChartJson = jsonChartData,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("SonyMediaTempData");
            }
        }

        public JsonResult SonySearchResults(int PageNumber)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                sonyMediaTempData = GetTempData();
                sonyMediaTempData.PageNumber = PageNumber;
                int ResultCount = 0;

                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["KantarPageSize"].ToString());



                List<IQAgent_TVResultsModel> tvResult = new List<IQAgent_TVResultsModel>();
                if (sonyMediaTempData.IQ_CC_Key != null && sonyMediaTempData.IQ_CC_Key.Count > 0)
                {
                    TVLogic tvlogic = (TVLogic)LogicFactory.GetLogic(LogicType.TV);
                    tvResult = tvlogic.KantorSearchResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, sonyMediaTempData.IQ_CC_Key, PageSize, sonyMediaTempData.PageNumber, ref ResultCount, IQMedia.WebApplication.Utility.CommonFunctions.GeneratePMGUrl(IQMedia.WebApplication.Utility.CommonFunctions.PMGUrlType.TV.ToString(), null, null), sonyMediaTempData.IQTVRegion, sonyMediaTempData.IQTVCountry);
                }
                sonyMediaTempData.ResultCount = ResultCount;

                hasMoreResultPage = false;
                hasPreviousResultPage = false;

                if (tvResult != null && ResultCount > 0)
                {
                    double totalHit = Convert.ToDouble(ResultCount) / PageSize;

                    if (totalHit > (sonyMediaTempData.PageNumber + 1))
                    {
                        hasMoreResultPage = true;
                        recordNumberDesc = (((sonyMediaTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ((sonyMediaTempData.PageNumber + 1) * PageSize).ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    else
                    {
                        hasMoreResultPage = false;
                        recordNumberDesc = (((sonyMediaTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ResultCount.ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                }
                if (sonyMediaTempData.PageNumber > 0)
                {
                    hasPreviousResultPage = true;
                }
                sonyMediaTempData.HasMoreResultPage = hasMoreResultPage;
                sonyMediaTempData.HasPreviousResultPage = hasPreviousResultPage;
                sonyMediaTempData.RecordNumber = recordNumberDesc;

                /*TempData["hasMoreResultPage"] = hasMoreResultPage;
                TempData["hasPreviousResultPage"] = hasPreviousResultPage;*/
                //TempData["recordNumber"] = recordNumberDesc;
                //TempData.Keep();
                SetTempData(sonyMediaTempData);
                string htmlResult = RenderPartialToString(PATH_SonyMediaPartialView, tvResult);

                return Json(new
                {
                    //hasMoreResults = HasMoreResults(searchTermWiseTotalRecords, shownRecords),                    

                    HTML = htmlResult != null ? htmlResult : "",
                    hasMoreResult = hasMoreResultPage,
                    hasPreviouResult = hasPreviousResultPage,
                    recordNumber = recordNumberDesc,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("SonyMediaTempData");
            }
        }

        public Boolean CheckForAuthentication()
        {
            try
            {
                if (Authentication.IsAuthenticated)
                {
                    ActiveUser sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                    if (sessionInformation != null && sessionInformation.IsLogIn)
                    {
                        if (!(sessionInformation.AuthorizedVersion == 0 || sessionInformation.AuthorizedVersion == 4))
                        {
                            IQMedia.WebApplication.Utility.ActiveUserMgr.RemoveUserFromCacheBySessionID();
                            IQMedia.Web.Common.Authentication.Logout();
                            SessionLogic sessionLgc = new SessionLogic();
                            sessionLgc.DeleteBySessionID(Session.SessionID);
                            return false;
                        }
                        else
                        {
                            if (IQMedia.Web.Common.Authentication.CurrentUser != null)
                            {
                                var IsValidSession = IQMedia.WebApplication.Utility.ActiveUserMgr.IsUserInCacheBySessionID();
                                if (IsValidSession)
                                    return true;
                                else
                                    return false;
                            }
                            return false;

                        }
                    }
                    else
                    {
                        var currentUser = IQMedia.Web.Common.Authentication.CurrentUser;
                        if (currentUser != null)
                        {
                            CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                            CustomerModel customerModel = customerLogic.GetClientGUIDByCustomerGUID(currentUser.Guid);

                            List<CustomerRoleModel> customerRoles = customerLogic.GetCustomerRoles(currentUser.Guid);
                            IQMedia.WebApplication.Utility.CommonFunctions.FillCustomerRoles(customerModel, customerRoles);

                            if (!(customerModel.AuthorizedVersion == 0 || customerModel.AuthorizedVersion == 4))
                            {
                                IQMedia.WebApplication.Utility.ActiveUserMgr.RemoveUserFromCacheBySessionID();
                                IQMedia.Web.Common.Authentication.Logout();
                                SessionLogic sessionLgc = new SessionLogic();
                                sessionLgc.DeleteBySessionID(Session.SessionID);
                                return false;
                            }
                            else
                            {
                                IQMedia.WebApplication.Utility.ActiveUserMgr.UpdateUserIntoCache();

                                return true;
                            }

                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

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


        #region Utility

        public KantorMediaTempData GetTempData()
        {
            sonyMediaTempData = TempData["SonyMediaTempData"] != null ? (KantorMediaTempData)TempData["SonyMediaTempData"] : new KantorMediaTempData();
            return sonyMediaTempData;
        }

        public void SetTempData(KantorMediaTempData p_KantorMediaTempData)
        {
            TempData["SonyMediaTempData"] = p_KantorMediaTempData;
            TempData.Keep("SonyMediaTempData");
        }

        #endregion

    }
}
