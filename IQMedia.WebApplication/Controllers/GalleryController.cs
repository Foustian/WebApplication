using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Model;
using System.IO;
using IQMedia.WebApplication.Models;
using IQMedia.Web.Logic;
using System.Net;
using IQMedia.Web.Logic.Base;
using System.Text;
using System.Configuration;
using IQMedia.WebApplication.Config;
using IQMedia.WebApplication.Utility;

namespace IQMedia.WebApplication.Controllers
{
    public class GalleryController : Controller
    {
        //
        // GET: /Tiles/

        #region Public Property

        ActiveUser sessionInformation = null;
        private string PATH_TilesLibraryResultsPartialView = "~/Views/Gallery/_LibraryGalleryResults.cshtml";
        private string PATH_GalleryResultsPartialView = "~/Views/Gallery/_GalleryResults.cshtml";

        #endregion

        public ActionResult Index(int ID)
        {
            IQGalleryModel _IQGalleryModel = new IQGalleryModel();
            try
            {

                // Fetch Client's Custom settings and store it in TempData
                SetIQCustomSettings();

                ViewBag.v4LibraryRollup = GetIQCustomSettings().v4LibraryRollup;
                ViewBag.DefaultArchivePageSize = GetIQCustomSettings().DefaultArchivePageSize;

                sessionInformation = ActiveUserMgr.GetActiveUser();
                GalleryLogic gallaryLogic = (GalleryLogic)LogicFactory.GetLogic(LogicType.Gallery);
                _IQGalleryModel = gallaryLogic.GetGalleryByID(ID, sessionInformation.CustomerGUID);
                ViewBag.gallery = _IQGalleryModel.Json;
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
            return View("Index", _IQGalleryModel);
        }

        private Dictionary<string, object> GetIQArchieveResults(long p_FromRecordID, string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, List<string> p_CategoryGUID, string p_SelectionType, bool p_IsAsc, string p_SortColumn, bool p_IsEnableFilter, int? p_PageSize)
        {
            sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

            IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);

            long totalResults = 0;
            long totalResultsDisplay = 0;
            long sinceID = TempData["TilesSinceID"] != null ? Convert.ToInt64(TempData["TilesSinceID"]) : 0;
            p_SortColumn = !string.IsNullOrEmpty(p_SortColumn) ? p_SortColumn : "CreatedDate";

            int PageSize;
            if (p_PageSize.HasValue)
                PageSize = p_PageSize.Value;
            else
                PageSize = GetIQCustomSettings().DefaultArchivePageSize;

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
            dictResult = iQArchieveLogic.GetIQAgentMediaResults(sessionInformation.ClientGUID.ToString(), p_CustomerGuid,
                                                p_FromRecordID, PageSize, p_FromDate, p_ToDate, "TV", p_SearchTerm, p_CategoryGUID, p_SelectionType, p_IsAsc, p_SortColumn, p_IsEnableFilter, Request.ServerVariables["HTTP_HOST"], ref sinceID, out totalResults, out totalResultsDisplay, GetIQCustomSettings().v4LibraryRollup);

            if (dictResult["Result"] != null)
            {
                dictResult["Result"] = CommonFunctions.GetGMTandDSTTime((List<IQArchive_MediaModel>)dictResult["Result"], CommonFunctions.ResultType.Library);
            }

            TempData["TilesTotalResults"] = totalResults;
            TempData["TilesTotalResultsDisplay"] = totalResultsDisplay;
            TempData["TilesSinceID"] = sinceID;
            TempData.Keep();

            return dictResult;
        }

