using IQMedia.Model;
using System.Collections.Generic;
using IQMedia.Data.Base;
using System.Data;
using System;
using System.Linq;
using System.Xml.Linq;

namespace IQMedia.Data
{
    public class CustomerDA : IDataAccess
    {
        public string InsertCustomer(CustomerModel p_Customer, string p_Roles, string p_DefaultCategory)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerKey", DbType.Int64, p_Customer.CustomerKey, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@FirstName", DbType.String, p_Customer.FirstName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@LastName", DbType.String, p_Customer.LastName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Email", DbType.String, p_Customer.Email, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@LoginID", DbType.String, p_Customer.LoginID, ParameterDirection.Input));
                //_ListOfDataType.Add(new DataType("@MasterCustomerID", DbType.Int64, p_Customer.MasterCustomerID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerPassword", DbType.String, p_Customer.Password, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Guid, p_Customer.CustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ContactNo", DbType.String, p_Customer.ContactNo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsMultiLogin", DbType.Boolean, p_Customer.MultiLogin, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, p_Customer.ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultPage", DbType.String, p_Customer.DefaultPage, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Comments", DbType.String, p_Customer.Comment, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CreatedBy", DbType.String, p_Customer.CreatedBy, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerRoles", DbType.Xml, p_Roles, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_Customer.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsFliqCustomer", DbType.Boolean, p_Customer.IsFliqCustomer, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultCategory", DbType.String, p_DefaultCategory, ParameterDirection.Input));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_Customer_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string UpdateCustomer(CustomerModel p_Customer, string p_Roles, string p_DefaultCategory)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerKey", DbType.Int32, p_Customer.CustomerKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FirstName", DbType.String, p_Customer.FirstName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@LastName", DbType.String, p_Customer.LastName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Email", DbType.String, p_Customer.Email, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@LoginID", DbType.String, p_Customer.LoginID, ParameterDirection.Input));
                //_ListOfDataType.Add(new DataType("@MasterCustomerID", DbType.Int64, p_Customer.MasterCustomerID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerPassword", DbType.String, p_Customer.Password, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ContactNo", DbType.String, p_Customer.ContactNo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsMultiLogin", DbType.Boolean, p_Customer.MultiLogin, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultPage", DbType.String, p_Customer.DefaultPage, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ModifiedDate", DbType.DateTime, p_Customer.ModifiedDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Comments", DbType.String, p_Customer.Comment, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, p_Customer.ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.String, p_Customer.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerRoles", DbType.String, p_Roles, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsFliqCustomer", DbType.Boolean, p_Customer.IsFliqCustomer, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultCategory", DbType.String, p_DefaultCategory, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Status", DbType.Int32, 0, ParameterDirection.Output));


                _Result = DataAccess.ExecuteNonQuery("usp_v4_Customer_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string DeleteCustomer(Int64 p_CustomerKey)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerKey", DbType.Int64, p_CustomerKey, ParameterDirection.Input));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_Customer_Delete", _ListOfDataType);

