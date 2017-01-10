using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using PMGSearch;
using System.Xml;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.Model;
using System.Xml.Linq;
using Sentiment;
using Sentiment.HelperClasses;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace IQMedia.WebApplication.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            //string tvstring = GetTVAgentInsertXml();
            //string nmstring = GetNMAgentInsertXml();
            //string smstring = GetSMAgentInsertXml();
            //string twstring = GetTWAgentInsertXml();
            return View();
        }

        /*  
         * To reenable these methods, replace GetIQAgentMediaResults call with SearchMediaResults, to get the data from solr instead of the database
         * 
        public string GetTVAgentInsertXml()
        {
            int PageSize = 5;
            int FragSize = Convert.ToInt32(ConfigurationManager.AppSettings["FragSize"]);
            //TempData["PageNumber"] = null;
            //hasMoreResultPage = false;
            //hasPreviousResultPage = false;


            SearchRequest tvRequest = new SearchRequest();
            tvRequest.IsShowCC = false;
            //tvRequest.IsTitleandCCSearchAllowed = true;
            tvRequest.FragSize = FragSize;
            tvRequest.IsSentiment = true;
            tvRequest.Terms = "music";
            tvRequest.PageSize = PageSize;
            tvRequest.StartDate = new DateTime(2013, 05, 18, 0, 0, 0);
            tvRequest.EndDate = new DateTime(2013, 05, 18, 23, 59, 59);
            tvRequest.PageNumber = 0;

            tvRequest.SortFields = "datetime-";

            Uri PMGSearchRequestUrl = new Uri(WebApplication.Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.TV.ToString(), tvRequest.StartDate, tvRequest.EndDate));
            SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);

            SearchResult tvResult = searchEngine.Search(tvRequest);


            string _Responce = string.Empty;
            XmlDocument _XmlDocument = new XmlDocument();

            _XmlDocument.LoadXml(tvResult.ResponseXml);

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

            XDocument XDoc = new XDocument(new XElement("IQAgentResultsList",
                                                from Hit item in tvResult.Hits
                                                select new XElement("IQAgentResult",
                                                      new XAttribute("SearchRequestID", 137),
                                                      new XAttribute("QueryVersion", 0),
                                                      new XAttribute("Title120", item.Title120),
                                                      new XAttribute("iq_cc_key", item.Iqcckey),
                                                      new XAttribute("RL_VideoGUID", item.Guid),
                                                      new XAttribute("GMTDatetime", item.GmtDateTime),
                                                      new XAttribute("Rl_Station", item.StationId),
                                                      new XAttribute("Rl_Market", item.Market),
                                                      new XAttribute("RL_Date", item.Timestamp.Date),
                                                      new XAttribute("RL_Time", item.Hour),
                                                      new XAttribute("Number_Hits", item.ClosedCaption.Count()),
                                                      new XAttribute("Nielsen_Audience", item.AUDIENCE == null ? "0" : item.AUDIENCE),
                                                      new XAttribute("IQSeqID", item.SeqID),
                                                      new XAttribute("IQAdShareValue", item.SQAD_SHAREVALUE == null ? "0" : item.SQAD_SHAREVALUE),
                                                      new XAttribute("Nielsen_Result", 0),
                                                      new XElement("HighlightedCCOutput", string.Empty),
                                                      new XElement("Sentiment", new XElement("PositiveSentiment", item.Sentiments.PositiveSentiment), new XElement("NegativeSentiment", item.Sentiments.NegativeSentiment)),
                                                      new XAttribute("_IQDmaID", "21")
                                                    )));




            SessionInformation sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

            long? sinceID = null;
            Int64? totalResults = 0, totalResultsDisplay = 0;

            IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
            Dictionary<string, object> dictResult = iQAgentLogic.GetIQAgentMediaResults(sessionInformation.ClientGUID,
                                                        Request.ServerVariables["HTTP_HOST"],
                                                        new List<string> { "75" },
                                                        50,
                                                        new DateTime(2013, 03, 15, 0, 0, 0),
                                                        new DateTime(2013, 03, 15, 0, 0, 0),
                                                        0,
                //Convert.ToInt64(TempData["FromRecordID"]),
                                                        "TV",
                                                        null,
                                                        null,
                                                        false,
                                                        ref sinceID,
                                                        out totalResults, out totalResultsDisplay, false, sessionInformation.Isv4TM, null, null, null, null, null, 0, false, null);


            List<IQAgent_MediaResultsModel> lstIQAgent_MediaResultsModel = (List<IQAgent_MediaResultsModel>)dictResult["Result"];

            XDoc.Element("IQAgentResultsList").Add(from IQAgent_MediaResultsModel item in lstIQAgent_MediaResultsModel
                                                   select new XElement("IQAgentResult",
                                                         new XAttribute("SearchRequestID", 75),
                                                         new XAttribute("QueryVersion", 0),
                                                         new XAttribute("Title120", ((IQAgent_TVResultsModel)item.MediaData).Title120),
                                                         new XAttribute("iq_cc_key", ((IQAgent_TVResultsModel)item.MediaData).IQ_CC_Key == null ? string.Empty : ((IQAgent_TVResultsModel)item.MediaData).IQ_CC_Key),
                                                         new XAttribute("RL_VideoGUID", ((IQAgent_TVResultsModel)item.MediaData).RL_VideoGUID),
                                                         new XAttribute("GMTDatetime", item.MediaDateTime),
                                                         new XAttribute("Rl_Station", ((IQAgent_TVResultsModel)item.MediaData).RL_Station),
                                                         new XAttribute("Rl_Market", ((IQAgent_TVResultsModel)item.MediaData).Market),
                                                         new XAttribute("RL_Date", ((IQAgent_TVResultsModel)item.MediaData).Date),
                                                         new XAttribute("RL_Time", "0"),
                                                         new XAttribute("Number_Hits", ((IQAgent_TVResultsModel)item.MediaData).Hits),
                                                         new XAttribute("Nielsen_Audience", ((IQAgent_TVResultsModel)item.MediaData).Nielsen_Audience == null ? 0 : ((IQAgent_TVResultsModel)item.MediaData).Nielsen_Audience),
                                                         new XAttribute("IQAdShareValue", ((IQAgent_TVResultsModel)item.MediaData).IQAdShareValue == null ? 0 : ((IQAgent_TVResultsModel)item.MediaData).IQAdShareValue),
                                                         new XAttribute("Nielsen_Result", ((IQAgent_TVResultsModel)item.MediaData).Nielsen_Result == null ? string.Empty : ((IQAgent_TVResultsModel)item.MediaData).Nielsen_Result),
                                                         new XElement("HighlightedCCOutput", string.Empty),
                                                         new XElement("Sentiment", new XElement("Sentiment", new XElement("PositiveSentiment", item.PositiveSentiment), new XElement("NegativeSentiment", item.NegativeSentiment))),
                                                         new XAttribute("_IQDmaID", "21")
                                                       ));

            return XDoc.ToString();
        }

        public string GetNMAgentInsertXml()
        {
            int PageSize = 5;
            int FragSize = Convert.ToInt32(ConfigurationManager.AppSettings["FragSize"]);
            //TempData["PageNumber"] = null;
            //hasMoreResultPage = false;
            //hasPreviousResultPage = false;


            SearchNewsRequest newsRequest = new SearchNewsRequest();
            newsRequest.IsReturnHighlight = true;
            newsRequest.FragSize = FragSize;
            newsRequest.IsSentiment = true;
            newsRequest.SearchTerm = "music";
            newsRequest.PageSize = PageSize;
            newsRequest.StartDate = new DateTime(2013, 05, 20, 15, 0, 0);
            newsRequest.EndDate = new DateTime(2013, 05, 20, 16, 59, 59);
            newsRequest.PageNumber = 0;

            newsRequest.SortFields = "datetime-";

            Uri PMGSearchRequestUrl = new Uri(WebApplication.Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.MO.ToString(), newsRequest.StartDate, newsRequest.EndDate));
            SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);

            SearchNewsResults newsResult = searchEngine.SearchNews(newsRequest);


            string _Responce = string.Empty;
            XmlDocument _XmlDocument = new XmlDocument();

            _XmlDocument.LoadXml(newsResult.ResponseXml);

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

            XDocument XDoc = new XDocument(new XElement("IQAgentNMResultsList",
                                                from NewsResult item in newsResult.newsResults
                                                select new XElement("IQAgentNMResult",
                                                      new XAttribute("IQAgentSearchRequestID", 137),
                                                      new XAttribute("QueryVersion", 0),
                                                      new XAttribute("Title", item.Title),
                                                      new XAttribute("harvest_time", item.date),
                                                      new XAttribute("ArticleID", item.IQSeqID),
                                                      new XAttribute("Url", item.Article),
                                                      new XAttribute("Publication", item.publication),
                                                      new XAttribute("Genre", item.Genre),
                                                      new XAttribute("Category", item.Category),
                                                      new XAttribute("IQLicense", item.IQLicense),
                                                      new XAttribute("DMA_Name", item.IQDmaName),
                                                      new XAttribute("Compete_Result", item.IsCompeteAll == true ? "A" : "E"),
                                                      new XAttribute("CompeteURL", item.HomeurlDomain),
                                                      new XAttribute("Number_Hits", item.Mentions),
                                                      new XAttribute("Compete_Audience", 200),
                                                      new XAttribute("IQAdShareValue", 31.54),
                                                      new XElement("HighlightedNewsOutput", string.Empty),
                                                      new XElement("Sentiment", new XElement("PositiveSentiment", item.Sentiments.PositiveSentiment), new XElement("NegativeSentiment", item.Sentiments.NegativeSentiment)),
                                                      new XAttribute("_IQDmaID", "21")
                                                    )));




            SessionInformation sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

            long? sinceID = null;
            Int64? totalResults = 0, totalResultsDisplay = 0;

            IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
            Dictionary<string, object> dictResult = iQAgentLogic.GetIQAgentMediaResults(sessionInformation.ClientGUID,
                                                        Request.ServerVariables["HTTP_HOST"],
                                                        new List<string> { "137" },
                                                        50,
                                                        new DateTime(2013, 02, 18, 0, 0, 0),
                                                        new DateTime(2013, 02, 18, 0, 0, 0),
                                                        0,
                //Convert.ToInt64(TempData["FromRecordID"]),
                                                        "NM",
                                                        null,
                                                        null,
                                                        false,
                                                        ref sinceID,
                                                        out totalResults, out totalResultsDisplay, false, sessionInformation.Isv4TM, null, null, null, null, null, 0, false, null);


            List<IQAgent_MediaResultsModel> lstIQAgent_MediaResultsModel = (List<IQAgent_MediaResultsModel>)dictResult["Result"];

            XDoc.Element("IQAgentNMResultsList").Add(from IQAgent_MediaResultsModel item in lstIQAgent_MediaResultsModel
                                                     select new XElement("IQAgentNMResult",
                                                           new XAttribute("IQAgentSearchRequestID", 137),
                                                           new XAttribute("QueryVersion", 0),
                                                           new XAttribute("Title", ((IQAgent_NewsResultsModel)item.MediaData).Title),
                                                           new XAttribute("harvest_time", ((IQAgent_NewsResultsModel)item.MediaData).Harvest_Time),
                                                           new XAttribute("ArticleID", ((IQAgent_NewsResultsModel)item.MediaData).ArticleID),
                                                           new XAttribute("Url", ((IQAgent_NewsResultsModel)item.MediaData).Url),
                                                           new XAttribute("Publication", ((IQAgent_NewsResultsModel)item.MediaData).Publication),
                                                           new XAttribute("Genre", ""),
                                                           new XAttribute("DMA_Name", ""),
                                                           new XAttribute("Category", ""),
                                                        new XAttribute("IQLicense", ((IQAgent_NewsResultsModel)item.MediaData).IQLicense),
                                                           new XAttribute("Compete_Result", "0"),
                                                           new XAttribute("Number_Hits", ((IQAgent_NewsResultsModel)item.MediaData).Number_Hits),
                                                           new XAttribute("Compete_Audience", ((IQAgent_NewsResultsModel)item.MediaData).Compete_Audience == null ? 0 : ((IQAgent_NewsResultsModel)item.MediaData).Compete_Audience),
                                                           new XAttribute("IQAdShareValue", ((IQAgent_NewsResultsModel)item.MediaData).IQAdShareValue == null ? 0 : ((IQAgent_TVResultsModel)item.MediaData).IQAdShareValue),
                                                           new XAttribute("Nielsen_Result", ((IQAgent_NewsResultsModel)item.MediaData).Compete_Result == null ? string.Empty : ((IQAgent_NewsResultsModel)item.MediaData).Compete_Result),
                                                           new XElement("HighlightedNewsOutput", string.Empty),
                                                           new XElement("Sentiment", new XElement("Sentiment", new XElement("PositiveSentiment", item.PositiveSentiment), new XElement("NegativeSentiment", item.NegativeSentiment))),
                                                           new XAttribute("_IQDmaID", "21")
                                                         ));

            return XDoc.ToString();
        }

        public string GetSMAgentInsertXml()
        {
            int PageSize = 5;
            int FragSize = Convert.ToInt32(ConfigurationManager.AppSettings["FragSize"]);
            //TempData["PageNumber"] = null;
            //hasMoreResultPage = false;
            //hasPreviousResultPage = false;


            SearchSMRequest smRequest = new SearchSMRequest();
            smRequest.IsReturnHighlight = true;
            smRequest.FragSize = FragSize;
            smRequest.IsSentiment = true;
            smRequest.SearchTerm = "music";
            smRequest.PageSize = PageSize;
            smRequest.StartDate = new DateTime(2013, 05, 25, 23, 0, 0);
            smRequest.EndDate = new DateTime(2013, 05, 25, 23, 59, 59);
            smRequest.PageNumber = 0;

            smRequest.SortFields = "datetime-";

            Uri PMGSearchRequestUrl = new Uri(WebApplication.Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.MO.ToString(), smRequest.StartDate, smRequest.EndDate));
            SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);

            SearchSMResult smResult = searchEngine.SearchSocialMedia(smRequest);


            string _Responce = string.Empty;
            XmlDocument _XmlDocument = new XmlDocument();

            _XmlDocument.LoadXml(smResult.ResponseXml);

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

            XDocument XDoc = new XDocument(new XElement("IQAgentSMResultsList",
                                                from SMResult item in smResult.smResults
                                                select new XElement("IQAgentSMResult",
                                                      new XAttribute("IQAgentSearchRequestID", 137),
                                                      new XAttribute("QueryVersion", 0),
                                                      new XAttribute("description", item.description),
                                                      new XAttribute("itemHarvestDate_DT", item.itemHarvestDate_DT),
                                                      new XAttribute("SeqID", item.IQSeqID),
                                                      new XAttribute("link", item.link),
                                                      new XAttribute("homelink", item.homeLink),
                                                      new XAttribute("feedClass", item.feedClass),
                                                      new XAttribute("CompeteURL", item.HomeurlDomain),
                                                      new XAttribute("Compete_Result", item.IsCompeteAll == true ? "A" : "E"),
                                                      new XAttribute("Number_Hits", item.Mentions),
                                                      new XAttribute("Compete_Audience", 200),
                                                      new XAttribute("IQAdShareValue", 31.54),
                                                      new XElement("HighlightedSMOutput", string.Empty),
                                                      new XElement("Sentiment", new XElement("PositiveSentiment", item.Sentiments.PositiveSentiment), new XElement("NegativeSentiment", item.Sentiments.NegativeSentiment)),
                                                      new XAttribute("_IQDmaID", "21")
                                                    )));




            SessionInformation sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

            long? sinceID = null;
            Int64? totalResults = 0, totalResultsDisplay = 0;

            IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
            Dictionary<string, object> dictResult = iQAgentLogic.GetIQAgentMediaResults(sessionInformation.ClientGUID,
                                                        Request.ServerVariables["HTTP_HOST"],
                                                        new List<string> { "137" },
                                                        50,
                                                        new DateTime(2013, 02, 18, 0, 0, 0),
                                                        new DateTime(2013, 02, 18, 0, 0, 0),
                                                        0,
                //Convert.ToInt64(TempData["FromRecordID"]),
                                                        "NM",
                                                        null,
                                                        null,
                                                        false,
                                                        ref sinceID,
                                                        out totalResults, out totalResultsDisplay, false, sessionInformation.Isv4TM, null, null, null, null, null, 0, false, null);


            List<IQAgent_MediaResultsModel> lstIQAgent_MediaResultsModel = (List<IQAgent_MediaResultsModel>)dictResult["Result"];

            XDoc.Element("IQAgentSMResultsList").Add(from IQAgent_MediaResultsModel item in lstIQAgent_MediaResultsModel
                                                     select new XElement("IQAgentSMResult",
                                                           new XAttribute("IQAgentSearchRequestID", 137),
                                                           new XAttribute("QueryVersion", 0),
                                                           new XAttribute("description", ((IQAgent_SMResultsModel)item.MediaData).Description),
                                                           new XAttribute("itemHarvestDate_DT", item.MediaDateTime),
                                                           new XAttribute("SeqID", item.ArticleID),
                                                           new XAttribute("link", ((IQAgent_SMResultsModel)item.MediaData).Link),
                                                           new XAttribute("homelink", ((IQAgent_SMResultsModel)item.MediaData).HomeLink),
                                                           new XAttribute("feedClass", ((IQAgent_SMResultsModel)item.MediaData).SourceCategory),
                                                           new XAttribute("CompeteURL", ((IQAgent_SMResultsModel)item.MediaData).HomeLink),
                                                           new XAttribute("Compete_Result", ((IQAgent_SMResultsModel)item.MediaData).Compete_Result),
                                                           new XAttribute("Number_Hits", ((IQAgent_SMResultsModel)item.MediaData).Number_Hits),
                                                           new XAttribute("Compete_Audience", 200),
                                                           new XAttribute("IQAdShareValue", 31.54),
                                                           new XElement("HighlightedSMOutput", string.Empty),
                                                           new XElement("Sentiment", new XElement("Sentiment", new XElement("PositiveSentiment", item.PositiveSentiment), new XElement("NegativeSentiment", item.NegativeSentiment))),
                                                           new XAttribute("_IQDmaID", "21")
                                                         ));

            return XDoc.ToString();
        }

        public string GetTWAgentInsertXml()
        {
            int PageSize = 5;
            int FragSize = Convert.ToInt32(ConfigurationManager.AppSettings["FragSize"]);
            //TempData["PageNumber"] = null;
            //hasMoreResultPage = false;
            //hasPreviousResultPage = false;


            SearchTwitterRequest twRequest = new SearchTwitterRequest();
            twRequest.IsHighlighting = false;
            //tvRequest.IsTitleandCCSearchAllowed = true;
            twRequest.IsSentiment = true;
            twRequest.FragSize = FragSize;
            twRequest.SearchTerm = "music";
            twRequest.PageSize = PageSize;
            twRequest.StartDate = new DateTime(2013, 07, 08, 06, 0, 0);
            twRequest.EndDate = new DateTime(2013, 07, 08, 06, 59, 59);
            twRequest.PageNumber = 0;

            twRequest.SortFields = "datetime-";
            twRequest.IsDeleted = false;

            Uri PMGSearchRequestUrl = new Uri(WebApplication.Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.TW.ToString(), twRequest.StartDate, twRequest.EndDate));
            SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);

            bool error = false;
            SearchTwitterResult twResult = searchEngine.SearchTwitter(twRequest, false, out error);


            string _Responce = string.Empty;
            XmlDocument _XmlDocument = new XmlDocument();

            _XmlDocument.LoadXml(twResult.ResponseXml);

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

            XDocument XDoc = new XDocument(new XElement("IQAgentTwitterResultsList",
                                                from TwitterResult item in twResult.TwitterResults
                                                select new XElement("IQAgentTwitterResult",
                                                      new XAttribute("QueryVersion", 0),
                                                      new XAttribute("actor_image", item.actor_image),
                                                      new XAttribute("actor_link", item.actor_link),
                                                      new XAttribute("TweetID", item.iqseqid),
                                                      new XAttribute("tweet_posteddatetime", item.tweet_postedDateTime),
                                                      new XAttribute("actor_followerscount", item.followers_count),
                                                      new XAttribute("actor_friendscount", item.friends_count),
                                                      new XAttribute("Summary", item.tweet_body),
                                                      new XAttribute("actor_displayname", item.actor_displayName),
                                                      new XAttribute("actor_preferredname", item.actor_prefferedUserName),
                                                      new XAttribute("gnip_klout_score", item.Klout_score),
                                                      new XAttribute("Number_Hits", 1),
                                                      new XElement("HighlightedTWOutput", string.Empty),
                                                      new XElement("Sentiment", new XElement("PositiveSentiment", item.Sentiments.PositiveSentiment), new XElement("NegativeSentiment", item.Sentiments.NegativeSentiment))
                                                    )));




            SessionInformation sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

            long? sinceID = null;
            Int64? totalResults = 0, totalResultsDisplay = 0;

            IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
            Dictionary<string, object> dictResult = iQAgentLogic.GetIQAgentMediaResults(sessionInformation.ClientGUID,
                                                        Request.ServerVariables["HTTP_HOST"],
                                                        new List<string> { "137" },
                                                        50,
                                                        new DateTime(2013, 03, 15, 0, 0, 0),
                                                        new DateTime(2013, 03, 15, 0, 0, 0),
                                                        0,
                //Convert.ToInt64(TempData["FromRecordID"]),
                                                        "TW",
                                                        null,
                                                        null,
                                                        false,
                                                        ref sinceID,
                                                        out totalResults, out totalResultsDisplay, false, sessionInformation.Isv4TM, null, null, null, null, null, 0, false, null);


            List<IQAgent_MediaResultsModel> lstIQAgent_MediaResultsModel = (List<IQAgent_MediaResultsModel>)dictResult["Result"];

            XDoc.Element("IQAgentResultsList").Add(from IQAgent_MediaResultsModel item in lstIQAgent_MediaResultsModel
                                                   select new XElement("IQAgentResult",
                                                         new XAttribute("SearchRequestID", 75),
                                                         new XAttribute("QueryVersion", 0),
                                                         new XAttribute("Title120", ((IQAgent_TVResultsModel)item.MediaData).Title120),
                                                         new XAttribute("iq_cc_key", ((IQAgent_TVResultsModel)item.MediaData).IQ_CC_Key == null ? string.Empty : ((IQAgent_TVResultsModel)item.MediaData).IQ_CC_Key),
                                                         new XAttribute("RL_VideoGUID", ((IQAgent_TVResultsModel)item.MediaData).RL_VideoGUID),
                                                         new XAttribute("GMTDatetime", item.MediaDateTime),
                                                         new XAttribute("Rl_Station", ((IQAgent_TVResultsModel)item.MediaData).RL_Station),
                                                         new XAttribute("Rl_Market", ((IQAgent_TVResultsModel)item.MediaData).Market),
                                                         new XAttribute("RL_Date", ((IQAgent_TVResultsModel)item.MediaData).Date),
                                                         new XAttribute("RL_Time", "0"),
                                                         new XAttribute("Number_Hits", ((IQAgent_TVResultsModel)item.MediaData).Hits),
                                                         new XAttribute("Nielsen_Audience", ((IQAgent_TVResultsModel)item.MediaData).Nielsen_Audience == null ? 0 : ((IQAgent_TVResultsModel)item.MediaData).Nielsen_Audience),
                                                         new XAttribute("IQAdShareValue", ((IQAgent_TVResultsModel)item.MediaData).IQAdShareValue == null ? 0 : ((IQAgent_TVResultsModel)item.MediaData).IQAdShareValue),
                                                         new XAttribute("Nielsen_Result", ((IQAgent_TVResultsModel)item.MediaData).Nielsen_Result == null ? string.Empty : ((IQAgent_TVResultsModel)item.MediaData).Nielsen_Result),
                                                         new XElement("HighlightedCCOutput", string.Empty),
                                                         new XElement("Sentiment", new XElement("Sentiment", new XElement("PositiveSentiment", item.PositiveSentiment), new XElement("NegativeSentiment", item.NegativeSentiment))),
                                                         new XAttribute("_IQDmaID", "21")
                                                       ));

            return XDoc.ToString();
        }
        */

        [HttpGet]
        public ActionResult Html5Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Html5Upload(string fileName, string txtName, string ddlItems, string TestGroup, string chk1)
        {
            FileStream fileStream = new FileStream(Server.MapPath("~/TestUpload/") + fileName, FileMode.OpenOrCreate);
            try
            {
                Stream inputStream = Request.InputStream;
                inputStream.CopyTo(fileStream);
            }
            catch (Exception)
            {
            }
            finally
            {
                fileStream.Close();
            }
            return View();
        }

        [HttpPost]
        public string Html5UploadIE(string txtName, string ddlItems, string TestGroup, string chk1)
        {
            throw new Exception("Custom exception...");
            if (Request.Files.Count > 0)
            {
                Request.Files[0].SaveAs(Server.MapPath("~/TestUpload/") + Path.GetFileName(Request.Files[0].FileName));
            }
            return "success";
        }
    }
}
