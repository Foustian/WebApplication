using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using IQMedia.Web.Logic;
using System.Text.RegularExpressions;
using IQMedia.Web.Logic.Base;
using System.Configuration;
using System.Security.Authentication;
using IQMedia.Shared.Utility;
using IQMedia.WebApplication.Models.TempData;
using IQMedia.WebApplication.Config;

namespace IQMedia.WebApplication.Controllers
{
    [NoCache]    
    public class AccountController : Controller
    {
        string PATH_AcntChangePwdView = "~/Views/Account/_ChangePwd.cshtml";
        string PATH_AcntRsetPwd2View = "~/Views/Account/ResetPwd2.cshtml";

        [CheckAuthentication()]
        [HttpGet]
        public ActionResult ChangePwd()
        {
            try
            {
                return View(PATH_AcntChangePwdView);
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

        [CheckAuthentication()]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePwd(string pwd_Current, string pwd_New, string pwd_Confirm)
        {
            var status = -3;
            var msg = "";
            var session = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

            try
            {
                if (string.IsNullOrWhiteSpace(pwd_Current) || string.IsNullOrWhiteSpace(pwd_New) || string.IsNullOrWhiteSpace(pwd_Confirm))
                {
                    throw new CustomException(ConfigSettings.Settings.ChPwdRequired);
                }
                else if (pwd_New.Trim() != pwd_Confirm.Trim())
                {
                    throw new CustomException(ConfigSettings.Settings.ChPwdCnfmPwdNotMtch);
                }
                else if (!Regex.IsMatch(pwd_New.Trim(), "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,30}$"))
                {
                    throw new CustomException(ConfigSettings.Settings.ChPwdCriteria);
                }
                else if(pwd_Current.Trim() == pwd_New.Trim())
                {
                    throw new CustomException(ConfigSettings.Settings.ChPwdSmOldPwd);
                }

                CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                bool isAuthenticated = customerLogic.CheckAuthentication(session.LoginID, pwd_Current, Convert.ToInt32(ConfigurationManager.AppSettings["MaxPasswordAttempts"]));

                if (isAuthenticated)
                {
                    customerLogic.UpdatePasswordByLoginID(session.LoginID, IQMedia.Security.Authentication.GetHashPassword(pwd_New.Trim()));

                    IQMedia.WebApplication.Utility.ActiveUserMgr.RemoveUserFromCacheBySessionID();
                    IQMedia.Web.Common.Authentication.Logout();

                    Session.Abandon();

                    status = 0;
                    msg = ConfigSettings.Settings.ChPwdSuccess;
                }
                else
                {
                    status = 1;
                    msg = ConfigSettings.Settings.ChPwdWrongPwd;
                }
            }
            catch (CustomException ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex,session.LoginID);
                status = -2;
                msg = ex.Message;                
            }
            catch (AuthenticationException ex)
            {
                IQMedia.WebApplication.Utility.ActiveUserMgr.RemoveUserFromCacheBySessionID();
                IQMedia.Web.Common.Authentication.Logout();

                Session.Abandon();

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex, session.LoginID);

                status = -1;
                msg = ex.Message;
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex, session.LoginID);

                status = -3;
                msg = IQMedia.WebApplication.Config.ConfigSettings.Settings.ErrorOccurred;
            }

            Dictionary<string, object> output = new Dictionary<string, object>();
            output["Status"] = status;
            output["Msg"] = msg;

            return View(PATH_AcntChangePwdView,output);
        }

