using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Data;
using IQMedia.Model;
using IQMedia.Logic.Base;
using PMGSearch;
using System.Configuration;

namespace IQMedia.Web.Logic
{
    public class TWLogic : ILogic
    {

        public string InsertArchiveTW(IQAgent_TwitterResultsModel p_IQAgent_TwitterResultsModel, Guid p_CustomerGUID, Guid p_ClientGUID, Guid p_CategoryGUID, string p_Keywords, string p_Description, string p_MediaType, string p_SubMediaType, Int64? MediaID = null)
        {
            TWDA twDA = (TWDA)DataAccessFactory.GetDataAccess(DataAccessType.TW);
            string result = twDA.InsertArchiveTW(p_IQAgent_TwitterResultsModel, p_CustomerGUID, p_ClientGUID, p_CategoryGUID, p_Keywords, p_Description, p_MediaType, p_SubMediaType, MediaID);
            return result;
        }

        public IQAgent_TwitterResultsModel SearchTwitterByTweetID(string tweetID, string pmgurl, string searchTem = "", IQClient_ThresholdValueModel iQClient_ThresholdValueModel = null)
        {
            try
            {
                bool isError = false;
                System.Uri PMGSearchRequestUrl = new Uri(pmgurl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchTwitterRequest searchTwitterRequest = new SearchTwitterRequest();
                
                searchTwitterRequest.IDs = new List<long>();
                searchTwitterRequest.IDs.Add(Convert.ToInt64(tweetID));
                searchTwitterRequest.Facet = false;
                searchTwitterRequest.IsSentiment = true;
                searchTwitterRequest.IsHighlighting = true;
                searchTwitterRequest.IsDeleted = false;

                if (!string.IsNullOrEmpty(searchTem))
                {
                    searchTwitterRequest.SearchTerm = searchTem;
                }

                if (iQClient_ThresholdValueModel != null)
                {
                    searchTwitterRequest.HighThreshold = iQClient_ThresholdValueModel.TwitterHighThreshold;
                    searchTwitterRequest.LowThreshold = iQClient_ThresholdValueModel.TwitterLowThreshold;
                }

                SearchTwitterResult searchTwitterResult = searchEngine.SearchTwitter(searchTwitterRequest, false, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));


                IQAgent_TwitterResultsModel iqAgent_TwitterResultsModel = new IQAgent_TwitterResultsModel();
                foreach (TwitterResult twitterResult in searchTwitterResult.TwitterResults)
                {
                    iqAgent_TwitterResultsModel.Actor_PreferredName = twitterResult.actor_prefferedUserName;
                    iqAgent_TwitterResultsModel.Summary = twitterResult.tweet_body;
                    iqAgent_TwitterResultsModel.Actor_FollowersCount = twitterResult.followers_count;
                    iqAgent_TwitterResultsModel.Actor_FriendsCount = twitterResult.friends_count;
                    iqAgent_TwitterResultsModel.Actor_Image = twitterResult.actor_image;
                    iqAgent_TwitterResultsModel.Actor_Link = twitterResult.actor_link;
                    iqAgent_TwitterResultsModel.KlOutScore = twitterResult.Klout_score;
                    iqAgent_TwitterResultsModel.Tweet_DateTime = Convert.ToDateTime(twitterResult.tweet_postedDateTime);
                    iqAgent_TwitterResultsModel.TweetID = twitterResult.iqseqid.ToString();
                    iqAgent_TwitterResultsModel.Actor_DisplayName = twitterResult.actor_displayName;
                    iqAgent_TwitterResultsModel.HighlightedOutput = new HighlightedTWOutput()
                    {
                        Highlights = twitterResult.Highlight 
                    };
                    iqAgent_TwitterResultsModel.SearchTerm = searchTem;
                    iqAgent_TwitterResultsModel.Number_Hits = 1;

                    if (twitterResult.Sentiments != null)
                    {
                        iqAgent_TwitterResultsModel.PositiveSentiment = twitterResult.Sentiments.PositiveSentiment != null ? twitterResult.Sentiments.PositiveSentiment : 0;
                        iqAgent_TwitterResultsModel.NegativeSentiment = twitterResult.Sentiments.NegativeSentiment != null ? twitterResult.Sentiments.NegativeSentiment : 0;
                    }

                }
                return iqAgent_TwitterResultsModel;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
