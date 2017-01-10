using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Logic.Base;
using IQMedia.Data;
using IQMedia.Model;
using IQRadioSearch;
using System.Configuration;

namespace IQMedia.Web.Logic
{
    public class RadioLogic : ILogic
    {
        public List<RadioStation> SelectRadioStations()
        {
            RadioDA radioDA = (RadioDA)DataAccessFactory.GetDataAccess(DataAccessType.Radio);
            return radioDA.SelectRadioStations();
        }
        public Dictionary<string, object> SelectRadioStationFilters()
        {
            RadioDA radioDA = (RadioDA)DataAccessFactory.GetDataAccess(DataAccessType.Radio);
            return radioDA.SelectRadioStationFilters();
        }
        public Dictionary<string, object> SelectRadioResults(DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_DMAList, List<string> p_StationList, string p_SolrURL, int? p_FragOffset, int? p_FragSize, bool p_IsHighlighting, bool p_IsInitialSearch, bool p_IsLogging, string p_LogFileLocation, bool p_IsShowCC, string p_SearchTerm, string p_SolrFL, bool p_IsAsc, int p_PageNo, int p_PageSize, ref long p_SinceID, out long p_TotalResults)
        {
            Dictionary<string, object> returnDictionary = new Dictionary<string, object>();
            p_TotalResults = 0;

            Uri solrRequestURL = new Uri(p_SolrURL);
            SearchEngine se = new SearchEngine(solrRequestURL);

            SearchRequest sr = new SearchRequest();

            if (p_DMAList != null && p_DMAList.Count > 0)
            {
                sr.DMAList = p_DMAList;
            }

            if (p_FragOffset != null)
            {
                sr.FragOffset = p_FragOffset.Value;
            }

            if (p_FragSize != null)
            {
                sr.FragSize = p_FragSize.Value;
            }

            if (p_FromDate != null)
            {
                sr.FromDate = p_FromDate.Value;
            }

            if (p_ToDate != null)
            {
                sr.ToDate = p_ToDate.Value;
            }

            sr.FromRecordID = p_PageNo * p_PageSize;

            sr.IsHighlighting = p_IsHighlighting;

            sr.IsInitialSearch = p_IsInitialSearch;

            sr.IsLogging = p_IsLogging;

            sr.IsShowCC = p_IsShowCC;

            sr.IsSortAsc = p_IsAsc;

            sr.SearchTerm = p_SearchTerm;

            sr.LogFileLocation = p_LogFileLocation;

            sr.PageSize = p_PageSize;

            sr.SinceID = p_SinceID;

            sr.SortBy = SortBy.Date;

            if (p_StationList != null && p_StationList.Count > 0)
            {
                sr.StationIDList = p_StationList;
            }

            Dictionary<string, object> searchEngineSearch = se.Search(sr, CustomSolrFl: p_SolrFL);
            SearchResult sresult = (SearchResult)searchEngineSearch["SearchResult"];
            Dictionary<string, Dictionary<string, string>> facetResults = (Dictionary<string, Dictionary<string, string>>)searchEngineSearch["Facet"];
            TadsFilterModel radioFilter = mapFacets(facetResults);

            List<RadioModel> radioResultList = new List<RadioModel>();

            if (sresult.Status == 0 && sresult.TotalHitCount > 0)
            {
                p_SinceID = sresult.MaxSinceID;
                p_TotalResults = sresult.TotalHitCount;

                foreach (var item in sresult.Hits)
                {
                    RadioModel rm = new RadioModel();

                    rm.IQ_Station_ID = item.StationID;
                    rm.Market = item.Market;
                    rm.RL_GUID = Convert.ToString(item.GUID);
                    rm.RL_GUIDSKey = item.ID;
                    rm.RL_StationDateTime = item.DateTime;
                    rm.TimeZone = item.TimeZone;
                    rm.Mentions = item.TotalNoOfOccurrence;

                    radioResultList.Add(rm);
                }
            }

            returnDictionary.Add("RadioList", radioResultList);
            returnDictionary.Add("Facets", radioFilter);
            return returnDictionary;
        }
        public TadsFilterModel mapFacets(Dictionary<string, Dictionary<string, string>> facets)
        {
            TadsFilterModel model = new TadsFilterModel();
            List<TadsDma> radioDmaList = new List<TadsDma>();
            List<TadsStation> radioStationList = new List<TadsStation>();

            foreach (var hit in facets)
            {
                switch (hit.Key)
                {
                    case "RadioMarketFacet":
                        foreach (var element in hit.Value)
                        {
                            TadsDma tadsDma = new TadsDma();
                            tadsDma.ID = element.Key;
                            tadsDma.Counts = Convert.ToInt32(element.Value);
                            radioDmaList.Add(tadsDma);
                        }
                        break;
                    case "RadioStationFacet":
                        foreach (var element in hit.Value)
                        {
                            TadsStation tadsStation = new TadsStation();
                            tadsStation.Name = "";
                            tadsStation.ID = element.Key;
                            tadsStation.Counts = Convert.ToInt32(element.Value);
                            radioStationList.Add(tadsStation);
                        }
                        break;
                }
            }

            model.RadioMarket = radioDmaList;
            model.RadioStation = radioStationList;

            return model;
        }
        public static string GetRawMediaCaption(string searchTerm, Guid rawMediaID, int? p_FragOffset, int? p_FragSize, bool p_IsLogging, string p_LogFileLocation, string p_SolrFl, out Int32? offset, out string fullCaption, string pmgurl, out List<int> lstSearchTermHits)
        {
            offset = null;
            fullCaption = string.Empty;
            StringBuilder strngCaption = new StringBuilder();
            StringBuilder strngFullCaption = new StringBuilder();
            //if (!string.IsNullOrWhiteSpace(searchTerm))
            //{
            Uri solrURL = new Uri(pmgurl);
            SearchEngine se = new SearchEngine(solrURL);

            SearchRequest sr = new SearchRequest();
            sr.GUIDList = new List<Guid> { rawMediaID };
            sr.SearchTerm = searchTerm.Trim();
            sr.IsHighlighting = true;
            sr.IsShowCC = true;

            if (p_FragOffset != null)
            {
                sr.FragOffset = p_FragOffset.Value;
            }

            if (p_FragSize != null)
            {
                sr.FragSize = p_FragSize.Value;
            }

            sr.IsLogging = p_IsLogging;

            sr.LogFileLocation = p_LogFileLocation;

            sr.PageSize = 1;

            Dictionary<string, object> searchEngingSearch = se.Search(sr, CustomSolrFl: p_SolrFl);
            SearchResult sresult = (SearchResult)searchEngingSearch["SearchResult"];

            if (sresult.Status == 0 && sresult.TotalHitCount > 0)
            {
                if (sresult.Hits[0].ClosedCaptions != null && sresult.Hits[0].ClosedCaptions.Count > 0)
                {
                    foreach (TermOccurrence _TermOccurrence in sresult.Hits[0].ClosedCaptions)
                    {
                        strngFullCaption.Append("<div onclick=\"setSeekPoint(" + (_TermOccurrence.TimeOffset - Convert.ToInt32(ConfigurationManager.AppSettings["RawMediaCaptionDelay"].ToString()) > 0 ? _TermOccurrence.TimeOffset - Convert.ToInt32(ConfigurationManager.AppSettings["RawMediaCaptionDelay"].ToString()) : 0) + ");\">"
                                                + _TermOccurrence.SurroundingText + "</div>");
                    }

                    fullCaption = strngFullCaption.ToString();
                }

                if (sresult.Hits[0].TermOccurrences != null && sresult.Hits[0].TermOccurrences.Count > 0)
                {
                    sresult.Hits[0].TermOccurrences = sresult.Hits[0].TermOccurrences.OrderBy(o => o.TimeOffset).ToList();
                    foreach (TermOccurrence _TermOccurrence in sresult.Hits[0].TermOccurrences)
                    {
                        if (offset == null)
                        {
                            offset = _TermOccurrence.TimeOffset;
                        }
                        strngCaption.Append("<div class=\"hit\" onclick=\"setSeekPoint(" + (_TermOccurrence.TimeOffset - Convert.ToInt32(ConfigurationManager.AppSettings["RawMediaCaptionDelay"].ToString())) + ");\">"
                                                + "<div class=\"boldgray\">" + UtilityLogic.formatOffset(_TermOccurrence.TimeOffset) + "</div>"
                                                + "<div class=\"caption\">" + _TermOccurrence.SurroundingText + "</div>"
                                            + "</div>");
                    }
                }
            }

            if (sresult.Status == 0 && sresult.TotalHitCount > 0 && sresult.Hits[0].TermOccurrences != null && sresult.Hits[0].TermOccurrences.Count > 0)
            {
                lstSearchTermHits = sresult.Hits[0].TermOccurrences.Select(x => x.TimeOffset).ToList();
            }
            else lstSearchTermHits = new List<int>();

            return Convert.ToString(strngCaption);
        }