        [HttpGet]
        public ActionResult ResetPwd(string ID)
        {
            if (!IQMedia.WebApplication.Utility.ActiveUserMgr.CheckAuthentication())
            {
                if (string.IsNullOrWhiteSpace(ID))
                {
                    if (TempData["RsetPwdTempData"] != null)
                    {
                        var tmpData = (RsetPwdTempData)TempData["RsetPwdTempData"];

                        if (tmpData.IsLinkExpired)
                        {
                            var dic = new Dictionary<string, object>();

                            dic["Status"] = -2;
                            dic["Msg"] = ConfigSettings.Settings.RsetPwdLinkExpired;

                            return View(dic);
                        }
                    }

                    return View();
                }
                else
                {
                    return View(PATH_AcntRsetPwd2View);
                } 
            }
            else
            {
                Response.Redirect("~/",true);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPwd(string ID,string p_LoginID,string p_Pwd,string p_CnfmPwd)
        {
            if (!IQMedia.WebApplication.Utility.ActiveUserMgr.CheckAuthentication())
            {
                if (string.IsNullOrWhiteSpace(ID))
                {
                    var output = RsetPwdStep1(p_LoginID);
                    return View(output);
                }
                else
                {
                    var output = RsetPwdStep2(ID, p_LoginID, p_Pwd, p_CnfmPwd);
                    return View(PATH_AcntRsetPwd2View, output);
                } 
            }
            else
            {
                Response.Redirect("~/", true);
                return View();
            }
        }

        private Dictionary<string, object> RsetPwdStep2(string ID, string p_LoginID, string p_Pwd, string p_CnfmPwd)
        { 
            var status = -3;
            var msg = "";

            try
            {
                if (string.IsNullOrWhiteSpace(p_LoginID) || string.IsNullOrWhiteSpace(p_Pwd) || string.IsNullOrWhiteSpace(p_CnfmPwd))
                {
                    throw new CustomException(ConfigSettings.Settings.RsetPwdRequired);
                }
                else if (!IQMedia.Shared.Utility.CommonFunctions.IsValidEmail(p_LoginID))
                {
                    throw new CustomException(ConfigSettings.Settings.RsetPwdInvalidLoginID);
                }
                else if (p_Pwd.Trim() != p_CnfmPwd.Trim())
                {
                    throw new CustomException(ConfigSettings.Settings.RsetPwdCnfmPwdNotMtch);
                }
                else if (!Regex.IsMatch(p_Pwd.Trim(), "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,30}$"))
                {
                    throw new CustomException(ConfigSettings.Settings.RsetPwdCriteria);
                }
                else if (string.IsNullOrWhiteSpace(Request.Params["g-recaptcha-response"]))
                {
                    throw new CustomException(ConfigSettings.Settings.RsetPwdCaptchaEmpty);
                }

                IQMedia.WebApplication.Utility.CommonFunctions.RecaptchaErrorCodes? captchaErr = null;

                if (IQMedia.WebApplication.Utility.CommonFunctions.ValidateCaptcha(Request.Params["g-recaptcha-response"], out captchaErr))
                {
                    var custLgc = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                    var rsetPwd = custLgc.GetRsetPwd(p_LoginID.Trim());

                    if (rsetPwd != null)
                    {
                        if (string.Compare(ID.Trim(), rsetPwd.Token, false) == 0)
                        {
                            if (DateTime.Now <= rsetPwd.DateExpired)
                            {
                                custLgc.UpdateRsetPwdNPassword(rsetPwd.ID, rsetPwd.CustomerGUID, p_Pwd.Trim());

                                status = 0;
                                msg = ConfigSettings.Settings.RsetPwdSuccess;
                            }
                            else
                            {
                                var tmpData = new RsetPwdTempData() { IsLinkExpired = true };
                                TempData["RsetPwdTempData"] = tmpData;
                                Response.Redirect("~/Account/ResetPwd");
                            }
                        }
                        else
                        {
                            throw new CustomException(ConfigSettings.Settings.RsetPwdInvalidDetail);
                        }
                    }
                    else
                    {
                        throw new CustomException(ConfigSettings.Settings.RsetPwdInvalidDetail);
                    } 
                }
                else
                {
                    Log4NetLogger.Error("Invalid Captcha: " + captchaErr.Value.ToString() + " LoginID: " + p_LoginID);
                    throw new CustomException(ConfigSettings.Settings.RsetPwdInvalidCaptcha);
                }
            }
            catch (CustomException ex)
            {
                status = -2;
                msg = ex.Message;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex, p_LoginID);
            }
            catch (Exception ex)
            {
                status = -3;
                msg = IQMedia.WebApplication.Config.ConfigSettings.Settings.RsetPwdError;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex, p_LoginID);
            }

            Dictionary<string, object> output = new Dictionary<string, object>();
            output["Status"] = status;
            output["Msg"] = msg;

            return output;
        }

