using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using HtmlAgilityPack;
using IQMedia.Model;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.WebApplication.Config;
using IQMedia.WebApplication.Models;
using IQMedia.WebApplication.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using HiQPdf;
using IQCommon.Model;



namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class LibraryController : Controller
    {
        #region Public Property

        ActiveUser sessionInformation = null;
        string PATH_LibraryPartialView = "~/Views/Library/_Library.cshtml";
        string PATH_LibraryEmailPartialView = "~/Views/Library/_Email.cshtml";
        string PATH_LibraryEditRecordPartialView = "~/Views/Library/_IQArchiveEdit.cshtml";
        string PATH_LibrarySavedReportsPartialView = "~/Views/Library/_SavedReports.cshtml";
        string PATH_LibraryGenerateReportPartialView = "~/Views/Library/_DisplayReport.cshtml";
        string PATH_LibraryReportPDFPartialView = "~/Views/Library/_Report-PDF.cshtml";
        string PATH_LibraryReportEmailPartialView = "~/Views/Library/_Report-Email.cshtml";
        string PATH_LibraryUGCFTPFileExplorerPartialView = "~/Views/Library/_UGCFTPFileExplorer.cshtml";
        string PATH_LibraryTopReferrerSites = "~/Views/Library/_TopReferrerSites.cshtml";
        string PATH_IQUGCArchiveResultsPartialView = "~/Views/Library/_IQUGCArchiveResults.cshtml";
        string PATH_IQUGCArchiveEditPartialView = "~/Views/Library/_IQUGCArchiveEdit.cshtml";
        string PATH_DashboardBroadCastPartialView = "~/Views/Dashboard/_Media.cshtml";
        string PATH_DashboardOverviewPartialView = "~/Views/Dashboard/_Overview.cshtml";
        string PATH_DashboardTopBroadcastDMA = "~/Views/Dashboard/_TopBroadcastDMA.cshtml";
        string PATH_DashboardTopBroadcastStation = "~/Views/Dashboard/_TopBroadcastStation.cshtml";
        string PATH_DashboardTopOnlineNewsDMA = "~/Views/Dashboard/_TopOnlineNewsDMA.cshtml";
        string PATH_DashboardTopOnlineNewsSites = "~/Views/Dashboard/_TopOnlineNewsSites.cshtml";
        string PATH_MCMediaResultsPartialView = "~/Views/Library/_MCMediaResults.cshtml";

        public string ClientGUID
        {
            get
            {
                ActiveUser sessionInformation = ActiveUserMgr.GetActiveUser();
                return sessionInformation.ClientGUID.ToString();
            }
        }

        public string CustomerGUID
        {
            get
            {
                ActiveUser sessionInformation = ActiveUserMgr.GetActiveUser();
                return sessionInformation.CustomerGUID.ToString();
            }
        }

        #endregion

        #region Library

        [HttpGet]
        public ActionResult Index()
        {
            Dictionary<string, object> resultdict = null;
            try
            {
                // Clear out temp data used by other pages
                if (TempData.ContainsKey("AnalyticsTempData")) { TempData["AnalyticsTempData"] = null; }
                if (TempData.ContainsKey("FeedsTempData")) { TempData["FeedsTempData"] = null; }
                if (TempData.ContainsKey("DiscoveryTempData")) { TempData["DiscoveryTempData"] = null; }
                if (TempData.ContainsKey("TimeShiftTempData")) { TempData["TimeShiftTempData"] = null; }
                if (TempData.ContainsKey("TAdsTempData")) { TempData["TAdsTempData"] = null; }
                if (TempData.ContainsKey("SetupTempData")) { TempData["SetupTempData"] = null; }
                if (TempData.ContainsKey("GlobalAdminTempData")) { TempData["GlobalAdminTempData"] = null; } 

                ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);

                TempData["TotalResults"] = null;
                TempData["TotalResultsDisplay"] = null; 
                TempData["SinceID"] = null;
                TempData["FromRecordID"] = null;
                TempData["FromRecordIDDisplay"] = null;
                TempData["v4Download"] = GetDownloadRoleByCustomerGuid();
                TempData["EmailTemplates"] = reportLogic.GetReportTypes(Shared.Utility.CommonFunctions.TemplateTypes.EmailTemplate.ToString());

                TempData["UGCDocument_RootPathID"] = null;
                TempData["UGCDocument_StoragePath"] = null;
                TempData["UGCDocument_FileTypes"] = null;

                IQUGCArchiveRefreshResults();
                //GetRefreshTVResults();
                
                // Fetch Client's Custom settings and store it in TempData
                SetIQCustomSettings();

                ViewBag.MaxLibraryReportItems = GetIQCustomSettings().v4MaxLibraryReportItems;
                ViewBag.MaxLibraryEmailItems = GetIQCustomSettings().v4MaxLibraryEmailReportItems;
                ViewBag.v4LibraryRollup = GetIQCustomSettings().v4LibraryRollup;
                ViewBag.DefaultArchivePageSize = GetIQCustomSettings().DefaultArchivePageSize;
                ViewBag.MaxEmailAddresses = System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"];
                resultdict = GetIQArchieveResults(0, null, null, null, null, null, null, null, false, string.Empty, true, null);


                TempData["FromRecordID"] = ((List<IQArchive_MediaModel>)resultdict["Result"]).Count;
                TempData["FromRecordIDDisplay"] = ((List<IQArchive_MediaModel>)resultdict["Result"]).Count
                                    + (((List<IQArchive_MediaModel>)resultdict["Result"]).Sum(a => (a.MediaType == "TV") ? (a.MediaData as IQArchive_ArchiveClipModel).ChildResults.Count() : 0)) 
                                    + (((List<IQArchive_MediaModel>)resultdict["Result"]).Sum(a => (a.MediaType == "NM") ? (a.MediaData as IQArchive_ArchiveNMModel).ChildResults.Count() : 0));


                IQClient_CustomImageLogic iQClient_CustomImageLogic = (IQClient_CustomImageLogic)LogicFactory.GetLogic(LogicType.IQClient_CustomImage);
                List<IQClient_CustomImageModel> lstIQClient_CustomImageModel = iQClient_CustomImageLogic.GetAllIQClient_CustomImageByClientGuid(sessionInformation.ClientGUID);
                resultdict.Add("ReportImages", lstIQClient_CustomImageModel);

                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                List<IQReport_FolderModel> lstIQReport_FolderModel = iQReport_FolderLogic.GetReportFolderByClientGuid(sessionInformation.ClientGUID);
                orderedFolderList = new List<IQReport_FolderModel>();
                string id = null;
                try
                {
                    id = lstIQReport_FolderModel.Where(x => x.parent == null).Select(x => x.id).First();
                    GetChildrenOrderedFolders(lstIQReport_FolderModel, id);
                }
                catch (Exception ex)
                {
                    id = null;
                    orderedFolderList = new List<IQReport_FolderModel>();
                }
                resultdict.Add("ReportFolders", orderedFolderList);
                
                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                Client_DropDown objClientDropDown = clientLogic.GetClientDropDownByClient(sessionInformation.ClientID, null);
                resultdict.Add("EmailTemplates", objClientDropDown.Client_MCMediaEmailTemplateList);

                IQClient_CustomSettingsModel customSettings = GetIQCustomSettings();
                resultdict.Add("DefaultEmailTemplateID", customSettings.MCMediaDefaultEmailTemplateID);
                resultdict.Add("UseCustomerEmailAsDefault", customSettings.UseCustomerEmailDefault.Value);
                resultdict.Add("DefaultEmailSender", customSettings.UseCustomerEmailDefault.Value ? sessionInformation.Email : ConfigurationManager.AppSettings["Sender"]);

                if (sessionInformation.Isv4UGCAccess)
                {
                    var manualClipDuration = clientLogic.GetClientManualClipDurationSettings(sessionInformation.ClientGUID);
                    resultdict.Add("ManualClipDuration", manualClipDuration);
                }

                resultdict.Add("ClipEmbedAutoPlay", customSettings.ClipEmbedAutoPlay.Value);

                TempDataModel _TempData = GetTempDataModel();

                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                IQClient_UGCMapModel objIQClient_UGCMapModel = iQArchieveLogic.GetUGCMapByClientGUID(ClientGUID);


                IQUGCArchiveLogic iQUGCArchiveLogic = (IQUGCArchiveLogic)LogicFactory.GetLogic(LogicType.IQUGCArchive);
                int _rootpathid; string storagepath;
                iQUGCArchiveLogic.GetUGCDocumentStoragePath(out _rootpathid, out storagepath);

                Dictionary<string, string> ugcFileTypes = iQUGCArchiveLogic.GetUGCFileTypes();

                TempData["UGCDocument_RootPathID"] = _rootpathid;
                TempData["UGCDocument_StoragePath"] = storagepath;
                TempData["UGCDocument_FileTypes"] = ugcFileTypes;

                _TempData.UGC_Client_MapModel = objIQClient_UGCMapModel;
                TempData["TempDataModel"] = _TempData;
                ViewBag.IsSuccess = true;
            }
            catch (Exception exception)
            {
            
                ViewBag.IsSuccess = false;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
            }
            finally
            {
                TempData.Keep();
            }
            return View(resultdict);
        }

        List<IQReport_FolderModel> orderedFolderList = new List<IQReport_FolderModel>();
        public void GetChildrenOrderedFolders(List<IQReport_FolderModel> list, string id)
        {
            foreach (var item in list.Where(x => x.id == id))
            {
                orderedFolderList.Add(item);
                foreach (var subItem in list.Where(x => x.parent == item.id))
                {
                    GetChildrenOrderedFolders(list, subItem.id);
                }
            }

        }

        private IQClient_CustomSettingsModel GetIQCustomSettings()
        {
            IQClient_CustomSettingsModel settings = null;
            try
            {
                settings = TempData["IQClientCustom_Settings"] as IQClient_CustomSettingsModel;
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
            }
            finally { TempData.Keep(); }

            return settings;
        }

        private void SetIQCustomSettings()
        {
            try
            {
                if (TempData["IQClientCustom_Settings"] == null)
                {
                    ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                    IQClient_CustomSettingsModel customSettings = clientLogic.GetClientCustomSettings(ClientGUID);
                    TempData["IQClientCustom_Settings"] = customSettings;
                }
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
            }
            finally { TempData.Keep(); }
        }

        private Dictionary<string, object> GetIQArchieveResults(long p_FromRecordID, string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_SubMediaType, List<string> p_CategoryGUID, string p_SelectionType, bool p_IsAsc, string p_SortColumn, bool p_IsEnableFilter, int? p_PageSize)
        {
            sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
            IQClient_CustomSettingsModel customSettings = GetIQCustomSettings();

            IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);

            long totalResults = 0;
            long totalResultsDisplay = 0;
            long sinceID = TempData["SinceID"] != null ? Convert.ToInt64(TempData["SinceID"]) : 0;
            p_SortColumn = !string.IsNullOrEmpty(p_SortColumn) ? p_SortColumn : "CreatedDate";

            int PageSize;
            if (p_PageSize.HasValue)
                PageSize = p_PageSize.Value;
            else
                PageSize = customSettings.DefaultArchivePageSize;

            if (p_FromDate.HasValue && p_ToDate.HasValue)
            {
                Shared.Utility.Log4NetLogger.Debug("Library From Date 1st: " + p_FromDate.Value.ToString());

                p_FromDate = Utility.CommonFunctions.GetGMTandDSTTime(p_FromDate);

                p_ToDate = p_ToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                p_ToDate = Utility.CommonFunctions.GetGMTandDSTTime(p_ToDate);

                Shared.Utility.Log4NetLogger.Debug("Library From Date : " + p_FromDate.Value.ToString());
                Shared.Utility.Log4NetLogger.Debug("Library To Date : " + p_ToDate.Value.ToString());
            }

            Dictionary<string, object> dictResult = null;
            dictResult = iQArchieveLogic.GetIQAgentMediaResults(ClientGUID, p_CustomerGuid,
                                                p_FromRecordID, PageSize, p_FromDate, p_ToDate, p_SubMediaType, p_SearchTerm, p_CategoryGUID, p_SelectionType, p_IsAsc, p_SortColumn, p_IsEnableFilter, Request.ServerVariables["HTTP_HOST"], ref sinceID, out totalResults, out totalResultsDisplay, customSettings.v4LibraryRollup);

            if (dictResult["Result"] != null)
            {
                List<IQArchive_MediaModel> lstArchiveMedia = (List<IQArchive_MediaModel>)dictResult["Result"];
                lstArchiveMedia.ToList().ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.CreatedDate, false));
                lstArchiveMedia = CommonFunctions.GetGMTandDSTTime(lstArchiveMedia, CommonFunctions.ResultType.Library);
                lstArchiveMedia = CommonFunctions.ProcessArchiveDisplayText(lstArchiveMedia, customSettings.LibraryTextType);
                dictResult["Result"] = lstArchiveMedia;
            }

            TempData["TotalResults"] = totalResults;
            TempData["TotalResultsDisplay"] = totalResultsDisplay;
            TempData["SinceID"] = sinceID;
            TempData.Keep();

            return dictResult;
        }

        [HttpPost]
        public ContentResult GetMoreResults(string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_SubMediaType, List<string> p_CategoryGUID, string p_SelectionType, bool p_IsAsc, string p_SortColumn, int? p_PageSize)
        {

            try
            {

                Dictionary<string, object> dictResult = GetIQArchieveResults(Convert.ToInt64(TempData["FromRecordID"]), p_CustomerGuid, p_FromDate, p_ToDate,
                                                                           p_SearchTerm, p_SubMediaType, p_CategoryGUID, p_SelectionType, p_IsAsc, p_SortColumn, false, p_PageSize);

                if (dictResult["Result"] != null)
                {
                    TempData["FromRecordID"] = Convert.ToInt64(TempData["FromRecordID"]) + ((List<IQArchive_MediaModel>)dictResult["Result"]).Count;
                    TempData["FromRecordIDDisplay"] = Convert.ToInt64(TempData["FromRecordIDDisplay"]) + ((List<IQArchive_MediaModel>)dictResult["Result"]).Count 
                                            + (((List<IQArchive_MediaModel>)dictResult["Result"]).Sum(a => (a.MediaType == "TV") ? (a.MediaData as IQArchive_ArchiveClipModel).ChildResults.Count() : 0))
                                            + (((List<IQArchive_MediaModel>)dictResult["Result"]).Sum(a => (a.MediaType == "NM") ? (a.MediaData as IQArchive_ArchiveNMModel).ChildResults.Count() : 0));
                    TempData.Keep();
                }

                LibraryResult json = new LibraryResult()
                {
                    isSuccess = true,
                    hasMoreResults = HasMoreResults(),
                    HTML = dictResult["Result"] != null ? RenderPartialToString(PATH_LibraryPartialView, dictResult["Result"]) : string.Empty,
                    totalRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["TotalResults"])),
                    totalRecordsDisplay = string.Format("{0:n0}", Convert.ToInt64(TempData["TotalResultsDisplay"])),
                    currentRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["FromRecordID"])),
                    currentRecordsDisplay = string.Format("{0:n0}", Convert.ToInt64(TempData["FromRecordIDDisplay"]))
                };
                return Content(IQMedia.Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                LibraryResult json = new LibraryResult
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Content(IQMedia.Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public ContentResult SearchLibraryResults(string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_SubMediaType, List<string> p_CategoryGUID, string p_SelectionType, bool p_IsAsc, string p_SortColumn, int? p_PageSize)
        {
            try
            {
                TempData["FromRecordID"] = 0;
                TempData["FromRecordIDDisplay"] = 0;

                Dictionary<string, object> dictResult = GetIQArchieveResults(Convert.ToInt64(TempData["FromRecordID"]), p_CustomerGuid, p_FromDate, p_ToDate,
                                                                               p_SearchTerm, p_SubMediaType, p_CategoryGUID, p_SelectionType, p_IsAsc, p_SortColumn, true, p_PageSize);

                IQArchive_FilterModel iqArchiveFilter = dictResult["Filter"] as IQArchive_FilterModel;

                if (dictResult["Result"] != null)
                {
                    TempData["FromRecordID"] = ((List<IQArchive_MediaModel>)dictResult["Result"]).Count;
                    TempData["FromRecordIDDisplay"] = ((List<IQArchive_MediaModel>)dictResult["Result"]).Count 
                                    + (((List<IQArchive_MediaModel>)dictResult["Result"]).Sum(a => (a.MediaType == "TV") ? (a.MediaData as IQArchive_ArchiveClipModel).ChildResults.Count() : 0))
                                    + (((List<IQArchive_MediaModel>)dictResult["Result"]).Sum(a => (a.MediaType == "NM") ? (a.MediaData as IQArchive_ArchiveNMModel).ChildResults.Count() : 0));
                    TempData.Keep();
                }

                //string dates = string.Join(",", iqArchiveFilter.Dates.Select(f => Convert.ToDateTime(f).ToString("MM/dd/yyyy")).ToArray());

                LibraryResult json = new LibraryResult
                {
                    isSuccess = true,
                    hasMoreResults = HasMoreResults(),
                    HTML = dictResult["Result"] != null ? RenderPartialToString(PATH_LibraryPartialView, dictResult["Result"]) : string.Empty,
                    filter = iqArchiveFilter,
                    totalRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["TotalResults"])),
                    totalRecordsDisplay = string.Format("{0:n0}", Convert.ToInt64(TempData["TotalResultsDisplay"])),
                    currentRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["FromRecordID"])),
                    currentRecordsDisplay = string.Format("{0:n0}", Convert.ToInt64(TempData["FromRecordIDDisplay"]))
                };

                return Content(IQMedia.Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                LibraryResult json = new LibraryResult
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Content(IQMedia.Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
            }
            finally { TempData.Keep(); }

        }

        [HttpPost]
        public JsonResult FilterCategory(string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_SubMediaType, List<string> p_CategoryGUID)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                if (p_FromDate.HasValue && p_ToDate.HasValue)
                {
                    p_FromDate = Utility.CommonFunctions.GetGMTandDSTTime(p_FromDate);

                    p_ToDate = p_ToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    p_ToDate = Utility.CommonFunctions.GetGMTandDSTTime(p_ToDate);
                }

                long sinceID = TempData["SinceID"] != null ? Convert.ToInt64(TempData["SinceID"]) : 0;

                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                List<IQArchive_Filter> lstCategoryFilter = iQArchieveLogic.GetCategoryFilter(ClientGUID, p_CustomerGuid, p_FromDate, p_ToDate, p_SubMediaType, p_SearchTerm, p_CategoryGUID, sinceID);

                var json = new
                {
                    isSuccess = true,
                    categoryFilter = lstCategoryFilter
                };

                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public ContentResult Delete(string ArchiveIDs, string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_SubMediaType, List<string> p_CategoryGUID, string p_SelectionType,int p_ParentCount)
        {

            IQMedia.Shared.Utility.Log4NetLogger.Info("Library Delete Params are :");
            IQMedia.Shared.Utility.Log4NetLogger.Info("MediaID :" + ArchiveIDs);
            IQMedia.Shared.Utility.Log4NetLogger.Info("p_CustomerGuid :" + p_CustomerGuid);
            IQMedia.Shared.Utility.Log4NetLogger.Info("p_FromDate :" + p_FromDate);
            IQMedia.Shared.Utility.Log4NetLogger.Info("p_ToDate :" + p_ToDate);
            IQMedia.Shared.Utility.Log4NetLogger.Info("p_SearchTerm :" + p_SearchTerm);
            IQMedia.Shared.Utility.Log4NetLogger.Info("p_SubMediaType :" + p_SubMediaType);
            IQMedia.Shared.Utility.Log4NetLogger.Info("p_CategoryGUID :" + (p_CategoryGUID != null ? string.Join(",", p_CategoryGUID) : string.Empty));
            IQMedia.Shared.Utility.Log4NetLogger.Info("p_SelectionType :" + p_SelectionType);
            IQMedia.Shared.Utility.Log4NetLogger.Info("p_ParentCount :" + p_ParentCount);

            try
            {
                string strArchiveID = string.Empty;
                IQArchive_FilterModel iqArchiveFilter = null;

                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                IQClient_CustomSettingsModel customSettings = GetIQCustomSettings();

                List<IQArchive_MediaModel> lstOfIQArchive_MediaModel = new List<IQArchive_MediaModel>();

                if (!string.IsNullOrEmpty(ArchiveIDs))
                {
                    string ArchiveXML = string.Empty;
                    foreach (string id in ArchiveIDs.Split(new char[] { ',' }))
                    {
                        if (!string.IsNullOrWhiteSpace(id.Trim()))
                        {
                            ArchiveXML += "<id> " + id + "</id>";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(ArchiveXML))
                    {
                        ArchiveXML = "<list>" + ArchiveXML + "</list>";

                    }

                    List<long> lstArchiveID = new List<long>();
                    IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                    lstOfIQArchive_MediaModel = iQArchieveLogic.Delete(ClientGUID, ArchiveXML, Request.ServerVariables["HTTP_HOST"], out lstArchiveID, customSettings.v4LibraryRollup);
                    lstOfIQArchive_MediaModel = CommonFunctions.ProcessArchiveDisplayText(lstOfIQArchive_MediaModel, customSettings.LibraryTextType);

                    IQMedia.Shared.Utility.Log4NetLogger.Info("old Shown Parent Records : " + (TempData["FromRecordID"] != null ? TempData["FromRecordID"].ToString() : string.Empty));
                    IQMedia.Shared.Utility.Log4NetLogger.Info("old Total Parent Records : " + (TempData["TotalResults"] != null ? TempData["TotalResults"].ToString() : string.Empty));
                    IQMedia.Shared.Utility.Log4NetLogger.Info("old Shown All Records : " + (TempData["FromRecordIDDisplay"] != null ? TempData["FromRecordIDDisplay"].ToString() : string.Empty));
                    IQMedia.Shared.Utility.Log4NetLogger.Info("old Total All Records : " + (TempData["TotalResultsDisplay"] != null ? TempData["TotalResultsDisplay"].ToString() : string.Empty));
                    IQMedia.Shared.Utility.Log4NetLogger.Info("old Total All Records : " + (TempData["TotalResultsDisplay"] != null ? TempData["TotalResultsDisplay"].ToString() : string.Empty));


                    IQMedia.Shared.Utility.Log4NetLogger.Info("deleted items count : " + (lstArchiveID != null ? lstArchiveID.Count.ToString() : string.Empty));

                    if (lstArchiveID != null && lstArchiveID.Count > 0)
                    {

                        if (TempData["FromRecordID"] != null && Convert.ToInt64(TempData["FromRecordID"]) > 0)
                        {
                            TempData["FromRecordID"] = Convert.ToInt64(TempData["FromRecordID"]) - p_ParentCount;
                        }

                        if (TempData["FromRecordIDDisplay"] != null && Convert.ToInt64(TempData["FromRecordIDDisplay"]) > 0)
                        {
                            TempData["FromRecordIDDisplay"] = Convert.ToInt64(TempData["FromRecordIDDisplay"]) - lstArchiveID.Count; 
                        }

                        if (TempData["TotalResults"] != null && Convert.ToInt64(TempData["TotalResults"]) > 0)
                        {
                            TempData["TotalResults"] = Convert.ToInt64(TempData["TotalResults"]) - p_ParentCount;
                        }

                        if (TempData["TotalResultsDisplay"] != null && Convert.ToInt64(TempData["TotalResultsDisplay"]) > 0)
                        {
                            TempData["TotalResultsDisplay"] = Convert.ToInt64(TempData["TotalResultsDisplay"]) - lstArchiveID.Count;
                        }

                        strArchiveID = string.Join(",", lstArchiveID.Select(id => id));

                        IQMedia.Shared.Utility.Log4NetLogger.Info("update filter values");
                        IQMedia.Shared.Utility.Log4NetLogger.Info("since id :" + (TempData["SinceID"] != null ? TempData["SinceID"].ToString() : string.Empty));

                        long SinceID = Convert.ToInt64(TempData["SinceID"]);
                        iqArchiveFilter = iQArchieveLogic.GetIQArchieveFilters(ClientGUID, p_CustomerGuid, p_FromDate, p_ToDate, p_SubMediaType, p_SearchTerm, p_CategoryGUID, p_SelectionType, SinceID, sessionInformation.Isv4TM);
                    }

                    TempData.Keep();
                }

                LibraryResult json = new LibraryResult
                {
                    isSuccess = true,
                    HTML = lstOfIQArchive_MediaModel != null ? RenderPartialToString(PATH_LibraryPartialView, lstOfIQArchive_MediaModel) : string.Empty,
                    filter = iqArchiveFilter,
                    archiveIDs = strArchiveID,
                    totalRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["TotalResults"])),
                    totalRecordsDisplay = string.Format("{0:n0}", Convert.ToInt64(TempData["TotalResultsDisplay"])),
                    currentRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["FromRecordID"])),
                    currentRecordsDisplay = string.Format("{0:n0}", Convert.ToInt64(TempData["FromRecordIDDisplay"]))
                };

                return Content(IQMedia.Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                LibraryResult json = new LibraryResult
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                
                return Content(IQMedia.Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
            }
            finally { TempData.Keep(); }
        }

        

        [HttpPost]
        public JsonResult GetChart(string ClipID)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();

                IQTimeSync_DataLogic iQTimeSync_DataLogic = (IQTimeSync_DataLogic)LogicFactory.GetLogic(LogicType.IQTimeSync_Data);
                List<IQTimeSync_DataModel> lstiQTimeSync_DataModel = iQTimeSync_DataLogic.GetClipTimeSyncDataByClipGuidAndCustomerGuid(new Guid(ClipID), sessionInformation.CustomerGUID);

                bool IsTimeSync = true;
                if (lstiQTimeSync_DataModel != null && lstiQTimeSync_DataModel.Count > 0)
                {
                    foreach (IQTimeSync_DataModel item in lstiQTimeSync_DataModel)
                    {
                        item.Data = iQTimeSync_DataLogic.TimeSyncHighLineChart(item.Data, item.GraphStructure);
                    }
                }
                else
                {
                    IsTimeSync = false;
                }

                return Json(new
                {
                    lineChartJson = lstiQTimeSync_DataModel,
                    isTimeSync = IsTimeSync,
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
                TempData.Keep("KantorMediaTempData");
            }
        }

        [HttpPost]
        public JsonResult SendEmail(string ArchiveIDs, string FromEmail, string ToEmail, string BCCEmail, string Subject, string UserBody)
        {

            IQMedia.Shared.Utility.Log4NetLogger.Info("Library Send Email Start");
            IQMedia.Shared.Utility.Log4NetLogger.Info("MediaID :" + ArchiveIDs);
            IQMedia.Shared.Utility.Log4NetLogger.Info("FromEmail :" + FromEmail);
            IQMedia.Shared.Utility.Log4NetLogger.Info("ToEmail :" + ToEmail);
            IQMedia.Shared.Utility.Log4NetLogger.Info("BCCEmail :" + BCCEmail);
            IQMedia.Shared.Utility.Log4NetLogger.Info("Subject :" + Subject);
            IQMedia.Shared.Utility.Log4NetLogger.Info("UserBody :" + UserBody);

            try
            {
                if (ToEmail.Split(new char[] { ';' }).Count() <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"]) &&
                        (String.IsNullOrEmpty(BCCEmail) || BCCEmail.Split(new char[] { ';' }).Count() <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"])))
                {
                    int EmailSendCount = 0;
                    int MaxCount = 0, Count = 1;
                    var maxemailreportitems = GetIQCustomSettings();
                    string[] bccEmails = !String.IsNullOrWhiteSpace(BCCEmail) ? BCCEmail.Split(new char[] { ';' }) : new string[0];

                    if (maxemailreportitems != null && maxemailreportitems.v4MaxLibraryEmailReportItems.HasValue)
                    {
                        MaxCount = maxemailreportitems.v4MaxLibraryEmailReportItems.Value;
                    }

                    if (!string.IsNullOrEmpty(ArchiveIDs))
                    {
                        string ArchiveXML = string.Empty;
                        foreach (string id in ArchiveIDs.Split(new char[] { ',' }))
                        {
                            if (Count <= MaxCount && !string.IsNullOrWhiteSpace(id.Trim()))
                            {
                                ArchiveXML += "<id> " + id + "</id>";
                                Count++;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(ArchiveXML))
                        {
                            ArchiveXML = "<list>" + ArchiveXML + "</list>";
                        }

                        IQMedia.Shared.Utility.Log4NetLogger.Info("fetch selected items details from DB");

                        IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                        List<IQArchive_MediaModel> lstIQArchive_Media = iQArchieveLogic.GetIQArchieveResultsForEmail(new Guid(ClientGUID), ArchiveXML, Request.ServerVariables["HTTP_HOST"]);

                        lstIQArchive_Media = CommonFunctions.GetGMTandDSTTime(lstIQArchive_Media, CommonFunctions.ResultType.Library);
                        lstIQArchive_Media = CommonFunctions.ProcessArchiveDisplayText(lstIQArchive_Media, maxemailreportitems.LibraryTextType);

                        string HTML = RenderPartialToString(PATH_LibraryEmailPartialView, lstIQArchive_Media);

                        if (!string.IsNullOrEmpty(UserBody))
                        {
                            UserBody = UserBody.Replace("\n", "<br />").Replace("\r\n", "<br />");
                            HTML = HTML.Replace("##UserMessage##", UserBody);
                        }
                        else
                        {
                            HTML = HTML.Replace("##UserMessage##", string.Empty);
                        }

                        StreamReader strmEmailPolicy = new StreamReader(Server.MapPath("~/content/EmailPolicy.txt"));
                        string emailPolicy = strmEmailPolicy.ReadToEnd();
                        strmEmailPolicy.Close();
                        strmEmailPolicy.Dispose();
                        HTML = HTML + emailPolicy;

                        if (!string.IsNullOrEmpty(ToEmail))
                        {
                            IQMedia.Shared.Utility.Log4NetLogger.Info("split by semicolon (;), and send email to all users in list");                            
                            foreach (string id in ToEmail.Split(new char[] { ';' }))
                            {
                                // send email code
                                IQMedia.Shared.Utility.Log4NetLogger.Info("fetch email to : " + id);
                                if (IQMedia.Shared.Utility.CommonFunctions.SendMail(id, string.Empty, bccEmails, FromEmail, Subject, HTML, true, null))
                                {
                                    EmailSendCount++;
                                }
                            }
                        }
                    }

                    var json = new
                    {
                        isSuccess = true,
                        emailSendCount = EmailSendCount + bccEmails.Count()
                    };

                    IQMedia.Shared.Utility.Log4NetLogger.Info("Library Send Email End");

                    return Json(json);
                }
                else
                {

                    var json = new
                    {
                        isSuccess = false,
                        errorMessage = Config.ConfigSettings.Settings.MaxEmailAdressLimitExceeds.Replace("@@MaxLimit@@", System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"])
                    };

                    return Json(json);
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                IQMedia.Shared.Utility.Log4NetLogger.Fatal("Library Send Email Error :" + _ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }

            
        }

        [HttpPost]
        public JsonResult GetEditRecord(long ID)
        {
            try
            {
                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);

                IQArchive_EditModel editModel = iQArchieveLogic.GetIQArchiveByIDForEdit(ClientGUID, ID);
                editModel.UseDisplayDescription = GetIQCustomSettings().LibraryTextType != Shared.Utility.CommonFunctions.LibraryTextTypes.Description && editModel.SubMediaType != Shared.Utility.CommonFunctions.CategoryType.MS;
                string editHTML = string.Empty;

                if (editModel != null)
                {
                    editHTML = RenderPartialToString(PATH_LibraryEditRecordPartialView, editModel);
                }

                var json = new
                {
                    isSuccess = true,
                    HTML = editHTML
                };
                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult UpdateMediaRecord(long p_ID, string p_Title, Guid? p_CategoryGuid, Guid? p_SubCategory1Guid, Guid? p_SubCategory2Guid,
                                                    Guid? p_SubCategory3Guid, string p_Keywords, string p_Description, bool? p_DisplayDescription,
                                                    short p_PositiveSentiment, short p_NegativeSentiment)
        {
            try
            {
                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);

                iQArchieveLogic.Update_MediaRecord(p_ID, p_Title, p_CategoryGuid, p_SubCategory1Guid, p_SubCategory2Guid, p_SubCategory3Guid, p_Keywords, p_Description, p_DisplayDescription, p_PositiveSentiment, p_NegativeSentiment, new Guid(ClientGUID));

                var json = new
                {
                    isSuccess = true
                };
                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }
        [HttpGet]
        public ActionResult RefreshTVResults()
        {
            GetRefreshTVResults();
            return RedirectToAction("Index");
        }

        private void GetRefreshTVResults()
        {
            try
            {
                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                List<IQArchive_RefreshResultsForTV> lstRefreshResultsForTV = iQArchieveLogic.GetRefreshResultsForTV(new Guid(ClientGUID));

                if (lstRefreshResultsForTV != null)
                {
                    foreach (IQArchive_RefreshResultsForTV item in lstRefreshResultsForTV)
                    {
                        if (!string.IsNullOrEmpty(item.ClipGuid))
                        {
                            string clipCCURL = string.Format(Convert.ToString(ConfigurationManager.AppSettings["CCService_RefreshResultURL"]), item.ClipGuid);

                            HttpWebRequest objRequest = null;
                            HttpWebResponse objResponse = null;

                            objRequest = (HttpWebRequest)WebRequest.Create(clipCCURL);
                            objRequest.Method = "GET";
                            objRequest.Timeout = 300000;
                            objRequest.ReadWriteTimeout = 300000;

                            objResponse = (HttpWebResponse)objRequest.GetResponse();
                            if (objResponse.StatusCode == HttpStatusCode.OK)
                            {
                                StreamReader _Response = new StreamReader(objResponse.GetResponseStream(), System.Text.Encoding.Default);
                                string responseCC = _Response.ReadToEnd();
                                if (!string.IsNullOrWhiteSpace(responseCC))
                                {
                                    iQArchieveLogic.Update_ArchiveClipClosedCaption(item.ArchiveClipKey, responseCC);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
            }
            finally { TempData.Keep(); }
        }

        private bool HasMoreResults()
        {
            if (Convert.ToInt64(TempData["FromRecordID"]) < Convert.ToInt64(TempData["TotalResults"]))
                return true;
            else
                return false;
        }

        [HttpPost]
        public JsonResult GetPlayLogNSummary(string p_AssetGuid, DateTime p_FromDate, DateTime p_ToDate)
        {
            try
            {
                ClipLogic clipLogic = (ClipLogic)LogicFactory.GetLogic(LogicType.Clip);

                List<IQTrackPlayLogModel> lstIQTrackPlayLogModel = clipLogic.GetPlayLogNSummary(p_AssetGuid, p_FromDate, p_ToDate);
                Dictionary<string, long> regionPlayMapList = lstIQTrackPlayLogModel.FirstOrDefault().RegionPlayMapList;

                Int64 totalViews;
                var jsonSparkChartRecords = clipLogic.GetTVViewsSparkChart(lstIQTrackPlayLogModel, p_FromDate, p_ToDate, out totalViews);
                var jsonFusionMapRecords = clipLogic.GetDemographicsFusionMap(regionPlayMapList);

                StringBuilder topRegionHTML = new StringBuilder();
                if (regionPlayMapList != null && regionPlayMapList.Count > 0)
                {
                    IEnumerable<KeyValuePair<string, long>> topRegionList = regionPlayMapList.OrderBy(x => x.Key).OrderByDescending(x => x.Value).Take(3);
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                    foreach (KeyValuePair<string, long> region in topRegionList)
                    {
                        topRegionHTML.Append("<li>" + textInfo.ToTitleCase(region.Key.ToLower()) + "</li>");
                    }
                }

                var json = new
                {
                    isSuccess = true,
                    jsonSparkChartRecords = jsonSparkChartRecords,
                    jsonFusionMapRecords = jsonFusionMapRecords,
                    topRegionHTML = topRegionHTML.ToString(),
                    topReferrerSitesHTML = RenderPartialToString(PATH_LibraryTopReferrerSites, lstIQTrackPlayLogModel.FirstOrDefault().TopReferrersList),
                    totalViews = totalViews,
                    clipTitle = lstIQTrackPlayLogModel.FirstOrDefault().ClipTitle,
                    lifeTimeCount = lstIQTrackPlayLogModel.FirstOrDefault().LifeTimeCount
                };
                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult GetDashboardArchiveIDs(string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_SubMediaType, List<string> p_CategoryGUID, string p_SelectionType, bool p_IsOnlyParents)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                long sinceID = TempData["SinceID"] != null ? Convert.ToInt64(TempData["SinceID"]) : 0;

                if (p_FromDate.HasValue && p_ToDate.HasValue)
                {
                    p_FromDate = Utility.CommonFunctions.GetGMTandDSTTime(p_FromDate);

                    p_ToDate = p_ToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    p_ToDate = Utility.CommonFunctions.GetGMTandDSTTime(p_ToDate);
                }

                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                List<Int64> archiveMediaIDs = iQArchieveLogic.GetIQArchiveResultsForDashboard(ClientGUID, p_CustomerGuid, p_FromDate, p_ToDate, p_SubMediaType, p_SearchTerm, p_CategoryGUID, p_SelectionType, sinceID, sessionInformation.Isv4TM, GetIQCustomSettings().v4LibraryRollup, p_IsOnlyParents);
                
                var json = new
                {
                    mediaIDs = archiveMediaIDs,
                    isSuccess = true
                };
                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }

        }

        [HttpGet]
        public ActionResult ShareClip(string p_ClipID, string p_SourceType)
        {
            SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
            Boolean IsSharing = sspLogic.GetSharing(p_ClipID, Request.Cookies[".IQAUTH"]);
            if (IsSharing)
            {
                if (p_SourceType == "FB")
                {
                    Response.Redirect("https://www.facebook.com/sharer.php?u=" + ConfigurationManager.AppSettings["ClipPlayerURL"] + p_ClipID);
                }
                else if (p_SourceType == "TW")
                {
                    Response.Redirect("https://twitter.com/home?status=" + ConfigurationManager.AppSettings["ClipPlayerURL"] + p_ClipID);
                }
                else if (p_SourceType == "GP")
                {
                    Response.Redirect("https://plus.google.com/share?url=" + ConfigurationManager.AppSettings["ClipPlayerURL"] + p_ClipID);
                }
                else if (p_SourceType == "TR")
                {
                    Response.Redirect("http://www.tumblr.com/share/video?embed=" + ConfigurationManager.AppSettings["ClipPlayerURL"] + p_ClipID);
                }
            }
            else
            {
                ViewBag.Message = ConfigSettings.Settings.ContentShareMessage;
            }
            return View();
        }

        [HttpPost] 
        public JsonResult GetProQuestRecordByID(long ID)
        {
            try
            {
                IQArchieveLogic iQArchiveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                IQArchive_MediaModel iQArchive_MediaModel = iQArchiveLogic.GetIQArchiveByIDForView(ID);

                if (iQArchive_MediaModel != null)
                {
                    IQArchive_ArchivePQModel iQArchive_ArchivePQModel = (IQArchive_ArchivePQModel)iQArchive_MediaModel.MediaData;
                    return Json(new
                    {
                        title = iQArchive_MediaModel.Title,
                        publication = iQArchive_ArchivePQModel.Publication,
                        authors = iQArchive_ArchivePQModel.Authors != null && iQArchive_ArchivePQModel.Authors.Count > 0 ? String.Join(", ", iQArchive_ArchivePQModel.Authors) : String.Empty,
                        content = iQArchive_ArchivePQModel.ContentHTML,
                        mediaDate = iQArchive_MediaModel.MediaDate.ToShortDateString(),
                        copyright = iQArchive_ArchivePQModel.Copyright,
                        isSuccess = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false
                    });
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
        }

        [HttpPost]
        public JsonResult AddToMCMedia(List<string> mediaIDs)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();

                IQArchieveLogic iQArchiveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                int retVal = iQArchiveLogic.AddToMCMedia(sessionInformation.MCID.Value, mediaIDs, sessionInformation.IsMediaRoomEditor);

                return Json(new
                {
                    isSuccess = retVal >= 0,
                    numAdded = retVal
                });
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
        }

        [HttpPost]
        public JsonResult RemoveFromMCMedia(List<string> mediaIDs)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();

                IQArchieveLogic iQArchiveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                int retVal = iQArchiveLogic.RemoveFromMCMedia(sessionInformation.MCID.Value, mediaIDs);

                return Json(new
                {
                    isSuccess = retVal >= 0,
                    numRemoved = retVal
                });
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
        }

        [HttpPost]
        public JsonResult SendMCMediaEmail(string ArchiveIDs, string FromEmail, string ToEmail, string BCCEmail, string Subject, string UserBody, int TemplateID)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();

                if (ToEmail.Split(new char[] { ';' }).Count() <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"]) &&
                        (String.IsNullOrEmpty(BCCEmail) || BCCEmail.Split(new char[] { ';' }).Count() <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"])))
                {
                    IQ_ReportTypeModel emailTemplate = ((List<IQ_ReportTypeModel>)TempData["EmailTemplates"]).Where(w => w.ID == TemplateID).FirstOrDefault();

                    if (emailTemplate != null)
                    {
                        int EmailSendCount = 0;
                        int MaxCount = 0, Count = 1;
                        var maxemailreportitems = GetIQCustomSettings();
                        string[] bccEmails = !String.IsNullOrWhiteSpace(BCCEmail) ? BCCEmail.Split(new char[] { ';' }) : new string[0];

                        if (maxemailreportitems != null && maxemailreportitems.v4MaxLibraryEmailReportItems.HasValue)
                        {
                            MaxCount = maxemailreportitems.v4MaxLibraryEmailReportItems.Value;
                        }

                        if (!string.IsNullOrEmpty(ArchiveIDs))
                        {
                            string ArchiveXML = string.Empty;
                            foreach (string id in ArchiveIDs.Split(new char[] { ',' }))
                            {
                                if (Count <= MaxCount && !string.IsNullOrWhiteSpace(id.Trim()))
                                {
                                    ArchiveXML += "<id> " + id + "</id>";
                                    Count++;
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(ArchiveXML))
                            {
                                ArchiveXML = "<list>" + ArchiveXML + "</list>";
                            }

                            EmailTemplateLogic emailTemplateLogic = (EmailTemplateLogic)LogicFactory.GetLogic(LogicType.EmailTemplate);
                            EmailResultsModel mediaResults = emailTemplateLogic.GetArchiveResultsForEmail(sessionInformation.MCID, sessionInformation.ClientGUID, ArchiveXML, emailTemplate.Settings, Request.ServerVariables["HTTP_HOST"]);

                            foreach (Email_GroupTier1Model groupTier1Result in mediaResults.GroupTier1Results)
                            {
                                foreach (Email_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                                {
                                    foreach (Email_GroupTier3Model groupTier3Result in groupTier2Result.GroupTier3Results)
                                    {
                                        IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(groupTier3Result.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Library);
                                        IQMedia.WebApplication.Utility.CommonFunctions.ProcessArchiveDisplayText(groupTier3Result.MediaResults, maxemailreportitems.LibraryTextType);
                                    }
                                }
                            }

                            string HTML = RenderPartialToString(emailTemplate.Settings.ViewPath, mediaResults);

                            if (!string.IsNullOrEmpty(UserBody))
                            {
                                UserBody = UserBody.Replace("\n", "<br />").Replace("\r\n", "<br />");
                                HTML = HTML.Replace("##UserMessage##", UserBody);
                            }
                            else
                            {
                                HTML = HTML.Replace("##UserMessage##", string.Empty);
                            }

                            StreamReader strmEmailPolicy = new StreamReader(Server.MapPath("~/content/EmailPolicy.txt"));
                            string emailPolicy = strmEmailPolicy.ReadToEnd();
                            strmEmailPolicy.Close();
                            strmEmailPolicy.Dispose();
                            HTML = HTML + emailPolicy;

                            if (!string.IsNullOrEmpty(ToEmail))
                            {
                                foreach (string id in ToEmail.Split(new char[] { ';' }))
                                {
                                    // send email code
                                    IQMedia.Shared.Utility.Log4NetLogger.Info("fetch email to : " + id);
                                    if (IQMedia.Shared.Utility.CommonFunctions.SendMail(id, string.Empty, bccEmails, FromEmail, Subject, HTML, true, null))
                                    {
                                        EmailSendCount++;
                                    }
                                }
                            }
                        }

                        var json = new
                        {
                            isSuccess = true,
                            emailSendCount = EmailSendCount + bccEmails.Count()
                        };

                        return Json(json);
                    }
                    else
                    {
                        var json = new
                        {
                            isSuccess = false,
                            errorMessage = ConfigSettings.Settings.ErrorOccurred
                        };

                        return Json(json);
                    }
                }
                else
                {
                    var json = new
                    {
                        isSuccess = false,
                        errorMessage = Config.ConfigSettings.Settings.MaxEmailAdressLimitExceeds.Replace("@@MaxLimit@@", System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"])
                    };

                    return Json(json);
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        #endregion

        #region Libarary Report

        #region Library Report Folder

        [HttpPost]
        public JsonResult GetLibraryReportFolders()
        {
            try
            {
                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                List<IQReport_FolderModel> lstIQReport_FolderModel = iQReport_FolderLogic.GetReportFolderByClientGuid(new Guid(ClientGUID));

                return Json(new
                {
                    reportFolders = lstIQReport_FolderModel,
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
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult LoadReportFolder()
        {
            try
            {
                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                List<IQReport_FolderModel> lstIQReport_FolderModel = iQReport_FolderLogic.GetReportAndReportFolderData(new Guid(ClientGUID));

                return Json(new
                {
                    isSuccess = true,
                    jsonFolders = lstIQReport_FolderModel
                });
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult CreateReportFolder(string p_Name, string p_ParentID)
        {
            try
            {
                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                string _result = iQReport_FolderLogic.CreateFolder(p_Name, p_ParentID, new Guid(ClientGUID));

                if (!string.IsNullOrEmpty(_result) && Convert.ToInt32(_result) > 0)
                {
                    return Json(new
                    {
                        isSuccess = true,
                        id = _result
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        isDuplicate = _result == "-2" ? true : false
                    });
                }
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult MoveReportFolder(string p_ID, string p_ParentID,string p_Type)
        {
            try
            {
                string _result = string.Empty;
                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                if (p_Type == "folder")
                {
                    _result= iQReport_FolderLogic.MoveFolder(p_ParentID, p_ID, new Guid(ClientGUID));
                }
                else
                {
                    _result= iQReport_FolderLogic.MoveReport(p_ParentID,  p_ID.Replace("_1",string.Empty), new Guid(ClientGUID));
                }

                if (!string.IsNullOrEmpty(_result) && Convert.ToInt32(_result) > 0)
                {
                    return Json(new
                    {
                        isSuccess = true,
                    });
                }
                else
                {
                    if (p_Type == "folder" && _result == "-1")
                    {
                        return Json(new
                        {
                            isSuccess = false,
                            isDuplicate = true
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            isSuccess = false
                        });
                    }
                }
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult RenameReportFolder(string p_ID, string p_Name)
        {
            try
            {
                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                string _result = iQReport_FolderLogic.RenameFolder(p_ID, p_Name, new Guid(ClientGUID));

                if (!string.IsNullOrEmpty(_result) && Convert.ToInt32(_result) > 0)
                {
                    return Json(new
                    {
                        isSuccess = true,
                    });
                }
                else
                {
                    
                    return Json(new
                    {
                        isSuccess = false,
                        isDuplicate = _result == "-1" ? true : false
                    });
                }
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult DeleteReportFolder(string p_ID)
        {
            try
            {
                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                string _result = iQReport_FolderLogic.DeleteFolder(p_ID, new Guid(ClientGUID));

                if (!string.IsNullOrEmpty(_result) && Convert.ToInt32(_result) > 0)
                {
                    return Json(new
                    {
                        isSuccess = true,
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                    });
                }
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult PasteReportFolder(string p_CopyID, string p_PasteID)
        {
            try
            {
                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                string _result = iQReport_FolderLogic.PasteFolder(p_CopyID, p_PasteID, new Guid(ClientGUID));

                if (!string.IsNullOrEmpty(_result) && Convert.ToInt32(_result) > 0)
                {
                    return Json(new
                    {
                        isSuccess = true,
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                    });
                }
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        #endregion

        [HttpGet]
        public ActionResult ReportSort(string reportID)
        {
            ViewBag.IsInvalidID = false;
            ViewBag.IsError = false;

            try
            {
                Int64 temp;
                if (!string.IsNullOrEmpty(reportID) && Int64.TryParse(reportID, out temp))
                {
                    sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                    IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                    IQArchive_DisplayLibraryReport objDisplayLibraryReport = iQArchieveLogic.GetIQArchieveResultsForLibraryReport(temp, Request.ServerVariables["HTTP_HOST"], new Guid(ClientGUID),
                                                                                                                                    sessionInformation.IsNielsenData, sessionInformation.IsCompeteData, sessionInformation.MediaTypes);
                    if (objDisplayLibraryReport != null & objDisplayLibraryReport.ArchiveResults != null)
                    {
                        foreach (IQArchive_GroupTier1Model groupTier1Result in objDisplayLibraryReport.GroupTier1Results)
                        {
                            foreach (IQArchive_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                            {
                                groupTier2Result.ArchiveResults.ToList().ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDate, false));
                                CommonFunctions.GetGMTandDSTTime(groupTier2Result.ArchiveResults, CommonFunctions.ResultType.Library);
                                CommonFunctions.ProcessArchiveDisplayText(groupTier2Result.ArchiveResults, GetIQCustomSettings().LibraryTextType);
                            }
                        }
                    }

                    Dictionary<string, object> dictView = new Dictionary<string, object>();
                    dictView.Add("Report", objDisplayLibraryReport);
                    dictView.Add("IsSort", true);

                    Dictionary<string, object> dictResult = new Dictionary<string, object>();
                    dictResult.Add("HTML", RenderPartialToString(PATH_LibraryGenerateReportPartialView, dictView));

                    return View(dictResult);
                }
                else
                {
                    ViewBag.IsInvalidID = true;
                }
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                ViewBag.IsError = true;
            }

            return View();
        }

        [HttpPost]
        public JsonResult InsertLibraryReport(string p_ReportIDs, string p_ReportTitle, Int64? p_ReportImage, Int64 p_FolderID)
        {
            try
            {
                bool flag = true;
                bool IsDuplicateReport = false;
                if (!string.IsNullOrEmpty(p_ReportIDs))
                {
                    int MaxCount = 0, Count = 1;
                    var maxreportitems = GetIQCustomSettings().v4MaxLibraryReportItems;
                    MaxCount = maxreportitems.HasValue ? maxreportitems.Value : 0;

                    IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);

                    StringBuilder sbReportRule = new StringBuilder();

                    sbReportRule.Append("<Report><Library><ArchiveMediaSet>");

                    foreach (string id in p_ReportIDs.Split(new char[] { ',' }))
                    {
                        if (Count <= MaxCount && !string.IsNullOrWhiteSpace(id))
                        {
                            sbReportRule.Append("<ID>" + id + "</ID>");
                            Count++;
                        }
                    }

                    sbReportRule.Append("</ArchiveMediaSet></Library></Report>");

                    string result = iQArchieveLogic.Insert_IQ_Report(p_ReportTitle, sbReportRule.ToString(), p_ReportImage, ClientGUID, p_FolderID);

                    if (!string.IsNullOrEmpty(result))
                    {
                        if (Convert.ToInt64(result) > 0)
                        {
                            flag = true;
                        }
                        if (Convert.ToInt64(result) == 0)
                        {
                            flag = true;
                            IsDuplicateReport = true;
                        }
                        if (Convert.ToInt64(result) < 0)
                        {
                            flag = false;
                        }
                    }
                    else
                        flag = false;
                }
                else
                {
                    flag = false;
                }

                var json = new
                {
                    isSuccess = flag,
                    isDuplicate = IsDuplicateReport
                };
                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult GetSavedIQReport()
        {
            try
            {
                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                List<IQ_ReportModel> lstReport = iQArchieveLogic.GetLibraryIQ_ReportByClient(ClientGUID);
                string reportHTML = RenderPartialToString(PATH_LibrarySavedReportsPartialView, lstReport);

                List<Tuple<long, string, long>> reports = new List<Tuple<long, string, long>>();

                foreach (IQ_ReportModel item in lstReport)
                {
                    reports.Add(new Tuple<long, string, long>(item.ID, item.Title, item.RecordCount));
                }

                var json = new
                {
                    isSuccess = true,
                    HTML = reportHTML,
                    reportItems = reports
                };
                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred,
                    HTML = string.Empty
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult ModifyReport(long p_ReportID, string p_ArchiveResultIDs)
        {
            try
            {
                if (p_ReportID > 0 && !string.IsNullOrWhiteSpace(p_ArchiveResultIDs))
                {
                    sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                    IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);

                    ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                    IQReport_DetailModel iQReport_DetailModel = reportLogic.IQReportCheckForLimitnData(sessionInformation.ClientGUID, p_ReportID);

                    Int32 maxRecordLimit = 0;
                    bool isLimitApplicable = false;

                    if (iQReport_DetailModel != null)
                    {
                        if (iQReport_DetailModel.IsFeedsReport)
                        {
                            isLimitApplicable = true;
                            maxRecordLimit = iQReport_DetailModel.MaxFeedsReportItems;
                        }

                        if (iQReport_DetailModel.IsDiscoveryReport)
                        {
                            isLimitApplicable = true;
                            maxRecordLimit = iQReport_DetailModel.MaxDiscoveryReportItems;
                        }

                        maxRecordLimit = maxRecordLimit - iQReport_DetailModel.CurrentReportTotal;
                    }


                    StringBuilder sb = new StringBuilder();
                    int currentIndex = 1;
                    foreach (string id in p_ArchiveResultIDs.Split(new char[] { ',' }))
                    {
                        if (!isLimitApplicable)
                        {
                            sb.Append("<ID>" + id + "</ID>");
                        }
                        else
                        {
                            if (currentIndex <= maxRecordLimit)
                            {
                                sb.Append("<ID>" + id + "</ID>");
                                currentIndex++;
                            }
                        }

                    }

                    int SuccessCount = iQArchieveLogic.AppendItemsIQReport(new Guid(ClientGUID), p_ReportID, sb.ToString());

                    var json = new
                    {
                        isSuccess = true,
                        updateCount = SuccessCount
                    };
                    return Json(json);
                }
                else
                {
                    var json = new
                    {
                        isSuccess = false,
                        errorMessage = ConfigSettings.Settings.MissingParameter
                    };
                    return Json(json);
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult RemoveReportItems(long p_ReportID, string p_ArchiveResultIDs)
        {
            try
            {
                if (p_ReportID > 0 && !string.IsNullOrWhiteSpace(p_ArchiveResultIDs))
                {
                    IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);

                    StringBuilder sb = new StringBuilder();
                    foreach (string id in p_ArchiveResultIDs.Split(new char[] { ',' }))
                    {
                        sb.Append("<ID>" + id + "</ID>");
                    }

                    int DeleteCount = iQArchieveLogic.RemoveItemsIQReport(new Guid(ClientGUID), p_ReportID, sb.ToString());

                    var json = new
                    {
                        isSuccess = true,
                        deleteCount = DeleteCount
                    };
                    return Json(json);
                }
                else
                {
                    var json = new
                    {
                        isSuccess = false,
                        errorMessage = ConfigSettings.Settings.MissingParameter
                    };
                    return Json(json);
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult RemoveReportByID(long p_ReportID)
        {
            try
            {
                if (p_ReportID > 0)
                {
                    IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);

                    int DeleteCount = iQArchieveLogic.RemoveIQReportByReportID(p_ReportID, ClientGUID);

                    var json = new
                    {
                        isSuccess = true,
                        deleteCount = DeleteCount
                    };
                    return Json(json);
                }
                else
                {
                    var json = new
                    {
                        isSuccess = false,
                        errorMessage = ConfigSettings.Settings.MissingParameter
                    };
                    return Json(json);
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        // 
        [HttpPost]
        public ContentResult GenerateLibraryReportByID(long p_ReportID)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                IQArchive_DisplayLibraryReport objDisplayLibraryReport = iQArchieveLogic.GetIQArchieveResultsForLibraryReport(p_ReportID, Request.ServerVariables["HTTP_HOST"], new Guid(ClientGUID),
                                                                                                                                sessionInformation.IsNielsenData, sessionInformation.IsCompeteData, sessionInformation.MediaTypes);
                if (objDisplayLibraryReport != null & objDisplayLibraryReport.ArchiveResults != null)
                {
                    foreach (IQArchive_GroupTier1Model groupTier1Result in objDisplayLibraryReport.GroupTier1Results)
                    {
                        foreach (IQArchive_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                        {
                            groupTier2Result.ArchiveResults.ToList().ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDate, false));
                            CommonFunctions.GetGMTandDSTTime(groupTier2Result.ArchiveResults, CommonFunctions.ResultType.Library);
                            CommonFunctions.ProcessArchiveDisplayText(groupTier2Result.ArchiveResults, GetIQCustomSettings().LibraryTextType);
                        }
                    }
                }

                Dictionary<string, object> dictView = new Dictionary<string, object>();
                dictView.Add("Report", objDisplayLibraryReport);
                dictView.Add("IsSort", false);
                string reportHTML = RenderPartialToString(PATH_LibraryGenerateReportPartialView, dictView);

                LibraryResult json = new LibraryResult
                {
                    isSuccess = true,
                    HTML = reportHTML,
                    title = objDisplayLibraryReport != null && objDisplayLibraryReport.ReportDetails != null ? objDisplayLibraryReport.ReportDetails.Title : null,
                    reportSettings = objDisplayLibraryReport != null && objDisplayLibraryReport.ReportDetails != null ? objDisplayLibraryReport.ReportDetails.Settings : null,
                    reportImageId = objDisplayLibraryReport != null && objDisplayLibraryReport.ReportDetails != null ? objDisplayLibraryReport.ReportDetails._ReportImageID : null,
                    reportHasCustomSort = objDisplayLibraryReport != null && objDisplayLibraryReport.ReportDetails != null ? objDisplayLibraryReport.ReportDetails.HasCustomSort : false
                };
                return Content(IQMedia.Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                LibraryResult json = new LibraryResult
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred,
                    HTML = string.Empty
                };
                return Content(IQMedia.Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public ContentResult GetAdhocMediumData(long p_ReportId, string p_Medium)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                string MediaIDXml = "<list><item id=\"" + p_ReportId + "\"/></list>";

                DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
                IQAgent_DashBoardModel iQAgent_DashBoardModel = dashboardLogic.GetAdhocSummaryData(MediaIDXml, "Report", p_Medium, sessionInformation.ClientGUID, sessionInformation.MediaTypes);

                dynamic jsonResult;

                if (iQAgent_DashBoardModel.ListOfIQAgentSummary.Count > 0)
                {
                    DateTime p_FromDate = iQAgent_DashBoardModel.ListOfIQAgentSummary.Min(x => x.DayDate);
                    DateTime p_ToDate = iQAgent_DashBoardModel.ListOfIQAgentSummary.Max(x => x.DayDate);
                    Int16 searchType;

                    if ((p_ToDate - p_FromDate).Days > 2)
                    {
                        searchType = 1;
                    }
                    else
                    {
                        searchType = 0;

                        // The line chart doesn't render correctly when only displaying a single day
                        if (p_FromDate == p_ToDate)
                        {
                            p_ToDate = p_ToDate.AddDays(1);
                        }
                    }

                    var user = ActiveUserMgr.GetActiveUser();

                    if (p_Medium.ToLower() == "overview")
                    {
                        List<SummaryReportModel> lstSummaryReport = iQAgent_DashBoardModel.ListOfIQAgentSummary.Select(s => new SummaryReportModel()
                        {
                            Audience = s.Audience,
                            GMT_DateTime = s.DayDate,
                            IQMediaValue = s.IQMediaValue,
                            MediaType = s.MediaType,
                            SubMediaType = s.SubMediaType,
                            Number_Docs = s.NoOfDocs,
                            Number_Of_Hits = s.NoOfHits,
                            SearchRequestID = s.SearchRequestID,
                            Query_Name = s.Query_Name,                            
                            DefaultMediaType = sessionInformation.MediaTypes.Where(mt => string.Compare(mt.MediaType, s.MediaType, true) == 0).Count() > 0 ? true : false

                        }).ToList();                        

                        Dictionary<string, object> dictResults = CommonController.GetDashboardAdhocResults(lstSummaryReport, p_FromDate, p_ToDate, searchType, null, sessionInformation.Isv4UGCAccess, user);
                        DashboardOverviewResults dashboardOverviewResults = (DashboardOverviewResults)dictResults["OverviewResults"];
                        jsonResult = (ExpandoObject)dictResults["JsonResult"];

                        jsonResult.HTML = RenderPartialToString(PATH_DashboardOverviewPartialView, dashboardOverviewResults);
                        jsonResult.hasData = true;
                        jsonResult.isSuccess = true;
                    }
                    else
                    {
                        if (user.MediaTypes.Where(mt=> mt.TypeLevel == 1 && mt.MediaType == "MS").Count() == 0)
                        {
                            user.MediaTypes.Add(new IQ_MediaTypeModel() { MediaType = "MS", TypeLevel = 1, HasAccess = user.Isv4UGCAccess, DisplayName = "Miscellaneous", DashboardData = new DashboardData() { ArchiveDataLists = new List<DataList>(), ChartTypes = new List<DataChart>()} });
                        }

                        var mediaType = user.MediaTypes.Where(mt => mt.TypeLevel == 1 && string.Compare(p_Medium, mt.MediaType, true) == 0).Single();

                        Dictionary<string, object> dictResults = CommonController.GetDashboardMediumResults(mediaType, iQAgent_DashBoardModel, p_FromDate, p_ToDate, searchType, null, false,user);
                        DashboardMediaResults dashboardMediaResults = (DashboardMediaResults)dictResults["MediaResults"];
                        jsonResult = (ExpandoObject)dictResults["JsonResult"];

                        var TopDmasHTML = string.Empty;
                        var TopStationsHTML = string.Empty;
                        var TopOnlineNewsDmasHTML = string.Empty;
                        var TopOnlineNewsSitesHTML = string.Empty;
                        var TopPrintPublicationsHTML = string.Empty;
                        var topPrintAuthorsHTML = string.Empty;

                        Func<DataList, List<DashboardTopResultsModel>, string> RenderDataList = (dl, data) =>
                        {
                            var templateHTML = "";

                            Dictionary<string, object> dictTopPubs = new Dictionary<string, object>();
                            dictTopPubs.Add("Results", data);
                            dictTopPubs.Add("Medium", mediaType);
                            dictTopPubs.Add("TitleGrid", dl.Title);
                            dictTopPubs.Add("TitleColumn", dl.TitleColumn);
                            dictTopPubs.Add("DataType", dl.DataType);
                            dictTopPubs.Add("CompeteAccess", sessionInformation.IsCompeteData);
                            dictTopPubs.Add("NielsenAccess", sessionInformation.IsNielsenData);
                            dictTopPubs.Add("OnClickEnable", false);

                            switch (dl.TemplateType)
                            {
                                case TemplateTypes.TVDMA:
                                    templateHTML = RenderPartialToString(PATH_DashboardTopBroadcastDMA, dictTopPubs);
                                    break;
                                case TemplateTypes.TVStation:
                                    templateHTML = RenderPartialToString(PATH_DashboardTopBroadcastStation, dictTopPubs);
                                    break;
                                /*case TemplateTypes.TVCountry:
                                    templateHTML = RenderPartialToString(PATH_DashboardTopBroadcastCountries, dictTopPubs);
                                    break;*/
                                case TemplateTypes.NMDMA:
                                    templateHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsDMA, dictTopPubs);
                                    break;
                                case TemplateTypes.Common:                                   
                                    templateHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsSites, dictTopPubs);
                                    break;
                                default:
                                    break;
                            }

                            return templateHTML;
                        };

                        List<Tuple<string, string>> dataLists = new List<Tuple<string, string>>();
                        var listHtml = "";

                        foreach (DataList dl in mediaType.DashboardData.ArchiveDataLists)
                        {
                            switch (dl.ListType)
                            {
                                case ListTypes.Country:
                                    listHtml = RenderDataList(dl, iQAgent_DashBoardModel.ListOfTopCountryBroadCast);
                                    break;
                                case ListTypes.DMA:
                                    listHtml = RenderDataList(dl, iQAgent_DashBoardModel.ListOfTopDMABroadCast);
                                    break;
                                case ListTypes.Station:
                                    listHtml = RenderDataList(dl, iQAgent_DashBoardModel.ListOfTopStationBroadCast);
                                    break;
                                default:
                                    break;
                            }

                            dataLists.Add(Tuple.Create(dl.TemplateType.ToString(), listHtml));
                        }
                        /*
                        if (p_Medium == IQMedia.Shared.Utility.CommonFunctions.CategoryType.TV.ToString())
                        {
                            TopDmasHTML = RenderPartialToString(PATH_DashboardTopBroadcastDMA, iQAgent_DashBoardModel.ListOfTopDMABroadCast);
                            TopStationsHTML = RenderPartialToString(PATH_DashboardTopBroadcastStation, iQAgent_DashBoardModel.ListOfTopStationBroadCast);
                        }
                        else if (p_Medium != IQMedia.Shared.Utility.CommonFunctions.CategoryType.Radio.ToString() && 
                                 p_Medium != IQMedia.Shared.Utility.CommonFunctions.CategoryType.PM.ToString() && 
                                 p_Medium != IQMedia.Shared.Utility.CommonFunctions.CategoryType.MS.ToString())
                        {
                            if (p_Medium == IQMedia.Shared.Utility.CommonFunctions.CategoryType.NM.ToString())
                            {
                                TopOnlineNewsDmasHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsDMA, iQAgent_DashBoardModel.ListOfTopDMABroadCast);
                            }
                            Dictionary<string, object> dicTopSites = new Dictionary<string, object>();

                            dicTopSites.Add("Results", iQAgent_DashBoardModel.ListOfTopStationBroadCast);
                            dicTopSites.Add("Medium", p_Medium);
                            TopOnlineNewsSitesHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsSites, dicTopSites);
                        }
                        else if (p_Medium == IQMedia.Shared.Utility.CommonFunctions.CategoryType.PM.ToString())
                        {
                            Dictionary<string, object> dictTopPubs = new Dictionary<string, object>();
                            dictTopPubs.Add("Results", iQAgent_DashBoardModel.ListOfTopStationBroadCast);
                            dictTopPubs.Add("Medium", p_Medium);
                            dictTopPubs.Add("TitleGrid", "Publications");
                            dictTopPubs.Add("TitleColumn", "Publication");
                            dictTopPubs.Add("DataType", "pub");
                            TopPrintPublicationsHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsSites, dictTopPubs);

                            Dictionary<string, object> dictTopAuthors = new Dictionary<string, object>();
                            dictTopAuthors.Add("Results", iQAgent_DashBoardModel.ListOfTopDMABroadCast);
                            dictTopAuthors.Add("Medium", p_Medium);
                            dictTopAuthors.Add("TitleGrid", "Authors");
                            dictTopAuthors.Add("TitleColumn", "Author");
                            dictTopAuthors.Add("DataType", "author");
                            topPrintAuthorsHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsSites, dictTopAuthors);
                        }
                        */
                        jsonResult.HTML = RenderPartialToString(PATH_DashboardBroadCastPartialView, dashboardMediaResults);
                        /*jsonResult.p_TopDmasHTML = TopDmasHTML;
                        jsonResult.p_TopStationsHTML = TopStationsHTML;
                        jsonResult.p_TopOnlineNewsDmasHTML = TopOnlineNewsDmasHTML;
                        jsonResult.p_TopOnlineNewsSitesHTML = TopOnlineNewsSitesHTML;
                        jsonResult.p_TopPrintPublicationsHTML = TopPrintPublicationsHTML;
                        jsonResult.p_TopPrintAuthorsHTML = topPrintAuthorsHTML;*/
                        jsonResult.DataLists = dataLists;
                        jsonResult.hasData = true;
                        jsonResult.isSuccess = true;
                    }

                    return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
                }
                else
                {
                    jsonResult = new ExpandoObject();

                    jsonResult.HTML = "<table width='100%' style='text-align: center;'><tbody><tr><td><span style='background: none repeat scroll 0 0 #F2F2F2; border: 1px solid #DCDCDC; margin-bottom: 10px; margin-right: 10px; padding: 3px 10px 3px 6px; position: relative; width: 300px !important;'>No Results found</span></td></tr></tbody></table>";
                    jsonResult.hasData = false;
                    jsonResult.isSuccess = true;

                    return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);

                dynamic jsonResult = new ExpandoObject();
                jsonResult.isSuccess = false;
                jsonResult.errorMessage = ConfigSettings.Settings.ErrorOccurred;

                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            finally { TempData.Keep(); }
        }
                
        [HttpPost]
        public JsonResult GenerateReportPDF()
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                Request.InputStream.Position = 0;

                Dictionary<string, object> dictParams;

                using (var sr = new StreamReader(Request.InputStream))
                {
                    dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
                }

                long reportID = long.Parse(dictParams["p_ReportID"].ToString());
                bool ignoreDuplicates = bool.Parse(dictParams["p_IgnoreDuplicates"].ToString());
                bool ignoreSizeLimit = bool.Parse(dictParams["p_IgnoreSizeLimit"].ToString());
                JArray chartData = (JArray)dictParams["p_ChartHTML"];

                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                IQArchive_DisplayLibraryReport objDisplayLibraryReport = iQArchieveLogic.GetIQArchieveResultsForLibraryReport(reportID, Request.ServerVariables["HTTP_HOST"], new Guid(ClientGUID),
                                                                                                                                sessionInformation.IsNielsenData, sessionInformation.IsCompeteData, sessionInformation.MediaTypes);

                // Reports larger than 1000 items can have problems processing in the ReportPDFExport service, so require customers to go through support for them (as indicated by an admin login ID)
                string matchPattern = "^admin.*@";
                if (objDisplayLibraryReport.ArchiveResults.Count > 1000 && !ignoreSizeLimit)
                {
                    return Json(new
                    {
                        isSuccess = false,
                        isSizeExceeded = true,
                        isExportAllowed = Regex.IsMatch(sessionInformation.LoginID.ToLower(), matchPattern) || sessionInformation.ClientGUID == new Guid("7722A116-C3BC-40AE-8070-8C59EE9E3D2A")
                    });
                }

                ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                if (!ignoreDuplicates && reportLogic.CheckReportPDFExportExists(reportID))
                {
                    return Json(new
                    {
                        isSuccess = false,
                        hasDuplicate = true
                    });
                }

                if (objDisplayLibraryReport != null & objDisplayLibraryReport.ArchiveResults != null)
                {
                    foreach (IQArchive_GroupTier1Model groupTier1Result in objDisplayLibraryReport.GroupTier1Results)
                    {
                        foreach (IQArchive_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                        {
                            groupTier2Result.ArchiveResults.ToList().ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDate, false));
                            CommonFunctions.GetGMTandDSTTime(groupTier2Result.ArchiveResults, CommonFunctions.ResultType.Library);
                            CommonFunctions.ProcessArchiveDisplayText(groupTier2Result.ArchiveResults, GetIQCustomSettings().LibraryTextType);
                        }
                    }
                }

                string reportHTML = RenderPartialToString(PATH_LibraryReportPDFPartialView, objDisplayLibraryReport);

                StringBuilder cssData = new StringBuilder();

                StreamReader strmReader = new StreamReader(Server.MapPath("~/css/feed.css"));
                cssData.Append(strmReader.ReadToEnd());
                strmReader.Close();
                strmReader.Dispose();

                strmReader = new StreamReader(Server.MapPath("~/css/bootstrap.css"));
                cssData.Append(strmReader.ReadToEnd());
                strmReader.Close();
                strmReader.Dispose();

                if (chartData.Count > 0)
                {
                    bool isFirstChart = true;
                    foreach (var chartItem in chartData.Children())
                    {
                        var itemProperties = chartItem.Children<JProperty>();
                        var mediumElement = itemProperties.FirstOrDefault(x => x.Name == "medium");
                        var medium = mediumElement.Value;

                        var htmlElement = itemProperties.FirstOrDefault(x => x.Name == "html");
                        var html = htmlElement.Value;
                        var pageBreakDiv = isFirstChart ? "" : "<div style='margin-top:3px; page-break-before:always; clear:both;'>&nbsp;</div>";
                        
                        reportHTML = reportHTML.Replace("<div id=\"div" + medium + "\"></div>", pageBreakDiv + html);
                        isFirstChart = false;
                    }

                    reportHTML = reportHTML.Replace("<div id=\"divResultsPageBreak\"></div>", "<div style='margin-top:3px; page-break-before:always; clear:both;'>&nbsp;</div>");

                    strmReader = new StreamReader(Server.MapPath("~/css/Dashboard.css"));
                    cssData.Append(strmReader.ReadToEnd());
                    strmReader.Close();
                    strmReader.Dispose();

                    cssData.Append(" .divSentimentNeg div{overflow:visible;} \n .divSentimentPos div{overflow:visible;} \n .divSentimentPos{width:24px;} \n .borderBottom{border-bottom:none;} \n body{background:none;}\n");

                }

                reportHTML = reportHTML.Replace("<!--DASHBOARD_CSS_PH-->", "<style type=\"text/css\">" + cssData + "</style>");

                // Create temporary html file for the service to convert from
                string tempHTMLDir = ConfigurationManager.AppSettings["DirReportExportHTML"];
                if (!Directory.Exists(tempHTMLDir))
                {
                    Directory.CreateDirectory(tempHTMLDir);
                }

                string dateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string tempHTMLFilename = CustomerGUID + "_" + dateTimeStamp + ".html";
                string tempHTMLPath = tempHTMLDir + tempHTMLFilename;

                using (FileStream fs = new FileStream(tempHTMLPath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.Write(reportHTML);
                    }
                }

                string res = reportLogic.InsertReportPDFExport(reportID, new Guid(CustomerGUID), String.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority), tempHTMLFilename);

                var json = new
                {
                    isSuccess = !string.IsNullOrEmpty(res) && Convert.ToInt32(res) > 0 ? true : false
                };
                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult GenerateReportCSV(long p_ReportID)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                IQArchive_DisplayLibraryReport objDisplayLibraryReport = iQArchieveLogic.GetIQArchieveResultsForLibraryReport(p_ReportID, Request.ServerVariables["HTTP_HOST"], new Guid(ClientGUID),
                                                                                                                                sessionInformation.IsNielsenData, sessionInformation.IsCompeteData, sessionInformation.MediaTypes);

                string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                string TempCSVPath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Library\\CSV\\" + CustomerGUID + "_" + DateTimeStamp + ".csv";
                string strCSVData = GetCSVDataForReport(objDisplayLibraryReport);
                bool IsFileGenerated = false;

                string DownloadCSVFileName = objDisplayLibraryReport.ReportDetails.Title.Replace(" ", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("\\", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("/", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("*", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("?", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace(":", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("\"", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("<", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace(">", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("|", "_");

                Session["DownloadLibraryReportCSVFileName"] = DownloadCSVFileName + ".csv";

                using (FileStream fs = new FileStream(TempCSVPath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.Write(strCSVData);
                    }
                }

                if (System.IO.File.Exists(TempCSVPath))
                {
                    IsFileGenerated = true;
                    Session["CSVFile"] = TempCSVPath;
                }

                var json = new
                {
                    isSuccess = IsFileGenerated
                };
                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpGet]
        public ActionResult DownloadCSVFile()
        {
            if (Session["CSVFile"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["CSVFile"])) && Session["DownloadLibraryReportCSVFileName"] != null)
            {
                string CSVFile = Convert.ToString(Session["CSVFile"]);
                string DownloadFileName = Convert.ToString(Session["DownloadLibraryReportCSVFileName"]);

                if (System.IO.File.Exists(CSVFile))
                {
                    Session.Remove("CSVFile");
                    return File(CSVFile, "application/csv", DownloadFileName);
                }
            }
            return Content(ConfigSettings.Settings.FileNotAvailable);
        }

        [HttpPost]
        public JsonResult ReportEmail()
        {
            try
            {
                int EmailSendCount = 0;

                Request.InputStream.Position = 0;

                Dictionary<string, object> dictParams;

                using (var sr = new StreamReader(Request.InputStream))
                {
                    dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
                }

                long ReportID = long.Parse(dictParams["p_ReportID"].ToString());
                string FromEmail = dictParams["FromEmail"].ToString();
                string ToEmail = dictParams["ToEmail"].ToString();
                string BCCEmail = dictParams["BCCEmail"].ToString();
                string Subject = dictParams["Subject"].ToString();
                string UserBody = dictParams["UserBody"].ToString();
                JArray chartData = (JArray)dictParams["p_ChartHTML"];

                if (ToEmail.Split(new char[] { ';' }).Count() <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"]) &&
                        (String.IsNullOrEmpty(BCCEmail) || BCCEmail.Split(new char[] { ';' }).Count() <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"])))
                {
                    string[] bccEmails = !String.IsNullOrWhiteSpace(BCCEmail) ? BCCEmail.Split(new char[] { ';' }) : new string[0];

                    if (ReportID > 0)
                    {
                        sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                        IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                        IQArchive_DisplayLibraryReport objDisplayLibraryReport = iQArchieveLogic.GetIQArchieveResultsForLibraryReport(ReportID, Request.ServerVariables["HTTP_HOST"], new Guid(ClientGUID),
                                                                                                                                        sessionInformation.IsNielsenData, sessionInformation.IsCompeteData, sessionInformation.MediaTypes);

                        if (objDisplayLibraryReport != null & objDisplayLibraryReport.ArchiveResults != null)
                        {
                            foreach (IQArchive_GroupTier1Model groupTier1Result in objDisplayLibraryReport.GroupTier1Results)
                            {
                                foreach (IQArchive_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                                {
                                    CommonFunctions.GetGMTandDSTTime(groupTier2Result.ArchiveResults, CommonFunctions.ResultType.Library);
                                    CommonFunctions.ProcessArchiveDisplayText(groupTier2Result.ArchiveResults, GetIQCustomSettings().LibraryTextType);
                                }
                            }
                        }

                        string reportHTML = RenderPartialToString(PATH_LibraryReportEmailPartialView, objDisplayLibraryReport);

                        string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        string TempImagePath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Library\\PDF\\" + sessionInformation.CustomerGUID + "_" + DateTimeStamp + "_{0}.jpg";
                        string[] alternateViewsName = null;
                        List<string> mediums = null;

                        if (chartData.Count > 0)
                        {
                            string chartsHTML = "<div><div id='Overview'/>";
                            foreach (IQ_MediaTypeModel objMediaType in sessionInformation.MediaTypes.Where(w => w.TypeLevel == 1 && w.HasAccess))
                            {
                                chartsHTML += "<div id='" + objMediaType.MediaType + "'/>";
                            }
                            chartsHTML += "</div>";
                            string attachmentId;
                            alternateViewsName = new string[chartData.Count];
                            mediums = new List<string>(chartData.Count);
                            int count = 0;

                            foreach (var chartItem in chartData.Children())
                            {
                                var itemProperties = chartItem.Children<JProperty>();
                                var mediumElement = itemProperties.FirstOrDefault(x => x.Name == "medium");
                                string medium = mediumElement.Value.ToString();

                                var htmlElement = itemProperties.FirstOrDefault(x => x.Name == "html");
                                string html = htmlElement.Value.ToString();

                                string imagePath = String.Format(TempImagePath, medium);
                                CreateChartAttachmentForEmail(html, imagePath);

                                attachmentId = Path.GetFileName(imagePath);
                                chartsHTML = chartsHTML.Replace("<div id='" + medium + "'/>", "<img src=\"cid:" + attachmentId + "\" /><div style='margin-top:15px;'>&nbsp;</div>");

                                alternateViewsName[count++] = String.Format(TempImagePath, medium);
                                mediums.Add(medium);
                            }

                            reportHTML = reportHTML.Replace("##DashboardCharts##", chartsHTML);
                        }
                        else
                        {
                            reportHTML = reportHTML.Replace("##DashboardCharts##", string.Empty);
                        }

                        if (!string.IsNullOrEmpty(UserBody))
                        {
                            UserBody = UserBody.Replace("\n", "<br />").Replace("\r\n", "<br />");
                            reportHTML = reportHTML.Replace("##UserMessage##", UserBody);
                        }
                        else
                        {
                            reportHTML = reportHTML.Replace("##UserMessage##", string.Empty);
                        }

                        StreamReader strmEmailPolicy = new StreamReader(Server.MapPath("~/content/EmailPolicy.txt"));
                        string emailPolicy = strmEmailPolicy.ReadToEnd();
                        strmEmailPolicy.Close();
                        strmEmailPolicy.Dispose();
                        reportHTML = reportHTML + emailPolicy;

                        if (!string.IsNullOrEmpty(ToEmail))
                        {
                            foreach (string id in ToEmail.Split(new char[] { ';' }))
                            {
                                // send email code
                                if (IQMedia.Shared.Utility.CommonFunctions.SendMail(id, string.Empty, bccEmails, FromEmail, Subject, reportHTML, true, null, alternateViewsName))
                                {
                                    EmailSendCount++;
                                }
                            }
                        }

                        if (chartData.Count > 0)
                        {
                            foreach (string medium in mediums)
                            {
                                if (System.IO.File.Exists(String.Format(TempImagePath, medium)))
                                {
                                    System.IO.File.Delete(String.Format(TempImagePath, medium));
                                }
                            }
                        }
                    }
                    var json = new
                    {
                        isSuccess = true,
                        emailSendCount = EmailSendCount + bccEmails.Count()
                    };
                    return Json(json);
                }
                else
                {
                    var json = new
                    {
                        isSuccess = false,
                        errorMessage = Config.ConfigSettings.Settings.MaxEmailAdressLimitExceeds.Replace("@@MaxLimit@@", System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"])
                    };

                    return Json(json);
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult SaveReportWithSettings(long p_ReportID, List<string> p_ShowHideSettings, string p_SortSettings, Int64? p_ReportImageID, bool p_IsSaveAs, string p_ReportTile, string p_PrimaryGroup, string p_SecondaryGroup, bool p_ResetSort, List<string> p_ChartMediaTypes)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();
                IQ_ReportSettingsModel iQ_ReportSettingsModel = new IQ_ReportSettingsModel();
                iQ_ReportSettingsModel.Sort = p_SortSettings;
                iQ_ReportSettingsModel.ShowAudience = false;
                iQ_ReportSettingsModel.ShowMediaValue = false;
                iQ_ReportSettingsModel.ShowTotalAudience = false;
                iQ_ReportSettingsModel.ShowTotalMediaValue = false;
                iQ_ReportSettingsModel.ShowSentiment = false;
                iQ_ReportSettingsModel.ShowNationalValues = false;
                iQ_ReportSettingsModel.ShowTotalNationalAudience = false;
                iQ_ReportSettingsModel.ShowTotalNationalMediaValue = false;
                iQ_ReportSettingsModel.ShowCoverageSources = false;
                iQ_ReportSettingsModel.PrimaryGroup = p_PrimaryGroup;
                iQ_ReportSettingsModel.SecondaryGroup = p_SecondaryGroup;
                iQ_ReportSettingsModel.ChartMediaTypes = p_ChartMediaTypes;
                if (p_ShowHideSettings != null && p_ShowHideSettings.Count > 0)
                {
                    foreach (string value in p_ShowHideSettings)
                    {
                        Shared.Utility.CommonFunctions.LibraryReportSettings setting = (IQMedia.Shared.Utility.CommonFunctions.LibraryReportSettings)Enum.Parse(typeof(IQMedia.Shared.Utility.CommonFunctions.LibraryReportSettings), value);
                        switch (setting)
                        {
                            case Shared.Utility.CommonFunctions.LibraryReportSettings.Audience:
                                iQ_ReportSettingsModel.ShowAudience = true;
                                break;
                            case Shared.Utility.CommonFunctions.LibraryReportSettings.MediaValue:
                                iQ_ReportSettingsModel.ShowMediaValue = true;
                                break;
                            case Shared.Utility.CommonFunctions.LibraryReportSettings.TotalAudience:
                                iQ_ReportSettingsModel.ShowTotalAudience = true;
                                break;
                            case Shared.Utility.CommonFunctions.LibraryReportSettings.TotalMediaValue:
                                iQ_ReportSettingsModel.ShowTotalMediaValue = true;
                                break;
                            case Shared.Utility.CommonFunctions.LibraryReportSettings.Sentiment:
                                iQ_ReportSettingsModel.ShowSentiment = true;
                                break;
                            case Shared.Utility.CommonFunctions.LibraryReportSettings.NationalValue:
                                iQ_ReportSettingsModel.ShowNationalValues = true;
                                break;
                            case Shared.Utility.CommonFunctions.LibraryReportSettings.TotalNationalAudience :
                                iQ_ReportSettingsModel.ShowTotalNationalAudience = true;
                                break;
                            case Shared.Utility.CommonFunctions.LibraryReportSettings.TotalNationalMediaValue :
                                iQ_ReportSettingsModel.ShowTotalNationalMediaValue = true;
                                break;
                            case Shared.Utility.CommonFunctions.LibraryReportSettings.CoverageSources :
                                iQ_ReportSettingsModel.ShowCoverageSources = true;
                                break;
                        }
                    }
                }

                ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                string result = reportLogic.SaveReportWithSettings(sessionInformation.ClientGUID, p_ReportID, iQ_ReportSettingsModel,p_ReportImageID, p_IsSaveAs, p_ReportTile, p_ResetSort);

                if (Convert.ToInt32(result) > 0)
                {
                    return Json(new
                    {
                        isSuccess = true,
                        reportId = Convert.ToInt32(result)
                    });
                }
                if (result == "-1")
                {
                    return Json(new
                    {
                        isSuccess = false,
                        isDuplicate = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        errorMessage = ConfigSettings.Settings.ErrorOccurred
                    });
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult MergeReports(string reportTitle, List<string> reportIDs, Int64? reportImage, Int64 folderID)
        {
            try
            {
                bool isSuccess = true;
                bool IsDuplicateReport = false;
                bool exceedsMaxLimit = false;

                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                string result = iQArchieveLogic.MergeReports(reportTitle, reportIDs, reportImage, ClientGUID, folderID);
                long intResult;

                if (Int64.TryParse(result, out intResult))
                {
                    if (intResult > 0)
                    {
                        isSuccess = true;
                    }
                    else if (intResult == 0)
                    {
                        isSuccess = true;
                        IsDuplicateReport = true;
                    }
                    else if (intResult == -1)
                    {
                        isSuccess = true;
                        exceedsMaxLimit = true;
                    }
                    else
                    {
                        isSuccess = false;
                    }
                }
                else
                    isSuccess = false;

                var json = new
                {
                    isSuccess = isSuccess,
                    isDuplicate = IsDuplicateReport,
                    exceedsMaxLimit = exceedsMaxLimit
                };
                return Json(json);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                var json = new
                {
                    isSuccess = false
                };
                return Json(json);
            }
        }

        [HttpPost]
        public JsonResult GetMergedReportItemCount(List<string> reportIDs)
        {
            try
            {
                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                string result = iQArchieveLogic.GetMergedReportItemCount(reportIDs);
                long intResult;

                if (Int64.TryParse(result, out intResult))
                {
                    return Json(new
                    {
                        isSuccess = true,
                        itemCount = intResult
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false
                    });
                }
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                var json = new
                {
                    isSuccess = false
                };
                return Json(json);
            }
        }

        [HttpPost]
        public JsonResult SaveReportItemPositions()
        {
            try
            {                
                Request.InputStream.Position = 0;

                Dictionary<string, object> dictParams;

                using (var sr = new StreamReader(Request.InputStream))
                {
                    dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
                }

                long reportID = long.Parse(dictParams["reportID"].ToString());
                JArray reportItems = (JArray)dictParams["reportItems"];
                XDocument xDoc = new XDocument();
                xDoc.Add(new XElement("ReportItems"));

                int position = 1;
                string prevGroupTier1Value = "";
                string prevGroupTier2Value = "";

                foreach (var reportItem in reportItems.Children())
                {
                    var itemProperties = reportItem.Children<JProperty>();
                    string groupTier1Value = itemProperties.FirstOrDefault(x => x.Name == "groupTier1Value").Value.ToString();
                    string groupTier2Value = itemProperties.FirstOrDefault(x => x.Name == "groupTier2Value").Value.ToString();
                    string mediaID = itemProperties.FirstOrDefault(x => x.Name == "mediaID").Value.ToString();

                    if (prevGroupTier1Value != groupTier1Value || (!String.IsNullOrEmpty(groupTier2Value) && prevGroupTier2Value != groupTier2Value))
                    {
                        position = 1;
                    }

                    xDoc.Root.Add(new XElement("ReportItem", new XAttribute("grouptier1value", groupTier1Value), new XAttribute("grouptier2value", groupTier2Value), new XAttribute("mediaid", mediaID), new XAttribute("position", position)));

                    prevGroupTier1Value = groupTier1Value;
                    prevGroupTier2Value = groupTier2Value;
                    position++;
                }

                ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                int retVal = reportLogic.SaveReportItemPositions(reportID, xDoc.ToString());

                return Json(new
                {
                    isSuccess = retVal == 1
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                var json = new
                {
                    isSuccess = false
                };
                return Json(json);
            }
        }

        [HttpPost]
        public JsonResult RevertReportItemPositions(long reportID)
        {
            try
            {
                ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                int retVal = reportLogic.RevertReportItemPositions(reportID);

                return Json(new
                {
                    isSuccess = retVal == 1
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                var json = new
                {
                    isSuccess = false
                };
                return Json(json);
            }
        }

        private string GetCSVDataForReport(IQArchive_DisplayLibraryReport objDisplayLibraryReport)
        {
            if (objDisplayLibraryReport != null && objDisplayLibraryReport.ReportDetails != null && objDisplayLibraryReport.ArchiveResults != null)
            {
                string NotApplicable = "";
                string CompeteValue = "(c)";
                string DQ = "\"";
                string hyperlinkFormat = "=HYPERLINK(\"{0}\")";
                Shared.Utility.CommonFunctions.LibraryTextTypes textType = GetIQCustomSettings().LibraryTextType;
                
                StringBuilder sb = new StringBuilder();
                sessionInformation = ActiveUserMgr.GetActiveUser();

                objDisplayLibraryReport.ArchiveResults = CommonFunctions.SortAndConvertGMTandDSTTime(objDisplayLibraryReport.ArchiveResults, CommonFunctions.ResultType.Library);

                // Determine if there are any results that use article stats (which only the SM data model uses), so that we know whether to include the corresponding columns
                List<string> SMSubMediaTypes = sessionInformation.MediaTypes.Where(w => w.DataModelType == "SM" && w.TypeLevel == 2).Select(s => s.SubMediaType).ToList();
                List<IQArchive_ArchiveSMModel> SMArchiveItems = objDisplayLibraryReport.ArchiveResults.Where(w => SMSubMediaTypes.Contains(w.SubMediaType.ToString())).Select(s => (IQArchive_ArchiveSMModel)s.MediaData).ToList();
                bool useArticleStats = SMArchiveItems.Count(c => c.ArticleStats != null) > 0;

                // Build media item header
                sb.Append("Media Date time,Time Zone,Source,Title,Outlet,DMA,URL,Category,Sub Category 1,Sub Category 2,Sub Category 3" + (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience && (sessionInformation.IsNielsenData || sessionInformation.IsCompeteData) ? ",Audience,Audience Source" : string.Empty) + (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue && (sessionInformation.IsNielsenData || sessionInformation.IsCompeteData) ? ",Media Value ($)" : string.Empty) + (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues && (sessionInformation.IsNielsenData) ? ",National Audience,National Media Value ($)" : string.Empty) + (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience ? ",Twitter Followers" : string.Empty) + ",Twitter Following,Twitter Klout Score" + (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience ? ",Circulation" : string.Empty) + (useArticleStats ? ",Likes,Comments,Shares" : string.Empty) + ",Text");
                sb.Append(Environment.NewLine);

                foreach (IQMedia.Model.IQArchive_MediaModel item in objDisplayLibraryReport.ArchiveResults)
                {
                    IQ_MediaTypeModel objSubMediaType = sessionInformation.MediaTypes.First(w => w.SubMediaType == item.SubMediaType.ToString() && w.TypeLevel == 2);
                    
                    // Set the displayed text as either description or content for every item, and then override with highlighting text if necessary
                    bool displayDescription = item.DisplayDescription || textType == Shared.Utility.CommonFunctions.LibraryTextTypes.Description;
                    string displayText = displayDescription ? item.Description : item.Content;
                    if (displayDescription && String.IsNullOrWhiteSpace(displayText))
                    {
                        displayText = "No Description";
                    }
                    displayText = ProcessCSVText(displayText, item.SubMediaType != Shared.Utility.CommonFunctions.CategoryType.TW);

                    switch (objSubMediaType.DataModelType)
                    {
                        case "TV":

                            IQArchive_ArchiveClipModel tvModel = item.MediaData as IQArchive_ArchiveClipModel;

                            // If there is no highlighting text, display content
                            if (!displayDescription && textType == Shared.Utility.CommonFunctions.LibraryTextTypes.HighlightingText && tvModel.HighlightedOutput != null && tvModel.HighlightedOutput.CC != null && tvModel.HighlightedOutput.CC.Count() > 0)
                            {
                                displayText = string.Join(" ", tvModel.HighlightedOutput.CC.Select(c => c.Text));
                                displayText = ProcessCSVText(displayText, true);
                            }

                            // Append Media Date
                            if (tvModel.LocalDateTime != null)
                            {
                                sb.Append((item.MediaData as IQArchive_ArchiveClipModel).LocalDateTime);
                            }
                            else
                            {
                                sb.Append(string.Empty);
                            }
                            sb.Append(",");

                            // Append TimeZone
                            sb.Append(tvModel.TimeZone);
                            sb.Append(",");

                            // Append Source
                            sb.Append(objSubMediaType.DisplayName);
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + item.Title + DQ);
                            sb.Append(",");

                            // Append Station Call Sign
                            sb.Append(DQ + tvModel.Station_Call_Sign + DQ);
                            sb.Append(",");

                            // Append DMA
                            sb.Append(DQ + tvModel.Market + DQ);
                            sb.Append(",");

                            // Append URL
                            sb.Append(String.Format(hyperlinkFormat, ConfigurationManager.AppSettings["ClipPlayerURL"] + tvModel.ClipID));
                            sb.Append(",");

                            // Append Categories
                            sb.Append(DQ + item.CategoryName + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory1Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory2Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory3Name + DQ);
                            sb.Append(",");

                            if (sessionInformation.IsNielsenData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    // Audience
                                    sb.Append(tvModel.Nielsen_Audience.HasValue ? tvModel.Nielsen_Audience.Value.ToString() : string.Empty);
                                    sb.Append(",");
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    // iqMediaValue
                                    sb.Append(tvModel.IQAdShareValue);
                                    //sb.Append(tvModel.IQAdShareValue.HasValue ? (!string.IsNullOrWhiteSpace(tvModel.Nielsen_Result) ? "(" + tvModel.Nielsen_Result.ToUpper() + ")" : string.Empty) : string.Empty);
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues)
                                {
                                    sb.Append(tvModel.National_Nielsen_Audience);
                                    sb.Append(",");

                                    sb.Append(tvModel.National_IQAdShareValue);
                                    sb.Append(",");
                                }
                            }
                            else if (sessionInformation.IsCompeteData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    sb.Append(string.Empty);
                                    sb.Append(",");
                                    sb.Append(",");
                                }
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    sb.Append(string.Empty);
                                    sb.Append(",");
                                }
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues)
                                {
                                    sb.Append(string.Empty);
                                    sb.Append(",");
                                    sb.Append(string.Empty);
                                    sb.Append(",");
                                }
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Folllowers Count
                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            // Friends Count
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Klout Score
                            sb.Append(string.Empty);
                            sb.Append(",");

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Circulation
                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            if (useArticleStats)
                            {
                                sb.Append(",,,");
                            }

                            // Text
                            sb.Append(DQ + displayText + DQ);


                            break;

                        case "TW":

                            IQArchive_ArchiveTweetsModel twitterModel = item.MediaData as IQArchive_ArchiveTweetsModel;

                            // If there is no highlighting text, display content
                            if (!displayDescription && textType == Shared.Utility.CommonFunctions.LibraryTextTypes.HighlightingText && twitterModel.HighlightedOutput != null && twitterModel.HighlightedOutput.Highlights != null && twitterModel.HighlightedOutput.Highlights.Count() > 0)
                            {
                                displayText = twitterModel.HighlightedOutput.Highlights;
                                displayText = displayText.Replace("\r\n", " ").Replace("\"", "\"\"");
                            }

                            // Append Media Date
                            if (item.MediaDate != null)
                            {
                                sb.Append(item.MediaDate.ToString());
                            }
                            else
                            {
                                sb.Append(string.Empty);
                            }
                            sb.Append(",");

                            // Append TimeZone
                            sb.Append(sessionInformation.TimeZone);
                            sb.Append(",");

                            // Append Source
                            sb.Append(objSubMediaType.DisplayName);
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + item.Title + DQ);
                            sb.Append(",");

                            // Append Publication
                            sb.Append(DQ + twitterModel.ActorDisplayname + DQ);
                            sb.Append(",");

                            // Append DMA
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Append URL
                            if (!String.IsNullOrEmpty(twitterModel.ActorLink) && twitterModel.TweetID > 0) ;
                            sb.Append(String.Format(hyperlinkFormat, twitterModel.ActorLink.Replace(",", "%2c") + "/status/" + twitterModel.TweetID));
                            sb.Append(",");

                            // Append Categories
                            sb.Append(DQ + item.CategoryName + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory1Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory2Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory3Name + DQ);
                            sb.Append(",");

                            if (sessionInformation.IsNielsenData || sessionInformation.IsCompeteData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    // Append Audience
                                    sb.Append(string.Empty);
                                    sb.Append(",");
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    // Append Media Value
                                    sb.Append(string.Empty);
                                    sb.Append(",");
                                }
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues && sessionInformation.IsNielsenData)
                            {
                                sb.Append(string.Empty);
                                sb.Append(",");

                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Folllowers Count
                                sb.Append(twitterModel.FollowersCount);
                                sb.Append(",");
                            }

                            // Friends Count
                            sb.Append(twitterModel.FreiendsCount);
                            sb.Append(",");

                            // Klout Score
                            sb.Append(twitterModel.KloutScore);
                            sb.Append(",");

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Circulation
                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            if (useArticleStats)
                            {
                                sb.Append(",,,");
                            }

                            // Text
                            sb.Append(DQ + displayText + DQ);

                            break;

                        case "PM":

                            IQArchive_ArchiveBLPMModel pmModel = item.MediaData as IQArchive_ArchiveBLPMModel;

                            // Append Media Date
                            if (item.MediaDate != null)
                            {
                                sb.Append(item.MediaDate.ToString());
                            }
                            else
                            {
                                sb.Append(string.Empty);
                            }
                            sb.Append(",");

                            // Append TimeZone
                            sb.Append(sessionInformation.TimeZone);
                            sb.Append(",");

                            // Append Source
                            sb.Append(objSubMediaType.DisplayName);
                            sb.Append(",");

                            // Append Title
                            sb.Append(item.Title);
                            sb.Append(",");

                            // Append Publication
                            sb.Append(pmModel.Pub_Name);
                            sb.Append(",");

                            // Append DMA
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Append URL
                            sb.Append(String.Format(hyperlinkFormat, Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IQArchieve_PMBaseUrl"]) + pmModel.FileLocation.Replace(@"\", @"/").Replace(",", "%2c")));
                            sb.Append(",");

                            // Append Categories
                            sb.Append(DQ + item.CategoryName + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory1Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory2Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory3Name + DQ);
                            sb.Append(",");

                            if (sessionInformation.IsNielsenData || sessionInformation.IsCompeteData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    // Append Audience
                                    sb.Append(string.Empty);
                                    sb.Append(",");
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    // Append Media Value
                                    sb.Append(string.Empty);
                                    sb.Append(",");
                                }
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues && sessionInformation.IsNielsenData)
                            {
                                sb.Append(string.Empty);
                                sb.Append(",");

                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Folllowers Count
                                sb.Append(String.Empty);
                                sb.Append(",");
                            }

                            // Friends Count
                            sb.Append(String.Empty);
                            sb.Append(",");

                            // Klout Score
                            sb.Append(String.Empty);
                            sb.Append(",");

                            // Circulation
                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                sb.Append(pmModel.Circulation);
                                sb.Append(","); 
                            }

                            if (useArticleStats)
                            {
                                sb.Append(",,,");
                            }

                            // Text
                            sb.Append(DQ + displayText + DQ);

                            break;

                        case "NM":

                            IQArchive_ArchiveNMModel newsModel = item.MediaData as IQArchive_ArchiveNMModel;

                            // If there is no highlighting text, display content
                            if (!displayDescription && textType == Shared.Utility.CommonFunctions.LibraryTextTypes.HighlightingText && newsModel.HighlightedOutput != null && newsModel.HighlightedOutput.Highlights != null && newsModel.HighlightedOutput.Highlights.Count() > 0)
                            {
                                displayText = string.Join(" ", newsModel.HighlightedOutput.Highlights.Select(c => c));
                                displayText = ProcessCSVText(displayText, true);
                            }

                            // Append Media Date
                            if (item.MediaDate != null)
                            {
                                sb.Append(item.MediaDate.ToString());
                            }
                            else
                            {
                                sb.Append(string.Empty);
                            }
                            sb.Append(",");

                            // Append TimeZone
                            sb.Append(sessionInformation.TimeZone);
                            sb.Append(",");

                            // Append Source
                            if (newsModel.IQLicense == 3)
                            {
                                sb.Append("LexisNexis(R)");
                            }
                            else
                            {
                                sb.Append(objSubMediaType.DisplayName);
                            }
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + HttpUtility.HtmlDecode(item.Title) + DQ);
                            sb.Append(",");

                            // Append Publication
                            sb.Append(DQ + newsModel.Publication + DQ);
                            sb.Append(",");

                            // Append DMA
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Append URL
                            string nmUrl = newsModel.IQLicense > 0 ? "http://" + Request.ServerVariables["HTTP_HOST"] + Url.Action("Index", "Article", new { au = Shared.Utility.CommonFunctions.EncryptLicenseStringAES(sessionInformation.CustomerKey + "¶Library CSV¶" + newsModel.Url + "&u1=cliq40&u2=" + sessionInformation.ClientID + "¶" + newsModel.IQLicense) }) : newsModel.Url;
                            if (!String.IsNullOrEmpty(nmUrl))
                            {
                                sb.Append(String.Format(hyperlinkFormat, nmUrl.Replace(",", "%2c")));
                            }
                            sb.Append(",");

                            // Append Categories
                            sb.Append(DQ + item.CategoryName + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory1Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory2Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory3Name + DQ);
                            sb.Append(",");


                            if (Decimal.Compare(Convert.ToDecimal(newsModel.IQAdShareValue), -1M) != 0 && newsModel.Compete_Audience != -1 && sessionInformation.IsCompeteData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    // Audience
                                    if (newsModel.Compete_Audience.HasValue)
                                    {
                                        sb.Append(newsModel.Compete_Audience.Value.ToString());
                                    }
                                    else
                                    {
                                        sb.Append(NotApplicable);
                                    }
                                    sb.Append(",");

                                    if (!string.IsNullOrWhiteSpace(newsModel.Compete_Result) && newsModel.Compete_Result.ToUpper() == "A")
                                    {
                                        sb.Append(CompeteValue);
                                    }
                                    sb.Append(",");
                                }


                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    // iQ Media value
                                    if (newsModel.IQAdShareValue.HasValue)
                                    {
                                        sb.Append(newsModel.IQAdShareValue);
                                    }
                                    else
                                    {
                                        sb.Append(NotApplicable);
                                    }
                                    sb.Append(",");
                                }
                            }
                            else if(sessionInformation.IsNielsenData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    sb.Append(",");
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    sb.Append(",");
                                }
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues && sessionInformation.IsNielsenData)
                            {
                                sb.Append(string.Empty);
                                sb.Append(",");

                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Twitter Followers
                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            // Twitter Following
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Klout Score
                            sb.Append(string.Empty);
                            sb.Append(",");

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Circulation
                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            if (useArticleStats)
                            {
                                sb.Append(",,,");
                            }

                            // Text
                            sb.Append(DQ + displayText + DQ);


                            break;

                        case "SM":
                            IQArchive_ArchiveSMModel socialModel = item.MediaData as IQArchive_ArchiveSMModel;

                            // If there is no highlighting text, display content
                            if (!displayDescription && textType == Shared.Utility.CommonFunctions.LibraryTextTypes.HighlightingText && socialModel.HighlightedOutput != null && socialModel.HighlightedOutput.Highlights != null && socialModel.HighlightedOutput.Highlights.Count() > 0)
                            {
                                displayText = string.Join(" ", socialModel.HighlightedOutput.Highlights.Select(c => c));
                                displayText = ProcessCSVText(displayText, true);
                            }

                            // Append Media Date
                            if (item.MediaDate != null)
                            {
                                sb.Append(item.MediaDate.ToString());
                            }
                            else
                            {
                                sb.Append(string.Empty);
                            }
                            sb.Append(",");

                            // Append TimeZone
                            sb.Append(sessionInformation.TimeZone);
                            sb.Append(",");

                            // Append Source
                            sb.Append(objSubMediaType.DisplayName);
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + HttpUtility.HtmlDecode(item.Title) + DQ);
                            sb.Append(",");

                            // Append Publication
                            sb.Append(DQ + socialModel.Publication + DQ);
                            sb.Append(",");

                            // Append DMA
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Append URL
                            if (!String.IsNullOrEmpty(socialModel.Url))
                            {
                                sb.Append(String.Format(hyperlinkFormat, socialModel.Url.Replace(",", "%2c")));
                            }
                            sb.Append(",");

                            // Append Categories
                            sb.Append(DQ + item.CategoryName + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory1Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory2Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory3Name + DQ);
                            sb.Append(",");

                            if (Decimal.Compare(Convert.ToDecimal(socialModel.IQAdShareValue), -1M) != 0 && socialModel.Compete_Audience != -1 && sessionInformation.IsCompeteData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    // Audience
                                    if (socialModel.Compete_Audience.HasValue)
                                    {
                                        sb.Append(socialModel.Compete_Audience.Value.ToString());
                                    }
                                    else
                                    {
                                        sb.Append(NotApplicable);
                                    }
                                    sb.Append(",");

                                    if (!string.IsNullOrWhiteSpace(socialModel.Compete_Result) && socialModel.Compete_Result.ToUpper() == "A")
                                    {
                                        sb.Append(CompeteValue);
                                    }
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    // iQ Media value
                                    if (socialModel.IQAdShareValue.HasValue)
                                    {
                                        sb.Append(socialModel.IQAdShareValue);
                                    }
                                    else
                                    {
                                        sb.Append(NotApplicable);
                                    }
                                    sb.Append(",");
                                }
                            }
                            else if(sessionInformation.IsNielsenData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    sb.Append(",");
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    sb.Append(",");
                                }
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues && sessionInformation.IsNielsenData)
                            {
                                sb.Append(string.Empty);
                                sb.Append(",");

                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Twitter Followers
                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            // Twitter Following
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Klout Score
                            sb.Append(string.Empty);
                            sb.Append(",");

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Circulation
                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            if (useArticleStats)
                            {
                                if (socialModel.ArticleStats != null)
                                {
                                    sb.Append(socialModel.ArticleStats.Likes.ToString());
                                    sb.Append(",");

                                    sb.Append(socialModel.ArticleStats.Comments.ToString());
                                    sb.Append(",");

                                    sb.Append(socialModel.ArticleStats.Shares.ToString());
                                    sb.Append(",");
                                }
                                else
                                {
                                    sb.Append(",,,");
                                }
                            }

                            // Text
                            sb.Append(DQ + displayText + DQ);

                            break;

                        case "TM":

                            IQArchive_ArchiveTVEyesModel tvEyesModel = item.MediaData as IQArchive_ArchiveTVEyesModel;

                            // Append Media Date
                            if (tvEyesModel.LocalDateTime != null)
                            {
                                sb.Append(tvEyesModel.LocalDateTime);
                            }
                            else
                            {
                                sb.Append(string.Empty);
                            }
                            sb.Append(",");

                            // Append TimeZone
                            sb.Append(tvEyesModel.TimeZone);
                            sb.Append(",");

                            // Append Source
                            sb.Append(objSubMediaType.DisplayName);
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + item.Title + DQ);
                            sb.Append(",");

                            // Append Station Call Sign
                            sb.Append(DQ + tvEyesModel.StationID + DQ);
                            sb.Append(",");

                            // Append DMA
                            sb.Append(DQ + tvEyesModel.Market + DQ);
                            sb.Append(",");

                            // Append URL
                            string radioUrl = ConfigurationManager.AppSettings["RadioClipPlayerURL"] + Url.Encode(IQMedia.Shared.Utility.CommonFunctions.GenerateRandomString() + IQMedia.Shared.Utility.CommonFunctions.EncryptStringAES(item.ID.ToString(), IQMedia.Shared.Utility.CommonFunctions.AesKeyLibRadioPlayer, IQMedia.Shared.Utility.CommonFunctions.AesIVLibRadioPlayer));
                            sb.Append(String.Format(hyperlinkFormat, radioUrl.Replace(",", "%2c")));
                            sb.Append(",");

                            // Append Categories
                            sb.Append(DQ + item.CategoryName + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory1Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory2Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory3Name + DQ);
                            sb.Append(",");

                            if (sessionInformation.IsNielsenData || sessionInformation.IsCompeteData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    // Audience
                                    sb.Append(string.Empty);
                                    sb.Append(",");
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    // iqMediaValue
                                    sb.Append(string.Empty);
                                    //sb.Append(tvModel.IQAdShareValue.HasValue ? (!string.IsNullOrWhiteSpace(tvModel.Nielsen_Result) ? "(" + tvModel.Nielsen_Result.ToUpper() + ")" : string.Empty) : string.Empty);
                                    sb.Append(",");
                                }
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues && sessionInformation.IsNielsenData)
                            {
                                sb.Append(string.Empty);
                                sb.Append(",");

                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                            {
                                // Folllowers Count
                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            // Friends Count
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Klout Score
                            sb.Append(string.Empty);
                            sb.Append(",");

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Circulation
                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            if (useArticleStats)
                            {
                                sb.Append(",,,");
                            }

                            // Text
                            sb.Append(DQ + displayText + DQ);


                            break;

                        case "MS":

                            IQArchive_ArchiveMiscModel miscModel = item.MediaData as IQArchive_ArchiveMiscModel;
                            
                            // Append Media Date
                            sb.Append(miscModel.CreateDT);
                            sb.Append(",");

                            // Append TimeZone
                            sb.Append(miscModel.TimeZone);
                            sb.Append(",");

                            // Append Source
                            sb.Append(objSubMediaType.DisplayName);
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + item.Title + "." + miscModel.FileTypeExt + DQ);
                            sb.Append(",");

                            // Append Station Call Sign
                            sb.Append(",");

                            // Append DMA
                            sb.Append(",");

                            // Append URL
                            sb.Append(String.Format(hyperlinkFormat, miscModel.MediaUrl));
                            sb.Append(",");

                            // Append Categories
                            sb.Append(DQ + item.CategoryName + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory1Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory2Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory3Name + DQ);
                            sb.Append(",");

                            if (sessionInformation.IsNielsenData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    // Audience
                                    sb.Append(",");
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    // iqMediaValue
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues)
                                {
                                    sb.Append(",");
                                    sb.Append(",");
                                }
                            }
                            else if (sessionInformation.IsCompeteData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    sb.Append(",");
                                    sb.Append(",");
                                }
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    sb.Append(",");
                                }
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues)
                                {
                                    sb.Append(",");
                                    sb.Append(",");
                                }
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Folllowers Count
                                sb.Append(",");
                            }

                            // Friends Count
                            sb.Append(",");

                            // Klout Score
                            sb.Append(",");

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Circulation
                                sb.Append(",");
                            }

                            if (useArticleStats)
                            {
                                sb.Append(",,,");
                            }

                            // Text
                            sb.Append(DQ + displayText + DQ);

                            break;
                        case "PQ":

                            IQArchive_ArchivePQModel pqModel = item.MediaData as IQArchive_ArchivePQModel;

                            // If there is no highlighting text, display content
                            if (!displayDescription && textType == Shared.Utility.CommonFunctions.LibraryTextTypes.HighlightingText && pqModel.HighlightedOutput != null && pqModel.HighlightedOutput.Highlights != null && pqModel.HighlightedOutput.Highlights.Count() > 0)
                            {
                                displayText = string.Join(" ", pqModel.HighlightedOutput.Highlights.Select(c => c));
                                displayText = ProcessCSVText(displayText, true);
                            }
                            
                            // Append Media Date
                            if (item.MediaDate != null)
                            {
                                sb.Append(item.MediaDate.ToString());
                            }
                            else
                            {
                                sb.Append(string.Empty);
                            }
                            sb.Append(",");

                            // Append TimeZone
                            sb.Append(sessionInformation.TimeZone);
                            sb.Append(",");

                            // Append Source
                            sb.Append(objSubMediaType.DisplayName);
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + item.Title + DQ);
                            sb.Append(",");

                            // Append Publication
                            sb.Append(DQ + pqModel.Publication + DQ);
                            sb.Append(",");

                            // Append DMA
                            sb.Append(",");

                            // Append URL
                            sb.Append(String.Format(hyperlinkFormat, String.Format(ConfigurationManager.AppSettings["ProQuestURL"], "library", item.ID)));
                            sb.Append(",");

                            // Append Categories
                            sb.Append(DQ + item.CategoryName + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory1Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory2Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory3Name + DQ);
                            sb.Append(",");

                            if (sessionInformation.IsNielsenData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    // Audience
                                    sb.Append(",");
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    // Media Value
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues)
                                {
                                    sb.Append(",");
                                    sb.Append(",");
                                }
                            }
                            else if (sessionInformation.IsCompeteData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    sb.Append(",");
                                    sb.Append(",");
                                }
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    sb.Append(",");
                                }
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues)
                                {
                                    sb.Append(",");
                                    sb.Append(",");
                                }
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Folllowers Count
                                sb.Append(",");
                            }

                            // Friends Count
                            sb.Append(",");

                            // Klout Score
                            sb.Append(",");

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Circulation
                                sb.Append(",");
                            }

                            if (useArticleStats)
                            {
                                sb.Append(",,,");
                            }

                            // Text
                            sb.Append(DQ + displayText + DQ);

                            break;
                        case "IQR":

                            IQArchive_ArchiveRadioModel radioModel = item.MediaData as IQArchive_ArchiveRadioModel;

                            // If there is no highlighting text, display content
                            if (!displayDescription && textType == Shared.Utility.CommonFunctions.LibraryTextTypes.HighlightingText && radioModel.HighlightedOutput != null && radioModel.HighlightedOutput.CC != null && radioModel.HighlightedOutput.CC.Count() > 0)
                            {
                                displayText = string.Join(" ", radioModel.HighlightedOutput.CC.Select(c => c.Text));
                                displayText = ProcessCSVText(displayText, true);
                            }

                            // Append Media Date
                            if (radioModel.LocalDateTime != null)
                            {
                                sb.Append(radioModel.LocalDateTime);
                            }
                            else
                            {
                                sb.Append(string.Empty);
                            }
                            sb.Append(",");

                            // Append TimeZone
                            sb.Append(radioModel.TimeZone);
                            sb.Append(",");

                            // Append Source
                            sb.Append(objSubMediaType.DisplayName);
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + item.Title + DQ);
                            sb.Append(",");

                            // Append Station Call Sign
                            sb.Append(DQ + radioModel.StationID + DQ);
                            sb.Append(",");

                            // Append DMA
                            sb.Append(DQ + radioModel.Market + DQ);
                            sb.Append(",");

                            // Append URL
                            sb.Append(String.Format(hyperlinkFormat, ConfigurationManager.AppSettings["ClipPlayerURL"] + radioModel.ClipGuid));
                            sb.Append(",");

                            // Append Categories
                            sb.Append(DQ + item.CategoryName + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory1Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory2Name + DQ);
                            sb.Append(",");
                            sb.Append(DQ + item.SubCategory3Name + DQ);
                            sb.Append(",");

                            if (sessionInformation.IsNielsenData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    // Audience
                                    sb.Append(",");
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    // Media Value
                                    sb.Append(",");
                                }

                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues)
                                {
                                    sb.Append(",");
                                    sb.Append(",");
                                }
                            }
                            else if (sessionInformation.IsCompeteData)
                            {
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                                {
                                    sb.Append(",");
                                    sb.Append(",");
                                }
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowMediaValue)
                                {
                                    sb.Append(",");
                                }
                                if (objDisplayLibraryReport.ReportDetails.Settings.ShowNationalValues)
                                {
                                    sb.Append(",");
                                    sb.Append(",");
                                }
                            }

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Folllowers Count
                                sb.Append(",");
                            }

                            // Friends Count
                            sb.Append(",");

                            // Klout Score
                            sb.Append(",");

                            if (objDisplayLibraryReport.ReportDetails.Settings.ShowAudience)
                            {
                                // Circulation
                                sb.Append(",");
                            }

                            if (useArticleStats)
                            {
                                sb.Append(",,,");
                            }

                            // Text
                            sb.Append(DQ + displayText + DQ);

                            break;
                        default:
                            sb.Append(",");
                            sb.Append(",");
                            break;
                    }

                    sb.Append(Environment.NewLine);
                }
                // }
                //}

                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private string ProcessCSVText(string text, bool truncateText)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("&lt;", "<").Replace("&gt;", ">");
                if (truncateText && text.Length > 255)
                {
                    int IndexAfter255Char = text.Substring(255).IndexOfAny(new[] { ' ', '\t', '\n', '\r' });
                    text = IndexAfter255Char == -1 ? text.Substring(0, text.Length) : text.Substring(0, 255 + IndexAfter255Char) + "...";
                }
                text = !string.IsNullOrEmpty(text) ? text.Replace("\"", "\"\"") : string.Empty;
            }

            return text;
        }

        private void CreateChartAttachmentForEmail(string chartHTML, string tempImagePath)
        {
            StringBuilder cssData = new StringBuilder();

            StreamReader strmReader = new StreamReader(Server.MapPath("~/css/Dashboard.css"));
            cssData.Append(strmReader.ReadToEnd());
            strmReader.Close();
            strmReader.Dispose();

            strmReader = new StreamReader(Server.MapPath("~/css/feed.css"));
            cssData.Append(strmReader.ReadToEnd());
            strmReader.Close();
            strmReader.Dispose();

            strmReader = new StreamReader(Server.MapPath("~/css/bootstrap.css"));
            cssData.Append(strmReader.ReadToEnd());
            strmReader.Close();
            strmReader.Dispose();

            cssData.Append(" .divSentimentNeg{width:25px;} \n body{background:none;}");

            chartHTML = "<html><head><style type=\"text/css\">" + Convert.ToString(cssData) + "</style></head>" + "<body>" + chartHTML + "</body></html>";

            HtmlToImage htmlToImageConverter = new HtmlToImage();
            htmlToImageConverter.SerialNumber = ConfigurationManager.AppSettings["HiQPdfSerialKey"];
            htmlToImageConverter.BrowserWidth = 1000;
            Image img = htmlToImageConverter.ConvertHtmlToImage(chartHTML, String.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority))[0];

            img.Save(tempImagePath);
            img.Dispose();
        }

        #endregion

        #region UGC

        [HttpPost]
        public JsonResult BindCustomCategoryDropDown()
        {
            try
            {
                CustomCategoryLogic customCategoryLogic = (CustomCategoryLogic)LogicFactory.GetLogic(LogicType.Category);
                List<CustomCategoryModel> customCategoryModelList = customCategoryLogic.GetCustomCategory(new Guid(ClientGUID)).ToList();

                return Json(new
                {
                    customCategories = customCategoryModelList,
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
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult UGCUploadContent(string txtUGCSelectedFileDisplay, bool chkAutoClip, string txtUGCTitle, string txtUGCKeywords, string ddlUGCCategory, string ddlUGCSubCategory1,
                                        string ddlUGCSubCategory2, string ddlUGCSubCategory3, string dpUGCAirDateTime, string ddlUGCtimeZone, string txtUGCDescription)
        {
            bool IsSuccess = true;
            string ErrorMessage = string.Empty;
            try
            {
                Dictionary<string, string> _ugcFileTypes = (Dictionary<string, string>)TempData["UGCDocument_FileTypes"];
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                if (sessionInformation.Isv4UGCAccess)
                {
                    if (!string.IsNullOrWhiteSpace(txtUGCSelectedFileDisplay))
                    {
                        if (_ugcFileTypes.ContainsKey(Path.GetExtension(txtUGCSelectedFileDisplay).Replace(".","").ToLower()))
                        {
                            string newFileName = string.Empty;
                            newFileName = Regex.Replace(txtUGCSelectedFileDisplay, @"[\s\\/]", "_");
                            newFileName = Path.GetFileNameWithoutExtension(newFileName) + DateTime.Now.ToString("MMddyyyy_hhmmss") + Path.GetExtension(newFileName);

                            if (_ugcFileTypes[Path.GetExtension(txtUGCSelectedFileDisplay).Replace(".", "").ToLower()] ==Shared.Utility.CommonFunctions.UGCMediaType)
                            {
                                bool IsFileMovedSuccessfully = MoveUGCFTPFile(txtUGCSelectedFileDisplay, newFileName);
                                if (IsFileMovedSuccessfully)
                                {
                                    IsSuccess = CreateUGCContentXML(newFileName, chkAutoClip, txtUGCTitle, txtUGCKeywords, ddlUGCCategory, ddlUGCSubCategory1, ddlUGCSubCategory2,
                                                                   ddlUGCSubCategory3, dpUGCAirDateTime, ddlUGCtimeZone, txtUGCDescription);
                                    //CallUGSFileUploadWebServiceAsync(newFileName);

                                }
                                else
                                {
                                    IsSuccess = false;
                                    ErrorMessage = ConfigSettings.Settings.ErrorWhileMovingFtp;
                                }
                            }
                            else
                            {
                                DateTime airDate= Convert.ToDateTime(dpUGCAirDateTime);
                                string location =   "\\" + airDate.Year + "\\" + airDate.Month + "\\" + airDate.Day + "\\" + newFileName;
                                bool IsFileMovedSuccessfully = MoveUGCFTPDocumentFile(txtUGCSelectedFileDisplay, location);
                                if (IsFileMovedSuccessfully)
                                {
                                    IQUGCArchiveLogic iQUGCArchiveLogic = (IQUGCArchiveLogic)LogicFactory.GetLogic(LogicType.IQUGCArchive);

                                    Guid categoryGuid = new Guid(ddlUGCCategory);
                                    Guid? subcategory1Guid = !string.IsNullOrEmpty(ddlUGCSubCategory1) ? (Guid?)new Guid(ddlUGCSubCategory1) : null;
                                    Guid? subcategory2Guid = !string.IsNullOrEmpty(ddlUGCSubCategory2) ? (Guid?)new Guid(ddlUGCSubCategory2) : null;
                                    Guid? subcategory3Guid = !string.IsNullOrEmpty(ddlUGCSubCategory3) ? (Guid?)new Guid(ddlUGCSubCategory3) : null;
                                    string res = iQUGCArchiveLogic.InsertIQUGCArchiveDocument(categoryGuid, subcategory1Guid, subcategory2Guid, subcategory3Guid, txtUGCTitle, txtUGCKeywords, txtUGCDescription, dpUGCAirDateTime, ddlUGCtimeZone, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, Path.GetExtension(txtUGCSelectedFileDisplay).Replace(".",""), Convert.ToInt32(TempData["UGCDocument_RootPathID"]), location);
                                    if (!string.IsNullOrEmpty(res) && Convert.ToInt32(res) > 0)
                                    {
                                        IsSuccess = true;
                                    }
                                    else
                                    {
                                        IsSuccess = false;
                                        ErrorMessage = ConfigSettings.Settings.ErrorWhileMovingFtp;
                                    }
                                }
                                else
                                {
                                    IsSuccess = false;
                                    ErrorMessage = ConfigSettings.Settings.ErrorWhileMovingFtp;
                                }
                            }
                        }
                        else
                        {
                            IsSuccess = false;
                            ErrorMessage = ConfigSettings.Settings.UGCInvalidFileUploadError;
                        }
                    }
                    else
                    {
                        if (Request.Files.Count > 0)
                        {
                            if (_ugcFileTypes.ContainsKey(Path.GetExtension(Request.Files[0].FileName).Replace(".", "").ToLower()))
                            {
                                string fileName = Path.GetFileName(Request.Files[0].FileName);
                                fileName = Regex.Replace(fileName, @"[\s\\/]", "_");
                                fileName = Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString("MMddyyyy_hhmmss") + Path.GetExtension(fileName);
                                if (_ugcFileTypes[Path.GetExtension(Request.Files[0].FileName).Replace(".", "").ToLower()] == Shared.Utility.CommonFunctions.UGCMediaType)
                                {
                                    bool saveFileSuccessfully = SaveUGCContent(fileName);
                                    if (saveFileSuccessfully)
                                    {
                                        IsSuccess = CreateUGCContentXML(fileName, chkAutoClip, txtUGCTitle, txtUGCKeywords, ddlUGCCategory, ddlUGCSubCategory1, ddlUGCSubCategory2,
                                                                    ddlUGCSubCategory3, dpUGCAirDateTime, ddlUGCtimeZone, txtUGCDescription);
                                        //CallUGSFileUploadWebServiceAsync(fileName);
                                    }
                                    else
                                    {
                                        IsSuccess = false;
                                        ErrorMessage = ConfigSettings.Settings.ErrorSavingFile;
                                    }
                                }
                                else
                                {
                                    DateTime airDate = Convert.ToDateTime(dpUGCAirDateTime);
                                    string location = "\\" + airDate.Year + "\\" + airDate.Month + "\\" + airDate.Day + "\\" + fileName;

                                    if (!Directory.Exists(Path.GetDirectoryName(TempData["UGCDocument_StoragePath"] + location)))
                                    {
                                        Directory.CreateDirectory(Path.GetDirectoryName(TempData["UGCDocument_StoragePath"] + location));
                                    }

                                    Request.Files[0].SaveAs(TempData["UGCDocument_StoragePath"] + location);

                                    Guid categoryGuid = new Guid(ddlUGCCategory);
                                    Guid? subcategory1Guid = !string.IsNullOrEmpty(ddlUGCSubCategory1) ? (Guid?)new Guid(ddlUGCSubCategory1) : null;
                                    Guid? subcategory2Guid = !string.IsNullOrEmpty(ddlUGCSubCategory2) ? (Guid?)new Guid(ddlUGCSubCategory2) : null;
                                    Guid? subcategory3Guid = !string.IsNullOrEmpty(ddlUGCSubCategory3) ? (Guid?)new Guid(ddlUGCSubCategory3) : null;
                                    IQUGCArchiveLogic iQUGCArchiveLogic = (IQUGCArchiveLogic)LogicFactory.GetLogic(LogicType.IQUGCArchive);
                                    string res = iQUGCArchiveLogic.InsertIQUGCArchiveDocument(categoryGuid, subcategory1Guid, subcategory2Guid, subcategory3Guid, txtUGCTitle, txtUGCKeywords, txtUGCDescription, dpUGCAirDateTime, ddlUGCtimeZone, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, Path.GetExtension(fileName).Replace(".", ""), Convert.ToInt32(TempData["UGCDocument_RootPathID"]), location);
                                    if (!string.IsNullOrEmpty(res ) && Convert.ToInt32(res) > 0)
                                    {
                                        IsSuccess = true;
                                    }
                                    else
                                    {
                                        IsSuccess = false;
                                        ErrorMessage = ConfigSettings.Settings.ErrorSavingFile;
                                    }
                                }
                            }
                            else
                            {
                                IsSuccess = false;
                                ErrorMessage = ConfigSettings.Settings.UGCInvalidFileUploadError;
                            }
                        }
                        else
                        {
                            IsSuccess = false;
                            ErrorMessage = ConfigSettings.Settings.ErrorSavingFile;
                        }
                    }
                }
                else
                {
                    IsSuccess = false;
                    ErrorMessage = ConfigSettings.Settings.UnauthorizedAccess;
                }

                return Json(new { isSuccess = IsSuccess, errorMsg = ErrorMessage });
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
                return Json(new { isSuccess = false, errorMsg = ConfigSettings.Settings.ErrorOccurred });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public string UGCUploadContentIE(string chkAutoClip, string hdnUGCSelectedFileDisplay, string txtUGCTitle, string txtUGCKeywords, string ddlUGCCategory, string ddlUGCSubCategory1,
                                        string ddlUGCSubCategory2, string ddlUGCSubCategory3, string dpUGCAirDateTime, string ddlUGCtimeZone, string txtUGCDescription)
        {
            bool IsSuccess = true;
            string ErrorMessage = string.Empty;
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                Dictionary<string, string> _ugcFileTypes = (Dictionary<string, string>)TempData["UGCDocument_FileTypes"];
                if (sessionInformation.Isv4UGCAccess)
                {
                    if (!string.IsNullOrWhiteSpace(hdnUGCSelectedFileDisplay))
                    {
                        if (_ugcFileTypes.ContainsKey(Path.GetExtension(hdnUGCSelectedFileDisplay).Replace(".", "").ToLower()))
                        {
                            string newFileName = string.Empty;
                            newFileName = Regex.Replace(hdnUGCSelectedFileDisplay, @"[\s\\/]", "_");
                            newFileName = Path.GetFileNameWithoutExtension(hdnUGCSelectedFileDisplay) + DateTime.Now.ToString("MMddyyyy_hhmmss") + Path.GetExtension(hdnUGCSelectedFileDisplay);

                            if (_ugcFileTypes[Path.GetExtension(hdnUGCSelectedFileDisplay).Replace(".", "").ToLower()] == Shared.Utility.CommonFunctions.UGCMediaType)
                            {
                                bool IsFileMovedSuccessfully = MoveUGCFTPFile(hdnUGCSelectedFileDisplay, newFileName);
                                if (IsFileMovedSuccessfully)
                                {
                                    IsSuccess = CreateUGCContentXML(newFileName, !string.IsNullOrEmpty(chkAutoClip), txtUGCTitle, txtUGCKeywords, ddlUGCCategory, ddlUGCSubCategory1, ddlUGCSubCategory2,
                                                                ddlUGCSubCategory3, dpUGCAirDateTime, ddlUGCtimeZone, txtUGCDescription);
                                    //CallUGSFileUploadWebServiceAsync(newFileName);
                                }
                                else
                                {
                                    IsSuccess = false;
                                    ErrorMessage = ConfigSettings.Settings.ErrorWhileMovingFtp;
                                }
                            }
                            else
                            {
                                DateTime airDate = Convert.ToDateTime(dpUGCAirDateTime);
                                string location = "\\" + airDate.Year + "\\" + airDate.Month + "\\" + airDate.Day + "\\" + newFileName;
                                bool IsFileMovedSuccessfully = MoveUGCFTPDocumentFile(hdnUGCSelectedFileDisplay, location);
                                if (IsFileMovedSuccessfully)
                                {
                                    IQUGCArchiveLogic iQUGCArchiveLogic = (IQUGCArchiveLogic)LogicFactory.GetLogic(LogicType.IQUGCArchive);

                                    Guid categoryGuid = new Guid(ddlUGCCategory);
                                    Guid? subcategory1Guid = !string.IsNullOrEmpty(ddlUGCSubCategory1) ? (Guid?)new Guid(ddlUGCSubCategory1) : null;
                                    Guid? subcategory2Guid = !string.IsNullOrEmpty(ddlUGCSubCategory2) ? (Guid?)new Guid(ddlUGCSubCategory2) : null;
                                    Guid? subcategory3Guid = !string.IsNullOrEmpty(ddlUGCSubCategory3) ? (Guid?)new Guid(ddlUGCSubCategory3) : null;
                                    string res = iQUGCArchiveLogic.InsertIQUGCArchiveDocument(categoryGuid, subcategory1Guid, subcategory2Guid, subcategory3Guid, txtUGCTitle, txtUGCKeywords, txtUGCDescription, dpUGCAirDateTime, ddlUGCtimeZone, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, Path.GetExtension(newFileName).Replace(".", ""), Convert.ToInt32(TempData["UGCDocument_RootPathID"]), location);
                                    if (!string.IsNullOrEmpty(res) && Convert.ToInt32(res) > 0)
                                    {
                                        IsSuccess = true;
                                    }
                                    else
                                    {
                                        IsSuccess = false;
                                        ErrorMessage = ConfigSettings.Settings.ErrorWhileMovingFtp;
                                    }
                                }
                                else
                                {
                                    IsSuccess = false;
                                    ErrorMessage = ConfigSettings.Settings.ErrorWhileMovingFtp;
                                }
                            }
                        }
                        else
                        {
                            IsSuccess = false;
                            ErrorMessage = ConfigSettings.Settings.UGCInvalidFileUploadError;
                        }
                    }
                    else
                    {
                        if (Request.Files.Count > 0)
                        {
                            if (_ugcFileTypes.ContainsKey(Path.GetExtension(Request.Files[0].FileName).Replace(".", "").ToLower()))
                            {
                                string fileName = Path.GetFileName(Request.Files[0].FileName);
                                fileName = Regex.Replace(fileName, @"[\s\\/]", "_");
                                fileName = Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString("MMddyyyy_hhmmss") + Path.GetExtension(fileName);
                                if (_ugcFileTypes[Path.GetExtension(Request.Files[0].FileName).Replace(".", "").ToLower()] == Shared.Utility.CommonFunctions.UGCMediaType)
                                {
                                    bool saveFileSuccessfully = SaveUGCContent(fileName);
                                    if (saveFileSuccessfully)
                                    {
                                        IsSuccess = CreateUGCContentXML(fileName, !string.IsNullOrEmpty(chkAutoClip), txtUGCTitle, txtUGCKeywords, ddlUGCCategory, ddlUGCSubCategory1, ddlUGCSubCategory2,
                                                                ddlUGCSubCategory3, dpUGCAirDateTime, ddlUGCtimeZone, txtUGCDescription);
                                        //CallUGSFileUploadWebServiceAsync(fileName);
                                    }
                                    else
                                    {
                                        IsSuccess = false;
                                        ErrorMessage = ConfigSettings.Settings.ErrorSavingFile;
                                    }
                                }
                                else
                                {
                                    DateTime airDate = Convert.ToDateTime(dpUGCAirDateTime);
                                    string location = "\\" + airDate.Year + "\\" + airDate.Month + "\\" + airDate.Day + "\\" + fileName;

                                    if (!Directory.Exists(Path.GetDirectoryName(TempData["UGCDocument_StoragePath"] + location)))
                                    {
                                        Directory.CreateDirectory(Path.GetDirectoryName(TempData["UGCDocument_StoragePath"] + location));
                                    }

                                    Request.Files[0].SaveAs(TempData["UGCDocument_StoragePath"] + location);
                                    IQUGCArchiveLogic iQUGCArchiveLogic = (IQUGCArchiveLogic)LogicFactory.GetLogic(LogicType.IQUGCArchive);

                                    Guid categoryGuid = new Guid(ddlUGCCategory);
                                    Guid? subcategory1Guid = !string.IsNullOrEmpty(ddlUGCSubCategory1) ? (Guid?)new Guid(ddlUGCSubCategory1) : null;
                                    Guid? subcategory2Guid = !string.IsNullOrEmpty(ddlUGCSubCategory2) ? (Guid?)new Guid(ddlUGCSubCategory2) : null;
                                    Guid? subcategory3Guid = !string.IsNullOrEmpty(ddlUGCSubCategory3) ? (Guid?)new Guid(ddlUGCSubCategory3) : null;
                                    string res = iQUGCArchiveLogic.InsertIQUGCArchiveDocument(categoryGuid, subcategory1Guid, subcategory2Guid, subcategory3Guid, txtUGCTitle, txtUGCKeywords, txtUGCDescription, dpUGCAirDateTime, ddlUGCtimeZone, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, Path.GetExtension(fileName).Replace(".", ""), Convert.ToInt32(TempData["UGCDocument_RootPathID"]), location);
                                    if (!string.IsNullOrEmpty(res) && Convert.ToInt32(res) > 0)
                                    {
                                        IsSuccess = true;
                                    }
                                    else
                                    {
                                        IsSuccess = false;
                                        ErrorMessage = ConfigSettings.Settings.ErrorSavingFile;
                                    }
                                }
                            }
                            else
                            {
                                IsSuccess = false;
                                ErrorMessage = ConfigSettings.Settings.UGCInvalidFileUploadError;
                            }
                        }
                        else
                        {
                            IsSuccess = false;
                            ErrorMessage = ConfigSettings.Settings.FileNotFound;
                        }
                    }
                }
                else
                {
                    IsSuccess = false;
                    ErrorMessage = ConfigSettings.Settings.UnauthorizedAccess;
                }
                return "{ \"isSuccess\" : " + IsSuccess.ToString().ToLower() + ", \"errorMsg\" : \"" + ErrorMessage + "\"}";
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
                return "{ \"isSuccess\" : " + false.ToString().ToLower() + ", \"errorMsg\" : \"" + _Exception.Message + "\"}";
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult GetUGCUploadFTPContent(string p_FolderName)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                if (sessionInformation.Isv4UGCAccess)
                {
                    List<UGCFileModel> lstDirectoriesAndFiles = new List<UGCFileModel>();
                    string ugcBasePath = string.Empty, ugcCurrentPath = string.Empty;

                    if (string.IsNullOrWhiteSpace(p_FolderName))
                    {
                        p_FolderName = string.Empty;
                        TempData["UGCCurrentPath"] = string.Empty;
                    }

                    if (TempData["UGCBasePath"] == null)
                    {
                        ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                        ugcBasePath = clientLogic.GetArchiveClipByClipID(ClientGUID).UGCFtpUploadLocation;
                    }
                    else
                    {
                        ugcBasePath = Convert.ToString(TempData["UGCBasePath"]);
                    }

                    if (!string.IsNullOrEmpty(p_FolderName))
                    {
                        ugcCurrentPath = p_FolderName;
                    }

                    if (!string.IsNullOrWhiteSpace(ugcBasePath))
                    {
                        System.IO.DirectoryInfo directory = new DirectoryInfo(ugcBasePath + ugcCurrentPath);
                        foreach (DirectoryInfo dir in directory.GetDirectories())
                        {
                            lstDirectoriesAndFiles.Add(new UGCFileModel()
                            {
                                Name = dir.Name,
                                Path = (!string.IsNullOrWhiteSpace(ugcCurrentPath) ? ugcCurrentPath + "\\" + dir.Name : dir.Name),
                                LastModifiedDate = dir.LastAccessTime,
                                IsDirectory = true
                            });
                        }
                        foreach (FileInfo file in directory.GetFiles())
                        {
                            lstDirectoriesAndFiles.Add(new UGCFileModel()
                            {
                                Name = file.Name,
                                Path = (!string.IsNullOrWhiteSpace(ugcCurrentPath) ? ugcCurrentPath + "\\" + file.Name : file.Name),
                                LastModifiedDate = file.LastAccessTime,
                                IsDirectory = false
                            });
                        }
                    }

                    TempData["UGCBasePath"] = ugcBasePath;
                    TempData["UGCCurrentPath"] = ugcCurrentPath;
                    string strHTML = RenderPartialToString(PATH_LibraryUGCFTPFileExplorerPartialView, lstDirectoriesAndFiles);
                    return Json(new
                    {
                        HTML = strHTML,
                        isSuccess = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        errorMessage = ConfigSettings.Settings.UnauthorizedAccess
                    });
                }
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
                return Json(new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpPost]
        public JsonResult AddIQUGCToLibrary(List<string> mediaIDs)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();
                
                UGCLogic UGCLogic = (UGCLogic)LogicFactory.GetLogic(LogicType.UGC);
                int result = UGCLogic.InsertArchiveMS(mediaIDs, sessionInformation.ClientGUID, sessionInformation.CustomerGUID);

                if (result > 0)
                {
                    return Json(new
                    {
                        isSuccess = true,
                        recordCount = result
                    });
                }
                else if (result == -2)
                {
                    return Json(new
                    {
                        isSuccess = false,
                        errorMessage = "Please select only non-video records."
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        errorMessage = ConfigSettings.Settings.ErrorOccurred
                    });
                }
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                });
            }
        }

        private bool MoveUGCFTPFile(string fileName, string newFileName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(TempData["UGCBasePath"])))
                {
                    string FTPFilePath = Convert.ToString(TempData["UGCBasePath"]) + Convert.ToString(TempData["UGCCurrentPath"]) + "\\" + fileName;
                    string NewFileLocation = ConfigurationManager.AppSettings["UGCFileUploadLocation"] + newFileName;

                    if (System.IO.File.Exists(FTPFilePath))
                    {
                        System.IO.File.Move(FTPFilePath, NewFileLocation);
                    }
                    if (System.IO.File.Exists(NewFileLocation))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
                return false;
            }
            finally { TempData.Keep(); }
        }

        private bool MoveUGCFTPDocumentFile(string fileName, string newFilePath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(TempData["UGCBasePath"])))
                {
                    string FTPFilePath = Convert.ToString(TempData["UGCBasePath"]) + Convert.ToString(TempData["UGCCurrentPath"]) + "\\" + fileName;
                    string NewFileLocation = Convert.ToString(TempData["UGCDocument_StoragePath"]) + "\\" + newFilePath;

                    if (!Directory.Exists(Path.GetDirectoryName(NewFileLocation)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(NewFileLocation));
                    }

                    if (System.IO.File.Exists(FTPFilePath))
                    {
                        System.IO.File.Move(FTPFilePath, NewFileLocation);
                    }
                    if (System.IO.File.Exists(NewFileLocation))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
                return false;
            }
            finally { TempData.Keep(); }
        }

        private bool CreateUGCContentXML(string UGCFileName, bool chkAutoClip, string txtUGCTitle, string txtUGCKeywords, string ddlUGCCategory, string ddlUGCSubCategory1,
                                        string ddlUGCSubCategory2, string ddlUGCSubCategory3, string dpUGCAirDateTime, string ddlUGCtimeZone, string txtUGCDescription)
        {

            string UGCFileLocation = ConfigurationManager.AppSettings["UGCFileUploadLocation"];
            string ugcFileDownloadLocation = ConfigurationManager.AppSettings["UGCFileDownloadLocation"];
            DateTime currentDateTime = DateTime.Now;


            TempDataModel _TempData = GetTempDataModel();

            dpUGCAirDateTime = !string.IsNullOrWhiteSpace(dpUGCAirDateTime) ? Convert.ToDateTime(dpUGCAirDateTime).ToString("M/d/yyyy hh:mm:ss tt").ToUpper() : string.Empty;

            XDocument xd = new XDocument(new XElement("IngestionData",
                                            new XElement("RawInfo",
                                                new XElement("SourceID", _TempData.UGC_Client_MapModel.SourceID),
                                                new XElement("UGCAutoClip", chkAutoClip.ToString().ToLower()),
                                                new XElement("MetaData",
                                                    new XElement("Meta", new XAttribute("Key", "UGC-Title"),
                                                                        new XAttribute("Value", txtUGCTitle.Trim())
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "UGC-Kwords"),
                                                                        new XAttribute("Value", txtUGCKeywords.Trim())
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "UGC-Category"),
                                                                        new XAttribute("Value", ddlUGCCategory)
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "UGC-SubCategory1"),
                                                                        new XAttribute("Value", ddlUGCSubCategory1)
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "UGC-SubCategory2"),
                                                                        new XAttribute("Value", ddlUGCSubCategory2)
                                                                )
                                                                ,
                                                    new XElement("Meta", new XAttribute("Key", "UGC-SubCategory3"),
                                                                        new XAttribute("Value", ddlUGCSubCategory3)
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "UGC-CreateDT"),
                                                                        new XAttribute("Value", dpUGCAirDateTime + " " + ddlUGCtimeZone)
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "UGC-UploadDT"),
                                                                        new XAttribute("Value", currentDateTime.ToString())
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "UGC-Desc"),
                                                                        new XAttribute("Value", txtUGCDescription.Trim())
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "iQUser"),
                                                                        new XAttribute("Value", CustomerGUID)
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "UGC-FileName"),
                                                                        new XAttribute("Value", UGCFileName)
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "UGC-FileLocation"),
                                                                        new XAttribute("Value", ugcFileDownloadLocation + currentDateTime.Year + @"\" + currentDateTime.Month + @"\" + currentDateTime.Day + @"\")
                                                                )
                                                            )
                                                        ),
                                                 _TempData.UGC_Client_MapModel.AutoClip_Status ?
                                            new XElement("ClipInfo",
                                                new XElement("Title", txtUGCTitle.Trim()),
                                                new XElement("Category", "PR"),
                                                new XElement("Keywords", txtUGCKeywords.Trim()),
                                                new XElement("Description", txtUGCDescription.Trim()),
                                                new XElement("User", ConfigurationManager.AppSettings["IQMediaUserGUID"]),
                                                new XElement("MetaData",
                                                    new XElement("Meta", new XAttribute("Key", "iQClientid"),
                                                                        new XAttribute("Value", ClientGUID)
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "iQUser"),
                                                                        new XAttribute("Value", CustomerGUID)
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "iQCategory"),
                                                                        new XAttribute("Value", ddlUGCCategory)
                                                                )
                                                                ,
                                                    new XElement("Meta", new XAttribute("Key", "SubCategory1GUID"),
                                                                        new XAttribute("Value", ddlUGCSubCategory1)
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "SubCategory2GUID"),
                                                                        new XAttribute("Value", ddlUGCSubCategory2)
                                                                ),
                                                    new XElement("Meta", new XAttribute("Key", "SubCategory3GUID"),
                                                                        new XAttribute("Value", ddlUGCSubCategory3)
                                                                )
                                                            )
                                                        ) : null
                                                     )
                                             );

            //........................................Insert record into UGC_Upload_Log table....................................................................

            UGCUploadLogModel objUGCUploadLogModel = new UGCUploadLogModel();
            objUGCUploadLogModel.CustomerGUID = new Guid(CustomerGUID);
            objUGCUploadLogModel.FileName = UGCFileName;
            objUGCUploadLogModel.UGCXml = new System.Data.SqlTypes.SqlXml(xd.CreateReader());
            objUGCUploadLogModel.UploadedDateTime = currentDateTime;

            var objUGCLogic = (UGCLogic)LogicFactory.GetLogic(LogicType.UGC);
            string result = objUGCLogic.InsertUGCUploadLog(objUGCUploadLogModel);

            if (!string.IsNullOrEmpty(result) && Convert.ToInt64(result) > 0)
            {
                xd.Root.Add(new XElement("ID",result));
                XmlWriterSettings xws = new XmlWriterSettings { OmitXmlDeclaration = true };
                using (XmlWriter xw = XmlWriter.Create(UGCFileLocation + UGCFileName + ".xml", xws))
                    xd.Save(xw);
            }

            ////........................................Insert record into UGC_Upload_Log table....................................................................

            return System.IO.File.Exists(UGCFileLocation + UGCFileName + ".xml");
        }

        private void RespCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                // State of request is asynchronous.
                RequestState myRequestState = (RequestState)asynchronousResult.AsyncState;
                HttpWebRequest myHttpWebRequest = myRequestState.request;
                myRequestState.response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(asynchronousResult);
            }
            catch (WebException _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
            }
            finally { TempData.Keep(); }
        }

        private bool SaveUGCContent(string p_FileName)
        {
            bool flag = true;
            string FileLocation = ConfigurationManager.AppSettings["UGCFileUploadLocation"];
            if (Request.Files.Count > 0)
            {
                Request.Files[0].SaveAs(FileLocation + Path.GetFileName(p_FileName));
            }
            else
                flag = false;

            return flag;
        }

        private bool ValidateUGCFileTypes(string p_FileName,out bool isDocument)
        {
            isDocument = false;
            if (!string.IsNullOrWhiteSpace(p_FileName))
            {
                string[] strUGCFileTypes = ConfigurationManager.AppSettings["UGCPermittedFileTypes"].ToUpper().Split(new char[] { ',' });
                string[] strUGCDocFileTypes = ConfigurationManager.AppSettings["UGCDocFileTypes"].ToUpper().Split(new char[] { ',' });
                p_FileName = Path.GetExtension(p_FileName).ToUpper().Replace(".", string.Empty);
                if(strUGCFileTypes.Contains(p_FileName))
                {
                    isDocument = false;
                    return true;
                }
                if (strUGCDocFileTypes.Contains(p_FileName))
                {
                    isDocument = true;
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        public JsonResult DisplayIQUGCArchiveResults(bool p_IsLoadMoreResults, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_CustomerGuid, List<string> p_CategoryGuid, string p_SelectionType,string p_MediaType, string p_Sortcolumn, bool p_IsAsc, int? p_PageSize)
        {
            try
            {
                p_Sortcolumn = string.IsNullOrWhiteSpace(p_Sortcolumn) ? "AirDate" : p_Sortcolumn;

                IQUGCArchiveResult _IQUGCArchiveResult = GetIQUGCArchiveResults(false, p_IsLoadMoreResults, p_FromDate, p_ToDate, p_SearchTerm, p_CustomerGuid, p_CategoryGuid, p_SelectionType, p_MediaType, p_Sortcolumn, p_IsAsc, !p_IsLoadMoreResults, p_PageSize);
                TempDataModel _TempData = GetTempDataModel();
                string strHTML = RenderPartialToString(PATH_IQUGCArchiveResultsPartialView, _IQUGCArchiveResult);
                var json = new
                {
                    isSuccess = true,
                    hasMoreResults = HasUGCMoreResults(),
                    HTML = strHTML,
                    filter = _IQUGCArchiveResult.Filter,
                    currentRecords = _TempData.UGCFromRecordID,
                    totalRecords = _TempData.UGCTotalResults
                };

                return Json(json);
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };

                return Json(json);
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpPost]
        public JsonResult DeleteIQUGCArchiveResults(string p_UGCArchiveIDs, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_CustomerGuid, List<string> p_CategoryGuid, string p_SelectionType, string p_MediaType)
        {
            try
            {
                string strUGCArchiveID = string.Empty;
                IQUGCArchiveResult_FilterModel iqUGCFilter = null;
                TempDataModel _TempData = GetTempDataModel();
                if (!string.IsNullOrEmpty(p_UGCArchiveIDs))
                {
                    string ugcArchiveXML = string.Empty;

                    foreach (string id in p_UGCArchiveIDs.Split(new char[] { ',' }))
                    {
                        if (!string.IsNullOrWhiteSpace(id.Trim()))
                        {
                            ugcArchiveXML += "<id> " + id + "</id>";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(ugcArchiveXML))
                    {
                        ugcArchiveXML = "<list>" + ugcArchiveXML + "</list>";

                    }

                    IQUGCArchiveLogic iQArchieveLogic = (IQUGCArchiveLogic)LogicFactory.GetLogic(LogicType.IQUGCArchive);
                    List<long> lstArchiveID = iQArchieveLogic.Delete(ClientGUID, ugcArchiveXML);

                    if (lstArchiveID.Count > 0)
                    {
                        _TempData.UGCFromRecordID = _TempData.UGCFromRecordID - lstArchiveID.Count;
                        _TempData.UGCTotalResults = _TempData.UGCTotalResults - lstArchiveID.Count;

                        strUGCArchiveID = string.Join(",", lstArchiveID.Select(id => id));

                        iqUGCFilter = GetIQUGCArchiveFilter(p_FromDate, p_ToDate, p_SearchTerm, p_CustomerGuid, p_CategoryGuid, p_SelectionType, p_MediaType);
                    }

                    TempData.Keep();
                }
                var json = new
                {
                    isSuccess = true,
                    ugcarchiveIDs = strUGCArchiveID,
                    filter = iqUGCFilter,
                    totalRecords = _TempData.UGCTotalResults,
                    currentRecords = _TempData.UGCFromRecordID
                };

                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult FilterIQUGCArchiveCategory(DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_CustomerGuid, List<string> p_CategoryGuid, string p_MediaType)
        {

            try
            {
                IQUGCArchiveLogic iQUGCArchiveLogic = (IQUGCArchiveLogic)LogicFactory.GetLogic(LogicType.IQUGCArchive);

                TempDataModel _TempData = GetTempDataModel();

                List<IQUGCArchiveResult_Filter> lstCategoryFilter = iQUGCArchiveLogic.GetCategoryFilter(ClientGUID, p_FromDate, p_ToDate, p_SearchTerm, p_CategoryGuid, p_CustomerGuid, _TempData.UGCSinceID,p_MediaType);

                var json = new
                {
                    isSuccess = true,
                    categoryFilter = lstCategoryFilter
                };

                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }

        }

        [HttpPost]
        public JsonResult GetIQUGCArchiveEdit(long p_IQUGCArchiveKey)
        {
            try
            {
                IQUGCArchiveLogic iQArchieveLogic = (IQUGCArchiveLogic)LogicFactory.GetLogic(LogicType.IQUGCArchive);
                IQUGCArchiveEditModel editModel = iQArchieveLogic.SelectForEdit(ClientGUID, p_IQUGCArchiveKey);
                string strHTML = RenderPartialToString(PATH_IQUGCArchiveEditPartialView, editModel);

                var json = new
                {
                    isSuccess = true,
                    HTML = strHTML
                };

                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult UpdateIQUGCArchive(long p_IQUGCArchiveKey, string p_Title, string p_Keywords, Guid p_Customer, Guid? p_Category, Guid? p_Subcategory1, Guid? p_Subcategory2, Guid? p_Subcategory3, string p_Description)
        {
            try
            {
                IQUGCArchiveLogic iQArchieveLogic = (IQUGCArchiveLogic)LogicFactory.GetLogic(LogicType.IQUGCArchive);
                iQArchieveLogic.UpdateIQUGCArchive(ClientGUID, p_IQUGCArchiveKey, p_Title, p_Keywords, p_Customer, p_Category, p_Subcategory1, p_Subcategory2, p_Subcategory3, p_Description);
                var json = new
                {
                    isSuccess = true
                };

                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult RefreshIQUGCArchiveResults()
        {
            try
            {
                IQUGCArchiveRefreshResults();

                // Render results again to reflect newly inserted records after refresh call above 

                IQUGCArchiveResult _IQUGCArchiveResult = GetIQUGCArchiveResults(true, false, null, null, null, null, null, null, null, null, false, true, null);
                TempDataModel _TempData = GetTempDataModel();
                string strHTML = RenderPartialToString(PATH_IQUGCArchiveResultsPartialView, _IQUGCArchiveResult);

                return Json(new
                {
                    isSuccess = true,
                    hasMoreResults = HasUGCMoreResults(),
                    HTML = strHTML,
                    filter = _IQUGCArchiveResult.Filter,
                    currentRecords = _TempData.UGCFromRecordID,
                    totalRecords = _TempData.UGCTotalResults
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpPost]
        public JsonResult CheckForUGCFile(long p_IQUGCArchiveKey)
        {
            bool IsSuccess = true;
            string ErrorMessage = string.Empty;
            try
            {
                IQUGCArchiveLogic iQUGCArchiveLogic = (IQUGCArchiveLogic)LogicFactory.GetLogic(LogicType.IQUGCArchive);
                IQUGCArchiveModel ugc = iQUGCArchiveLogic.SelectUGCFileLocationAndName(ClientGUID, p_IQUGCArchiveKey);

                if (!string.IsNullOrWhiteSpace(ugc.UGCFileLocation) && !string.IsNullOrWhiteSpace(ugc.UGCFileName))
                {
                    if (!ugc.UGCFileLocation.EndsWith("\\"))
                    {
                        ugc.UGCFileLocation += "\\";
                    }

                    if (System.IO.File.Exists(ugc.UGCFileLocation + ugc.UGCFileName))
                    {

                        Session["UGCDownloadFileLocation"] = ugc.UGCFileLocation + ugc.UGCFileName;
                    }
                    else
                    {
                        IsSuccess = false;
                        ErrorMessage = Config.ConfigSettings.Settings.UGCFileNotAvailableForDownload;
                    }
                }
                else
                {
                    IsSuccess = false;
                    ErrorMessage = Config.ConfigSettings.Settings.UGCFileNotAvailableForDownload;

                }
                return Json(new
                {
                    isSuccess = IsSuccess,
                    errorMessage = ErrorMessage
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpGet]
        public ActionResult DownloadUGCFile()
        {
            try
            {
                if (Session["UGCDownloadFileLocation"] != null)
                {
                    string UGCFile = Convert.ToString(Session["UGCDownloadFileLocation"]);

                    if (System.IO.File.Exists(UGCFile))
                    {
                        string contentType = IQMedia.Shared.Utility.CommonFunctions.GetFileContentTypeByExtension(Path.GetExtension(UGCFile));
                        Session.Remove("UGCDownloadFileLocation");
                        return File(UGCFile, contentType, Path.GetFileName(UGCFile));
                    }
                }
                return Content(ConfigSettings.Settings.UGCFileNotAvailableForDownload);
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
                return Content(ConfigSettings.Settings.UGCFileDownloadError);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult GetArchiveTVEyesLocation(Int64 p_ID)
        {
            try
            {
                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                string p_TranscriptFileLocation = string.Empty;
                string p_AudioFileLocation = string.Empty;
                iQArchieveLogic.GetArchiveTVeyesLocationByArchiveTVEyesKey(p_ID, out p_TranscriptFileLocation, out p_AudioFileLocation);
                string transcriptHtml = string.Empty;
                if (!string.IsNullOrEmpty(p_TranscriptFileLocation) && System.IO.File.Exists(p_TranscriptFileLocation))
                {
                    StreamReader strmTranscriptHtml = new StreamReader(p_TranscriptFileLocation);
                    transcriptHtml = strmTranscriptHtml.ReadToEnd();
                    strmTranscriptHtml.Close();
                    strmTranscriptHtml.Dispose();
                }

                return Json(new
                {
                    HTML = transcriptHtml,
                    AudiFileLocation = p_AudioFileLocation,
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
            finally
            {
                TempData.Keep();
            }
        }

        private IQUGCArchiveResult GetIQUGCArchiveResults(bool IsRefreshResults, bool IsLoadMoreResults, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_CustomerGuid, List<string> p_CategoryGuid, string p_SelectionType, string p_MediaType, string p_Sortcolumn, bool p_IsAsc, bool p_IsEnableFilter, int? p_PageSize)
        {
            IQUGCArchiveLogic iQUGCArchiveLogic = (IQUGCArchiveLogic)LogicFactory.GetLogic(LogicType.IQUGCArchive);

            TempDataModel _TempData = GetTempDataModel();
            if (IsRefreshResults)
            {
                _TempData.UGCFromRecordID = 0;
                _TempData.UGCSinceID = 0;
                _TempData.UGCTotalResults = 0;
            }

            long FromRecordID = (IsLoadMoreResults ? _TempData.UGCFromRecordID : 0);

            int PageSize;
            if (p_PageSize.HasValue)
                PageSize = p_PageSize.Value;
            else
                PageSize = GetIQCustomSettings().DefaultArchivePageSize;

            if (!string.IsNullOrWhiteSpace(p_SearchTerm))
            {
                p_SearchTerm = GenerateSearchTerm(p_SearchTerm.Trim());
            }

            IQUGCArchiveResult objIQUGCArchiveResult = iQUGCArchiveLogic.GetIQArchieveResults(ClientGUID, p_FromDate, p_ToDate, p_SearchTerm, p_CategoryGuid, p_SelectionType, p_CustomerGuid, p_MediaType, FromRecordID, PageSize, _TempData.UGCSinceID, p_Sortcolumn, p_IsAsc, p_IsEnableFilter);

            _TempData.UGCTotalResults = objIQUGCArchiveResult.TotalResults;
            _TempData.UGCSinceID = objIQUGCArchiveResult.SinceID;

            if (IsLoadMoreResults)
            {
                _TempData.UGCFromRecordID += objIQUGCArchiveResult.IQUGCArchiveList.Count();
            }
            else
            {
                _TempData.UGCFromRecordID = objIQUGCArchiveResult.IQUGCArchiveList.Count();
            }
            TempData["TempDataModel"] = _TempData;
            TempData.Keep();
            return objIQUGCArchiveResult;
        }

        private IQUGCArchiveResult_FilterModel GetIQUGCArchiveFilter(DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_CustomerGuid, List<string> p_CategoryGuid, string p_SelectionType, string p_MediaType)
        {
            IQUGCArchiveLogic iQUGCArchiveLogic = (IQUGCArchiveLogic)LogicFactory.GetLogic(LogicType.IQUGCArchive);

            TempDataModel _TempData = GetTempDataModel();

            IQUGCArchiveResult_FilterModel filter = iQUGCArchiveLogic.GetIQArchieveFilter(ClientGUID, p_FromDate, p_ToDate, p_SearchTerm, p_CategoryGuid, p_SelectionType, p_CustomerGuid, _TempData.UGCSinceID, p_MediaType);

            TempData["TempDataModel"] = _TempData;
            TempData.Keep();

            return filter;
        }

        private void IQUGCArchiveRefreshResults()
        {
            try
            {
                IQUGCArchiveLogic iQUGCArchiveLogic = (IQUGCArchiveLogic)LogicFactory.GetLogic(LogicType.IQUGCArchive);
                iQUGCArchiveLogic.RefreshResults(ClientGUID);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            finally { TempData.Keep(); }
        }

        private bool HasUGCMoreResults()
        {
            TempDataModel _TempData = GetTempDataModel();

            if (_TempData.UGCFromRecordID < _TempData.UGCTotalResults)
                return true;
            else
                return false;
        }

        private TempDataModel GetTempDataModel()
        {
            return TempData["TempDataModel"] == null ? new TempDataModel() : TempData["TempDataModel"] as TempDataModel;
        }

        protected string GenerateSearchTerm(string SearchVal)
        {

            if (SearchVal.Substring(SearchVal.Length - 1, 1) == "#")
            {
                SearchVal = SearchVal.Remove(SearchVal.Length - 1, 1);

                var StrStemmed = Regex.Replace(SearchVal,
                            @"([\""][\w ]+[\""])|(\w+)",
                            m => (Enum.GetValues(typeof(IQMedia.Shared.Utility.CommonFunctions.FullTextLogicalOperator)).Cast<IQMedia.Shared.Utility.CommonFunctions.FullTextLogicalOperator>().Select(v => v.ToString()).ToList()).Contains(m.Value.ToUpper()) ? m.Value : "FORMSOF (INFLECTIONAL," + m.Value + ")"
                          );
                return Convert.ToString(StrStemmed);
            }
            else
            {
                return SearchVal;
            }
        }

        #endregion

        #region MCMedia

        [HttpPost]
        public ContentResult SearchMCMediaResults(string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_SubMediaType, List<string> p_CategoryGUID, string p_SelectionType, bool p_IsAsc, string p_SortColumn, int? p_PageSize, bool p_IsRefresh = false)
        {
            try
            {
                TempData["FromRecordID_MCMedia"] = 0;

                Dictionary<string, object> dictResult = GetMCMediaResults(Convert.ToInt64(TempData["FromRecordID_MCMedia"]), p_CustomerGuid, p_FromDate, p_ToDate,
                                                                               p_SearchTerm, p_SubMediaType, p_CategoryGUID, p_SelectionType, p_IsAsc, p_SortColumn, true, p_PageSize, p_IsRefresh);

                IQArchive_FilterModel iqArchiveFilter = dictResult["Filter"] as IQArchive_FilterModel;

                if (dictResult["Result"] != null)
                {
                    TempData["FromRecordID_MCMedia"] = ((List<IQArchive_MediaModel>)dictResult["Result"]).Count;
                    TempData.Keep();
                }

                LibraryResult json = new LibraryResult
                {
                    isSuccess = true,
                    hasMoreResults = Convert.ToInt64(TempData["FromRecordID_MCMedia"]) < Convert.ToInt64(TempData["TotalResults_MCMedia"]),
                    HTML = dictResult["Result"] != null ? RenderPartialToString(PATH_MCMediaResultsPartialView, dictResult["Result"]) : string.Empty,
                    filter = iqArchiveFilter,
                    totalRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["TotalResults_MCMedia"])),
                    currentRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["FromRecordID_MCMedia"]))
                };

                return Content(IQMedia.Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                LibraryResult json = new LibraryResult
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Content(IQMedia.Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public ContentResult GetMoreMCMediaResults(string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_SubMediaType, List<string> p_CategoryGUID, string p_SelectionType, bool p_IsAsc, string p_SortColumn, int? p_PageSize)
        {
            try
            {
                Dictionary<string, object> dictResult = GetMCMediaResults(Convert.ToInt64(TempData["FromRecordID_MCMedia"]), p_CustomerGuid, p_FromDate, p_ToDate,
                                                                           p_SearchTerm, p_SubMediaType, p_CategoryGUID, p_SelectionType, p_IsAsc, p_SortColumn, false, p_PageSize, false);

                if (dictResult["Result"] != null)
                {
                    TempData["FromRecordID_MCMedia"] = Convert.ToInt64(TempData["FromRecordID_MCMedia"]) + ((List<IQArchive_MediaModel>)dictResult["Result"]).Count;
                    TempData.Keep();
                }

                LibraryResult json = new LibraryResult()
                {
                    isSuccess = true,
                    hasMoreResults = Convert.ToInt64(TempData["FromRecordID_MCMedia"]) < Convert.ToInt64(TempData["TotalResults_MCMedia"]),
                    HTML = dictResult["Result"] != null ? RenderPartialToString(PATH_MCMediaResultsPartialView, dictResult["Result"]) : string.Empty,
                    totalRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["TotalResults_MCMedia"])),
                    currentRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["FromRecordID_MCMedia"]))
                };
                return Content(IQMedia.Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                LibraryResult json = new LibraryResult
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Content(IQMedia.Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult FilterMCMediaCategory(string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_SubMediaType, List<string> p_CategoryGUID)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                if (p_FromDate.HasValue && p_ToDate.HasValue)
                {
                    p_FromDate = Utility.CommonFunctions.GetGMTandDSTTime(p_FromDate);

                    p_ToDate = p_ToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    p_ToDate = Utility.CommonFunctions.GetGMTandDSTTime(p_ToDate);
                }

                long sinceID = TempData["SinceID_MCMedia"] != null ? Convert.ToInt64(TempData["SinceID_MCMedia"]) : 0;

                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                List<IQArchive_Filter> lstCategoryFilter = iQArchieveLogic.GetMCMediaCategoryFilter(ClientGUID, p_CustomerGuid, p_FromDate, p_ToDate, p_SubMediaType, p_SearchTerm, p_CategoryGUID, sinceID, sessionInformation.MCID, null);

                var json = new
                {
                    isSuccess = true,
                    categoryFilter = lstCategoryFilter
                };

                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult AddToMCMediaReport(List<string> p_MediaIDs, string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_SubMediaType, List<string> p_CategoryGUID, string p_SelectionType)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                if (p_MediaIDs == null || p_MediaIDs.Count == 0)
                {
                    Dictionary<string, object> dictResults = GetMCMediaResults(0, p_CustomerGuid, p_FromDate, p_ToDate, p_SearchTerm, p_SubMediaType, p_CategoryGUID, p_SelectionType, true, null, false, 999999, false, false);
                    p_MediaIDs = ((List<IQArchive_MediaModel>)dictResults["Result"]).Select(s => s.ID.ToString()).ToList();
                }

                Guid? reportGuid = TempData["MCMediaReportGuid"] != null ? new Guid(TempData["MCMediaReportGuid"].ToString()) : (Guid?)null;
                IQClient_CustomSettingsModel customSettings = GetIQCustomSettings();

                IQArchieveLogic iQArchiveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                reportGuid = iQArchiveLogic.AddToMCMediaReport(reportGuid, sessionInformation.MCID.HasValue ? sessionInformation.MCID.Value : sessionInformation.ClientID, customSettings.MCMediaPublishedTemplateID, p_MediaIDs);

                if (reportGuid.HasValue)
                {
                    TempData["MCMediaReportGuid"] = reportGuid.Value;
                }

                var json = new
                {
                    isSuccess = true,
                    recordCount = p_MediaIDs.Count
                };

                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult RemoveFromMCMediaReport(List<string> p_MediaIDs, string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_SubMediaType, List<string> p_CategoryGUID, string p_SelectionType)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                if (p_MediaIDs == null || p_MediaIDs.Count == 0)
                {
                    Dictionary<string, object> dictResults = GetMCMediaResults(0, p_CustomerGuid, p_FromDate, p_ToDate, p_SearchTerm, p_SubMediaType, p_CategoryGUID, p_SelectionType, true, null, false, 999999, false, false);
                    p_MediaIDs = ((List<IQArchive_MediaModel>)dictResults["Result"]).Select(s => s.ID.ToString()).ToList();
                }

                Guid? reportGuid = TempData["MCMediaReportGuid"] != null ? new Guid(TempData["MCMediaReportGuid"].ToString()) : (Guid?)null;

                if (reportGuid.HasValue)
                {
                    IQArchieveLogic iQArchiveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                    iQArchiveLogic.RemoveFromMCMediaReport(reportGuid.Value, p_MediaIDs);
                }

                return Json(new
                {
                    isSuccess = true,
                    recordCount = p_MediaIDs.Count
                });
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult GetMCMediaReportGUID()
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                Guid? reportGUID = null;
                if (TempData["MCMediaReportGuid"] == null)
                {
                    IQArchieveLogic iQArchiveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                    reportGUID = iQArchiveLogic.GetMCMediaReportGUID(sessionInformation.MCID.HasValue ? sessionInformation.MCID.Value : sessionInformation.ClientID);

                    if (reportGUID.HasValue)
                    {
                        TempData["MCMediaReportGuid"] = reportGUID.Value;
                    }
                }
                else
                {
                    reportGUID = new Guid(TempData["MCMediaReportGuid"].ToString());
                }

                return Json(new
                {
                    isSuccess = true,
                    reportGUID = reportGUID.HasValue ? reportGUID.Value.ToString() : ""
                });
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally { TempData.Keep(); }
        }

        private Dictionary<string, object> GetMCMediaResults(long p_FromRecordID, string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, string p_SubMediaType, List<string> p_CategoryGUID, 
                                                                string p_SelectionType, bool p_IsAsc, string p_SortColumn, bool p_IsEnableFilter, int? p_PageSize, bool p_IsRefresh, bool p_UpdateTempData = true)
        {
            sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

            IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);

            long totalResults = 0;
            long sinceID = TempData["SinceID_MCMedia"] != null && !p_IsRefresh ? Convert.ToInt64(TempData["SinceID_MCMedia"]) : 0;
            p_SortColumn = !string.IsNullOrEmpty(p_SortColumn) ? p_SortColumn : "CreatedDate";

            int PageSize;
            if (p_PageSize.HasValue)
                PageSize = p_PageSize.Value;
            else
                PageSize = GetIQCustomSettings().DefaultArchivePageSize;

            if (p_FromDate.HasValue && p_ToDate.HasValue)
            {
                p_FromDate = Utility.CommonFunctions.GetGMTandDSTTime(p_FromDate);

                p_ToDate = p_ToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                p_ToDate = Utility.CommonFunctions.GetGMTandDSTTime(p_ToDate);
            }

            Dictionary<string, object> dictResult = null;
            dictResult = iQArchieveLogic.GetMCMediaResults(ClientGUID, p_CustomerGuid, p_FromRecordID, PageSize, p_FromDate, p_ToDate, p_SubMediaType, p_SearchTerm, p_CategoryGUID, p_SelectionType, p_IsAsc, p_SortColumn, 
                                                            p_IsEnableFilter, Request.ServerVariables["HTTP_HOST"], ref sinceID, out totalResults, sessionInformation.MCID);

            if (dictResult["Result"] != null)
            {
                List<IQArchive_MediaModel> mediaResults = (List<IQArchive_MediaModel>)dictResult["Result"];
                mediaResults.ToList().ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.CreatedDate, false));
                mediaResults = CommonFunctions.GetGMTandDSTTime(mediaResults, CommonFunctions.ResultType.Library);
                mediaResults = CommonFunctions.ProcessArchiveDisplayText(mediaResults, GetIQCustomSettings().LibraryTextType);
                dictResult["Result"] = mediaResults;
            }

            if (p_UpdateTempData)
            {
                TempData["TotalResults_MCMedia"] = totalResults;
                TempData["SinceID_MCMedia"] = sinceID;
                TempData.Keep();
            }

            return dictResult;
        }

        #endregion

        #region Common functions

        private string RenderPartialToString(string viewName, object model)
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

        public Boolean GetDownloadRoleByCustomerGuid()
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);

                return customerLogic.GetDownloadRoleByCustomerGuid(sessionInformation.CustomerGUID);
            }
            catch (Exception)
            {

                throw;
            }
            finally { TempData.Keep(); }
        }

        #endregion
    }

    public class RequestState
    {
        // This class stores the State of the request.
        const int BUFFER_SIZE = 1024;
        public StringBuilder requestData;
        public byte[] BufferRead;
        public HttpWebRequest request;
        public HttpWebResponse response;
        public Stream streamResponse;
        public RequestState()
        {
            BufferRead = new byte[BUFFER_SIZE];
            requestData = new StringBuilder("");
            request = null;
            streamResponse = null;
        }
    }

}
