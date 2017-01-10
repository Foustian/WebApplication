using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class fliq_ApplicationDA : IDataAccess
    {
        public string Insertfliq_Application(fliQ_ApplicationModel p_Application)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@Application", DbType.String, p_Application.Application, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Version", DbType.String, p_Application.Version, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Path", DbType.String, p_Application.Path, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, p_Application.Description, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_Application.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ApplicationID", DbType.Int64, 0, ParameterDirection.Output));


                _Result = DataAccess.ExecuteNonQuery("usp_v4_fliQ_Application_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string Updatefliq_Application(fliQ_ApplicationModel p_Application)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@Application", DbType.String, p_Application.Application, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Version", DbType.String, p_Application.Version, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Path", DbType.String, p_Application.Path, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, p_Application.Description, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_Application.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ApplicationID", DbType.Int64, p_Application.ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Status", DbType.Int32, 0, ParameterDirection.Output));


                _Result = DataAccess.ExecuteNonQuery("usp_v4_fliQ_Application_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string Deletefliq_Application(Int64 p_ApplicationID)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ApplicationID", DbType.Int64, p_ApplicationID, ParameterDirection.Input));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_fliQ_Application_Delete", _ListOfDataType);

                return _Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public fliQ_ApplicationModel Getliq_ApplicationByID(Int64 p_ApplicationID)
        {
            try
            {
                fliQ_ApplicationModel objfliQ_ApplicationModel = new fliQ_ApplicationModel();

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ApplicationID", DbType.Int64, p_ApplicationID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_fliQ_Application_SelectByID", dataTypeList);

                List<fliQ_ApplicationModel> lstfliQ_ApplicationModel = Fillfliq_Application(dataset);

                if (lstfliQ_ApplicationModel.Count > 0)
                {
                    objfliQ_ApplicationModel = lstfliQ_ApplicationModel[0];
                }
                return objfliQ_ApplicationModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<fliQ_ApplicationModel> GetAllfliq_Application(string p_Application, int p_PageNumner, int p_PageSize, out int p_TotalResults)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                p_TotalResults = 0;
                dataTypeList.Add(new DataType("@Application", DbType.String, p_Application, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageNumner", DbType.Int16, p_PageNumner, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int16, p_PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, p_TotalResults, ParameterDirection.Output));

                Dictionary<string, string> _Output;
                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v4_fliQ_Application_SelectAll", dataTypeList, out  _Output);

                if (_Output != null && _Output.Count > 0)
                {
                    p_TotalResults = !string.IsNullOrWhiteSpace(_Output["@TotalResults"]) ? Convert.ToInt32(_Output["@TotalResults"]) : 0;
                }

                List<fliQ_ApplicationModel> lstfliQ_ApplicationModel = Fillfliq_Application(dataset);

                return lstfliQ_ApplicationModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<fliQ_ApplicationModel> Fillfliq_Application(DataSet dataSet)
        {
            List<fliQ_ApplicationModel> lstfliQ_ApplicationModel = new List<fliQ_ApplicationModel>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    fliQ_ApplicationModel objfliQ_ApplicationModel = new fliQ_ApplicationModel();
                    if (dataSet.Tables[0].Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                    {
                        objfliQ_ApplicationModel.ID = Convert.ToInt32(dr["ID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Application") && !dr["Application"].Equals(DBNull.Value))
                    {
                        objfliQ_ApplicationModel.Application = Convert.ToString(dr["Application"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Version") && !dr["Version"].Equals(DBNull.Value))
                    {
                        objfliQ_ApplicationModel.Version = Convert.ToString(dr["Version"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Path") && !dr["Path"].Equals(DBNull.Value))
                    {
                        objfliQ_ApplicationModel.Path = Convert.ToString(dr["Path"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Description") && !dr["Description"].Equals(DBNull.Value))
                    {
                        objfliQ_ApplicationModel.Description = Convert.ToString(dr["Description"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("DateCreated") && !dr["DateCreated"].Equals(DBNull.Value))
                    {
                        objfliQ_ApplicationModel.DateCreated = Convert.ToDateTime(dr["DateCreated"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("DateModified") && !dr["DateModified"].Equals(DBNull.Value))
                    {
                        objfliQ_ApplicationModel.DateModified = Convert.ToDateTime(dr["DateModified"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("IsActive") && !dr["IsActive"].Equals(DBNull.Value))
                    {
                        objfliQ_ApplicationModel.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }

                    lstfliQ_ApplicationModel.Add(objfliQ_ApplicationModel);
                }

            }
            return lstfliQ_ApplicationModel;
        }
    }
}
