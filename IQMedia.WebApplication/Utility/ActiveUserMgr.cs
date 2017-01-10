using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMedia.Model;
using System.Collections;
using System.Web.Security;
using Alachisoft.NCache.Web.Caching;
using System.Configuration;
using IQMedia.Common.Util;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using Alachisoft.NCache.Runtime.Exceptions;

namespace IQMedia.WebApplication.Utility
{
    public static class ActiveUserMgr
    {
        internal static void AddUserIntoCache(ActiveUser p_User)
        {
            Alachisoft.NCache.Web.Caching.Cache cache = GetCacheInstance();

            if (!p_User.MultiLogin.Value)
            {
                Hashtable values = new Hashtable();
                values.Add("LoginID", p_User.LoginID);

                string query = "SELECT IQMedia.Model.ActiveUser where this.LoginID = ?";
                ICollection cacheKeys = cache.Search(query, values);

                if (cacheKeys.Count > 0)
                {
                    (new IQMedia.Web.Logic.SessionLogic()).DeleteByLoginID(p_User.LoginID);

                    foreach (string key in cacheKeys)
                    {
                        cache.Delete(key);
                    }
                }
            }

            FormsAuthenticationTicket formAuthTicket = System.Web.Security.FormsAuthentication.Decrypt(System.Web.HttpContext.Current.Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName].Value);

            p_User.SessionTimeOut = formAuthTicket.Expiration;
            p_User.LastAccessTime = DateTime.Now;
            p_User.SessionID = HttpContext.Current.Session.SessionID;

            var ipadd = CommonFunctions.GetAllNetworkInterfaceIpv4Addresses();

            p_User.Server = ipadd[0].ToString();

            SessionModel session = new SessionModel { SessionID = HttpContext.Current.Session.SessionID, LoginID = p_User.LoginID, LastAccessTime = p_User.LastAccessTime, SessionTimeOut = formAuthTicket.Expiration, Server = ipadd[0].ToString() };
            (new IQMedia.Web.Logic.SessionLogic()).InsertSession(session);

            InsertUserIntoCache(p_User, cache,HttpContext.Current.Session.SessionID);
        }

        private static Alachisoft.NCache.Web.Caching.Cache GetCacheInstance()
        {
            Alachisoft.NCache.Web.Caching.Cache cache = null;

            try
            {
                cache = (Alachisoft.NCache.Web.Caching.Cache)HttpContext.Current.Application["ActiveUsers"];

                if (cache == null)
                {
                    System.Web.HttpContext.Current.Application["ActiveUsers"] = NCache.InitializeCache(ConfigurationManager.AppSettings["CacheActiveUsers"]);
                }

                lock (cache)
                {
                    Hashtable values = new Hashtable();

                    string query = "SELECT IQMedia.Model.ActiveUser";

                    ICollection cacheKeys = cache.Search(query, values);

                    if (cacheKeys.Count == 0)
                    {
                        PopulateCacheFromDB(cache);
                    }
                }
            }
            catch (Exception ex)
            {
                Shared.Utility.Log4NetLogger.Error("CacheError: ", ex);
                IQMedia.Web.Logic.UtilityLogic.WriteException(ex);
            }

            return cache;
        }

        private static void PopulateCacheFromDB(Alachisoft.NCache.Web.Caching.Cache p_Cache)
        {
            SessionLogic sLgc = (SessionLogic)LogicFactory.GetLogic(LogicType.Session);

            List<ActiveUser> activeUsers = sLgc.GetActiveUsersFromDB();

            foreach (ActiveUser user in activeUsers)
            {
                user.IsLogIn = true;

                InsertUserIntoCache(user, p_Cache,user.SessionID);
            }
        }

