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

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class TimeshiftController : Controller
    {
        //
        // GET: /Timeshift/
        #region Public Property
        ActiveUser sessionInformation = null;
        TimeShiftTempData timeShiftTempData = null;
        string PATH_DiscoveryPartialView = "~/Views/Timeshift/_Result.cshtml";
        bool hasMoreResultPage = false;
        bool hasPreviousResultPage = false;
        string recordNumberDesc = string.Empty;
        string PATH_TimeshiftRadioResults = "~/Views/Timeshift/_RadioResults.cshtml";
        string PATH_TimeshiftSavedSearchPartialView = "~/Views/Timeshift/_SavedSearch.cshtml";

        #endregion

        public ActionResult Index()
        {
            try
            {
                // Clear out temp data used by other pages
                if (TempData.ContainsKey("AnalyticsTempData")) { TempData["AnalyticsTempData"] = null; }
                if (TempData.ContainsKey("FeedsTempData")) { TempData["FeedsTempData"] = null; }
                if (TempData.ContainsKey("DiscoveryTempData")) { TempData["DiscoveryTempData"] = null; }
                if (TempData.ContainsKey("TAdsTempData")) { TempData["TAdsTempData"] = null; }
                if (TempData.ContainsKey("SetupTempData")) { TempData["SetupTempData"] = null; }
                if (TempData.ContainsKey("GlobalAdminTempData")) { TempData["GlobalAdminTempData"] = null; }

                SetTempData(null);
                timeShiftTempData = GetTempData();
                timeShiftTempData.SinceID = null;
                timeShiftTempData.TotalResults = null;
                timeShiftTempData.CurrentPage = null;
                timeShiftTempData.p_IsAllClassAllowed = false;
                timeShiftTempData.p_IsAllClassAllowed = false;
                timeShiftTempData.p_IsAllStationAllowed = false;
                timeShiftTempData.ActiveSearch = null;

                /*TempData["SinceID"] = null;
                TempData["TotalResults"] = null;
                TempData["CurrentPage"] = null;


                TempData["p_IsAllDmaAllowed"] = null;
                TempData["p_IsAllClassAllowed"] = null;
                TempData["p_IsAllStationAllowed"] = null;*/
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                // if (sessionInformation != null)

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                //Add TV Country filter
                //timeShiftTempData.IQTVRegion = clientLogic.GetClientTVRegionSettings(sessionInformation.ClientGUID);
                //var IQTVRegionCountry = clientLogic.GetClientTVRegionSettings(sessionInformation.ClientGUID);
                //if (IQTVRegionCountry != null && IQTVRegionCountry.Count > 0)
                //{
                //    timeShiftTempData.IQTVRegion = (List<int>)IQTVRegionCountry[0];
                //}
                //if (IQTVRegionCountry != null && IQTVRegionCountry.Count > 1)
                //{
                //    timeShiftTempData.IQTVCountry = (List<int>)IQTVRegionCountry[1];
                //}
                var manualClipDuration = clientLogic.GetClientManualClipDurationSettings(sessionInformation.ClientGUID);
                Int16 rawMediaPauseSecs = clientLogic.GetClientRawMediaPauseSecs(sessionInformation.ClientGUID);


                // {
                SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                bool p_IsAllDmaAllowed;
                bool p_IsAllClassAllowed;
                bool p_IsAllStationAllowed;
                Dictionary<string, object> dicSSP = sspLogic.GetSSPDataByClientGUID(sessionInformation.ClientGUID, out p_IsAllDmaAllowed, out p_IsAllClassAllowed, out p_IsAllStationAllowed, timeShiftTempData.IQTVRegion);
                /*TempData["p_IsAllDmaAllowed"] = p_IsAllDmaAllowed;
                TempData["p_IsAllClassAllowed"] = p_IsAllClassAllowed;
                TempData["p_IsAllStationAllowed"] = p_IsAllStationAllowed;*/


                timeShiftTempData.p_IsAllDmaAllowed = p_IsAllDmaAllowed;
                timeShiftTempData.p_IsAllClassAllowed = p_IsAllClassAllowed;
                timeShiftTempData.p_IsAllStationAllowed = p_IsAllStationAllowed;
                List<IQ_Region> IQTVRegionList = (List<IQ_Region>)dicSSP["IQ_Region"];
                List<IQ_Country> IQTVCountryList = (List<IQ_Country>)dicSSP["IQ_Country"];
                timeShiftTempData.IQTVRegion = IQTVRegionList.Select(r => r.Num).ToList();
                timeShiftTempData.IQTVCountry = IQTVCountryList.Select(c => c.Num).ToList();

                SetTempData(timeShiftTempData);

                //TempData.Keep();
                string strHtml = TimeshiftDefaultResults("", "", null, null, null, null, null, "", null, null, false);
                dicSSP.Add("DefaultHTML", strHtml);
                ViewBag.IsSuccess = true;
                ViewBag.ManualClipDuration = manualClipDuration;
                ViewBag.RawMediaPauseSecs = rawMediaPauseSecs;
                return View(dicSSP);
                // }


                //else
                //{
                //    return RedirectToAction("Index", "Home");
                //}

            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                ViewBag.IsSuccess = false;
            }
            finally
            {
                TempData.Keep("TimeShiftTempData");
            }
            return View();
        }
        public string TimeshiftDefaultResults(string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                int ResultCount = 0;
                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftPageSize"]);

                timeShiftTempData = GetTempData();
                timeShiftTempData.PageNumber = 0;

                List<IQAgent_TVResultsModel> tvResult = new List<IQAgent_TVResultsModel>();
                TVLogic tvlogic = new TVLogic();
                bool isallDmaAllowed = timeShiftTempData.p_IsAllDmaAllowed;
                bool isallClassAllowed = timeShiftTempData.p_IsAllClassAllowed;
                bool isallStationAllowed = timeShiftTempData.p_IsAllStationAllowed;

                /*TempData["p_IsAllDmaAllowed"] = isallDmaAllowed;
                TempData["p_IsAllClassAllowed"] = isallClassAllowed;
                TempData["p_IsAllStationAllowed"] = isallStationAllowed;*/
                //TempData.Keep();

                //TempData["p_IsAllStationAllowed"] = p_IsAllStationAllowed;

                tvResult = tvlogic.TimeshiftSearchResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, p_SearchTerm, p_Title, p_FromDate, p_ToDate, p_Dma, p_Station, p_IQStationID, p_Class, p_IsAsc, 0, isallDmaAllowed, isallClassAllowed, isallStationAllowed, string.Empty, ref ResultCount, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.TV.ToString(), p_FromDate, p_ToDate), timeShiftTempData.IQTVRegion, timeShiftTempData.IQTVCountry, p_Region, p_Country);
                //tvResult = CommonFunctions.GetGMTandDSTTime(tvResult, CommonFunctions.ResultType.Library);
                // var culture = CultureInfo.CreateSpecificCulture("en-US");
                timeShiftTempData.ResultCount = ResultCount;
                //TempData["ResultCount"] = ResultCount;
                hasMoreResultPage = false;
                hasPreviousResultPage = false;

                if (tvResult != null && ResultCount > 0)
                {
                    double totalHit = Convert.ToDouble(ResultCount) / PageSize;
                    if (totalHit > (timeShiftTempData.PageNumber + 1))
                    {
                        hasMoreResultPage = true;

                        recordNumberDesc = (((timeShiftTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ((timeShiftTempData.PageNumber + 1) * PageSize).ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    else
                    {
                        hasMoreResultPage = false;
                        recordNumberDesc = (((timeShiftTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ResultCount.ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                }
                if (timeShiftTempData.PageNumber > 0)
                {
                    hasPreviousResultPage = true;
                }
                timeShiftTempData.HasMoreResultPage = hasMoreResultPage;
                timeShiftTempData.HasPreviousResultPage = hasPreviousResultPage;
                timeShiftTempData.RecordNumber = recordNumberDesc;

                SetTempData(timeShiftTempData);
                //TempData.Keep();
                string htmlResult = RenderPartialToString(PATH_DiscoveryPartialView, tvResult);

                return htmlResult;

            }
            catch (IQMedia.Shared.Utility.CustomException ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex, ex.Message);
                return ConfigSettings.Settings.ErrorOccurred;
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return "";
            }
            finally
            {
                TempData.Keep("TimeShiftTempData");
            }
            //return Json(new object());
        }
        [HttpPost]
        public JsonResult TimeshiftSearchResults(string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc, string p_SortColumn, bool p_IsDefaultLoad)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                int ResultCount = 0;
                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftPageSize"]);


                timeShiftTempData = GetTempData();
                timeShiftTempData.PageNumber = 0;

                if (p_IsDefaultLoad)
                {
                    timeShiftTempData.ActiveSearch = null;
                }

                //TempData["PageNumber"] = 0;
                List<IQAgent_TVResultsModel> tvResult = new List<IQAgent_TVResultsModel>();
                TVLogic tvlogic = new TVLogic();
                bool isallDmaAllowed = timeShiftTempData.p_IsAllDmaAllowed;// Convert.ToBoolean(TempData["p_IsAllDmaAllowed"]);
                bool isallClassAllowed = timeShiftTempData.p_IsAllClassAllowed; //Convert.ToBoolean(TempData["p_IsAllClassAllowed"]);
                bool isallStationAllowed = timeShiftTempData.p_IsAllStationAllowed;// Convert.ToBoolean(TempData["p_IsAllStationAllowed"]);

                Dictionary<string, object> filters = null;

                //if (timeShiftTempData.SelectedDma != p_Dma || timeShiftTempData.SelectedStation != p_Station)
                //{
                string strDmaXml = null;
                if (p_Dma != null && p_Dma.Count > 0)
                {
                    XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in p_Dma
                                             select new XElement("dma", new XAttribute("name", ele))
                                                     ));
                    strDmaXml = xdoc.ToString();
                }

                string strStationXml = null;
                if (p_Station != null && p_Station.Count > 0)
                {
                    XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in p_Station
                                             select new XElement("station", new XAttribute("name", ele))
                                                     ));
                    strStationXml = xdoc.ToString();
                }

                string strStationIDXml = null;
                if (p_IQStationID != null && p_IQStationID.Count > 0)
                {
                    XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in p_IQStationID
                                             select new XElement("stationid", new XAttribute("id", ele))
                                                     ));
                    strStationIDXml = xdoc.ToString();
                }
                SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                filters = sspLogic.GetSSPDataByClientGUIDAndFilter(sessionInformation.ClientGUID, strDmaXml, strStationXml, strStationIDXml, p_Region, p_Country, timeShiftTempData.IQTVRegion);
                //}

                timeShiftTempData.SelectedDma = p_Dma;
                timeShiftTempData.SelectedStation = p_Station;

                /*TempData["p_IsAllDmaAllowed"] = isallDmaAllowed;
                TempData["p_IsAllClassAllowed"] = isallClassAllowed;
                TempData["p_IsAllStationAllowed"] = isallStationAllowed;*/
                //TempData.Keep();
                //TempData["p_IsAllStationAllowed"] = p_IsAllStationAllowed;
                tvResult = tvlogic.TimeshiftSearchResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, p_SearchTerm, p_Title, p_FromDate, p_ToDate, p_Dma, p_Station, p_IQStationID, p_Class, p_IsAsc, 0, isallDmaAllowed, isallClassAllowed, isallStationAllowed, p_SortColumn, ref ResultCount, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.TV.ToString(), p_FromDate, p_ToDate), timeShiftTempData.IQTVRegion, timeShiftTempData.IQTVCountry, p_Region, p_Country);
                //tvResult = CommonFunctions.GetGMTandDSTTime(tvResult, CommonFunctions.ResultType.Library);
                timeShiftTempData.ResultCount = ResultCount;
                //TempData["ResultCount"] = ResultCount;
                hasMoreResultPage = false;
                hasPreviousResultPage = false;

                //Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["PMGSearchUrl"].ToString());
                //SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);

                //SearchRequest tvRequest = new SearchRequest();
                //tvRequest.IsShowCC = false;
                //tvRequest.IsSentiment = false;
                //tvRequest.Terms = !string.IsNullOrEmpty(p_SearchTerm) ? p_SearchTerm : null;
                //tvRequest.PageSize = PageSize;
                //if (p_IsAsc == true)
                //{
                //    tvRequest.SortFields = "datetime";

                //}
                //else
                //{
                //    tvRequest.SortFields = "datetime-";
                //}

                //tvRequest.ClientGuid = new Guid("7722A116-C3BC-40AE-8070-8C59EE9E3D2A");
                //if (p_FromDate != null)
                //{
                //    tvRequest.StartDate = p_FromDate;
                //}
                //if (p_ToDate != null)
                //{
                //    tvRequest.EndDate = p_ToDate;
                //}
                //if (!string.IsNullOrEmpty(p_Dma))
                //{
                //    List<string> tempDma = new List<string>();
                //    tempDma.Add(p_Dma);

                //    tvRequest.IQDmaName = tempDma; // !string.IsNullOrEmpty(p_Dma) ? p_Dma.ToList() : null;
                //}
                //if (!string.IsNullOrEmpty(p_Station))
                //{
                //    List<string> tempStation = new List<string>();
                //    tempStation.Add(p_Station);

                //    tvRequest.StationAffil = tempStation; // !string.IsNullOrEmpty(p_Dma) ? p_Dma.ToList() : null;
                //}
                //if (!string.IsNullOrEmpty(p_Class))
                //{
                //    List<string> tempClass = new List<string>();
                //    tempClass.Add(p_Class);

                //    tvRequest.IQClassNum = tempClass; // !string.IsNullOrEmpty(p_Dma) ? p_Dma.ToList() : null;
                //}



                //SearchResult tvResult = searchEngine.Search(tvRequest);

                if (tvResult != null && ResultCount > 0)
                {
                    double totalHit = Convert.ToDouble(ResultCount) / PageSize;

                    if (totalHit > (timeShiftTempData.PageNumber + 1))
                    {
                        hasMoreResultPage = true;
                        recordNumberDesc = (((timeShiftTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ((timeShiftTempData.PageNumber + 1) * PageSize).ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    else
                    {
                        hasMoreResultPage = false;
                        recordNumberDesc = (((timeShiftTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ResultCount.ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                }
                if (timeShiftTempData.PageNumber > 0)
                {
                    hasPreviousResultPage = true;
                }
                timeShiftTempData.HasMoreResultPage = hasMoreResultPage;
                timeShiftTempData.HasPreviousResultPage = hasPreviousResultPage;
                timeShiftTempData.RecordNumber = recordNumberDesc;

                /*TempData["hasMoreResultPage"] = hasMoreResultPage;
                TempData["hasPreviousResultPage"] = hasPreviousResultPage;*/
                //TempData["recordNumber"] = recordNumberDesc;
                //TempData.Keep();
                SetTempData(timeShiftTempData);
                string htmlResult = RenderPartialToString(PATH_DiscoveryPartialView, tvResult);

                return Json(new
                {
                    //hasMoreResults = HasMoreResults(searchTermWiseTotalRecords, shownRecords),                    

                    HTML = htmlResult != null ? htmlResult : "",
                    hasMoreResult = hasMoreResultPage,
                    filters = filters,
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
                TempData.Keep("TimeShiftTempData");
            }
            //return Json(new object());
        }

        [HttpPost]
        public JsonResult TimeshiftSearchResultsPaging(string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc, bool p_IsNext, string p_SortColumn)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                timeShiftTempData = GetTempData();

                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftPageSize"]);
                hasMoreResultPage = false;
                hasPreviousResultPage = false;
                int ResultCount = 0;
                //Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["PMGSearchUrl"].ToString());
                //SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                Int32 pagenumber = 0;
                //SearchRequest tvRequest = new SearchRequest();
                //tvRequest.IsShowCC = false;
                //tvRequest.IsSentiment = false;
                //tvRequest.Terms = !string.IsNullOrEmpty(p_SearchTerm) ? p_SearchTerm : null;
                //tvRequest.PageSize = PageSize;
                if (p_IsNext)
                {
                    if (timeShiftTempData.HasMoreResultPage)
                    {
                        if (timeShiftTempData.PageNumber != null)
                        {
                            pagenumber = timeShiftTempData.PageNumber + 1;
                            timeShiftTempData.PageNumber = timeShiftTempData.PageNumber + 1;
                        }
                        else
                        {
                            timeShiftTempData.PageNumber = 1;
                            pagenumber = 1;
                        }
                    }
                    else
                    {
                        // IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                        return Json(new
                        {
                            isSuccess = false
                        });
                    }
                }
                else
                {
                    if (timeShiftTempData.HasPreviousResultPage)
                    {
                        if (timeShiftTempData.PageNumber != null)
                        {
                            pagenumber = timeShiftTempData.PageNumber - 1;
                            timeShiftTempData.PageNumber = timeShiftTempData.PageNumber - 1;
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            isSuccess = false
                        });
                    }

                }

                //if (p_IsAsc == true)
                //{
                //    tvRequest.SortFields = "datetime";

                //}
                //else
                //{
                //    tvRequest.SortFields = "datetime-";
                //}

                //tvRequest.ClientGuid = new Guid("7722A116-C3BC-40AE-8070-8C59EE9E3D2A");
                //if (p_FromDate != null)
                //{
                //    tvRequest.StartDate = p_FromDate;
                //}
                //if (p_ToDate != null)
                //{
                //    tvRequest.EndDate = p_ToDate;
                //}
                //if (!string.IsNullOrEmpty(p_Dma))
                //{
                //    List<string> tempDma = new List<string>();
                //    tempDma.Add(p_Dma);

                //    tvRequest.IQDmaName = tempDma; // !string.IsNullOrEmpty(p_Dma) ? p_Dma.ToList() : null;
                //}
                //if (!string.IsNullOrEmpty(p_Station))
                //{
                //    List<string> tempStation = new List<string>();
                //    tempStation.Add(p_Station);

                //    tvRequest.StationAffil = tempStation; // !string.IsNullOrEmpty(p_Dma) ? p_Dma.ToList() : null;
                //}
                //if (!string.IsNullOrEmpty(p_Class))
                //{
                //    List<string> tempClass = new List<string>();
                //    tempClass.Add(p_Class);

                //    tvRequest.IQClassNum = tempClass; // !string.IsNullOrEmpty(p_Dma) ? p_Dma.ToList() : null;
                //}



                //SearchResult tvResult = searchEngine.Search(tvRequest);
                ////foreach (Hit hit in tvResult.Hits)
                //{
                //    xDoc.Root.Add(new XElement("item", new XAttribute("iq_cc_key", hit.Iqcckey), new XAttribute("iq_dma", hit.IQDmaNum)));
                //}


                List<IQAgent_TVResultsModel> tvResult = new List<IQAgent_TVResultsModel>();
                TVLogic tvlogic = new TVLogic();
                bool isallDmaAllowed = timeShiftTempData.p_IsAllDmaAllowed;// Convert.ToBoolean(TempData["p_IsAllDmaAllowed"]);
                bool isallClassAllowed = timeShiftTempData.p_IsAllClassAllowed;// Convert.ToBoolean(TempData["p_IsAllClassAllowed"]);
                bool isallStationAllowed = timeShiftTempData.p_IsAllStationAllowed;// Convert.ToBoolean(TempData["p_IsAllStationAllowed"]);

                /*TempData["p_IsAllDmaAllowed"] = isallDmaAllowed;
                TempData["p_IsAllClassAllowed"] = isallClassAllowed;
                TempData["p_IsAllStationAllowed"] = isallStationAllowed;*/
                //TempData.Keep();
                tvResult = tvlogic.TimeshiftSearchResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, p_SearchTerm, p_Title, p_FromDate, p_ToDate, p_Dma, p_Station, p_IQStationID, p_Class, p_IsAsc, pagenumber, isallDmaAllowed, isallClassAllowed, isallStationAllowed, p_SortColumn, ref ResultCount, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.TV.ToString(), p_FromDate, p_ToDate), timeShiftTempData.IQTVRegion, timeShiftTempData.IQTVCountry, p_Region, p_Country);
                //tvResult = CommonFunctions.GetGMTandDSTTime(tvResult, CommonFunctions.ResultType.Library);
                if (tvResult != null && ResultCount > 0)
                {
                    double totalHit = Convert.ToDouble(ResultCount) / PageSize;
                    if (totalHit > (timeShiftTempData.PageNumber + 1))
                    {
                        hasMoreResultPage = true;
                        recordNumberDesc = (((timeShiftTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ((timeShiftTempData.PageNumber + 1) * PageSize).ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    else
                    {
                        hasMoreResultPage = false;
                        recordNumberDesc = (((timeShiftTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ResultCount.ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    //if (totalHit > (pagenumber + 1))
                    //{
                    //    hasMoreResultPage = true;
                    //    recordNumberDesc = Convert.ToString(((pagenumber + 1) * PageSize) - PageSize + 1) + " - " + Convert.ToString(((pagenumber + 1) * PageSize)) + " of " + ResultCount.ToString("N0");
                    //}
                    //else
                    //{
                    //    hasMoreResultPage = false;
                    //    recordNumberDesc = Convert.ToString(((pagenumber + 1) * PageSize) - PageSize + 1) + " - " + Convert.ToString(ResultCount) + " of " + ResultCount.ToString("N0");
                    //}
                }
                if (pagenumber > 0)
                {
                    hasPreviousResultPage = true;
                }
                else
                {
                    hasPreviousResultPage = false;
                }
                timeShiftTempData.HasMoreResultPage = hasMoreResultPage;
                timeShiftTempData.HasPreviousResultPage = hasPreviousResultPage;
                SetTempData(timeShiftTempData);

                string htmlResult = RenderPartialToString(PATH_DiscoveryPartialView, tvResult);

                return Json(new
                {
                    //hasMoreResults = HasMoreResults(searchTermWiseTotalRecords, shownRecords),                    

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
                TempData.Keep("TimeShiftTempData");
            }
            //return Json(new object());
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
        public JsonResult SelectAllRadioStations()
        {
            sessionInformation = ActiveUserMgr.GetActiveUser();
            try
            {
                if (sessionInformation.Isv4TimeshiftRadioAccess)
                {
                    RadioLogic radioLogic = (RadioLogic)LogicFactory.GetLogic(LogicType.Radio);
                    List<RadioStation> radioStationList = radioLogic.SelectRadioStations();
                    return Json(new
                    {
                        isSuccess = true,
                        market = radioStationList.Select(rs => rs.DMA).Distinct().OrderBy(a => a),
                        station = radioStationList.Select(rs => rs.StationID).Distinct().OrderBy(s => s)
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = true,
                        market = new List<string>(),
                        station = new List<string>()
                    });
                }
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("TimeShiftTempData");
            }
        }

        [HttpPost]
        public JsonResult SelectRadioStationResults(DateTime? p_FromDate, DateTime? p_ToDate, string p_Market, bool p_IsAsc, bool p_IsNext, bool p_IsPrevNext, string p_Station, string p_SearchTerm)
        {
            sessionInformation = ActiveUserMgr.GetActiveUser();
            timeShiftTempData = GetTempData();

            try
            {
                if (sessionInformation.Isv4TimeshiftRadioAccess)
                {
                    long totalResults = 0;
                    long sinceID = timeShiftTempData.SinceID != null && p_IsPrevNext ? timeShiftTempData.SinceID.Value : 0;
                    int CurrentPage = (p_IsPrevNext && timeShiftTempData.CurrentPage != null) ? timeShiftTempData.CurrentPage.Value : 0;
                    bool IsPreviousEnable = false, IsNextEnable = false;
                    int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftPageSize"]);


                    if (p_IsPrevNext)
                    {
                        if (p_IsNext)
                        {
                            if (timeShiftTempData.TotalResults > ((CurrentPage+1)*PageSize))
                            {
                                CurrentPage = CurrentPage + 1; 
                            }
                            else
                            {
                                CurrentPage = 0;
                            }
                        }
                        else
                        {
                            if (CurrentPage > 0)
                            {
                                CurrentPage = CurrentPage - 1; 
                            }
                            else
                            {
                                CurrentPage = 0;
                            }
                        }
                    }

                    RadioLogic radioLogic = (RadioLogic)LogicFactory.GetLogic(LogicType.Radio);

                    List<string> marketList = null;

                    if (!string.IsNullOrWhiteSpace(p_Market))
                    {
                        marketList = new List<string> { p_Market };
                    }

                    List<string> stationList = null;

                    if (!string.IsNullOrWhiteSpace(p_Station))
                    {
                        stationList = new List<string> { p_Station };
                    }

                    if (p_ToDate != null)
                    {
                        p_ToDate = new DateTime(p_ToDate.Value.Year, p_ToDate.Value.Month, p_ToDate.Value.Day, 23, 59, 59);
                    }

                    Dictionary<string, object> radioSearchResults = radioLogic.SelectRadioResults(p_FromDate, p_ToDate, marketList, stationList, CommonFunctions.GeneratePMGUrl("QR", p_FromDate, p_ToDate), Convert.ToInt32(ConfigurationManager.AppSettings["IQRadioFragOffset"]), Convert.ToInt32(ConfigurationManager.AppSettings["IQRadioFragSize"]), true, !p_IsPrevNext, Convert.ToBoolean(ConfigurationManager.AppSettings["IQRadioIsLogging"]), ConfigurationManager.AppSettings["IQRadioLogFileLocation"], false, p_SearchTerm, ConfigurationManager.AppSettings["IQRadioSolrFL"], p_IsAsc, CurrentPage, PageSize, ref sinceID, out totalResults);
                    List<RadioModel> lstRadio = (List<RadioModel>)radioSearchResults["RadioList"];
                    //  radioLogic.SelectRadioResults(p_FromDate, p_ToDate, p_Market, p_IsAsc, CurrentPage, PageSize, ref sinceID, out totalResults);

                    string resultHTML = RenderPartialToString(PATH_TimeshiftRadioResults, lstRadio);


                    if (CurrentPage > 0)
                    {
                        IsPreviousEnable = true;
                    }

                    if (totalResults > 0)
                    {
                        long NoofPages = 0;

                        if (totalResults % PageSize == 0)
                        {
                            NoofPages = totalResults / PageSize;
                        }
                        else
                        {
                            NoofPages = Convert.ToInt32(totalResults / PageSize) + 1;
                        }
                        IsNextEnable = (CurrentPage < NoofPages);
                    }

                    timeShiftTempData.TotalResults = totalResults;
                    timeShiftTempData.SinceID = p_IsPrevNext ? timeShiftTempData.SinceID : sinceID;
                    timeShiftTempData.CurrentPage = CurrentPage;
                    SetTempData(timeShiftTempData);

                    return Json(new
                    {
                        isSuccess = true,
                        HTML = resultHTML,
                        isPrevEnable = IsPreviousEnable,
                        isNextEnable = IsNextEnable,
                        startRecord = ((CurrentPage * PageSize) + 1).ToString("N0"),
                        endRecord = (((CurrentPage+1) * PageSize) < totalResults ? ((CurrentPage+1) * PageSize) : totalResults).ToString("N0"),
                        totalRecords = totalResults.ToString("N0"),
                        hasResults = (totalResults > 0)
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = true,
                        HTML = string.Empty
                    });
                }
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("TimeShiftTempData");
            }
        }

        [HttpPost]
        public JsonResult GetIsTimeshiftFacet()
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();

                return Json(new
                {
                    isSuccess = true,
                    isTimeshiftFacet = sessionInformation.IsTimeshiftFacet
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

        #region TimeShift Saved Search

        public JsonResult SaveSearch(string p_Title, TimeShiftSearchTerm p_SearchTerm)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                Timeshift_SavedSearchModel timeshift_SavedSearchModel = new Timeshift_SavedSearchModel();
                timeshift_SavedSearchModel.Title = p_Title;
                timeshift_SavedSearchModel.SearchTerm = p_SearchTerm;
                timeshift_SavedSearchModel.CustomerGuid = sessionInformation.CustomerGUID;
                timeshift_SavedSearchModel.ClientGuid = sessionInformation.ClientGUID;
                IQTimeshift_SavedSearchLogic iQTimeshift_SavedSearchLogic = (IQTimeshift_SavedSearchLogic)LogicFactory.GetLogic(LogicType.Timeshift_SavedSearch);
                string result = iQTimeshift_SavedSearchLogic.InsertTimeshiftSavedSearch(timeshift_SavedSearchModel);
                string resultMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(result))
                {
                    resultMessage = ConfigSettings.Settings.ErrorOccurred; // "Some error occured, try again later";
                }
                else if (Convert.ToInt64(result) == -2)
                {
                    resultMessage = ConfigSettings.Settings.SearchWithSameNameExists; //"Search with same title already exists";
                }
                else if (Convert.ToInt64(result) == -3)
                {
                    resultMessage = ConfigSettings.Settings.SearchWithSameFilterExists;
                }
                else if (Convert.ToInt64(result) > 0)
                {
                    resultMessage = ConfigSettings.Settings.SearchSaved;// "Search saved successfully.";
                    timeShiftTempData = GetTempData();
                    timeShiftTempData.ActiveSearch = timeshift_SavedSearchModel;
                    SetTempData(timeShiftTempData);

                }
                return Json(new
                {
                    message = resultMessage,

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
                TempData.Keep("TimeShiftTempData");
            }
        }

        [HttpPost]
        public JsonResult UpdateSavedSearch(Int32 p_ID, string p_Title, TimeShiftSearchTerm p_SearchTerm)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                Timeshift_SavedSearchModel timeshift_SavedSearchModel = new Timeshift_SavedSearchModel();
                timeshift_SavedSearchModel.ID = p_ID;
                timeshift_SavedSearchModel.Title = p_Title;
                timeshift_SavedSearchModel.SearchTerm = p_SearchTerm;
                timeshift_SavedSearchModel.CustomerGuid = sessionInformation.CustomerGUID;
                timeshift_SavedSearchModel.ClientGuid = sessionInformation.ClientGUID;
                IQTimeshift_SavedSearchLogic iQTimeshift_SavedSearchLogic = (IQTimeshift_SavedSearchLogic)LogicFactory.GetLogic(LogicType.Timeshift_SavedSearch);
                string result = iQTimeshift_SavedSearchLogic.UpdateTimeshiftSavedSearch(timeshift_SavedSearchModel);
                string resultMessage = string.Empty;
                bool isError = false;
                bool isSuccess = false;
                if (string.IsNullOrWhiteSpace(result) || Convert.ToInt64(result) == 0)
                {
                    resultMessage = ConfigSettings.Settings.ErrorOccurred; // "Some error occured, try again later";
                    isError = true;
                }
                else if (Convert.ToInt64(result) == -2)
                {
                    resultMessage = ConfigSettings.Settings.SearchWithSameNameExists; //"Search with same title already exists";
                }
                else if (Convert.ToInt64(result) == -3)
                {
                    resultMessage = ConfigSettings.Settings.SearchWithSameFilterExists;
                }
                else if (Convert.ToInt64(result) > 0)
                {
                    resultMessage = ConfigSettings.Settings.SearchUpdated;// "Search saved successfully.";
                    isSuccess = true;
                    timeShiftTempData = GetTempData();
                    timeShiftTempData.ActiveSearch = timeshift_SavedSearchModel;
                    SetTempData(timeShiftTempData);

                }
                return Json(new
                {
                    message = resultMessage,
                    isSuccess = isSuccess,
                    isError = isError
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false,
                    isError = true
                });
            }
            finally
            {
                TempData.Keep("TimeShiftTempData");
            }
        }

        [HttpPost]
        public JsonResult GetSaveSearch(bool isNext, bool isInitialize)
        {
            try
            {
                Int32? currentPagenumber = 0;
                Int64 totalRecords = 0;
                timeShiftTempData = GetTempData();
                currentPagenumber = (Int32?)timeShiftTempData.CurrentSavedSearchPageNumner;

                if (isInitialize)
                {
                    currentPagenumber = 0;
                }
                else
                {
                    if (isNext)
                    {
                        currentPagenumber = currentPagenumber + 1;
                    }
                    else
                    {
                        currentPagenumber = currentPagenumber - 1;
                    }
                }

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                IQTimeshift_SavedSearchLogic iQTimeshift_SavedSearchLogic = (IQTimeshift_SavedSearchLogic)LogicFactory.GetLogic(LogicType.Timeshift_SavedSearch);
                List<Timeshift_SavedSearchModel> lstTimeshift_SavedSearchModel = iQTimeshift_SavedSearchLogic.SelectTimeshiftSavedSearch(currentPagenumber, Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftSavedSearchPageSize"]), sessionInformation.CustomerGUID, out totalRecords);

                Int64 startIndex = (Int64)((currentPagenumber * Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftSavedSearchPageSize"])) + 1);
                Int64 endIndex = startIndex + Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftSavedSearchPageSize"]) - 1;
                if (endIndex > totalRecords)
                {
                    endIndex = totalRecords;
                }
                string recordDetail = startIndex + " - " + endIndex + " of " + totalRecords;

                timeShiftTempData.CurrentSavedSearchPageNumner = currentPagenumber.Value;
                SetTempData(timeShiftTempData);

                bool HasMoreResult = HasMoreResults(totalRecords, Convert.ToInt64((currentPagenumber + 1) * Convert.ToInt64(ConfigurationManager.AppSettings["TimeshiftSavedSearchPageSize"])));
                return Json(new
                {
                    isSuccess = true,
                    HTML = RenderPartialToString(PATH_TimeshiftSavedSearchPartialView, lstTimeshift_SavedSearchModel),
                    HasMoreResult = HasMoreResult,
                    isPreviousAvailable = currentPagenumber > 0 ? true : false,
                    saveSearchRecordDetail = recordDetail
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
                TempData.Keep("TimeShiftTempData");
            }
        }

        [HttpPost]
        public JsonResult DeleteSavedSearchByID(Int64 p_ID)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                IQTimeshift_SavedSearchLogic iQTimeshift_SavedSearchLogic = (IQTimeshift_SavedSearchLogic)LogicFactory.GetLogic(LogicType.Timeshift_SavedSearch);
                string result = iQTimeshift_SavedSearchLogic.DeleteTimeshiftSavedSearchByID(p_ID, sessionInformation.CustomerGUID);

                string returnMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(result))
                {
                    returnMessage = ConfigSettings.Settings.ErrorOccurred;// "Some error occured, try again later";
                }
                else if (Convert.ToInt64(result) > 0)
                {
                    returnMessage = ConfigSettings.Settings.RecordDeleted;// "Record deleted successfully";
                }
                else if (Convert.ToInt64(result) <= 0)
                {
                    returnMessage = ConfigSettings.Settings.RecordNotDeleted; //"Record not deleted";
                }

                return Json(new
                {
                    message = returnMessage,
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
                TempData.Keep("TimeShiftTempData");
            }

        }

        [HttpPost]
        public JsonResult LoadSavedSearch(Int64 p_ID)
        {
            try
            {
                timeShiftTempData = GetTempData();
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                IQTimeshift_SavedSearchLogic iQTimeshift_SavedSearchLogic = (IQTimeshift_SavedSearchLogic)LogicFactory.GetLogic(LogicType.Timeshift_SavedSearch);
                List<Timeshift_SavedSearchModel> lstTimeshift_SavedSearchModel = iQTimeshift_SavedSearchLogic.SelectTimeshiftSavedSearchByID(p_ID, sessionInformation.CustomerGUID);


                Int32? currentPagenumber = 0;
                Int64 totalRecords = 0;
                currentPagenumber = timeShiftTempData.CurrentSavedSearchPageNumner;


                List<Timeshift_SavedSearchModel> lstTimeshift_SavedSearchModelList = iQTimeshift_SavedSearchLogic.SelectTimeshiftSavedSearch(currentPagenumber, Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftSavedSearchPageSize"]), sessionInformation.CustomerGUID, out totalRecords);

                Int64 startIndex = (Int64)((currentPagenumber * Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftSavedSearchPageSize"])) + 1);
                Int64 endIndex = startIndex + Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftSavedSearchPageSize"]) - 1;
                if (endIndex > totalRecords)
                {
                    endIndex = totalRecords;
                }
                string recordDetail = startIndex + " - " + endIndex + " of " + totalRecords;

                timeShiftTempData.CurrentSavedSearchPageNumner = currentPagenumber.Value;
                SetTempData(timeShiftTempData);

                bool HasMoreResult = HasMoreResults(totalRecords, Convert.ToInt64((currentPagenumber + 1) * Convert.ToInt64(ConfigurationManager.AppSettings["TimeshiftSavedSearchPageSize"])));

                if (lstTimeshift_SavedSearchModel.Count > 0)
                {
                    timeShiftTempData.ActiveSearch = lstTimeshift_SavedSearchModel[0];
                    return Json(new
                    {
                        timeshift_SavedSearch = lstTimeshift_SavedSearchModel[0],
                        HTML = RenderPartialToString(PATH_TimeshiftSavedSearchPartialView, lstTimeshift_SavedSearchModelList),
                        HasMoreResult = HasMoreResult,
                        isPreviousAvailable = currentPagenumber > 0 ? true : false,
                        saveSearchRecordDetail = recordDetail,
                        isSuccess = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        msg = ConfigSettings.Settings.NoResultFound
                    });
                }
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false,
                    msg = ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("TimeShiftTempData");
            }
        }

        [HttpPost]
        public JsonResult GetChart(string IQ_CC_KEY)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();

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

        private bool HasMoreResults(Int64 TotalRecords, Int64 shownRecords)
        {
            if (shownRecords < TotalRecords)
                return true;
            else
                return false;
        }

        #endregion

        #region Utility

        public TimeShiftTempData GetTempData()
        {
            timeShiftTempData = TempData["TimeShiftTempData"] != null ? (TimeShiftTempData)TempData["TimeShiftTempData"] : new TimeShiftTempData();
            return timeShiftTempData;
        }

        public void SetTempData(TimeShiftTempData p_TimeShiftTempData)
        {
            TempData["TimeShiftTempData"] = p_TimeShiftTempData;
            TempData.Keep("TimeShiftTempData");
        }

        #endregion

    }
}
