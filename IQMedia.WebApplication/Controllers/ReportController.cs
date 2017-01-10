using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Model;
using IQMedia.Web.Logic.Base;
using IQMedia.Shared.Utility;
using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Text;
using IQMedia.WebApplication.Config;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class ReportController : Controller
    {
        string PATH_IQAgentReportPDFPartialView = "~/Views/Report/_IQAgentOutsideReport-PDF.cshtml";
        //
        // GET: /Report/

        [HttpGet]
        public ActionResult IQAgent(string id, string Source)
        {
            ViewBag.IsInvalidID = false;
            ViewBag.IsError = false;

            IQAgentReport_WithoutAuthentication objIQAgentReport_WithoutAuthentication = new IQAgentReport_WithoutAuthentication();
            try
            {
                Guid temp;
                if (!string.IsNullOrEmpty(id) && Guid.TryParse(id, out temp))
                {
                    int IQAgentReportMaxRecordDisplay = Convert.ToInt32(ConfigurationManager.AppSettings["IQAgentReportMaxRecordDisplay"]);
                    bool IsSourceEmail = false;
                    if (!string.IsNullOrWhiteSpace(Source) && Source.ToLower() == "email")
                    {
                        IsSourceEmail = true;
                    }
                    else
                    {
                        IsSourceEmail = false;
                    }
                    IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                    objIQAgentReport_WithoutAuthentication = iqAgentLogic.GetIQAgent_MediaResultReportByReportGuid(id, IQAgentReportMaxRecordDisplay, IsSourceEmail);
                    objIQAgentReport_WithoutAuthentication.MediaResults.ToList().ForEach(x => x.timeDifference = WebApplication.Utility.CommonFunctions.GetTimeDifference(x.MediaDateTime));
                    objIQAgentReport_WithoutAuthentication.MediaResults = IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(objIQAgentReport_WithoutAuthentication.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Report);
                    objIQAgentReport_WithoutAuthentication.ReportID = id;

                    ProcessIQAgentMediaResultHighlightingText(objIQAgentReport_WithoutAuthentication.MediaResults);

                    if (IsSourceEmail)
                    {
                        string reportHTML = RenderPartialToString(PATH_IQAgentReportPDFPartialView, objIQAgentReport_WithoutAuthentication);
                        Response.Write(reportHTML);
                        Response.ContentType = "text/html";
                        Response.End();
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

            return View(objIQAgentReport_WithoutAuthentication);
        }

        [HttpGet]
        public ActionResult DailyDigest(string id, string Source, Int64? searchrequest, string mediatype)
        {
            ViewBag.IsInvalidID = false;
            ViewBag.IsError = false;

            IQAgentReport objIQAgentReport_WithoutAuthentication = new IQAgentReport();
            try
            {
                Guid temp;
                if (!string.IsNullOrEmpty(id) && Guid.TryParse(id, out temp))
                {
                    int IQAgentReportMaxRecordDisplay = Convert.ToInt32(ConfigurationManager.AppSettings["IQAgentReportMaxRecordDisplay"]);

                    
                    if (!string.IsNullOrWhiteSpace(Source) && Source.ToLower() == "email")
                    {
                        IQMedia.WebApplication.Utility.CommonFunctions.WriteException(new ArgumentException("DailyDigest-Email"));
                        ViewBag.IsError = true;
                    }
                    
                    IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                    objIQAgentReport_WithoutAuthentication = iqAgentLogic.GetIQAgent_MediaResultReportByReportGuid(id, searchrequest, mediatype, IQAgentReportMaxRecordDisplay, false);
                    foreach (IQAgentReport_SearchRequestModel reqModel in objIQAgentReport_WithoutAuthentication.Results)
                    {
                        reqModel.MediaResults.ToList().ForEach(x => x.timeDifference = WebApplication.Utility.CommonFunctions.GetTimeDifference(x.MediaDateTime));
                        IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(reqModel.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Report);
                        ProcessIQAgentMediaResultHighlightingText(reqModel.MediaResults);
                    }
                    objIQAgentReport_WithoutAuthentication.ReportID = id;

                    /*
                    if (IsSourceEmail)
                    {
                        string reportHTML = RenderPartialToString(PATH_IQAgentReportPDFPartialView, objIQAgentReport_WithoutAuthentication);
                        Response.Write(reportHTML);
                        Response.ContentType = "text/html";
                        Response.End();
                    }
                     */ 
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

            return View(objIQAgentReport_WithoutAuthentication);
        }

        // Landing page for MCMedia report templates. Redirect to the correct page based on the template settings.
        [HttpGet]
        public ActionResult MCMediaRoom(string id)
        {
            try
            {
                Guid temp;
                if (!string.IsNullOrEmpty(id) && Guid.TryParse(id, out temp))
                {                    
                    ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                    IQ_ReportTypeModel mcMediaTemplate = reportLogic.GetReportTypeByReportGuid(temp);

                    if (mcMediaTemplate != null)
                    {
                        Response.Redirect("../MCMediaTemplate/" + mcMediaTemplate.Settings.TemplateType + "?ID=" + id);
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
        public JsonResult GenerateReportPDF(string p_ReportGuid)
        {
            try
            {
                int IQAgentReportMaxRecordDisplay = Convert.ToInt32(ConfigurationManager.AppSettings["IQAgentReportMaxRecordDisplay"]);

                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                IQAgentReport_WithoutAuthentication objIQAgentReport_WithoutAuthentication = iqAgentLogic.GetIQAgent_MediaResultReportByReportGuid(p_ReportGuid, IQAgentReportMaxRecordDisplay, false);
                objIQAgentReport_WithoutAuthentication.MediaResults.ToList().ForEach(x => x.timeDifference = WebApplication.Utility.CommonFunctions.GetTimeDifference(x.MediaDateTime));
                objIQAgentReport_WithoutAuthentication.MediaResults = IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(objIQAgentReport_WithoutAuthentication.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Report); 
                ProcessIQAgentMediaResultHighlightingText(objIQAgentReport_WithoutAuthentication.MediaResults);

                string reportHTML = RenderPartialToString(PATH_IQAgentReportPDFPartialView, objIQAgentReport_WithoutAuthentication);

                string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                string TempHTMLPath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Report\\PDF\\" + "IQAgentHTMLReport_" + DateTimeStamp + ".html";
                string TempPDFPath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Report\\PDF\\" + "IQAgentPDFReport_" + DateTimeStamp + ".pdf";
                // \/*?:"<>|

                string DownloadPDFFileName = objIQAgentReport_WithoutAuthentication.ReportTitle.Replace(" ", "_");
                DownloadPDFFileName = DownloadPDFFileName.Replace("\\", "_");
                DownloadPDFFileName = DownloadPDFFileName.Replace("/", "_");
                DownloadPDFFileName = DownloadPDFFileName.Replace("*", "_");
                DownloadPDFFileName = DownloadPDFFileName.Replace("?", "_");
                DownloadPDFFileName = DownloadPDFFileName.Replace(":", "_");
                DownloadPDFFileName = DownloadPDFFileName.Replace("\"", "_");
                DownloadPDFFileName = DownloadPDFFileName.Replace("<", "_");
                DownloadPDFFileName = DownloadPDFFileName.Replace(">", "_");
                DownloadPDFFileName = DownloadPDFFileName.Replace("|", "_");

                Session["DownloadPDFFileName"] = DownloadPDFFileName + ".pdf";

                bool IsFileGenerated = false;

                using (FileStream fs = new FileStream(TempHTMLPath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.Write(reportHTML);
                    }
                }

                Utility.CommonFunctions.RunProcess(ConfigurationManager.AppSettings["WKHtmlToPDFPath"], TempHTMLPath + " " + TempPDFPath);

                if (System.IO.File.Exists(TempPDFPath))
                {
                    IsFileGenerated = true;
                    Session["IQAgentPDFFile"] = TempPDFPath;

                    if (System.IO.File.Exists(TempHTMLPath))
                    {
                        System.IO.File.Delete(TempHTMLPath);
                    }
                }

                var json = new
                {
                    isSuccess = IsFileGenerated
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

        [HttpGet]
        public ActionResult DownloadPDFFile()
        {
            if (Session["IQAgentPDFFile"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["IQAgentPDFFile"])) && Session["DownloadPDFFileName"] != null)
            {
                string PDFFile = Convert.ToString(Session["IQAgentPDFFile"]);
                string DownloadFileName = Convert.ToString(Session["DownloadPDFFileName"]);

                if (System.IO.File.Exists(PDFFile))
                {
                    Session.Remove("IQAgentPDFFile");
                    Session.Remove("DownloadPDFFileName");
                    return File(PDFFile, "application/pdf", DownloadFileName);
                }
            }
            return Content(ConfigSettings.Settings.FileNotAvailable);// "File not available.Please try again later");
        }

        [HttpPost]
        public JsonResult GenerateReportCSV(string p_ReportGuid)
        {
            try
            {
                int IQAgentReportMaxRecordDisplay = Convert.ToInt32(ConfigurationManager.AppSettings["IQAgentReportMaxRecordDisplay"]);

                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                IQAgentReport_WithoutAuthentication objIQAgentReport_WithoutAuthentication = iqAgentLogic.GetIQAgent_MediaResultReportByReportGuid(p_ReportGuid, IQAgentReportMaxRecordDisplay, false);
                objIQAgentReport_WithoutAuthentication.MediaResults = IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(objIQAgentReport_WithoutAuthentication.MediaResults, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Report);
                ProcessIQAgentMediaResultHighlightingText(objIQAgentReport_WithoutAuthentication.MediaResults);

                string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                string TempCSVPath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Report\\CSV\\" + "IQAgentCSV_" + DateTimeStamp + ".csv";
                string strCSVData = GetCSVDataForReport(objIQAgentReport_WithoutAuthentication);

                string DownloadCSVFileName = objIQAgentReport_WithoutAuthentication.ReportTitle.Replace(" ", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("\\", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("/", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("*", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("?", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace(":", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("\"", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("<", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace(">", "_");
                DownloadCSVFileName = DownloadCSVFileName.Replace("|", "_");

                Session["DownloadCSVFileName"] = DownloadCSVFileName + ".csv";

                bool IsFileGenerated = false;

                using (FileStream fs = new FileStream(TempCSVPath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.Write(strCSVData);
                    }
                }

                if (System.IO.File.Exists(TempCSVPath))
                {
                    IsFileGenerated = true;
                    Session["IQAgentCSVFile"] = TempCSVPath;
                }

                var json = new
                {
                    isSuccess = IsFileGenerated
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

        [HttpGet]
        public ActionResult DownloadCSVFile()
        {
            if (Session["IQAgentCSVFile"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["IQAgentCSVFile"])))
            {
                string CSVFile = Convert.ToString(Session["IQAgentCSVFile"]);
                string DownloadCSVFile = Convert.ToString(Session["DownloadCSVFileName"]);
                if (System.IO.File.Exists(CSVFile))
                {
                    Session.Remove("IQAgentCSVFile");
                    Session.Remove("DownloadCSVFileName");
                    return File(CSVFile, "application/csv", DownloadCSVFile);
                }
            }
            return Content(ConfigSettings.Settings.FileNotAvailable);//"File not available.Please try again later");
        }

        private string GetCSVDataForReport(IQAgentReport_WithoutAuthentication objIQAgentReport_WithoutAuthentication)
        {

            if (objIQAgentReport_WithoutAuthentication != null && objIQAgentReport_WithoutAuthentication.MediaResults != null)
            {
                StringBuilder sb = new StringBuilder();
                string DQ = "\"";

                sb.Append(objIQAgentReport_WithoutAuthentication.ReportTitle);

                // Iterate sub media wise and generate category wise media items

                var subMediaTypes = objIQAgentReport_WithoutAuthentication.MediaResults.Select(c => c.CategoryType).Distinct();

                foreach (string type in subMediaTypes)
                {
                    CommonFunctions.CategoryType categoryType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), type);

                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append(IQMedia.Shared.Utility.CommonFunctions.GetEnumDescription(categoryType));
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);

                    // Build media item header

                    switch (categoryType)
                    {
                        case CommonFunctions.CategoryType.TV:
                            sb.Append("Date,Title,Highlighting Text");
                            break;
                        case CommonFunctions.CategoryType.TW:
                            sb.Append("Date,Title,Highlighting Text,Klout Score,Followers Count,Following Count");
                            break;
                        case CommonFunctions.CategoryType.NM:
                            sb.Append("Date,Title,Highlighting Text");
                            break;
                        case CommonFunctions.CategoryType.SocialMedia:
                        case CommonFunctions.CategoryType.Forum:
                        case CommonFunctions.CategoryType.Blog:
                            sb.Append("Date,Title,Highlighting Text");
                            break;
                        case CommonFunctions.CategoryType.Radio:
                            sb.Append("Date,Title,Highlighting Text");
                            break;
                        default:
                            break;
                    }

                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);

                    foreach (IQMedia.Model.IQAgent_MediaResultsModel item in objIQAgentReport_WithoutAuthentication.MediaResults.Where(c => c.CategoryType == type))
                    {
                        // Append Media Date
                        if (item.MediaDateTime != null)
                        {
                            sb.Append(item.MediaDateTime.ToString("MM/dd/yyyy"));
                        }
                        sb.Append(",");

                        try
                        {
                            switch (categoryType)
                            {
                                case IQMedia.Shared.Utility.CommonFunctions.CategoryType.TV:

                                    IQAgent_TVResultsModel tvModel = item.MediaData as IQAgent_TVResultsModel;

                                    // Title
                                    sb.Append(DQ + tvModel.Title120 + DQ);
                                    sb.Append(",");

                                    //Highlighting Text
                                    sb.Append(DQ + ReplaceHTMLTags(tvModel.higlightedCC) + DQ);

                                    break;

                                case IQMedia.Shared.Utility.CommonFunctions.CategoryType.TW:

                                    IQAgent_TwitterResultsModel twitterModel = item.MediaData as IQAgent_TwitterResultsModel;

                                    // Title
                                    sb.Append(twitterModel.Actor_DisplayName + " " + "@" + " " + twitterModel.Actor_PreferredName);
                                    sb.Append(",");

                                    // Highlighting Text
                                    sb.Append(DQ + ReplaceHTMLTags(twitterModel.Summary) + DQ);
                                    sb.Append(",");

                                    // Klout Score
                                    sb.Append(twitterModel.KlOutScore);
                                    sb.Append(",");

                                    // Folllowers Count
                                    sb.Append(twitterModel.Actor_FollowersCount);
                                    sb.Append(",");

                                    // Friends Count
                                    sb.Append(twitterModel.Actor_FriendsCount);

                                    break;


                                case IQMedia.Shared.Utility.CommonFunctions.CategoryType.NM:

                                    IQAgent_NewsResultsModel newsMediaModel = item.MediaData as IQAgent_NewsResultsModel;

                                    // Title
                                    sb.Append(DQ + newsMediaModel.Title + DQ);
                                    sb.Append(",");

                                    //Highlighting Text
                                    sb.Append(DQ + ReplaceHTMLTags(newsMediaModel.HighlightingText) + DQ);
                                    break;

                                case IQMedia.Shared.Utility.CommonFunctions.CategoryType.SocialMedia:
                                case IQMedia.Shared.Utility.CommonFunctions.CategoryType.Forum:
                                case IQMedia.Shared.Utility.CommonFunctions.CategoryType.Blog:

                                    IQAgent_SMResultsModel socialMediaModel = item.MediaData as IQAgent_SMResultsModel;

                                    // Title
                                    sb.Append(DQ + socialMediaModel.Description + DQ);
                                    sb.Append(",");

                                    //Highlighting Text
                                    sb.Append(DQ + ReplaceHTMLTags(socialMediaModel.HighlightingText) + DQ);

                                    break;
                                case CommonFunctions.CategoryType.Radio:

                                    IQAgent_TVEyesResultsModel iQAgent_TVEyesResultsModel = item.MediaData as IQAgent_TVEyesResultsModel;

                                    // Title
                                    sb.Append(DQ + iQAgent_TVEyesResultsModel.Title + DQ);
                                    sb.Append(",");

                                    //Highlighting Text
                                    sb.Append(DQ + ReplaceHTMLTags(iQAgent_TVEyesResultsModel.HighlightingText) + DQ);

                                    break;
                                default:
                                    sb.Append(",");
                                    sb.Append(",");
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        sb.Append(Environment.NewLine);
                    }
                }

                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private void ProcessIQAgentMediaResultHighlightingText(List<IQAgent_MediaResultsModel> lstIQAgent_MediaResults)
        {
            foreach (IQAgent_MediaResultsModel item in lstIQAgent_MediaResults)
            {
                string highlightedText = string.Empty;
                string highlightedCCOutput = string.Empty;

                CommonFunctions.CategoryType mediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), item.MediaType);

                switch (mediaType)
                {
                    case CommonFunctions.CategoryType.TV:

                        IQAgent_TVResultsModel iQAgent_TVResultsModel = (IQAgent_TVResultsModel)item.MediaData;
                        if (iQAgent_TVResultsModel.highlightedCCOutput != null && iQAgent_TVResultsModel.highlightedCCOutput.CC != null)
                        {
                            highlightedCCOutput = string.Join(" ", iQAgent_TVResultsModel.highlightedCCOutput.CC.Select(c => c.Text));
                        }
                        if (highlightedCCOutput.Length > 300)
                        {
                            highlightedText = highlightedCCOutput.Substring(0, 300);
                            highlightedText = Regex.Replace(highlightedText, "(</span(?!>)|</spa(?!n>)|</sp(?!an>)|</s(?!pan>)|</(?!span>))\\Z", "</span>");
                        }
                        else
                        {
                            highlightedText = highlightedCCOutput;
                        }

                        iQAgent_TVResultsModel.higlightedCC = CommonFunctions.ProcessHighlightingText(highlightedCCOutput, highlightedText);

                        break;

                    case CommonFunctions.CategoryType.NM:
                    case CommonFunctions.CategoryType.LN:
                        IQAgent_NewsResultsModel iQAgent_NewsResultsModel = (IQAgent_NewsResultsModel)item.MediaData;

                        if (iQAgent_NewsResultsModel.HighlightedNewsOutput != null && iQAgent_NewsResultsModel.HighlightedNewsOutput.Highlights != null)
                        {
                            highlightedCCOutput = string.Join(" ", iQAgent_NewsResultsModel.HighlightedNewsOutput.Highlights.Select(c => c));
                        }
                        if (highlightedCCOutput.Length > 300)
                        {
                            highlightedText = highlightedCCOutput.Substring(0, 300);
                            highlightedText = Regex.Replace(highlightedText, "(</em(?!>)|</e(?!m>)|</(?!em>))\\Z", "</em>");
                        }
                        else
                        {
                            highlightedText = highlightedCCOutput;
                        }

                        iQAgent_NewsResultsModel.HighlightingText = CommonFunctions.ProcessHighlightingText(highlightedCCOutput, highlightedText);

                        break;
                    case CommonFunctions.CategoryType.SocialMedia:
                    case CommonFunctions.CategoryType.FB:
                    case CommonFunctions.CategoryType.IG:
                    case CommonFunctions.CategoryType.Blog:
                    case CommonFunctions.CategoryType.Forum:
                        IQAgent_SMResultsModel iQAgent_SMResultsModel = (IQAgent_SMResultsModel)item.MediaData;

                        if (iQAgent_SMResultsModel.HighlightedSMOutput != null && iQAgent_SMResultsModel.HighlightedSMOutput.Highlights != null)
                        {
                            highlightedCCOutput = string.Join(" ", iQAgent_SMResultsModel.HighlightedSMOutput.Highlights.Select(c => c));
                        }
                        if (highlightedCCOutput.Length > 300)
                        {
                            highlightedText = highlightedCCOutput.Substring(0, 300);
                            highlightedText = Regex.Replace(highlightedText, "(</span(?!>)|</s(?!pan>)|</sp(?!an>)|</spa(?!n>)|</(?!span>))\\Z", "</span>");
                        }
                        else
                        {
                            highlightedText = highlightedCCOutput;
                        }
                        iQAgent_SMResultsModel.HighlightingText = CommonFunctions.ProcessHighlightingText(highlightedCCOutput, highlightedText);

                        break;
                    case CommonFunctions.CategoryType.Radio:
                        IQAgent_TVEyesResultsModel iQAgent_TVEyesResultsModel = (IQAgent_TVEyesResultsModel)item.MediaData;
                        highlightedCCOutput = iQAgent_TVEyesResultsModel.HighlightingText.Replace("&gt;", ">").Replace("&lt;", "<");

                         if (highlightedCCOutput.Length > 300)
                        {
                            highlightedText = highlightedCCOutput.Substring(0, 300);
                        }
                        else
                        {
                            highlightedText = highlightedCCOutput;
                        }

                         iQAgent_TVEyesResultsModel.HighlightingText = CommonFunctions.ProcessHighlightingText(highlightedCCOutput, highlightedText);
                        break;
                    case CommonFunctions.CategoryType.PQ:

                        IQAgent_PQResultsModel iQAgent_PQResultsModel = (IQAgent_PQResultsModel)item.MediaData;

                        if (iQAgent_PQResultsModel.HighlightedPQOutput != null && iQAgent_PQResultsModel.HighlightedPQOutput.Highlights != null)
                        {
                            highlightedCCOutput = string.Join(" ", iQAgent_PQResultsModel.HighlightedPQOutput.Highlights.Select(c => c));
                        }
                        if (highlightedCCOutput.Length > 300)
                        {
                            highlightedText = highlightedCCOutput.Substring(0, 300);
                            highlightedText = Regex.Replace(highlightedText, "(</span(?!>)|</s(?!pan>)|</sp(?!an>)|</spa(?!n>)|</(?!span>))\\Z", "</span>");
                        }
                        else
                        {
                            highlightedText = highlightedCCOutput;
                        }
                        iQAgent_PQResultsModel.HighlightingText = CommonFunctions.ProcessHighlightingText(highlightedCCOutput, highlightedText);

                        break;
                    default:
                        break;
                }
            }
        }

        private string ReplaceHTMLTags(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string HighlighedText = Regex.Replace(value, @"<(.|\n)*?>", string.Empty);
                HighlighedText = HighlighedText.Replace("\"", "\"\"");
                return HighlighedText;
            }
            else
                return string.Empty;
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
    }
}
