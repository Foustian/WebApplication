using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.WebApplication.Utility;
using PMGSearch;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class LNArticleController : Controller
    {
        //
        // GET: /ProQuest/

        [HttpGet]
        public ActionResult Index(string id, string source)
        {
            ViewBag.IsInvalidID = false;
            ViewBag.IsError = false;

            try
            {
                Int64 mediaID;
                if (!string.IsNullOrEmpty(id) && Int64.TryParse(id, out mediaID))
                {
                    System.Uri PMGSearchRequestUrl = new Uri(CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.MO.ToString(), null, null));
                    SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                    SearchNewsRequest searchRequest = new SearchNewsRequest();

                    searchRequest.IDs = new List<string>() { mediaID.ToString() };
                    searchRequest.FieldList = "harvestdate_dt,title,source,authorname,content,copyright,activationurl";
                    searchRequest.PageSize = 1;

                    SearchNewsResults searchResult = searchEngine.SearchNews(searchRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));

                    if (searchResult.newsResults != null && searchResult.newsResults.Count == 1)
                    {
                        NewsResult newsResult = searchResult.newsResults[0];

                        if (!String.IsNullOrWhiteSpace(newsResult.Content))
                        {
                            newsResult.Content = newsResult.Content.Replace(ConfigurationManager.AppSettings["LexisNexisLineBreakPlaceholder"], "<br/>");
                        }

                        if ((DateTime.Now - DateTime.Parse(newsResult.date)).Days <= 90 || (!String.IsNullOrEmpty(source) && source.ToLower() == "library"))
                        {
                            ViewBag.IsExpired = false;
                            if (!String.IsNullOrEmpty(newsResult.ActivationUrl))
                            {
                                try
                                {
                                    // The url throws a server error
                                    Shared.Utility.CommonFunctions.DoHttpPostRequest(newsResult.ActivationUrl, String.Empty);
                                }
                                catch (Exception)
                                { }
                            }
                        }
                        else
                        {
                            ViewBag.IsExpired = true;
                        }

                        return View(newsResult);
                    }
                }
                else
                {
                    ViewBag.IsInvalidID = true;
                }
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                ViewBag.IsError = true;
            }

            return View();
        }

    }
}
