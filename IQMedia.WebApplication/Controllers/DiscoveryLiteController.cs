using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using PMGSearch;
using System.Configuration;
using IQMedia.Model;
using System.Threading.Tasks;
using System.Xml.Linq;
using IQMedia.Shared.Utility;
using IQMedia.Web.Logic.Base;
using System.Threading;
using System.Text;
using System.Collections;
using System.IO;
using IQMedia.WebApplication.Models;
using IQMedia.WebApplication.Config;
using IQMedia.WebApplication.Models.TempData;
using System.Diagnostics;
using HtmlAgilityPack;
using System.Drawing;
using HiQPdf;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class DiscoveryLiteController : Controller
    {
        //
        // GET: /Discovery/

        #region Public Member

        ActiveUser sessionInformation = null;
        DiscoveryLiteTempData discoveryLiteTempData = null;
        string PATH_DiscoveryTopResultsPartialView = "~/Views/DiscoveryLite/_TopResult.cshtml";
        List<DiscoveryLiteSearchResponse> lstSMResponseFeedClass = null;
        List<DiscoveryLiteSearchResponse> lstMainDiscoverySearchResponse = null;
        List<String> lstTVMarket = null;
        object lockObject = new object();
        #endregion

        public ActionResult Index()
        {
            try
            {
                //SetSessionData(null);
                SetTempData(null);
                discoveryLiteTempData = GetTempData();

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                //Add TV Country filter
                //discoveryLiteTempData.IQTVRegion = clientLogic.GetClientTVRegionSettings(sessionInformation.ClientGUID);
                var IQTVRegionCountry = clientLogic.GetClientTVRegionSettings(sessionInformation.ClientGUID);
                if (IQTVRegionCountry != null && IQTVRegionCountry.Count > 0)
                {
                    discoveryLiteTempData.IQTVRegion = (List<int>)IQTVRegionCountry[0];
                }
                if (IQTVRegionCountry != null && IQTVRegionCountry.Count > 1)
                {
                    discoveryLiteTempData.IQTVCountry = (List<int>)IQTVRegionCountry[1];
                }

                SetTempData(discoveryLiteTempData);

                GetSSPData(sessionInformation.ClientGUID);

                SetTempData(discoveryLiteTempData);
                ViewBag.IsSuccess = true;
                return View();
            }
            catch (Exception exception)
            {
                ViewBag.IsSuccess = false;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return View();
            }
        }

        #region Ajax Request
        [HttpPost]
        public JsonResult MediaJsonChart(string[] searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket, bool isDefaultLoad)
        {
            try
            {
                MediaChartJsonResponse mediaChartJsonResponse = new MediaChartJsonResponse();
                if (!isDefaultLoad)
                {
                    List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = new List<DiscoveryResultRecordTrack>();
                    foreach (string str in searchTerm)
                    {
                        DiscoveryResultRecordTrack discoveryResultRecordTrack = new DiscoveryResultRecordTrack();
                        discoveryResultRecordTrack.SearchTerm = str;

                        discoveryResultRecordTrack.IsNMValid = true;
                        discoveryResultRecordTrack.IsSMValid = true;
                        discoveryResultRecordTrack.IsTVValid = true;
                        discoveryResultRecordTrack.IsPQValid = true;

                        discoveryResultRecordTrack.TVRecordTotal = null;
                        discoveryResultRecordTrack.NMRecordTotal = null;
                        discoveryResultRecordTrack.SMRecordTotal = null;
                        discoveryResultRecordTrack.PQRecordTotal = null;

                        lstDiscoveryResultRecordTrack.Add(discoveryResultRecordTrack);
                    }

                    if (fromDate.HasValue && toDate.HasValue)
                    {
                        fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);

                        toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                        toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                    }
                    else
                    {
                        fromDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                        toDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59).AddSeconds(59);

                        fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);
                        toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                    }

                    Shared.Utility.Log4NetLogger.Debug(Session.SessionID + "Discovery From Date : " + fromDate.Value.ToString());
                    Shared.Utility.Log4NetLogger.Debug(Session.SessionID + "Discovery To Date : " + toDate.Value.ToString());

                    discoveryLiteTempData = GetTempData();
                    discoveryLiteTempData.lstDiscoveryResultRecordTrack = lstDiscoveryResultRecordTrack;
                    SetTempData(discoveryLiteTempData);
                    //SetSessionData(lstDiscoveryResultRecordTrack);
                    sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                    mediaChartJsonResponse = GetChartData(searchTerm, fromDate, toDate, medium, tvMarket, sessionInformation.ClientGUID, true, sessionInformation.gmt, sessionInformation.dst);
                    UpdateFromRecordID(lstMainDiscoverySearchResponse);

                    mediaChartJsonResponse.DataAvailableList = GetChartMessage(mediaChartJsonResponse.DataAvailableList, medium);
                }
                else
                {
                    discoveryLiteTempData = GetTempData();
                    SetTempData(discoveryLiteTempData);

                    mediaChartJsonResponse.IsSuccess = true;
                    mediaChartJsonResponse.IsSearchTermValid = true;
                }

                return Json(new
                {
                    columnChartJson = mediaChartJsonResponse.ColumnChartData,
                    lineChartJson = mediaChartJsonResponse.LineChartData,
                    pieChartMediumJson = mediaChartJsonResponse.PieChartMediumData,
                    pieChartSearchTermJson = mediaChartJsonResponse.PieChartSearchTermData != null ? mediaChartJsonResponse.PieChartSearchTermData["JsonResult"] : null,
                    pieChartSearchTermTotals = mediaChartJsonResponse.PieChartSearchTermData != null ? mediaChartJsonResponse.PieChartSearchTermData["TotalRecords"] : null,
                    //notAvailableDataChart = mediaChartJsonResponse.DataNotAvailableList,
                    availableDataChart = mediaChartJsonResponse.DataAvailableList,
                    discoveryDateFilter = mediaChartJsonResponse.DateFilter,
                    discoveryMediumFilter = mediaChartJsonResponse.MediumFilter,
                    discoveryTVMarketFilter = mediaChartJsonResponse.TVMarket,
                    isSearchTermValid = mediaChartJsonResponse.IsSearchTermValid,
                    chartTotal = lstMainDiscoverySearchResponse != null ? string.Format("{0:N0}", lstMainDiscoverySearchResponse.Sum(s => s.TotalResult)) : null,
                    isSuccess = mediaChartJsonResponse.IsSuccess
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
                TempData.Keep("DiscoveryTempData");
            }
        }

        #region Download PDF

        [HttpPost]
        public JsonResult GenerateDiscoveryPDF(string p_HTML, string p_FromDate, string p_ToDate, string[] p_SearchTerm)
        {
            try
            {
                string reportHTML = GetHTMLWithCSSIncluded(p_HTML, p_FromDate, p_ToDate, false, p_SearchTerm);

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                string TempPDFPath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Discovery\\PDF\\" + sessionInformation.CustomerGUID + "_" + DateTimeStamp + ".pdf"; ;

                HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
                htmlToPdfConverter.SerialNumber = ConfigurationManager.AppSettings["HiQPdfSerialKey"];
                htmlToPdfConverter.Document.Margins = new PdfMargins(20);
                htmlToPdfConverter.BrowserWidth = 1000;
                htmlToPdfConverter.ConvertHtmlToFile(reportHTML, String.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority), TempPDFPath);

                bool IsFileGenerated = false;

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

        #endregion

        #endregion

        #region Methods


        #region PMG Search

        #region Chart

        public void SearchMedia(string[] searchTerm, DateTime? fromdate, DateTime? toDate, string medium, string tvMarket, bool IsInsertFromRecordID, Guid p_ClientGUID, out string availableData)
        {
            try
            {
                availableData = string.Empty;
                //notAvailableData = string.Empty;
                /*SearchTV(searchTerm, date, medium, tvMarket);
                SearchSocialMedia(searchTerm, date, medium, tvMarket);*/

                List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryLiteTempData)GetTempData()).lstDiscoveryResultRecordTrack;// TempData["DiscoveryResultRecordTrack"];
                discoveryLiteTempData = GetTempData();

                string TVFromRecordID = null;
                string NMFromRecordID = null;
                string SMFromRecordID = null;
                string PQFromRecordID = null;

                List<Task> lstTask = new List<Task>();
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;

                Int16 searchTermCount = 0;
                Dictionary<String, object> dictSSPData = new Dictionary<string, object>();
                if (!discoveryLiteTempData.IsAllDmaAllowed ||
                        !discoveryLiteTempData.IsAllClassAllowed ||
                        !discoveryLiteTempData.IsAllStationAllowed)
                {
                    dictSSPData = GetSSPData(p_ClientGUID);
                }
                else
                {
                    dictSSPData.Add("IQ_Dma", new List<IQ_Dma>());
                    dictSSPData.Add("IQ_Class", new List<IQ_Class>());
                    dictSSPData.Add("Station_Affil", new List<Station_Affil>());
                }

                foreach (string sTerm in searchTerm)
                {

                    searchTermCount++;
                    if (searchTermCount <= 5)
                    {
                        var term = sTerm;
                        DiscoveryResultRecordTrack discoveryResultRecordTrack = (DiscoveryResultRecordTrack)(lstDiscoveryResultRecordTrack != null ?
                            lstDiscoveryResultRecordTrack.Where(w => w.SearchTerm.Equals(term)).SingleOrDefault() : null);

                        if (sessionInformation.Isv4TV && (discoveryResultRecordTrack.IsTVValid) && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.TV.ToString()))
                        {
                            DiscoveryLiteSearchResponse dsrTV = new DiscoveryLiteSearchResponse() { SearchTerm = term, MediumType = CommonFunctions.CategoryType.TV.ToString(), IsValid = false };

                            lstTask.Add(Task<DiscoveryLiteSearchResponse>.Factory.StartNew((object obj) => SearchTV(term, fromdate, toDate, medium, tvMarket,
                                                                                    discoveryLiteTempData.IsAllDmaAllowed, (List<IQ_Dma>)dictSSPData["IQ_Dma"],
                                                                                    discoveryLiteTempData.IsAllClassAllowed, (List<IQ_Class>)dictSSPData["IQ_Class"],
                                                                                    discoveryLiteTempData.IsAllStationAllowed, (List<Station_Affil>)dictSSPData["Station_Affil"], discoveryLiteTempData.IQTVRegion, discoveryLiteTempData.IQTVCountry, token, dsrTV), dsrTV));
                        }

                        // News Task
                        if (sessionInformation.Isv4NM && (discoveryResultRecordTrack.IsNMValid) && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.NM.ToString()))
                        {
                            DiscoveryLiteSearchResponse dsrNews = new DiscoveryLiteSearchResponse() { SearchTerm = term, MediumType = CommonFunctions.CategoryType.NM.ToString(), IsValid = false };

                            lstTask.Add(Task<DiscoveryLiteSearchResponse>.Factory.StartNew((object obj) => SearchNews(term, fromdate, toDate, medium, tvMarket, NMFromRecordID, token, dsrNews), dsrNews));
                        }

                        // SM Task
                        if (sessionInformation.Isv4SM && (discoveryResultRecordTrack.IsSMValid) &&
                            (string.IsNullOrWhiteSpace(medium) || medium == "Social Media" ||
                                medium == CommonFunctions.CategoryType.Blog.ToString() ||
                            medium == CommonFunctions.CategoryType.Forum.ToString()))
                        {

                            DiscoveryLiteSearchResponse dsrSM = new DiscoveryLiteSearchResponse() { SearchTerm = term, MediumType = CommonFunctions.CategoryType.SocialMedia.ToString(), IsValid = false };

                            lstTask.Add(Task<LiteSocialMediaFacet>.Factory.StartNew((object obj) => SearchSocialMedia(term, fromdate, toDate, medium, tvMarket, SMFromRecordID, token, dsrSM), dsrSM));
                        }

                        // ProQuest Task
                        if (sessionInformation.Isv4PQ && (discoveryResultRecordTrack.IsPQValid) && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.PQ.ToString()))
                        {
                            DiscoveryLiteSearchResponse dsrProQuest = new DiscoveryLiteSearchResponse() { SearchTerm = term, MediumType = CommonFunctions.CategoryType.PQ.ToString(), IsValid = false };

                            lstTask.Add(Task<DiscoveryLiteSearchResponse>.Factory.StartNew((object obj) => SearchProQuest(term, fromdate, toDate, medium, tvMarket, PQFromRecordID, token, dsrProQuest), dsrProQuest));
                        }
                    }

                }

                //Task[] searchTasks = (Task[])lstTask.ToArray();
                try
                {

                    Task.WaitAll(lstTask.ToArray(), Convert.ToInt32(ConfigurationManager.AppSettings["MaxRequestDuration"]), token);
                    tokenSource.Cancel();

                }
                catch (AggregateException ex)
                {
                    foreach (var item in ex.InnerExceptions)
                    {
                        if (item is System.Security.Authentication.AuthenticationException)
                        {
                            Log4NetLogger.Error("Manual Exception from AggregateException");
                        }
                    }
                    Log4NetLogger.Error("AggregateException " + ex.ToString());
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Error("Exception " + ex.ToString());
                }

                StringBuilder strngNotAvailableData = new StringBuilder();
                StringBuilder strngAvailableData = new StringBuilder();

                foreach (var tsk in lstTask)
                {
                    if (((DiscoveryLiteSearchResponse)tsk.AsyncState).IsValid)
                    {
                        DiscoveryLiteSearchResponse discoverySearchResponse = null;
                        if (((DiscoveryLiteSearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.SocialMedia.ToString()
                            || ((DiscoveryLiteSearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.Blog.ToString()
                            || ((DiscoveryLiteSearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.Forum.ToString())
                        {

                            discoverySearchResponse = ((Task<LiteSocialMediaFacet>)tsk).Result.DateData;
                            discoverySearchResponse.IsValid = true;

                            lstSMResponseFeedClass.Add(((Task<LiteSocialMediaFacet>)tsk).Result.FeedClassData);
                            lstMainDiscoverySearchResponse.Add(discoverySearchResponse);

                            if (((Task<LiteSocialMediaFacet>)tsk).Result.DateData.ListRecordData.Count > 0)
                            {
                                if (string.IsNullOrWhiteSpace(medium))
                                {
                                    strngAvailableData.Append(" Social Media on search " + ((DiscoveryLiteSearchResponse)tsk.AsyncState).SearchTerm + ", ");
                                    strngAvailableData.Append(" Blog on search " + ((DiscoveryLiteSearchResponse)tsk.AsyncState).SearchTerm + ", ");
                                    strngAvailableData.Append(" Forum on search " + ((DiscoveryLiteSearchResponse)tsk.AsyncState).SearchTerm + ", ");
                                }
                                else
                                {
                                    strngAvailableData.Append(" " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(medium.Replace(" ", string.Empty))) + " on search " + ((DiscoveryLiteSearchResponse)tsk.AsyncState).SearchTerm + " ,");
                                }
                            }

                        }
                        else
                        {
                            discoverySearchResponse = ((Task<DiscoveryLiteSearchResponse>)tsk).Result;
                            discoverySearchResponse.IsValid = true;

                            lstMainDiscoverySearchResponse.Add(discoverySearchResponse);
                            if (((Task<DiscoveryLiteSearchResponse>)tsk).Result.ListRecordData.Count > 0)
                            {
                                strngAvailableData.Append(" " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(((DiscoveryLiteSearchResponse)tsk.AsyncState).MediumType.Replace(" ", string.Empty))) + " on search " + ((DiscoveryLiteSearchResponse)tsk.AsyncState).SearchTerm + " ,");
                            }
                        }
                    }
                    else
                    {
                        DiscoveryLiteSearchResponse discoverySearchResponse = null;
                        if (((DiscoveryLiteSearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.SocialMedia.ToString()
                            || ((DiscoveryLiteSearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.Blog.ToString()
                            || ((DiscoveryLiteSearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.Forum.ToString())
                        {
                            discoverySearchResponse = new DiscoveryLiteSearchResponse();
                            discoverySearchResponse.ListRecordData = new List<LiteRecordData>();
                            discoverySearchResponse.SearchTerm = ((DiscoveryLiteSearchResponse)tsk.AsyncState).SearchTerm;
                            discoverySearchResponse.IsValid = false;
                            discoverySearchResponse.ListTopResults = new List<LiteTopResults>();
                            discoverySearchResponse.MediumType = ((DiscoveryLiteSearchResponse)tsk.AsyncState).MediumType;
                            discoverySearchResponse.ListRecordData = new List<LiteRecordData>();

                            lstSMResponseFeedClass.Add(discoverySearchResponse);

                            if (string.IsNullOrWhiteSpace(medium))
                            {
                                strngNotAvailableData.Append(" " + ((DiscoveryLiteSearchResponse)tsk.AsyncState).SearchTerm + " for " + "Social Media" + ", ");
                                strngNotAvailableData.Append(" " + ((DiscoveryLiteSearchResponse)tsk.AsyncState).SearchTerm + " for " + "Blog" + ", ");
                                strngNotAvailableData.Append(" " + ((DiscoveryLiteSearchResponse)tsk.AsyncState).SearchTerm + " for " + "Forum" + ", ");
                            }
                            else
                            {
                                strngNotAvailableData.Append(" " + ((DiscoveryLiteSearchResponse)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(medium.Replace(" ", string.Empty))) + ", ");
                            }
                        }
                        else
                        {
                            discoverySearchResponse = new DiscoveryLiteSearchResponse();
                            discoverySearchResponse.ListRecordData = new List<LiteRecordData>();
                            discoverySearchResponse.SearchTerm = ((DiscoveryLiteSearchResponse)tsk.AsyncState).SearchTerm;
                            discoverySearchResponse.MediumType = ((DiscoveryLiteSearchResponse)tsk.AsyncState).MediumType;
                            discoverySearchResponse.IsValid = false;
                            discoverySearchResponse.ListTopResults = new List<LiteTopResults>();
                            strngNotAvailableData.Append(" " + ((DiscoveryLiteSearchResponse)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(((DiscoveryLiteSearchResponse)tsk.AsyncState).MediumType.Replace(" ", string.Empty))) + ", ");
                        }
                        lstMainDiscoverySearchResponse.Add(discoverySearchResponse);
                    }
                }

                //notAvailableData = Convert.ToString(strngNotAvailableData);
                availableData = Convert.ToString(strngAvailableData);

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }


        public DiscoveryLiteSearchResponse SearchTV(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, string tvMarket,
                                              bool IsAllDmaAllowed, List<IQ_Dma> listDma,
                                                bool IsAllClassAllowed, List<IQ_Class> listClass,
                                                bool IsAllStationAllowed, List<Station_Affil> listStation, List<int> listRegion, List<int> listCountry, CancellationToken token, DiscoveryLiteSearchResponse dsrTV)
        {
            try
            {

                DiscoveryLiteLogic discoveryLiteLogic = (DiscoveryLiteLogic)LogicFactory.GetLogic(LogicType.DiscoveryLite);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.TV.ToString(), fromdate, toDate);
                DiscoveryLiteSearchResponse discoverySearchResponseTV = discoveryLiteLogic.SearchTV(srcTerm, fromdate, toDate, medium, tvMarket, IsAllDmaAllowed, listDma, IsAllClassAllowed, listClass, IsAllStationAllowed, listStation, listRegion, listCountry, out lstTVMarket, pmgUrl);

                if (!token.IsCancellationRequested)
                {
                    dsrTV.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }
                return discoverySearchResponseTV;
            }
            catch (Exception ex)
            {
                dsrTV.IsValid = false;
                //throw ex;
                /*if (!token.IsCancellationRequested)
                {
                    Log4NetLogger.Error("SearchTV - IsCancellationRequested false");
                    throw;
                }
                else
                {
                    Log4NetLogger.Error("SearchTV - IsCancellationRequested true");
                }*/
            }
            return new DiscoveryLiteSearchResponse();
        }

        public DiscoveryLiteSearchResponse SearchNews(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, string tvMarket, string p_fromRecordID, CancellationToken token, DiscoveryLiteSearchResponse dsrNews)
        {
            try
            {
                discoveryLiteTempData = GetTempData();
                DiscoveryLiteLogic discoveryLiteLogic = (DiscoveryLiteLogic)LogicFactory.GetLogic(LogicType.DiscoveryLite);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.MO.ToString(), fromdate, toDate);
                DiscoveryLiteSearchResponse discoverySearchResponseNews = discoveryLiteLogic.SearchNews(srcTerm, fromdate, toDate, medium, tvMarket, p_fromRecordID, pmgUrl, null);

                if (!token.IsCancellationRequested)
                {
                    dsrNews.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }
                return discoverySearchResponseNews;
            }
            catch (Exception ex)
            {
                dsrNews.IsValid = false;
                //throw ex;
                /*if (token.IsCancellationRequested)
                {
                    Log4NetLogger.Error("SearchNews - IsCancellationRequested false");
                    token.ThrowIfCancellationRequested();
                }
                else
                {
                    Log4NetLogger.Error("SearchNews - IsCancellationRequested true");
                    throw ex;
                }*/

            }
            return new DiscoveryLiteSearchResponse();

        }

        public LiteSocialMediaFacet SearchSocialMedia(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, string tvMarket, string p_fromRecordID, CancellationToken token, DiscoveryLiteSearchResponse dsrSM)
        {
            try
            {

                DiscoveryLiteLogic discoveryLiteLogic = (DiscoveryLiteLogic)LogicFactory.GetLogic(LogicType.DiscoveryLite);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.MO.ToString(), fromdate, toDate);
                LiteSocialMediaFacet socialMediaFacet = discoveryLiteLogic.SearchSocialMedia(srcTerm, fromdate, toDate, medium, tvMarket, p_fromRecordID, pmgUrl);

                if (!token.IsCancellationRequested)
                {
                    dsrSM.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }
                return socialMediaFacet;
            }
            catch (Exception ex)
            {
                dsrSM.IsValid = false;
                //throw ex;
                /*if (!token.IsCancellationRequested)
                {
                    Log4NetLogger.Error("SearchSocialMedia - IsCancellationRequested false");
                    throw;
                }
                else
                {
                    Log4NetLogger.Error("SearchSocialMedia - IsCancellationRequested true");
                }*/
            }
            return new LiteSocialMediaFacet();
        }

        public DiscoveryLiteSearchResponse SearchProQuest(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, string tvMarket, string fromRecordID, CancellationToken token, DiscoveryLiteSearchResponse dsrPQ)
        {
            try
            {
                DiscoveryLiteLogic discoveryLiteLogic = (DiscoveryLiteLogic)LogicFactory.GetLogic(LogicType.DiscoveryLite);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.PQ.ToString(), fromdate, toDate);
                DiscoveryLiteSearchResponse discoverySearchResponse = discoveryLiteLogic.SearchProQuest(srcTerm, fromdate, toDate, medium, tvMarket, fromRecordID, pmgUrl);

                if (!token.IsCancellationRequested)
                {
                    dsrPQ.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }
                return discoverySearchResponse;

            }
            catch (Exception ex)
            {
                dsrPQ.IsValid = false;
            }
            return new DiscoveryLiteSearchResponse();
        }

        #endregion

        #endregion

        #region Chart Binding

        public string ColumnChart(List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse)
        {
            try
            {
                DiscoveryLiteLogic discoveryLiteLogic = (DiscoveryLiteLogic)LogicFactory.GetLogic(LogicType.DiscoveryLite);
                string jsonResult = discoveryLiteLogic.HighChartsColumnChart(lstDiscoverySearchResponse);
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        public string LineChart(List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse, Boolean isHourData, decimal clientGmtOffset, decimal clientDstOffset)
        {

            DiscoveryLiteLogic discoveryLiteLogic = (DiscoveryLiteLogic)LogicFactory.GetLogic(LogicType.DiscoveryLite);
            string jsonLineChartResult = discoveryLiteLogic.HighChartsLineChart(lstDiscoverySearchResponse, isHourData, clientGmtOffset, clientDstOffset);
            return jsonLineChartResult;
        }

        public Dictionary<string, object> PieChartBySearchTerm(List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse, string[] searchTerm, string medium)
        {

            DiscoveryLiteLogic discoveryLiteLogic = (DiscoveryLiteLogic)LogicFactory.GetLogic(LogicType.DiscoveryLite);
            Dictionary<string, object> dictPieChartResponse = discoveryLiteLogic.HighChartsPieChartBySearchTerm(lstDiscoverySearchResponse, lstSMResponseFeedClass, searchTerm, medium,
                                                                    sessionInformation.Isv4TV, sessionInformation.Isv4NM, sessionInformation.Isv4SM, sessionInformation.Isv4PQ);
            return dictPieChartResponse;
        }

        public List<PieChartResponse> PieChartByMedium(List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse, string[] searchTerm, string medium)
        {

            DiscoveryLiteLogic discoveryLiteLogic = (DiscoveryLiteLogic)LogicFactory.GetLogic(LogicType.DiscoveryLite);
            List<PieChartResponse> lstPieChartReponse = discoveryLiteLogic.HighChartsPieChartByMedium(lstDiscoverySearchResponse, lstSMResponseFeedClass, searchTerm, medium,
                                                            sessionInformation.Isv4TV, sessionInformation.Isv4NM, sessionInformation.Isv4SM, sessionInformation.Isv4PQ);
            return lstPieChartReponse;
        }

        public IEnumerable GetMediumFilter(List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse)
        {
            DiscoveryLiteLogic discoveryLiteLogic = (DiscoveryLiteLogic)LogicFactory.GetLogic(LogicType.DiscoveryLite);
            return discoveryLiteLogic.GetMediumFilter(lstDiscoverySearchResponse, lstSMResponseFeedClass);
        }
        #endregion

        #region Validation

        public bool SearchTermValidation(string[] searchTerm)
        {
            try
            {

                var searchTermCount = searchTerm.GroupBy(g => g).Select(s => new { count = s.Count() }).Where(w => w.count > 1);
                if (searchTermCount != null && searchTermCount.Count() > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("error while validating search term", ex);
                throw;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }

        }

        #endregion

        #region Utility

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

        private bool HasMoreResults(Int64 TotalRecords, Int64 shownRecords)
        {
            if (shownRecords < TotalRecords)
                return true;
            else
                return false;
        }

        public string GetAvailableDataString(string dataString)
        {
            /*dataString = string.IsNullOrWhiteSpace(dataString) ? dataString : "Wow, we are searching on lot of data! We returned results for " + dataString
                                                + " Would you like to continue your search  ";*/

            dataString = string.IsNullOrWhiteSpace(dataString) ? dataString : ConfigSettings.Settings.DiscoveryMessage;

            return dataString;
        }

        public string GetNoDataDataString(string dataString)
        {
            dataString = ConfigSettings.Settings.DiscoveryNoDataAvailable;
            return dataString;
        }

        public string GetResultMessage(string sTerm, string dataAvailableList, string medium, out bool anyDataAvailable)
        {
            // Check if Result Data contains any Valid Request and Set its message accordingly
            anyDataAvailable = false;
            List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryLiteTempData)GetTempData()).lstDiscoveryResultRecordTrack;
            DiscoveryResultRecordTrack drrt = lstDiscoveryResultRecordTrack.Where(w => String.Compare(w.SearchTerm, sTerm, true) == 0).FirstOrDefault();
            bool isAnyDataAvailable = false;
            bool isAllDataAvailable = true;
            if (drrt != null)
            {
                if (string.IsNullOrWhiteSpace(medium))
                {
                    if (drrt.IsTVValid && drrt.IsNMValid && drrt.IsSMValid)
                    {
                        isAnyDataAvailable = true;
                    }
                    else
                    {
                        isAllDataAvailable = false;
                        if ((sessionInformation.Isv4TV && drrt.IsTVValid) || (sessionInformation.Isv4NM && drrt.IsNMValid) || (sessionInformation.Isv4SM && drrt.IsSMValid))
                        {
                            isAnyDataAvailable = true;
                        }
                    }
                }
                else
                {
                    if (medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        if (drrt.IsTVValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }

                    else if (medium == CommonFunctions.CategoryType.NM.ToString())
                    {
                        if (drrt.IsNMValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }
                    else if (medium == "Social Media" ||
                                medium == CommonFunctions.CategoryType.Blog.ToString() ||
                            medium == CommonFunctions.CategoryType.Forum.ToString())
                    {
                        if (drrt.IsSMValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }
                }
            }

            if (isAllDataAvailable)
            {
                anyDataAvailable = true;
                return string.Empty;
            }
            else
            {
                if (isAnyDataAvailable)
                {
                    anyDataAvailable = true;
                    return GetAvailableDataString(dataAvailableList);
                }
                else
                {
                    anyDataAvailable = false;
                    return GetNoDataDataString(dataAvailableList);
                }
            }
        }

        public string GetChartMessage(string dataAvailableList, string medium)
        {
            List<DiscoveryResultRecordTrack> lstrecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryLiteTempData)GetTempData()).lstDiscoveryResultRecordTrack;
            bool isAllDataAvailable = true;
            bool isAnyDataAvailable = false;
            foreach (DiscoveryResultRecordTrack drrt in lstrecordTrack)
            {
                if (string.IsNullOrWhiteSpace(medium))
                {
                    if (drrt.IsTVValid && drrt.IsNMValid && drrt.IsSMValid)
                    {
                        isAnyDataAvailable = true;
                    }
                    else
                    {
                        isAllDataAvailable = false;
                        if ((sessionInformation.Isv4TV && drrt.IsTVValid) || (sessionInformation.Isv4NM && drrt.IsNMValid) || (sessionInformation.Isv4SM && drrt.IsSMValid))
                        {
                            isAnyDataAvailable = true;
                        }
                    }
                }
                else
                {
                    if (medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        if (drrt.IsTVValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }

                    else if (medium == CommonFunctions.CategoryType.NM.ToString())
                    {
                        if (drrt.IsNMValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }
                    else if (medium == "Social Media" ||
                                medium == CommonFunctions.CategoryType.Blog.ToString() ||
                            medium == CommonFunctions.CategoryType.Forum.ToString())
                    {
                        if (drrt.IsSMValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }


                }


            }
            if (isAllDataAvailable)
            {
                return string.Empty;
            }
            else
            {
                if (isAnyDataAvailable)
                {
                    return GetAvailableDataString(dataAvailableList);
                }
                else
                {
                    return GetNoDataDataString(dataAvailableList);
                }
            }
        }

        private string GetHTMLWithCSSIncluded(string p_HTML, string p_FromDate, string p_ToDate, bool p_IsEmail, string[] p_SearchTerm)
        {
            StringBuilder cssData = new StringBuilder();


            StreamReader strmReader = new StreamReader(Server.MapPath("~/css/Feed.css"));
            cssData.Append(strmReader.ReadToEnd());
            strmReader.Close();
            strmReader.Dispose();

            strmReader = new StreamReader(Server.MapPath("~/css/bootstrap.css"));
            cssData.Append(strmReader.ReadToEnd());
            strmReader.Close();
            strmReader.Dispose();

            cssData.Append(" .divtopres {width: 50%;} \n .pieChartChks {margin-top: 59px;} \n body {background:none;}");


            p_HTML = "<html><head><style type=\"text/css\">" + Convert.ToString(cssData) + "</style></head>" + "<body>" + "<img src=\"../../" + ConfigurationManager.AppSettings["IQMediaEmailLogo"] + "\" alt='IQMedia Logo'/>" + p_HTML + "</body></html>";

            HtmlDocument doc = new HtmlDocument();
            doc.Load(new StringReader(p_HTML));
            doc.OptionOutputOriginalCase = true;

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//svg"))
            {
                if (link.ParentNode != null && link.ParentNode.Name == "div" && link.ParentNode.Attributes["style"] != null && !link.ParentNode.Attributes["style"].Value.Contains("float"))
                {
                    link.ParentNode.Attributes["style"].Value = link.ParentNode.Attributes["style"].Value + ";float:left";
                }
            }

            if (p_IsEmail)
            {
                doc.DocumentNode.SelectSingleNode("//body").SetAttributeValue("style", "width:1000px;");
            }

            StringBuilder finalHTML = new StringBuilder(); ;
            string divColumnChart = string.Empty;
            if (doc.DocumentNode.SelectSingleNode("//div[@id='divColumnChart']") != null)
            {
                divColumnChart = doc.DocumentNode.SelectSingleNode("//div[@id='divColumnChart']").InnerHtml;
            }

            string divLineChart = string.Empty;
            if (doc.DocumentNode.SelectSingleNode("//div[@id='divLineChart']") != null)
            {
                divLineChart = doc.DocumentNode.SelectSingleNode("//div[@id='divLineChart']").InnerHtml;
            }

            string divPieChartSearchTerm = string.Empty;
            if (p_SearchTerm.Length > 1 && doc.DocumentNode.SelectSingleNode("//div[@id='divPieChartSearchTerm']") != null)
            {
                divPieChartSearchTerm = doc.DocumentNode.SelectSingleNode("//div[@id='divPieChartSearchTerm']").InnerHtml;
            }

            List<string> lstPieChart = new List<string>();
            for (int i = 0; i < p_SearchTerm.Length; i++)
            {
                finalHTML.Append("<div style=\"float:left;clear:both;width:100%;\">");
                if (!p_IsEmail && i > 0)
                {
                    finalHTML.Append("<div class=\"pagebreak\" >&nbsp;</div>");
                }
                else
                {
                    finalHTML.Append("<div class=\"clear\"><br/><br/></div>");
                }
                finalHTML.Append("<div class=\"searchTermData\">Search Term : " + p_SearchTerm[i] + "</div>");
                finalHTML.Append(divColumnChart);
                finalHTML.Append(divLineChart);
                finalHTML.Append(divPieChartSearchTerm);
                finalHTML.Append("</div>");
                finalHTML.Append("<div style=\"float:left;clear:both;width:100%;\">");
                if (doc.DocumentNode.SelectSingleNode("//div[@id='divPieChart_Child_" + i + "']") != null)
                {
                    finalHTML.Append(doc.DocumentNode.SelectSingleNode("//div[@id='divPieChart_Child_" + i + "']").InnerHtml);
                }
                finalHTML.Append("</div>");
            }

            string finalHTMLData = "<html><head><style type=\"text/css\">.pagebreak {display: block;clear: both; page-break-after: always;} .searchTermData {font-weight:bold} " + Convert.ToString(cssData) + "</style></head>" + "<body>" + "<img src=\"../../" + ConfigurationManager.AppSettings["IQMediaEmailLogo"] + "\" alt='IQMedia Logo'/><br/>" + Convert.ToString(finalHTML) + "</body></html>";
            return finalHTMLData;
        }

        #endregion

        #region SSP
        public Dictionary<string, object> GetSSPData(Guid p_ClientGUID)
        {
            try
            {
                bool isAllDmaAllowed = false;
                bool isAllClassAllowed = false;
                bool isAllStationAllowed = false;

                discoveryLiteTempData = GetTempData();

                SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                Dictionary<string, object> dictSSP = sspLogic.GetSSPDataByClientGUID(p_ClientGUID, out isAllDmaAllowed, out isAllClassAllowed, out isAllStationAllowed, discoveryLiteTempData.IQTVRegion);
                discoveryLiteTempData.IsAllDmaAllowed = isAllDmaAllowed;
                discoveryLiteTempData.IsAllClassAllowed = isAllClassAllowed;
                discoveryLiteTempData.IsAllStationAllowed = isAllStationAllowed;

                SetTempData(discoveryLiteTempData);
                //TempData.Keep();

                return dictSSP;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }
        #endregion
        #region

        public MediaChartJsonResponse GetChartData(string[] searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket, Guid p_ClientGUID, bool IsInsertFromRecordID, decimal clientGmtOffset, decimal clientDstOffset)
        {

            MediaChartJsonResponse mediaChartJsonResponse = new MediaChartJsonResponse();
            try
            {
                if (SearchTermValidation(searchTerm))
                {
                    lstSMResponseFeedClass = new List<DiscoveryLiteSearchResponse>();

                    lstMainDiscoverySearchResponse = new List<DiscoveryLiteSearchResponse>();
                    //string dataNotAvailableList = string.Empty;
                    string dataAvailableList = string.Empty;

                    SearchMedia(searchTerm, fromDate, toDate, medium, tvMarket, IsInsertFromRecordID, p_ClientGUID, out dataAvailableList);

                    //dataNotAvailableList = string.IsNullOrWhiteSpace(dataNotAvailableList) ? dataNotAvailableList : "Data not available : " + dataNotAvailableList;

                    var columnChartData = ColumnChart(lstMainDiscoverySearchResponse);

                    bool isHourData = false;

                    /* if ((toDate.Value - fromDate.Value).TotalDays <= 1)
                     {
                         isHourData = true;
                     }*/

                    TimeSpan dateDiff = (TimeSpan)(toDate.Value - fromDate.Value);

                    if (dateDiff.Days <= 1)
                    {
                        isHourData = true;
                    }

                    Log4NetLogger.Debug("IsHourData: " + isHourData.ToString());

                    var lineChartData = LineChart(lstMainDiscoverySearchResponse, isHourData, clientGmtOffset, clientDstOffset);

                    var pieChartSearchTermData = PieChartBySearchTerm(lstMainDiscoverySearchResponse, searchTerm, medium);
                    var pieChartMediumData = PieChartByMedium(lstMainDiscoverySearchResponse, searchTerm, medium);
                    var mediumFilter = GetMediumFilter(lstMainDiscoverySearchResponse);

                    pieChartMediumData = GetTopResult((List<PieChartResponse>)pieChartMediumData);
                    mediaChartJsonResponse.ColumnChartData = columnChartData;
                    mediaChartJsonResponse.LineChartData = lineChartData;
                    mediaChartJsonResponse.PieChartMediumData = pieChartMediumData;
                    mediaChartJsonResponse.PieChartSearchTermData = pieChartSearchTermData;
                    //mediaChartJsonResponse.DataNotAvailableList = dataNotAvailableList;
                    mediaChartJsonResponse.DataAvailableList = dataAvailableList;
                    mediaChartJsonResponse.MediumFilter = mediumFilter;
                    if (lstTVMarket != null)
                    {
                        mediaChartJsonResponse.TVMarket = lstTVMarket.Select(s => s).Distinct().ToList();
                    }
                    mediaChartJsonResponse.IsSearchTermValid = true;
                    mediaChartJsonResponse.IsSuccess = true;


                }
                else
                {
                    mediaChartJsonResponse.IsSuccess = true;
                    mediaChartJsonResponse.IsSearchTermValid = false;
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("error while get chart data", ex);
                mediaChartJsonResponse.IsSuccess = false;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
            return mediaChartJsonResponse;
        }
        #endregion


        #region From Record ID and Paging Track

        public void UpdateFromRecordID(List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse)
        {
            List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryLiteTempData)GetTempData()).lstDiscoveryResultRecordTrack;// TempData["DiscoveryResultRecordTrack"];
            string[] distinctSearchTerm = lstDiscoverySearchResponse.Select(s => s.SearchTerm).Distinct().ToArray();


            foreach (string sTerm in distinctSearchTerm)
            {

                DiscoveryResultRecordTrack discoveryResultRecordTrack = lstDiscoveryResultRecordTrack.Where(w => w.SearchTerm.Equals(sTerm)).FirstOrDefault();
                discoveryResultRecordTrack.SearchTerm = sTerm;

                if (lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.TV.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault() != null)
                {
                    discoveryResultRecordTrack.TVRecordTotal = lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.TV.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().TotalResult;
                    discoveryResultRecordTrack.IsTVValid = //discoveryResultRecordTrack.TVRecordTotal > 0 ? true : false;
                    lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.TV.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().IsValid;
                }
                else
                {
                    if (sessionInformation.Isv4TV)
                    {
                        discoveryResultRecordTrack.IsTVValid = false;
                    }
                }

                if (lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault() != null)
                {
                    discoveryResultRecordTrack.NMRecordTotal = lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().TotalResult;
                    discoveryResultRecordTrack.IsNMValid = //discoveryResultRecordTrack.NMRecordTotal > 0 ? true : false;
                    lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().IsValid;
                    //discoveryResultRecordTrack.NMFromRecordID = lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).Select(s => s.FromRecordID).FirstOrDefault();
                }
                else
                {
                    if (sessionInformation.Isv4NM)
                    {
                        discoveryResultRecordTrack.IsNMValid = false;
                    }
                }

                if (lstDiscoverySearchResponse.Where(w => (w.MediumType.Equals(CommonFunctions.CategoryType.SocialMedia.ToString())
                                                                                || w.MediumType.Equals(CommonFunctions.CategoryType.Blog.ToString())
                                                                                || w.MediumType.Equals(CommonFunctions.CategoryType.Forum.ToString())) && w.SearchTerm.Equals(sTerm)).FirstOrDefault() != null)
                {
                    discoveryResultRecordTrack.SMRecordTotal = lstDiscoverySearchResponse.Where(w => (w.MediumType.Equals(CommonFunctions.CategoryType.SocialMedia.ToString())
                                                                                || w.MediumType.Equals(CommonFunctions.CategoryType.Blog.ToString())
                                                                                || w.MediumType.Equals(CommonFunctions.CategoryType.Forum.ToString())) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().TotalResult;

                    discoveryResultRecordTrack.IsSMValid = //discoveryResultRecordTrack.SMRecordTotal > 0 ? true : false;
                    lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.SocialMedia.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().IsValid;
                    /*discoveryResultRecordTrack.SMFromRecordID = lstDiscoverySearchResponse.Where(w => (w.MediumType.Equals(CommonFunctions.CategoryType.SocialMedia.ToString())
                                                                                || w.MediumType.Equals(CommonFunctions.CategoryType.Blog.ToString())
                                                                                || w.MediumType.Equals(CommonFunctions.CategoryType.Forum.ToString()))
                                                                                && (w.SearchTerm.Equals(sTerm)))
                                                                                .Select(s => s.FromRecordID).FirstOrDefault();*/
                }
                else
                {
                    if (sessionInformation.Isv4SM)
                    {
                        discoveryResultRecordTrack.IsSMValid = false;
                    }
                }

                discoveryResultRecordTrack.TotalRecords = lstDiscoverySearchResponse.Where(w => w.SearchTerm.Equals(sTerm)).Sum(s => s.TotalResult);
            }

            discoveryLiteTempData = GetTempData();
            discoveryLiteTempData.lstDiscoveryResultRecordTrack = lstDiscoveryResultRecordTrack;
            SetTempData(discoveryLiteTempData);
        }
        #endregion

        #endregion

        #region TempData

        private object GetSessionData()
        {

            if (Session["DiscoveryResultRecordTrack"] == null)
            {

                DiscoveryResultRecordTrack discoveryResultRecordTrack = new DiscoveryResultRecordTrack();
                List<DiscoveryResultRecordTrack> lst = new List<DiscoveryResultRecordTrack>();
                lst.Add(discoveryResultRecordTrack);
                Session["DiscoveryResultRecordTrack"] = lst;
            }

            return Session["DiscoveryResultRecordTrack"];

        }
        private void SetSessionData(List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack)
        {
            Session["DiscoveryResultRecordTrack"] = lstDiscoveryResultRecordTrack;
        }
        #endregion

        #region Top Results

        public List<PieChartResponse> GetTopResult(List<PieChartResponse> lstPieChartResponse)
        {
            try
            {
                List<String> distinctSearchTerm = lstMainDiscoverySearchResponse.Select(s => s.SearchTerm).Distinct().ToList();
                foreach (string sTerm in distinctSearchTerm)
                {
                    if (lstMainDiscoverySearchResponse.Where(w => string.Compare(w.SearchTerm, sTerm, true) == 0).SelectMany(s => s.ListTopResults) != null)
                    {
                        lstPieChartResponse.Where(w => string.Compare(w.SearchTerm, sTerm, true) == 0).FirstOrDefault().TopResultHtml =
                            RenderPartialToString(PATH_DiscoveryTopResultsPartialView, lstMainDiscoverySearchResponse.Where(w => string.Compare(w.SearchTerm, sTerm, true) == 0).SelectMany(s => s.ListTopResults).ToList());
                    }
                }

                return lstPieChartResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        #endregion

        #region Utility

        public DiscoveryLiteTempData GetTempData()
        {
            if (TempData["DiscoveryLiteTempData"] == null)
            {
                DiscoveryResultRecordTrack discoveryResultRecordTrack = new DiscoveryResultRecordTrack();
                List<DiscoveryResultRecordTrack> lst = new List<DiscoveryResultRecordTrack>();
                lst.Add(discoveryResultRecordTrack);
                discoveryLiteTempData = new DiscoveryLiteTempData();
                discoveryLiteTempData.lstDiscoveryResultRecordTrack = lst;
                //Session["DiscoveryResultRecordTrack"] = lst;
            }
            else
            {
                discoveryLiteTempData = (DiscoveryLiteTempData)TempData["DiscoveryLiteTempData"];
            }
            //discoveryTempData = TempData["DiscoveryTempData"] != null ? (DiscoveryTempData)TempData["DiscoveryTempData"] : new DiscoveryTempData();

            return discoveryLiteTempData;
        }

        public void SetTempData(DiscoveryLiteTempData p_DiscoveryTempData)
        {
            TempData["DiscoveryLiteTempData"] = p_DiscoveryTempData;
            TempData.Keep("DiscoveryLiteTempData");
        }

        #endregion
    }
}