                return _Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CustomerModel GetCustomerWithRoleByCustomerID(Int64 p_CustomerID)
        {
            try
            {
                CustomerModel objCustomerModel = new CustomerModel();

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@CustomerID", DbType.Int64, p_CustomerID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_Customer_SelectCustomerWithRoleByCustomerID", dataTypeList);

                List<CustomerModel> lstCustomerModel = FillCustomerWithRole(dataset);

                if (lstCustomerModel.Count > 0)
                {
                    objCustomerModel = lstCustomerModel[0];
                }
                return objCustomerModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Customer_DropDown GetAllDropDown()
        {
            try
            {
                Customer_DropDown objCustomer_DropDown = new Customer_DropDown();
                objCustomer_DropDown.Customer_RoleList = new List<RoleModel>();
                objCustomer_DropDown.Customer_MasterList = new List<CustomerModel>();
                objCustomer_DropDown.ClientList = new List<ClientModel>();

                List<DataType> dataTypeList = new List<DataType>();

                DataSet dataset = DataAccess.GetDataSet("usp_v4_Customer_SelectAllDropDown", dataTypeList);

                if (dataset != null && dataset.Tables.Count > 0)
                {
                    List<CustomerModel> lstCustomerModel = new List<CustomerModel>();
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        CustomerModel objCustomerModel = new CustomerModel();
                        if (!dr["CustomerKey"].Equals(DBNull.Value))
                        {
                            objCustomerModel.CustomerKey = Convert.ToInt32(dr["CustomerKey"]);
                        }

                        if (!dr["LoginID"].Equals(DBNull.Value))
                        {
                            objCustomerModel.LoginID = Convert.ToString(dr["LoginID"]);
                        }
                        lstCustomerModel.Add(objCustomerModel);
                    }
                    objCustomer_DropDown.Customer_MasterList = lstCustomerModel;
                }

                if (dataset != null && dataset.Tables.Count > 1)
                {
                    List<RoleModel> lstRoleModel = new List<RoleModel>();
                    foreach (DataRow dr in dataset.Tables[1].Rows)
                    {
                        RoleModel objRoleModel = new RoleModel();
                        if (!dr["RoleKey"].Equals(DBNull.Value))
                        {
                            objRoleModel.RoleKey = Convert.ToInt32(dr["RoleKey"]);
                        }

                        if (!dr["RoleName"].Equals(DBNull.Value))
                        {
                            objRoleModel.RoleName = Convert.ToString(dr["RoleName"]);
                        }

                        if (!dr["UIName"].Equals(DBNull.Value))
                        {
                            objRoleModel.DisplayName = Convert.ToString(dr["UIName"]);
                        }

                        if (!dr["Description"].Equals(DBNull.Value))
                        {
                            objRoleModel.Description = Convert.ToString(dr["Description"]);
                        }

                        if (!dr["IsEnabledInSetup"].Equals(DBNull.Value))
                        {
                            objRoleModel.IsEnabledInSetup = Convert.ToBoolean(dr["IsEnabledInSetup"]);
                        }

                        if (!dr["EnabledCustomerIDs"].Equals(DBNull.Value))
                        {
                            objRoleModel.EnabledCustomerIDs = Convert.ToString(dr["EnabledCustomerIDs"]).Split(',').ToList();
                        }

                        if (!dr["GroupName"].Equals(DBNull.Value))
                        {
                            objRoleModel.GroupName = Convert.ToString(dr["GroupName"]);
                        }

                        if (!dr["HasDefaultAccess"].Equals(DBNull.Value))
                        {
                            objRoleModel.HasDefaultAccess = Convert.ToBoolean(dr["HasDefaultAccess"]);
                        }
                        lstRoleModel.Add(objRoleModel);
                    }
                    objCustomer_DropDown.Customer_RoleList = lstRoleModel;
                }

                if (dataset != null && dataset.Tables.Count > 2)
                {
                    List<ClientModel> lstClientModel = new List<ClientModel>();
                    foreach (DataRow dr in dataset.Tables[2].Rows)
                    {
                        ClientModel objClientModel = new ClientModel();
                        if (!dr["ClientKey"].Equals(DBNull.Value))
                        {
                            objClientModel.ClientKey = Convert.ToInt32(dr["ClientKey"]);
                        }

                        if (!dr["ClientName"].Equals(DBNull.Value))
                        {
                            objClientModel.ClientName = Convert.ToString(dr["ClientName"]);
                        }

                        if (!dr["IsFliq"].Equals(DBNull.Value))
                        {
                            objClientModel.IsFliq = Convert.ToBoolean(dr["IsFliq"]);
                        }
                        lstClientModel.Add(objClientModel);
                    }
                    objCustomer_DropDown.ClientList = lstClientModel;
                }

                return objCustomer_DropDown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<_CustomerModel> GetAllCustomers()
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                IDataReader reader = DataAccess.GetDataReader("usp_Customer_SelectAll", dataTypeList);
                List<_CustomerModel> customerList = new List<_CustomerModel>();

                while (reader.Read())
                {
                    _CustomerModel customer = new _CustomerModel();

                    customer.FirstName = Convert.ToString(reader["FirstName"]);
                    customer.LastName = Convert.ToString(reader["LastName"]);

                    customerList.Add(customer);
                }

                return customerList.AsEnumerable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public CustomerModel CheckAuthentication(string email, string password)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@Email", DbType.String, email, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerPassword", DbType.String, password, ParameterDirection.Input));
                IDataReader reader = DataAccess.GetDataReader("usp_v4_Customer_CheckAuthentication", dataTypeList);
                List<CustomerModel> customerList = new List<CustomerModel>();
                CustomerModel customer = new CustomerModel();

