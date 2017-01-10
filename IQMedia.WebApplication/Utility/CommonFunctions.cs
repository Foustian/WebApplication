using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMedia.Model;
using System.Web.Mvc;
using System.Web.Caching;
using System.Web.Security;
using System.Security.Principal;
using System.Diagnostics;
using IQMedia.Common.Util;
using System.Configuration;
using System.Text.RegularExpressions;
using IQMedia.Web.Logic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using IQMedia.Web.Logic.Base;
using Alachisoft.NCache.Runtime.Exceptions;
using Alachisoft.NCache.Web.Caching;
using System.Collections;
using IQCommon.Model;

namespace IQMedia.WebApplication.Utility
{
    public static class CommonFunctions
    {

        /*public static TempDataInformation GetTempDataInformation()
        {
            TempDataInformation tempDataInformation = null;
            if (TempData["TempDataInformation"] == null)
            {
                tempDataInformation = new TempDataInformation();
            }
            else
            {
                tempDataInformation = (TempDataInformation)TempData["TempDataInformation"];
            }
            return tempDataInformation;
        }

        public static void SetTempDataInformation(TempDataInformation tempDataInformation)
        {
            TempData["TempDataInformation"] = tempDataInformation;
        }*/


        public enum ResultType
        {
            Discovery,
            TimeShift,
            Library,
            Feeds,
            Report,
            Analytics
        }

        public enum PMGUrlType
        {
            TV,
            MO,
            TW,
            FE,
            PQ,
            MT,
            QR
        }

        public enum DMAWithOutRank
        {
            NATIONAL,
            INTERNATIONAL,
            GLOBAL
        }

        public enum RecaptchaErrorCodes
        { 
            MissingSecret,
            InvalidSecret,
            MissingUserResponse,
            InvalidUserResponse,
            Unknown
        }

        public static List<IQSolrEngineModel> ListOfSolrEngines {
            get
            {
                // Get solr engine data if:
                // - It hasn't yet been retrieved
                // - It was last retrieved prior to the current hour
                // - Month rollover has occurred
                bool getEngineData = _ListOfSolrEngines == null;

                getEngineData = getEngineData || DateTime.Now.Hour != LastRefreshTime.Hour || (DateTime.Now.Day == 1 && LastRefreshTime.Day == DateTime.DaysInMonth(LastRefreshTime.Year, LastRefreshTime.Month));

                if (getEngineData)
                {
                    _ListOfSolrEngines = IQSolrEngineLogic.GetSolrEngines();
                    LastRefreshTime = DateTime.Now;
                }
                return _ListOfSolrEngines;
            }
            set
            {
                _ListOfSolrEngines = value;
            }
        } static List<IQSolrEngineModel> _ListOfSolrEngines;

        public static DateTime LastRefreshTime
        {
            get { return _LastRefreshTime; }
            set { _LastRefreshTime = value; }
        } static DateTime _LastRefreshTime = DateTime.MinValue;
        /*
        public static SessionInformation GetSessionInformation()
        {
            SessionInformation _SessionInformation = null;

            try
            {
                _SessionInformation = (SessionInformation)HttpContext.Current.Session["SessionInformation"];

                if (_SessionInformation == null) _SessionInformation = new SessionInformation();
            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }

            return _SessionInformation;
        }
        */

        /*
        public static void SetSessionInformation(SessionInformation p_sessionInformation)
        {
            SessionInformation sessionInformation = new SessionInformation();
            sessionInformation.ClientGUID = p_sessionInformation.ClientGUID;
            sessionInformation.ClientID = p_sessionInformation.ClientID;
            sessionInformation.ClientName = p_sessionInformation.ClientName;
            sessionInformation.ClientPlayerLogoImage = p_sessionInformation.ClientPlayerLogoImage;
            sessionInformation.CustomerKey = p_sessionInformation.CustomerKey;
            sessionInformation.Email = p_sessionInformation.Email;
            sessionInformation.FirstName = p_sessionInformation.FirstName;
            sessionInformation.LastName = p_sessionInformation.LastName;
            sessionInformation.IsClientPlayerLogoActive = p_sessionInformation.IsClientPlayerLogoActive;
            sessionInformation.IsLogIn = true;
            sessionInformation.IsUgcAutoClip = false;
            sessionInformation.MultiLogin = p_sessionInformation.MultiLogin;
            sessionInformation.CustomerGUID = p_sessionInformation.CustomerGUID;
            sessionInformation.AuthorizedVersion = p_sessionInformation.AuthorizedVersion;
            sessionInformation.DefaultPage = p_sessionInformation.DefaultPage;
            sessionInformation.TimeZone = p_sessionInformation.TimeZone;
            sessionInformation.dst = p_sessionInformation.dst;
            sessionInformation.gmt = p_sessionInformation.gmt;
            sessionInformation.MCID = p_sessionInformation.MCID;
            sessionInformation.MasterCustomerID = p_sessionInformation.MasterCustomerID;
            sessionInformation.Isv4Group = p_sessionInformation.Isv4Group;
            sessionInformation.LoginID = p_sessionInformation.LoginID;
            // Customer Roles Information

            sessionInformation.Isv4FeedsAccess = p_sessionInformation.Isv4FeedsAccess;
            sessionInformation.Isv4DiscoveryAccess = p_sessionInformation.Isv4DiscoveryAccess;
            sessionInformation.Isv4LibraryAccess = p_sessionInformation.Isv4LibraryAccess;
            sessionInformation.Isv4TimeshiftAccess = p_sessionInformation.Isv4TimeshiftAccess;
            sessionInformation.Isv4TAdsAccess = p_sessionInformation.Isv4TAdsAccess;
            sessionInformation.Isv4AnalyticsAccess = p_sessionInformation.Isv4AnalyticsAccess;
            sessionInformation.Isv4DashboardAccess = p_sessionInformation.Isv4DashboardAccess;
            sessionInformation.Isv4LibraryDashboardAccess = p_sessionInformation.Isv4LibraryDashboardAccess;
            sessionInformation.Isv4TimeshiftRadioAccess = p_sessionInformation.Isv4TimeshiftRadioAccess;
            sessionInformation.Isv4SetupAccess = p_sessionInformation.Isv4SetupAccess;
            sessionInformation.isv4OptiqAccess = p_sessionInformation.isv4OptiqAccess;
            sessionInformation.IsGlobalAdminAccess = p_sessionInformation.IsGlobalAdminAccess;
            sessionInformation.Isv4UGCAccess = p_sessionInformation.Isv4UGCAccess;
            sessionInformation.Isv4UGCDownload = p_sessionInformation.Isv4UGCDownload;
            sessionInformation.Isv4UGCUploadEdit = p_sessionInformation.Isv4UGCUploadEdit;
            sessionInformation.Isv4IQAgentAccess = p_sessionInformation.Isv4IQAgentAccess;
            sessionInformation.Isv4TV = p_sessionInformation.Isv4TV;
            sessionInformation.Isv4NM = p_sessionInformation.Isv4NM;
            sessionInformation.Isv4SM = p_sessionInformation.Isv4SM;
            sessionInformation.Isv4TW = p_sessionInformation.Isv4TW;
            sessionInformation.Isv4TM = p_sessionInformation.Isv4TM;
            sessionInformation.Isv4CustomImage = p_sessionInformation.Isv4CustomImage;
            sessionInformation.IsCompeteData = p_sessionInformation.IsCompeteData;
            sessionInformation.IsNielsenData = p_sessionInformation.IsNielsenData;
            sessionInformation.Isv4BLPM = p_sessionInformation.Isv4BLPM;
            sessionInformation.IsNewsRights = p_sessionInformation.IsNewsRights;
            sessionInformation.Isv4CustomSettings = p_sessionInformation.Isv4CustomSettings;
            sessionInformation.Isv4DiscoveryLiteAccess = p_sessionInformation.Isv4DiscoveryLiteAccess;
            sessionInformation.IsfliQAdmin = p_sessionInformation.IsfliQAdmin;
            sessionInformation.Isv4PQ = p_sessionInformation.Isv4PQ;
            sessionInformation.IsMediaRoomContributor = p_sessionInformation.IsMediaRoomContributor;
            sessionInformation.IsMediaRoomEditor = p_sessionInformation.IsMediaRoomEditor;
            sessionInformation.Isv4Google = p_sessionInformation.Isv4Google;
            sessionInformation.IsTimeshiftFacet = p_sessionInformation.IsTimeshiftFacet;
            sessionInformation.IsShareTV = p_sessionInformation.IsShareTV;

            //System.Web.HttpContext.Current.Session["SessionInformation"] = sessionInformation;
            HttpContext.Current.Session["SessionInformation"] = sessionInformation;
        }
        */

