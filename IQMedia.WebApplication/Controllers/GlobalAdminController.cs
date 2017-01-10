using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using IQMedia.Model;
using IQMedia.Shared.Utility;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.WebApplication.Models;
using IQMedia.WebApplication.Utility;


namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication]
    public class GlobalAdminController : Controller
    {
        //
        // GET: /GlobalAdmin/

        string PATH_GlobalAdminPartialView = "~/Views/GlobalAdmin/_Results.cshtml";
        string PATH__GloblaAdminClientListPartialView = "~/Views/GlobalAdmin/_ClientList.cshtml";
        string PATH_GloblaAdminCustomerListPartialView = "~/Views/GlobalAdmin/_CustomerList.cshtml";
        string PATH_GloblaAdminClientRegistationPartialView = "~/Views/GlobalAdmin/_ClientRegistation.cshtml";
        string PATH_GloblaAdminCustomerRegistationPartialView = "~/Views/GlobalAdmin/_CustomerRegistation.cshtml";
        string PATH_GloblaAdminUGCSetupPartialView = "~/Views/GlobalAdmin/_UGCSetup.cshtml";
        string PATH_GloblaAdminClientUGC_MapListPartialView = "~/Views/GlobalAdmin/_ClientUGC_MapList.cshtml";
        string PATH_GloblaAdminGroupPartialView = "~/Views/GlobalAdmin/_Group.cshtml";
        private string _ClientExistMessage = "Client Already Exists.";
        private string _CustomerExistMessage = "Customer is already exists.";
        private string _CustomerCanNotFliq = "Customer can not be Fliq Customer, as Selected Client is not Fliq Client";
        private string _ImageExistMessage = "Image already exists with another user, Please upload image with another name.";
        private string _InvalidImageMessage = "Custom Header/Player Logo image is not in correct format";
        
        GlobalAdminTempData globalAdminTempData = null;
             
        #region Active Users

        #region Action
        public ActionResult Index()
        {
            try
            {
                // Clear out temp data used by other pages
                if (TempData.ContainsKey("AnalyticsTempData")) { TempData["AnalyticsTempData"] = null; }
                if (TempData.ContainsKey("FeedsTempData")) { TempData["FeedsTempData"] = null; }
                if (TempData.ContainsKey("DiscoveryTempData")) { TempData["DiscoveryTempData"] = null; }
                if (TempData.ContainsKey("TimeShiftTempData")) { TempData["TimeShiftTempData"] = null; }
                if (TempData.ContainsKey("TAdsTempData")) { TempData["TAdsTempData"] = null; }
                if (TempData.ContainsKey("SetupTempData")) { TempData["SetupTempData"] = null; }

                globalAdminTempData = new GlobalAdminTempData();
                globalAdminTempData.ClientPageNumber = 0;
                globalAdminTempData.ClientHasMoreRecords = true;

                globalAdminTempData.CustomerPageNumber = 0;
                globalAdminTempData.CustomerHasMoreRecords = true;

                globalAdminTempData.fliq_CustomerPageNumber = 0;
                globalAdminTempData.fliq_CustomerHasMoreRecords = true;

                globalAdminTempData.ClientUGCMapPageNumber = 0;
                globalAdminTempData.ClientUGCMapHasMoreRecords = true;

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                Client_DropDown objClient_DropDown = clientLogic.GetAllClientDropDown();

                globalAdminTempData.Client_DropDowns = objClient_DropDown;

                

                SetTempData(globalAdminTempData);

                ViewBag.IsSuccess = true;
                return View(ListActiveUsers(null,null,false));
            }
            catch (Exception exception)
            {
                ViewBag.IsSuccess = false;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return View();
            }
            finally
            {
                TempData.Keep("GlobalAdminTempData");
            }


        }

        public JsonResult GetActiveUsersFromCache(string p_SearchTerm, string p_SortColumn, bool p_IsAsc)
        {
            try
            {
                IEnumerable<ActiveUser> activeUSers = ListActiveUsers(p_SearchTerm, p_SortColumn, p_IsAsc);
                return Json(new
                {
                    HTML = RenderPartialToString(PATH_GlobalAdminPartialView, activeUSers),
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
            finally
            {
                TempData.Keep("GlobalAdminTempData");
            }
        }

        private IEnumerable<ActiveUser> ListActiveUsers(string p_SearchTerm,string p_SortColumn,bool p_IsAsc)
        {
            List<ActiveUser> activeUsers = ActiveUserMgr.GetAllActiveUsers();
            IEnumerable<ActiveUser> ienumUsers = null;

            p_SortColumn = !string.IsNullOrEmpty(p_SortColumn) ? p_SortColumn : "LastAccessTime";
            var pi = typeof(ActiveUser).GetProperty(p_SortColumn);  

            if (p_IsAsc == true)
            {
               ienumUsers = activeUsers.Where(item => string.IsNullOrEmpty(p_SearchTerm)
                        || (item.FirstName + " " + item.LastName).ToLower().Contains(p_SearchTerm.ToLower())
                        || item.LoginID.ToLower().Contains(p_SearchTerm.ToLower()))
                    .OrderBy(x => pi.GetValue(x, null));
            }
            else
            {
                ienumUsers = activeUsers.Where(item => string.IsNullOrEmpty(p_SearchTerm)
                         || (item.FirstName + " " + item.LastName).ToLower().Contains(p_SearchTerm.ToLower())
                         || item.LoginID.ToLower().Contains(p_SearchTerm.ToLower()))
                     .OrderByDescending(x => pi.GetValue(x, null));
            }           

            return ienumUsers;
        }

        [HttpPost]
        public JsonResult RemoveUserFromCache(string p_EmailAddress, string p_SessionID)
        {

            try
            {
                IQMedia.WebApplication.Utility.ActiveUserMgr.RemoveUserFromCacheBySessionID(p_SessionID);                
                
                return Json(new
                 {
                     globalAdminHTML = RenderPartialToString(PATH_GlobalAdminPartialView, ListActiveUsers("","",true)),
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
            finally
            {
                TempData.Keep("GlobalAdminTempData");
            }
        }

        [HttpPost]
        public JsonResult RemoveAllUsers()
        {
            try
            {
                IQMedia.WebApplication.Utility.ActiveUserMgr.RemoveAllUsers();

                return Json(new
                {                    
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
            finally
            {
                
            }
        }
        #endregion

        #region Methods

        public List<CustomerModel> GetActiveUser(List<SessionModel> lstSessionModel)
        {
            List<String> lstUser = lstSessionModel.Select(s => s.LoginID).Distinct().ToList();
            XDocument xdoc = new XDocument(new XElement("EmailIDs", lstUser.ToList().Select(s => new XElement("EmailID", s))));
            CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
            List<CustomerModel> lstCustomerModel = customerLogic.GetCustomerDetailByEmailList(xdoc);
            return lstCustomerModel;
        }

        #endregion

        #endregion

        #region Client 

        [HttpPost]
        public JsonResult DisplayClients(bool? p_IsNext = null, string p_ClientName = null)
        {
            try
            {

                globalAdminTempData = GetTempData();
                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                if (p_IsNext != null)
                {
                    if (p_IsNext == true)
                    {
                        if (globalAdminTempData.ClientHasMoreRecords == true)
                        {
                            globalAdminTempData.ClientPageNumber = globalAdminTempData.ClientPageNumber + 1;
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
                        if (globalAdminTempData.ClientPageNumber > 0)
                        {
                            globalAdminTempData.ClientPageNumber = globalAdminTempData.ClientPageNumber - 1;
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
                    globalAdminTempData.ClientHasMoreRecords = true;
                    globalAdminTempData.ClientPageNumber = 0;

                    if(string.IsNullOrEmpty(p_ClientName))
                    {
                        Client_DropDown objClient_DropDown = clientLogic.GetAllClientDropDown();

                        globalAdminTempData.Client_DropDowns = objClient_DropDown;
                    }
                }

                int totalResults = 0;
                int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ClientPageSize"]);
                List<ClientModel> lstClientModel = clientLogic.GetAllClientWithRole(p_ClientName,globalAdminTempData.ClientPageNumber, pageSize, out totalResults);

                if (totalResults > ((globalAdminTempData.ClientPageNumber + 1) * pageSize))
                {
                    globalAdminTempData.ClientHasMoreRecords = true;
                }
                else
                {
                    globalAdminTempData.ClientHasMoreRecords = false;
                }

                string strHTML = RenderPartialToString(PATH__GloblaAdminClientListPartialView, lstClientModel);

                string strRecordLabel = " ";
                if (lstClientModel.Count > 0)
                {
                    strRecordLabel = "" + ((globalAdminTempData.ClientPageNumber * pageSize) + 1).ToString() + " - " + ((globalAdminTempData.ClientPageNumber * pageSize) + lstClientModel.Count).ToString() + " Of " + totalResults.ToString() + "";
                }

                SetTempData(globalAdminTempData);

                return Json(new
                {
                    isSuccess = true,
                    hasMoreRecords = globalAdminTempData.ClientHasMoreRecords,
                    hasPreviousRecords = globalAdminTempData.ClientPageNumber > 0 ? true : false,
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
                TempData.Keep("GlobalAdminTempData");
            }
        }

        [HttpPost]
        public JsonResult GetClientsList()
        {
            try
            {
                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                List<ClientModel> _ListOfClient = new List<ClientModel>();
                _ListOfClient = clientLogic.SelectAllClient();
                return Json(new
                {
                    isSuccess = true,
                    clientList = _ListOfClient.Select(a=>a.ClientName).ToArray()
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
                TempData.Keep("GlobalAdminTempData");
            }
        }

        [HttpPost]
        public JsonResult GetClientRegistation(int p_ClientKey)
        {
            try
            {
                globalAdminTempData = GetTempData();
                if (p_ClientKey == 0)
                {
                    ClientPostModel client = new ClientPostModel();
                    client.Client_DropDown = globalAdminTempData.Client_DropDowns;
                    return Json(new
                    {
                        isSuccess = true,
                        masterClient = globalAdminTempData.Client_DropDowns.Client_MasterClientList.ToArray(),
                        HTML = RenderPartialToString(PATH_GloblaAdminClientRegistationPartialView, client)
                    });
                }
                else
                {
                    //adds 'All' category to temp data                   
                    IQ_Industry allIndustry = new IQ_Industry();
                    allIndustry.ID = "0";
                    allIndustry.Name = "All";
                    globalAdminTempData.Client_DropDowns.Client_LRIndustryList.Insert(0, allIndustry);

                    ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                    ClientPostModel objClientModel = clientLogic.GetClientPostModel(clientLogic.GetClientWithRoleByClientID(p_ClientKey));
                    objClientModel.Client_DropDown = clientLogic.GetClientDropDownByClient(p_ClientKey, globalAdminTempData.Client_DropDowns);
                   
                    return Json(new
                    {
                        isSuccess = true,
                        masterClient = globalAdminTempData.Client_DropDowns.Client_MasterClientList.ToArray(),
                        HTML = RenderPartialToString(PATH_GloblaAdminClientRegistationPartialView, objClientModel),
                    });
                }
            }
            catch (Exception exception)
            {
                Log4NetLogger.Error(exception);
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("GlobalAdminTempData");
            }
        }

        [HttpPost]
        public JsonResult ClientRegistration(ClientPostModel p_Client, HttpPostedFileBase flPlayerLogo)
        {
            ViewBag.ErrorMessage = string.Empty;
            string _Result = string.Empty;
           
            try
            {
                if (CheckForImage(flPlayerLogo))
                {
                    if (((p_Client.txtCompeteMultiplier == null || p_Client.txtOnlineNewsAdRate == null || p_Client.txtOtherOnlineAdRate == null || p_Client.txtURLPercentRead == null)))
                    {
                        ViewBag.ErrorMessage = "Please Enter Compete Values";
                    }
                    else
                    {
                        VisibleLRIndustries listOfVisualLRIndustries = new VisibleLRIndustries();
                        if(p_Client.selectedVisibleLRIndustries!=null){

                            globalAdminTempData = GetTempData();
                            listOfVisualLRIndustries.Industries = new List<IQ_Industry>();
                            if (p_Client.selectedVisibleLRIndustries[0] == "0")
                            {
                                IQ_Industry industry = new IQ_Industry();
                                industry.ID = "0";
                                listOfVisualLRIndustries.Industries.Add(industry);
                            }
                            else
                            {
                                foreach (string selectedIndustryID in p_Client.selectedVisibleLRIndustries)
                                {
                                    IQ_Industry industry = new IQ_Industry();
                                    //matches industry Name with industry ID and converts the list of strings into a list of industries to be matched with the Client Model
                                    string industryName = globalAdminTempData.Client_DropDowns.Client_LRIndustryList.First(w => w.ID == selectedIndustryID).Name;
                                    industry.Name = industryName;
                                    industry.ID = selectedIndustryID;
                                    listOfVisualLRIndustries.Industries.Add(industry);
                                }
                            }
                        }

                        if (p_Client.hdnClientKey == 0)
                        {
                            if (p_Client.txtNoOfUsers > 0)
                            {
                                bool _Checked = p_Client.chkRoles.Count() > 0 ? true : false;
                                
                                if (_Checked == true)
                                {
                                    string hfPlayerLogoImage;

                                    ClientModel _Client = new ClientModel();
                                    _Client.ClientName = p_Client.txtClientName.Trim();
                                    _Client.ClientGuid = System.Guid.NewGuid();


                                    CreateImage(p_Client.txtClientName, flPlayerLogo, out hfPlayerLogoImage, _Client.ClientGuid);

                                    string _DefaultCategory = ConfigurationManager.AppSettings["DefaultCustomCategory"];
                                    _Client.DefaultCategory = _DefaultCategory;
                                    //Cluster _Cluster = new Cluster();
                                    _Client.PricingCodeID = p_Client.ddlPricingCode;
                                    _Client.BillFrequencyID = p_Client.ddlBillFrequency;
                                    _Client.BillTypeID = p_Client.ddlBillType;
                                    _Client.IndustryID = p_Client.ddlIndustry;
                                    _Client.StateID = p_Client.ddlState;
                                    _Client.Address1 = p_Client.txtAddress1.Trim();
                                    _Client.Address2 =  !string.IsNullOrEmpty(p_Client.txtAddress2) ? p_Client.txtAddress2.Trim() : string.Empty;
                                    _Client.City = p_Client.txtCity.Trim();
                                    _Client.Zip = p_Client.txtZip.Trim();
                                    _Client.Attention = p_Client.txtAttention.Trim();
                                    _Client.Phone = p_Client.txtPhone.Trim();
                                    _Client.MCID = p_Client.ddlMCID;
                                    _Client.MasterClient = !string.IsNullOrEmpty(p_Client.txtMasterClient) ? p_Client.txtMasterClient.Trim() : string.Empty;
                                    _Client.IsActive = p_Client.chkIsActive.HasValue ? p_Client.chkIsActive.Value : false;

                                    _Client.IQLicense = new List<Int16> {0, 1};
                                    if (p_Client.chkHasPremium.HasValue && p_Client.chkHasPremium == true)
                                    {
                                        _Client.IQLicense.Add(2);
                                        _Client.IQLicense.Add(3);
                                    }
                                    
                                    //_Client.MasterClient = "test1";
                                    _Client.NoOfUser = p_Client.txtNoOfUsers;

                                    _Client.NoOfIQNotification = p_Client.txtNoOfNotification;

                                    _Client.NoOfIQAgent = p_Client.txtNoOfIQAgent;
                                    _Client.CompeteMultiplier = p_Client.txtCompeteMultiplier.Value;
                                    _Client.OnlineNewsAdRate = p_Client.txtOnlineNewsAdRate.Value;
                                    _Client.OtherOnlineAdRate = p_Client.txtOtherOnlineAdRate.Value;
                                    _Client.URLPercentRead = p_Client.txtURLPercentRead.Value;
                                    _Client.IsActivePlayerLogo = p_Client.chkIsPlayerLogo;
                                    _Client.TimeZone = p_Client.ddlTimeZone;
                                    _Client.IsCDNUpload = p_Client.chkIsCDNUpload;
                                    _Client.Multiplier = p_Client.txtMultiplier;
                                    _Client.CompeteAudienceMultiplier = p_Client.txtCompeteAudienceMultiplier;
                                    _Client.visibleLRIndustries = listOfVisualLRIndustries;
                                    _Client.v4MaxDiscoveryReportItems = p_Client.txtv4MaxDiscoveryReportItems;
                                    _Client.v4MaxDiscoveryExportItems = p_Client.txtv4MaxDiscoveryExportItems;
                                    _Client.v4MaxDiscoveryHistory = p_Client.txtv4MaxDiscoveryHistory;
                                    _Client.v4MaxFeedsExportItems = p_Client.txtv4MaxFeedsExportItems;
                                    _Client.v4MaxFeedsReportItems= p_Client.txtv4MaxFeedsReportItems;
                                    _Client.v4MaxLibraryEmailReportItems = p_Client.txtv4MaxLibraryEmailReportItems;
                                    _Client.v4MaxLibraryReportItems = p_Client.txtv4MaxLibraryReportItems;
                                    _Client.UseProminence = p_Client.chkUseProminence.HasValue ? p_Client.chkUseProminence.Value : false;
                                    _Client.UseProminenceMediaValue = p_Client.chkUseProminenceMediaValue.HasValue ? p_Client.chkUseProminenceMediaValue.Value : true; //If new client, default to true
                                    _Client.ForceCategorySelection = p_Client.chkForceCategorySelection.HasValue ? p_Client.chkForceCategorySelection.Value : false;
                                    _Client.MCMediaPublishedTemplateID = p_Client.ddlMCMediaPubTemplate;
                                    _Client.MCMediaDefaultEmailTemplateID = p_Client.ddlMCMediaEmailTemplate;
                                    _Client.IQRawMediaExpiration = p_Client.txtIQRawMediaExpiration;
                                    _Client.LibraryTextType = p_Client.ddlLibraryTextType;
                                    _Client.DefaultFeedsPageSize = p_Client.ddlDefaultFeedsPageSize;
                                    _Client.DefaultDiscoveryPageSize = p_Client.ddlDefaultDiscoveryPageSize;
                                    _Client.DefaultArchivePageSize = p_Client.ddlDefaultArchivePageSize;
                                    _Client.ClipEmbedAutoPlay = p_Client.chkClipEmbedAutoPlay;
                                    _Client.DefaultFeedsShowUnread = p_Client.chkDefaultFeedsShowUnread;
                                    _Client.UseCustomerEmailDefault = p_Client.chkUseCustomerEmailDefault;
                                    _Client.TVHighThreshold = p_Client.txtTVHighThreshold;
                                    _Client.TVLowThreshold = p_Client.txtTVLowThreshold;
                                    _Client.NMHighThreshold = p_Client.txtNMHighThreshold;
                                    _Client.NMLowThreshold = p_Client.txtNMLowThreshold;
                                    _Client.SMHighThreshold = p_Client.txtSMHighThreshold;
                                    _Client.SMLowThreshold = p_Client.txtSMLowThreshold;
                                    _Client.TwitterHighThreshold = p_Client.txtTwitterHighThreshold;
                                    _Client.TwitterLowThreshold = p_Client.txtTwitterLowThreshold;
                                    _Client.PQHighThreshold = p_Client.txtPQHighThreshold;
                                    _Client.PQLowThreshold = p_Client.txtPQLowThreshold;
                                    _Client.IsFliq = p_Client.chkIsFliq.HasValue ? p_Client.chkIsFliq.Value : false;                                    
                                    
                                    XDocument xdoc = new XDocument(new XElement("Roles", from i in p_Client.chkRoles select new XElement("Role", i)));

                                    if (!Directory.Exists(ConfigurationManager.AppSettings["DirReportHeader"] + _Client.ClientGuid))
                                    {
                                        Directory.CreateDirectory(ConfigurationManager.AppSettings["DirReportHeader"] + _Client.ClientGuid);
                                    }
                                    string strFilePath = Server.MapPath("~/" + ConfigurationManager.AppSettings["ClientDefaultHeaderImage"]);
                                    if(System.IO.File.Exists(strFilePath))
                                    {
                                        _Client.CustomHeaderImage = Path.GetFileName(strFilePath);
                                        System.IO.File.Copy(strFilePath, ConfigurationManager.AppSettings["DirReportHeader"] + _Client.ClientGuid + @"\" + _Client.CustomHeaderImage, true);
                                    }
                                    

                                    if (flPlayerLogo != null)
                                    {
                                        _Client.PlayerLogo = hfPlayerLogoImage;
                                    }

                                    
                                    if (!Directory.Exists(ConfigurationManager.AppSettings["DirReportHeader"] + _Client.ClientGuid))
                                    {
                                        Directory.CreateDirectory(ConfigurationManager.AppSettings["DirReportHeader"] + _Client.ClientGuid);
                                    }
                                    strFilePath = Server.MapPath("~/" + ConfigurationManager.AppSettings["ClientDefaultEmailHeaderImage"]);
                                    if (System.IO.File.Exists(strFilePath))
                                    {
                                        _Client.NotificationHeaderImage = Path.GetFileName(strFilePath);
                                        if (_Client.NotificationHeaderImage != _Client.CustomHeaderImage)
                                        {
                                            System.IO.File.Copy(strFilePath, ConfigurationManager.AppSettings["DirReportHeader"] + _Client.ClientGuid + @"\" + _Client.NotificationHeaderImage, true);
                                        }
                                    }
                                    

                                    int Status = 0;
                                    ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                                    _Result = clientLogic.InsertClient(_Client, xdoc.ToString(), out Status, ConfigurationManager.AppSettings["ClientRootFolderName"]);

                                    if (_Result == "0")
                                    {
                                        RemoveImage(_Client.CustomHeaderImage, _Client.PlayerLogo, _Client.NotificationHeaderImage, _Client.ClientGuid);
                                        ViewBag.ErrorMessage = _ClientExistMessage;
                                        
                                    }
                                    else if (Status == 1)
                                    {
                                        RemoveImage(_Client.CustomHeaderImage, _Client.PlayerLogo, _Client.NotificationHeaderImage, _Client.ClientGuid);
                                        ViewBag.ErrorMessage  = _ImageExistMessage;
                                    }
                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "Please Select Atleast One Role";
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "No of Users can not be less then 1";
                            }
                        }
                        else
                        {
                            if (p_Client.txtNoOfUsers > 0)
                            {
                                bool _Checked = p_Client.chkRoles.Count() > 0 ? true : false;
                                
                                if (_Checked == true)
                                {
                                    string  hfPlayerLogoImage;

                                    ClientModel _Client = new ClientModel();
                                    _Client.ClientName = p_Client.txtClientName.Trim();
                                    _Client.ClientKey = p_Client.hdnClientKey;

                                    CreateImage(p_Client.txtClientName, flPlayerLogo,  out hfPlayerLogoImage, _Client.ClientGuid);

                                    _Client.PricingCodeID = p_Client.ddlPricingCode;
                                    _Client.BillFrequencyID = p_Client.ddlBillFrequency;
                                    _Client.BillTypeID = p_Client.ddlBillType;
                                    _Client.IndustryID = p_Client.ddlIndustry;
                                    _Client.StateID = p_Client.ddlState;
                                    _Client.Address1 = p_Client.txtAddress1.Trim();
                                    _Client.Address2 = !string.IsNullOrEmpty(p_Client.txtAddress2) ? p_Client.txtAddress2.Trim() : string.Empty;
                                    _Client.City = p_Client.txtCity.Trim();
                                    _Client.Zip = p_Client.txtZip.Trim();
                                    _Client.Attention = p_Client.txtAttention.Trim();
                                    _Client.Phone = p_Client.txtPhone.Trim();
                                    _Client.MCID = p_Client.ddlMCID;
                                    _Client.MasterClient = !string.IsNullOrEmpty(p_Client.txtMasterClient) ? p_Client.txtMasterClient.Trim() : string.Empty;
                                    _Client.IsActive = p_Client.chkIsActive.HasValue ? p_Client.chkIsActive.Value : false;
                                    _Client.IsFliq = p_Client.chkIsFliq.HasValue ? p_Client.chkIsFliq.Value : false;

                                    _Client.IQLicense = new List<Int16> {0, 1};
                                    if (p_Client.chkHasPremium.HasValue && p_Client.chkHasPremium == true)
                                    {
                                        _Client.IQLicense.Add(2);
                                        _Client.IQLicense.Add(3);
                                    }

                                    //_Client.MasterClient = "test1";
                                    _Client.NoOfUser = p_Client.txtNoOfUsers;
                                    _Client.NoOfIQNotification = p_Client.txtNoOfNotification;
                                    _Client.NoOfIQAgent = p_Client.txtNoOfIQAgent;
                                    _Client.CompeteMultiplier = p_Client.txtCompeteMultiplier.Value;
                                    _Client.OnlineNewsAdRate = p_Client.txtOnlineNewsAdRate.Value;
                                    _Client.OtherOnlineAdRate = p_Client.txtOtherOnlineAdRate.Value;
                                    _Client.URLPercentRead = p_Client.txtURLPercentRead.Value;
                                    _Client.IsActivePlayerLogo = p_Client.chkIsPlayerLogo;
                                    _Client.TimeZone = p_Client.ddlTimeZone;
                                    _Client.IsCDNUpload = p_Client.chkIsCDNUpload;
                                    _Client.Multiplier = p_Client.txtMultiplier;
                                    _Client.CompeteAudienceMultiplier = p_Client.txtCompeteAudienceMultiplier;
                                    _Client.visibleLRIndustries = listOfVisualLRIndustries;
                                    _Client.v4MaxDiscoveryReportItems = p_Client.txtv4MaxDiscoveryReportItems;
                                    _Client.v4MaxDiscoveryExportItems = p_Client.txtv4MaxDiscoveryExportItems;
                                    _Client.v4MaxDiscoveryHistory = p_Client.txtv4MaxDiscoveryHistory;
                                    _Client.v4MaxFeedsExportItems = p_Client.txtv4MaxFeedsExportItems;
                                    _Client.v4MaxFeedsReportItems = p_Client.txtv4MaxFeedsReportItems;
                                    _Client.v4MaxLibraryEmailReportItems = p_Client.txtv4MaxLibraryEmailReportItems;
                                    _Client.v4MaxLibraryReportItems = p_Client.txtv4MaxLibraryReportItems;
                                    _Client.UseProminence = p_Client.chkUseProminence.HasValue ? p_Client.chkUseProminence.Value : false;
                                    _Client.UseProminenceMediaValue = p_Client.chkUseProminenceMediaValue.HasValue ? p_Client.chkUseProminenceMediaValue.Value : true; //If no value, default to true
                                    _Client.ForceCategorySelection = p_Client.chkForceCategorySelection.HasValue ? p_Client.chkForceCategorySelection.Value : false;
                                    _Client.MCMediaPublishedTemplateID = p_Client.ddlMCMediaPubTemplate;
                                    _Client.MCMediaDefaultEmailTemplateID = p_Client.ddlMCMediaEmailTemplate;
                                    _Client.IQRawMediaExpiration = p_Client.txtIQRawMediaExpiration;
                                    _Client.LibraryTextType = p_Client.ddlLibraryTextType;
                                    _Client.DefaultFeedsPageSize = p_Client.ddlDefaultFeedsPageSize;
                                    _Client.DefaultDiscoveryPageSize = p_Client.ddlDefaultDiscoveryPageSize;
                                    _Client.DefaultArchivePageSize = p_Client.ddlDefaultArchivePageSize;
                                    _Client.ClipEmbedAutoPlay = p_Client.chkClipEmbedAutoPlay;
                                    _Client.DefaultFeedsShowUnread = p_Client.chkDefaultFeedsShowUnread;
                                    _Client.UseCustomerEmailDefault = p_Client.chkUseCustomerEmailDefault;
                                    _Client.TVHighThreshold = p_Client.txtTVHighThreshold;
                                    _Client.TVLowThreshold = p_Client.txtTVLowThreshold;
                                    _Client.NMHighThreshold = p_Client.txtNMHighThreshold;
                                    _Client.NMLowThreshold = p_Client.txtNMLowThreshold;
                                    _Client.SMHighThreshold = p_Client.txtSMHighThreshold;
                                    _Client.SMLowThreshold = p_Client.txtSMLowThreshold;
                                    _Client.TwitterHighThreshold = p_Client.txtTwitterHighThreshold;
                                    _Client.TwitterLowThreshold = p_Client.txtTwitterLowThreshold;
                                    _Client.PQHighThreshold = p_Client.txtPQHighThreshold;
                                    _Client.PQLowThreshold = p_Client.txtPQLowThreshold;

                                    if (flPlayerLogo != null)
                                    {
                                        _Client.PlayerLogo = hfPlayerLogoImage;
                                    }

                                    XDocument xdoc = new XDocument(new XElement("Roles", from i in p_Client.chkRoles select new XElement("Role", i)));

                                    int Status = 0;
                                    int NotificationStatus = 0;
                                    int IQAgentStatus = 0;
                                    ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                                    _Result = clientLogic.UpdateClient(_Client, xdoc.ToString(), out Status, out NotificationStatus, out IQAgentStatus);

                                    if (NotificationStatus == -2)
                                    {
                                        RemoveImage(_Client.CustomHeaderImage, _Client.PlayerLogo, _Client.NotificationHeaderImage, _Client.ClientGuid);
                                        ViewBag.ErrorMessage = "Client already has more notification, then mentioned.";
                                    }
                                    else if (IQAgentStatus == -2)
                                    {
                                        RemoveImage(_Client.CustomHeaderImage, _Client.PlayerLogo, _Client.NotificationHeaderImage, _Client.ClientGuid);
                                        ViewBag.ErrorMessage = "Client already has more IQAgent Queries, then mentioned.";
                                        

                                    }
                                    else if (_Result == "-1")
                                    {
                                        RemoveImage(_Client.CustomHeaderImage, _Client.PlayerLogo, _Client.NotificationHeaderImage, _Client.ClientGuid);
                                        ViewBag.ErrorMessage = "Client With Same Name already Exist";
                                    }
                                    else if (Status == 1)
                                    {
                                        RemoveImage(_Client.CustomHeaderImage, _Client.PlayerLogo, _Client.NotificationHeaderImage, _Client.ClientGuid);
                                        ViewBag.ErrorMessage = _ImageExistMessage;
                                    }
                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "Please Select Atleast One Role";
                                }

                            }
                            else
                            {
                                ViewBag.ErrorMessage = "No of Users can not be less then 1";
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = _InvalidImageMessage;
                }
                if (string.IsNullOrEmpty(ViewBag.ErrorMessage))
                {
                    return Json(new
                    {
                        isSuccess = true,
                        clientId = _Result
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        errorMsg = ViewBag.ErrorMessage
                    });
                }
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false,
                    errorMsg = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("GlobalAdminTempData");
            }
        }

        public JsonResult DeleteClient(Int64 p_ClientKey)
        {
            try
            {
                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                clientLogic.DeleteClient(p_ClientKey);
                return Json(new
                {
                    isSuccess = true
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
                TempData.Keep("GlobalAdminTempData");
            }
        }
        [HttpPost]
        public JsonResult AddClientToAnewstip(long clientKey, string clientName)
        {
            try
            {
                string url = "https://connect.iqmcorp.com/api/client/create";
                string data = String.Format("key={0}&client_id={1}&client_name={2}", ConfigurationManager.AppSettings["AnewstipAPIKey"], clientKey, clientName);
                string response = Shared.Utility.CommonFunctions.DoHttpPostRequest(url, data, p_ContentType: "application/x-www-form-urlencoded", p_IgnoreResponseLength: true);

                bool isSuccess = false;
                if (!String.IsNullOrEmpty(response))
                {
                    Newtonsoft.Json.Linq.JObject jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(response);

                    if (Convert.ToString(jsonData["code"]) == "1")
                    {
                        isSuccess = true;
                        ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                        clientLogic.AddClientToAnewstip(clientKey, clientKey);
                    }
                    else
                        Utility.CommonFunctions.WriteException(new Exception(String.Format("Received error code from Anewstip Add Client API call.  Url: {0}?{1} || Error Code: {2} || Error Message: {3}", url, data, jsonData["code"], jsonData["msg"])));
                }
                else
                {
                    Utility.CommonFunctions.WriteException(new Exception(String.Format("No response received from Anewstip Add Client API call.  Url: {0}?{1}", url, data)));
                }

                return Json(new
                {
                    isSuccess = isSuccess
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false
                });
            }
        }

        private bool CheckForImage( HttpPostedFileBase flPlayerLogo,bool isCheckSize = true)
        {
            string[] Extentions = ConfigurationManager.AppSettings["ClientImageExtensions"].Split(new char[] { ',' });

            if (flPlayerLogo != null && flPlayerLogo.ContentLength > 0)
            {
                if (Extentions.Contains(System.IO.Path.GetExtension(flPlayerLogo.FileName).ToLower().Substring(1)))
                {
                    if (isCheckSize)
                    {
                        System.Drawing.Bitmap imagePlayerLogo = new System.Drawing.Bitmap(flPlayerLogo.InputStream);
                        int height = imagePlayerLogo.Height;
                        int width = imagePlayerLogo.Width;

                        if (height > Convert.ToInt16(ConfigurationManager.AppSettings["PlayerLogoHeight"])
                            || width > Convert.ToInt16(ConfigurationManager.AppSettings["PlayerLogoWidth"]))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void RemoveImage(string flCustomHeader, string flPlayerLogo, string flNotificationHeader,Guid clientGuid)
        {
            if (!String.IsNullOrEmpty(Convert.ToString(flCustomHeader)))
            {
                if (System.IO.File.Exists(ConfigurationManager.AppSettings["DirReportHeader"] + clientGuid + @"\" + Convert.ToString(flCustomHeader)))
                {
                    System.IO.File.Delete(ConfigurationManager.AppSettings["DirReportHeader"] + clientGuid + @"\" + Convert.ToString(flCustomHeader));
                }
            }

            if (!String.IsNullOrEmpty(Convert.ToString(flPlayerLogo)))
            {
                if (System.IO.File.Exists(ConfigurationManager.AppSettings["DirPlayerLogo"] + @"\" + Convert.ToString(flPlayerLogo)))
                {
                    System.IO.File.Delete(ConfigurationManager.AppSettings["DirPlayerLogo"] + @"\" + Convert.ToString(flPlayerLogo));
                }
            }

            if (!String.IsNullOrEmpty(Convert.ToString(flNotificationHeader)))
            {
                if (System.IO.File.Exists(ConfigurationManager.AppSettings["DirReportHeader"] + clientGuid + @"\" + Convert.ToString(flNotificationHeader)))
                {
                    System.IO.File.Delete(ConfigurationManager.AppSettings["DirReportHeader"] + clientGuid + @"\" + Convert.ToString(flNotificationHeader));
                }
            }
        }

        private void CreateImage(string clientName, HttpPostedFileBase flPlayerLogo ,out string hfPlayerLogoImage,Guid clientGuid)
        {
            string image = string.Empty;
            
            hfPlayerLogoImage = string.Empty;

            try
            {
                if (flPlayerLogo != null)
                {
                    image = Regex.Replace(clientName.Trim().Replace("\"", "_").Replace(@"\", "_"), @"[\/?:*|<>]", "_") + "_" + DateTime.Now.ToString().Replace(':', '_').Replace('/', '_') + "_PlayerLogo" + flPlayerLogo.FileName.ToString().Substring(flPlayerLogo.FileName.ToString().LastIndexOf('.'));
                    image = image.Replace(" ", "_");
                    hfPlayerLogoImage = image;
                    flPlayerLogo.SaveAs(ConfigurationManager.AppSettings["DirPlayerLogo"] + @"\" + image);
                }
            }
            catch (DirectoryNotFoundException ex)
            {


            }
        }

        #endregion

        #region Customer 

        public JsonResult DisplayCustomers(string p_ClientName, string p_CustomerName, bool? p_IsNext = null)
        {
            try
            {
                globalAdminTempData = GetTempData();

                if (p_IsNext != null)
                {
                    if (p_IsNext == true)
                    {
                        if (globalAdminTempData.CustomerHasMoreRecords == true)
                        {
                            globalAdminTempData.CustomerPageNumber = globalAdminTempData.CustomerPageNumber + 1;
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
                        if (globalAdminTempData.CustomerPageNumber > 0)
                        {
                            globalAdminTempData.CustomerPageNumber = globalAdminTempData.CustomerPageNumber - 1;
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
                    globalAdminTempData.CustomerHasMoreRecords = true;
                    globalAdminTempData.CustomerPageNumber = 0;
                }

                int totalResults = 0;
                int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CustomerPageSize"]);
                CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                List<CustomerModel> lstCustomerModel = customerLogic.GetAllCustomerWithRole(p_ClientName,p_CustomerName, globalAdminTempData.CustomerPageNumber, pageSize, out totalResults);

                if (totalResults > ((globalAdminTempData.CustomerPageNumber + 1) * pageSize))
                {
                    globalAdminTempData.CustomerHasMoreRecords = true;
                }
                else
                {
                    globalAdminTempData.CustomerHasMoreRecords = false;
                }

                string strHTML = RenderPartialToString(PATH_GloblaAdminCustomerListPartialView, lstCustomerModel);

                string strRecordLabel = " ";
                if (lstCustomerModel.Count > 0)
                {
                    strRecordLabel = "" + ((globalAdminTempData.CustomerPageNumber * pageSize) + 1).ToString() + " - " + ((globalAdminTempData.CustomerPageNumber * pageSize) + lstCustomerModel.Count).ToString() + " Of " + totalResults.ToString() + "";
                }
                SetTempData(globalAdminTempData);

                return Json(new
                {
                    isSuccess = true,
                    hasMoreRecords = globalAdminTempData.CustomerHasMoreRecords,
                    hasPreviousRecords = globalAdminTempData.CustomerPageNumber > 0 ? true : false,
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
                TempData.Keep("GlobalAdminTempData");
            }
        }

        [HttpPost]
        public JsonResult GetCustomerRegistration(int p_CustomerKey)
        {
            try
            {
                CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                Customer_DropDown objCustomer_DropDown = customerLogic.GetAllDropDown();

                CustomerPostModel customerPostModel = new CustomerPostModel();
                customerPostModel.Customer_DropDown = objCustomer_DropDown;

                if (p_CustomerKey == 0)
                {
                    customerPostModel.customer = new CustomerModel(); 
                }
                else
                {
                    CustomerModel objCustomerModel = customerLogic.GetCustomerWithRoleByCustomerID(p_CustomerKey);
                    customerPostModel.chkRoles = objCustomerModel.CustomerRoles.Where(a => a.Value == true).Select(a => a.Key).ToArray();
                    customerPostModel.customer = objCustomerModel;
                }

                return Json(new
                {
                    isSuccess = true,
                    HTML = RenderPartialToString(PATH_GloblaAdminCustomerRegistationPartialView, customerPostModel)
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
                TempData.Keep("GlobalAdminTempData");
            }
        }

        [HttpPost]
        public JsonResult CustomerRegistration(CustomerPostModel p_Customer)
        {
            try
            {
                if (p_Customer.customer.CustomerKey == 0 || Convert.ToBoolean(Request.Params["chkUpdatePassword"]) == true)
                {
                    if(string.IsNullOrWhiteSpace(p_Customer.customer.Password) || !Regex.IsMatch(p_Customer.customer.Password.Trim(),"^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,30}$"))
                    {
                        throw new CustomException("Invalid Password");
                    }
                }
                
                bool hasPassword = !string.IsNullOrWhiteSpace(p_Customer.customer.Password);

                if (p_Customer.customer.CustomerKey == 0 && !hasPassword)
                {
                    throw new CustomException("Password is required");
                }
                if (hasPassword && !Regex.IsMatch(p_Customer.customer.Password.Trim(), "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,30}$"))
                {
                    throw new CustomException("Invalid Password");
                }

                string ErrorMessage = string.Empty;
                string _Result = string.Empty;
                CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                
                bool _Checked = p_Customer.chkRoles.Count() > 0 ? true : false;
                if (_Checked == true)
                {
                    CustomerModel _Customer = new CustomerModel();

                    _Customer = p_Customer.customer;
                    _Customer.FirstName = !string.IsNullOrEmpty(_Customer.FirstName) ? _Customer.FirstName.Trim() : _Customer.FirstName;
                    _Customer.LastName = !string.IsNullOrEmpty(_Customer.LastName) ? _Customer.LastName.Trim() : _Customer.LastName;
                    _Customer.Email = !string.IsNullOrEmpty(_Customer.Email) ? _Customer.Email.Trim() : _Customer.Email;
                    _Customer.LoginID = !string.IsNullOrEmpty(_Customer.LoginID) ? _Customer.LoginID.Trim() : _Customer.LoginID;

                    if (hasPassword)
                    {
                        _Customer.Password = IQMedia.Security.Authentication.GetHashPassword(_Customer.Password.Trim()); 
                    }

                    _Customer.ContactNo = !string.IsNullOrEmpty(_Customer.ContactNo) ? _Customer.ContactNo.Trim() : _Customer.ContactNo;
                    _Customer.Comment = !string.IsNullOrEmpty(_Customer.Comment) ? _Customer.Comment.Trim() : _Customer.Comment;
                    _Customer.MultiLogin = _Customer.MultiLogin.HasValue ? _Customer.MultiLogin.Value : false;
                    _Customer.IsActive = _Customer.IsActive.HasValue ? _Customer.IsActive.Value : false;
                    _Customer.IsFliqCustomer = _Customer.IsFliqCustomer.HasValue ? _Customer.IsFliqCustomer : false;
                    XDocument xdoc = new XDocument(new XElement("Roles", from i in p_Customer.chkRoles select new XElement("Role", i)));

                    if (p_Customer.customer.CustomerKey == 0)
                    {
                        _Customer.CustomerGUID = System.Guid.NewGuid();
                        _Customer.CreatedBy = Utility.ActiveUserMgr.GetActiveUser().CustomerGUID.ToString();

                        _Result = customerLogic.InsertCustomer(_Customer, xdoc.ToString(), ConfigurationManager.AppSettings["DefaultCustomCategory"]);
                        if (_Result == "0" || _Result == "")
                        {
                            ErrorMessage = _CustomerExistMessage;
                        }
                        else if (_Result == "-1")
                        {
                            ErrorMessage = _CustomerCanNotFliq;
                        }
                    }
                    else
                    {
                        _Customer.ModifiedDate = DateTime.Now;
                        _Customer.ModifiedBy = Utility.ActiveUserMgr.GetActiveUser().CustomerGUID.ToString();

                        _Result = customerLogic.UpdateCustomer(_Customer, xdoc.ToString(), ConfigurationManager.AppSettings["DefaultCustomCategory"]);
                        if (_Result == "0")
                        {
                            ErrorMessage = _CustomerExistMessage;
                        }
                        else if (_Result == "-1")
                        {
                            ErrorMessage = _CustomerCanNotFliq;
                        }
                    }
                }
                else
                {
                    ErrorMessage = "Please Select Atleast One Role";
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    return Json(new
                    {
                        isSuccess = true,
                        customerId = _Result
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
            catch (CustomException ex)
            {
                return Json(new
                {
                    isSuccess = false,
                    errorMsg = ex.Message
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    isSuccess = false,
                    errorMsg = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("GlobalAdminTempData");
            }
        }

        [HttpPost]
        public JsonResult DeleteCustomer(Int64 p_CustomerKey)
        {
            try
            {
                CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                customerLogic.DeleteCustomer(p_CustomerKey);
                return Json(new
                {
                    isSuccess = true
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
                TempData.Keep("GlobalAdminTempData");
            }
        }

        [HttpPost]
        public JsonResult ResetPasswordAttempts(long p_CustomerKey)
        {
            try
            {
                CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                int result = customerLogic.ResetPasswordAttempts(p_CustomerKey);

                return Json(new
                {
                    isSuccess = result == 1
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

        [HttpPost]
        public JsonResult SetRolesFromClient(Int64 p_ClientID)
        {
            try
            {
                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                ClientModel clientModel = clientLogic.GetClientWithRoleByClientID(p_ClientID);

                List<string> activeRoles = clientModel.ClientRoles.Where(w => w.Value).Select(s => s.Key).ToList();
                
                return Json(new
                {
                    isSuccess = true,
                    activeRoles = activeRoles
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
        }

        [HttpPost]
        public JsonResult AddCustomerToAnewstip(long customerKey)
        {
            try
            {
                CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                CustomerModel customer = customerLogic.GetCustomerWithRoleByCustomerID(customerKey);

                string url = "https://connect.iqmcorp.com/api/user/create";
                string data = String.Format("key={0}&client_id={1}&user_id={2}&user_name={3}&user_email={4}", ConfigurationManager.AppSettings["AnewstipAPIKey"], customer.ClientID, customer.LoginID, customer.FirstName + " " + customer.LastName, customer.Email);
                string response = Shared.Utility.CommonFunctions.DoHttpPostRequest(url, data, p_ContentType: "application/x-www-form-urlencoded", p_IgnoreResponseLength: true);

                bool isSuccess = false;
                if (!String.IsNullOrEmpty(response))
                {
                    Newtonsoft.Json.Linq.JObject jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(response);

                    if (Convert.ToString(jsonData["code"]) == "1")
                    {
                        isSuccess = true;
                        customerLogic.AddCustomerToAnewstip(customerKey, customer.LoginID);
                    }
                    else
                        Utility.CommonFunctions.WriteException(new Exception(String.Format("Received error code from Anewstip Add User API call.  Url: {0}?{1} || Error Code: {2} || Error Message: {3}", url, data, jsonData["code"], jsonData["msg"])));
                }
                else
                {
                    Utility.CommonFunctions.WriteException(new Exception(String.Format("No response received from Anewstip Add User API call.  Url: {0}?{1}", url, data)));
                }

                return Json(new
                {
                    isSuccess = isSuccess
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false
                });
            }
        }
        #endregion
        #region Client UGC Settings

        [HttpPost]
        public JsonResult DisplayUGCSetupSettings(string p_ClientName, string p_SearchTerm, bool? p_IsNext = null)
        {
            try
            {
                globalAdminTempData = GetTempData();

                if (p_IsNext != null)
                {
                    if (p_IsNext == true)
                    {
                        if (globalAdminTempData.ClientUGCMapHasMoreRecords == true)
                        {
                            globalAdminTempData.ClientUGCMapPageNumber = globalAdminTempData.ClientUGCMapPageNumber + 1;
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
                        if (globalAdminTempData.ClientUGCMapPageNumber > 0)
                        {
                            globalAdminTempData.ClientUGCMapPageNumber = globalAdminTempData.ClientUGCMapPageNumber - 1;
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
                    globalAdminTempData.ClientUGCMapHasMoreRecords = true;
                    globalAdminTempData.ClientUGCMapPageNumber = 0;
                }

                Int64 totalResults = 0;
                int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ClientUGCMapPageSize"]);
                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                List<IQClient_UGCMapModel> lstIQClient_UGCMapModel = clientLogic.GetAllClientUGCSettings(p_ClientName, p_SearchTerm, globalAdminTempData.ClientUGCMapPageNumber, pageSize, out totalResults);

                if (totalResults > ((globalAdminTempData.ClientUGCMapPageNumber + 1) * pageSize))
                {
                    globalAdminTempData.ClientUGCMapHasMoreRecords = true;
                }
                else
                {
                    globalAdminTempData.ClientUGCMapHasMoreRecords = false;
                }


                string strHTML = RenderPartialToString(PATH_GloblaAdminClientUGC_MapListPartialView, lstIQClient_UGCMapModel);

                string strRecordLabel = " ";
                if (lstIQClient_UGCMapModel.Count > 0)
                {
                    strRecordLabel = "" + ((globalAdminTempData.ClientUGCMapPageNumber * pageSize) + 1).ToString() + " - " + ((globalAdminTempData.ClientUGCMapPageNumber * pageSize) + lstIQClient_UGCMapModel.Count).ToString() + " Of " + totalResults.ToString() + "";
                }
                SetTempData(globalAdminTempData);

                return Json(new
                {
                    isSuccess = true,
                    hasMoreRecords = globalAdminTempData.ClientUGCMapHasMoreRecords,
                    hasPreviousRecords = globalAdminTempData.ClientUGCMapPageNumber > 0 ? true : false,
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
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult EditUGCSetupSettings(Int64 p_IQClient_UGCMapKey)
        {
            try
            {
                IQClient_UGCMapPostModel objIQClient_UGCMapPostModel = new IQClient_UGCMapPostModel();

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);

                objIQClient_UGCMapPostModel.IQClient_UGCMapDropDowns = clientLogic.GetUGCSettingsDropdown();
                if (p_IQClient_UGCMapKey > 0)
                {
                    objIQClient_UGCMapPostModel.IQClient_UGCMapModel = clientLogic.GetUGCSettingsByUGCMapKey(p_IQClient_UGCMapKey);
                }
                else
                {
                    objIQClient_UGCMapPostModel.IQClient_UGCMapModel = new IQClient_UGCMapModel();
                }

                return Json(new
                {
                    isSuccess = true,
                    HTML = RenderPartialToString(PATH_GloblaAdminUGCSetupPartialView, objIQClient_UGCMapPostModel)
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

        public JsonResult SubmitUGCSettings(IQClient_UGCMapPostModel p_IQClient_UGCMapPostModel, HttpPostedFileBase flUGCMapLogo)
        {
            try
            {

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.Title = !string.IsNullOrEmpty(p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.Title) ? p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.Title.Trim() : p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.Title;
                p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.URL = !string.IsNullOrEmpty(p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.URL) ? p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.URL.Trim() : p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.URL;
                p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.SourceID = !string.IsNullOrEmpty(p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.SourceID) ? p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.SourceID.Trim() : p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.SourceID;

                if (p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.IQClient_UGCMapKey == 0)
                {
                    if (CheckForImage(flUGCMapLogo,false))
                    {
                        if (flUGCMapLogo != null)
                        {
                            p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.Logo = p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.SourceID + Path.GetExtension(flUGCMapLogo.FileName);

                            flUGCMapLogo.SaveAs(ConfigurationManager.AppSettings["UGCMapLogo"] + @"\" + p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.Logo);
                        }

                        string result = clientLogic.InsertIQClient_UGCMap(p_IQClient_UGCMapPostModel.IQClient_UGCMapModel);

                        if (Convert.ToInt64(result) > 0)
                        {
                            return Json(new
                            {
                                isSuccess = true,
                            });
                        }
                        else if (Convert.ToInt32(result) == -1)
                        {
                            return Json(new
                            {
                                isSuccess = false,
                                error = Config.ConfigSettings.Settings.SourceAlreadyExist
                            });
                        }
                        else if (Convert.ToInt32(result) == -3)
                        {
                            return Json(new
                            {
                                isSuccess = false,
                                error = Config.ConfigSettings.Settings.ClientUGCMapSettingsAlreadyExist
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                isSuccess = false,
                                error = Config.ConfigSettings.Settings.ErrorOccurred
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            isSuccess = false,
                            error = _InvalidImageMessage
                        });
                    }
                }
                else
                {
                    if (flUGCMapLogo == null || CheckForImage(flUGCMapLogo,false))
                    {
                        if (flUGCMapLogo != null)
                        {
                            p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.Logo = p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.SourceID + Path.GetExtension(flUGCMapLogo.FileName);
                            if (System.IO.File.Exists(ConfigurationManager.AppSettings["UGCMapLogo"] + p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.Logo))
                            {
                                System.IO.File.Delete(ConfigurationManager.AppSettings["UGCMapLogo"] + p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.Logo);
                            }
                            flUGCMapLogo.SaveAs(ConfigurationManager.AppSettings["UGCMapLogo"] + p_IQClient_UGCMapPostModel.IQClient_UGCMapModel.Logo);
                        }

                        string result = clientLogic.UpdateIQClient_UGCMap(p_IQClient_UGCMapPostModel.IQClient_UGCMapModel);
                        if (Convert.ToInt32(result) > 0)
                        {
                            return Json(new
                            {
                                isSuccess = true,
                            });
                        }
                        else if (Convert.ToInt32(result) == -1)
                        {
                            return Json(new
                            {
                                isSuccess = false,
                                error = Config.ConfigSettings.Settings.SourceAlreadyExist
                            });
                        }
                        else if (Convert.ToInt32(result) == -2)
                        {
                            return Json(new
                            {
                                isSuccess = false,
                                error = Config.ConfigSettings.Settings.ClientUGCMapSettingsAlreadyExist
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                isSuccess = false,
                                error = Config.ConfigSettings.Settings.ErrorOccurred
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            isSuccess = false,
                            error = _InvalidImageMessage
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

        #endregion

        #region Utility

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

        

        public GlobalAdminTempData GetTempData()
        {
            globalAdminTempData = TempData["GlobalAdminTempData"] != null ? (GlobalAdminTempData)TempData["GlobalAdminTempData"] : new GlobalAdminTempData();
            return globalAdminTempData;
        }

        public void SetTempData(GlobalAdminTempData p_GlobalAdminTempData)
        {
            TempData["GlobalAdminTempData"] = p_GlobalAdminTempData;
            TempData.Keep("GlobalAdminTempData");
        }


        #endregion
        #region Group

        public JsonResult GetAllActiveClient()
        {
            try
            {
                var clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                var clientList = clientLogic.SelectAllActive();

                clientList = clientList.OrderBy(c => c.ClientName).ToList();

                var clients = from client in clientList
                              select new { ID = client.ClientKey, Name = client.ClientName, RID = client.MCID };

                var groupHtml = RenderPartialToString(PATH_GloblaAdminGroupPartialView, clientList);

                return Json(new
                {

                    isSuccess = true,
                    html = groupHtml,
                    clients = Newtonsoft.Json.JsonConvert.SerializeObject(clients)
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
                TempData.Keep("GlobalAdminTempData");
            }
        }

        public JsonResult GroupAddSubClient(Int64 p_ID, List<Int64> p_RIDList)
        {
            try
            {
                if ((p_ID <= 0 || p_RIDList.Count() == 0 || p_RIDList.Where(sc => sc <= 0).Count() > 0 || p_RIDList.Where(sc => sc == p_ID).Count() > 0))
                {
                    throw new CustomException("Invalid Input.");
                }

                var clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                var clientList = clientLogic.SelectAllActive();

                var isValid = clientLogic.GroupAddSubClient(p_ID, p_RIDList, clientList, ActiveUserMgr.GetActiveUser().CustomerGUID);

                return Json(new { isSuccess = isValid });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("GlobalAdminTempData");
            }
        }

        public JsonResult GroupRemoveSubClient(Int64 p_ID, Int64 p_RID)
        {
            try
            {
                if (p_ID <= 0 || p_RID <= 0 || p_ID == p_RID)
                {
                    throw new CustomException("Invalid Input.");
                }

                var clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);

                var isValid = clientLogic.GroupRemoveSubClient(p_ID, p_RID, ActiveUserMgr.GetActiveUser().CustomerGUID);

                return Json(new { isSuccess = isValid });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("GlobalAdminTempData");
            }
        }

        public JsonResult GroupGetCLCUST(Int64 p_ID, Int64? p_CID)
        {
            try
            {
                if (p_ID <= 0 || p_CID <= 0)
                {
                    throw new CustomException("Invalid Input.");
                }

                var clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);

                var custList = clientLogic.GroupGetCustomerByClient(p_ID, p_CID);

                var customers = (from cust in custList
                                 select new { ID = cust.CustomerKey, Name = cust.FirstName + " " + cust.LastName }).OrderBy(c => c.Name);

                return Json(new { isSuccess = true, customers = Newtonsoft.Json.JsonConvert.SerializeObject(customers) });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("GlobalAdminTempData");
            }
        }

        public JsonResult GroupAddSubCustomer(Int64 p_GrpID, Int64 p_MCID, Int64 p_SCID, Int64 p_MCCID, Int64 p_SCCID)
        {
            try
            {
                var isValid = false;

                if (p_GrpID == 0 || p_MCID == 0 || p_SCID == 0 || p_MCCID == 0 || p_SCCID == 0)
                {
                    throw new CustomException("Invalid Input");
                }

                var custLgc = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                isValid = custLgc.GroupAddSubCustomer(p_GrpID, p_MCID, p_SCID, p_MCCID, p_SCCID, ActiveUserMgr.GetActiveUser().CustomerGUID);

                return Json(new
                {
                    isSuccess = isValid
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("GlobalAdminTempData");
            }
        }

        public JsonResult GroupGetSubCustomer(Int64 p_MCustID)
        {
            try
            {
                if (!(p_MCustID > 0))
                {
                    throw new CustomException("Invalid Input");
                }

                var custLgc = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                var custList = custLgc.GroupGetSubCustomerByCustomer(p_MCustID);

                var customers = from cust in custList
                                select new { ID = cust.CustomerKey, Name = cust.FirstName + " " + cust.LastName, CID = cust.ClientID, LID = cust.LoginID };

                return Json(new { isSuccess = true, customers = Newtonsoft.Json.JsonConvert.SerializeObject(customers) });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("GlobalAdminTempData");
            }
        }

        public JsonResult GroupRemoveSubCustomer(Int64 p_MCustID, Int64 p_SCustID)
        {
            try
            {
                if (p_MCustID <= 0 || p_SCustID <= 0 || p_MCustID == p_SCustID)
                {
                    throw new CustomException("Invalid Input.");
                }

                var custLgc = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);

                var isValid = custLgc.GroupRemoveSubCustomer(p_MCustID, p_SCustID, ActiveUserMgr.GetActiveUser().CustomerGUID);

                return Json(new { isSuccess = isValid });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("GlobalAdminTempData");
            }
        }
        #endregion Group
    }
}
