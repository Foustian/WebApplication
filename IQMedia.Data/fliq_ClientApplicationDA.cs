using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class fliq_ClientApplicationDA : IDataAccess
    {
        public string Insertfliq_ClientApplication(fliQ_ClientApplicationModel p_ClientApplication)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, p_ClientApplication.ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ApplicationID", DbType.Int64, p_ClientApplication.FliqApplicationID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGuid", DbType.Guid, p_ClientApplication.DefaultCategory, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FTPHost", DbType.String, p_ClientApplication.FTPHost, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FTPPath", DbType.String, p_ClientApplication.FTPPath, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FTPLoginID", DbType.String, p_ClientApplication.FTPLoginID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FTPPwd", DbType.String, p_ClientApplication.FTPPwd, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MaxVideoDuration", DbType.Int32, p_ClientApplication.MaxVideoDuration, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ForceLandscape", DbType.Boolean, p_ClientApplication.IsLandscapeOnly, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_ClientApplication.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsCategoryEnable", DbType.Boolean, p_ClientApplication.IsCategoryEnable, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, 0, ParameterDirection.Output));


                _Result = DataAccess.ExecuteNonQuery("usp_v4_fliQ_ClientApplication_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string Updatefliq_ClientApplication(fliQ_ClientApplicationModel p_ClientApplication)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, p_ClientApplication.ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, p_ClientApplication.ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ApplicationID", DbType.Int64, p_ClientApplication.FliqApplicationID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGuid", DbType.Guid, p_ClientApplication.DefaultCategory, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FTPHost", DbType.String, p_ClientApplication.FTPHost, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FTPPath", DbType.String, p_ClientApplication.FTPPath, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FTPLoginID", DbType.String, p_ClientApplication.FTPLoginID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FTPPwd", DbType.String, p_ClientApplication.FTPPwd, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MaxVideoDuration", DbType.Int32, p_ClientApplication.MaxVideoDuration, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ForceLandscape", DbType.Boolean, p_ClientApplication.IsLandscapeOnly, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_ClientApplication.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsCategoryEnable", DbType.Boolean, p_ClientApplication.IsCategoryEnable, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Status", DbType.Int64, 0, ParameterDirection.Output));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_fliQ_ClientApplication_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string Deletefliq_ClientApplication(Int64 p_ID)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_fliQ_ClientApplication_Delete", _ListOfDataType);

                return _Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public fliQ_ClientApplicationModel Getliq_ClientApplicationByID(Int64 p_ID)
        {
            try
            {
                fliQ_ClientApplicationModel objfliQ_ClientApplicationModel = new fliQ_ClientApplicationModel();

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_fliQ_ClientApplication_SelectByID", dataTypeList);

                List<fliQ_ClientApplicationModel> lstfliQ_ClientApplicationModel = Fillfliq_ClientApplication(dataset);

                if (lstfliQ_ClientApplicationModel.Count > 0)
                {
                    objfliQ_ClientApplicationModel = lstfliQ_ClientApplicationModel[0];
                }
                return objfliQ_ClientApplicationModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ClientApplication_DropDown GetClientApplication_Dropdowns()
        {
            try
            {
                ClientApplication_DropDown objClientApplication_DropDown = new ClientApplication_DropDown();
                objClientApplication_DropDown.ApplicationList = new List<fliQ_ApplicationModel>();
                objClientApplication_DropDown.ClientList = new List<ClientModel>();

                List<DataType> dataTypeList = new List<DataType>();

                DataSet dataset = DataAccess.GetDataSet("usp_v4_fliQ_ClientApplication_SelectAllDropDown", dataTypeList);

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
                    objClientApplication_DropDown.ClientList = lstClientModel;
                }

                if (dataset != null && dataset.Tables.Count > 1)
                {
                    List<fliQ_ApplicationModel> lstfliQ_ApplicationModel = new List<fliQ_ApplicationModel>();
                    foreach (DataRow dr in dataset.Tables[1].Rows)
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
                    objClientApplication_DropDown.ApplicationList = lstfliQ_ApplicationModel;
                }

                return objClientApplication_DropDown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<fliQ_ClientApplicationModel> GetAllfliq_ClientApplication(string p_ClientName,string p_ApplicationName, int p_PageNumner, int p_PageSize, out int p_TotalResults)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                p_TotalResults = 0;
                dataTypeList.Add(new DataType("@ClientName", DbType.String, p_ClientName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ApplicationName", DbType.String, p_ApplicationName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageNumner", DbType.Int16, p_PageNumner, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int16, p_PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, p_TotalResults, ParameterDirection.Output));

                Dictionary<string, string> _Output;
                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v4_fliQ_ClientApplication_SelectAll", dataTypeList, out  _Output);

                if (_Output != null && _Output.Count > 0)
                {
                    p_TotalResults = !string.IsNullOrWhiteSpace(_Output["@TotalResults"]) ? Convert.ToInt32(_Output["@TotalResults"]) : 0;
                }

                List<fliQ_ClientApplicationModel> lstfliQ_ClientApplicationModel = Fillfliq_ClientApplication(dataset);

                return lstfliQ_ClientApplicationModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<fliQ_ClientApplicationModel> Fillfliq_ClientApplication(DataSet dataSet)
        {
            List<fliQ_ClientApplicationModel> lstffliQ_ClientApplicationModel = new List<fliQ_ClientApplicationModel>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    fliQ_ClientApplicationModel objfliQ_ClientApplicationModel = new fliQ_ClientApplicationModel();
                    if (dataSet.Tables[0].Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.ID = Convert.ToInt64(dr["ID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("ClientKey") && !dr["ClientKey"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.ClientID = Convert.ToInt64(dr["ClientKey"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("_FliqApplicationID") && !dr["_FliqApplicationID"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.FliqApplicationID = Convert.ToInt64(dr["_FliqApplicationID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("FTPHost") && !dr["FTPHost"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.FTPHost = Convert.ToString(dr["FTPHost"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("FTPPath") && !dr["FTPPath"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.FTPPath = Convert.ToString(dr["FTPPath"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("FTPLoginID") && !dr["FTPLoginID"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.FTPLoginID = Convert.ToString(dr["FTPLoginID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("FTPPwd") && !dr["FTPPwd"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.FTPPwd = Convert.ToString(dr["FTPPwd"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Application") && !dr["Application"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.Application = Convert.ToString(dr["Application"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("ClientName") && !dr["ClientName"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.ClientName = Convert.ToString(dr["ClientName"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("DefaultCategory") && !dr["DefaultCategory"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.DefaultCategory = new Guid(Convert.ToString(dr["DefaultCategory"]));
                    }

                    if (dataSet.Tables[0].Columns.Contains("IsActive") && !dr["IsActive"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("IsCategoryEnable") && !dr["IsCategoryEnable"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.IsCategoryEnable = Convert.ToBoolean(dr["IsCategoryEnable"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("ForceLandscape") && !dr["ForceLandscape"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.IsLandscapeOnly = Convert.ToBoolean(dr["ForceLandscape"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("MaxVideoDuration") && !dr["MaxVideoDuration"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.MaxVideoDuration = Convert.ToInt32(dr["MaxVideoDuration"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Description") && !dr["Description"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.ApplicationDescription = Convert.ToString(dr["Description"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Path") && !dr["Path"].Equals(DBNull.Value))
                    {
                        objfliQ_ClientApplicationModel.DownloadPath = Convert.ToString(dr["Path"]);
                    }

                    lstffliQ_ClientApplicationModel.Add(objfliQ_ClientApplicationModel);
                }

            }
            return lstffliQ_ClientApplicationModel;
        }
    }
}
