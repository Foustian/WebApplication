using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class DashboardDA
    {
        public List<SummaryReportModel> GetSummaryReportData(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate)
        {

            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));

            Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
            DataSet dsSSP = DataAccess.GetDataSet("usp_v4_SummaryReport_Select", dataTypeList);

            List<SummaryReportModel> listOfSummaryReportData = new List<SummaryReportModel>();

            if (dsSSP != null && dsSSP.Tables.Count > 0)
            {
                foreach (DataRow datarow in dsSSP.Tables[0].Rows)
                {
                    SummaryReportModel dashboardSumRep = new SummaryReportModel();
                    dashboardSumRep.GMT_DateTime = Convert.ToDateTime(datarow["DayDate"]);
                    dashboardSumRep.Number_Docs = Convert.ToInt64(datarow["NoOfDocs"]);
                    dashboardSumRep.MediaType = Convert.ToString(datarow["MediaType"]);
                    dashboardSumRep.SubMediaType = Convert.ToString(datarow["SubMediaType"]);
                    dashboardSumRep.Number_Of_Hits = Convert.ToInt64(datarow["NoOfHits"]);
                    dashboardSumRep.IQMediaValue = Convert.ToDecimal(datarow["IQMediaValue"]);
                    dashboardSumRep.Audience = Convert.ToInt64(datarow["Audience"]);

                    listOfSummaryReportData.Add(dashboardSumRep);

                }

                //dicSummaryReport.Add("SummaryReport", listOfSummaryReportData);
            }
            return listOfSummaryReportData;

        }

        public List<IQAgent_DaySummaryModel> GetHourSummaryMediumWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));

                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_HourSummary_SelectByMedium", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<IQAgent_DaySummaryModel> GetDaySummaryMediumWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));

                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_DaySummary_SelectByMedium", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private List<IQAgent_DaySummaryModel> FillIQAgent_DaySummaryModel(DataSet ds)
        {
            try
            {
                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = new List<IQAgent_DaySummaryModel>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow datarow in ds.Tables[0].Rows)
                    {
                        IQAgent_DaySummaryModel iQAgent_DaySummaryModel = new IQAgent_DaySummaryModel();

                        if (ds.Tables[0].Columns.Contains("ClientGuid"))
                        {
                            iQAgent_DaySummaryModel.ClientGuid = new Guid(Convert.ToString(datarow["ClientGuid"]));
                        }
                        
                        if (ds.Tables[0].Columns.Contains("DayDate"))
                        {
                            iQAgent_DaySummaryModel.DayDate = Convert.ToDateTime(datarow["DayDate"]);
                        }
                        else if (ds.Tables[0].Columns.Contains("HourDateTime"))
                        {
                            iQAgent_DaySummaryModel.DayDate = Convert.ToDateTime(datarow["HourDateTime"]);
                        }

                        if (ds.Tables[0].Columns.Contains("Query_Name"))
                        {
                            iQAgent_DaySummaryModel.Query_Name = Convert.ToString(datarow["Query_Name"]);
                        }
                        else if (ds.Tables[0].Columns.Contains("DMA_Name"))
                        {
                            iQAgent_DaySummaryModel.Query_Name = Convert.ToString(datarow["DMA_Name"]);
                        }
                        else if (ds.Tables[0].Columns.Contains("Province"))
                        {
                            iQAgent_DaySummaryModel.Query_Name = Convert.ToString(datarow["Province"]);
                        }

                        if (ds.Tables[0].Columns.Contains("MediaType"))
                        {
                            iQAgent_DaySummaryModel.MediaType = Convert.ToString(datarow["MediaType"]);
                        }

                        if (ds.Tables[0].Columns.Contains("SubMediaType"))
                        {
                            iQAgent_DaySummaryModel.SubMediaType = Convert.ToString(datarow["SubMediaType"]);
                        }

                        if (ds.Tables[0].Columns.Contains("NoOfDocs"))
                        {
                            iQAgent_DaySummaryModel.NoOfDocs = Convert.ToInt64(datarow["NoOfDocs"]);
                        }

                        if (ds.Tables[0].Columns.Contains("NoOfHits"))
                        {
                            iQAgent_DaySummaryModel.NoOfHits = Convert.ToInt64(datarow["NoOfHits"]);
                        }

                        if (ds.Tables[0].Columns.Contains("Audience"))
                        {
                            iQAgent_DaySummaryModel.Audience = Convert.ToInt64(datarow["Audience"]);
                        }

                        if (ds.Tables[0].Columns.Contains("IQMediaValue"))
                        {
                            iQAgent_DaySummaryModel.IQMediaValue = Convert.ToDecimal(datarow["IQMediaValue"]);
                        }

                        if (ds.Tables[0].Columns.Contains("PositiveSentiment"))
                        {
                            iQAgent_DaySummaryModel.PositiveSentiment = Convert.ToInt64(datarow["PositiveSentiment"]);
                        }

                        if (ds.Tables[0].Columns.Contains("NegativeSentiment"))
                        {
                            iQAgent_DaySummaryModel.NegativeSentiment = Convert.ToInt64(datarow["NegativeSentiment"]);
                        }

                        lstIQAgent_DaySummaryModel.Add(iQAgent_DaySummaryModel);

                    }


                }
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public IQAgent_DashBoardModel GetDaySummaryDataDayWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_MediaTypeXml, string p_ListSPName)
        {

            IQAgent_DashBoardModel objIQAgent_DashBoardModel = new IQAgent_DashBoardModel();
            
            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));            
            dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@MediaTypeAccessXml", DbType.Xml, p_MediaTypeXml, ParameterDirection.Input));

            Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
            DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQAgent_DaySummary_SelectByDay", dataTypeList);

            objIQAgent_DashBoardModel = FillIQAgentSummary(dsSSP, p_Medium);

            // Can make both DB request parallely using Task. Or can make separate request for list separately, so chart response loads first.
            if (!string.IsNullOrEmpty(p_Medium) && !string.IsNullOrEmpty(p_ListSPName))
            {
                GetMediaTypeResults(objIQAgent_DashBoardModel, p_ClientGUID, p_FromDate, p_ToDate, p_Medium, p_SearchRequestXml, p_MediaTypeXml, p_ListSPName); 
            }

            return objIQAgent_DashBoardModel;

        }

        public IQAgent_DashBoardModel GetDaySummaryDataMonthWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_MediaTypeXml, string p_ListSPName)
        {
            IQAgent_DashBoardModel objIQAgent_DashBoardModel = new IQAgent_DashBoardModel();

            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));            
            dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@MediaTypeAccessXml", DbType.Xml, p_MediaTypeXml, ParameterDirection.Input));            

            Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
            DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQAgent_DaySummary_SelectByMonth", dataTypeList);

            objIQAgent_DashBoardModel = FillIQAgentSummary(dsSSP, p_Medium);

            if (!string.IsNullOrEmpty(p_Medium) && !string.IsNullOrEmpty(p_ListSPName))
            {
                GetMediaTypeResults(objIQAgent_DashBoardModel, p_ClientGUID, p_FromDate, p_ToDate, p_Medium, p_SearchRequestXml, p_MediaTypeXml, p_ListSPName);
            }

            return objIQAgent_DashBoardModel;

        }

        public IQAgent_DashBoardModel GetHourSummaryDataHourWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_MediaTypeXml, string p_ListSPName)
        {
            IQAgent_DashBoardModel objIQAgent_DashBoardModel = new IQAgent_DashBoardModel();

            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));            
            dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@MediaTypeAccessXml", DbType.Xml, p_MediaTypeXml, ParameterDirection.Input));

            Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
            DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQAgent_HourSummary_SelectByHour", dataTypeList);

            objIQAgent_DashBoardModel = FillIQAgentSummary(dsSSP, p_Medium);

            if (!string.IsNullOrEmpty(p_Medium) && !string.IsNullOrEmpty(p_ListSPName))
            {
                GetMediaTypeResults(objIQAgent_DashBoardModel, p_ClientGUID, p_FromDate, p_ToDate, p_Medium, p_SearchRequestXml, p_MediaTypeXml, p_ListSPName);
            }

            return objIQAgent_DashBoardModel;

        }        

        public IQAgent_DashBoardModel FillIQAgentSummary(DataSet dsSSP, string p_Medium)
        {

            IQAgent_DashBoardModel objIQAgent_DashBoardModel = new IQAgent_DashBoardModel();

            objIQAgent_DashBoardModel.ListOfIQAgentSummary = new List<IQAgent_DaySummaryModel>();

            if (dsSSP != null && dsSSP.Tables.Count > 0)
            {
                foreach (DataRow datarow in dsSSP.Tables[0].Rows)
                {
                    IQAgent_DaySummaryModel iQAgent_DaySummaryModel = new IQAgent_DaySummaryModel();

                    if (dsSSP.Tables[0].Columns.Contains("DayDate") && !datarow["DayDate"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.DayDate = Convert.ToDateTime(datarow["DayDate"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("NoOfDocs") && !datarow["NoOfDocs"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.NoOfDocs = Convert.ToInt64(datarow["NoOfDocs"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("MediaType") && !datarow["MediaType"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.MediaType = Convert.ToString(datarow["MediaType"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("SubMediaType") && !datarow["SubMediaType"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.SubMediaType = Convert.ToString(datarow["SubMediaType"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("NoOfHits") && !datarow["NoOfHits"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.NoOfHits = Convert.ToInt64(datarow["NoOfHits"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("IQMediaValue") && !datarow["IQMediaValue"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.IQMediaValue = Convert.ToDecimal(datarow["IQMediaValue"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("Audience") && !datarow["Audience"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.Audience = Convert.ToInt64(datarow["Audience"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("PositiveSentiment") && !datarow["PositiveSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.PositiveSentiment = Convert.ToInt64(datarow["PositiveSentiment"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("NegativeSentiment") && !datarow["NegativeSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.NegativeSentiment = Convert.ToInt64(datarow["NegativeSentiment"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("ID"))
                    {
                        iQAgent_DaySummaryModel.SearchRequestID = Convert.ToInt64(datarow["ID"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("Query_Name"))
                    {
                        iQAgent_DaySummaryModel.Query_Name = Convert.ToString(datarow["Query_Name"]);
                    }

                    objIQAgent_DashBoardModel.ListOfIQAgentSummary.Add(iQAgent_DaySummaryModel);
                }
            }

            if (dsSSP != null)
            {
                objIQAgent_DashBoardModel.PrevIQAgentSummary = new IQAgent_DashBoardPrevSummaryModel();
                DataTable dtPrevDashboardSummary = new DataTable();
                if (dsSSP.Tables.Count > 1 && dsSSP.Tables[1].Rows.Count > 0)
                {
                    dtPrevDashboardSummary = dsSSP.Tables[1];
                    objIQAgent_DashBoardModel.PrevIQAgentSummary.IsEnoughData = true;
                }

                if (dtPrevDashboardSummary.Rows.Count > 0)
                {
                    List<IQAgent_ComparisionValues> ListOfIQAgent_ComparisionValues = new List<IQAgent_ComparisionValues>();
                    foreach (DataRow datarow in dtPrevDashboardSummary.Rows)
                    {
                        IQAgent_ComparisionValues objIQAgent_ComparisionValues = new IQAgent_ComparisionValues();
                        if (dtPrevDashboardSummary.Columns.Contains("NoOfDocs") && !datarow["NoOfDocs"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.NoOfDocs = Convert.ToInt64(datarow["NoOfDocs"]);
                            objIQAgent_ComparisionValues.TotalAirSeconds = Convert.ToInt64(datarow["NoOfDocs"]) * 8;
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("NoOfHits") && !datarow["NoOfHits"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.NoOfHits = Convert.ToInt64(datarow["NoOfHits"]);
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("IQMediaValue") && !datarow["IQMediaValue"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.IQMediaValue = Convert.ToDecimal(datarow["IQMediaValue"]);
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("Audience") && !datarow["Audience"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.Audience = Convert.ToInt64(datarow["Audience"]);
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("PositiveSentiment") && !datarow["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.PositiveSentiment = Convert.ToInt64(datarow["PositiveSentiment"]);
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("NegativeSentiment") && !datarow["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.NegativeSentiment = Convert.ToInt64(datarow["NegativeSentiment"]);
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("MediaType") && !datarow["MediaType"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.MediaType = Convert.ToString(datarow["MediaType"]);
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("SubMediaType") && !datarow["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.SubMediaType = Convert.ToString(datarow["SubMediaType"]);
                        }

                        ListOfIQAgent_ComparisionValues.Add(objIQAgent_ComparisionValues);
                    }

                    objIQAgent_DashBoardModel.PrevIQAgentSummary.ListOfIQAgentPrevSummary = ListOfIQAgent_ComparisionValues;
                }
            }       

            return objIQAgent_DashBoardModel;
        }

        private void GetMediaTypeResults(IQAgent_DashBoardModel objIQAgent_DashBoardModel, Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_MediaTypeXml, string p_ListSPName)
        {
            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@MediaType", DbType.String, p_Medium, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@MediaTypeAccessXml", DbType.Xml, p_MediaTypeXml, ParameterDirection.Input));

            Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
            DataSet dsSSP = DataAccess.GetDataSet(p_ListSPName, dataTypeList);

            FillMediaTypeResults(dsSSP,objIQAgent_DashBoardModel);            
        }

        private void FillMediaTypeResults(DataSet dsSSP, IQAgent_DashBoardModel objIQAgent_DashBoardModel)
        {
            if (dsSSP != null && dsSSP.Tables.Count > 0)
            {
                objIQAgent_DashBoardModel.ListOfTopStationBroadCast = new List<DashboardTopResultsModel>();
                DataTable dt2 = dsSSP.Tables[0];

                foreach (DataRow datarow in dt2.Rows)
                {
                    DashboardTopResultsModel iQAgent_TVResultsBroadCastModel = new DashboardTopResultsModel();
                    if (dt2.Columns.Contains("IQ_Station_ID") && !datarow["IQ_Station_ID"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Outlet_Name = Convert.ToString(datarow["IQ_Station_ID"]);
                    }
                    else if (dt2.Columns.Contains("CompeteURL") && !datarow["CompeteURL"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Outlet_Name = Convert.ToString(datarow["CompeteURL"]);
                    }
                    else if (dt2.Columns.Contains("actor_preferredname") && !datarow["actor_preferredname"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Outlet_Name = Convert.ToString(datarow["actor_preferredname"]);
                    }
                    else if (dt2.Columns.Contains("Publication") && !datarow["Publication"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Outlet_Name = Convert.ToString(datarow["Publication"]);
                    }

                    if (dt2.Columns.Contains("DMA_Name") && !datarow["DMA_Name"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.DMA_Name = Convert.ToString(datarow["DMA_Name"]);
                    }

                    if (dt2.Columns.Contains("DMA_Num") && !datarow["DMA_Num"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.DMA_Num = Convert.ToString(datarow["DMA_Num"]);
                    }

                    if (dt2.Columns.Contains("_IQDmaID") && !datarow["_IQDmaID"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel._IQDmaIDs = Convert.ToInt32(datarow["_IQDmaID"]);
                    }

                    if (dt2.Columns.Contains("NoOfDocs") && !datarow["NoOfDocs"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.NoOfDocs = Convert.ToInt32(datarow["NoOfDocs"]);
                    }

                    if (dt2.Columns.Contains("Mentions") && !datarow["Mentions"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Mentions = Convert.ToInt64(datarow["Mentions"]);
                    }

                    if (dt2.Columns.Contains("MediaValue") && !datarow["MediaValue"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.MediaValue = Convert.ToDecimal(datarow["MediaValue"]);
                    }

                    if (dt2.Columns.Contains("Audience") && !datarow["Audience"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Audience = Convert.ToInt64(datarow["Audience"]);
                    }

                    if (dt2.Columns.Contains("FriendsCount") && !datarow["FriendsCount"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.FriendsCount = Convert.ToInt64(datarow["FriendsCount"]);
                    }

                    if (dt2.Columns.Contains("PositiveSentiment") && !datarow["PositiveSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.PositiveSentiment = Convert.ToInt32(datarow["PositiveSentiment"]);
                    }

                    if (dt2.Columns.Contains("NegativeSentiment") && !datarow["NegativeSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.NegativeSentiment = Convert.ToInt32(datarow["NegativeSentiment"]);
                    }

                    if (dt2.Columns.Contains("SubMediaType") && !datarow["SubMediaType"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.SubMediaType = Convert.ToString(datarow["SubMediaType"]);
                    }

                    objIQAgent_DashBoardModel.ListOfTopStationBroadCast.Add(iQAgent_TVResultsBroadCastModel);
                }
            }

            if (dsSSP != null && dsSSP.Tables.Count > 1)
            {
                objIQAgent_DashBoardModel.ListOfTopDMABroadCast = new List<DashboardTopResultsModel>();
                DataTable dt3 = dsSSP.Tables[1];

                foreach (DataRow datarow in dt3.Rows)
                {
                    DashboardTopResultsModel iQAgent_TVResultsBroadCastModel = new DashboardTopResultsModel();
                    if (dt3.Columns.Contains("Author") && !datarow["Author"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Outlet_Name = Convert.ToString(datarow["Author"]);
                    }

                    if (dt3.Columns.Contains("DMA_Name") && !datarow["DMA_Name"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.DMA_Name = Convert.ToString(datarow["DMA_Name"]);
                    }

                    if (dt3.Columns.Contains("DMA_Num") && !datarow["DMA_Num"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.DMA_Num = Convert.ToString(datarow["DMA_Num"]);
                    }

                    if (dt3.Columns.Contains("_IQDmaID") && !datarow["_IQDmaID"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel._IQDmaIDs = Convert.ToInt32(datarow["_IQDmaID"]);
                    }

                    if (dt3.Columns.Contains("NoOfDocs") && !datarow["NoOfDocs"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.NoOfDocs = Convert.ToInt32(datarow["NoOfDocs"]);
                    }

                    if (dt3.Columns.Contains("Mentions") && !datarow["Mentions"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Mentions = Convert.ToInt64(datarow["Mentions"]);
                    }

                    if (dt3.Columns.Contains("MediaValue") && !datarow["MediaValue"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.MediaValue = Convert.ToDecimal(datarow["MediaValue"]);
                    }

                    if (dt3.Columns.Contains("Audience") && !datarow["Audience"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Audience = Convert.ToInt64(datarow["Audience"]);
                    }

                    if (dt3.Columns.Contains("PositiveSentiment") && !datarow["PositiveSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.PositiveSentiment = Convert.ToInt32(datarow["PositiveSentiment"]);
                    }

                    if (dt3.Columns.Contains("NegativeSentiment") && !datarow["NegativeSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.NegativeSentiment = Convert.ToInt32(datarow["NegativeSentiment"]);
                    }

                    if (dt3.Columns.Contains("actor_preferredname") && !datarow["actor_preferredname"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Outlet_Name = "@" + Convert.ToString(datarow["actor_preferredname"]);
                    }

                    if (dt3.Columns.Contains("SubMediaType") && !datarow["SubMediaType"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.SubMediaType = Convert.ToString(datarow["SubMediaType"]);
                    }

                    objIQAgent_DashBoardModel.ListOfTopDMABroadCast.Add(iQAgent_TVResultsBroadCastModel);
                }
            }

            if (dsSSP != null && dsSSP.Tables.Count > 2)
            {
                objIQAgent_DashBoardModel.DmaMentionMapList = new Dictionary<string, long>();
                DataTable dt4 = dsSSP.Tables[2];

                foreach (DataRow datarow in dt4.Rows)
                {

                    string key = string.Empty;
                    string dma = string.Empty;
                    long mention = 0;

                    if (dt4.Columns.Contains("DMA_Name") && !datarow["DMA_Name"].Equals(DBNull.Value))
                    {
                        dma = Convert.ToString(datarow["DMA_Name"]);
                    }

                    if (dt4.Columns.Contains("DMA_Num") && !datarow["DMA_Num"].Equals(DBNull.Value))
                    {
                        key = Convert.ToString(datarow["DMA_Num"]);
                    }

                    if (dt4.Columns.Contains("Mentions") && !datarow["Mentions"].Equals(DBNull.Value))
                    {
                        mention = Convert.ToInt64(datarow["Mentions"]);
                    }

                    objIQAgent_DashBoardModel.DmaMentionMapList.Add(dma, mention);
                }
            }

            if (dsSSP != null && dsSSP.Tables.Count > 3)
            {
                DataTable dt;

                dt = dsSSP.Tables[3];

                objIQAgent_DashBoardModel.CanadaMentionMapList = new Dictionary<string, long>();
                foreach (DataRow dr in dt.Rows)
                {
                    string province = String.Empty;
                    long mentions = 0;

                    if (dt.Columns.Contains("Province") && !dr["Province"].Equals(DBNull.Value))
                    {
                        province = Convert.ToString(dr["Province"]);
                    }
                    if (dt.Columns.Contains("Mentions") && !dr["Mentions"].Equals(DBNull.Value))
                    {
                        mentions = Convert.ToInt64(dr["Mentions"]);
                    }

                    objIQAgent_DashBoardModel.CanadaMentionMapList.Add(province, mentions);
                }
            }

            if (dsSSP != null && dsSSP.Tables.Count > 4)
            {
                objIQAgent_DashBoardModel.ListOfTopCountryBroadCast = new List<DashboardTopResultsModel>();
                DataTable dt5 = dsSSP.Tables[4];

                foreach (DataRow datarow in dt5.Rows)
                {
                    DashboardTopResultsModel iQAgent_TVResultsBroadCastModel = new DashboardTopResultsModel();

                    if (dt5.Columns.Contains("Country") && !datarow["Country"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Country = Convert.ToString(datarow["Country"]);
                    }

                    if (dt5.Columns.Contains("Country_Num") && !datarow["Country_Num"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Country_Num = Convert.ToString(datarow["Country_Num"]);
                    }

                    if (dt5.Columns.Contains("NoOfDocs") && !datarow["NoOfDocs"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.NoOfDocs = Convert.ToInt32(datarow["NoOfDocs"]);
                    }

                    if (dt5.Columns.Contains("Mentions") && !datarow["Mentions"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Mentions = Convert.ToInt64(datarow["Mentions"]);
                    }

                    if (dt5.Columns.Contains("PositiveSentiment") && !datarow["PositiveSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.PositiveSentiment = Convert.ToInt32(datarow["PositiveSentiment"]);
                    }

                    if (dt5.Columns.Contains("NegativeSentiment") && !datarow["NegativeSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.NegativeSentiment = Convert.ToInt32(datarow["NegativeSentiment"]);
                    }

                    if (dt5.Columns.Contains("SubMediaType") && !datarow["SubMediaType"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.SubMediaType = Convert.ToString(datarow["SubMediaType"]);
                    }

                    objIQAgent_DashBoardModel.ListOfTopCountryBroadCast.Add(iQAgent_TVResultsBroadCastModel);
                }
            }
        }

        public List<IQAgent_DaySummaryModel> GetDmaSummaryDataDayWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_DmaXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DmaXml", DbType.Xml, p_DmaXml, ParameterDirection.Input));


                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_DaySummary_SelectDmaSummaryByDay", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<IQAgent_DaySummaryModel> GetProvinceSummaryDataDayWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_ProvinceXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ProvinceXml", DbType.Xml, p_ProvinceXml, ParameterDirection.Input));

                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_DaySummary_SelectProvinceSummaryByDay", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IQAgent_DaySummaryModel> GetDmaSummaryDataMonthWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_DmaXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DmaXml", DbType.Xml, p_DmaXml, ParameterDirection.Input));


                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_DaySummary_SelectDmaSummaryByMonth", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<IQAgent_DaySummaryModel> GetProvinceSummaryDataMonthWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_ProvinceXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ProvinceXml", DbType.Xml, p_ProvinceXml, ParameterDirection.Input));

                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_DaySummary_SelectProvinceSummaryByMonth", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IQAgent_DaySummaryModel> GetDmaSummaryDataHourWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_DmaXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DmaXml", DbType.Xml, p_DmaXml, ParameterDirection.Input));


                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_HourSummary_SelectDmaSummaryByHour", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<IQAgent_DaySummaryModel> GetProvinceSummaryDataHourWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_ProvinceXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ProvinceXml", DbType.Xml, p_ProvinceXml, ParameterDirection.Input));

                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_HourSummary_SelectProvinceSummaryByHour", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQAgent_DashBoardModel GetAdhocSummaryData(string p_MediaIDXml, string p_Source, string p_Medium, Guid p_ClientGUID, string p_ListSPName, string p_MediaTypeXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@MediaIDXml", DbType.Xml, p_MediaIDXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Source", DbType.String, p_Source, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                DataSet ds = DataAccess.GetDataSet("usp_v5_IQDashboard_AdhocSummary_SelectByID", dataTypeList);

                IQAgent_DashBoardModel objIQAgent_DashBoardModel = FillIQAgentSummary(ds, p_Medium);                               

                if (!string.IsNullOrEmpty(p_Medium) && !string.IsNullOrEmpty(p_ListSPName))
                {
                    dataTypeList.Add(new DataType("@MediaTypeAccessXml", DbType.Xml, p_MediaTypeXml, ParameterDirection.Input));

                    DataSet dsSSP = DataAccess.GetDataSet(p_ListSPName, dataTypeList);
                    FillMediaTypeResults(dsSSP, objIQAgent_DashBoardModel); 
                }

                return objIQAgent_DashBoardModel;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
