using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using FeedsSearch;
using System.Configuration;

namespace IQMedia.Web.Logic
{
    public class TVEyesLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public int SelectDownloadLimit(Guid CustomerGUID)
        {
            TVEyesDA tvEyesDA = (TVEyesDA)DataAccessFactory.GetDataAccess(DataAccessType.TVEyes);
            return tvEyesDA.SelectDownloadLimit(CustomerGUID);
        }

        public string Insert_ArticleTVEyesDownload(Guid CustomerGUID, long ID)
        {
            TVEyesDA tvEyesDA = (TVEyesDA)DataAccessFactory.GetDataAccess(DataAccessType.TVEyes);
            return tvEyesDA.Insert_ArticleTVEyesDownload(CustomerGUID, ID);
        }

        public List<ArticleTVEyesDownload> SelectArticleTVEyesDownloadByCustomer(Guid CustomerGUID)
        {
            TVEyesDA tvEyesDA = (TVEyesDA)DataAccessFactory.GetDataAccess(DataAccessType.TVEyes);
            return tvEyesDA.SelectArticleTVEyesDownloadByCustomer(CustomerGUID);
        }

        public ArticleTVEyesDownload SelectArticleTVEyesByID(long ID, Guid CustomerGuid)
        {
            TVEyesDA tvEyesDA = (TVEyesDA)DataAccessFactory.GetDataAccess(DataAccessType.TVEyes);
            return tvEyesDA.SelectArticleTVEyesByID(ID,CustomerGuid);
        }

        public string UpdateDownloadStatusByID(long ID, Guid CustomerGuid)
        {
            TVEyesDA tvEyesDA = (TVEyesDA)DataAccessFactory.GetDataAccess(DataAccessType.TVEyes);
            return tvEyesDA.UpdateDownloadStatusByID(ID, CustomerGuid);
        }

        public string InsertArchiveTVEyes(long mediaID, Guid customerGUID, Guid clientGUID, Guid categoryGUID, IQAgent_TVEyesResultsModel iqAgent_TVEyesResultsModel, string keywords, string description, string p_MediaType, string p_SubMediaType)
        {
            TVEyesDA tvEyesDA = (TVEyesDA)DataAccessFactory.GetDataAccess(DataAccessType.TVEyes);
            return tvEyesDA.InsertArchiveTVEyes(mediaID, customerGUID, clientGUID, categoryGUID, iqAgent_TVEyesResultsModel, keywords, description, p_MediaType, p_SubMediaType);
        }

        public IQAgent_TVEyesResultsModel SearchTVEyesByMediaID(long mediaID, string feedsUrl)
        {
            System.Uri FeedsSearchRequestUrl = new Uri(feedsUrl);
            SearchEngine searchEngine = new SearchEngine(FeedsSearchRequestUrl);
            SearchRequest searchRequest = new SearchRequest();

            searchRequest.MediaIDs = new List<string>() { mediaID.ToString() };
            searchRequest.PageSize = 1;

            Dictionary<string, SearchResult> dictResult = searchEngine.Search(searchRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));

            IQAgent_TVEyesResultsModel iqAgent_TVEyesResultsModel = null;

            if (dictResult.ContainsKey("Results"))
            {
                SearchResult searchResult = dictResult["Results"];
                if (searchResult != null)
                {
                    iqAgent_TVEyesResultsModel = new IQAgent_TVEyesResultsModel();

                    foreach (Hit hit in searchResult.Hits)
                    {
                        iqAgent_TVEyesResultsModel.StationID = hit.StationID;
                        iqAgent_TVEyesResultsModel.Title = hit.Title;
                        iqAgent_TVEyesResultsModel.Market = hit.Market;
                        iqAgent_TVEyesResultsModel.DMARank = hit.DmaID;
                        iqAgent_TVEyesResultsModel.StationIDNum = hit.StationIDNum;
                        iqAgent_TVEyesResultsModel.Duration = hit.Duration;
                        iqAgent_TVEyesResultsModel.HighlightingText = hit.HighlightingText;
                        iqAgent_TVEyesResultsModel.UTCDateTime = hit.MediaDate;
                        iqAgent_TVEyesResultsModel.LocalDateTime = hit.LocalDate;
                        iqAgent_TVEyesResultsModel.TimeZone = hit.TimeZone;
                        iqAgent_TVEyesResultsModel.PositiveSentiment = hit.PositiveSentiment;
                        iqAgent_TVEyesResultsModel.NegativeSentiment = hit.NegativeSentiment;
                    }
                }
            }
            return iqAgent_TVEyesResultsModel;
        }
    }
}
