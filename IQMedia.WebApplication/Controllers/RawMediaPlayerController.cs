using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using IQMedia.Model;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using System.Configuration;
using IQMedia.Common.Util;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class RawMediaPlayerController : Controller
    {
        //
        // GET: /RawMediaPlayer/

        [HttpGet]
        public ActionResult Index(string media)
        {
            IQAgentReport_RawMediaPlayer objIQAgentReport_RawMediaPlayer = new IQAgentReport_RawMediaPlayer();

            Shared.Utility.Log4NetLogger.Info("RawMedia Player Load Start");

            try
            {
                if (!string.IsNullOrEmpty(media))
                {
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] Key = encoding.GetBytes("43DD9199E882F08814E1864B45E4F117");
                    byte[] IV = encoding.GetBytes("40275DC0B57CD8D6");

                    int offset = 0;
                    string decryptedText = IQMedia.Shared.Utility.CommonFunctions.DecryptStringFromBytes_Aes(media, Key, IV);

                    Shared.Utility.Log4NetLogger.Info("decryptedText = " + decryptedText);

                    Shared.Utility.Log4NetLogger.Info("going to fetch rawmedia details by iqagent iframe id.");

                    IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                    objIQAgentReport_RawMediaPlayer = iqAgentLogic.GetIQAgentReport_DetailsToPlayRawMedia(decryptedText, out offset);

                    Shared.Utility.Log4NetLogger.Info("details fetched from DB successfully");

                    if (objIQAgentReport_RawMediaPlayer != null && !string.IsNullOrEmpty(objIQAgentReport_RawMediaPlayer.RawMediaGuid) && !string.IsNullOrEmpty(objIQAgentReport_RawMediaPlayer.SearchTerm))
                    {
                        Shared.Utility.Log4NetLogger.Info("going to fetch closed caption and highlights from solr as per search term");

                        int? offset1 = 0;
                        string captionString = string.Empty;
                        string highlightString = string.Empty;
                        List<int> SearchTermList = new List<int>(); // needed for TAds

                        if (String.IsNullOrEmpty(objIQAgentReport_RawMediaPlayer.DataModelType) || objIQAgentReport_RawMediaPlayer.DataModelType == "TV")
                        { 
                            highlightString = UtilityLogic.GetRawMediaCaption(objIQAgentReport_RawMediaPlayer.SearchTerm, new Guid(objIQAgentReport_RawMediaPlayer.RawMediaGuid), out offset1, out captionString, WebApplication.Utility.CommonFunctions.GeneratePMGUrl(WebApplication.Utility.CommonFunctions.PMGUrlType.TV.ToString(), null, null), out SearchTermList);
                        }
                        else
                        {
                            highlightString = RadioLogic.GetRawMediaCaption(objIQAgentReport_RawMediaPlayer.SearchTerm, new Guid(Convert.ToString(objIQAgentReport_RawMediaPlayer.RawMediaGuid)), Convert.ToInt32(ConfigurationManager.AppSettings["IQRadioFragOffset"]), Convert.ToInt32(ConfigurationManager.AppSettings["IQRadioFragSize"]), Convert.ToBoolean(ConfigurationManager.AppSettings["IQRadioIsLogging"]), ConfigurationManager.AppSettings["IQRadioLogFileLocation"], ConfigurationManager.AppSettings["IQRadioSolrFL"], out offset1, out captionString, IQMedia.WebApplication.Utility.CommonFunctions.GeneratePMGUrl(IQMedia.WebApplication.Utility.CommonFunctions.PMGUrlType.QR.ToString(), null, null), out SearchTermList); 
                        } 
                        
                        objIQAgentReport_RawMediaPlayer.ClosedCaption = captionString;
                        objIQAgentReport_RawMediaPlayer.CC_Highlight = highlightString; 
                        offset = offset1.HasValue ? offset1.Value : offset;

                        Shared.Utility.Log4NetLogger.Info("fetched cc and highlights from solr");                
                    }

                    Shared.Utility.Log4NetLogger.Info("check is video is expired?");

                    ViewBag.IsVideoExpired = objIQAgentReport_RawMediaPlayer.ExpiryDate.HasValue ? (objIQAgentReport_RawMediaPlayer.ExpiryDate.Value <= DateTime.Now) : false;
                    var paddedOffset = offset - Convert.ToInt32(ConfigurationManager.AppSettings["PlayerDefaultOffset"]);
                    offset = paddedOffset > 0 ? paddedOffset : offset;

                    if (!string.IsNullOrEmpty(objIQAgentReport_RawMediaPlayer.RawMediaGuid))
                    {
                        if ((Request.UserAgent.ToLower().Contains("ipad") || Request.UserAgent.ToLower().Contains("iphone") || Request.UserAgent.ToLower().Contains("ipod")) || (Request.UserAgent.ToLower().Contains("android") && Utility.CommonFunctions.CheckVersion()))
                        {
                            Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " Mobile Device");

                            objIQAgentReport_RawMediaPlayer.IsMobileDevice = true;
                            objIQAgentReport_RawMediaPlayer.PlayOffset = offset;

                            string url = string.Format(ConfigurationManager.AppSettings["RawMediaGetVars"], HttpUtility.UrlEncode(media));

                            string respone = IQMedia.Shared.Utility.CommonFunctions.DoHttpGetRequest(url);

                            Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " GetVars Response : " + respone);

                            XDocument xdoc = XDocument.Parse(respone);

                            Shared.Utility.Log4NetLogger.Info("Client's IP :" + HttpContext.Request.UserHostAddress);

                            string LogRawMediaPlayUrl = string.Format(ConfigurationManager.AppSettings["LogRawMediaPlayService"], objIQAgentReport_RawMediaPlayer.RawMediaGuid, Request.Url.AbsoluteUri, HttpContext.Request.UserHostAddress);

                            Shared.Utility.Log4NetLogger.Info("LogRawMediaPlay URL : " + LogRawMediaPlayUrl);

                            string response = Shared.Utility.CommonFunctions.DoHttpGetRequest(LogRawMediaPlayUrl, Request.UserAgent);
                            Shared.Utility.Log4NetLogger.Info("LogRawMediaPlay Reponse :" + respone);

                            XElement varsele = xdoc.Root.Descendants("vars").FirstOrDefault();
                            if (varsele != null)
                            {
                                string fileName = varsele.Attribute("fileName").Value;
                                string streamUrl = varsele.Attribute("streamUrl").Value;
                                string appNameFMS = varsele.Attribute("appNameFMS").Value;
                                string mediaurl = string.Format("http://{0}/{1}/{2}.m3u8", streamUrl, appNameFMS, fileName);
                                string logurl = getUpdateURLPattern(response);
                                //objIQAgentReport_RawMediaPlayer.RawMediaPlayerHTML = "<iframe frameborder=\"0\" class=\"span11\" style=\"height:200px\" src=\"" + mediaurl + "\" allowfullscreen=\"1\" mozallowfullscreen=\"1\" webkitallowfullscreen=\"1\"></iframe>";
                                objIQAgentReport_RawMediaPlayer.RawMediaPlayerHTML = String.Format("<video class=\"span11\" style=\"height:200\" autoplay=\"\" controls=\"\" id=\"myvideo\" src=\"{0}\" data-log-url-pattern=\"{1}\" data-update-interval=\"{2}\"></video>", mediaurl, logurl, ConfigurationManager.AppSettings["ClipLogUpdateInterval"]);
                            }
                        }
                        else
                        {
                            Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " Web Device");
                            objIQAgentReport_RawMediaPlayer.RawMediaPlayerHTML = UtilityLogic.RenderRawMediaPlayer(media, true, offset, Request.Browser.Type);
                        }
                    }

                    ViewBag.IsErrorOccurred = false;
                }
            }
            catch (Exception _ex)
            {
                Shared.Utility.Log4NetLogger.Error(_ex);
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                ViewBag.IsErrorOccurred = true;
            }

            Shared.Utility.Log4NetLogger.Info("RawMedia Player Load End");

            return View(objIQAgentReport_RawMediaPlayer);
        }

        private string getUpdateURLPattern(string logResponse)
        {
            JObject responseJSON = JObject.Parse(logResponse);
            string updateURL = ConfigurationManager.AppSettings["UpdateRawMediaPlayLogService"];
            var logID = responseJSON.GetValue("PlayLogID");
            if (updateURL != null && logID != null)
            {
                return String.Format(updateURL, (long)logID, "%d");
            }
            return null;
        }
    }
}
