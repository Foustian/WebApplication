using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using System.Configuration;
using IQMedia.Model;
using System.IO;
using System.Net;
using IQMedia.WebApplication.Utility;
using IQMedia.WebApplication.Config;
using IQMedia.Shared;
using Newtonsoft.Json.Linq;
using IQMedia.Common.Util;

namespace IQMedia.WebApplication.Controllers
{
    [CheckForDownloadRights()]
    [CheckAuthentication()]    
    public class DownloadController : Controller
    {
        //
        // GET: /Download/

        // Assign this from Session

        public string CustomerGUID
        {
            get
            {
                ActiveUser sessionInformation = ActiveUserMgr.GetActiveUser();
                return sessionInformation.CustomerGUID.ToString();
            }
        }

        public ActionResult Index(string id, string type)
        {
            try
            {
                ViewBag.IsSuccess = true;
                long ID = 0;
                Session["id"] = id;
                Session["type"] = type;
                Session["IsDownloadLimitExceed"] = false;

                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type) && Int64.TryParse(id, out ID))
                {
                    bool IsDownloadLimitExceed = false;

                    if (type.ToUpper() == "NM")
                    {
                        NMLogic NMLogic = (NMLogic)LogicFactory.GetLogic(LogicType.NM);
                        int DownloadLimit = Convert.ToInt32(ConfigurationManager.AppSettings["DownloadLimitNM"]);
                        int count = NMLogic.SelectDownloadLimit(CustomerGUID);

                        IsDownloadLimitExceed = (count >= DownloadLimit);

                    }
                    else if (type.ToUpper() == "SM")
                    {
                        SMLogic SMLogic = (SMLogic)LogicFactory.GetLogic(LogicType.SM);
                        int DownloadLimit = Convert.ToInt32(ConfigurationManager.AppSettings["DownloadLimitSM"]);
                        int count = SMLogic.SelectDownloadLimit(CustomerGUID);

                        IsDownloadLimitExceed = (count >= DownloadLimit);
                    }
                    else if (type.ToUpper() == "TV")
                    {
                        TVLogic TVLogic = (TVLogic)LogicFactory.GetLogic(LogicType.TV);
                        int DownloadLimit = Convert.ToInt32(ConfigurationManager.AppSettings["DownloadLimitTV"]);
                        int count = TVLogic.SelectDownloadLimit(CustomerGUID);

                        IsDownloadLimitExceed = (count >= DownloadLimit);
                    }
                    else if (type.ToUpper() == "TVEYES")
                    {
                        TVEyesLogic TVEyesLogic = (TVEyesLogic)LogicFactory.GetLogic(LogicType.TVEyes);
                        int DownloadLimit = Convert.ToInt32(ConfigurationManager.AppSettings["DownloadLimitTVEyes"]);
                        int count = TVEyesLogic.SelectDownloadLimit(new Guid(CustomerGUID));

                        IsDownloadLimitExceed = (count >= DownloadLimit);
                    }
                    else if (type.ToUpper() == "RADIO")
                    {
                        RadioLogic RadioLogic = (RadioLogic)LogicFactory.GetLogic(LogicType.Radio);
                        int DownloadLimit = Convert.ToInt32(ConfigurationManager.AppSettings["DownloadLimitIQRadio"]);
                        int count = RadioLogic.SelectDownloadLimit(CustomerGUID);

                        IsDownloadLimitExceed = count >= DownloadLimit;
                    }

                    if (IsDownloadLimitExceed)
                    {
                        ViewBag.IsError = false;
                        ViewBag.IsDownloadLimitExceed = true;
                        Session["IsDownloadLimitExceed"] = true;

                        return View();
                    }
                    else
                    {
                        return RedirectToAction("TermsAndCondition", "Download");
                    }
                }
                else
                {
                    ViewBag.IsError = true;
                    return View();
                }
            }
            catch (Exception exception)
            {
                ViewBag.IsSuccess = false;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return View();
            }
                

        }

        [HttpGet]
        public ActionResult TermsAndCondition()
        {
           
            ViewBag.Content = GetTermsConditionFileContent();
            return View();
        }

        [HttpPost]
        public ActionResult TermsAndCondition(FormCollection frmCollection)
        {           

            long ID = 0;
            string id = Convert.ToString(Session["id"]);
            string type = Convert.ToString(Session["type"]);
            bool IsDownloadLimitExceed =  Convert.ToBoolean(Session["IsDownloadLimitExceed"]);

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type) && Int64.TryParse(id, out ID))
            {
                if (type.ToUpper() == "NM")
                {
                    if (!IsDownloadLimitExceed)
                    {
                        NMLogic NMLogic = (NMLogic)LogicFactory.GetLogic(LogicType.NM);
                        NMLogic.Insert_ArticleNMDownload(CustomerGUID, Convert.ToInt64(ID));
                    }

                    return RedirectToAction("NM", "Download", new { id = id });
                }
                else if (type.ToUpper() == "SM")
                {
                    if (!IsDownloadLimitExceed)
                    {
                        SMLogic SMLogic = (SMLogic)LogicFactory.GetLogic(LogicType.SM);
                        SMLogic.Insert_ArticleSMDownload(CustomerGUID, Convert.ToInt64(ID));
                    }
                    return RedirectToAction("SM", "Download", new { id = id });
                }
                else if (type.ToUpper() == "TV")
                {
                    if (!IsDownloadLimitExceed)
                    {
                        TVLogic TVLogic = (TVLogic)LogicFactory.GetLogic(LogicType.TV);
                        TVLogic.Insert_ClipDownload(CustomerGUID, Convert.ToInt64(ID));
                    }
                    return RedirectToAction("TV", "Download", new { id = id });
                }
                else if (type.ToUpper() == "TVEYES")
                {
                    if (!IsDownloadLimitExceed)
                    {
                        TVEyesLogic TVEyesLogic = (TVEyesLogic)LogicFactory.GetLogic(LogicType.TVEyes);
                        TVEyesLogic.Insert_ArticleTVEyesDownload(new Guid(CustomerGUID), Convert.ToInt64(ID));
                    }
                    return RedirectToAction("TVEyes", "Download", new { id = id });
                }
                else if (type.ToUpper() == "RADIO")
                {
                    if (!IsDownloadLimitExceed)
                    {
                        RadioLogic RadioLogic = (RadioLogic)LogicFactory.GetLogic(LogicType.Radio);
                        RadioLogic.Insert_ArticleRadioDownload(CustomerGUID, Convert.ToInt64(ID));
                    }
                    return RedirectToAction("Radio", "Download", new { id = id });
                }
                else
                {
                    return RedirectToAction("TermsAndCondition", "Download");
                }
            }
            else
            {
                return RedirectToAction("TermsAndCondition", "Download");
            }
        }

        #region NM

        [HttpGet]
        public ActionResult NM(string id)
        {
            NMLogic NMLogic = (NMLogic)LogicFactory.GetLogic(LogicType.NM);

            List<ArticleNMDownload> lstArticleNMDownload = NMLogic.SelectArticleNMDownloadByCustomer(CustomerGUID);

            return View(lstArticleNMDownload);
        }

        [HttpPost]
        public ActionResult NM(FormCollection frmCollection)
        {
            NMLogic NMLogic = (NMLogic)LogicFactory.GetLogic(LogicType.NM);

            List<ArticleNMDownload> lstArticleNMDownload = NMLogic.SelectArticleNMDownloadByCustomer(CustomerGUID);

            return View(lstArticleNMDownload);
        }


        [HttpGet]
        public ActionResult DownloadNMFile(long NMDownloadID, string id)
        {

            NMLogic NMLogic = (NMLogic)LogicFactory.GetLogic(LogicType.NM);
            ArticleNMDownload objArticleNMDownload = NMLogic.SelectArticleNMByID(NMDownloadID, new Guid(CustomerGUID));
            if (objArticleNMDownload != null)
            {
                if (!string.IsNullOrEmpty(objArticleNMDownload.DownloadLocation) && System.IO.File.Exists(objArticleNMDownload.DownloadLocation))
                {
                    NMLogic.UpdateDownloadStatusByID(NMDownloadID, new Guid(CustomerGUID));
                    return File(objArticleNMDownload.DownloadLocation, "application/pdf", Path.GetFileName(objArticleNMDownload.DownloadLocation));
                }
                else
                {
                    return Content(ConfigSettings.Settings.FileNotAvailableForDownload);// "File is not available to dowload");
                }
            }
            else
            {
                return Content(ConfigSettings.Settings.ErrorOnDownloadFile);// "Some error occurred while downloading file");
            }
        }

        #endregion

        #region SM

        [HttpGet]
        public ActionResult SM(string id)
        {
            SMLogic SMLogic = (SMLogic)LogicFactory.GetLogic(LogicType.SM);

            List<ArticleSMDownload> lstArticleSMDownload = SMLogic.SelectArticleSMDownloadByCustomer(CustomerGUID);

            return View(lstArticleSMDownload);
        }

        [HttpPost]
        public ActionResult SM(FormCollection frmCollection)
        {
            SMLogic SMLogic = (SMLogic)LogicFactory.GetLogic(LogicType.SM);

            List<ArticleSMDownload> lstArticleSMDownload = SMLogic.SelectArticleSMDownloadByCustomer(CustomerGUID);

            return View(lstArticleSMDownload);
        }

        [HttpGet]
        public ActionResult DownloadSMFile(long SMDownloadID, string id)
        {
            SMLogic SMLogic = (SMLogic)LogicFactory.GetLogic(LogicType.SM);
            ArticleSMDownload objArticleSMDownload = SMLogic.SelectArticleSMByID(SMDownloadID, new Guid(CustomerGUID));
            if (objArticleSMDownload != null)
            {
                if (!string.IsNullOrEmpty(objArticleSMDownload.DownloadLocation) && System.IO.File.Exists(objArticleSMDownload.DownloadLocation))
                {
                    SMLogic.UpdateDownloadStatusByID(SMDownloadID, new Guid(CustomerGUID));
                    return File(objArticleSMDownload.DownloadLocation, "application/pdf", Path.GetFileName(objArticleSMDownload.DownloadLocation));
                }
                else
                {
                    return Content(ConfigSettings.Settings.FileNotAvailableForDownload);// "File is not available to dowload");
                }
            }
            else
            {
                return Content(ConfigSettings.Settings.ErrorOnDownloadFile);//"Some error occurred while downloading file");
            }
        }

        #endregion

        #region TV

        [HttpGet]
        public ActionResult TV(string id)
        {
            ViewBag.ClipFormats = new SelectList(GetClipFormats());

            try
            {
                TVLogic TVLogic = (TVLogic)LogicFactory.GetLogic(LogicType.TV);

                List<ClipDownload> lstClipDownload = TVLogic.SelectClipDownloadByCustomer(CustomerGUID);

                TVCheckDownloadStatus(lstClipDownload);

                return View(lstClipDownload);
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
                List<ClipDownload> lstClipDownload = new List<ClipDownload>();
                return View(lstClipDownload);
            }
        }

        [HttpPost]
        public ActionResult TV(List<ClipDownload> ClipCollection)
        {
            ViewBag.ClipFormats = new SelectList(GetClipFormats());

            try
            {
                TVCheckDownloadStatus(ClipCollection);

                TVLogic TVLogic = (TVLogic)LogicFactory.GetLogic(LogicType.TV);
                List<ClipDownload> lstClipDownload = TVLogic.SelectClipDownloadByCustomer(CustomerGUID);                

                return View(lstClipDownload);
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
                List<ClipDownload> lstClipDownload = new List<ClipDownload>();
                return View("TV", lstClipDownload);
            }
        }

        public void TVCheckDownloadStatus(List<ClipDownload> ClipCollection)
        {
            try
            {
                TVLogic TVLogic = (TVLogic)LogicFactory.GetLogic(LogicType.TV);

                if (ClipCollection != null)
                {
                    foreach (ClipDownload objClip in ClipCollection)
                    {
                        if (!string.IsNullOrEmpty(objClip.ClipFormat))
                        {
                            ClipDownload objClipDownload_Temp = TVLogic.SelectByClipDownloadKey(objClip.ID, new Guid(CustomerGUID));
                            string clipGUID = objClipDownload_Temp.ClipGUID;

                            string fileName = clipGUID + "." + objClip.ClipFormat;

                            // Case 1: Check into IQCore_ClipMeta table

                            string _IQCore_ClipMeta_FileLocation = TVLogic.SelectClipLocationFromIQCore_Meta(objClip.ID, new Guid(CustomerGUID));

                            string fileLocation = string.Empty;

                            bool IsClipLocationFound = false;

                            if (!string.IsNullOrWhiteSpace(_IQCore_ClipMeta_FileLocation))
                            {
                                _IQCore_ClipMeta_FileLocation = !_IQCore_ClipMeta_FileLocation.EndsWith(@"\") ? _IQCore_ClipMeta_FileLocation + @"\" : _IQCore_ClipMeta_FileLocation;
                                fileLocation = _IQCore_ClipMeta_FileLocation;
                            }

                            // Case 2: Check into web.config file

                            if (string.IsNullOrEmpty(fileLocation) && !string.IsNullOrEmpty(clipGUID))
                            {
                                fileLocation = Convert.ToString(ConfigurationManager.AppSettings["ClipDownloadLocation"]);
                            }

                            if (!string.IsNullOrEmpty(clipGUID) && System.IO.File.Exists(fileLocation + fileName))
                            {
                                FileInfo file = new FileInfo(fileLocation + fileName);
                                if (file.Length > 0)
                                {
                                    UpdateClipDownload(objClip.ID, fileLocation, objClip.ClipFormat, 3);
                                    IsClipLocationFound = true;
                                }                                
                            }                            

                            // Case 3: Check into IQService_Export && IQRemoteService_Export table

                            if (!IsClipLocationFound)
                            {
                                bool IsClipLocationAvailable = TVLogic.CheckIntoIQServiceAndIQRemoetService_Export(clipGUID, objClip.ClipFormat);
                                if (IsClipLocationAvailable)
                                {
                                    UpdateClipDownload(objClip.ID, null, objClip.ClipFormat, 2);
                                    IsClipLocationFound = true;
                                }
                            }

                            // Case 4: Made HttpWebRequest and find response

                            if (!IsClipLocationFound)
                            {
                                string clipURL = Convert.ToString(ConfigurationManager.AppSettings["ExportClip"]);
                                //string clipURL = Convert.ToString(ConfigurationManager.AppSettings["ExportMediaClip"]);
                                string clipResponse = ConfigSettings.Settings.ExportClipMsg;// Convert.ToString(ConfigurationManager.AppSettings["ExportClipMsg"]);
                                string response = string.Empty;

                                //clipURL += clipGUID + "&fmt=" + objClip.ClipFormat;
                                clipURL += "?fid=" + clipGUID + "&fmt=" + objClip.ClipFormat;

                                HttpWebRequest objRequest = null;
                                HttpWebResponse objResponse = null;

                                objRequest = (HttpWebRequest)WebRequest.Create(clipURL);
                                objRequest.Method = "GET";
                                objRequest.Timeout = 300000;
                                objRequest.ReadWriteTimeout = 300000;
                                //objRequest.Headers.Add("Origin", "http://qav4.iqmediacorp.com/");

                                objResponse = (HttpWebResponse)objRequest.GetResponse();
                                //JObject jsonData = null;
                                if (objResponse.StatusCode == HttpStatusCode.OK)
                                {
                                    StreamReader _Response = new StreamReader(objResponse.GetResponseStream(), System.Text.Encoding.Default);
                                    response = _Response.ReadToEnd();
                                    //jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                                }

                                //if (jsonData != null && string.Compare("0", jsonData["Status"].ToString(), true) == 0)
                                if (string.Compare(clipResponse, response, true) == 0)
                                {
                                    UpdateClipDownload(objClip.ID, null, objClip.ClipFormat, 2);
                                    IsClipLocationFound = true;
                                }
                            }

                            if (!IsClipLocationFound)
                            {
                                ViewBag.ErrorMessage = ConfigSettings.Settings.ClipUnableToEnqueForDownload;
                            }
                            else
                            {
                                ViewBag.ErrorMessage = string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                IQMedia.Shared.Utility.Log4NetLogger.Error(ex.ToString());
            }
        }

        [HttpGet]
        public ActionResult ClipDelete(long ClipDownloadKey)
        {
            try
            {
                TVLogic TVLogic = (TVLogic)LogicFactory.GetLogic(LogicType.TV);
                TVLogic.Delete_ClipDownload(CustomerGUID, ClipDownloadKey);

                return RedirectToAction("TV", new { id = string.Empty });
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
                return RedirectToAction("TV", new { id = string.Empty });
            }
        }

        [HttpGet]
        public ActionResult DownloadClip(long ClipDownloadKey)
        {
            try
            {
                TVLogic TVLogic = (TVLogic)LogicFactory.GetLogic(LogicType.TV);
                ClipDownload objClipDownload = TVLogic.SelectByClipDownloadKey(ClipDownloadKey, new Guid(CustomerGUID));
                if (objClipDownload != null)
                {
                    if (!string.IsNullOrWhiteSpace(objClipDownload.ClipFileLocation) && !string.IsNullOrWhiteSpace(objClipDownload.ClipFormat))
                    {
                        string fileName = objClipDownload.ClipFileLocation + objClipDownload.ClipGUID + "." + objClipDownload.ClipFormat;

                        if (System.IO.File.Exists(fileName))
                        {
                            UpdateClipDownload(ClipDownloadKey, null, null, 4);

                            string fileContentType = IQMedia.Shared.Utility.CommonFunctions.GetFileContentTypeByExtension(objClipDownload.ClipFormat);

                            return File(fileName, fileContentType, Path.GetFileName(objClipDownload.ClipGUID + "." + objClipDownload.ClipFormat));
                        }
                        else
                        {
                            return Content(ConfigSettings.Settings.ClipNotAvailableForDownload);// "Clip is not available to dowload");
                        }
                    }
                    else
                    {
                        return Content(ConfigSettings.Settings.ClipNotAvailableForDownload);//"Clip is not available to dowload");
                    }
                }
                else
                {
                    return Content(ConfigSettings.Settings.ErrorOnDownloadClip);//"Some error occurred while downloading clip");
                }
            }
            catch (Exception ex)
            {
                Shared.Utility.Log4NetLogger.Error(ex);
                return Content(ConfigSettings.Settings.ErrorOnDownloadClip);
            }
            finally
            {
                TempData.Keep();
            }
        }

        #endregion

        #region Radio

        [HttpGet]
        public ActionResult Radio(string id)
        {
            ViewBag.ClipFormats = new SelectList(GetClipFormats());

            try
            {
                RadioLogic RadioLogic = (RadioLogic)LogicFactory.GetLogic(LogicType.Radio);
                List<ArticleRadioDownload> lstDownloads = RadioLogic.SelectArticleRadioDownloadByCustomer(CustomerGUID);

                RadioCheckDownloadStatus(lstDownloads);

                return View(lstDownloads);
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
                List<ArticleRadioDownload> lstDownloads = new List<ArticleRadioDownload>();
                return View(lstDownloads);
            }
        }

        [HttpPost]
        public ActionResult Radio(List<ArticleRadioDownload> ClipCollection)
        {
            ViewBag.ClipFormats = new SelectList(GetClipFormats());

            try
            {
                RadioCheckDownloadStatus(ClipCollection);

                RadioLogic RadioLogic = (RadioLogic)LogicFactory.GetLogic(LogicType.Radio);
                List<ArticleRadioDownload> lstDownloads = RadioLogic.SelectArticleRadioDownloadByCustomer(CustomerGUID);

                return View(lstDownloads);
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
                List<ArticleRadioDownload> lstDownloads = new List<ArticleRadioDownload>();
                return View("Radio", lstDownloads);
            }
        }

        public void RadioCheckDownloadStatus(List<ArticleRadioDownload> ClipCollection)
        {
            try
            {
                RadioLogic RadioLogic = (RadioLogic)LogicFactory.GetLogic(LogicType.Radio);
                TVLogic TVLogic = (TVLogic)LogicFactory.GetLogic(LogicType.TV);

                if (ClipCollection != null)
                {
                    foreach (ArticleRadioDownload objClip in ClipCollection)
                    {
                        if (!string.IsNullOrEmpty(objClip.ClipFormat))
                        {
                            ArticleRadioDownload objClipDownload_Temp = RadioLogic.SelectArticleRadioDownloadByID(objClip.ID, new Guid(CustomerGUID));
                            string clipGUID = objClipDownload_Temp.ClipGUID;

                            string fileName = clipGUID + "." + objClip.ClipFormat;

                            // Case 1: Check into IQCore_ClipMeta table

                            string _IQCore_ClipMeta_FileLocation = RadioLogic.SelectClipLocationFromIQCore_Meta(objClip.ID, new Guid(CustomerGUID));

                            string fileLocation = string.Empty;

                            bool IsClipLocationFound = false;

                            if (!string.IsNullOrWhiteSpace(_IQCore_ClipMeta_FileLocation))
                            {
                                _IQCore_ClipMeta_FileLocation = !_IQCore_ClipMeta_FileLocation.EndsWith(@"\") ? _IQCore_ClipMeta_FileLocation + @"\" : _IQCore_ClipMeta_FileLocation;
                                fileLocation = _IQCore_ClipMeta_FileLocation;
                            }

                            // Case 2: Check into web.config file

                            if (string.IsNullOrEmpty(fileLocation) && !string.IsNullOrEmpty(clipGUID))
                            {
                                fileLocation = Convert.ToString(ConfigurationManager.AppSettings["ClipDownloadLocation"]);
                            }

                            if (!string.IsNullOrEmpty(clipGUID) && System.IO.File.Exists(fileLocation + fileName))
                            {
                                FileInfo file = new FileInfo(fileLocation + fileName);
                                if (file.Length > 0)
                                {
                                    UpdateRadioClipDownload(objClip.ID, fileLocation, objClip.ClipFormat, 3);
                                    IsClipLocationFound = true;
                                }
                            }

                            // Case 3: Check into IQService_Export && IQRemoteService_Export table

                            if (!IsClipLocationFound)
                            {
                                bool IsClipLocationAvailable = TVLogic.CheckIntoIQServiceAndIQRemoetService_Export(clipGUID, objClip.ClipFormat);
                                if (IsClipLocationAvailable)
                                {
                                    UpdateRadioClipDownload(objClip.ID, null, objClip.ClipFormat, 2);
                                    IsClipLocationFound = true;
                                }
                            }

                            // Case 4: Made HttpWebRequest and find response

                            if (!IsClipLocationFound)
                            {
                                string clipURL = Convert.ToString(ConfigurationManager.AppSettings["ExportClip"]);
                                string clipResponse = ConfigSettings.Settings.ExportClipMsg;
                                string response = string.Empty;

                                clipURL += "?fid=" + clipGUID + "&fmt=" + objClip.ClipFormat;

                                HttpWebRequest objRequest = null;
                                HttpWebResponse objResponse = null;

                                objRequest = (HttpWebRequest)WebRequest.Create(clipURL);
                                objRequest.Method = "GET";
                                objRequest.Timeout = 300000;
                                objRequest.ReadWriteTimeout = 300000;

                                objResponse = (HttpWebResponse)objRequest.GetResponse();
                                if (objResponse.StatusCode == HttpStatusCode.OK)
                                {
                                    StreamReader _Response = new StreamReader(objResponse.GetResponseStream(), System.Text.Encoding.Default);
                                    response = _Response.ReadToEnd();
                                }

                                if (string.Compare(clipResponse, response, true) == 0)
                                {
                                    UpdateRadioClipDownload(objClip.ID, null, objClip.ClipFormat, 2);
                                    IsClipLocationFound = true;
                                }
                            }

                            if (!IsClipLocationFound)
                            {
                                ViewBag.ErrorMessage = ConfigSettings.Settings.ClipUnableToEnqueForDownload;
                            }
                            else
                            {
                                ViewBag.ErrorMessage = string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                IQMedia.Shared.Utility.Log4NetLogger.Error(ex.ToString());
            }
        }

        [HttpGet]
        public ActionResult RadioClipDelete(long ClipDownloadKey)
        {
            try
            {
                RadioLogic RadioLogic = (RadioLogic)LogicFactory.GetLogic(LogicType.Radio);
                RadioLogic.Delete_ArticleRadioDownload(CustomerGUID, ClipDownloadKey);

                return RedirectToAction("Radio", new { id = string.Empty });
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
                return RedirectToAction("Radio", new { id = string.Empty });
            }
        }

        [HttpGet]
        public ActionResult DownloadRadioClip(long ClipDownloadKey)
        {
            try
            {
                RadioLogic RadioLogic = (RadioLogic)LogicFactory.GetLogic(LogicType.Radio);
                ArticleRadioDownload objDownload = RadioLogic.SelectArticleRadioDownloadByID(ClipDownloadKey, new Guid(CustomerGUID));
                if (objDownload != null)
                {
                    if (!string.IsNullOrWhiteSpace(objDownload.ClipFileLocation) && !string.IsNullOrWhiteSpace(objDownload.ClipFormat))
                    {
                        string fileName = objDownload.ClipFileLocation + objDownload.ClipGUID + "." + objDownload.ClipFormat;

                        if (System.IO.File.Exists(fileName))
                        {
                            UpdateRadioClipDownload(ClipDownloadKey, null, null, 4);

                            string fileContentType = IQMedia.Shared.Utility.CommonFunctions.GetFileContentTypeByExtension(objDownload.ClipFormat);

                            return File(fileName, fileContentType, Path.GetFileName(objDownload.ClipGUID + "." + objDownload.ClipFormat));
                        }
                        else
                        {
                            return Content(ConfigSettings.Settings.ClipNotAvailableForDownload);// "Clip is not available to dowload");
                        }
                    }
                    else
                    {
                        return Content(ConfigSettings.Settings.ClipNotAvailableForDownload);//"Clip is not available to dowload");
                    }
                }
                else
                {
                    return Content(ConfigSettings.Settings.ErrorOnDownloadClip);//"Some error occurred while downloading clip");
                }
            }
            catch (Exception ex)
            {
                Shared.Utility.Log4NetLogger.Error(ex);
                return Content(ConfigSettings.Settings.ErrorOnDownloadClip);
            }
            finally
            {
                TempData.Keep();
            }
        }

        #endregion

        #region TVEyes

        [HttpGet]
        public ActionResult TVEyes(string id)
        {
            TVEyesLogic TVEyesLogic = (TVEyesLogic)LogicFactory.GetLogic(LogicType.TVEyes);

            List<ArticleTVEyesDownload> lstArticleTVEyesDownload = TVEyesLogic.SelectArticleTVEyesDownloadByCustomer(new Guid(CustomerGUID));

            return View(lstArticleTVEyesDownload);
        }

        [HttpPost]
        public ActionResult TVEyes(FormCollection frmCollection)
        {
            TVEyesLogic TVEyesLogic = (TVEyesLogic)LogicFactory.GetLogic(LogicType.TVEyes);

            List<ArticleTVEyesDownload> lstArticleTVEyesDownload = TVEyesLogic.SelectArticleTVEyesDownloadByCustomer(new Guid(CustomerGUID));

            return View(lstArticleTVEyesDownload);
        }

        [HttpGet]
        public ActionResult DownloadTVEyesFile(long TVEyesDownloadID, string id)
        {
            TVEyesLogic TVEyesLogic = (TVEyesLogic)LogicFactory.GetLogic(LogicType.TVEyes);
            ArticleTVEyesDownload objArticleTVEyesDownload = TVEyesLogic.SelectArticleTVEyesByID(TVEyesDownloadID, new Guid(CustomerGUID));
            if (objArticleTVEyesDownload != null)
            {
                if (!string.IsNullOrEmpty(objArticleTVEyesDownload.DownloadLocation) && System.IO.File.Exists(objArticleTVEyesDownload.DownloadLocation))
                {
                    TVEyesLogic.UpdateDownloadStatusByID(TVEyesDownloadID, new Guid(CustomerGUID));

                    string fileContentType = IQMedia.Shared.Utility.CommonFunctions.GetFileContentTypeByExtension(Path.GetExtension(objArticleTVEyesDownload.DownloadLocation));

                    return File(objArticleTVEyesDownload.DownloadLocation, fileContentType, Path.GetFileName(objArticleTVEyesDownload.DownloadLocation));
                }
                else
                {
                    return Content(ConfigSettings.Settings.FileNotAvailableForDownload);
                }
            }
            else
            {
                return Content(ConfigSettings.Settings.ErrorOnDownloadFile);
            }
        }

        #endregion

        private void UpdateClipDownload(long ClipDownloadKey, string FileLocation, string FileExtension, int DownloadStatus)
        {
            TVLogic TVLogic = (TVLogic)LogicFactory.GetLogic(LogicType.TV);
            TVLogic.Update_ClipDownload(ClipDownloadKey, FileLocation, FileExtension, DownloadStatus, new Guid(CustomerGUID));
        }

        private void UpdateRadioClipDownload(long ClipDownloadKey, string FileLocation, string FileExtension, int DownloadStatus)
        {
            RadioLogic RadioLogic = (RadioLogic)LogicFactory.GetLogic(LogicType.Radio);
            RadioLogic.Update_ArticleRadioDownload(ClipDownloadKey, FileLocation, FileExtension, DownloadStatus, new Guid(CustomerGUID));
        }

        private string[] GetClipFormats()
        {
            return new string[] { "MP4", "MP3", "WMV", "WMA" };
        }

        private string GetTermsConditionFileContent()
        {
            string strFileContent = System.IO.File.ReadAllText(Server.MapPath("~/Content/policy.txt"));
            return strFileContent.Replace("\r\n", "<br />");
        }

        [HttpGet]
        public ActionResult DownloadClipCC(Int64 ClipDownloadKey, Guid ID)
        {
            try
            {
                Dictionary<Guid, string> clipCCLocation = (Dictionary<Guid, string>)TempData["DownloadTV"];
                ClipLogic clipLgc = (ClipLogic)LogicFactory.GetLogic(LogicType.Clip);

                if (clipCCLocation == null || clipCCLocation.Where(ccl => ccl.Key == ID).Count() == 0)
                {
                    ActiveUser activeUser = ActiveUserMgr.GetActiveUser();

                    clipCCLocation = clipLgc.GetClipCCLocation(activeUser.ClientGUID, new List<Guid>() { ID }.AsEnumerable());
                }

                if (clipCCLocation.Where(ccl => ccl.Key == ID).Count() > 0)
                {
                    string fileLocation = clipCCLocation[ID];

                    if(clipLgc.UpdateClipDownloadCCStatus(ClipDownloadKey, ID) > 0)
                    {
                        return File(fileLocation, IQMedia.Shared.Utility.CommonFunctions.GetFileContentTypeByExtension(Path.GetExtension(fileLocation)), Path.GetFileName(fileLocation));
                    }
                    else
                    {
                        Shared.Utility.Log4NetLogger.Error("An error occurred in updating DownloadCCStatus.");
                        return Content("An error occurred, please try again.");
                    }
                }
                else
                {
                    Shared.Utility.Log4NetLogger.Error("No download available.");
                    return Content("No download available.");
                }
            }
            catch (Exception ex)
            {
                Shared.Utility.Log4NetLogger.Error(ex);
                return Content("An error occurred, please try again.");
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpGet]
        public ActionResult DownloadRadioClipCC(Int64 ClipDownloadKey, Guid ID)
        {
            try
            {
                Dictionary<Guid, string> clipCCLocation = (Dictionary<Guid, string>)TempData["DownloadRadio"];
                ClipLogic clipLgc = (ClipLogic)LogicFactory.GetLogic(LogicType.Clip);

                if (clipCCLocation == null || clipCCLocation.Where(ccl => ccl.Key == ID).Count() == 0)
                {
                    ActiveUser activeUser = ActiveUserMgr.GetActiveUser();

                    clipCCLocation = clipLgc.GetClipCCLocation(activeUser.ClientGUID, new List<Guid>() { ID }.AsEnumerable());
                }

                if (clipCCLocation.Where(ccl => ccl.Key == ID).Count() > 0)
                {
                    string fileLocation = clipCCLocation[ID];

                    if (clipLgc.UpdateRadioClipDownloadCCStatus(ClipDownloadKey, ID) > 0)
                    {
                        return File(fileLocation, IQMedia.Shared.Utility.CommonFunctions.GetFileContentTypeByExtension(Path.GetExtension(fileLocation)), Path.GetFileName(fileLocation));
                    }
                    else
                    {
                        Shared.Utility.Log4NetLogger.Error("An error occurred in updating DownloadCCStatus for radio.");
                        return Content("An error occurred, please try again.");
                    }
                }
                else
                {
                    Shared.Utility.Log4NetLogger.Error("No download available for radio CC.");
                    return Content("No download available.");
                }
            }
            catch (Exception ex)
            {
                Shared.Utility.Log4NetLogger.Error(ex);
                return Content("An error occurred, please try again.");
            }
            finally
            {
                TempData.Keep();
            }
        }
    }
}
