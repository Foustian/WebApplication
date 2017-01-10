using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using PMGSearch;
using System.Configuration;
using IQMedia.Shared.Utility;
using System.Threading;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using IQMedia.Logic.Base;
using IQMedia.Data;
using IQMedia.Web.Logic.Base;
using System.Xml;
using System.Diagnostics;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using IQCommon.Model;


namespace IQMedia.Web.Logic
{
    public class DiscoveryLogic : IQMedia.Web.Logic.Base.ILogic
    {
        #region Chart
        public DiscoveryAdvanceSearch_DropDown GetSSPDataWithStationByClientGUID(string ClientGuid)
        {
            IQService_DiscoveryDA iQService_DiscoveryDA = (IQService_DiscoveryDA)DataAccessFactory.GetDataAccess(DataAccessType.IQService_Discovery);
            return iQService_DiscoveryDA.GetSSPDataWithStationByClientGUID(ClientGuid);
        }

        public DiscoverySearchResponse SearchTV(string searchTerm, string searchTermName, DateTime? fromDate, DateTime? toDate, int pageSize, bool IsAllDmaAllowed, List<IQ_Dma> listDma,bool IsAllClassAllowed, List<IQ_Class> listClass,bool IsAllStationAllowed, List<IQ_Station> listStation, List<Station_Affil> listAffiliate, List<IQ_Region> listRegion, List<IQ_Country> listCountry, out List<String> tvMarketList, string pmgSearchUrl, List<int> TVRegions, IQ_MediaTypeModel objMediaType, TVAdvanceSearchSettings TVSearchSettings)
        {
            try
            {
                string updatedSearchTerm = (TVSearchSettings == null || string.IsNullOrWhiteSpace(TVSearchSettings.SearchTerm)) ? searchTerm : TVSearchSettings.SearchTerm.Trim();

                Boolean isError = false;
                tvMarketList = new List<String>();
                DiscoverySearchResponse discoverySearchResponse = new DiscoverySearchResponse();
                discoverySearchResponse.SearchTerm = updatedSearchTerm;
                discoverySearchResponse.SearchTermParent = searchTerm;
                discoverySearchResponse.SearchName = searchTermName;
                discoverySearchResponse.MediumType = objMediaType.SubMediaType;

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchRequest searchRequest = new SearchRequest();
                searchRequest.IsTitleNContentSearch = true;
                searchRequest.IncludeRegionsNum = TVRegions;
                searchRequest.Facet = true;
                searchRequest.FacetRangeOther = "all";
//Criteria
                searchRequest.Terms = updatedSearchTerm;

                #region Search Title
                if (TVSearchSettings != null && !string.IsNullOrWhiteSpace(TVSearchSettings.ProgramTitle))
                {
                    searchRequest.Title120 = TVSearchSettings.ProgramTitle.Trim();
                }
                #endregion

                #region Search Appearing
                if (TVSearchSettings != null && !string.IsNullOrWhiteSpace(TVSearchSettings.Appearing))
                {
                    searchRequest.Appearing = TVSearchSettings.Appearing.Trim();
                }
                #endregion

                #region Search Class (Category) List
                bool isCategoryValid = IsAllClassAllowed;

                searchRequest.IQClassNum = new List<string>();
                if (TVSearchSettings != null && TVSearchSettings.CategoryList != null && TVSearchSettings.CategoryList.Count > 0)
                {
                    if (!IsAllClassAllowed)
                    {
                        List<string> lstclass = listClass.Join(TVSearchSettings.CategoryList, a => a.Name, b => b, (a, b) => b).ToList();
                        if (lstclass != null && lstclass.Count > 0)
                        {
                            searchRequest.IQClassNum = lstclass;
                            isCategoryValid = true;
                        }
                        else
                        {
                            isCategoryValid = false;
                        }
                    }
                    else
                    {
                        searchRequest.IQClassNum = TVSearchSettings.CategoryList;
                        isCategoryValid = true;
                    }
                }
                else if (!IsAllClassAllowed && listClass != null && listClass.Count > 0)
                {
                    searchRequest.IQClassNum = listClass.Select(s => s.Num).ToList();
                    isCategoryValid = true;
                }

                if (!isCategoryValid)
                {
                    throw new Exception();
                }
                #endregion

                #region Search DMA (Market) List
                Log4NetLogger.Debug("IsAllDmaAllowed : " + IsAllDmaAllowed);

                bool isDmaValid = IsAllDmaAllowed;

                searchRequest.IQDmaName = new List<string>();
                if (TVSearchSettings != null && TVSearchSettings.IQDmaList != null && TVSearchSettings.IQDmaList.Count > 0)
                {
                    if (!IsAllDmaAllowed)
                    {
                        List<string> lstdma = listDma.Join(TVSearchSettings.IQDmaList, a => a.Name, b => b, (a, b) => b).ToList();
                        if (lstdma != null && lstdma.Count > 0)
                        {
                            searchRequest.IQDmaName = lstdma;
                            isDmaValid = true;
                        }
                        else
                        {
                            isDmaValid = false;
                        }
                    }
                    else
                    {
                        searchRequest.IQDmaName = TVSearchSettings.IQDmaList;
                        isDmaValid = true;
                    }
                }
                else if (!IsAllDmaAllowed && listDma != null && listDma.Count > 0)
                {
                    searchRequest.IQDmaName = listDma.Select(s => s.Name).ToList();
                    isDmaValid = true;
                }

                if (!isDmaValid)
                {
                    throw new Exception();
                }
                #endregion

                #region Search Affiliate List
                bool isAffiliateValid = IsAllStationAllowed;

                searchRequest.StationAffil = new List<string>();

                if (TVSearchSettings != null && TVSearchSettings.AffiliateList != null && TVSearchSettings.AffiliateList.Count > 0)
                {
                    if (!IsAllStationAllowed)
                    {
                        List<string> lstaffil = listAffiliate.Join(TVSearchSettings.AffiliateList, a => a.Name, b => b, (a, b) => b).ToList();
                        if (lstaffil != null && lstaffil.Count > 0)
                        {
                            searchRequest.StationAffil = lstaffil;
                            isAffiliateValid = true;
                        }
                        else
                        {
                            isAffiliateValid = false;
                        }
                    }
                    else
                    {
                        searchRequest.StationAffil = TVSearchSettings.AffiliateList;
                        isAffiliateValid = true;
                    }
                }
                else if (!IsAllStationAllowed && listAffiliate != null && listAffiliate.Count > 0)
                {
                    searchRequest.StationAffil = listAffiliate.Select(s => s.Name).ToList();
                    isAffiliateValid = true;
                }

                if (!isAffiliateValid)
                {
                    throw new Exception();
                }
                #endregion

                #region Search Station List
                bool isStationValid = IsAllStationAllowed;

                searchRequest.Stations = new List<string>();

                if (TVSearchSettings != null && TVSearchSettings.StationList != null && TVSearchSettings.StationList.Count > 0)
                {
                    if (!IsAllStationAllowed)
                    {
                        List<string> lststation = listStation.Join(TVSearchSettings.StationList, a => a.IQ_Station_ID, b => b, (a, b) => b).ToList();
                        if (lststation != null && lststation.Count > 0)
                        {
                            searchRequest.Stations = lststation;
                            isStationValid = true;
                        }
                        else
                        {
                            isStationValid = false;
                        }
                    }
                    else
                    {
                        searchRequest.Stations = TVSearchSettings.StationList;
                        isStationValid = true;
                    }
                }
                else if (!IsAllStationAllowed && listStation != null && listStation.Count > 0)
                {
                    searchRequest.Stations = listStation.Select(s => s.IQ_Station_ID).ToList();
                    isStationValid = true;
                }

                if (!isStationValid)
                {
                    throw new Exception();
                }
                #endregion

                #region Search Region List
                var isRegionValid = false;
                if (TVSearchSettings != null && TVSearchSettings.RegionList != null && TVSearchSettings.RegionList.Count > 0)
                {
                    List<int> lstTVRegion = listRegion.Join(TVSearchSettings.RegionList, a => a.Num, b => Convert.ToInt32(b), (a, b) => a.Num).ToList();
                    if (lstTVRegion != null && lstTVRegion.Count > 0)
                    {
                        searchRequest.IncludeRegionsNum = lstTVRegion;
                        isRegionValid = true;
                    }
                    else
                    {
                        isRegionValid = false;
                    }
                }
                else if (listRegion != null && listRegion.Count > 0)
                {
                    searchRequest.IncludeRegionsNum = listRegion.Select(a => a.Num).ToList();
                    isRegionValid = true;
                }

                if (!isRegionValid)
                {
                    throw new Exception();
                }
                #endregion

                #region Search Country List
                var isCountryValid = false;
                if (TVSearchSettings != null && TVSearchSettings.CountryList != null && TVSearchSettings.CountryList.Count > 0)
                {
                    List<int> lstTVCountry = listCountry.Join(TVSearchSettings.CountryList, a => a.Num, b => Convert.ToInt32(b), (a, b) => a.Num).ToList();
                    if (lstTVCountry != null && lstTVCountry.Count > 0)
                    {
                        searchRequest.CountryNums = lstTVCountry;
                        isCountryValid = true;
                    }
                    else
                    {
                        isCountryValid = false;
                    }
                }
                else if (listCountry != null && listCountry.Count > 0)
                {
                    searchRequest.CountryNums = listCountry.Select(a => a.Num).ToList();
                    isCountryValid = true;
                }

                if (!isCountryValid)
                {
                    throw new Exception();
                }
                #endregion
//End Criteria

                searchRequest.PageSize = pageSize;
                searchRequest.SortFields = "date-";

                if (fromDate == null || toDate == null)
                {
                    searchRequest.FacetRangeStarts = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchRequest.FacetRangeEnds = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchRequest.FacetRangeStarts = fromDate;
                    searchRequest.FacetRangeEnds = toDate;
                }

                searchRequest.StartDate = searchRequest.FacetRangeStarts;
                searchRequest.EndDate = searchRequest.FacetRangeEnds;
                TimeSpan dateDiff = (TimeSpan)(searchRequest.FacetRangeEnds - searchRequest.FacetRangeStarts);
                if (dateDiff.Days <= 1)
                {
                    searchRequest.FacetRangeGap = RangeGap.HOUR;
                }
                else
                {
                    searchRequest.FacetRangeGap = RangeGap.DAY;
                }
                searchRequest.FacetRangeGapDuration = 1;
                searchRequest.FacetRange = "gmtdatetime_dt";
                searchRequest.FacetField = "market";
                searchRequest.AffilForFacet = new Dictionary<Dictionary<string, string>, List<string>>();
                searchRequest.wt = ReponseType.json;

                SearchResult searchResult = searchEngine.SearchTVChart(searchRequest, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));
                jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(searchResult.ResponseXml);
                string totalResult = Convert.ToString(jsonData["facet_counts"]["facet_ranges"]["gmtdatetime_dt"]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                string[] facetData = totalResult.Split(',');

                discoverySearchResponse.ListRecordData = new List<RecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {
                    RecordData recorddata = new RecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);
                }

                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<TopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    TopResults topResults = new TopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["title120"]).Replace("[", "").Replace("]", "");
                    topResults.Logo = objMediaType.MediaIconPath;
                    topResults.Publisher = "<img src=\"" + ConfigurationManager.AppSettings["StationLogo"] + Convert.ToString(jsonData["response"]["docs"][i]["stationid"]) + ".jpg" + "\" alt=\"\" />";

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }

                string totalResultmarket = Convert.ToString(jsonData["facet_counts"]["facet_fields"]["market"]).Replace("\r\n", string.Empty).Replace("[", "").Replace("]", "");
                string[] facetDatamarket = Regex.Split(totalResultmarket, "\"(.*?)\"");

                List<String> tvMarketListtemp = new List<String>();

                for (int i = 1; i < facetDatamarket.Length; i = i + 2)
                {
                    if (Convert.ToInt64(facetDatamarket[i + 1].Trim().Replace(",", string.Empty)) > 0)
                    {
                        tvMarketListtemp.Add(facetDatamarket[i].Trim().Replace("\"", string.Empty).Replace("[", string.Empty).Trim());
                    }
                }

                tvMarketListtemp = tvMarketListtemp.Distinct().ToList();

                if (tvMarketListtemp.Contains("National"))
                {
                    tvMarketList.Add("National");
                }
                var allMarketData = (from item in tvMarketListtemp
                                     where item != "National"
                                     orderby item
                                     select item).ToList();
                tvMarketList.AddRange(allMarketData);
                discoverySearchResponse.TotalResult = Convert.ToInt64(jsonData["response"]["numFound"]);

