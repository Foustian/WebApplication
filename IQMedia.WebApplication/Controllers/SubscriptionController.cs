using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Shared.Utility;
using IQMedia.Web.Logic.Base;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class SubscriptionController : Controller
    {
        //
        // GET: /Subscription/

        public ActionResult Index(string ID, string Action)
        {
            try
            {
                Action = Request.QueryString["Action"];
                if (ID != null && Action != null)
                {
                    ViewBag.IsSuccess = true;
                    ViewBag.Message = string.Empty;
                    if (Action.ToLower() == "s" || Action.ToLower() == "u")
                    {
                        byte[] Key = Convert.FromBase64String("pF6tvq4GexXSUaGFKXGqaFmiG2X6ihG3joVZ8RiSwpk=");
                        byte[] IV = Convert.FromBase64String("g3tqe+8V4H/JwMe0X69TGw==");
                        string encryptedText = ID;
                        string decryptedText = string.Empty;
                        try
                        {
                            decryptedText = CommonFunctions.DecryptStringFromBytes_Aes(encryptedText, Key, IV);
                        }
                        catch (Exception)
                        {
                            ViewBag.Message = IQMedia.WebApplication.Config.ConfigSettings.Settings.SubscriptionInvalidID;
                            ViewBag.IsSuccess = false;
                            return View();
                        }
                        if (!string.IsNullOrWhiteSpace(decryptedText))
                        {
                            string[] InputIDs = decryptedText.Split('&');

                            if (InputIDs.Length > 1)
                            {
                                Int64 searchRequestID;
                                Int64 hubSpotID;

                                if (Int64.TryParse(InputIDs[0], out searchRequestID) && Int64.TryParse(InputIDs[1], out hubSpotID))
                                {
                                    IQ_SMSCampaignLogic iQ_SMSCampaignLogic = (IQ_SMSCampaignLogic)LogicFactory.GetLogic(LogicType.IQ_SMSCampaign);

                                    bool isActivated = string.Compare(Action.ToLower(), "s", true) == 0 ? true : false;

                                    string Result = iQ_SMSCampaignLogic.UpdateIQ_SMSCampaignIsActive(searchRequestID, hubSpotID, isActivated);

                                    if (Convert.ToInt32(Result) > 0)
                                    {
                                        if (Action.ToLower() == "s")
                                        {
                                            //lblSuccessMessge.Text = "You have successfully subscribed to SMS text alerts.";
                                            ViewBag.IsSuccess =true;
                                            ViewBag.Message = IQMedia.WebApplication.Config.ConfigSettings.Settings.SubscriptionSuccess;
                                        }
                                        else
                                        {
                                            ViewBag.IsSuccess = true;
                                            ViewBag.Message = IQMedia.WebApplication.Config.ConfigSettings.Settings.UnSubscriptionSuccess;
                                        }
                                    }
                                    else
                                    {
                                        if (Action.ToLower() == "s")
                                        {
                                            ViewBag.Message = IQMedia.WebApplication.Config.ConfigSettings.Settings.SubscriptionFailed;
                                            ViewBag.IsSuccess = false;
                                        }
                                        else
                                        {
                                            ViewBag.Message = IQMedia.WebApplication.Config.ConfigSettings.Settings.UnSubscriptionFailed;
                                            ViewBag.IsSuccess = false;
                                        }
                                    }
                                }
                                else
                                {
                                    ViewBag.Message = IQMedia.WebApplication.Config.ConfigSettings.Settings.SubscriptionInvalidID;
                                    ViewBag.IsSuccess = false;
                                }
                            }
                            else
                            {
                                ViewBag.Message = IQMedia.WebApplication.Config.ConfigSettings.Settings.SubscriptionInvalidID;
                                ViewBag.IsSuccess = false;
                            }
                        }
                        else
                        {
                            ViewBag.Message = IQMedia.WebApplication.Config.ConfigSettings.Settings.SubscriptionInvalidID;
                            ViewBag.IsSuccess = false;
                        }
                    }
                    else
                    {
                        ViewBag.Message = IQMedia.WebApplication.Config.ConfigSettings.Settings.SubscriptionInvalidAction;
                        ViewBag.IsSuccess = false;
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid Input";
                    ViewBag.IsSuccess = false;
                }
                return View();
            }
            catch(Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                ViewBag.Message = "Some error occured, please try again.";
                ViewBag.IsSuccess = false;
                return View();
            }
        }

    }
}
