using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using IQMedia.Model;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.WebApplication.Config;
using IQMedia.WebApplication.Utility;
using IQCommon.Model;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class MCMediaTemplateController : Controller
    {
        #region MCMediaTemplate1

        [HttpGet]
        public ActionResult MCMediaTemplate1(string id)
        {
            ViewBag.IsInvalidID = false;
            ViewBag.IsError = false;

            Dictionary<string, object> dictResults = new Dictionary<string, object>();
            try
            {
                Guid temp;
                if (!string.IsNullOrEmpty(id) && Guid.TryParse(id, out temp))
                {
                    IQ_ReportTypeModel mcMediaTemplate = GetMCMediaTemplate(temp);

                    if (mcMediaTemplate != null)
                    {
                        MCMediaTemplateLogic mcMediaTemplateLogic = (MCMediaTemplateLogic)LogicFactory.GetLogic(LogicType.MCMediaTemplate);
                        MCMediaReportModel report = mcMediaTemplateLogic.GetMCMediaResultsForReport(temp, null, mcMediaTemplate.Settings, Request.ServerVariables["HTTP_HOST"]);

                        SetIQCustomSettings(report.MasterClientGuid.Value.ToString());
                        SetMediaTypes();

                        foreach (MCMediaReport_GroupTier1Model groupTier1Result in report.GroupTier1Results)
                        {
                            foreach (MCMediaReport_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                            {
                                foreach (MCMediaReport_GroupTier3Model groupTier3Result in groupTier2Result.GroupTier3Results)
                                {
                                    groupTier3Result.MediaResults.ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDate));
                                    IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(groupTier3Result.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Library);
                                    IQMedia.WebApplication.Utility.CommonFunctions.ProcessArchiveDisplayText(groupTier3Result.MediaResults, GetIQCustomSettings().LibraryTextType);
                                }
                            }
                        }

                        SetMCMediaTemplate(temp, mcMediaTemplate);

                        dictResults.Add("MCMediaReport", report);
                        dictResults.Add("MediaTypes", GetMediaTypes());
                    }
                    else
                    {
                        ViewBag.IsInvalidID = true;
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

            return View(dictResults);
        }

        #endregion

        #region MCMediaTemplate2

        [HttpGet]
        public ActionResult MCMediaTemplate2(string id)
        {
            ViewBag.IsInvalidID = false;
            ViewBag.IsError = false;

            MCMediaReportModel report = new MCMediaReportModel();
            try
            {
                Guid temp;
                if (!string.IsNullOrEmpty(id) && Guid.TryParse(id, out temp))
                {
                    IQ_ReportTypeModel mcMediaTemplate = GetMCMediaTemplate(temp);

                    if (mcMediaTemplate != null)
                    {
                        MCMediaSearchModel searchSettings = new MCMediaSearchModel();
                        searchSettings.FromDate = null;
                        searchSettings.ToDate = null;
                        searchSettings.SearchTerm = null;
                        searchSettings.SubMediaType = null;
                        searchSettings.ClientGuid = null;
                        searchSettings.SelectionType = null;
                        searchSettings.CategorySet = new CategorySet();

                        MCMediaTemplateLogic mcMediaTemplateLogic = (MCMediaTemplateLogic)LogicFactory.GetLogic(LogicType.MCMediaTemplate);
                        report = mcMediaTemplateLogic.GetMCMediaResultsForReport(temp, searchSettings, mcMediaTemplate.Settings, Request.ServerVariables["HTTP_HOST"]);

                        SetIQCustomSettings(report.MasterClientGuid.Value.ToString());
                        SetMediaTypes();

                        foreach (MCMediaReport_GroupTier1Model groupTier1Result in report.GroupTier1Results)
                        {
                            foreach (MCMediaReport_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                            {
                                foreach (MCMediaReport_GroupTier3Model groupTier3Result in groupTier2Result.GroupTier3Results)
                                {
                                    groupTier3Result.MediaResults.ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDate));
                                    IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(groupTier3Result.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Library);
                                    IQMedia.WebApplication.Utility.CommonFunctions.ProcessArchiveDisplayText(groupTier3Result.MediaResults, GetIQCustomSettings().LibraryTextType);
                                }
                            }
                        }

                        SetMCMediaTemplate(temp, mcMediaTemplate);

                        ActiveUser sessionInfo = ActiveUserMgr.GetActiveUser();
                        List<IQ_MediaTypeModel> lstMediaTypes;
                        if (sessionInfo == null)
                        {
                            lstMediaTypes = IQCommon.CommonFunctions.GetMediaTypes(new Guid());
                        }
                        else
                        {
                            lstMediaTypes = sessionInfo.MediaTypes;
                        }

                        Dictionary<string, object> dictPartial = new Dictionary<string, object>();
                        dictPartial.Add("Results", report.GroupTier1Results);
                        dictPartial.Add("MediaTypes", GetMediaTypes());

                        Dictionary<string, object> dictResult = new Dictionary<string, object>();
                        dictResult.Add("MCMediaReport", report);
                        dictResult.Add("HTML", RenderPartialToString(mcMediaTemplate.Settings.ResultsViewPath, dictPartial));

                        return View(dictResult);
                    }
                    else
                    {
                        ViewBag.IsInvalidID = true;
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

        [HttpPost]
        public JsonResult MCMediaTemplate2_SearchResults(Guid reportGuid, DateTime? fromDate, DateTime? toDate, string searchTerm, string subMediaType, Guid? clientGuid, List<string> categoryNames, string selectionType)
        {
            try
            {
                IQ_ReportTypeModel mcMediaTemplate = GetMCMediaTemplate(reportGuid);

                if (fromDate.HasValue && toDate.HasValue)
                {
                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);

                    toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }

                if (searchTerm.StartsWith("\"") && searchTerm.EndsWith("\""))
                {
                    searchTerm = searchTerm.Substring(1, searchTerm.Length - 2);
                }

                MCMediaSearchModel searchSettings = new MCMediaSearchModel();
                searchSettings.FromDate = fromDate;
                searchSettings.ToDate = toDate;
                searchSettings.SearchTerm = searchTerm;
                searchSettings.SubMediaType = subMediaType;
                searchSettings.ClientGuid = clientGuid;
                searchSettings.SelectionType = selectionType;
                searchSettings.CategorySet = new CategorySet();
                if (categoryNames != null && categoryNames.Count > 0)
                {
                    searchSettings.CategorySet.IsAllowAll = false;
                    searchSettings.CategorySet.CategoryList = categoryNames;
                }
                else
                {
                    searchSettings.CategorySet.IsAllowAll = true;
                }

                MCMediaTemplateLogic mcMediaTemplateLogic = (MCMediaTemplateLogic)LogicFactory.GetLogic(LogicType.MCMediaTemplate);
                MCMediaReportModel report = mcMediaTemplateLogic.GetMCMediaResultsForReport(reportGuid, searchSettings, mcMediaTemplate.Settings, Request.ServerVariables["HTTP_HOST"]);
                foreach (MCMediaReport_GroupTier1Model groupTier1Result in report.GroupTier1Results)
                {
                    foreach (MCMediaReport_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                    {
                        foreach (MCMediaReport_GroupTier3Model groupTier3Result in groupTier2Result.GroupTier3Results)
                        {
                            groupTier3Result.MediaResults.ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDate));
                            IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(groupTier3Result.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Library);
                            IQMedia.WebApplication.Utility.CommonFunctions.ProcessArchiveDisplayText(groupTier3Result.MediaResults, GetIQCustomSettings().LibraryTextType);
                        }
                    }
                }

                Dictionary<string, object> dictPartial = new Dictionary<string, object>();
                dictPartial.Add("Results", report.GroupTier1Results);
                dictPartial.Add("MediaTypes", GetMediaTypes());

                return Json(new
                {
                    isSuccess = true,
                    filter = report.FilterResults,
                    HTML = RenderPartialToString(mcMediaTemplate.Settings.ResultsViewPath, dictPartial)
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
        }

        [HttpPost]
        public JsonResult MCMediaTemplate2_FilterCategory(string reportGuid, DateTime? fromDate, DateTime? toDate, string searchTerm, string subMediaType, string clientGuid, List<string> categoryNames)
        {
            try
            {
                if (fromDate.HasValue && toDate.HasValue)
                {
                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);

                    toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }

                MCMediaTemplateLogic mcMediaTemplateLogic = (MCMediaTemplateLogic)LogicFactory.GetLogic(LogicType.MCMediaTemplate);
                List<IQArchive_Filter> lstCategoryFilter = mcMediaTemplateLogic.MCMediaTemplate2_FilterCategory(clientGuid, fromDate, toDate, subMediaType, searchTerm, categoryNames, reportGuid);

                var json = new
                {
                    isSuccess = true,
                    categoryFilter = lstCategoryFilter
                };

                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
        }

        #endregion

        #region MCMediaTemplate3

        [HttpGet]
        public ActionResult MCMediaTemplate3(string id)
        {
            ViewBag.IsInvalidID = false;
            ViewBag.IsError = false;

            MCMediaReportModel report = new MCMediaReportModel();
            try
            {
                Guid temp;
                if (!string.IsNullOrEmpty(id) && Guid.TryParse(id, out temp))
                {
                    IQ_ReportTypeModel mcMediaTemplate = GetMCMediaTemplate(temp);

                    if (mcMediaTemplate != null)
                    {
                        MCMediaSearchModel searchSettings = new MCMediaSearchModel();
                        searchSettings.FromDate = null;
                        searchSettings.ToDate = null;
                        searchSettings.SearchTerm = null;
                        searchSettings.SubMediaType = null;
                        searchSettings.ClientGuid = null;
                        searchSettings.SelectionType = null;
                        searchSettings.CategorySet = new CategorySet();

                        MCMediaTemplateLogic mcMediaTemplateLogic = (MCMediaTemplateLogic)LogicFactory.GetLogic(LogicType.MCMediaTemplate);
                        report = mcMediaTemplateLogic.GetMCMediaResultsForReport(temp, searchSettings, mcMediaTemplate.Settings, Request.ServerVariables["HTTP_HOST"]);

                        SetIQCustomSettings(report.MasterClientGuid.Value.ToString());
                        SetMediaTypes();

                        foreach (MCMediaReport_GroupTier1Model groupTier1Result in report.GroupTier1Results)
                        {
                            foreach (MCMediaReport_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                            {
                                foreach (MCMediaReport_GroupTier3Model groupTier3Result in groupTier2Result.GroupTier3Results)
                                {
                                    groupTier3Result.MediaResults.ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDate));
                                    IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(groupTier3Result.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Library);
                                    IQMedia.WebApplication.Utility.CommonFunctions.ProcessArchiveDisplayText(groupTier3Result.MediaResults, GetIQCustomSettings().LibraryTextType);
                                }
                            }
                        }

                        SetMCMediaTemplate(temp, mcMediaTemplate);

                        Dictionary<string, object> dictPartial = new Dictionary<string, object>();
                        dictPartial.Add("Results", report.GroupTier1Results);
                        dictPartial.Add("MediaTypes", GetMediaTypes());

                        Dictionary<string, object> dictResult = new Dictionary<string, object>();
                        dictResult.Add("MCMediaReport", report);
                        dictResult.Add("HTML", RenderPartialToString(mcMediaTemplate.Settings.ResultsViewPath, dictPartial));

                        return View(dictResult);
                    }
                    else
                    {
                        ViewBag.IsInvalidID = true;
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

        [HttpPost]
        public JsonResult MCMediaTemplate3_SearchResults(Guid reportGuid, string searchTerm, string subMediaType, List<string> categoryNames, string selectionType)
        {
            try
            {
                IQ_ReportTypeModel mcMediaTemplate = GetMCMediaTemplate(reportGuid);

                if (searchTerm.StartsWith("\"") && searchTerm.EndsWith("\""))
                {
                    searchTerm = searchTerm.Substring(1, searchTerm.Length - 2);
                }

                MCMediaSearchModel searchSettings = new MCMediaSearchModel();
                searchSettings.FromDate = null;
                searchSettings.ToDate = null;
                searchSettings.SearchTerm = searchTerm;
                searchSettings.SubMediaType = subMediaType;
                searchSettings.ClientGuid = null;
                searchSettings.SelectionType = selectionType;
                searchSettings.CategorySet = new CategorySet();
                if (categoryNames != null && categoryNames.Count > 0)
                {
                    searchSettings.CategorySet.IsAllowAll = false;
                    searchSettings.CategorySet.CategoryList = categoryNames;
                }
                else
                {
                    searchSettings.CategorySet.IsAllowAll = true;
                }

                MCMediaTemplateLogic mcMediaTemplateLogic = (MCMediaTemplateLogic)LogicFactory.GetLogic(LogicType.MCMediaTemplate);
                MCMediaReportModel report = mcMediaTemplateLogic.GetMCMediaResultsForReport(reportGuid, searchSettings, mcMediaTemplate.Settings, Request.ServerVariables["HTTP_HOST"]);
                foreach (MCMediaReport_GroupTier1Model groupTier1Result in report.GroupTier1Results)
                {
                    foreach (MCMediaReport_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                    {
                        foreach (MCMediaReport_GroupTier3Model groupTier3Result in groupTier2Result.GroupTier3Results)
                        {
                            groupTier3Result.MediaResults.ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDate));
                            IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(groupTier3Result.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Library);
                            IQMedia.WebApplication.Utility.CommonFunctions.ProcessArchiveDisplayText(groupTier3Result.MediaResults, GetIQCustomSettings().LibraryTextType);
                        }
                    }
                }

                Dictionary<string, object> dictPartial = new Dictionary<string, object>();
                dictPartial.Add("Results", report.GroupTier1Results);
                dictPartial.Add("MediaTypes", GetMediaTypes());

                return Json(new
                {
                    isSuccess = true,
                    filter = report.FilterResults,
                    HTML = RenderPartialToString(mcMediaTemplate.Settings.ResultsViewPath, dictPartial)
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
        }
        
        [HttpPost]
        public JsonResult MCMediaTemplate3_FilterCategory(string reportGuid, string searchTerm, string subMediaType, List<string> categoryNames)
        {
            try
            {
                MCMediaTemplateLogic mcMediaTemplateLogic = (MCMediaTemplateLogic)LogicFactory.GetLogic(LogicType.MCMediaTemplate);
                List<IQArchive_Filter> lstCategoryFilter = mcMediaTemplateLogic.MCMediaTemplate3_FilterCategory(subMediaType, searchTerm, categoryNames, reportGuid);

                var json = new
                {
                    isSuccess = true,
                    categoryFilter = lstCategoryFilter
                };

                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
        }

        #endregion

        #region MCMediaTemplateDemo

        [HttpGet]
        public ActionResult MCMediaTemplateDemo(string id)
        {
            ViewBag.IsInvalidID = false;
            ViewBag.IsError = false;

            MCMediaReportModel report = new MCMediaReportModel();
            try
            {
                Guid temp;
                if (!string.IsNullOrEmpty(id) && Guid.TryParse(id, out temp))
                {
                    IQ_ReportTypeModel mcMediaTemplate = GetMCMediaTemplate(temp);

                    if (mcMediaTemplate != null)
                    {
                        MCMediaSearchModel searchSettings = new MCMediaSearchModel();
                        searchSettings.FromDate = null;
                        searchSettings.ToDate = null;
                        searchSettings.SearchTerm = null;
                        searchSettings.SubMediaType = null;
                        searchSettings.ClientGuid = null;
                        searchSettings.SelectionType = null;
                        searchSettings.CategorySet = new CategorySet();

                        MCMediaTemplateLogic mcMediaTemplateLogic = (MCMediaTemplateLogic)LogicFactory.GetLogic(LogicType.MCMediaTemplate);
                        report = mcMediaTemplateLogic.GetMCMediaResultsForReport(temp, searchSettings, mcMediaTemplate.Settings, Request.ServerVariables["HTTP_HOST"]);

                        SetIQCustomSettings(report.MasterClientGuid.Value.ToString());
                        SetMediaTypes();

                        foreach (MCMediaReport_GroupTier1Model groupTier1Result in report.GroupTier1Results)
                        {
                            foreach (MCMediaReport_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                            {
                                foreach (MCMediaReport_GroupTier3Model groupTier3Result in groupTier2Result.GroupTier3Results)
                                {
                                    groupTier3Result.MediaResults.ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDate));
                                    IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(groupTier3Result.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Library);
                                    IQMedia.WebApplication.Utility.CommonFunctions.ProcessArchiveDisplayText(groupTier3Result.MediaResults, GetIQCustomSettings().LibraryTextType);
                                }
                            }
                        }

                        SetMCMediaTemplate(temp, mcMediaTemplate);

                        Dictionary<string, object> dictPartial = new Dictionary<string, object>();
                        dictPartial.Add("Results", report.GroupTier1Results);
                        dictPartial.Add("MediaTypes", GetMediaTypes());

                        Dictionary<string, object> dictResult = new Dictionary<string, object>();
                        dictResult.Add("MCMediaReport", report);
                        dictResult.Add("HTML", RenderPartialToString(mcMediaTemplate.Settings.ResultsViewPath, dictPartial));

                        return View(dictResult);
                    }
                    else
                    {
                        ViewBag.IsInvalidID = true;
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

        [HttpPost]
        public JsonResult MCMediaTemplateDemo_SearchResults(Guid reportGuid, DateTime? fromDate, DateTime? toDate, string searchTerm, string subMediaType, Guid? clientGuid, List<string> categoryNames, string selectionType)
        {
            try
            {
                IQ_ReportTypeModel mcMediaTemplate = GetMCMediaTemplate(reportGuid);

                if (fromDate.HasValue && toDate.HasValue)
                {
                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);

                    toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }

                if (searchTerm.StartsWith("\"") && searchTerm.EndsWith("\""))
                {
                    searchTerm = searchTerm.Substring(1, searchTerm.Length - 2);
                }

                MCMediaSearchModel searchSettings = new MCMediaSearchModel();
                searchSettings.FromDate = fromDate;
                searchSettings.ToDate = toDate;
                searchSettings.SearchTerm = searchTerm;
                searchSettings.SubMediaType = subMediaType;
                searchSettings.ClientGuid = clientGuid;
                searchSettings.SelectionType = selectionType;
                searchSettings.CategorySet = new CategorySet();
                if (categoryNames != null && categoryNames.Count > 0)
                {
                    searchSettings.CategorySet.IsAllowAll = false;
                    Shared.Utility.Log4NetLogger.Info(String.Join(",", categoryNames));
                    searchSettings.CategorySet.CategoryList = categoryNames;
                }
                else
                {
                    searchSettings.CategorySet.IsAllowAll = true;
                }

                MCMediaTemplateLogic mcMediaTemplateLogic = (MCMediaTemplateLogic)LogicFactory.GetLogic(LogicType.MCMediaTemplate);
                MCMediaReportModel report = mcMediaTemplateLogic.GetMCMediaResultsForReport(reportGuid, searchSettings, mcMediaTemplate.Settings, Request.ServerVariables["HTTP_HOST"]);
                foreach (MCMediaReport_GroupTier1Model groupTier1Result in report.GroupTier1Results)
                {
                    foreach (MCMediaReport_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                    {
                        foreach (MCMediaReport_GroupTier3Model groupTier3Result in groupTier2Result.GroupTier3Results)
                        {
                            groupTier3Result.MediaResults.ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDate));
                            IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(groupTier3Result.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Library);
                            IQMedia.WebApplication.Utility.CommonFunctions.ProcessArchiveDisplayText(groupTier3Result.MediaResults, GetIQCustomSettings().LibraryTextType);
                        }
                    }
                }

                Dictionary<string, object> dictPartial = new Dictionary<string, object>();
                dictPartial.Add("Results", report.GroupTier1Results);
                dictPartial.Add("MediaTypes", GetMediaTypes());

                return Json(new
                {
                    isSuccess = true,
                    filter = report.FilterResults,
                    HTML = RenderPartialToString(mcMediaTemplate.Settings.ResultsViewPath, dictPartial)
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
        }

        #endregion

        #region MCMediaTemplateTrivago

        [HttpGet]
        public ActionResult MCMediaTemplateTrivago(string id)
        {
            ViewBag.IsInvalidID = false;
            ViewBag.IsError = false;

            MCMediaReportModel report = new MCMediaReportModel();
            try
            {
                Guid temp;
                if (!string.IsNullOrEmpty(id) && Guid.TryParse(id, out temp))
                {
                    IQ_ReportTypeModel mcMediaTemplate = GetMCMediaTemplate(temp);

                    if (mcMediaTemplate != null)
                    {
                        MCMediaTemplateLogic mcMediaTemplateLogic = (MCMediaTemplateLogic)LogicFactory.GetLogic(LogicType.MCMediaTemplate);
                        report = mcMediaTemplateLogic.GetMCMediaResultsForReport(temp, new MCMediaSearchModel(), mcMediaTemplate.Settings, Request.ServerVariables["HTTP_HOST"]);

                        SetIQCustomSettings(report.MasterClientGuid.Value.ToString());
                        SetMediaTypes();

                        foreach (MCMediaReport_GroupTier1Model groupTier1Result in report.GroupTier1Results)
                        {
                            foreach (MCMediaReport_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                            {
                                foreach (MCMediaReport_GroupTier3Model groupTier3Result in groupTier2Result.GroupTier3Results)
                                {
                                    groupTier3Result.MediaResults.ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDate));
                                    IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(groupTier3Result.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Library);
                                    IQMedia.WebApplication.Utility.CommonFunctions.ProcessArchiveDisplayText(groupTier3Result.MediaResults, GetIQCustomSettings().LibraryTextType);
                                }
                            }
                        }

                        SetMCMediaTemplate(temp, mcMediaTemplate);

                        Dictionary<string, object> dictPartial = new Dictionary<string, object>();
                        dictPartial.Add("Results", report.GroupTier1Results);
                        dictPartial.Add("MediaTypes", GetMediaTypes());

                        Dictionary<string, object> dictResult = new Dictionary<string, object>();
                        dictResult.Add("MCMediaReport", report);
                        dictResult.Add("HTML", RenderPartialToString(mcMediaTemplate.Settings.ResultsViewPath, dictPartial));

                        return View(dictResult);
                    }
                    else
                    {
                        ViewBag.IsInvalidID = true;
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

        [HttpPost]
        public JsonResult MCMediaTemplateTrivago_SearchResults(Guid reportGuid, DateTime? fromDate, DateTime? toDate, string searchTerm, string subMediaType, List<string> categoryNames, string selectionType, int? sentimentFlag)
        {
            try
            {
                IQ_ReportTypeModel mcMediaTemplate = GetMCMediaTemplate(reportGuid);

                if (fromDate.HasValue && toDate.HasValue)
                {
                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);

                    toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }

                if (searchTerm.StartsWith("\"") && searchTerm.EndsWith("\""))
                {
                    searchTerm = searchTerm.Substring(1, searchTerm.Length - 2);
                }

                MCMediaSearchModel searchSettings = new MCMediaSearchModel();
                searchSettings.FromDate = fromDate;
                searchSettings.ToDate = toDate;
                searchSettings.SearchTerm = searchTerm;
                searchSettings.SubMediaType = subMediaType;
                searchSettings.SelectionType = selectionType;
                searchSettings.SentimentFlag = sentimentFlag;
                searchSettings.CategorySet = new CategorySet();
                if (categoryNames != null && categoryNames.Count > 0)
                {
                    searchSettings.CategorySet.IsAllowAll = false;
                    searchSettings.CategorySet.CategoryList = categoryNames;
                }
                else
                {
                    searchSettings.CategorySet.IsAllowAll = true;
                }

                MCMediaTemplateLogic mcMediaTemplateLogic = (MCMediaTemplateLogic)LogicFactory.GetLogic(LogicType.MCMediaTemplate);
                MCMediaReportModel report = mcMediaTemplateLogic.GetMCMediaResultsForReport(reportGuid, searchSettings, mcMediaTemplate.Settings, Request.ServerVariables["HTTP_HOST"]);
                foreach (MCMediaReport_GroupTier1Model groupTier1Result in report.GroupTier1Results)
                {
                    foreach (MCMediaReport_GroupTier2Model groupTier2Result in groupTier1Result.GroupTier2Results)
                    {
                        foreach (MCMediaReport_GroupTier3Model groupTier3Result in groupTier2Result.GroupTier3Results)
                        {
                            groupTier3Result.MediaResults.ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDate));
                            IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(groupTier3Result.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Library);
                            IQMedia.WebApplication.Utility.CommonFunctions.ProcessArchiveDisplayText(groupTier3Result.MediaResults, GetIQCustomSettings().LibraryTextType);
                        }
                    }
                }

                Dictionary<string, object> dictPartial = new Dictionary<string, object>();
                dictPartial.Add("Results", report.GroupTier1Results);
                dictPartial.Add("MediaTypes", GetMediaTypes());

                return Json(new
                {
                    isSuccess = true,
                    filter = report.FilterResults,
                    HTML = RenderPartialToString(mcMediaTemplate.Settings.ResultsViewPath, dictPartial)
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
        }

        [HttpPost]
        public JsonResult MCMediaTemplateTrivago_FilterCategory(string reportGuid, DateTime? fromDate, DateTime? toDate, string searchTerm, string subMediaType, List<string> categoryNames, int? sentimentFlag)
        {
            try
            {
                if (fromDate.HasValue && toDate.HasValue)
                {
                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);

                    toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }

                MCMediaTemplateLogic mcMediaTemplateLogic = (MCMediaTemplateLogic)LogicFactory.GetLogic(LogicType.MCMediaTemplate);
                List<IQArchive_Filter> lstCategoryFilter = mcMediaTemplateLogic.MCMediaTemplateTrivago_FilterCategory(fromDate, toDate, subMediaType, searchTerm, sentimentFlag, categoryNames, reportGuid);

                var json = new
                {
                    isSuccess = true,
                    categoryFilter = lstCategoryFilter
                };

                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
        }

        #endregion

        #region Utility

        private IQ_ReportTypeModel GetMCMediaTemplate(Guid reportGuid)
        {
            IQ_ReportTypeModel mcMediaTemplate = null;
            try
            {
                if (TempData["MCMediaTemplates"] != null)
                {
                    Dictionary<Guid, IQ_ReportTypeModel> dictTemplates = (Dictionary<Guid, IQ_ReportTypeModel>)TempData["MCMediaTemplates"];
                    if (dictTemplates.ContainsKey(reportGuid))
                    {
                        mcMediaTemplate = dictTemplates[reportGuid];
                    }
                }

                if (mcMediaTemplate == null)
                {
                    ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                    mcMediaTemplate = reportLogic.GetReportTypeByReportGuid(reportGuid);
                }
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
            }
            finally { TempData.Keep(); }

            return mcMediaTemplate;
        }

        private void SetMCMediaTemplate(Guid reportGuid, IQ_ReportTypeModel mcMediaTemplate)
        {
            try
            {
                if (TempData["MCMediaTemplates"] == null)
                {
                    Dictionary<Guid, IQ_ReportTypeModel> dictTemplates = new Dictionary<Guid, IQ_ReportTypeModel>();
                    dictTemplates.Add(reportGuid, mcMediaTemplate);
                    TempData["MCMediaTemplates"] = dictTemplates;
                }
                else
                {
                    Dictionary<Guid, IQ_ReportTypeModel> dictTemplates = (Dictionary<Guid, IQ_ReportTypeModel>)TempData["MCMediaTemplates"];
                    if (!dictTemplates.ContainsKey(reportGuid))
                    {
                        dictTemplates.Add(reportGuid, mcMediaTemplate);
                    }
                }
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
            }
            finally { TempData.Keep(); }
        }

        private IQClient_CustomSettingsModel GetIQCustomSettings()
        {
            IQClient_CustomSettingsModel settings = null;
            try
            {
                settings = TempData["IQClientCustom_Settings"] as IQClient_CustomSettingsModel;
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
            }
            finally { TempData.Keep(); }

            return settings;
        }

        private void SetIQCustomSettings(string clientGuid)
        {
            try
            {
                if (TempData["IQClientCustom_Settings"] == null)
                {
                    ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                    IQClient_CustomSettingsModel customSettings = clientLogic.GetClientCustomSettings(clientGuid);
                    TempData["IQClientCustom_Settings"] = customSettings;
                }
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
            }
            finally { TempData.Keep(); }
        }

        private List<IQ_MediaTypeModel> GetMediaTypes()
        {
            List<IQ_MediaTypeModel> mediaTypes = null;
            try
            {
                mediaTypes = TempData["MediaTypes"] as List<IQ_MediaTypeModel>;
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
            }
            finally { TempData.Keep(); }

            return mediaTypes;
        }

        private void SetMediaTypes()
        {
            try
            {
                if (TempData["MediaTypes"] == null)
                {
                    ActiveUser sessionInfo = ActiveUserMgr.GetActiveUser();
                    if (sessionInfo != null && sessionInfo.MediaTypes != null && sessionInfo.MediaTypes.Count > 0)
                    {
                        TempData["MediaTypes"] = sessionInfo.MediaTypes;
                    }
                    else
                    {
                        // The customer guid is only needed to determine the user's access to the media type roles. Media Room functionality doesn't use any of those roles.
                        TempData["MediaTypes"] = IQCommon.CommonFunctions.GetMediaTypes(new Guid());
                    }
                }
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
            }
            finally { TempData.Keep(); }
        }

        private string RenderPartialToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion
    }
}
