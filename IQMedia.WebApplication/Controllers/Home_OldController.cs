using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using IQMedia.Model;
using IQMedia.Web.Common;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.WebApplication.Utility;

namespace IQMedia.WebApplication.Controllers
{
    //[CheckAuthentication()]
    public class Home_OldController : Controller
    {

        Random randomNumber = new Random();



        public ActionResult Index()
        {
            /*if (Authentication.IsAuthenticated)
            {
                SessionInformation sessionInformation = (SessionInformation)Session["SessionInformation"];
                if (sessionInformation != null &&
                    sessionInformation.AuthorizedVersion.HasValue && (sessionInformation.AuthorizedVersion == 0 || sessionInformation.AuthorizedVersion == 4))
                {
                    return RedirectToAction("Index", "Feeds");
                }
            }*/
            try
            {
                ////
                IQMedia.Shared.Utility.Log4NetLogger.Debug("Start");

                bool IsValidSession = false;
                if (IQMedia.Web.Common.Authentication.CurrentUser != null)
                {
                    IsValidSession = IQMedia.WebApplication.Utility.CommonFunctions.IsUserInCacheBySessionID();
                }

                SessionInformation sessionInformation = CommonFunctions.GetSessionInformation();
                if (IsValidSession)
                {
                    if (sessionInformation != null && sessionInformation.IsLogIn)
                    {
                        if (!(sessionInformation.AuthorizedVersion == 0 || sessionInformation.AuthorizedVersion == 4))
                        {
                            IQMedia.WebApplication.Utility.CommonFunctions.RemoveUserFromCacheBySessionID();
                            IQMedia.Web.Common.Authentication.Logout();
                        }
                        else
                        {
                            IQMedia.WebApplication.Utility.CommonFunctions.UpdateUserIntoCache(sessionInformation.LoginID);
                        }
                    }
                }
                else if (Authentication.IsAuthenticated)
                {
                    var currentUser = IQMedia.Web.Common.Authentication.CurrentUser;
                    if (currentUser != null)
                    {
                        CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                        CustomerModel customerModel = customerLogic.GetClientGUIDByCustomerGUID(currentUser.Guid);
                        List<CustomerRoleModel> customerRoles = customerLogic.GetCustomerRoles(currentUser.Guid);
                        CommonFunctions.FillCustomerRoles(customerModel, customerRoles);

                        if (!(customerModel.AuthorizedVersion == 0 || customerModel.AuthorizedVersion == 4))
                        {
                            IQMedia.WebApplication.Utility.CommonFunctions.RemoveUserFromCacheBySessionID();
                            IQMedia.Web.Common.Authentication.Logout();
                        }
                        else
                        {
                            IQMedia.WebApplication.Utility.CommonFunctions.AddUserIntoCache(customerModel.LoginID, customerModel.MultiLogin.HasValue ? customerModel.MultiLogin.Value : false);
                            CommonFunctions.SetSessionInformation(customerModel);
                        }
                    }
                    else
                    {
                        IQMedia.WebApplication.Utility.CommonFunctions.RemoveUserFromCacheBySessionID();
                        IQMedia.Web.Common.Authentication.Logout();
                        CommonFunctions.SetSessionInformation(new SessionInformation());
                    }
                }
                else
                {
                    IQMedia.WebApplication.Utility.CommonFunctions.RemoveUserFromCacheBySessionID();
                    IQMedia.Web.Common.Authentication.Logout();
                    CommonFunctions.SetSessionInformation(new SessionInformation());
                }
                ViewBag.CurrentMenu = "spnHome";
                SetMainBanner();
            }
            catch (Exception exception)
            {
                ViewBag.IsSuccess = false;
                UtilityLogic.WriteException(exception);
            }
            return View();

        }


        public string SetMainBanner()
        {
            string mainBannerHTML = string.Empty;
            try
            {

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/images/home/Banner"), "*.png");

                if (filePaths.Count() > 0)
                {

                    Int32 finalNumber = randomNumber.Next(1, filePaths.Count() + 1);
                    StreamReader text = new StreamReader(Server.MapPath("~/images/home/Text/" + finalNumber + ".txt"));
                    mainBannerHTML = "<img  id=\"imgBanner\" src=' " + "../images/home/Banner/" + finalNumber + ".png" + "'>";
                    ViewBag.MainBanner = "../images/home/Banner/" + finalNumber + ".png";
                    ViewBag.SubBanner = "../images/home/SubBanner/" + finalNumber + ".png";
                    ViewBag.pText = text.ReadToEnd();
                    text.Close();
                }


                //Random randomNumber = new Random();

                //int randomNumber = Math.Round(new Random());
            }
            catch (Exception)
            {
                try
                {
                    StreamReader text = new StreamReader(Server.MapPath("~/images/home/Text/1.txt"));
                    ViewBag.MainBanner = "../images/home/Banner/1.png";
                    ViewBag.SubBanner = "../images/home/SubBanner/1.png";
                    ViewBag.pText = text.ReadToEnd();
                    text.Close();
                }
                catch (Exception)
                {


                }

            }
            return mainBannerHTML;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogIn(CustomerModel cust)
        {
            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult Create(CustomerModel cust)
        {
            return RedirectToAction("Index");
        }

        public ActionResult LogIn()
        {
            return View();
        }
    }
}
