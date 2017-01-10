using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.WebApplication.Models.TempData;
using IQMedia.Web.Logic;
using IQMedia.Model;
using System.IO;
using IQMedia.Web.Logic.Base;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class IQNotificationController : Controller
    {
        SetupTempData setupTempData = null;

        string PATH_SetupIQNotificationSettingsListPartialView = "~/Views/Setup/_IQNotificationSettingsList.cshtml";
        string PATH_SetupIQNotificationSettingsAddEditPartialView = "~/Views/Setup/_IQNotificationSettingsAddEdit.cshtml";

        private string _IQNotificationExistMessage = "IQ Notification Already Exists.";

        [HttpPost]
        public JsonResult DisplayClientNotification(bool? p_IsNext = null)
        {
            try
            {
                setupTempData = GetTempData();
                ActiveUser sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                if (p_IsNext != null)
                {
                    if (p_IsNext == true)
                    {
                        if (setupTempData.IQNotificationHasMoreRecords == true)
                        {
                            setupTempData.IQNotificationPageNumber = setupTempData.IQNotificationPageNumber + 1;
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
                        if (setupTempData.IQNotificationPageNumber > 0)
                        {
                            setupTempData.IQNotificationPageNumber = setupTempData.IQNotificationPageNumber - 1;
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
                    setupTempData.IQNotificationHasMoreRecords = true;
                    setupTempData.IQNotificationPageNumber = 0;
                }

                int totalResults = 0;
                int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IQNotificationPageSize"]);
                IQNotificationSettingsLogic iQNotificationSettingsLogic = (IQNotificationSettingsLogic)LogicFactory.GetLogic(LogicType.IQNotification);
                List<IQNotifationSettingsModel> lstIQNotifationSettingsModel = iQNotificationSettingsLogic.SelectIQNotifcationsByClientGuid(sessionInformation.ClientGUID, setupTempData.IQNotificationPageNumber, pageSize, out totalResults);

                if (totalResults > ((setupTempData.IQNotificationPageNumber + 1) * pageSize))
                {
                    setupTempData.IQNotificationHasMoreRecords = true;
                }
                else
                {
                    setupTempData.IQNotificationHasMoreRecords = false;
                }

                string strHTML = RenderPartialToString(PATH_SetupIQNotificationSettingsListPartialView, lstIQNotifationSettingsModel);

                string strRecordLabel = " ";
                if (lstIQNotifationSettingsModel.Count > 0)
                {
                    strRecordLabel = "" + ((setupTempData.IQNotificationPageNumber * pageSize) + 1).ToString() + " - " + ((setupTempData.IQNotificationPageNumber * pageSize) + lstIQNotifationSettingsModel.Count).ToString() + " Of " + totalResults.ToString() + "";
                }
                SetTempData(setupTempData);

                return Json(new
                {
                    isSuccess = true,
                    hasMoreRecords = setupTempData.IQNotificationHasMoreRecords,
                    hasPreviousRecords = setupTempData.IQNotificationPageNumber > 0 ? true : false,
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
        public JsonResult GetIQNotificationByID(Int64 p_ID)
        {
            try
            {

                ActiveUser sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                IQNotificationSettingsLogic iQNotificationSettingsLogic = (IQNotificationSettingsLogic)LogicFactory.GetLogic(LogicType.IQNotification);

                IQNotifationSettingsPostModel objIQAgent_DailyDigestPostModel = new IQNotifationSettingsPostModel();
                objIQAgent_DailyDigestPostModel.IQNotifationSettings_DropDown = iQNotificationSettingsLogic.GetIQNotificationDropDown(sessionInformation.ClientGUID);

                if (p_ID == 0)
                {
                    objIQAgent_DailyDigestPostModel.IQNotifationSettings = new IQNotifationSettingsModel();
                }
                else
                {
                    objIQAgent_DailyDigestPostModel.IQNotifationSettings = iQNotificationSettingsLogic.SelectIQNotifcationsByID(p_ID);

                    if (IQCommon.CommonFunctions.GetAccessibleSubMediaType(sessionInformation.MediaTypes).Select(m => m.SubMediaType).Except(objIQAgent_DailyDigestPostModel.IQNotifationSettings.MediaTypeList).Count() == 0)
                    {
                        objIQAgent_DailyDigestPostModel.IQNotifationSettings.MediaTypeList = null;
                    }
                }

                return Json(new
                {
                    isSuccess = true,
                    HTML = RenderPartialToString(PATH_SetupIQNotificationSettingsAddEditPartialView, objIQAgent_DailyDigestPostModel)
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
        public JsonResult SubmitIQNotification(IQNotifationSettingsPostModel p_IQNotifationSettingsPostModel)
        {
            try
            {
                ActiveUser sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                string ErrorMessage = string.Empty;
                string _Result = string.Empty;
                IQNotificationSettingsLogic iQNotificationSettingsLogic = (IQNotificationSettingsLogic)LogicFactory.GetLogic(LogicType.IQNotification);

                p_IQNotifationSettingsPostModel.IQNotifationSettings.Notification_Day = p_IQNotifationSettingsPostModel.IQNotifationSettings.Notification_Day == -1 ? null : p_IQNotifationSettingsPostModel.IQNotifationSettings.Notification_Day;
                p_IQNotifationSettingsPostModel.IQNotifationSettings.Notification_Time = p_IQNotifationSettingsPostModel.IQNotifationSettings.Notification_Time == "0" ? null : p_IQNotifationSettingsPostModel.IQNotifationSettings.Notification_Time;
                p_IQNotifationSettingsPostModel.IQNotifationSettings.Notification_Address = (p_IQNotifationSettingsPostModel.IQNotifationSettings.Notification_Address != null && p_IQNotifationSettingsPostModel.IQNotifationSettings.Notification_Address.Count > 0) ? p_IQNotifationSettingsPostModel.IQNotifationSettings.Notification_Address[0].Split(';').Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim()).ToList() : null;


                if (p_IQNotifationSettingsPostModel.IQNotifationSettings.Notification_Address.Count <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"]))
                {
                    if (p_IQNotifationSettingsPostModel.IQNotifationSettings.MediaTypeList == null || p_IQNotifationSettingsPostModel.IQNotifationSettings.MediaTypeList.Contains("0"))
                    {
                        p_IQNotifationSettingsPostModel.IQNotifationSettings.MediaTypeList = new List<string>();

                        foreach (var mt in IQCommon.CommonFunctions.GetAccessibleSubMediaType(sessionInformation.MediaTypes))
                        {
                            p_IQNotifationSettingsPostModel.IQNotifationSettings.MediaTypeList.Add(mt.SubMediaType);
                        }
                    }

                    if (p_IQNotifationSettingsPostModel.IQNotifationSettings.IQNotificationKey == 0)
                    {

                        _Result = iQNotificationSettingsLogic.InsertIQNotificationSettings(p_IQNotifationSettingsPostModel.IQNotifationSettings, sessionInformation.ClientGUID);

                        if (_Result == "-1" || _Result == "")
                        {
                            ErrorMessage = _IQNotificationExistMessage;
                        }
                        else if (string.IsNullOrEmpty(_Result) || Convert.ToInt32(_Result) <= 0)
                        {
                            ErrorMessage = Config.ConfigSettings.Settings.ErrorOccurred;
                        }

                    }
                    else
                    {

                        _Result = iQNotificationSettingsLogic.UdateIQNotificationSettings(p_IQNotifationSettingsPostModel.IQNotifationSettings, sessionInformation.ClientGUID);
                        if (_Result == "-1")
                        {
                            ErrorMessage = _IQNotificationExistMessage;
                        }
                        else if (string.IsNullOrEmpty(_Result) || Convert.ToInt32(_Result) <= 0)
                        {
                            ErrorMessage = Config.ConfigSettings.Settings.ErrorOccurred;
                        }
                    }
                }
                else
                {
                    ErrorMessage = Config.ConfigSettings.Settings.MaxEmailAdressLimitExceeds.Replace("@@MaxLimit@@", System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"]);
                }




                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    return Json(new
                    {
                        isSuccess = true,
                        appId = _Result
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        errorMsg = ErrorMessage
                    });
                }
            }
            catch (Exception ex)
            {
                Utility.CommonFunctions.WriteException(ex);

                return Json(new
                {
                    isSuccess = false,
                    errorMsg = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep();
            }
        }

        [HttpPost]
        public JsonResult DeleteIQNotification(Int32 p_ID)
        {
            try
            {
                ActiveUser sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                IQNotificationSettingsLogic iQNotificationSettingsLogic = (IQNotificationSettingsLogic)LogicFactory.GetLogic(LogicType.IQNotification);
                int output = iQNotificationSettingsLogic.DeleteIQNotification(p_ID, sessionInformation.ClientGUID);

                if (output > 0)
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
            finally
            {
                TempData.Keep();
            }
        }

        #region Utility
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

        #endregion
    }
}