                while (reader.Read())
                {
                    customer.CustomerKey = Convert.ToInt32(reader["CustomerKey"]);
                    customer.ClientID = Convert.ToInt32(reader["ClientID"]);
                    customer.ClientName = Convert.ToString(reader["ClientName"]);
                    customer.FirstName = Convert.ToString(reader["FirstName"]);
                    customer.LastName = Convert.ToString(reader["LastName"]);
                    customer.Email = Convert.ToString(reader["Email"]);
                    customer.LoginID = Convert.ToString(reader["LoginID"]);
                    customer.MultiLogin = Convert.ToBoolean(reader["MultiLogin"]);
                    customer.CustomerGUID = new Guid(Convert.ToString(reader["CustomerGUID"]));
                    customer.ClientGUID = new Guid(Convert.ToString(reader["ClientGUID"]));
                    customer.ClientPlayerLogoImage = Convert.ToString(reader["PlayerLogo"]);
                    customer.IsClientPlayerLogoActive = Convert.ToBoolean(reader["IsActivePlayerLogo"]);
                    customer.DefaultPage = Convert.ToString(reader["DefaultPage"]);
                    customer.AuthorizedVersion = !string.IsNullOrWhiteSpace(Convert.ToString(reader["AuthorizedVersion"])) ? Convert.ToInt16(reader["AuthorizedVersion"]) : (Int16?)null;
                    customer.TimeZone = Convert.ToString(reader["TimeZone"]);
                    customer.gmt = Convert.ToDecimal(reader["gmt"]);
                    customer.dst = Convert.ToDecimal(reader["dst"]);

                    if (reader["MCID"] != DBNull.Value)
                    {
                        customer.MCID = Convert.ToInt32(reader["MCID"]);
                    }

                    if (reader["MasterCustomerID"] != DBNull.Value)
                    {
                        customer.MasterCustomerID = Convert.ToInt32(reader["MasterCustomerID"]);
                    }

                }

