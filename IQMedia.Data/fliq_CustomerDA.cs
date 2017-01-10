using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data.Base;
using System.Data;

namespace IQMedia.Data
{
    public class fliq_CustomerDA : IDataAccess
    {
        public string Insertfliq_Customer(fliq_CustomerModel p_Customer, string p_DefaultCategory)
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
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, p_Customer.ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Comments", DbType.String, p_Customer.Comment, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CreatedBy", DbType.String, p_Customer.CreatedBy, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_Customer.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultCategory", DbType.String, p_DefaultCategory, ParameterDirection.Input));


                _Result = DataAccess.ExecuteNonQuery("usp_v4_fliQ_Customer_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string Updatefliq_Customer(fliq_CustomerModel p_Customer)
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
                _ListOfDataType.Add(new DataType("@ModifiedDate", DbType.DateTime, p_Customer.ModifiedDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Comments", DbType.String, p_Customer.Comment, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, p_Customer.ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.String, p_Customer.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Status", DbType.Int32, 0, ParameterDirection.Output));


                _Result = DataAccess.ExecuteNonQuery("usp_v4_fliQ_Customer_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string Deletefliq_Customer(Int64 p_CustomerKey)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerKey", DbType.Int64, p_CustomerKey, ParameterDirection.Input));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_fliq_Customer_Delete", _ListOfDataType);

                return _Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public fliq_CustomerModel Gefliq_CustomerByCustomerID(Int64 p_CustomerID)
        {
            try
            {
                fliq_CustomerModel objCustomerModel = new fliq_CustomerModel();

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@CustomerID", DbType.Int64, p_CustomerID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_fliQ_Customer_SelectByCustomerKey", dataTypeList);

                List<fliq_CustomerModel> lstCustomerModel = Fillfliq_Customer(dataset);

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
                objCustomer_DropDown.Customer_MasterList = new List<CustomerModel>();
                objCustomer_DropDown.ClientList = new List<ClientModel>();

                List<DataType> dataTypeList = new List<DataType>();

                DataSet dataset = DataAccess.GetDataSet("usp_v4_fliq_Customer_SelectAllDropDown", dataTypeList);

                if (dataset != null && dataset.Tables.Count > 0)
                {
                    List<ClientModel> lstClientModel = new List<ClientModel>();
                    foreach (DataRow dr in dataset.Tables[0].Rows)
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

        public List<fliq_CustomerModel> GetAllfliq_Customer(string p_ClientName, string p_CustomerName, int p_PageNumner, int p_PageSize, out int p_TotalResults)
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
                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v4_fliq_Customer_SelectAll", dataTypeList, out  _Output);

                if (_Output != null && _Output.Count > 0)
                {
                    p_TotalResults = !string.IsNullOrWhiteSpace(_Output["@TotalResults"]) ? Convert.ToInt32(_Output["@TotalResults"]) : 0;
                }

                List<fliq_CustomerModel> lstCustomerModel = Fillfliq_Customer(dataset);

                return lstCustomerModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<fliq_CustomerModel> Fillfliq_Customer(DataSet dataSet)
        {
            List<fliq_CustomerModel> lstCustomerModel = new List<fliq_CustomerModel>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    fliq_CustomerModel objCustomerModel = new fliq_CustomerModel();
                    if (dataSet.Tables[0].Columns.Contains("CustomerKey") && !dr["CustomerKey"].Equals(DBNull.Value))
                    {
                        objCustomerModel.CustomerKey = Convert.ToInt32(dr["CustomerKey"]);
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

                    if (dataSet.Tables[0].Columns.Contains("CustomerPassword") && !dr["CustomerPassword"].Equals(DBNull.Value))
                    {
                        objCustomerModel.Password = Convert.ToString(dr["CustomerPassword"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("CustomerComment") && !dr["CustomerComment"].Equals(DBNull.Value))
                    {
                        objCustomerModel.Comment = Convert.ToString(dr["CustomerComment"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("ClientID") && !dr["ClientID"].Equals(DBNull.Value))
                    {
                        objCustomerModel.ClientID = Convert.ToInt32(dr["ClientID"]);
                    }
                    
                    if (dataSet.Tables[0].Columns.Contains("IsActive") && !dr["IsActive"].Equals(DBNull.Value))
                    {
                        objCustomerModel.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("LoginID") && !dr["LoginID"].Equals(DBNull.Value))
                    {
                        objCustomerModel.LoginID = Convert.ToString(dr["LoginID"]);
                    }

                    lstCustomerModel.Add(objCustomerModel);
                }

            }
            return lstCustomerModel;
        }
    }
}