        /*
        public static void SetSessionInformation(CustomerModel customerModel)
        {
            SessionInformation sessionInformation = new SessionInformation();
            sessionInformation.ClientGUID = customerModel.ClientGUID;
            sessionInformation.ClientID = customerModel.ClientID;
            sessionInformation.ClientName = customerModel.ClientName;
            sessionInformation.ClientPlayerLogoImage = customerModel.ClientPlayerLogoImage;
            sessionInformation.CustomerKey = customerModel.CustomerKey;
            sessionInformation.Email = customerModel.Email;
            sessionInformation.FirstName = customerModel.FirstName;
            sessionInformation.LastName = customerModel.LastName;
            sessionInformation.IsClientPlayerLogoActive = customerModel.IsClientPlayerLogoActive;
            sessionInformation.IsLogIn = true;
            sessionInformation.IsUgcAutoClip = false;
            sessionInformation.MultiLogin = customerModel.MultiLogin;
            sessionInformation.CustomerGUID = customerModel.CustomerGUID;
            sessionInformation.AuthorizedVersion = customerModel.AuthorizedVersion;
            sessionInformation.DefaultPage = customerModel.DefaultPage;
            sessionInformation.TimeZone = customerModel.TimeZone;
            sessionInformation.dst = customerModel.dst;
            sessionInformation.gmt = customerModel.gmt;
            sessionInformation.MCID = customerModel.MCID;
            sessionInformation.MasterCustomerID = customerModel.MasterCustomerID == null ? customerModel.CustomerKey : customerModel.MasterCustomerID.Value;
            sessionInformation.Isv4Group = customerModel.Isv4Group;
            sessionInformation.LoginID = customerModel.LoginID;

            // Customer Roles Information

            sessionInformation.Isv4FeedsAccess = customerModel.Isv4FeedsAccess;
            sessionInformation.Isv4DiscoveryAccess = customerModel.Isv4DiscoveryAccess;
            sessionInformation.Isv4LibraryAccess = customerModel.Isv4LibraryAccess;
            sessionInformation.Isv4TimeshiftAccess = customerModel.Isv4TimeshiftAccess;
            sessionInformation.Isv4TAdsAccess = customerModel.Isv4TAdsAccess;
            sessionInformation.Isv4AnalyticsAccess = customerModel.Isv4AnalyticsAccess;
            sessionInformation.Isv4DashboardAccess = customerModel.Isv4DashboardAccess;
            sessionInformation.Isv4LibraryDashboardAccess = customerModel.Isv4LibraryDashboardAccess;
            sessionInformation.Isv4TimeshiftRadioAccess = customerModel.Isv4TimeshiftRadioAccess;
            sessionInformation.Isv4SetupAccess = customerModel.Isv4SetupAccess;
            sessionInformation.isv4OptiqAccess = customerModel.isv4OptiqAccess;
            sessionInformation.IsGlobalAdminAccess = customerModel.IsGlobalAdminAccess;
            sessionInformation.Isv4UGCAccess = customerModel.Isv4UGCAccess;
            sessionInformation.Isv4IQAgentAccess = customerModel.Isv4IQAgentAccess;
            sessionInformation.Isv4UGCDownload = customerModel.IsUGCDownload;
            sessionInformation.Isv4UGCUploadEdit = customerModel.IsUGCUploadEdit;
            sessionInformation.Isv4TV = customerModel.Isv4TV;
            sessionInformation.Isv4NM = customerModel.Isv4NM;
            sessionInformation.Isv4SM = customerModel.Isv4SM;
            sessionInformation.Isv4TW = customerModel.Isv4TW;
            sessionInformation.Isv4TM = customerModel.Isv4TM;
            sessionInformation.Isv4CustomImage = customerModel.Isv4CustomImage;
            sessionInformation.IsCompeteData = customerModel.IsCompeteData;
            sessionInformation.IsNielsenData = customerModel.IsNielsenData;
            sessionInformation.Isv4BLPM = customerModel.Isv4BLPM;
            sessionInformation.IsNewsRights = customerModel.IsNewsRights;
            sessionInformation.Isv4CustomSettings = customerModel.Isv4CustomSettings;
            sessionInformation.Isv4DiscoveryLiteAccess = customerModel.Isv4DiscoveryLiteAccess;
            sessionInformation.IsfliQAdmin = customerModel.IsfliQAdmin;
            sessionInformation.Isv4PQ = customerModel.Isv4PQ;
            sessionInformation.IsMediaRoomContributor = customerModel.IsMediaRoomContributor;
            sessionInformation.IsMediaRoomEditor = customerModel.IsMediaRoomEditor;
            sessionInformation.Isv4Google = customerModel.Isv4Google;
            sessionInformation.IsTimeshiftFacet = customerModel.IsTimeshiftFacet;
            sessionInformation.IsShareTV = customerModel.IsShareTV;

            System.Web.HttpContext.Current.Session["SessionInformation"] = sessionInformation;
        }
        */

