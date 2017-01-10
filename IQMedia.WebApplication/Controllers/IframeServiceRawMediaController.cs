using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Common;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.Model;
using System.Configuration;
using System.Web.Security;
using IQMedia.WebApplication.Utility;
using System.IO;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class IframeServiceRawMediaController : Controller
    {
        //
        // GET: /IframeServiceRawMedia/

        public ActionResult Index()
        {
            PlayerObjectsModel playerObjects = null;

            try
            {
                Guid rawMediaGUID;
                Int64 iQAgentID;
                string searchTerm;
                string iqCCKey;

                if (!string.IsNullOrWhiteSpace(Request.QueryString["RawMediaID"]) || !string.IsNullOrWhiteSpace(Request.QueryString["IQAgentID"]) || !string.IsNullOrWhiteSpace(Request.QueryString["SeqID"]))
                {
                    bool isAuthenticated = false;
                    if (Authentication.IsAuthenticated)
                    {
                        isAuthenticated = true;
                    }
                    else if (!string.IsNullOrWhiteSpace(Request.QueryString["AU"]))
                    {

                        FormsAuthenticationTicket formAuthTicket = System.Web.Security.FormsAuthentication.Decrypt(Request.QueryString["AU"].Trim());

                        string[] roles = formAuthTicket.UserData.Split(new char[] { '|' });

                        System.Security.Principal.IIdentity id = new System.Web.Security.FormsIdentity(formAuthTicket);

                        System.Security.Principal.IPrincipal principal = new System.Security.Principal.GenericPrincipal(id, roles);

                        System.Web.HttpContext.Current.User = principal;

                        if (Authentication.IsAuthenticated && (formAuthTicket != null && !formAuthTicket.Expired))
                        {
                            isAuthenticated = true;
                            HttpCookie httpCookie = new HttpCookie(".IQAUTH");
                            httpCookie.Value = Convert.ToString(Request.QueryString["AU"]);
                            httpCookie.Domain = ".iqmediacorp.com";

                            Response.Cookies.Add(httpCookie);
                        }
                    }

                    if (isAuthenticated)
                    {
                        if (!string.IsNullOrWhiteSpace(Request.QueryString["RawMediaID"]) && Guid.TryParse(Request.QueryString["RawMediaID"].Trim(), out rawMediaGUID))
                        {
                            playerObjects = GeneratePlayerHtml(rawMediaGUID, Request.QueryString["SearchTerm"]);
                        }
                        else if ((!string.IsNullOrWhiteSpace(Request.QueryString["IQAgentID"]) && Int64.TryParse(Request.QueryString["IQAgentID"].Trim(), out iQAgentID)) || (!string.IsNullOrWhiteSpace(Request.QueryString["SeqID"]) && Int64.TryParse(Request.QueryString["SeqID"].Trim(), out iQAgentID)))
                        {
                            IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                            iQAgentLogic.GetSearchTermByIQAgentTVResultID(Authentication.CurrentUser.ClientGuid.Value, iQAgentID, out rawMediaGUID, out searchTerm, out iqCCKey);
                            playerObjects = GeneratePlayerHtml(rawMediaGUID, searchTerm);
                        }
                        ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                        var manualClipDuration = clientLogic.GetClientManualClipDurationSettings(Authentication.CurrentUser.ClientGuid.Value);
                        ViewBag.ManualClipDuration = manualClipDuration;

                        if (!string.IsNullOrWhiteSpace(Request.QueryString["css"]) && clientLogic.GetAPIIframeCSSOverrideSettings(Authentication.CurrentUser.ClientGuid.Value) && string.Compare(Path.GetExtension(Request.QueryString["css"]),".css",0)==0)
                        {
                            ViewBag.ClientCSS = Request.QueryString["css"];
                        }
                    }
                    else
                    {
                        return Redirect("~/Error/Unauthorized");
                    }
                }
                else
                {
                    return Redirect("~/Error/Unauthorized");
                }

                return View(playerObjects);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Redirect("~/Error/Unauthorized");
            }           
        }

        public ActionResult RawMediaPlayer()
        {
            PlayerObjectsModel playerObjects = null;

            try
            {
                Guid rawMediaGUID;
                Int64 iQAgentID;
                string searchTerm;
                string iqCCKey;

                if (!string.IsNullOrWhiteSpace(Request.QueryString["RawMediaID"]) || !string.IsNullOrWhiteSpace(Request.QueryString["IQAgentID"]) || !string.IsNullOrWhiteSpace(Request.QueryString["SeqID"]))
                {
                    bool isAuthenticated = false;
                    if (Authentication.IsAuthenticated)
                    {
                        isAuthenticated = true;
                    }
                    else if (!string.IsNullOrWhiteSpace(Request.QueryString["AU"]))
                    {

                        FormsAuthenticationTicket formAuthTicket = System.Web.Security.FormsAuthentication.Decrypt(Request.QueryString["AU"].Trim());

                        string[] roles = formAuthTicket.UserData.Split(new char[] { '|' });

                        System.Security.Principal.IIdentity id = new System.Web.Security.FormsIdentity(formAuthTicket);

                        System.Security.Principal.IPrincipal principal = new System.Security.Principal.GenericPrincipal(id, roles);

                        System.Web.HttpContext.Current.User = principal;

                        if (Authentication.IsAuthenticated && (formAuthTicket != null && !formAuthTicket.Expired))
                        {
                            isAuthenticated = true;
                            HttpCookie httpCookie = new HttpCookie(".IQAUTH");
                            httpCookie.Value = Convert.ToString(Request.QueryString["AU"]);
                            httpCookie.Domain = ".iqmediacorp.com";

                            Response.Cookies.Add(httpCookie);
                        }
                    }

                    if (isAuthenticated)
                    {
                        if (!string.IsNullOrWhiteSpace(Request.QueryString["RawMediaID"]) && Guid.TryParse(Request.QueryString["RawMediaID"].Trim(), out rawMediaGUID))
                        {
                            playerObjects = GeneratePlayerHtml(rawMediaGUID, Request.QueryString["SearchTerm"]);
                        }
                        else if ((!string.IsNullOrWhiteSpace(Request.QueryString["IQAgentID"]) && Int64.TryParse(Request.QueryString["IQAgentID"].Trim(), out iQAgentID)) || (!string.IsNullOrWhiteSpace(Request.QueryString["SeqID"]) && Int64.TryParse(Request.QueryString["SeqID"].Trim(), out iQAgentID)))
                        {
                            IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                            iQAgentLogic.GetSearchTermByIQAgentTVResultID(Authentication.CurrentUser.ClientGuid.Value, iQAgentID, out rawMediaGUID, out searchTerm, out iqCCKey);
                            playerObjects = GeneratePlayerHtml(rawMediaGUID, searchTerm);
                        }
                        ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                        var manualClipDuration = clientLogic.GetClientManualClipDurationSettings(Authentication.CurrentUser.ClientGuid.Value);
                        ViewBag.ManualClipDuration = manualClipDuration;
                    }
                    else
                    {
                        return Redirect("~/Error/Unauthorized");
                    }
                }
                else
                {
                    return Redirect("~/Error/Unauthorized");
                }

                return View(playerObjects);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Redirect("~/Error/Unauthorized");
            }
        }

        private PlayerObjectsModel GeneratePlayerHtml(Guid p_RawMediaGUID, string SearchTerm)
        {
            PlayerObjectsModel playerObjects;

            CustomerLogic clientLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
            var customer = clientLogic.GetClientGUIDByCustomerGUID(IQMedia.Web.Common.Authentication.CurrentUser.Guid);

            PlayerParamModel playerParams = new PlayerParamModel();

            playerParams.CustomerGUID = customer.CustomerGUID;
            playerParams.ClientGUID = customer.ClientGUID;
            playerParams.RawMediaGUID = p_RawMediaGUID;
            playerParams.SearchTerm = !string.IsNullOrEmpty(SearchTerm) ? SearchTerm.Trim() : string.Empty;
            playerParams.ServiceBaseURL = ConfigurationManager.AppSettings["ServicesBaseURL"];
            playerParams.PlayerLogoImage = customer.ClientPlayerLogoImage;
            playerParams.IsActivePlayerLogo = customer.IsClientPlayerLogoActive.Value;
            playerParams.Offset = Convert.ToInt32(Request.QueryString["Offset"]);
            playerParams.PlayerDefaultOffset = Convert.ToInt32(ConfigurationManager.AppSettings["PlayerDefaultOffset"]);
            playerParams.EnableCC = true;
            playerParams.BrowserType = Request.Browser.Type;
            playerParams.KeyValues = !string.IsNullOrEmpty(Request.QueryString["KeyValues"]) ? Request.QueryString["KeyValues"].Trim() : string.Empty;
            playerParams.AutoPlayback = !string.IsNullOrEmpty(Request.QueryString["xdomplayer"]) && Convert.ToInt32(Request.QueryString["xdomplayer"].Trim()) ==1? false: true;
            playerParams.PreviewImageOption = !string.IsNullOrWhiteSpace(Request.QueryString["pio"]) ? Convert.ToChar(Request.QueryString["pio"].Trim().ToUpper()) : 'N';
            playerParams.PreviewImageURL = (playerParams.PreviewImageOption == 'C' && !string.IsNullOrWhiteSpace(Request.QueryString["piu"])) ? Request.QueryString["piu"].Trim() : "";

            PlayerLogic playerLogic = (PlayerLogic)LogicFactory.GetLogic(LogicType.Player);
            playerObjects = playerLogic.GetRawMediaPlayerNCC(playerParams, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.TV.ToString(), null, null));
            playerObjects.KeyValues = !string.IsNullOrEmpty(Request.QueryString["KeyValues"]) ? Request.QueryString["KeyValues"].Trim() : string.Empty;
            playerObjects.RawMediaGUID = p_RawMediaGUID;

            return playerObjects;
        }
    }
}
