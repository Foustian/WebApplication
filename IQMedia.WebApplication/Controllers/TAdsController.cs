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
    public class TAdsController : Controller
    {
        #region Public Property
        ActiveUser sessionInformation = null;
        TAdsTempData tAdsTempData = null;
        string PATH_TAdsPartialView = "~/Views/TAds/_Result.cshtml";
        bool hasMoreResultPage = false;
        bool hasPreviousResultPage = false;
        string recordNumberDesc = string.Empty;
        string PATH_TadsRadioResults = "~/Views/Tads/_RadioResults.cshtml";
        string PATH_TadsSavedSearchPartialView = "~/Views/TAds/_SavedSearch.cshtml";
        #endregion

        public ActionResult Index()
        {
            try
            {
                // Clear out temp data used by other pages
                if (TempData.ContainsKey("AnalyticsTempData")) { TempData["AnalyticsTempData"] = null; }
                if (TempData.ContainsKey("FeedsTempData")) { TempData["FeedsTempData"] = null; }
                if (TempData.ContainsKey("DiscoveryTempData")) { TempData["DiscoveryTempData"] = null; }
                if (TempData.ContainsKey("TimeShiftTempData")) { TempData["TimeShiftTempData"] = null; }
                if (TempData.ContainsKey("SetupTempData")) { TempData["SetupTempData"] = null; }
                if (TempData.ContainsKey("GlobalAdminTempData")) { TempData["GlobalAdminTempData"] = null; } 

                SetTempData(null);
                tAdsTempData = GetTempData();
                tAdsTempData.SinceID = null;
                tAdsTempData.TotalResults = null;
                tAdsTempData.CurrentPage = null;
                tAdsTempData.p_IsAllClassAllowed = false;
                tAdsTempData.p_IsAllClassAllowed = false;
                tAdsTempData.p_IsAllStationAllowed = false;
                

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                var manualClipDuration = clientLogic.GetClientManualClipDurationSettings(sessionInformation.ClientGUID);
                Int16 rawMediaPauseSecs = clientLogic.GetClientRawMediaPauseSecs(sessionInformation.ClientGUID);
                IQClient_CustomSettingsModel customSettingsModel = clientLogic.GetClientCustomSettings(sessionInformation.ClientGUID.ToString());

                //temp data is a list of strings, not industries. so we only need to grab visible industries' IDs. Brands only need to be restricted if industries are restricted.
                if (customSettingsModel.visibleLRIndustries != null && customSettingsModel.visibleLRIndustries.Count>0)
                {
                    tAdsTempData.VisibleLRIndustries = new List<string>();
                    tAdsTempData.VisibleLRBrands = new List<string>();                    
                    foreach (IQ_Industry ind in customSettingsModel.visibleLRIndustries)
                    {
                        tAdsTempData.VisibleLRIndustries.Add(ind.ID);
                    }
                    tAdsTempData.VisibleLRBrands = customSettingsModel.visibleLRBrands;
                }
                

                SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                bool p_IsAllDmaAllowed;
                bool p_IsAllClassAllowed;
                bool p_IsAllStationAllowed;
                Dictionary<string, object> dicSSP = sspLogic.GetSSPDataByClientGUID(sessionInformation.ClientGUID, out p_IsAllDmaAllowed, out p_IsAllClassAllowed, out p_IsAllStationAllowed, tAdsTempData.IQTVRegion, true);

                //add dicssp info to temp data for mapping throughout controller
                tAdsTempData.SSPData = new Dictionary<string, object>();
                tAdsTempData.SSPData.Add("IQ_Class", dicSSP["IQ_Class"]);
                tAdsTempData.SSPData.Add("IQ_Country", dicSSP["IQ_Country"]);
                tAdsTempData.SSPData.Add("IQ_Region", dicSSP["IQ_Region"]);
                tAdsTempData.SSPData.Add("IQ_Station", dicSSP["IQ_Station"]);

                tAdsTempData.p_IsAllDmaAllowed = p_IsAllDmaAllowed;
                tAdsTempData.p_IsAllClassAllowed = p_IsAllClassAllowed;
                tAdsTempData.p_IsAllStationAllowed = p_IsAllStationAllowed;
                List<IQ_Region> IQTVRegionList = (List<IQ_Region>)dicSSP["IQ_Region"];
                List<IQ_Country> IQTVCountryList = (List<IQ_Country>)dicSSP["IQ_Country"];
                tAdsTempData.IQTVRegion = IQTVRegionList.Select(r => r.Num).ToList();
                tAdsTempData.IQTVCountry = IQTVCountryList.Select(c => c.Num).ToList();

                SetTempData(tAdsTempData);

                TVLogic tvlogic = new TVLogic();
                tAdsTempData.IQTVStations = (List<string>)dicSSP["stations"];

                //don't get any results before2016
                List<Dictionary<string, string>> logoHits;
                List<Dictionary<string, string>> adHits;

                Dictionary<string, object> defaultDictionary = TAdsDefaultResults("", "", DateTime.Now.AddMonths(-3), DateTime.Now, null, null, null, "", null, null, false, out logoHits, out adHits);
                string strHtml = (string)defaultDictionary["strHtml"];
                dicSSP.Add("DefaultHTML", strHtml);

                //country and region need their display name from the database mapped.
                TadsFilterModel filters = (TadsFilterModel)defaultDictionary["Filters"];
                filters = MapFilters(filters, tAdsTempData.SSPData);

                //add filters to return object
                dicSSP["IQ_Dma"] = filters.TadsDmas;
                dicSSP["IQ_Station"] = filters.TadsStations;
                dicSSP["IQ_Region"] = filters.TadsRegions;
                dicSSP["IQ_Country"] = filters.TadsCountries;
                dicSSP["Station_Affil"] = filters.TadsAffiliates;
                dicSSP["IQ_Class"] = filters.TadsClasses;
                dicSSP["IQ_Industry"] = filters.AllIndustries;
                dicSSP["IQ_Brand"] = filters.AllBrands;
                dicSSP["IQ_Logo"] = filters.AllLogos;
                dicSSP["IQ_PaidEarned"] = filters.TadsPaidEarned;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                dicSSP.Add("DefaultLogo", oSerializer.Serialize(logoHits));
                dicSSP.Add("DefaultAds", oSerializer.Serialize(adHits));

                ViewBag.IsSuccess = true;
                ViewBag.ManualClipDuration = manualClipDuration;
                ViewBag.RawMediaPauseSecs = rawMediaPauseSecs;
                return View(dicSSP);
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                ViewBag.IsSuccess = false;
            }
            finally
            {
                TempData.Keep("TAdsTempData");
            }
            return View();
        }
        public Dictionary<string, object> TAdsDefaultResults(string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc, out List<Dictionary<string, string>> logoHits, out List<Dictionary<string, string>> adHits)
        {
            Dictionary<string, object> defaultDictionary = new Dictionary<string, object>();
            logoHits = new List<Dictionary<string, string>>();
            adHits = new List<Dictionary<string, string>>();

            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                int ResultCount = 0;
                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TAdsPageSize"]);

                tAdsTempData = GetTempData();
                tAdsTempData.PageNumber = 0;

                List<IQAgent_TVFullResultsModel> tvResult = new List<IQAgent_TVFullResultsModel>();
                bool isallDmaAllowed = tAdsTempData.p_IsAllDmaAllowed;
                bool isallClassAllowed = tAdsTempData.p_IsAllClassAllowed;
                bool isallStationAllowed = tAdsTempData.p_IsAllStationAllowed;

                TVLogic tvlogic = new TVLogic();
                if (p_IQStationID == null || p_IQStationID.Count == 0) p_IQStationID = tvlogic.GetTAdsStations();             

                var dicTVResult = tvlogic.TAdsSearchResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, p_SearchTerm, p_Title, p_FromDate, p_ToDate, p_Dma, p_Station, p_IQStationID, p_Class, p_IsAsc, 0, isallDmaAllowed, isallClassAllowed, isallStationAllowed, string.Empty, ref ResultCount, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.MT.ToString(), p_FromDate, p_ToDate), tAdsTempData.IQTVRegion, tAdsTempData.IQTVCountry, p_Region, p_Country, null, null, tAdsTempData.VisibleLRIndustries, null, "", true);
                tvResult = (List<IQAgent_TVFullResultsModel>)dicTVResult["result"];

                TadsFilterModel filters = (TadsFilterModel)dicTVResult["filters"];

                tAdsTempData.ResultCount = ResultCount;
                hasMoreResultPage = false;
                hasPreviousResultPage = false;

                if (tvResult != null && ResultCount > 0)
                {
                    foreach (var result in tvResult)
                    {
                        logoHits.AddRange(result.Logos.Distinct().Select(x => new Dictionary<string, string> { { result.IQ_CC_Key, x } }).ToList());
                        adHits.AddRange(result.Ads.Distinct().Select(x => new Dictionary<string, string> { { result.IQ_CC_Key, x } }).ToList());
                    }

                    double totalHit = Convert.ToDouble(ResultCount) / PageSize;
                    if (totalHit > (tAdsTempData.PageNumber + 1))
                    {
                        hasMoreResultPage = true;

                        recordNumberDesc = (((tAdsTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ((tAdsTempData.PageNumber + 1) * PageSize).ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    else
                    {
                        hasMoreResultPage = false;
                        recordNumberDesc = (((tAdsTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ResultCount.ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                }
                if (tAdsTempData.PageNumber > 0)
                {
                    hasPreviousResultPage = true;
                }
                tAdsTempData.HasMoreResultPage = hasMoreResultPage;
                tAdsTempData.HasPreviousResultPage = hasPreviousResultPage;
                tAdsTempData.RecordNumber = recordNumberDesc;
                
                SetTempData(tAdsTempData);
                string htmlResult = RenderPartialToString(PATH_TAdsPartialView, tvResult);

                defaultDictionary.Add("Filters", filters);
                defaultDictionary.Add("strHtml", htmlResult);
                return defaultDictionary;

            }
            catch (IQMedia.Shared.Utility.CustomException ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex, ex.Message);
                Dictionary<string, object> errorDict = new Dictionary<string, object>();
                object blank = new object();               
                errorDict.Add(ConfigSettings.Settings.ErrorOccurred,blank);               
                return defaultDictionary;
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return new Dictionary<string, object>();
            }
            finally
            {
                TempData.Keep("TAdsTempData");
            }
        }
        
        [HttpPost]
        public JsonResult TAdsSearchResults(string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc, string p_SortColumn, bool p_IsDefaultLoad, List<string> p_SearchLogo, List<string> p_Brand, List<string> p_Industry, List<string> p_Company, string p_PaidEarned)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                int ResultCount = 0;
                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TAdsPageSize"]);

                tAdsTempData = GetTempData();
                tAdsTempData.PageNumber = 0;

                //check if industries is restricted
                if ((p_Industry == null || p_Industry.Count <= 0) && tAdsTempData.VisibleLRIndustries != null) 
                {
                    p_Industry = tAdsTempData.VisibleLRIndustries;
                }

                List<IQAgent_TVFullResultsModel> tvResult = new List<IQAgent_TVFullResultsModel>();
                bool isallDmaAllowed = tAdsTempData.p_IsAllDmaAllowed;
                bool isallClassAllowed = tAdsTempData.p_IsAllClassAllowed;
                bool isallStationAllowed = tAdsTempData.p_IsAllStationAllowed;

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
                TVLogic tvlogic = new TVLogic();
                var TAdsIQStations = tvlogic.GetTAdsStations();
                if (p_IQStationID == null || p_IQStationID.Count == 0)
                {
                    p_IQStationID = TAdsIQStations;
                }
                else
                {
                    p_IQStationID = p_IQStationID.Where(x => TAdsIQStations.Contains(x)).ToList();
                    if (p_IQStationID == null || p_IQStationID.Count == 0) p_IQStationID = new List<string>();
                }
                if (p_IQStationID != null && p_IQStationID.Count > 0)
                {
                    XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in p_IQStationID
                                             select new XElement("stationid", new XAttribute("id", ele))
                                                     ));
                    strStationIDXml = xdoc.ToString();
                }

                tAdsTempData.SelectedDma = p_Dma;
                tAdsTempData.SelectedStation = p_Station;

                var dicTVResult = tvlogic.TAdsSearchResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, p_SearchTerm, p_Title, p_FromDate, p_ToDate, p_Dma, p_Station, p_IQStationID, p_Class, p_IsAsc, 0, isallDmaAllowed, isallClassAllowed, isallStationAllowed, p_SortColumn, ref ResultCount, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.MT.ToString(), p_FromDate, p_ToDate), tAdsTempData.IQTVRegion, tAdsTempData.IQTVCountry, p_Region, p_Country, p_SearchLogo, p_Brand, p_Industry, p_Company, p_PaidEarned, true);
                tvResult = (List<IQAgent_TVFullResultsModel>)dicTVResult["result"];

                TadsFilterModel filter = (TadsFilterModel)dicTVResult["filters"];

                filter = MapFilters(filter, tAdsTempData.SSPData);

                tAdsTempData.ResultCount = ResultCount;
                hasMoreResultPage = false;
                hasPreviousResultPage = false;

                var logoHits = new List<Dictionary<string, string>>();
                var adHits = new List<Dictionary<string, string>>();
                if (tvResult != null && ResultCount > 0)
                {
                    var logos = new List<long>();
                    var brands = new List<long>();
                    var companies = new List<long>();
                    var industries = new List<long>();

                    foreach (var result in tvResult)
                    {
                        logoHits.AddRange(result.Logos.Distinct().Select(x => new Dictionary<string, string>{{result.IQ_CC_Key, x}}).ToList());
                        adHits.AddRange(result.Ads.Distinct().Select(x => new Dictionary<string, string>{{result.IQ_CC_Key, x}}).ToList());
                    }

                    double totalHit = Convert.ToDouble(ResultCount) / PageSize;
                   
                    if (totalHit > (tAdsTempData.PageNumber + 1))
                    {
                        hasMoreResultPage = true;
                        recordNumberDesc = (((tAdsTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ((tAdsTempData.PageNumber + 1) * PageSize).ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    else
                    {
                        hasMoreResultPage = false;
                        recordNumberDesc = (((tAdsTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ResultCount.ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                }
                if (tAdsTempData.PageNumber > 0)
                {
                    hasPreviousResultPage = true;
                }
                tAdsTempData.HasMoreResultPage = hasMoreResultPage;
                tAdsTempData.HasPreviousResultPage = hasPreviousResultPage;
                tAdsTempData.RecordNumber = recordNumberDesc;

                SetTempData(tAdsTempData);
                string htmlResult = RenderPartialToString(PATH_TAdsPartialView, tvResult);

                return Json(new
                {
                    HTML = htmlResult != null ? htmlResult : "",
                    hasMoreResult = hasMoreResultPage,
                    filters = filter,
                    hasPreviouResult = hasPreviousResultPage,
                    recordNumber = recordNumberDesc,
                    logoHits = logoHits,
                    adHits = adHits,
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
                TempData.Keep("TAdsTempData");
            }
        }

        public TadsFilterModel MapFilters(TadsFilterModel model, Dictionary<string, object> fullList)
        {
            TadsFilterModel tempModel = new TadsFilterModel();

            //get filters not in fullList
            TVLogic logic = new TVLogic();
            var narrowResultsList = logic.GetFilters();
            
            if(model.TadsCountries!= null && model.TadsCountries.Count>0){
                List<IQ_Country> allCountries = (List<IQ_Country>)fullList["IQ_Country"];
                tempModel.TadsCountries = new List<TadsCountry>();
                 foreach(TadsCountry cunt in model.TadsCountries){
                    IQ_Country match = allCountries.First(c => c.Num == Convert.ToInt32(cunt.ID));
                    cunt.Name = match.Name;
                    tempModel.TadsCountries.Add(cunt);
                 }
                 model.TadsCountries = tempModel.TadsCountries;
            }

            if(model.TadsRegions!= null && model.TadsRegions.Count>0)
            {
                List<IQ_Region> allRegions = (List<IQ_Region>)fullList["IQ_Region"];
                tempModel.TadsRegions = new List<TadsRegion>();
                foreach (TadsRegion reg in model.TadsRegions)
                {
                    IQ_Region match = allRegions.First(r => r.Num == Convert.ToInt32(reg.ID));
                    reg.Name = match.Name;
                    tempModel.TadsRegions.Add(reg);
                }
                model.TadsRegions = tempModel.TadsRegions;
            }

            if (model.TadsStations != null && model.TadsStations.Count > 0)
            {
                List<IQ_Station> allStations = (List<IQ_Station>)fullList["IQ_Station"];
                tempModel.TadsStations = new List<TadsStation>();
                foreach (TadsStation reg in model.TadsStations)
                {
                    IQ_Station match = allStations.First(r => r.IQ_Station_ID == reg.ID);
                    reg.Name = match.Station_Call_Sign; 
                    tempModel.TadsStations.Add(reg);
                }
                model.TadsStations= tempModel.TadsStations;
            }

            if (model.TadsClasses != null && model.TadsClasses.Count > 0)
            {
                List<IQ_Class> allClasses = (List<IQ_Class>)fullList["IQ_Class"];
                tempModel.TadsClasses = new List<TadsClass>();
                foreach(TadsClass cla in model.TadsClasses)
                {
                    IQ_Class match = allClasses.First(r => r.Name == cla.Name);
                    cla.ID = match.Num;
                    tempModel.TadsClasses.Add(cla);
                }
                model.TadsClasses = tempModel.TadsClasses;
            }

            if (model.AllIndustries!= null && model.AllIndustries.Count > 0)
            {
                List<IQ_Industry> fullIndustries = (List<IQ_Industry>)narrowResultsList["IQ_Industry"];
                tempModel.AllIndustries = new List<TadsIndustry>();

                
                
                    foreach (IQ_Industry ind in fullIndustries)
                    {
                        TadsIndustry match = model.AllIndustries.FirstOrDefault(i => i.ID == Convert.ToInt32(ind.ID));
                        if (match != null)
                        {
                            match.Name = ind.Name;
                            tempModel.AllIndustries.Add(match);
                        }
                    }
                    if (tAdsTempData.VisibleLRIndustries != null && tAdsTempData.VisibleLRIndustries.Count > 0)
                    {
                        List<TadsIndustry> industriesTempModel = new List<TadsIndustry>();
                        foreach (TadsIndustry hit in tempModel.AllIndustries)
                        {                          
                            if (tAdsTempData.VisibleLRIndustries.Contains(hit.ID.ToString()))
                            {
                                industriesTempModel.Add(hit);
                            }
                        }
                        tempModel.AllIndustries = industriesTempModel;
                    }
                model.AllIndustries = tempModel.AllIndustries;
            }

            if (model.AllBrands != null && model.AllBrands.Count > 0)
            {
                List<IQ_Brand> fullBrands = (List<IQ_Brand>)narrowResultsList["IQ_Brand"];
                tempModel.AllBrands = new List<TadsBrand>();
                foreach (IQ_Brand ind in fullBrands)
                {
                    TadsBrand match = model.AllBrands.FirstOrDefault(i => i.ID == Convert.ToInt32(ind.ID));
                    if (match != null)
                    {
                        match.Name = ind.Name;
                        tempModel.AllBrands.Add(match);
                    }
                }
                if (tAdsTempData.VisibleLRBrands != null && tAdsTempData.VisibleLRBrands.Count > 0)
                {
                    List<TadsBrand> brandsTempModel = new List<TadsBrand>();
                    foreach (TadsBrand hit in tempModel.AllBrands)
                    {
                        if (tAdsTempData.VisibleLRBrands.Contains(hit.ID.ToString()))
                        {
                            brandsTempModel.Add(hit);
                        }
                    }
                    tempModel.AllBrands = brandsTempModel;
                }
                model.AllBrands = tempModel.AllBrands;
            }

            if (model.AllLogos != null && model.AllLogos.Count > 0)
            {
                List<IQ_Logo> fullLogos = (List<IQ_Logo>)narrowResultsList["IQ_Logo"];
                tempModel.AllLogos = new List<TadsLogo>();
                foreach (IQ_Logo ind in fullLogos)
                {
                    TadsLogo match = model.AllLogos.FirstOrDefault(i => i.ID == Convert.ToInt32(ind.ID));
                    if (match != null)
                    {
                        match.Name = ind.Name;
                        match.BrandId = Convert.ToInt32(ind.BrandID);
                        match.URL = ind.URL;
                        tempModel.AllLogos.Add(match);
                    }
                }
                model.AllLogos = tempModel.AllLogos;
            }
            if (model.RadioStation != null && model.RadioStation.Count > 0) 
            {
                List<TadsStation> fullstation = (List<TadsStation>)fullList["Station"];
                tempModel.RadioStation = new List<TadsStation>();
                foreach (TadsStation stat in fullstation)
                {
                    TadsStation match = model.RadioStation.FirstOrDefault(s => s.ID == stat.ID);
                    if (match != null)
                    {
                        stat.Counts = match.Counts;
                        tempModel.RadioStation.Add(stat);
                    }
                }
                model.RadioStation = tempModel.RadioStation;
            }
            if (model.RadioMarket != null && model.RadioMarket.Count > 0)
            {
                List<TadsDma> fullmarket = (List<TadsDma>)fullList["Market"];
                tempModel.RadioMarket = new List<TadsDma>();
                foreach (TadsDma dma in fullmarket)
                {
                    TadsDma match = model.RadioMarket.FirstOrDefault(d => d.ID == dma.ID);
                    if (match != null)
                    {
                        dma.Counts = match.Counts;
                        tempModel.RadioMarket.Add(dma);
                    }
                }
                model.RadioMarket = tempModel.RadioMarket;
            }

            return model;
        }


        [HttpPost]
        public JsonResult GetFilters()
        {
            try
            {
                TVLogic logic = new TVLogic();
                var result = logic.GetFilters();
                tAdsTempData = GetTempData();

                List<IQ_Logo> logoList = new List<IQ_Logo>();
                List<IQ_Brand> brandList = new List<IQ_Brand>();
                List<IQ_Industry> industryList = new List<IQ_Industry>();
                List<IQ_Company> companyList = new List<IQ_Company>();

                if (result["IQ_Logo"] != null) 
                {
                    logoList = (List<IQ_Logo>)result["IQ_Logo"];
                    Session["FullSearchLogoList"] = (List<IQ_Logo>)result["IQ_Logo"];
                }
                if (result["IQ_Brand"] != null) 
                {
                    brandList = (List<IQ_Brand>)result["IQ_Brand"];
                    Session["FullBrandList"] = (List<IQ_Brand>)result["IQ_Brand"];
                }
                if (result["IQ_Industry"] != null) industryList = (List<IQ_Industry>)result["IQ_Industry"];
                if (result["IQ_Company"] != null) companyList = (List<IQ_Company>)result["IQ_Company"];               


                return Json(new
                {
                    logoList = logoList,
                    brandList = brandList,
                    companyList = companyList,
                    industryList = industryList,
                    visibleLRIndustries = tAdsTempData.VisibleLRIndustries,
                    visibleLRBrands = tAdsTempData.VisibleLRBrands,
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
                TempData.Keep("TAdsTempData");
            }
        }

        [HttpPost]
        public JsonResult TAdsSearchResultsPaging(string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc, bool p_IsNext, string p_SortColumn, List<string> p_SearchLogo, List<string> p_Brand, List<string> p_Industry, List<string> p_Company, string p_PaidEarned)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                tAdsTempData = GetTempData();

                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TAdsPageSize"]);
                hasMoreResultPage = false;
                hasPreviousResultPage = false;
                int ResultCount = 0;
                Int32 pagenumber = 0;

                if (p_IsNext)
                {
                    if (tAdsTempData.HasMoreResultPage)
                    {
                        if (tAdsTempData.PageNumber != null)
                        {
                            pagenumber = tAdsTempData.PageNumber + 1;
                            tAdsTempData.PageNumber = tAdsTempData.PageNumber + 1;
                        }
                        else
                        {
                            tAdsTempData.PageNumber = 1;
                            pagenumber = 1;
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
                else
                {
                    if (tAdsTempData.HasPreviousResultPage)
                    {
                        if (tAdsTempData.PageNumber != null)
                        {
                            pagenumber = tAdsTempData.PageNumber - 1;
                            tAdsTempData.PageNumber = tAdsTempData.PageNumber - 1;
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

                List<IQAgent_TVFullResultsModel> tvResult = new List<IQAgent_TVFullResultsModel>();
                bool isallDmaAllowed = tAdsTempData.p_IsAllDmaAllowed;
                bool isallClassAllowed = tAdsTempData.p_IsAllClassAllowed;
                bool isallStationAllowed = tAdsTempData.p_IsAllStationAllowed;

                TVLogic tvlogic = new TVLogic();
                var TAdsIQStations = tvlogic.GetTAdsStations();
                if (p_IQStationID == null || p_IQStationID.Count == 0) p_IQStationID = TAdsIQStations;
                else
                {
                    p_IQStationID = p_IQStationID.Where(x => TAdsIQStations.Contains(x)).ToList();
                    if (p_IQStationID == null || p_IQStationID.Count == 0) p_IQStationID = new List<string>();
                }

                // If not filtering by Industry, check if the client is restricted to a subset of industries
                if (p_Industry == null && tAdsTempData.VisibleLRIndustries != null)
                {
                    p_Industry = tAdsTempData.VisibleLRIndustries;
                }

                var dicTVResult = tvlogic.TAdsSearchResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, p_SearchTerm, p_Title, p_FromDate, p_ToDate, p_Dma, p_Station, p_IQStationID, p_Class, p_IsAsc, pagenumber, isallDmaAllowed, isallClassAllowed, isallStationAllowed, p_SortColumn, ref ResultCount, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.MT.ToString(), p_FromDate, p_ToDate), tAdsTempData.IQTVRegion, tAdsTempData.IQTVCountry, p_Region, p_Country, p_SearchLogo, p_Brand, p_Industry, p_Company, p_PaidEarned);
                tvResult = (List<IQAgent_TVFullResultsModel>)dicTVResult["result"];
                
                var logoHits = new List<Dictionary<string, string>>();
                var adHits = new List<Dictionary<string, string>>();
                if (tvResult != null && ResultCount > 0)
                {
                    foreach (var result in tvResult)
                    {
                        logoHits.AddRange(result.Logos.Distinct().Select(x => new Dictionary<string, string> { { result.IQ_CC_Key, x } }).ToList());
                        adHits.AddRange(result.Ads.Distinct().Select(x => new Dictionary<string, string> { { result.IQ_CC_Key, x } }).ToList());
                    }

                    double totalHit = Convert.ToDouble(ResultCount) / PageSize;
                    if (totalHit > (tAdsTempData.PageNumber + 1))
                    {
                        hasMoreResultPage = true;
                        recordNumberDesc = (((tAdsTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ((tAdsTempData.PageNumber + 1) * PageSize).ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    else
                    {
                        hasMoreResultPage = false;
                        recordNumberDesc = (((tAdsTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ResultCount.ToString("N0") + " of " + ResultCount.ToString("N0");
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
                tAdsTempData.HasMoreResultPage = hasMoreResultPage;
                tAdsTempData.HasPreviousResultPage = hasPreviousResultPage;
                SetTempData(tAdsTempData);

                string htmlResult = RenderPartialToString(PATH_TAdsPartialView, tvResult);

                return Json(new
                {
                    HTML = htmlResult != null ? htmlResult : "",
                    hasMoreResult = hasMoreResultPage,
                    hasPreviouResult = hasPreviousResultPage,
                    recordNumber = recordNumberDesc,
                    logoHits = logoHits,
                    adHits = adHits,
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
                TempData.Keep("TAdsTempData");
            }
        }

        [HttpPost]
        public JsonResult SelectRadioStationResults(DateTime? p_FromDate, DateTime? p_ToDate, string p_Market, bool p_IsAsc, bool p_IsNext, bool p_IsPrevNext, string p_Station, string p_SearchTerm)
        {
            sessionInformation = ActiveUserMgr.GetActiveUser();
            tAdsTempData = GetTempData();

            try
            {
               if(true)// if (sessionInformation.Isv4TadsRadioAccess)
                {
                    long totalResults = 0;
                    long sinceID = tAdsTempData.SinceID != null && p_IsPrevNext ? tAdsTempData.SinceID.Value : 0;
                    int CurrentPage = (p_IsPrevNext && tAdsTempData.CurrentPage != null) ? tAdsTempData.CurrentPage.Value : 0;
                    bool IsPreviousEnable = false, IsNextEnable = false;
                    int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TAdsPageSize"]);


                    if (p_IsPrevNext)
                    {
                        if (p_IsNext)
                        {
                            if (tAdsTempData.TotalResults > ((CurrentPage + 1) * PageSize))
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

                    Dictionary<string,object> searchRadioResult = radioLogic.SelectRadioResults(p_FromDate, p_ToDate, marketList, stationList, CommonFunctions.GeneratePMGUrl("QR", p_FromDate, p_ToDate), Convert.ToInt32(ConfigurationManager.AppSettings["IQRadioFragOffset"]), Convert.ToInt32(ConfigurationManager.AppSettings["IQRadioFragSize"]), true, !p_IsPrevNext, Convert.ToBoolean(ConfigurationManager.AppSettings["IQRadioIsLogging"]), ConfigurationManager.AppSettings["IQRadioLogFileLocation"], false, p_SearchTerm, ConfigurationManager.AppSettings["IQRadioSolrFL"], p_IsAsc, CurrentPage, PageSize, ref sinceID, out totalResults);
                    List<RadioModel> lstRadio = (List<RadioModel>)searchRadioResult["RadioList"];
                    TadsFilterModel Filters = (TadsFilterModel)searchRadioResult["Facets"];
                    Dictionary<string, object> databaseFilters = radioLogic.SelectRadioStationFilters();
                    Filters = MapFilters(Filters, databaseFilters);

                    string resultHTML = RenderPartialToString(PATH_TadsRadioResults, lstRadio);


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

                    tAdsTempData.TotalResults = totalResults;
                    tAdsTempData.SinceID = p_IsPrevNext ? tAdsTempData.SinceID : sinceID;
                    tAdsTempData.CurrentPage = CurrentPage;
                    SetTempData(tAdsTempData);

                    return Json(new
                    {
                        isSuccess = true,
                        HTML = resultHTML,
                        isPrevEnable = IsPreviousEnable,
                        isNextEnable = IsNextEnable,
                        startRecord = ((CurrentPage * PageSize) + 1).ToString("N0"),
                        endRecord = (((CurrentPage + 1) * PageSize) < totalResults ? ((CurrentPage + 1) * PageSize) : totalResults).ToString("N0"),
                        totalRecords = totalResults.ToString("N0"),
                        hasResults = (totalResults > 0),
                        FacetFilters = Filters
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
                TempData.Keep("TAdsTempData");
            }
        }
        #region SaveSearch

         [HttpPost]
        public JsonResult GetSaveSearch(bool isNext, bool isInitialize)
        {
            try
            {
                Int32? currentPagenumber = 0;
                Int64 totalRecords = 0;
                tAdsTempData = GetTempData();
                currentPagenumber = (Int32?)tAdsTempData.CurrentSavedSearchPageNumber;

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
                IQTads_SavedSearchLogic iQTads_SavedSearchLogic = (IQTads_SavedSearchLogic)LogicFactory.GetLogic(LogicType.Tads_SavedSearch);
                List<Tads_SavedSearchModel> lstTads_SavedSearchModel = iQTads_SavedSearchLogic.SelectTadsSavedSearch(currentPagenumber, Convert.ToInt32(ConfigurationManager.AppSettings["TAdsSavedSearchPageSize"]), sessionInformation.CustomerGUID, out totalRecords);

                Int64 startIndex = (Int64)((currentPagenumber * Convert.ToInt32(ConfigurationManager.AppSettings["TAdsSavedSearchPageSize"])) + 1);
                Int64 endIndex = startIndex + Convert.ToInt32(ConfigurationManager.AppSettings["TAdsSavedSearchPageSize"]) - 1;
                if (endIndex > totalRecords)
                {
                    endIndex = totalRecords;
                }
                string recordDetail = startIndex + " - " + endIndex + " of " + totalRecords;

                tAdsTempData.CurrentSavedSearchPageNumber = currentPagenumber.Value;
                SetTempData(tAdsTempData);

                bool HasMoreResult = HasMoreResults(totalRecords, Convert.ToInt64((currentPagenumber + 1) * Convert.ToInt64(ConfigurationManager.AppSettings["TAdsSavedSearchPageSize"])));
                return Json(new
                {
                    isSuccess = true,
                    HTML = RenderPartialToString(PATH_TadsSavedSearchPartialView, lstTads_SavedSearchModel),
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
                TempData.Keep("TAdsTempData");
            }
        }

         public JsonResult SaveSearch(string p_Title, TadsSearchTerm p_SearchTerm)
         {
             try
             {
                 sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                 Tads_SavedSearchModel tads_SavedSearchModel = new Tads_SavedSearchModel();
                 tads_SavedSearchModel.Title = p_Title;
                 tads_SavedSearchModel.SearchTerm = p_SearchTerm;
                 tads_SavedSearchModel.CustomerGuid = sessionInformation.CustomerGUID;
                 tads_SavedSearchModel.ClientGuid = sessionInformation.ClientGUID;
                 IQTads_SavedSearchLogic iQTads_SavedSearchLogic = (IQTads_SavedSearchLogic)LogicFactory.GetLogic(LogicType.Tads_SavedSearch);
                 string result = iQTads_SavedSearchLogic.InsertTadsSavedSearch(tads_SavedSearchModel);
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
                     tAdsTempData = GetTempData();
                     tAdsTempData.ActiveSearch = tads_SavedSearchModel;
                     SetTempData(tAdsTempData);

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
                 TempData.Keep("TAdsTempData");
             }
         }

         [HttpPost]
         public JsonResult DeleteSavedSearchByID(Int64 p_ID)
         {
             try
             {
                 sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                 IQTads_SavedSearchLogic iQTads_SavedSearchLogic = (IQTads_SavedSearchLogic)LogicFactory.GetLogic(LogicType.Tads_SavedSearch);
                 string result = iQTads_SavedSearchLogic.DeleteTadsSavedSearchByID(p_ID, sessionInformation.CustomerGUID);

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
                 TempData.Keep("TAdsTempData");
             }

         }

         [HttpPost]
         public JsonResult LoadSavedSearch(Int64 p_ID)
         {
             try
             {
                 tAdsTempData = GetTempData();
                 sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                 IQTads_SavedSearchLogic iQTads_SavedSearchLogic = (IQTads_SavedSearchLogic)LogicFactory.GetLogic(LogicType.Tads_SavedSearch);
                 List<Tads_SavedSearchModel> lstTads_SavedSearchModel = iQTads_SavedSearchLogic.SelectTadsSavedSearchByID(p_ID, sessionInformation.CustomerGUID);


                 Int32? currentPagenumber = 0;
                 Int64 totalRecords = 0;
                 currentPagenumber = tAdsTempData.CurrentSavedSearchPageNumber;


                 List<Tads_SavedSearchModel> lstTads_SavedSearchModelList = iQTads_SavedSearchLogic.SelectTadsSavedSearch(currentPagenumber, Convert.ToInt32(ConfigurationManager.AppSettings["TAdsSavedSearchPageSize"]), sessionInformation.CustomerGUID, out totalRecords);

                 Int64 startIndex = (Int64)((currentPagenumber * Convert.ToInt32(ConfigurationManager.AppSettings["TAdsSavedSearchPageSize"])) + 1);
                 Int64 endIndex = startIndex + Convert.ToInt32(ConfigurationManager.AppSettings["TAdsSavedSearchPageSize"]) - 1;
                 if (endIndex > totalRecords)
                 {
                     endIndex = totalRecords;
                 }
                 string recordDetail = startIndex + " - " + endIndex + " of " + totalRecords;

                 tAdsTempData.CurrentSavedSearchPageNumber = currentPagenumber.Value;
                 SetTempData(tAdsTempData);

                 bool HasMoreResult = HasMoreResults(totalRecords, Convert.ToInt64((currentPagenumber + 1) * Convert.ToInt64(ConfigurationManager.AppSettings["TAdsSavedSearchPageSize"])));

                 if (lstTads_SavedSearchModel.Count > 0)
                 {
                     tAdsTempData.ActiveSearch = lstTads_SavedSearchModel[0];
                     return Json(new
                     {
                         tads_SavedSearch = lstTads_SavedSearchModel[0],
                         HTML = RenderPartialToString(PATH_TadsSavedSearchPartialView, lstTads_SavedSearchModelList),
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
                 TempData.Keep("TAdsTempData");
             }
         }

         [HttpPost]
         public JsonResult UpdateSavedSearch(Int32 p_ID, string p_Title, TadsSearchTerm p_SearchTerm)
         {
             try
             {
                 sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                 Tads_SavedSearchModel tads_SavedSearchModel = new Tads_SavedSearchModel();
                 tads_SavedSearchModel.ID = p_ID;
                 tads_SavedSearchModel.Title = p_Title;
                 tads_SavedSearchModel.SearchTerm = p_SearchTerm;
                 tads_SavedSearchModel.CustomerGuid = sessionInformation.CustomerGUID;
                 tads_SavedSearchModel.ClientGuid = sessionInformation.ClientGUID;
                 IQTads_SavedSearchLogic iQTads_SavedSearchLogic = (IQTads_SavedSearchLogic)LogicFactory.GetLogic(LogicType.Tads_SavedSearch);
                 string result = iQTads_SavedSearchLogic.UpdateTadsSavedSearch(tads_SavedSearchModel);
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
                     tAdsTempData = GetTempData();
                     tAdsTempData.ActiveSearch = tads_SavedSearchModel;
                     SetTempData(tAdsTempData);

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
                 TempData.Keep("TAdsTempData");
             }
         }

        #endregion

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

        [HttpPost]
        public JsonResult GetRawData(string iqcckey)
        {
            try
            {
                TVLogic logic = new TVLogic();
                var result = logic.GetRawData(iqcckey);

                if (result != null && result.Count == 2)
                {
                    Session["xmlFileLoc"] = result.First();
                    Session["tgzFileLoc"] = result.Last();

                    return Json(new
                    {
                        isSuccess = true
                    });
                }

                return Json(new
                {
                    isSuccess = false
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
                TempData.Keep("TAdsTempData");
            }
        }

        [HttpGet]
        public ActionResult GetRawDataXML()
        {
            if (Session["xmlFileLoc"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["xmlFileLoc"])))
            {
                string fileLoc = Convert.ToString(Session["xmlFileLoc"]);

                if (System.IO.File.Exists(fileLoc))
                {
                    Session.Remove("xmlFileLoc");
                    return File(fileLoc, "text/xml", Path.GetFileName(fileLoc));
                }
            }

            return Content(ConfigSettings.Settings.FileNotAvailable);
        }

        [HttpGet]
        public ActionResult GetRawDataTGZ()
        {
            if (Session["tgzFileLoc"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["tgzFileLoc"])))
            {
                string fileLoc = Convert.ToString(Session["tgzFileLoc"]);

                if (System.IO.File.Exists(fileLoc))
                {
                    Session.Remove("tgzFileLoc");
                    return File(fileLoc, "application/x-compressed", Path.GetFileName(fileLoc));
                }
            }

            return Content(ConfigSettings.Settings.FileNotAvailable);
        }

        #region Utility
        public TAdsTempData GetTempData()
        {
            tAdsTempData = TempData["TAdsTempData"] != null ? (TAdsTempData)TempData["TAdsTempData"] : new TAdsTempData();
            return tAdsTempData;
        }
        public void SetTempData(TAdsTempData p_TAdsTempData)
        {
            TempData["TAdsTempData"] = p_TAdsTempData;
            TempData.Keep("TAdsTempData");
        }
        #endregion

    }
}
