using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.WebApplication.Models;
using System.IO;
using IQMedia.Web.Logic;
using IQMedia.Model;
using IQMedia.Web.Logic.Base;
using System.Configuration;
using IQMedia.Shared.Utility;
using System.Text.RegularExpressions;

namespace IQMedia.WebApplication.Controllers
{
    public class FliqCustomerController : Controller
    {

        GlobalAdminTempData globalAdminTempData = null;
        private string _CustomerExistMessage = "Customer is already exists.";
        private string _ApplicationExistMessage = "Application is already exists.";
        private string _ClientApplicationExistMessage = "Client application is already exists.";
        private string _ClientNotFliqMessage = "Selected client is not fliq client";
        private string _CustomerApplicationExistMessage = "Customer application is already exists.";
        private string _CustomerNotFliqMessage = "invalid selection";
        private string _CustomerCanNotFliq = "Customer can not be Fliq Customer, as Selected Client is not Fliq Client";
        string PATH_GloblaAdminFliq_CustomerListPartialView = "~/Views/GlobalAdmin/_Fliq_CustomerList.cshtml";
        string PATH_GloblaAdminFliq_CustomerRegistationPartialView = "~/Views/GlobalAdmin/_Fliq_CustomerRegistation.cshtml";
        string PATH_GloblaAdminFliq_ApplicationListPartialView = "~/Views/GlobalAdmin/_Fliq_ApplicationList.cshtml";
        string PATH_GloblaAdminFliq_ApplicationRegistationPartialView = "~/Views/GlobalAdmin/_Fliq_ApplicationRegistation.cshtml";
        string PATH_GloblaAdminFliq_ClientApplicationListPartialView = "~/Views/GlobalAdmin/_Fliq_ClientApplicationList.cshtml";
        string PATH_GloblaAdminFliq_ClientApplicationRegistationPartialView = "~/Views/GlobalAdmin/_Fliq_ClientApplicationRegistation.cshtml";
        string PATH_GloblaAdminFliq_CustomerApplicationListPartialView = "~/Views/GlobalAdmin/_Fliq_CustomerApplicationList.cshtml";
        string PATH_GloblaAdminFliq_CustomerApplicationRegistationPartialView = "~/Views/GlobalAdmin/_Fliq_CustomerApplicationRegistation.cshtml";
        //
        // GET: /FliqCustomer/

        public ActionResult Index()
        {
            return View();
        }

        #region Customer

