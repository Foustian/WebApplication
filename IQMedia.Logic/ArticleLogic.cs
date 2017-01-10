using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMGSearch;
using System.Configuration;
using IQMedia.Model;

namespace IQMedia.Web.Logic
{
    public class ArticleLogic : IQMedia.Web.Logic.Base.ILogic
    {

        public void GetArticleDetailByArticleID(string mediaType, string articleID)
        {
            try
            {

            }
            catch (Exception)
            {
                
                throw;
            }
        }

       /* public void SearchNews(string articleID)
        {
            try
            {
                System.Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["SolrNewsUrl"].ToString());
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchNewsRequest searchNewsRequest = new SearchNewsRequest();

                
                searchNewsRequest.Facet = false;                
                searchNewsRequest.SortFields = "date-";

                searchNewsRequest.IsSentiment = true;
                SearchNewsResults searchNewsResults = searchEngine.SearchNews(searchNewsRequest);

                List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<DiscoveryMediaResult>();

                foreach (NewsResult newsResult in searchNewsResults.newsResults)
                {
                    DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                    discoveryMediaResult.Date = Convert.ToDateTime(newsResult.date);
                    discoveryMediaResult.Title = newsResult.Title;
                    discoveryMediaResult.PositiveSentiment = newsResult.Sentiments.PositiveSentiment;
                    discoveryMediaResult.NegativeSentiment = newsResult.Sentiments.NegativeSentiment;
                    discoveryMediaResult.Body = newsResult.Content;
                    discoveryMediaResult.ArticleURL = newsResult.Article;
                    discoveryMediaResult.Publication = newsResult.publication;
                    discoveryMediaResult.ArticleID = newsResult.ID;

                    discoveryMediaResult.MediumType = CommonFunctions.CategoryType.NM;
                    discoveryMediaResult.SearchTerm = searchTerm;
                    discoveryMediaResult.TotalRecords = searchNewsResults.TotalResults;

                    lstDiscoveryMediaResult.Add(discoveryMediaResult);

                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }*/

        public void SearchSM(string articleID)
        {

        }

        public void SearchTwitter(string articleID)
        {

        }
    }
}
