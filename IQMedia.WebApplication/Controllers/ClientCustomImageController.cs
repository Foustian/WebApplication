using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.Model;
using System.IO;
using System.Configuration;
using System.Web.UI;
using System.Drawing;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication]
    public class ClientCustomImageController : Controller
    {
        //
        // GET: /IQReport_Image/

        string PATH_SetupClientCustomImageListPartialView = "~/Views/Setup/_ClientCustomImageList.cshtml";
        private string _InvalidImageMessage = "Report image is not in correct format";
        public ActionResult Index()
        {
            return View();
        }

        ActiveUser sessionInformation;

        [HttpPost]
        public JsonResult DisplayReportImages()
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                IQClient_CustomImageLogic iQClient_CustomImageLogic = (IQClient_CustomImageLogic)LogicFactory.GetLogic(LogicType.IQClient_CustomImage);
                List<IQClient_CustomImageModel> lstIQClient_CustomImageModel = iQClient_CustomImageLogic.GetAllIQClient_CustomImageByClientGuid(sessionInformation.ClientGUID);

                return Json(new
                {
                    isSuccess = true,
                    HTML = RenderPartialToString(PATH_SetupClientCustomImageListPartialView, lstIQClient_CustomImageModel)
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
        public JsonResult DeleteReportImage(Int64 p_ID, string p_Image)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                IQClient_CustomImageLogic iQClient_CustomImageLogic = (IQClient_CustomImageLogic)LogicFactory.GetLogic(LogicType.IQClient_CustomImage);
                string result = iQClient_CustomImageLogic.DeleteIQClient_CustomImage(p_ID, sessionInformation.ClientGUID);
                if (!string.IsNullOrEmpty(result) && Convert.ToInt32(result) > 0)
                {
                    RemoveImage(p_Image);
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
                        error = Config.ConfigSettings.Settings.DeleteFailed
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
        public JsonResult InsertReportImage(HttpPostedFileBase flCustomHeader, bool? chkIsReplaceImage, bool? chkIsDefault, bool? chkIsDefaultEmail, int? hdnCropWidth, int? hdnCropHeight, int? hdnCropX, int? hdnCropY)
        {
            try
            {
                if (CheckForImage(flCustomHeader, hdnCropWidth, hdnCropHeight))
                {
                    sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                    chkIsReplaceImage = chkIsReplaceImage.HasValue ? chkIsReplaceImage : false;
                    if (!Directory.Exists(ConfigurationManager.AppSettings["DirReportHeader"] + sessionInformation.ClientGUID))
                    {
                        Directory.CreateDirectory(ConfigurationManager.AppSettings["DirReportHeader"] + sessionInformation.ClientGUID);
                    }

                    IQClient_CustomImageModel iQClient_CustomImageModel = new Model.IQClient_CustomImageModel();
                    iQClient_CustomImageModel.ClientGuid = sessionInformation.ClientGUID;
                    iQClient_CustomImageModel.Location = Path.GetFileName(flCustomHeader.FileName);
                    iQClient_CustomImageModel.IsDefault = chkIsDefault.HasValue ? chkIsDefault.Value : false;
                    iQClient_CustomImageModel.IsDefaultEmail = chkIsDefaultEmail.HasValue ? chkIsDefaultEmail.Value : false;

                    IQClient_CustomImageLogic iQClient_CustomImageLogic = (IQClient_CustomImageLogic)LogicFactory.GetLogic(LogicType.IQClient_CustomImage);
                    if (!chkIsReplaceImage.Value)
                    {
                        int? MaxCopyCount = iQClient_CustomImageLogic.CheckForImageCopy(iQClient_CustomImageModel.Location, sessionInformation.ClientGUID);
                        if (MaxCopyCount.HasValue)
                        {
                            iQClient_CustomImageModel.Location = Path.GetFileNameWithoutExtension(iQClient_CustomImageModel.Location) + "_" + (MaxCopyCount + 1).ToString() + Path.GetExtension(iQClient_CustomImageModel.Location);
                        }
                    }

                    if (hdnCropWidth != null && hdnCropHeight != null && hdnCropX != null && hdnCropY != null)
                    {
                        using (var input = new System.Drawing.Bitmap(flCustomHeader.InputStream))
                        {
                            System.Drawing.Rectangle CropArea = new System.Drawing.Rectangle(
                                    hdnCropX.Value,
                                    hdnCropY.Value,
                                    hdnCropWidth.Value,
                                    hdnCropHeight.Value);
                            System.Drawing.Bitmap bitMap = new System.Drawing.Bitmap(CropArea.Width, CropArea.Height);
                            using (System.Drawing.Graphics g = Graphics.FromImage(bitMap))
                            {
                                g.DrawImage(input, new Rectangle(0, 0, bitMap.Width, bitMap.Height), CropArea, GraphicsUnit.Pixel);
                            } 
                            bitMap.Save(ConfigurationManager.AppSettings["DirReportHeader"] + sessionInformation.ClientGUID + @"\" + iQClient_CustomImageModel.Location);
                        }
                    }
                    else
                    {
                        flCustomHeader.SaveAs(ConfigurationManager.AppSettings["DirReportHeader"] + sessionInformation.ClientGUID + @"\" + iQClient_CustomImageModel.Location);
                    }

                    string result = iQClient_CustomImageLogic.InsertIQClient_CustomImage(iQClient_CustomImageModel, chkIsReplaceImage.Value);
                    if (result == "0")
                    {
                        RemoveImage(iQClient_CustomImageModel.Location);
                        return Json(new
                        {
                            isSuccess = false,
                            errorMsg = Config.ConfigSettings.Settings.ErrorOccurred
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            isSuccess = true,
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        errorMsg = _InvalidImageMessage
                    });
                }
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                Shared.Utility.Log4NetLogger.Fatal("Report Image Insert Error : ", ex);
                return Json(new
                {
                    isSuccess = false,
                    errorMsg = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult UpdateIsDefault(Int64 p_ID)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                IQClient_CustomImageLogic iQClient_CustomImageLogic = (IQClient_CustomImageLogic)LogicFactory.GetLogic(LogicType.IQClient_CustomImage);
                string result = iQClient_CustomImageLogic.UpdateIsDefaultIQClient_CustomImage(p_ID, sessionInformation.ClientGUID);
                if (!string.IsNullOrEmpty(result) && Convert.ToInt32(result) > 0)
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
                        error = Config.ConfigSettings.Settings.SetDefaultFailed
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
        public JsonResult UpdateIsDefaultEmail(Int64 p_ID)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                IQClient_CustomImageLogic iQClient_CustomImageLogic = (IQClient_CustomImageLogic)LogicFactory.GetLogic(LogicType.IQClient_CustomImage);
                string result = iQClient_CustomImageLogic.UpdateIsDefaultEmailIQClient_CustomImage(p_ID, sessionInformation.ClientGUID);
                if (!string.IsNullOrEmpty(result) && Convert.ToInt32(result) > 0)
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
                        error = Config.ConfigSettings.Settings.SetDefaultEmailFailed
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

        #region Common Functions

        private bool CheckForImage(HttpPostedFileBase flImage, int? hdnCropWidth, int? hdnCropHeight)
        {
            string[] Extentions = ConfigurationManager.AppSettings["ClientReportImageExtensions"].Split(new char[] { ',' });

            if (flImage != null && flImage.ContentLength > 0)
            {
                if (Extentions.Contains(System.IO.Path.GetExtension(flImage.FileName).ToLower().Substring(1)))
                {
                    int height = 0;
                    int width = 0;
                    if (hdnCropWidth.HasValue && hdnCropHeight.HasValue)
                    {
                        height = hdnCropHeight.Value;
                        width = hdnCropWidth.Value;
                    }
                    else
                    {
                        System.Drawing.Bitmap imageHeader = new System.Drawing.Bitmap(flImage.InputStream);
                        height = imageHeader.Height;
                        width = imageHeader.Width;
                    }



                    if (height > Convert.ToInt16(ConfigurationManager.AppSettings["ReportHeaderHeight"])
                        || width > Convert.ToInt16(ConfigurationManager.AppSettings["ReportHeaderWidth"]))
                    {
                        return false;
                    }
                }
                else
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

        private void RemoveImage(string flCustomHeader)
        {
            sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
            if (!String.IsNullOrEmpty(Convert.ToString(flCustomHeader)))
            {
                if (System.IO.File.Exists(ConfigurationManager.AppSettings["DirReportHeader"] + sessionInformation.ClientGUID + @"\" + Convert.ToString(flCustomHeader)))
                {
                    System.IO.File.Delete(ConfigurationManager.AppSettings["DirReportHeader"] + sessionInformation.ClientGUID + @"\" + Convert.ToString(flCustomHeader));
                }
            }
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
