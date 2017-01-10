using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Model;
using System.Configuration;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.WebApplication.Utility;
using System.Text.RegularExpressions;
using IQMedia.Common.Util;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class ClipPlayerController : Controller
    {
        public string ClientGUID
        {
            get
            {
                ActiveUser sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                if (sessionInformation != null)
                    return sessionInformation.ClientGUID.ToString();
                else
                    return null;
            }
        }

        public string ClipImage
        {
            get;
            set;
        }

        //
        // GET: /ClipPlayer/

        [HttpGet]
        public ActionResult Index(string clipid)
        {
            Dictionary<string, string> clipModel = new Dictionary<string, string>();
            Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " ClipID : " + clipid);

            Shared.Utility.Log4NetLogger.Info("Start ClipPlayer Load");

            try
            {
                Shared.Utility.Log4NetLogger.Info("input clipid : " + clipid);

                Guid temp;
                if (!string.IsNullOrEmpty(clipid) && Guid.TryParse(clipid, out temp))
                {
                    Shared.Utility.Log4NetLogger.Info("clip id pased to guid");                   

                    IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                    Shared.Utility.Log4NetLogger.Info("created archive object");


                    string ClipTitle = string.Empty;

                    Shared.Utility.Log4NetLogger.Info("going to check for browser type : " + Request.UserAgent);
                    if (!string.IsNullOrEmpty(Request.UserAgent) && (Request.UserAgent.ToLower().Contains("ipad") || Request.UserAgent.ToLower().Contains("iphone") || Request.UserAgent.ToLower().Contains("ipod") || (Request.UserAgent.ToLower().Contains("android") && Utility.CommonFunctions.CheckVersion())))
                    {
                        clipModel["IsIOSAndroid"] = "True";
                        Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " " + Request.UserAgent);

                        string url = string.Format(ConfigurationManager.AppSettings["IOSSVCGetVarsUrl"], clipid);

                        string respone = IQMedia.Shared.Utility.CommonFunctions.DoHttpGetRequest(url);

                        Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " IOSGetVars Response : " + respone);

                        Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();
                        jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(respone.Replace(@"\", string.Empty).Replace("([", string.Empty).Replace("])", string.Empty).Replace("\"", "'"));

                        Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " Parsed Json : " + string.Join(",", jsonData.ToString()));

                        if (Convert.ToBoolean(jsonData["IsValidMedia"]))
                        {
                            string media = Convert.ToString(jsonData["Media"]);

                            string logurl = ConfigurationManager.AppSettings["IOSAndroidUpdateClipPlayLogURL"] != null ? String.Format(ConfigurationManager.AppSettings["IOSAndroidUpdateClipPlayLogURL"], "%d", "%d") : "";
                            clipModel["ClipPlayer"] = String.Format("<video  class=\"span11\" style=\"height:100%; width:100%;\" autoplay=\"\" controls=\"\" id=\"myvideo\" src=\"{0}.m3u8\" data-log-url-pattern=\"{1}\" data-update-interval=\"{2}\"></video>", media, logurl, ConfigurationManager.AppSettings["ClipLogUpdateInterval"]);

                        }
                    }
                    else
                    {
                        Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " going to fetch clip title");
                        ClipTitle = iQArchieveLogic.GetClipTitleByClipID(clipid);

                        clipModel["ClipTitle"] = ClipTitle;
                    }
                }
                else
                {
                    Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " Invalid ClipID");
                }
            }
            catch (Exception exception)
            {
                Shared.Utility.Log4NetLogger.Debug(Session.SessionID + "ClipPlayer Error : " + exception);
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
            }

            Shared.Utility.Log4NetLogger.Info("End ClipPlayer Load");

            return View(clipModel);
        }

        [HttpGet]
        public ActionResult Index2(string clipid)
        {
            Dictionary<string, string> clipModel = new Dictionary<string, string>();
            Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " ClipID : " + clipid);

            Shared.Utility.Log4NetLogger.Info("Start ClipPlayer Load");

            try
            {
                Shared.Utility.Log4NetLogger.Info("input clipid : " + clipid);

                Guid temp;
                if (!string.IsNullOrEmpty(clipid) && Guid.TryParse(clipid, out temp))
                {
                    Shared.Utility.Log4NetLogger.Info("clip id pased to guid");                   

                    IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                    Shared.Utility.Log4NetLogger.Info("created archive object");


                    string ClipTitle = string.Empty;

                    Shared.Utility.Log4NetLogger.Info("going to check for browser type : " + Request.UserAgent);
                    if (!string.IsNullOrEmpty(Request.UserAgent) && (Request.UserAgent.ToLower().Contains("ipad") || Request.UserAgent.ToLower().Contains("iphone") || Request.UserAgent.ToLower().Contains("ipod") || (Request.UserAgent.ToLower().Contains("android") && Utility.CommonFunctions.CheckVersion())))
                    {
                        clipModel["IsIOSAndroid"] = "True";
                        Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " " + Request.UserAgent);

                        string url = string.Format(ConfigurationManager.AppSettings["IOSSVCGetVarsUrl"], clipid);

                        string respone = IQMedia.Shared.Utility.CommonFunctions.DoHttpGetRequest(url);

                        Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " IOSGetVars Response : " + respone);

                        Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();
                        jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(respone.Replace(@"\", string.Empty).Replace("([", string.Empty).Replace("])", string.Empty).Replace("\"", "'"));

                        Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " Parsed Json : " + string.Join(",", jsonData.ToString()));

                        if (Convert.ToBoolean(jsonData["IsValidMedia"]))
                        {
                            string media = Convert.ToString(jsonData["Media"]);

                            string logurl = ConfigurationManager.AppSettings["IOSAndroidUpdateClipPlayLogURL"] != null ? String.Format(ConfigurationManager.AppSettings["IOSAndroidUpdateClipPlayLogURL"], "%d", "%d") : "";
                            clipModel["ClipPlayer"] = String.Format("<video  class=\"span11\" style=\"height:100%; width:100%;\" autoplay=\"\" controls=\"\" id=\"myvideo\" src=\"{0}.m3u8\" data-log-url-pattern=\"{1}\" data-update-interval=\"{2}\"></video>", media, logurl, ConfigurationManager.AppSettings["ClipLogUpdateInterval"]);

                        }
                    }
                    else
                    {
                        Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " going to fetch clip title");
                        ClipTitle = iQArchieveLogic.GetClipTitleByClipID(clipid);

                        clipModel["ClipTitle"] = ClipTitle;
                    }
                }
                else
                {
                    Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " Invalid ClipID");
                }
            }
            catch (Exception exception)
            {
                Shared.Utility.Log4NetLogger.Debug(Session.SessionID + "ClipPlayer Error : " + exception);
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
            }

            Shared.Utility.Log4NetLogger.Info("End ClipPlayer Load");

            return View(clipModel);
        }

    }
}