        public JsonResult DisplayFliq_Customers(string p_ClientName, string p_CustomerName, bool? p_IsNext = null)
        {
            try
            {
                globalAdminTempData = GetTempData();

                if (p_IsNext != null)
                {
                    if (p_IsNext == true)
                    {
                        if (globalAdminTempData.fliq_CustomerHasMoreRecords == true)
                        {
                            globalAdminTempData.fliq_CustomerPageNumber = globalAdminTempData.fliq_CustomerPageNumber + 1;
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
                        if (globalAdminTempData.fliq_CustomerPageNumber > 0)
                        {
                            globalAdminTempData.fliq_CustomerPageNumber = globalAdminTempData.fliq_CustomerPageNumber - 1;
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
                    globalAdminTempData.fliq_CustomerHasMoreRecords = true;
                    globalAdminTempData.fliq_CustomerPageNumber = 0;
                }

                int totalResults = 0;
                int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CustomerPageSize"]);
                fliq_CustomerLogic fliq_CustomerLogic = (fliq_CustomerLogic)LogicFactory.GetLogic(LogicType.fliq_Customer);
                List<fliq_CustomerModel> lstfliq_CustomerModel = fliq_CustomerLogic.GetAllfliq_Customer(p_ClientName,p_CustomerName, globalAdminTempData.fliq_CustomerPageNumber, pageSize, out totalResults);

                if (totalResults > ((globalAdminTempData.fliq_CustomerPageNumber + 1) * pageSize))
                {
                    globalAdminTempData.fliq_CustomerHasMoreRecords = true;
                }
                else
                {
                    globalAdminTempData.fliq_CustomerHasMoreRecords = false;
                }

                string strHTML = RenderPartialToString(PATH_GloblaAdminFliq_CustomerListPartialView, lstfliq_CustomerModel);

                string strRecordLabel = " ";
                if (lstfliq_CustomerModel.Count > 0)
                {
                    strRecordLabel = "" + ((globalAdminTempData.fliq_CustomerPageNumber * pageSize) + 1).ToString() + " - " + ((globalAdminTempData.fliq_CustomerPageNumber * pageSize) + lstfliq_CustomerModel.Count).ToString() + " Of " + totalResults.ToString() + "";
                }
                SetTempData(globalAdminTempData);

                return Json(new
                {
                    isSuccess = true,
                    hasMoreRecords = globalAdminTempData.fliq_CustomerHasMoreRecords,
                    hasPreviousRecords = globalAdminTempData.fliq_CustomerPageNumber > 0 ? true : false,
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
        public JsonResult GetFliq_CustomerRegistration(int p_CustomerKey)
        {
            try
            {
                fliq_CustomerLogic fliq_CustomerLogic = (fliq_CustomerLogic)LogicFactory.GetLogic(LogicType.fliq_Customer);
                Customer_DropDown objCustomer_DropDown = fliq_CustomerLogic.GetAllDropDown();

                fliq_CustomerPostModel customerPostModel = new fliq_CustomerPostModel();
                customerPostModel.Customer_DropDown = objCustomer_DropDown;

                if (p_CustomerKey == 0)
                {
                    customerPostModel.customer = new fliq_CustomerModel();
                }
                else
                {
                    fliq_CustomerModel objfliq_CustomerModel = fliq_CustomerLogic.Gefliq_CustomerByCustomerID(p_CustomerKey);
                    customerPostModel.customer = objfliq_CustomerModel;
                }

                return Json(new
                {
                    isSuccess = true,
                    HTML = RenderPartialToString(PATH_GloblaAdminFliq_CustomerRegistationPartialView, customerPostModel)
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
        public JsonResult Fliq_CustomerRegistration(fliq_CustomerPostModel p_Customer)
        {
            try
            {
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
                fliq_CustomerLogic fliq_CustomerLogic = (fliq_CustomerLogic)LogicFactory.GetLogic(LogicType.fliq_Customer);


                fliq_CustomerModel _Customer = new fliq_CustomerModel();
                _Customer = p_Customer.customer;
                _Customer.CustomerGUID = System.Guid.NewGuid();
                _Customer.IsActive = _Customer.IsActive.HasValue ? _Customer.IsActive : false;

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

                
                if (p_Customer.customer.CustomerKey == 0)
                {
                    _Customer.CreatedBy = Utility.ActiveUserMgr.GetActiveUser().CustomerGUID.ToString();

                    _Result = fliq_CustomerLogic.Insertfliq_Customer(_Customer, ConfigurationManager.AppSettings["DefaultCustomCategory"]);
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

                    _Result = fliq_CustomerLogic.Updatefliq_Customer(_Customer);
                    if (_Result == "0")
                    {
                        ErrorMessage = _CustomerExistMessage;
                    }
                    else if (_Result == "-1")
                    {
                        ErrorMessage = _CustomerCanNotFliq;
                    }
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
        public JsonResult DeleteFliq_Customer(Int64 p_CustomerKey)
        {
            try
            {
                fliq_CustomerLogic fliq_CustomerLogic = (fliq_CustomerLogic)LogicFactory.GetLogic(LogicType.fliq_Customer);
                fliq_CustomerLogic.Deletefliq_Customer(p_CustomerKey);
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
        public JsonResult GetFliqClientsList()
        {
            try
            {
                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                List<ClientModel> _ListOfClient = new List<ClientModel>();
                _ListOfClient = clientLogic.SelectAllFliqClient();
                return Json(new
                {
                    isSuccess = true,
                    clientList = _ListOfClient.Select(a => a.ClientName).ToArray()
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

        #endregion

        #region Application

        [HttpPost]
        public JsonResult DisplayFliq_Application(string p_Application, bool? p_IsNext = null)
        {
            try
            {
                globalAdminTempData = GetTempData();

                if (p_IsNext != null)
                {
                    if (p_IsNext == true)
                    {
                        if (globalAdminTempData.fliq_ApplicationHasMoreRecords == true)
                        {
                            globalAdminTempData.fliq_ApplicationPageNumber = globalAdminTempData.fliq_ApplicationPageNumber + 1;
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
                        if (globalAdminTempData.fliq_ApplicationPageNumber > 0)
                        {
                            globalAdminTempData.fliq_ApplicationPageNumber = globalAdminTempData.fliq_ApplicationPageNumber - 1;
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
                    globalAdminTempData.fliq_ApplicationHasMoreRecords = true;
                    globalAdminTempData.fliq_ApplicationPageNumber = 0;
                }

                int totalResults = 0;
                int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ApplicationPageSize"]);
                fliq_ApplicationLogic fliq_ApplicationLogic = (fliq_ApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_Application);
                List<fliQ_ApplicationModel> lstfliQ_ApplicationModel = fliq_ApplicationLogic.GetAllfliq_Application(p_Application, globalAdminTempData.fliq_ApplicationPageNumber, pageSize, out totalResults);

                if (totalResults > ((globalAdminTempData.fliq_ApplicationPageNumber + 1) * pageSize))
                {
                    globalAdminTempData.fliq_ApplicationHasMoreRecords = true;
                }
                else
                {
                    globalAdminTempData.fliq_ApplicationHasMoreRecords = false;
                }

                string strHTML = RenderPartialToString(PATH_GloblaAdminFliq_ApplicationListPartialView, lstfliQ_ApplicationModel);

                string strRecordLabel = " ";
                if (lstfliQ_ApplicationModel.Count > 0)
                {
                    strRecordLabel = "" + ((globalAdminTempData.fliq_ApplicationPageNumber * pageSize) + 1).ToString() + " - " + ((globalAdminTempData.fliq_ApplicationPageNumber * pageSize) + lstfliQ_ApplicationModel.Count).ToString() + " Of " + totalResults.ToString() + "";
                }
                SetTempData(globalAdminTempData);

                return Json(new
                {
                    isSuccess = true,
                    hasMoreRecords = globalAdminTempData.fliq_ApplicationHasMoreRecords,
                    hasPreviousRecords = globalAdminTempData.fliq_ApplicationPageNumber > 0 ? true : false,
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
        public JsonResult GetFliq_ApplicationRegistration(Int64 p_ApplicationID)
        {
            try
            {
                fliq_ApplicationLogic fliq_ApplicationLogic = (fliq_ApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_Application);

                fliQ_ApplicationModel objfliQ_ApplicationModel;
                if (p_ApplicationID == 0)
                {
                    objfliQ_ApplicationModel = new fliQ_ApplicationModel();
                }
                else
                {
                    objfliQ_ApplicationModel = fliq_ApplicationLogic.Getliq_ApplicationByID(p_ApplicationID);
                }

                return Json(new
                {
                    isSuccess = true,
                    HTML = RenderPartialToString(PATH_GloblaAdminFliq_ApplicationRegistationPartialView, objfliQ_ApplicationModel)
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
        public JsonResult Fliq_ApplicationRegistration(fliQ_ApplicationModel p_Application)
        {
            try
            {
                string ErrorMessage = string.Empty;
                string _Result = string.Empty;
                fliq_ApplicationLogic fliq_ApplicationLogic = (fliq_ApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_Application);

                p_Application.IsActive = p_Application.IsActive.HasValue ? p_Application.IsActive : false;
                p_Application.Application = !string.IsNullOrEmpty(p_Application.Application) ? p_Application.Application.Trim() : p_Application.Application;
                p_Application.Path = !string.IsNullOrEmpty(p_Application.Path) ? p_Application.Path.Trim() : p_Application.Path;
                p_Application.Version = !string.IsNullOrEmpty(p_Application.Version) ? p_Application.Version.Trim() : p_Application.Version;
                p_Application.Description = !string.IsNullOrEmpty(p_Application.Description) ? p_Application.Description.Trim() : p_Application.Description;

                if (p_Application.ID == 0)
                {
                    _Result = fliq_ApplicationLogic.Insertfliq_Application(p_Application);
                    if (_Result == "0" || _Result == "")
                    {
                        ErrorMessage = _ApplicationExistMessage;
                    }
                }
                else
                {
                    _Result = fliq_ApplicationLogic.Updatefliq_Application(p_Application);
                    if (_Result == "0")
                    {
                        ErrorMessage = _ApplicationExistMessage;
                    }
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
        public JsonResult DeleteFliq_Application(Int64 p_ApplicationID)
        {
            try
            {
                fliq_ApplicationLogic fliq_ApplicationLogic = (fliq_ApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_Application);
                fliq_ApplicationLogic.Deletefliq_Application(p_ApplicationID);
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

        #endregion

        #region Client Application

        [HttpPost]
        public JsonResult BindCustomCategoryDropDown(Int64 p_ClientID)
        {
            try
            {
                CustomCategoryLogic customCategoryLogic = (CustomCategoryLogic)LogicFactory.GetLogic(LogicType.Category);
                List<CustomCategoryModel> customCategoryModelList = customCategoryLogic.GetCustomCategoryByClientID(p_ClientID).ToList();

                return Json(new
                {
                    customCategories = customCategoryModelList,
                    isSuccess = true
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
            finally { TempData.Keep("GlobalAdminTempData"); }
        }

        [HttpPost]
        public JsonResult DisplayFliq_ClientApplication(string p_ClientName, string p_ApplicationName, bool? p_IsNext = null)
        {
            try
            {
                p_ClientName = string.IsNullOrEmpty(p_ClientName) ? null : p_ClientName;

                globalAdminTempData = GetTempData();

                if (p_IsNext != null)
                {
                    if (p_IsNext == true)
                    {
                        if (globalAdminTempData.fliq_ClientApplicationHasMoreRecords == true)
                        {
                            globalAdminTempData.fliq_ClientApplicationPageNumber = globalAdminTempData.fliq_ClientApplicationPageNumber + 1;
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
                        if (globalAdminTempData.fliq_ClientApplicationPageNumber > 0)
                        {
                            globalAdminTempData.fliq_ClientApplicationPageNumber = globalAdminTempData.fliq_ClientApplicationPageNumber - 1;
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
                    globalAdminTempData.fliq_ClientApplicationHasMoreRecords = true;
                    globalAdminTempData.fliq_ClientApplicationPageNumber = 0;
                }

                int totalResults = 0;
                int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ClientApplicationPageSize"]);
                fliq_ClientApplicationLogic fliq_ClientApplicationLogic = (fliq_ClientApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_ClientApplication);
                List<fliQ_ClientApplicationModel> lstfliQ_ClientApplicationModel = fliq_ClientApplicationLogic.GetAllfliq_ClientApplication(p_ClientName, p_ApplicationName, globalAdminTempData.fliq_ClientApplicationPageNumber, pageSize, out totalResults);

                if (totalResults > ((globalAdminTempData.fliq_ClientApplicationPageNumber + 1) * pageSize))
                {
                    globalAdminTempData.fliq_ClientApplicationHasMoreRecords = true;
                }
                else
                {
                    globalAdminTempData.fliq_ClientApplicationHasMoreRecords = false;
                }

                string strHTML = RenderPartialToString(PATH_GloblaAdminFliq_ClientApplicationListPartialView, lstfliQ_ClientApplicationModel);

                string strRecordLabel = " ";
                if (lstfliQ_ClientApplicationModel.Count > 0)
                {
                    strRecordLabel = "" + ((globalAdminTempData.fliq_ClientApplicationPageNumber * pageSize) + 1).ToString() + " - " + ((globalAdminTempData.fliq_ClientApplicationPageNumber * pageSize) + lstfliQ_ClientApplicationModel.Count).ToString() + " Of " + totalResults.ToString() + "";
                }
                SetTempData(globalAdminTempData);

                return Json(new
                {
                    isSuccess = true,
                    hasMoreRecords = globalAdminTempData.fliq_ClientApplicationHasMoreRecords,
                    hasPreviousRecords = globalAdminTempData.fliq_ClientApplicationPageNumber > 0 ? true : false,
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
        public JsonResult GetFliq_ClientApplicationRegistration(Int64 p_ID)
        {
            try
            {
                fliq_ClientApplicationLogic fliq_ClientApplicationLogic = (fliq_ClientApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_ClientApplication);

                fliQ_ClientApplicationPostModel objfliQ_ClientApplicationPostModel = new fliQ_ClientApplicationPostModel();
                objfliQ_ClientApplicationPostModel.ClientApplication_DropDown = fliq_ClientApplicationLogic.GetClientApplication_Dropdowns();
                if (p_ID == 0)
                {
                    objfliQ_ClientApplicationPostModel.clientApplication = new fliQ_ClientApplicationModel();
                }
                else
                {
                    objfliQ_ClientApplicationPostModel.clientApplication = fliq_ClientApplicationLogic.Getliq_ClientApplicationByID(p_ID);
                }

                return Json(new
                {
                    isSuccess = true,
                    HTML = RenderPartialToString(PATH_GloblaAdminFliq_ClientApplicationRegistationPartialView, objfliQ_ClientApplicationPostModel)
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
        public JsonResult Fliq_ClientApplicationRegistration(fliQ_ClientApplicationPostModel p_ClientApplication)
        {
            try
            {
                string ErrorMessage = string.Empty;
                string _Result = string.Empty;
                fliq_ClientApplicationLogic fliq_ClientApplicationLogic = (fliq_ClientApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_ClientApplication);

                p_ClientApplication.clientApplication.IsActive = p_ClientApplication.clientApplication.IsActive.HasValue ? p_ClientApplication.clientApplication.IsActive : false;
                p_ClientApplication.clientApplication.IsCategoryEnable = p_ClientApplication.clientApplication.IsCategoryEnable.HasValue ? p_ClientApplication.clientApplication.IsCategoryEnable : false;
                p_ClientApplication.clientApplication.IsLandscapeOnly = p_ClientApplication.clientApplication.IsLandscapeOnly.HasValue ? p_ClientApplication.clientApplication.IsLandscapeOnly : false;
                p_ClientApplication.clientApplication.FTPHost = !string.IsNullOrEmpty(p_ClientApplication.clientApplication.FTPHost) ? p_ClientApplication.clientApplication.FTPHost.Trim() : p_ClientApplication.clientApplication.FTPHost;
                p_ClientApplication.clientApplication.FTPPath = !string.IsNullOrEmpty(p_ClientApplication.clientApplication.FTPPath) ? p_ClientApplication.clientApplication.FTPPath.Trim() : p_ClientApplication.clientApplication.FTPPath;
                p_ClientApplication.clientApplication.FTPLoginID = !string.IsNullOrEmpty(p_ClientApplication.clientApplication.FTPLoginID) ? p_ClientApplication.clientApplication.FTPLoginID.Trim() : p_ClientApplication.clientApplication.FTPLoginID;
                p_ClientApplication.clientApplication.FTPPwd = !string.IsNullOrEmpty(p_ClientApplication.clientApplication.FTPPwd) ? p_ClientApplication.clientApplication.FTPPwd.Trim() : p_ClientApplication.clientApplication.FTPPwd;

                if (p_ClientApplication.clientApplication.ID == 0)
                {
                    _Result = fliq_ClientApplicationLogic.Insertfliq_ClientApplication(p_ClientApplication.clientApplication);
                    if (_Result == "0" || _Result == "")
                    {
                        ErrorMessage = _ClientApplicationExistMessage;
                    }
                    else if (_Result == "-1")
                    {
                        ErrorMessage = _ClientNotFliqMessage;
                    }
                }
                else
                {
                    _Result = fliq_ClientApplicationLogic.Updatefliq_ClientApplication(p_ClientApplication.clientApplication);
                    if (_Result == "0" || _Result == "")
                    {
                        ErrorMessage = _ClientApplicationExistMessage;
                    }
                    else if (_Result == "-1")
                    {
                        ErrorMessage = _ClientNotFliqMessage;
                    }
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
        public JsonResult DeleteFliq_ClientApplication(Int64 p_ID)
        {
            try
            {
                fliq_ClientApplicationLogic fliq_ClientApplicationLogic = (fliq_ClientApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_ClientApplication);
                fliq_ClientApplicationLogic.Deletefliq_ClientApplication(p_ID);
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

        #endregion

        #region Customer Application

        [HttpPost]
        public JsonResult DisplayFliq_CustomerApplication(string p_ClientName, string p_CustomerName, bool p_IsAsc, string p_SortColumn,bool? p_IsNext = null)
        {
            try
            {
                p_ClientName = string.IsNullOrEmpty(p_ClientName) ? null : p_ClientName;
                p_CustomerName = string.IsNullOrEmpty(p_CustomerName) ? null : p_CustomerName;

                globalAdminTempData = GetTempData();

                if (p_IsNext != null)
                {
                    if (p_IsNext == true)
                    {
                        if (globalAdminTempData.fliq_CustomerApplicationHasMoreRecords == true)
                        {
                            globalAdminTempData.fliq_CustomerApplicationPageNumber = globalAdminTempData.fliq_CustomerApplicationPageNumber + 1;
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
                        if (globalAdminTempData.fliq_CustomerApplicationPageNumber > 0)
                        {
                            globalAdminTempData.fliq_CustomerApplicationPageNumber = globalAdminTempData.fliq_CustomerApplicationPageNumber - 1;
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
                    globalAdminTempData.fliq_CustomerApplicationHasMoreRecords = true;
                    globalAdminTempData.fliq_CustomerApplicationPageNumber = 0;
                }

                int totalResults = 0;
                int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CustomerApplicationPageSize"]);
                fliq_CustomerApplicationLogic fliq_CustomerApplicationLogic = (fliq_CustomerApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_CustomerApplication);
                List<fliQ_CustomerApplicationModel> lstfliQ_CustomerApplicationModel = fliq_CustomerApplicationLogic.GetAllfliq_CustomerApplication(p_ClientName,p_CustomerName, globalAdminTempData.fliq_CustomerApplicationPageNumber, pageSize, p_IsAsc,p_SortColumn , out totalResults);

                if (totalResults > ((globalAdminTempData.fliq_CustomerApplicationPageNumber + 1) * pageSize))
                {
                    globalAdminTempData.fliq_CustomerApplicationHasMoreRecords = true;
                }
                else
                {
                    globalAdminTempData.fliq_CustomerApplicationHasMoreRecords = false;
                }

                string strHTML = RenderPartialToString(PATH_GloblaAdminFliq_CustomerApplicationListPartialView, lstfliQ_CustomerApplicationModel);

                string strRecordLabel = " ";
                if (lstfliQ_CustomerApplicationModel.Count > 0)
                {
                    strRecordLabel = "" + ((globalAdminTempData.fliq_CustomerApplicationPageNumber * pageSize) + 1).ToString() + " - " + ((globalAdminTempData.fliq_CustomerApplicationPageNumber * pageSize) + lstfliQ_CustomerApplicationModel.Count).ToString() + " Of " + totalResults.ToString() + "";
                }
                SetTempData(globalAdminTempData);

                return Json(new
                {
                    isSuccess = true,
                    hasMoreRecords = globalAdminTempData.fliq_CustomerApplicationHasMoreRecords,
                    hasPreviousRecords = globalAdminTempData.fliq_CustomerApplicationPageNumber > 0 ? true : false,
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
        public JsonResult GetFliq_CustomerApplicationRegistration(Int64 p_ID)
        {
            try
            {
                fliq_CustomerApplicationLogic fliq_CustomerApplicationLogic = (fliq_CustomerApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_CustomerApplication);

                fliQ_CustomerApplicationPostModel objfliQ_CustomerApplicationPostModel = new fliQ_CustomerApplicationPostModel();
                if (p_ID == 0)
                {
                    objfliQ_CustomerApplicationPostModel.customerApplication = new fliQ_CustomerApplicationModel();
                    objfliQ_CustomerApplicationPostModel.CustomerApplication_DropDown = fliq_CustomerApplicationLogic.GetClientApplication_Dropdowns(true, null);
                }
                else
                {
                    objfliQ_CustomerApplicationPostModel.customerApplication = fliq_CustomerApplicationLogic.Getfliq_CustomerApplicationByID(p_ID);
                    objfliQ_CustomerApplicationPostModel.CustomerApplication_DropDown = fliq_CustomerApplicationLogic.GetClientApplication_Dropdowns(true, objfliQ_CustomerApplicationPostModel.customerApplication.ClientID);
                }

                return Json(new
                {
                    isSuccess = true,
                    HTML = RenderPartialToString(PATH_GloblaAdminFliq_CustomerApplicationRegistationPartialView, objfliQ_CustomerApplicationPostModel)
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
        public JsonResult GetFliq_CustomerApplicationDropDowns(Int64 p_ClientID)
        {
            try
            {
                fliq_CustomerApplicationLogic fliq_CustomerApplicationLogic = (fliq_CustomerApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_CustomerApplication);

                CustomerApplication_DropDown CustomerApplication_DropDown = fliq_CustomerApplicationLogic.GetClientApplication_Dropdowns(false, p_ClientID == 0 ? null : (Int64?)p_ClientID);

                return Json(new
                {
                    isSuccess = true,
                    Fliq_Customers = CustomerApplication_DropDown.CustomerList,
                    Fliq_Application = CustomerApplication_DropDown.ApplicationList,
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
        public JsonResult Fliq_CustomerApplicationRegistration(fliQ_CustomerApplicationPostModel p_CustomerApplication)
        {
            try
            {
                string ErrorMessage = string.Empty;
                string _Result = string.Empty;
                fliq_CustomerApplicationLogic fliq_CustomerApplicationLogic = (fliq_CustomerApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_CustomerApplication);


                if (p_CustomerApplication.customerApplication.ID == 0)
                {
                    p_CustomerApplication.customerApplication.IsActive = p_CustomerApplication.customerApplication.IsActive.HasValue ? p_CustomerApplication.customerApplication.IsActive : false;

                    _Result = fliq_CustomerApplicationLogic.Insertfliq_CustomerApplication(p_CustomerApplication.customerApplication);
                    if (_Result == "0" || _Result == "")
                    {
                        ErrorMessage = _CustomerApplicationExistMessage;
                    }
                    else if (_Result == "-1")
                    {
                        ErrorMessage = _CustomerCanNotFliq;
                    }
                }
                else
                {
                    p_CustomerApplication.customerApplication.IsActive = p_CustomerApplication.customerApplication.IsActive.HasValue ? p_CustomerApplication.customerApplication.IsActive : false;

                    _Result = fliq_CustomerApplicationLogic.Updatefliq_CustomerApplication(p_CustomerApplication.customerApplication);
                    if (_Result == "0" || _Result == "")
                    {
                        ErrorMessage = _CustomerApplicationExistMessage;
                    }
                    else if (_Result == "-1")
                    {
                        ErrorMessage = _CustomerCanNotFliq;
                    }
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
        public JsonResult DeleteFliq_CustomerApplication(Int64 p_ID)
        {
            try
            {
                fliq_CustomerApplicationLogic fliq_CustomerApplicationLogic = (fliq_CustomerApplicationLogic)LogicFactory.GetLogic(LogicType.fliq_CustomerApplication);
                fliq_CustomerApplicationLogic.Deletefliq_CustomerApplication(p_ID);
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

    }
}