        private static void InsertUserIntoCache(ActiveUser p_User, Alachisoft.NCache.Web.Caching.Cache p_Cache,string p_SessionID)
        {
            p_Cache.Insert("S-" + p_SessionID, p_User, p_User.SessionTimeOut, Alachisoft.NCache.Web.Caching.Cache.NoSlidingExpiration, Alachisoft.NCache.Runtime.CacheItemPriority.Normal);
        }

        internal static bool IsUserInCacheBySessionID()
        {
            var isUserInCache = false;

            Cache cache = GetCacheInstance();
            bool userInCache = cache.Contains("S-" + HttpContext.Current.Session.SessionID);

            if (!userInCache)
            {
                SessionLogic slgc = (SessionLogic)LogicFactory.GetLogic(LogicType.Session);
                ActiveUser user = slgc.GetUserBySessionID(HttpContext.Current.Session.SessionID);

                if (user != null)
                {
                    cache.Insert("S-" + HttpContext.Current.Session.SessionID, user, user.SessionTimeOut, Alachisoft.NCache.Web.Caching.Cache.NoSlidingExpiration, Alachisoft.NCache.Runtime.CacheItemPriority.Normal);
                    isUserInCache = true;
                }
            }
            else
            {
                isUserInCache = true;
            }

            return isUserInCache;
        }

        internal static void RemoveUserFromCacheBySessionID()
        {            
            (new IQMedia.Web.Logic.SessionLogic()).DeleteBySessionID(HttpContext.Current.Session.SessionID);
            Cache cache = GetCacheInstance();            
            cache.Delete("S-" + HttpContext.Current.Session.SessionID);
        }

        internal static void RemoveUserFromCacheBySessionID(string p_SessionID)
        {
            (new IQMedia.Web.Logic.SessionLogic()).DeleteBySessionID(p_SessionID);
            Cache cache = GetCacheInstance();
            cache.Delete("S-" + p_SessionID);
        }

        internal static Boolean CheckAuthentication()
        {
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
                return true;
            }
        }

        public static ActiveUser GetActiveUser()
        {
            var user = GetUserBySessionID();
            return user == null ? new ActiveUser() : user;
        }

        private static ActiveUser GetUserBySessionID()
        {
            ActiveUser user = null;            

            if (IsUserInCacheBySessionID())
            {
                Cache cache = GetCacheInstance();
                user = (ActiveUser)cache.Get("S-" + HttpContext.Current.Session.SessionID);
            }

            return user;
        }

        internal static void UpdateUserIntoCache()
        {
            var user = GetUserBySessionID();

            user.LastAccessTime = DateTime.Now;

            FormsAuthenticationTicket formAuthTicket = System.Web.Security.FormsAuthentication.Decrypt(System.Web.HttpContext.Current.Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName].Value);

            if (user.SessionTimeOut != formAuthTicket.Expiration)
            {
                user.SessionTimeOut = formAuthTicket.Expiration;

                var sLgc = (SessionLogic)LogicFactory.GetLogic(LogicType.Session);
                sLgc.Update(new SessionModel() { SessionID = HttpContext.Current.Session.SessionID, LastAccessTime = user.LastAccessTime, SessionTimeOut = user.SessionTimeOut });
            }

            Cache cache = GetCacheInstance();
            InsertUserIntoCache(user, cache,HttpContext.Current.Session.SessionID);
        }

        internal static List<ActiveUser> GetAllActiveUsers()
        {
            List<ActiveUser> activeUsers = new List<ActiveUser>();

            Cache cache = GetCacheInstance();

            Hashtable values = new Hashtable();            

            string query = "SELECT IQMedia.Model.ActiveUser";

            ICollection cacheKeys = cache.Search(query, values);

            foreach (var key in cacheKeys)
            {
                activeUsers.Add((ActiveUser)cache.Get(Convert.ToString(key)));
            }

            return activeUsers;
        }

        internal static void RemoveAllUsers()
        {
            (new IQMedia.Web.Logic.SessionLogic()).DeleteAll();
            Cache cache = GetCacheInstance();
            cache.Clear();
        }
    }
}
