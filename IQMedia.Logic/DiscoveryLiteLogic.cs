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


namespace IQMedia.Web.Logic
{
    public class DiscoveryLiteLogic : IQMedia.Web.Logic.Base.ILogic
    {
        #region Chart

        public DiscoveryLiteSearchResponse SearchTV(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket,
                                                bool IsAllDmaAllowed, List<IQ_Dma> listDma,
                                                bool IsAllClassAllowed, List<IQ_Class> listClass,
                                                bool IsAllStationAllowed, List<Station_Affil> listStation, List<int> listRegion, List<int> listCountry,
                                                out List<String> tvMarketList,string pmgSearchUrl)
        {
            try
            {
                Boolean isError = false;
                tvMarketList = new List<String>();
                DiscoveryLiteSearchResponse discoverySearchResponse = new DiscoveryLiteSearchResponse();
                discoverySearchResponse.SearchTerm = searchTerm;
                discoverySearchResponse.MediumType = CommonFunctions.CategoryType.TV.ToString();
                //List<DiscoverySearchResponse> lstDiscoverySearchResponse = new List<DiscoverySearchResponse>();

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();

                //foreach (string sterm in searchTerm)
                //{



                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchRequest searchRequest = new SearchRequest();

                searchRequest.Terms = searchTerm;
                searchRequest.IsTitleNContentSearch = true;
                searchRequest.Facet = true;
                searchRequest.FacetRangeOther = "all";

                if (!string.IsNullOrWhiteSpace(medium))
                {
                    searchRequest.PageSize = 4;
                }
                else
                {
                    searchRequest.PageSize = 1;
                }

                searchRequest.SortFields = "date-";

                bool isDmaValid = false;
                isDmaValid = IsAllDmaAllowed;

                searchRequest.IQDmaName = new List<string>();
                List<String> lstDMAString = listDma.Select(s => s.Name).ToList();


                if (!string.IsNullOrWhiteSpace(tvMarket))
                {
                    if (IsAllDmaAllowed)
                    {
                        searchRequest.IQDmaName.Add(tvMarket);
                        isDmaValid = true;
                    }
                    else
                    {
                        if (lstDMAString.Contains(tvMarket))
                        {
                            isDmaValid = true;
                            searchRequest.IQDmaName.Add(tvMarket);
                        }
                        else
                        {
                            isDmaValid = false;
                        }
                    }
                }
                else
                {
                    if (!IsAllDmaAllowed)
                    {
                        isDmaValid = true;
                        searchRequest.IQDmaName = lstDMAString;
                    }
                }


                if (!isDmaValid)
                {
                    throw new Exception();
                }

                if (!IsAllClassAllowed)
                {
                    searchRequest.IQClassNum = listClass.Select(s => s.Num).ToList();
                }

                if (!IsAllStationAllowed)
                {
                    searchRequest.StationAffil = listStation.Select(s => s.Name).ToList();
                }

                if (listRegion != null && listRegion.Count > 0)
                {
                    searchRequest.IncludeRegionsNum = listRegion.ToList();
                }

                if (listCountry != null && listCountry.Count > 0)
                {
                    searchRequest.CountryNums = listCountry.ToList();
                }

                /*if (!string.IsNullOrWhiteSpace(tvMarket))
                {
                    searchRequest.IQDmaName = new List<string>();
                    searchRequest.IQDmaName.Add(tvMarket);
                }*/

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

                //searchRequest.FacetRangeEnds = searchRequest.FacetRangeEnds.Value.ToUniversalTime();

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

                discoverySearchResponse.ListRecordData = new List<LiteRecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {

                    LiteRecordData recorddata = new LiteRecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);

                }

                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<LiteTopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    LiteTopResults topResults = new LiteTopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["title120"]).Replace("[", "").Replace("]", "");
                    topResults.Logo = "../images/MediaIcon/network-icon.png";//ConfigurationManager.AppSettings["StationLogo"] + Convert.ToString(jsonData["response"]["docs"][i]["stationid"]) + ".jpg";
                    topResults.Publisher = "<img src=\"" + ConfigurationManager.AppSettings["StationLogo"] + Convert.ToString(jsonData["response"]["docs"][i]["stationid"]) + ".jpg" + "\" alt=\"\" />";// ;
                    //Convert.ToString(jsonData["response"]["docs"][i]["market"]);

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }

                string totalResultmarket = Convert.ToString(jsonData["facet_counts"]["facet_fields"]["market"]).Replace("\r\n", string.Empty).Replace("[", "").Replace("]", "");
                //string[] facetDatamarket = totalResultmarket.Split(',');

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
            catch (Exception)
            {

                throw;
            }

        }


        public DiscoveryLiteSearchResponse SearchNews(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket, string p_fromRecordID, string pmgSearchUrl, List<Int16> lstOfIQLicense)
        {
            try
            {

                Boolean isError = false;
                //List<DiscoverySearchResponse> lstDiscoverySearchResponse = new List<DiscoverySearchResponse>();
                DiscoveryLiteSearchResponse discoverySearchResponse = new DiscoveryLiteSearchResponse();
                discoverySearchResponse.SearchTerm = searchTerm;
                discoverySearchResponse.MediumType = CommonFunctions.CategoryType.NM.ToString();

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();

                //foreach (string sterm in searchTerm)
                //{

                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchNewsRequest searchNewsRequest = new SearchNewsRequest();

                searchNewsRequest.SearchTerm = searchTerm;
                searchNewsRequest.IsTitleNContentSearch = true;
                searchNewsRequest.Facet = true;
                searchNewsRequest.FacetRangeOther = "all";

                if (lstOfIQLicense != null && lstOfIQLicense.Count > 0)
                {
                    foreach (Int16 iqlicense in lstOfIQLicense)
                    {
                        searchNewsRequest.IQLicense.Add(iqlicense);
                    }
                }

                if (!string.IsNullOrWhiteSpace(medium))
                {
                    searchNewsRequest.PageSize = 4;
                }
                else
                {
                    searchNewsRequest.PageSize = 1;
                }

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

                //searchNewsRequest.FacetRangeEnds = searchNewsRequest.FacetRangeEnds.Value.ToUniversalTime();

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
                {


                }

                if (!string.IsNullOrWhiteSpace(fromRecordID))
                {
                    discoverySearchResponse.FromRecordID = fromRecordID;
                }

                discoverySearchResponse.ListRecordData = new List<LiteRecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {

                    LiteRecordData recorddata = new LiteRecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);

                }


                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<LiteTopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    LiteTopResults topResults = new LiteTopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["title"]);
                    topResults.Logo = "../images/MediaIcon/News.png";                    
                    //Uri aPublisherUri;
                    topResults.Publisher = Convert.ToString(jsonData["response"]["docs"][i]["docurl"]).Replace("\n", "");

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }

                //lstDiscoverySearchResponse.Add(discoverySearchResponse);
                // }
                discoverySearchResponse.TotalResult = Convert.ToInt64(jsonData["response"]["numFound"]);
                return discoverySearchResponse;


            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("Error in News : " + ex.ToString());
                throw;
            }

        }

        public LiteSocialMediaFacet SearchSocialMedia(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket, string p_fromRecordID, string pmgSearchUrl)
        {
            try
            {
                Boolean isError = false;

                LiteSocialMediaFacet socialMediaFacet = new LiteSocialMediaFacet();

                DiscoveryLiteSearchResponse discoverySearchResponse = new DiscoveryLiteSearchResponse();
                discoverySearchResponse.SearchTerm = searchTerm;
                discoverySearchResponse.MediumType = CommonFunctions.CategoryType.SocialMedia.ToString();

                DiscoveryLiteSearchResponse discoverySearchResponseFeedClass = new DiscoveryLiteSearchResponse();
                discoverySearchResponseFeedClass.SearchTerm = searchTerm;
                discoverySearchResponseFeedClass.MediumType = CommonFunctions.CategoryType.SocialMedia.ToString();


                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();

                //foreach (string sterm in searchTerm)
                //{

                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchSMRequest searchSMRequest = new SearchSMRequest();

                searchSMRequest.SearchTerm = searchTerm;
                searchSMRequest.IsTitleNContentSearch = true;
                searchSMRequest.Facet = true;
                searchSMRequest.FacetRangeOther = "all";

                if (!string.IsNullOrWhiteSpace(medium))
                {
                    searchSMRequest.PageSize = 4;
                }
                else
                {
                    searchSMRequest.PageSize = 1;
                }

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

                //searchSMRequest.FacetRangeEnds = searchSMRequest.FacetRangeEnds.Value;

                searchSMRequest.StartDate = searchSMRequest.FacetRangeStarts;
                searchSMRequest.EndDate = searchSMRequest.FacetRangeEnds;

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
                searchSMRequest.FacetField = "iqsubmediatype";
                searchSMRequest.StartDate = searchSMRequest.FacetRangeStarts;
                searchSMRequest.EndDate = searchSMRequest.FacetRangeEnds;
                searchSMRequest.IsTaggingExcluded = true;
                if (!string.IsNullOrWhiteSpace(medium))
                {
                    searchSMRequest.SourceType = new List<string>();

                    if (medium == CommonFunctions.CategoryType.Blog.ToString())
                    {
                        searchSMRequest.SourceType.Add(CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.Blog));
                        //searchSMRequest.SourceType.Add(medium);
                        //searchSMRequest.SourceType.Add("Comment");
                    }
                    else if (medium == CommonFunctions.CategoryType.Forum.ToString())
                    {
                        searchSMRequest.SourceType.Add(CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.Forum));
                        //searchSMRequest.SourceType.Add(medium);
                        //searchSMRequest.SourceType.Add("Review");
                    }
                    else
                    {
                        searchSMRequest.SourceType.Add(CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.SocialMedia));
                    }
                }

                searchSMRequest.wt = ReponseType.json;

                string smReponse = searchEngine.SearchSocialMediaChart(searchSMRequest, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));
                jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(smReponse);

                string totalResult = Convert.ToString(jsonData["facet_counts"]["facet_ranges"]["harvestdate_dt"]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                string[] facetData = totalResult.Split(',');

                discoverySearchResponse.ListRecordData = new List<LiteRecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {

                    LiteRecordData recorddata = new LiteRecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);
                }


                string totalResultFeedClass = Convert.ToString(jsonData["facet_counts"]["facet_fields"]["iqsubmediatype"]).Replace("\r\n", string.Empty);
                string[] facetDataFeedClass = totalResultFeedClass.Split(',');

                string fromRecordID = string.Empty;
                try
                {
                    fromRecordID = Convert.ToString(jsonData["response"]["docs"][0]["iqseqid"]);
                }
                catch (Exception)
                {

                }

                if (!string.IsNullOrWhiteSpace(fromRecordID))
                {
                    discoverySearchResponse.FromRecordID = fromRecordID;
                }

                discoverySearchResponseFeedClass.ListRecordData = new List<LiteRecordData>();

                for (int i = 0; i < facetDataFeedClass.Length; i = i + 2)
                {
                    string feedClass = GetFeedClassFromInt(Convert.ToInt32(facetDataFeedClass[i].Trim().Replace("\"", string.Empty).Replace("[", string.Empty).Trim()));
                    LiteRecordData recorddata = new LiteRecordData();
                    recorddata.FeedClass = feedClass;
                    recorddata.TotalRecord = Convert.ToString(facetDataFeedClass[i + 1].Trim().Replace("\"", string.Empty).Replace("]", string.Empty));
                    if (string.IsNullOrWhiteSpace(medium))
                    {
                        discoverySearchResponseFeedClass.ListRecordData.Add(recorddata);
                    }
                    else if (medium == feedClass)
                    {
                        discoverySearchResponseFeedClass.ListRecordData.Add(recorddata);
                    }
                }

                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<LiteTopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    LiteTopResults topResults = new LiteTopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["title"]);
                    topResults.Logo = "../images/MediaIcon/" + GetFeedClassFromInt(Convert.ToInt32(jsonData["response"]["docs"][i]["iqsubmediatype"])).Replace(" ", "-") + ".png";
                    //Uri aPublisherUri;
                    topResults.Publisher = Convert.ToString(jsonData["response"]["docs"][i]["homeurl_domain"]);

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }

                //lstDiscoverySearchResponseFeedClass.Add(discoverySearchResponseFeedClass);

                //lstDiscoverySearchResponse.Add(discoverySearchResponse);
                //}
                socialMediaFacet.DateData = discoverySearchResponse;
                socialMediaFacet.FeedClassData = discoverySearchResponseFeedClass;
                discoverySearchResponse.TotalResult = Convert.ToInt64(jsonData["response"]["numFound"]);
                return socialMediaFacet;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("error occured ", ex);
                throw;
            }

        }

        public DiscoveryLiteSearchResponse SearchProQuest(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket, string p_fromRecordID, string pmgSearchUrl)
        {
            try
            {
                Boolean isError = false;
                List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse = new List<DiscoveryLiteSearchResponse>();

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();

                DiscoveryLiteSearchResponse discoverySearchResponse = new DiscoveryLiteSearchResponse();
                discoverySearchResponse.SearchTerm = searchTerm;
                discoverySearchResponse.MediumType = CommonFunctions.CategoryType.PQ.ToString();

                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchProQuestRequest searchProQuestRequest = new SearchProQuestRequest();

                searchProQuestRequest.SearchTerm = searchTerm;
                searchProQuestRequest.Facet = true;
                searchProQuestRequest.FacetRangeOther = "all";

                if (!string.IsNullOrWhiteSpace(medium))
                {
                    searchProQuestRequest.PageSize = 4;
                }
                else
                {
                    searchProQuestRequest.PageSize = 1;
                }

                searchProQuestRequest.SortFields = "date-";

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

                discoverySearchResponse.ListRecordData = new List<LiteRecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {
                    LiteRecordData recorddata = new LiteRecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);
                }

                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<LiteTopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    LiteTopResults topResults = new LiteTopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["title"]);
                    topResults.Logo = "../images/MediaIcon/print-media_t.png";
                    topResults.Publisher = Convert.ToString(jsonData["response"]["docs"][i]["publication"]);

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }
                
                discoverySearchResponse.TotalResult = Convert.ToInt64(jsonData["response"]["numFound"]);
                return discoverySearchResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ColumnChart(List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse)
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

        public string LineChart(List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse, Boolean isHourData, decimal p_ClientGmtOffset, decimal p_ClientDstOffset)
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
                        datum.link = "javascript:ChartClick();";
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

        public string HighChartsColumnChart(List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse)
        {
            try
            {
                // generate list of series from discovery response, as search term and total no. of record exist for that search term
                var dataValue = (List<Series>)(from x in
                                                   (
                                                       from sr in lstDiscoverySearchResponse
                                                       from dc in sr.ListRecordData
                                                       select new { sr.SearchTerm, dc }
                                                   )
                                               group x by new { x.SearchTerm } into g
                                               select new Series()
                                               {
                                                   name = g.Key.SearchTerm,
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
                highColumnChartModel.yAxis = new YAxis() { title = new Title2() { text = "" } };

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
                /*XDocument xdoc = new XDocument(new XElement("chart", new XAttribute("yAxisName", ""), new XAttribute("caption", "Average Report"),
                                                  searchTermTotal.Select(s => new XElement("Set", new XAttribute("label", s.SearchTerm), new XAttribute("value", s.TotalRecord)))));*/
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string HighChartsLineChart(List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse, Boolean isHourData, decimal p_ClientGmtOffset, decimal p_ClientDstOffset)
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


                highLineChartOutput.yAxis = new List<YAxis>() { new YAxis(){title = new Title2(), plotLines = plotlines }};



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

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories,
                    labels = new labels()
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
                                click = "ChartClick",
                            }
                        }
                    }
                };


                var distinctSearchTerm = totalRecords.Select(d => d.SearchTerm.Trim()).Distinct().ToList();

                //if (distinctSearchTerm.Count == 1)
                //{
                //    highLineChartOutput.hChart.events = new PlotEvents()
                //    {
                //        load = "SetHoverAuto"
                //    };

                //    highLineChartOutput.plotOption.series.point.events.mouseOver = "ChartHoverManage";
                //    highLineChartOutput.plotOption.series.point.events.mouseOver = "ChartHoverOutManage";
                //}

                // start to set series of data for multiline search term chart 
                List<Series> lstSeries = new List<Series>();

                foreach (string sTerm in distinctSearchTerm)
                {

                    // set sereies name as search term , will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = sTerm.Length > 10 ? sTerm.Substring(0, 10) + "..." : sTerm;


                    var sTermWiseRecord = totalRecords.Where(w => w.SearchTerm.Equals(sTerm)).Select(s => s).ToList();
                    foreach (SearchTermTotalRecords searchTermTotalRecord in sTermWiseRecord)
                    {
                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                            *  SearchTerm = applied searh term value
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = searchTermTotalRecord.TotalRecords;
                        highChartDatum.SearchTerm = sTerm;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);

                }

                // assign set of series data to search term multiline (or multi line searchrequest chart)
                highLineChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Dictionary<string, object> HighChartsPieChartBySearchTerm(List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse, List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponseFeedClass,
                                                            string[] searchTerm, string medium,
                                                            bool p_Isv4TV, bool p_Isv4NM,
                                                            bool p_Isv4SM, bool p_Isv4PQ)
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

                List<SearchTermTotalRecords> totalRecords = new List<SearchTermTotalRecords>();
                if (lstDiscoverySearchResponseFeedClass != null)
                {
                    totalRecords = (List<SearchTermTotalRecords>)(from x in
                                                                      (
                                                                          from sr in lstDiscoverySearchResponseFeedClass
                                                                          from dc in sr.ListRecordData
                                                                          select new { sr.SearchTerm, dc }
                                                                      )
                                                                  group x by new { x.SearchTerm, x.dc.FeedClass } into g
                                                                  select new SearchTermTotalRecords
                                                                  {
                                                                      SearchTerm = g.Key.SearchTerm,
                                                                      FeedClass = g.Key.FeedClass,
                                                                      TotalRecords = g.Sum(g1 => Convert.ToInt64(g1.dc.TotalRecord))
                                                                  }).ToList();
                }

                PSeries pSeries = new PSeries();

                pSeries.type = "pie";
                pSeries.name = "";
                pSeries.data = new List<object>();
                List<PSeries> lstPseries = new List<PSeries>();
                lstPseries.Add(pSeries);

                Dictionary<string, long> dictCounts = new Dictionary<string, long>();
                foreach (string sTerm in searchTerm)
                {
                    dictCounts.Add(sTerm, 0);

                    // Get TV Data
                    if (p_Isv4TV && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.TV.ToString()))
                    {
                        dictCounts[sTerm] += lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.TV.ToString()) && w.SearchTerm.Equals(sTerm)).GroupBy(g => g.SearchTerm)
                                                                                .Select(s =>
                                                                                    Convert.ToInt64(s.Sum(s1 => Convert.ToInt64(s1.TotalResult)))).FirstOrDefault();
                    }

                    // Get NEWS Data
                    if (p_Isv4NM && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.NM.ToString()))
                    {
                        dictCounts[sTerm] += lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).GroupBy(g => g.SearchTerm)
                                                                                .Select(s =>
                                                                                    Convert.ToInt64(s.Sum(s1 => Convert.ToInt64(s1.TotalResult)))).FirstOrDefault();
                    }

                    // Get Social Media Data
                    if (p_Isv4SM && (string.IsNullOrWhiteSpace(medium) || medium == "Social Media" ||
                            medium == CommonFunctions.CategoryType.Blog.ToString() ||
                        medium == CommonFunctions.CategoryType.Forum.ToString()))
                    {
                        if (totalRecords != null && totalRecords.Count > 0)
                        {
                            dictCounts[sTerm] += totalRecords.Where(w => w.SearchTerm.Equals(sTerm)).GroupBy(g => g.SearchTerm)
                                                                    .Select(s =>
                                                                        Convert.ToInt64(s.Sum(s1 => Convert.ToInt64(s1.TotalRecords)))).FirstOrDefault();
                        }
                    }

                    // Get ProQuest Data
                    if (p_Isv4PQ && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.PQ.ToString()))
                    {
                        dictCounts[sTerm] += lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.PQ.ToString()) && String.Compare(w.SearchTerm, sTerm, true) == 0).GroupBy(g => g.SearchTerm)
                                                                                .Select(s =>
                                                                                   Convert.ToInt64(s.Sum(s1 => Convert.ToInt64(s1.TotalResult)))).FirstOrDefault();
                    }
                }

                List<PieChartTotal> lstPieChartTotals = new List<PieChartTotal>();
                lstPieChartTotals.AddRange(lstDiscoverySearchResponse.Where(w => !w.MediumType.Equals(CommonFunctions.CategoryType.SocialMedia.ToString()))
                                                                                .Select(s =>
                                                                                    new PieChartTotal() { searchTerm = s.SearchTerm, medium = s.MediumType, totalResult = s.TotalResult }).ToList());
                lstPieChartTotals.AddRange(totalRecords.Select(s => new PieChartTotal() { searchTerm = s.SearchTerm, medium = (s.FeedClass == "Social Media" ? "SocialMedia" : s.FeedClass), totalResult = s.TotalRecords }).ToList());

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

        public List<PieChartResponse> HighChartsPieChartByMedium(List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse, List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponseFeedClass,
                                                string[] searchTerm, string medium,
                                                bool p_Isv4TV, bool p_Isv4NM,
                                                bool p_Isv4SM, bool p_Isv4PQ)
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

                List<SearchTermTotalRecords> totalRecords = new List<SearchTermTotalRecords>();
                if (lstDiscoverySearchResponseFeedClass != null)
                {
                    totalRecords = (List<SearchTermTotalRecords>)(from x in
                                                                      (
                                                                          from sr in lstDiscoverySearchResponseFeedClass
                                                                          from dc in sr.ListRecordData
                                                                          select new { sr.SearchTerm, dc }
                                                                      )
                                                                  group x by new { x.SearchTerm, x.dc.FeedClass } into g
                                                                  select new SearchTermTotalRecords
                                                                  {
                                                                      SearchTerm = g.Key.SearchTerm,
                                                                      FeedClass = g.Key.FeedClass,
                                                                      TotalRecords = g.Sum(g1 => Convert.ToInt64(g1.dc.TotalRecord))
                                                                  }).ToList();
                }


                //Get Distinct Search Term and By Looping through it , Get Data of Feedclass and its total

                //var distinctSearchTerm = totalRecords.Select(s => s.SearchTerm).Distinct().ToList();

                PSeries pSeries = new PSeries();

                pSeries.type = "pie";
                pSeries.name = "";
                //pSeries.data = new List<List<PSeriesData>>();
                pSeries.data = new List<object>();
                List<Object> lstObject = new List<object>();
                List<PSeries> lstPseries = new List<PSeries>();
                lstPseries.Add(pSeries);

                Dictionary<string, double> dictPie = new Dictionary<string, double>();


                // set pie chart series for each search term 
                foreach (string sTerm in searchTerm)
                {

                    // create series with each medium type and its total records. 
                    lstObject = new List<object>();
                    pSeries.data = new List<object>();
                    //List<List<PSeriesData>> lstoflstPSeriesData = new List<List<PSeriesData>>();
                    // Get TV Data
                    if (p_Isv4TV && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.TV.ToString()))
                    {


                        lstObject.Add(new object[] { CommonFunctions.CategoryType.TV.ToString(), lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.TV.ToString()) && w.SearchTerm.Equals(sTerm)).GroupBy(g => g.MediumType)
                                                                                .Select(s =>
                                                                                    Convert.ToInt64(s.Sum(s1 => Convert.ToInt64(s1.TotalResult)))).FirstOrDefault()});
                    }

                    // Get NEWS Data
                    if (p_Isv4NM && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.NM.ToString()))
                    {

                        lstObject.Add(new object[] { CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.NM) ,
                        
                            lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).GroupBy(g => g.MediumType)
                                                                                .Select(s =>
                                                                                    Convert.ToInt64(s.Sum(s1 => Convert.ToInt64(s1.TotalResult)))).FirstOrDefault()});
                    }

                    // Get Social Media Data
                    if (p_Isv4SM && (string.IsNullOrWhiteSpace(medium) || medium == "Social Media" ||
                            medium == CommonFunctions.CategoryType.Blog.ToString() ||
                        medium == CommonFunctions.CategoryType.Forum.ToString()))
                    {
                        if (totalRecords == null || totalRecords.Count <= 0)
                        {
                            lstObject.Add(new object[] { "Social Media", 0 });
                            lstObject.Add(new object[] { "Blog", 0 });
                            lstObject.Add(new object[] { "Forum", 0 });

                        }
                        else
                        {
                            lstObject.AddRange(totalRecords.Where(w => w.SearchTerm.Equals(sTerm)).Select(s => new object[]
                            {
                                s.FeedClass,s.TotalRecords
                            }).ToList());
                        }
                    }

                    // Get ProQuest Data
                    if (p_Isv4PQ && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.PQ.ToString()))
                    {
                        lstObject.Add(new object[]{CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.PQ),
                            lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.PQ.ToString()) && String.Compare(w.SearchTerm, sTerm, true) == 0).GroupBy(g => g.MediumType)
                                                                                .Select(s =>
                                                                                   Convert.ToInt64(s.Sum(s1 => Convert.ToInt64(s1.TotalResult)))).FirstOrDefault()});
                    }


                    pSeries.data = lstObject;
                    // set series for selected search term
                    highPieChartModel.series = lstPseries;
                    string jsonResult = CommonFunctions.SearializeJson(highPieChartModel);

                    // add peichart to list of piechart response
                    PieChartResponse pieChartResponse = new PieChartResponse();
                    pieChartResponse.JsonResult = jsonResult;
                    pieChartResponse.SearchTerm = sTerm;
                    /*pieChartReponse.SearchTerm = sTerm;
                    pieChartReponse.JsonResult = jsonResult;*/
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
            bool isDefinedResult = Enum.IsDefined(typeof(CommonFunctions.CategoryType), iqSubMediaType);
            if (isDefinedResult)
            {
                string feedClass = ((CommonFunctions.CategoryType)iqSubMediaType).ToString();
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

        public IEnumerable GetMediumFilter(List<DiscoveryLiteSearchResponse> lstDiscoverySearchResponse, List<DiscoveryLiteSearchResponse> lstDiscoveryFeedClass)
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
                                          && x.MediumType != "SocialMedia"
                                          group x by new { x.TotalRecord, x.MediumType } into g
                                          select new
                                          {
                                              //Medium = CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(g.Key.MediumType)),
                                              Medium = g.Key.MediumType
                                          }).Distinct().ToList();


                if (lstDiscoveryFeedClass != null && lstDiscoveryFeedClass.Count > 0 && lstDiscoveryFeedClass.FirstOrDefault().ListRecordData.Count > 0)
                {
                    var distinctFeedClass = (from x in
                                                 (
                                                     from sr in lstDiscoveryFeedClass
                                                     from dc in sr.ListRecordData
                                                     select new { dc.TotalRecord, dc.FeedClass }

                                                 )
                                             where Convert.ToInt64(x.TotalRecord) > 0
                                             group x by new { x.TotalRecord, x.FeedClass } into g
                                             select new
                                             {
                                                 //Medium = GetFeedClass(g.Key.FeedClass),
                                                 Medium = GetFeedClass(g.Key.FeedClass)
                                             }).Distinct().ToList();

                    distinctDateFilter.AddRange(distinctFeedClass);
                }
                //distinctDateFilter = distinctDateFilter.Distinct().ToList();
                /* TODO: Update to use MediaTypeFilterData
                List<MediumFilterData> lstMediumFilterData = distinctDateFilter.Select(s => new MediumFilterData()
                {
                    Medium = s.Medium,
                    MediumValue = CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(s.Medium.Replace(" ", string.Empty))),
                }).ToList();
                 */
                return new List<MediaTypeFilterData>();// lstMediumFilterData;
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
}