        [HttpPost]
        public ContentResult GetMoreResults(string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, List<string> p_CategoryGUID, string p_SelectionType, bool p_IsAsc, string p_SortColumn, int? p_PageSize)
        {

            try
            {

                Dictionary<string, object> dictResult = GetIQArchieveResults(Convert.ToInt64(TempData["FromRecordTilesID"]), p_CustomerGuid, p_FromDate, p_ToDate,
                                                                           p_SearchTerm, p_CategoryGUID, p_SelectionType, p_IsAsc, p_SortColumn, false, p_PageSize);

                if (dictResult["Result"] != null)
                {
                    TempData["FromRecordTilesID"] = Convert.ToInt64(TempData["FromRecordTilesID"]) + ((List<IQArchive_MediaModel>)dictResult["Result"]).Count;
                    TempData["FromRecordTilesIDDisplay"] = Convert.ToInt64(TempData["FromRecordTilesIDDisplay"]) + ((List<IQArchive_MediaModel>)dictResult["Result"]).Count
                                            + (((List<IQArchive_MediaModel>)dictResult["Result"]).Sum(a => (a.MediaType == "TV") ? (a.MediaData as IQArchive_ArchiveClipModel).ChildResults.Count() : 0))
                                            + (((List<IQArchive_MediaModel>)dictResult["Result"]).Sum(a => (a.MediaType == "NM") ? (a.MediaData as IQArchive_ArchiveNMModel).ChildResults.Count() : 0));
                    TempData.Keep();
                }

                LibraryResult json = new LibraryResult()
                {
                    isSuccess = true,
                    hasMoreResults = HasMoreResults(),
                    HTML = dictResult["Result"] != null ? RenderPartialToString(PATH_TilesLibraryResultsPartialView, dictResult["Result"]) : string.Empty,
                    totalRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["TilesTotalResults"])),
                    totalRecordsDisplay = string.Format("{0:n0}", Convert.ToInt64(TempData["TilesTotalResultsDisplay"])),
                    currentRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["FromRecordTilesID"])),
                    currentRecordsDisplay = string.Format("{0:n0}", Convert.ToInt64(TempData["FromRecordTilesIDDisplay"]))
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
        public ContentResult SearchLibraryResults(string p_CustomerGuid, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTerm, List<string> p_CategoryGUID, string p_SelectionType, bool p_IsAsc, string p_SortColumn, int? p_PageSize)
        {
            try
            {
                TempData["FromRecordTilesID"] = 0;
                TempData["FromRecordTilesIDDisplay"] = 0;

                Dictionary<string, object> dictResult = GetIQArchieveResults(Convert.ToInt64(TempData["FromRecordTilesID"]), p_CustomerGuid, p_FromDate, p_ToDate,
                                                                               p_SearchTerm, p_CategoryGUID, p_SelectionType, p_IsAsc, p_SortColumn, true, p_PageSize);

                IQArchive_FilterModel iqArchiveFilter = dictResult["Filter"] as IQArchive_FilterModel;

                if (dictResult["Result"] != null)
                {
                    TempData["FromRecordTilesID"] = ((List<IQArchive_MediaModel>)dictResult["Result"]).Count;
                    TempData["FromRecordTilesIDDisplay"] = ((List<IQArchive_MediaModel>)dictResult["Result"]).Count
                                    + (((List<IQArchive_MediaModel>)dictResult["Result"]).Sum(a => (a.MediaType == "TV") ? (a.MediaData as IQArchive_ArchiveClipModel).ChildResults.Count() : 0))
                                    + (((List<IQArchive_MediaModel>)dictResult["Result"]).Sum(a => (a.MediaType == "NM") ? (a.MediaData as IQArchive_ArchiveNMModel).ChildResults.Count() : 0));
                    TempData.Keep();
                }

                //string dates = string.Join(",", iqArchiveFilter.Dates.Select(f => Convert.ToDateTime(f).ToString("MM/dd/yyyy")).ToArray());

                LibraryResult json = new LibraryResult
                {
                    isSuccess = true,
                    hasMoreResults = HasMoreResults(),
                    HTML = dictResult["Result"] != null ? RenderPartialToString(PATH_TilesLibraryResultsPartialView, dictResult["Result"]) : string.Empty,
                    filter = iqArchiveFilter,
                    totalRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["TilesTotalResults"])),
                    totalRecordsDisplay = string.Format("{0:n0}", Convert.ToInt64(TempData["TilesTotalResultsDisplay"])),
                    currentRecords = string.Format("{0:n0}", Convert.ToInt64(TempData["FromRecordTilesID"])),
                    currentRecordsDisplay = string.Format("{0:n0}", Convert.ToInt64(TempData["FromRecordTilesIDDisplay"]))
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
                List<IQArchive_Filter> lstCategoryFilter = iQArchieveLogic.GetCategoryFilter(sessionInformation.ClientGUID.ToString(), p_CustomerGuid, p_FromDate, p_ToDate, p_SubMediaType, p_SearchTerm, p_CategoryGUID, sinceID);

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
        public JsonResult LoadClipPlayer(string ClipID)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();
                string ServiceBaseURL = Convert.ToString(ConfigurationManager.AppSettings["ServicesBaseURL"]);
                bool IsPlayFromLocal = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]);


                // Generate Clip Player Object
                string ClipPlayer = UtilityLogic.RenderClipPlayer(ClipID, ServiceBaseURL, IsPlayFromLocal, sessionInformation.ClientGUID.ToString(), Request.Browser.Type);

                // Get Closed Caption from ArchiveClip table
                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                string ClosedCaption = string.Empty;
                IQArchive_ArchiveClipModel clip = iQArchieveLogic.GetArchiveClipByClipID(ClipID);
                if (clip != null)
                {
                    ClosedCaption = Server.HtmlDecode(clip.ClosedCaption);
                }

                var json = new
                {
                    isSuccess = true,
                    clipHTML = ClipPlayer,
                    closedCaption = ClosedCaption
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
        public JsonResult RenderClipObject(string ClipID)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();
                string ToEmail = sessionInformation.Email;
                string ServiceBaseURL = Convert.ToString(ConfigurationManager.AppSettings["ServicesBaseURL"]);
                bool IsPlayFromLocal = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]);
                string PlayerLogo = (sessionInformation.IsClientPlayerLogoActive.HasValue && sessionInformation.IsClientPlayerLogoActive.Value == true && !string.IsNullOrEmpty(sessionInformation.ClientPlayerLogoImage)) ? Convert.ToString(ConfigurationManager.AppSettings["ClipPlayerLogoBasePath"]) + sessionInformation.ClientPlayerLogoImage : string.Empty;


                // Generate Clip Player Object
                string ClipPlayer = UtilityLogic.RenderClipPlayerWithFullHeightWidth(ToEmail, ClipID, ServiceBaseURL, IsPlayFromLocal, PlayerLogo, sessionInformation.ClientGUID.ToString(), Request.Browser.Type);

                var json = new
                {
                    isSuccess = true,
                    clipHTML = ClipPlayer
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
        public ContentResult RefreshTVResults()
        {
            GetRefreshTVResults();
            return SearchLibraryResults(null, null, null, null, null, null, false, null, null);
        }

        private void GetRefreshTVResults()
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();
                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                List<IQArchive_RefreshResultsForTV> lstRefreshResultsForTV = iQArchieveLogic.GetRefreshResultsForTV(sessionInformation.ClientGUID);

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

        [HttpPost]
        public JsonResult InsertGallery(string p_Name, string p_Title, string p_Description, string p_GalleryType, string p_Json)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();
                IQGalleryModel _IQGalleryModel = new IQGalleryModel();
                _IQGalleryModel.Name = p_Name;
                _IQGalleryModel.Title = p_Title;
                _IQGalleryModel.Description = p_Description;
                string result = string.Empty;
                GalleryLogic gallaryLogic = (GalleryLogic)LogicFactory.GetLogic(LogicType.Gallery);
                List<IQGallery> lstIQGallery = new List<IQGallery>();
                lstIQGallery = (List<IQGallery>)Newtonsoft.Json.JsonConvert.DeserializeObject<List<IQGallery>>(p_Json);
                string _xml = string.Empty;
                _xml = Shared.Utility.CommonFunctions.SerializeToXml(lstIQGallery);
                _IQGalleryModel.xml = _xml;
                result = gallaryLogic.InsertGallery(sessionInformation.CustomerGUID, _IQGalleryModel);

                if (result == "-1")
                {
                    return Json(new
                    {
                        isSuccess = false,
                        isExist = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = true
                    });
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult UpdateGallery(Int64 p_ID, string p_Name, string p_Title, string p_Description, string p_Json)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();
                IQGalleryModel _IQGalleryModel = new IQGalleryModel();
                _IQGalleryModel.Name = p_Name;
                _IQGalleryModel.Title = p_Title;
                _IQGalleryModel.Description = p_Description;
                string result = string.Empty;
                GalleryLogic gallaryLogic = (GalleryLogic)LogicFactory.GetLogic(LogicType.Gallery);
                List<IQGallery> lstIQGallery = new List<IQGallery>();
                lstIQGallery = (List<IQGallery>)Newtonsoft.Json.JsonConvert.DeserializeObject<List<IQGallery>>(p_Json);
                string _xml = string.Empty;
                _xml = Shared.Utility.CommonFunctions.SerializeToXml(lstIQGallery);
                _IQGalleryModel.xml = _xml;
                result = gallaryLogic.UpdateGallery(p_ID, sessionInformation.CustomerGUID, _IQGalleryModel);

                if (result == "-1")
                {
                    return Json(new
                    {
                        isSuccess = false,
                        isExist = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = true
                    });
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult DisplayGalleryResults()
        {
            try
            {

                sessionInformation = ActiveUserMgr.GetActiveUser();
                GalleryLogic gallaryLogic = (GalleryLogic)LogicFactory.GetLogic(LogicType.Gallery);
                IQGalleryResult _IQGalleryResult = gallaryLogic.GetGalleryListByCustomerGUID(sessionInformation.CustomerGUID);

                string strHTML = RenderPartialToString(PATH_GalleryResultsPartialView, _IQGalleryResult);
                var json = new
                {
                    isSuccess = true,
                    HTML = strHTML,
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
        public JsonResult GetGalleryByID(Int64 p_ID)
        {
            try
            {

                sessionInformation = ActiveUserMgr.GetActiveUser();
                GalleryLogic gallaryLogic = (GalleryLogic)LogicFactory.GetLogic(LogicType.Gallery);
                IQGalleryModel _IQGalleryModel = gallaryLogic.GetGalleryByID(p_ID, sessionInformation.CustomerGUID);
                var json = new
                {
                    isSuccess = true,
                    gallery = _IQGalleryModel,
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

        public List<IQGalleryItemType> GetBlockTypeList()
        {
            try
            {
                List<IQGalleryItemType> lstIQGalleryItemType = new List<IQGalleryItemType>();
                GalleryLogic gallaryLogic = (GalleryLogic)LogicFactory.GetLogic(LogicType.Gallery);

                lstIQGalleryItemType = gallaryLogic.GetGalleryItemTypeList();
                return lstIQGalleryItemType;
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return null;
            }
        }

        #region Common functions
        private bool HasMoreResults()
        {
            if (Convert.ToInt64(TempData["FromRecordTilesID"]) < Convert.ToInt64(TempData["TilesTotalResults"]))
                return true;
            else
                return false;
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
                    sessionInformation = ActiveUserMgr.GetActiveUser();

                    ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                    IQClient_CustomSettingsModel customSettings = clientLogic.GetClientCustomSettings(sessionInformation.ClientGUID.ToString());
                    TempData["IQClientCustom_Settings"] = customSettings;
                }
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
            }
            finally { TempData.Keep(); }
        }

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
        #endregion
    }
}
