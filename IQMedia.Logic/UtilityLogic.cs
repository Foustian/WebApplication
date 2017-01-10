using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using System.Configuration;
using System.Web;
using PMGSearch;
using IQMedia.Web.Logic.Base;

namespace IQMedia.Web.Logic
{
    public static class UtilityLogic
    {
        public static string InsertActionLog(IQLog_UserActionsModel p_IQLog_UserActionsModel)
        {
            UtilityDA utilityDA = (UtilityDA)DataAccessFactory.GetDataAccess(DataAccessType.Utility);
            return utilityDA.InsertActionLog(p_IQLog_UserActionsModel);
        }

        public static void WriteException(Exception _Exception,Guid? CustomerGUID=null, string Info = "")
        {
            try
            {
                IQMediaGroupExceptions iQMediaGroupExceptions = new IQMediaGroupExceptions();
                iQMediaGroupExceptions.ExceptionStackTrace = "Inner Exception : " + _Exception.InnerException + " Stack Trace : " + _Exception.StackTrace;
                iQMediaGroupExceptions.ExceptionMessage = _Exception.Message + " " + Info;
                iQMediaGroupExceptions.CreatedBy = "Base - Write Exception";
                iQMediaGroupExceptions.CustomerGuid = CustomerGUID;

                Shared.Utility.Log4NetLogger.Error("Error encountered by user: " + (iQMediaGroupExceptions.CustomerGuid.HasValue ? iQMediaGroupExceptions.CustomerGuid.ToString() : "NULL"));
                Shared.Utility.Log4NetLogger.Error(_Exception);

                string _ReturnValue = string.Empty;
                UtilityDA utilityDA = (UtilityDA)DataAccessFactory.GetDataAccess(DataAccessType.Utility);
                utilityDA.InsertException(iQMediaGroupExceptions);

            }
            catch (Exception)
            {

            }
        }

