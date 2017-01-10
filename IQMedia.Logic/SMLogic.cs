using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using PMGSearch;
using System.Configuration;

namespace IQMedia.Web.Logic
{
    public class SMLogic : ILogic
    {
        public string InsertArchiveSM(IQAgent_SMResultsModel p_IQAgent_SMResultsModel, Guid p_CustomerGUID, Guid p_ClientGUID, Guid p_CategoryGUID, string p_Keywords, string p_Description, string p_MediaType, string p_SubMediaType, Int64? MediaID = null, bool UseProminenceMultiplier = false)
        {
            SMDA smDA = (SMDA)DataAccessFactory.GetDataAccess(DataAccessType.SM);
            string result = smDA.InsertArchiveSM(p_IQAgent_SMResultsModel, p_CustomerGUID, p_ClientGUID, p_CategoryGUID, p_Keywords, p_Description, p_MediaType, p_SubMediaType, MediaID, UseProminenceMultiplier);
            return result;
        }

        public IQAgent_SMResultsModel SearchSocialMediaByArticleID(string articleID, string pmgurl, string searchTem = "", IQClient_ThresholdValueModel iQClient_ThresholdValueModel = null)
        {
            try
            {
                System.Uri PMGSearchRequestUrl = new Uri(pmgurl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchSMRequest searchSMRequest = new SearchSMRequest();
                searchSMRequest.Facet = false;
                searchSMRequest.IsSentiment = true;
                searchSMRequest.isShowContent = true;
                searchSMRequest.IsReturnHighlight = true;
                searchSMRequest.IsTitleNContentSearch = true;

                searchSMRequest.ids = new List<string>();
                searchSMRequest.ids.Add(articleID);

                if (!string.IsNullOrEmpty(searchTem))
                {
                    searchSMRequest.SearchTerm = searchTem;
                }

                if (iQClient_ThresholdValueModel != null)
                {
                    searchSMRequest.HighThreshold = iQClient_ThresholdValueModel.SMHighThreshold;
                    searchSMRequest.LowThreshold = iQClient_ThresholdValueModel.SMLowThreshold;
                }

                SearchSMResult searchSMResult = searchEngine.SearchSocialMedia(searchSMRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));

                IQAgent_SMResultsModel iqAgent_SMResultsModel = new IQAgent_SMResultsModel();
                foreach (SMResult smResult in searchSMResult.smResults)
                {
                    Uri aUri;
                    iqAgent_SMResultsModel.Description = smResult.description;
                    iqAgent_SMResultsModel.ArticleID = smResult.IQSeqID;
                    iqAgent_SMResultsModel.ArticleUri = smResult.link;
                    iqAgent_SMResultsModel.ItemHarvestDate = Convert.ToDateTime(smResult.itemHarvestDate_DT);
                    iqAgent_SMResultsModel.HighlightingText = smResult.content;
                    iqAgent_SMResultsModel.HomeLink = smResult.homeLink;
                    iqAgent_SMResultsModel.CompeteURL = smResult.HomeurlDomain;
                    iqAgent_SMResultsModel.SourceCategory = smResult.feedClass;
                    if (smResult.Sentiments != null)
                    {
                        iqAgent_SMResultsModel.PositiveSentiment = smResult.Sentiments.PositiveSentiment != null ? smResult.Sentiments.PositiveSentiment : 0;
                        iqAgent_SMResultsModel.NegativeSentiment = smResult.Sentiments.NegativeSentiment != null ? smResult.Sentiments.NegativeSentiment : 0;
                    }
                    
                    iqAgent_SMResultsModel.HighlightedSMOutput = new HighlightedSMOutput()
                    {
                        Highlights = smResult.Highlights
                    };
                    iqAgent_SMResultsModel.SearchTerm = searchTem;
                    iqAgent_SMResultsModel.Number_Hits = smResult.Mentions;
                    

                }
                return iqAgent_SMResultsModel;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public int SelectDownloadLimit(string CustomerGUID)
        {
            SMDA smDA = (SMDA)DataAccessFactory.GetDataAccess(DataAccessType.SM);
            int result = smDA.SelectDownloadLimit(CustomerGUID);
            return result;
        }

        public string Insert_ArticleSMDownload(string CustomerGUID, long ID)
        {
            SMDA smDA = (SMDA)DataAccessFactory.GetDataAccess(DataAccessType.SM);
            string result = smDA.Insert_ArticleSMDownload(CustomerGUID, ID);
            return result;
        }

        public List<ArticleSMDownload> SelectArticleSMDownloadByCustomer(string CustomerGUID)
        {
            SMDA smDA = (SMDA)DataAccessFactory.GetDataAccess(DataAccessType.SM);
            List<ArticleSMDownload> result = smDA.SelectArticleSMDownloadByCustomer(CustomerGUID);
            return result;
        }

        public ArticleSMDownload SelectArticleSMByID(long ID,Guid CustomerGuid)
        {
            SMDA smDA = (SMDA)DataAccessFactory.GetDataAccess(DataAccessType.SM);
            ArticleSMDownload result = smDA.SelectArticleSMByID(ID, CustomerGuid);
            return result;
        }

        public string UpdateDownloadStatusByID(long ID, Guid CustomerGuid)
        {
            SMDA smDA = (SMDA)DataAccessFactory.GetDataAccess(DataAccessType.SM);
            string result = smDA.UpdateDownloadStatusByID(ID, CustomerGuid);
            return result;
        }
    }
}
