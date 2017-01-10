using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Model;
using IQMedia.Web.Logic;
using PMGSearch;
using System.Configuration;
using System.IO;
using System.Text;
using IQMedia.Web.Logic.Base;
using System.Globalization;
using IQMedia.WebApplication.Utility;
using IQMedia.WebApplication.Models.TempData;
using IQMedia.WebApplication.Config;
using System.Xml.Linq;
using System.Dynamic;
using Newtonsoft.Json;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class ImagiQController : Controller
    {
        //
        // GET: /ImagiQ/
        #region Public Property
        ActiveUser sessionInformation = null;
        ImagiQTempData ImagiQTempData = null;
        string PATH_ResultsPartialView = "~/Views/ImagiQ/_Result.cshtml";
        bool hasMoreResultPage = false;
        bool hasPreviousResultPage = false;
        string recordNumberDesc = string.Empty;

        #endregion

        public ActionResult Index()
        {
            try
            {
                SetTempData(null);
                ImagiQTempData = GetTempData();
                ImagiQTempData.SinceID = null;
                ImagiQTempData.TotalResults = null;

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);

                var manualClipDuration = clientLogic.GetClientManualClipDurationSettings(sessionInformation.ClientGUID);
                Int16 rawMediaPauseSecs = clientLogic.GetClientRawMediaPauseSecs(sessionInformation.ClientGUID);


                SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                bool p_IsAllDmaAllowed;
                bool p_IsAllClassAllowed;
                bool p_IsAllStationAllowed;
                Dictionary<string, object> dicSSP = sspLogic.GetSSPDataByClientGUID(sessionInformation.ClientGUID, out p_IsAllDmaAllowed, out p_IsAllClassAllowed, out p_IsAllStationAllowed, null);
                
                SetTempData(ImagiQTempData);

                List<string> dmaList = ((List<IQ_Dma>)dicSSP["IQ_Dma"]).Select(x => x.Name).ToList();
                List<string> stationAffilList = ((List<Station_Affil>)dicSSP["Station_Affil"]).Select(x => x.Name).ToList();
                List<string> stationIDList = ((List<IQ_Station>)dicSSP["IQ_Station"]).Select(x => x.IQ_Station_ID).ToList();

                Dictionary<string, object> dictResults = ImagiQDefaultResults(null, null, null, dmaList, stationAffilList, stationIDList, "", null, null, false, null, null);

                ViewBag.IsSuccess = true;
                ViewBag.ManualClipDuration = manualClipDuration;
                ViewBag.RawMediaPauseSecs = rawMediaPauseSecs;
                return View(dictResults);

            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                ViewBag.IsSuccess = false;
            }
            finally
            {
                TempData.Keep("ImagiQTempData");
            }
            return View();
        }
        public Dictionary<string, object> ImagiQDefaultResults(DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Logo, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc, List<string> p_Industry, List<string> p_Brand)
        {
            Dictionary<string, object> dictResults = new Dictionary<string, object>();
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                int ResultCount = 0;
                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftPageSize"]);

                ImagiQTempData = GetTempData();
                ImagiQTempData.PageNumber = 0;
                long? sinceID = ImagiQTempData.SinceID;

                ImagiQLogic ImagiQLogic = (ImagiQLogic)LogicFactory.GetLogic(LogicType.ImagiQ);

                dictResults = ImagiQLogic.GetLRResults(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Logo, p_Dma, p_Station, p_IQStationID, p_Class, null, null, p_IsAsc, false, 0, PageSize, p_Industry, p_Brand, ref sinceID, out ResultCount);
                List<IQAgent_TVResultsModel> tvResult = (List<IQAgent_TVResultsModel>)dictResults["Results"];

                hasMoreResultPage = false;
                hasPreviousResultPage = false;

                if (tvResult != null && ResultCount > 0)
                {
                    double totalHit = Convert.ToDouble(ResultCount) / PageSize;
                    if (totalHit > (ImagiQTempData.PageNumber + 1))
                    {
                        hasMoreResultPage = true;

                        recordNumberDesc = (((ImagiQTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ((ImagiQTempData.PageNumber + 1) * PageSize).ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    else
                    {
                        hasMoreResultPage = false;
                        recordNumberDesc = (((ImagiQTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ResultCount.ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                }
                if (ImagiQTempData.PageNumber > 0)
                {
                    hasPreviousResultPage = true;
                }
                ImagiQTempData.HasMoreResultPage = hasMoreResultPage;
                ImagiQTempData.HasPreviousResultPage = hasPreviousResultPage;
                ImagiQTempData.RecordNumber = recordNumberDesc;
                ImagiQTempData.SinceID = sinceID;

                SetTempData(ImagiQTempData);

                string htmlResult = RenderPartialToString(PATH_ResultsPartialView, tvResult);
                dictResults.Add("DefaultHTML", htmlResult);
            }
            catch (IQMedia.Shared.Utility.CustomException ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex, ex.Message);
                dictResults.Add("DefaultHTML", ConfigSettings.Settings.ErrorOccurred);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                dictResults.Add("DefaultHTML", String.Empty);
            }
            finally
            {
                TempData.Keep("ImagiQTempData");
            }
            return dictResults;
        }
        [HttpPost]
        public JsonResult ImagiQSearchResults(DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Logo, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc, bool p_isMarketSort, List<string> p_Industry, List<string> p_Brand)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                int ResultCount = 0;
                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftPageSize"]);
                
                ImagiQTempData = GetTempData();
                ImagiQTempData.PageNumber = 0;
                long? sinceID = ImagiQTempData.SinceID;

                ImagiQLogic ImagiQLogic = (ImagiQLogic)LogicFactory.GetLogic(LogicType.ImagiQ);
                Dictionary<string, object> dictResults = ImagiQLogic.GetLRResults(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Logo, p_Dma, p_Station, p_IQStationID, p_Class, p_Region, p_Country, p_IsAsc, p_isMarketSort, 0, PageSize, p_Industry, p_Brand, ref sinceID, out ResultCount);
                List<IQAgent_TVResultsModel> tvResult = (List<IQAgent_TVResultsModel>)dictResults["Results"];

                hasMoreResultPage = false;
                hasPreviousResultPage = false;

                if (tvResult != null && ResultCount > 0)
                {
                    double totalHit = Convert.ToDouble(ResultCount) / PageSize;

                    if (totalHit > (ImagiQTempData.PageNumber + 1))
                    {
                        hasMoreResultPage = true;
                        recordNumberDesc = (((ImagiQTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ((ImagiQTempData.PageNumber + 1) * PageSize).ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    else
                    {
                        hasMoreResultPage = false;
                        recordNumberDesc = (((ImagiQTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ResultCount.ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                }
                if (ImagiQTempData.PageNumber > 0)
                {
                    hasPreviousResultPage = true;
                }
                ImagiQTempData.HasMoreResultPage = hasMoreResultPage;
                ImagiQTempData.HasPreviousResultPage = hasPreviousResultPage;
                ImagiQTempData.RecordNumber = recordNumberDesc;
                ImagiQTempData.SinceID = sinceID;

                SetTempData(ImagiQTempData);
                string htmlResult = RenderPartialToString(PATH_ResultsPartialView, tvResult);

                return Json(new
                {
                    HTML = htmlResult != null ? htmlResult : "",
                    hasMoreResult = hasMoreResultPage,
                    filters = dictResults,
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
                TempData.Keep("ImagiQTempData");
            }
        }

        [HttpPost]
        public JsonResult ImagiQSearchResultsPaging(DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Logo, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc, bool p_IsNext, bool p_isMarketSort, List<string> p_Industry, List<string> p_Brand)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                ImagiQTempData = GetTempData();

                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftPageSize"]);
                hasMoreResultPage = false;
                hasPreviousResultPage = false;
                int ResultCount = 0;
                Int32 pagenumber = 0;
                long? sinceID = ImagiQTempData.SinceID;

                if (p_IsNext)
                {
                    if (ImagiQTempData.HasMoreResultPage)
                    {
                        pagenumber = ImagiQTempData.PageNumber + 1;
                        ImagiQTempData.PageNumber = pagenumber;
                    }
                    else
                    {
                        return Json(new
                        {
                            isSuccess = false
                        });
                    }
                }
                else
                {
                    if (ImagiQTempData.HasPreviousResultPage)
                    {
                        pagenumber = ImagiQTempData.PageNumber - 1;
                        ImagiQTempData.PageNumber = pagenumber;
                    }
                    else
                    {
                        return Json(new
                        {
                            isSuccess = false
                        });
                    }

                }

                ImagiQLogic ImagiQLogic = (ImagiQLogic)LogicFactory.GetLogic(LogicType.ImagiQ);
                Dictionary<string, object> dictResults = ImagiQLogic.GetLRResults(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Logo, p_Dma, p_Station, p_IQStationID, p_Class, p_Region, p_Country, p_IsAsc, p_isMarketSort, pagenumber * PageSize, PageSize, p_Industry, p_Brand, ref sinceID, out ResultCount);
                List<IQAgent_TVResultsModel> tvResult = (List<IQAgent_TVResultsModel>)dictResults["Results"];

                if (tvResult != null && ResultCount > 0)
                {
                    double totalHit = Convert.ToDouble(ResultCount) / PageSize;
                    if (totalHit > (ImagiQTempData.PageNumber + 1))
                    {
                        hasMoreResultPage = true;
                        recordNumberDesc = (((ImagiQTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ((ImagiQTempData.PageNumber + 1) * PageSize).ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    else
                    {
                        hasMoreResultPage = false;
                        recordNumberDesc = (((ImagiQTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ResultCount.ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                }
                if (pagenumber > 0)
                {
                    hasPreviousResultPage = true;
                }
                else
                {
                    hasPreviousResultPage = false;
                }
                ImagiQTempData.HasMoreResultPage = hasMoreResultPage;
                ImagiQTempData.HasPreviousResultPage = hasPreviousResultPage;
                ImagiQTempData.SinceID = sinceID;
                SetTempData(ImagiQTempData);

                string htmlResult = RenderPartialToString(PATH_ResultsPartialView, tvResult);

                return Json(new
                {               

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
                TempData.Keep("ImagiQTempData");
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

        [HttpPost]
        public ContentResult GetChart(string IQ_CC_KEY, Guid RAW_MEDIA_GUID)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();

                ImagiQLogic ImagiQLogic = (ImagiQLogic)LogicFactory.GetLogic(LogicType.ImagiQ);
                List<ImagiQLogoModel> lstLogoHits = ImagiQLogic.GetLRResultsByGuid(RAW_MEDIA_GUID);
                List<string> yAxisCompanies = new List<string>();
                List<string> yAxisLogoPaths = new List<string>();
                string LRResult_string = ImagiQLogic.LRHighLineChart(lstLogoHits, out yAxisCompanies, out yAxisLogoPaths);

                bool HasLRResults = lstLogoHits != null && lstLogoHits.Count > 0;

                IQTimeSync_DataLogic iQTimeSync_DataLogic = (IQTimeSync_DataLogic)LogicFactory.GetLogic(LogicType.IQTimeSync_Data);
                List<IQTimeSync_DataModel> lstiQTimeSync_DataModel = iQTimeSync_DataLogic.GetTimeSyncDataByIQCCKeyAndCustomerGuid(IQ_CC_KEY, sessionInformation.CustomerGUID);

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

                dynamic jsonResult = new ExpandoObject();
                jsonResult.yAxisCompanies = yAxisCompanies;
                jsonResult.yAxisLogoPaths = yAxisLogoPaths;
                jsonResult.LRResultsJson = LRResult_string;
                jsonResult.hasLRResults = HasLRResults;
                jsonResult.lineChartJson = lstiQTimeSync_DataModel;
                jsonResult.isTimeSync = IsTimeSync;
                jsonResult.isSuccess = true;

                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
               
                return Content(IQMedia.WebApplication.Utility.CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep("KantorMediaTempData");
            }
        }

        #region Utility

        public ImagiQTempData GetTempData()
        {
            ImagiQTempData = TempData["ImagiQTempData"] != null ? (ImagiQTempData)TempData["ImagiQTempData"] : new ImagiQTempData();
            return ImagiQTempData;
        }

        public void SetTempData(ImagiQTempData p_ImagiQTempData)
        {
            TempData["ImagiQTempData"] = p_ImagiQTempData;
            TempData.Keep("ImagiQTempData");
        }

        #endregion

    }
}