                return discoverySearchResponse;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("Error in TV : " + ex.ToString());
                throw;
            }
        }

        public DiscoverySearchResponse SearchNews(string searchTerm, string searchTermName, DateTime? fromDate, DateTime? toDate, int pageSize, string p_fromRecordID, string pmgSearchUrl, List<Int16> lstOfIQLicense, IQ_MediaTypeModel objMediaType, NewsAdvanceSearchSettings NewsSearchSettings)
        {
            try
            {
                string updatedSearchTerm = (NewsSearchSettings == null || string.IsNullOrWhiteSpace(NewsSearchSettings.SearchTerm)) ? searchTerm : NewsSearchSettings.SearchTerm.Trim();

                Boolean isError = false;
                DiscoverySearchResponse discoverySearchResponse = new DiscoverySearchResponse();
                discoverySearchResponse.SearchTerm = updatedSearchTerm;
                discoverySearchResponse.SearchTermParent = searchTerm;
                discoverySearchResponse.SearchName = searchTermName;
                discoverySearchResponse.MediumType = objMediaType.SubMediaType;

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchNewsRequest searchNewsRequest = new SearchNewsRequest();
                searchNewsRequest.IsTitleNContentSearch = true;
                searchNewsRequest.Facet = true;
                searchNewsRequest.FacetRangeOther = "all";
                foreach (Int16 iqlicense in lstOfIQLicense)
                {
                    searchNewsRequest.IQLicense.Add(iqlicense);
                }
                searchNewsRequest.PageSize = pageSize;
                searchNewsRequest.SortFields = "date-";
                if (!string.IsNullOrWhiteSpace(p_fromRecordID))
                {
                    searchNewsRequest.FromRecordID = Convert.ToString(p_fromRecordID);
                }
                if (fromDate == null || toDate == null)
                {
                    searchNewsRequest.FacetRangeStarts = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchNewsRequest.FacetRangeEnds = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchNewsRequest.FacetRangeStarts = fromDate;
                    searchNewsRequest.FacetRangeEnds = toDate;
                }
//Criteria
                searchNewsRequest.SearchTerm = updatedSearchTerm;
                searchNewsRequest.SourceType = new List<int>() { (int)PMGSearch.SourceType.OnlineNews };

                if (NewsSearchSettings != null)
                {
                    searchNewsRequest.Publications = NewsSearchSettings.PublicationList;
                    searchNewsRequest.NewsCategory = NewsSearchSettings.CategoryList;
                    searchNewsRequest.PublicationCategory = NewsSearchSettings.PublicationCategoryList;
                    searchNewsRequest.Genre = NewsSearchSettings.GenreList;
                    searchNewsRequest.Market = NewsSearchSettings.MarketList;
                    searchNewsRequest.NewsRegion = NewsSearchSettings.RegionList;
                    searchNewsRequest.Country = NewsSearchSettings.CountryList;
                    searchNewsRequest.Language = NewsSearchSettings.LanguageList;
                    searchNewsRequest.ExcludeDomains = NewsSearchSettings.ExcludeDomainList;
                }
//End Criteria

                searchNewsRequest.StartDate = searchNewsRequest.FacetRangeStarts;
                searchNewsRequest.EndDate = searchNewsRequest.FacetRangeEnds;

                TimeSpan dateDiff = (TimeSpan)(searchNewsRequest.FacetRangeEnds - searchNewsRequest.FacetRangeStarts);
                if (dateDiff.Days <= 1)
                {
                    searchNewsRequest.FacetRangeGap = RangeGap.HOUR;
                }
                else
                {
                    searchNewsRequest.FacetRangeGap = RangeGap.DAY;
                }

                searchNewsRequest.FacetRangeGapDuration = 1;
                searchNewsRequest.FacetRange = "harvestdate_dt";
                searchNewsRequest.wt = ReponseType.json;

                string newsReponse = searchEngine.SearchNewsChart(searchNewsRequest, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));
                jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(newsReponse);
                
                string totalResult = Convert.ToString(jsonData["facet_counts"]["facet_ranges"]["harvestdate_dt"]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                string[] facetData = totalResult.Split(',');

                string fromRecordID = string.Empty;
                try
                {
                    fromRecordID = Convert.ToString(jsonData["response"]["docs"][0]["iqseqid"]);
                }
                catch (Exception)
                { }
                
                if (!string.IsNullOrWhiteSpace(fromRecordID))
                {
                    discoverySearchResponse.FromRecordID = fromRecordID;
                }

                discoverySearchResponse.ListRecordData = new List<RecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {
                    RecordData recorddata = new RecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);
                }

                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<TopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    TopResults topResults = new TopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["title"]);
                    topResults.Logo = objMediaType.MediaIconPath;
                    topResults.Publisher = Convert.ToString(jsonData["response"]["docs"][i]["homeurl_domain"]).Replace("\n", "");

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }

                discoverySearchResponse.TotalResult = Convert.ToInt64(jsonData["response"]["numFound"]);
                return discoverySearchResponse;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("Error in News : " + ex.ToString());
                throw;
            }
        }

        public DiscoverySearchResponse SearchLexisNexis(string searchTerm, string searchTermName, DateTime? fromDate, DateTime? toDate, int pageSize, string p_fromRecordID, string pmgSearchUrl, List<Int16> lstOfIQLicense, IQ_MediaTypeModel objMediaType, LexisNexisAdvanceSearchSettings lexisNexisSearchSettings)
        {
            try
            {
                string updatedSearchTerm = (lexisNexisSearchSettings == null || string.IsNullOrWhiteSpace(lexisNexisSearchSettings.SearchTerm)) ? searchTerm : lexisNexisSearchSettings.SearchTerm.Trim();

                Boolean isError = false;
                DiscoverySearchResponse discoverySearchResponse = new DiscoverySearchResponse();
                discoverySearchResponse.SearchTerm = updatedSearchTerm;
                discoverySearchResponse.SearchTermParent = searchTerm;
                discoverySearchResponse.SearchName = searchTermName;
                discoverySearchResponse.MediumType = objMediaType.SubMediaType;

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchNewsRequest searchNewsRequest = new SearchNewsRequest();
                searchNewsRequest.IsTitleNContentSearch = true;
                searchNewsRequest.Facet = true;
                searchNewsRequest.FacetRangeOther = "all";
                foreach (Int16 iqlicense in lstOfIQLicense)
                {
                    searchNewsRequest.IQLicense.Add(iqlicense);
                }
                searchNewsRequest.PageSize = pageSize;
                searchNewsRequest.SortFields = "date-";
                if (!string.IsNullOrWhiteSpace(p_fromRecordID))
                {
                    searchNewsRequest.FromRecordID = Convert.ToString(p_fromRecordID);
                }
                if (fromDate == null || toDate == null)
                {
                    searchNewsRequest.FacetRangeStarts = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchNewsRequest.FacetRangeEnds = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchNewsRequest.FacetRangeStarts = fromDate;
                    searchNewsRequest.FacetRangeEnds = toDate;
                }
                //Criteria
                searchNewsRequest.SearchTerm = updatedSearchTerm;
                searchNewsRequest.SourceType = new List<int>() { (int)PMGSearch.SourceType.Print };

                if (lexisNexisSearchSettings != null)
                {
                    searchNewsRequest.Publications = lexisNexisSearchSettings.PublicationList;
                    searchNewsRequest.NewsCategory = lexisNexisSearchSettings.CategoryList;
                    searchNewsRequest.PublicationCategory = lexisNexisSearchSettings.PublicationCategoryList;
                    searchNewsRequest.Genre = lexisNexisSearchSettings.GenreList;
                    searchNewsRequest.NewsRegion = lexisNexisSearchSettings.RegionList;
                    searchNewsRequest.Country = lexisNexisSearchSettings.CountryList;
                    searchNewsRequest.Language = lexisNexisSearchSettings.LanguageList;
                    searchNewsRequest.ExcludeDomains = lexisNexisSearchSettings.ExcludeDomainList;
                }
                //End Criteria

                searchNewsRequest.StartDate = searchNewsRequest.FacetRangeStarts;
                searchNewsRequest.EndDate = searchNewsRequest.FacetRangeEnds;

                TimeSpan dateDiff = (TimeSpan)(searchNewsRequest.FacetRangeEnds - searchNewsRequest.FacetRangeStarts);
                if (dateDiff.Days <= 1)
                {
                    searchNewsRequest.FacetRangeGap = RangeGap.HOUR;
                }
                else
                {
                    searchNewsRequest.FacetRangeGap = RangeGap.DAY;
                }

                searchNewsRequest.FacetRangeGapDuration = 1;
                searchNewsRequest.FacetRange = "harvestdate_dt";
                searchNewsRequest.wt = ReponseType.json;

                string newsReponse = searchEngine.SearchNewsChart(searchNewsRequest, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));
                jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(newsReponse);

                string totalResult = Convert.ToString(jsonData["facet_counts"]["facet_ranges"]["harvestdate_dt"]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                string[] facetData = totalResult.Split(',');

                string fromRecordID = string.Empty;
                try
                {
                    fromRecordID = Convert.ToString(jsonData["response"]["docs"][0]["iqseqid"]);
                }
                catch (Exception)
                { }

                if (!string.IsNullOrWhiteSpace(fromRecordID))
                {
                    discoverySearchResponse.FromRecordID = fromRecordID;
                }

                discoverySearchResponse.ListRecordData = new List<RecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {
                    RecordData recorddata = new RecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);
                }

                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<TopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    TopResults topResults = new TopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["title"]);
                    topResults.Logo = objMediaType.MediaIconPath;
                    topResults.Publisher = Convert.ToString(jsonData["response"]["docs"][i]["homeurl_domain"]).Replace("\n", "");

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }

                discoverySearchResponse.TotalResult = Convert.ToInt64(jsonData["response"]["numFound"]);
                return discoverySearchResponse;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("Error in LexisNexis : " + ex.ToString());
                throw;
            }
        }

        public DiscoverySearchResponse SearchBlog(string searchTerm, string searchTermName, DateTime? fromDate, DateTime? toDate, int pageSize, string p_fromRecordID, string pmgSearchUrl, IQ_MediaTypeModel objMediaType, BlogAdvanceSearchSettings blogSearchSettings)
        {
            try
            {
                string updatedSearchTerm = (blogSearchSettings == null || string.IsNullOrWhiteSpace(blogSearchSettings.SearchTerm)) ? searchTerm : blogSearchSettings.SearchTerm.Trim(); ;

                Boolean isError = false;
                DiscoverySearchResponse discoverySearchResponse = new DiscoverySearchResponse();
                discoverySearchResponse.SearchTerm = updatedSearchTerm;
                discoverySearchResponse.SearchTermParent = searchTerm;
                discoverySearchResponse.SearchName = searchTermName;
                discoverySearchResponse.MediumType = objMediaType.SubMediaType;

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                
                SearchSMRequest searchSMRequest = new SearchSMRequest();
                searchSMRequest.IsTitleNContentSearch = true;
                searchSMRequest.Facet = true;
                searchSMRequest.FacetRangeOther = "all";
                searchSMRequest.PageSize = pageSize;
                searchSMRequest.SortFields = "date-";
                if (!string.IsNullOrWhiteSpace(p_fromRecordID))
                {
                    searchSMRequest.FromRecordID = Convert.ToString(p_fromRecordID);
                }
                if (fromDate == null || toDate == null)
                {
                    searchSMRequest.FacetRangeStarts = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchSMRequest.FacetRangeEnds = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchSMRequest.FacetRangeStarts = fromDate;
                    searchSMRequest.FacetRangeEnds = toDate;
                }
                searchSMRequest.StartDate = searchSMRequest.FacetRangeStarts;
                searchSMRequest.EndDate = searchSMRequest.FacetRangeEnds;

//Criteria
                searchSMRequest.SearchTerm = updatedSearchTerm;

                if (blogSearchSettings != null)
                {
                    searchSMRequest.SocialMediaSources = blogSearchSettings.SourceList;
                    searchSMRequest.Author = blogSearchSettings.Author;
                    searchSMRequest.Title = blogSearchSettings.Title;
                    searchSMRequest.ExcludeDomains = blogSearchSettings.ExcludeDomainList;
                }
//End Criteria

                TimeSpan dateDiff = (TimeSpan)(searchSMRequest.FacetRangeEnds - searchSMRequest.FacetRangeStarts);
                if (dateDiff.Days <= 1)
                {
                    searchSMRequest.FacetRangeGap = RangeGap.HOUR;
                }
                else
                {
                    searchSMRequest.FacetRangeGap = RangeGap.DAY;
                }

                searchSMRequest.FacetRangeGapDuration = 1;
                searchSMRequest.FacetRange = "harvestdate_dt";
                searchSMRequest.StartDate = searchSMRequest.FacetRangeStarts;
                searchSMRequest.EndDate = searchSMRequest.FacetRangeEnds;
                searchSMRequest.IsTaggingExcluded = true;
                searchSMRequest.SourceType = new List<string>() { CommonFunctions.GetEnumDescription(PMGSearch.SourceType.Blog) };
                searchSMRequest.wt = ReponseType.json;

                string smReponse = searchEngine.SearchSocialMediaChart(searchSMRequest, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));
                jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(smReponse);

                string totalResult = Convert.ToString(jsonData["facet_counts"]["facet_ranges"]["harvestdate_dt"]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                string[] facetData = totalResult.Split(',');

                discoverySearchResponse.ListRecordData = new List<RecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {
                    RecordData recorddata = new RecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);
                }
                
                string fromRecordID = string.Empty;
                try
                {
                    fromRecordID = Convert.ToString(jsonData["response"]["docs"][0]["iqseqid"]);
                }
                catch (Exception)
                { }

                if (!string.IsNullOrWhiteSpace(fromRecordID))
                {
                    discoverySearchResponse.FromRecordID = fromRecordID;
                }

                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<TopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    TopResults topResults = new TopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["title"]);
                    topResults.Logo = objMediaType.MediaIconPath;
                    topResults.Publisher = Convert.ToString(jsonData["response"]["docs"][i]["homeurl_domain"]);

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }

                discoverySearchResponse.TotalResult = Convert.ToInt64(jsonData["response"]["numFound"]);
                return discoverySearchResponse;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("Blog error occured ", ex);
                throw;
            }
        }

        public DiscoverySearchResponse SearchForum(string searchTerm, string searchTermName, DateTime? fromDate, DateTime? toDate, int pageSize, string p_fromRecordID, string pmgSearchUrl, IQ_MediaTypeModel objMediaType, ForumAdvanceSearchSettings forumSearchSettings)
        {
            try
            {
                string updatedSearchTerm = (forumSearchSettings == null || string.IsNullOrWhiteSpace(forumSearchSettings.SearchTerm)) ? searchTerm : forumSearchSettings.SearchTerm.Trim(); ;

                Boolean isError = false;
                DiscoverySearchResponse discoverySearchResponse = new DiscoverySearchResponse();
                discoverySearchResponse.SearchTerm = updatedSearchTerm;
                discoverySearchResponse.SearchTermParent = searchTerm;
                discoverySearchResponse.SearchName = searchTermName;
                discoverySearchResponse.MediumType = objMediaType.SubMediaType;

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchSMRequest searchSMRequest = new SearchSMRequest();
                searchSMRequest.IsTitleNContentSearch = true;
                searchSMRequest.Facet = true;
                searchSMRequest.FacetRangeOther = "all";
                searchSMRequest.PageSize = pageSize;
                searchSMRequest.SortFields = "date-";
                if (!string.IsNullOrWhiteSpace(p_fromRecordID))
                {
                    searchSMRequest.FromRecordID = Convert.ToString(p_fromRecordID);
                }
                if (fromDate == null || toDate == null)
                {
                    searchSMRequest.FacetRangeStarts = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchSMRequest.FacetRangeEnds = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchSMRequest.FacetRangeStarts = fromDate;
                    searchSMRequest.FacetRangeEnds = toDate;
                }
                searchSMRequest.StartDate = searchSMRequest.FacetRangeStarts;
                searchSMRequest.EndDate = searchSMRequest.FacetRangeEnds;

                //Criteria
                searchSMRequest.SearchTerm = updatedSearchTerm;

                if (forumSearchSettings != null)
                {
                    searchSMRequest.SocialMediaSources = forumSearchSettings.SourceList;
                    searchSMRequest.Author = forumSearchSettings.Author;
                    searchSMRequest.Title = forumSearchSettings.Title;
                    searchSMRequest.SourceType = forumSearchSettings.SourceTypeList;
                    searchSMRequest.ExcludeDomains = forumSearchSettings.ExcludeDomainList;
                }
                //End Criteria

                TimeSpan dateDiff = (TimeSpan)(searchSMRequest.FacetRangeEnds - searchSMRequest.FacetRangeStarts);
                if (dateDiff.Days <= 1)
                {
                    searchSMRequest.FacetRangeGap = RangeGap.HOUR;
                }
                else
                {
                    searchSMRequest.FacetRangeGap = RangeGap.DAY;
                }

                searchSMRequest.FacetRangeGapDuration = 1;
                searchSMRequest.FacetRange = "harvestdate_dt";
                searchSMRequest.StartDate = searchSMRequest.FacetRangeStarts;
                searchSMRequest.EndDate = searchSMRequest.FacetRangeEnds;
                searchSMRequest.IsTaggingExcluded = true;
                if (searchSMRequest.SourceType == null || searchSMRequest.SourceType.Count == 0)
                {
                    searchSMRequest.SourceType = new List<string>() { CommonFunctions.GetEnumDescription(PMGSearch.SourceType.Forum) };
                }
                searchSMRequest.wt = ReponseType.json;

                string smReponse = searchEngine.SearchSocialMediaChart(searchSMRequest, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));
                jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(smReponse);

                string totalResult = Convert.ToString(jsonData["facet_counts"]["facet_ranges"]["harvestdate_dt"]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                string[] facetData = totalResult.Split(',');

                discoverySearchResponse.ListRecordData = new List<RecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {
                    RecordData recorddata = new RecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);
                }

                string fromRecordID = string.Empty;
                try
                {
                    fromRecordID = Convert.ToString(jsonData["response"]["docs"][0]["iqseqid"]);
                }
                catch (Exception)
                { }

                if (!string.IsNullOrWhiteSpace(fromRecordID))
                {
                    discoverySearchResponse.FromRecordID = fromRecordID;
                }

                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<TopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    TopResults topResults = new TopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["title"]);
                    topResults.Logo = objMediaType.MediaIconPath;
                    topResults.Publisher = Convert.ToString(jsonData["response"]["docs"][i]["homeurl_domain"]);

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }

                discoverySearchResponse.TotalResult = Convert.ToInt64(jsonData["response"]["numFound"]);
                return discoverySearchResponse;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("Forum error occured ", ex);
                throw;
            }
        }
 
        public DiscoverySearchResponse SearchProQuest(string searchTerm, string searchTermName, DateTime? fromDate, DateTime? toDate, int pageSize, string p_fromRecordID, string pmgSearchUrl, IQ_MediaTypeModel objMediaType, ProQuestAdvanceSearchSettings ProQuestSearchSettings)
        {
            try
            {
                string updatedSearchTerm = (ProQuestSearchSettings == null || string.IsNullOrWhiteSpace(ProQuestSearchSettings.SearchTerm)) ? searchTerm : ProQuestSearchSettings.SearchTerm.Trim();

                Boolean isError = false;
                List<DiscoverySearchResponse> lstDiscoverySearchResponse = new List<DiscoverySearchResponse>();

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();

                DiscoverySearchResponse discoverySearchResponse = new DiscoverySearchResponse();
                discoverySearchResponse.SearchTerm = updatedSearchTerm;
                discoverySearchResponse.SearchTermParent = searchTerm;
                discoverySearchResponse.SearchName = searchTermName;
                discoverySearchResponse.MediumType = objMediaType.SubMediaType;
                
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchProQuestRequest searchProQuestRequest = new SearchProQuestRequest();
                searchProQuestRequest.Facet = true;
                searchProQuestRequest.FacetRangeOther = "all";
                searchProQuestRequest.PageSize = pageSize;
                
                if (!string.IsNullOrWhiteSpace(p_fromRecordID))
                {
                    searchProQuestRequest.FromRecordID = Convert.ToString(p_fromRecordID);
                }

                if (fromDate == null || toDate == null)
                {
                    searchProQuestRequest.FacetRangeStarts = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchProQuestRequest.FacetRangeEnds = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchProQuestRequest.FacetRangeStarts = fromDate;
                    searchProQuestRequest.FacetRangeEnds = toDate;
                }

                searchProQuestRequest.StartDate = searchProQuestRequest.FacetRangeStarts;
                searchProQuestRequest.EndDate = searchProQuestRequest.FacetRangeEnds;
//Criteria
                searchProQuestRequest.SearchTerm = updatedSearchTerm;

                if (ProQuestSearchSettings != null)
                {
                    searchProQuestRequest.Publications = ProQuestSearchSettings.PublicationList;
                    searchProQuestRequest.Authors = ProQuestSearchSettings.AuthorList;
                    searchProQuestRequest.Languages = ProQuestSearchSettings.LanguageList;
                }
//End Criteria
                TimeSpan dateDiff = (TimeSpan)(searchProQuestRequest.FacetRangeEnds - searchProQuestRequest.FacetRangeStarts);
                if (dateDiff.Days <= 1)
                {
                    searchProQuestRequest.FacetRangeGap = RangeGap.HOUR;
                }
                else
                {
                    searchProQuestRequest.FacetRangeGap = RangeGap.DAY;
                }

                searchProQuestRequest.FacetRangeGapDuration = 1;
                searchProQuestRequest.FacetRange = "mediadatedt";

                searchProQuestRequest.wt = ReponseType.json;

                SearchProQuestResult searchProQuestResult = searchEngine.SearchProQuest(searchProQuestRequest, true, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));
                jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(searchProQuestResult.ResponseXml);

                string totalResult = Convert.ToString(jsonData["facet_counts"]["facet_ranges"]["mediadatedt"]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                string[] facetData = totalResult.Split(',');

                discoverySearchResponse.ListRecordData = new List<RecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {
                    RecordData recorddata = new RecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);
                }

                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<TopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    TopResults topResults = new TopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["title"]);
                    topResults.Logo = objMediaType.MediaIconPath;
                    topResults.Publisher = Convert.ToString(jsonData["response"]["docs"][i]["publication"]);

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }

                discoverySearchResponse.TotalResult = Convert.ToInt64(jsonData["response"]["numFound"]);

                return discoverySearchResponse;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("ProQuest error occured ", ex);
                throw;
            }
        }

        public string ColumnChart(List<DiscoverySearchResponse> lstDiscoverySearchResponse)
        {
            try
            {
                /*var dataValue = (List<data>)lstDiscoverySearchResponse.GroupBy(g => g.SearchTerm)
                                                                    .Select(group => new data()
                                                                    {
                                                                        label = group.FirstOrDefault().SearchTerm,
                                                                        value = Convert.ToString(group.Sum(s => Convert.ToDecimal(s.ListRecordData.FirstOrDefault().TotalRecord)))

                                                                    }).ToList();*/
                var dataValue = (List<data>)(from x in
                                                 (
                                                     from sr in lstDiscoverySearchResponse
                                                     from dc in sr.ListRecordData
                                                     select new { sr.SearchTerm, dc }
                                                 )
                                             group x by new { x.SearchTerm } into g
                                             select new data()
                                             {
                                                 label = g.Key.SearchTerm,
                                                 value = Convert.ToString(g.Sum(g1 => Convert.ToInt64(g1.dc.TotalRecord)))
                                             }).ToList();
                ColumnChartOutput columnChartOutput = new ColumnChartOutput();

                columnChartOutput.chartdata = new chart();
                columnChartOutput.chartdata.yaxisname = "";
                columnChartOutput.chartdata.caption = "Share of Coverage";
                columnChartOutput.chartdata.bgcolor = "FFFFFF";
                columnChartOutput.chartdata.alternatehgridcolor = "FFFFFF";
                columnChartOutput.chartdata.divLineThickness = "0";
                columnChartOutput.chartdata.showBorder = "0";
                columnChartOutput.chartdata.canvasBorderAlpha = "0";
                columnChartOutput.chartdata.showYAxisValues = "0";
                columnChartOutput.chartdata.showLegend = "1";
                columnChartOutput.chartdata.legendPosition = "BOTTOM";
                columnChartOutput.chartdata.rotateValues = "1";
                columnChartOutput.lstdata = dataValue;


                string jsonResult = CommonFunctions.SearializeJson(columnChartOutput);
                /*XDocument xdoc = new XDocument(new XElement("chart", new XAttribute("yAxisName", ""), new XAttribute("caption", "Average Report"),
                                                  searchTermTotal.Select(s => new XElement("Set", new XAttribute("label", s.SearchTerm), new XAttribute("value", s.TotalRecord)))));*/
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public string LineChart(List<DiscoverySearchResponse> lstDiscoverySearchResponse, Boolean isHourData, decimal p_ClientGmtOffset, decimal p_ClientDstOffset)
        {
            try
            {

                var totalRecords = (List<SearchTermTotalRecords>)(from x in
                                                                      (
                                                                          from sr in lstDiscoverySearchResponse
                                                                          from dc in sr.ListRecordData
                                                                          select new { sr.SearchTerm, dc }
                                                                      )
                                                                  group x by new { x.SearchTerm, x.dc.Date } into g
                                                                  select new SearchTermTotalRecords
                                                                  {
                                                                      SearchTerm = g.Key.SearchTerm,
                                                                      RecordDate = Convert.ToDateTime(g.Key.Date),
                                                                      TotalRecords = g.Sum(g1 => Convert.ToInt64(g1.dc.TotalRecord))
                                                                  }).ToList();
                Chart chart = new Chart();
                chart.subcaption = "";
                chart.caption = "Trend Over Time";
                chart.linethickness = "1";
                chart.showvalues = "0";
                chart.formatnumberscale = "0";
                chart.anchorRadius = "3";
                chart.divlinealpha = "FFFFFF";
                chart.divlinecolor = "FFFFFF";
                chart.divlineisdashed = "1";
                chart.showalternatehgridcolor = "1";
                chart.alternatehgridcolor = "FFFFFF";
                chart.shadowalpha = "40";
                chart.labelstep = "1";
                chart.numvdivlines = "5";
                chart.chartrightmargin = "10";
                chart.bgcolor = "FFFFFF";
                chart.bgangle = "270";
                chart.bgalpha = "10,10";
                chart.alternatehgridalpha = "5";
                chart.legendposition = "BOTTOM";
                chart.drawAnchors = "1";
                chart.showBorder = "0";
                chart.canvasBorderAlpha = "0";



                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();


                var distinctDate = totalRecords.Select(d => d.RecordDate).Distinct().ToList();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();

                foreach (DateTime rDate in distinctDate)
                {

                    Category2 category2 = new Category2();
                    if (isHourData)
                    {
                        if (rDate.IsDaylightSavingTime())
                        {

                            category2.label = rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString("MM/dd/yyyy hh:mm tt");
                        }
                        else
                        {
                            category2.label = rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString("MM/dd/yyyy hh:mm tt");
                        }
                    }
                    else
                    {
                        category2.label = rDate.ToShortDateString();
                    }

                    allCategory.category.Add(category2);

                }

                lstallCategory.Add(allCategory);

                var distinctSearchTerm = totalRecords.Select(d => d.SearchTerm).Distinct().ToList();

                List<SeriesData> lstSeriesData = new List<SeriesData>();
                foreach (string sTerm in distinctSearchTerm)
                {
                    SeriesData seriesData = new SeriesData();
                    seriesData.data = new List<Datum>();

                    seriesData.seriesname = sTerm;
                    seriesData.color = "";
                    /*seriesData.anchorBorderColor = "";
                    seriesData.anchorBgColor = "";*/

                    var sTermWiseRecord = totalRecords.Where(w => w.SearchTerm.Equals(sTerm)).Select(s => s).ToList();
                    foreach (SearchTermTotalRecords searchTermTotalRecord in sTermWiseRecord)
                    {
                        Datum datum = new Datum();
                        datum.value = searchTermTotalRecord.TotalRecords.ToString();
                        datum.link = "javascript:SearchByChartDate('" + searchTermTotalRecord.RecordDate.ToShortDateString() + "',\"" + sTerm.Trim().Replace("\"", "\\\"") + "\");";
                        seriesData.data.Add(datum);
                    }
                    lstSeriesData.Add(seriesData);
                }


                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public string HighChartsColumnChart(List<DiscoverySearchResponse> lstDiscoverySearchResponse)
        {
            try
            {
                // generate list of series from discovery response, as search term and total no. of record exist for that search term
                var dataValue = (List<Series>)(from x in
                                                   (
                                                       from sr in lstDiscoverySearchResponse
                                                       from dc in sr.ListRecordData
                                                       select new { sr.SearchTerm, sr.SearchName, dc }
                                                   )
                                               group x by new { x.SearchTerm, x.SearchName } into g
                                               select new Series()
                                               {
                                                   name = g.Key.SearchName,
                                                   data = GetSeriesList(g.Sum(g1 => Convert.ToInt64(g1.dc.TotalRecord)))
                                               }).ToList();

                // column chart for all search terms
                HighColumnChartModel highColumnChartModel = new HighColumnChartModel();

                // set chart type , width and height
                highColumnChartModel.chart = new HChart() { height = 300, width = 150, type = "column" };

                // set chart title , its x position and style
                highColumnChartModel.title = new Title()
                {
                    text = "Level of Coverage",
                    x = 12,
                    style = new HStyle
                    {
                        color = "#555555",
                        fontWeight = "bold",
                        fontFamily = "Verdana",
                        fontSize = "13px"
                    }
                };

                // hide legend for column chart,  by setting enabled = false
                highColumnChartModel.legend = new Legend() { enabled = false };
                highColumnChartModel.subtitle = new Subtitle() { text = "" };

                // set x-axis and label as enabled  = false
                highColumnChartModel.xAxis = new XAxis() { categories = new List<string>(), labels = new labels() { enabled = false } };
                highColumnChartModel.yAxis = new YAxis() { title = new Title2() { text = "" }, min = 0 };

                // set list of series for column chart
                highColumnChartModel.series = dataValue;

                // set plot option with borderwidth = 0 
                highColumnChartModel.plotOptions = new PlotOptions() { column = new Column() { borderWidth = 0, pointPadding = 0.2 } };
                highColumnChartModel.tooltip = new Tooltip()
                {
                    pointFormat = "<div class=\"trimtext\" style=\"width:130px;\">{series.name}</div><b>{point.y}</b>",
                    useHTML = true
                };

                string jsonResult = CommonFunctions.SearializeJson(highColumnChartModel);
                return jsonResult;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string HighChartsLineChart(List<DiscoverySearchResponse> lstDiscoverySearchResponse, Boolean isHourData, decimal p_ClientGmtOffset, decimal p_ClientDstOffset)
        {
            try
            {
                var totalRecords = (List<SearchTermTotalRecords>)(from x in
                                                                      (
                                                                          from sr in lstDiscoverySearchResponse
                                                                          from dc in sr.ListRecordData
                                                                          select new { sr.SearchTerm, sr.SearchName, dc }
                                                                      )
                                                                  group x by new { x.SearchTerm, x.SearchName, x.dc.Date } into g
                                                                  select new SearchTermTotalRecords
                                                                  {
                                                                      SearchName = g.Key.SearchName,
                                                                      SearchTerm = g.Key.SearchTerm,
                                                                      RecordDate = Convert.ToDateTime(g.Key.Date),
                                                                      TotalRecords = g.Sum(g1 => Convert.ToInt64(g1.dc.TotalRecord))
                                                                  }).ToList();



                // multi line chart, one for each search term. 
                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();

                // set chart title and title style
                highLineChartOutput.title = new Title()
                {
                    text = "Trend over Time",
                    x = -20,
                    style = new HStyle
                    {
                        color = "#555555",
                        fontWeight = "bold",
                        fontFamily = "Verdana",
                        fontSize = "13px"
                    }
                };
                highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };


                List<PlotLine> plotlines = new List<PlotLine>();

                PlotLine plotLine = new PlotLine();
                plotLine.color = "#808080";
                plotLine.value = "0";
                plotLine.width = "1";
                plotlines.Add(plotLine);


                highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { title = new Title2(), plotLines = plotlines } };



                List<string> categories = new List<string>();

                var distinctDate = totalRecords.Select(d => d.RecordDate).Distinct().ToList();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();

                foreach (DateTime rDate in distinctDate)
                {
                    if (isHourData)
                    {
                        if (rDate.IsDaylightSavingTime())
                        {

                            categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString("MM/dd/yyyy hh:mm tt"));
                        }
                        else
                        {
                            categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString("MM/dd/yyyy hh:mm tt"));
                        }
                    }
                    else
                    {
                        categories.Add(rDate.ToShortDateString());
                    }

                }

                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 7 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories,
                    labels = new labels() { staggerLines = isHourData ? 2 : 0 }
                };

                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { borderWidth = "0" };

                // set chart with height and width
                highLineChartOutput.hChart = new HChart() { height = 370, width = 750, type = "spline", marginRight = 25 };

                // set plot options and click event for series points (which will again assigned in JS as this is string value)
                // legendItemClick event to show / hide column chart series on line legend click
                highLineChartOutput.plotOption = new PlotOptions()
                {
                    column = null,
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = true
                        }
                    },
                    series = new PlotSeries()
                    {
                        cursor = "pointer",
                        events = new PlotEvents()
                        {
                            legendItemClick = "ShowHideColumnChart"
                        },
                        point = new PlotPoint()
                        {
                            events = new PlotEvents()
                            {
                                click = "ChartClick"
                            }
                        }
                    }
                };


                var distinctSearchTerm = totalRecords.Select(d => new Tuple<string, string>(d.SearchTerm.Trim(), d.SearchName == null ? "" : d.SearchName.Trim())).Distinct().ToList();

                // start to set series of data for multiline search term chart 
                List<Series> lstSeries = new List<Series>();

                foreach (var sTuple in distinctSearchTerm)
                {

                    // set series name as search term , will shown in legend and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = sTuple.Item2.Length > 10 ? sTuple.Item2.Substring(0, 10) + "..." : sTuple.Item2;


                    var sTermWiseRecord = totalRecords.Where(w => w.SearchTerm.Equals(sTuple.Item1)).Select(s => s).ToList();
                    foreach (SearchTermTotalRecords searchTermTotalRecord in sTermWiseRecord)
                    {
                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at particular date 
                            *  SearchTerm = applied search term value
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = searchTermTotalRecord.TotalRecords;
                        highChartDatum.SearchTerm = sTuple.Item1;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);

                }

                // assign set of series data to search term multiline (or multi line search-request chart)
                highLineChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<PieChartResponse> HighChartsLineChartByMedium(List<DiscoverySearchResponse> lstDiscoverySearchResponse, string[] searchTerms, List<string> mediums, Boolean isHourData, decimal clientGmtOffset, decimal clientDstOffset, List<IQ_MediaTypeModel> lstMasterMediaTypes)
        {
            try
            {
                List<PieChartResponse> lstChartResponse = new List<PieChartResponse>();

                var totalRecords = (List<SearchTermTotalRecords>)(from x in
                                                                      (
                                                                          from sr in lstDiscoverySearchResponse
                                                                          from dc in sr.ListRecordData
                                                                          select new { sr.SearchTerm, sr.MediumType, dc }
                                                                      )
                                                                  group x by new { x.SearchTerm, x.MediumType, x.dc.Date } into g
                                                                  select new SearchTermTotalRecords
                                                                  {
                                                                      SearchTerm = g.Key.SearchTerm,
                                                                      FeedClass = g.Key.MediumType,
                                                                      RecordDate = Convert.ToDateTime(g.Key.Date),
                                                                      TotalRecords = g.Sum(g1 => Convert.ToInt64(g1.dc.TotalRecord))
                                                                  }).ToList();

                // multi line chart, one for each search term. 
                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();

                // set chart title and title style
                highLineChartOutput.title = new Title()
                {
                    text = "Trend over Time (Medium)",
                    x = -20,
                    style = new HStyle
                    {
                        color = "#555555",
                        fontWeight = "bold",
                        fontFamily = "Verdana",
                        fontSize = "13px"
                    }
                };
                highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                List<PlotLine> plotlines = new List<PlotLine>();
                PlotLine plotLine = new PlotLine();
                plotLine.color = "#808080";
                plotLine.value = "0";
                plotLine.width = "1";
                plotlines.Add(plotLine);

                List<string> categories = new List<string>();
                var distinctDate = totalRecords.Select(d => d.RecordDate).Distinct().ToList();
                foreach (DateTime rDate in distinctDate)
                {
                    if (isHourData)
                    {
                        if (rDate.IsDaylightSavingTime())
                        {
                            categories.Add(rDate.AddHours((Convert.ToDouble(clientGmtOffset)) + Convert.ToDouble(clientDstOffset)).ToString("MM/dd/yyyy hh:mm tt"));
                        }
                        else
                        {
                            categories.Add(rDate.AddHours((Convert.ToDouble(clientGmtOffset))).ToString("MM/dd/yyyy hh:mm tt"));
                        }
                    }
                    else
                    {
                        categories.Add(rDate.ToShortDateString());
                    }
                }

                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 7 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories,
                    labels = new labels() { staggerLines = isHourData ? 2 : 0 }
                };

                highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { title = new Title2(), plotLines = plotlines } };

                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { borderWidth = "0" };

                // set chart with height and width
                highLineChartOutput.hChart = new HChart() { height = 325, width = 900, type = "spline" };

                // set plot options and click event for series points (which will again assigned in JS as this is string value)
                // legendItemClick event to show / hide column chart series on line legend click
                highLineChartOutput.plotOption = new PlotOptions()
                {
                    column = null,
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = true
                        }
                    },
                    series = new PlotSeries()
                    {
                        cursor = "pointer",
                        events = new PlotEvents()
                        {
                            legendItemClick = "ShowHideColumnChart"
                        },
                        point = new PlotPoint()
                        {
                            events = new PlotEvents()
                            {
                                click = "ChartClick"
                            }
                        }
                    }
                };

                foreach (string sTerm in searchTerms)
                {
                    List<Series> lstSeries = new List<Series>();
                    Series series = null;

                    foreach (IQ_MediaTypeModel objMediaType in lstMasterMediaTypes.OrderBy(o => o.SortOrder))
                    {
                        if (objMediaType.HasAccess && (mediums == null || mediums.Count == 0 || mediums.Contains(objMediaType.SubMediaType)))
                        {
                            series = new Series();
                            series.name = objMediaType.DisplayName;
                            series.data = totalRecords.Where(w => w.FeedClass.Equals(objMediaType.SubMediaType) && w.SearchTerm.Equals(sTerm))
                                                                        .Select(s => new HighChartDatum()
                                                                        {
                                                                            y = s.TotalRecords,
                                                                            SearchTerm = sTerm,
                                                                            Type = objMediaType.SubMediaType
                                                                        }).ToList();
                            lstSeries.Add(series);
                        }
                    }

                    highLineChartOutput.series = lstSeries;

                    PieChartResponse chartResponse = new PieChartResponse();
                    chartResponse.JsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
                    chartResponse.SearchTerm = sTerm;
                    lstChartResponse.Add(chartResponse);
                }

                return lstChartResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<string, object> HighChartsPieChartBySearchTerm(List<DiscoverySearchResponse> lstDiscoverySearchResponse, string[] searchTerm, string[] searchName, List<string> mediums, List<IQ_MediaTypeModel> lstMasterMediaTypes)
        {
            try
            {
                HighPieChartModel highPieChartModel = new HighPieChartModel();

                // set chart width and height
                highPieChartModel.chart = new PChart() { height = 225, width = 300 };

                // set chart title and style
                highPieChartModel.title = new PTitle()
                {
                    text = "Share of Voice", // Overridden in JS. Needs to be set here for correct sizing.
                    style = new HStyle
                    {
                        color = "#555555",
                        fontFamily = "Verdana",
                        fontSize = "13px",
                        fontWeight = "bold"
                    }
                };

                // set chart tooltip format  using pointformat property
                highPieChartModel.tooltip = new PTooltip() { pointFormat = "{series.name}: <b>{point.percentage:.1f}%</b>" };

                // set pie chart plotoptions with legend and enable datalabel = false
                highPieChartModel.plotOptions = new PPlotOptions() { pie = new Pie() { allowPointSelect = true, cursor = "pointer", showInLegend = false, size = "95%", innerSize = "60%", dataLabels = new DataLabels() { enabled = false } } };
                highPieChartModel.series = new List<PSeries>();

                // set legend width and layout
                highPieChartModel.legend = new Legend()
                {
                    align = "center",
                    borderWidth = "0",
                    enabled = true,
                    layout = "horizontal",
                    width = 380,
                    verticalAlign = "bottom"
                };

                PSeries pSeries = new PSeries();
                pSeries.type = "pie";
                pSeries.name = "";
                pSeries.data = new List<object>();
                List<PSeries> lstPseries = new List<PSeries>();
                lstPseries.Add(pSeries);

                Dictionary<string, long> dictCounts = new Dictionary<string, long>();
                for (int i = 0; i < searchTerm.Length; i++ )
                {
                    var sTerm = searchTerm[i].ToString();
                    var sName = searchName[i].ToString();

                    dictCounts.Add(sName, 0);

                    foreach (IQ_MediaTypeModel objMediaType in lstMasterMediaTypes)
                    {
                        if (objMediaType.HasAccess && (mediums == null || mediums.Count == 0 || mediums.Contains(objMediaType.SubMediaType)))
                        {
                            dictCounts[sName] += lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(objMediaType.SubMediaType) && w.SearchTerm.Equals(sTerm)).GroupBy(g => g.SearchTerm)
                                                                                    .Select(s =>
                                                                                        Convert.ToInt64(s.Sum(s1 => Convert.ToInt64(s1.TotalResult)))).FirstOrDefault();
                        }
                    }
                }

                List<PieChartTotal> lstPieChartTotals = new List<PieChartTotal>();
                foreach(DiscoverySearchResponse searchResponse in lstDiscoverySearchResponse)
                {
                    foreach(string sDate in searchResponse.ListRecordData.Select(x => x.Date).Distinct())
                    {
                        lstPieChartTotals.Add(new PieChartTotal() { date = (DateTime.Parse(sDate)).ToString("MM/dd/yy H:mm:ss"), searchTerm = searchResponse.SearchTerm, searchName = searchResponse.SearchName, medium = searchResponse.MediumType, totalResult = searchResponse.ListRecordData.Where(w => w.Date == sDate).Sum(s => Int64.Parse(s.TotalRecord)) });
                    }
                }

                pSeries.data = dictCounts.Select(s => new object[] { s.Key, s.Value }).ToList<Object>();
                // set series for selected search term
                highPieChartModel.series = lstPseries;

                Dictionary<string, object> dictResults = new Dictionary<string, object>();
                dictResults["JsonResult"] = CommonFunctions.SearializeJson(highPieChartModel);
                dictResults["TotalRecords"] = lstPieChartTotals;

                return dictResults;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PieChartResponse> HighChartsPieChartByMedium(List<DiscoverySearchResponse> lstDiscoverySearchResponse, string[] searchTerm, string[] searchName, List<string> mediums, List<IQ_MediaTypeModel> lstMasterMediaTypes)
        {
            try
            {
                List<PieChartResponse> lstPieChartResponse = new List<PieChartResponse>();

                // pie chart used for each search term, to get no. of records share for each medium type.
                HighPieChartModel highPieChartModel = new HighPieChartModel();

                // set chart width and height
                highPieChartModel.chart = new PChart() { height = 300, width = 400 };

                // set chart title and style
                highPieChartModel.title = new PTitle()
                {
                    text = "Sources",
                    style = new HStyle
                    {
                        color = "#555555",
                        fontFamily = "Verdana",
                        fontSize = "13px",
                        fontWeight = "bold"
                    }
                };

                // set chart tooltip format  using pointformat property
                highPieChartModel.tooltip = new PTooltip() { pointFormat = "{series.name}: <b>{point.percentage:.1f}%</b>" };

                // set pie chart plotoptions with legend and enable datalabel = false
                highPieChartModel.plotOptions = new PPlotOptions() { pie = new Pie() { allowPointSelect = true, cursor = "pointer", showInLegend = true, innerSize = "60%", dataLabels = new DataLabels() { enabled = false } } };
                highPieChartModel.series = new List<PSeries>();

                // set legend width and layout
                highPieChartModel.legend = new Legend()
                {
                    align = "center",
                    borderWidth = "0",
                    enabled = true,
                    layout = "horizontal",
                    width = 380,
                    verticalAlign = "bottom"
                };

                PSeries pSeries = new PSeries();
                pSeries.type = "pie";
                pSeries.name = "";
                pSeries.data = new List<object>();
                List<Object> lstObject = new List<object>();
                List<PSeries> lstPseries = new List<PSeries>();
                lstPseries.Add(pSeries);

                // set pie chart series for each search term 
                for (int i = 0; i < searchTerm.Length; i++ )
                {
                    string sTerm = searchTerm[i];
                    string sName = searchName[i];
                    // create series with each medium type and its total records. 
                    lstObject = new List<object>();
                    pSeries.data = new List<object>();

                    foreach (IQ_MediaTypeModel objMediaType in lstMasterMediaTypes.OrderBy(o => o.SortOrder))
                    {
                        if (objMediaType.HasAccess && (mediums == null || mediums.Count == 0 || mediums.Contains(objMediaType.SubMediaType)))
                        {
                            lstObject.Add(new object[] { objMediaType.DisplayName, lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(objMediaType.SubMediaType) && w.SearchTerm.Equals(sTerm)).GroupBy(g => g.MediumType)
                                                                                .Select(s =>
                                                                                    Convert.ToInt64(s.Sum(s1 => Convert.ToInt64(s1.TotalResult)))).FirstOrDefault()});
                        }
                    }

                    pSeries.data = lstObject;
                    // set series for selected search term
                    highPieChartModel.series = lstPseries;
                    string jsonResult = CommonFunctions.SearializeJson(highPieChartModel);

                    // add piechart to list of piechart response
                    PieChartResponse pieChartResponse = new PieChartResponse();
                    pieChartResponse.JsonResult = jsonResult;
                    pieChartResponse.SearchTerm = sTerm;
                    pieChartResponse.SearchName = sName;
                    lstPieChartResponse.Add(pieChartResponse);
                }

                return lstPieChartResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Result
        public List<DiscoveryMediaResult> SearchTVResult(int p_CustomerKey, string searchTerm, DateTime? fromDate, DateTime? toDate, string subMediaType, bool isAsc, Guid clientGUID, bool IsAllDmaAllowed, List<IQ_Dma> listDma, bool IsAllClassAllowed, List<IQ_Class> listClass, bool IsAllStationAllowed, List<IQ_Station> listStation, List<Station_Affil> listAffiliate, List<IQ_Region> listRegion, List<IQ_Country> listCountry, IQClient_ThresholdValueModel p_IQClient_ThresholdValueModel, Int32 p_PageSize, out List<String> tvMarketList, string pmgSearchUrl, List<int> TVRegions, TVAdvanceSearchSettings TVSearchSettings) 
        {
            string updatedSearchTerm = (TVSearchSettings == null || string.IsNullOrWhiteSpace(TVSearchSettings.SearchTerm)) ? searchTerm : TVSearchSettings.SearchTerm.Trim();
            List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<DiscoveryMediaResult>();
            tvMarketList = new List<string>();
            try
            {
                Log4NetLogger.Debug("Task: SearchTVResult -- Searchterm :" + updatedSearchTerm);

                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                
                SearchRequest searchRequest = new SearchRequest();
                searchRequest.IsTitleNContentSearch = true;
                searchRequest.IncludeRegionsNum = TVRegions;

//Criteria                
                searchRequest.Terms = updatedSearchTerm;

                #region Search Title
                if (TVSearchSettings != null && !string.IsNullOrWhiteSpace(TVSearchSettings.ProgramTitle))
                {
                    searchRequest.Title120 = TVSearchSettings.ProgramTitle.Trim();
                }
                #endregion

                #region Search Appearing
                if (TVSearchSettings != null && !string.IsNullOrWhiteSpace(TVSearchSettings.Appearing))
                {
                    searchRequest.Appearing = TVSearchSettings.Appearing.Trim();
                }
                #endregion

                #region Search Class (Category) List
                bool isCategoryValid = IsAllClassAllowed;

                searchRequest.IQClassNum = new List<string>();
                if (TVSearchSettings != null && TVSearchSettings.CategoryList != null && TVSearchSettings.CategoryList.Count > 0)
                {
                    if (!IsAllClassAllowed)
                    {
                        List<string> lstclass = listClass.Join(TVSearchSettings.CategoryList, a => a.Name, b => b, (a, b) => b).ToList();
                        if (lstclass != null && lstclass.Count > 0)
                        {
                            searchRequest.IQClassNum = lstclass;
                            isCategoryValid = true;
                        }
                        else
                        {
                            isCategoryValid = false;
                        }
                    }
                    else
                    {
                        searchRequest.IQClassNum = TVSearchSettings.CategoryList;
                        isCategoryValid = true;
                    }
                }
                else if (!IsAllClassAllowed && listClass != null && listClass.Count > 0)
                {
                    searchRequest.IQClassNum = listClass.Select(s => s.Num).ToList();
                    isCategoryValid = true;
                }

                if (!isCategoryValid)
                {
                    throw new Exception();
                }
                #endregion

                #region Search DMA (Market) List
                Log4NetLogger.Debug("IsAllDmaAllowed : " + IsAllDmaAllowed);

                bool isDmaValid = false;
                isDmaValid = IsAllDmaAllowed;

                searchRequest.IQDmaName = new List<string>();
                if (TVSearchSettings != null && TVSearchSettings.IQDmaList != null && TVSearchSettings.IQDmaList.Count > 0)
                {
                    if (!IsAllDmaAllowed)
                    {
                        List<string> lstdma = listDma.Join(TVSearchSettings.IQDmaList, a => a.Name, b => b, (a, b) => b).ToList();
                        if (lstdma != null && lstdma.Count > 0)
                        {
                            searchRequest.IQDmaName = lstdma;
                            isDmaValid = true;
                        }
                        else
                        {
                            isDmaValid = false;
                        }
                    }
                    else
                    {
                        searchRequest.IQDmaName = TVSearchSettings.IQDmaList;
                        isDmaValid = true;
                    }
                }
                else if (!IsAllDmaAllowed && listDma != null && listDma.Count > 0)
                {
                    searchRequest.IQDmaName = listDma.Select(s => s.Name).ToList();
                    isDmaValid = true;
                }

                if (!isDmaValid)
                {
                    throw new Exception();
                }
                #endregion

                #region Search Affiliate List
                bool isAffiliateValid = false;
                isAffiliateValid = IsAllStationAllowed;

                searchRequest.StationAffil = new List<string>();

                if (TVSearchSettings != null && TVSearchSettings.AffiliateList != null && TVSearchSettings.AffiliateList.Count > 0)
                {
                    if (!IsAllStationAllowed)
                    {
                        List<string> lstaffil = listAffiliate.Join(TVSearchSettings.AffiliateList, a => a.Name, b => b, (a, b) => b).ToList();
                        if (lstaffil != null && lstaffil.Count > 0)
                        {
                            searchRequest.StationAffil = lstaffil;
                            isAffiliateValid = true;
                        }
                        else
                        {
                            isAffiliateValid = false;
                        }
                    }
                    else
                    {
                        searchRequest.StationAffil = TVSearchSettings.AffiliateList;
                        isAffiliateValid = true;
                    }
                }
                else if (!IsAllStationAllowed && listAffiliate != null && listAffiliate.Count > 0)
                {
                    searchRequest.StationAffil = listAffiliate.Select(s => s.Name).ToList();
                    isAffiliateValid = true;
                }

                if (!isAffiliateValid)
                {
                    throw new Exception();
                }
                #endregion

                #region Search Station List
                bool isStationValid = false;
                isStationValid = IsAllStationAllowed;

                searchRequest.Stations = new List<string>();

                if (TVSearchSettings != null && TVSearchSettings.StationList != null && TVSearchSettings.StationList.Count > 0)
                {
                    if (!IsAllStationAllowed)
                    {
                        List<string> lststation = listStation.Join(TVSearchSettings.StationList, a => a.IQ_Station_ID, b => b, (a, b) => b).ToList();
                        if (lststation != null && lststation.Count > 0)
                        {
                            searchRequest.Stations = lststation;
                            isStationValid = true;
                        }
                        else
                        {
                            isStationValid = false;
                        }
                    }
                    else
                    {
                        searchRequest.Stations = TVSearchSettings.StationList;
                        isStationValid = true;
                    }
                }
                else if (!IsAllStationAllowed && listStation != null && listStation.Count > 0)
                {
                    searchRequest.Stations = listStation.Select(s => s.IQ_Station_ID).ToList();
                    isStationValid = true;
                }

                if (!isStationValid)
                {
                    throw new Exception();
                }
                #endregion

                #region Search Region List
                var isRegionValid = false;
                if (TVSearchSettings != null && TVSearchSettings.RegionList != null && TVSearchSettings.RegionList.Count > 0)
                {
                    List<int> lstTVRegion = listRegion.Join(TVSearchSettings.RegionList, a => a.Num, b => Convert.ToInt32(b), (a, b) => a.Num).ToList();
                    if (lstTVRegion != null && lstTVRegion.Count > 0)
                    {
                        searchRequest.IncludeRegionsNum = lstTVRegion;
                        isRegionValid = true;
                    }
                    else
                    {
                        isRegionValid = false;
                    }
                }
                else if (listRegion != null && listRegion.Count > 0)
                {
                    searchRequest.IncludeRegionsNum = listRegion.Select(a => a.Num).ToList();
                    isRegionValid = true;
                }

                if (!isRegionValid)
                {
                    throw new Exception();
                }
                #endregion

                #region Search Country List
                var isCountryValid = false;
                if (TVSearchSettings != null && TVSearchSettings.CountryList != null && TVSearchSettings.CountryList.Count > 0)
                {
                    List<int> lstTVCountry = listCountry.Join(TVSearchSettings.CountryList, a => a.Num, b => Convert.ToInt32(b), (a, b) => a.Num).ToList();
                    if (lstTVCountry != null && lstTVCountry.Count > 0)
                    {
                        searchRequest.CountryNums = lstTVCountry;
                        isCountryValid = true;
                    }
                    else
                    {
                        isCountryValid = false;
                    }
                }
                else if (listCountry != null && listCountry.Count > 0)
                {
                    searchRequest.CountryNums = listCountry.Select(a => a.Num).ToList();
                    isCountryValid = true;
                }

                if (!isCountryValid)
                {
                    throw new Exception();
                }
                #endregion
//End Criteria
                if (isAsc)
                {
                    searchRequest.SortFields = "date";
                }
                else
                {
                    searchRequest.SortFields = "date-";
                }
                searchRequest.PageSize = p_PageSize;

                if (fromDate == null || toDate == null)
                {
                    searchRequest.StartDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchRequest.EndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchRequest.StartDate = fromDate;
                    searchRequest.EndDate = toDate;
                }

                searchRequest.IsSentiment = true;
                searchRequest.IsShowCC = true;
                searchRequest.LowThreshold = p_IQClient_ThresholdValueModel.TVLowThreshold;
                searchRequest.HighThreshold = p_IQClient_ThresholdValueModel.TVHighThreshold;

                string CommaSepratedDmaList = string.Empty;
                string CommaSepratedClassList = string.Empty;
                string CommaSepratedStationList = string.Empty;
                if (searchRequest.IQClassNum != null && searchRequest.IQClassNum.Count > 0)
                {
                    CommaSepratedClassList = string.Join(",", searchRequest.IQClassNum.ToArray());
                    Log4NetLogger.Debug("Class List :" + CommaSepratedClassList);
                }
                if (searchRequest.IQDmaName != null && searchRequest.IQDmaName.Count > 0)
                {
                    CommaSepratedDmaList = string.Join(",", searchRequest.IQDmaName.ToArray());
                    Log4NetLogger.Debug("DMA List :" + CommaSepratedDmaList);
                }
                if (searchRequest.Stations != null && searchRequest.Stations.Count > 0)
                {
                    CommaSepratedStationList = string.Join(",", searchRequest.Stations.ToArray());
                }

                SearchResult searchResult = searchEngine.Search(searchRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]), false);

                #region insert pmg log
                string _Responce = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.LoadXml(searchResult.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Responce = _XmlDocument.InnerXml;
                            _Responce = _Responce.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Responce = null;
                }

                UtilityLogic.InsertPMGSearchLog(p_CustomerKey, searchRequest.Terms, string.Empty, CommaSepratedDmaList, CommaSepratedStationList, CommaSepratedClassList, searchRequest.PageNumber, searchRequest.PageSize, searchRequest.MaxHighlights, searchRequest.StartDate, searchRequest.EndDate, IQMedia.Shared.Utility.CommonFunctions.SearchType.Discovery_TV.ToString(), _Responce);
                #endregion

                XDocument xDoc = new XDocument(new XElement("list"));
                if (searchResult.Hits != null)
                {
                    foreach (Hit hit in searchResult.Hits)
                    {
                        xDoc.Root.Add(new XElement("item", new XAttribute("iq_cc_key", hit.Iqcckey), new XAttribute("iq_dma", hit.IQDmaNum)));
                        DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                        discoveryMediaResult.Date = hit.GmtDateTime;
                        discoveryMediaResult.LocalDateTime = Convert.ToDateTime(hit.RLStationDateTime);
                        discoveryMediaResult.VideoGuid = new Guid(hit.Guid);
                        discoveryMediaResult.Title = hit.Title120;
                        discoveryMediaResult.PositiveSentiment = hit.Sentiments.PositiveSentiment;
                        discoveryMediaResult.NegativeSentiment = hit.Sentiments.NegativeSentiment;
                        discoveryMediaResult.IQ_CC_Key = hit.Iqcckey;
                        discoveryMediaResult.Body = string.Join(" ", hit.TermOccurrences.Select(s => s.SurroundingText).ToArray());

                        discoveryMediaResult.MediumType = subMediaType;
                        discoveryMediaResult.SearchTerm = searchTerm;
                        discoveryMediaResult.TotalRecords = searchResult.TotalHitCount;

                        discoveryMediaResult.StationLogo = ConfigurationManager.AppSettings["StationLogo"] + hit.StationId + ".jpg";
                        discoveryMediaResult.Market = hit.Market;
                        discoveryMediaResult.IsValid = true;
                        discoveryMediaResult.IncludeInResult = true;
                        discoveryMediaResult.TimeZone = hit.ClipTimeZone;
                        lstDiscoveryMediaResult.Add(discoveryMediaResult);
                    }

                    if (Convert.ToString(xDoc).Length > 0)
                    {
                        IQNielsenDA iqNielsenDA = (IQNielsenDA)DataAccessFactory.GetDataAccess(DataAccessType.IQNielsen);
                        lstDiscoveryMediaResult = iqNielsenDA.GetNielsenDataByXML(xDoc, lstDiscoveryMediaResult, clientGUID);
                    }
                }
                Log4NetLogger.Debug("Task: SearchTVResult -- Result_Count :" + lstDiscoveryMediaResult.Count() + "_" + DateTime.Now);

                return lstDiscoveryMediaResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DiscoveryMediaResult> SearchNewsResult(int p_CustomerKey, string searchTerm, DateTime? fromDate, DateTime? toDate, string subMediaType, bool isAsc, Guid clientGUID, IQClient_ThresholdValueModel p_IQClient_ThresholdValueModel, Int32 p_PageSize, string pmgSearchUrl, List<Int16> lstOfIQLicense, NewsAdvanceSearchSettings NewsSearchSettings)
        {
            List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<DiscoveryMediaResult>();
            try
            {
                string updatedSearchTerm = (NewsSearchSettings == null || string.IsNullOrWhiteSpace(NewsSearchSettings.SearchTerm)) ? searchTerm : NewsSearchSettings.SearchTerm.Trim();

                Log4NetLogger.Debug("Task: SearchNewsResult -- searchTerm :" + updatedSearchTerm);
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchNewsRequest searchNewsRequest = new SearchNewsRequest();
                searchNewsRequest.IsTitleNContentSearch = true;
                searchNewsRequest.IsReturnHighlight = true;
                searchNewsRequest.Facet = false;
                searchNewsRequest.PageSize = p_PageSize;
                
                if (isAsc)
                {
                    searchNewsRequest.SortFields = "date";
                }
                else
                {
                    searchNewsRequest.SortFields = "date-";
                }

                foreach (Int16 iqlicense in lstOfIQLicense)
                {
                    searchNewsRequest.IQLicense.Add(iqlicense);
                }

                if (fromDate == null || toDate == null)
                {
                    searchNewsRequest.StartDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchNewsRequest.EndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchNewsRequest.StartDate = fromDate;
                    searchNewsRequest.EndDate = toDate;
                }

//Criteria
                searchNewsRequest.SearchTerm = updatedSearchTerm;
                searchNewsRequest.SourceType = new List<int>() { (int)PMGSearch.SourceType.OnlineNews };

                if (NewsSearchSettings != null)
                {
                    searchNewsRequest.Publications = NewsSearchSettings.PublicationList;
                    searchNewsRequest.NewsCategory = NewsSearchSettings.CategoryList;
                    searchNewsRequest.PublicationCategory = NewsSearchSettings.PublicationCategoryList;
                    searchNewsRequest.Genre = NewsSearchSettings.GenreList;
                    searchNewsRequest.Market = NewsSearchSettings.MarketList;
                    searchNewsRequest.NewsRegion = NewsSearchSettings.RegionList;
                    searchNewsRequest.Country = NewsSearchSettings.CountryList;
                    searchNewsRequest.Language = NewsSearchSettings.LanguageList;
                    searchNewsRequest.ExcludeDomains = NewsSearchSettings.ExcludeDomainList;
                }
//End Criteria

                searchNewsRequest.IsSentiment = true;

                searchNewsRequest.HighThreshold = p_IQClient_ThresholdValueModel.NMHighThreshold;
                searchNewsRequest.LowThreshold = p_IQClient_ThresholdValueModel.NMLowThreshold;

                SearchNewsResults searchNewsResults = searchEngine.SearchNews(searchNewsRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));

                Log4NetLogger.Info("pmg results parsing in logic , search term : " + updatedSearchTerm + "");

                #region insert pmg log
                string _Responce = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.LoadXml(searchNewsResults.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Responce = _XmlDocument.InnerXml;
                            _Responce = _Responce.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Responce = null;
                }

                UtilityLogic.InsertPMGSearchLog(p_CustomerKey, searchNewsRequest.SearchTerm, string.Empty, string.Empty, string.Empty, string.Empty, searchNewsRequest.PageNumber, searchNewsRequest.PageSize, 0, searchNewsRequest.StartDate, searchNewsRequest.EndDate, IQMedia.Shared.Utility.CommonFunctions.SearchType.Discovery_NM.ToString(), _Responce);


                #endregion

                int wordsBeforeSpan = Convert.ToInt32(ConfigurationManager.AppSettings["HighlightWordsBeforeSpan"]);
                int wordsAfterSpan = Convert.ToInt32(ConfigurationManager.AppSettings["HighlightWordsAfterSpan"]);
                string seprator = "...&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;...";

                if (searchNewsResults.newsResults != null)
                {
                    foreach (NewsResult newsResult in searchNewsResults.newsResults)
                    {
                        DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                        discoveryMediaResult.Date = Convert.ToDateTime(newsResult.date);
                        discoveryMediaResult.Title = newsResult.Title;
                        discoveryMediaResult.PositiveSentiment = newsResult.Sentiments.PositiveSentiment;
                        discoveryMediaResult.NegativeSentiment = newsResult.Sentiments.NegativeSentiment;
                        string body = string.Empty;
                        if (newsResult.Highlights != null && newsResult.Highlights.Count > 0)
                        {
                            body = CommonFunctions.GetWordsAround(string.Join(" ", newsResult.Highlights), "span", wordsBeforeSpan, wordsAfterSpan + 4, seprator);
                        }

                        discoveryMediaResult.Body = body;
                        discoveryMediaResult.ArticleURL = newsResult.Article;
                        discoveryMediaResult.Publication = newsResult.HomeurlDomain;
                        discoveryMediaResult.ArticleID = newsResult.IQSeqID;

                        discoveryMediaResult.MediumType = subMediaType;
                        discoveryMediaResult.SearchTerm = searchTerm;
                        discoveryMediaResult.TotalRecords = searchNewsResults.TotalResults;
                        discoveryMediaResult.IsValid = true;
                        discoveryMediaResult.IncludeInResult = true;
                        discoveryMediaResult.IQLicense = newsResult.IQLicense;
                        lstDiscoveryMediaResult.Add(discoveryMediaResult);

                    }

                    Log4NetLogger.Info("pmg results parsed as discovery result , search term : " + updatedSearchTerm + "");

                    var distinctDisplayUrl = searchNewsResults.newsResults.Select(a => a.HomeurlDomain).Distinct().ToList();

                    var displyUrlXml = new XElement("list",
                                            from string websiteurl in distinctDisplayUrl
                                            select new XElement("item", new XAttribute("url", websiteurl)));

                    IQCompeteAllDA iqCompeteAllDA = (IQCompeteAllDA)DataAccessFactory.GetDataAccess(DataAccessType.IQCompeteAll);
                    List<IQCompeteAll> _ListOfIQ_CompeteAll = iqCompeteAllDA.GetArtileAdShareValueByClientGuidAndXml(clientGUID, Convert.ToString(displyUrlXml), CommonFunctions.CategoryType.NM.ToString());
                    foreach (DiscoveryMediaResult discoveryMediaResult in lstDiscoveryMediaResult)
                    {
                        string href = discoveryMediaResult.Publication;
                        IQCompeteAll _IQCompeteAll = _ListOfIQ_CompeteAll.Find(a => a.CompeteURL.Equals(href));

                        discoveryMediaResult.Audience = _IQCompeteAll != null && _IQCompeteAll.c_uniq_visitor.HasValue && _IQCompeteAll.c_uniq_visitor.Value > 0 ? _IQCompeteAll.c_uniq_visitor : null;

                        discoveryMediaResult.CompeteImage = (_IQCompeteAll.IsCompeteAll ? "<img src=\"../Images/compete.jpg\" style=\"width:14px\"  title=\"Powered by Compete\" />" : "");

                        discoveryMediaResult.IQAdsharevalue = _IQCompeteAll != null && _IQCompeteAll.IQ_AdShare_Value.HasValue && _IQCompeteAll.IQ_AdShare_Value.Value > 0 ? _IQCompeteAll.IQ_AdShare_Value : null;
                    }
                }

                Log4NetLogger.Debug("Task: SearchNewsResult -- Result_Count :" + lstDiscoveryMediaResult.Count() + "_" + DateTime.Now);
                return lstDiscoveryMediaResult;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("search news result error, search term : " + searchTerm + "", ex);
                throw;
            }
        }

        public List<DiscoveryMediaResult> SearchLexisNexisResult(int p_CustomerKey, string searchTerm, DateTime? fromDate, DateTime? toDate, string subMediaType, bool isAsc, Guid clientGUID, IQClient_ThresholdValueModel p_IQClient_ThresholdValueModel, Int32 p_PageSize, string pmgSearchUrl, List<Int16> lstOfIQLicense, LexisNexisAdvanceSearchSettings LexisNexisSearchSettings)
        {
            List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<DiscoveryMediaResult>();
            try
            {
                string updatedSearchTerm = (LexisNexisSearchSettings == null || string.IsNullOrWhiteSpace(LexisNexisSearchSettings.SearchTerm)) ? searchTerm : LexisNexisSearchSettings.SearchTerm.Trim();

                Log4NetLogger.Debug("Task: SearchLexisNexisResult -- searchTerm :" + updatedSearchTerm);
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchNewsRequest searchNewsRequest = new SearchNewsRequest();
                searchNewsRequest.IsTitleNContentSearch = true;
                searchNewsRequest.IsReturnHighlight = true;
                searchNewsRequest.Facet = false;
                searchNewsRequest.PageSize = p_PageSize;

                if (isAsc)
                {
                    searchNewsRequest.SortFields = "date";
                }
                else
                {
                    searchNewsRequest.SortFields = "date-";
                }

                foreach (Int16 iqlicense in lstOfIQLicense)
                {
                    searchNewsRequest.IQLicense.Add(iqlicense);
                }

                if (fromDate == null || toDate == null)
                {
                    searchNewsRequest.StartDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchNewsRequest.EndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchNewsRequest.StartDate = fromDate;
                    searchNewsRequest.EndDate = toDate;
                }

                //Criteria
                searchNewsRequest.SearchTerm = updatedSearchTerm;
                searchNewsRequest.SourceType = new List<int>() { (int)PMGSearch.SourceType.Print };

                if (LexisNexisSearchSettings != null)
                {
                    searchNewsRequest.Publications = LexisNexisSearchSettings.PublicationList;
                    searchNewsRequest.NewsCategory = LexisNexisSearchSettings.CategoryList;
                    searchNewsRequest.PublicationCategory = LexisNexisSearchSettings.PublicationCategoryList;
                    searchNewsRequest.Genre = LexisNexisSearchSettings.GenreList;
                    searchNewsRequest.NewsRegion = LexisNexisSearchSettings.RegionList;
                    searchNewsRequest.Country = LexisNexisSearchSettings.CountryList;
                    searchNewsRequest.Language = LexisNexisSearchSettings.LanguageList;
                    searchNewsRequest.ExcludeDomains = LexisNexisSearchSettings.ExcludeDomainList;
                }
                //End Criteria

                searchNewsRequest.IsSentiment = true;

                searchNewsRequest.HighThreshold = p_IQClient_ThresholdValueModel.NMHighThreshold;
                searchNewsRequest.LowThreshold = p_IQClient_ThresholdValueModel.NMLowThreshold;

                SearchNewsResults searchNewsResults = searchEngine.SearchNews(searchNewsRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));

                Log4NetLogger.Info("pmg results parsing in logic , search term : " + updatedSearchTerm + "");

                #region insert pmg log
                string _Responce = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.LoadXml(searchNewsResults.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Responce = _XmlDocument.InnerXml;
                            _Responce = _Responce.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Responce = null;
                }

                UtilityLogic.InsertPMGSearchLog(p_CustomerKey, searchNewsRequest.SearchTerm, string.Empty, string.Empty, string.Empty, string.Empty, searchNewsRequest.PageNumber, searchNewsRequest.PageSize, 0, searchNewsRequest.StartDate, searchNewsRequest.EndDate, IQMedia.Shared.Utility.CommonFunctions.SearchType.Discovery_LN.ToString(), _Responce);
                
                #endregion

                int wordsBeforeSpan = Convert.ToInt32(ConfigurationManager.AppSettings["HighlightWordsBeforeSpan"]);
                int wordsAfterSpan = Convert.ToInt32(ConfigurationManager.AppSettings["HighlightWordsAfterSpan"]);
                string seprator = "...&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;...";

                if (searchNewsResults.newsResults != null)
                {
                    foreach (NewsResult newsResult in searchNewsResults.newsResults)
                    {
                        DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                        discoveryMediaResult.Date = Convert.ToDateTime(newsResult.date);
                        discoveryMediaResult.Title = newsResult.Title;
                        discoveryMediaResult.PositiveSentiment = newsResult.Sentiments.PositiveSentiment;
                        discoveryMediaResult.NegativeSentiment = newsResult.Sentiments.NegativeSentiment;
                        string body = string.Empty;
                        if (newsResult.Highlights != null && newsResult.Highlights.Count > 0)
                        {
                            body = CommonFunctions.GetWordsAround(string.Join(" ", newsResult.Highlights), "span", wordsBeforeSpan, wordsAfterSpan + 4, seprator);
                        }

                        discoveryMediaResult.Body = body;
                        discoveryMediaResult.ArticleURL = newsResult.Article;
                        discoveryMediaResult.Publication = newsResult.HomeurlDomain;
                        discoveryMediaResult.ArticleID = newsResult.IQSeqID;

                        discoveryMediaResult.MediumType = subMediaType;
                        discoveryMediaResult.SearchTerm = searchTerm;
                        discoveryMediaResult.TotalRecords = searchNewsResults.TotalResults;
                        discoveryMediaResult.IsValid = true;
                        discoveryMediaResult.IncludeInResult = true;
                        discoveryMediaResult.IQLicense = newsResult.IQLicense;
                        lstDiscoveryMediaResult.Add(discoveryMediaResult);

                    }

                    Log4NetLogger.Info("pmg results parsed as discovery result , search term : " + updatedSearchTerm + "");

                    var distinctDisplayUrl = searchNewsResults.newsResults.Select(a => a.HomeurlDomain).Distinct().ToList();

                    var displyUrlXml = new XElement("list",
                                            from string websiteurl in distinctDisplayUrl
                                            select new XElement("item", new XAttribute("url", websiteurl)));

                    IQCompeteAllDA iqCompeteAllDA = (IQCompeteAllDA)DataAccessFactory.GetDataAccess(DataAccessType.IQCompeteAll);
                    List<IQCompeteAll> _ListOfIQ_CompeteAll = iqCompeteAllDA.GetArtileAdShareValueByClientGuidAndXml(clientGUID, Convert.ToString(displyUrlXml), CommonFunctions.CategoryType.NM.ToString());
                    foreach (DiscoveryMediaResult discoveryMediaResult in lstDiscoveryMediaResult)
                    {
                        string href = discoveryMediaResult.Publication;
                        IQCompeteAll _IQCompeteAll = _ListOfIQ_CompeteAll.Find(a => a.CompeteURL.Equals(href));

                        discoveryMediaResult.Audience = _IQCompeteAll != null && _IQCompeteAll.c_uniq_visitor.HasValue && _IQCompeteAll.c_uniq_visitor.Value > 0 ? _IQCompeteAll.c_uniq_visitor : null;

                        discoveryMediaResult.CompeteImage = (_IQCompeteAll.IsCompeteAll ? "<img src=\"../Images/compete.jpg\" style=\"width:14px\"  title=\"Powered by Compete\" />" : "");

                        discoveryMediaResult.IQAdsharevalue = _IQCompeteAll != null && _IQCompeteAll.IQ_AdShare_Value.HasValue && _IQCompeteAll.IQ_AdShare_Value.Value > 0 ? _IQCompeteAll.IQ_AdShare_Value : null;
                    }
                }

                Log4NetLogger.Debug("Task: SearchLexisNexisResult -- Result_Count :" + lstDiscoveryMediaResult.Count() + "_" + DateTime.Now);
                return lstDiscoveryMediaResult;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("search LexisNexis result error, search term : " + searchTerm + "", ex);
                throw;
            }
        }

        public List<DiscoveryMediaResult> SearchBlogResult(int p_CustomerKey, string searchTerm, DateTime? fromDate, DateTime? toDate, string subMediaType, bool isAsc, Guid clientGUID, IQClient_ThresholdValueModel p_IQClient_ThresholdValueModel, Int32 p_PageSize, string pmgSearchUrl, BlogAdvanceSearchSettings blogSearchSettings)
        {
            string updatedSearchTerm = (blogSearchSettings == null || string.IsNullOrWhiteSpace(blogSearchSettings.SearchTerm)) ? searchTerm : blogSearchSettings.SearchTerm.Trim();
            List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<DiscoveryMediaResult>();
            try
            {
                Log4NetLogger.Debug("Task: SearchBlogResult -- searchTerm :" + updatedSearchTerm);
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchSMRequest searchSMRequest = new SearchSMRequest();
                searchSMRequest.IsTitleNContentSearch = true;
                searchSMRequest.IsReturnHighlight = true;
                searchSMRequest.Facet = true;
                searchSMRequest.FacetRangeOther = "all";

                searchSMRequest.PageSize = p_PageSize;
                if (isAsc)
                {
                    searchSMRequest.SortFields = "date";
                }
                else
                {
                    searchSMRequest.SortFields = "date-";
                }

                if (fromDate == null || toDate == null)
                {
                    searchSMRequest.StartDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchSMRequest.EndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchSMRequest.StartDate = fromDate;
                    searchSMRequest.EndDate = toDate;
                }

                //Criteria
                searchSMRequest.SearchTerm = updatedSearchTerm;

                if (blogSearchSettings != null)
                {
                    searchSMRequest.SocialMediaSources = blogSearchSettings.SourceList;
                    searchSMRequest.Author = blogSearchSettings.Author;
                    searchSMRequest.Title = blogSearchSettings.Title;
                    searchSMRequest.ExcludeDomains = blogSearchSettings.ExcludeDomainList;
                }
                //End Criteria

                searchSMRequest.IsTaggingExcluded = true;
                searchSMRequest.SourceType = new List<string>() { CommonFunctions.GetEnumDescription(PMGSearch.SourceType.Blog) };
                searchSMRequest.IsSentiment = true;

                searchSMRequest.HighThreshold = p_IQClient_ThresholdValueModel.SMHighThreshold;
                searchSMRequest.LowThreshold = p_IQClient_ThresholdValueModel.SMLowThreshold;

                SearchSMResult searchSMResult = searchEngine.SearchSocialMedia(searchSMRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));

                #region insert pmg log
                string _Responce = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.LoadXml(searchSMResult.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Responce = _XmlDocument.InnerXml;
                            _Responce = _Responce.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Responce = null;
                }

                UtilityLogic.InsertPMGSearchLog(p_CustomerKey, searchSMRequest.SearchTerm, string.Empty, string.Empty, string.Empty, string.Empty, searchSMRequest.PageNumber, searchSMRequest.PageSize, 0, searchSMRequest.StartDate, searchSMRequest.EndDate, IQMedia.Shared.Utility.CommonFunctions.SearchType.Discovery_BL.ToString(), _Responce);

                #endregion

                int wordsBeforeSpan = Convert.ToInt32(ConfigurationManager.AppSettings["HighlightWordsBeforeSpan"]);
                int wordsAfterSpan = Convert.ToInt32(ConfigurationManager.AppSettings["HighlightWordsAfterSpan"]);
                string seprator = "...&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;...";

                if (searchSMResult != null && searchSMResult.smResults != null)
                {
                    foreach (SMResult smResult in searchSMResult.smResults)
                    {

                        DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                        discoveryMediaResult.Date = Convert.ToDateTime(smResult.itemHarvestDate_DT);
                        discoveryMediaResult.Title = smResult.description;
                        discoveryMediaResult.PositiveSentiment = smResult.Sentiments.PositiveSentiment;
                        discoveryMediaResult.NegativeSentiment = smResult.Sentiments.NegativeSentiment;

                        string body = string.Empty;
                        if (smResult.Highlights != null && smResult.Highlights.Count > 0)
                        {
                            body = CommonFunctions.GetWordsAround(string.Join(" ", smResult.Highlights), "span", wordsBeforeSpan, wordsAfterSpan + 4, seprator);
                        }

                        discoveryMediaResult.Body = body;
                        discoveryMediaResult.ArticleURL = smResult.link;
                        discoveryMediaResult.Publication = smResult.HomeurlDomain;
                        discoveryMediaResult.ArticleID = smResult.IQSeqID;
                        discoveryMediaResult.TotalRecords = searchSMResult.TotalResults;
                        discoveryMediaResult.IsValid = true;
                        discoveryMediaResult.IncludeInResult = true;
                        discoveryMediaResult.MediumType = subMediaType;
                        discoveryMediaResult.SearchTerm = searchTerm;

                        lstDiscoveryMediaResult.Add(discoveryMediaResult);
                    }

                    List<SMResult> lstSMResults = new List<SMResult>();

                    lstSMResults = searchSMResult.smResults.Select(a => new SMResult()
                    {
                        HomeurlDomain = a.HomeurlDomain,
                        feedClass = !string.IsNullOrWhiteSpace(a.feedClass) ? a.feedClass : string.Empty
                    }).GroupBy(h => h.HomeurlDomain)
                        .Select(s => s.First()).ToList();

                    var displyUrlXml = new XElement("list",
                                            from SMResult smres in lstSMResults
                                            select new XElement("item", new XAttribute("url", smres.HomeurlDomain), new XAttribute("sourceCategory", GetFeedClass(smres.feedClass))));
                    IQCompeteAllDA iqCompeteAllDA = (IQCompeteAllDA)DataAccessFactory.GetDataAccess(DataAccessType.IQCompeteAll);
                    List<IQCompeteAll> _ListOfIQ_CompeteAll = iqCompeteAllDA.GetArtileAdShareValueByClientGuidAndXml(clientGUID, Convert.ToString(displyUrlXml), CommonFunctions.MediaType.SM.ToString());
                    foreach (DiscoveryMediaResult discoveryMediaResult in lstDiscoveryMediaResult)
                    {
                        string href = discoveryMediaResult.Publication;
                        IQCompeteAll _IQCompeteAll = _ListOfIQ_CompeteAll.Find(a => a.CompeteURL.Equals(href));

                        discoveryMediaResult.Audience = _IQCompeteAll != null && _IQCompeteAll.c_uniq_visitor.HasValue && _IQCompeteAll.c_uniq_visitor.Value > 0 ? _IQCompeteAll.c_uniq_visitor : null;

                        discoveryMediaResult.CompeteImage = (_IQCompeteAll.IsCompeteAll ? "<img src=\"../Images/compete.jpg\" style=\"width:14px\"  title=\"Powered by Compete\" />" : "");

                        discoveryMediaResult.IQAdsharevalue = _IQCompeteAll != null && _IQCompeteAll.IQ_AdShare_Value.HasValue && _IQCompeteAll.IQ_AdShare_Value.Value > 0 ? _IQCompeteAll.IQ_AdShare_Value : null;
                    }
                }

                Log4NetLogger.Debug("Task: SearchBlogResult -- Result_Count :" + lstDiscoveryMediaResult.Count() + "_" + DateTime.Now);
                return lstDiscoveryMediaResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DiscoveryMediaResult> SearchForumResult(int p_CustomerKey, string searchTerm, DateTime? fromDate, DateTime? toDate, string subMediaType, bool isAsc, Guid clientGUID, IQClient_ThresholdValueModel p_IQClient_ThresholdValueModel, Int32 p_PageSize, string pmgSearchUrl, ForumAdvanceSearchSettings forumSearchSettings)
        {
            string updatedSearchTerm = (forumSearchSettings == null || string.IsNullOrWhiteSpace(forumSearchSettings.SearchTerm)) ? searchTerm : forumSearchSettings.SearchTerm.Trim();
            List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<DiscoveryMediaResult>();
            try
            {
                Log4NetLogger.Debug("Task: SearchForumResult -- searchTerm :" + updatedSearchTerm);
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchSMRequest searchSMRequest = new SearchSMRequest();
                searchSMRequest.IsTitleNContentSearch = true;
                searchSMRequest.IsReturnHighlight = true;
                searchSMRequest.Facet = true;
                searchSMRequest.FacetRangeOther = "all";

                searchSMRequest.PageSize = p_PageSize;
                if (isAsc)
                {
                    searchSMRequest.SortFields = "date";
                }
                else
                {
                    searchSMRequest.SortFields = "date-";
                }

                if (fromDate == null || toDate == null)
                {
                    searchSMRequest.StartDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchSMRequest.EndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchSMRequest.StartDate = fromDate;
                    searchSMRequest.EndDate = toDate;
                }

                //Criteria
                searchSMRequest.SearchTerm = updatedSearchTerm;

                if (forumSearchSettings != null)
                {
                    searchSMRequest.SocialMediaSources = forumSearchSettings.SourceList;
                    searchSMRequest.Author = forumSearchSettings.Author;
                    searchSMRequest.Title = forumSearchSettings.Title;
                    searchSMRequest.SourceType = forumSearchSettings.SourceTypeList;
                    searchSMRequest.ExcludeDomains = forumSearchSettings.ExcludeDomainList;
                }
                //End Criteria

                searchSMRequest.IsTaggingExcluded = true;
                if (searchSMRequest.SourceType == null || searchSMRequest.SourceType.Count == 0)
                {
                    searchSMRequest.SourceType = new List<string>() { CommonFunctions.GetEnumDescription(PMGSearch.SourceType.Forum) };
                }
                searchSMRequest.IsSentiment = true;

                searchSMRequest.HighThreshold = p_IQClient_ThresholdValueModel.SMHighThreshold;
                searchSMRequest.LowThreshold = p_IQClient_ThresholdValueModel.SMLowThreshold;

                SearchSMResult searchSMResult = searchEngine.SearchSocialMedia(searchSMRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));

                #region insert pmg log
                string _Responce = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.LoadXml(searchSMResult.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Responce = _XmlDocument.InnerXml;
                            _Responce = _Responce.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Responce = null;
                }

                UtilityLogic.InsertPMGSearchLog(p_CustomerKey, searchSMRequest.SearchTerm, string.Empty, string.Empty, string.Empty, string.Empty, searchSMRequest.PageNumber, searchSMRequest.PageSize, 0, searchSMRequest.StartDate, searchSMRequest.EndDate, IQMedia.Shared.Utility.CommonFunctions.SearchType.Discovery_FO.ToString(), _Responce);

                #endregion

                int wordsBeforeSpan = Convert.ToInt32(ConfigurationManager.AppSettings["HighlightWordsBeforeSpan"]);
                int wordsAfterSpan = Convert.ToInt32(ConfigurationManager.AppSettings["HighlightWordsAfterSpan"]);
                string seprator = "...&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;...";

                if (searchSMResult != null && searchSMResult.smResults != null)
                {
                    foreach (SMResult smResult in searchSMResult.smResults)
                    {
                        DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                        discoveryMediaResult.Date = Convert.ToDateTime(smResult.itemHarvestDate_DT);
                        discoveryMediaResult.Title = smResult.description;
                        discoveryMediaResult.PositiveSentiment = smResult.Sentiments.PositiveSentiment;
                        discoveryMediaResult.NegativeSentiment = smResult.Sentiments.NegativeSentiment;

                        string body = string.Empty;
                        if (smResult.Highlights != null && smResult.Highlights.Count > 0)
                        {
                            body = CommonFunctions.GetWordsAround(string.Join(" ", smResult.Highlights), "span", wordsBeforeSpan, wordsAfterSpan + 4, seprator);
                        }

                        discoveryMediaResult.Body = body;
                        discoveryMediaResult.ArticleURL = smResult.link;
                        discoveryMediaResult.Publication = smResult.HomeurlDomain;
                        discoveryMediaResult.ArticleID = smResult.IQSeqID;
                        discoveryMediaResult.TotalRecords = searchSMResult.TotalResults;
                        discoveryMediaResult.IsValid = true;
                        discoveryMediaResult.IncludeInResult = true;
                        discoveryMediaResult.MediumType = subMediaType;
                        discoveryMediaResult.SearchTerm = searchTerm;

                        lstDiscoveryMediaResult.Add(discoveryMediaResult);
                    }

                    List<SMResult> lstSMResults = new List<SMResult>();

                    lstSMResults = searchSMResult.smResults.Select(a => new SMResult()
                    {
                        HomeurlDomain = a.HomeurlDomain,
                        feedClass = !string.IsNullOrWhiteSpace(a.feedClass) ? a.feedClass : string.Empty
                    }).GroupBy(h => h.HomeurlDomain)
                        .Select(s => s.First()).ToList();

                    var displyUrlXml = new XElement("list",
                                            from SMResult smres in lstSMResults
                                            select new XElement("item", new XAttribute("url", smres.HomeurlDomain), new XAttribute("sourceCategory", GetFeedClass(smres.feedClass))));
                    IQCompeteAllDA iqCompeteAllDA = (IQCompeteAllDA)DataAccessFactory.GetDataAccess(DataAccessType.IQCompeteAll);
                    List<IQCompeteAll> _ListOfIQ_CompeteAll = iqCompeteAllDA.GetArtileAdShareValueByClientGuidAndXml(clientGUID, Convert.ToString(displyUrlXml), CommonFunctions.MediaType.SM.ToString());
                    foreach (DiscoveryMediaResult discoveryMediaResult in lstDiscoveryMediaResult)
                    {
                        string href = discoveryMediaResult.Publication;
                        IQCompeteAll _IQCompeteAll = _ListOfIQ_CompeteAll.Find(a => a.CompeteURL.Equals(href));

                        discoveryMediaResult.Audience = _IQCompeteAll != null && _IQCompeteAll.c_uniq_visitor.HasValue && _IQCompeteAll.c_uniq_visitor.Value > 0 ? _IQCompeteAll.c_uniq_visitor : null;

                        discoveryMediaResult.CompeteImage = (_IQCompeteAll.IsCompeteAll ? "<img src=\"../Images/compete.jpg\" style=\"width:14px\"  title=\"Powered by Compete\" />" : "");

                        discoveryMediaResult.IQAdsharevalue = _IQCompeteAll != null && _IQCompeteAll.IQ_AdShare_Value.HasValue && _IQCompeteAll.IQ_AdShare_Value.Value > 0 ? _IQCompeteAll.IQ_AdShare_Value : null;
                    }
                }

                Log4NetLogger.Debug("Task: SearchForumResult -- Result_Count :" + lstDiscoveryMediaResult.Count() + "_" + DateTime.Now);
                return lstDiscoveryMediaResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DiscoveryMediaResult> SearchProQuestResult(int p_CustomerKey, string searchTerm, DateTime? fromDate, DateTime? toDate, string subMediaType, bool isAsc, IQClient_ThresholdValueModel p_IQClient_ThresholdValueModel, Int32 p_PageSize, string pmgSearchUrl, ProQuestAdvanceSearchSettings ProQuestSearchSettings)
        {
            List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<Model.DiscoveryMediaResult>();
            try
            {
                string updatedSearchTerm = (ProQuestSearchSettings == null || string.IsNullOrWhiteSpace(ProQuestSearchSettings.SearchTerm)) ? searchTerm : ProQuestSearchSettings.SearchTerm.Trim();

                Log4NetLogger.Debug("Task: SearchProQuestResult -- searchTerm :" + updatedSearchTerm);
                bool isError = false;
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchProQuestRequest searchProQuestRequest = new SearchProQuestRequest();
                searchProQuestRequest.Facet = false;
                searchProQuestRequest.PageSize = p_PageSize;
                searchProQuestRequest.IsReturnHighlight = true;
                
                if (isAsc)
                {
                    searchProQuestRequest.SortFields = "date";
                }
                else
                {
                    searchProQuestRequest.SortFields = "date-";
                }

                if (fromDate == null || toDate == null)
                {
                    searchProQuestRequest.StartDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchProQuestRequest.EndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchProQuestRequest.StartDate = fromDate;
                    searchProQuestRequest.EndDate = toDate;
                }

//Criteria
                searchProQuestRequest.SearchTerm = updatedSearchTerm;

                if (ProQuestSearchSettings != null)
                {
                    searchProQuestRequest.Publications = ProQuestSearchSettings.PublicationList;
                    searchProQuestRequest.Authors = ProQuestSearchSettings.AuthorList;
                    searchProQuestRequest.Languages = ProQuestSearchSettings.LanguageList;
                }
//End Criteria

                searchProQuestRequest.IsSentiment = true;

                searchProQuestRequest.HighThreshold = p_IQClient_ThresholdValueModel.PQHighThreshold;
                searchProQuestRequest.LowThreshold = p_IQClient_ThresholdValueModel.PQLowThreshold;

                SearchProQuestResult searchProQuestResult = searchEngine.SearchProQuest(searchProQuestRequest, false, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));

                #region insert pmg log
                string _Response = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.LoadXml(searchProQuestResult.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Response = _XmlDocument.InnerXml;
                            _Response = _Response.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Response = null;
                }

                UtilityLogic.InsertPMGSearchLog(p_CustomerKey, searchProQuestRequest.SearchTerm, string.Empty, string.Empty, string.Empty, string.Empty, searchProQuestRequest.PageNumber, searchProQuestRequest.PageSize, 0, searchProQuestRequest.StartDate, searchProQuestRequest.EndDate, IQMedia.Shared.Utility.CommonFunctions.SearchType.Discovery_PQ.ToString(), _Response);

                #endregion

                int wordsBeforeSpan = Convert.ToInt32(ConfigurationManager.AppSettings["HighlightWordsBeforeSpan"]);
                int wordsAfterSpan = Convert.ToInt32(ConfigurationManager.AppSettings["HighlightWordsAfterSpan"]);
                string separator = "...&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;...";
                if (searchProQuestResult != null && searchProQuestResult.ProQuestResults != null)
                {
                    int i = 1;
                    foreach (ProQuestResult proQuestResult in searchProQuestResult.ProQuestResults)
                    {
                        DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                        discoveryMediaResult.Date = proQuestResult.MediaDate;
                        discoveryMediaResult.Title = proQuestResult.Title; 
                        string body = String.Empty;
                        if (proQuestResult.Highlights != null && proQuestResult.Highlights.Count > 0)
                        {
                            body = CommonFunctions.GetWordsAround(string.Join(" ", proQuestResult.Highlights), "span", wordsBeforeSpan, wordsAfterSpan + 4, separator);
                        }
                        discoveryMediaResult.Body = body;
                        discoveryMediaResult.Publication = proQuestResult.Publication;
                        string authors = String.Empty;
                        if (proQuestResult.Authors != null && proQuestResult.Authors.Count > 0)
                        {
                            if (proQuestResult.Authors.Count > 2)
                            {
                                authors = String.Join(", ", proQuestResult.Authors.Take(2)) + ", ...";
                            }
                            else
                            {
                                authors = String.Join(", ", proQuestResult.Authors);
                            }
                        }
                        discoveryMediaResult.ProQuestCopyright = proQuestResult.Copyright;
                        discoveryMediaResult.ProQuestAuthors = authors;
                        discoveryMediaResult.PositiveSentiment = proQuestResult.Sentiments.PositiveSentiment;
                        discoveryMediaResult.NegativeSentiment = proQuestResult.Sentiments.NegativeSentiment;
                        discoveryMediaResult.TotalRecords = searchProQuestResult.TotalResults;
                        discoveryMediaResult.ArticleID = Convert.ToString(proQuestResult.IQSeqID);
                        discoveryMediaResult.MediumType = subMediaType;
                        discoveryMediaResult.SearchTerm = searchTerm;
                        discoveryMediaResult.IsValid = true;
                        discoveryMediaResult.IncludeInResult = true;

                        lstDiscoveryMediaResult.Add(discoveryMediaResult);
                        i++;
                    }
                }

                Log4NetLogger.Debug("Task: SearchProQuestResult -- Result_Count :" + lstDiscoveryMediaResult.Count() + "_" + DateTime.Now);
                return lstDiscoveryMediaResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DiscoveryTopicsResponse GetTVTopics(string searchTerm, DateTime fromDate, DateTime toDate, string pmgSearchUrl, TVAdvanceSearchSettings tvSettings, CancellationToken token, DiscoveryTopicsResponse dtr)
        {
            try
            {
                // Build request XML
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchRequest searchRequest = new SearchRequest(); XDocument xDoc = new XDocument();

                XElement root = new XElement("parameters",
                                    new XElement("televisionbaseurl", pmgSearchUrl),
                                    new XElement("searchterm", searchTerm),
                                    new XElement("date", "[" + fromDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + " TO " + toDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "]"),
                                    new XElement("resultnum", "10"),
                                    new XElement("title"),
                                    new XElement("market"),
                                    new XElement("region"),
                                    new XElement("country"),
                                    new XElement("affiliate"),
                                    new XElement("stationid")
                                );

                if (tvSettings != null)
                {
                    if (!String.IsNullOrWhiteSpace(tvSettings.ProgramTitle))
                    {
                        root.Descendants("title").FirstOrDefault().Value = tvSettings.ProgramTitle;
                    }
                    if (tvSettings.IQDmaList != null && tvSettings.IQDmaList.Count > 0)
                    {
                        root.Descendants("market").FirstOrDefault().Value = "(\"" + String.Join("\" OR \"", tvSettings.IQDmaList) + "\")";
                    }
                    if (tvSettings.RegionList != null && tvSettings.RegionList.Count > 0)
                    {
                        root.Descendants("region").FirstOrDefault().Value = "(" + String.Join(" OR ", tvSettings.RegionList) + ")";
                    }
                    if (tvSettings.CountryList != null && tvSettings.CountryList.Count > 0)
                    {
                        root.Descendants("country").FirstOrDefault().Value = "(" + String.Join(" OR ", tvSettings.CountryList) + ")";
                    }
                    if (tvSettings.AffiliateList != null && tvSettings.AffiliateList.Count > 0)
                    {
                        root.Descendants("affiliate").FirstOrDefault().Value = "(\"" + String.Join("\" OR \"", tvSettings.AffiliateList) + "\")";
                    }
                    if (tvSettings.StationList != null && tvSettings.StationList.Count > 0)
                    {
                        root.Descendants("stationid").FirstOrDefault().Value = "(\"" + String.Join("\" OR \"", tvSettings.StationList) + "\")";
                    }
                }

                XDocument xdoc = new XDocument(root);
                Log4NetLogger.Info("TV Topics - " + xdoc.ToString());
                // Execute request
                List<Topics> lstTopics = GenerateTopics(xdoc);

                if (!token.IsCancellationRequested)
                {
                    dtr.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                dtr.ListTopics = lstTopics;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("GetTVTopics: " + ex.Message + " || " + ex.StackTrace);
                dtr.IsValid = false;
            }
            return dtr;
        }
        public DiscoveryTopicsResponse GetNewsTopics(string searchTerm, DateTime fromDate, DateTime toDate, string pmgSearchUrl, NewsAdvanceSearchSettings newsSettings, CancellationToken token, DiscoveryTopicsResponse dtr)
        {
            try
            {
                // Build request XML
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchRequest searchRequest = new SearchRequest(); XDocument xDoc = new XDocument();

                XElement root = new XElement("parameters",
                                    new XElement("moreovernmbaseurl", pmgSearchUrl),
                                    new XElement("searchterm", searchTerm),
                                    new XElement("date", "[" + fromDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + " TO " + toDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "]"),
                                    new XElement("iqsubmediatype", "(1)"),
                                    new XElement("region"),
                                    new XElement("country"),
                                    new XElement("publication"),
                                    new XElement("excludedomain"),
                                    new XElement("language"),
                                    new XElement("resultnum", "10")
                                );

                if (newsSettings != null)
                {
                    if (newsSettings.RegionList != null && newsSettings.RegionList.Count > 0)
                    {
                        root.Descendants("region").FirstOrDefault().Value = "(" + String.Join(" OR ", newsSettings.RegionList) + ")";
                    }
                    if (newsSettings.CountryList != null && newsSettings.CountryList.Count > 0)
                    {
                        root.Descendants("country").FirstOrDefault().Value = "(\"" + String.Join("\" OR \"", newsSettings.CountryList) + "\")";
                    }
                    if (newsSettings.PublicationList != null && newsSettings.PublicationList.Count > 0)
                    {
                        root.Descendants("publication").FirstOrDefault().Value = "(" + String.Join(" OR ", newsSettings.PublicationList) + ")";
                    }
                    if (newsSettings.LanguageList != null && newsSettings.LanguageList.Count > 0)
                    {
                        root.Descendants("language").FirstOrDefault().Value = "(\"" + String.Join("\" OR \"", newsSettings.LanguageList) + "\")";
                    }
                    if (newsSettings.ExcludeDomainList != null && newsSettings.ExcludeDomainList.Count > 0)
                    {
                        root.Descendants("excludedomain").FirstOrDefault().Value = "(" + String.Join(" OR ", newsSettings.ExcludeDomainList) + ")";
                    }
                }

                XDocument xdoc = new XDocument(root);
                Log4NetLogger.Info("NM Topics - " + xdoc.ToString());

                // Execute request
                List<Topics> lstTopics = GenerateTopics(xdoc);

                if (!token.IsCancellationRequested)
                {
                    dtr.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                dtr.ListTopics = lstTopics;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("GetNewsTopics: " + ex.Message + " || " + ex.StackTrace);
                dtr.IsValid = false;
            }
            return dtr;
        }
        public DiscoveryTopicsResponse GetSocialMediaTopics(string searchTerm, string medium, DateTime fromDate, DateTime toDate, string pmgSearchUrl, ForumAdvanceSearchSettings smSettings, CancellationToken token, DiscoveryTopicsResponse dtr)
        {
            try
            {
                // Build a list of all submedia types that belong to the selected medium, or if medium is empty default to SocialMedia
                List<string> subMediaTypes = new List<string>();
                if (String.IsNullOrEmpty(medium) || medium == CommonFunctions.CategoryType.SocialMedia.ToString())
                {
                    subMediaTypes.Add(CommonFunctions.GetEnumDescription(PMGSearch.SourceType.Classified));
                    subMediaTypes.Add(CommonFunctions.GetEnumDescription(PMGSearch.SourceType.Microblog));
                    subMediaTypes.Add(CommonFunctions.GetEnumDescription(PMGSearch.SourceType.Podcast));
                    subMediaTypes.Add(CommonFunctions.GetEnumDescription(PMGSearch.SourceType.QnA));
                    subMediaTypes.Add(CommonFunctions.GetEnumDescription(PMGSearch.SourceType.SocialNetwork));
                    subMediaTypes.Add(CommonFunctions.GetEnumDescription(PMGSearch.SourceType.SocialPhoto));
                    subMediaTypes.Add(CommonFunctions.GetEnumDescription(PMGSearch.SourceType.SocialVideo));
                    subMediaTypes.Add(CommonFunctions.GetEnumDescription(PMGSearch.SourceType.Wiki));
                }
                else if (medium == CommonFunctions.CategoryType.Forum.ToString())
                {
                    subMediaTypes.Add(CommonFunctions.GetEnumDescription(PMGSearch.SourceType.Forum));
                    subMediaTypes.Add(CommonFunctions.GetEnumDescription(PMGSearch.SourceType.Review));
                }
                else if (medium == CommonFunctions.CategoryType.Blog.ToString())
                {
                    subMediaTypes.Add(CommonFunctions.GetEnumDescription(PMGSearch.SourceType.Blog));
                }

                // Build request XML
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchRequest searchRequest = new SearchRequest(); XDocument xDoc = new XDocument();

                XElement root = new XElement("parameters",
                                    new XElement("moreoversmbaseurl", pmgSearchUrl),
                                    new XElement("searchterm", searchTerm),
                                    new XElement("date", "[" + fromDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + " TO " + toDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "]"),
                                    new XElement("iqsubmediatype", "(" + String.Join(" OR ", subMediaTypes.Select(s => (int)SearchEngine.GetValueFromDescription<SourceType>(s))) + ")"), // By default, return all submedia types
                                    new XElement("source"),
                                    new XElement("title"),
                                    new XElement("author"),
                                    new XElement("sourcetype"),
                                    new XElement("excludedomain"),
                                    new XElement("resultnum", "10")
                                );

                if (smSettings != null)
                {
                    if (!String.IsNullOrWhiteSpace(smSettings.Title))
                    {
                        root.Descendants("title").FirstOrDefault().Value = smSettings.Title;
                    }
                    if (!String.IsNullOrWhiteSpace(smSettings.Author))
                    {
                        root.Descendants("author").FirstOrDefault().Value = smSettings.Author;
                    }
                    if (smSettings.SourceList != null && smSettings.SourceList.Count > 0)
                    {
                        root.Descendants("source").FirstOrDefault().Value = "(" + String.Join(" OR ", smSettings.SourceList) + ")";
                    }
                    if (smSettings.ExcludeDomainList != null && smSettings.ExcludeDomainList.Count > 0)
                    {
                        root.Descendants("excludedomain").FirstOrDefault().Value = "(" + String.Join(" OR ", smSettings.ExcludeDomainList) + ")";
                    }
                    if (smSettings.SourceTypeList != null && smSettings.SourceTypeList.Count > 0)
                    {
                        // If any submedia types are selected in Advanced Search, filter out any that don't belong to the selected medium. If none do, don't display anything.
                        List<string> filteredSubMediaTypes = smSettings.SourceTypeList.Where(w => subMediaTypes.Contains(w)).ToList();
                        if (filteredSubMediaTypes.Count > 0)
                        {
                            root.Descendants("iqsubmediatype").FirstOrDefault().Value = "(" + String.Join(" OR ", smSettings.SourceTypeList.Select(s => (int)SearchEngine.GetValueFromDescription<SourceType>(s)).ToList()) + ")";
                        }
                        else
                        {
                            root.Descendants("iqsubmediatype").FirstOrDefault().Value = "(0)";
                        }
                    }
                }

                XDocument xdoc = new XDocument(root);

                // Execute request
                List<Topics> lstTopics = GenerateTopics(xdoc);

                if (!token.IsCancellationRequested)
                {
                    dtr.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                dtr.ListTopics = lstTopics;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("GetSocialMediaTopics: " + ex.Message + " || " + ex.StackTrace);
                dtr.IsValid = false;
            }
            return dtr;
        }

        public DiscoveryTopicsResponse GetProQuestTopics(string searchTerm, DateTime fromDate, DateTime toDate, string pmgSearchUrl, ProQuestAdvanceSearchSettings proQuestSettings, CancellationToken token, DiscoveryTopicsResponse dtr)
        {
            try
            {
                // Build request XML
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchRequest searchRequest = new SearchRequest(); XDocument xDoc = new XDocument();

                XElement root = new XElement("parameters",
                                    new XElement("proquestbaseurl", pmgSearchUrl),
                                    new XElement("searchterm", searchTerm),
                                    new XElement("date", "[" + fromDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + " TO " + toDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "]"),
                                    new XElement("publication"),
                                    new XElement("author"),
                                    new XElement("language"),
                                    new XElement("resultnum", "10")
                                );

                if (proQuestSettings != null)
                {
                    if (proQuestSettings.PublicationList != null && proQuestSettings.PublicationList.Count > 0)
                    {
                        root.Descendants("publication").FirstOrDefault().Value = "(" + String.Join(" OR ", proQuestSettings.PublicationList) + ")";
                    }
                    if (proQuestSettings.AuthorList != null && proQuestSettings.AuthorList.Count > 0)
                    {
                        root.Descendants("author").FirstOrDefault().Value = "(" + String.Join(" OR ", proQuestSettings.AuthorList) + ")";
                    }
                    if (proQuestSettings.LanguageList != null && proQuestSettings.LanguageList.Count > 0)
                    {
                        root.Descendants("language").FirstOrDefault().Value = "(" + String.Join(" OR ", proQuestSettings.LanguageList) + ")";
                    }
                }

                XDocument xdoc = new XDocument(root);

                // Execute request
                List<Topics> lstTopics = GenerateTopics(xdoc);

                if (!token.IsCancellationRequested)
                {
                    dtr.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                dtr.ListTopics = lstTopics;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("GetProQuestTopics: " + ex.Message + " || " + ex.StackTrace);
                dtr.IsValid = false;
            }
            return dtr;
        }

        private List<Topics> GenerateTopics(XDocument requestXml)
        {
            string result = CommonFunctions.DoHttpPostRequest(ConfigurationManager.AppSettings["DiscoveryTopicsUrl"], requestXml.ToString());

            UTF8Encoding enc = new UTF8Encoding();
            XmlDocument xDocResult = new XmlDocument();
            xDocResult.Load(new MemoryStream(enc.GetBytes(result)));
            XmlNodeList bigrams = xDocResult.SelectNodes("/results/bigrams/bigram");

            List<Topics> lstTopics = new List<Topics>();
            if (bigrams != null && bigrams.Count > 0)
            {
                foreach (XmlNode bigram in bigrams)
                {
                    lstTopics.Add(new Topics()
                    {
                        Topic = bigram.ChildNodes[0].InnerText,
                        Frequency = Convert.ToInt32(bigram.ChildNodes[1].InnerText)
                    });
                }
            }

            return lstTopics;
        }

        public HighColumnChartModel HighChartsTopicsBarChart(List<Topics> lstTopics, string searchTerm, string searchName, string medium, string mediumName)
        {
            HighColumnChartModel highColumnChartModel = new HighColumnChartModel();

            // set chart type , width and height
            highColumnChartModel.chart = new HChart() { height = 300, width = 450, type = "bar" };

            // hide legend for chart,  by setting enabled = false
            highColumnChartModel.legend = new Legend() { enabled = false };
            highColumnChartModel.subtitle = new Subtitle() { text = "" };

            // set y-axis and label as enabled  = false
            highColumnChartModel.yAxis = new YAxis() { title = new Title2() { text = "" }, labels = new labels() { enabled = false }, gridLineWidth = 0 };
            highColumnChartModel.xAxis = new XAxis()
            {
                categories = lstTopics.Select(s => s.Topic).ToList(),
                labels = new labels()
                {
                    enabled = true,
                    style = new HStyle()
                    {
                        fontWeight = "bold",
                        fontSize = "15px"
                    }
                }
            };

            // set list of series
            highColumnChartModel.series = new List<Series>() 
            { 
                new Series() 
                { 
                    data = lstTopics.Select(s => new HighChartDatum() 
                    { 
                        y = s.Frequency,
                        SearchTerm = searchTerm,
                        SearchName = searchName,
                        Medium = medium,
                        MediumName = mediumName,
                        Value = s.Topic
                    }).ToList() 
                } 
            };

            // set bar height
            highColumnChartModel.plotOptions = new PlotOptions()

            {
                series = new PlotSeries()
                {
                    cursor = "pointer",
                    pointWidth = 15,
                    point = new PlotPoint()
                    {
                        events = new PlotEvents()
                        {
                            click = "TopicClick" // Indicates use of click event. Actual function is set in Discovery.js GetTopics().
                        }
                    }
                }
            };

            return highColumnChartModel;
        }
        #endregion

        public string GetFeedClass(string feedClass)
        {
            if (feedClass.Trim() == "Blog")
            {
                return "Blog";
            }
            else if (feedClass.Trim() == "Review" || feedClass.Trim() == "Forum")
            {
                return "Forum";
            }
            else
            {
                return "Social Media";
            }
        }

        public string GetFeedClassFromInt(int iqSubMediaType)
        {
            bool isDefinedResult = Enum.IsDefined(typeof(PMGSearch.SourceType), iqSubMediaType);
            if (isDefinedResult)
            {
                string feedClass = ((PMGSearch.SourceType)iqSubMediaType).ToString();
                if (feedClass == "Blog")
                {
                    return "Blog";
                }
                else if (feedClass == "Review" || feedClass == "Forum")
                {
                    return "Forum";
                }
                else
                {
                    return "Social Media";
                }
            }
            else
            {
                return "Social Media";
            }
        }

        #region GetFilter

        public IEnumerable GetDateFilter(List<DiscoverySearchResponse> lstDiscoverySearchResponse)
        {
            try
            {
                var distinctDateFilter = (from x in
                                              (
                                                  from sr in lstDiscoverySearchResponse
                                                  from dc in sr.ListRecordData
                                                  select new { dc.Date, dc.TotalRecord }

                                              )
                                          where Convert.ToInt64(x.TotalRecord) > 0
                                          group x by new { x.Date, x.TotalRecord } into g
                                          select new
                                          {
                                              Date = Convert.ToDateTime(g.Key.Date).ToString("MM/dd/yyyy")
                                          }).Distinct().ToList();

                return distinctDateFilter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable GetMediumFilter(List<DiscoverySearchResponse> lstDiscoverySearchResponse, List<IQ_MediaTypeModel> lstMasterMediaTypes)
        {
            try
            {
                var distinctDateFilter = (from x in
                                              (
                                                  from sr in lstDiscoverySearchResponse
                                                  from dc in sr.ListRecordData
                                                  select new { dc.TotalRecord, sr.MediumType }
                                              )
                                          where Convert.ToInt64(x.TotalRecord) > 0
                                          group x by new { x.TotalRecord, x.MediumType } into g
                                          select new
                                          {
                                              Medium = g.Key.MediumType
                                          }).Distinct().ToList();

                List<IQ_MediaTypeModel> lstSubMediaTypes = lstMasterMediaTypes.Where(w => distinctDateFilter.Exists(e => e.Medium.Equals(w.SubMediaType)) && w.TypeLevel == 2).ToList();
                List<IQ_MediaTypeModel> lstMediaTypes = lstMasterMediaTypes.Where(w => lstSubMediaTypes.Exists(e => e.MediaType.Equals(w.MediaType)) && w.TypeLevel == 1).ToList();

                List<MediaTypeFilterData> lstFilterData = lstMediaTypes.Select(s => new MediaTypeFilterData()
                {
                    MediaType = s.MediaType,
                    MediaTypeName = s.DisplayName,
                    SortOrder = s.SortOrder,
                    SubMediaTypeFilterData = !s.HasSubMediaTypes ? null : lstSubMediaTypes.Where(w => w.MediaType.Equals(s.MediaType)).Select(s1 => new SubMediaTypeFilterData()
                    {
                        SubMediaType = s1.SubMediaType,
                        SubMediaTypeName = s1.DisplayName,
                        SortOrder = s.SortOrder
                    }).OrderBy(o => o.SortOrder).ToList()
                }).OrderBy(o => o.SortOrder).ToList();

                return lstFilterData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public List<HighChartDatum> GetSeriesList(Int64 value)
        {
            HighChartDatum highChartDatum = new HighChartDatum();
            List<HighChartDatum> lsthighChartDatum = new List<HighChartDatum>();

            highChartDatum.y = value;

            lsthighChartDatum.Add(highChartDatum);
            return lsthighChartDatum;
        }
    }

    public class SearchTermTotalRecords
    {
        public string SearchName { get; set; }
        public string SearchTerm { get; set; }
        public DateTime RecordDate { get; set; }
        public Int64 TotalRecords { get; set; }
        public string FeedClass { get; set; }
    }
}
