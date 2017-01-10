using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using PMGSearch;
using IQMedia.Logic.Base;
using IQMedia.Data;

namespace IQMedia.Web.Logic
{
    public class PQLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public string InsertArchivePQ(IQAgent_PQResultsModel iQAgent_PQResultsModel, Guid customerGUID, Guid clientGUID, Guid categoryGUID, string p_Keywords, string p_Description, string p_MediaType, string p_SubMediaType, Int64? mediaID = null)
        {
            PQDA pqDA = (PQDA)DataAccessFactory.GetDataAccess(DataAccessType.PQ);
            return pqDA.InsertArchivePQ(iQAgent_PQResultsModel, customerGUID, clientGUID, categoryGUID, p_Keywords, p_Description, p_MediaType, p_SubMediaType, mediaID);
        }

        public IQAgent_PQResultsModel SearchProQuestByArticleID(string articleID, string pmgurl, string searchTem = "", IQClient_ThresholdValueModel iQClient_ThresholdValueModel = null)
        {
            System.Uri PMGSearchRequestUrl = new Uri(pmgurl);
            SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
            SearchProQuestRequest searchProQuestRequest = new SearchProQuestRequest();

            searchProQuestRequest.IDs = new List<string>();
            searchProQuestRequest.IDs.Add(articleID);
            searchProQuestRequest.Facet = false;
            searchProQuestRequest.IsSentiment = true;
            searchProQuestRequest.IsReturnHighlight = true;

            if (!string.IsNullOrEmpty(searchTem))
            {
                searchProQuestRequest.SearchTerm = searchTem;
            }

            if (iQClient_ThresholdValueModel != null)
            {
                searchProQuestRequest.HighThreshold = iQClient_ThresholdValueModel.PQHighThreshold;
                searchProQuestRequest.LowThreshold = iQClient_ThresholdValueModel.PQLowThreshold;
            }

            bool isError = false;
            SearchProQuestResult searchProQuestResult = searchEngine.SearchProQuest(searchProQuestRequest, false, out isError);

            IQAgent_PQResultsModel iqAgent_PQResultsModel = null;

            if (searchProQuestResult.ProQuestResults != null)
            {
                iqAgent_PQResultsModel = new IQAgent_PQResultsModel();

                foreach (ProQuestResult proQuestResult in searchProQuestResult.ProQuestResults)
                {
                    iqAgent_PQResultsModel.ArticleID = proQuestResult.IQSeqID.ToString();
                    iqAgent_PQResultsModel.AvailableDate = proQuestResult.AvailableDate;
                    iqAgent_PQResultsModel.MediaDate = proQuestResult.MediaDate;
                    iqAgent_PQResultsModel.Content = proQuestResult.Content;
                    iqAgent_PQResultsModel.ContentHTML = proQuestResult.ContentHTML;
                    iqAgent_PQResultsModel.Publication = proQuestResult.Publication;
                    iqAgent_PQResultsModel.Title = proQuestResult.Title;
                    iqAgent_PQResultsModel.Abstract = proQuestResult.Abstract;
                    iqAgent_PQResultsModel.Authors = proQuestResult.Authors;
                    iqAgent_PQResultsModel.LanguageNum = proQuestResult.LanguageNum;
                    iqAgent_PQResultsModel.MediaCategory = proQuestResult.MediaCategory;
                    if (proQuestResult.Sentiments != null)
                    {
                        iqAgent_PQResultsModel.PositiveSentiment = proQuestResult.Sentiments.PositiveSentiment;
                        iqAgent_PQResultsModel.NegativeSentiment = proQuestResult.Sentiments.NegativeSentiment;
                    }
                    iqAgent_PQResultsModel.HighlightedPQOutput = new HighlightedPQOutput()
                    {
                        Highlights = proQuestResult.Highlights
                    };
                    iqAgent_PQResultsModel.SearchTerm = searchTem;
                    iqAgent_PQResultsModel.Number_Hits = proQuestResult.Mentions;
                    iqAgent_PQResultsModel.Copyright = proQuestResult.Copyright;
                }
            }
            return iqAgent_PQResultsModel;
        }

        /// <summary>
        /// Get decrypted AgentResultID for a PQ article
        /// </summary>
        /// <param name="p_AgentResultID">Encrypted AgentResultID</param>
        /// <returns>long</returns>
        public long DecryptPQArticleID(string p_AgentResultID)
        {
            UTF8Encoding enc = new UTF8Encoding();

            int keylength = Convert.ToInt32(p_AgentResultID.Substring(32, 2));

            string encIVStr = p_AgentResultID.Substring(34 + (keylength - 16), 16) + p_AgentResultID.Substring(16, 16) + p_AgentResultID.Substring(50 + (keylength - 16));
            string ivStr = IQMedia.Shared.Utility.CommonFunctions.DecryptAESHex(encIVStr, enc.GetBytes("6A26F02B6D9EB6DD68F85A012BD8322B"), IQMedia.Shared.Utility.CommonFunctions.StringToByteArray("C6DBC2575C2652B01B3F80D27225058D"));


            byte[] key = enc.GetBytes("0B358AB55C5D059DFFDD7028AD9985EB");
            byte[] iv = IQMedia.Shared.Utility.CommonFunctions.StringToByteArray(ivStr);

            var articleID = p_AgentResultID.Substring(0, 16) + p_AgentResultID.Substring(34, keylength - 16);

            return Convert.ToInt64(IQMedia.Shared.Utility.CommonFunctions.DecryptAESHex(articleID, key, iv));          
        }
    }
}
