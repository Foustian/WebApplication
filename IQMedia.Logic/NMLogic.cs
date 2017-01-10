using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using System.Configuration;
using PMGSearch;

namespace IQMedia.Web.Logic
{
    public class NMLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public int SelectDownloadLimit(string CustomerGUID)
        {
            NMDA nmDA = (NMDA)DataAccessFactory.GetDataAccess(DataAccessType.NM);
            int result = nmDA.SelectDownloadLimit(CustomerGUID);
            return result;
        }

        public string Insert_ArticleNMDownload(string CustomerGUID, long ID)
        {
            NMDA nmDA = (NMDA)DataAccessFactory.GetDataAccess(DataAccessType.NM);
            string result = nmDA.Insert_ArticleNMDownload(CustomerGUID, ID);
            return result;
        }

        public List<ArticleNMDownload> SelectArticleNMDownloadByCustomer(string CustomerGUID)
        {
            NMDA nmDA = (NMDA)DataAccessFactory.GetDataAccess(DataAccessType.NM);
            List<ArticleNMDownload> result = nmDA.SelectArticleNMDownloadByCustomer(CustomerGUID);
            return result;
        }

        public ArticleNMDownload SelectArticleNMByID(long ID,Guid CustomerGuid)
        {
            NMDA nmDA = (NMDA)DataAccessFactory.GetDataAccess(DataAccessType.NM);
            ArticleNMDownload result = nmDA.SelectArticleNMByID(ID, CustomerGuid);
            return result;
        }

        public string UpdateDownloadStatusByID(long ID,Guid CustomerGuid)
        {
            NMDA nmDA = (NMDA)DataAccessFactory.GetDataAccess(DataAccessType.NM);
            string result = nmDA.UpdateDownloadStatusByID(ID, CustomerGuid);
            return result;
        }

        public string InsertArchiveNM(IQAgent_NewsResultsModel p_IQAgent_NewsResultsModel, Guid p_CustomerGUID, Guid p_ClientGUID, Guid p_CategoryGUID, string p_Event, string p_Keywords, string p_Description, string p_MediaType, string p_SubMediaType, Int64? MediaID = null, bool UseProminenceMultiplier = false)
        {
            NMDA nmDA = (NMDA)DataAccessFactory.GetDataAccess(DataAccessType.NM);
            string result = nmDA.InsertArchiveNM(p_IQAgent_NewsResultsModel, p_CustomerGUID, p_ClientGUID, p_CategoryGUID, p_Event, p_Keywords, p_Description, p_MediaType, p_SubMediaType, MediaID, UseProminenceMultiplier);
            return result;
        }

        public IQAgent_NewsResultsModel SearchNewsByArticleID(string articleID, string pmgurl, string searchTem = "", IQClient_ThresholdValueModel iQClient_ThresholdValueModel = null)
        {
            System.Uri PMGSearchRequestUrl = new Uri(pmgurl);
            SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
            SearchNewsRequest searchNewsRequest = new SearchNewsRequest();

            searchNewsRequest.IDs = new List<string>();
            searchNewsRequest.IDs.Add(articleID);
            searchNewsRequest.Facet = false;
            searchNewsRequest.IsSentiment = true;
            searchNewsRequest.IsTitleNContentSearch = true;
            searchNewsRequest.IsShowContent = true;
            searchNewsRequest.IsReturnHighlight = true;

            if (!string.IsNullOrEmpty(searchTem))
            {
                searchNewsRequest.SearchTerm = searchTem;
            }

            if (iQClient_ThresholdValueModel != null)
            {
                searchNewsRequest.HighThreshold = iQClient_ThresholdValueModel.NMHighThreshold;
                searchNewsRequest.LowThreshold = iQClient_ThresholdValueModel.NMLowThreshold;
            }


            SearchNewsResults searchNewsResults = searchEngine.SearchNews(searchNewsRequest);

            IQAgent_NewsResultsModel iqAgent_NewsResultsModel = new IQAgent_NewsResultsModel();

            if (searchNewsResults.newsResults != null)
            {
                List<string> highlights;
                foreach (NewsResult newsResult in searchNewsResults.newsResults)
                {
                    // Replace LexisNexis linebreak placeholder text with whitespace
                    if (!String.IsNullOrWhiteSpace(newsResult.Content))
                    {
                        newsResult.Content = newsResult.Content.Replace(ConfigurationManager.AppSettings["LexisNexisLineBreakPlaceholder"], " ");
                    }
                    if (newsResult.Highlights != null && newsResult.Highlights.Count > 0)
                    {
                        highlights = newsResult.Highlights.Select(s => !String.IsNullOrWhiteSpace(s) ? s.Replace(ConfigurationManager.AppSettings["LexisNexisLineBreakPlaceholder"], " ") : s).ToList();
                    }
                    else
                    {
                        highlights = newsResult.Highlights;
                    }

                    Uri aUri;
                    iqAgent_NewsResultsModel.ArticleID = newsResult.IQSeqID;
                    iqAgent_NewsResultsModel.ArticleUri = newsResult.Article;
                    iqAgent_NewsResultsModel.Harvest_Time = Convert.ToDateTime(newsResult.date);
                    iqAgent_NewsResultsModel.HighlightingText = newsResult.Content;
                    iqAgent_NewsResultsModel.Publication = newsResult.publication;
                    iqAgent_NewsResultsModel.CompeteUrl = newsResult.HomeurlDomain;
                    iqAgent_NewsResultsModel.Title = newsResult.Title;
                    iqAgent_NewsResultsModel.IQLicense = newsResult.IQLicense;
                    if (newsResult.Sentiments != null)
                    {
                        iqAgent_NewsResultsModel.PositiveSentiment = newsResult.Sentiments.PositiveSentiment != null ? newsResult.Sentiments.PositiveSentiment : 0;
                        iqAgent_NewsResultsModel.NegativeSentiment = newsResult.Sentiments.NegativeSentiment != null ? newsResult.Sentiments.NegativeSentiment : 0;
                    }

                    iqAgent_NewsResultsModel.HighlightedNewsOutput = new HighlightedNewsOutput()
                    {
                        Highlights = highlights
                    };
                    iqAgent_NewsResultsModel.SearchTerm = searchTem;
                    iqAgent_NewsResultsModel.Number_Hits = newsResult.Mentions;
                    iqAgent_NewsResultsModel.FeedClass = newsResult.MediaType;
                }
            }
            return iqAgent_NewsResultsModel;
        }
    }
}