        #region Download

        public int SelectDownloadLimit(string CustomerGUID)
        {
            RadioDA radioDA = (RadioDA)DataAccessFactory.GetDataAccess(DataAccessType.Radio);
            return radioDA.SelectDownloadLimit(CustomerGUID);
        }

        public string Insert_ArticleRadioDownload(string CustomerGUID, long ID)
        {
            RadioDA radioDA = (RadioDA)DataAccessFactory.GetDataAccess(DataAccessType.Radio);
            return radioDA.Insert_ArticleRadioDownload(CustomerGUID, ID);
        }

        public string Update_ArticleRadioDownload(long ClipdownloadKey, string FileLocation, string FileExtension, int DownloadStatus, Guid CustomerGuid)
        {
            RadioDA radioDA = (RadioDA)DataAccessFactory.GetDataAccess(DataAccessType.Radio);
            return radioDA.Update_ArticleRadioDownload(ClipdownloadKey, FileLocation, FileExtension, DownloadStatus, CustomerGuid);
        }

        public string Delete_ArticleRadioDownload(string CustomerGUID, long ClipdownloadKey)
        {
            RadioDA radioDA = (RadioDA)DataAccessFactory.GetDataAccess(DataAccessType.Radio);
            return radioDA.Delete_ArticleRadioDownload(CustomerGUID, ClipdownloadKey);
        }

        public List<ArticleRadioDownload> SelectArticleRadioDownloadByCustomer(string CustomerGUID)
        {
            RadioDA radioDA = (RadioDA)DataAccessFactory.GetDataAccess(DataAccessType.Radio);
            return radioDA.SelectArticleRadioDownloadByCustomer(CustomerGUID);
        }

        public ArticleRadioDownload SelectArticleRadioDownloadByID(long ClipDownloadKey, Guid CustomerGuid)
        {
            RadioDA radioDA = (RadioDA)DataAccessFactory.GetDataAccess(DataAccessType.Radio);
            return radioDA.SelectArticleRadioDownloadByID(ClipDownloadKey, CustomerGuid);
        }

        public string SelectClipLocationFromIQCore_Meta(long ID, Guid CustomerGuid)
        {
            RadioDA radioDA = (RadioDA)DataAccessFactory.GetDataAccess(DataAccessType.Radio);
            return radioDA.SelectClipLocationFromIQCore_Meta(ID, CustomerGuid);
        }

        #endregion
    }
}

