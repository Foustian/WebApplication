using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class fliq_CustomerApplicationDA : IDataAccess
    {
        public string Insertfliq_CustomerApplication(fliQ_CustomerApplicationModel p_CustomerApplication)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int64, p_CustomerApplication.CustomerID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ApplicationID", DbType.Int64, p_CustomerApplication.FliqApplicationID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_CustomerApplication.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Status", DbType.Int64, 0, ParameterDirection.Output));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_fliQ_CustomerApplication_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string Updatefliq_CustomerApplication(fliQ_CustomerApplicationModel p_CustomerApplication)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, p_CustomerApplication.ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int64, p_CustomerApplication.CustomerID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ApplicationID", DbType.Int64, p_CustomerApplication.FliqApplicationID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_CustomerApplication.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Status", DbType.Int64, 0, ParameterDirection.Output));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_fliQ_CustomerApplication_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string Deletefliq_CustomerApplication(Int64 p_ID)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_fliQ_CustomerApplication_Delete", _ListOfDataType);

                return _Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public fliQ_CustomerApplicationModel Getfliq_CustomerApplicationByID(Int64 p_ID)
        {
            try
            {
                fliQ_CustomerApplicationModel objfliQ_CustomerApplicationModel = new fliQ_CustomerApplicationModel();

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_fliQ_CustomerApplication_SelectByID", dataTypeList);

                List<fliQ_CustomerApplicationModel> lstfliQ_CustomerApplicationModel = Fillfliq_CustomerApplication(dataset);

                if (lstfliQ_CustomerApplicationModel.Count > 0)
                {
                    objfliQ_CustomerApplicationModel = lstfliQ_CustomerApplicationModel[0];
                }
                return objfliQ_CustomerApplicationModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CustomerApplication_DropDown GetClientApplication_Dropdowns(bool p_isFetchClient,Int64? p_ClientID)
        {
            try
            {
                CustomerApplication_DropDown objCustomerApplication_DropDown = new CustomerApplication_DropDown();

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@IsFetchClient", DbType.Boolean, p_isFetchClient, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientID", DbType.Int64, p_ClientID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_fliQ_CustomerApplication_SelectAllDropdown", dataTypeList);

                if (p_isFetchClient)
                {
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
                        objCustomerApplication_DropDown.ClientList = lstClientModel;
                    }    
                }

                DataTable dbApplication = null;
                if (!p_isFetchClient && dataset.Tables.Count > 0)
                {
                    dbApplication = dataset.Tables[0];
                }
                else if (p_isFetchClient && dataset.Tables.Count > 1)
                {
                    dbApplication = dataset.Tables[1];
                }
                

                if (dbApplication != null)
                {
                    List<fliQ_ApplicationModel> lstfliQ_ApplicationModel = new List<fliQ_ApplicationModel>();
                    foreach (DataRow dr in dbApplication.Rows)
                    {
                        fliQ_ApplicationModel objfliQ_ApplicationModel = new fliQ_ApplicationModel();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objfliQ_ApplicationModel.ID = Convert.ToInt32(dr["ID"]);
                        }

                        if (!dr["Application"].Equals(DBNull.Value))
                        {
                            objfliQ_ApplicationModel.Application = Convert.ToString(dr["Application"]);
                        }
                        lstfliQ_ApplicationModel.Add(objfliQ_ApplicationModel);
                    }
                    objCustomerApplication_DropDown.ApplicationList = lstfliQ_ApplicationModel;
                }

                DataTable dbCustomers = null;
                if (!p_isFetchClient && dataset.Tables.Count > 1)
                {
                    dbCustomers = dataset.Tables[1];
                }
                else if (p_isFetchClient && dataset.Tables.Count > 2)
                {
                    dbCustomers = dataset.Tables[2];
                }
                
                if (dbCustomers != null)
                {
                    List<fliq_CustomerModel> lstfliq_CustomerModel = new List<fliq_CustomerModel>();
                    foreach (DataRow dr in dbCustomers.Rows)
                    {
                        fliq_CustomerModel objfliq_CustomerModel = new fliq_CustomerModel();
                        if (!dr["CustomerKey"].Equals(DBNull.Value))
                        {
                            objfliq_CustomerModel.CustomerKey = Convert.ToInt32(dr["CustomerKey"]);
                        }

                        if (!dr["FirstName"].Equals(DBNull.Value))
                        {
                            objfliq_CustomerModel.FirstName = Convert.ToString(dr["FirstName"]);
                        }

                        if (!dr["LastName"].Equals(DBNull.Value))
                        {
                            objfliq_CustomerModel.LastName = Convert.ToString(dr["LastName"]);
                        }
                        lstfliq_CustomerModel.Add(objfliq_CustomerModel);
                    }
                    objCustomerApplication_DropDown.CustomerList = lstfliq_CustomerModel;
                }


                return objCustomerApplication_DropDown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<fliQ_CustomerApplicationModel> GetAllfliq_CustomerApplication(string p_ClientName, string p_CustomerName, int p_PageNumner, int p_PageSize, bool p_IsAsc, string p_SortColumn, out int p_TotalResults)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                p_TotalResults = 0;
                dataTypeList.Add(new DataType("@ClientName", DbType.String, p_ClientName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerName", DbType.String, p_CustomerName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageNumner", DbType.Int16, p_PageNumner, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int16, p_PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsAsc", DbType.Boolean, p_IsAsc, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SortColumn", DbType.String, p_SortColumn, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, p_TotalResults, ParameterDirection.Output));

                Dictionary<string, string> _Output;
                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v4_fliQ_CustomerApplication_SelectAll", dataTypeList, out  _Output);

                if (_Output != null && _Output.Count > 0)
                {
                    p_TotalResults = !string.IsNullOrWhiteSpace(_Output["@TotalResults"]) ? Convert.ToInt32(_Output["@TotalResults"]) : 0;
                }

                List<fliQ_CustomerApplicationModel> lstfliQ_CustomerApplicationModel = Fillfliq_CustomerApplication(dataset);

                return lstfliQ_CustomerApplicationModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<fliQ_CustomerApplicationModel> Fillfliq_CustomerApplication(DataSet dataSet)
        {
            List<fliQ_CustomerApplicationModel> lstffliQ_CustomerApplicationModel = new List<fliQ_CustomerApplicationModel>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    fliQ_CustomerApplicationModel objfliQ_CustomerApplicationModel = new fliQ_CustomerApplicationModel();
                    if (dataSet.Tables[0].Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                    {
                        objfliQ_CustomerApplicationModel.ID = Convert.ToInt64(dr["ID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("ClientKey") && !dr["ClientKey"].Equals(DBNull.Value))
                    {
                        objfliQ_CustomerApplicationModel.ClientID = Convert.ToInt64(dr["ClientKey"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("CustomerKey") && !dr["CustomerKey"].Equals(DBNull.Value))
                    {
                        objfliQ_CustomerApplicationModel.CustomerID = Convert.ToInt64(dr["CustomerKey"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("FirstName") && !dr["FirstName"].Equals(DBNull.Value))
                    {
                        objfliQ_CustomerApplicationModel.CustomerFirstName = Convert.ToString(dr["FirstName"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("LastName") && !dr["LastName"].Equals(DBNull.Value))
                    {
                        objfliQ_CustomerApplicationModel.CustomerLastName = Convert.ToString(dr["LastName"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("_FliqApplicationID") && !dr["_FliqApplicationID"].Equals(DBNull.Value))
                    {
                        objfliQ_CustomerApplicationModel.FliqApplicationID = Convert.ToInt64(dr["_FliqApplicationID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Application") && !dr["Application"].Equals(DBNull.Value))
                    {
                        objfliQ_CustomerApplicationModel.Application = Convert.ToString(dr["Application"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("IsActive") && !dr["IsActive"].Equals(DBNull.Value))
                    {
                        objfliQ_CustomerApplicationModel.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }

                    lstffliQ_CustomerApplicationModel.Add(objfliQ_CustomerApplicationModel);
                }

            }
            return lstffliQ_CustomerApplicationModel;
        }

        public List<fliQ_UploadTrackingModel> Getfliq_UplaodsByClientGuid(Guid p_ClientGuid, int p_PageNumner, int p_PageSize,bool p_IsAsc,string p_SortColumn, out int p_TotalResults)
        {
            try
            {
                p_TotalResults = 0;
                fliQ_CustomerApplicationModel objfliQ_CustomerApplicationModel = new fliQ_CustomerApplicationModel();

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageNumner", DbType.Int16, p_PageNumner, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int16, p_PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsAsc", DbType.Boolean, p_IsAsc, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SortColumn", DbType.String, p_SortColumn, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, p_TotalResults, ParameterDirection.Output));

                Dictionary<string, string> _Output;
                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v4_fliQ_UploadTracking_SelectByClientGuid", dataTypeList, out  _Output);

                if (_Output != null && _Output.Count > 0)
                {
                    p_TotalResults = !string.IsNullOrWhiteSpace(_Output["@TotalResults"]) ? Convert.ToInt32(_Output["@TotalResults"]) : 0;
                }

                List<fliQ_UploadTrackingModel> lstfliQ_UploadTrackingModel = FillfliQ_UploadTracking(dataset);


                return lstfliQ_UploadTrackingModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<fliQ_UploadTrackingModel> FillfliQ_UploadTracking(DataSet dataSet)
        {
            List<fliQ_UploadTrackingModel> lstfliQ_UploadTrackingModel = new List<fliQ_UploadTrackingModel>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    fliQ_UploadTrackingModel objfliQ_UploadTrackingModel = new fliQ_UploadTrackingModel();
                    if (dataSet.Tables[0].Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                    {
                        objfliQ_UploadTrackingModel.ID = Convert.ToInt64(dr["ID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("UploadedDateTime") && !dr["UploadedDateTime"].Equals(DBNull.Value))
                    {
                        objfliQ_UploadTrackingModel.UploadedDateTime = Convert.ToDateTime(dr["UploadedDateTime"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("FirstName") && !dr["FirstName"].Equals(DBNull.Value))
                    {
                        objfliQ_UploadTrackingModel.CustomerFirstName = Convert.ToString(dr["FirstName"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("LastName") && !dr["LastName"].Equals(DBNull.Value))
                    {
                        objfliQ_UploadTrackingModel.CustomerLastName = Convert.ToString(dr["LastName"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Status") && !dr["Status"].Equals(DBNull.Value))
                    {
                        objfliQ_UploadTrackingModel.Status = Convert.ToString(dr["Status"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Tags") && !dr["Tags"].Equals(DBNull.Value))
                    {
                        objfliQ_UploadTrackingModel.Tags = Convert.ToString(dr["Tags"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                    {
                        objfliQ_UploadTrackingModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                    }

                    lstfliQ_UploadTrackingModel.Add(objfliQ_UploadTrackingModel);
                }

            }
            return lstfliQ_UploadTrackingModel;
        }
    }
}