        public static string RenderRawMediaPlayer(string p_UserID, string p_RawMediaID, string p_IsRawMedia, string p_IsUGCRawMedia, string p_ClientGUID, string IsAutoDownload, string p_CustomerGUID, string p_ServicesBaseURL, int? p_Offset, bool? IsActivePlayerLogo, string PlayerLogoImage, string _BrowserVersion, string p_KeyValues = null)
        {
            try
            {
                string _ParamValue = "userId=" + p_UserID + "&RL_User_GUID=" + ConfigurationManager.AppSettings["RL_User_GUID"] + "&clientGUID=" + p_ClientGUID + "&IsAutoDownload=" + IsAutoDownload + "&customerGUID=" + p_CustomerGUID + "&categoryCode=" + ConfigurationManager.AppSettings["CategoryCode"] + "&IsRawMedia=" + p_IsRawMedia + "&IsUGC=" + p_IsUGCRawMedia + "&embedId=" + p_RawMediaID + "&ServicesBaseURL=" + ConfigurationManager.AppSettings["ServicesBaseURL"] + "&autoPlayback=true&Offset=" + Convert.ToString(p_Offset) + "&ClipLength=" + ConfigurationManager.AppSettings["MaxClipLength"] + "&DefaultClipLength=" + ConfigurationManager.AppSettings["DefaultClipLength"] + "&sd=" + ConfigurationManager.AppSettings["SubDomainSWF"] + "&isec=" + ConfigurationManager.AppSettings["IsSecuredServiceSWF"];

                if (IsActivePlayerLogo != null && IsActivePlayerLogo == true && !string.IsNullOrEmpty(PlayerLogoImage))
                {
                    _ParamValue += "&PlayerLogo=" + ConfigurationManager.AppSettings["URLWaterMark"] + PlayerLogoImage + "";
                }

                if (!string.IsNullOrWhiteSpace(p_KeyValues))
                {
                    _ParamValue = _ParamValue + "&KeyValues=" + HttpContext.Current.Server.HtmlEncode(p_KeyValues);
                }

                StringBuilder _StringBuilder = new StringBuilder();

                _StringBuilder.Append("<object id=\"HUY\" name=\"HYETA\" type=\"application/x-shockwave-flash\"");

                if (_BrowserVersion.Contains("IE"))
                {
                    _StringBuilder.Append(" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"");
                }


                _StringBuilder.AppendFormat(" width=\"{0}\"", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat(" height=\"{0}\"", ConfigurationManager.AppSettings["RawMediaObjectHeight"].ToString());

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat(" data=\"{0}\"", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat(" data=\"{0}\"", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                }

                _StringBuilder.Append(">");

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                }

                _StringBuilder.Append("<param name=\"allowfullscreen\" value=\"true\"></param>");
                _StringBuilder.Append("<param name=\"allowscriptaccess\" value=\"always\"></param>");
                _StringBuilder.Append("<param name=\"quality\" value=\"high\"></param>");
                _StringBuilder.Append("<param name=\"wmode\" value=\"transparent\"></param>");
                _StringBuilder.AppendFormat("<param name=\"flashvars\" value=\"{0}\"></param>", _ParamValue);

                _StringBuilder.Append("<embed ");
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                }

                _StringBuilder.AppendFormat("width=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat("height=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectHeight"].ToString());
                _StringBuilder.Append("type=\"application/x-shockwave-flash\" ");
                _StringBuilder.Append("allowscriptaccess=\"true\" ");
                _StringBuilder.Append("allowfullscreen=\"always\" ");
                _StringBuilder.Append("name=\"IQMedia\" ");
                _StringBuilder.AppendFormat("flashvars=\"{0}\" ", _ParamValue);

                _StringBuilder.Append(" /></object>");

                return _StringBuilder.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string GetRawMediaCaption(string searchTerm, Guid rawMediaID, out Int32? offset, out string fullCaption, string pmgurl, out List<int> lstSearchTermHits, string p_Title120 = null, bool isOptiQ = false)
        {
            try
            {
                offset = null;
                fullCaption = string.Empty;
                StringBuilder strngCaption = new StringBuilder();
                StringBuilder strngFullCaption = new StringBuilder();
                //if (!string.IsNullOrWhiteSpace(searchTerm))
                //{
                Uri PMGSearchRequestUrl = new Uri(pmgurl);
                SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchRequest _SearchRequest = new SearchRequest();
                _SearchRequest.GuidList = Convert.ToString(rawMediaID);
                _SearchRequest.Terms = searchTerm.Trim();
                _SearchRequest.IsShowCC = true;
                if (!string.IsNullOrEmpty(p_Title120))
                {
                    _SearchRequest.Title120 = p_Title120.Trim();
                }


                if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings["SolrQTIframe"]))
                {
                    _SearchRequest.SolrQT = ConfigurationSettings.AppSettings["SolrQTIframe"];
                }

                int _PMGMaxHighlights = 20;

                if (ConfigurationSettings.AppSettings["PMGMaxHighlights"] != null)
                {
                    int.TryParse(ConfigurationSettings.AppSettings["PMGMaxHighlights"], out _PMGMaxHighlights);
                }

                _SearchRequest.MaxHighlights = _PMGMaxHighlights;

                SearchResult _SearchResult = _SearchEngine.Search(_SearchRequest);



                if (!string.IsNullOrEmpty(_SearchResult.ResponseXml) && _SearchResult.Hits.Count > 0)
                {
                    if (_SearchResult.Hits[0].ClosedCaption.Count > 0)
                    {
                        foreach (TermOccurrence _TermOccurrence in _SearchResult.Hits[0].ClosedCaption)
                        {
                            strngFullCaption.Append("<div onclick=\"setSeekPoint(" + (_TermOccurrence.TimeOffset - Convert.ToInt32(ConfigurationSettings.AppSettings["RawMediaCaptionDelay"].ToString()) > 0 ? _TermOccurrence.TimeOffset - Convert.ToInt32(ConfigurationSettings.AppSettings["RawMediaCaptionDelay"].ToString()) : 0) + ");\">"
                                                    + _TermOccurrence.SurroundingText + "</div>");
                        }

                        fullCaption = strngFullCaption.ToString();

                    }

                    if (_SearchResult.Hits[0].TermOccurrences.Count > 0)
                    {
                        _SearchResult.Hits[0].TermOccurrences = _SearchResult.Hits[0].TermOccurrences.OrderBy(o => o.TimeOffset).ToList();
                        foreach (TermOccurrence _TermOccurrence in _SearchResult.Hits[0].TermOccurrences)
                        {
                            if (offset == null)
                            {
                                offset = _TermOccurrence.TimeOffset;
                            }
                            strngCaption.Append("<div class=\"hit\" onclick=\"setSeekPoint(" + (_TermOccurrence.TimeOffset - Convert.ToInt32(ConfigurationSettings.AppSettings["RawMediaCaptionDelay"].ToString())) + ");\">"
                                                    + "<div class=\"boldgray\">" + formatOffset(_TermOccurrence.TimeOffset) + "</div>"
                                                    + "<div class=\"caption\">" + _TermOccurrence.SurroundingText + "</div>"
                                                + "</div>");
                        }
                    }
                    else if (!string.IsNullOrEmpty(p_Title120) && _SearchResult.Hits[0].StartMinute != null)
                    {
                        offset = _SearchResult.Hits[0].StartMinute * 60;
                    }
                    //else
                    //{
                    //    strngCaption.Append("No Results Found");
                    //}
                }

                if (isOptiQ)
                {
                    ImagiQLogic ImagiQLogic = (ImagiQLogic)LogicFactory.GetLogic(LogicType.ImagiQ);
                    List<ImagiQLogoModel> lstLogoHits = ImagiQLogic.GetLRResultsByGuid(rawMediaID);

                    if (lstLogoHits.Count > 0)
                    {
                        int prevOffset = -2;
                        long currLogoID = 0;
                        int startOffset = 0;
                        int endOffset = 0;
                        bool isFirstHit = true;
                        string offsetText;
                        foreach (ImagiQLogoModel hit in lstLogoHits)
                        {
                            // Combine hits that are less than 10 seconds apart
                            if (hit.ID != currLogoID || hit.Offset - prevOffset > 10)
                            {
                                currLogoID = hit.ID;

                                if (!isFirstHit)
                                {
                                    offsetText = formatOffset(startOffset) + (startOffset != endOffset ?  " - " + formatOffset(endOffset) : "");
                                    strngCaption.Append("<span style=\"color:#ffffff;\">" + offsetText + "</span></div>");
                                }
                                else
                                {
                                    isFirstHit = false;
                                }

                                strngCaption.Append("<div class=\"caption\" onclick=\"setSeekPoint(" + (hit.Offset - 2) + ");\">"
                                                        + "<img style=\"margin-right:10px;\" src=\"" + hit.ThumbnailPath + "\" title=\"" + hit.CompanyName + "\"/>");

                                startOffset = hit.Offset;
                                endOffset = hit.Offset;
                            }
                            else
                            {
                                endOffset = hit.Offset;
                            }

                            prevOffset = hit.Offset;
                        }

                        offsetText = formatOffset(startOffset) + (startOffset != endOffset ? " - " + formatOffset(endOffset) : "");
                        strngCaption.Append("<span style=\"color:#ffffff;\">" + offsetText + "</span></div>");
                    }
                }

                //else
                //{
                //    strngCaption.Append("No Results Found");
                //}
                //}
                //else
                //{

                //    strngCaption.Append("No Results Found");
                //}

                if (!string.IsNullOrEmpty(_SearchResult.ResponseXml) && _SearchResult.Hits.Count > 0 && _SearchResult.Hits[0].TermOccurrences.Count > 0)
                {
                    lstSearchTermHits = _SearchResult.Hits[0].TermOccurrences.Select(x => x.TimeOffset).ToList();
                }
                else lstSearchTermHits = new List<int>();

                return Convert.ToString(strngCaption);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetRawMediaCaption(string searchTerm, string iqcckey, out Int32? offset, out string fullCaption, string pmgurl, out List<int> lstSearchTermHits,out Guid rawMediaID, string p_Title120 = null, bool isOptiQ = false)
        {
            try
            {
                rawMediaID = Guid.Empty;
                offset = null;
                fullCaption = string.Empty;
                StringBuilder strngCaption = new StringBuilder();
                StringBuilder strngFullCaption = new StringBuilder();
                //if (!string.IsNullOrWhiteSpace(searchTerm))
                //{
                Uri PMGSearchRequestUrl = new Uri(pmgurl);
                SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchRequest _SearchRequest = new SearchRequest();
                _SearchRequest.IQCCKeyList = iqcckey;
                _SearchRequest.Terms = searchTerm.Trim();
                _SearchRequest.IsShowCC = true;
                if (!string.IsNullOrEmpty(p_Title120))
                {
                    _SearchRequest.Title120 = p_Title120.Trim();
                }


                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SolrQTIframe"]))
                {
                    _SearchRequest.SolrQT = ConfigurationManager.AppSettings["SolrQTIframe"];
                }

                int _PMGMaxHighlights = 20;

                if (ConfigurationManager.AppSettings["PMGMaxHighlights"] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings["PMGMaxHighlights"], out _PMGMaxHighlights);
                }

                _SearchRequest.MaxHighlights = _PMGMaxHighlights;

                SearchResult _SearchResult = _SearchEngine.Search(_SearchRequest);

                if (!string.IsNullOrEmpty(_SearchResult.ResponseXml) && _SearchResult.Hits.Count > 0)
                {
                    rawMediaID = Guid.Parse(_SearchResult.Hits[0].Guid);

                    if (_SearchResult.Hits[0].ClosedCaption.Count > 0)
                    {
                        foreach (TermOccurrence _TermOccurrence in _SearchResult.Hits[0].ClosedCaption)
                        {
                            strngFullCaption.Append("<div onclick=\"setSeekPoint(" + (_TermOccurrence.TimeOffset - Convert.ToInt32(ConfigurationManager.AppSettings["RawMediaCaptionDelay"].ToString()) > 0 ? _TermOccurrence.TimeOffset - Convert.ToInt32(ConfigurationManager.AppSettings["RawMediaCaptionDelay"].ToString()) : 0) + ");\">"
                                                    + _TermOccurrence.SurroundingText + "</div>");
                        }

                        fullCaption = strngFullCaption.ToString();

                    }

                    if (_SearchResult.Hits[0].TermOccurrences.Count > 0)
                    {
                        _SearchResult.Hits[0].TermOccurrences = _SearchResult.Hits[0].TermOccurrences.OrderBy(o => o.TimeOffset).ToList();
                        foreach (TermOccurrence _TermOccurrence in _SearchResult.Hits[0].TermOccurrences)
                        {
                            if (offset == null)
                            {
                                offset = _TermOccurrence.TimeOffset;
                            }
                            strngCaption.Append("<div class=\"hit\" onclick=\"setSeekPoint(" + (_TermOccurrence.TimeOffset - Convert.ToInt32(ConfigurationManager.AppSettings["RawMediaCaptionDelay"].ToString())) + ");\">"
                                                    + "<div class=\"boldgray\">" + formatOffset(_TermOccurrence.TimeOffset) + "</div>"
                                                    + "<div class=\"caption\">" + _TermOccurrence.SurroundingText + "</div>"
                                                + "</div>");
                        }
                    }
                    else if (!string.IsNullOrEmpty(p_Title120) && _SearchResult.Hits[0].StartMinute != null)
                    {
                        offset = _SearchResult.Hits[0].StartMinute * 60;
                    }
                    //else
                    //{
                    //    strngCaption.Append("No Results Found");
                    //}
                }

                if (isOptiQ && !rawMediaID.Equals(Guid.Empty))
                {
                    ImagiQLogic ImagiQLogic = (ImagiQLogic)LogicFactory.GetLogic(LogicType.ImagiQ);
                    List<ImagiQLogoModel> lstLogoHits = ImagiQLogic.GetLRResultsByGuid(rawMediaID);

                    if (lstLogoHits.Count > 0)
                    {
                        int prevOffset = -2;
                        long currLogoID = 0;
                        int startOffset = 0;
                        int endOffset = 0;
                        bool isFirstHit = true;
                        string offsetText;
                        foreach (ImagiQLogoModel hit in lstLogoHits)
                        {
                            // Combine hits that are less than 10 seconds apart
                            if (hit.ID != currLogoID || hit.Offset - prevOffset > 10)
                            {
                                currLogoID = hit.ID;

                                if (!isFirstHit)
                                {
                                    offsetText = formatOffset(startOffset) + (startOffset != endOffset ? " - " + formatOffset(endOffset) : "");
                                    strngCaption.Append("<span style=\"color:#ffffff;\">" + offsetText + "</span></div>");
                                }
                                else
                                {
                                    isFirstHit = false;
                                }

                                strngCaption.Append("<div class=\"caption\" onclick=\"setSeekPoint(" + (hit.Offset - 2) + ");\">"
                                                        + "<img style=\"margin-right:10px;\" src=\"" + hit.ThumbnailPath + "\" title=\"" + hit.CompanyName + "\"/>");

                                startOffset = hit.Offset;
                                endOffset = hit.Offset;
                            }
                            else
                            {
                                endOffset = hit.Offset;
                            }

                            prevOffset = hit.Offset;
                        }

                        offsetText = formatOffset(startOffset) + (startOffset != endOffset ? " - " + formatOffset(endOffset) : "");
                        strngCaption.Append("<span style=\"color:#ffffff;\">" + offsetText + "</span></div>");
                    }
                }

                //else
                //{
                //    strngCaption.Append("No Results Found");
                //}
                //}
                //else
                //{

                //    strngCaption.Append("No Results Found");
                //}

                if (!string.IsNullOrEmpty(_SearchResult.ResponseXml) && _SearchResult.Hits.Count > 0 && _SearchResult.Hits[0].TermOccurrences.Count > 0)
                {
                    lstSearchTermHits = _SearchResult.Hits[0].TermOccurrences.Select(x => x.TimeOffset).ToList();
                }
                else lstSearchTermHits = new List<int>();

                return Convert.ToString(strngCaption);
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal static string formatOffset(int offs)
        {
            int h = 0;
            int m = 0;
            offs -= ((h = offs / 3600) * 3600);
            offs -= ((m = offs / 60) * 60);
            string str = ("" + h).PadLeft(2, '0') + ":" + ("" + m).PadLeft(2, '0') + ":" + ("" + offs).PadLeft(2, '0');
            return str;
        }

        public static string RenderClipPlayer(string ClipID, string ServiceBaseURL, bool IsPlayFromLocal, string ClientGUID, string _BrowserVersion, bool IsMicrosite = false, bool EB = true, Int16 HideCC=0, bool p_AutoResize=false, bool p_AutoSize=false, bool p_AutoPlayback=true)
        {
            try
            {
                string _ParamValue = "IsRawMedia=false"
                                    + "&IsUGC=false"                                    
                                    + "&embedId=" + ClipID
                                    + "&ServicesBaseURL=" + ServiceBaseURL
                                    + "&PlayerFromLocal=" + IsPlayFromLocal
                                    + "&autoPlayback=" + p_AutoPlayback
                                    + "&clientGUID=" + ClientGUID
                                    + "&sd=" + ConfigurationManager.AppSettings["SubDomainSWF"]
                                    + "&isec=" + ConfigurationManager.AppSettings["IsSecuredServiceSWF"]
                                    +"&hcc="+Convert.ToString(HideCC);

                if (!EB)
                {
                    _ParamValue += "&EB=false";
                }

                _ParamValue = _ParamValue + "&arsz="+((p_AutoResize==true)?1:0);

                StringBuilder _StringBuilder = new StringBuilder();

                _StringBuilder.Append("<object id=\"HUY\" name=\"HYETA\" type=\"application/x-shockwave-flash\"");

                if (_BrowserVersion.Contains("IE"))
                {
                    _StringBuilder.Append(" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"");
                }

                if (!p_AutoSize)
                {
                    _StringBuilder.AppendFormat(" width=\"{0}\"", ConfigurationManager.AppSettings["ClipPlayerObjectWidth"].ToString());
                    _StringBuilder.AppendFormat(" height=\"{0}\"", ConfigurationManager.AppSettings["ClipPlayerObjectHeight"].ToString()); 
                }
                else
                {
                    _StringBuilder.Append(" style=\"width:100%; height:100%;\"");
                }

                if (!IsMicrosite)
                {
                    if (IsPlayFromLocal)
                    {
                        _StringBuilder.AppendFormat(" data=\"{0}\"", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                    }
                    else
                    {
                        _StringBuilder.AppendFormat(" data=\"{0}\"", ConfigurationManager.AppSettings["BasicPlayerLocation"].ToString());
                    }
                }
                else
                {
                    _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["ResizePlayerLocation"]);
                }

                _StringBuilder.Append(">");

                if (!IsMicrosite)
                {
                    if (IsPlayFromLocal)
                    {
                        _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                    }
                    else
                    {
                        _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["BasicPlayerLocation"].ToString());
                    }
                }
                else
                {
                    _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["ResizePlayerLocation"]);
                }

                _StringBuilder.Append("<param name=\"allowfullscreen\" value=\"true\"></param>");
                _StringBuilder.Append("<param name=\"allowscriptaccess\" value=\"always\"></param>");
                _StringBuilder.Append("<param name=\"quality\" value=\"high\"></param>");
                _StringBuilder.Append("<param name=\"wmode\" value=\"transparent\"></param>");
                _StringBuilder.AppendFormat("<param name=\"flashvars\" value=\"{0}\"></param>", _ParamValue);

                _StringBuilder.Append("<embed ");
                if (!IsMicrosite)
                {
                    if (IsPlayFromLocal)
                    {
                        _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                    }
                    else
                    {
                        _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["BasicPlayerLocation"].ToString());
                    }
                }
                else
                {
                    _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["ResizePlayerLocation"]);
                }

                _StringBuilder.AppendFormat("width=\"{0}\" ", ConfigurationManager.AppSettings["ClipPlayerObjectWidth"].ToString());
                _StringBuilder.AppendFormat("height=\"{0}\" ", ConfigurationManager.AppSettings["ClipPlayerObjectHeight"].ToString());
                _StringBuilder.Append("type=\"application/x-shockwave-flash\" ");
                _StringBuilder.Append("allowscriptaccess=\"true\" ");
                _StringBuilder.Append("allowfullscreen=\"always\" ");
                _StringBuilder.Append("name=\"IQMedia\" ");
                _StringBuilder.AppendFormat("flashvars=\"{0}\" ", _ParamValue);

                _StringBuilder.Append(" /></object>");

                return _StringBuilder.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string RenderRawMediaPlayer(string embedID, bool IsAutoPlayback, int OffSet, string _BrowserVersion)
        {
            try
            {
                string _ParamValue = "embedId=" + HttpUtility.UrlEncode(embedID)
                                    + "&ServicesBaseURL=" + ConfigurationManager.AppSettings["ServicesBaseURL"]
                                    + "&autoPlayback=" + IsAutoPlayback
                                    + "&Offset=" + Convert.ToString(OffSet)
                                    + "&sd=" + ConfigurationManager.AppSettings["SubDomainSWF"]
                                    + "&isec=" + ConfigurationManager.AppSettings["IsSecuredServiceSWF"];


                StringBuilder _StringBuilder = new StringBuilder();

                _StringBuilder.Append("<object id=\"HUY\" name=\"HYETA\" type=\"application/x-shockwave-flash\"");
                if (_BrowserVersion.Contains("IE"))
                {
                    _StringBuilder.Append(" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"");
                }
                _StringBuilder.AppendFormat(" width=\"{0}\"", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat(" height=\"{0}\"", ConfigurationManager.AppSettings["RawMediaObjectHeight"].ToString());

                _StringBuilder.AppendFormat(" data=\"{0}\"", ConfigurationManager.AppSettings["IQAgentPlayerLocation"].ToString());


                _StringBuilder.Append(">");

                _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["IQAgentPlayerLocation"].ToString());


                _StringBuilder.Append("<param name=\"allowfullscreen\" value=\"true\"></param>");
                _StringBuilder.Append("<param name=\"allowscriptaccess\" value=\"always\"></param>");
                _StringBuilder.Append("<param name=\"quality\" value=\"high\"></param>");
                _StringBuilder.Append("<param name=\"wmode\" value=\"transparent\"></param>");
                _StringBuilder.AppendFormat("<param name=\"flashvars\" value=\"{0}\"></param>", _ParamValue);

                _StringBuilder.Append("<embed ");

                _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["IQAgentPlayerLocation"].ToString());


                _StringBuilder.AppendFormat("width=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat("height=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectHeight"].ToString());
                _StringBuilder.Append("type=\"application/x-shockwave-flash\" ");
                _StringBuilder.Append("allowscriptaccess=\"true\" ");
                _StringBuilder.Append("allowfullscreen=\"always\" ");
                _StringBuilder.Append("name=\"IQMedia\" ");
                _StringBuilder.AppendFormat("flashvars=\"{0}\" ", _ParamValue);

                _StringBuilder.Append(" /></object>");

                return _StringBuilder.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string RenderBasicRawMediaPlayer(string embedID, bool IsAutoPlayback, int OffSet, string _BrowserVersion)
        {
            try
            {
                string _ParamValue = "embedId=" + HttpUtility.UrlEncode(embedID) + "&ServicesBaseURL=" + ConfigurationManager.AppSettings["ServicesBaseURL"];
                _ParamValue += "&autoPlayback=" + IsAutoPlayback + "&Offset=" + Convert.ToString(OffSet);


                StringBuilder _StringBuilder = new StringBuilder();

                _StringBuilder.Append("<object id=\"HUY\" name=\"HYETA\" type=\"application/x-shockwave-flash\"");
                if (_BrowserVersion.Contains("IE"))
                {
                    _StringBuilder.Append(" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"");
                }
                _StringBuilder.AppendFormat(" width=\"{0}\"", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat(" height=\"{0}\"", ConfigurationManager.AppSettings["RawMediaObjectHeight"].ToString());

                _StringBuilder.AppendFormat(" data=\"{0}\"", ConfigurationManager.AppSettings["IQAgentBasicPlayerLocation"].ToString());


                _StringBuilder.Append(">");

                _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["IQAgentBasicPlayerLocation"].ToString());


                _StringBuilder.Append("<param name=\"allowfullscreen\" value=\"true\"></param>");
                _StringBuilder.Append("<param name=\"allowscriptaccess\" value=\"always\"></param>");
                _StringBuilder.Append("<param name=\"quality\" value=\"high\"></param>");
                _StringBuilder.Append("<param name=\"wmode\" value=\"transparent\"></param>");
                _StringBuilder.AppendFormat("<param name=\"flashvars\" value=\"{0}\"></param>", _ParamValue);

                _StringBuilder.Append("<embed ");

                _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["IQAgentBasicPlayerLocation"].ToString());


                _StringBuilder.AppendFormat("width=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat("height=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectHeight"].ToString());
                _StringBuilder.Append("type=\"application/x-shockwave-flash\" ");
                _StringBuilder.Append("allowscriptaccess=\"true\" ");
                _StringBuilder.Append("allowfullscreen=\"always\" ");
                _StringBuilder.Append("name=\"IQMedia\" ");
                _StringBuilder.AppendFormat("flashvars=\"{0}\" ", _ParamValue);

                _StringBuilder.Append(" /></object>");

                return _StringBuilder.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        public static string GetFeedsReportLimit(Guid clientGuid)
        {
            try
            {
                string _ReturnValue = string.Empty;
                UtilityDA utilityDA = (UtilityDA)DataAccessFactory.GetDataAccess(DataAccessType.Utility);
                return utilityDA.GetFeedsReportLimit(clientGuid);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static bool GetForceCategorySelection(Guid clientGuid)
        {
            try
            {
                string _ReturnValue = string.Empty;
                UtilityDA utilityDA = (UtilityDA)DataAccessFactory.GetDataAccess(DataAccessType.Utility);
                return utilityDA.GetForceCategorySelection(clientGuid);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static IQClient_CustomSettingsModel GetDiscoveryReportAndExportLimit(Guid clientGuid)
        {
            try
            {
                string _ReturnValue = string.Empty;
                UtilityDA utilityDA = (UtilityDA)DataAccessFactory.GetDataAccess(DataAccessType.Utility);
                return utilityDA.GetDiscoveryReportAndExportLimit(clientGuid);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void InsertPMGSearchLog(int p_CustomerID, string p_Terms, string p_Title120, string p_Dma, string p_Station, string p_Class, int p_PageNumber, int p_PageSize, int p_MaxHighlights, DateTime? p_StartDate, DateTime? p_EndDate, string p_SearchType, string p_Response, string IQ_CC_KEys = "")
        {

            string _FileContent = string.Empty;
            _FileContent = "<PMGRequest>";
            _FileContent += "<Terms>" + HttpUtility.UrlEncode(p_Terms) + "</Terms>";
            _FileContent += "<PageNumber>" + p_PageNumber + "</PageNumber>";
            _FileContent += "<PageSize>" + p_PageSize + "</PageSize>";
            _FileContent += "<MaxHighlights>" + p_MaxHighlights + "</MaxHighlights>";

            if ((p_SearchType == IQMedia.Shared.Utility.CommonFunctions.SearchType.Discovery_TV.ToString()) || (p_SearchType == IQMedia.Shared.Utility.CommonFunctions.SearchType.TimeShift.ToString()))
            {
                _FileContent += "<IQ_DMA_Name>" + HttpUtility.UrlEncode(p_Dma) + "</IQ_DMA_Name>";
                _FileContent += "<Title120>" + HttpUtility.UrlEncode(p_Title120) + "</Title120>";
                _FileContent += "<Station_Affil>" + HttpUtility.UrlEncode(p_Station) + "</Station_Affil>";
                _FileContent += "<IQ_Class_Num>" + HttpUtility.UrlEncode(p_Class) + "</IQ_Class_Num>";
            }

            _FileContent += "<IQ_CC_KEy>" + IQ_CC_KEys + "</IQ_CC_KEy>";

            if (p_StartDate.HasValue)
            {
                _FileContent += "<StartDate>" + p_StartDate + "</StartDate>";
            }
            else
            {
                _FileContent += "<StartDate></StartDate>";
            }
            if (p_EndDate.HasValue)
            {
                _FileContent += "<EndDate>" + p_EndDate + "</EndDate>";
            }
            else
            {
                _FileContent += "<EndDate></EndDate>";
            }
            _FileContent += "</PMGRequest>";



            string _Result = string.Empty;
            SearchLogModel _SearchLog = new SearchLogModel();
            _SearchLog.CustomerID = p_CustomerID;
            _SearchLog.SearchType = p_SearchType;
            _SearchLog.RequestXML = _FileContent;
            _SearchLog.ErrorResponseXML = p_Response;

            UtilityDA utilityDA = (UtilityDA)DataAccessFactory.GetDataAccess(DataAccessType.Utility);
            _Result = utilityDA.InsertPMGSearchLog(_SearchLog);
        }

        public static void InsertFeedsSearchLog(int customerID, string mediaIDs, string mediaCategory, string excludeMediaCategories, DateTime? startDate, DateTime? endDate, string searchRequestIDs, string keyword, short? sentiment, string dma, string station, string competeUrl, List<string> iQDmaIDs, string handle,
                                                    string publication, string author, short? prominenceValue, bool isProminenceAudience, bool isAsc, bool? isAudienceSort, int pageSize, long? fromRecordID, long? sinceID, bool isOnlyParents, bool? isRead, string searchType, string response)
        {

            string _FileContent = string.Empty;
            _FileContent = "<FeedsRequest>";
            _FileContent += "<MediaIDs>" + HttpUtility.UrlEncode(mediaIDs) + "</MediaIDs>";
            _FileContent += "<MediaCategory>" + mediaCategory + "</MediaCategory>";
            _FileContent += "<ExcludeMediaCategories>" + HttpUtility.UrlEncode(excludeMediaCategories) + "</ExcludeMediaCategories>";
            _FileContent += "<SearchRequestIDs>" + HttpUtility.UrlEncode(searchRequestIDs) + "</SearchRequestIDs>";
            _FileContent += "<DMA>" + dma + "</DMA>";
            _FileContent += "<Station>" + station + "</Station>";
            _FileContent += "<CompeteUrl>" + competeUrl + "</CompeteUrl>";
            _FileContent += "<TwitterHandle>" + handle + "</TwitterHandle>";
            _FileContent += "<Publication>" + publication + "</Publication>";
            _FileContent += "<Author>" + author + "</Author>";
            
            if (sentiment.HasValue)
            {
                _FileContent += "<Sentiment>" + sentiment.Value + "</Sentiment>";
            }
            else
            {
                _FileContent += "<Sentiment></Sentiment>";
            }

            if (iQDmaIDs != null)
            {
                foreach(string iqid in iQDmaIDs){
                _FileContent += "<IQDMAID>" + iqid + "</IQDMAID>";
                    }
            }
            else
            {
                _FileContent += "<IQDMAID></IQDMAID>";
            }

            if (startDate.HasValue)
            {
                _FileContent += "<StartDate>" + startDate + "</StartDate>";
            }
            else
            {
                _FileContent += "<StartDate></StartDate>";
            }
            if (endDate.HasValue)
            {
                _FileContent += "<EndDate>" + endDate + "</EndDate>";
            }
            else
            {
                _FileContent += "<EndDate></EndDate>";
            }

            if (prominenceValue.HasValue)
            {
                _FileContent += "<ProminenceValue>" + prominenceValue.Value + "</ProminenceValue>";
            }
            else
            {
                _FileContent += "<ProminenceValue></ProminenceValue>";
            }

            _FileContent += "<IsProminenceAudience>" + isProminenceAudience + "</IsProminenceAudience>";
            _FileContent += "<IsAsc>" + isAsc + "</IsAsc>";

            if (isAudienceSort.HasValue)
            {
                _FileContent += "<IsAudienceSort>" + isAudienceSort.Value + "</IsAudienceSort>";
            }
            else
            {
                _FileContent += "<IsAudienceSort></IsAudienceSort>";
            }

            _FileContent += "<IsOnlyParents>" + isOnlyParents + "</IsOnlyParents>";
            _FileContent += "<PageSize>" + pageSize + "</PageSize>";

            if (fromRecordID.HasValue)
            {
                _FileContent += "<FromRecordID>" + fromRecordID.Value + "</FromRecordID>";
            }
            else
            {
                _FileContent += "<FromRecordID></FromRecordID>";
            }

            if (sinceID.HasValue)
            {
                _FileContent += "<SinceID>" + sinceID.Value + "</SinceID>";
            }
            else
            {
                _FileContent += "<SinceID></SinceID>";
            }

            if (isRead.HasValue)
            {
                _FileContent += "<IsRead>" + isRead.Value + "</IsRead>";
            }
            else
            {
                _FileContent += "<IsRead></IsRead>";
            }

            _FileContent += "</FeedsRequest>";


            string _Result = string.Empty;
            SearchLogModel _SearchLog = new SearchLogModel();
            _SearchLog.CustomerID = customerID;
            _SearchLog.SearchType = searchType;
            _SearchLog.RequestXML = _FileContent;
            _SearchLog.ErrorResponseXML = response;

            UtilityDA utilityDA = (UtilityDA)DataAccessFactory.GetDataAccess(DataAccessType.Utility);
            _Result = utilityDA.InsertFeedsSearchLog(_SearchLog);
        }

        public static string RenderBasicRawMediaPlayer(string p_UserID, string p_RawMediaID, string p_IsRawMedia, string p_IsUGCRawMedia, string p_ClientGUID, string IsAutoDownload, string p_CustomerGUID, string p_ServicesBaseURL, int? p_Offset, bool? IsActivePlayerLogo, string PlayerLogoImage, string _BrowserVersion, string p_KeyValues = null, bool p_AutoPlayback = true,char p_PreviewImageOption='N',string p_PreviewImageURL="", bool p_AutoResize=false)
        {
            try
            {
                string _ParamValue = "userId=" + p_UserID + "&RL_User_GUID=" + ConfigurationManager.AppSettings["RL_User_GUID"] + "&clientGUID=" + p_ClientGUID + "&IsAutoDownload=" + IsAutoDownload + "&customerGUID=" + p_CustomerGUID + "&categoryCode=" + ConfigurationManager.AppSettings["CategoryCode"] + "&IsRawMedia=" + p_IsRawMedia + "&IsUGC=" + p_IsUGCRawMedia + "&embedId=" + p_RawMediaID + "&ServicesBaseURL=" + ConfigurationManager.AppSettings["ServicesBaseURL"] + "&autoPlayback=" + p_AutoPlayback + "&pio=" + p_PreviewImageOption + "&piu=" + p_PreviewImageURL + "&Offset=" + Convert.ToString(p_Offset) + "&ClipLength=" + ConfigurationManager.AppSettings["MaxClipLength"] + "&DefaultClipLength=" + ConfigurationManager.AppSettings["DefaultClipLength"];

                if (IsActivePlayerLogo != null && IsActivePlayerLogo == true && !string.IsNullOrEmpty(PlayerLogoImage))
                {
                    _ParamValue = _ParamValue + "&PlayerLogo=" + ConfigurationManager.AppSettings["URLWaterMark"] + PlayerLogoImage + "";
                }

                if (!string.IsNullOrWhiteSpace(p_KeyValues))
                {
                    _ParamValue = _ParamValue + "&KeyValues=" + HttpContext.Current.Server.HtmlEncode(p_KeyValues);
                }

                _ParamValue = _ParamValue + "&sd=" + ConfigurationManager.AppSettings["SubDomainSWF"];
                _ParamValue = _ParamValue + "&isec=" + ConfigurationManager.AppSettings["IsSecuredServiceSWF"];
                _ParamValue = _ParamValue + "&arsz="+(p_AutoResize==true?1:0);

                StringBuilder _StringBuilder = new StringBuilder();

                _StringBuilder.Append("<object id=\"HUY\" name=\"HYETA\" type=\"application/x-shockwave-flash\"");

                if (_BrowserVersion.Contains("IE"))
                {
                    _StringBuilder.Append(" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"");
                }


                _StringBuilder.AppendFormat(" width=\"{0}\"", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat(" height=\"312\" ");

                /*if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat(" data=\"{0}\"", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat(" data=\"{0}\"", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                }*/

                _StringBuilder.AppendFormat(" data=\"{0}\"", ConfigurationManager.AppSettings["BasicPlayerLocation"].ToString());

                _StringBuilder.Append(">");

                /*if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                }*/

                _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["BasicPlayerLocation"].ToString());

                _StringBuilder.Append("<param name=\"allowfullscreen\" value=\"true\"></param>");
                _StringBuilder.Append("<param name=\"allowscriptaccess\" value=\"always\"></param>");
                _StringBuilder.Append("<param name=\"quality\" value=\"high\"></param>");
                _StringBuilder.Append("<param name=\"wmode\" value=\"transparent\"></param>");
                _StringBuilder.AppendFormat("<param name=\"flashvars\" value=\"{0}\"></param>", _ParamValue);

                _StringBuilder.Append("<embed ");

                /*if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                }*/

                _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["BasicPlayerLocation"].ToString());

                _StringBuilder.AppendFormat("width=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat("height=\"312\" ");
                _StringBuilder.Append("type=\"application/x-shockwave-flash\" ");
                _StringBuilder.Append("allowscriptaccess=\"true\" ");
                _StringBuilder.Append("allowfullscreen=\"always\" ");
                _StringBuilder.Append("name=\"IQMedia\" ");
                _StringBuilder.AppendFormat("flashvars=\"{0}\" ", _ParamValue);

                _StringBuilder.Append(" /></object>");

                return _StringBuilder.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string RenderClipPlayerWithFullHeightWidth(string ToEmail, string ClipID, string ServiceBaseURL, bool IsPlayFromLocal, string PlayerLogo, string ClientGUID, string _BrowserVersion, bool IsMicrosite = false, bool EB = true)
        {
            try
            {
                string _ParamValue = "IsRawMedia=false";
                _ParamValue += "&IsUGC=false";
                _ParamValue += "&ToEmail=" + ToEmail;
                _ParamValue += "&embedId=" + ClipID;
                _ParamValue += "&ServicesBaseURL=" + ServiceBaseURL;
                _ParamValue += "&PlayerFromLocal=" + IsPlayFromLocal;
                _ParamValue += "&autoPlayback=true";
                _ParamValue += "&PlayerLogo=" + PlayerLogo;
                _ParamValue += "&clientGUID=" + ClientGUID;

                if (!EB)
                {
                    _ParamValue += "&EB=false";
                }

                StringBuilder _StringBuilder = new StringBuilder();

                _StringBuilder.Append("<object id=\"HUY\" name=\"HYETA\" type=\"application/x-shockwave-flash\"");

                if (_BrowserVersion.Contains("IE"))
                {
                    _StringBuilder.Append(" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"");
                }
                _StringBuilder.Append(" width=\"100%\"");
                _StringBuilder.Append(" height=\"100%\"");
                _StringBuilder.Append(" style=\"width:100%;height:100%\"");

                if (!IsMicrosite)
                {
                    if (IsPlayFromLocal)
                    {
                        _StringBuilder.AppendFormat(" data=\"{0}\"", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                    }
                    else
                    {
                        _StringBuilder.AppendFormat(" data=\"{0}\"", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                    }
                }
                else
                {
                    _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["ResizePlayerLocation"]);
                }

                _StringBuilder.Append(">");

                if (!IsMicrosite)
                {
                    if (IsPlayFromLocal)
                    {
                        _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                    }
                    else
                    {
                        _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                    }
                }
                else
                {
                    _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["ResizePlayerLocation"]);
                }

                _StringBuilder.Append("<param name=\"allowfullscreen\" value=\"true\"></param>");
                _StringBuilder.Append("<param name=\"allowscriptaccess\" value=\"always\"></param>");
                _StringBuilder.Append("<param name=\"quality\" value=\"high\"></param>");
                _StringBuilder.Append("<param name=\"wmode\" value=\"transparent\"></param>");
                _StringBuilder.AppendFormat("<param name=\"flashvars\" value=\"{0}\"></param>", _ParamValue);

                _StringBuilder.Append("<embed ");
                if (!IsMicrosite)
                {
                    if (IsPlayFromLocal)
                    {
                        _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                    }
                    else
                    {
                        _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                    }
                }
                else
                {
                    _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["ResizePlayerLocation"]);
                }

                _StringBuilder.Append(" width=\"100%\"");
                _StringBuilder.Append(" height=\"100%\"");
                _StringBuilder.Append("type=\"application/x-shockwave-flash\" ");
                _StringBuilder.Append("allowscriptaccess=\"true\" ");
                _StringBuilder.Append("allowfullscreen=\"always\" ");
                _StringBuilder.Append("name=\"IQMedia\" ");
                _StringBuilder.AppendFormat("flashvars=\"{0}\" ", _ParamValue);

                _StringBuilder.Append(" /></object>");

                return _StringBuilder.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
