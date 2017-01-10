using System;
using System.Collections.Generic;
using System.Data;
using IQMedia.Data.Base;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class GoogleDA : IDataAccess
    {
        public void UpdateAuthCode(Guid clientGuid, string authCode)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AuthCode", DbType.String, authCode, ParameterDirection.Input));
                DataAccess.ExecuteNonQuery("usp_v4_IQ_Google_UpdateAuthCode", dataTypeList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckClientAccess(Guid clientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                Dictionary<string, string> outParameter = null;
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@HasAccess", DbType.Boolean, false, ParameterDirection.Output));
                DataSet result = DataAccess.GetDataSetWithOutParam("usp_v4_IQ_Google_CheckClientAccess", dataTypeList, out outParameter);

                if (outParameter != null && outParameter.Count > 0)
                {
                    return !string.IsNullOrWhiteSpace(outParameter["@HasAccess"]) ? Convert.ToBoolean(outParameter["@HasAccess"]) : false;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetClientID()
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                DataSet result = DataAccess.GetDataSet("usp_v4_IQ_Google_GetClientID", dataTypeList);

                if (result.Tables.Count == 1)
                {
                    return Convert.ToString(result.Tables[0].Rows[0][0]);
                }
                return String.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<GoogleSummaryModel> GetGoogleDataByHour(Guid clientGuid, DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, fromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, toDate, ParameterDirection.Input));
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQDashboard_GoogleSummary_SelectByHour", dataTypeList);

                return FillGoogleSummaryResults(ds);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<GoogleSummaryModel> GetGoogleDataByDay(Guid clientGuid, DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, fromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, toDate, ParameterDirection.Input));
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQDashboard_GoogleSummary_SelectByDay", dataTypeList);

                return FillGoogleSummaryResults(ds);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<GoogleSummaryModel> GetGoogleDataByMonth(Guid clientGuid, DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, fromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, toDate, ParameterDirection.Input));
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQDashboard_GoogleSummary_SelectByMonth", dataTypeList);

                return FillGoogleSummaryResults(ds);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<GoogleSummaryModel> FillGoogleSummaryResults(DataSet dataSet)
        {
            List<GoogleSummaryModel> lstGoogleSummaryModel = new List<GoogleSummaryModel>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow datarow in dataSet.Tables[0].Rows)
                {
                    GoogleSummaryModel googleSummaryModel = new GoogleSummaryModel();

                    if (dataSet.Tables[0].Columns.Contains("DayDate") && !datarow["DayDate"].Equals(DBNull.Value))
                    {
                        googleSummaryModel.DayDate = Convert.ToDateTime(datarow["DayDate"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("NoOfDocs") && !datarow["NoOfDocs"].Equals(DBNull.Value))
                    {
                        googleSummaryModel.NoOfDocs = Convert.ToInt64(datarow["NoOfDocs"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("MediaType") && !datarow["MediaType"].Equals(DBNull.Value))
                    {
                        googleSummaryModel.MediaType = Convert.ToString(datarow["MediaType"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Query_Name"))
                    {
                        googleSummaryModel.DataType = Convert.ToString(datarow["Query_Name"]);
                    }

                    lstGoogleSummaryModel.Add(googleSummaryModel);
                }
            }

            return lstGoogleSummaryModel;
        }
    }
}
