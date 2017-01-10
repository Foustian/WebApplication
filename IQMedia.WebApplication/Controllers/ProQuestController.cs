using System;
using System.Web.Mvc;
using IQMedia.Model;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using PMGSearch;
using IQMedia.WebApplication.Utility;
using System.Collections.Generic;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class ProQuestController : Controller
    {
        //
        // GET: /ProQuest/

        [HttpGet]
        public ActionResult Index(string id, string source)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    ProQuestModel proQuestModel = null;
                    long mediaID = 0;

                    // Legacy PQArticle urls will be redirected here, but without a source value
                    if (String.IsNullOrEmpty(source))
                    {
                        source = "notification";
                    }

                    // Notification links use an encrypted non-numeric ID
                    if (source.ToLower() == "notification")
                    {
                        PQLogic pqLgc = (PQLogic)(IQMedia.Web.Logic.Base.LogicFactory.GetLogic(Web.Logic.Base.LogicType.PQ));
                        mediaID = pqLgc.DecryptPQArticleID(id);
                    }
                    else if (!Int64.TryParse(id, out mediaID))
                    {
                        throw new Exception("Invalid ID");
                    }

                    switch (source.ToLower())
                    {
                        case "library":
                            IQArchieveLogic iQArchiveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                            IQArchive_MediaModel objIQArchive_MediaModel = iQArchiveLogic.GetIQArchiveByIDForView(mediaID);
                            IQArchive_ArchivePQModel objIQArchive_PQModel = (IQArchive_ArchivePQModel)objIQArchive_MediaModel.MediaData;

                            proQuestModel = new ProQuestModel();
                            proQuestModel.ID = objIQArchive_MediaModel.ID;
                            proQuestModel.Title = objIQArchive_MediaModel.Title;
                            proQuestModel.MediaDate = objIQArchive_MediaModel.MediaDate;
                            proQuestModel.Publication = objIQArchive_PQModel.Publication;
                            proQuestModel.Content = objIQArchive_PQModel.ContentHTML;
                            proQuestModel.Authors = objIQArchive_PQModel.Authors;
                            proQuestModel.Copyright = objIQArchive_PQModel.Copyright;
                            break;
                        case "solr":
                            System.Uri PMGSearchRequestUrl = new Uri(CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.PQ.ToString(), null, null));
                            SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                            SearchProQuestRequest searchRequest = new SearchProQuestRequest();

                            searchRequest.IDs = new List<string>() { mediaID.ToString() };
                            searchRequest.PageSize = 1;

                            bool isError;
                            SearchProQuestResult searchResult = searchEngine.SearchProQuest(searchRequest, false, out isError);

                            if (searchResult.ProQuestResults != null && searchResult.ProQuestResults.Count > 0)
                            {
                                proQuestModel = new ProQuestModel();
                                foreach (ProQuestResult result in searchResult.ProQuestResults)
                                {
                                    proQuestModel.ID = result.IQSeqID;
                                    proQuestModel.Title = result.Title;
                                    proQuestModel.MediaDate = result.MediaDate;
                                    proQuestModel.Publication = result.Publication;
                                    proQuestModel.Content = result.ContentHTML;
                                    proQuestModel.Authors = result.Authors;
                                    proQuestModel.Copyright = result.Copyright;
                                }
                            }
                            break;
                        case "feeds":
                        case "notification":
                            System.Uri FeedsSearchRequestUrl = new Uri(CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.FE.ToString(), null, null));
                            FeedsSearch.SearchEngine feSearchEngine = new FeedsSearch.SearchEngine(FeedsSearchRequestUrl);
                            FeedsSearch.SearchRequest feSearchRequest = new FeedsSearch.SearchRequest();

                            feSearchRequest.MediaIDs = new List<string>() { mediaID.ToString() };
                            feSearchRequest.PageSize = 1;
                            feSearchRequest.IncludeDeleted = true;

                            Dictionary<string, FeedsSearch.SearchResult> dictResults = feSearchEngine.Search(feSearchRequest);

                            if (dictResults.ContainsKey("Results"))
                            {
                                FeedsSearch.SearchResult feSearchResult = (FeedsSearch.SearchResult)dictResults["Results"];
                                if (feSearchResult.Hits.Count > 0)
                                {
                                    proQuestModel = new ProQuestModel();
                                    foreach (FeedsSearch.Hit hit in feSearchResult.Hits)
                                    {
                                        proQuestModel.ID = hit.ID;
                                        proQuestModel.Title = hit.Title;
                                        proQuestModel.MediaDate = hit.MediaDate;
                                        proQuestModel.Publication = hit.Publication;
                                        proQuestModel.Content = hit.Content;
                                        proQuestModel.Authors = hit.Authors;
                                        proQuestModel.Copyright = hit.Copyright;
                                    }
                                }
                            }
                            break;
                    }

                    ViewBag.IsSuccess = true;
                    return View(proQuestModel);
                }
                else
                {
                    throw new Exception("Blank ID");
                }
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                ViewBag.IsSuccess = false;
            }

            return View();
        }

    }
}