        private Dictionary<string, object> RsetPwdStep1(string p_LoginID)
        {
            var status = -3;
            var msg = "";

            try
            {
                if (string.IsNullOrWhiteSpace(p_LoginID))
                {
                    throw new CustomException(ConfigSettings.Settings.RsetPwdRequired);
                }
                else if (!IQMedia.Shared.Utility.CommonFunctions.IsValidEmail(p_LoginID))
                {
                    throw new CustomException(ConfigSettings.Settings.RsetPwdInvalidLoginID);
                }
                else if (string.IsNullOrWhiteSpace(Request.Params["g-recaptcha-response"]))
                {
                    throw new CustomException(ConfigSettings.Settings.RsetPwdCaptchaEmpty);
                }

                IQMedia.WebApplication.Utility.CommonFunctions.RecaptchaErrorCodes? captchaErr = null;

                if (IQMedia.WebApplication.Utility.CommonFunctions.ValidateCaptcha(Request.Params["g-recaptcha-response"], out captchaErr))
                {
                    var loginID = p_LoginID.Trim();

                    var custLgc = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);

                    Int16 rsetPwdEmailCount = 0;
                    var email = "";

                    bool isValidCust = custLgc.ValidateLoginIDForRsetPwd(p_LoginID, out rsetPwdEmailCount, out email);

                    if (isValidCust)
                    {
                        if (rsetPwdEmailCount < Convert.ToInt16(ConfigurationManager.AppSettings["RsetPwdEmailCount"]))
                        {
                            var token = IQMedia.Shared.Utility.CommonFunctions.GenerateRandomString(20);
                            var link = ConfigurationManager.AppSettings["RsetPwdBaseUrl"] + token;
                            var body = "";
                            var policy = "";

                            using (StreamReader strmEmailPolicy = new StreamReader(Server.MapPath("~/content/EmailDisclaimer.txt")))
                            {
                                policy = strmEmailPolicy.ReadToEnd();
                            }

                            using (StreamReader strmRsetPwd = new StreamReader(Server.MapPath("~/content/RsetPwdTemplate.txt")))
                            {
                                body = strmRsetPwd.ReadToEnd();
                            }

                            body = body.Replace("##Link##",link);

                            body = body + policy;

                            var id = custLgc.InsertRsetPwd(p_LoginID.Trim(), Convert.ToInt16(ConfigurationManager.AppSettings["RsetPwdLinkTimeoutMinutes"]), token);

                            if (id > 0)
                            {
                                var emailSent = IQMedia.Shared.Utility.CommonFunctions.SendMail(email, "", null, ConfigurationManager.AppSettings["LoginEmail"], ConfigurationManager.AppSettings["RsetPwdEmailSubject"], body, true, null);

                                if (emailSent)
                                {
                                    custLgc.UpdateRsetPwdEmailCount(p_LoginID.Trim());

                                    status = 0;
                                    msg = ConfigSettings.Settings.RsetPwdEmailSent;
                                }
                                else
                                {
                                    status = 1;
                                    msg = ConfigSettings.Settings.RsetPwdEmailError;
                                }
                            }
                            else
                            {
                                throw new Exception("Unable to insert DB Rset Pwd record LoginID:" + loginID + " token: " + token);
                            }
                        }
                        else
                        {
                            throw new CustomException(ConfigSettings.Settings.RsetPwdEmailLimit);
                        }
                    }
                    else
                    {
                        throw new CustomException(ConfigSettings.Settings.RsetPwdInvalidLoginID);
                    } 
                }
                else
                {
                    Log4NetLogger.Error("Invalid Captcha: " + captchaErr.Value.ToString() + " LoginID: " + p_LoginID);
                    throw new CustomException(ConfigSettings.Settings.RsetPwdInvalidCaptcha);
                }
            }
            catch (CustomException ex)
            {
                status = -2;
                msg = ex.Message;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex, p_LoginID);
            }
            catch (Exception ex)
            {
                status = -3;
                msg = IQMedia.WebApplication.Config.ConfigSettings.Settings.RsetPwdError;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex, p_LoginID);
            }

            Dictionary<string, object> output = new Dictionary<string, object>();
            output["Status"] = status;
            output["Msg"] = msg;

            return output;
        }        

        private string RenderPartialToString(string viewName, object model = null)
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
