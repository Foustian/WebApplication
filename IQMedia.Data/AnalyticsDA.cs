using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;
using IQMedia.Shared.Utility;
using System.Xml.Linq;
using System.Globalization;

namespace IQMedia.Data
{
    public class AnalyticsDA : IDataAccess
    {
        public List<AnalyticsCampaign> GetCampaigns(Guid clientGUID)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //Log4NetLogger.Debug(string.Format("DA.GetCampaigns"));
                //Log4NetLogger.Debug(string.Format("-SP: usp_v5_IQAgent_Campaign_SelectByClientGuid"));
                //Log4NetLogger.Debug(string.Format("-@ClientGuid: {0}", clientGUID));

                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@ClientGuid", DbType.Guid, clientGUID, ParameterDirection.Input));
                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQAgent_Campaign_SelectByClientGuid", dataTypes);
                List<AnalyticsCampaign> campaigns = FillAnalyticsCampaigns(dsSSP);

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("DA.GetCampaigns: {0} ms", sw.ElapsedMilliseconds));
                return campaigns;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
            }
            return new List<AnalyticsCampaign>();
        }

        public List<AnalyticsSecondaryTable> GetSecondaryTables(SecondaryTabID tab, string pageType)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //Log4NetLogger.Debug(string.Format("DA.GetSecondaryTables"));
                //Log4NetLogger.Debug(string.Format("-SP: usp_v5_Analytics_Secondaries"));
                //Log4NetLogger.Debug(string.Format("-@MainTab: {0}", tab));
                //Log4NetLogger.Debug(string.Format("-@PageType: {0}", pageType));

                List<AnalyticsSecondaryTable> analyticsTables = new List<AnalyticsSecondaryTable>();
                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@MainTab", DbType.String, tab.ToString(), ParameterDirection.Input));
                dataTypes.Add(new DataType("@PageType", DbType.String, pageType, ParameterDirection.Input));

                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_Analytics_Secondaries", dataTypes);
                analyticsTables = FillSecondaryTables(dsSSP);

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("DA.GetSecondaryTables: {0} ms", sw.ElapsedMilliseconds));
                return analyticsTables;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
            }
            return new List<AnalyticsSecondaryTable>();
        }

        public List<AnalyticsActiveElement> GetActiveElements()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug("DA.GetActiveElements");

                List<AnalyticsActiveElement> activeElements = new List<AnalyticsActiveElement>();
                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_Analytics_ActiveElements", new List<DataType>());
                activeElements = FillActiveElements(dsSSP);

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("DA.GetActiveElements: {0} ms", sw.ElapsedMilliseconds));
                return activeElements;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
            }
            return new List<AnalyticsActiveElement>();
        }

        public AnalyticsDataModel GetDaySummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, bool inDefaultRange, bool loadEverything = true)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                // Prevent SP from using new Analytics Table
                //if (string.Compare(clientGUID.ToString(), "7722A116-C3BC-40AE-8070-8C59EE9E3D2A", true) == 0)
                //{
                //    clientGUID = new Guid();
                //}

                //Log4NetLogger.Debug(string.Format("DA.GetDaySummaryData"));
                //Log4NetLogger.Debug(string.Format("-SP: usp_v5_IQAgent_Analytics_DaySummary"));
                //Log4NetLogger.Debug(string.Format("-@ClientGUID: {0}", clientGUID));
                //Log4NetLogger.Debug(string.Format("-@SearchRequestIDXml: {0}", searchRequestXml));
                //Log4NetLogger.Debug(string.Format("-@Medium: {0}", subMediaType));
                //Log4NetLogger.Debug(string.Format("-loadEverything: {0}", loadEverything));

                AnalyticsDataModel dataModel = new AnalyticsDataModel();
                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@ClientGUID", DbType.Guid, clientGUID, ParameterDirection.Input));

                // SP does not treat empty string sub media type as null value, should
                if (subMediaType != "")
                {
                    dataTypes.Add(new DataType("@Medium", DbType.String, subMediaType, ParameterDirection.Input));
                }
                else
                {
                    // Requires an actual DBNull value to be passed as parameter, not an empty string - SP only checks if null and not if empty string
                    dataTypes.Add(new DataType("@Medium", DbType.String, DBNull.Value, ParameterDirection.Input));
                }
                dataTypes.Add(new DataType("@SearchRequestIDXml", DbType.Xml, searchRequestXml, ParameterDirection.Input));
                dataTypes.Add(new DataType("@LoadEverything", DbType.Boolean, loadEverything, ParameterDirection.Input));
                //dataTypes.Add(new DataType("@InDefaultRange", DbType.Boolean, inDefaultRange, ParameterDirection.Input));

                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQAgent_Analytics_DaySummary", dataTypes);

                dataModel = FillIQAgentSummary(dsSSP, gmtAdjustment, dstAdjustment);

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("{0} summaries returned", dataModel.SummaryDataList.Count));
                //Log4NetLogger.Debug(string.Format("DA.GetDaySummaryData: {0} ms", sw.ElapsedMilliseconds));
                return dataModel;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
            }
            return new AnalyticsDataModel();
        }

        public List<string> GetTopNetworkShows(Guid clientGUID, string searchRequestXml, SecondaryTabID tab)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //Log4NetLogger.Debug(string.Format("DA.GetTopTenData"));
                //Log4NetLogger.Debug(string.Format("-SP: usp_v5_IQAgent_Analytics_TopTenData"));
                //Log4NetLogger.Debug(string.Format("-@ClientGUID: {0}", clientGUID));
                //Log4NetLogger.Debug(string.Format("-@Tab: {0}", tab.ToString()));
                //Log4NetLogger.Debug(string.Format("-@SearchRequestIDXml: {0}", searchRequestXml));

                AnalyticsDataModel dataModel = new AnalyticsDataModel();
                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@ClientGUID", DbType.Guid, clientGUID, ParameterDirection.Input));
                dataTypes.Add(new DataType("@Tab", DbType.String, tab.ToString(), ParameterDirection.Input));
                dataTypes.Add(new DataType("@SearchRequestIDXml", DbType.Xml, searchRequestXml, ParameterDirection.Input));

                DataSet dataSet = DataAccess.GetDataSet("usp_v5_IQAgent_Analytics_TopTenData", dataTypes);

                List<string> lstTopTen = new List<string>();
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0] != null)
                {
                    //Log4NetLogger.Debug(string.Format("dataSet.Tables[0].Rows.Count: {0}", dataSet.Tables[0].Rows.Count));
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        if (dataSet.Tables[0].Columns.Contains("Category") && !dr["Category"].Equals(DBNull.Value))
                        {
                            lstTopTen.Add(dr["Category"].ToString());
                        }
                    }
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("DA.GetTopTenData: {0} ms", sw.ElapsedMilliseconds));
                return lstTopTen;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
            }
            return new List<string>();
        }

        public AnalyticsDataModel GetNetworkShowSummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, List<string> lstTopTen, SecondaryTabID tab, string dateInterval)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                var newTopTenList = new List<string>();
                foreach (var item in lstTopTen)
                {
                    var i = item;
                    i = item.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("\'", "&apos;").Replace("<", "&lt;").Replace(">", "&gt;");
                    newTopTenList.Add(i);
                }

                //Log4NetLogger.Debug(string.Format("DA.GetNetworkShowSummaryData"));
                //Log4NetLogger.Debug(string.Format("-SP: usp_v5_IQAgent_Analytics_NetworkShowSummary"));
                //Log4NetLogger.Debug(string.Format("-@ClientGUID: {0}", clientGUID));
                //Log4NetLogger.Debug(string.Format("-@SearchRequestIDXml: {0}", searchRequestXml));
                //Log4NetLogger.Debug(string.Format("-@Medium: {0}", subMediaType));
                //Log4NetLogger.Debug(string.Format("-@DateInterval: {0}", dateInterval));
                //Log4NetLogger.Debug(string.Format("-@Tab: {0}", tab.ToString()));
                //Log4NetLogger.Debug(string.Format("-@TopTen: {0}", "<list><item id=\"" + String.Join("\" /><item id=\"", newTopTenList) + "\" /></list>"));

                AnalyticsDataModel dataModel = new AnalyticsDataModel();
                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@ClientGUID", DbType.Guid, clientGUID, ParameterDirection.Input));

                // SP does not treat empty string sub media type as null value, should
                if (subMediaType != "")
                {
                    dataTypes.Add(new DataType("@Medium", DbType.String, subMediaType, ParameterDirection.Input));
                }
                else
                {
                    // Requires an actual DBNull value to be passed as parameter, not an empty string - SP only checks if null and not if empty string
                    dataTypes.Add(new DataType("@Medium", DbType.String, DBNull.Value, ParameterDirection.Input));
                }
                dataTypes.Add(new DataType("@Tab", DbType.String, tab.ToString(), ParameterDirection.Input));
                dataTypes.Add(new DataType("@DateInterval", DbType.String, dateInterval, ParameterDirection.Input));
                dataTypes.Add(new DataType("@TopTen", DbType.Xml, "<list><item id=\"" + String.Join("\" /><item id=\"", newTopTenList) + "\" /></list>", ParameterDirection.Input));
                dataTypes.Add(new DataType("@SearchRequestIDXml", DbType.Xml, searchRequestXml, ParameterDirection.Input));

                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQAgent_Analytics_NetworkShowSummary", dataTypes);

                dataModel = FillIQAgentSummary(dsSSP, gmtAdjustment, dstAdjustment, tab);

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("DA.GetNetworkShowSummaryData: {0} ms", sw.ElapsedMilliseconds));
                return dataModel;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
            }
            return new AnalyticsDataModel();
        }

        public AnalyticsDataModel GetMonthSummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("DA.GetMonthSummaryData"));
                //Log4NetLogger.Debug(string.Format("-SP: usp_v5_IQAgent_Analytics_MonthSummary"));
                //Log4NetLogger.Debug(string.Format("-@ClientGUID: {0}", clientGUID));
                //Log4NetLogger.Debug(string.Format("-@Medium: {0}", subMediaType));
                //Log4NetLogger.Debug(string.Format("-@SearchRequestIDXml: {0}", searchRequestXml));

                AnalyticsDataModel dataModel = new AnalyticsDataModel();
                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@ClientGUID", DbType.Guid, clientGUID, ParameterDirection.Input));
                dataTypes.Add(new DataType("@Medium", DbType.String, subMediaType, ParameterDirection.Input));
                dataTypes.Add(new DataType("@SearchRequestIDXml", DbType.Xml, searchRequestXml, ParameterDirection.Input));

                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQAgent_Analytics_MonthSummary", dataTypes);
                dataModel = FillIQAgentSummary(dsSSP, gmtAdjustment, dstAdjustment);
                return dataModel;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public AnalyticsDataModel GetHourSummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, bool inDefaultRange, bool loadEverything = true)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                // Prevent SP from using new Analytics Table
                //if (string.Compare(clientGUID.ToString(), "7722A116-C3BC-40AE-8070-8C59EE9E3D2A", true) == 0)
                //{
                //    clientGUID = new Guid();
                //}

                //Log4NetLogger.Debug(string.Format("DA.GetHourSummaryData"));
                //Log4NetLogger.Debug(string.Format("-SP: usp_v5_IQAgent_Analytics_HourSummary"));
                //Log4NetLogger.Debug(string.Format("-@ClientGUID: {0}", clientGUID));
                //Log4NetLogger.Debug(string.Format("-@Medium: {0}", subMediaType));
                //Log4NetLogger.Debug(string.Format("-@SearchRequestIDXml: {0}", searchRequestXml));

                AnalyticsDataModel dataModel = new AnalyticsDataModel();
                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@ClientGUID", DbType.Guid, clientGUID, ParameterDirection.Input));
                if (subMediaType != "")
                {
                    dataTypes.Add(new DataType("@Medium", DbType.String, subMediaType, ParameterDirection.Input));
                }
                else
                {
                    dataTypes.Add(new DataType("@Medium", DbType.String, DBNull.Value, ParameterDirection.Input));
                }
                dataTypes.Add(new DataType("@SearchRequestIDXml", DbType.Xml, searchRequestXml, ParameterDirection.Input));
                dataTypes.Add(new DataType("@LoadEverything", DbType.Boolean, loadEverything, ParameterDirection.Input));
                //dataTypes.Add(new DataType("@DefaultDateRange", DbType.Boolean, inDefaultRange, ParameterDirection.Input));

                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQAgent_Analytics_HourSummary", dataTypes);
                dataModel = FillIQAgentSummary(dsSSP, gmtAdjustment, dstAdjustment);

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("{0} summaries returned", dataModel.SummaryDataList.Count));
                //Log4NetLogger.Debug(string.Format("DA.GetHourSummaryData: {0} ms", sw.ElapsedMilliseconds));
                return dataModel;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public AnalyticsDataModel GetDaySummaryDataForCampaign(string campaignIDXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, bool loadEverything = true)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //Log4NetLogger.Debug(string.Format("DA.GetDaySummaryDataForCampaign"));
                //Log4NetLogger.Debug(string.Format("-SP: usp_v5_IQAgent_Analytics_DaySummary_Campaign"));
                //Log4NetLogger.Debug(string.Format("-@CampaignIDXml: {0}", campaignIDXml));
                //Log4NetLogger.Debug(string.Format("-@SubMediaType: {0}", subMediaType));
                List<DataType> dataTypes = new List<DataType>();
                if (!string.IsNullOrEmpty(subMediaType))
                {
                    dataTypes.Add(new DataType("@SubMediaType", DbType.String, subMediaType, ParameterDirection.Input));
                }
                else
                {
                    dataTypes.Add(new DataType("@SubMediaType", DbType.String, DBNull.Value, ParameterDirection.Input));
                }

                dataTypes.Add(new DataType("@CampaignIDXml", DbType.Xml, campaignIDXml, ParameterDirection.Input));
                dataTypes.Add(new DataType("@LoadEverything", DbType.Boolean, loadEverything, ParameterDirection.Input));

                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQAgent_Analytics_DaySummary_Campaign", dataTypes);
                var daySummaryData = FillIQAgentSummary(dsSSP, gmtAdjustment, dstAdjustment);

                sw.Stop();
                Log4NetLogger.Debug(string.Format("DA.GetDaySummaryDataForCampaign: {0} ms", sw.ElapsedMilliseconds));
                return daySummaryData;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public AnalyticsDataModel GetHourSummaryDataForCampaign(string campaignIDXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, bool loadEverything = true, string GroupByHeader = "")
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //Log4NetLogger.Debug(string.Format("DA.GetHourSummaryDataForCampaign"));
                //Log4NetLogger.Debug(string.Format("-SP: usp_v5_IQAgent_Analytics_HourSummary_Campaign"));
                //Log4NetLogger.Debug(string.Format("-@CampaignIDXml: {0}", campaignIDXml));
                //Log4NetLogger.Debug(string.Format("-@SubMediaType: {0}", subMediaType));

                List<DataType> dataTypes = new List<DataType>();
                if (!string.IsNullOrEmpty(subMediaType))
                {
                    dataTypes.Add(new DataType("@SubMediaType", DbType.String, subMediaType, ParameterDirection.Input));
                }
                else
                {
                    dataTypes.Add(new DataType("@SubMediaType", DbType.String, DBNull.Value, ParameterDirection.Input));
                }
                dataTypes.Add(new DataType("@CampaignIDXml", DbType.Xml, campaignIDXml, ParameterDirection.Input));
                dataTypes.Add(new DataType("@LoadEverything", DbType.Boolean, loadEverything, ParameterDirection.Input));

                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQAgent_Analytics_HourSummary_Campaign", dataTypes);
                var hourSummaryData = FillIQAgentSummary(dsSSP, gmtAdjustment, dstAdjustment);

                sw.Stop();
                Log4NetLogger.Debug(string.Format("DA.GetHourSummaryDataForCampaign: {0} ms", sw.ElapsedMilliseconds));
                return hourSummaryData;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public Int64 CreateCampaign(string campaignName, Int64 agentSRID, DateTime startDate, DateTime endDate, DateTime startDateGMT, DateTime endDateGMT)
        {
            try
            {
                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@CampaignName", DbType.String, campaignName, ParameterDirection.Input));
                dataTypes.Add(new DataType("@_SearchRequestID", DbType.Int64, agentSRID, ParameterDirection.Input));
                dataTypes.Add(new DataType("@StartDateTime", DbType.DateTime, startDate, ParameterDirection.Input));
                dataTypes.Add(new DataType("@EndDateTime", DbType.DateTime, endDate, ParameterDirection.Input));
                dataTypes.Add(new DataType("@StartDateTimeGMT", DbType.DateTime, startDateGMT, ParameterDirection.Input));
                dataTypes.Add(new DataType("@EndDateTimeGMT", DbType.DateTime, endDateGMT, ParameterDirection.Input));
                dataTypes.Add(new DataType("@CampaignID", DbType.Int64, 0, ParameterDirection.Output));

                Int64 campID = Convert.ToInt64(DataAccess.ExecuteNonQuery("usp_v5_IQAgent_Analytics_CreateCampaign", dataTypes));
                return campID;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                throw;
            }
        }

        public DateTime EditCampaign(Int64 campaignID, string campaignName, Int64? agentSRID, DateTime? startDate, DateTime? endDate, DateTime? startDateGMT, DateTime? endDateGMT)
        {
            try
            {
                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@CampaignID", DbType.Int64, campaignID, ParameterDirection.Input));

                if (string.IsNullOrWhiteSpace(campaignName))
                {
                    dataTypes.Add(new DataType("@CampaignName", DbType.String, DBNull.Value, ParameterDirection.Input));
                }
                else
                {
                    dataTypes.Add(new DataType("@CampaignName", DbType.String, campaignName, ParameterDirection.Input));
                }
                dataTypes.Add(new DataType("@SearchRequestID", DbType.Int64, agentSRID, ParameterDirection.Input));
                dataTypes.Add(new DataType("@StartDateTime", DbType.DateTime, startDate, ParameterDirection.Input));
                dataTypes.Add(new DataType("@EndDateTime", DbType.DateTime, endDate, ParameterDirection.Input));
                dataTypes.Add(new DataType("@StartDateTimeGMT", DbType.DateTime, startDateGMT, ParameterDirection.Input));
                dataTypes.Add(new DataType("@EndDateTimeGMT", DbType.DateTime, endDateGMT, ParameterDirection.Input));
                dataTypes.Add(new DataType("@ModifiedDate", DbType.DateTime, 0, ParameterDirection.Output));

                DateTime modifiedDate = Convert.ToDateTime(DataAccess.ExecuteNonQuery("usp_v5_IQAgent_Analytics_EditCampaign", dataTypes));
                return modifiedDate;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                throw;
            }
        }

        public void DeleteCampaign(Int64 campaignID)
        {
            try
            {
                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@CampaignID", DbType.Int64, campaignID, ParameterDirection.Input));
                DataAccess.GetDataSet("usp_v5_IQAgent_Analytics_DeleteCampaign", dataTypes);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                throw;
            }
        }

        public AnalyticsCampaign GetCampaignByID(Int64 campaignID)
        {
            try
            {
                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@CampaignID", DbType.Int64, campaignID, ParameterDirection.Input));
                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQAgent_Analytics_Campaign_SelectByID", dataTypes);

                return FillAnalyticsCampaigns(dsSSP)[0];    // Should only return a single campaign model
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                throw;
            }
        }

        public Dictionary<string, string> GetAllDMAs()
        {
            try
            {
                List<DataType> dataTypes = new List<DataType>();
                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQAgent_Analytics_DMA_SelectAll", dataTypes);

                return FillDMAs(dsSSP);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                throw;
            }
        }

        private Dictionary<string, string> FillDMAs(DataSet dsSSP)
        {
            try
            {
                Dictionary<string, string> dictDMAs = new Dictionary<string, string>();

                if (dsSSP != null && dsSSP.Tables.Count > 0)
                {
                    foreach (DataTable dt in dsSSP.Tables)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                if (string.Compare(Convert.ToString(row["strDmaName"]), "Unknown") == 0)
                                {
                                    dictDMAs.Add(Convert.ToString(row["strDmaID"]), "Global");
                                }
                                else
                                {
                                    dictDMAs.Add(Convert.ToString(row["strDmaID"]), Convert.ToString(row["strDmaName"]));
                                }
                            }
                        }
                    }
                }

                return dictDMAs;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                throw;
            }
        }

        public AnalyticsDataModel FillIQAgentSummary(DataSet dsSSP, decimal gmtAdjustment, decimal dstAdjustment, SecondaryTabID tab = SecondaryTabID.OverTime)
        {
            //Log4NetLogger.Debug("DA.FillIQAgentSummary");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            AnalyticsDataModel dataModel = new AnalyticsDataModel();
            dataModel.SummaryDataList = new List<AnalyticsSummaryModel>();
            dataModel.DmaMentionMapList = new List<AnalyticsMapSummaryModel>();
            dataModel.CanadaMentionMapList = new List<AnalyticsMapSummaryModel>();

            if (dsSSP != null && dsSSP.Tables.Count > 0)
            {
                //Log4NetLogger.Debug(string.Format("dsSSP != null && dsSSP.Tables.Count > 0"));
                foreach (DataTable dt in dsSSP.Tables)
                {
                    Log4NetLogger.Debug(string.Format("dt.Rows.Count: {0}", dt.Rows.Count));
                    //Log4NetLogger.Debug(string.Format("dt.Columns {0} contain tabletype", dt.Columns.Contains("TableType") ? "does" : "does not"));
                    if (dt.Rows.Count > 0 && dt.Columns.Contains("TableType"))
                    {
                        //Log4NetLogger.Debug(string.Format("dt.Rows.Count > 0 && dt.Columns.Contains(TableType)"));
                        //Log4NetLogger.Debug(string.Format("TableType: {0}", dt.Rows[0]["TableType"]));
                        switch (Convert.ToString(dt.Rows[0]["TableType"]))
                        {
                            case "OverallSummary":
                                //Get Day Part Data
                                List<DayPartDataItem> dayPartData = GetDayPartData("A");

                                foreach (DataRow row in dt.Rows)
                                {
                                    AnalyticsSummaryModel summaryModel = new AnalyticsSummaryModel();

                                    if (dt.Columns.Contains("DayDate") && !row["DayDate"].Equals(DBNull.Value))
                                    {
                                        summaryModel.SummaryDateTime = Convert.ToDateTime(row["DayDate"]);

                                        // If table does not contain GMT column or it does and contains a NULL value
                                        if (!dt.Columns.Contains("GMTDateTime") || (dt.Columns.Contains("GMTDateTime") && row["GMTDateTime"].Equals(DBNull.Value)))
                                        {
                                            DateTime gmtDT = summaryModel.SummaryDateTime;
                                            if (gmtDT.IsDaylightSavingTime())
                                            {
                                                gmtDT = gmtDT.AddHours(Convert.ToDouble(gmtAdjustment) + Convert.ToDouble(dstAdjustment));
                                            }
                                            else
                                            {
                                                gmtDT = gmtDT.AddHours(Convert.ToDouble(gmtAdjustment));
                                            }

                                            summaryModel.DayTimeDisplay = string.Format("{0} {1}", gmtDT.DayOfWeek, gmtDT.ToShortTimeString());
                                            summaryModel.DayTimeID = string.Format("{0}_{1}", gmtDT.DayOfWeek, gmtDT.Hour);
                                        }

                                        if (!dt.Columns.Contains("LocalDateTime") || (dt.Columns.Contains("LocalDateTime") && row["LocalDateTime"].Equals(DBNull.Value)))
                                        {
                                            DateTime localDT = summaryModel.SummaryDateTime;
                                            if (localDT.IsDaylightSavingTime())
                                            {
                                                localDT = localDT.AddHours(Convert.ToDouble(gmtAdjustment) + Convert.ToDouble(dstAdjustment));
                                            }
                                            else
                                            {
                                                localDT = localDT.AddHours(Convert.ToDouble(gmtAdjustment));
                                            }
                                            summaryModel.LocalDateTime = localDT;
                                            // Get Day Specific Day Part Data
                                            List<DayPartDataItem> dayPartItem = dayPartData == null ? new List<DayPartDataItem>() : dayPartData.Where(w => w.DayOfWeek == localDT.DayOfWeek && localDT.Hour == w.HourOfDay).ToList();
                                            summaryModel.DayPartDisplay = (dayPartItem.Any() && !string.IsNullOrEmpty(dayPartItem.First().DayPartName)) ? dayPartItem.First().DayPartName : "Other";
                                            summaryModel.DayPartID = (dayPartItem.Any() && !string.IsNullOrEmpty(dayPartItem.First().DayPartCode)) ? dayPartItem.First().DayPartCode : "Other";
                                        }

                                        // LOCAL DATE TIME BELOW ACTUALLY REFERS TO CLIENT TIME
                                        // The SummaryDateTime values are convert to local time in the controller, but the DayTime and DayPart values require the conversion here as well
                                        //DateTime localDateTime = summaryModel.SummaryDateTime;
                                        //if (localDateTime.IsDaylightSavingTime())
                                        //{
                                        //    localDateTime = localDateTime.AddHours(Convert.ToDouble(gmtAdjustment) + Convert.ToDouble(dstAdjustment));
                                        //}
                                        //else
                                        //{
                                        //    localDateTime = localDateTime.AddHours(Convert.ToDouble(gmtAdjustment));
                                        //}

                                        //summaryModel.DayTimeDisplay = string.Format("{0} {1}", localDateTime.DayOfWeek, localDateTime.ToShortTimeString());
                                        //summaryModel.DayTimeID = string.Format("{0}_{1}", localDateTime.DayOfWeek, localDateTime.Hour);

                                        ////Get Specific Day Part Data
                                        //List<DayPartDataItem> dayPartItem = dayPartData == null ? new List<DayPartDataItem>() : dayPartData.Where(x => x.DayOfWeek == localDateTime.DayOfWeek && localDateTime.Hour == x.HourOfDay).ToList();

                                        //summaryModel.DayPartDisplay = (dayPartItem.Any() && !String.IsNullOrEmpty(dayPartItem.First().DayPartName)) ? dayPartItem.First().DayPartName : "Other";
                                        //summaryModel.DayPartID = (dayPartItem.Any() && !String.IsNullOrEmpty(dayPartItem.First().DayPartCode)) ? dayPartItem.First().DayPartCode : "Other";
                                    }

                                    if (dt.Columns.Contains("GMTDateTime") && !row["GMTDateTime"].Equals(DBNull.Value))
                                    {
                                        var gmtDT = Convert.ToDateTime(row["GMTDateTime"]);
                                        summaryModel.GMTDateTime = gmtDT;

                                        if (dt.Columns.Contains("DayDate") && row["DayDate"].Equals(DBNull.Value))
                                        {
                                            summaryModel.SummaryDateTime = Convert.ToDateTime(row["GMTDateTime"]);
                                        }

                                        summaryModel.DayTimeDisplay = string.Format("{0} {1}", gmtDT.DayOfWeek, gmtDT.ToShortTimeString());
                                        summaryModel.DayTimeID = string.Format("{0}_{1}", gmtDT.DayOfWeek, gmtDT.Hour);
                                    }

                                    if (dt.Columns.Contains("LocalDateTime") && !row["LocalDateTime"].Equals(DBNull.Value))
                                    {
                                        var localDT = Convert.ToDateTime(row["LocalDateTime"]);
                                        summaryModel.LocalDateTime = localDT;

                                        List<DayPartDataItem> dpItem = dayPartData == null ? new List<DayPartDataItem>() : dayPartData.Where(w =>
                                            w.DayOfWeek == localDT.DayOfWeek && w.HourOfDay == localDT.Hour).ToList();
                                        summaryModel.DayPartDisplay = (dpItem.Any() && !string.IsNullOrEmpty(dpItem.First().DayPartName)) ? dpItem.First().DayPartName : "Other";
                                        summaryModel.DayPartID = (dpItem.Any() && !string.IsNullOrEmpty(dpItem.First().DayPartCode)) ? dpItem.First().DayPartCode : "Other";
                                    }

                                    if (dt.Columns.Contains("CampaignID") && !row["CampaignID"].Equals(DBNull.Value))
                                    {
                                        summaryModel.CampaignID = Convert.ToInt64(row["CampaignID"]);
                                    }

                                    if (dt.Columns.Contains("CampaignName") && !row["CampaignName"].Equals(DBNull.Value))
                                    {
                                        summaryModel.CampaignName = Convert.ToString(row["CampaignName"]);
                                    }

                                    if (dt.Columns.Contains("Market") && !row["Market"].Equals(DBNull.Value))
                                    {
                                        summaryModel.Market = Convert.ToString(row["Market"]);
                                    }

                                    if (dt.Columns.Contains("Category") && !row["Category"].Equals(DBNull.Value))
                                    {
                                        if (tab == SecondaryTabID.Networks) summaryModel.Networks = Convert.ToString(row["Category"]);
                                        if (tab == SecondaryTabID.Shows) summaryModel.Shows = Convert.ToString(row["Category"]);
                                    }

                                    if (dt.Columns.Contains("NoOfDocs") && !row["NoOfDocs"].Equals(DBNull.Value))
                                    {
                                        summaryModel.Number_Docs = Convert.ToInt64(row["NoOfDocs"]);
                                    }

                                    if (dt.Columns.Contains("NoOfHits") && !row["NoOfHits"].Equals(DBNull.Value))
                                    {
                                        summaryModel.Number_Of_Hits = Convert.ToInt64(row["NoOfHits"]);
                                    }

                                    if (dt.Columns.Contains("Audience") && !row["Audience"].Equals(DBNull.Value))
                                    {
                                        summaryModel.Audience = Convert.ToInt64(row["Audience"]);
                                    }

                                    if (dt.Columns.Contains("IQMediaValue") && !row["IQMediaValue"].Equals(DBNull.Value))
                                    {
                                        summaryModel.IQMediaValue = Convert.ToDecimal(row["IQMediaValue"]);
                                    }

                                    if (dt.Columns.Contains("PositiveSentiment") && !row["PositiveSentiment"].Equals(DBNull.Value))
                                    {
                                        summaryModel.PositiveSentiment = Convert.ToInt64(row["PositiveSentiment"]);
                                    }

                                    if (dt.Columns.Contains("NegativeSentiment") && !row["NegativeSentiment"].Equals(DBNull.Value))
                                    {
                                        summaryModel.NegativeSentiment = Convert.ToInt64(row["NegativeSentiment"]);
                                    }

                                    if (dt.Columns.Contains("ID"))
                                    {
                                        summaryModel.SearchRequestID = Convert.ToInt64(row["ID"]);
                                    }

                                    if (dt.Columns.Contains("Query_Name") && !row["Query_Name"].Equals(DBNull.Value))
                                    {
                                        summaryModel.Query_Name = Convert.ToString(row["Query_Name"]);
                                    }

                                    if (dt.Columns.Contains("SubMediaType") && !row["SubMediaType"].Equals(DBNull.Value))
                                    {
                                        summaryModel.SubMediaType = Convert.ToString(row["SubMediaType"]);
                                    }

                                    if (dt.Columns.Contains("ReadEarned") && !row["ReadEarned"].Equals(DBNull.Value))
                                    {
                                        summaryModel.ReadEarned = Convert.ToInt64(row["ReadEarned"]);
                                    }

                                    if (dt.Columns.Contains("SeenEarned") && !row["SeenEarned"].Equals(DBNull.Value))
                                    {
                                        summaryModel.SeenEarned = Convert.ToInt64(row["SeenEarned"]);
                                    }

                                    if (dt.Columns.Contains("SeenPaid") && !row["SeenPaid"].Equals(DBNull.Value))
                                    {
                                        summaryModel.SeenPaid = Convert.ToInt64(row["SeenPaid"]);
                                    }

                                    if (dt.Columns.Contains("HeardEarned") && !row["HeardEarned"].Equals(DBNull.Value))
                                    {
                                        summaryModel.HeardEarned = Convert.ToInt64(row["HeardEarned"]);
                                    }

                                    if (dt.Columns.Contains("HeardPaid") && !row["HeardPaid"].Equals(DBNull.Value))
                                    {
                                        summaryModel.HeardPaid = Convert.ToInt64(row["HeardPaid"]);
                                    }

                                    summaryModel.MaleAudience = 0;
                                    summaryModel.FemaleAudience = 0;

                                    if (dt.Columns.Contains("AM18_20") && !row["AM18_20"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AM18_20 = Convert.ToInt64(row["AM18_20"]);
                                        summaryModel.MaleAudience += Convert.ToInt64(row["AM18_20"]);
                                    }

                                    if (dt.Columns.Contains("AM21_24") && !row["AM21_24"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AM21_24 = Convert.ToInt64(row["AM21_24"]);
                                        summaryModel.MaleAudience += Convert.ToInt64(row["AM21_24"]);
                                    }

                                    if (dt.Columns.Contains("AM25_34") && !row["AM25_34"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AM25_34 = Convert.ToInt64(row["AM25_34"]);
                                        summaryModel.MaleAudience += Convert.ToInt64(row["AM25_34"]);
                                    }

                                    if (dt.Columns.Contains("AM35_49") && !row["AM35_49"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AM35_49 = Convert.ToInt64(row["AM35_49"]);
                                        summaryModel.MaleAudience += Convert.ToInt64(row["AM35_49"]);
                                    }

                                    if (dt.Columns.Contains("AM50_54") && !row["AM50_54"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AM50_54 = Convert.ToInt64(row["AM50_54"]);
                                        summaryModel.MaleAudience += Convert.ToInt64(row["AM50_54"]);
                                    }

                                    if (dt.Columns.Contains("AM55_64") && !row["AM55_64"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AM55_64 = Convert.ToInt64(row["AM55_64"]);
                                        summaryModel.MaleAudience += Convert.ToInt64(row["AM55_64"]);
                                    }

                                    if (dt.Columns.Contains("AM65_Plus") && !row["AM65_Plus"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AM65_Plus = Convert.ToInt64(row["AM65_Plus"]);
                                        summaryModel.MaleAudience += Convert.ToInt64(row["AM65_Plus"]);
                                    }

                                    if (dt.Columns.Contains("AF18_20") && !row["AF18_20"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AF18_20 = Convert.ToInt64(row["AF18_20"]);
                                        summaryModel.FemaleAudience += Convert.ToInt64(row["AF18_20"]);
                                    }

                                    if (dt.Columns.Contains("AF21_24") && !row["AF21_24"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AF21_24 = Convert.ToInt64(row["AF21_24"]);
                                        summaryModel.FemaleAudience += Convert.ToInt64(row["AF21_24"]);
                                    }

                                    if (dt.Columns.Contains("AF25_34") && !row["AF25_34"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AF25_34 = Convert.ToInt64(row["AF25_34"]);
                                        summaryModel.FemaleAudience += Convert.ToInt64(row["AF25_34"]);
                                    }

                                    if (dt.Columns.Contains("AF35_49") && !row["AF35_49"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AF35_49 = Convert.ToInt64(row["AF35_49"]);
                                        summaryModel.FemaleAudience += Convert.ToInt64(row["AF35_49"]);
                                    }

                                    if (dt.Columns.Contains("AF50_54") && !row["AF50_54"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AF50_54 = Convert.ToInt64(row["AF50_54"]);
                                        summaryModel.FemaleAudience += Convert.ToInt64(row["AF50_54"]);
                                    }

                                    if (dt.Columns.Contains("AF55_64") && !row["AF55_64"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AF55_64 = Convert.ToInt64(row["AF55_64"]);
                                        summaryModel.FemaleAudience += Convert.ToInt64(row["AF55_64"]);
                                    }

                                    if (dt.Columns.Contains("AF65_Plus") && !row["AF65_Plus"].Equals(DBNull.Value))
                                    {
                                        summaryModel.AF65_Plus = Convert.ToInt64(row["AF65_Plus"]);
                                        summaryModel.FemaleAudience += Convert.ToInt64(row["AF65_Plus"]);
                                    }

                                    dataModel.SummaryDataList.Add(summaryModel);
                                }
                                break;
                            case "DMAMap":
                                foreach (DataRow datarow in dt.Rows)
                                {
                                    AnalyticsMapSummaryModel mapSummaryModel = new AnalyticsMapSummaryModel();

                                    if (dt.Columns.Contains("DMA_Name") && !datarow["DMA_Name"].Equals(DBNull.Value))
                                    {
                                        mapSummaryModel.DMAName = Convert.ToString(datarow["DMA_Name"]);
                                    }

                                    if (dt.Columns.Contains("Mentions") && !datarow["Mentions"].Equals(DBNull.Value))
                                    {
                                        mapSummaryModel.NumberOfHits = Convert.ToInt64(datarow["Mentions"]);
                                    }

                                    if (dt.Columns.Contains("SearchRequestID") && !datarow["SearchRequestID"].Equals(DBNull.Value))
                                    {
                                        mapSummaryModel.SearchRequestID = Convert.ToInt64(datarow["SearchRequestID"]);
                                    }

                                    dataModel.DmaMentionMapList.Add(mapSummaryModel);
                                }
                                break;
                            case "CanadaMap":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    AnalyticsMapSummaryModel mapSummaryModel = new AnalyticsMapSummaryModel();

                                    if (dt.Columns.Contains("Province") && !dr["Province"].Equals(DBNull.Value))
                                    {
                                        mapSummaryModel.DMAName = Convert.ToString(dr["Province"]);
                                    }

                                    if (dt.Columns.Contains("Mentions") && !dr["Mentions"].Equals(DBNull.Value))
                                    {
                                        mapSummaryModel.NumberOfHits = Convert.ToInt64(dr["Mentions"]);
                                    }

                                    if (dt.Columns.Contains("SearchRequestID") && !dr["SearchRequestID"].Equals(DBNull.Value))
                                    {
                                        mapSummaryModel.SearchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                                    }

                                    dataModel.CanadaMentionMapList.Add(mapSummaryModel);
                                }
                                break;
                            default:
                                Log4NetLogger.Error(String.Format("Encountered unsupported TableType value when filling Analytics Summary results: {0}", dt.Rows[0]["TableType"]));
                                break;
                        }
                    }
                }
            }

            sw.Stop();
            //Log4NetLogger.Debug(string.Format("DA.FillIQAgentSummary: {0} ms", sw.ElapsedMilliseconds));
            return dataModel;
        }

        private List<AnalyticsSecondaryTable> FillSecondaryTables(DataSet dataSet)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("DA.FillSecondaryTables"));
                List<AnalyticsSecondaryTable> analyticsTables = new List<AnalyticsSecondaryTable>();
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0] != null)
                {
                    //Log4NetLogger.Debug(string.Format("dataSet.Tables[0].Rows.Count: {0}", dataSet.Tables[0].Rows.Count));
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        AnalyticsSecondaryTable table = new AnalyticsSecondaryTable();
                        if (dataSet.Tables[0].Columns.Contains("GroupByHeader") && !dr["GroupByHeader"].Equals(DBNull.Value))
                        {
                            table.GroupByHeader = dr["GroupByHeader"].ToString();
                        }
                        if (dataSet.Tables[0].Columns.Contains("ColumnHeaders") && !dr["ColumnHeaders"].Equals(DBNull.Value))
                        {
                            string rawHeaders = dr["ColumnHeaders"].ToString();
                            table.ColumnHeaders = rawHeaders.Split(',').ToList();
                        }
                        if (dataSet.Tables[0].Columns.Contains("ColumnHeadersLR") && !dr["ColumnHeadersLR"].Equals(DBNull.Value))
                        {
                            string rawHeaders = dr["ColumnHeadersLR"].ToString();
                            table.ColumnHeadersLR = rawHeaders.Split(',').ToList();
                        }
                        if (dataSet.Tables[0].Columns.Contains("ColumnHeadersAds") && !dr["ColumnHeadersAds"].Equals(DBNull.Value))
                        {
                            string rawHeaders = dr["ColumnHeadersAds"].ToString();
                            table.ColumnHeadersAds = rawHeaders.Split(',').ToList();
                        }
                        if (dataSet.Tables[0].Columns.Contains("ColumnHeadersAdsLR") && !dr["ColumnHeadersAdsLR"].Equals(DBNull.Value))
                        {
                            string rawHeaders = dr["ColumnHeadersAdsLR"].ToString();
                            table.ColumnHeadersAdsLR = rawHeaders.Split(',').ToList();
                        }
                        if (dataSet.Tables[0].Columns.Contains("GroupBy") && !dr["GroupBy"].Equals(DBNull.Value))
                        {
                            table.GroupBy = dr["GroupBy"].ToString();
                        }
                        if (dataSet.Tables[0].Columns.Contains("GroupByDisplay") && !dr["GroupByDisplay"].Equals(DBNull.Value))
                        {
                            table.GroupByDisplay = dr["GroupByDisplay"].ToString();
                        }
                        if (dataSet.Tables[0].Columns.Contains("TabDisplay") && !dr["TabDisplay"].Equals(DBNull.Value))
                        {
                            table.TabDisplay = dr["TabDisplay"].ToString();
                        }
                        if (dataSet.Tables[0].Columns.Contains("PageType") && !dr["PageType"].Equals(DBNull.Value))
                        {
                            table.PageType = dr["PageType"].ToString();
                        }

                        analyticsTables.Add(table);
                    }
                }

                return analyticsTables;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                throw;
            }
        }

        private List<AnalyticsActiveElement> FillActiveElements(DataSet dataSet)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("DA.FillActiveElements"));
                List<AnalyticsActiveElement> analyticsTables = new List<AnalyticsActiveElement>();
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0] != null)
                {
                    //Log4NetLogger.Debug(string.Format("dataSet.Tables[0].Rows.Count: {0}", dataSet.Tables[0].Rows.Count));
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        AnalyticsActiveElement element = new AnalyticsActiveElement();
                        if (dataSet.Tables[0].Columns.Contains("ActivePage") && !dr["ActivePage"].Equals(DBNull.Value))
                        {
                            element.ActivePage = dr["ActivePage"].ToString();
                        }
                        if (dataSet.Tables[0].Columns.Contains("ElementSelector") && !dr["ElementSelector"].Equals(DBNull.Value))
                        {
                            element.ElementSelector = dr["ElementSelector"].ToString();
                        }
                        if (dataSet.Tables[0].Columns.Contains("ElementSelectorID") && !dr["ElementSelectorID"].Equals(DBNull.Value))
                        {
                            element.ElementSelectorID = dr["ElementSelectorID"].ToString();
                        }
                        if (dataSet.Tables[0].Columns.Contains("ActiveTabs") && !dr["ActiveTabs"].Equals(DBNull.Value))
                        {
                            string rawHeaders = dr["ActiveTabs"].ToString();
                            element.ActiveTabs = rawHeaders.Split(',').ToList();
                        }
                        if (dataSet.Tables[0].Columns.Contains("IsActiveWithPESH") && !dr["IsActiveWithPESH"].Equals(DBNull.Value))
                        {
                            string rawHeaders = dr["IsActiveWithPESH"].ToString();
                            element.IsActiveWithPESH = Boolean.Parse(rawHeaders);
                        }
                        if (dataSet.Tables[0].Columns.Contains("IsActiveWithMaps") && !dr["IsActiveWithMaps"].Equals(DBNull.Value))
                        {
                            string rawHeaders = dr["IsActiveWithMaps"].ToString();
                            element.IsActiveWithMaps = Boolean.Parse(rawHeaders);
                        }
                        if (dataSet.Tables[0].Columns.Contains("IsActiveWithLineCharts") && !dr["IsActiveWithLineCharts"].Equals(DBNull.Value))
                        {
                            string rawHeaders = dr["IsActiveWithLineCharts"].ToString();
                            element.IsActiveWithLineCharts = Boolean.Parse(rawHeaders);
                        }
                        if (dataSet.Tables[0].Columns.Contains("IsActiveWithOtherCharts") && !dr["IsActiveWithOtherCharts"].Equals(DBNull.Value))
                        {
                            string rawHeaders = dr["IsActiveWithOtherCharts"].ToString();
                            element.IsActiveWithOtherCharts = Boolean.Parse(rawHeaders);
                        }
                        if (dataSet.Tables[0].Columns.Contains("HiddenTabs") && !dr["HiddenTabs"].Equals(DBNull.Value))
                        {
                            string rawHeaders = dr["HiddenTabs"].ToString();
                            element.HiddenTabs = rawHeaders.Split(',').ToList();
                        }

                        analyticsTables.Add(element);
                    }
                }

                return analyticsTables;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                throw;
            }
        }

        public List<DayPartDataItem> GetDayPartData(string AffiliateCode)
        {
            try
            {
                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@AffiliateCode", DbType.String, AffiliateCode, ParameterDirection.Input));
                DataSet dataSet = DataAccess.GetDataSet("usp_v5_IQAgent_Analytics_GetDayPartData", dataTypes);
                return FillDayPartItems(dataSet);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        private List<DayPartDataItem> FillDayPartItems(DataSet dataSet)
        {
            var dataItemList = new List<DayPartDataItem>();
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0] != null)
            {
                foreach (DataRow datarow in dataSet.Tables[0].Rows)
                {
                    var dataItem = new DayPartDataItem();

                    if (dataSet.Tables[0].Columns.Contains("AffiliateCode") && !datarow["AffiliateCode"].Equals(DBNull.Value))
                    {
                        dataItem.AffiliateCode = datarow["AffiliateCode"].ToString();
                    }
                    if (dataSet.Tables[0].Columns.Contains("DayPartName") && !datarow["DayPartName"].Equals(DBNull.Value))
                    {
                        dataItem.DayPartName = datarow["DayPartName"].ToString();
                    }
                    if (dataSet.Tables[0].Columns.Contains("DayPartCode") && !datarow["DayPartCode"].Equals(DBNull.Value))
                    {
                        dataItem.DayPartCode = datarow["DayPartCode"].ToString();
                    }
                    if (dataSet.Tables[0].Columns.Contains("DayOfWeek") && !datarow["DayOfWeek"].Equals(DBNull.Value))
                    {
                        dataItem.DayOfWeek = (DayOfWeek)(Int32.Parse(datarow["DayOfWeek"].ToString()) - 1);
                    }
                    if (dataSet.Tables[0].Columns.Contains("HourOfDay") && !datarow["HourOfDay"].Equals(DBNull.Value))
                    {
                        dataItem.HourOfDay = Int32.Parse(datarow["HourOfDay"].ToString());
                    }

                    dataItemList.Add(dataItem);
                }
            }

            return dataItemList;
        }

        private List<AnalyticsCampaign> FillAnalyticsCampaigns(DataSet dataSet)
        {
            try
            {
                List<AnalyticsCampaign> lstCampaigns = new List<AnalyticsCampaign>();
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0] != null)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        lstCampaigns.Add(FillAnalyticsCampaign(dr));
                    }
                }

                return lstCampaigns;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                throw;
            }
        }

        private AnalyticsCampaign FillAnalyticsCampaign(DataRow dr)
        {
            try
            {
                AnalyticsCampaign campaign = new AnalyticsCampaign();
                if (!dr["ID"].Equals(DBNull.Value))
                {
                    campaign.CampaignID = Convert.ToInt64(dr["ID"]);
                }
                if (!dr["Name"].Equals(DBNull.Value))
                {
                    campaign.CampaignName = Convert.ToString(dr["Name"]);
                }
                if (!dr["SearchRequestID"].Equals(DBNull.Value))
                {
                    campaign.SearchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                }
                if (!dr["Query_Name"].Equals(DBNull.Value))
                {
                    campaign.QueryName = Convert.ToString(dr["Query_Name"]);
                }
                if (!dr["StartDatetime"].Equals(DBNull.Value))
                {
                    campaign.StartDate = Convert.ToDateTime(dr["StartDatetime"]);
                }
                if (!dr["EndDatetime"].Equals(DBNull.Value))
                {
                    campaign.EndDate = Convert.ToDateTime(dr["EndDatetime"]);
                }
                if (!dr["Query_Version"].Equals(DBNull.Value))
                {
                    campaign.QueryVersion = Convert.ToString(dr["Query_Version"]);
                }
                if (!dr["ModifiedDate"].Equals(DBNull.Value))
                {
                    campaign.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                }
                if (!dr["IsActive"].Equals(DBNull.Value))
                {
                    campaign.IsActive = Convert.ToInt64(dr["IsActive"]);
                }

                return campaign;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                throw;
            }
        }
    }
}
