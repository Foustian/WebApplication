using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using System.IO;
using IQMedia.WebApplication.Config;
using System.Text;
using IQMedia.WebApplication.Models.TempData;
using IQMedia.Shared.Utility;

namespace IQMedia.WebApplication.Controllers
{
    public class JobStatusController : Controller
    {

        SetupTempData setupTempData = null;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult IsExistJobStatusFile(long p_ID)
        {

            try
            {
                JobStatusLogic JobStatusLogic = (JobStatusLogic)LogicFactory.GetLogic(LogicType.JobStatus);

                string DownloadLocation = JobStatusLogic.SelectJobStatusDownloadByID(p_ID);

                if (!string.IsNullOrEmpty(DownloadLocation))
                {
                    if (System.IO.File.Exists(DownloadLocation))
                    {
                        setupTempData = GetTempData();
                        setupTempData.DownloadLocation = DownloadLocation;
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
                            error = ConfigSettings.Settings.FileNotAvailableForDownload
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        error = ConfigSettings.Settings.ErrorOnDownloadFile
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

        [HttpGet]
        public ActionResult DownloadJobStatusFile(long p_ID)
        {
            try
            {
                setupTempData = GetTempData();
                string fileExtension = Path.GetExtension(setupTempData.DownloadLocation);
                string fileContentType = IQMedia.Shared.Utility.CommonFunctions.GetFileContentTypeByExtension(fileExtension);
                return File(setupTempData.DownloadLocation, fileContentType, Path.GetFileName(setupTempData.DownloadLocation));
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Content(IQMedia.WebApplication.Utility.CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep();
            }
        }

        public SetupTempData GetTempData()
        {
            SetupTempData setupTempData = (SetupTempData)TempData["SetupTempData"];
            return setupTempData;
        }
    }
}