                return customer;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public CustomerModel CheckAuthenticationByClient(Int64 masterCustomerId, Int64 clientId)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@MasterCustomerID", DbType.Int64, masterCustomerId, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientID", DbType.Int64, clientId, ParameterDirection.Input));
                IDataReader reader = DataAccess.GetDataReader("usp_v4_Customer_CheckAuthenticationByMasterClient", dataTypeList);
                List<CustomerModel> customerList = new List<CustomerModel>();
                CustomerModel customer = new CustomerModel();

                while (reader.Read())
                {
                    customer.CustomerKey = Convert.ToInt32(reader["CustomerKey"]);
                    customer.ClientID = Convert.ToInt32(reader["ClientID"]);
                    customer.ClientName = Convert.ToString(reader["ClientName"]);
                    customer.FirstName = Convert.ToString(reader["FirstName"]);
                    customer.LastName = Convert.ToString(reader["LastName"]);
                    customer.Email = Convert.ToString(reader["Email"]);
                    customer.LoginID = Convert.ToString(reader["LoginID"]);

                    if (reader["MultiLogin"] != DBNull.Value)
                    {
                        customer.MultiLogin = Convert.ToBoolean(reader["MultiLogin"]);
                    }

                    customer.CustomerGUID = new Guid(Convert.ToString(reader["CustomerGUID"]));
                    customer.ClientGUID = new Guid(Convert.ToString(reader["ClientGUID"]));
                    customer.ClientPlayerLogoImage = Convert.ToString(reader["PlayerLogo"]);

                    if (reader["MasterCustomerID"] != DBNull.Value)
                    {
                        customer.MasterCustomerID = Convert.ToInt32(reader["MasterCustomerID"]);
                    }

                    if (reader["IsActivePlayerLogo"] != DBNull.Value)
                    {
                        customer.IsClientPlayerLogoActive = Convert.ToBoolean(reader["IsActivePlayerLogo"]);
                    }

                    if (reader["DefaultPage"] != DBNull.Value)
                    {
                        customer.DefaultPage = Convert.ToString(reader["DefaultPage"]);
                    }

                    if (reader["AuthorizedVersion"] != DBNull.Value)
                    {
                        customer.AuthorizedVersion = !string.IsNullOrWhiteSpace(Convert.ToString(reader["AuthorizedVersion"])) ? Convert.ToInt16(reader["AuthorizedVersion"]) : (Int16?)null;
                    }

                    if (reader["TimeZone"] != DBNull.Value)
                    {
                        customer.TimeZone = Convert.ToString(reader["TimeZone"]);
                    }

                    if (reader["gmt"] != DBNull.Value)
                    {
                        customer.gmt = Convert.ToDecimal(reader["gmt"]);
                    }

                    if (reader["dst"] != DBNull.Value)
                    {
                        customer.dst = Convert.ToDecimal(reader["dst"]);
                    }

                    if (reader["MCID"] != DBNull.Value)
                    {
                        customer.MCID = Convert.ToInt32(reader["MCID"]);
                    }

                    if (reader["CustomerPassword"] != DBNull.Value)
                    {
                        customer.Password = Convert.ToString(reader["CustomerPassword"]);
                    }


                }

                return customer;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public CustomerModel GetClientGUIDByCustomerGUID(Guid p_CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
                IDataReader reader = DataAccess.GetDataReader("usp_v4_Client_SelectByCustomerGUID", dataTypeList);
                List<CustomerModel> customerList = new List<CustomerModel>();
                CustomerModel customer = new CustomerModel();

                while (reader.Read())
                {
                    customer.CustomerKey = Convert.ToInt32(reader["CustomerKey"]);
                    customer.ClientID = Convert.ToInt32(reader["ClientID"]);
                    customer.ClientName = Convert.ToString(reader["ClientName"]);
                    customer.FirstName = Convert.ToString(reader["FirstName"]);
                    customer.LastName = Convert.ToString(reader["LastName"]);
                    customer.Email = Convert.ToString(reader["Email"]);
                    customer.MultiLogin = Convert.ToBoolean(reader["MultiLogin"]);
                    customer.CustomerGUID = new Guid(Convert.ToString(reader["CustomerGUID"]));
                    customer.TimeZone = Convert.ToString(reader["TimeZone"]);
                    customer.gmt = Convert.ToDecimal(reader["gmt"]);
                    customer.dst = Convert.ToDecimal(reader["dst"]);

                    if (reader["MCID"] != DBNull.Value)
                    {
                        customer.MCID = Convert.ToInt32(reader["MCID"]);
                    }

                    if (reader["LoginID"] != DBNull.Value)
                    {
                        customer.LoginID = Convert.ToString(reader["LoginID"]);
                    }

                    if (reader["MasterCustomerID"] != DBNull.Value)
                    {
                        customer.MasterCustomerID = Convert.ToInt32(reader["MasterCustomerID"]);
                    }

                    customer.ClientGUID = new Guid(Convert.ToString(reader["ClientGUID"]));
                    customer.ClientPlayerLogoImage = Convert.ToString(reader["PlayerLogo"]);
                    customer.IsClientPlayerLogoActive = Convert.ToBoolean(reader["IsActivePlayerLogo"]);
                    customer.DefaultPage = Convert.ToString(reader["DefaultPage"]);
                    customer.AuthorizedVersion = !string.IsNullOrWhiteSpace(Convert.ToString(reader["AuthorizedVersion"])) ? Convert.ToInt16(reader["AuthorizedVersion"]) : (Int16?)null;
                }

                return customer;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<CustomerModel> GetCustomerDetailByEmailList(XDocument p_EmailXML)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@LoginIDXML", DbType.Xml, p_EmailXML.ToString(), ParameterDirection.Input));
                IDataReader reader = DataAccess.GetDataReader("usp_v4_GetCustomerDetailByLoginIDList", dataTypeList);
                List<CustomerModel> customerList = new List<CustomerModel>();

                while (reader.Read())
                {
                    CustomerModel customer = new CustomerModel();
                    customer.FirstName = Convert.ToString(reader["FirstName"]);
                    customer.LastName = Convert.ToString(reader["LastName"]);
                    customer.LoginID = Convert.ToString(reader["LoginID"]);
                    customerList.Add(customer);
                }

                return customerList;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<CustomerRoleModel> GetCustomerRoles(Guid CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, CustomerGuid, ParameterDirection.Input));

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_Role_SelectRoleByCustomerGUID", dataTypeList);

                List<CustomerRoleModel> customerRoleList = new List<CustomerRoleModel>();

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        CustomerRoleModel customerRole = new CustomerRoleModel();

                        if (!dr["RoleKey"].Equals(DBNull.Value))
                        {
                            customerRole.RoleID = Convert.ToInt32(dr["RoleKey"]);
                        }
                        if (!dr["RoleName"].Equals(DBNull.Value))
                        {
                            customerRole.RoleName = Convert.ToString(dr["RoleName"]);
                        }
                        if (!dr["CustomerKey"].Equals(DBNull.Value))
                        {
                            customerRole.CustomerID = Convert.ToInt32(dr["CustomerKey"]);
                        }
                        customerRoleList.Add(customerRole);
                    }
                }

                return customerRoleList;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public Boolean GetDownloadRoleByCustomerGuid(Guid p_CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));
                DataSet ds = DataAccess.GetDataSet("usp_v4_Role_SelectDownloadRoleByCustomerGUID", dataTypeList);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CustomerModel> GetAllCustomerWithRole(string p_ClientName, string p_CustomerName, int p_PageNumner, int p_PageSize, out int p_TotalResults)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                p_TotalResults = 0;
                dataTypeList.Add(new DataType("@ClientName", DbType.String, p_ClientName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerName", DbType.String, p_CustomerName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageNumner", DbType.Int16, p_PageNumner, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int16, p_PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, p_TotalResults, ParameterDirection.Output));

                Dictionary<string, string> _Output;
                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v4_Customer_SelectAllCustomerWithRole", dataTypeList, out  _Output);

                if (_Output != null && _Output.Count > 0)
                {
                    p_TotalResults = !string.IsNullOrWhiteSpace(_Output["@TotalResults"]) ? Convert.ToInt32(_Output["@TotalResults"]) : 0;
                }

                List<CustomerModel> lstCustomerModel = FillCustomerWithRole(dataset);

                return lstCustomerModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerModel> FillCustomerWithRole(DataSet dataSet)
        {
            List<CustomerModel> lstCustomerModel = new List<CustomerModel>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    CustomerModel objCustomerModel = new CustomerModel();
                    if (dataSet.Tables[0].Columns.Contains("CustomerKey") && !dr["CustomerKey"].Equals(DBNull.Value))
                    {
                        objCustomerModel.CustomerKey = Convert.ToInt32(dr["CustomerKey"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("AnewstipUserID") && !dr["AnewstipUserID"].Equals(DBNull.Value))
                    {
                        objCustomerModel.AnewstipUserID = Convert.ToString(dr["AnewstipUserID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("FirstName") && !dr["FirstName"].Equals(DBNull.Value))
                    {
                        objCustomerModel.FirstName = Convert.ToString(dr["FirstName"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("LastName") && !dr["LastName"].Equals(DBNull.Value))
                    {
                        objCustomerModel.LastName = Convert.ToString(dr["LastName"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Email") && !dr["Email"].Equals(DBNull.Value))
                    {
                        objCustomerModel.Email = Convert.ToString(dr["Email"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("ContactNo") && !dr["ContactNo"].Equals(DBNull.Value))
                    {
                        objCustomerModel.ContactNo = Convert.ToString(dr["ContactNo"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("CustomerComment") && !dr["CustomerComment"].Equals(DBNull.Value))
                    {
                        objCustomerModel.Comment = Convert.ToString(dr["CustomerComment"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("ClientID") && !dr["ClientID"].Equals(DBNull.Value))
                    {
                        objCustomerModel.ClientID = Convert.ToInt32(dr["ClientID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("MultiLogin") && !dr["MultiLogin"].Equals(DBNull.Value))
                    {
                        objCustomerModel.MultiLogin = Convert.ToBoolean(dr["MultiLogin"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("IsActive") && !dr["IsActive"].Equals(DBNull.Value))
                    {
                        objCustomerModel.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("DefaultPage") && !dr["DefaultPage"].Equals(DBNull.Value))
                    {
                        objCustomerModel.DefaultPage = Convert.ToString(dr["DefaultPage"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                    {
                        objCustomerModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("MasterCustomerID") && !dr["MasterCustomerID"].Equals(DBNull.Value))
                    {
                        objCustomerModel.MasterCustomerID = Convert.ToInt32(dr["MasterCustomerID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("LoginID") && !dr["LoginID"].Equals(DBNull.Value))
                    {
                        objCustomerModel.LoginID = Convert.ToString(dr["LoginID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("IsFliqCustomer") && !dr["IsFliqCustomer"].Equals(DBNull.Value))
                    {
                        objCustomerModel.IsFliqCustomer = Convert.ToBoolean(dr["IsFliqCustomer"]);
                    }

                    objCustomerModel.CustomerRoles = new Dictionary<string, bool>();
                    foreach (DataColumn dc in dataSet.Tables[0].Columns)
                    {
                        if (dc.ColumnName != "CustomerKey" && dc.ColumnName != "AnewstipUserID" && dc.ColumnName != "FirstName" && dc.ColumnName != "LastName"
                               && dc.ColumnName != "Email" && dc.ColumnName != "ContactNo" && dc.ColumnName != "IsActive"
                               && dc.ColumnName != "CustomerPassword" && dc.ColumnName != "CustomerComment" && dc.ColumnName != "ClientID"
                               && dc.ColumnName != "MultiLogin" && dc.ColumnName != "DefaultPage" && dc.ColumnName != "MasterCustomerID"
                               && dc.ColumnName != "LoginID" && dc.ColumnName != "CreatedDate" && dc.ColumnName != "IsFliqCustomer")
                        {
                            objCustomerModel.CustomerRoles.Add(dc.ColumnName, Convert.ToBoolean(dr[dc.ColumnName]));
                        }
                    }

                    lstCustomerModel.Add(objCustomerModel);
                }

            }
            return lstCustomerModel;
        }

        public CustomerModel GetCustomerDetailsForAuthentication(string p_LoginID)
        {
            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@LoginID", DbType.String, p_LoginID, ParameterDirection.Input));

            IDataReader reader = DataAccess.GetDataReader("usp_v4_Customer_SelectForAuthentication", dataTypeList);

            CustomerModel customer = null;

            while (reader.Read())
            {
                customer = new CustomerModel();

                customer.PasswordAttempts = reader["PasswordAttempts"] != DBNull.Value ? Convert.ToInt32(reader["PasswordAttempts"]) : 0;
                customer.Password = Convert.ToString(reader["CustomerPassword"]);
            }

            return customer;
        }

        public void UpdatePasswordAttempts(string p_LoginID, bool p_ResetPasswordAttempts)
        {
            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@LoginID", DbType.String, p_LoginID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ResetPasswordAttempts", DbType.Boolean, p_ResetPasswordAttempts, ParameterDirection.Input));

            DataAccess.ExecuteNonQuery("usp_v4_Customer_UpdatedPasswordAttempts", dataTypeList);
        }

        public CustomerModel GetCustomerDetailsByLoginID(string p_LoginID)
        {
            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@LoginID", DbType.String, p_LoginID, ParameterDirection.Input));

            IDataReader reader = DataAccess.GetDataReader("usp_v4_Customer_SelectByLoginID", dataTypeList);

            CustomerModel customer = new CustomerModel();

            while (reader.Read())
            {
                customer.CustomerKey = Convert.ToInt32(reader["CustomerKey"]);
                customer.ClientID = Convert.ToInt32(reader["ClientID"]);
                customer.ClientName = Convert.ToString(reader["ClientName"]);
                customer.FirstName = Convert.ToString(reader["FirstName"]);
                customer.LastName = Convert.ToString(reader["LastName"]);
                customer.Email = Convert.ToString(reader["Email"]);
                customer.LoginID = Convert.ToString(reader["LoginID"]);
                customer.MultiLogin = Convert.ToBoolean(reader["MultiLogin"]);
                customer.CustomerGUID = new Guid(Convert.ToString(reader["CustomerGUID"]));
                customer.ClientGUID = new Guid(Convert.ToString(reader["ClientGUID"]));
                customer.ClientPlayerLogoImage = Convert.ToString(reader["PlayerLogo"]);
                customer.IsClientPlayerLogoActive = Convert.ToBoolean(reader["IsActivePlayerLogo"]);
                customer.DefaultPage = Convert.ToString(reader["DefaultPage"]);
                customer.AuthorizedVersion = !string.IsNullOrWhiteSpace(Convert.ToString(reader["AuthorizedVersion"])) ? Convert.ToInt16(reader["AuthorizedVersion"]) : (Int16?)null;
                customer.TimeZone = Convert.ToString(reader["TimeZone"]);
                customer.gmt = Convert.ToDecimal(reader["gmt"]);
                customer.dst = Convert.ToDecimal(reader["dst"]);

                if (reader["MCID"] != DBNull.Value)
                {
                    customer.MCID = Convert.ToInt32(reader["MCID"]);
                }

                if (reader["MasterCustomerID"] != DBNull.Value)
                {
                    customer.MasterCustomerID = Convert.ToInt32(reader["MasterCustomerID"]);
                }

            }

            return customer;
        }

        public void UpdatePasswordByLoginID(string p_LoginID, string p_Pwd)
        {
            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@LoginID", DbType.String, p_LoginID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@Password", DbType.String, p_Pwd, ParameterDirection.Input));

            DataAccess.ExecuteNonQuery("usp_v4_Customer_UpdatedPassword", dataTypeList);
        }

        public bool ValidateLoginIDForRsetPwd(string p_LoginID, out short rsetPwdEmailCount, out string email)
        {
            bool isValid = false;
            rsetPwdEmailCount = 0;
            email = "";

            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@LoginID", DbType.String, p_LoginID, ParameterDirection.Input));

            var ds = DataAccess.GetDataSet("usp_v4_Customer_ValidateLoginID_RsetPwd", dataTypeList);

            isValid = Convert.ToBoolean(ds.Tables[0].Rows[0]["ISValid"]);
            rsetPwdEmailCount = Convert.ToInt16(ds.Tables[0].Rows[0]["RsetPwdEmailCount"]);
            email = Convert.ToString(ds.Tables[0].Rows[0]["Email"]);

            return isValid;
        }

        public Int64 InsertRsetPwd(string p_LoginID, DateTime dateExpired, string p_Token)
        {
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@Token", DbType.String, p_Token, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@LoginID", DbType.String, p_LoginID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@DateExpired", DbType.DateTime2, dateExpired, ParameterDirection.Input));

            return Convert.ToInt64(DataAccess.ExecuteScalar("usp_v4_IQCustomer_RsetPwd_Insert", dataTypeList));
        }

        public void UpdateRsetPwdEmailCount(string p_LoginID)
        {
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@LoginID", DbType.String, p_LoginID, ParameterDirection.Input));

            Convert.ToInt64(DataAccess.ExecuteNonQuery("usp_v4_Customer_UpdateRsetPwdEmailCount", dataTypeList));
        }

        public CustomerRsetPwdModel GetRsetPwd(string p_LoginID)
        {
            CustomerRsetPwdModel custRsetPwd = null;

            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@LoginID", DbType.String, p_LoginID, ParameterDirection.Input));

            var ds = DataAccess.GetDataSet("usp_v4_IQCustomer_RsetPwd_SelectByLoginID", dataTypeList);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                custRsetPwd = new CustomerRsetPwdModel();
                var drow = ds.Tables[0].Rows[0];

                custRsetPwd.ID = Convert.ToInt64(drow["ID"]);
                custRsetPwd.CustomerGUID = Guid.Parse(Convert.ToString(drow["_CustomerGUID"]));
                custRsetPwd.Token = Convert.ToString(drow["Token"]);
                custRsetPwd.DateExpired = Convert.ToDateTime(drow["DateExpired"]);
                custRsetPwd.IsActive = Convert.ToBoolean(drow["IsActive"]);
                custRsetPwd.IsUsed = Convert.ToBoolean(drow["IsUsed"]);
            }

            return custRsetPwd;
        }

        public void UpdateRsetPwdNPassword(long p_ID, Guid p_CustomerGUID, string p_Pwd)
        {
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@Password", DbType.String, p_Pwd, ParameterDirection.Input));

            Convert.ToInt64(DataAccess.ExecuteNonQuery("usp_v4_IQCustomer_RsetPwd_UpdatePassword", dataTypeList));
        }

        public int ResetPasswordAttempts(long p_CustomerKey)
        {
            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@CustomerKey", DbType.Int64, p_CustomerKey, ParameterDirection.Input));
            return Convert.ToInt32(DataAccess.ExecuteScalar("usp_v5_Customer_ResetPasswordAttempts", dataTypeList));
        }

        public bool GroupAddSubCustomer(Int64 p_GrpID, Int64 p_MCID, Int64 p_SCID, Int64 p_MasterCustomerID, Int64 p_SubCustomerID, Guid p_CustomerGUID)
        {
            var output = false;

            List<DataType> dtL = new List<DataType>();

            dtL.Add(new DataType("@GroupID", DbType.Int64, p_GrpID, ParameterDirection.Input));
            dtL.Add(new DataType("@MCID", DbType.Int64, p_MCID, ParameterDirection.Input));
            dtL.Add(new DataType("@MasterCustomerID", DbType.Int64, p_MasterCustomerID, ParameterDirection.Input));
            dtL.Add(new DataType("@SCID", DbType.Int64, p_SCID, ParameterDirection.Input));
            dtL.Add(new DataType("@SubCustomerID", DbType.Int64, p_SubCustomerID, ParameterDirection.Input));
            dtL.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
            dtL.Add(new DataType("@Output", DbType.Boolean, output, ParameterDirection.Output));

            output = Convert.ToBoolean(DataAccess.ExecuteNonQuery("usp_V5_Group_Customer_AddSubCustomer", dtL));

            return output;
        }

        public List<CustomerModel> GroupGetSubCustomerByCustomer(Int64 p_MasterCustomerID)
        {
            List<DataType> dtL = new List<DataType>();            

            dtL.Add(new DataType("@MasterCustomerID", DbType.Int64, p_MasterCustomerID, ParameterDirection.Input));            

            var ds = DataAccess.GetDataSet("usp_V5_Group_Customer_SelectSubCustomer", dtL);

            List<CustomerModel> custList = new List<CustomerModel>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CustomerModel cust = new CustomerModel();

                    cust.CustomerKey = Convert.ToInt32(dr["CustomerKey"]);
                    cust.FirstName = Convert.ToString(dr["FirstName"]);
                    cust.LastName = Convert.ToString(dr["LastName"]);
                    cust.ClientID = Convert.ToInt32(dr["ClientID"]);
                    cust.LoginID = Convert.ToString(dr["LoginID"]);

                    custList.Add(cust);
                }
            }

            return custList;
        }

        public bool GroupRemoveSubCustomer(Int64 p_MasterCustomerID, Int64 p_SubCustomerID, Guid p_CustomerGUID)
        {
            List<DataType> dtL = new List<DataType>();

            dtL.Add(new DataType("@MasterCustomerID", DbType.Int64, p_MasterCustomerID, ParameterDirection.Input));
            dtL.Add(new DataType("@SubCustomerID", DbType.Int64, p_SubCustomerID, ParameterDirection.Input));
            dtL.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));

            var output = DataAccess.ExecuteNonQuery("usp_V5_Group_Customer_RemoveSubCustomer", dtL);

            return true;
        }

        public void AddCustomerToAnewstip(long customerKey, string AnewstipUserID)
        {
            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@CustomerKey", DbType.Int64, customerKey, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@AnewstipUserID", DbType.String, AnewstipUserID, ParameterDirection.Input));
            DataAccess.ExecuteNonQuery("usp_v5_Customer_AddToAnewstip", dataTypeList);
        }
    }
}
