using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class JobStatusDA : IDataAccess
    {
        public List<JobStatusModel> GetJobStatusByClientGuid(Guid p_ClientGuid, int p_PageNumner, int p_PageSize, bool p_IsAsc, string p_SortColumn, int? p_JobTypeID, out int p_TotalResults)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                p_TotalResults = 0;
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageNumner", DbType.Int16, p_PageNumner, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int16, p_PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsAsc", DbType.Boolean, p_IsAsc, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SortColumn", DbType.String, p_SortColumn, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@JobTypeID", DbType.String, p_JobTypeID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, p_TotalResults, ParameterDirection.Output));

                Dictionary<string, string> _Output;
                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v4_JobStatus_SelectByClientGUID", dataTypeList, out  _Output);

                if (_Output != null && _Output.Count > 0)
                {
                    p_TotalResults = !string.IsNullOrWhiteSpace(_Output["@TotalResults"]) ? Convert.ToInt32(_Output["@TotalResults"]) : 0;
                }

                List<JobStatusModel> lstJobStatusModel = FillJobStatus(dataset);

                return lstJobStatusModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JobStatusModel> FillJobStatus(DataSet dataSet)
        {
            List<JobStatusModel> lstJobStatusModel = new List<JobStatusModel>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    JobStatusModel objJobStatusModel = new JobStatusModel();
                    if (dataSet.Tables[0].Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                    {
                        objJobStatusModel.ID = Convert.ToInt64(dr["ID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Name") && !dr["Name"].Equals(DBNull.Value))
                    {
                        objJobStatusModel.Name = Convert.ToString(dr["Name"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Description") && !dr["Description"].Equals(DBNull.Value))
                    {
                        objJobStatusModel.Description = Convert.ToString(dr["Description"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("_Title") && !dr["_Title"].Equals(DBNull.Value))
                    {
                        objJobStatusModel.Title = Convert.ToString(dr["_Title"]);
                    }
                    else
                    {
                        objJobStatusModel.Title = "N/A";
                    }

                    if (dataSet.Tables[0].Columns.Contains("_DownloadPath") && !dr["_DownloadPath"].Equals(DBNull.Value))
                    {
                        objJobStatusModel.DownloadPath = Convert.ToString(dr["_DownloadPath"]);
                    }
                    else
                    {
                        objJobStatusModel.DownloadPath = "N/A";
                    }

                    if (dataSet.Tables[0].Columns.Contains("Status") && !dr["Status"].Equals(DBNull.Value))
                    {
                        objJobStatusModel.Status = Convert.ToString(dr["Status"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("_RequestedDateTime") && !dr["_RequestedDateTime"].Equals(DBNull.Value))
                    {
                        objJobStatusModel.RequestedDateTime = Convert.ToDateTime(dr["_RequestedDateTime"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("_CompletedDateTime") && !dr["_CompletedDateTime"].Equals(DBNull.Value))
                    {
                        objJobStatusModel.CompletedDateTime = Convert.ToDateTime(dr["_CompletedDateTime"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("FirstName") && !dr["FirstName"].Equals(DBNull.Value))
                    {
                        objJobStatusModel.CustomerFirstName = Convert.ToString(dr["FirstName"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("LastName") && !dr["LastName"].Equals(DBNull.Value))
                    {
                        objJobStatusModel.CustomerLastName = Convert.ToString(dr["LastName"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("RequestID") && !dr["RequestID"].Equals(DBNull.Value))
                    {
                        objJobStatusModel.RequestID = Convert.ToInt64(dr["RequestID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("CanReset") && !dr["CanReset"].Equals(DBNull.Value))
                    {
                        objJobStatusModel.CanReset = Convert.ToBoolean(dr["CanReset"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("ResetProcedureName") && !dr["ResetProcedureName"].Equals(DBNull.Value))
                    {
                        objJobStatusModel.ResetProcedureName = Convert.ToString(dr["ResetProcedureName"]);
                    }

                    lstJobStatusModel.Add(objJobStatusModel);
                }

            }
            return lstJobStatusModel;
        }

        public string SelectJobStatusDownload_SelectByID(long p_ID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));

                string _Result = Convert.ToString(DataAccess.ExecuteScalar("usp_v4_JobStatus_Download_SelectByID", dataTypeList));

                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<JobType> GetJobTypeList()
        {
            try
            {
                DataSet dataset = DataAccess.GetDataSetByProcedure("usp_v4_JobType_SelectAll");

                List<JobType> lstJobRequestName = FillJobRequestName(dataset);

                return lstJobRequestName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JobType> FillJobRequestName(DataSet dataSet)
        {
            List<JobType> lstJobType = new List<JobType>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    JobType objJobType = new JobType();
                    if (dataSet.Tables[0].Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                    {
                        objJobType.ID = Convert.ToInt32(dr["ID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Description") && !dr["Description"].Equals(DBNull.Value))
                    {
                        objJobType.Description = Convert.ToString(dr["Description"]);
                    }

                    lstJobType.Add(objJobType);
                }

            }
            return lstJobType;
        }

        public bool ResetJob(long ID, long requestID, string resetProcedureName)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@RequestID", DbType.Int64, requestID, ParameterDirection.Input));

                short result = Convert.ToInt16(DataAccess.ExecuteScalar(resetProcedureName, dataTypeList));
                return result == 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
