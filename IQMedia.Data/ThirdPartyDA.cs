using System;
using System.Collections.Generic;
using System.Data;
using IQMedia.Data.Base;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class ThirdPartyDA : IDataAccess
    {
        public List<ThirdPartyDataTypeModel> GetThirdPartyDataTypesWithCustomerSelection(Guid customerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, customerGuid, ParameterDirection.Input));
                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQ3rdP_DataTypes_SelectWithCustomer", dataTypeList);

                List<ThirdPartyDataTypeModel> lstDataTypes = new List<ThirdPartyDataTypeModel>();

                if (dsSSP != null && dsSSP.Tables.Count > 0)
                {
                    foreach (DataRow datarow in dsSSP.Tables[0].Rows)
                    {
                        ThirdPartyDataTypeModel dataType = new ThirdPartyDataTypeModel();
                        dataType.ID = Convert.ToInt32(datarow["ID"]);
                        dataType.DataType = Convert.ToString(datarow["DataType"]);
                        dataType.DisplayName = Convert.ToString(datarow["DisplayName"]);
                        dataType.YAxisID = Convert.ToInt32(datarow["YAxisID"]);
                        dataType.YAxisName = Convert.ToString(datarow["YAxisName"]);
                        dataType.SPName = Convert.ToString(datarow["SPName"]);
                        dataType.IsAgentSpecific = Convert.ToBoolean(datarow["IsAgentSpecific"]);
                        dataType.UseHourData = Convert.ToBoolean(datarow["UseHourData"]);
                        dataType.UseIDParam = Convert.ToBoolean(datarow["UseIDParam"]);
                        dataType.SeriesLineType = Convert.ToString(datarow["SeriesLineType"]);
                        dataType.GroupID = Convert.ToInt32(datarow["GroupID"]);
                        dataType.GroupName = Convert.ToString(datarow["GroupName"]);
                        dataType.IsSelected = Convert.ToBoolean(datarow["IsSelected"]);
                        dataType.HasAccess = Convert.ToBoolean(datarow["HasAccess"]);

                        lstDataTypes.Add(dataType);
                    }
                }

                return lstDataTypes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SaveThirdPartyDataTypeSelections(Guid customerGuid, string dataTypeIDXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, customerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DataTypeIDXml", DbType.Xml, dataTypeIDXml, ParameterDirection.Input));
                object result = DataAccess.ExecuteScalar("usp_v5_IQ3rdP_DataTypes_SaveSelection", dataTypeList);

                return Convert.ToInt32(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SummaryReportModel> GetThirdPartySummaryData(Guid clientGuid, ThirdPartyDataTypeModel dataTypeModel, DateTime fromDate, DateTime toDate, int dateIntervalType, string searchRequestIDXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.DateTime, fromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.DateTime, toDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DateIntervalType", DbType.Int16, dateIntervalType, ParameterDirection.Input));
                if (dataTypeModel.IsAgentSpecific)
                {
                    dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, searchRequestIDXml, ParameterDirection.Input));
                }
                if (dataTypeModel.UseIDParam)
                {
                    dataTypeList.Add(new DataType("@DataTypeID", DbType.Int32, dataTypeModel.ID, ParameterDirection.Input));
                }
                DataSet dsSSP = DataAccess.GetDataSet(dataTypeModel.SPName, dataTypeList);

                return FillThirdPartySummaryData(dsSSP, dataTypeModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SummaryReportModel> FillThirdPartySummaryData(DataSet dataSet, ThirdPartyDataTypeModel dataTypeModel)
        {
            List<SummaryReportModel> lstSummaryReportModel = new List<SummaryReportModel>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow datarow in dataSet.Tables[0].Rows)
                {
                    SummaryReportModel summaryReportModel = new SummaryReportModel();
                    summaryReportModel.ThirdPartyDataTypeID = dataTypeModel.ID;

                    if (!datarow["GMTDateTime"].Equals(DBNull.Value))
                    {
                        summaryReportModel.GMT_DateTime = Convert.ToDateTime(datarow["GMTDateTime"]);
                    }

                    if (!datarow["TotalValue"].Equals(DBNull.Value))
                    {
                        summaryReportModel.Number_Docs = Convert.ToInt64(datarow["TotalValue"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("SearchRequestID") && !datarow["SearchRequestID"].Equals(DBNull.Value))
                    {
                        summaryReportModel.SearchRequestID = Convert.ToInt64(datarow["SearchRequestID"]);
                    }

                    lstSummaryReportModel.Add(summaryReportModel);
                }
            }

            return lstSummaryReportModel;
        }
    }
}
