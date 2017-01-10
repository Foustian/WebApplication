using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Model;
using IQMedia.WebApplication.Utility;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using System.Dynamic;
using System.IO;
using System.Xml;
using IQMedia.WebApplication.Models.TempData;
using System.Configuration;
using System.Xml.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using PMGSearch;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication]
    public class SetupController : Controller
    {
        //const string ALL = "All";
        const string ZERO = "0";
        const string IQ_DMA_SET_REGION_ATTR = "Region";
        private string _InvalidImageMessage = "image is not in correct format";

        SetupTempData setupTempData = null;

        //
        // GET: /Setup/

        public string ClientGUID
        {
            get
            {
                ActiveUser sessionInformation = ActiveUserMgr.GetActiveUser();
                return sessionInformation.ClientGUID.ToString();
            }
        }

        public string CustomerGUID
        {
            get
            {
                ActiveUser sessionInformation = ActiveUserMgr.GetActiveUser();
                return sessionInformation.CustomerGUID.ToString();
            }
        }

        string PATH_SetupCustomCategoryList = "~/Views/Setup/_CustomCategoryList.cshtml";
        string PATH_SetupCustomCategoryAddEdit = "~/Views/Setup/_CustomCategoryAddEdit.cshtml";
        string PATH_SetupIQAgentSetupDisplay = "~/Views/Setup/_IQAgent.cshtml";
        string PATH_SetupIQAgentNotificationSettingsDisplay = "~/Views/Setup/_IQNotificationSettings.cshtml";
        string PATH_SetupClientCustomSettingsDisplay = "~/Views/Setup/_ClientCustomSettings.cshtml";
        string PATH_SetupAdminFliq_ClientApplicationListPartialView = "~/Views/Setup/_Fliq_ClientApplicationList.cshtml";
        string PATH_SetupAdminFliq_CustomerApplicationListPartialView = "~/Views/Setup/_Fliq_CustomerApplicationList.cshtml";
        string PATH_SetupAdminFliq_UGCUploadTrackingListPartialView = "~/Views/Setup/_Fliq_UGCUploadTrackingList.cshtml";
        string PATH_SetupJobStatusListPartialView = "~/Views/Setup/_JobStatusResult.cshtml";
        string PATH_SetupJobStatusPartialView = "~/Views/Setup/_JobStatus.cshtml";
        string PATH_SetupGoogleSignInPartialView = "~/Views/Setup/_GoogleSignIn.cshtml";
        string PATH_SetupCampaignSetupDisplay = "~/Views/Setup/_Campaign.cshtml";
        string PATH_SetupInstagramSetupPartialView = "~/Views/Setup/_InstagramSetup.cshtml";

        public ActionResult Index()
        {
            // Clear out temp data used by other pages
            if (TempData.ContainsKey("AnalyticsTempData")) { TempData["AnalyticsTempData"] = null; }
            if (TempData.ContainsKey("FeedsTempData")) { TempData["FeedsTempData"] = null; }
            if (TempData.ContainsKey("DiscoveryTempData")) { TempData["DiscoveryTempData"] = null; }
            if (TempData.ContainsKey("TimeShiftTempData")) { TempData["TimeShiftTempData"] = null; }
            if (TempData.ContainsKey("TAdsTempData")) { TempData["TAdsTempData"] = null; }
            if (TempData.ContainsKey("GlobalAdminTempData")) { TempData["GlobalAdminTempData"] = null; } 

            setupTempData = new SetupTempData();

            setupTempData.lstIQAgent_SearchRequestModel = new List<IQAgent_SearchRequestModel>();
            setupTempData.lstFBPageModel = new List<FBPageModel>();
            SetTempData(setupTempData);
            return View();
        }

        [ChildActionOnly]
        public ActionResult _FillDropDownIQAgentSetup()
        {
            IQAgentSearchRequest_DropDown allDropDown = new IQAgentSearchRequest_DropDown();
            try
            {
                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                allDropDown = iqAgentLogic.SelectAllDropdown(ClientGUID);

                /*allDropDown.TV_DMAList.Insert(0, new IQAgentDropDown_TV_DMA { DMAName = ALL, DMANumber = ZERO });
                allDropDown.TV_ClassList.Insert(0, new IQAgentDropDown_TV_Class { ClassName = ALL, ClassID = ZERO });
                allDropDown.TV_StationList.Insert(0, new IQAgentDropDown_TV_Station { IQ_Station_ID = ALL });

                allDropDown.NM_GenereList.Insert(0, new IQAgentDropDown_NM_Genere { ID = 0, Label = ALL });
                allDropDown.NM_RegionList.Insert(0, new IQAgentDropDown_NM_Region { ID = 0, Label = ALL });
                allDropDown.NM_CategoryList.Insert(0, new IQAgentDropDown_NM_Category { ID = 0, Label = ALL });
                allDropDown.NM_PublicationCategoryList.Insert(0, new IQAgentDropDown_NM_PublicationCategory { ID = 0, Label = ALL });

                allDropDown.SM_SourceCategoryList.Insert(0, new IQAgentDropDown_SM_SourceCategory { ID = 0, Label = ALL, Value = ALL });
                allDropDown.SM_SourceTypeList.Insert(0, new IQAgentDropDown_SM_SourceType { ID = 0, Label = ALL });*/

            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
            }
            finally { TempData.Keep(); }
            return PartialView("_IQAgentSetupAddEdit", allDropDown);
        }

        public JsonResult _Setup()
        {
            ActiveUser sessionInformation = ActiveUserMgr.GetActiveUser();
            object setupHTML = PartialView("_Setup", sessionInformation);
            return new JsonResult();
        }

        public JsonResult GetSetup()
        {
            try
            {
                return Json(new
                {
                    HTML = GenerateSetupHTML(),
                    isSuccess = true
                });
            }
            catch (Exception)
            {

                return Json(new
                        {
                            isSuccess = false
                        });
            }

        }

        public string GenerateSetupHTML()
        {
            try
            {
                ActiveUser sessionInformation = ActiveUserMgr.GetActiveUser();
                string setupHTML = "<div>" + sessionInformation.FirstName + " " + sessionInformation.LastName + "</div>";
                return setupHTML;

            }
            catch (Exception)
            {

                throw;
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult SelectCustomCategories()
        {
            try
            {
                CustomCategoryLogic customcategoryLogic = (CustomCategoryLogic)LogicFactory.GetLogic(LogicType.Category);
                List<CustomCategoryModel> lstCustomCategories = customcategoryLogic.SelectCustomCategoryForSetup(new Guid(ClientGUID));
                string strHTML = RenderPartialToString(PATH_SetupCustomCategoryList, lstCustomCategories);
                return Json(new
                {
                    isSuccess = true,
                    HTML = strHTML
                });
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult GetCustomCategoriesForRanking()
        {
            try
            {
                CustomCategoryLogic customcategoryLogic = (CustomCategoryLogic)LogicFactory.GetLogic(LogicType.Category);
                List<CustomCategoryModel> lstCustomCategories = customcategoryLogic.SelectCustomCategoryForSetup(new Guid(ClientGUID)).OrderBy(o => o.CategoryRanking).ToList();

                List<IQReport_FolderModel> lstTreeObjects = lstCustomCategories.Select(s => new IQReport_FolderModel()
                {
                    id = s.CategoryGUID.ToString(),
                    parent = "#",
                    text = s.CategoryName
                }).ToList();

                return Json(new
                {
                    isSuccess = true,
                    jsonCategories = Shared.Utility.CommonFunctions.SearializeJson(lstTreeObjects)
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }


            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult UpdateCustomCategoryRankings(List<string> categoryGUIDs)
        {
            try
            {
                CustomCategoryLogic customCategoryLogic = (CustomCategoryLogic)LogicFactory.GetLogic(LogicType.Category);
                int rowsUpdated = customCategoryLogic.UpdateCustomCategoryRankings(categoryGUIDs);

                if (rowsUpdated == categoryGUIDs.Count)
                {
                    return Json(new
                    {
                        isSuccess = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        errorMsg = Config.ConfigSettings.Settings.ErrorOccurred
                    });
                }
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    errorMsg = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
        }

        [HttpPost]
        public JsonResult GetAddEditCustomCategory(long p_CustomCategoryKey)
        {
            try
            {
                CustomCategoryLogic customcategoryLogic = (CustomCategoryLogic)LogicFactory.GetLogic(LogicType.Category);
                CustomCategoryModel objCustomCategory = new CustomCategoryModel();
                if (p_CustomCategoryKey > 0)
                {
                    objCustomCategory = customcategoryLogic.SelectCustomCategoryByCategoryKeyForSetup(new Guid(ClientGUID), p_CustomCategoryKey);
                }

                string strHTML = RenderPartialToString(PATH_SetupCustomCategoryAddEdit, objCustomCategory);
                return Json(new
                {
                    isSuccess = true,
                    HTML = strHTML
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult SaveCustomCategory(long p_CustomCategoryKey, string p_CategoryName, string p_CategoryDescription)
        {
            try
            {
                CustomCategoryLogic customcategoryLogic = (CustomCategoryLogic)LogicFactory.GetLogic(LogicType.Category);
                string result = string.Empty;

                CustomCategoryModel objCustomCategory = new CustomCategoryModel();

                if (p_CustomCategoryKey > 0)
                {
                    objCustomCategory.CategoryKey = p_CustomCategoryKey;
                    objCustomCategory.CategoryName = p_CategoryName.Trim();
                    objCustomCategory.CategoryDescription = !string.IsNullOrEmpty(p_CategoryDescription) ? p_CategoryDescription.Trim() : string.Empty;
                    objCustomCategory.ClientGUID = new Guid(ClientGUID);

                    result = customcategoryLogic.UpdateCustomCategoryForSetup(objCustomCategory);
                }
                else
                {
                    objCustomCategory.CategoryName = p_CategoryName.Trim();
                    objCustomCategory.CategoryDescription = !string.IsNullOrEmpty(p_CategoryDescription) ? p_CategoryDescription.Trim() : string.Empty;
                    objCustomCategory.ClientGUID = new Guid(ClientGUID);

                    result = customcategoryLogic.InsertCustomCategoryForSetup(objCustomCategory);
                }

                return Json(new
                {
                    isSuccess = true,
                    isDuplicateCategory = (!string.IsNullOrEmpty(result) && Convert.ToInt64(result) < 0) ? true : false
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult DeleteCustomCategory(long p_CustomCategoryKey)
        {
            try
            {
                CustomCategoryLogic customcategoryLogic = (CustomCategoryLogic)LogicFactory.GetLogic(LogicType.Category);
                string result = customcategoryLogic.DeleteCustomCategoryForSetup(new Guid(ClientGUID), p_CustomCategoryKey);
                return Json(new
                {
                    isSuccess = true,
                    isChildRecordExists = (!string.IsNullOrEmpty(result) && Convert.ToInt64(result) < 0) ? true : false
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult DisplayIQAgentSetupContent()
        {
            try
            {
                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                List<IQAgent_SearchRequestModel> lstIQAgentSearchRequest = iqAgentLogic.SelectIQAgentSearchRequestByClientGuid(ClientGUID);

                FacebookLogic facebookLogic = (FacebookLogic)LogicFactory.GetLogic(LogicType.Facebook);
                List<FBPageModel> lstFBPageModel = facebookLogic.GetFBPages().Distinct(new FBPageModelComparer()).ToList();

                setupTempData = GetTempData();
                if (setupTempData == null)
                    setupTempData = new SetupTempData();

                setupTempData.lstIQAgent_SearchRequestModel = lstIQAgentSearchRequest;
                setupTempData.lstFBPageModel = lstFBPageModel;
                SetTempData(setupTempData);

                string strHTML = RenderPartialToString(PATH_SetupIQAgentSetupDisplay, lstIQAgentSearchRequest);
                return Json(new
                {
                    isSuccess = true,
                    HTML = strHTML
                });
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult DisplayIQAgentSearchXMLByID(long p_ID)
        {
            try
            {
                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                IQAgent_SearchRequestModel objIQAgentSearchRequest = iqAgentLogic.SelectIQAgentSearchRequestByID(ClientGUID, p_ID);

                string strHTML = string.Empty;
                if (!string.IsNullOrWhiteSpace(objIQAgentSearchRequest.SearchTerm))
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(objIQAgentSearchRequest.SearchTerm);
                    XmlNode node = xdoc.SelectSingleNode("//SearchRequest");

                    strHTML = FormatXml(node).Trim();
                }
                return Json(new
                {
                    isSuccess = true,
                    HTML = strHTML
                });
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        protected string FormatXml(System.Xml.XmlNode xmlNode)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            // We will use stringWriter to push the formated xml into our StringBuilder bob.
            using (StringWriter stringWriter = new StringWriter(builder))
            {
                // We will use the Formatting of our xmlTextWriter to provide our indentation.
                using (System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(stringWriter))
                {
                    xmlTextWriter.Formatting = System.Xml.Formatting.Indented;
                    xmlNode.WriteTo(xmlTextWriter);
                }
            }

            return builder.ToString();
        }

        [HttpPost]
        public JsonResult DeleteIQAgent(long p_ID)
        {
            try
            {
                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                Int64 result = iqAgentLogic.RequestDeleteIQAgentSearchRequest(p_ID, new Guid(ClientGUID), new Guid(CustomerGUID));

                return Json(new
                {
                    isSuccess = true,
                    msg = Config.ConfigSettings.Settings.IQAgentDeleteMsg
                });
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult SuspendAgent(long p_ID)
        {
            try
            {
                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                Int16 result = iqAgentLogic.SuspendAgentSearchRequest(p_ID, new Guid(ClientGUID), new Guid(CustomerGUID));

                if (result > 0)
                {
                    setupTempData = GetTempData();

                    List<IQAgent_SearchRequestModel> searchRequestList = setupTempData.lstIQAgent_SearchRequestModel;

                    searchRequestList.Where(sr => sr.ID == p_ID).First().IsActive = 2;

                    setupTempData.lstIQAgent_SearchRequestModel = searchRequestList;
                    SetTempData(setupTempData);

                    return Json(new
                    {
                        isSuccess = true,
                        msg = Config.ConfigSettings.Settings.AgentSuspendMsg
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false
                    });
                }
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult ResumeSuspendedAgent(long p_ID)
        {
            try
            {
                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                Int16 result = iqAgentLogic.ResumeSuspendedAgent(p_ID, new Guid(ClientGUID), new Guid(CustomerGUID));

                if (result > 0)
                {
                    setupTempData = GetTempData();

                    List<IQAgent_SearchRequestModel> searchRequestList = setupTempData.lstIQAgent_SearchRequestModel;

                    searchRequestList.Where(sr => sr.ID == p_ID).First().IsActive = 1;

                    setupTempData.lstIQAgent_SearchRequestModel = searchRequestList;
                    SetTempData(setupTempData);

                    return Json(new
                    {
                        isSuccess = true,
                        msg = Config.ConfigSettings.Settings.AgentSuspendMsg
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false
                    });
                }
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult GetIQAgentSetupAddEditForm(long p_ID)
        {
            try
            {
                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                IQAgent_SearchRequestModel objIQAgentSearchRequest = iqAgentLogic.SelectIQAgentSearchRequestByID(ClientGUID, p_ID);

                string strHTML = string.Empty;
                IQMedia.Model.IQAgentXML.SearchRequest _SearchRequest = new Model.IQAgentXML.SearchRequest();
                if (!string.IsNullOrWhiteSpace(objIQAgentSearchRequest.SearchTerm))
                {
                    _SearchRequest = IQMedia.Shared.Utility.CommonFunctions.DeserialiazeXml(objIQAgentSearchRequest.SearchTerm, _SearchRequest) as IQMedia.Model.IQAgentXML.SearchRequest;
                    _SearchRequest.TVSpecified = (_SearchRequest.TV != null);
                    _SearchRequest.NewsSpecified = (_SearchRequest.News != null);
                    _SearchRequest.SocialMediaSpecified = (_SearchRequest.SocialMedia != null);
                    _SearchRequest.FacebookSpecified = (_SearchRequest.Facebook != null);
                    _SearchRequest.TwitterSpecified = (_SearchRequest.Twitter != null);
                    _SearchRequest.TMSpecified = (_SearchRequest.TM != null);
                    _SearchRequest.PMSpecified = (_SearchRequest.PM != null);
                    _SearchRequest.PQSpecified = (_SearchRequest.PQ != null);
                    _SearchRequest.LRSpecified = (_SearchRequest.LR != null);
                    _SearchRequest.LexisNexisSpecified = (_SearchRequest.LexisNexis != null);
                    _SearchRequest.BlogSpecified = (_SearchRequest.Blog != null);
                    _SearchRequest.ForumSpecified = (_SearchRequest.Forum != null);
                    _SearchRequest.IQRadioSpecified = (_SearchRequest.IQRadio != null);
                }

                return Json(new
                {
                    isSuccess = true,
                    iqAgentKey = objIQAgentSearchRequest.ID,
                    queryName = objIQAgentSearchRequest.QueryName,
                    searchRequestObject = _SearchRequest
                });
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SubmitIQAgentSetupAddEdit(IQAgentSearchRequestPost objIQAgentSearchRequestPost)
        {
            try
            {
                string Message = string.Empty, IQAgentSearchRequestKey = string.Empty;

                IQMedia.Model.IQAgentXML.SearchRequest _SearchRequest = FillSearchXML(objIQAgentSearchRequestPost);
                string strIQAgentXML = IQMedia.Shared.Utility.CommonFunctions.SerializeToXml(_SearchRequest);

                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                string result = string.Empty;

                if (objIQAgentSearchRequestPost.hdnIQAgentSetupAddEditKey == 0)
                {
                    result = iqAgentLogic.InsertIQAgentSearchRequest(ClientGUID, objIQAgentSearchRequestPost.txtIQAgentSetupTitle.Trim(), strIQAgentXML);

                    if (!string.IsNullOrWhiteSpace(result) && Convert.ToInt64(result) < 0)
                    {
                        if (Convert.ToInt64(result) == -2)
                        {
                            Message = Config.ConfigSettings.Settings.IQAgentMaxLimitExceeds;
                        }
                        else
                        {
                            Message = Config.ConfigSettings.Settings.IAgentQueryNameExist;
                        }

                    }
                    else
                    {
                        Message = Config.ConfigSettings.Settings.IAgentQueryNameInserted;
                    }

                    IQAgentSearchRequestKey = result.Trim();
                }
                else
                {
                    result = iqAgentLogic.UpdateIQAgentSearchRequest(ClientGUID, objIQAgentSearchRequestPost.hdnIQAgentSetupAddEditKey, objIQAgentSearchRequestPost.txtIQAgentSetupTitle.Trim(), strIQAgentXML);
                    if (!string.IsNullOrWhiteSpace(result) && Convert.ToInt64(result) < 0)
                        Message = Config.ConfigSettings.Settings.IAgentQueryNameExist;
                    else
                        Message = Config.ConfigSettings.Settings.IAgentQueryNameUpdted;

                    IQAgentSearchRequestKey = result;// Convert.ToString(objIQAgentSearchRequestPost.hdnIQAgentSetupAddEditKey).Trim();
                }

                if (_SearchRequest.Facebook != null)
                {
                    FacebookLogic facebookLogic = (FacebookLogic)LogicFactory.GetLogic(LogicType.Facebook);
                    facebookLogic.InsertFBPages(Shared.Utility.CommonFunctions.SerializeToXml(_SearchRequest.Facebook));
                }

                if (_SearchRequest.Instagram != null && !String.IsNullOrEmpty(_SearchRequest.Instagram.UserTagString))
                {
                    string tagPattern = @"#([\w._]+)(?:\)|$|\s)"; // Limit tags to alphanumeric characters, periods, and underscores.
                    string userPattern = @"@([\w]+(?(?=\.)\.[\w]+)|[\w]?)(?:\)|$|\s)"; // Usernames can only include letters, numbers, periods, and underscores. Periods cannot appear at the start or end of the name.

                    List<string> tags = new List<string>();
                    List<string> users = new List<string>();

                    foreach (Match match in Regex.Matches(_SearchRequest.Instagram.UserTagString, tagPattern))
                    {
                        // Get the second group, as the first is always the full match
                        if (!String.IsNullOrWhiteSpace(match.Groups[1].Value))
                        {
                            tags.Add(match.Groups[1].Value);
                        }
                    }
                    foreach (Match match in Regex.Matches(_SearchRequest.Instagram.UserTagString, userPattern))
                    {
                        // Get the second group, as the first is always the full match
                        if (!String.IsNullOrWhiteSpace(match.Groups[1].Value))
                        {
                            users.Add(match.Groups[1].Value);
                        }
                    }

                    InstagramLogic instagramLogic = (InstagramLogic)LogicFactory.GetLogic(LogicType.Instagram);
                    instagramLogic.InsertSources(new Guid(ClientGUID), tags, users);
                }

                return Json(new
                {
                    isSuccess = true,
                    msg = Message,
                    iqAgentKey = IQAgentSearchRequestKey
                });
            }
            catch (Exception _Exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_Exception);
                return Json(new
                {
                    isSuccess = false,
                    msg = "Some error occurred while saving query."
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult DisplayIQAgentNotificationSettings(long p_SearchRequestID)
        {
            try
            {
                IQNotificationSettingsLogic iQNotificationSettingsLogic = (IQNotificationSettingsLogic)LogicFactory.GetLogic(LogicType.IQNotification);
                List<IQNotifationSettingsModel> lstIQNotification = iQNotificationSettingsLogic.SelectIQNotifcationsBySearchRequestID(ClientGUID, p_SearchRequestID);
                string strHTML = RenderPartialToString(PATH_SetupIQAgentNotificationSettingsDisplay, lstIQNotification);
                return Json(new
                {
                    isSuccess = true,
                    HTML = strHTML
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult LoadReportFolder()
        {
            try
            {
                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                List<IQReport_FolderModel> lstIQAgentSearchRequest = iQReport_FolderLogic.GetFolderData(new Guid(ClientGUID));

                string jsonStr = Shared.Utility.CommonFunctions.SearializeJson(lstIQAgentSearchRequest);
                return Json(new
                {
                    isSuccess = true,
                    jsonFolders = jsonStr
                });
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult RenameReportFolder(string p_ID, string p_Name)
        {
            try
            {
                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                string _result = iQReport_FolderLogic.RenameFolder(p_ID, p_Name, new Guid(ClientGUID));

                if (!string.IsNullOrEmpty(_result) && Convert.ToInt32(_result) > 0)
                {
                    return Json(new
                    {
                        isSuccess = true,
                    });
                }
                else
                {

                    return Json(new
                    {
                        isSuccess = false,
                        isDuplicate = _result == "-1" ? true : false
                    });
                }
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult CreateReportFolder(string p_Name, string p_ParentID)
        {
            try
            {
                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                string _result = iQReport_FolderLogic.CreateFolder(p_Name, p_ParentID, new Guid(ClientGUID));

                if (!string.IsNullOrEmpty(_result) && Convert.ToInt32(_result) > 0)
                {
                    return Json(new
                    {
                        isSuccess = true,
                        id = _result
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        isDuplicate = _result == "-2" ? true : false
                    });
                }
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult MoveReportFolder(string p_ID, string p_ParentID)
        {
            try
            {
                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                string _result = iQReport_FolderLogic.MoveFolder(p_ParentID, p_ID, new Guid(ClientGUID));

                if (!string.IsNullOrEmpty(_result) && Convert.ToInt32(_result) > 0)
                {
                    return Json(new
                    {
                        isSuccess = true,
                    });
                }
                else
                {
                    if (_result == "-1")
                    {
                        return Json(new
                        {
                            isSuccess = false,
                            isDuplicate = true
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            isSuccess = false
                        });
                    }
                }
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult DeleteReportFolder(string p_ID)
        {
            try
            {
                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                string _result = iQReport_FolderLogic.DeleteFolder(p_ID, new Guid(ClientGUID));

                if (!string.IsNullOrEmpty(_result) && Convert.ToInt32(_result) > 0)
                {
                    return Json(new
                    {
                        isSuccess = true,
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        isHasChildReport = _result == "-1" ? true : false
                    });
                }
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult PasteReportFolder(string p_CopyID, string p_PasteID)
        {
            try
            {
                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                string _result = iQReport_FolderLogic.PasteFolder(p_CopyID, p_PasteID, new Guid(ClientGUID));

                if (!string.IsNullOrEmpty(_result) && Convert.ToInt32(_result) > 0)
                {
                    return Json(new
                    {
                        isSuccess = true,
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                    });
                }
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        private Model.IQAgentXML.SearchRequest FillSearchXML(IQAgentSearchRequestPost objIQAgentSearchRequestPost)
        {
            try
            {
                if (objIQAgentSearchRequestPost != null)
                {
                    ActiveUser sessionInformation = ActiveUserMgr.GetActiveUser();
                    setupTempData = GetTempData();

                    IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                    IQMedia.Model.IQAgentXML.SearchRequest _SearchRequest = new Model.IQAgentXML.SearchRequest();
                    _SearchRequest.TV = new Model.IQAgentXML.TV();
                    _SearchRequest.News = new IQMedia.Model.IQAgentXML.News();
                    _SearchRequest.SocialMedia = new IQMedia.Model.IQAgentXML.SocialMedia();
                    _SearchRequest.Blog = new Model.IQAgentXML.Blog();
                    _SearchRequest.Forum = new Model.IQAgentXML.Forum();
                    _SearchRequest.Facebook = new Model.IQAgentXML.Facebook();
                    _SearchRequest.Instagram = new Model.IQAgentXML.Instagram();
                    _SearchRequest.Twitter = new IQMedia.Model.IQAgentXML.Twitter();
                    _SearchRequest.TM = new IQMedia.Model.IQAgentXML.TM();
                    _SearchRequest.PM = new Model.IQAgentXML.PM();
                    _SearchRequest.PQ = new Model.IQAgentXML.PQ();
                    _SearchRequest.LexisNexis = new Model.IQAgentXML.LexisNexis();
                    _SearchRequest.LR = new Model.IQAgentXML.LR();
                    _SearchRequest.TM = new Model.IQAgentXML.TM();
                    _SearchRequest.IQRadio = new Model.IQAgentXML.IQRadio();

                    // We need to query database for the all the dropdown values. To generate XML for few fields, we required "TextField" as well "ValueField".
                    // From Web page we will get only "ValueField" so we will fetch "TextField" from this request.

                    IQAgentSearchRequest_DropDown allDropDown = iqAgentLogic.SelectAllDropdown(ClientGUID);


                    _SearchRequest.SearchTerm = objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm.Trim();

                    #region TV

                    if (sessionInformation.Isv4TV && !string.IsNullOrEmpty(objIQAgentSearchRequestPost.chkIQAgentSetup_TV))
                    {
                        _SearchRequest.TVSpecified = true;

                        _SearchRequest.TV.ProgramTitle = objIQAgentSearchRequestPost.txtIQAgentSetupProgramTitle != null ? objIQAgentSearchRequestPost.txtIQAgentSetupProgramTitle.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupProgramTitle;
                        _SearchRequest.TV.Appearing = objIQAgentSearchRequestPost.txtIQAgentSetupAppearing != null ? objIQAgentSearchRequestPost.txtIQAgentSetupAppearing.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupAppearing;

                        _SearchRequest.TV.SearchTerm = new Model.IQAgentXML.MediumSearchTerm();

                        _SearchRequest.TV.SearchTerm.SearchTerm = objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_TV != null ? objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_TV.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_TV;
                        if (objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_TV != null && objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_TV == true)
                        {
                            _SearchRequest.TV.SearchTerm.IsUserMaster = true;
                        }
                        else
                        {
                            _SearchRequest.TV.SearchTerm.IsUserMaster = false;
                        }

                        #region IQ_Dma_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupDMA_TV != null)
                        {
                            _SearchRequest.TV.IQ_Dma_Set.SelectionMethod = IQ_DMA_SET_REGION_ATTR;

                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupDMA_TV.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupDMA_TV[0].Trim(), ZERO, true) == 0)
                            {
                                _SearchRequest.TV.IQ_Dma_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.TV.IQ_Dma_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupDMA_TV)
                                {
                                    IQ_Dma tvDMA = allDropDown.TV_DMAList.Where(dma => dma.Name == str).FirstOrDefault();
                                    if (tvDMA != null)
                                    {
                                        _SearchRequest.TV.IQ_Dma_Set.IQ_Dma.Add(new Model.IQAgentXML.IQ_Dma() { num = tvDMA.Num, name = tvDMA.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.TV.IQ_Dma_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region IQ_Station_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupStation_TV != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupStation_TV.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupStation_TV[0].Trim(), ZERO, true) == 0)
                            {
                                _SearchRequest.TV.IQ_Station_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.TV.IQ_Station_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupStation_TV)
                                {
                                    _SearchRequest.TV.IQ_Station_Set.IQ_Station_ID.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.TV.IQ_Station_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region Station_Affiliate_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupAffiliate_TV != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupAffiliate_TV.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupAffiliate_TV[0].Trim(), ZERO, true) == 0)
                            {
                                _SearchRequest.TV.Station_Affiliate_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.TV.Station_Affiliate_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupAffiliate_TV)
                                {
                                    Station_Affil tvAffil = allDropDown.TV_AffiliateList.Where(affil => affil.Name == str).FirstOrDefault();
                                    if (tvAffil != null)
                                    {
                                        _SearchRequest.TV.Station_Affiliate_Set.Station_Affil.Add(new Model.IQAgentXML.Station_Affil() { name = tvAffil.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.TV.Station_Affiliate_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region IQ_Class_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupCategory_TV != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupCategory_TV.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupCategory_TV[0].Trim(), ZERO, true) == 0)
                            {
                                _SearchRequest.TV.IQ_Class_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.TV.IQ_Class_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupCategory_TV)
                                {
                                    IQ_Class tvClass = allDropDown.TV_ClassList.Where(cls => cls.Num == str).FirstOrDefault();
                                    if (tvClass != null)
                                    {
                                        _SearchRequest.TV.IQ_Class_Set.IQ_Class.Add(new Model.IQAgentXML.IQ_Class() { num = tvClass.Num, name = tvClass.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.TV.IQ_Class_Set.IsAllowAll = true;
                        }
                        #endregion

                        #region IQ_Region_Set
                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_TV != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_TV.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_TV[0].Trim(), ZERO, true) == 0)
                            {
                                _SearchRequest.TV.IQ_Region_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.TV.IQ_Region_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_TV)
                                {
                                    IQ_Region tvRegion = allDropDown.TV_RegionList.Where(region => region.Num == Convert.ToInt32(str)).FirstOrDefault();
                                    if (tvRegion != null)
                                    {
                                        _SearchRequest.TV.IQ_Region_Set.IQ_Region.Add(new Model.IQAgentXML.IQ_Region() { num = Convert.ToString(tvRegion.Num), name = tvRegion.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.TV.IQ_Region_Set.IsAllowAll = true;
                        }
                        #endregion

                        #region IQ_Country_Set
                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_TV != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_TV.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_TV[0].Trim(), ZERO, true) == 0)
                            {
                                _SearchRequest.TV.IQ_Country_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.TV.IQ_Country_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_TV)
                                {
                                    IQ_Country tvCountry = allDropDown.TV_CountryList.Where(c => c.Num == Convert.ToInt32(str)).FirstOrDefault();
                                    if (tvCountry != null)
                                    {
                                        _SearchRequest.TV.IQ_Country_Set.IQ_Country.Add(new Model.IQAgentXML.IQ_Country() { num = Convert.ToString(tvCountry.Num), name = tvCountry.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.TV.IQ_Country_Set.IsAllowAll = true;
                        }
                        #endregion

                        #region Zip Codes
                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupZipCodes))
                        {
                            _SearchRequest.TV.ZipCodes = objIQAgentSearchRequestPost.txtIQAgentSetupZipCodes.Split(';').Select(a => a.Trim()).ToList();
                        }
                        #endregion

                        #region Exclude_IQ_Dma_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupExcludeDMA_TV != null && !(objIQAgentSearchRequestPost.ddlIQAgentSetupExcludeDMA_TV.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupExcludeDMA_TV[0].Trim(), ZERO, true) == 0))
                        {
                            _SearchRequest.TV.Exclude_IQ_Dma_Set.SelectionMethod = IQ_DMA_SET_REGION_ATTR;

                            foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupExcludeDMA_TV)
                            {
                                IQ_Dma tvDMA = allDropDown.TV_DMAList.Where(dma => dma.Name == str).FirstOrDefault();
                                if (tvDMA != null)
                                {
                                    _SearchRequest.TV.Exclude_IQ_Dma_Set.Exclude_IQ_Dma.Add(new Model.IQAgentXML.IQ_Dma() { num = tvDMA.Num, name = tvDMA.Name });
                                }
                            }
                        }

                        #endregion

                        #region Exclude Zip Codes
                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupExcludeZipCodes))
                        {
                            _SearchRequest.TV.ExcludeZipCodes = objIQAgentSearchRequestPost.txtIQAgentSetupExcludeZipCodes.Split(';').Select(a => a.Trim()).ToList();
                        }
                        #endregion
                    }
                    else
                    {
                        _SearchRequest.TVSpecified = false;
                    }
                    #endregion

                    #region Online News

                    if (sessionInformation.Isv4NM && !string.IsNullOrEmpty(objIQAgentSearchRequestPost.chkIQAgentSetup_NM))
                    {
                        _SearchRequest.NewsSpecified = true;

                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupPublication_NM))
                        {
                            _SearchRequest.News.Publications = objIQAgentSearchRequestPost.txtIQAgentSetupPublication_NM.Split(';').Select(a => a.Trim()).ToList();
                        }

                        _SearchRequest.News.SearchTerm = new Model.IQAgentXML.MediumSearchTerm();

                        _SearchRequest.News.SearchTerm.SearchTerm = objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_NM != null ? objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_NM.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_NM;
                        if (objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_NM != null && objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_NM == true)
                        {
                            _SearchRequest.News.SearchTerm.IsUserMaster = true;
                        }
                        else
                        {
                            _SearchRequest.News.SearchTerm.IsUserMaster = false;
                        }

                        #region NewsCategory_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupCategory_NM != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupCategory_NM.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupCategory_NM[0], ZERO, true) == 0)
                            {
                                _SearchRequest.News.NewsCategory_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.News.NewsCategory_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupCategory_NM)
                                {
                                    _SearchRequest.News.NewsCategory_Set.NewsCategory.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.News.NewsCategory_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region PublicationCategory_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupPublicationCategory_NM != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupPublicationCategory_NM.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupPublicationCategory_NM[0], ZERO, true) == 0)
                            {
                                _SearchRequest.News.PublicationCategory_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.News.PublicationCategory_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupPublicationCategory_NM)
                                {
                                    _SearchRequest.News.PublicationCategory_Set.PublicationCategory.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.News.PublicationCategory_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region Genre_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupGenere_NM != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupGenere_NM.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupGenere_NM[0], ZERO, true) == 0)
                            {
                                _SearchRequest.News.Genre_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.News.Genre_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupGenere_NM)
                                {
                                    _SearchRequest.News.Genre_Set.Genre.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.News.Genre_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region Region_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_NM != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_NM.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_NM[0], ZERO, true) == 0)
                            {
                                _SearchRequest.News.Region_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.News.Region_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_NM)
                                {
                                    _SearchRequest.News.Region_Set.Region.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.News.Region_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region Language_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupLanguage_NM != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupLanguage_NM.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupLanguage_NM[0], ZERO, true) == 0)
                            {
                                _SearchRequest.News.Language_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.News.Language_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupLanguage_NM)
                                {
                                    _SearchRequest.News.Language_Set.Language.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.News.Language_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region Country_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_NM != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_NM.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_NM[0], ZERO, true) == 0)
                            {
                                _SearchRequest.News.Country_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.News.Country_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_NM)
                                {
                                    _SearchRequest.News.Country_Set.Country.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.News.Country_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region Exclude Domains

                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupExcludeDomains_NM))
                        {
                            _SearchRequest.News.ExlcudeDomains = objIQAgentSearchRequestPost.txtIQAgentSetupExcludeDomains_NM.Split(';').Select(a => a.Trim()).ToList();
                        }

                        #endregion
                    }
                    else
                    {
                        _SearchRequest.NewsSpecified = false;
                    }

                    #endregion

                    #region Social Media

                    if (sessionInformation.Isv4SM && !string.IsNullOrEmpty(objIQAgentSearchRequestPost.chkIQAgentSetup_SM))
                    {
                        _SearchRequest.SocialMediaSpecified = true;

                        _SearchRequest.SocialMedia.Author = objIQAgentSearchRequestPost.txtIQAgentSetupAuthor_SM != null ? objIQAgentSearchRequestPost.txtIQAgentSetupAuthor_SM.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupAuthor_SM;
                        _SearchRequest.SocialMedia.Title = objIQAgentSearchRequestPost.txtIQAgentSetupTitle_SM != null ? objIQAgentSearchRequestPost.txtIQAgentSetupTitle_SM.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupTitle_SM;


                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupSource_SM))
                        {
                            _SearchRequest.SocialMedia.Sources = objIQAgentSearchRequestPost.txtIQAgentSetupSource_SM.Split(';').Select(a => a.Trim()).ToList();
                        }

                        _SearchRequest.SocialMedia.SearchTerm = new Model.IQAgentXML.MediumSearchTerm();
                        _SearchRequest.SocialMedia.SearchTerm.SearchTerm = objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_SM != null ? objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_SM.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_SM;
                        if (objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_SM != null && objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_SM == true)
                        {
                            _SearchRequest.SocialMedia.SearchTerm.IsUserMaster = true;
                        }
                        else
                        {
                            _SearchRequest.SocialMedia.SearchTerm.IsUserMaster = false;
                        }

                        #region SourceType_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupSourceType_SM != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupSourceType_SM.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupSourceType_SM[0], ZERO, true) == 0)
                            {
                                _SearchRequest.SocialMedia.SourceType_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.SocialMedia.SourceType_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupSourceType_SM)
                                {
                                    _SearchRequest.SocialMedia.SourceType_Set.SourceType.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.SocialMedia.SourceType_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region Exclude Domains

                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupExcludeDomains_SM))
                        {
                            _SearchRequest.SocialMedia.ExlcudeDomains = objIQAgentSearchRequestPost.txtIQAgentSetupExcludeDomains_SM.Split(';').Select(a => a.Trim()).ToList();
                        }

                        #endregion
                    }
                    else
                    {
                        _SearchRequest.SocialMediaSpecified = false;
                    }
                    #endregion

                    #region Facebook

                    if (sessionInformation.Isv4SM && !string.IsNullOrEmpty(objIQAgentSearchRequestPost.chkIQAgentSetup_FB))
                    {
                        _SearchRequest.FacebookSpecified = true;

                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupFBPageID))
                        {
                            List<string> FBPageIDs = objIQAgentSearchRequestPost.txtIQAgentSetupFBPageID.Split(';').Select(s => s.Trim()).Distinct().ToList();
                            _SearchRequest.Facebook.FBPages = setupTempData.lstFBPageModel.Where(s => FBPageIDs.Contains(s.FBPageID.ToString())).Distinct(new FBPageModelComparer()).
                                    Select(s => new Model.IQAgentXML.FBPage() 
                                    { 
                                        ID = s.FBPageID.ToString(), 
                                        Page = s.FBPageUrl 
                                    }).ToList();
                        }

                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupExcludeFBPageID))
                        {
                            List<string> excludeFBPageIDs = objIQAgentSearchRequestPost.txtIQAgentSetupExcludeFBPageID.Split(';').Select(s => s.Trim()).Distinct().ToList();

                            // If a page is excluded, it is not added to the IQ_FBProfile table. When the agent is reopened, the page will not be added to the global page list, and will therefore not be saved on update.
                            // Run the lookup here to ensure that all of the excluded pages are in the global list.
                            GetFBPageUrlsByID(excludeFBPageIDs.Select(s => Convert.ToInt64(s)).ToArray());

                            _SearchRequest.Facebook.ExcludeFBPages = setupTempData.lstFBPageModel.Where(s => excludeFBPageIDs.Contains(s.FBPageID.ToString())).Distinct(new FBPageModelComparer()).
                                    Select(s => new Model.IQAgentXML.FBPage()
                                    {
                                        ID = s.FBPageID.ToString(),
                                        Page = s.FBPageUrl
                                    }).ToList();
                        }

                        _SearchRequest.Facebook.IncludeDefaultPages = objIQAgentSearchRequestPost.chkIQAgentSetupIncludeDefault != null && objIQAgentSearchRequestPost.chkIQAgentSetupIncludeDefault.Value;
                        _SearchRequest.Facebook.SearchTerm = new Model.IQAgentXML.MediumSearchTerm();
                        _SearchRequest.Facebook.SearchTerm.SearchTerm = objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_FB != null ? objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_FB.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_FB;
                        if (objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_FB != null && objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_FB == true)
                        {
                            _SearchRequest.Facebook.SearchTerm.IsUserMaster = true;
                        }
                        else
                        {
                            _SearchRequest.Facebook.SearchTerm.IsUserMaster = false;
                        }
                    }
                    else
                    {
                        _SearchRequest.FacebookSpecified = false;
                    }

                    #endregion

                    #region Instagram

                    if (sessionInformation.Isv4SM && !string.IsNullOrEmpty(objIQAgentSearchRequestPost.chkIQAgentSetup_IG))
                    {
                        _SearchRequest.InstagramSpecified = true;

                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupIGTag))
                        {
                            _SearchRequest.Instagram.UserTagString = objIQAgentSearchRequestPost.txtIQAgentSetupIGTag;
                        }

                        _SearchRequest.Instagram.SearchTerm = new Model.IQAgentXML.MediumSearchTerm();
                        _SearchRequest.Instagram.SearchTerm.SearchTerm = objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_IG != null ? objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_IG.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_IG;
                        _SearchRequest.Instagram.SearchTerm.IsUserMaster = objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_IG != null && objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_IG == true;
                    }
                    else
                    {
                        _SearchRequest.InstagramSpecified = false;
                    }

                    #endregion

                    #region Twitter

                    if (sessionInformation.Isv4TW && !string.IsNullOrEmpty(objIQAgentSearchRequestPost.chkIQAgentSetup_TW))
                    {
                        _SearchRequest.TwitterSpecified = true;

                        if (!string.IsNullOrWhiteSpace(objIQAgentSearchRequestPost.txtIQAgentSetupGnipTag_TW))
                        {
                            _SearchRequest.Twitter.GnipTagList = objIQAgentSearchRequestPost.txtIQAgentSetupGnipTag_TW.Trim().Split(';').Select(a => new Guid(a.Trim())).ToList();
                        }
                    }
                    else
                    {
                        _SearchRequest.TwitterSpecified = false;
                    }
                    #endregion

                    #region ProQuest

                    if (sessionInformation.Isv4PQ && !string.IsNullOrEmpty(objIQAgentSearchRequestPost.chkIQAgentSetup_PQ))
                    {
                        _SearchRequest.PQSpecified = true;

                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupPublication_PQ))
                        {
                            _SearchRequest.PQ.Publications = objIQAgentSearchRequestPost.txtIQAgentSetupPublication_PQ.Split(';').Select(a => a.Trim()).ToList();
                        }
                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupAuthor_PQ))
                        {
                            _SearchRequest.PQ.Authors = objIQAgentSearchRequestPost.txtIQAgentSetupAuthor_PQ.Split(';').Select(a => a.Trim()).ToList();
                        }

                        _SearchRequest.PQ.SearchTerm = new Model.IQAgentXML.MediumSearchTerm();
                        _SearchRequest.PQ.SearchTerm.SearchTerm = objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_PQ != null ? objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_PQ.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_PQ;
                        if (objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_PQ != null && objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_PQ == true)
                        {
                            _SearchRequest.PQ.SearchTerm.IsUserMaster = true;
                        }
                        else
                        {
                            _SearchRequest.PQ.SearchTerm.IsUserMaster = false;
                        }

                        #region Language_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupLanguage_PQ != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupLanguage_PQ.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupLanguage_PQ[0], ZERO, true) == 0)
                            {
                                _SearchRequest.PQ.Language_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.PQ.Language_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupLanguage_PQ)
                                {
                                    _SearchRequest.PQ.Language_Set.Language.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.PQ.Language_Set.IsAllowAll = true;
                        }

                        #endregion
                    }
                    else
                    {
                        _SearchRequest.PQSpecified = false;
                    }
                    #endregion

                    #region LR
                    if (sessionInformation.isv5LRAccess && !string.IsNullOrEmpty(objIQAgentSearchRequestPost.chkIQAgentSetup_LR))
                    {
                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupSearchImageId_LR))
                        {
                            _SearchRequest.LRSpecified = true;
                            _SearchRequest.LR.SearchIDs = objIQAgentSearchRequestPost.txtIQAgentSetupSearchImageId_LR.Split(';').Select(a => a.Trim()).ToList();
                        }
                        else
                        {
                            _SearchRequest.LRSpecified = false;
                        }
                    }
                    #endregion

                    #region LexisNexis

                    if (sessionInformation.IsLN && !string.IsNullOrEmpty(objIQAgentSearchRequestPost.chkIQAgentSetup_LN))
                    {
                        _SearchRequest.LexisNexisSpecified = true;

                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupPublication_LN))
                        {
                            _SearchRequest.LexisNexis.Publications = objIQAgentSearchRequestPost.txtIQAgentSetupPublication_LN.Split(';').Select(a => a.Trim()).ToList();
                        }

                        _SearchRequest.LexisNexis.SearchTerm = new Model.IQAgentXML.MediumSearchTerm();

                        _SearchRequest.LexisNexis.SearchTerm.SearchTerm = objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_LN != null ? objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_LN.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_LN;
                        if (objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_LN != null && objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_LN == true)
                        {
                            _SearchRequest.LexisNexis.SearchTerm.IsUserMaster = true;
                        }
                        else
                        {
                            _SearchRequest.LexisNexis.SearchTerm.IsUserMaster = false;
                        }

                        #region NewsCategory_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupCategory_LN != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupCategory_LN.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupCategory_LN[0], ZERO, true) == 0)
                            {
                                _SearchRequest.LexisNexis.NewsCategory_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.LexisNexis.NewsCategory_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupCategory_LN)
                                {
                                    _SearchRequest.LexisNexis.NewsCategory_Set.NewsCategory.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.LexisNexis.NewsCategory_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region PublicationCategory_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupPublicationCategory_LN != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupPublicationCategory_LN.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupPublicationCategory_LN[0], ZERO, true) == 0)
                            {
                                _SearchRequest.LexisNexis.PublicationCategory_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.LexisNexis.PublicationCategory_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupPublicationCategory_LN)
                                {
                                    _SearchRequest.LexisNexis.PublicationCategory_Set.PublicationCategory.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.LexisNexis.PublicationCategory_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region Genre_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupGenere_LN != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupGenere_LN.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupGenere_LN[0], ZERO, true) == 0)
                            {
                                _SearchRequest.LexisNexis.Genre_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.LexisNexis.Genre_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupGenere_LN)
                                {
                                    _SearchRequest.LexisNexis.Genre_Set.Genre.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.LexisNexis.Genre_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region Region_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_LN != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_LN.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_LN[0], ZERO, true) == 0)
                            {
                                _SearchRequest.LexisNexis.Region_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.LexisNexis.Region_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_LN)
                                {
                                    _SearchRequest.LexisNexis.Region_Set.Region.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.LexisNexis.Region_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region Language_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupLanguage_LN != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupLanguage_LN.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupLanguage_LN[0], ZERO, true) == 0)
                            {
                                _SearchRequest.LexisNexis.Language_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.LexisNexis.Language_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupLanguage_LN)
                                {
                                    _SearchRequest.LexisNexis.Language_Set.Language.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.LexisNexis.Language_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region Country_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_LN != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_LN.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_LN[0], ZERO, true) == 0)
                            {
                                _SearchRequest.LexisNexis.Country_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.LexisNexis.Country_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_LN)
                                {
                                    _SearchRequest.LexisNexis.Country_Set.Country.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.LexisNexis.Country_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region Exclude Domains

                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupExcludeDomains_LN))
                        {
                            _SearchRequest.LexisNexis.ExlcudeDomains = objIQAgentSearchRequestPost.txtIQAgentSetupExcludeDomains_LN.Split(';').Select(a => a.Trim()).ToList();
                        }

                        #endregion
                    }
                    else
                    {
                        _SearchRequest.LexisNexisSpecified = false;
                    }

                    #endregion

                    #region Blog

                    if (sessionInformation.IsFO && !string.IsNullOrEmpty(objIQAgentSearchRequestPost.chkIQAgentSetup_BL))
                    {
                        _SearchRequest.BlogSpecified = true;

                        _SearchRequest.Blog.Author = objIQAgentSearchRequestPost.txtIQAgentSetupAuthor_BL != null ? objIQAgentSearchRequestPost.txtIQAgentSetupAuthor_BL.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupAuthor_BL;
                        _SearchRequest.Blog.Title = objIQAgentSearchRequestPost.txtIQAgentSetupTitle_BL != null ? objIQAgentSearchRequestPost.txtIQAgentSetupTitle_BL.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupTitle_BL;


                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupSource_BL))
                        {
                            _SearchRequest.Blog.Sources = objIQAgentSearchRequestPost.txtIQAgentSetupSource_BL.Split(';').Select(a => a.Trim()).ToList();
                        }

                        _SearchRequest.Blog.SearchTerm = new Model.IQAgentXML.MediumSearchTerm();
                        _SearchRequest.Blog.SearchTerm.SearchTerm = objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_BL != null ? objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_BL.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_BL;
                        if (objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_BL != null && objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_BL == true)
                        {
                            _SearchRequest.Blog.SearchTerm.IsUserMaster = true;
                        }
                        else
                        {
                            _SearchRequest.Blog.SearchTerm.IsUserMaster = false;
                        }

                        #region Exclude Domains

                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupExcludeDomains_BL))
                        {
                            _SearchRequest.Blog.ExlcudeDomains = objIQAgentSearchRequestPost.txtIQAgentSetupExcludeDomains_BL.Split(';').Select(a => a.Trim()).ToList();
                        }

                        #endregion
                    }
                    else
                    {
                        _SearchRequest.BlogSpecified = false;
                    }
                    #endregion

                    #region Forum

                    if (sessionInformation.IsFO && !string.IsNullOrEmpty(objIQAgentSearchRequestPost.chkIQAgentSetup_FO))
                    {
                        _SearchRequest.ForumSpecified = true;

                        _SearchRequest.Forum.Author = objIQAgentSearchRequestPost.txtIQAgentSetupAuthor_FO != null ? objIQAgentSearchRequestPost.txtIQAgentSetupAuthor_FO.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupAuthor_FO;
                        _SearchRequest.Forum.Title = objIQAgentSearchRequestPost.txtIQAgentSetupTitle_FO != null ? objIQAgentSearchRequestPost.txtIQAgentSetupTitle_FO.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupTitle_FO;


                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupSource_FO))
                        {
                            _SearchRequest.Forum.Sources = objIQAgentSearchRequestPost.txtIQAgentSetupSource_FO.Split(';').Select(a => a.Trim()).ToList();
                        }

                        _SearchRequest.Forum.SearchTerm = new Model.IQAgentXML.MediumSearchTerm();
                        _SearchRequest.Forum.SearchTerm.SearchTerm = objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_FO != null ? objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_FO.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_FO;
                        if (objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_FO != null && objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_FO == true)
                        {
                            _SearchRequest.Forum.SearchTerm.IsUserMaster = true;
                        }
                        else
                        {
                            _SearchRequest.Forum.SearchTerm.IsUserMaster = false;
                        }

                        #region SourceType_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupSourceType_FO != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupSourceType_FO.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupSourceType_FO[0], ZERO, true) == 0)
                            {
                                _SearchRequest.Forum.SourceType_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.Forum.SourceType_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupSourceType_FO)
                                {
                                    _SearchRequest.Forum.SourceType_Set.SourceType.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.Forum.SourceType_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region Exclude Domains

                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupExcludeDomains_FO))
                        {
                            _SearchRequest.Forum.ExlcudeDomains = objIQAgentSearchRequestPost.txtIQAgentSetupExcludeDomains_FO.Split(';').Select(a => a.Trim()).ToList();
                        }

                        #endregion
                    }
                    else
                    {
                        _SearchRequest.ForumSpecified = false;
                    }
                    #endregion

                    #region TVEyes Radio

                    if (sessionInformation.Isv4TM && !string.IsNullOrEmpty(objIQAgentSearchRequestPost.chkIQAgentSetup_TM))
                    {
                        _SearchRequest.TMSpecified = true;

                        if (objIQAgentSearchRequestPost.hdnIQAgentSetupTVEyesSettingsKey != "0")
                        {
                            _SearchRequest.TM.TVEyesSettingsKey = objIQAgentSearchRequestPost.hdnIQAgentSetupTVEyesSettingsKey;
                        }

                        Guid tempGuid;
                        _SearchRequest.TM.TVEyesSearchGUID = ""; // Create node even if it's empty
                        if (!string.IsNullOrWhiteSpace(objIQAgentSearchRequestPost.txtIQAgentSetupTVEyesSearchGUID_TM) && Guid.TryParse(objIQAgentSearchRequestPost.txtIQAgentSetupTVEyesSearchGUID_TM, out tempGuid))
                        {
                            _SearchRequest.TM.TVEyesSearchGUID = objIQAgentSearchRequestPost.txtIQAgentSetupTVEyesSearchGUID_TM;
                        }
                    }
                    else
                    {
                        _SearchRequest.TMSpecified = false;
                    }

                    #endregion

                    #region IQRadio

                    if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.chkIQAgentSetup_IQRadio))
                    {
                        _SearchRequest.IQRadioSpecified = true;

                        _SearchRequest.IQRadio.SearchTerm = new Model.IQAgentXML.MediumSearchTerm();

                        _SearchRequest.IQRadio.SearchTerm.SearchTerm = objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_IQRadio != null ? objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_IQRadio.Trim() : objIQAgentSearchRequestPost.txtIQAgentSetupSearchTerm_IQRadio;
                        if (objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_IQRadio != null && objIQAgentSearchRequestPost.chkIQAgentSetupUserMasterSearchTerm_IQRadio == true)
                        {
                            _SearchRequest.IQRadio.SearchTerm.IsUserMaster = true;
                        }
                        else
                        {
                            _SearchRequest.IQRadio.SearchTerm.IsUserMaster = false;
                        }

                        #region IQ_Dma_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupDMA_IQRadio != null)
                        {
                            _SearchRequest.IQRadio.IQ_Dma_Set.SelectionMethod = IQ_DMA_SET_REGION_ATTR;

                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupDMA_IQRadio.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupDMA_IQRadio[0].Trim(), ZERO, true) == 0)
                            {
                                _SearchRequest.IQRadio.IQ_Dma_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.IQRadio.IQ_Dma_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupDMA_IQRadio)
                                {
                                    IQ_Dma IQRadioDMA = allDropDown.TV_DMAList.Where(dma => dma.Name == str).FirstOrDefault();
                                    if (IQRadioDMA != null)
                                    {
                                        _SearchRequest.IQRadio.IQ_Dma_Set.IQ_Dma.Add(new Model.IQAgentXML.IQ_Dma() { num = IQRadioDMA.Num, name = IQRadioDMA.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.IQRadio.IQ_Dma_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region IQ_Station_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupStation_IQRadio != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupStation_IQRadio.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupStation_IQRadio[0].Trim(), ZERO, true) == 0)
                            {
                                _SearchRequest.IQRadio.IQ_Station_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.IQRadio.IQ_Station_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupStation_IQRadio)
                                {
                                    _SearchRequest.IQRadio.IQ_Station_Set.IQ_Station_ID.Add(str);
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.IQRadio.IQ_Station_Set.IsAllowAll = true;
                        }

                        #endregion

                        #region IQ_Region_Set
                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_IQRadio != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_IQRadio.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_IQRadio[0].Trim(), ZERO, true) == 0)
                            {
                                _SearchRequest.IQRadio.IQ_Region_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.IQRadio.IQ_Region_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupRegion_IQRadio)
                                {
                                    IQ_Region IQRadioRegion = allDropDown.TV_RegionList.Where(region => region.Num == Convert.ToInt32(str)).FirstOrDefault();
                                    if (IQRadioRegion != null)
                                    {
                                        _SearchRequest.IQRadio.IQ_Region_Set.IQ_Region.Add(new Model.IQAgentXML.IQ_Region() { num = Convert.ToString(IQRadioRegion.Num), name = IQRadioRegion.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.IQRadio.IQ_Region_Set.IsAllowAll = true;
                        }
                        #endregion

                        #region IQ_Country_Set
                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_IQRadio != null)
                        {
                            if (objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_IQRadio.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_IQRadio[0].Trim(), ZERO, true) == 0)
                            {
                                _SearchRequest.IQRadio.IQ_Country_Set.IsAllowAll = true;
                            }
                            else
                            {
                                _SearchRequest.IQRadio.IQ_Country_Set.IsAllowAll = false;
                                foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupCountry_IQRadio)
                                {
                                    IQ_Country IQRadioCountry = allDropDown.TV_CountryList.Where(c => c.Num == Convert.ToInt32(str)).FirstOrDefault();
                                    if (IQRadioCountry != null)
                                    {
                                        _SearchRequest.IQRadio.IQ_Country_Set.IQ_Country.Add(new Model.IQAgentXML.IQ_Country() { num = Convert.ToString(IQRadioCountry.Num), name = IQRadioCountry.Name });
                                    }
                                }
                            }
                        }
                        else
                        {
                            _SearchRequest.IQRadio.IQ_Country_Set.IsAllowAll = true;
                        }
                        #endregion

                        #region Zip Codes
                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupZipCodes_IQRadio))
                        {
                            _SearchRequest.IQRadio.ZipCodes = objIQAgentSearchRequestPost.txtIQAgentSetupZipCodes_IQRadio.Split(';').Select(a => a.Trim()).ToList();
                        }
                        #endregion

                        #region Exclude_IQ_Dma_Set

                        if (objIQAgentSearchRequestPost.ddlIQAgentSetupExcludeDMA_IQRadio != null && !(objIQAgentSearchRequestPost.ddlIQAgentSetupExcludeDMA_IQRadio.Count == 1 && string.Compare(objIQAgentSearchRequestPost.ddlIQAgentSetupExcludeDMA_IQRadio[0].Trim(), ZERO, true) == 0))
                        {
                            _SearchRequest.IQRadio.Exclude_IQ_Dma_Set.SelectionMethod = IQ_DMA_SET_REGION_ATTR;

                            foreach (string str in objIQAgentSearchRequestPost.ddlIQAgentSetupExcludeDMA_IQRadio)
                            {
                                IQ_Dma IQRadioDMA = allDropDown.TV_DMAList.Where(dma => dma.Name == str).FirstOrDefault();
                                if (IQRadioDMA != null)
                                {
                                    _SearchRequest.IQRadio.Exclude_IQ_Dma_Set.Exclude_IQ_Dma.Add(new Model.IQAgentXML.IQ_Dma() { num = IQRadioDMA.Num, name = IQRadioDMA.Name });
                                }
                            }
                        }

                        #endregion

                        #region Exclude Zip Codes
                        if (!string.IsNullOrEmpty(objIQAgentSearchRequestPost.txtIQAgentSetupExcludeZipCodes_IQRadio))
                        {
                            _SearchRequest.IQRadio.ExcludeZipCodes = objIQAgentSearchRequestPost.txtIQAgentSetupExcludeZipCodes_IQRadio.Split(';').Select(a => a.Trim()).ToList();
                        }
                        #endregion
                    }
                    else
                    {
                        _SearchRequest.IQRadioSpecified = false;
                    }
                    #endregion

                    return _SearchRequest;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult DisplayCustomSettings()
        {
            try
            {
                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                Dictionary<string, IQClient_CustomSettingsModel> discCustomSettingsModel = clientLogic.GetClientAllCustomSettings(new Guid(ClientGUID));
                string strHTML = RenderPartialToString(PATH_SetupClientCustomSettingsDisplay, discCustomSettingsModel);
                return Json(new
                {
                    isSuccess = true,
                    HTML = strHTML
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult DisplayFliq_ClientApplication(string p_ApplicationName, bool? p_IsNext = null)
        {
            try
            {
                ActiveUser sessionInformation = ActiveUserMgr.GetActiveUser();
                setupTempData = GetTempData();

                if (p_IsNext != null)
                {
                    if (p_IsNext == true)
                    {
                        if (setupTempData.fliq_ClientApplicationHasMoreRecords == true)
                        {
                            setupTempData.fliq_ClientApplicationPageNumber = setupTempData.fliq_ClientApplicationPageNumber + 1;
                        }
                        else
                        {
                            // IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                            return Json(new
                            {
                                isSuccess = false
                            });
                        }
                    }
                    else
                    {
                        if (setupTempData.fliq_ClientApplicationPageNumber > 0)
                        {
                            setupTempData.fliq_ClientApplicationPageNumber = setupTempData.fliq_ClientApplicationPageNumber - 1;
                        }
                        else
                        {
                            return Json(new
                            {
                                isSuccess = false
                            });
                        }

                    }
                }
                else
                {
                    setupTempData.fliq_ClientApplicationHasMoreRecords = true;
                    setupTempData.fliq_ClientApplicationPageNumber = 0;
                }

                int totalResults = 0;
                int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ClientApplicationPageSize"]);
                fliq_ClientApplicationLogic fliq_ClientApplicationLogic = (fliq_ClientApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_ClientApplication);
                List<fliQ_ClientApplicationModel> lstfliQ_ClientApplicationModel = fliq_ClientApplicationLogic.GetAllfliq_ClientApplication(sessionInformation.ClientName, p_ApplicationName, setupTempData.fliq_ClientApplicationPageNumber, pageSize, out totalResults);

                if (totalResults > ((setupTempData.fliq_ClientApplicationPageNumber + 1) * pageSize))
                {
                    setupTempData.fliq_ClientApplicationHasMoreRecords = true;
                }
                else
                {
                    setupTempData.fliq_ClientApplicationHasMoreRecords = false;
                }

                string strHTML = RenderPartialToString(PATH_SetupAdminFliq_ClientApplicationListPartialView, lstfliQ_ClientApplicationModel);

                string strRecordLabel = " ";
                if (lstfliQ_ClientApplicationModel.Count > 0)
                {
                    strRecordLabel = "" + ((setupTempData.fliq_ClientApplicationPageNumber * pageSize) + 1).ToString() + " - " + ((setupTempData.fliq_ClientApplicationPageNumber * pageSize) + lstfliQ_ClientApplicationModel.Count).ToString() + " Of " + totalResults.ToString() + "";
                }
                SetTempData(setupTempData);

                return Json(new
                {
                    isSuccess = true,
                    hasMoreRecords = setupTempData.fliq_ClientApplicationHasMoreRecords,
                    hasPreviousRecords = setupTempData.fliq_ClientApplicationPageNumber > 0 ? true : false,
                    recordLabel = strRecordLabel,
                    HTML = strHTML
                });
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpPost]
        public JsonResult DisplayFliq_CustomerApplication(string p_CustomerName, bool p_IsAsc, string p_SortColumn, bool? p_IsNext = null)
        {
            try
            {

                ActiveUser sessionInformation = ActiveUserMgr.GetActiveUser();
                p_CustomerName = string.IsNullOrEmpty(p_CustomerName) ? null : p_CustomerName;
                setupTempData = GetTempData();

                if (p_IsNext != null)
                {
                    if (p_IsNext == true)
                    {
                        if (setupTempData.fliq_CustomerApplicationHasMoreRecords == true)
                        {
                            setupTempData.fliq_CustomerApplicationPageNumber = setupTempData.fliq_CustomerApplicationPageNumber + 1;
                        }
                        else
                        {
                            // IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                            return Json(new
                            {
                                isSuccess = false
                            });
                        }
                    }
                    else
                    {
                        if (setupTempData.fliq_CustomerApplicationPageNumber > 0)
                        {
                            setupTempData.fliq_CustomerApplicationPageNumber = setupTempData.fliq_CustomerApplicationPageNumber - 1;
                        }
                        else
                        {
                            return Json(new
                            {
                                isSuccess = false
                            });
                        }

                    }
                }
                else
                {
                    setupTempData.fliq_CustomerApplicationHasMoreRecords = true;
                    setupTempData.fliq_CustomerApplicationPageNumber = 0;
                }

                int totalResults = 0;
                int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CustomerApplicationPageSize"]);
                fliq_CustomerApplicationLogic fliq_CustomerApplicationLogic = (fliq_CustomerApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_CustomerApplication);
                List<fliQ_CustomerApplicationModel> lstfliQ_CustomerApplicationModel = fliq_CustomerApplicationLogic.GetAllfliq_CustomerApplication(sessionInformation.ClientName, p_CustomerName, setupTempData.fliq_CustomerApplicationPageNumber, pageSize, p_IsAsc, p_SortColumn, out totalResults);

                if (totalResults > ((setupTempData.fliq_CustomerApplicationPageNumber + 1) * pageSize))
                {
                    setupTempData.fliq_CustomerApplicationHasMoreRecords = true;
                }
                else
                {
                    setupTempData.fliq_CustomerApplicationHasMoreRecords = false;
                }

                string strHTML = RenderPartialToString(PATH_SetupAdminFliq_CustomerApplicationListPartialView, lstfliQ_CustomerApplicationModel);

                string strRecordLabel = " ";
                if (lstfliQ_CustomerApplicationModel.Count > 0)
                {
                    strRecordLabel = "" + ((setupTempData.fliq_CustomerApplicationPageNumber * pageSize) + 1).ToString() + " - " + ((setupTempData.fliq_CustomerApplicationPageNumber * pageSize) + lstfliQ_CustomerApplicationModel.Count).ToString() + " Of " + totalResults.ToString() + "";
                }
                SetTempData(setupTempData);

                return Json(new
                {
                    isSuccess = true,
                    hasMoreRecords = setupTempData.fliq_CustomerApplicationHasMoreRecords,
                    hasPreviousRecords = setupTempData.fliq_CustomerApplicationPageNumber > 0 ? true : false,
                    recordLabel = strRecordLabel,
                    HTML = strHTML
                });
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpPost]
        public JsonResult DisplayJobStatusList(bool p_IsAsc, string p_SortColumn, int? p_JobTypeID, bool p_IsLoadPartial = true, bool? p_IsNext = null)
        {
            try
            {

                ActiveUser sessionInformation = ActiveUserMgr.GetActiveUser();
                setupTempData = GetTempData();
                if (setupTempData == null)
                    setupTempData = new SetupTempData();

                if (p_IsNext != null)
                {
                    if (p_IsNext == true)
                    {
                        if (setupTempData.JobStatusHasMoreRecords == true)
                        {
                            setupTempData.JobStatusPageNumber = setupTempData.JobStatusPageNumber + 1;
                        }
                        else
                        {
                            // IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                            return Json(new
                            {
                                isSuccess = false
                            });
                        }
                    }
                    else
                    {
                        if (setupTempData.JobStatusPageNumber > 0)
                        {
                            setupTempData.JobStatusPageNumber = setupTempData.JobStatusPageNumber - 1;
                        }
                        else
                        {
                            return Json(new
                            {
                                isSuccess = false
                            });
                        }

                    }
                }
                else
                {
                    setupTempData.JobStatusHasMoreRecords = true;
                    setupTempData.JobStatusPageNumber = 0;
                }

                int totalResults = 0;
                int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["JobStatusPageSize"]);
                JobStatusLogic JobStatusLogic = (JobStatusLogic)LogicFactory.GetLogic(LogicType.JobStatus);
                List<JobStatusModel> lstJobStatusModel = JobStatusLogic.GetJobStatusByClientGuid(sessionInformation.ClientGUID, setupTempData.JobStatusPageNumber, pageSize, p_IsAsc, p_SortColumn, p_JobTypeID, out totalResults);

                if (totalResults > ((setupTempData.JobStatusPageNumber + 1) * pageSize))
                {
                    setupTempData.JobStatusHasMoreRecords = true;
                }
                else
                {
                    setupTempData.JobStatusHasMoreRecords = false;
                }

                string strHTML = string.Empty;
                if (p_IsLoadPartial)
                {
                    strHTML = RenderPartialToString(PATH_SetupJobStatusListPartialView, lstJobStatusModel);
                }
                else
                {
                    strHTML = RenderPartialToString(PATH_SetupJobStatusPartialView, lstJobStatusModel);
                }

                string strRecordLabel = " ";
                if (lstJobStatusModel.Count > 0)
                {
                    strRecordLabel = "" + ((setupTempData.JobStatusPageNumber * pageSize) + 1).ToString() + " - " + ((setupTempData.JobStatusPageNumber * pageSize) + lstJobStatusModel.Count).ToString() + " Of " + totalResults.ToString() + "";
                }
                SetTempData(setupTempData);

                return Json(new
                {
                    isSuccess = true,
                    hasMoreRecords = setupTempData.JobStatusHasMoreRecords,
                    hasPreviousRecords = setupTempData.JobStatusPageNumber > 0 ? true : false,
                    recordLabel = strRecordLabel,
                    HTML = strHTML
                });
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpPost]
        public JsonResult DisplayFliq_UGCUploads(bool p_IsAsc, string p_SortColumn, bool? p_IsNext = null)
        {
            try
            {

                ActiveUser sessionInformation = ActiveUserMgr.GetActiveUser();

                setupTempData = GetTempData();

                if (p_IsNext != null)
                {
                    if (p_IsNext == true)
                    {
                        if (setupTempData.fliq_UGCUploadHasMoreRecords == true)
                        {
                            setupTempData.fliq_UGCUploadPageNumber = setupTempData.fliq_UGCUploadPageNumber + 1;
                        }
                        else
                        {
                            // IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                            return Json(new
                            {
                                isSuccess = false
                            });
                        }
                    }
                    else
                    {
                        if (setupTempData.fliq_UGCUploadPageNumber > 0)
                        {
                            setupTempData.fliq_UGCUploadPageNumber = setupTempData.fliq_UGCUploadPageNumber - 1;
                        }
                        else
                        {
                            return Json(new
                            {
                                isSuccess = false
                            });
                        }

                    }
                }
                else
                {
                    setupTempData.fliq_UGCUploadHasMoreRecords = true;
                    setupTempData.fliq_UGCUploadPageNumber = 0;
                }

                int totalResults = 0;
                int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["FliqUploadPageSize"]);
                fliq_CustomerApplicationLogic fliq_CustomerApplicationLogic = (fliq_CustomerApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_CustomerApplication);
                List<fliQ_UploadTrackingModel> lstfliQ_UploadTrackingModel = fliq_CustomerApplicationLogic.Getfliq_UplaodsByClientGuid(sessionInformation.ClientGUID, setupTempData.fliq_UGCUploadPageNumber, pageSize, p_IsAsc, p_SortColumn, out totalResults);

                if (totalResults > ((setupTempData.fliq_UGCUploadPageNumber + 1) * pageSize))
                {
                    setupTempData.fliq_UGCUploadHasMoreRecords = true;
                }
                else
                {
                    setupTempData.fliq_UGCUploadHasMoreRecords = false;
                }

                string strHTML = RenderPartialToString(PATH_SetupAdminFliq_UGCUploadTrackingListPartialView, lstfliQ_UploadTrackingModel);

                string strRecordLabel = " ";
                if (lstfliQ_UploadTrackingModel.Count > 0)
                {
                    strRecordLabel = "" + ((setupTempData.fliq_UGCUploadPageNumber * pageSize) + 1).ToString() + " - " + ((setupTempData.fliq_UGCUploadPageNumber * pageSize) + lstfliQ_UploadTrackingModel.Count).ToString() + " Of " + totalResults.ToString() + "";
                }
                SetTempData(setupTempData);

                return Json(new
                {
                    isSuccess = true,
                    hasMoreRecords = setupTempData.fliq_UGCUploadHasMoreRecords,
                    hasPreviousRecords = setupTempData.fliq_UGCUploadPageNumber > 0 ? true : false,
                    recordLabel = strRecordLabel,
                    HTML = strHTML
                });
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpPost]
        public JsonResult ResetJob(long ID, long requestID, string resetProcedureName)
        {
            try
            {
                JobStatusLogic JobStatusLogic = (JobStatusLogic)LogicFactory.GetLogic(LogicType.JobStatus);
                bool isSuccess = JobStatusLogic.ResetJob(ID, requestID, resetProcedureName);

                return Json(new
                {
                    isSuccess = isSuccess,
                    requestedDateTime = DateTime.Now.ToString("MMM dd, yyyy hh:mm:ss tt")
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
        public JsonResult GetGoogleAnalytics()
        {
            try
            {
                GoogleLogic googleLogic = (GoogleLogic)LogicFactory.GetLogic(LogicType.Google);
                bool hasAccess = googleLogic.CheckClientAccess(new Guid(ClientGUID));
                string clientID = googleLogic.GetClientID();

                Dictionary<string, object> dictModel = new Dictionary<string, object>();
                dictModel.Add("ClientID", clientID);
                dictModel.Add("HasAccess", hasAccess);

                string strHTML = RenderPartialToString(PATH_SetupGoogleSignInPartialView, dictModel);
                return Json(new
                {
                    isSuccess = true,
                    HTML = strHTML
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult UpdateAuthCode(string authCode)
        {
            try
            {
                GoogleLogic googleLogic = (GoogleLogic)LogicFactory.GetLogic(LogicType.Google);
                googleLogic.UpdateAuthCode(new Guid(ClientGUID), authCode);

                return Json(new
                {
                    isSuccess = true
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
        public JsonResult GetFBPageUrlsByID(long[] FBPageIDs)
        {
            try
            {
                setupTempData = GetTempData();
                List<FBPageModel> lstFBPageModel = setupTempData.lstFBPageModel;
                List<string> FBPages = new List<string>();
                List<long> invalidIDs = new List<long>();

                FBPageModel FBPageModel;
                foreach (long FBPageID in FBPageIDs.Distinct())
                {
                    FBPageModel = lstFBPageModel.Where(s => s.FBPageID == FBPageID).FirstOrDefault();

                    if (FBPageModel != null && !String.IsNullOrWhiteSpace(FBPageModel.FBPageUrl))
                    {
                        FBPages.Add(FBPageModel.FBPageUrl);
                    }
                    else
                    {
                        string response = GetFacebookPageFromAPI(FBPageID.ToString());
                        if (!String.IsNullOrWhiteSpace(response))
                        {
                            Dictionary<string, object> objFBPage = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
                            FBPages.Add(objFBPage["link"].ToString());

                            if (FBPageModel != null)
                            {
                                FBPageModel.FBPageUrl = objFBPage["link"].ToString();
                            }
                            else
                            {
                                lstFBPageModel.Add(new FBPageModel() { FBPageID = Convert.ToInt64(objFBPage["id"]), FBPageUrl = objFBPage["link"].ToString() });
                            }

                            SetTempData(setupTempData);
                        }
                        else
                        {
                            invalidIDs.Add(FBPageID);
                        }
                    }
                }

                return Json(new
                {
                    isSuccess = true,
                    FBPages = FBPages.Count > 0 ? String.Join(";", FBPages) : String.Empty,
                    invalidIDs = invalidIDs.Count > 0 ? String.Join(", ", invalidIDs) : String.Empty
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
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult GetFBPageIDsByUrl(string[] FBPages)
        {
            try
            {
                setupTempData = GetTempData();
                List<FBPageModel> lstFBPageModel = setupTempData.lstFBPageModel;
                List<long> FBPageIDs = new List<long>();
                List<string> invalidPages = new List<string>();

                FBPageModel FBPageModel;
                foreach (string FBPage in FBPages.Distinct())
                {
                    // Since page urls can be entered in a variety of formats, match on just the page name.
                    // For example, https://www.facebook.com/audi will match just "audi". 
                    FBPageModel = lstFBPageModel.Where(s => GetTruncatedFBPageUrl(s.FBPageUrl.ToLower()) == GetTruncatedFBPageUrl(FBPage.ToLower())).FirstOrDefault();

                    if (FBPageModel != null && FBPageModel.FBPageID != 0)
                    {
                        FBPageIDs.Add(FBPageModel.FBPageID);
                    }
                    else
                    {
                        string response = GetFacebookPageFromAPI(FBPage);
                        if (!String.IsNullOrWhiteSpace(response))
                        {
                            Dictionary<string, object> objFBPage = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
                            FBPageIDs.Add(Convert.ToInt64(objFBPage["id"]));

                            if (FBPageModel != null)
                            {
                                FBPageModel.FBPageID = Convert.ToInt64(objFBPage["id"]);
                            }
                            else
                            {
                                lstFBPageModel.Add(new FBPageModel() { FBPageID = Convert.ToInt64(objFBPage["id"]), FBPageUrl = objFBPage["link"].ToString() });
                            }

                            SetTempData(setupTempData);
                        }
                        else
                        {
                            invalidPages.Add(FBPage);
                        }
                    }
                }

                return Json(new
                {
                    isSuccess = true,
                    FBPageIDs = FBPageIDs.Count > 0 ? String.Join(";", FBPageIDs) : String.Empty,
                    invalidPages = invalidPages.Count > 0 ? String.Join(", ", invalidPages) : String.Empty
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
            finally { TempData.Keep(); }
        }

        private bool CheckForImage(HttpPostedFileBase flImage)
        {
            string[] Extentions = ConfigurationManager.AppSettings["ClientReportImageExtensions"].Split(new char[] { ',' });

            if (flImage != null && flImage.ContentLength > 0)
            {
                if (!Extentions.Contains(System.IO.Path.GetExtension(flImage.FileName).ToLower().Substring(1)))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
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

        private string GetFacebookPageFromAPI(string id)
        {
            string response = string.Empty;

            try
            {
                Dictionary<string, string> authHeader = new Dictionary<string,string>();
                authHeader.Add("Authorization", "Bearer 461274904033218|3512c59b5271322ba1ac66b7378c592c");

                response = Shared.Utility.CommonFunctions.DoHttpGetRequest("https://graph.facebook.com/v2.4/" + id + "?fields=id,link", null, null, authHeader);
            }
            catch (WebException ex)
            {
                using (WebResponse webResponse = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)webResponse;
                    if (httpResponse.StatusCode != HttpStatusCode.BadRequest)
                    {
                        // BadRequest means an invalid ID was entered. There's no need to log that.
                        IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
            }

            return response;
        }

        private string GetTruncatedFBPageUrl(string FBPageUrl)
        {
            if (FBPageUrl.EndsWith("/"))
            {
                FBPageUrl = FBPageUrl.Substring(0, FBPageUrl.Length - 1);
            }
            if (!String.IsNullOrWhiteSpace(FBPageUrl) && FBPageUrl.IndexOf("/") > 0)
            {
                return FBPageUrl.Substring(FBPageUrl.LastIndexOf("/") + 1);
            }
            return FBPageUrl;
        }
        
        [HttpPost]
        public JsonResult LoadInstagramSetup()
        {
            try
            {
                InstagramLogic instagramLogic = (InstagramLogic)LogicFactory.GetLogic(LogicType.Instagram);
                bool hasAccess = instagramLogic.CheckClientAccess(new Guid(ClientGUID));
                string clientID = instagramLogic.GetClientID();

                Dictionary<string, object> dictModel = new Dictionary<string, object>();
                dictModel.Add("ClientID", clientID);
                dictModel.Add("HasAccess", hasAccess);

                string strHTML = RenderPartialToString(PATH_SetupInstagramSetupPartialView, dictModel);
                return Json(new
                {
                    isSuccess = true,
                    HTML = strHTML
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
        }

        #region Logo Recognition
        [HttpPost]
        public JsonResult GetSearchImageIDs(long BrandID)
        {
            try
            {
                setupTempData = GetTempData();
                ImagiQLogic ImagiQLogic = (ImagiQLogic)LogicFactory.GetLogic(LogicType.ImagiQ);
                List<long> lstSearchImageIds = ImagiQLogic.GetSearchImageIDs(BrandID);

                return Json(new
                {
                    isSuccess = true,
                    lstSearchImageIds = lstSearchImageIds
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
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult GetSearchImagesById(List<long> LRSearchIDs)
        {
            try
            {
                setupTempData = GetTempData();
                List<string> lstSearchImages = new List<string>();
                ImagiQLogic ImagiQLogic = (ImagiQLogic)LogicFactory.GetLogic(LogicType.ImagiQ);
                List<long> lstSearchImageIDs = ImagiQLogic.GetSearchImagesById(LRSearchIDs, out lstSearchImages);

                List<long> invalidIDs = LRSearchIDs.Except(lstSearchImageIDs).ToList();
                var invalidIDsStr = invalidIDs.Count() > 0 ? String.Join(", ", invalidIDs) : "";

                return Json(new
                {
                    isSuccess = true,
                    lstSearchImages = lstSearchImages,
                    lstSearchImageIDs = lstSearchImageIDs,
                    lstInvalidIDs = invalidIDsStr
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
            finally { TempData.Keep(); }
        }
        #endregion

        #region Utility

        public SetupTempData GetTempData()
        {

            SetupTempData setupTempData = (SetupTempData)TempData["SetupTempData"];

            return setupTempData;
        }

        public void SetTempData(SetupTempData p_SetupTempData)
        {
            TempData["SetupTempData"] = p_SetupTempData;
            TempData.Keep("SetupTempData");
        }

        public List<JobType> GetJobTypeList()
        {
            try
            {
                List<JobType> lstJobType = new List<JobType>();
                JobStatusLogic JobStatusLogic = (JobStatusLogic)LogicFactory.GetLogic(LogicType.JobStatus);

                lstJobType = JobStatusLogic.GetJobTypeList();
                return lstJobType;
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return null;
            }
        }

        #endregion

        #region CampaignSetup

        [HttpPost]
        public JsonResult DisplayCampaignSetupContent()
        {
            try
            {
                ActiveUser sessionInfo = ActiveUserMgr.GetActiveUser();
                AnalyticsLogic analyticsLogic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                IQAgentLogic agentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                List<AnalyticsCampaign> campaigns = analyticsLogic.GetCampaigns(sessionInfo.ClientGUID);
                List<IQAgent_SearchRequestModel> agents;
                setupTempData = GetTempData();

                setupTempData.ClientCampaigns = campaigns;

                if (setupTempData.ClientAgents == null)
                {
                    agents = agentLogic.SelectIQAgentSearchRequestByClientGuid(sessionInfo.ClientGUID.ToString());
                    setupTempData.ClientAgents = agents;
                }
                else
                {
                    agents = setupTempData.ClientAgents;
                }

                SetTempData(setupTempData);

                Dictionary<string, object> dictModel = new Dictionary<string, object>();
                dictModel.Add("campaigns", campaigns);
                dictModel.Add("agents", agents);

                string strHTML = RenderPartialToString(PATH_SetupCampaignSetupDisplay, dictModel);
                return Json(new {
                    isSuccess = true,
                    HTML = strHTML
                });
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                return Json(new {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpPost]
        public ContentResult GetCampaignSetupEdit(Int64 campaignID)
        {
            try
            {
                AnalyticsLogic analyticsLogic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                setupTempData = GetTempData();
                AnalyticsCampaign campaign;

                if (setupTempData.ClientCampaigns.Where(camp => camp.CampaignID == campaignID).Any())
                {
                    // Campaign exists in temp
                    campaign = setupTempData.ClientCampaigns.Where(camp => camp.CampaignID == campaignID).First();
                }
                else
                {
                    campaign = analyticsLogic.GetCampaignByID(campaignID);
                    setupTempData.ClientCampaigns.Add(campaign);
                }

                SetTempData(setupTempData);

                dynamic json = new ExpandoObject();
                json.isSuccess = true;
                json.campaignName = campaign.CampaignName;
                json.searchRequestID = campaign.SearchRequestID;
                json.startDate = campaign.StartDate.ToShortDateString();
                json.endDate = campaign.EndDate.ToShortDateString();

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                return Content(CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpPost]
        public ContentResult SaveCampaign(string campaignID, string campaignName, int agentSRID, DateTime startDate, DateTime endDate)
        {
            try
            {
                endDate = endDate.AddHours(23).AddMinutes(59);
                AnalyticsLogic analyticsLogic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                DateTime startDateGMT = (DateTime)CommonFunctions.GetGMTandDSTTime(startDate);
                DateTime endDateGMT = (DateTime)CommonFunctions.GetGMTandDSTTime(endDate);
                DateTime? modifiedDate = null;

                if (campaignID != "null")
                {
                    modifiedDate = analyticsLogic.EditCampaign(Convert.ToInt64(campaignID), campaignName, agentSRID, startDate, endDate, startDateGMT, endDateGMT);
                }
                else
                {
                    campaignID = analyticsLogic.CreateCampaign(campaignName, agentSRID, startDate, endDate, startDateGMT, endDateGMT).ToString();
                }

                dynamic json = new ExpandoObject();
                json.isSuccess = true;
                json.CampaignID = campaignID;
                json.ModifiedDate = modifiedDate;

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                return Content(CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
        }

        [HttpPost]
        public ContentResult DeleteCampaign(Int64 campaignID)
        {
            try
            {
                AnalyticsLogic analyticsLogic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                analyticsLogic.DeleteCampaign(campaignID);

                dynamic json = new ExpandoObject();
                json.isSuccess = true;

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                return Content(CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
        }

        [HttpPost]
        public ContentResult StopCampaign(Int64 campaignID, DateTime endDate)
        {
            try
            {
                AnalyticsLogic analyticsLogic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                DateTime modifiedDate = analyticsLogic.EditCampaign(campaignID, null, null, null, endDate, null, CommonFunctions.GetGMTandDSTTime(endDate));
                setupTempData = GetTempData();
                setupTempData.ClientCampaigns.Where(c => c.CampaignID == campaignID).First().EndDate = endDate;
                setupTempData.ClientCampaigns.Where(c => c.CampaignID == campaignID).First().ModifiedDate = modifiedDate;
                SetTempData(setupTempData);

                dynamic json = new ExpandoObject();
                json.isSuccess = true;
                json.modifiedDate = String.Format("{0:MMMM dd, yyyy hh:mm:ss tt}", modifiedDate); // January 01, 2016 01:00:00 PM

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                return Content(CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep();
            }
        }

        #endregion

        #region External Rule Setup

        [HttpPost]
        public JsonResult DeleteExternalRules(long searchRequestID, Guid? trackGuid, long? tvEyesSettingsKey)
        {
            try
            {
                int success = 1;
                IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                if (trackGuid.HasValue)
                {
                    success = iQAgentLogic.DeleteTwitterRule(trackGuid.Value);
                }
                if (success == 1 && tvEyesSettingsKey.HasValue)
                {
                    success = iQAgentLogic.DeleteTVEyesRule(tvEyesSettingsKey.Value);
                }

                return Json(new
                {
                    isSuccess = success == 1
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

        #region Twitter

        [HttpPost]
        public JsonResult GetTwitterRule(Guid trackGuid)
        {
            try
            {
                IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                string twitterRule = iQAgentLogic.GetTwitterRuleByTrackGUID(trackGuid);

                List<string> lstRules = new List<string>();
                if (!String.IsNullOrEmpty(twitterRule))
                {
                    XDocument xDoc = XDocument.Parse(twitterRule);
                    lstRules = xDoc.Descendants("value").Select(s => s.Value).ToList();
                }

                return Json(new
                {
                    isSuccess = true,
                    rules = lstRules
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
        public JsonResult SaveTwitterRule(Guid? trackGuid, List<string> twitterRules, string agentName, long searchRequestID)
        {
            try
            {
                ActiveUser sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                int success = 0;
                bool isInsert = false;
                if (trackGuid.HasValue)
                {
                    success = iQAgentLogic.UpdateTwitterRule(trackGuid.Value, twitterRules, agentName);
                }
                else
                {
                    isInsert = true;
                    trackGuid = Guid.NewGuid();
                    success = iQAgentLogic.InsertTwitterRule(sessionInformation.ClientGUID, trackGuid.Value, twitterRules, agentName, searchRequestID);
                }

                if (success == 1)
                {
                    iQAgentLogic.InsertTwitterRuleJob(sessionInformation.ClientGUID, sessionInformation.CustomerGUID, searchRequestID);
                }

                return Json(new
                {
                    isSuccess = success == 1,
                    trackGuid = trackGuid,
                    isInsert = isInsert
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

        #region TVEyes

        [HttpPost]
        public JsonResult GetTVEyesRule(long tvEyesSettingsKey)
        {
            try
            {
                IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                string tvEyesRule = iQAgentLogic.GetTVEyesRuleByID(tvEyesSettingsKey);

                return Json(new
                {
                    isSuccess = true,
                    ruleText = tvEyesRule
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
        public JsonResult SaveTVEyesRule(long ruleID, string searchTerm, string agentName, long searchRequestID)
        {
            try
            {
                ActiveUser sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                int result = 0;
                bool isInsert = false;
                if (ruleID > 0)
                {
                    result = iQAgentLogic.UpdateTVEyesRule(ruleID, searchTerm, agentName);
                }
                else
                {
                    isInsert = true;
                    result = iQAgentLogic.InsertTVEyesRule(sessionInformation.ClientGUID, searchRequestID, searchTerm, agentName);
                }

                if (result > 0)
                {
                    iQAgentLogic.InsertTVEyesRuleJob(sessionInformation.ClientGUID, sessionInformation.CustomerGUID, searchRequestID);
                }

                return Json(new
                {
                    isSuccess = result > 0,
                    ruleID = result, // insert only
                    isInsert = isInsert
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

        #endregion
    }


}