        public static void FillCustomerRoles(CustomerModel customerModel, List<CustomerRoleModel> customerRoles)
        {
            if (customerModel != null && customerRoles != null)
            {
                foreach (CustomerRoleModel customerrole in customerRoles)
                {
                    IQMedia.Shared.Utility.CommonFunctions.Roles role = (IQMedia.Shared.Utility.CommonFunctions.Roles)Enum.Parse(typeof(IQMedia.Shared.Utility.CommonFunctions.Roles), customerrole.RoleName);

                    switch (role)
                    {
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Feeds:
                            customerModel.Isv4FeedsAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Discovery:
                            customerModel.Isv4DiscoveryAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Library:
                            customerModel.Isv4LibraryAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Timeshift:
                            customerModel.Isv4TimeshiftAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4TAds:
                            customerModel.Isv4TAdsAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v5Ads:
                            customerModel.Isv5AdsAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v5Analytics:
                            customerModel.Isv5AnalyticsAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Dashboard:
                            customerModel.Isv4DashboardAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4LibraryDashboard:
                            customerModel.Isv4LibraryDashboardAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Radio:
                            customerModel.Isv4TimeshiftRadioAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Setup:
                            customerModel.Isv4SetupAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v5LR:
                            customerModel.isv5LRAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.GlobalAdminAccess:
                            customerModel.IsGlobalAdminAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4UGC:
                            customerModel.Isv4UGCAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4IQAgentSetup:
                            customerModel.Isv4IQAgentAccess = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4TV:
                            customerModel.Isv4TV = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4NM:
                            customerModel.Isv4NM = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4SM:
                            customerModel.Isv4SM = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4TW:
                            customerModel.Isv4TW = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4TM:
                            customerModel.Isv4TM = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.UGCAutoClip:
                            customerModel.IsUGCAutoClip = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.UGCDownload:
                            customerModel.IsUGCDownload = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.UGCUploadEdit:
                            customerModel.IsUGCUploadEdit = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Group:
                            customerModel.Isv4Group = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.v4CustomImage:
                            customerModel.Isv4CustomImage = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.CompeteData:
                            customerModel.IsCompeteData = true;
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.Roles.NielsenData:
                            customerModel.IsNielsenData = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.v4BLPM:
                            customerModel.Isv4BLPM = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.NewsRight:
                            customerModel.IsNewsRights = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.v4CustomSettings:
                            customerModel.Isv4CustomSettings = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.v4DiscoveryLite:
                            customerModel.Isv4DiscoveryLiteAccess = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.fliQAdmin:
                            customerModel.IsfliQAdmin = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.v4PQ:
                            customerModel.Isv4PQ = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.MediaRoomContributor:
                            customerModel.IsMediaRoomContributor = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.MediaRoomEditor:
                            customerModel.IsMediaRoomEditor = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.v4Google:
                            customerModel.Isv4Google = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.TimeshiftFacet:
                            customerModel.IsTimeshiftFacet = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.ShareTV:
                            customerModel.IsShareTV = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.SMOther:
                            customerModel.IsSMOther = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.FB:
                            customerModel.IsFB = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.IG:
                            customerModel.IsIG = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.BL:
                            customerModel.IsBL = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.FO:
                            customerModel.IsFO = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.PR:
                            customerModel.IsPR = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.LN:
                            customerModel.IsLN = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.ThirdPartyData:
                            customerModel.IsThirdPartyData = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.ClientSpecificData:
                            customerModel.IsClientSpecificData = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.ConnectAccess:
                            customerModel.IsConnectAccess = true;
                            break;
                        case Shared.Utility.CommonFunctions.Roles.ExternalRuleEditor:
                            customerModel.IsExternalRuleEditor = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /*
        public static List<SessionModel> GetUserCache(string p_SearchTerm = "", string p_SortColumn ="", bool p_IsAsc = false)
        {
            List<SessionModel> users = null;
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            if (cache != null)
            {
                users = cache["ActiveUsers"] as List<SessionModel>;
                if (users == null)
                {
                    users = (new IQMedia.Web.Logic.SessionLogic()).GetAll(p_SearchTerm, p_SortColumn, p_IsAsc);
                    cache.Insert("ActiveUsers", users, null, DateTime.Today.AddMonths(1), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }
            }
            return users;
        }
        */

        /*
        
        public static void AddUserIntoCache(string p_LoginID, bool IsUserMultiLoginEnable)
        {
            List<SessionModel> users = GetUserCache();

            //SessionModel existingUser = users.Where(u => u.EmailAddress == p_EmailAddress && u.SessionID != HttpContext.Current.Session.SessionID).FirstOrDefault();

            if (!IsUserMultiLoginEnable)
            {
                SessionModel existingUser = users.Where(u => string.Compare(u.LoginID, p_LoginID, true) == 0).FirstOrDefault();


                (new IQMedia.Web.Logic.SessionLogic()).DeleteByLoginID(p_LoginID);


                users.Remove(existingUser);
            }

            var ipadd = GetAllNetworkInterfaceIpv4Addresses();

            SessionModel session = new SessionModel { SessionID = HttpContext.Current.Session.SessionID, LoginID = p_LoginID, LastAccessTime = DateTime.Now, SessionTimeOut = DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout), Server=ipadd[0].ToString() };
            (new IQMedia.Web.Logic.SessionLogic()).InsertSession(session);

            SessionModel existingSession = users.Find(u => string.Compare(u.SessionID, HttpContext.Current.Session.SessionID, true) == 0);

            if (existingSession == null)
            {
                users.Add(new SessionModel() { 
                    LoginID = session.LoginID, 
                    LastAccessTime = session.LastAccessTime, 
                    SessionID = HttpContext.Current.Session.SessionID, 
                    SessionTimeOut = session.SessionTimeOut,
                    Server=ipadd[0].ToString()
                });
            }
            else
            {
                existingSession.SessionTimeOut = session.SessionTimeOut;
                existingSession.LastAccessTime = session.LastAccessTime;
                existingSession.Server = ipadd[0].ToString();
            }
        }

        */

        public static List<IPAddress> GetAllNetworkInterfaceIpv4Addresses()
        {
            var map = new List<IPAddress>();

            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var uipi in ni.GetIPProperties().UnicastAddresses)
                {
                    if (uipi.Address.AddressFamily != AddressFamily.InterNetwork)
                    {
                        continue;
                    }
                    else
                    {
                        map.Add(uipi.Address);
                    }
                }
            }
            return map;
        }

        /*

        public static void AddUserIntoCache(SessionModel p_Session, bool p_IsUserMultiLoginEnable)
        {
            List<SessionModel> users = GetUserCache();

            //SessionModel existingUser = users.Where(u => u.EmailAddress == p_EmailAddress && u.SessionID != HttpContext.Current.Session.SessionID).FirstOrDefault();

            if (!p_IsUserMultiLoginEnable)
            {
                SessionModel existingUser = users.Where(u => string.Compare(u.LoginID, p_Session.LoginID, true) == 0).FirstOrDefault();
                users.Remove(existingUser);
            }

            SessionModel existingSession = users.Find(u => string.Compare(u.SessionID, HttpContext.Current.Session.SessionID, true) == 0);

            if (existingSession == null)
            {
                users.Add(new SessionModel() { LoginID = p_Session.LoginID, LastAccessTime = p_Session.LastAccessTime, SessionID = HttpContext.Current.Session.SessionID, SessionTimeOut = p_Session.SessionTimeOut });
            }
            else
            {
                existingSession.SessionTimeOut = p_Session.SessionTimeOut;
                existingSession.LastAccessTime = p_Session.LastAccessTime;
            }
        }

        */

        /*public static bool IsCurrentSessionValid(string p_EmailAddress)
        {
            List<SessionModel> users = GetUserCache();
            int Count = users.Where(u => u.EmailAddress == p_EmailAddress && u.SessionID == HttpContext.Current.Session.SessionID).Count();

            return Count > 0;
        }*/

        /*

        public static bool IsUserInCache(string p_LoginID)
        {
            List<SessionModel> users = GetUserCache();
            SessionModel SessionModel = users.Where(u => string.Compare(u.LoginID, p_LoginID, true) == 0 && string.Compare(u.SessionID, HttpContext.Current.Session.SessionID, true) == 0 && u.SessionTimeOut > DateTime.Now).FirstOrDefault();

            if (SessionModel != null)
            {
                return true;
            }
            else
            {

                var invalidUsers = users.Where(u => string.Compare(u.LoginID, p_LoginID, true) == 0 && string.Compare(u.SessionID, HttpContext.Current.Session.SessionID, true) == 0);
                users.RemoveAll(u => string.Compare(u.LoginID, p_LoginID, true) == 0 && string.Compare(u.SessionID, HttpContext.Current.Session.SessionID, true) == 0);

                return false;
            }

            //int Count = users.Where(u => u.EmailAddress == p_EmailAddress).Count();// && u.SessionID == HttpContext.Current.Session.SessionID).Count();

            //return Count > 0;
        }

        

        public static bool IsUserInCacheBySessionID()
        {
            List<SessionModel> users = GetUserCache();
            var useConunt = users.Where(u => string.Compare(u.SessionID, HttpContext.Current.Session.SessionID, true) == 0 && u.SessionTimeOut > DateTime.Now).Count();

            if (useConunt > 0)
            {
                return true;
            }
            else
            {
                (new IQMedia.Web.Logic.SessionLogic()).DeleteBySessionID(HttpContext.Current.Session.SessionID);
                users.RemoveAll(s => s.SessionID == HttpContext.Current.Session.SessionID);
                return false;
            }

            //int Count = users.Where(u => u.EmailAddress == p_EmailAddress).Count();// && u.SessionID == HttpContext.Current.Session.SessionID).Count();

            //return Count > 0;
        }

        public static void RemoveUserFromCacheByLoginIDnSession(string p_EmailAddress)
        {
            List<SessionModel> users = GetUserCache();
            users.RemoveAll(s => string.Compare(s.LoginID, p_EmailAddress, true) == 0 && string.Compare(s.SessionID, HttpContext.Current.Session.SessionID, true) == 0);
            (new IQMedia.Web.Logic.SessionLogic()).DeleteBySessionID(HttpContext.Current.Session.SessionID);
        }

        public static void RemoveUserFromCacheByLoginIDnSession(string p_EmailAddress, string p_SessionID)
        {
            List<SessionModel> users = GetUserCache();
            users.RemoveAll(s => string.Compare(s.LoginID, p_EmailAddress, true) == 0 && string.Compare(s.SessionID, p_SessionID, true) == 0);
            (new IQMedia.Web.Logic.SessionLogic()).DeleteBySessionID(HttpContext.Current.Session.SessionID);
        }

        public static void RemoveUserFromCacheBySessionID()
        {
            List<SessionModel> users = GetUserCache();
            users.RemoveAll(s => s.SessionID == HttpContext.Current.Session.SessionID);
            (new IQMedia.Web.Logic.SessionLogic()).DeleteBySessionID(HttpContext.Current.Session.SessionID);
        }

        public static void RemoveUserFromCacheByLoginID(string p_EmailAddress)
        {
            List<SessionModel> users = GetUserCache();
            users.RemoveAll(s => string.Compare(s.LoginID, p_EmailAddress, true) == 0);
        }

        public static void UpdateUserIntoCache(string p_EmailAddress)
        {
            List<SessionModel> users = GetUserCache();
            if (users != null && users.Count() > 0)
            {
                var user = users.Where(u => string.Compare(u.LoginID, p_EmailAddress, true) == 0 && string.Compare(u.SessionID, HttpContext.Current.Session.SessionID, true) == 0).FirstOrDefault();

                if (user!=null)
                {
                    user.SessionTimeOut = DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout);
                    user.LastAccessTime = DateTime.Now; 
                }
                else
                {
                    Shared.Utility.Log4NetLogger.Fatal("UpdateUserCache: "+p_EmailAddress+"...users"+string.Join(",",users.Select(u=>u.LoginID)));
                }
            }       
        }
       
        public static void UpdateUserLoginIDInChache(string p_EmailAddress)
        {
            List<SessionModel> users = GetUserCache();
            if (users.Count() > 0)
            {
                var currentUser = users.Where(u => u.SessionID == HttpContext.Current.Session.SessionID).FirstOrDefault();
                if (currentUser != null)
                {
                    currentUser.LoginID = p_EmailAddress;
                    currentUser.LastAccessTime = DateTime.Now;
                }
            }
        }

         */
        public static string GetTimeDifference(DateTime? date, bool isGMT = true)
        {
            var timeDifference = "";

            if (date != null)
            {
                DateTime mediaDate = DateTime.MinValue;

                DateTime dateFrom = (DateTime)date;
                DateTime dateTo = new DateTime();
                if (isGMT) dateTo = DateTime.UtcNow;
                else dateTo = DateTime.Now;

                var diffY = dateTo.Year - dateFrom.Year;
                var diffM = dateTo.Month - dateFrom.Month;
                var diffD = dateTo.Day - dateFrom.Day;
                var diffH = dateTo.Hour - dateFrom.Hour;
                var diffMin = dateTo.Minute - dateFrom.Minute;

                //Year diff
                var yearDiff = diffY;

                if (yearDiff > 0 && (diffM < 0 || (diffM == 0 && (diffD < 0 || (diffD == 0 && (diffH < 0 || (diffH == 0 && (diffMin < 0))))))))
                {
                    yearDiff--;
                }

                if (yearDiff <= 0)
                {
                    //Month diff
                    if (yearDiff == 0 && diffM == -11)
                    {
                        diffM = 1;
                    }

                    var monthDiff = diffM > 0 ? diffM : (diffY - yearDiff) * 12 + diffM;

                    if (monthDiff > 0 && (diffD < 0 || (diffD == 0 && (diffH < 0 || (diffH == 0 && (diffMin < 0))))))
                    {
                        monthDiff--;
                    }

                    if (monthDiff <= 0)
                    {
                        //Day diff
                        int daysInMonth = dateTo.AddDays(-1 * dateTo.Day).Day;
                        var dayDiff = diffD > 0 ? diffD : (diffM - monthDiff) * daysInMonth + diffD;

                        if (dayDiff > 0 && (diffH < 0 || (diffH == 0 && (diffMin < 0))))
                        {
                            dayDiff--;
                        }

                        if (dayDiff <= 0)
                        {
                            //Hour diff
                            var hourDiff = diffH > 0 ? diffH : (diffD - dayDiff) * 24 + diffH;

                            if (hourDiff > 0 && diffMin < 0)
                            {
                                hourDiff--;
                            }

                            if (hourDiff <= 0)
                            {
                                //Minute Diff
                                var mntDiff = diffH == 0 ? diffMin : ((hourDiff * 60) + (diffMin < 0 ? (60 + diffMin) : diffMin));

                                if (mntDiff < 0)
                                {
                                    mntDiff = 0;
                                }

                                timeDifference = mntDiff + (mntDiff == 1 ? " minute ago" : " minutes ago");
                            }
                            else
                            {
                                timeDifference = hourDiff + (hourDiff == 1 ? " hour ago" : " hours ago");
                            }
                        }
                        else
                        {
                            timeDifference = dayDiff + (dayDiff == 1 ? " day ago" : " days ago");
                        }
                    }
                    else
                    {
                        timeDifference = monthDiff + (monthDiff == 1 ? " month ago" : " months ago");
                    }
                }
                else
                {
                    timeDifference = yearDiff + (yearDiff == 1 ? " year ago" : " years ago");
                }
            }

            return timeDifference;
        }

        public static dynamic GetGMTandDSTTime(dynamic p_List, ResultType type)
        {
            ActiveUser sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
            switch (type)
            {
                case ResultType.Discovery:
                    List<DiscoveryMediaResult> listDiscoveryMediaResult = (List<DiscoveryMediaResult>)p_List;
                    listDiscoveryMediaResult.ForEach(s =>
                    {
                        if (s.Date.HasValue && s.MediumType != Shared.Utility.CommonFunctions.CategoryType.TV.ToString() && s.MediumType != Shared.Utility.CommonFunctions.CategoryType.PQ.ToString())
                        {
                            if (s.Date.Value.IsDaylightSavingTime())
                            {

                                s.Date = s.Date.Value.AddHours((Convert.ToDouble(sessionInformation.gmt)) + Convert.ToDouble(sessionInformation.dst));
                            }
                            else
                            {
                                s.Date = s.Date.Value.AddHours((Convert.ToDouble(sessionInformation.gmt)));
                            }
                        }

                    });
                    return listDiscoveryMediaResult;
                case ResultType.TimeShift:
                    List<IQAgent_TVResultsModel> listOfTVResult = (List<IQAgent_TVResultsModel>)p_List;
                    listOfTVResult.ForEach(s =>
                    {
                        if (s.Date != null)
                        {
                            if (s.Date.IsDaylightSavingTime())
                            {

                                s.Date = s.Date.AddHours((Convert.ToDouble(sessionInformation.gmt)) + Convert.ToDouble(sessionInformation.dst));
                            }
                            else
                            {
                                s.Date = s.Date.AddHours((Convert.ToDouble(sessionInformation.gmt)));
                            }
                        }

                    });

                    return listOfTVResult;
                case ResultType.Feeds:
                case ResultType.Report:
                    List<IQAgent_MediaResultsModel> listIQAgent_MediaResultsModel = (List<IQAgent_MediaResultsModel>)p_List;
                    listIQAgent_MediaResultsModel.ForEach(s =>
                    {
                        if (s.MediaType != IQMedia.Shared.Utility.CommonFunctions.CategoryType.TV.ToString() && s.MediaType != IQMedia.Shared.Utility.CommonFunctions.CategoryType.PM.ToString() && s.MediaType != IQMedia.Shared.Utility.CommonFunctions.CategoryType.PQ.ToString())
                        {
                            if (s.MediaDateTime.IsDaylightSavingTime())
                            {
                                s.MediaDateTime = s.MediaDateTime.AddHours((Convert.ToDouble(sessionInformation.gmt)) + Convert.ToDouble(sessionInformation.dst));
                            }
                            else
                            {
                                s.MediaDateTime = s.MediaDateTime.AddHours((Convert.ToDouble(sessionInformation.gmt)));
                            }
                        }

                    });

                    return listIQAgent_MediaResultsModel;
                case ResultType.Library:
                    List<IQArchive_MediaModel> listIQArchive_MediaModel = (List<IQArchive_MediaModel>)p_List;
                    listIQArchive_MediaModel.ForEach(s =>
                    {
                        if (s.MediaType != IQMedia.Shared.Utility.CommonFunctions.CategoryType.TV.ToString() && s.MediaType != IQMedia.Shared.Utility.CommonFunctions.CategoryType.PM.ToString() && s.MediaType != IQMedia.Shared.Utility.CommonFunctions.CategoryType.PQ.ToString())
                        {
                            if (s.MediaDate.IsDaylightSavingTime())
                            {

                                s.MediaDate = s.MediaDate.AddHours((Convert.ToDouble(sessionInformation.gmt)) + Convert.ToDouble(sessionInformation.dst));
                            }
                            else
                            {
                                s.MediaDate = s.MediaDate.AddHours((Convert.ToDouble(sessionInformation.gmt)));
                            }

                            if (s.MediaType == IQMedia.Shared.Utility.CommonFunctions.CategoryType.NM.ToString())
                            {
                                IQArchive_ArchiveNMModel archiveNMModel = (IQArchive_ArchiveNMModel)s.MediaData;
                                if (archiveNMModel.ChildResults != null && archiveNMModel.ChildResults.Count > 0)
                                {
                                    archiveNMModel.ChildResults.ForEach(x =>
                                    {
                                        if (x.MediaDate.IsDaylightSavingTime())
                                        {

                                            x.MediaDate = x.MediaDate.AddHours((Convert.ToDouble(sessionInformation.gmt)) + Convert.ToDouble(sessionInformation.dst));
                                        }
                                        else
                                        {
                                            x.MediaDate = x.MediaDate.AddHours((Convert.ToDouble(sessionInformation.gmt)));
                                        }
                                    });
                                }
                            }
                        }

                    });
                    return listIQArchive_MediaModel;
                case ResultType.Analytics:
                    List<AnalyticsSummaryModel> listAnalyticsSummaryModel = (List<AnalyticsSummaryModel>)p_List;
                    listAnalyticsSummaryModel.ForEach(s =>
                    {
                        if (s.SummaryDateTime.IsDaylightSavingTime())
                        {
                            s.SummaryDateTime = s.SummaryDateTime.AddHours((Convert.ToDouble(sessionInformation.gmt)) + Convert.ToDouble(sessionInformation.dst));
                        }
                        else
                        {
                            s.SummaryDateTime = s.SummaryDateTime.AddHours((Convert.ToDouble(sessionInformation.gmt)));
                        }
                    });
                    return listAnalyticsSummaryModel;
                default:
                    return p_List;

            }
        }


        public static dynamic SortAndConvertGMTandDSTTime(dynamic p_List, ResultType type)
        {
            ActiveUser sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
            switch (type)
            {
                case ResultType.Discovery:
                    List<DiscoveryMediaResult> listDiscoveryMediaResult = (List<DiscoveryMediaResult>)p_List;
                    listDiscoveryMediaResult.ForEach(s =>
                    {
                        if (s.Date.HasValue && s.MediumType != Shared.Utility.CommonFunctions.CategoryType.TV.ToString())
                        {
                            if (s.Date.Value.IsDaylightSavingTime())
                            {

                                s.Date = s.Date.Value.AddHours((Convert.ToDouble(sessionInformation.gmt)) + Convert.ToDouble(sessionInformation.dst));
                            }
                            else
                            {
                                s.Date = s.Date.Value.AddHours((Convert.ToDouble(sessionInformation.gmt)));
                            }
                        }

                    });
                    return listDiscoveryMediaResult.OrderByDescending(s => s.Date).ToList();
                case ResultType.TimeShift:
                    List<IQAgent_TVResultsModel> listOfTVResult = (List<IQAgent_TVResultsModel>)p_List;
                    listOfTVResult.ForEach(s =>
                    {
                        if (s.Date != null)
                        {
                            if (s.Date.IsDaylightSavingTime())
                            {

                                s.Date = s.Date.AddHours((Convert.ToDouble(sessionInformation.gmt)) + Convert.ToDouble(sessionInformation.dst));
                            }
                            else
                            {
                                s.Date = s.Date.AddHours((Convert.ToDouble(sessionInformation.gmt)));
                            }
                        }

                    });

                    return listOfTVResult.OrderByDescending(s => s.Date).ToList();
                case ResultType.Feeds:
                case ResultType.Report:
                    List<IQAgent_MediaResultsModel> listIQAgent_MediaResultsModel = (List<IQAgent_MediaResultsModel>)p_List;
                    listIQAgent_MediaResultsModel.ForEach(s =>
                    {
                        if (s.MediaType != IQMedia.Shared.Utility.CommonFunctions.CategoryType.TV.ToString())
                        {
                            if (s.MediaDateTime.IsDaylightSavingTime())
                            {
                                s.MediaDateTime = s.MediaDateTime.AddHours((Convert.ToDouble(sessionInformation.gmt)) + Convert.ToDouble(sessionInformation.dst));
                            }
                            else
                            {
                                s.MediaDateTime = s.MediaDateTime.AddHours((Convert.ToDouble(sessionInformation.gmt)));
                            }
                        }

                    });

                    return listIQAgent_MediaResultsModel.OrderByDescending(s => s.MediaDateTime).ToList();
                case ResultType.Library:
                    List<IQArchive_MediaModel> listIQArchive_MediaModel = (List<IQArchive_MediaModel>)p_List;
                    listIQArchive_MediaModel.ForEach(s =>
                    {
                        if (s.MediaType != IQMedia.Shared.Utility.CommonFunctions.CategoryType.TV.ToString() && s.MediaType != IQMedia.Shared.Utility.CommonFunctions.CategoryType.PQ.ToString())
                        {
                            if (s.MediaDate.IsDaylightSavingTime())
                            {

                                s.MediaDate = s.MediaDate.AddHours((Convert.ToDouble(sessionInformation.gmt)) + Convert.ToDouble(sessionInformation.dst));
                            }
                            else
                            {
                                s.MediaDate = s.MediaDate.AddHours((Convert.ToDouble(sessionInformation.gmt)));
                            }
                        }

                    });
                    return listIQArchive_MediaModel.OrderByDescending(s => s.MediaDate).ToList();
                default:
                    return p_List;

            }
        }

        public static bool RunProcess(string p_Filename, string p_Arguments)
        {
            try
            {
                var startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                //NOTE: there seems to be an issue with the redirect 
                startInfo.RedirectStandardError = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = p_Filename;
                startInfo.Arguments = p_Arguments;


                Shared.Utility.Log4NetLogger.Debug(startInfo.FileName + " " + startInfo.Arguments);
                using (var exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    //NOTE: Can't do this; see error above about redirect 
                    //var res = exeProcess.StandardError.ReadToEnd(); 
                    //Shared.Utility.Log4NetLogger.Debug(res); 
                }
                return true;
            }
            catch(Exception ex)
            {
                Shared.Utility.Log4NetLogger.Error(ex);
                return false;
            }
        }

        public static bool CheckVersion()
        {
            Shared.Utility.Log4NetLogger.Debug(HttpContext.Current.Session.SessionID + "Android device, checking version");

            Version defaultAndroidVersion = new Version(ConfigurationManager.AppSettings["AndroidDefaultVersion"]);
            string useragent = HttpContext.Current.Request.UserAgent.ToLower(); //"Mozilla/5.0 (Linux; U; Android 2.1-update1; en-gb; GT-I5801 Build/ECLAIR) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17";
            //Regex regex = new Regex(@"(?<=\bandroid\s\b)(\d+(?:\.\d+)+)");
            Regex regex = new Regex(ConfigurationManager.AppSettings["AndroidVersionRegex"]);
            string version = Convert.ToString(regex.Match(useragent));

            Shared.Utility.Log4NetLogger.Debug(HttpContext.Current.Session.SessionID + "Android device, version : " + version);

            if (string.IsNullOrWhiteSpace(version))
            {
                return false;
            }
            else
            {
                try
                {
                    Version currentVersion = new Version(version);
                    if (currentVersion >= defaultAndroidVersion)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {

                    return false;
                }

            }
            return false;

        }

        public static DateTime? GetGMTandDSTTime(DateTime? date)
        {
            ActiveUser sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
            if (date.HasValue)
            {
                if (date.Value.IsDaylightSavingTime())
                {
                    date = date.Value.AddHours((-1) * (Convert.ToDouble(sessionInformation.gmt) + Convert.ToDouble(sessionInformation.dst)));
                }
                else
                {
                    date = date.Value.AddHours((-1) * Convert.ToDouble(sessionInformation.gmt));
                }
            }
            return date;
        }

        public static DateTime? GetLocalTime(DateTime? date)
        {
            ActiveUser sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
            if (date.HasValue)
            {
                if (date.Value.IsDaylightSavingTime())
                {
                    date = date.Value.AddHours((Convert.ToDouble(sessionInformation.gmt) + Convert.ToDouble(sessionInformation.dst)));
                }
                else
                {
                    date = date.Value.AddHours(Convert.ToDouble(sessionInformation.gmt));
                }
            }
            return date;
        }

        public static string GeneratePMGUrl(string p_Type, DateTime? p_FromDate, DateTime? p_ToDate)
        {
            try
            {
                string pmgUrl = string.Empty;

                /*List<string> solrCoreUrls = Config.ConfigSettings.SolrSettings.SolrCores.Where(a => a.Type == p_Type).
                                                        Where(a=> 
                                                                (a.FromDate >= p_FromDate && a.FromDate <= p_ToDate) || 
                                                                (a.ToDate >= p_FromDate && a.ToDate <= p_ToDate)
                                                             ).OrderByDescending(a => a.ToDate).
                                                             Select(a => a.Url).ToList();*/

                List<string> solrCoreUrls = (from core in ListOfSolrEngines
                                             where core.Type == p_Type &&
                                                  (
                                                    (
                                                        (core.FromDate >= p_FromDate && core.FromDate <= p_ToDate) ||
                                                        (core.ToDate >= p_FromDate && core.ToDate <= p_ToDate)
                                                    )
                                                        ||
                                                    (
                                                        (p_FromDate >= core.FromDate && p_FromDate <= core.ToDate) ||
                                                        (p_ToDate >= core.FromDate && p_ToDate <= core.ToDate)
                                                    )
                                                  )
                                             orderby core.ToDate descending
                                             select core.BaseUrl
                 ).ToList();

                if (solrCoreUrls == null || solrCoreUrls.Count == 0)
                {
                    solrCoreUrls = ListOfSolrEngines.Where(a => a.Type == p_Type).OrderByDescending(a => a.ToDate).Select(a => a.BaseUrl).ToList();
                }

                pmgUrl = solrCoreUrls[0] + "select/?";
                if (solrCoreUrls.Count > 1)
                {
                    pmgUrl = pmgUrl + "shards=" + string.Join(",", solrCoreUrls).Replace("http://", "") + "&";
                }

                return pmgUrl;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetSuccessFalseJson()
        { 
            var jsonResult= "{\"isSuccess\": false}";

            return jsonResult;
        }

        // Set which text is displayed as the item's content.
        // Description takes precedence if the option is selected for an individual item. If there is no Description, display "No Description".
        // Otherwise, the LibraryTextType custom setting is used. If Highlighting Text is selected and it exists, it is displayed. Else Content is displayed.
        public static List<IQArchive_MediaModel> ProcessArchiveDisplayText(List<IQArchive_MediaModel> lstArchiveMedia, Shared.Utility.CommonFunctions.LibraryTextTypes textType)
        {
            ActiveUser sessionInfo = ActiveUserMgr.GetActiveUser();

            int wordsBeforeSpan = Convert.ToInt32(ConfigurationManager.AppSettings["HighlightWordsBeforeSpan"]);
            int wordsAfterSpan = Convert.ToInt32(ConfigurationManager.AppSettings["HighlightWordsAfterSpan"]);
            string separator = "...&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;...";
            string highlightKeyword = "span";

            foreach (IQArchive_MediaModel archiveMedia in lstArchiveMedia)
            {
                IQ_MediaTypeModel objSubMediaType = null;
                if (sessionInfo != null && sessionInfo.MediaTypes != null && sessionInfo.MediaTypes.Count > 0)
                {
                    objSubMediaType = sessionInfo.MediaTypes.First(w => archiveMedia.SubMediaType.ToString() == w.SubMediaType && w.TypeLevel == 2);
                }

                if (archiveMedia.DisplayDescription || textType == Shared.Utility.CommonFunctions.LibraryTextTypes.Description)
                {
                    archiveMedia.DisplayText = !String.IsNullOrWhiteSpace(archiveMedia.Description) ? ProcessDisplayText_Helper(archiveMedia.Description, true) : "No Description";
                }
                else
                {
                    // Set display text as Content initially if it exists. Then if Highlighting Text is enabled and it exists, set it as the display text.
                    if (!String.IsNullOrWhiteSpace(archiveMedia.Content))
                    {
                        archiveMedia.DisplayText = ProcessDisplayText_Helper(archiveMedia.Content, archiveMedia.SubMediaType != Shared.Utility.CommonFunctions.CategoryType.TW);
                    }

                    if (textType == Shared.Utility.CommonFunctions.LibraryTextTypes.HighlightingText)
                    {
                        string origHighlightingText;
                        string highlightingText = null;

                        // PM, TVEyes Radio, and Miscellaneous items don't have highlighting text
                        switch (objSubMediaType != null ? objSubMediaType.DataModelType : archiveMedia.DataModelType)
                        {
                            case "TV":
                                IQArchive_ArchiveClipModel tvModel = (IQArchive_ArchiveClipModel)archiveMedia.MediaData;
                                if (tvModel.HighlightedOutput != null && tvModel.HighlightedOutput.CC != null)
                                {
                                    highlightingText = String.Join(" ", tvModel.HighlightedOutput.CC.Select(s => s.Text));
                                }
                                break;
                            case "TW":
                                IQArchive_ArchiveTweetsModel twModel = (IQArchive_ArchiveTweetsModel)archiveMedia.MediaData;
                                if (twModel.HighlightedOutput != null)
                                {
                                    highlightingText = twModel.HighlightedOutput.Highlights;
                                }
                                break;
                            case "NM":
                                IQArchive_ArchiveNMModel nmModel = (IQArchive_ArchiveNMModel)archiveMedia.MediaData;
                                if (nmModel.HighlightedOutput != null && nmModel.HighlightedOutput.Highlights != null)
                                {
                                    origHighlightingText = String.Join(" ", nmModel.HighlightedOutput.Highlights.Select(s => s));
                                    highlightingText = Shared.Utility.CommonFunctions.GetWordsAround(origHighlightingText, highlightKeyword, wordsBeforeSpan, wordsAfterSpan, separator);

                                    if (String.IsNullOrWhiteSpace(highlightingText))
                                    {
                                        highlightingText = origHighlightingText;
                                    }
                                }
                                break;
                            case "SM":
                                IQArchive_ArchiveSMModel smModel = (IQArchive_ArchiveSMModel)archiveMedia.MediaData;
                                if (smModel.HighlightedOutput != null && smModel.HighlightedOutput.Highlights != null)
                                {
                                    origHighlightingText = String.Join(" ", smModel.HighlightedOutput.Highlights.Select(s => s));
                                    highlightingText = Shared.Utility.CommonFunctions.GetWordsAround(origHighlightingText, highlightKeyword, wordsBeforeSpan, wordsAfterSpan, separator);

                                    if (String.IsNullOrWhiteSpace(highlightingText))
                                    {
                                        highlightingText = origHighlightingText;
                                    }
                                }
                                break;
                            case "PQ":
                                IQArchive_ArchivePQModel pqModel = (IQArchive_ArchivePQModel)archiveMedia.MediaData;
                                if (pqModel.HighlightedOutput != null && pqModel.HighlightedOutput.Highlights != null)
                                {
                                    origHighlightingText = String.Join(" ", pqModel.HighlightedOutput.Highlights.Select(s => s));
                                    highlightingText = Shared.Utility.CommonFunctions.GetWordsAround(origHighlightingText, highlightKeyword, wordsBeforeSpan, wordsAfterSpan, separator);

                                    if (String.IsNullOrWhiteSpace(highlightingText))
                                    {
                                        highlightingText = origHighlightingText;
                                    }
                                }
                                break;
                            case "IQR":
                                IQArchive_ArchiveRadioModel radioModel = (IQArchive_ArchiveRadioModel)archiveMedia.MediaData;
                                if (radioModel.HighlightedOutput != null && radioModel.HighlightedOutput.CC != null)
                                {
                                    highlightingText = String.Join(" ", radioModel.HighlightedOutput.CC.Select(s => s.Text));
                                }
                                break;
                        }

                        if (!String.IsNullOrWhiteSpace(highlightingText))
                        {
                            highlightingText = ProcessDisplayText_Helper(highlightingText, archiveMedia.SubMediaType != Shared.Utility.CommonFunctions.CategoryType.TW);

                            if (!String.IsNullOrWhiteSpace(highlightingText))
                            {
                                archiveMedia.DisplayText = highlightingText;
                            }
                        }
                    }
                }
            }

            return lstArchiveMedia;
        }

        private static string ProcessDisplayText_Helper(string text, bool limitSize)
        {
            string formattedText;

            text = text.Replace("&lt;", "<").Replace("&gt;", ">");
            if (limitSize && text.Length > 300)
            {
                formattedText = text.Substring(0, 300);
                formattedText = Regex.Replace(formattedText, "(</span(?!>)|</spa(?!n>)|</sp(?!an>)|</s(?!pan>)|</(?!span>))\\Z", "</span>");
            }
            else
            {
                formattedText = text;
            }
            formattedText = Shared.Utility.CommonFunctions.ProcessHighlightingText(text, formattedText);

            return formattedText;
        }

        /*

        public static Boolean CheckAuthentication()
        {
            try
            {
                Int64 customerID = 0;
                bool isValidSession = false;

                if (IQMedia.Web.Common.Authentication.CurrentUser != null)
                {
                    isValidSession = IsUserInCacheBySessionID();
                }

                if (!isValidSession || !IQMedia.Web.Common.Authentication.IsAuthenticated)
                {
                    RemoveUserFromCacheBySessionID();

                    if (IQMedia.Web.Common.Authentication.IsAuthenticated)
                    {
                        IQMedia.Web.Common.Authentication.Logout(); 
                    }

                    return false;
                }
                else
                {
                    var sessionInformation = ActiveUserMgr.GetActiveUser();
                    customerID = sessionInformation.CustomerKey;

                    if (sessionInformation != null && sessionInformation.IsLogIn)
                    {
                        if (!(sessionInformation.AuthorizedVersion == 0 || sessionInformation.AuthorizedVersion == 4))
                        {
                            RemoveUserFromCacheBySessionID();

                            if (IQMedia.Web.Common.Authentication.IsAuthenticated)
                            {
                                IQMedia.Web.Common.Authentication.Logout(); 
                            }

                            return false;
                        }
                        else
                        {
                            AddUserIntoCache(sessionInformation.LoginID, sessionInformation.MultiLogin.HasValue ? sessionInformation.MultiLogin.Value : false);

                            return true;
                        }
                    }
                    else
                    {
                        var currentUser = IQMedia.Web.Common.Authentication.CurrentUser;

                        customerID = currentUser.CustomerID;

                        CustomerLogic customerLogic = (CustomerLogic)IQMedia.Web.Logic.Base.LogicFactory.GetLogic(IQMedia.Web.Logic.Base.LogicType.Customer);
                        CustomerModel customerModel = customerLogic.GetClientGUIDByCustomerGUID(currentUser.Guid);

                        if (!(customerModel.AuthorizedVersion == 0 || customerModel.AuthorizedVersion == 4))
                        {
                            RemoveUserFromCacheBySessionID();

                            if (IQMedia.Web.Common.Authentication.IsAuthenticated)
                            {
                                IQMedia.Web.Common.Authentication.Logout(); 
                            }

                            return false;
                        }

                        List<CustomerRoleModel> customerRoles = customerLogic.GetCustomerRoles(currentUser.Guid);

                        FillCustomerRoles(customerModel, customerRoles);

                        AddUserIntoCache(customerModel.LoginID, customerModel.MultiLogin.HasValue ? customerModel.MultiLogin.Value : false);

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        */

        public static bool ValidateCaptcha(string p_CaptchaResponse,out RecaptchaErrorCodes? outRecaptchaErrCode)
        {
            outRecaptchaErrCode = null;

            bool isValid = false;
            var validateResponse = "";

            string dataString = "secret=" + ConfigurationManager.AppSettings["RecaptchaSecretKey"]
                                + "&response=" + p_CaptchaResponse;

            byte[] data = Encoding.UTF8.GetBytes(dataString);

            string url = ConfigurationManager.AppSettings["RecaptchaVerifyAPI"];

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";

            webRequest.ContentLength = data.Length;

            using (Stream dataStream = webRequest.GetRequestStream())
            {
                // Write the data to the request stream.
                dataStream.Write(data, 0, data.Length);
            }

            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            using (StreamReader responseReader = new StreamReader(webResponse.GetResponseStream()))
            {
                validateResponse = responseReader.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(validateResponse))
            {
                var res = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(validateResponse);                

                if (!Convert.ToBoolean(res["success"]))
                {
                    switch (Convert.ToString(res["error-codes"]))
                    {
                        case "missing-input-secret":
                            outRecaptchaErrCode = RecaptchaErrorCodes.MissingSecret;
                            break;
                        case "invalid-input-secret":
                            outRecaptchaErrCode = RecaptchaErrorCodes.InvalidSecret;
                            break;
                        case "missing-input-response":
                            outRecaptchaErrCode = RecaptchaErrorCodes.MissingUserResponse;
                            break;
                        case "invalid-input-response":
                            outRecaptchaErrCode = RecaptchaErrorCodes.InvalidUserResponse;
                            break;
                        default:
                            outRecaptchaErrCode = RecaptchaErrorCodes.Unknown;
                            break;
                    }
                }
                else
                {
                    isValid = true;
                }
            }

            return isValid;            
        }

        public static void WriteException(Exception p_ex, string p_Info="")
        {
            ActiveUser user = ActiveUserMgr.GetActiveUser();

            Guid? customerGUID=null;

            if (user != null)
            {
                customerGUID = user.CustomerGUID;
            }

            UtilityLogic.WriteException(p_ex,customerGUID, p_Info);
        }

    }
}
