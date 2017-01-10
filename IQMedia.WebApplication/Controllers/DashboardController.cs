using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using HtmlAgilityPack;
using IQMedia.Model;
using IQMedia.Shared.Utility;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.WebApplication.Config;
using IQMedia.WebApplication.Models;
using IQMedia.WebApplication.Models.TempData;
using IQMedia.WebApplication.Controllers;
using WebAppUtil = IQMedia.WebApplication.Utility;
using WebAppCommon = IQMedia.WebApplication.Utility.CommonFunctions;
using Newtonsoft.Json;
using HiQPdf;
using IQCommon.Model;
using System.Reflection;
using System.Globalization;
using System.Web.UI.HtmlControls;
using WebControls = System.Web.UI.WebControls;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/
        #region Public Property

        ActiveUser sessionInformation = null;
        string PATH_DashboardBroadCastPartialView = "~/Views/Dashboard/_Media.cshtml";
        string PATH_DashboardOverviewPartialView = "~/Views/Dashboard/_Overview.cshtml";
        string PATH_DashboardTopBroadcastDMA = "~/Views/Dashboard/_TopBroadcastDMA.cshtml";
        string PATH_DashboardTopBroadcastStation = "~/Views/Dashboard/_TopBroadcastStation.cshtml";
        string PATH_DashboardTopOnlineNewsDMA = "~/Views/Dashboard/_TopOnlineNewsDMA.cshtml";
        string PATH_DashboardTopOnlineNewsSites = "~/Views/Dashboard/_TopOnlineNewsSites.cshtml";
        string PATH_DashboardTopBroadcastCountries = "~/Views/Dashboard/_TopBroadcastCountries.cshtml";

        string PATH_DashboardReportsOverview = "~/Views/Dashboard/Reports/_Overview.cshtml";
        List<string> topListNS = new List<string>();
        Dictionary<string, string> topDictNS = new Dictionary<string, string>();
        #endregion

        public ActionResult Index()
        {
            try
            {
                // Clear out temp data used by other pages except Feeds and Analytics, due to Build Dashboard functionality
                if (TempData.ContainsKey("DiscoveryTempData")) { TempData["DiscoveryTempData"] = null; }
                if (TempData.ContainsKey("TimeShiftTempData")) { TempData["TimeShiftTempData"] = null; }
                if (TempData.ContainsKey("TAdsTempData")) { TempData["TAdsTempData"] = null; }
                if (TempData.ContainsKey("SetupTempData")) { TempData["SetupTempData"] = null; }
                if (TempData.ContainsKey("GlobalAdminTempData")) { TempData["GlobalAdminTempData"] = null; } 

                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                List<IQAgent_SearchRequestModel> lstIQAgent_SearchRequestModel = iQAgentLogic.SelectIQAgentSearchRequestByClientGuid(sessionInformation.ClientGUID.ToString());
                TempData["Agents"] = lstIQAgent_SearchRequestModel;

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                IQClient_CustomSettingsModel customSettings = clientLogic.GetClientCustomSettings(sessionInformation.ClientGUID.ToString());
                TempData["CustomSettings"] = customSettings;

                ThirdPartyLogic thirdPartyLogic = (ThirdPartyLogic)LogicFactory.GetLogic(LogicType.ThirdParty);
                List<ThirdPartyDataTypeModel> lstThirdPartyDataTypeModel = thirdPartyLogic.GetThirdPartyDataTypesWithCustomerSelection(sessionInformation.CustomerGUID);
                TempData["ThirdPartyDataTypes"] = lstThirdPartyDataTypeModel;

                DataImportLogic dataImportLogic = (DataImportLogic)LogicFactory.GetLogic(LogicType.DataImport);
                DataImportClientModel dataImportClient = dataImportLogic.GetDataImportClient(sessionInformation.ClientGUID);
                TempData["DataImportClient"] = dataImportClient;

                Dictionary<string, object> dictViewModel = new Dictionary<string, object>();
                dictViewModel.Add("Agents", lstIQAgent_SearchRequestModel);
                dictViewModel.Add("ThirdPartyDataTypes", lstThirdPartyDataTypeModel);
                dictViewModel.Add("HasThirdPartyDataAccess", sessionInformation.IsThirdPartyData);
                dictViewModel.Add("UseCustomerEmailAsDefault", customSettings.UseCustomerEmailDefault.Value);
                dictViewModel.Add("DefaultEmailSender", customSettings.UseCustomerEmailDefault.Value ? sessionInformation.Email : ConfigurationManager.AppSettings["Sender"]);
                dictViewModel.Add("UseClientSpecificData", dataImportClient != null && sessionInformation.IsClientSpecificData);

                #region Cohort Model
                CohortLogic cohortLogic = (CohortLogic)LogicFactory.GetLogic(LogicType.Cohort);
                var cohorts = cohortLogic.GetAllCohorts();
                // TODO - need way to get cohorts for a client
                dictViewModel.Add("IndustryCohorts", cohorts);
                // TODO - need way to determine "default" cohort for a client
                dictViewModel.Add("DefaultCohort", cohorts[1]);
                #endregion

                ViewBag.MaxEmailAddresses = System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"];
                ViewBag.IsSuccess = true;
                return View(dictViewModel);
            }
            catch (Exception exception)
            {
                ViewBag.IsSuccess = false;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return View();
            }
            finally
            {
                TempData.Keep();
            }
        }
        
        /*
        [HttpPost]
        public ContentResult SummaryReportResults(DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_SearchRequestIDs, Int16 p_SearchType)
        {


            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                DateTime FromDate;
                DateTime ToDate;

                if (p_FromDate == null)
                {
                    FromDate = DateTime.Today.AddDays(-30);
                    ToDate = DateTime.Today;
                }
                else
                {
                    FromDate = Convert.ToDateTime(p_FromDate);
                    ToDate = Convert.ToDateTime(p_ToDate);
                }

                IQAgent_DashBoardPrevSummaryModel PrevIQAgentSummary = null;
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                List<SummaryReportModel> listOfSummaryReportData = GetSummaryReportData(sessionInformation.ClientGUID, FromDate, ToDate, p_SearchType, p_SearchRequestIDs, p_ThirdPartyDataTypeIDs, out PrevIQAgentSummary);

                return GetJsonSummaryWise(listOfSummaryReportData, FromDate, ToDate, p_SearchType, p_SearchRequestIDs, PrevIQAgentSummary);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);

                return Content(IQMedia.WebApplication.Utility.CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
        }*/

        /*
        private List<string> MultiLinechart(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate)
        {
            DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
            List<string> lstMultiLineChart = dashboardLogic.MultiLinechart(listOfSummaryReportData, p_FromDate, p_ToDate);
            return lstMultiLineChart;
        }*/

        [HttpPost]
        public ContentResult GetAdhocSummaryData(string mediaIDs, string source, DateTime? fromDate, DateTime? toDate, List<string> searchRequestID, List<string> mediumTypes, string keyword, short? sentiment, short? prominenceValue, bool? isProminenceAudience, bool? isOnlyParents, bool? isRead, long? sinceID, List<string> dmaIds,
                                                    bool? isHeardFilter, bool? isSeenFilter, bool? isPaidFilter, bool? isEarnedFilter, bool? usePESHFilters, string showTitle, List<int> dayOfWeek, List<int> timeOfDay, bool? isHour, bool? isMonth)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                List<IQAgent_DaySummaryModel> listOfSummaryReportData = null;
                bool isValidResponse = true;

                if (!isOnlyParents.HasValue && !sinceID.HasValue)
                {
                    string mediaIDXml = null;
                    if (mediaIDs != null)
                    {
                        XDocument doc = new XDocument(new XElement("list", from i in mediaIDs.Split(',') select new XElement("item", new XAttribute("id", i))));
                        mediaIDXml = doc.ToString();
                    }

                    DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
                    listOfSummaryReportData = dashboardLogic.GetAdhocSummaryData(mediaIDXml, source, null, sessionInformation.ClientGUID, sessionInformation.MediaTypes).ListOfIQAgentSummary;
                }
                else
                {
                    if (fromDate.HasValue && toDate.HasValue)
                    {
                        fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);
                        if ((!isHour.HasValue || !isHour.Value) && (!isMonth.HasValue || !isMonth.Value))
                        {
                            toDate = toDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                        toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                    }
                    else
                    {
                        fromDate = Utility.CommonFunctions.GetGMTandDSTTime(DateTime.Now.Date.AddMonths(-3));
                        toDate = Utility.CommonFunctions.GetGMTandDSTTime(DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
                    }   

                    List<string> excludeMediumTypes = new List<string>();
                    if (mediumTypes == null || mediumTypes.Count == 0)
                    {
                        foreach (var mt in sessionInformation.MediaTypes.Where(m=>m.TypeLevel == 2 && !m.HasAccess))
                        {
                            excludeMediumTypes.Add(mt.SubMediaType);
                        }
                    }
                    
                    IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                    string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.FE.ToString(), null, null);

                    List<Task> lstTask = new List<Task>();
                    var tokenSource = new CancellationTokenSource();
                    var token = tokenSource.Token;
                    Dictionary<string, object> dictResults = new Dictionary<string, object>();
                    dictResults.Add("IsValid", false);
                    
                    lstTask.Add(Task<Dictionary<string, object>>.Factory.StartNew((object obj) =>
                        iQAgentLogic.SearchDashboardResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, (mediaIDs != null ? mediaIDs.Split(',').ToList() : null), fromDate, toDate, searchRequestID, mediumTypes, excludeMediumTypes, keyword, sentiment, prominenceValue, isProminenceAudience.Value,
                                                                isOnlyParents.Value, isRead, sinceID, pmgUrl, token, dictResults, dmaIds, isHeardFilter, isSeenFilter, isPaidFilter, isEarnedFilter, usePESHFilters, showTitle, dayOfWeek, timeOfDay),
                            dictResults));

                    try
                    {
                        Task.WaitAll(lstTask.ToArray(), Convert.ToInt32(ConfigurationManager.AppSettings["MaxFeedsRequestDuration"]), token);
                        tokenSource.Cancel();
                    }
                    catch (AggregateException ex)
                    {
                        IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                    }
                    catch (Exception ex)
                    {
                        IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                    }

                    foreach (var tsk in lstTask)
                    {
                        dictResults = ((Task<Dictionary<string, object>>)tsk).Result;
                    }

                    if ((bool)dictResults["IsValid"])
                    {
                        listOfSummaryReportData = (List<IQAgent_DaySummaryModel>)dictResults["Results"];

                        // Set MediaType

                        foreach (var sm in listOfSummaryReportData)
                        {
                            sm.MediaType = sessionInformation.MediaTypes.Where(mt => string.Compare(mt.SubMediaType, sm.SubMediaType, true) == 0 && mt.HasAccess).Single().MediaType;
                        }
                    }
                    else
                    {
                        isValidResponse = false;
                    }
                }

                if (isValidResponse)
                {
                    DateTime fDate;
                    DateTime tDate;
                    Int16 searchType;
                    bool isUGCEnabled = false;                    

                    // Only display UGC data if coming from Library/Reports
                    if (source.ToLower() == "library" || source.ToLower() == "report")
                    {
                        isUGCEnabled = sessionInformation.Isv4UGCAccess;
                    }

                    fDate = listOfSummaryReportData.Min(x => x.DayDate);
                    tDate = listOfSummaryReportData.Max(x => x.DayDate);

                    if ((tDate - fDate).Days > 2)
                    {
                        searchType = 1;
                    }
                    else
                    {
                        searchType = 0;

                        // The line chart doesn't render correctly when only displaying a single data point
                        if (fDate == tDate)
                        {
                            tDate = tDate.AddHours(1);
                        }
                    }

                    List<SummaryReportModel> lstSummaryReport = listOfSummaryReportData.Select(s => new SummaryReportModel()
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

                    return GetJsonForAdhocSummary(lstSummaryReport, fDate, tDate, searchType, sessionInformation, isUGCEnabled);
                }
                else
                {
                    dynamic json = new ExpandoObject();
                    json.isSuccess = false;
                    json.errorMessage = "Data not available. Please try again.";

                    return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8); 
                }
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);

                dynamic json = new ExpandoObject();
                json.isSuccess = false;
                json.errorMessage = ConfigSettings.Settings.ErrorOccurred;

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);           
            }
        }

        [HttpPost]
        public JsonResult SaveThirdPartyDataTypeSelections(List<string> dataTypeIDs)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                ThirdPartyLogic thirdPartyLogic = (ThirdPartyLogic)LogicFactory.GetLogic(LogicType.ThirdParty);
                int success = thirdPartyLogic.SaveThirdPartyDataTypeSelections(sessionInformation.CustomerGUID, dataTypeIDs);

                return Json(new
                {
                    isSuccess = success >= 0
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
        }

        #region SummaryReport
        public List<SummaryReportModel> GetSummaryReportData(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, List<string> p_SearchRequestIDs, List<string> p_ThirdPartyDataTypeIDs, out IQAgent_DashBoardPrevSummaryModel iQAgent_DashBoardPrevSummaryModel, List<IQ_MediaTypeModel> p_MediaTypeList)
        {
            try
            {
                string searchRequestXml = null;
                iQAgent_DashBoardPrevSummaryModel = new IQAgent_DashBoardPrevSummaryModel();
                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    XDocument doc = new XDocument(new XElement("list", from i in p_SearchRequestIDs select new XElement("item", new XAttribute("id", i))));
                    searchRequestXml = doc.ToString();
                }

                DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
                List<SummaryReportModel> lstSummaryReport = null;
                if (p_SearchType == 0)
                {
                    IQAgent_DashBoardModel iQAgent_DashBoardModel = dashboardLogic.GetHourSummaryDataHourWise(p_ClientGUID, p_FromDate, p_ToDate, null, searchRequestXml, p_MediaTypeList);
                    iQAgent_DashBoardPrevSummaryModel = iQAgent_DashBoardModel.PrevIQAgentSummary;
                    lstSummaryReport = iQAgent_DashBoardModel.ListOfIQAgentSummary.Select(s => new SummaryReportModel()
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
                        DefaultMediaType = p_MediaTypeList.Where(mt => string.Compare(mt.MediaType, s.MediaType,true) == 0).Count() > 0

                    }).ToList();
                }
                else if (p_SearchType == 1)
                {
                    IQAgent_DashBoardModel iQAgent_DashBoardModel = dashboardLogic.GetDaySummaryDataDayWise(p_ClientGUID, p_FromDate, p_ToDate, null, searchRequestXml, p_MediaTypeList);
                    iQAgent_DashBoardPrevSummaryModel = iQAgent_DashBoardModel.PrevIQAgentSummary;
                    lstSummaryReport = iQAgent_DashBoardModel.ListOfIQAgentSummary.Select(s => new SummaryReportModel()
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
                        DefaultMediaType = p_MediaTypeList.Where(mt => string.Compare(mt.MediaType, s.MediaType, true) == 0).Count() > 0

                    }).ToList();

                }
                else if (p_SearchType == 3)
                {
                    IQAgent_DashBoardModel iQAgent_DashBoardModel = dashboardLogic.GetDaySummaryDataMonthWise(p_ClientGUID, p_FromDate, p_ToDate, null, searchRequestXml, p_MediaTypeList);
                    iQAgent_DashBoardPrevSummaryModel = iQAgent_DashBoardModel.PrevIQAgentSummary;
                    lstSummaryReport = iQAgent_DashBoardModel.ListOfIQAgentSummary.Select(s => new SummaryReportModel()
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
                        DefaultMediaType = p_MediaTypeList.Where(mt => string.Compare(mt.MediaType, s.MediaType, true) == 0).Count() > 0

                    }).ToList();
                }

                if (p_ThirdPartyDataTypeIDs != null && p_ThirdPartyDataTypeIDs.Count > 0)
                {
                    ThirdPartyLogic thirdPartyLogic = (ThirdPartyLogic)LogicFactory.GetLogic(LogicType.ThirdParty);
                    List<ThirdPartyDataTypeModel> lstDataTypeModels = (List<ThirdPartyDataTypeModel>)TempData["ThirdPartyDataTypes"];
                    foreach (ThirdPartyDataTypeModel dataTypeModel in lstDataTypeModels.Where(w => w.HasAccess && p_ThirdPartyDataTypeIDs.Contains(w.ID.ToString())))
                    {
                        if (p_SearchType != 0 || dataTypeModel.UseHourData)
                        {
                            List<SummaryReportModel> lstThirdPartySummaries = thirdPartyLogic.GetThirdPartySummaryData(p_ClientGUID, dataTypeModel, p_FromDate, p_ToDate, p_SearchType, p_SearchRequestIDs);
                            if (lstThirdPartySummaries != null && lstThirdPartySummaries.Count > 0)
                            {
                                lstSummaryReport.AddRange(lstThirdPartySummaries);
                            }
                            else
                            {
                                // If no data exists for the given time range, add a dummy record so that a series is still created
                                lstSummaryReport.Add(new SummaryReportModel()
                                {
                                    GMT_DateTime = DateTime.Now,
                                    Number_Docs = 0,
                                    ThirdPartyDataTypeID = dataTypeModel.ID
                                });
                            }
                        }
                    }
                }

                return lstSummaryReport;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TempData.Keep();
            }
        }
        #endregion

        #region BroadCast

        [HttpPost]
        public ContentResult GetMediumData(DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_SearchRequestIDs, string p_Medium, Int16 p_SearchType, bool p_IsDefaultLoad, List<string> p_ThirdPartyDataTypeIDs)
        {
            /*
             * 0- Hourly
             * 1 - Day
             * 2 - Week
             * 3 - Month
             */
            try
            {

                DateTime FromDate;
                DateTime ToDate;

                if (p_FromDate == null || p_IsDefaultLoad)
                {
                    FromDate = DateTime.Today.AddDays(-30);
                    ToDate = DateTime.Today;
                }
                else
                {
                    FromDate = Convert.ToDateTime(p_FromDate);
                    ToDate = Convert.ToDateTime(p_ToDate);
                }

                if (p_SearchType == 0)
                {
                    if (p_IsDefaultLoad)
                    {
                        ToDate = DateTime.Today.AddHours(23).AddMinutes(59);
                        FromDate = DateTime.Today.AddDays(-1);
                    }
                    else
                    {
                        ToDate = ToDate.Date.AddHours(23).AddMinutes(59);
                    }
                }
                else if (p_SearchType == 3)
                {
                    FromDate = new DateTime(FromDate.Year, FromDate.Month, 1);
                    ToDate = new DateTime(ToDate.Year, ToDate.Month, DateTime.DaysInMonth(ToDate.Year, ToDate.Month));
                }
   
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                if (p_Medium.ToLower() == IQMedia.Shared.Utility.CommonFunctions.DashBoardMediumType.Overview.ToString().ToLower())
                {
                    IQAgent_DashBoardPrevSummaryModel PrevIQAgentSummary = null;
                    List<SummaryReportModel> listOfSummaryReportData = GetSummaryReportData(sessionInformation.ClientGUID, FromDate, ToDate, p_SearchType, p_SearchRequestIDs, p_ThirdPartyDataTypeIDs, out PrevIQAgentSummary, sessionInformation.MediaTypes);

                    return GetJsonSummaryWise(listOfSummaryReportData, FromDate, ToDate, p_SearchType, p_SearchRequestIDs, PrevIQAgentSummary,sessionInformation);
                }
                else if (p_Medium.ToLower() == IQMedia.Shared.Utility.CommonFunctions.DashBoardMediumType.ClientSpecific.ToString().ToLower())
                {
                    if (TempData["DataImportClient"] != null)
                    {
                        DataImportClientModel clientModel = (DataImportClientModel)TempData["DataImportClient"];

                        /* Call the appropriate method based on the GetResultsMethod field of the IQDataImport_Clients table
                         * The method must accept the following parameters in this order:
                         *      - From Date
                         *      - To Date
                         *      - Search Request IDs (as List<string>)
                         *      - Date Interval Type (as short)
                         *      - Master list of agents (TempData can't be accessed in a method called via reflection)
                         *      
                         * The method must return a Dictionary<string, object> object. It should contain the following two entries:
                         *      - Key: "JsonResult" - An ExpandoObject that will be serialized and returned to the client
                         *      - Key: "ViewModel" - An object that will be used as the model for the view
                         */
                        Type type = this.GetType();
                        MethodInfo methodInfo = type.GetMethod(clientModel.GetResultsMethod);
                        object classInstance = Activator.CreateInstance(type, null);
                        object[] parameters = new object[] { FromDate, ToDate, p_SearchRequestIDs, p_SearchType, (List<IQAgent_SearchRequestModel>)TempData["Agents"] };

                        Dictionary<string, object> dictResults = (Dictionary<string, object>)methodInfo.Invoke(classInstance, parameters);

                        dynamic jsonResult = new ExpandoObject();
                        jsonResult.isSuccess = false;
                        if (dictResults.ContainsKey("JsonResult") && dictResults.ContainsKey("ViewModel"))
                        {
                            jsonResult = (ExpandoObject)dictResults["JsonResult"];
                            jsonResult.isSuccess = true;
                            jsonResult.fromDate = FromDate.ToShortDateString();
                            jsonResult.toDate = ToDate.ToShortDateString();
                            jsonResult.CategoryDescription = "My Data";
                            jsonResult.HTML = RenderPartialToString(clientModel.ViewPath, dictResults["ViewModel"]);
                        }

                        return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
                    }
                    else
                    {
                        throw new Exception("No DataImportClient object could be found in TempData.");
                    }
                }
                else
                {
                    IQAgent_DashBoardModel iQAgent_DashBoardModel = GetDaySummaryMediumWise(sessionInformation.ClientGUID, FromDate, ToDate, p_Medium, p_SearchType, p_SearchRequestIDs, sessionInformation.MediaTypes);

                    var mediaType = sessionInformation.MediaTypes.Where(mt => mt.TypeLevel == 1 && string.Compare(p_Medium, mt.MediaType, true) == 0).Single();

                    if (mediaType.HasAccess)
                    {
                        return GetJsonMediumWise(mediaType, iQAgent_DashBoardModel, FromDate, ToDate, p_SearchType, p_SearchRequestIDs, sessionInformation); 
                    }
                    else
                    {
                        throw new CustomException("User doesn't have requested media type \""+ mediaType.MediaType+"\" access.");
                    }
                }
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);

                return Content(IQMedia.WebApplication.Utility.CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep();
            }
        }

        #endregion

        #region Client Specific Data

        #region Sony

        public Dictionary<string, object> GetSonyResults(DateTime p_FromDate, DateTime p_ToDate, List<string> p_SearchRequestIDs, Int16 p_SearchType, List<IQAgent_SearchRequestModel> p_lstAgents)
        {
            try
            {
                Dictionary<string, object> dictResults = new Dictionary<string, object>();
                SummaryReportMulti lineChart = GetSonyLineChart(p_FromDate, p_ToDate, p_SearchRequestIDs, p_SearchType, p_lstAgents, null, null, null, "Artist");

                if (lineChart != null)
                {
                    dynamic jsonResult = new ExpandoObject();
                    jsonResult.jsonMediaRecord = lineChart.MediaRecords;
                    jsonResult.jsonSubMediaRecord = lineChart.SubMediaRecords;

                    NumberFormatInfo numInfo = new NumberFormatInfo();
                    numInfo.NumberGroupSeparator = String.Empty; // Format the number without comma separators
                    jsonResult.totalHits = Decimal.Parse(lineChart.TotalNumOfHits).ToString("N0", numInfo);

                    dictResults["ViewModel"] = null;
                    dictResults["JsonResult"] = jsonResult;
                }

                return dictResults;
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return new Dictionary<string,object>();
            }
        }

        [HttpPost]
        public ContentResult GetSonyChart(DateTime p_FromDate, DateTime p_ToDate, List<string> p_SearchRequestIDs, Int16 p_SearchType, List<string> p_Artists, List<string> p_Albums, List<string> p_Tracks, string p_TableType)
        {
            try
            {
                List<IQAgent_SearchRequestModel> lstAgents = (List<IQAgent_SearchRequestModel>)TempData["Agents"];
                DateTime FromDate;
                DateTime ToDate;

                if (p_FromDate == null || p_ToDate == null)
                {
                    FromDate = DateTime.Today.AddDays(-30);
                    ToDate = DateTime.Today;
                }
                else
                {
                    FromDate = Convert.ToDateTime(p_FromDate);
                    ToDate = Convert.ToDateTime(p_ToDate);
                }

                if (p_SearchType == 0)
                {
                    ToDate = ToDate.Date.AddHours(23).AddMinutes(59);
                }
                else if (p_SearchType == 3)
                {
                    FromDate = new DateTime(FromDate.Year, FromDate.Month, 1);
                    ToDate = new DateTime(ToDate.Year, ToDate.Month, DateTime.DaysInMonth(ToDate.Year, ToDate.Month));
                }

                SummaryReportMulti lineChart = GetSonyLineChart(FromDate, ToDate, p_SearchRequestIDs, p_SearchType, lstAgents, p_Artists, p_Albums, p_Tracks, p_TableType);
                if (lineChart != null)
                {
                    dynamic jsonResult = new ExpandoObject();
                    jsonResult.jsonMediaRecord = lineChart.MediaRecords;
                    jsonResult.jsonSubMediaRecord = lineChart.SubMediaRecords;
                    jsonResult.isSuccess = true;

                    return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
                }
                else
                {
                    throw new Exception("Error occurred while updating Sony line chart");
                }
            }
            catch (Exception ex)
            {
                Utility.CommonFunctions.WriteException(ex);
                return Content(IQMedia.WebApplication.Utility.CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpPost]
        public JsonResult GetSonyTable(DateTime p_FromDate, DateTime p_ToDate, int p_SearchType, List<string> p_SearchRequestIDs, string p_TableType, int p_PageNumber)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                DateTime FromDate;
                DateTime ToDate;
                int numTotalRecords;
                int pageSize = 20;

                if (p_FromDate == null || p_ToDate == null)
                {
                    FromDate = DateTime.Today.AddDays(-30);
                    ToDate = DateTime.Today;
                }
                else
                {
                    FromDate = Convert.ToDateTime(p_FromDate);
                    ToDate = Convert.ToDateTime(p_ToDate);
                }

                if (p_SearchType == 0)
                {
                    ToDate = ToDate.Date.AddHours(23).AddMinutes(59);
                }
                else if (p_SearchType == 3)
                {
                    FromDate = new DateTime(FromDate.Year, FromDate.Month, 1);
                    ToDate = new DateTime(ToDate.Year, ToDate.Month, DateTime.DaysInMonth(ToDate.Year, ToDate.Month));
                }
                
                DataImportLogic dataImportLogic = (DataImportLogic)LogicFactory.GetLogic(LogicType.DataImport);
                List<SonyTableModel> lstTableData = dataImportLogic.GetSonyTableData(sessionInformation.ClientGUID, FromDate, ToDate, p_SearchRequestIDs, pageSize, p_PageNumber * pageSize, p_TableType, out numTotalRecords);

                string rowClass = p_TableType.ToLower() + "Row";

                HtmlTable table = new HtmlTable();
                table.Attributes.Add("class", "table clearBorders");
                table.Style.Add("font-size", "12px");

                #region Header Row
                HtmlTableRow tr = new HtmlTableRow();
                tr.Attributes.Add("class", "headerRow " + rowClass);
                table.Rows.Add(tr);

                // Checkbox Cell
                HtmlTableCell td = new HtmlTableCell();
                td.Style.Add("width", "20px");
                tr.Cells.Add(td);

                // Artist Cell
                td = new HtmlTableCell();
                td.Attributes.Add("class", "artist");
                td.InnerText = "Artist";
                tr.Cells.Add(td);

                // Album Cell
                if (p_TableType != "Artist")
                {
                    td = new HtmlTableCell();
                    td.Attributes.Add("class", "album");
                    td.InnerText = "Album";
                    tr.Cells.Add(td);
                }

                // Track Cell
                if (p_TableType == "Track")
                {
                    td = new HtmlTableCell();
                    td.Attributes.Add("class", "track");
                    td.InnerText = "Track";
                    tr.Cells.Add(td);
                }

                // Total Count Cell
                td = new HtmlTableCell();
                td.Attributes.Add("class", "numericCol");
                td.InnerText = "Total";
                tr.Cells.Add(td);

                // Spotify Count Cell
                td = new HtmlTableCell();
                td.Attributes.Add("class", "numericCol");
                td.InnerText = "Spotify";
                tr.Cells.Add(td);

                // ITunes Album Count Cell
                if (p_TableType != "Track")
                {
                    td = new HtmlTableCell();
                    td.Attributes.Add("class", "numericCol");
                    td.InnerText = "iTunes Album";
                    tr.Cells.Add(td);
                }

                // ITunes Track Count Cell
                if (p_TableType != "Album")
                {
                    td = new HtmlTableCell();
                    td.Attributes.Add("class", "numericCol");
                    td.InnerText = "iTunes Track";
                    tr.Cells.Add(td);
                }

                // Apple Streaming Count Cell
                td = new HtmlTableCell();
                td.Attributes.Add("class", "numericCol");
                td.InnerText = "Apple Music";
                tr.Cells.Add(td);
                #endregion

                foreach (SonyTableModel data in lstTableData)
                {
                    tr = new HtmlTableRow();
                    tr.Attributes.Add("class", "dataRow " + rowClass);
                    table.Rows.Add(tr);

                    // Checkbox Cell
                    td = new HtmlTableCell();
                    HtmlInputCheckBox chk = new HtmlInputCheckBox();
                    chk.ID = "chk" + data.RowID;
                    chk.Attributes.Add("rowID", data.RowID.ToString());
                    td.Controls.Add(chk);
                    tr.Cells.Add(td);

                    // Artist Cell
                    td = new HtmlTableCell();
                    td.ID = "tdArtist" + data.RowID;
                    td.Attributes.Add("class", "artist");
                    td.Attributes.Add("title", data.Artist);
                    td.InnerText = data.Artist;
                    tr.Cells.Add(td);

                    // Album Cell
                    if (p_TableType != "Artist")
                    {
                        td = new HtmlTableCell();
                        td.ID = "tdAlbum" + data.RowID;
                        td.Attributes.Add("class", "album");
                        td.Attributes.Add("title", data.Album);
                        td.InnerText = data.Album;
                        tr.Cells.Add(td);
                    }

                    // Track Cell
                    if (p_TableType == "Track")
                    {
                        td = new HtmlTableCell();
                        td.ID = "tdTrack" + data.RowID;
                        td.Attributes.Add("class", "track");
                        td.Attributes.Add("title", data.Track);
                        td.InnerText = data.Track;
                        tr.Cells.Add(td);
                    }

                    // Total Count Cell
                    td = new HtmlTableCell();
                    td.Attributes.Add("class", "numericCol");
                    td.InnerText = data.TotalCount.ToString("N0");
                    tr.Cells.Add(td);

                    // Spotify Count Cell
                    td = new HtmlTableCell();
                    td.Attributes.Add("class", "numericCol");
                    td.InnerText = data.SpotifyCount.ToString("N0");
                    tr.Cells.Add(td);

                    // ITunes Album Count Cell
                    if (p_TableType != "Track")
                    {
                        td = new HtmlTableCell();
                        td.Attributes.Add("class", "numericCol");
                        td.InnerText = data.ITunesAlbumCount.ToString("N0");
                        tr.Cells.Add(td);
                    }

                    // ITunes Track Count Cell
                    if (p_TableType != "Album")
                    {
                        td = new HtmlTableCell();
                        td.Attributes.Add("class", "numericCol");
                        td.InnerText = data.ITunesTrackCount.ToString("N0");
                        tr.Cells.Add(td);
                    }

                    // Apple Music Count Cell
                    td = new HtmlTableCell();
                    td.Attributes.Add("class", "numericCol");
                    td.InnerText = data.AppleMusicCount.ToString("N0");
                    tr.Cells.Add(td);
                }

                StringWriter tableStrWriter = new StringWriter();
                table.RenderControl(new System.Web.UI.HtmlTextWriter(tableStrWriter));
                Log4NetLogger.Info("PageNumber: " + p_PageNumber + "    TotalRecords: " + numTotalRecords);
                return Json(new 
                {
                    isSuccess = true,
                    HTML = tableStrWriter.ToString(),
                    startIndex = lstTableData != null && lstTableData.Count > 0 ? lstTableData.Select(s => s.RowID).Min() : 0,
                    endIndex = lstTableData != null && lstTableData.Count > 0 ? lstTableData.Select(s => s.RowID).Max() : 0,
                    numTotalRecords = numTotalRecords,
                    hasMoreRecords = ((p_PageNumber + 1) * pageSize) < numTotalRecords
                });
            }
            catch (Exception ex)
            {
                Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
        }

        [HttpPost]
        public JsonResult GetSonyExportData(DateTime p_FromDate, DateTime p_ToDate, int p_SearchType, List<string> p_SearchRequestIDs, string p_TableType)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                DateTime FromDate;
                DateTime ToDate;
                bool IsFileGenerated = false;

                if (p_FromDate == null || p_ToDate == null)
                {
                    FromDate = DateTime.Today.AddDays(-30);
                    ToDate = DateTime.Today;
                }
                else
                {
                    FromDate = Convert.ToDateTime(p_FromDate);
                    ToDate = Convert.ToDateTime(p_ToDate);
                }

                if (p_SearchType == 0)
                {
                    ToDate = ToDate.Date.AddHours(23).AddMinutes(59);
                }
                else if (p_SearchType == 3)
                {
                    FromDate = new DateTime(FromDate.Year, FromDate.Month, 1);
                    ToDate = new DateTime(ToDate.Year, ToDate.Month, DateTime.DaysInMonth(ToDate.Year, ToDate.Month));
                }

                DataImportLogic dataImportLogic = (DataImportLogic)LogicFactory.GetLogic(LogicType.DataImport);
                List<SonyTableModel> lstExportData = dataImportLogic.GetSonyExportData(sessionInformation.ClientGUID, FromDate, ToDate, p_SearchRequestIDs, p_TableType, sessionInformation.MediaTypes);

                string csvData = GetSonyCSVData(lstExportData, p_TableType);
                string TempCSVPath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Dashboard\\CSV\\" + sessionInformation.CustomerGUID + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_Dashboard.csv";

                Session["DownloadDashboardCSVFileName"] = Path.GetFileName("iQMedia_Dashboard_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".csv");

                using (FileStream fs = new FileStream(TempCSVPath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.Write(csvData);
                    }
                }

                if (System.IO.File.Exists(TempCSVPath))
                {
                    IsFileGenerated = true;
                    Session["DashboardCSVFile"] = TempCSVPath;
                }

                var json = new
                {
                    isSuccess = IsFileGenerated,
                    errorMessage = "Data not available. Please try again."
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
        }

        private string GetSonyCSVData(List<SonyTableModel> lstResults, string tableType)
        {
            string DQ = "\"";
            StringBuilder sb = new StringBuilder();

            if (lstResults != null)
            {
                sb.Append("Date,Agent,Media Type,Artist," + (tableType != "Artist" ? "Album," : "") + (tableType == "Track" ? "Track," : "") + "Documents,Hits,Spotify," + (tableType != "Track" ? "iTunes Album," : "") + (tableType != "Album" ? "iTunes Track," : "") + "Apple Music");
                sb.Append(Environment.NewLine);

                foreach (SonyTableModel result in lstResults)
                {
                    sb.Append(result.DayDate.ToShortDateString());
                    sb.Append(",");

                    sb.Append(DQ + result.AgentName.Replace("\"", "\"\"") + DQ);
                    sb.Append(",");

                    sb.Append(result.MediaType);
                    sb.Append(",");

                    sb.Append(DQ + result.Artist.Replace("\"", "\"\"") + DQ);
                    sb.Append(",");

                    if (tableType != "Artist")
                    {
                        sb.Append(DQ + result.Album.Replace("\"", "\"\"") + DQ);
                        sb.Append(",");
                    }

                    if (tableType == "Track")
                    {
                        sb.Append(DQ + result.Track.Replace("\"", "\"\"") + DQ);
                        sb.Append(",");
                    }

                    if (result.MediaType != "Custom")
                    {
                        sb.Append(DQ + result.NumberOfDocs.ToString("N0") + DQ);
                        sb.Append(",");

                        sb.Append(DQ + result.NumberOfHits.ToString("N0") + DQ);
                        sb.Append(",,,,,");
                    }
                    else
                    {
                        sb.Append(",,");

                        sb.Append(DQ + result.SpotifyCount.ToString("N0") + DQ);
                        sb.Append(",");

                        if (tableType != "Track")
                        {
                            sb.Append(DQ + result.ITunesAlbumCount.ToString("N0") + DQ);
                            sb.Append(",");
                        }

                        if (tableType != "Album")
                        {
                            sb.Append(DQ + result.ITunesTrackCount.ToString("N0") + DQ);
                            sb.Append(",");
                        }

                        sb.Append(DQ + result.AppleMusicCount.ToString("N0") + DQ);
                        sb.Append(",");
                    }

                    sb.Append(Environment.NewLine);
                }

                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private SummaryReportMulti GetSonyLineChart(DateTime p_FromDate, DateTime p_ToDate, List<string> p_SearchRequestIDs, Int16 p_SearchType, List<IQAgent_SearchRequestModel> p_lstAgents, List<string> p_Artists, List<string> p_Albums, List<string> p_Tracks, string p_TableType)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                DataImportLogic dataImportLogic = (DataImportLogic)LogicFactory.GetLogic(LogicType.DataImport);
                List<SonySummaryModel> lstSummaryData = dataImportLogic.GetSonySummaryData(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_SearchType, p_SearchRequestIDs, p_Artists, p_Albums, p_Tracks, p_TableType, sessionInformation.MediaTypes);

                Dictionary<long, string> dictSelectedAgents = new Dictionary<long, string>();
                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    dictSelectedAgents = p_lstAgents.Where(w => p_SearchRequestIDs.Contains(w.ID.ToString()))
                                                .Select(s => new { s.ID, s.QueryName })
                                                .ToDictionary(t => t.ID, t => t.QueryName);
                }

                return dataImportLogic.SonyLineChart(lstSummaryData, p_FromDate, p_ToDate, p_SearchType, null, dictSelectedAgents, p_Artists, p_Albums, p_Tracks, sessionInformation.MediaTypes.Where(w => !w.IsArchiveOnly).ToList());
            }
            catch (Exception ex)
            {
                Utility.CommonFunctions.WriteException(ex);
                return null;
            }
        }

        #endregion

        #endregion

        #region Download PDF

        [HttpPost]
        public JsonResult GenerateDashboardPDF()
        {
            try
            {
                Request.InputStream.Position = 0;
                Dictionary<string, object> dictParams;

                using (var sr = new StreamReader(Request.InputStream))
                {
                    dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
                }

                string HTML = dictParams["p_HTML"].ToString();
                string fromDate = dictParams["p_FromDate"].ToString();
                string toDate = dictParams["p_ToDate"].ToString();
                string searchRequests = dictParams["p_SearchRequests"].ToString();

                string html = GetHTMLWithCSSIncluded(HTML, fromDate, toDate, false, searchRequests);

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                string TempPDFPath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Dashboard\\PDF\\" + sessionInformation.CustomerGUID + "_" + DateTimeStamp + ".pdf";

                //string TempPDFPath = "C:\\Logs\\Download\\Dashboard\\PDF\\" + sessionInformation.CustomerGUID + "_" + DateTimeStamp + ".pdf";
                //string TempHTMLPath = "C:\\Logs\\Download\\Dashboard\\HTML\\" + sessionInformation.CustomerGUID + "_" + DateTimeStamp + ".html";

                //using (FileStream fs = System.IO.File.Create(TempHTMLPath))
                //{
                //    Byte[] info = new UTF8Encoding(true).GetBytes(html);
                //    fs.Write(info, 0, info.Length);
                //}

                bool IsFileGenerated = false;

                HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
                htmlToPdfConverter.SerialNumber = ConfigurationManager.AppSettings["HiQPdfSerialKey"];
                htmlToPdfConverter.Document.Margins = new PdfMargins(20);
                htmlToPdfConverter.BrowserWidth = 1000;
                htmlToPdfConverter.ConvertHtmlToFile(html, String.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority), TempPDFPath);

                if (System.IO.File.Exists(TempPDFPath))
                {
                    IsFileGenerated = true;
                    Session["PDFFile"] = TempPDFPath;
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
        }

        [HttpPost]
        public JsonResult SendDashBoardEmail()
        {
            try
            {
                Request.InputStream.Position = 0;

                Dictionary<string, object> dictParams;

                using (var sr = new StreamReader(Request.InputStream))
                {
                    dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
                }

                string HTML = dictParams["p_HTML"].ToString();
                string fromDate = dictParams["p_FromDate"].ToString();
                string toDate = dictParams["p_ToDate"].ToString();
                string fromEmail = dictParams["p_FromEmail"].ToString();
                string toEmail = dictParams["p_ToEmail"].ToString();
                string bccEmail = dictParams["p_BCCEmail"].ToString();
                string subject = dictParams["p_Subject"].ToString();
                string userBody = dictParams["p_UserBody"].ToString();
                string searchRequests = dictParams["p_SearchRequests"].ToString();

                if (toEmail.Split(new char[] { ';' }).Count() <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"]) &&
                        (String.IsNullOrEmpty(bccEmail) || bccEmail.Split(new char[] { ';' }).Count() <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"])))
                {
                    int EmailSendCount = 0;
                    string[] bccEmails = !String.IsNullOrWhiteSpace(bccEmail) ? bccEmail.Split(new char[] { ';' }) : new string[0];

                    string html = GetHTMLWithCSSIncluded(HTML, fromDate, toDate, true, searchRequests);

                    sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                    string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    string TempImagePath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Dashboard\\PDF\\" + sessionInformation.CustomerGUID + "_" + DateTimeStamp + ".jpg";

                    HtmlToImage htmlToImageConverter = new HtmlToImage();
                    htmlToImageConverter.SerialNumber = ConfigurationManager.AppSettings["HiQPdfSerialKey"];
                    htmlToImageConverter.BrowserWidth = 1000;
                    Image img = htmlToImageConverter.ConvertHtmlToImage(html, String.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority))[0];

                    img.Save(TempImagePath);

                    string attachmentId = Path.GetFileName(TempImagePath);
                    userBody = userBody + "<br/>" + "<img src=\"cid:" + attachmentId + "\" alt='Dashboard'/>";

                    StreamReader strmEmailPolicy = new StreamReader(Server.MapPath("~/content/EmailPolicy.txt"));
                    string emailPolicy = strmEmailPolicy.ReadToEnd();
                    strmEmailPolicy.Close();
                    strmEmailPolicy.Dispose();
                    userBody = userBody + emailPolicy;

                    string[] alternetViewsName = new string[1];
                    alternetViewsName[0] = TempImagePath;

                    if (!string.IsNullOrEmpty(toEmail))
                    {
                        foreach (string id in toEmail.Split(new char[] { ';' }))
                        {
                            // send email code

                            if (IQMedia.Shared.Utility.CommonFunctions.SendMail(id, string.Empty, bccEmails, fromEmail, subject, userBody, true, null, alternetViewsName))
                            {
                                EmailSendCount++;
                            }
                        }
                    }

                    img.Dispose();
                    if (System.IO.File.Exists(TempImagePath))
                    {
                        System.IO.File.Delete(TempImagePath);
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
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
        }

        [HttpGet]
        public ActionResult DownloadPDFFile()
        {
            if (Session["PDFFile"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["PDFFile"])))
            {
                string PDFFile = Convert.ToString(Session["PDFFile"]);

                if (System.IO.File.Exists(PDFFile))
                {
                    Session.Remove("PDFFile");
                    return File(PDFFile, "application/pdf", Path.GetFileName(PDFFile));
                }
            }
            return Content(ConfigSettings.Settings.FileNotAvailable);
        }

        [HttpGet]
        public ActionResult DownloadCSVFile()
        {
            if (Session["DashboardCSVFile"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["DashboardCSVFile"])) && Session["DownloadDashboardCSVFileName"] != null)
            {
                string CSVFile = Convert.ToString(Session["DashboardCSVFile"]);
                string DownloadFileName = Convert.ToString(Session["DownloadDashboardCSVFileName"]);

                if (System.IO.File.Exists(CSVFile))
                {
                    Session.Remove("DashboardCSVFile");
                    return File(CSVFile, "application/csv", DownloadFileName);
                }
            }
            return Content(ConfigSettings.Settings.FileNotAvailable);
        }

        #endregion

        #region Compare Province

        public JsonResult CompareProvince(DateTime p_FromDate, DateTime p_ToDate, List<string> p_SearchRequests, List<DashboardDMAChartSelectionModel> p_Provinces, Int16 p_SearchType, string p_Medium)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                string searchRequestXml = null;
                string provinceXml = null;

                if (p_SearchRequests != null && p_SearchRequests.Count > 0)
                {
                    XDocument doc = new XDocument(new XElement("list", from i in p_SearchRequests select new XElement("item", new XAttribute("id", i))));
                    searchRequestXml = doc.ToString();
                }

                if (p_Provinces != null && p_Provinces.Count > 0)
                {
                    p_Provinces = IQProvinceToFusionIDMapModel.IQProvinceToFusionIDMap.Join(p_Provinces, a => a.Value.ToString(), b => b.id, (a, b) => new DashboardDMAChartSelectionModel { id = a.Key, clickColor = b.clickColor }).ToList();
                    XDocument doc = new XDocument(new XElement("list", from i in p_Provinces select new XElement("item", new XAttribute("province", i.id))));
                    provinceXml = doc.ToString();
                }

                string noOfDocs = "";
                string noOfHits = "";
                string noOfMinofAiring = "";
                string noOfView = "";

                DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = null;
                if (p_SearchType == 0)
                {
                    lstIQAgent_DaySummaryModel = dashboardLogic.GetProvinceSummaryDataHourWise(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, provinceXml);
                    noOfDocs = dashboardLogic.GetHighChartsForDocsForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces, sessionInformation.gmt, sessionInformation.dst);
                    noOfHits = dashboardLogic.GetHighChartsForHitsForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces, sessionInformation.gmt, sessionInformation.dst);
                    noOfView = dashboardLogic.GetHighChartsForViewsForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces, sessionInformation.gmt, sessionInformation.dst);

                    if (p_Medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        noOfMinofAiring = dashboardLogic.GetHighChartsForMinutesOfAiringForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces, sessionInformation.gmt, sessionInformation.dst);
                    }
                }
                else if (p_SearchType == 1)
                {
                    lstIQAgent_DaySummaryModel = dashboardLogic.GetProvinceSummaryDataDayWise(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, provinceXml);
                    noOfDocs = dashboardLogic.GetHighChartsForDocsForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);
                    noOfHits = dashboardLogic.GetHighChartsForHitsForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);
                    noOfView = dashboardLogic.GetHighChartsForViewsForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);

                    if (p_Medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        noOfMinofAiring = dashboardLogic.GetHighChartsForMinutesOfAiringForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);
                    }
                }
                else if (p_SearchType == 3)
                {
                    lstIQAgent_DaySummaryModel = dashboardLogic.GetProvinceSummaryDataMonthWise(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, provinceXml);
                    noOfDocs = dashboardLogic.GetHighChartsForDocsForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);
                    noOfHits = dashboardLogic.GetHighChartsForHitsForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);
                    noOfView = dashboardLogic.GetHighChartsForViewsForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);

                    if (p_Medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        noOfMinofAiring = dashboardLogic.GetHighChartsForMinutesOfAiringForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);
                    }
                }

                var json = new
                {
                    noOfDocsJson = noOfDocs,
                    noOfHitsJson = noOfHits,
                    noOfMinOfAiringJson = noOfMinofAiring,
                    noOfViewJson = noOfView,
                    isSuccess = true
                };
                return Json(json);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
        }

        #endregion

        #region Compare DMA

        public JsonResult CompareDma(DateTime p_FromDate, DateTime p_ToDate, List<string> p_SearchRequests, List<DashboardDMAChartSelectionModel> p_Dmas, Int16 p_SearchType, string p_Medium)
        {
            try
            {

                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                string searchRequestXml = null;
                string dmaXml = null;

                if (p_SearchRequests != null && p_SearchRequests.Count > 0)
                {
                    XDocument doc = new XDocument(new XElement("list", from i in p_SearchRequests select new XElement("item", new XAttribute("id", i))));
                    searchRequestXml = doc.ToString();
                }

                if (p_Dmas != null && p_Dmas.Count > 0)
                {
                    p_Dmas = IQDmaToFusionIDMapModel.IQDmaToFusionIDMap.Join(p_Dmas, a => a.Value.ToString(), b => b.id, (a, b) => new DashboardDMAChartSelectionModel { id = a.Key, clickColor = b.clickColor }).ToList();
                    XDocument doc = new XDocument(new XElement("list", from i in p_Dmas select new XElement("item", new XAttribute("dma", i.id))));
                    dmaXml = doc.ToString();
                }

                string noOfDocs = "";
                string noOfHits = "";
                string noOfMinofAiring = "";
                string noOfView = "";

                DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = null;
                if (p_SearchType == 0)
                {
                    lstIQAgent_DaySummaryModel = dashboardLogic.GetDmaSummaryDataHourWise(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, dmaXml);
                    noOfDocs = dashboardLogic.GetHighChartsForDocsForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas, sessionInformation.gmt, sessionInformation.dst);
                    noOfHits = dashboardLogic.GetHighChartsForHitsForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas, sessionInformation.gmt, sessionInformation.dst);
                    noOfView = dashboardLogic.GetHighChartsForViewsForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas, sessionInformation.gmt, sessionInformation.dst);

                    if (p_Medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        noOfMinofAiring = dashboardLogic.GetHighChartsForMinutesOfAiringForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas, sessionInformation.gmt, sessionInformation.dst);
                    }

                }
                else if (p_SearchType == 1)
                {
                    lstIQAgent_DaySummaryModel = dashboardLogic.GetDmaSummaryDataDayWise(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, dmaXml);
                    noOfDocs = dashboardLogic.GetHighChartsForDocsForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);
                    noOfHits = dashboardLogic.GetHighChartsForHitsForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);
                    noOfView = dashboardLogic.GetHighChartsForViewsForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);

                    if (p_Medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        noOfMinofAiring = dashboardLogic.GetHighChartsForMinutesOfAiringForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);
                    }

                }
                else if (p_SearchType == 3)
                {
                    lstIQAgent_DaySummaryModel = dashboardLogic.GetDmaSummaryDataMonthWise(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, dmaXml);
                    noOfDocs = dashboardLogic.GetHighChartsForDocsForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);
                    noOfHits = dashboardLogic.GetHighChartsForHitsForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);
                    noOfView = dashboardLogic.GetHighChartsForViewsForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);

                    if (p_Medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        noOfMinofAiring = dashboardLogic.GetHighChartsForMinutesOfAiringForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);
                    }
                }

                var json = new
                {
                    noOfDocsJson = noOfDocs,
                    noOfHitsJson = noOfHits,
                    noOfMinOfAiringJson = noOfMinofAiring,
                    noOfViewJson = noOfView,
                    isSuccess = true
                };
                return Json(json);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
        }

        #endregion

        #region Utility

        public IQAgent_DashBoardModel GetDaySummaryMediumWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, Int16 p_SearchType, List<string> p_SearchRequestIDs, List<IQ_MediaTypeModel> p_MediaTypeList)
        {
            try
            {
                string searchRequestXml = null;

                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    XDocument doc = new XDocument(new XElement("list", from i in p_SearchRequestIDs select new XElement("item", new XAttribute("id", i))));
                    searchRequestXml = doc.ToString();
                }

                DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
                IQAgent_DashBoardModel iQAgent_DashBoardModel = null;
                if (p_SearchType == 0)
                {
                    iQAgent_DashBoardModel = dashboardLogic.GetHourSummaryDataHourWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, p_MediaTypeList);
                }
                else if (p_SearchType == 1)
                {
                    iQAgent_DashBoardModel = dashboardLogic.GetDaySummaryDataDayWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, p_MediaTypeList);
                }
                else if (p_SearchType == 3)
                {
                    iQAgent_DashBoardModel = dashboardLogic.GetDaySummaryDataMonthWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, p_MediaTypeList);
                }

                return iQAgent_DashBoardModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetMediumHTML()
        {
            try
            {
                //if (medium.ToLower() == MediumType.TV.ToString().ToLower())
                //{
                return RenderPartialToString(PATH_DashboardBroadCastPartialView, new object());
                //}
            }
            catch (Exception)
            {

                throw;
            }

            return string.Empty;
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

        public ContentResult GetJsonMediumWise(IQ_MediaTypeModel p_MediaType, IQAgent_DashBoardModel iQAgent_DashBoardModel, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, List<string> p_SearchRequestIDs,ActiveUser p_ActiveUser)
        {
            try
            {
                Dictionary<string, object> dictResults = CommonController.GetDashboardMediumResults(p_MediaType, iQAgent_DashBoardModel, p_FromDate, p_ToDate, p_SearchType, p_SearchRequestIDs, true,p_ActiveUser);
                DashboardMediaResults dashboardMediaResults = (DashboardMediaResults)dictResults["MediaResults"];
                dynamic jsonResult = (ExpandoObject)dictResults["JsonResult"];

                var TopDmasHTML = string.Empty;
                var TopStationsHTML = string.Empty;
                var TopOnlineNewsDmasHTML = string.Empty;
                var TopOnlineNewsSitesHTML = string.Empty;
                var TopPrintPublicationsHTML = string.Empty;
                var topPrintAuthorsHTML = string.Empty;
                var topCountriesHTML = string.Empty;

                Func<DataList, List<DashboardTopResultsModel>, string> RenderDataList = (dl,data) =>
                {
                    var templateHTML = "";

                    Dictionary<string, object> dictTopResults = new Dictionary<string, object>();
                    dictTopResults.Add("Results", data);
                    dictTopResults.Add("Medium", p_MediaType);
                    dictTopResults.Add("TitleGrid", dl.Title);
                    dictTopResults.Add("TitleColumn", dl.TitleColumn);
                    dictTopResults.Add("DataType", dl.DataType);
                    dictTopResults.Add("CompeteAccess", p_ActiveUser.IsCompeteData);
                    dictTopResults.Add("NielsenAccess", p_ActiveUser.IsNielsenData);                    

                    switch (dl.TemplateType)
                    {
                        case TemplateTypes.TVDMA:
                            templateHTML = RenderPartialToString(PATH_DashboardTopBroadcastDMA, dictTopResults);
                            break;
                        case TemplateTypes.TVStation:
                            templateHTML = RenderPartialToString(PATH_DashboardTopBroadcastStation, dictTopResults);
                            break;
                        case TemplateTypes.TVCountry:
                            templateHTML = RenderPartialToString(PATH_DashboardTopBroadcastCountries, dictTopResults);
                            break;
                        case TemplateTypes.NMDMA:
                            templateHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsDMA, dictTopResults);
                            break;
                        case TemplateTypes.Common:                            
                            templateHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsSites, dictTopResults);
                            break;
                        default:
                            break;
                    }

                    return templateHTML;
                };

                List<Tuple<string, string>> dataLists = new List<Tuple<string, string>>();
                var listHtml = "";

                foreach (DataList dl in p_MediaType.DashboardData.DataLists)
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
                if (p_MediaType == CommonFunctions.CategoryType.TV.ToString())
                {
                    TopDmasHTML = RenderPartialToString(PATH_DashboardTopBroadcastDMA, iQAgent_DashBoardModel.ListOfTopDMABroadCast);
                    TopStationsHTML = RenderPartialToString(PATH_DashboardTopBroadcastStation, iQAgent_DashBoardModel.ListOfTopStationBroadCast);
                    topCountriesHTML = RenderPartialToString(PATH_DashboardTopBroadcastCountries, iQAgent_DashBoardModel.ListOfTopCountryBroadCast);
                }
                else if (p_MediaType != CommonFunctions.CategoryType.Radio.ToString() && p_MediaType != CommonFunctions.CategoryType.PM.ToString())
                {
                    if (p_MediaType == CommonFunctions.CategoryType.NM.ToString())
                    {
                        TopOnlineNewsDmasHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsDMA, iQAgent_DashBoardModel.ListOfTopDMABroadCast);
                    }
                    Dictionary<string, object> dicTopSites = new Dictionary<string, object>();

                    dicTopSites.Add("Results", iQAgent_DashBoardModel.ListOfTopStationBroadCast);
                    dicTopSites.Add("Medium", p_MediaType);
                    TopOnlineNewsSitesHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsSites, dicTopSites);
                }
                else if (p_MediaType == IQMedia.Shared.Utility.CommonFunctions.CategoryType.PM.ToString() && sessionInformation.Isv4PQ)
                {
                    Dictionary<string, object> dictTopPubs = new Dictionary<string, object>();
                    dictTopPubs.Add("Results", iQAgent_DashBoardModel.ListOfTopStationBroadCast);
                    dictTopPubs.Add("Medium", p_MediaType);
                    dictTopPubs.Add("TitleGrid", "Publications");
                    dictTopPubs.Add("TitleColumn", "Publication");
                    dictTopPubs.Add("DataType", "pub");
                    TopPrintPublicationsHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsSites, dictTopPubs);

                    Dictionary<string, object> dictTopAuthors = new Dictionary<string, object>();
                    dictTopAuthors.Add("Results", iQAgent_DashBoardModel.ListOfTopDMABroadCast);
                    dictTopAuthors.Add("Medium", p_MediaType);
                    dictTopAuthors.Add("TitleGrid", "Authors");
                    dictTopAuthors.Add("TitleColumn", "Author");
                    dictTopAuthors.Add("DataType", "author");
                    topPrintAuthorsHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsSites, dictTopAuthors);
                }
                */
                // Determine if the client has Canadian TV. If not, hide Canadian heat map.
                IQClient_CustomSettingsModel customSettings = (IQClient_CustomSettingsModel)TempData["CustomSettings"];
                List<string> tvCountries = customSettings.IQTVCountry.Split(',').ToList();
                List<string> tvRegions = customSettings.IQTVRegion.Split(',').ToList();
                dashboardMediaResults.ShowCanadaMap = (p_MediaType.DashboardData.UseCanadaMap && (!p_MediaType.DashboardData.CheckCanadaSettings || (tvCountries.Contains("2") && tvRegions.Contains("2"))));

                jsonResult.HTML = RenderPartialToString(PATH_DashboardBroadCastPartialView, dashboardMediaResults);
                /*jsonResult.p_TopDmasHTML = TopDmasHTML;
                jsonResult.p_TopStationsHTML = TopStationsHTML;
                jsonResult.p_TopOnlineNewsDmasHTML = TopOnlineNewsDmasHTML;
                jsonResult.p_TopOnlineNewsSitesHTML = TopOnlineNewsSitesHTML;
                jsonResult.p_TopPrintPublicationsHTML = TopPrintPublicationsHTML;
                jsonResult.p_TopPrintAuthorsHTML = topPrintAuthorsHTML;
                jsonResult.p_TopCountriesHTML = topCountriesHTML;*/
                jsonResult.DataLists = dataLists;
                jsonResult.isSuccess = true;

                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TempData.Keep();
            }
        }

        private ContentResult GetJsonSummaryWise(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, List<string> p_SearchRequestIDs, IQAgent_DashBoardPrevSummaryModel p_PrevIQAgentSummary, ActiveUser p_User)
        {
            try
            {
                List<IQAgent_SearchRequestModel> lstAgents = (List<IQAgent_SearchRequestModel>)TempData["Agents"];
                Dictionary<long, string> dictSelectedAgents = new Dictionary<long, string>();

                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    dictSelectedAgents = lstAgents.Where(w => p_SearchRequestIDs.Contains(w.ID.ToString()))
                                                .Select(s => new { s.ID, s.QueryName })
                                                .ToDictionary(t => t.ID, t => t.QueryName);
                }

                Dictionary<string, object> dictResults = CommonController.GetDashboardOverviewResults(listOfSummaryReportData, p_FromDate, p_ToDate, p_SearchType, dictSelectedAgents, p_PrevIQAgentSummary, (List<ThirdPartyDataTypeModel>)TempData["ThirdPartyDataTypes"], p_User);
                DashboardOverviewResults dashboardOverviewResults = (DashboardOverviewResults)dictResults["OverviewResults"];
                dynamic jsonResult = (ExpandoObject)dictResults["JsonResult"];

                jsonResult.HTML = RenderPartialToString(PATH_DashboardOverviewPartialView, dashboardOverviewResults);
                jsonResult.isSuccess = true;

                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TempData.Keep();
            }
        }

        private ContentResult GetJsonForAdhocSummary(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, short p_SearchType, ActiveUser p_User, bool isUGCEnabled = false)
        {
            try
            {
                Dictionary<string, object> dictResults = CommonController.GetDashboardAdhocResults(listOfSummaryReportData, p_FromDate, p_ToDate, p_SearchType, 850, isUGCEnabled,p_User);
                DashboardOverviewResults dashboardOverviewResults = (DashboardOverviewResults)dictResults["OverviewResults"];
                dynamic jsonResult = (ExpandoObject)dictResults["JsonResult"];

                jsonResult.HTML = RenderPartialToString(PATH_DashboardOverviewPartialView, dashboardOverviewResults);
                jsonResult.isSuccess = true;

                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetHTMLWithCSSIncluded(string p_HTML, string p_FromDate, string p_ToDate, bool p_IsEmail, string p_SearchRequests)
        {
            StringBuilder cssData = new StringBuilder();

            StreamReader strmReader = new StreamReader(Server.MapPath("~/css/Dashboard.css"));
            cssData.Append(strmReader.ReadToEnd());
            strmReader.Close();
            strmReader.Dispose();


            strmReader = new StreamReader(Server.MapPath("~/css/Feed.css"));
            cssData.Append(strmReader.ReadToEnd());
            strmReader.Close();
            strmReader.Dispose();

            strmReader = new StreamReader(Server.MapPath("~/css/bootstrap.css"));
            cssData.Append(strmReader.ReadToEnd());
            strmReader.Close();
            strmReader.Dispose();

            cssData.Append(" li.liDmaMap{width:70%;float:left;}   li.liDmaChart{width:30%;float:left;}   \n");
            cssData.Append(" .divSentimentNeg div{overflow:visible;} \n .divSentimentPos div{overflow:visible;} \n .borderBottom{border-bottom:none;} \n body{background:none;}\n");

            p_HTML = "<html><head><style type=\"text/css\">" + Convert.ToString(cssData) + "</style></head>" + "<body>" + "<img src=\"../../" + ConfigurationManager.AppSettings["IQMediaEmailLogo"] + "\" alt='IQMedia Logo'/>" + p_HTML + "</body></html>";

            HtmlDocument doc = new HtmlDocument();
            doc.Load(new StringReader(p_HTML));
            doc.OptionOutputOriginalCase = true;

            if (p_IsEmail)
            {
                doc.DocumentNode.SelectSingleNode("//body").SetAttributeValue("style", "width:1000px;");
            }

            HtmlNode dpFromDiv = doc.DocumentNode.SelectSingleNode("//input[@id='dpFrom']");
            var newdpFromNodeHTML = p_FromDate + "  - ";
            var newdpFromNode = HtmlNode.CreateNode(newdpFromNodeHTML);
            dpFromDiv.ParentNode.ReplaceChild(newdpFromNode, dpFromDiv);

            HtmlNode dpToDiv = doc.DocumentNode.SelectSingleNode("//input[@id='dpTo']");
            var newdpToNodeHTML = p_ToDate;
            var newdpToNode = HtmlNode.CreateNode(newdpToNodeHTML);
            dpToDiv.ParentNode.ReplaceChild(newdpToNode, dpToDiv);

            /* HtmlNode dpSearchRequestDiv = doc.DocumentNode.SelectSingleNode("//div[@id='ddlSearchRequest_chosen']");
             var newdpSearchRequestNodeHTML = "<div style=\"margin=top:5px\">" + p_SearchRequests + "</div>";
             var newdpSearchRequestNode = HtmlNode.CreateNode(newdpSearchRequestNodeHTML);
             dpSearchRequestDiv.ParentNode.ReplaceChild(newdpSearchRequestNode, dpSearchRequestDiv);

             HtmlNode dpSearchRequestButtonDiv = doc.DocumentNode.SelectSingleNode("//input[@id='btnGetDataOnSearchRequest']");
             var newdpSearchRequestButtonNodeHTML = string.Empty;
             var newdpSearchRequestButtonNode = HtmlNode.CreateNode(newdpSearchRequestButtonNodeHTML);
             dpSearchRequestButtonDiv.ParentNode.ReplaceChild(newdpSearchRequestButtonNode, dpSearchRequestButtonDiv);*/

            HtmlNode dpDurationDiv = doc.DocumentNode.SelectSingleNode("//div[@id='divDuration']");
            var newdpDurationNodeHTML = string.Empty;
            if (dpDurationDiv != null)
            {
                var newdpDurationNode = HtmlNode.CreateNode(newdpDurationNodeHTML);
                dpDurationDiv.ParentNode.ReplaceChild(newdpDurationNode, dpDurationDiv);
            }

            HtmlNode dpDashboardUtilityDiv = doc.DocumentNode.SelectSingleNode("//div[@id='divDashboardUtility']");
            var newdpDashboardUtilityNodeHTML = string.Empty;
            if (dpDashboardUtilityDiv != null)
            {
                var newdpDashboardUtilityNode = HtmlNode.CreateNode(newdpDashboardUtilityNodeHTML);
                dpDashboardUtilityDiv.ParentNode.ReplaceChild(newdpDashboardUtilityNode, dpDashboardUtilityDiv);
            }

            HtmlNode dpDashboardMapCompareDiv = doc.DocumentNode.SelectSingleNode("//div[@class='divcompareItalic']");
            var newdpDashboardMapCompareNodeHTML = string.Empty;
            if (dpDashboardMapCompareDiv != null)
            {
                var newdpDashboardMapCompareNode = HtmlNode.CreateNode(newdpDashboardUtilityNodeHTML);
                dpDashboardMapCompareDiv.ParentNode.ReplaceChild(newdpDashboardMapCompareNode, dpDashboardMapCompareDiv);
            }

            HtmlNode dpMapTooltipCompareDiv = doc.DocumentNode.SelectSingleNode("//div[@id='mapToolTip']");
            var newMapTooltipNodeHTML = string.Empty;
            if (dpMapTooltipCompareDiv != null)
            {
                var newMapTooltipNode = HtmlNode.CreateNode(newMapTooltipNodeHTML);
                dpMapTooltipCompareDiv.ParentNode.ReplaceChild(newMapTooltipNode, dpMapTooltipCompareDiv);
            }

            //HtmlNode divMediumData = doc.DocumentNode.SelectSingleNode("//div[@id='divMediumData']");
            //if (divMediumData != null)
            //{
            //    divMediumData.SetAttributeValue("style", "width:1000px;");
            //}

            //HtmlNode divHeader = doc.DocumentNode.SelectSingleNode("//div[@id='divOverviewHeader']");
            //if (divHeader != null)
            //{
            //    divHeader.SetAttributeValue("style", "width:1000px;");
            //}

            //HtmlNode divCharts = doc.DocumentNode.SelectSingleNode("//div[@id='divCharts']");
            //if (divCharts != null)
            //{
            //    divCharts.SetAttributeValue("style", "width:1000px;");
            //}

            //HtmlNode chart0 = doc.DocumentNode.SelectSingleNode("//div[@id='divChart0']");
            //if (chart0 != null)
            //{
            //    chart0.SetAttributeValue("style", "width:450px;");
            //}

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//img[@class='ui-datepicker-trigger']"))
            {
                var newNodeHTML = string.Empty;
                var newNode = HtmlNode.CreateNode(newNodeHTML);
                link.ParentNode.ReplaceChild(newNode, link);
            }

            return doc.DocumentNode.OuterHtml;

        }

        #endregion

        #region iFrame

        [HttpPost]
        public JsonResult OpenIFrame()
        {

            return Json(new
            {
                isSuccess = true
            });
        }

        #endregion

        #region Cohorts

        [HttpPost]
        public ContentResult GetReport(DateTime startDate, DateTime endDate, string report, string cohortID = null, string subGroup = null)
        {
            try
            {
                sessionInformation = WebAppUtil.ActiveUserMgr.GetActiveUser();
                AnalyticsLogic analyticsLogic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                IQAgentLogic agentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                CohortLogic cohortLogic = (CohortLogic)LogicFactory.GetLogic(LogicType.Cohort);

                Log4NetLogger.Debug(string.Format("GetReport"));
                Log4NetLogger.Debug(string.Format("DateRange: {0} - {1}", startDate, endDate));
                Log4NetLogger.Debug(string.Format("report: {0}", report));
                Log4NetLogger.Debug(string.Format("cohortID: {0}", string.IsNullOrEmpty(cohortID) ? "null" : cohortID));
                Log4NetLogger.Debug(string.Format("subGroup: {0}", string.IsNullOrEmpty(subGroup) ? "null" : subGroup));

                string selectedCohort;

                if (cohortID != null)
                {
                    selectedCohort = cohortID;
                }
                else
                {
                    // TODO - method to get default cohort for client
                    selectedCohort = "1";   // Cohort 1 is Automobiles
                    cohortID = selectedCohort;
                }

                Dictionary<long, string> cohorts = cohortLogic.GetAllCohorts();
                Dictionary<long, string> industryAgents = cohortLogic.GetCohortAgents(selectedCohort);
                Dictionary<long, string> allIndustryAgents = industryAgents;
                string selectedOverview = "";

                if (report == "Brand")
                {
                    Log4NetLogger.Debug("Report == Brand");
                    // Only get selected cohort agent
                    if (subGroup != null)
                    {
                        industryAgents = industryAgents.Where(w => w.Key.ToString() == subGroup).Select(s => new { s.Key, s.Value }).ToDictionary(ag => ag.Key, ag => ag.Value);
                        selectedOverview = industryAgents.First().Value;
                    }
                    else
                    {
                        KeyValuePair<long, string> firstKvP = industryAgents.First();
                        subGroup = firstKvP.Key.ToString();
                        selectedOverview = firstKvP.Value;
                        industryAgents = new Dictionary<long, string>();
                        industryAgents.Add(firstKvP.Key, firstKvP.Value);
                        Log4NetLogger.Debug(string.Format("Sub group set to {0}", subGroup));
                    }
                }

                List<IQ_MediaTypeModel> smts = sessionInformation.MediaTypes == null ? new List<IQ_MediaTypeModel>() : sessionInformation.MediaTypes.Where(w => w.TypeLevel == 2 && w.IsArchiveOnly == false).ToList();
                Dictionary<string, string> markets = analyticsLogic.GetAllDMAs();

                XDocument xDoc = new XDocument(new XElement(
                    "list",
                    from i in industryAgents
                    select new XElement("item",
                        new XAttribute("id", i.Key),
                        new XAttribute("fromDate", startDate.ToString()),
                        new XAttribute("toDate", endDate.ToString()),
                        new XAttribute("fromDateGMT", WebAppCommon.GetGMTandDSTTime(startDate).ToString()),
                        new XAttribute("toDateGMT", WebAppCommon.GetGMTandDSTTime(endDate).ToString())
                    )
                ));
                string searchXML = xDoc.ToString();

                var analyticsRequest = new AnalyticsRequest() {
                    DateFrom = startDate.ToString(),
                    DateTo = endDate.ToString(),
                    HCChartType = HCChartTypes.spline,
                    IsCompareMode = false,
                    IsFilter = false,
                    ChartType = ChartType.Line,
                    DateInterval = "day",
                    PageType = "dashboard",
                    RequestIDs = industryAgents.Keys.ToList(),
                    Tab = SecondaryTabID.Overview,
                    SummationProperty = SummationProperty.Docs,
                    PESHTypes = new List<string>()
                };

                AnalyticsDataModel requestedData = new AnalyticsDataModel();
                AnalyticsDataModel earnedData = new AnalyticsDataModel();
                List<CohortSolrFacet> networkTableFacets = new List<CohortSolrFacet>();
                List<CohortSolrFacet> programTableFacets = new List<CohortSolrFacet>();
                List<CohortSolrFacet> stationTableFacets = new List<CohortSolrFacet>();
                string pmgUrl = WebAppCommon.GeneratePMGUrl(WebAppCommon.PMGUrlType.FE.ToString(), null, null);
                List<string> SRIDs = industryAgents.Keys.Select(s => s.ToString()).ToList();
                List<string> topFacetList = new List<string>();
                string dmaID = null;
                CohortSolrFacet selectedFacet = new CohortSolrFacet();
                List<HtmlGenericControl> dropDownItems = new List<HtmlGenericControl>();

                if (report == "Overview" || report == "Market" || report == "Brand")
                {
                    requestedData = analyticsLogic.GetDaySummaryData(sessionInformation.ClientGUID, searchXML, "", sessionInformation.gmt, sessionInformation.dst, false, true);

                    foreach (AnalyticsSummaryModel summary in requestedData.SummaryDataList)
                    {
                        if (!string.IsNullOrEmpty(summary.SubMediaType))
                        {
                            var subMediaType = smts.Where(sm => sm.SubMediaType == summary.SubMediaType);
                            summary.SMTDisplayName = subMediaType.Any() ? subMediaType.First().DisplayName : "";
                        }
                        if (!string.IsNullOrEmpty(summary.Market))
                        {
                            var marketID = markets.Where(dma => dma.Value == summary.Market);
                            summary.MarketID = marketID.Any() ? Convert.ToInt64(marketID.First().Key) : -1;
                        }
                        if (summary.MarketID == null)
                        {
                            summary.MarketID = -1;
                        }
                        if (string.IsNullOrEmpty(summary.SubMediaType) || string.IsNullOrWhiteSpace(summary.SubMediaType))
                        {
                            summary.SubMediaType = "";
                        }
                    }

                    earnedData.SummaryDataList = requestedData.SummaryDataList.Where(w => !w.SubMediaType.Equals("TV")).ToList();
                    requestedData.SummaryDataList = requestedData.SummaryDataList.Where(w => w.SubMediaType.Equals("TV")).ToList();

                    // If market tab - need to limit data to only that market
                    if (report == "Market")
                    {
                        // Trim paid data to only specified market
                        if (subGroup != null)
                        {
                            dmaID = subGroup;
                            selectedOverview = markets[dmaID];
                            if (requestedData.SummaryDataList.Any(a => a.MarketID.ToString().Equals(subGroup)))
                            {
                                requestedData.SummaryDataList = requestedData.SummaryDataList.Where(w => w.MarketID.ToString().Equals(subGroup)).ToList();
                            }
                            else
                            {
                                // Selected market is not present in the data
                                requestedData.SummaryDataList = new List<AnalyticsSummaryModel>();
                            }
                        }
                        else
                        {
                            // If no market selected on page - need to choose the top market
                            if (requestedData.SummaryDataList.Any())
                            {
                                // Get top market
                                AnalyticsSummaryModel topMarket = requestedData.SummaryDataList.OrderByDescending(o => o.IQMediaValue).First();
                                subGroup = dmaID = topMarket.MarketID.ToString();

                                // Trim requested data to top market
                                requestedData.SummaryDataList = requestedData.SummaryDataList.Where(w => w.MarketID.Equals(topMarket.MarketID)).ToList();
                                selectedOverview = requestedData.SummaryDataList.First().Market;
                            }
                            else
                            {
                                // If there is no data - no markets - default to National simply for visual continuity 
                                subGroup = "1";
                                selectedOverview = "National";
                            }
                        }
                    }
                    else if (report == "Overview")
                    {
                        subGroup = cohortID;
                        selectedOverview = cohorts[Convert.ToInt64(cohortID)];
                    }

                    if (SRIDs.Count > 0)
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        networkTableFacets = cohortLogic.GetTopNetworks(pmgUrl, startDate, endDate, SRIDs, dmaID, null, null, false);
                        sw.Stop();
                        Log4NetLogger.Debug(string.Format("network facet: {0} ms", sw.ElapsedMilliseconds));

                        sw.Reset();
                        sw.Start();
                        programTableFacets = cohortLogic.GetTopPrograms(pmgUrl, startDate, endDate, SRIDs, dmaID, null, null, false);
                        sw.Stop();
                        Log4NetLogger.Debug(string.Format("show facet: {0} ms", sw.ElapsedMilliseconds));

                        sw.Reset();
                        sw.Start();
                        stationTableFacets = cohortLogic.GetTopStations(pmgUrl, startDate, endDate, SRIDs, dmaID, null, null, false);
                        sw.Stop();
                        Log4NetLogger.Debug(string.Format("station facet: {0} ms", sw.ElapsedMilliseconds));
                    }

                    if (report == "Brand")
                    {
                        dropDownItems = CreateCohortListItems(report, new List<CohortSolrFacet>(), subGroup, allIndustryAgents);
                    }
                    else
                    {
                        dropDownItems = CreateCohortListItems(report, networkTableFacets, subGroup, new Dictionary<long,string>());
                    }
                }
                else if (report == "Network")
                {
                    Stopwatch ntwkSw = new Stopwatch();
                    ntwkSw.Start();
                    // If network tab - need to limit data to only that network - get ALL Networks initially (to build dropdown selection)
                    List<CohortSolrFacet> networkFacets = networkTableFacets = cohortLogic.GetTopNetworks(pmgUrl, startDate, endDate, SRIDs, null, null, null, true);
                    ntwkSw.Stop();
                    Log4NetLogger.Debug(string.Format("Main Network Facet: {0} ms {1} facets", ntwkSw.ElapsedMilliseconds, networkFacets.Count));
                    ntwkSw.Reset();
                    if (subGroup != null)
                    {
                        // If subGroup already known, only want this network
                        topFacetList = new List<string>() { subGroup };
                        // Also need the top network facet object to contain network name
                        selectedFacet = networkFacets.Where(w => w.Name.Replace(" ", "").Equals(subGroup)).DefaultIfEmpty(null).FirstOrDefault();
                    }
                    else
                    {
                        // If no specific network selected - need the top network so get facets
                        selectedFacet = networkFacets.OrderByDescending(o => o.AdValue).First();
                        topFacetList = new List<string>() { selectedFacet.Name };
                        subGroup = selectedFacet.Name.Replace(" ", "");
                    }

                    selectedOverview = selectedFacet.Name;
                    analyticsRequest.Tab = SecondaryTabID.Networks;

                    ntwkSw.Start();
                    requestedData = GetNetworkShowSummaries(sessionInformation.ClientGUID, analyticsRequest, topFacetList);
                    ntwkSw.Stop();
                    Log4NetLogger.Debug(string.Format("Network DB Call: {0} ms", ntwkSw.ElapsedMilliseconds));
                    ntwkSw.Reset();

                    // Give markets their correct ID 
                    foreach (AnalyticsSummaryModel summary in requestedData.SummaryDataList)
                    {
                        if (!string.IsNullOrEmpty(summary.Market) && summary.MarketID == null)
                        {
                            var marketID = markets.Where(dma => dma.Value == summary.Market);
                            summary.MarketID = marketID.Any() ? Convert.ToInt64(marketID.First().Key) : -1;
                        }
                    }
                    ntwkSw.Start();
                    programTableFacets = cohortLogic.GetTopPrograms(pmgUrl, startDate, endDate, SRIDs, null, selectedFacet.Name, null, false);
                    stationTableFacets = cohortLogic.GetTopStations(pmgUrl, startDate, endDate, SRIDs, null, selectedFacet.Name, null, false);
                    ntwkSw.Stop();
                    Log4NetLogger.Debug(string.Format("Program & Station facets: {0} ms", ntwkSw.ElapsedMilliseconds));

                    dropDownItems = CreateCohortListItems(report, networkFacets, subGroup, new Dictionary<long,string>());

                    analyticsRequest.Tab = SecondaryTabID.Overview;
                }
                else if (report == "Program")
                {
                    Stopwatch programSW = new Stopwatch();
                    programSW.Start();
                    // If show tab - need to limit data to only that show - get ALL shows initially (to build dropdown selection)
                    List<CohortSolrFacet> programFacets = programTableFacets = cohortLogic.GetTopPrograms(pmgUrl, startDate, endDate, SRIDs, null, null, null, true);
                    programSW.Stop();
                    Log4NetLogger.Debug(string.Format("Main Program Facet: {0} ms {1} facets", programSW.ElapsedMilliseconds, programFacets.Count));
                    programSW.Reset();

                    if (subGroup != null)
                    {
                        selectedFacet = programFacets.Where(w => w.Name.Replace(" ", "").Equals(subGroup)).DefaultIfEmpty(null).FirstOrDefault();
                        topFacetList = new List<string> { selectedFacet.Name };
                    }
                    else
                    {
                        selectedFacet = programFacets.OrderByDescending(o => o.AdValue).First();
                        topFacetList = new List<string>() { selectedFacet.Name };
                        subGroup = selectedFacet.Name.Replace(" ", "");
                    }

                    selectedOverview = selectedFacet.Name;
                    analyticsRequest.Tab = SecondaryTabID.Shows;
                    programSW.Start();
                    requestedData = GetNetworkShowSummaries(sessionInformation.ClientGUID, analyticsRequest, topFacetList);
                    programSW.Stop();
                    Log4NetLogger.Debug(string.Format("Program DB call: {0} ms", programSW.ElapsedMilliseconds));
                    programSW.Reset();

                    foreach (AnalyticsSummaryModel summary in requestedData.SummaryDataList)
                    {
                        if (!string.IsNullOrEmpty(summary.Market) && summary.MarketID == null)
                        {
                            var marketID = markets.Where(dma => dma.Value == summary.Market);
                            summary.MarketID = marketID.Any() ? Convert.ToInt64(marketID.First().Key) : -1;
                        }
                    }

                    programSW.Start();
                    networkTableFacets = cohortLogic.GetTopNetworks(pmgUrl, startDate, endDate, SRIDs, null, selectedFacet.Name, null, false);
                    stationTableFacets = cohortLogic.GetTopStations(pmgUrl, startDate, endDate, SRIDs, null, null, selectedFacet.Name, false);
                    programSW.Stop();
                    Log4NetLogger.Debug(string.Format("Network & Station facets: {0} ms", programSW.ElapsedMilliseconds));

                    dropDownItems = CreateCohortListItems(report, programFacets, subGroup, new Dictionary<long,string>());

                    analyticsRequest.Tab = SecondaryTabID.Overview;
                }
                else if (report == "Station")
                {
                    Stopwatch stationSW = new Stopwatch();
                    stationSW.Start();
                    List<CohortSolrFacet> stationFacet = stationTableFacets = cohortLogic.GetTopStations(pmgUrl, startDate, endDate, SRIDs, null, null, null, true);
                    stationSW.Stop();
                    Log4NetLogger.Debug(string.Format("Main Station Facet: {0} ms {1} facets", stationSW.ElapsedMilliseconds, stationFacet.Count));
                    stationSW.Reset();

                    if (subGroup != null)
                    {
                        topFacetList = new List<string> { subGroup };
                        selectedFacet = stationFacet.Where(w => w.Name.Equals(subGroup)).DefaultIfEmpty(null).FirstOrDefault();
                    }
                    else
                    {
                        selectedFacet = stationFacet.OrderByDescending(o => o.AdValue).First();
                        topFacetList = new List<string> { selectedFacet.Name };
                        subGroup = selectedFacet.Name;
                    }

                    selectedOverview = selectedFacet.Name;
                    analyticsRequest.Tab = SecondaryTabID.Stations;
                    stationSW.Start();
                    requestedData = GetNetworkShowSummaries(sessionInformation.ClientGUID, analyticsRequest, topFacetList);
                    stationSW.Stop();
                    Log4NetLogger.Debug(string.Format("Station DB call: {0} ms", stationSW.ElapsedMilliseconds));
                    stationSW.Reset();

                    foreach (AnalyticsSummaryModel summary in requestedData.SummaryDataList)
                    {
                        if (!string.IsNullOrEmpty(summary.Market) && summary.MarketID == null)
                        {
                            var marketID = markets.Where(dma => dma.Value == summary.Market);
                            summary.MarketID = marketID.Any() ? Convert.ToInt64(marketID.First().Key) : -1;
                        }
                    }

                    stationSW.Start();
                    networkTableFacets = cohortLogic.GetTopNetworks(pmgUrl, startDate, endDate, SRIDs, null, null, selectedFacet.Name, false);
                    programTableFacets = cohortLogic.GetTopPrograms(pmgUrl, startDate, endDate, SRIDs, null, null, selectedFacet.Name, false);
                    stationSW.Stop();
                    Log4NetLogger.Debug(string.Format("Network & Program facets: {0} ms", stationSW.ElapsedMilliseconds));

                    dropDownItems = CreateCohortListItems(report, stationFacet, subGroup, new Dictionary<long,string>());

                    analyticsRequest.Tab = SecondaryTabID.Overview;
                }

                List<AnalyticsGrouping> PESHGroupings = new List<AnalyticsGrouping>();
                PESHGroupings.Add(new AnalyticsGrouping() {
                    ID = "Paid",
                    Name = "Paid",
                    Summaries = requestedData.SummaryDataList.Where(w => w.HeardPaid > 0 || w.SeenPaid > 0).ToList()
                });
                PESHGroupings.Add(new AnalyticsGrouping() {
                    ID = "Earned",
                    Name = "Earned",
                    Summaries = requestedData.SummaryDataList.Where(w => w.ReadEarned > 0 || w.SeenEarned > 0).ToList()
                });

                List<AnalyticsGrouping> agentGroupings = new List<AnalyticsGrouping>();
                if (report == "Brand")
                {
                    agentGroupings.Add(new AnalyticsGrouping() {
                        ID = "Heard",
                        Name = "Heard",
                        Summaries = requestedData.SummaryDataList.Where(w => w.HeardEarned > 0 || w.HeardPaid > 0).ToList()
                    });

                    agentGroupings.Add(new AnalyticsGrouping() {
                        ID = "Seen",
                        Name = "Seen",
                        Summaries = requestedData.SummaryDataList.Where(w => w.SeenEarned > 0 || w.SeenPaid > 0).ToList()
                    });
                }
                else
                {
                    foreach (var ag in industryAgents)
                    {
                        agentGroupings.Add(new AnalyticsGrouping() {
                            ID = ag.Key.ToString(),
                            Name = ag.Value,
                            Summaries = requestedData.SummaryDataList.Where(w => w.SearchRequestID == ag.Key).ToList()
                        });
                    }
                }

                List<AnalyticsGrouping> demoGroupings = new List<AnalyticsGrouping>() {
                    new AnalyticsGrouping() {
                        ID = "",
                        Name = "",
                        Summaries = requestedData.SummaryDataList.ToList()
                    }
                };

                List<AnalyticsGrouping> marketGroupings = new List<AnalyticsGrouping>();
                foreach(var g in requestedData.SummaryDataList.GroupBy(gb => gb.MarketID).ToList())
                {
                    if (!string.IsNullOrEmpty(g.First().Market))
                    {
                        marketGroupings.Add(new AnalyticsGrouping() {
                            ID = g.First().MarketID.ToString(),
                            Name = g.First().Market,
                            Summaries = g.ToList()
                        });
                    }
                }

                //List<AnalyticsGrouping> networkGroupings = new List<AnalyticsGrouping>();
                //foreach (var g in requestedData.SummaryDataList.GroupBy(gb => gb.Networks).ToList())
                //{
                //    networkGroupings.Add(new AnalyticsGrouping() {
                //        ID = g.First().Networks,
                //        Name = g.First().Networks,
                //        Summaries = g.ToList()
                //    });
                //}

                //List<AnalyticsGrouping> programGroupings = new List<AnalyticsGrouping>();
                //foreach (var g in requestedData.SummaryDataList.GroupBy(gb => gb.Shows).ToList())
                //{
                //    programGroupings.Add(new AnalyticsGrouping() {
                //        ID = g.First().Shows,
                //        Name = g.First().Shows,
                //        Summaries = g.ToList()
                //    });
                //}

                AnalyticsSecondaryTable emptyTable = new AnalyticsSecondaryTable();
                Dictionary<string, object> chartAndSeries = new Dictionary<string, object>();

                dynamic jsonResult = new ExpandoObject();
                jsonResult.HTML = RenderPartialToString(PATH_DashboardReportsOverview, new object { });
                jsonResult.Industries = cohorts.Values.ToList();
                jsonResult.Brands = industryAgents.Count;
                jsonResult.Airings = string.Format("{0:N0}", requestedData.SummaryDataList.Sum(s => s.Number_Docs));
                jsonResult.HiddenElements = cohortLogic.GetHiddenElements(report);
                jsonResult.SelectedOverview = selectedOverview;

                var adSpend = requestedData.SummaryDataList.Sum(s => s.AdSpend);
                jsonResult.AdSpend = string.Format("{0:C}", adSpend);
                jsonResult.Audience = string.Format("{0:N0}", requestedData.SummaryDataList.Sum(s => s.Audience));

                // Set to null so don't need to check for undefined in JS
                jsonResult.LineChartAiring = null;
                jsonResult.PieChartAiring = null;
                jsonResult.PieChartShare = null;
                jsonResult.AreaChartShare = null;
                jsonResult.PieChartAdSpend = null;
                jsonResult.LineChartAdSpend = null;

                // Get Spline 0 - Occurrences
                if (report != "Brand")
                {
                    chartAndSeries = analyticsLogic.GetChart(analyticsRequest, emptyTable, requestedData, agentGroupings, smts);
                    jsonResult.LineChartAiring = chartAndSeries["chart"];
                    // Get Pie 1 - Ocurrences
                    analyticsRequest.ChartType = ChartType.Pie;
                    analyticsRequest.HCChartType = HCChartTypes.pie;
                    chartAndSeries = analyticsLogic.GetChart(analyticsRequest, emptyTable, requestedData, agentGroupings, smts);
                    jsonResult.PieChartAiring = chartAndSeries["chart"];

                    // Get Pie 3 - Ad Spend
                    analyticsRequest.SummationProperty = SummationProperty.AdSpend;
                    chartAndSeries = analyticsLogic.GetChart(analyticsRequest, emptyTable, requestedData, agentGroupings, smts);
                    jsonResult.PieChartAdSpend = chartAndSeries["chart"];

                    // Get Spline 2 - Ad Spend
                    analyticsRequest.ChartType = ChartType.Line;
                    analyticsRequest.HCChartType = HCChartTypes.spline;
                    chartAndSeries = analyticsLogic.GetChart(analyticsRequest, emptyTable, requestedData, agentGroupings, smts);
                    jsonResult.LineChartAdSpend = chartAndSeries["chart"];
                }
                else
                {
                    jsonResult.AreaChartShare = analyticsLogic.GetChart(analyticsRequest, emptyTable, requestedData, PESHGroupings, smts)["chart"];
                    analyticsRequest.ChartType = ChartType.Pie;
                    analyticsRequest.HCChartType = HCChartTypes.pie;
                    chartAndSeries = analyticsLogic.GetChart(analyticsRequest, emptyTable, requestedData, agentGroupings, smts);
                    jsonResult.PieChartShare = chartAndSeries["chart"];
                }

                // Get gender pie - Demographic tab is unique in that it doesn't respect normal summation procedures
                analyticsRequest.ChartType = ChartType.Pie;
                analyticsRequest.HCChartType = HCChartTypes.pie;
                analyticsRequest.Tab = SecondaryTabID.Demographic;
                analyticsRequest.SubTab = "gender";
                chartAndSeries = analyticsLogic.GetChart(analyticsRequest, emptyTable, requestedData, demoGroupings, smts);
                jsonResult.PieChartGender = chartAndSeries["chart"];
                //jsonResult.Series4 = chartAndSeries["series"];

                // Get age pie
                analyticsRequest.SubTab = "age";
                chartAndSeries = analyticsLogic.GetChart(analyticsRequest, emptyTable, requestedData, demoGroupings, smts);
                jsonResult.PieChartAge = chartAndSeries["chart"];
                //jsonResult.Series5 = chartAndSeries["series"];

                // Build tables
                HtmlTable marketTable = BuildTable(marketGroupings, "Market");
                StringWriter mktTableWriter = new StringWriter();
                marketTable.RenderControl(new System.Web.UI.HtmlTextWriter(mktTableWriter));
                jsonResult.marketTable = mktTableWriter.ToString();

                HtmlTable networkTable = BuildFacetTable(networkTableFacets, "Network");
                StringWriter ntwkTableWriter = new StringWriter();
                networkTable.RenderControl(new System.Web.UI.HtmlTextWriter(ntwkTableWriter));
                jsonResult.networkTable = ntwkTableWriter.ToString();

                HtmlTable showTable = BuildFacetTable(programTableFacets, "Program");
                StringWriter showTableWriter = new StringWriter();
                showTable.RenderControl(new System.Web.UI.HtmlTextWriter(showTableWriter));
                jsonResult.showTable = showTableWriter.ToString();

                HtmlTable stationTable = BuildFacetTable(stationTableFacets, "Station");
                StringWriter stationTableWriter = new StringWriter();
                stationTable.RenderControl(new System.Web.UI.HtmlTextWriter(stationTableWriter));
                jsonResult.stationTable = stationTableWriter.ToString();

                // Build dropdown list
                StringWriter listItems = new StringWriter();
                List<string> htmlItemList = new List<string>();
                foreach (HtmlGenericControl li in dropDownItems)
                {
                    StringWriter liWriter = new StringWriter();
                    li.RenderControl(new System.Web.UI.HtmlTextWriter(liWriter));
                    htmlItemList.Add(liWriter.ToString());
                }
                jsonResult.DropDownList = htmlItemList;

                // Earned data for NM, BL, and Print
                if (report == "Overview" || report == "Market" || report == "Brand")
                {
                    List<AnalyticsSummaryModel> earnedList = new List<AnalyticsSummaryModel>();
                    List<AnalyticsSummaryModel> topNMList = new List<AnalyticsSummaryModel>();
                    topNMList = earnedList = earnedData.SummaryDataList.Where(w => w.SubMediaType.Equals("NM")).ToList();
                    
                    jsonResult.EarnedNM = earnedList.Any() ? earnedList.Sum(s => s.Number_Docs) : 0;
                    earnedList = earnedData.SummaryDataList.Where(w => w.SubMediaType.Equals("BL")).ToList();
                    jsonResult.EarnedBL = earnedList.Any() ? earnedList.Sum(s => s.Number_Docs) : 0;
                    var printSMTs = sessionInformation.MediaTypes.Where(w => w.MediaType.Equals("PR") && w.TypeLevel == 2).Select(s => s.SubMediaType).ToList();
                    earnedList = earnedData.SummaryDataList.Where(w => printSMTs.Contains(w.SubMediaType)).ToList();
                    jsonResult.EarnedPR = earnedList.Any() ? earnedList.Sum(s => s.Number_Docs) : 0;
                }

                jsonResult.isSuccess = true;
                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception exc)
            {
                WebAppCommon.WriteException(exc);
                return Content(WebAppCommon.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
        }

        [HttpPost]
        public ContentResult GetNewLineChart(string dateInterval, DateTime startDate, DateTime endDate, string summation)
        {
            try
            {
                sessionInformation = WebAppUtil.ActiveUserMgr.GetActiveUser();
                AnalyticsLogic analyticsLogic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                IQAgentLogic agentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                List<IQAgent_SearchRequestModel> clientAgents = agentLogic.SelectIQAgentSearchRequestByClientGuid(sessionInformation.ClientGUID.ToString());
                Dictionary<long, string> agents = clientAgents.Select(ag => new {
                    ag.ID,
                    ag.QueryName
                }).ToDictionary(
                    ag => ag.ID,
                    ag => ag.QueryName
                );

                // Change end date for hour
                if (dateInterval.Equals("hour"))
                {
                    if (endDate.Date.Equals(DateTime.Now.Date))
                    {
                        endDate = WebAppCommon.GetLocalTime(new DateTime(
                            endDate.Year,
                            endDate.Month,
                            endDate.Day,
                            DateTime.UtcNow.Hour,
                            0,
                            0
                        ).AddHours(-3)).Value;
                    }
                    else
                    {
                        endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                    }
                }

                var smts = sessionInformation.MediaTypes == null ? new List<IQ_MediaTypeModel>() : sessionInformation.MediaTypes.Where(w => w.TypeLevel == 2 && w.IsArchiveOnly == false).ToList();
                var markets = analyticsLogic.GetAllDMAs();

                XDocument xDoc = new XDocument(new XElement(
                    "list",
                    from i in clientAgents
                    select new XElement("item",
                        new XAttribute("id", i.ID),
                        new XAttribute("fromDate", startDate.ToString()),
                        new XAttribute("toDate", endDate.ToString()),
                        new XAttribute("fromDateGMT", WebAppCommon.GetGMTandDSTTime(startDate).ToString()),
                        new XAttribute("toDateGMT", WebAppCommon.GetGMTandDSTTime(endDate).ToString())
                    )
                ));
                string searchXML = xDoc.ToString();

                var analyticsRequest = new AnalyticsRequest() {
                    DateFrom = startDate.ToString(),
                    DateTo = endDate.ToString(),
                    HCChartType = HCChartTypes.spline,
                    IsCompareMode = false,
                    IsFilter = false,
                    ChartType = ChartType.Line,
                    DateInterval = dateInterval,
                    PageType = "dashboard",
                    RequestIDs = agents.Keys.ToList(),
                    Tab = SecondaryTabID.Overview,
                    SummationProperty = SummationProperty.Docs // Docs or IQMediaValue
                };

                AnalyticsDataModel analyticsData = new AnalyticsDataModel();
                if (analyticsRequest.DateInterval.Equals("hour"))
                {
                    analyticsData = analyticsLogic.GetHourSummaryData(sessionInformation.ClientGUID, searchXML, "TV", sessionInformation.gmt, sessionInformation.dst, false, true);
                }
                else if (analyticsRequest.DateInterval.Equals("day"))
                {
                    analyticsData = analyticsLogic.GetDaySummaryData(sessionInformation.ClientGUID, searchXML, "TV", sessionInformation.gmt, sessionInformation.dst, false, true);
                }
                else
                {
                    // Month
                    analyticsData = analyticsLogic.GetMonthSummaryData(sessionInformation.ClientGUID, searchXML, "TV", sessionInformation.gmt, sessionInformation.dst);
                }

                foreach (AnalyticsSummaryModel summary in analyticsData.SummaryDataList)
                {
                    if (!string.IsNullOrEmpty(summary.SubMediaType))
                    {
                        var subMediaType = smts.Where(sm => sm.SubMediaType == summary.SubMediaType);
                        summary.SMTDisplayName = subMediaType.Any() ? subMediaType.First().DisplayName : "";
                    }
                    if (!string.IsNullOrEmpty(summary.Market))
                    {
                        var marketID = markets.Where(dma => dma.Value == summary.Market);
                        summary.MarketID = marketID.Any() ? Convert.ToInt64(marketID.First().Key) : -1;
                    }
                    if (summary.MarketID == null)
                    {
                        summary.MarketID = -1;
                    }
                    if (string.IsNullOrEmpty(summary.SubMediaType) || string.IsNullOrWhiteSpace(summary.SubMediaType))
                    {
                        summary.SubMediaType = "";
                    }
                }

                List<AnalyticsGrouping> agentGroupings = new List<AnalyticsGrouping>();
                foreach (var ag in agents)
                {
                    // Remove summaryies with null values for their SRIDs
                    agentGroupings.Add(new AnalyticsGrouping() {
                        ID = ag.Key.ToString(),
                        Name = ag.Value,
                        Summaries = analyticsData.SummaryDataList.Where(w => w.SearchRequestID == ag.Key).ToList()
                    });
                }

                var chartAndSeries = analyticsLogic.GetChart(analyticsRequest, new AnalyticsSecondaryTable(), analyticsData, agentGroupings, smts);
                dynamic jsonResult = new ExpandoObject();
                jsonResult.isSuccess = true;
                jsonResult.Chart = chartAndSeries["chart"];
                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception exc)
            {
                WebAppCommon.WriteException(exc);
                return Content(WebAppCommon.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
        }

        private HtmlTable BuildTable(List<AnalyticsGrouping> groupings, string tableName)
        {
            try
            {
                HtmlTable table = new HtmlTable();
                table.Attributes["class"] = "topResult";
                table.Attributes["style"] = "width:95%;";
                HtmlTableRow headerRow = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell() {
                    InnerText = tableName
                };
                headerRow.Cells.Add(tc);
                tc = new HtmlTableCell() {
                    InnerText = "Volume"
                };
                headerRow.Cells.Add(tc);
                tc = new HtmlTableCell() {
                    InnerText = "EST. TV Spend"
                };
                tc.Attributes["style"] = "text-align:right;";
                headerRow.Cells.Add(tc);
                table.Rows.Add(headerRow);

                foreach (var group in groupings.OrderByDescending(ob => ob.Summaries.Sum(s => s.IQMediaValue)).Take(5))
                {
                    HtmlTableRow tr = new HtmlTableRow();

                    // Name Column
                    tc = new HtmlTableCell() {
                        InnerText = group.Name
                    };
                    tr.Cells.Add(tc);

                    // Volume Column
                    tc = new HtmlTableCell() {
                        InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.Number_Docs))
                    };
                    tr.Cells.Add(tc);

                    // Spend Column
                    tc = new HtmlTableCell() {
                        InnerText = string.Format("{0:C}", group.Summaries.Sum(s => s.IQMediaValue))
                    };
                    tc.Attributes["style"] = "text-align:right;";
                    tr.Cells.Add(tc);

                    table.Rows.Add(tr);
                }

                return table;
            }
            catch (Exception exc)
            {
                WebAppCommon.WriteException(exc);
                return new HtmlTable();
            }
        }

        private HtmlTable BuildFacetTable(List<CohortSolrFacet> facets, string tableName)
        {
            HtmlTable facetTable = new HtmlTable();
            facetTable.Attributes["class"] = "topResult";
            facetTable.Attributes["style"] = "width:95%;";
            HtmlTableRow headerRow = new HtmlTableRow();

            HtmlTableCell tc = new HtmlTableCell() {
                InnerText = tableName
            };
            headerRow.Cells.Add(tc);
            tc = new HtmlTableCell() {
                InnerText = "Volume"
            };
            headerRow.Cells.Add(tc);
            tc = new HtmlTableCell() {
                InnerText = "EST. TV Spend"
            };
            tc.Attributes["style"] = "text-align:right;";
            headerRow.Cells.Add(tc);
            facetTable.Rows.Add(headerRow);

            foreach (var facet in facets.OrderByDescending(o => o.AdValue).Take(5))
            {
                HtmlTableRow tr = new HtmlTableRow();

                // Name
                tc = new HtmlTableCell() {
                    InnerText = facet.Name
                };
                tr.Cells.Add(tc);

                // Volume
                tc = new HtmlTableCell() {
                    InnerText = string.Format("{0:N0}", facet.Count)
                };
                tr.Cells.Add(tc);

                // Spend
                tc = new HtmlTableCell() {
                    InnerText = string.Format("{0:C}", facet.AdValue)
                };
                tc.Attributes["style"] = "text-align:right;";
                tr.Cells.Add(tc);

                facetTable.Rows.Add(tr);
            }

            return facetTable;
        }

        private List<HtmlGenericControl> CreateCohortListItems(string report, List<CohortSolrFacet> facetList, string selectedID, Dictionary<long, string> brands)
        {
            try
            {
                Log4NetLogger.Debug(string.Format("CreateCohortListItem: {0}, {1}", report, selectedID));
                List<HtmlGenericControl> listItems = new List<HtmlGenericControl>();
                sessionInformation = WebAppUtil.ActiveUserMgr.GetActiveUser();
                AnalyticsLogic analyticsLogic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                IQAgentLogic agentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                CohortLogic cohortLogic = (CohortLogic)LogicFactory.GetLogic(LogicType.Cohort);
                Dictionary<long, string> cohorts = cohortLogic.GetAllCohorts();

                var smts = sessionInformation.MediaTypes == null ? new List<IQ_MediaTypeModel>() : sessionInformation.MediaTypes.Where(w => w.TypeLevel == 2 && w.IsArchiveOnly == false).ToList();
                var markets = analyticsLogic.GetAllDMAs();

                if (report == "Overview")
                {
                    foreach (KeyValuePair<long, string> cohort in cohorts.OrderBy(o => o.Value))
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        li.Attributes.Add("role", "presentation");
                        li.Attributes.Add("onclick", "ChangeCohort(this)");
                        li.ID = string.Format("cohort_{0}", cohort.Key);
                        li.InnerText = cohort.Value;

                        if (selectedID != null && cohort.Key.ToString().Equals(selectedID))
                        {
                            li.Attributes.Add("class", "cursorPointer highlightedli");
                        }
                        else
                        {
                            li.Attributes.Add("class", "cursorPointer");
                        }

                        listItems.Add(li);
                    }
                }
                else if (report == "Market")
                {
                    foreach (var mkt in markets.OrderBy(o => o.Value))
                    {
                        //Log4NetLogger.Debug(string.Format("create li for cohort {0} w ID {1}", mkt.Value, mkt.Key));
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        li.Attributes.Add("role", "presentation");
                        li.Attributes.Add("onclick", "ChangeMarket(this)");
                        li.ID = string.Format("cohort_{0}", mkt.Key);
                        li.InnerText = mkt.Value;

                        if (selectedID != null && mkt.Key.Equals(selectedID))
                        {
                            li.Attributes.Add("class", "cursorPointer highlightedli");
                        }
                        else
                        {
                            li.Attributes.Add("class", "cursorPointer");
                        }

                        listItems.Add(li);
                    }

                }
                else if (report == "Network")
                {
                    foreach (var ntwk in facetList.OrderBy(o =>  o.Name))
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        li.Attributes.Add("role", "presentation");
                        li.Attributes.Add("onclick", "ChangeNetwork(this)");
                        li.ID = string.Format("cohort_{0}", ntwk.Name.Replace(" ", ""));

                        li.InnerText = ntwk.Name;

                        if (selectedID != null && ntwk.Name.Replace(" ", "").Equals(selectedID))
                        {
                            li.Attributes.Add("class", "cursorPointer highlightedli");
                        }
                        else
                        {
                            li.Attributes.Add("class", "cursorPointer");
                        }

                        listItems.Add(li);
                    }
                }
                else if (report == "Station")
                {
                    foreach (var station in facetList.OrderBy(o => o.Name))
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        li.Attributes.Add("role", "presentation");
                        li.Attributes.Add("onclick", "ChangeStation(this)");
                        li.ID = string.Format("cohort_{0}", station.Name);

                        li.InnerText = station.Name;

                        if (selectedID != null && station.Name.Equals(selectedID))
                        {
                            li.Attributes.Add("class", "cursorPointer highlightedli");
                        }
                        else
                        {
                            li.Attributes.Add("class", "cursorPointer");
                        }

                        listItems.Add(li);
                    }
                }
                else if (report == "Program")
                {
                    foreach (var program in facetList.OrderBy(o => o.Name))
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        li.Attributes.Add("role", "presentation");
                        li.Attributes.Add("onclick", "ChangeProgram(this)");
                        li.ID = string.Format("cohort_{0}", program.Name.Replace(" ", ""));

                        li.InnerText = program.Name;

                        if (selectedID != null && program.Name.Replace(" ", "").Equals(selectedID))
                        {
                            li.Attributes.Add("class", "cursorPointer highlightedli");
                        }
                        else
                        {
                            li.Attributes.Add("class", "cursorPointer");
                        }

                        listItems.Add(li);
                    }
                }
                else if (report == "Brand")
                {
                    foreach (var brand in brands.OrderBy(o => o.Value))
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        li.Attributes.Add("role", "presentation");
                        li.Attributes.Add("onclick", "ChangeBrand(this)");
                        li.ID = string.Format("cohort_{0}", brand.Key);

                        li.InnerText = brand.Value;

                        if (selectedID != null && brand.Key.ToString() == selectedID)
                        {
                            li.Attributes.Add("class", "cursorPointer highlightedli");
                        }
                        else
                        {
                            li.Attributes.Add("class", "cursorPointer");
                        }

                        listItems.Add(li);
                    }
                }

                return listItems;
            }
            catch (Exception exc)
            {
                WebAppCommon.WriteException(exc);
            }
            return new List<HtmlGenericControl>();
        }

        private AnalyticsDataModel GetNetworkShowSummaries(Guid clientGuid, AnalyticsRequest request, List<string> topNetworks)
        {
            try
            {
                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                AnalyticsDataModel networksShows;
                string requestXML = null;

                if (request.RequestIDs != null && request.RequestIDs.Count > 0)
                {
                    XDocument xDoc = new XDocument(new XElement(
                        "list",
                        from i in request.RequestIDs
                        select new XElement(
                            "item",
                            new XAttribute("id", i),
                            new XAttribute("fromDate", request.DateFrom),
                            new XAttribute("toDate", request.DateTo),
                            new XAttribute("fromDateGMT", WebAppCommon.GetGMTandDSTTime(Convert.ToDateTime(request.DateFrom)).ToString()),
                            new XAttribute("toDateGMT", WebAppCommon.GetGMTandDSTTime(Convert.ToDateTime(request.DateTo)).ToString())
                        )
                    ));
                    requestXML = xDoc.ToString();
                }

                //topListNS = logic.GetTopNetworkShows(clientGuid, requestXML, request.Tab).Where(x => x != null && x.Trim() != "").ToList();
                //topDictNS = new Dictionary<string, string>();
                //var count = 0;
                //foreach (var item in topListNS)
                //{
                //    topDictNS.Add("Top" + count, item);
                //    count++;
                //}

                networksShows = logic.GetNetworkShowSummaryData(clientGuid, requestXML, request.SubMediaType, sessionInformation.gmt, sessionInformation.dst, topNetworks, request.Tab, request.DateInterval);

                return networksShows;
            }
            catch (Exception exc)
            {
                WebAppCommon.WriteException(exc);
            }
            return new AnalyticsDataModel();
        }

        #endregion

    }
}
