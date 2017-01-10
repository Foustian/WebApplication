using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.WebApplication.Models.TempData;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.Shared.Utility;
using IQMedia.Model;
using System.IO;
using IQMedia.WebApplication.Models;
using System.Text.RegularExpressions;
using System.Threading;
using IQMedia.WebApplication.Config.Sections;
using IQMedia.WebApplication.Config;
using System.Configuration;
using IQMedia.Common.Util;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class IFrameMicrositeController : Controller
    {
        #region Member Variables

        private int MaxRows = 5;
        private int MinRows = 2;

        private int MaxCols = 10;
        private int MinCols = 4;

        Int64 p_TotalRecordsClipCount = 0;

         

        private enum MQueryString
        {
            ClientID,
            Title,
            CustID,
            Cat,
            SubCat1,
            SubCat2,
            SubCat3,
            Rows,
            Cols,
            Sort
        }

        public enum MicroMyIQSortFeilds
        {
            ClipDate,
            ClipCreationDate,
            ClipTitle
        }

        #endregion

        #region POST/GET Methods

        public ActionResult Index(string st = "", int pn = 0)
        {
            Dictionary<string, object> dictResult = new Dictionary<string, object>();
            try
            {
                Log4NetLogger.Info("Start IframeMircosite Load");
                List<IFrameMicrositeModel> lstIFrameMicrositeModel = null;

                var cols = 0;
                var rows = 0;
                var hasMSDownloadRight = false;
                Guid? clipID = null;

                if (Request.QueryString[MQueryString.ClientID.ToString()] != null)
                {

                    Log4NetLogger.Info("Microsite Input Request Params are :");
                    Log4NetLogger.Info("ClientID : " + Request.QueryString[MQueryString.ClientID.ToString()]);
                    Log4NetLogger.Info("CustID : " + Request.QueryString[MQueryString.CustID.ToString()]);
                    Log4NetLogger.Info("Cat : " + Request.QueryString[MQueryString.Cat.ToString()]);
                    Log4NetLogger.Info("SubCat1 : " + Request.QueryString[MQueryString.SubCat1.ToString()]);
                    Log4NetLogger.Info("SubCat2 : " + Request.QueryString[MQueryString.SubCat2.ToString()]);
                    Log4NetLogger.Info("SubCat3 : " + Request.QueryString[MQueryString.SubCat3.ToString()]);
                    Log4NetLogger.Info("Title : " + Request.QueryString[MQueryString.Title.ToString()]);
                    Log4NetLogger.Info("Rows : " + Request.QueryString[MQueryString.Rows.ToString()]);
                    Log4NetLogger.Info("Cols : " + Request.QueryString[MQueryString.Cols.ToString()]);
                    Log4NetLogger.Info("Sort : " + Request.QueryString[MQueryString.Sort.ToString()]);

                    ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                    hasMSDownloadRight = (clientLogic.GetClientRoleByClientGUIDRoleName(new Guid(Request.QueryString[MQueryString.ClientID.ToString()].Trim()), IQMedia.Shared.Utility.CommonFunctions.Roles.MicrositeDownload.ToString()) == 1 ? true : false);

                    if (clientLogic.GetClientRoleByClientGUIDRoleName(new Guid(Request.QueryString[MQueryString.ClientID.ToString()].Trim()), IQMedia.Shared.Utility.CommonFunctions.Roles.IframeMicrosite.ToString()) == 1)
                    {
                        var clientGUID = new Guid(Request.QueryString[MQueryString.ClientID.ToString()].Trim());
                        rows = (Request.QueryString[MQueryString.Rows.ToString()] != null && (Convert.ToInt32(Request.QueryString[MQueryString.Rows.ToString()].Trim()) <= MaxRows && Convert.ToInt32(Request.QueryString[MQueryString.Rows.ToString()].Trim()) >= MinRows)) ? Convert.ToInt32(Request.QueryString[MQueryString.Rows.ToString()].Trim()) : MinRows;
                        cols = (Request.QueryString[MQueryString.Cols.ToString()] != null && (Convert.ToInt32(Request.QueryString[MQueryString.Cols.ToString()].Trim()) <= MaxCols && Convert.ToInt32(Request.QueryString[MQueryString.Cols.ToString()].Trim()) >= MinCols)) ? Convert.ToInt32(Request.QueryString[MQueryString.Cols.ToString()].Trim()) : MinCols;
                        var clipTitle = Request.QueryString[MQueryString.Title.ToString()] ?? string.Empty;
                        var customerGUIDs = Request.QueryString[MQueryString.CustID.ToString()] == null ? string.Empty : "'" + string.Join("','",Request.QueryString[MQueryString.CustID.ToString()].Split(',').Select(a=>a.Trim())) + "'";
                        var categories = Request.QueryString[MQueryString.Cat.ToString()] == null ? string.Empty : "'" + string.Join("','",Request.QueryString[MQueryString.Cat.ToString()].Split(',').Select(a=>a.Trim())) + "'";
                        var subCat1 = Request.QueryString[MQueryString.SubCat1.ToString()] == null ? string.Empty : "'" + string.Join("','", Request.QueryString[MQueryString.SubCat1.ToString()].Split(',').Select(a => a.Trim())) + "'";
                        var subCat2 = Request.QueryString[MQueryString.SubCat2.ToString()] == null ? string.Empty : "'" + string.Join("','", Request.QueryString[MQueryString.SubCat2.ToString()].Split(',').Select(a => a.Trim())) + "'";
                        var subCat3 = Request.QueryString[MQueryString.SubCat3.ToString()] == null ? string.Empty : "'" + string.Join("','", Request.QueryString[MQueryString.SubCat3.ToString()].Split(',').Select(a => a.Trim())) + "'";
                        var sort = Request.QueryString[MQueryString.Sort.ToString()] ?? string.Empty;

                        string searchTerm = string.IsNullOrEmpty(st) ? string.Empty : st.Trim();

                        var IsSortDirecitonAsc = false;

                        if (sort.Contains("-"))
                        {
                            sort = sort.Replace("-", "");
                            IsSortDirecitonAsc = false;
                        }
                        else
                        {
                            sort = sort.Replace("+", "");
                            IsSortDirecitonAsc = true;
                        }

                        MicroMyIQSortFeilds tempVar = new MicroMyIQSortFeilds();

                        if (!Enum.TryParse(sort, true, out tempVar))
                        {
                            IsSortDirecitonAsc = false;
                            sort = "ClipCreationDate";
                        }                       

                        IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                        lstIFrameMicrositeModel = iQArchieveLogic.GetArchiveClipByParams(clientGUID, categories, subCat1, subCat2, subCat3, customerGUIDs, pn, (rows * cols), sort, IsSortDirecitonAsc, searchTerm, clipTitle, out clipID, out p_TotalRecordsClipCount, HttpContext.Request.Url.Host);                        
                    }
                }

                IFrameMicrositeData iFrameMicrositeData = new IFrameMicrositeData();
                iFrameMicrositeData.MaxCols = cols;
                iFrameMicrositeData.MaxRows = rows;
                iFrameMicrositeData.lstIFrameMicrositeModel = lstIFrameMicrositeModel;
                iFrameMicrositeData.HasMSDownloadRight = hasMSDownloadRight;

                ViewBag.IsSuccess = true;
                if (!Request.IsAjaxRequest())
                {
                    Log4NetLogger.Info("END IframeMircosite Load");
                    dictResult.Add("Result", iFrameMicrositeData);
                    dictResult.Add("MaxWidth", cols * 140);
                    dictResult.Add("CurrentPageNumber", 0);
                    dictResult.Add("HasMoreResult", HasMoreData(p_TotalRecordsClipCount, (rows*cols), 0));
                    dictResult.Add("ClipID", clipID);
                }
                else
                {
                    Log4NetLogger.Info("END IframeMircosite Load");
                    return Json(new
                    {
                        hasMoreResults = HasMoreData(p_TotalRecordsClipCount, (rows*cols), pn),
                        hasPreviouResult = pn > 0,
                        HTML = RenderPartialToString("~/Views/IFrameMicrosite/_Results.cshtml", iFrameMicrositeData),
                        MaxWidth = cols * 140,
                        isSuccess = true
                    });
                }
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                Log4NetLogger.Error("END IframeMircosite Load with exception", ex);
                ViewBag.IsSuccess = false;

                if (Request.IsAjaxRequest())
                {
                    return Json(new
                    {
                        isSuccess = false
                    });
                }
            }
            finally
            {
                TempData.Keep();
            }

            return View(dictResult);
        }

        public JsonResult DownloadClip(string p_ClipGuid)
        {
            try
            {
                // when download request starts , then first this ajax call made 
                // to check if file is exist and available at download location or not!!
                // there is no log for this ajax request in client's given log file.  
                Log4NetLogger.Info("Start Download Clip for Clip ID :" + p_ClipGuid);
                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);

                Log4NetLogger.Info("Fetch Clip FilePath by ClipGuid :" + p_ClipGuid + " And ClientGuid : " + Request.QueryString[MQueryString.ClientID.ToString()]);

                Dictionary<string, string> dictResult = iQArchieveLogic.GetClipPathByClipGUID(p_ClipGuid, new Guid(Request.QueryString[MQueryString.ClientID.ToString()].Trim()));

                Log4NetLogger.Info("Fetched Clip FilePath");

                if (dictResult.Count > 0)
                {
                    Log4NetLogger.Info("File Path :" + dictResult["FilePath"]);
                    if (!string.IsNullOrEmpty(dictResult["FilePath"]))
                    {
                        FileInfo _FileInfo = new FileInfo(dictResult["FilePath"]);

                        Log4NetLogger.Info("Check if File Exist at location");

                        if (_FileInfo.Exists && _FileInfo.Length > 0)
                        {
                            TempData["FilePath"] = dictResult["FilePath"];
                            Log4NetLogger.Info("File Exist return true for download");
                            var json = new
                            {
                                isSuccess = true
                            };
                            return Json(json);
                        }
                        else
                        {
                            Log4NetLogger.Info("File not exist at location. return false for download");
                            var json = new
                            {
                                isSuccess = false,
                                message = ConfigSettings.Settings.FileNotAvailable
                            };
                            return Json(json);
                        }
                    }
                    else
                    {
                        Log4NetLogger.Info("File path is empty. return false for download");
                        var json = new
                        {
                            isSuccess = false,
                            message = ConfigSettings.Settings.ClipNotAvailableForDownload
                        };
                        return Json(json);
                    }
                }
                else
                {
                    Log4NetLogger.Info("clip data not exist. return false for download");
                    var json = new
                    {
                        isSuccess = false,
                        message = ConfigSettings.Settings.ClipNotAvailableForDownload
                    };
                    return Json(json);
                }
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                Log4NetLogger.Error("exception occred for clip, return false for download", ex);
                var json = new
                {
                    isSuccess = false,
                    message = ConfigSettings.Settings.ErrorOnDownloadClip
                };
                return Json(json);
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpGet]
        public ActionResult DownloadClip(string p_ClipGUID, string p_ClipTitle)
        {
            Log4NetLogger.Info("Start Download Clip for Clip ID :" + p_ClipGUID + ",  Clip Title : " + p_ClipTitle);
            Log4NetLogger.Info("Client Guid : " + Request.QueryString[MQueryString.ClientID.ToString()]);
            string returnMessage = string.Empty;
            try
            {

                //Log4NetLogger.Info("going to fetch clip path by clipguid and clientguid");

                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                //Dictionary<string, string> dictResult = iQArchieveLogic.GetClipPathByClipGUID(p_ClipGUID, new Guid(Request.QueryString[MQueryString.ClientID.ToString()].Trim()));

                //Log4NetLogger.Info("Fetch Clip FilePath by ClipGuid :" + p_ClipGUID + " And ClientGuid : " + Request.QueryString[MQueryString.ClientID.ToString()]);

                if (TempData["FilePath"] != null && !string.IsNullOrEmpty(Convert.ToString(TempData["FilePath"])))
                {
                    Log4NetLogger.Info("File Path :" + Convert.ToString(TempData["FilePath"]));
                    FileInfo _FileInfo = new FileInfo(Convert.ToString(TempData["FilePath"]));

                    Log4NetLogger.Info("Check if File Exist at location");

                    if (_FileInfo.Exists && _FileInfo.Length > 0)
                    {

                        Log4NetLogger.Info("file exist, update clip update count");

                        
                        iQArchieveLogic.UpdateDownloadCountByClipGUID(p_ClipGUID);

                        Log4NetLogger.Info("download clip with clip title, format clip title for file name");

                        p_ClipTitle = !string.IsNullOrEmpty(p_ClipTitle) ? Regex.Replace(p_ClipTitle.Trim().Replace("\"", "_"), @"[\/?:*|<>, ]", "_") : string.Empty;
                        //p_ClipTitle = Regex.Replace(p_ClipTitle.Trim().Replace("\"", "_"), @"[\/?:*|<>, ]", "_");

                        Log4NetLogger.Info("return file with clip title as file name");
                        return File(_FileInfo.FullName, ReturnExtension(_FileInfo.Extension.ToLower()), p_ClipTitle + _FileInfo.Extension);
                    }
                    else
                    {
                        Log4NetLogger.Info("file not exist, return file not exist");

                        return Content(ConfigSettings.Settings.ClipNotAvailableForDownload);
                    }
                    
                }
                else
                {
                    Log4NetLogger.Info("file path is empty, return file not exist");
                    return Content(ConfigSettings.Settings.ClipNotAvailableForDownload);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("exception occred for clip download", ex);
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Content(ConfigSettings.Settings.ErrorOnDownloadClip);
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpPost]
        public JsonResult LoadClipPlayer(string ClipID)
        {
            try
            {
                if ((Request.UserAgent.ToLower().Contains("ipad") || Request.UserAgent.ToLower().Contains("iphone") || Request.UserAgent.ToLower().Contains("ipod")) || (Request.UserAgent.ToLower().Contains("android") && Utility.CommonFunctions.CheckVersion()))
                {
                    Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " IOS Device");

                    string url = string.Format(ConfigurationManager.AppSettings["IOSSVCGetVarsUrl"], ClipID);

                    string respone = IQMedia.Shared.Utility.CommonFunctions.DoHttpGetRequest(url);

                    Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " IOSGetVars Response : " + respone);

                    Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();
                    jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(respone.Replace(@"\", string.Empty).Replace("([", string.Empty).Replace("])", string.Empty).Replace("\"", "'"));

                    Shared.Utility.Log4NetLogger.Debug(Session.SessionID + " Parsed Json : " + string.Join(",", jsonData.ToString()));

                    if (Convert.ToBoolean(jsonData["IsValidMedia"]))
                    {
                        string media = Convert.ToString(jsonData["Media"]);
                        var json = new
                        {
                            isSuccess = true,
                            RedirectUrl = media + ".m3u8",
                            isRedirect = true
                        };

                        return Json(json);
                    }
                }
                else
                {
                    string ServiceBaseURL = Convert.ToString(ConfigurationManager.AppSettings["ServicesBaseURL"]);
                    bool IsPlayFromLocal = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]);
                    string PlayerLogo = string.Empty;                    

                    // Generate Clip Player Object
                    string ClipPlayer = UtilityLogic.RenderClipPlayer(ClipID, ServiceBaseURL, IsPlayFromLocal, Request.QueryString[MQueryString.ClientID.ToString()].Trim(), Request.Browser.Type, true, false);

                    // Get Closed Caption from ArchiveClip table
                    IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                    string ClosedCaption = string.Empty;
                    IQArchive_ArchiveClipModel clip = iQArchieveLogic.GetArchiveClipByClipID(ClipID);
                    if (clip != null)
                    {
                        ClosedCaption = Server.HtmlDecode(clip.ClosedCaption);
                    }

                    var json = new
                    {
                        isSuccess = true,
                        clipHTML = ClipPlayer,
                        closedCaption = ClosedCaption,
                        isRedirect = false
                    };

                    return Json(json);
                }
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
            finally
            {
                TempData.Keep();
            }
            
            return Json(new object());
        }

        #endregion

        #region Custom Methods

        private List<IFrameMicrositeModel> GetClientClip(string p_SearchTerm = "", int p_CurrentPage = 0)
        {
            List<IFrameMicrositeModel> _ListOfIFrameMicrositeModel = null;
            try
            {
                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                if (Request.QueryString[MQueryString.ClientID.ToString().Trim()] != null)
                {
                    

                    Guid? _ClipID = null;

                    
                }
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                throw ex;
            }
            finally
            {
                TempData.Keep();
            }
            
            return _ListOfIFrameMicrositeModel;
        }

        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".mp3": return "audio/mpeg3";
                case ".mp4": return "video/mp4";
                case ".mpeg": return "video/mpeg";
                case ".mov": return "video/quicktime";
                case ".wmv":
                case ".avi": return "video/x-ms-wmv";
                //and so on          
                default: return "application/octet-stream";
            }

        }

        #endregion

        #region Utility Method

        public string RenderPartialToString(string viewName, object model)
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

        public bool HasMoreData(Int64 p_TotalResults, Int32 p_PageSize, Int32 p_CurrentPage)
        {
            double totalPages = Convert.ToDouble(p_TotalResults) / p_PageSize;
            if (totalPages > (p_CurrentPage + 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }

}
