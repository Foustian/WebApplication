using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Model;
using System.Text;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.Common.Util;
using System.Xml.Linq;
using System.Configuration;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class BasicPlayerController : Controller
    {
        //
        // GET: /RawMediaPlayer/

        [HttpGet]
        public ActionResult Index(string media)
        {
            IQAgentReport_RawMediaPlayer objIQAgentReport_RawMediaPlayer = new IQAgentReport_RawMediaPlayer();
            try
            {
                if (!string.IsNullOrEmpty(media))
                {
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] Key = encoding.GetBytes("43DD9199E882F08814E1864B45E4F117");
                    byte[] IV = encoding.GetBytes("40275DC0B57CD8D6");

                    int offset = 0;
                    string decryptedText = IQMedia.Shared.Utility.CommonFunctions.DecryptStringFromBytes_Aes(media, Key, IV);

                    IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                    objIQAgentReport_RawMediaPlayer = iqAgentLogic.GetIQAgentReport_DetailsToPlayRawMedia(decryptedText, out offset);

                    if (!string.IsNullOrEmpty(objIQAgentReport_RawMediaPlayer.RawMediaGuid) && !string.IsNullOrEmpty(objIQAgentReport_RawMediaPlayer.SearchTerm))
                    {
                        string captionString = string.Empty;
                        List<int> SearchTermList = new List<int>(); // needed for TAds 
                        int? offset1 = 0;
                        string highlightString = UtilityLogic.GetRawMediaCaption(objIQAgentReport_RawMediaPlayer.SearchTerm, new Guid(objIQAgentReport_RawMediaPlayer.RawMediaGuid), out offset1, out captionString, WebApplication.Utility.CommonFunctions.GeneratePMGUrl(WebApplication.Utility.CommonFunctions.PMGUrlType.TV.ToString(), null, null), out SearchTermList);
                        objIQAgentReport_RawMediaPlayer.ClosedCaption = captionString;
                        objIQAgentReport_RawMediaPlayer.CC_Highlight = highlightString;
                        offset = offset1.HasValue ? offset1.Value : offset;

                    }

                    ViewBag.IsVideoExpired = objIQAgentReport_RawMediaPlayer.ExpiryDate.HasValue ? (objIQAgentReport_RawMediaPlayer.ExpiryDate.Value < DateTime.Today) : false;

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

                            XElement varsele = xdoc.Root.Descendants("vars").FirstOrDefault();
                            if (varsele != null)
                            {
                                string fileName = varsele.Attribute("fileName").Value;
                                string streamUrl = varsele.Attribute("streamUrl").Value;
                                string appNameFMS = varsele.Attribute("appNameFMS").Value;
                                string mediaurl = string.Format("http://{0}/{1}/{2}.m3u8", streamUrl, appNameFMS, fileName);
                                //objIQAgentReport_RawMediaPlayer.RawMediaPlayerHTML = "<iframe frameborder=\"0\" class=\"span11\" style=\"height:200px\" src=\"" + mediaurl + "\" allowfullscreen=\"1\" mozallowfullscreen=\"1\" webkitallowfullscreen=\"1\"></iframe>";
                                objIQAgentReport_RawMediaPlayer.RawMediaPlayerHTML = "<video  class=\"span11\" style=\"height:200\" autoplay=\"\" controls=\"\" id=\"myvideo\" src=\"" + mediaurl + "\"></video>";
                            }

                            Shared.Utility.Log4NetLogger.Info("Client's IP :" + HttpContext.Request.UserHostAddress);

                            string LogRawMediaPlayUrl = string.Format(ConfigurationManager.AppSettings["LogRawMediaPlayService"], objIQAgentReport_RawMediaPlayer.RawMediaGuid, Request.Url.AbsoluteUri, HttpContext.Request.UserHostAddress);

                            Shared.Utility.Log4NetLogger.Info("LogRawMediaPlay URL : " + LogRawMediaPlayUrl);

                            string response = Shared.Utility.CommonFunctions.DoHttpGetRequest(LogRawMediaPlayUrl, Request.UserAgent);
                            Shared.Utility.Log4NetLogger.Info("LogRawMediaPlay Reponse :" + respone);
                        }
                        else
                        {
                            offset = offset - Convert.ToInt32(ConfigurationManager.AppSettings["PlayerDefaultOffset"]) > 0 ? offset - Convert.ToInt32(ConfigurationManager.AppSettings["PlayerDefaultOffset"]) : offset;
                            objIQAgentReport_RawMediaPlayer.RawMediaPlayerHTML = UtilityLogic.RenderBasicRawMediaPlayer(media, true, offset, Request.Browser.Type);
                        }
                    }

                    ViewBag.IsErrorOccurred = false;
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                ViewBag.IsErrorOccurred = true;
            }

            return View(objIQAgentReport_RawMediaPlayer);
        }

    }
}
