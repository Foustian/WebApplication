using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data;
using IQMedia.Model;
using System.Xml.Linq;
using IQMedia.Shared.Utility;
using System.Globalization;
using IQCommon.Model;

namespace IQMedia.Web.Logic
{
    public class DashboardLogic : IQMedia.Web.Logic.Base.ILogic
    {

        #region Get Data

        public IQAgent_DashBoardModel GetDaySummaryDataDayWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, List<IQCommon.Model.IQ_MediaTypeModel> p_MediaTypeList)
        {
            var mediaTypeXml = GetXmlOfMediaType(p_MediaTypeList);

            var listSPName = GetListSPNameOfMediaType(p_MediaTypeList, p_Medium);

            DashboardDA dashboardDA = new DashboardDA();

            IQAgent_DashBoardModel lstIQAgent_DaySummaryModel = dashboardDA.GetDaySummaryDataDayWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, p_SearchRequestXml, mediaTypeXml, listSPName);

            return lstIQAgent_DaySummaryModel;
        }        

        public IQAgent_DashBoardModel GetDaySummaryDataMonthWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, List<IQCommon.Model.IQ_MediaTypeModel> p_MediaTypeList)
        {
            var mediaTypeXml = GetXmlOfMediaType(p_MediaTypeList);

            var listSPName = GetListSPNameOfMediaType(p_MediaTypeList, p_Medium);

            DashboardDA dashboardDA = new DashboardDA();

            IQAgent_DashBoardModel iQAgent_DashBoardModel = dashboardDA.GetDaySummaryDataMonthWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, p_SearchRequestXml, mediaTypeXml, listSPName);

            return iQAgent_DashBoardModel;
        }

        public IQAgent_DashBoardModel GetHourSummaryDataHourWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, List<IQCommon.Model.IQ_MediaTypeModel> p_MediaTypeList)
        {
            var mediaTypeXml = GetXmlOfMediaType(p_MediaTypeList);

            var listSPName = GetListSPNameOfMediaType(p_MediaTypeList, p_Medium);

            DashboardDA dashboardDA = new DashboardDA();

            IQAgent_DashBoardModel iQAgent_DashBoardModel = dashboardDA.GetHourSummaryDataHourWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, p_SearchRequestXml, mediaTypeXml, listSPName);

            return iQAgent_DashBoardModel;
        }


        public List<IQAgent_DaySummaryModel> GetDmaSummaryDataDayWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_DmaXml)
        {
            DashboardDA dashboardDA = new DashboardDA();

            List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = dashboardDA.GetDmaSummaryDataDayWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, p_SearchRequestXml, p_DmaXml);

            return lstIQAgent_DaySummaryModel;
        }

        public List<IQAgent_DaySummaryModel> GetProvinceSummaryDataDayWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_ProvinceXml)
        {
            DashboardDA dashboardDA = new DashboardDA();

            List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = dashboardDA.GetProvinceSummaryDataDayWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, p_SearchRequestXml, p_ProvinceXml);

            return lstIQAgent_DaySummaryModel;
        }

        public List<IQAgent_DaySummaryModel> GetDmaSummaryDataMonthWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_DmaXml)
        {
            DashboardDA dashboardDA = new DashboardDA();

            List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = dashboardDA.GetDmaSummaryDataMonthWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, p_SearchRequestXml, p_DmaXml);

            return lstIQAgent_DaySummaryModel;
        }

        public List<IQAgent_DaySummaryModel> GetProvinceSummaryDataMonthWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_ProvinceXml)
        {
            DashboardDA dashboardDA = new DashboardDA();

            List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = dashboardDA.GetProvinceSummaryDataMonthWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, p_SearchRequestXml, p_ProvinceXml);

            return lstIQAgent_DaySummaryModel;
        }

        public List<IQAgent_DaySummaryModel> GetDmaSummaryDataHourWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_DmaXml)
        {
            DashboardDA dashboardDA = new DashboardDA();

            List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = dashboardDA.GetDmaSummaryDataHourWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, p_SearchRequestXml, p_DmaXml);

            return lstIQAgent_DaySummaryModel;
        }

        public List<IQAgent_DaySummaryModel> GetProvinceSummaryDataHourWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_ProvinceXml)
        {
            DashboardDA dashboardDA = new DashboardDA();

            List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = dashboardDA.GetProvinceSummaryDataHourWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, p_SearchRequestXml, p_ProvinceXml);

            return lstIQAgent_DaySummaryModel;
        }

        public IQAgent_DashBoardModel GetAdhocSummaryData(string p_MediaIDXml, string p_Source, string p_Medium, Guid p_ClientGUID, List<IQCommon.Model.IQ_MediaTypeModel> p_MediaTypeList)
        {
            DashboardDA dashboardDA = new DashboardDA();

            var listSPName = "";
            /*
            switch (p_Medium)
            {
                case "PM":
                    p_Medium = "PR";
                    break;
                case "PQ":
                    p_Medium = "PR";
                    break;
                case "Blog":
                    p_Medium = "BL";
                    break;
                case "Forum":
                    p_Medium = "FO";
                    break;
                case "TW":
                    p_Medium = "SM";
                    break;
            }*/

            if (!string.IsNullOrEmpty(p_Medium))
            {
                // Medium can be "Overview" and "MS". So Single() cannot be used.
                var mediatype = p_MediaTypeList.Where(mt => string.Compare(mt.MediaType, p_Medium, true) == 0 && mt.TypeLevel == 1).FirstOrDefault();

                if (mediatype != null)
                {
                    listSPName = mediatype.DashboardData.ArchiveListSPName;    
                }                
            }

            string xmlMediaTypes = GetXmlOfMediaType(p_MediaTypeList);

            IQAgent_DashBoardModel lstIQAgent_DaySummaryModel = dashboardDA.GetAdhocSummaryData(p_MediaIDXml, p_Source, p_Medium, p_ClientGUID, listSPName, xmlMediaTypes);

            // Remove when mediatypes properly set
            /*lstIQAgent_DaySummaryModel.ListOfIQAgentSummary.ForEach(delegate(IQAgent_DaySummaryModel dsm) {

                switch (dsm.SubMediaType)
                {
                    case "PM":
                        dsm.MediaType = "PR";
                        break;
                    case "PQ":
                        dsm.MediaType = "PR";
                        break;
                    case "Blog":
                        dsm.MediaType = "BL";
                        break;
                    case "Forum":
                        dsm.MediaType = "FO";
                        break;     
                    case "TW":
                        dsm.MediaType = "SM";
                        break;
                }
            });*/

            return lstIQAgent_DaySummaryModel;
        }

        #endregion

        #region Overview

        public List<SummaryReportModel> GetSummaryReportData(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate)
        {
            DashboardDA dashboardDA = new DashboardDA();

            List<SummaryReportModel> lstSummaryReport = dashboardDA.GetSummaryReportData(p_ClientGUID, p_FromDate, p_ToDate);

            return lstSummaryReport;
        }



        #region DayWise Chart

        /*

        public SummaryReportMulti LineChart(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, bool p_Isv4TMAccess, List<string> p_SearchRequestIDs, List<IQAgent_ComparisionValues> p_ListOfIQAgent_ComparisionValues,
            bool p_Isv4NMAccess, bool p_Isv4SMAccess, bool p_Isv4TWAccess, bool p_Isv4TVAccess, bool p_Isv4BLPMAccess, bool p_NielsenAccess, bool p_CompeteDataAccess)
        {
            try
            {
                List<DateTime> dateRange = new List<DateTime>();
                TimeSpan ts = p_ToDate.Subtract(p_FromDate);
                for (int i = 0; i <= ts.Days; i++)
                {
                    dateRange.Add(p_FromDate.AddDays(i));
                }

                SummaryReportMulti lstSummaryReportMulti = new SummaryReportMulti();


                //var mediaRecords = listOfSummaryReportData.GroupBy(sr => sr.GMT_DateTime).Select(tr => new SummaryReportModel { GMT_DateTime = tr.Key, Number_Docs = tr.Sum(r => r.Number_Docs) });
                //var subMediaRecords = listOfSummaryReportData.GroupBy(sr => new { sr.SubMediaType, sr.GMT_DateTime }).Select(tr => new SummaryReportModel { SubMediaType = tr.Key.SubMediaType, GMT_DateTime = tr.Key.GMT_DateTime, Number_Docs = tr.Sum(r => r.Number_Docs) });
                //var audienceRecords = listOfSummaryReportData.GroupBy(sr => sr.GMT_DateTime).Select(tr => new SummaryReportModel { GMT_DateTime = tr.Key, Audience = tr.Sum(r => r.Audience) });
                //var iqMediaValueRecords = listOfSummaryReportData.GroupBy(sr => sr.GMT_DateTime).Select(tr => new SummaryReportModel { GMT_DateTime = tr.Key, IQMediaValue = tr.Sum(r => r.IQMediaValue) });

                // this is chART FOR SUMMARY REPORT BY IQAGENT REQUESTS.
                Chart chart = new Chart();
                chart.subcaption = "";
                chart.caption = string.Empty;// "Summary Report";
                chart.linethickness = "1";
                chart.showvalues = "0";
                //chart.showLabels = "0";
                //chart.showYAxisValues = "0";
                chart.formatnumberscale = "0";
                chart.anchorRadius = "3";
                chart.divlinealpha = "20";
                chart.divlinecolor = "000000";
                chart.divlineisdashed = "0";
                chart.showalternatehgridcolor = "1";
                chart.alternatehgridcolor = "FFFFFF";
                chart.shadowalpha = "40";
                chart.labelstep = "1";
                chart.numvdivlines = "0";
                chart.chartrightmargin = "10";
                chart.bgcolor = "FFFFFF";
                chart.bgangle = "270";
                chart.bgalpha = "10,10";
                chart.alternatehgridalpha = "5";
                chart.legendposition = "BOTTOM";
                chart.drawAnchors = "1";
                chart.showBorder = "0";
                chart.canvasBorderAlpha = "0";
                //chart.lineColor = "#4493D6";

                //Multi Line chart
                // THIS IS CHART BY SUB MEDIA TYPE. 
                Chart multiChart = new Chart();
                multiChart.subcaption = "";
                multiChart.caption = "";
                multiChart.linethickness = "1";
                //multiChart.showLabels = "0";
                multiChart.showvalues = "0";
                //multiChart.showYAxisValues = "0";
                multiChart.formatnumberscale = "0";
                multiChart.anchorRadius = "3";
                multiChart.divlinealpha = "100";
                multiChart.divlinecolor = "000000";
                multiChart.divlineisdashed = "0";
                multiChart.showalternatehgridcolor = "1";
                multiChart.alternatehgridcolor = "FFFFFF";
                multiChart.shadowalpha = "40";
                multiChart.labelstep = "1";
                multiChart.numvdivlines = "0";
                multiChart.chartrightmargin = "10";
                multiChart.bgcolor = "FFFFFF";
                multiChart.bgangle = "270";
                multiChart.bgalpha = "10,10";
                multiChart.alternatehgridalpha = "5";
                multiChart.legendposition = "BOTTOM";
                multiChart.drawAnchors = "1";
                multiChart.showBorder = "0";
                multiChart.canvasBorderAlpha = "0";
                multiChart.palettecolors = "15335D,448FF2,7A045C,FFB451,E14A02,394900,005E8F,A7B1B3";
                //multiChart.lineColor = "#4493D6";


                //LineChartOutput multiLineChartOutput = new LineChartOutput();
                //multiLineChartOutput.chart = multiChart;
                //

                //Media Records

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();

                foreach (var date in dateRange)
                {

                    Category2 category2 = new Category2();

                    category2.label = date.ToShortDateString();

                    allCategory.category.Add(category2);

                }

                lstallCategory.Add(allCategory);

                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();
                seriesData.color = "";

                SparkChart sparkChartMediaWise = new SparkChart();
                SparkChartOutput sparkChartOutputMediaWise = new SparkChartOutput();
                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    //Multi Line Charts
                    List<SeriesData> multiLstSeriesData = new List<SeriesData>();
                    foreach (var searchRequest in p_SearchRequestIDs)
                    {
                        // Spark Chart Series Init
                        sparkChartOutputMediaWise.chart = sparkChartMediaWise;
                        List<SparkSeriesData> lstSparkSeriesDataMediaWise = new List<SparkSeriesData>();
                        SparkSeriesData sparkSeriesMediaWise = new SparkSeriesData();
                        sparkSeriesMediaWise.data = new List<SparkDatum>();
                        //sparkChartMediaWise.caption = CommonFunctions.GetEnumDescription(subMedia);


                        var SearchRequest = listOfSummaryReportData.Where(a => string.Compare(a.SearchRequestID.ToString(), searchRequest) == 0).FirstOrDefault();
                        if (SearchRequest != null)
                        {


                            SeriesData multiSeriesData = new SeriesData();
                            multiSeriesData.data = new List<Datum>();

                            multiSeriesData.seriesname = SearchRequest.Query_Name;
                            multiSeriesData.color = "";
                            //multiSeriesData.anchorBorderColor = "";
                            //multiSeriesData.anchorBgColor = "";

                            foreach (var item in dateRange)
                            {
                                //var singleSubMediaRec =
                                var daywiseSum = listOfSummaryReportData.Where(smr => String.Compare(smr.SearchRequestID.ToString(), searchRequest, true) == 0 && smr.GMT_DateTime.Equals(item)
                                        && (
                                            (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                            (p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) &&
                                            (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) &&
                                            (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) &&
                                            (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) &&
                                            (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                            (p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString()) &&
                                            (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))
                                    ).Sum(s => s.Number_Docs);

                                Datum datum = new Datum();
                                datum.value = Convert.ToString(daywiseSum != null ? daywiseSum : 0);
                                datum.link = "javascript:OpenFeed('" + item.ToShortDateString() + "','','','[" + searchRequest + "]','[\"" + System.Web.HttpUtility.UrlEncode(SearchRequest.Query_Name.Replace("\"", "\\\"")) + "\"]')";

                                multiSeriesData.data.Add(datum);


                                SparkDatum sparkDatum = new SparkDatum();
                                sparkDatum.value = Convert.ToString(daywiseSum != null ? daywiseSum : 0);
                                sparkSeriesMediaWise.data.Add(sparkDatum);
                            }



                            lstSparkSeriesDataMediaWise.Add(sparkSeriesMediaWise);
                            sparkChartOutputMediaWise.dataset = lstSparkSeriesDataMediaWise;

                            //Multi Line Charts
                            multiLstSeriesData.Add(multiSeriesData);
                        }
                    }
                    lineChartOutput.categories = lstallCategory;
                    lineChartOutput.dataset = multiLstSeriesData;
                }
                else
                {
                    seriesData.seriesname = "Media";

                    //seriesData.drawAnchors = "1";
                    //seriesData.anchorRadius = "10";
                    //seriesData.anchorBorderColor = "#DCDCDC";
                    //seriesData.anchorBgColor = "#DCDCDC";
                    //seriesData.anchorAlpha = "100";

                    foreach (var item in dateRange)
                    {
                        //var singleSubMediaRec =
                        var sumOfDocs = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Equals(item)
                            && (
                                    (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                    (p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) &&
                                    (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                    (p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() &&
                                    (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))
                                )
                            ).Sum(s => s.Number_Docs);// SingleOrDefault();

                        Datum datum = new Datum();
                        datum.value = Convert.ToString(sumOfDocs != null ? sumOfDocs : 0);
                        datum.link = "javascript:OpenFeed('" + item.ToShortDateString() + "','','','','')";
                        seriesData.data.Add(datum);
                    }
                    lstSeriesData.Add(seriesData);

                    lineChartOutput.categories = lstallCategory;
                    lineChartOutput.dataset = lstSeriesData;
                }

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                lstSummaryReportMulti.MediaRecords = jsonResult;


                //Sub Media Recors

                lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = multiChart;

                var distinctSubMedia = listOfSummaryReportData.Select(d => d.SubMediaType).Distinct().ToList();
                lstSeriesData = new List<SeriesData>();

                List<CommonFunctions.CategoryType> lstMediaCategories = Enum.GetValues(typeof(CommonFunctions.CategoryType)).Cast<CommonFunctions.CategoryType>().ToList();
                Int64 totNumOfHits = listOfSummaryReportData.Where(smr =>
                                    (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                    (p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) &&
                                    (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                    (p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() &&
                                    (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))
                                ).Sum(s => s.Number_Docs);
                lstSummaryReportMulti.TotalNumOfHits = totNumOfHits.ToString("N0");

                //Spark Chart Init
                sparkChartMediaWise = new SparkChart();

                sparkChartOutputMediaWise = new SparkChartOutput();

                string searchRequestIds = string.Empty;
                string searchRequestNames = string.Empty;

                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    searchRequestIds = Newtonsoft.Json.JsonConvert.SerializeObject(listOfSummaryReportData.Select(a => a.SearchRequestID).Distinct());
                    searchRequestNames = Newtonsoft.Json.JsonConvert.SerializeObject(listOfSummaryReportData.Select(a => a.Query_Name).Distinct());
                }

                foreach (var subMedia in lstMediaCategories)
                {
                    if (
                        (p_Isv4TMAccess || subMedia != CommonFunctions.CategoryType.Radio) &&
                        (p_Isv4NMAccess || subMedia != CommonFunctions.CategoryType.NM) &&
                        (p_Isv4SMAccess || subMedia != CommonFunctions.CategoryType.SocialMedia) &&
                        (p_Isv4SMAccess || subMedia != CommonFunctions.CategoryType.Forum) &&
                        (p_Isv4SMAccess || subMedia != CommonFunctions.CategoryType.Blog) &&
                        (p_Isv4TWAccess || subMedia != CommonFunctions.CategoryType.TW) &&
                        (p_Isv4TVAccess || subMedia != CommonFunctions.CategoryType.TV) &&
                        (p_Isv4BLPMAccess || subMedia != CommonFunctions.CategoryType.PM))
                    {
                        //}
                        //foreach (string subMedia in distinctSubMedia)
                        //{

                        // Spark Chart Series Init
                        sparkChartOutputMediaWise.chart = sparkChartMediaWise;
                        List<SparkSeriesData> lstSparkSeriesDataMediaWise = new List<SparkSeriesData>();
                        SparkSeriesData sparkSeriesMediaWise = new SparkSeriesData();
                        sparkSeriesMediaWise.data = new List<SparkDatum>();
                        //sparkChartMediaWise.caption = CommonFunctions.GetEnumDescription(subMedia);


                        // Line Chart Series Init
                        seriesData = new SeriesData();
                        seriesData.data = new List<Datum>();
                        string enumDesc = CommonFunctions.GetEnumDescription(subMedia);
                        //multiChart.caption = enumDesc;
                        seriesData.seriesname = enumDesc;
                        seriesData.color = "";
                        //seriesData.anchorBorderColor = "";
                        //seriesData.anchorBgColor = "";

                        SeriesData multiSeriesData = new SeriesData();
                        multiSeriesData.data = new List<Datum>();

                        multiSeriesData.seriesname = "";
                        multiSeriesData.color = "";
                        //multiSeriesData.anchorBorderColor = "";
                        //multiSeriesData.anchorBgColor = "";


                        //Multi Line Charts
                        List<SeriesData> multiLstSeriesData = new List<SeriesData>();
                        //
                        foreach (var item in dateRange)
                        {
                            //var singleSubMediaRec =
                            var daywiseSum = listOfSummaryReportData.Where(smr => String.Compare(smr.SubMediaType, subMedia.ToString(), true) == 0 && smr.GMT_DateTime.Equals(item)).Sum(s => s.Number_Docs);

                            Datum datum = new Datum();
                            datum.value = Convert.ToString(daywiseSum != null ? daywiseSum : 0);
                            datum.link = "javascript:OpenFeed('" + item.ToShortDateString() + "','" + subMedia.ToString() + "','" + CommonFunctions.GetEnumDescription(subMedia) + "','" + searchRequestIds + "','" + searchRequestNames.Replace("\'", "\\\'") + "')";


                            seriesData.data.Add(datum);
                            multiSeriesData.data.Add(datum);


                            SparkDatum sparkDatum = new SparkDatum();
                            sparkDatum.value = Convert.ToString(daywiseSum != null ? daywiseSum : 0);
                            sparkSeriesMediaWise.data.Add(sparkDatum);
                        }



                        lstSparkSeriesDataMediaWise.Add(sparkSeriesMediaWise);
                        sparkChartOutputMediaWise.dataset = lstSparkSeriesDataMediaWise;

                        //Multi Line Charts
                        multiLstSeriesData.Add(multiSeriesData);
                        //multiLineChartOutput.categories = lstallCategory;
                        //multiLineChartOutput.dataset = multiLstSeriesData;
                        if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.TV.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.TVRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.TVRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.TVPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.TV.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.NM.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.NMRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.NMRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.NMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.NM.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.TW.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.TWRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.TWRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.TWPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.TW.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.Forum.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.ForumRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.ForumRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.ForumPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.Forum.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.SocialMedia.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.SocialMediaRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.SocialMediaRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.SocialMediaPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.SocialMedia.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.Blog.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.BlogRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.BlogRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.BlogPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.Blog.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.PM.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.PMRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.PMRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.PMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.PM.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.Radio.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.TMRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.TMRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.TMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.Radio.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        //

                        lstSeriesData.Add(seriesData);
                    }

                }


                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                lstSummaryReportMulti.SubMediaRecords = jsonResult;

                //Audience
                //multiLineChartOutput = new LineChartOutput();
                //multiLineChartOutput.chart = multiChart;
                //lstSeriesData = new List<SeriesData>();
                //seriesData = new SeriesData();
                //seriesData.data = new List<Datum>();

                //seriesData.seriesname = "";
                //seriesData.color = "";
                //seriesData.anchorbordercolor = "";
                //seriesData.anchorbgcolor = "";
                //foreach (var item in dateRange)
                //{

                //    var singleAudienceRec = audienceRecords.Where(smr => smr.GMT_DateTime.Equals(item)).SingleOrDefault();

                //    Datum datum = new Datum();
                //    datum.value = Convert.ToString(singleAudienceRec != null ? singleAudienceRec.Audience : 0);
                //    seriesData.data.Add(datum);
                //}
                //lstSeriesData.Add(seriesData);
                //multiLineChartOutput.categories = lstallCategory;
                //multiLineChartOutput.dataset = lstSeriesData;

                //lstSummaryReportMulti.AudienceRecords = CommonFunctions.SearializeJson(multiLineChartOutput);
                //lstSummaryReportMulti.AudienceRecordsSum = seriesData.data.Sum(s => Convert.ToInt32(s.value));
                //
                if (p_Isv4TMAccess || p_Isv4NMAccess || p_Isv4SMAccess || p_Isv4TWAccess || p_Isv4TVAccess || p_Isv4BLPMAccess)
                {
                    //IQ Media Value
                    SparkChart sparkChart = new SparkChart();
                    //sparkChart.caption = "Ad Value";
                    sparkChart.palette = "5";
                    sparkChart.setAdaptiveYMin = "0";
                    sparkChart.showCloseAnchor = "0";
                    sparkChart.showCloseValue = "0";
                    sparkChart.showHighAnchor = "0";
                    sparkChart.showHighLowValue = "0";
                    sparkChart.showOpenAnchor = "0";
                    sparkChart.showOpenValue = "0";
                    sparkChart.showLowAnchor = "0";
                    sparkChart.showToolTip = "1";
                    sparkChart.bgColor = "FFFFFF";
                    sparkChart.lineColor = "#4493D6";
                    // chart1.formatNumber = "0";
                    //chart1.thousandSeparator = ",";
                    sparkChart.thousandSeparatorPosition = "0";
                    sparkChart.formatNumberScale = "0";
                    //sparkChart.caption = "Ad Value";
                    SparkChartOutput sparkChartOutput = new SparkChartOutput();
                    sparkChartOutput.chart = sparkChart;
                    List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                    SparkSeriesData sparkSeries = new SparkSeriesData();
                    sparkSeries.data = new List<SparkDatum>();

                    foreach (var item in dateRange)
                    {

                        var singleIQMediaValueRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Equals(item)
                            && (
                                        (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                        ((p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) && p_CompeteDataAccess) &&
                                        (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                        ((p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() && p_NielsenAccess) &&
                                        (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))
                               )
                          ).Sum(s => s.IQMediaValue);// SingleOrDefault();

                        SparkDatum datum = new SparkDatum();
                        datum.value = Convert.ToString(singleIQMediaValueRec != null ? singleIQMediaValueRec : 0);
                        sparkSeries.data.Add(datum);
                    }
                    lstSparkSeriesData.Add(sparkSeries);
                    sparkChartOutput.dataset = lstSparkSeriesData;
                    lstSummaryReportMulti.IQMediaValueRecords = CommonFunctions.SearializeJson(sparkChartOutput);
                    //lstSummaryReportMulti.IQMediaValueRecords = temp;
                    lstSummaryReportMulti.IQMediaValueRecordsSum = (sparkSeries.data.Sum(s => Convert.ToDecimal(s.value)));
                    lstSummaryReportMulti.IQMediaValuePrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(smr => (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                        ((p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) && p_CompeteDataAccess) &&
                                        (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                        ((p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() && p_NielsenAccess) &&
                                        (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))).Sum(a => a.IQMediaValue) : 0;


                    //Audience
                    sparkChartOutput = new SparkChartOutput();
                    //sparkChart.caption = "Views";
                    sparkChartOutput.chart = sparkChart;
                    lstSparkSeriesData = new List<SparkSeriesData>();
                    sparkSeries = new SparkSeriesData();
                    sparkSeries.data = new List<SparkDatum>();
                    foreach (var item in dateRange)
                    {

                        var singleAudienceRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Equals(item)
                            && (
                                        (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                        ((p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) && p_CompeteDataAccess) &&
                                        (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                        ((p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() && p_NielsenAccess) &&
                                        (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))
                                )
                        ).Sum(s => s.Audience);// SingleOrDefault();

                        SparkDatum datum = new SparkDatum();
                        datum.value = Convert.ToString(singleAudienceRec != null ? singleAudienceRec : 0);
                        sparkSeries.data.Add(datum);

                    }
                    lstSparkSeriesData.Add(sparkSeries);
                    sparkChartOutput.dataset = lstSparkSeriesData;
                    lstSummaryReportMulti.AudienceRecords = CommonFunctions.SearializeJson(sparkChartOutput);

                    lstSummaryReportMulti.AudienceRecordsSum = (sparkSeries.data.Sum(s => Convert.ToInt64(s.value)));
                    lstSummaryReportMulti.AudiencePrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(smr => (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                        ((p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) && p_CompeteDataAccess) &&
                                        (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                        ((p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() && p_NielsenAccess) &&
                                        (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))).Sum(a => a.Audience) : 0;
                }

                return lstSummaryReportMulti;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        */

        /*
        public string PieChart(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, bool p_Isv4TMAccess, bool p_Isv4NMAccess, bool p_Isv4SMAccess, bool p_Isv4TWAccess, bool p_Isv4TVAccess, bool p_Isv4BLPMAccess, bool p_NielsenAccess, bool p_CompeteDataAccess)
        {
            try
            {
                //List<PieChartResponse> lstPieChartReponse = new List<PieChartResponse>();

                Piechart piechart = new Piechart();
                piechart.caption = "";
                piechart.showpercentageinlabel = "0";
                piechart.showvalues = "0";
                piechart.showlabels = "0";
                piechart.showlegend = "1";
                piechart.legendPosition = "BOTTOM";
                piechart.bgcolor = "FFFFFF";
                piechart.showBorder = "0";
                piechart.pieRadius = "100";
                piechart.paletteColors = "15335D,448FF2,7A045C,FFB451,E14A02,394900,005E8F,A7B1B3";
                piechart.plotFillAlpha = "75";
                piechart.legendBorderThickness = "0";
                piechart.legendShadow = "0";
                //piechart.

                var subMediaRecords = listOfSummaryReportData.GroupBy(sr => new { sr.SubMediaType }).Select(tr => new SummaryReportModel { SubMediaType = tr.Key.SubMediaType, Number_Docs = tr.Sum(r => r.Number_Docs) });


                //Get Distinct Search Term and By Looping through it , Get Data of Feedclass and its total

                //var distinctSearchTerm = totalRecords.Select(s => s.SearchTerm).Distinct().ToList();

                //foreach (string sTerm in searchTerm)
                //{
                List<PieChartdata> lstPieChartdata = new List<PieChartdata>();
                //  PieChartResponse pieChartReponse = new PieChartResponse();


                List<CommonFunctions.CategoryType> lstMediaCategories = Enum.GetValues(typeof(CommonFunctions.CategoryType)).Cast<CommonFunctions.CategoryType>().ToList();

                foreach (var subMedia in lstMediaCategories)
                {
                    if (
                        (p_Isv4TMAccess || subMedia != CommonFunctions.CategoryType.Radio) &&
                        (p_Isv4NMAccess || subMedia != CommonFunctions.CategoryType.NM) &&
                        (p_Isv4SMAccess || subMedia != CommonFunctions.CategoryType.SocialMedia) &&
                        (p_Isv4SMAccess || subMedia != CommonFunctions.CategoryType.Forum) &&
                        (p_Isv4SMAccess || subMedia != CommonFunctions.CategoryType.Blog) &&
                        (p_Isv4TWAccess || subMedia != CommonFunctions.CategoryType.TW) &&
                        (p_Isv4TVAccess || subMedia != CommonFunctions.CategoryType.TV) &&
                        (p_Isv4BLPMAccess || subMedia != CommonFunctions.CategoryType.PM))
                    {
                        //foreach (var item in subMediaRecords)
                        //{
                        PieChartdata pieChartData = new PieChartdata();
                        var numOfDocs = subMediaRecords.Where(smr => String.Compare(smr.SubMediaType, subMedia.ToString(), true) == 0).Select(tr => tr.Number_Docs).SingleOrDefault();
                        //CommonFunctions.
                        string enumDesc = CommonFunctions.GetEnumDescription(subMedia);
                        Int64 sumNumDocs = numOfDocs != null ? numOfDocs : 0;
                        pieChartData.label = enumDesc + " " + sumNumDocs.ToString("N0");
                        pieChartData.value = Convert.ToInt64(numOfDocs != null ? numOfDocs : 0);
                        lstPieChartdata.Add(pieChartData);
                        // }
                    }
                }


                PieChartOutput pieChartOutput = new PieChartOutput();
                pieChartOutput.pieChart = piechart;
                pieChartOutput.lstPieChartData = lstPieChartdata;

                string jsonResult = CommonFunctions.SearializeJson(pieChartOutput);

                //pieChartReponse.SearchTerm = sTerm;
                //pieChartReponse.JsonResult = jsonResult;
                // lstPieChartReponse.Add(pieChartReponse);
                // }

                return jsonResult;
            }
            catch (Exception)
            {

                throw;
            }
        }*/


        public string HighChartPieChart(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, List<IQ_MediaTypeModel> p_MediaTypeList)
        {
            try
            {
                List<PieChartResponse> lstPieChartResponse = new List<PieChartResponse>();

                // pie chart used for share of records of each medium type
                HighPieChartModel highPieChartModel = new HighPieChartModel();

                // set height and width of pie chart
                highPieChartModel.chart = new PChart() { height = 400, width = 300 };

                // title for pie chart
                highPieChartModel.title = new PTitle()
                {
                    text = "Sources",
                    style = new HStyle
                    {
                        color = "#555555",
                        fontFamily = "Verdana",
                        fontSize = "13px",
                        fontWeight = "bold"
                    }
                };

                // set tooltip format for pie chart
                highPieChartModel.tooltip = new PTooltip() { 
                    pointFormat = "{series.name}: <b>{point.percentage:.1f}%</b>" 
                };

                // set pie chart plotoptions with legend and enable datalabel = false
                highPieChartModel.plotOptions = new PPlotOptions() { 
                    pie = new Pie() { 
                        allowPointSelect = true, 
                        cursor = "pointer", 
                        showInLegend = true, 
                        innerSize = "60%", 
                        dataLabels = new DataLabels() { 
                            enabled = false 
                        } 
                    } 
                };
                highPieChartModel.series = new List<PSeries>();

                // set legend width and layout
                highPieChartModel.legend = new Legend()
                {
                    align = "center",
                    width = 200,
                    layout = "vertical",
                    verticalAlign = "bottom",
                    borderWidth = "0"
                };


                var mediaRecords = listOfSummaryReportData.GroupBy(sr => new { sr.MediaType }).Select(tr => new SummaryReportModel { MediaType = tr.Key.MediaType, Number_Docs = tr.Sum(r => r.Number_Docs) });

                PSeries pSeries = new PSeries();

                // ser series type
                pSeries.type = "pie";
                pSeries.name = "";
                //pSeries.data = new List<List<PSeriesData>>();
                pSeries.data = new List<object>();
                List<Object> lstObject = new List<object>();
                List<PSeries> lstPseries = new List<PSeries>();
                lstPseries.Add(pSeries);

                Dictionary<string, double> dictPie = new Dictionary<string, double>();

                lstObject = new List<object>();
                // set list of data for pie series
                foreach (var mt in p_MediaTypeList.Where(m => m.TypeLevel == 1 && m.HasAccess == true).OrderBy(om => om.SortOrder))
                {
                    /* Uncomment below statement and comment its following to check submediatype has access or not in individual records.  */
                    //var numOfDocs = mediaRecords.Where(smr => (String.Compare(smr.MediaType, mt.MediaType.ToString(), true) == 0) && 
                    //      (p_MediaTypeList.Where(sm=> (string.Compare(mt.MediaType,sm.MediaType,true) == 0) && sm.TypeLevel == 2 && sm.HasAccess == true).Select(smsm=>smsm.SubMediaType.ToUpper()).Contains(smr.SubMediaType.ToUpper()))).Select(tr => tr.Number_Docs).SingleOrDefault();

                    /* Assumed that recordlist contains data of only submediatypes for which has access. Read above. */

                    var numOfDocs = mediaRecords.Where(smr => (String.Compare(smr.MediaType, mt.MediaType.ToString(), true) == 0)).Select(tr => tr.Number_Docs).SingleOrDefault();

                    Int64 sumNumDocs = numOfDocs != null ? numOfDocs : 0;
                    lstObject.Add(new object[] { mt.DisplayName + " " + sumNumDocs.ToString("N0"), Convert.ToInt64(numOfDocs) });
                }

                pSeries.data = lstObject;
                highPieChartModel.series = lstPseries;
                string jsonResult = CommonFunctions.SearializeJson(highPieChartModel);

                return jsonResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*
        public List<string> MultiLinechart(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate)
        {
            try
            {
                List<DateTime> dateRange = new List<DateTime>();
                TimeSpan ts = p_ToDate.Subtract(p_FromDate);
                for (int i = 0; i <= ts.Days; i++)
                {

                    dateRange.Add(p_FromDate.AddDays(i));
                }

                SummaryReportMulti lstSummaryReportMulti = new SummaryReportMulti();


                //var mediaRecords = listOfSummaryReportData.GroupBy(sr => sr.GMT_DateTime).Select(tr => new SummaryReportModel { GMT_DateTime = tr.Key, Number_Docs = tr.Sum(r => r.Number_Docs) });

                var subMediaRecords = listOfSummaryReportData.GroupBy(sr => new { sr.SubMediaType, sr.GMT_DateTime }).Select(tr => new SummaryReportModel { SubMediaType = tr.Key.SubMediaType, GMT_DateTime = tr.Key.GMT_DateTime, Number_Docs = tr.Sum(r => r.Number_Docs) });
                //var tvRecords=listOfSummaryReportData.GroupBy(sr => new { sr.SubMediaType, sr.GMT_DateTime }).Select(tr => new SummaryReportModel { SubMediaType = tr.Key.SubMediaType, GMT_DateTime = tr.Key.GMT_DateTime, Number_Docs = tr.Sum(r => r.Number_Docs) }).Where(smr => String.Compare(smr.SubMediaType, CommonFunctions.CategoryType.TV.ToString(), true));

                Chart chart = new Chart();
                chart.subcaption = "";
                chart.caption = "";
                chart.linethickness = "1";
                chart.showvalues = "0";
                chart.formatnumberscale = "0";
                chart.anchorRadius = "10";
                chart.divlinealpha = "FFFFFF";
                chart.divlinecolor = "FFFFFF";
                chart.divlineisdashed = "1";
                chart.showalternatehgridcolor = "1";
                chart.alternatehgridcolor = "FFFFFF";
                chart.shadowalpha = "40";
                chart.labelstep = "1";
                chart.numvdivlines = "5";
                chart.chartrightmargin = "10";
                chart.bgcolor = "FFFFFF";
                chart.bgangle = "270";
                chart.bgalpha = "10,10";
                chart.alternatehgridalpha = "5";
                chart.legendposition = "BOTTOM";
                chart.drawAnchors = "1";
                chart.showBorder = "0";
                chart.canvasBorderAlpha = "0";


                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                //AllCategory allCategory = new AllCategory();
                //allCategory.category = new List<Category2>();

                //foreach (var date in dateRange)
                //{

                //    Category2 category2 = new Category2();

                //    category2.label = date.ToShortDateString();

                //    allCategory.category.Add(category2);

                //}

                //lstallCategory.Add(allCategory);

                //List<SeriesData> lstSeriesData = new List<SeriesData>();
                //SeriesData seriesData = new SeriesData();
                //seriesData.data = new List<Datum>();

                //seriesData.seriesname = "Media";
                //seriesData.color = "";
                //seriesData.anchorbordercolor = "";
                //seriesData.anchorbgcolor = "";

                //foreach (var sumofdoc in mediaRecords)
                //{

                //    Datum datum = new Datum();
                //    datum.value = Convert.ToString(sumofdoc.Number_Docs);
                //    seriesData.data.Add(datum);
                //}
                //foreach (var item in dateRange)
                //{
                //    var singleSubMediaRec = mediaRecords.Where(smr => smr.GMT_DateTime.Equals(item)).SingleOrDefault();

                //    Datum datum = new Datum();
                //    datum.value = Convert.ToString(singleSubMediaRec != null ? singleSubMediaRec.Number_Docs : 0);
                //    seriesData.data.Add(datum);
                //}
                //lstSeriesData.Add(seriesData);

                //lineChartOutput.categories = lstallCategory;
                //lineChartOutput.dataset = lstSeriesData;

                //string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                //lstSummaryReportMulti.MediaRecords = jsonResult;

                //Sub Media Recors
                lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                //var distinctDate = subMediaRecords.Select(d => d.GMT_DateTime).Distinct().ToList();

                var distinctSubMedia = subMediaRecords.Select(d => d.SubMediaType).Distinct().ToList();
                //lstSeriesData = new List<SeriesData>();

                //For TV
                var tvRecords = subMediaRecords.Where(smr => String.Compare(smr.SubMediaType, CommonFunctions.CategoryType.TV.ToString(), true) == 0).Sum(tr => tr.Number_Docs);
                var nmRecords = subMediaRecords.Where(smr => String.Compare(smr.SubMediaType, CommonFunctions.CategoryType.NM.ToString(), true) == 0).Sum(tr => tr.Number_Docs);
                var twRecords = subMediaRecords.Where(smr => String.Compare(smr.SubMediaType, CommonFunctions.CategoryType.TW.ToString(), true) == 0).Sum(tr => tr.Number_Docs);
                //var tvRecords = subMediaRecords.Where(smr => String.Compare(smr.SubMediaType, CommonFunctions.CategoryType.TV.ToString(), true) == 0).Sum(tr => tr.Number_Docs);
                //var tvRecords = subMediaRecords.Where(smr => String.Compare(smr.SubMediaType, CommonFunctions.CategoryType.TV.ToString(), true) == 0).Sum(tr => tr.Number_Docs);
                //var tvRecords = subMediaRecords.Where(smr => String.Compare(smr.SubMediaType, CommonFunctions.CategoryType.TV.ToString(), true) == 0).Sum(tr => tr.Number_Docs);
                //var tvRecords = subMediaRecords.Where(smr => String.Compare(smr.SubMediaType, CommonFunctions.CategoryType.TV.ToString(), true) == 0).Sum(tr => tr.Number_Docs);
                //var tvRecords = subMediaRecords.Where(smr => String.Compare(smr.SubMediaType, CommonFunctions.CategoryType.TV.ToString(), true) == 0).Sum(tr => tr.Number_Docs);

                //foreach (string subMedia in distinctSubMedia)
                //{
                //    //seriesData = new SeriesData();
                //    //seriesData.data = new List<Datum>();

                //    //seriesData.seriesname = subMedia;
                //    //seriesData.color = "";
                //    //seriesData.anchorbordercolor = "";
                //    //seriesData.anchorbgcolor = "";


                //    foreach (var item in dateRange)
                //    {
                //        var singleSubMediaRec = subMediaRecords.Where(smr => String.Compare(smr.SubMediaType, subMedia, true) == 0 && smr.GMT_DateTime.Equals(item)).SingleOrDefault();

                //        Datum datum = new Datum();
                //        datum.value = Convert.ToString(singleSubMediaRec != null ? singleSubMediaRec.Number_Docs : 0);
                //        seriesData.data.Add(datum);
                //    }


                //    lstSeriesData.Add(seriesData);

                //}


                //lineChartOutput.categories = lstallCategory;
                //lineChartOutput.dataset = lstSeriesData;

                //jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                //lstSummaryReportMulti.SubMediaRecords = jsonResult;
                List<string> lstMultiLineChart = new List<string>();
                lstMultiLineChart.Add("");
                return lstMultiLineChart;
            }
            catch (Exception ex)
            {

                throw;
            }
        }*/

        public SummaryReportMulti HighChartsLineChart(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, int? chartWidth, Dictionary<long, string> p_SearchRequests, List<IQAgent_ComparisionValues> p_ListOfIQAgent_ComparisionValues,
            List<ThirdPartyDataTypeModel> p_ThirdPartyDataTypes, bool p_NielsenAccess, bool p_CompeteDataAccess, bool p_IsThirdPartyAccess, List<IQ_MediaTypeModel> p_MediaTypeList)
        {
            try
            {
                Dictionary<long, string> dictSeriesColors = new Dictionary<long, string>();

                List<DateTime> dateRange = new List<DateTime>();
                TimeSpan ts = p_ToDate.Subtract(p_FromDate);
                for (int i = 0; i <= ts.Days; i++)
                {
                    dateRange.Add(p_FromDate.AddDays(i));
                }

                SummaryReportMulti lstSummaryReportMulti = new SummaryReportMulti();

                List<string> categories = new List<string>();

                foreach (var date in dateRange)
                {
                    categories.Add(date.ToShortDateString());
                }

                // this signle line medium chart, with out applying any medium filter.... 
                // if one or more request request applid, then it will show multi line chart, one for each request request. 
                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
                highLineChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartOutput.yAxis = new List<YAxis>() { new YAxis(){ min = 0, title = new Title2() }};

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories, // all x-axis values 
                    labels = new labels()
                };

                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { borderWidth = "0", width = 750 };

                // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
                highLineChartOutput.hChart = new HChart() { height = 300, type = "spline" };

                // start to set series of data for medium chart (or multi line search request chart)
                List<Series> lstSeries = new List<Series>();

                // if one or more search request is applied, then will set multiple seareis , each series for each request request
                // with total no. of records for that search request on perticular date (value of category)
                if (p_SearchRequests != null && p_SearchRequests.Count > 0)
                {
                    // set plot options and click event for series points (which will again assigned in JS as this is string value)
                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        spline = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };

                    // set list of data for each series 
                    List<SeriesData> multiLstSeriesData = new List<SeriesData>();
                    foreach (var searchRequest in p_SearchRequests)
                    {
                        // set sereies name as search request query name, will shown in legent and tooltip.
                        Series series = new Series();
                        series.data = new List<HighChartDatum>();
                        series.name = searchRequest.Value;
                        series.color = highLineChartOutput.colors[lstSeries.Count % highLineChartOutput.colors.Count];

                        // loop for each date to create list of data for selected search request series. 
                        foreach (var item in dateRange)
                        {
                            var daywiseSum = listOfSummaryReportData.Where(smr => smr.SearchRequestID == searchRequest.Key && smr.GMT_DateTime.Equals(item)
                                    && smr.DefaultMediaType
                                    && (
                                        CheckSubMediaTypeAccess(p_MediaTypeList, smr.SubMediaType)
                                      )
                                ).Sum(s => s.Number_Docs);


                            // set data point of current series 
                            /*
                                *  y = y series value of current point === total no. of records for current search request at perticular date 
                                *  SearchTerm = query name  , used in chart drill down click event
                                *  Value = Search Request ID  , used in chart drill down click event
                                *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                            */
                            HighChartDatum highChartDatum = new HighChartDatum();
                            highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                            highChartDatum.SearchTerm = searchRequest.Value;
                            highChartDatum.Value = searchRequest.Key.ToString();
                            highChartDatum.Type = "Media";
                            series.data.Add(highChartDatum);
                        }

                        // Keep track of the association between agent and series color so that third party data can match colors by agent
                        dictSeriesColors.Add(searchRequest.Key, series.color);
                        lstSeries.Add(series);
                    }
                }
                else
                {

                    // as its single media chart, we will show it as area chart, by setting chart type to "area"
                    highLineChartOutput.hChart.type = "areaspline";

                    // set plot options for area chart, for series click event, and plot marker.
                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        area = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };

                    // set sereies name as "Media" , will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = "Media";

                    // loop for each date to create list of data for media series
                    foreach (var item in dateRange)
                    {

                        var sumOfDocs = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Equals(item) 
                            && smr.DefaultMediaType
                            && (
                                    CheckSubMediaTypeAccess(p_MediaTypeList, smr.SubMediaType)
                                )
                            ).Sum(s => s.Number_Docs);


                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                            *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                            *  we will not set SearchTerm and Value properies of data, as this is signle medium chart , without any search request
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = sumOfDocs != null ? sumOfDocs : 0;
                        highChartDatum.Type = "Media";
                        series.data.Add(highChartDatum);
                    }
                    lstSeries.Add(series);
                }


                // assign set of series data to medium chart (or multi line searchrequest chart)
                highLineChartOutput.series = lstSeries;


                //Sub Media  vice Records
                // this is multi line sub media chart, for each line series, for each medium type , for records exist for that medium type. 
                HighLineChartOutput highLineChartSubMediaOutput = new HighLineChartOutput();
                highLineChartSubMediaOutput.title = new Title() { text = "", x = -20 };
                highLineChartSubMediaOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartSubMediaOutput.yAxis = new List<YAxis>() { new YAxis() { title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSubMediaOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories,
                    labels = new labels()
                };


                highLineChartSubMediaOutput.tooltip = new Tooltip() { valueSuffix = "" };
                highLineChartSubMediaOutput.legend = new Legend() { borderWidth = "0" };
                highLineChartSubMediaOutput.hChart = new HChart() { height = 300, width = chartWidth, type = "spline" };

                // set plot options and click event for series points (which will again assigned in JS as this is string value)
                highLineChartSubMediaOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = true
                        }
                    },
                    series = new PlotSeries()
                    {
                        cursor = "pointer",
                        point = new PlotPoint()
                        {
                            events = new PlotEvents()
                            {
                                click = "LineChartClick"
                            }
                        }
                    }
                };


                // this is signle line spark chart , different for each medium type (with all commom properties set here)
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };


                // set min = 0 , to force chart to start from 0 , and show line in bottom, 
                // gridLineWidth = 0 , to hide grid lines on y axis. 
                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                // not to show x axis labels for spark charts 
                // we have set default value for TickWidth to 0 in XAxis class defination, to not to show line below x-axis for ticks.
                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = categories,
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // add event on chart click , to load medium type summary of selected medium chart. 
                highLineChartSingleMediaChartOutput.hChart = new HChart()
                {
                    events = new PlotEvents()
                    {
                        click = "ChangeMediumType"
                    },
                    height = 100,
                    width = 120,
                    type = "spline"
                };
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };


                // add event on chart points click , to load medium type summary of selected medium chart. 
                // also disble marker on chart
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        point = new PlotPoint()
                        {
                            events = new PlotEvents()
                            {
                                click = "ChangeMediumTypeOnPointClick"
                            }
                        },
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };                

                Int64 totNumOfHits = listOfSummaryReportData.Where(smr =>
                    smr.DefaultMediaType
                    && CheckSubMediaTypeAccess(p_MediaTypeList, smr.SubMediaType)

                                ).Sum(s => s.Number_Docs);
                lstSummaryReportMulti.TotalNumOfHits = totNumOfHits.ToString("N0");

                // start to set series of data for  multi line medium chart
                List<Series> lstSeriesSubMediaType = new List<Series>();
                lstSummaryReportMulti.SummaryReportMedium = new List<SummaryReportMedium>();

                foreach (var media in p_MediaTypeList.Where(m => m.TypeLevel == 1 && m.HasAccess && p_MediaTypeList.Where(sm=>string.Compare(m.MediaType, sm.MediaType, true) == 0 && sm.TypeLevel == 2 && sm.HasAccess).Count() > 0).OrderBy(om => om.SortOrder))
                {
                    // set sereies name of multiline medium chart as display name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = media.DisplayName;

                    // set sereies name of signle line spark medium chart  as medium description, will shown in legent and tooltip.
                    Series seriesSingleMedia = new Series();
                    seriesSingleMedia.data = new List<HighChartDatum>();
                    seriesSingleMedia.name = media.DisplayName;

                    // loop for each date to create list of data for selected medium type
                    foreach (var item in dateRange)
                    {
                        var daywiseSum = listOfSummaryReportData.Where(smr => String.Compare(smr.MediaType, media.MediaType, true) == 0 && smr.GMT_DateTime.Equals(item) && CheckSubMediaTypeAccess(p_MediaTypeList,smr.SubMediaType)).Sum(s => s.Number_Docs);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current medium type at perticular date 
                            *  SearchTerm = medium description  , used in chart drill down click event
                            *  Value = medium tpye  , used in chart drill down click event
                            *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                        highChartDatum.SearchTerm = media.DisplayName;
                        highChartDatum.Value = media.MediaType;
                        highChartDatum.Type = "SubMedia";
                        series.data.Add(highChartDatum);

                        seriesSingleMedia.data.Add(highChartDatum);
                    }

                    lstSeriesSubMediaType.Add(series);


                    // set signle series for spark chart of current medium type, and assign list of data for that series.
                    List<Series> lstSeriesSingleMediaType = new List<Series>();
                    lstSeriesSingleMediaType.Add(seriesSingleMedia);
                    highLineChartSingleMediaChartOutput.series = lstSeriesSingleMediaType;

                    // set json chart for spark chart based on medium type                    

                    lstSummaryReportMulti.SummaryReportMedium.Add(new SummaryReportMedium()
                    {
                        MediaTypeModel = media,
                        PrevRecordsSum = (p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType != null && string.Compare(media.MediaType, a.MediaType, true) == 0 && CheckSubMediaTypeAccess(p_MediaTypeList,a.SubMediaType)).Sum(s => s.NoOfDocs) : 0),
                        Records = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput),
                        RecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)))

                    });

                    /*
                        if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.TV.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.TVRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                            lstSummaryReportMulti.TVRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                            lstSummaryReportMulti.TVPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.TV.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.NM.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.NMRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                            lstSummaryReportMulti.NMRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                            lstSummaryReportMulti.NMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.NM.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.TW.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.TWRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                            lstSummaryReportMulti.TWRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                            lstSummaryReportMulti.TWPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.TW.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.Forum.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.ForumRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                            lstSummaryReportMulti.ForumRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                            lstSummaryReportMulti.ForumPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.Forum.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.SocialMedia.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.SocialMediaRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                            lstSummaryReportMulti.SocialMediaRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                            lstSummaryReportMulti.SocialMediaPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.SocialMedia.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.Blog.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.BlogRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                            lstSummaryReportMulti.BlogRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                            lstSummaryReportMulti.BlogPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.Blog.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.PM.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.PMRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                            lstSummaryReportMulti.PMRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                            lstSummaryReportMulti.PMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.PM.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.Radio.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.TMRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                            lstSummaryReportMulti.TMRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                            lstSummaryReportMulti.TMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.Radio.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.MS.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.MSRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                            lstSummaryReportMulti.MSRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                        }*/

                }

                // Create series for third party data
                if (p_IsThirdPartyAccess)
                {
                    BuildThirdPartySeries(listOfSummaryReportData, p_SearchRequests, p_ThirdPartyDataTypes, dateRange, dictSeriesColors, highLineChartOutput.series, highLineChartOutput.yAxis, lstSeriesSubMediaType, highLineChartSubMediaOutput.yAxis, false);
                }

                // assign set of series data to multi line medium type chart
                highLineChartSubMediaOutput.series = lstSeriesSubMediaType;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSubMediaOutput);
                lstSummaryReportMulti.SubMediaRecords = jsonResult;

                jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
                lstSummaryReportMulti.MediaRecords = jsonResult;

                
                // create spark chart for audience and media value
                if (p_MediaTypeList.Where(m => m.TypeLevel == 2 && m.HasAccess == true && (m.UseAudience == true || m.UseMediaValue == true)).Count() > 0)
                {
                    //Single Media Chart 
                    HighLineChartOutput highLineChartAudienceMediaValue = new HighLineChartOutput();
                    highLineChartAudienceMediaValue.title = new Title() { text = "", x = -20 };
                    highLineChartAudienceMediaValue.subtitle = new Subtitle() { text = "", x = -20 };
                    //highLineChartAudienceMediaValue.Colors = new List<string>();

                    highLineChartAudienceMediaValue.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                    highLineChartAudienceMediaValue.xAxis = new XAxis()
                    {
                        tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                        tickmarkPlacement = "off",
                        categories = categories,
                        labels = new labels()
                        {
                            enabled = false
                        }
                    };


                    highLineChartAudienceMediaValue.hChart = new HChart() { height = 100, width = 120, type = "spline" };
                    highLineChartAudienceMediaValue.plotOption = new PlotOptions()
                    {
                        spline = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = false,
                                lineWidth = 0
                            }
                        }
                    };
                    highLineChartAudienceMediaValue.tooltip = new Tooltip()
                    {
                        valueSuffix = ""
                    };
                    highLineChartAudienceMediaValue.legend = new Legend() { enabled = false };

                    if (p_MediaTypeList.Where(m => m.TypeLevel == 2 && m.HasAccess == true && m.UseMediaValue == true).Count() > 0)
                    {
                        List<Series> lstSeriesMediaValue = new List<Series>();

                        Series series = new Series();
                        series.data = new List<HighChartDatum>();
                        series.name = "Media Value";

                        foreach (var item in dateRange)
                        {

                            var singleIQMediaValueRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Equals(item)
                                && smr.DefaultMediaType
                                && (
                                             p_MediaTypeList.Where(m => CheckSubMediaTypeAccess(m,smr.SubMediaType)
                                            && CommonFunctions.CheckNielsenCompeteAccess(m.UseMediaValue, m.RequireNielsenAccess,p_NielsenAccess,m.RequireCompeteAccess,p_CompeteDataAccess)).Count() > 0
                                   )
                              ).Sum(s => s.IQMediaValue);// SingleOrDefault();

                            HighChartDatum highChartDatum = new HighChartDatum();
                            highChartDatum.y = Convert.ToDecimal(singleIQMediaValueRec != null ? singleIQMediaValueRec : 0);
                            series.data.Add(highChartDatum);
                        }

                        lstSeriesMediaValue.Add(series);

                        highLineChartAudienceMediaValue.series = lstSeriesMediaValue;

                        lstSummaryReportMulti.IQMediaValueRecords = CommonFunctions.SearializeJson(highLineChartAudienceMediaValue);


                        //lstSummaryReportMulti.IQMediaValueRecords = temp;
                        lstSummaryReportMulti.IQMediaValueRecordsSum = (series.data.Sum(s => Convert.ToDecimal(s.y)));
                        lstSummaryReportMulti.IQMediaValuePrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(smr =>
                             p_MediaTypeList.Where(m => CheckSubMediaTypeAccess(m, smr.SubMediaType)
                                            && CommonFunctions.CheckNielsenCompeteAccess(m.UseMediaValue, m.RequireNielsenAccess,p_NielsenAccess,m.RequireCompeteAccess,p_CompeteDataAccess)).Count() > 0

                                     ).Sum(a => a.IQMediaValue) : 0;
                    }



                    if (p_MediaTypeList.Where(m => m.TypeLevel == 2 && m.HasAccess == true && m.UseAudience == true).Count() > 0)
                    {
                        List<Series> lstSeriesAudience = new List<Series>();
                        Series seriesAudience = new Series();
                        seriesAudience.name = "Audience";
                        seriesAudience.data = new List<HighChartDatum>();

                        foreach (var item in dateRange)
                        {
                            var singleAudienceRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Equals(item)
                                && smr.DefaultMediaType
                                && (
                                            p_MediaTypeList.Where(m => CheckSubMediaTypeAccess(m, smr.SubMediaType)
                                            && CommonFunctions.CheckNielsenCompeteAccess(m.UseAudience, m.RequireNielsenAccess,p_NielsenAccess,m.RequireCompeteAccess,p_CompeteDataAccess)).Count() > 0
                                    )
                            ).Sum(s => s.Audience);// SingleOrDefault();

                            HighChartDatum highChartDatum = new HighChartDatum();
                            highChartDatum.y = singleAudienceRec;
                            seriesAudience.data.Add(highChartDatum);
                        }

                        lstSeriesAudience.Add(seriesAudience);

                        highLineChartAudienceMediaValue.series = lstSeriesAudience;

                        lstSummaryReportMulti.AudienceRecords = CommonFunctions.SearializeJson(highLineChartAudienceMediaValue);

                        lstSummaryReportMulti.AudienceRecordsSum = (seriesAudience.data.Sum(s => Convert.ToInt64(s.y)));
                        lstSummaryReportMulti.AudiencePrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(smr =>
                                            p_MediaTypeList.Where(m => CheckSubMediaTypeAccess(m, smr.SubMediaType)
                                            && CommonFunctions.CheckNielsenCompeteAccess(m.UseAudience, m.RequireNielsenAccess, p_NielsenAccess, m.RequireCompeteAccess, p_CompeteDataAccess)).Count() > 0

                                            ).Sum(a => a.Audience) : 0;
                    }
                }

                return lstSummaryReportMulti;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region HourWise

        public List<IQAgent_DaySummaryModel> GetDaySummaryHourWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium)
        {
            DashboardDA dashboardDA = new DashboardDA();

            List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = dashboardDA.GetHourSummaryMediumWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium);

            return lstIQAgent_DaySummaryModel;
        }

        #endregion

        #region Monthly Chart
        /*

        public SummaryReportMulti LineChartMonth(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, bool p_Isv4TMAccess, List<string> p_SearchRequestIDs, List<IQAgent_ComparisionValues> p_ListOfIQAgent_ComparisionValues,
            bool p_Isv4NMAccess, bool p_Isv4SMAccess, bool p_Isv4TWAccess, bool p_Isv4TVAccess, bool p_Isv4BLPMAccess, bool p_NielsenAccess, bool p_CompeteDataAccess)
        {
            try
            {

                var dateRange = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    dateRange.Add(dt);
                }

                SummaryReportMulti lstSummaryReportMulti = new SummaryReportMulti();


                //var mediaRecords = listOfSummaryReportData.GroupBy(sr => new { sr.GMT_DateTime.Month, sr.GMT_DateTime.Year }).Select(tr => new SummaryReportModel { GMT_DateTime = new DateTime(tr.Key.Year, tr.Key.Month, 1), Number_Docs = tr.Sum(r => r.Number_Docs) });
                //var subMediaRecords = listOfSummaryReportData.GroupBy(sr => new { sr.SubMediaType, sr.GMT_DateTime.Month, sr.GMT_DateTime.Year }).Select(tr => new SummaryReportModel { SubMediaType = tr.Key.SubMediaType, GMT_DateTime = new DateTime(tr.Key.Year, tr.Key.Month, 1), Number_Docs = tr.Sum(r => r.Number_Docs) });
                //var audienceRecords = listOfSummaryReportData.GroupBy(sr => new { sr.GMT_DateTime.Month, sr.GMT_DateTime.Year }).Select(tr => new SummaryReportModel { GMT_DateTime = new DateTime(tr.Key.Year, tr.Key.Month, 1), Audience = tr.Sum(r => r.Audience) });
                //var iqMediaValueRecords = listOfSummaryReportData.GroupBy(sr => new { sr.GMT_DateTime.Month, sr.GMT_DateTime.Year }).Select(tr => new SummaryReportModel { GMT_DateTime = new DateTime(tr.Key.Year, tr.Key.Month, 1), IQMediaValue = tr.Sum(r => r.IQMediaValue) });


                Chart chart = new Chart();
                chart.subcaption = "";
                chart.caption = string.Empty;//  "Summary Report";
                chart.linethickness = "1";
                chart.showvalues = "0";
                //chart.showLabels = "0";
                //chart.showYAxisValues = "0";
                chart.formatnumberscale = "0";
                chart.anchorRadius = "3";
                chart.divlinealpha = "20";
                chart.divlinecolor = "000000";
                chart.divlineisdashed = "0";
                chart.showalternatehgridcolor = "1";
                chart.alternatehgridcolor = "FFFFFF";
                chart.shadowalpha = "40";
                chart.labelstep = "1";
                chart.numvdivlines = "0";
                chart.chartrightmargin = "10";
                chart.bgcolor = "FFFFFF";
                chart.bgangle = "270";
                chart.bgalpha = "10,10";
                chart.alternatehgridalpha = "5";
                chart.legendposition = "BOTTOM";
                chart.drawAnchors = "1";
                chart.showBorder = "0";
                chart.canvasBorderAlpha = "0";
                //chart.lineColor = "#4493D6";
                chart.canvasRightMargin = "20";
                //Multi Line chart

                Chart multiChart = new Chart();
                multiChart.subcaption = "";
                multiChart.caption = "";
                multiChart.linethickness = "1";
                //multiChart.showLabels = "0";
                multiChart.showvalues = "0";
                //multiChart.showYAxisValues = "0";
                multiChart.formatnumberscale = "0";
                multiChart.anchorRadius = "3";
                multiChart.divlinealpha = "20";
                multiChart.divlinecolor = "000000";
                multiChart.divlineisdashed = "0";
                multiChart.showalternatehgridcolor = "1";
                multiChart.alternatehgridcolor = "FFFFFF";
                multiChart.shadowalpha = "40";
                multiChart.labelstep = "1";
                multiChart.numvdivlines = "0";
                multiChart.chartrightmargin = "10";
                multiChart.bgcolor = "FFFFFF";
                multiChart.bgangle = "270";
                multiChart.bgalpha = "10,10";
                multiChart.alternatehgridalpha = "5";
                multiChart.legendposition = "BOTTOM";
                multiChart.drawAnchors = "1";
                multiChart.showBorder = "0";
                multiChart.canvasBorderAlpha = "0";
                //multiChart.lineColor = "#4493D6";
                multiChart.canvasRightMargin = "20";
                multiChart.palettecolors = "15335D,448FF2,7A045C,FFB451,E14A02,394900,005E8F,A7B1B3";


                //

                //Media Records

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();

                foreach (var date in dateRange)
                {
                    Category2 category2 = new Category2();
                    category2.label = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(date.Month) + " - " + date.Year;
                    allCategory.category.Add(category2);
                }

                lstallCategory.Add(allCategory);

                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();

                SparkChart sparkChartMediaWise = new SparkChart();
                SparkChartOutput sparkChartOutputMediaWise = new SparkChartOutput();
                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    //Multi Line Charts
                    List<SeriesData> multiLstSeriesData = new List<SeriesData>();
                    foreach (var searchRequest in p_SearchRequestIDs)
                    {
                        // Spark Chart Series Init
                        sparkChartOutputMediaWise.chart = sparkChartMediaWise;
                        List<SparkSeriesData> lstSparkSeriesDataMediaWise = new List<SparkSeriesData>();
                        SparkSeriesData sparkSeriesMediaWise = new SparkSeriesData();
                        sparkSeriesMediaWise.data = new List<SparkDatum>();
                        //sparkChartMediaWise.caption = CommonFunctions.GetEnumDescription(subMedia);


                        var SearchRequest = listOfSummaryReportData.Where(a => string.Compare(a.SearchRequestID.ToString(), searchRequest) == 0).FirstOrDefault();
                        if (SearchRequest != null)
                        {


                            SeriesData multiSeriesData = new SeriesData();
                            multiSeriesData.data = new List<Datum>();

                            multiSeriesData.seriesname = SearchRequest.Query_Name;
                            multiSeriesData.color = "";
                            //multiSeriesData.anchorBorderColor = "";
                            //multiSeriesData.anchorBgColor = "";

                            foreach (var item in dateRange)
                            {
                                var singleSubMediaRec = listOfSummaryReportData.Where(smr => String.Compare(smr.SearchRequestID.ToString(), searchRequest, true) == 0 && smr.GMT_DateTime.Month.Equals(item.Month) && smr.GMT_DateTime.Year.Equals(item.Year)
                                    && (
                                            (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                            (p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) &&
                                            (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) &&
                                            (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) &&
                                            (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) &&
                                            (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                            (p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString()) &&
                                            (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString())
                                        )
                                    ).Sum(s => s.Number_Docs);

                                Datum datum = new Datum();
                                datum.value = Convert.ToString(singleSubMediaRec != null ? singleSubMediaRec : 0);
                                datum.link = "javascript:OpenFeed('" + item.ToShortDateString() + "','','','[" + searchRequest + "]','[\"" + System.Web.HttpUtility.UrlEncode(SearchRequest.Query_Name.Replace("\"", "\\\"")) + "\"]')";

                                multiSeriesData.data.Add(datum);

                                SparkDatum sparkDatum = new SparkDatum();
                                sparkDatum.value = Convert.ToString(singleSubMediaRec != null ? singleSubMediaRec : 0);
                                sparkSeriesMediaWise.data.Add(sparkDatum);
                            }

                            lstSparkSeriesDataMediaWise.Add(sparkSeriesMediaWise);
                            sparkChartOutputMediaWise.dataset = lstSparkSeriesDataMediaWise;

                            //Multi Line Charts
                            multiLstSeriesData.Add(multiSeriesData);
                        }
                    }
                    lineChartOutput.categories = lstallCategory;
                    lineChartOutput.dataset = multiLstSeriesData;
                }
                else
                {

                    seriesData.seriesname = "Media";
                    seriesData.color = "";

                    foreach (var item in dateRange)
                    {
                        var singleSubMediaRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Month.Equals(item.Month) && smr.GMT_DateTime.Year.Equals(item.Year)
                            && (
                                    (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                    (p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) &&
                                    (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                    (p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString()) &&
                                    (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString())
                                )
                         ).Sum(s => s.Number_Docs);// SingleOrDefault();

                        Datum datum = new Datum();
                        datum.value = Convert.ToString(singleSubMediaRec != null ? singleSubMediaRec : 0);
                        datum.link = "javascript:OpenFeed('" + item.ToShortDateString() + "','','','','')";
                        seriesData.data.Add(datum);
                    }
                    lstSeriesData.Add(seriesData);

                    lineChartOutput.categories = lstallCategory;
                    lineChartOutput.dataset = lstSeriesData;
                }

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                lstSummaryReportMulti.MediaRecords = jsonResult;


                //Sub Media Recors

                lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = multiChart;

                var distinctSubMedia = listOfSummaryReportData.Select(d => d.SubMediaType).Distinct().ToList();
                lstSeriesData = new List<SeriesData>();

                List<CommonFunctions.CategoryType> lstMediaCategories = Enum.GetValues(typeof(CommonFunctions.CategoryType)).Cast<CommonFunctions.CategoryType>().ToList();
                Int64 totNumOfHits = listOfSummaryReportData.Where(smr =>
                                    (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                    (p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) &&
                                    (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                    (p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString()) &&
                                    (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString())
                                ).Sum(s => s.Number_Docs);
                lstSummaryReportMulti.TotalNumOfHits = totNumOfHits.ToString("N0");

                //Spark Chart Init
                sparkChartMediaWise = new SparkChart();
                sparkChartOutputMediaWise = new SparkChartOutput();

                string searchRequestIds = string.Empty;
                string searchRequestNames = string.Empty;

                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    searchRequestIds = Newtonsoft.Json.JsonConvert.SerializeObject(listOfSummaryReportData.Select(a => a.SearchRequestID).Distinct());
                    searchRequestNames = Newtonsoft.Json.JsonConvert.SerializeObject(listOfSummaryReportData.Select(a => a.Query_Name).Distinct());
                }

                foreach (var subMedia in lstMediaCategories)
                {

                    if (
                    (p_Isv4TMAccess || subMedia != CommonFunctions.CategoryType.Radio) &&
                    (p_Isv4NMAccess || subMedia != CommonFunctions.CategoryType.NM) &&
                    (p_Isv4SMAccess || subMedia != CommonFunctions.CategoryType.SocialMedia) &&
                    (p_Isv4SMAccess || subMedia != CommonFunctions.CategoryType.Forum) &&
                    (p_Isv4SMAccess || subMedia != CommonFunctions.CategoryType.Blog) &&
                    (p_Isv4TWAccess || subMedia != CommonFunctions.CategoryType.TW) &&
                    (p_Isv4TVAccess || subMedia != CommonFunctions.CategoryType.TV) &&
                    (p_Isv4BLPMAccess || subMedia != CommonFunctions.CategoryType.PM))
                    {

                        //}
                        //foreach (string subMedia in distinctSubMedia)
                        //{

                        // Spark Chart Series Init
                        sparkChartOutputMediaWise.chart = sparkChartMediaWise;
                        List<SparkSeriesData> lstSparkSeriesDataMediaWise = new List<SparkSeriesData>();
                        SparkSeriesData sparkSeriesMediaWise = new SparkSeriesData();
                        sparkSeriesMediaWise.data = new List<SparkDatum>();
                        //sparkChartMediaWise.caption = CommonFunctions.GetEnumDescription(subMedia);


                        // Line Chart Series Init
                        seriesData = new SeriesData();
                        seriesData.data = new List<Datum>();
                        string enumDesc = CommonFunctions.GetEnumDescription(subMedia);
                        //multiChart.caption = enumDesc;
                        seriesData.seriesname = enumDesc;
                        seriesData.color = "";
                        //seriesData.anchorBorderColor = "";
                        //seriesData.anchorBgColor = "";

                        SeriesData multiSeriesData = new SeriesData();
                        multiSeriesData.data = new List<Datum>();

                        multiSeriesData.seriesname = "";
                        multiSeriesData.color = "";
                        //multiSeriesData.anchorBorderColor = "";
                        //multiSeriesData.anchorBgColor = "";


                        //Multi Line Charts
                        List<SeriesData> multiLstSeriesData = new List<SeriesData>();
                        //
                        foreach (var item in dateRange)
                        {
                            var singleSubMediaRec = listOfSummaryReportData.Where(smr => String.Compare(smr.SubMediaType, subMedia.ToString(), true) == 0 && smr.GMT_DateTime.Month.Equals(item.Month) && smr.GMT_DateTime.Year.Equals(item.Year))
                                                        .Sum(s => s.Number_Docs);// SingleOrDefault();

                            Datum datum = new Datum();
                            datum.value = Convert.ToString(singleSubMediaRec != null ? singleSubMediaRec : 0);
                            datum.link = "javascript:OpenFeed('" + item.ToShortDateString() + "','" + subMedia.ToString() + "','" + CommonFunctions.GetEnumDescription(subMedia) + "','" + searchRequestIds + "','" + searchRequestNames.Replace("\'", "\\\'") + "')";

                            seriesData.data.Add(datum);
                            multiSeriesData.data.Add(datum);

                            SparkDatum sparkDatum = new SparkDatum();
                            sparkDatum.value = Convert.ToString(singleSubMediaRec != null ? singleSubMediaRec : 0);
                            sparkSeriesMediaWise.data.Add(sparkDatum);
                        }

                        lstSparkSeriesDataMediaWise.Add(sparkSeriesMediaWise);
                        sparkChartOutputMediaWise.dataset = lstSparkSeriesDataMediaWise;


                        //Multi Line Charts
                        multiLstSeriesData.Add(multiSeriesData);

                        if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.TV.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.TVRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.TVRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.TVPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.TV.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.NM.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.NMRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.NMRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.NMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.NM.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.TW.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.TWRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.TWRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.TWPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.TW.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.Forum.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.ForumRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.ForumRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.ForumPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.Forum.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.SocialMedia.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.SocialMediaRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.SocialMediaRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.SocialMediaPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.SocialMedia.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.Blog.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.BlogRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.BlogRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.BlogPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.Blog.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.PM.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.PMRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.PMRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.PMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.PM.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.Radio.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.TMRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.TMRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.TMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.Radio.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        //

                        lstSeriesData.Add(seriesData);
                    }

                }


                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                lstSummaryReportMulti.SubMediaRecords = jsonResult;

                if (p_Isv4TMAccess || p_Isv4NMAccess || p_Isv4SMAccess || p_Isv4TWAccess || p_Isv4TVAccess || p_Isv4BLPMAccess)
                {
                    //IQ Media Value
                    SparkChart sparkChart = new SparkChart();
                    //sparkChart.caption = "Ad Value";
                    sparkChart.palette = "5";
                    sparkChart.setAdaptiveYMin = "0";
                    sparkChart.showCloseAnchor = "0";
                    sparkChart.showCloseValue = "0";
                    sparkChart.showHighAnchor = "0";
                    sparkChart.showHighLowValue = "0";
                    sparkChart.showOpenAnchor = "0";
                    sparkChart.showOpenValue = "0";
                    sparkChart.showLowAnchor = "0";
                    sparkChart.showToolTip = "1";
                    sparkChart.bgColor = "FFFFFF";
                    sparkChart.lineColor = "#4493D6";
                    // chart1.formatNumber = "0";
                    //chart1.thousandSeparator = ",";
                    sparkChart.thousandSeparatorPosition = "0";
                    sparkChart.formatNumberScale = "0";
                    //sparkChart.caption = "Ad Value";
                    SparkChartOutput sparkChartOutput = new SparkChartOutput();
                    sparkChartOutput.chart = sparkChart;
                    List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                    SparkSeriesData sparkSeries = new SparkSeriesData();
                    sparkSeries.data = new List<SparkDatum>();

                    foreach (var item in dateRange)
                    {

                        var singleIQMediaValueRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Month.Equals(item.Month) && smr.GMT_DateTime.Year.Equals(item.Year)
                            && (
                                        (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                        ((p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) && p_CompeteDataAccess) &&
                                        (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                        (p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() && p_NielsenAccess) &&
                                        (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString())
                               )
                             ).Sum(s => s.IQMediaValue);

                        SparkDatum datum = new SparkDatum();
                        datum.value = Convert.ToString(singleIQMediaValueRec != null ? singleIQMediaValueRec : 0);
                        sparkSeries.data.Add(datum);
                    }
                    lstSparkSeriesData.Add(sparkSeries);
                    sparkChartOutput.dataset = lstSparkSeriesData;
                    lstSummaryReportMulti.IQMediaValueRecords = CommonFunctions.SearializeJson(sparkChartOutput);

                    lstSummaryReportMulti.IQMediaValueRecordsSum = (sparkSeries.data.Sum(s => Convert.ToDecimal(s.value)));
                    lstSummaryReportMulti.IQMediaValuePrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(smr => (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                        ((p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) && p_CompeteDataAccess) &&
                                        (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                        (p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() && p_NielsenAccess) &&
                                        (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString())).Sum(a => a.IQMediaValue) : 0;


                    //Audience
                    sparkChartOutput = new SparkChartOutput();
                    //sparkChart.caption = "Views";
                    sparkChartOutput.chart = sparkChart;
                    lstSparkSeriesData = new List<SparkSeriesData>();
                    sparkSeries = new SparkSeriesData();
                    sparkSeries.data = new List<SparkDatum>();
                    foreach (var item in dateRange)
                    {

                        var singleAudienceRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Month.Equals(item.Month) && smr.GMT_DateTime.Year.Equals(item.Year)
                            && (
                                       (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                        ((p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) && p_CompeteDataAccess) &&
                                        (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                        (p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() && p_NielsenAccess) &&
                                        (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString())
                               )
                           ).Sum(s => s.Audience);// SingleOrDefault();

                        SparkDatum datum = new SparkDatum();
                        datum.value = Convert.ToString(singleAudienceRec != null ? singleAudienceRec : 0);
                        sparkSeries.data.Add(datum);
                    }
                    lstSparkSeriesData.Add(sparkSeries);
                    sparkChartOutput.dataset = lstSparkSeriesData;
                    lstSummaryReportMulti.AudienceRecords = CommonFunctions.SearializeJson(sparkChartOutput);

                    lstSummaryReportMulti.AudienceRecordsSum = (sparkSeries.data.Sum(s => Convert.ToInt64(s.value)));
                    lstSummaryReportMulti.AudiencePrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(smr => (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                        ((p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) && p_CompeteDataAccess) &&
                                        (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                        (p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() && p_NielsenAccess) &&
                                        (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString())).Sum(a => a.Audience) : 0;
                }

                return lstSummaryReportMulti;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        */

        public SummaryReportMulti HighChartsLineChartMonth(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, Dictionary<long, string> p_SearchRequests, List<IQAgent_ComparisionValues> p_ListOfIQAgent_ComparisionValues,
            List<ThirdPartyDataTypeModel> p_ThirdPartyDataTypes, bool p_NielsenAccess, bool p_CompeteDataAccess, bool p_IsThirdPartyAccess, List<IQ_MediaTypeModel> p_MediaTypeList)
        {
            try
            {
                Dictionary<long, string> dictSeriesColors = new Dictionary<long, string>();

                var dateRange = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    dateRange.Add(dt);
                }

                SummaryReportMulti lstSummaryReportMulti = new SummaryReportMulti();


                List<string> categories = new List<string>();

                foreach (var date in dateRange)
                {
                    categories.Add(date.ToShortDateString());
                }


                // this signle line medium chart, with out applying any medium filter.... 
                // if one or more request request applid, then it will show multi line chart, one for each request request. 
                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
                highLineChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartOutput.yAxis = new List<YAxis>() { new YAxis(){ min = 0, title = new Title2() }};

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                // formatter to show "Month(MMM) - Year(YYYY)" format in monthly summary
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories,
                    labels = new labels()
                    {
                        formatter = "GetMonth"
                    }
                };

                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { borderWidth = "0", width = 750 };

                // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
                highLineChartOutput.hChart = new HChart() { height = 300, type = "spline" };


                // start to set series of data for medium chart (or multi line search request chart)
                List<Series> lstSeries = new List<Series>();

                // if one or more search request is applied, then will set multiple seareis , each series for each request request
                // with total no. of records for that search request on perticular date (value of category)
                if (p_SearchRequests != null && p_SearchRequests.Count > 0)
                {

                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        spline = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };

                    // set list of data for each series 
                    List<SeriesData> multiLstSeriesData = new List<SeriesData>();
                    foreach (KeyValuePair<long, string> searchRequest in p_SearchRequests)
                    {
                        // set sereies name as search request query name, will shown in legent and tooltip.
                        Series series = new Series();
                        series.data = new List<HighChartDatum>();
                        series.name = searchRequest.Value;
                        series.color = highLineChartOutput.colors[lstSeries.Count % highLineChartOutput.colors.Count];

                        // loop for each date to create list of data for selected search request series. 
                        foreach (var item in dateRange)
                        {
                                var daywiseSum = listOfSummaryReportData.Where(smr => smr.SearchRequestID == searchRequest.Key && smr.GMT_DateTime.Month.Equals(item.Month) && smr.GMT_DateTime.Year.Equals(item.Year)
                                        && smr.DefaultMediaType
                                        && (
                                            CheckSubMediaTypeAccess(p_MediaTypeList, smr.SubMediaType))
                                    ).Sum(s => s.Number_Docs);

                            // set data point of current series 
                            /*
                                *  y = y series value of current point === total no. of records for current search request at perticular date 
                                *  SearchTerm = query name  , used in chart drill down click event
                                *  Value = Search Request ID  , used in chart drill down click event
                                *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                            */
                            HighChartDatum highChartDatum = new HighChartDatum();
                            highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                            highChartDatum.SearchTerm = searchRequest.Value;
                            highChartDatum.Value = searchRequest.Key.ToString();
                            highChartDatum.Type = "Media";
                            series.data.Add(highChartDatum);
                        }

                        // Keep track of the association between agent and series color so that third party data can match colors by agent
                        dictSeriesColors.Add(searchRequest.Key, series.color);
                        lstSeries.Add(series);
                    }
                }
                else
                {
                    // as its single media chart, we will show it as area chart, by setting chart type to "area"
                    highLineChartOutput.hChart.type = "areaspline";

                    // set plot options for area chart, for series click event, and plot marker.
                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        area = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };


                    // set sereies name as "Media" , will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = "Media";

                    // loop for each date to create list of data for media series
                    foreach (var item in dateRange)
                    {
                        //var singleSubMediaRec =
                        var sumOfDocs = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Month.Equals(item.Month) && smr.GMT_DateTime.Year.Equals(item.Year)
                            && smr.DefaultMediaType
                            && (
                                    CheckSubMediaTypeAccess(p_MediaTypeList, smr.SubMediaType)
                                )
                            ).Sum(s => s.Number_Docs);


                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                            *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                            *  we will not set SearchTerm and Value properies of data, as this is signle medium chart , without any search request
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = sumOfDocs != null ? sumOfDocs : 0;
                        highChartDatum.Type = "Media";
                        series.data.Add(highChartDatum);
                    }
                    lstSeries.Add(series);
                }

                // assign set of series data to medium chart (or multi line searchrequest chart)
                highLineChartOutput.series = lstSeries;


                //Sub Media  vice Records
                // this is multi line sub media chart, for each line series, for each medium type , for records exist for that medium type. 
                HighLineChartOutput highLineChartSubMediaOutput = new HighLineChartOutput();
                highLineChartSubMediaOutput.title = new Title() { text = "", x = -20 };
                highLineChartSubMediaOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartSubMediaOutput.yAxis = new List<YAxis>() { new YAxis() { title = new Title2() } };


                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSubMediaOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories,
                    labels = new labels()
                    {
                        formatter = "GetMonth"
                    }
                };
                highLineChartSubMediaOutput.tooltip = new Tooltip() { valueSuffix = "" };
                highLineChartSubMediaOutput.legend = new Legend() { borderWidth = "0" };
                highLineChartSubMediaOutput.hChart = new HChart() { height = 300, type = "spline" };

                // set plot options and click event for series points (which will again assigned in JS as this is string value)
                highLineChartSubMediaOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = true
                        }
                    },
                    series = new PlotSeries()
                    {
                        cursor = "pointer",
                        point = new PlotPoint()
                        {
                            events = new PlotEvents()
                            {
                                click = "LineChartClick"
                            }
                        }
                    }
                };


                // this is signle line spark chart , different for each medium type (with all commom properties set here)
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
                //highLineChartSingleMediaChartOutput.Colors = new List<string>();

                List<PlotLine> plotlinesSingleMedia = new List<PlotLine>();

                // set min = 0 , to force chart to start from 0 , and show line in bottom, 
                // gridLineWidth = 0 , to hide grid lines on y axis. 
                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                // not to show x axis labels for spark charts 
                // we have set default value for TickWidth to 0 in XAxis class defination, to not to show line below x-axis for ticks.
                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = categories,
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // add event on chart click , to load medium type summary of selected medium chart. 
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // add event on chart points click , to load medium type summary of selected medium chart. 
                // also disble marker on chart
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        point = new PlotPoint()
                        {
                            events = new PlotEvents()
                            {
                                click = "ChangeMediumTypeOnPointClick"
                            }
                        },
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };

                Int64 totNumOfHits = listOfSummaryReportData.Where(smr =>
                                    smr.DefaultMediaType
                                 && CheckSubMediaTypeAccess(p_MediaTypeList, smr.SubMediaType)
                                ).Sum(s => s.Number_Docs);
                lstSummaryReportMulti.TotalNumOfHits = totNumOfHits.ToString("N0");

                // start to set series of data for  multi line medium chart
                List<Series> lstSeriesSubMediaType = new List<Series>();
                lstSummaryReportMulti.SummaryReportMedium = new List<SummaryReportMedium>();

                foreach (var media in p_MediaTypeList.Where(m => m.TypeLevel == 1 && m.HasAccess && p_MediaTypeList.Where(sm=>string.Compare(m.MediaType, sm.MediaType, true) == 0 && sm.TypeLevel == 2 && sm.HasAccess).Count() > 0))
                {

                    // set sereies name of multiline medium chart as display name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = media.DisplayName;

                    // set sereies name of signle line spark medium chart  as medium description, will shown in legent and tooltip.
                    Series seriesSingleMedia = new Series();
                    seriesSingleMedia.data = new List<HighChartDatum>();
                    seriesSingleMedia.name = "";

                    // loop for each date to create list of data for selected medium type
                    foreach (var item in dateRange)
                    {
                        //var singleSubMediaRec =
                        var daywiseSum = listOfSummaryReportData.Where(smr => String.Compare(smr.MediaType, media.MediaType, true) == 0 &&  CheckSubMediaTypeAccess(p_MediaTypeList, smr.SubMediaType) && smr.GMT_DateTime.Month.Equals(item.Month) && smr.GMT_DateTime.Year.Equals(item.Year)).Sum(s => s.Number_Docs);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current medium type at perticular date 
                            *  SearchTerm = medium description  , used in chart drill down click event
                            *  Value = medium tpye  , used in chart drill down click event
                            *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                        highChartDatum.SearchTerm = media.DisplayName;
                        highChartDatum.Value = media.MediaType;
                        highChartDatum.Type = "SubMedia";
                        series.data.Add(highChartDatum);

                        seriesSingleMedia.data.Add(highChartDatum);
                    }

                    lstSeriesSubMediaType.Add(series);

                    // set signle series for spark chart of current medium type, and assign list of data for that series.
                    List<Series> lstSeriesSingleMediaType = new List<Series>();
                    lstSeriesSingleMediaType.Add(seriesSingleMedia);
                    highLineChartSingleMediaChartOutput.series = lstSeriesSingleMediaType;

                    // set json chart for spark chart based on medium type                    

                    lstSummaryReportMulti.SummaryReportMedium.Add(new SummaryReportMedium()
                    {
                        MediaTypeModel = media,
                        PrevRecordsSum = (p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType != null && string.Compare(media.MediaType, a.MediaType, true) == 0 && CheckSubMediaTypeAccess(p_MediaTypeList, a.SubMediaType)).Sum(s => s.NoOfDocs) : 0),
                        Records = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput),
                        RecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)))

                    });

                    /*
                    if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.TV.ToString(), true) == 0)
                    {
                        lstSummaryReportMulti.TVRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                        lstSummaryReportMulti.TVRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                        lstSummaryReportMulti.TVPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.TV.ToString()).Sum(s => s.NoOfDocs) : 0;
                    }
                    else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.NM.ToString(), true) == 0)
                    {
                        lstSummaryReportMulti.NMRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                        lstSummaryReportMulti.NMRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                        lstSummaryReportMulti.NMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.NM.ToString()).Sum(s => s.NoOfDocs) : 0;
                    }
                    else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.TW.ToString(), true) == 0)
                    {
                        lstSummaryReportMulti.TWRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                        lstSummaryReportMulti.TWRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                        lstSummaryReportMulti.TWPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.TW.ToString()).Sum(s => s.NoOfDocs) : 0;
                    }
                    else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.Forum.ToString(), true) == 0)
                    {
                        lstSummaryReportMulti.ForumRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                        lstSummaryReportMulti.ForumRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                        lstSummaryReportMulti.ForumPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.Forum.ToString()).Sum(s => s.NoOfDocs) : 0;
                    }
                    else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.SocialMedia.ToString(), true) == 0)
                    {
                        lstSummaryReportMulti.SocialMediaRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                        lstSummaryReportMulti.SocialMediaRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                        lstSummaryReportMulti.SocialMediaPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.SocialMedia.ToString()).Sum(s => s.NoOfDocs) : 0;
                    }
                    else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.Blog.ToString(), true) == 0)
                    {
                        lstSummaryReportMulti.BlogRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                        lstSummaryReportMulti.BlogRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                        lstSummaryReportMulti.BlogPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.Blog.ToString()).Sum(s => s.NoOfDocs) : 0;
                    }
                    else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.PM.ToString(), true) == 0)
                    {
                        lstSummaryReportMulti.PMRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                        lstSummaryReportMulti.PMRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                        lstSummaryReportMulti.PMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.PM.ToString()).Sum(s => s.NoOfDocs) : 0;
                    }
                    else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.Radio.ToString(), true) == 0)
                    {
                        lstSummaryReportMulti.TMRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                        lstSummaryReportMulti.TMRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                        lstSummaryReportMulti.TMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.DashBoardMediumType.Radio.ToString()).Sum(s => s.NoOfDocs) : 0;
                    }
                    else if (string.Compare(subMedia.ToString(), CommonFunctions.DashBoardMediumType.MS.ToString(), true) == 0)
                    {
                        lstSummaryReportMulti.MSRecords = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                        lstSummaryReportMulti.MSRecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)));
                    }*/

                }

                // Create series for third party data
                if (p_IsThirdPartyAccess)
                {
                    BuildThirdPartySeries(listOfSummaryReportData, p_SearchRequests, p_ThirdPartyDataTypes, dateRange, dictSeriesColors, highLineChartOutput.series, highLineChartOutput.yAxis, lstSeriesSubMediaType, highLineChartSubMediaOutput.yAxis, true);
                }

                // assign set of series data to multi line medium type chart
                highLineChartSubMediaOutput.series = lstSeriesSubMediaType;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSubMediaOutput);
                lstSummaryReportMulti.SubMediaRecords = jsonResult;

                jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
                lstSummaryReportMulti.MediaRecords = jsonResult;

                
                // create spark chart for audience and media value
                if (p_MediaTypeList.Where(m => m.TypeLevel == 2 && m.HasAccess == true && (m.UseAudience == true || m.UseMediaValue == true)).Count() > 0)
                {
                    //Single Media Chart 
                    HighLineChartOutput highLineChartAudienceMediaValue = new HighLineChartOutput();
                    highLineChartAudienceMediaValue.title = new Title() { text = "", x = -20 };
                    highLineChartAudienceMediaValue.subtitle = new Subtitle() { text = "", x = -20 };
                    //highLineChartAudienceMediaValue.Colors = new List<string>();

                    highLineChartAudienceMediaValue.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                    highLineChartAudienceMediaValue.xAxis = new XAxis()
                    {
                        tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                        tickmarkPlacement = "off",
                        categories = categories,
                        labels = new labels()
                        {
                            enabled = false
                        }
                    };


                    highLineChartAudienceMediaValue.hChart = new HChart() { height = 100, width = 120, type = "spline" };
                    highLineChartAudienceMediaValue.plotOption = new PlotOptions()
                    {
                        spline = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = false,
                                lineWidth = 0
                            }
                        }
                    };
                    highLineChartAudienceMediaValue.tooltip = new Tooltip() { valueSuffix = "" };
                    highLineChartAudienceMediaValue.legend = new Legend() { enabled = false };

                    if (p_MediaTypeList.Where(m => m.TypeLevel == 2 && m.HasAccess == true && m.UseMediaValue == true).Count() > 0)
                    {
                        List<Series> lstSeriesMediaValue = new List<Series>();

                        Series series = new Series();
                        series.data = new List<HighChartDatum>();
                        series.name = "Media Value";

                        foreach (var item in dateRange)
                        {

                            var singleIQMediaValueRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Month.Equals(item.Month) && smr.GMT_DateTime.Year.Equals(item.Year)
                                && smr.DefaultMediaType
                                && (
                                            p_MediaTypeList.Where(m => CheckSubMediaTypeAccess(m, smr.SubMediaType)
                                            && CommonFunctions.CheckNielsenCompeteAccess(m.UseMediaValue, m.RequireNielsenAccess, p_NielsenAccess, m.RequireCompeteAccess, p_CompeteDataAccess)).Count() > 0
                                   )
                              ).Sum(s => s.IQMediaValue);// SingleOrDefault();

                            HighChartDatum highChartDatum = new HighChartDatum();
                            highChartDatum.y = Convert.ToDecimal(singleIQMediaValueRec != null ? singleIQMediaValueRec : 0);
                            series.data.Add(highChartDatum);
                        }

                        lstSeriesMediaValue.Add(series);

                        highLineChartAudienceMediaValue.series = lstSeriesMediaValue;

                        lstSummaryReportMulti.IQMediaValueRecords = CommonFunctions.SearializeJson(highLineChartAudienceMediaValue);


                        //lstSummaryReportMulti.IQMediaValueRecords = temp;
                        lstSummaryReportMulti.IQMediaValueRecordsSum = (series.data.Sum(s => Convert.ToDecimal(s.y)));
                        lstSummaryReportMulti.IQMediaValuePrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(smr =>

                                        p_MediaTypeList.Where(m => CheckSubMediaTypeAccess(m, smr.SubMediaType)
                                            && CommonFunctions.CheckNielsenCompeteAccess(m.UseMediaValue, m.RequireNielsenAccess, p_NielsenAccess, m.RequireCompeteAccess, p_CompeteDataAccess)).Count() > 0

                                          ).Sum(a => a.IQMediaValue) : 0;
                    }



                    if (p_MediaTypeList.Where(m => m.TypeLevel == 2 && m.HasAccess == true && m.UseAudience == true).Count() > 0)
                    {
                        List<Series> lstSeriesAudience = new List<Series>();
                        Series seriesAudience = new Series();
                        seriesAudience.name = "Audience";
                        seriesAudience.data = new List<HighChartDatum>();

                        foreach (var item in dateRange)
                        {

                            var singleAudienceRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Month.Equals(item.Month) && smr.GMT_DateTime.Year.Equals(item.Year)
                                && smr.DefaultMediaType
                                && (
                                            p_MediaTypeList.Where(m => CheckSubMediaTypeAccess(m, smr.SubMediaType)
                                            && CommonFunctions.CheckNielsenCompeteAccess(m.UseAudience, m.RequireNielsenAccess, p_NielsenAccess, m.RequireCompeteAccess, p_CompeteDataAccess)).Count() > 0
                                    )
                            ).Sum(s => s.Audience);// SingleOrDefault();

                            HighChartDatum highChartDatum = new HighChartDatum();
                            highChartDatum.y = singleAudienceRec;
                            seriesAudience.data.Add(highChartDatum);

                        }

                        lstSeriesAudience.Add(seriesAudience);

                        highLineChartAudienceMediaValue.series = lstSeriesAudience;

                        lstSummaryReportMulti.AudienceRecords = CommonFunctions.SearializeJson(highLineChartAudienceMediaValue);

                        lstSummaryReportMulti.AudienceRecordsSum = (seriesAudience.data.Sum(s => Convert.ToInt64(s.y)));
                        lstSummaryReportMulti.AudiencePrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(smr =>

                            p_MediaTypeList.Where(m => CheckSubMediaTypeAccess(m, smr.SubMediaType)
                                            && CommonFunctions.CheckNielsenCompeteAccess(m.UseAudience, m.RequireNielsenAccess, p_NielsenAccess, m.RequireCompeteAccess, p_CompeteDataAccess)).Count() > 0

                                            ).Sum(a => a.Audience) : 0;
                    }
                }

                return lstSummaryReportMulti;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region Hourly Chart

        /*

        public SummaryReportMulti LineChartHour(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, bool p_Isv4TMAccess, decimal p_ClientGmtOffset, decimal p_ClientDstOffset, List<string> p_SearchRequestIDs, List<IQAgent_ComparisionValues> p_ListOfIQAgent_ComparisionValues,
            bool p_Isv4NMAccess, bool p_Isv4SMAccess, bool p_Isv4TWAccess, bool p_Isv4TVAccess, bool p_Isv4BLPMAccess, bool p_NielsenAccess, bool p_CompeteDataAccess)
        {
            try
            {

                var dateRange = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    dateRange.Add(dt);
                }

                SummaryReportMulti lstSummaryReportMulti = new SummaryReportMulti();


                //var mediaRecords = listOfSummaryReportData.GroupBy(sr => sr.GMT_DateTime).Select(tr => new SummaryReportModel { GMT_DateTime = tr.Key, Number_Docs = tr.Sum(r => r.Number_Docs) });
                //var subMediaRecords = listOfSummaryReportData.GroupBy(sr => new { sr.SubMediaType, sr.GMT_DateTime }).Select(tr => new SummaryReportModel { SubMediaType = tr.Key.SubMediaType, GMT_DateTime = tr.Key.GMT_DateTime, Number_Docs = tr.Sum(r => r.Number_Docs) });
                //var audienceRecords = listOfSummaryReportData.GroupBy(sr => sr.GMT_DateTime).Select(tr => new SummaryReportModel { GMT_DateTime = tr.Key, Audience = tr.Sum(r => r.Audience) });
                //var iqMediaValueRecords = listOfSummaryReportData.GroupBy(sr => sr.GMT_DateTime).Select(tr => new SummaryReportModel { GMT_DateTime = tr.Key, IQMediaValue = tr.Sum(r => r.IQMediaValue) });


                Chart chart = new Chart();
                chart.subcaption = "";
                chart.caption = string.Empty;// "Summary Report";
                chart.linethickness = "1";
                chart.showvalues = "0";
                //chart.showLabels = "1";
                //chart.showYAxisValues = "1";
                chart.formatnumberscale = "0";
                chart.anchorRadius = "3";
                chart.divlinealpha = "20";
                chart.divlinecolor = "000000";
                chart.divlineisdashed = "0";
                chart.showalternatehgridcolor = "1";
                chart.alternatehgridcolor = "FFFFFF";
                chart.shadowalpha = "40";
                chart.labelstep = "1";
                chart.numvdivlines = "0";
                chart.chartrightmargin = "10";
                chart.bgcolor = "FFFFFF";
                chart.bgangle = "270";
                chart.bgalpha = "10,10";
                chart.alternatehgridalpha = "5";
                chart.legendposition = "BOTTOM";
                chart.drawAnchors = "1";
                chart.showBorder = "0";
                chart.canvasBorderAlpha = "0";
                //chart.lineColor = "#4493D6";

                //Multi Line chart

                Chart multiChart = new Chart();
                multiChart.subcaption = "";
                multiChart.caption = "";
                multiChart.linethickness = "1";
                //multiChart.showLabels = "0";
                multiChart.showvalues = "0";
                //multiChart.showYAxisValues = "0";
                multiChart.formatnumberscale = "0";
                multiChart.anchorRadius = "3";
                multiChart.divlinealpha = "20";
                multiChart.divlinecolor = "000000";
                multiChart.divlineisdashed = "0";
                multiChart.showalternatehgridcolor = "1";
                multiChart.alternatehgridcolor = "FFFFFF";
                multiChart.shadowalpha = "40";
                multiChart.labelstep = "1";
                multiChart.numvdivlines = "0";
                multiChart.chartrightmargin = "10";
                multiChart.bgcolor = "FFFFFF";
                multiChart.bgangle = "270";
                multiChart.bgalpha = "10,10";
                multiChart.alternatehgridalpha = "5";
                multiChart.legendposition = "BOTTOM";
                multiChart.drawAnchors = "1";
                multiChart.showBorder = "0";
                multiChart.canvasBorderAlpha = "0";
                multiChart.palettecolors = "15335D,448FF2,7A045C,FFB451,E14A02,394900,005E8F,A7B1B3";

                //

                //Media Records

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();

                foreach (var date in dateRange)
                {
                    Category2 category2 = new Category2();
                    category2.label = date.ToString();
                    if (date.IsDaylightSavingTime())
                    {

                        category2.label = date.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString();
                    }
                    else
                    {
                        category2.label = date.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString();
                    }
                    allCategory.category.Add(category2);
                }

                lstallCategory.Add(allCategory);

                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();

                SparkChart sparkChartMediaWise = new SparkChart();
                SparkChartOutput sparkChartOutputMediaWise = new SparkChartOutput();
                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    //Multi Line Charts
                    List<SeriesData> multiLstSeriesData = new List<SeriesData>();
                    foreach (var searchRequest in p_SearchRequestIDs)
                    {
                        // Spark Chart Series Init
                        sparkChartOutputMediaWise.chart = sparkChartMediaWise;
                        List<SparkSeriesData> lstSparkSeriesDataMediaWise = new List<SparkSeriesData>();
                        SparkSeriesData sparkSeriesMediaWise = new SparkSeriesData();
                        sparkSeriesMediaWise.data = new List<SparkDatum>();
                        //sparkChartMediaWise.caption = CommonFunctions.GetEnumDescription(subMedia);


                        var SearchRequest = listOfSummaryReportData.Where(a => string.Compare(a.SearchRequestID.ToString(), searchRequest) == 0).FirstOrDefault();
                        if (SearchRequest != null)
                        {


                            SeriesData multiSeriesData = new SeriesData();
                            multiSeriesData.data = new List<Datum>();

                            multiSeriesData.seriesname = SearchRequest.Query_Name;
                            multiSeriesData.color = "";
                            //multiSeriesData.anchorBorderColor = "";
                            //multiSeriesData.anchorBgColor = "";

                            foreach (var item in dateRange)
                            {
                                var singleSubMediaRec = listOfSummaryReportData.Where(smr => String.Compare(smr.SearchRequestID.ToString(), searchRequest, true) == 0 && smr.GMT_DateTime.Equals(item)
                                    && (
                                            (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                            (p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) &&
                                            (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) &&
                                            (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) &&
                                            (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) &&
                                            (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                            (p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() &&
                                            (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))
                                        )
                                ).Sum(s => s.Number_Docs);// SingleOrDefault();

                                Datum datum = new Datum();
                                datum.value = Convert.ToString(singleSubMediaRec != null ? singleSubMediaRec : 0);
                                datum.link = "javascript:OpenFeed('" + item.ToShortDateString() + "','','','[" + searchRequest + "]','[\"" + System.Web.HttpUtility.UrlEncode(SearchRequest.Query_Name.Replace("\"", "\\\"")) + "\"]')";

                                seriesData.data.Add(datum);
                                multiSeriesData.data.Add(datum);

                                SparkDatum sparkDatum = new SparkDatum();
                                sparkDatum.value = Convert.ToString(singleSubMediaRec != null ? singleSubMediaRec : 0);
                                sparkSeriesMediaWise.data.Add(sparkDatum);
                            }

                            lstSparkSeriesDataMediaWise.Add(sparkSeriesMediaWise);
                            sparkChartOutputMediaWise.dataset = lstSparkSeriesDataMediaWise;

                            //Multi Line Charts
                            multiLstSeriesData.Add(multiSeriesData);
                        }
                    }
                    lineChartOutput.categories = lstallCategory;
                    lineChartOutput.dataset = multiLstSeriesData;
                }
                else
                {


                    seriesData.seriesname = "Media";
                    seriesData.color = "";
                    //seriesData.anchorBorderColor = "";
                    //seriesData.anchorBgColor = "";

                    foreach (var item in dateRange)
                    {
                        var singleSubMediaRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Equals(item)
                            && (
                                    (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                    (p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) &&
                                    (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                    (p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() &&
                                    (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))
                                )
                         ).Sum(s => s.Number_Docs);// SingleOrDefault();

                        Datum datum = new Datum();
                        datum.value = Convert.ToString(singleSubMediaRec != null ? singleSubMediaRec : 0);
                        datum.link = "javascript:OpenFeed('" + item.ToShortDateString() + "','','','','')";
                        seriesData.data.Add(datum);
                    }
                    lstSeriesData.Add(seriesData);

                    lineChartOutput.categories = lstallCategory;
                    lineChartOutput.dataset = lstSeriesData;
                }

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                lstSummaryReportMulti.MediaRecords = jsonResult;


                //Sub Media Recors

                lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = multiChart;

                var distinctSubMedia = listOfSummaryReportData.Select(d => d.SubMediaType).Distinct().ToList();
                lstSeriesData = new List<SeriesData>();

                List<CommonFunctions.CategoryType> lstMediaCategories = Enum.GetValues(typeof(CommonFunctions.CategoryType)).Cast<CommonFunctions.CategoryType>().ToList();
                Int64 totNumOfHits = listOfSummaryReportData.Where(smr =>
                                    (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                    (p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) &&
                                    (p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) &&
                                    (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                    (p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() &&
                                    (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))
                                ).Sum(s => s.Number_Docs);
                lstSummaryReportMulti.TotalNumOfHits = totNumOfHits.ToString("N0");


                //Spark Chart Init
                sparkChartMediaWise = new SparkChart();
                sparkChartOutputMediaWise = new SparkChartOutput();

                string searchRequestIds = string.Empty;
                string searchRequestNames = string.Empty;

                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    searchRequestIds = Newtonsoft.Json.JsonConvert.SerializeObject(listOfSummaryReportData.Select(a => a.SearchRequestID).Distinct());
                    searchRequestNames = Newtonsoft.Json.JsonConvert.SerializeObject(listOfSummaryReportData.Select(a => a.Query_Name).Distinct());
                }

                foreach (var subMedia in lstMediaCategories)
                {
                    if (
                        (p_Isv4TMAccess || subMedia != CommonFunctions.CategoryType.Radio) &&
                        (p_Isv4NMAccess || subMedia != CommonFunctions.CategoryType.NM) &&
                        (p_Isv4SMAccess || subMedia != CommonFunctions.CategoryType.SocialMedia) &&
                        (p_Isv4SMAccess || subMedia != CommonFunctions.CategoryType.Forum) &&
                        (p_Isv4SMAccess || subMedia != CommonFunctions.CategoryType.Blog) &&
                        (p_Isv4TWAccess || subMedia != CommonFunctions.CategoryType.TW) &&
                        (p_Isv4TVAccess || subMedia != CommonFunctions.CategoryType.TV) &&
                        (p_Isv4BLPMAccess || subMedia != CommonFunctions.CategoryType.PM))
                    {

                        //}
                        //foreach (string subMedia in distinctSubMedia)
                        //{

                        // Spark Chart Series Init
                        sparkChartOutputMediaWise.chart = sparkChartMediaWise;
                        List<SparkSeriesData> lstSparkSeriesDataMediaWise = new List<SparkSeriesData>();
                        SparkSeriesData sparkSeriesMediaWise = new SparkSeriesData();
                        sparkSeriesMediaWise.data = new List<SparkDatum>();
                        //sparkChartMediaWise.caption = CommonFunctions.GetEnumDescription(subMedia);


                        // Line Chart Series Init
                        seriesData = new SeriesData();
                        seriesData.data = new List<Datum>();
                        string enumDesc = CommonFunctions.GetEnumDescription(subMedia);
                        //multiChart.caption = enumDesc;
                        seriesData.seriesname = enumDesc;
                        seriesData.color = "";
                        //seriesData.anchorBorderColor = "";
                        //seriesData.anchorBgColor = "";

                        SeriesData multiSeriesData = new SeriesData();
                        multiSeriesData.data = new List<Datum>();

                        multiSeriesData.seriesname = "";
                        multiSeriesData.color = "";
                        //multiSeriesData.anchorBorderColor = "";
                        //multiSeriesData.anchorBgColor = "";


                        //Multi Line Charts
                        List<SeriesData> multiLstSeriesData = new List<SeriesData>();
                        //
                        foreach (var item in dateRange)
                        {
                            var singleSubMediaRec = listOfSummaryReportData.Where(smr => String.Compare(smr.SubMediaType, subMedia.ToString(), true) == 0 && smr.GMT_DateTime.Equals(item)).Sum(s => s.Number_Docs);// SingleOrDefault();

                            Datum datum = new Datum();
                            datum.value = Convert.ToString(singleSubMediaRec != null ? singleSubMediaRec : 0);
                            datum.link = "javascript:OpenFeed('" + item.ToShortDateString() + "','" + subMedia.ToString() + "','" + CommonFunctions.GetEnumDescription(subMedia) + "','" + searchRequestIds + "','" + searchRequestNames.Replace("\'", "\\\'") + "')";

                            seriesData.data.Add(datum);
                            multiSeriesData.data.Add(datum);

                            SparkDatum sparkDatum = new SparkDatum();
                            sparkDatum.value = Convert.ToString(singleSubMediaRec != null ? singleSubMediaRec : 0);
                            sparkSeriesMediaWise.data.Add(sparkDatum);
                        }

                        lstSparkSeriesDataMediaWise.Add(sparkSeriesMediaWise);
                        sparkChartOutputMediaWise.dataset = lstSparkSeriesDataMediaWise;


                        //Multi Line Charts
                        multiLstSeriesData.Add(multiSeriesData);

                        if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.TV.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.TVRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.TVRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.TVPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.TV.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.NM.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.NMRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.NMRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.NMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.NM.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.TW.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.TWRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.TWRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.TWPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.TW.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.Forum.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.ForumRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.ForumRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.ForumPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.Forum.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.SocialMedia.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.SocialMediaRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.SocialMediaRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.SocialMediaPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.SocialMedia.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.Blog.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.BlogRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.BlogRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.BlogPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.Blog.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.PM.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.PMRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.PMRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.PMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.PM.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        else if (string.Compare(subMedia.ToString(), CommonFunctions.CategoryType.Radio.ToString(), true) == 0)
                        {
                            lstSummaryReportMulti.TMRecords = CommonFunctions.SearializeJson(sparkChartOutputMediaWise);
                            lstSummaryReportMulti.TMRecordsSum = (multiSeriesData.data.Sum(s => Convert.ToInt64(s.value)));
                            lstSummaryReportMulti.TMPrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType == CommonFunctions.CategoryType.Radio.ToString()).Sum(s => s.NoOfDocs) : 0;
                        }
                        //

                        lstSeriesData.Add(seriesData);
                    }
                }


                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                lstSummaryReportMulti.SubMediaRecords = jsonResult;

                if (p_Isv4TMAccess || p_Isv4NMAccess || p_Isv4SMAccess || p_Isv4TWAccess || p_Isv4TVAccess || p_Isv4BLPMAccess)
                {
                    //IQ Media Value
                    SparkChart sparkChart = new SparkChart();
                    //sparkChart.caption = "Ad Value";
                    sparkChart.palette = "5";
                    sparkChart.setAdaptiveYMin = "0";
                    sparkChart.showCloseAnchor = "0";
                    sparkChart.showCloseValue = "0";
                    sparkChart.showHighAnchor = "0";
                    sparkChart.showHighLowValue = "0";
                    sparkChart.showOpenAnchor = "0";
                    sparkChart.showOpenValue = "0";
                    sparkChart.showLowAnchor = "0";
                    sparkChart.showToolTip = "1";
                    sparkChart.bgColor = "FFFFFF";
                    sparkChart.lineColor = "#4493D6";
                    // chart1.formatNumber = "0";
                    //chart1.thousandSeparator = ",";
                    sparkChart.thousandSeparatorPosition = "0";
                    sparkChart.formatNumberScale = "0";
                    //sparkChart.caption = "Ad Value";
                    SparkChartOutput sparkChartOutput = new SparkChartOutput();
                    sparkChartOutput.chart = sparkChart;
                    List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                    SparkSeriesData sparkSeries = new SparkSeriesData();
                    sparkSeries.data = new List<SparkDatum>();

                    foreach (var item in dateRange)
                    {

                        var singleIQMediaValueRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Equals(item)
                            && (
                                        (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                        ((p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) && p_CompeteDataAccess) &&
                                        (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                        ((p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() && p_NielsenAccess) &&
                                        (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))
                               )
                          ).Sum(sm => sm.IQMediaValue);// SingleOrDefault();

                        SparkDatum datum = new SparkDatum();
                        datum.value = Convert.ToString(singleIQMediaValueRec != null ? singleIQMediaValueRec : 0);
                        sparkSeries.data.Add(datum);
                    }
                    lstSparkSeriesData.Add(sparkSeries);
                    sparkChartOutput.dataset = lstSparkSeriesData;
                    lstSummaryReportMulti.IQMediaValueRecords = CommonFunctions.SearializeJson(sparkChartOutput);

                    lstSummaryReportMulti.IQMediaValueRecordsSum = (sparkSeries.data.Sum(s => Convert.ToDecimal(s.value)));
                    lstSummaryReportMulti.IQMediaValuePrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(smr => (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                        ((p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) && p_CompeteDataAccess) &&
                                        (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                        ((p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() && p_NielsenAccess) &&
                                        (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))).Sum(a => a.IQMediaValue) : 0;


                    //Audience
                    sparkChartOutput = new SparkChartOutput();
                    //sparkChart.caption = "Views";
                    sparkChartOutput.chart = sparkChart;
                    lstSparkSeriesData = new List<SparkSeriesData>();
                    sparkSeries = new SparkSeriesData();
                    sparkSeries.data = new List<SparkDatum>();
                    foreach (var item in dateRange)
                    {

                        var singleAudienceRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Equals(item)
                            && (
                                        (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                        ((p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) && p_CompeteDataAccess) &&
                                        (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                        ((p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() && p_NielsenAccess) &&
                                        (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))
                               )
                        ).Sum(sm => sm.Audience);// SingleOrDefault();

                        SparkDatum datum = new SparkDatum();
                        datum.value = Convert.ToString(singleAudienceRec != null ? singleAudienceRec : 0);
                        sparkSeries.data.Add(datum);
                    }
                    lstSparkSeriesData.Add(sparkSeries);
                    sparkChartOutput.dataset = lstSparkSeriesData;
                    lstSummaryReportMulti.AudienceRecords = CommonFunctions.SearializeJson(sparkChartOutput);

                    lstSummaryReportMulti.AudienceRecordsSum = (sparkSeries.data.Sum(s => Convert.ToInt64(s.value)));
                    lstSummaryReportMulti.AudiencePrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(smr => (p_Isv4TMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Radio.ToString()) &&
                                        ((p_Isv4NMAccess || smr.SubMediaType != CommonFunctions.CategoryType.NM.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.SocialMedia.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Forum.ToString()) && p_CompeteDataAccess) &&
                                        ((p_Isv4SMAccess || smr.SubMediaType != CommonFunctions.CategoryType.Blog.ToString()) && p_CompeteDataAccess) &&
                                        (p_Isv4TWAccess || smr.SubMediaType != CommonFunctions.CategoryType.TW.ToString()) &&
                                        ((p_Isv4TVAccess || smr.SubMediaType != CommonFunctions.CategoryType.TV.ToString() && p_NielsenAccess) &&
                                        (p_Isv4BLPMAccess || smr.SubMediaType != CommonFunctions.CategoryType.PM.ToString()))).Sum(a => a.Audience) : 0;
                }

                return lstSummaryReportMulti;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        */

        public SummaryReportMulti HighChartsLineChartHour(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, int? chartWidth, decimal p_ClientGmtOffset, decimal p_ClientDstOffset, Dictionary<long, string> p_SearchRequests, List<IQAgent_ComparisionValues> p_ListOfIQAgent_ComparisionValues, List<ThirdPartyDataTypeModel> p_ThirdPartyDataTypes, bool p_NielsenAccess, bool p_CompeteDataAccess, bool p_IsThirdPartyAccess, List<IQ_MediaTypeModel> p_MediaTypeList)
        {
            try
            {
                Dictionary<long, string> dictSeriesColors = new Dictionary<long, string>();

                var dateRange = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    dateRange.Add(dt);
                }

                SummaryReportMulti lstSummaryReportMulti = new SummaryReportMulti();


                List<string> categories = new List<string>();

                foreach (var date in dateRange)
                {
                    if (date.IsDaylightSavingTime())
                    {
                        categories.Add(date.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString());
                    }
                    else
                    {
                        categories.Add(date.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString());
                    }
                }

                // this signle line medium chart, with out applying any medium filter.... 
                // if one or more request request applid, then it will show multi line chart, one for each request request. 

                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
                highLineChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartOutput.yAxis = new List<YAxis>() { new YAxis(){ min = 0, title = new Title2() }};

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories,
                    labels = new labels() { staggerLines = 2 }
                };

                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { borderWidth = "0", width = 750 };

                // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
                highLineChartOutput.hChart = new HChart() { height = 300, type = "spline" };

                // start to set series of data for medium chart (or multi line search request chart)
                List<Series> lstSeries = new List<Series>();

                // if one or more search request is applied, then will set multiple seareis , each series for each request request
                // with total no. of records for that search request on perticular date (value of category)
                if (p_SearchRequests != null && p_SearchRequests.Count > 0)
                {

                    // set plot options and click event for series points (which will again assigned in JS as this is string value)
                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        spline = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };

                    // set list of data for each series 
                    List<SeriesData> multiLstSeriesData = new List<SeriesData>();
                    foreach (var searchRequest in p_SearchRequests)
                    {
                        // set sereies name as search request query name, will shown in legent and tooltip.
                        Series series = new Series();
                        series.data = new List<HighChartDatum>();
                        series.name = searchRequest.Value;
                        series.color = highLineChartOutput.colors[lstSeries.Count % highLineChartOutput.colors.Count];

                        // loop for each date to create list of data for selected search request series. 
                        foreach (var item in dateRange)
                        {
                            var daywiseSum = listOfSummaryReportData.Where(smr => smr.SearchRequestID == searchRequest.Key && smr.GMT_DateTime.Equals(item)
                                    && smr.DefaultMediaType
                                    && CheckSubMediaTypeAccess(p_MediaTypeList, smr.SubMediaType)
                                ).Sum(s => s.Number_Docs);

                            // set data point of current series 
                            /*
                                *  y = y series value of current point === total no. of records for current search request at perticular date 
                                *  SearchTerm = query name  , used in chart drill down click event
                                *  Value = Search Request ID  , used in chart drill down click event
                                *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                            */
                            HighChartDatum highChartDatum = new HighChartDatum();
                            highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                            highChartDatum.SearchTerm = searchRequest.Value;
                            highChartDatum.Value = searchRequest.Key.ToString();
                            highChartDatum.Type = "Media";
                            series.data.Add(highChartDatum);
                        }

                        lstSeries.Add(series);

                        // Keep track of the association between agent and series color so that third party data can match colors by agent
                        dictSeriesColors.Add(searchRequest.Key, series.color);
                        lstSeries.Add(series);
                    }
                }
                else
                {
                    // as its single media chart, we will show it as area chart, by setting chart type to "area"
                    highLineChartOutput.hChart.type = "areaspline";

                    // set plot options for area chart, for series click event, and plot marker.
                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        area = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };

                    // set sereies name as "Media" , will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = "Media";

                    // loop for each date to create list of data for media series
                    foreach (var item in dateRange)
                    {

                        var sumOfDocs = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Equals(item)
                            && smr.DefaultMediaType
                            && CheckSubMediaTypeAccess(p_MediaTypeList, smr.SubMediaType)).Sum(s => s.Number_Docs);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                            *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                            *  we will not set SearchTerm and Value properies of data, as this is signle medium chart , without any search request
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = sumOfDocs != null ? sumOfDocs : 0;
                        highChartDatum.Type = "Media";
                        series.data.Add(highChartDatum);
                    }
                    lstSeries.Add(series);
                }

                // assign set of series data to medium chart (or multi line searchrequest chart)
                highLineChartOutput.series = lstSeries;


                //Sub Media  vice Records
                // this is multi line sub media chart, for each line series, for each medium type , for records exist for that medium type. 
                HighLineChartOutput highLineChartSubMediaOutput = new HighLineChartOutput();
                highLineChartSubMediaOutput.title = new Title() { text = "", x = -20 };
                highLineChartSubMediaOutput.subtitle = new Subtitle() { text = "", x = -20 };
                //highLineChartSubMediaOutput.Colors = new List<string>() { "#15335D", "#448FF2", "#7A045C", "#FFB451", "#E14A02", "#394900", "#005E8F", "#A7B1B3" };

                highLineChartSubMediaOutput.yAxis = new List<YAxis>() { new YAxis() { title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSubMediaOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories,
                    labels = new labels() { staggerLines = 2 }
                };
                highLineChartSubMediaOutput.tooltip = new Tooltip() { valueSuffix = "" };
                highLineChartSubMediaOutput.legend = new Legend() { borderWidth = "0" };
                highLineChartSubMediaOutput.hChart = new HChart() { height = 300, width = chartWidth, type = "spline" };

                // set plot options and click event for series points (which will again assigned in JS as this is string value)
                highLineChartSubMediaOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = true
                        }
                    },
                    series = new PlotSeries()
                    {
                        cursor = "pointer",
                        point = new PlotPoint()
                        {
                            events = new PlotEvents()
                            {
                                click = "LineChartClick"
                            }
                        }
                    }
                };


                // this is signle line spark chart , different for each medium type (with all commom properties set here)
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                List<PlotLine> plotlinesSingleMedia = new List<PlotLine>();

                // set min = 0 , to force chart to start from 0 , and show line in bottom, 
                // gridLineWidth = 0 , to hide grid lines on y axis. 
                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                // not to show x axis labels for spark charts 
                // we have set default value for TickWidth to 0 in XAxis class defination, to not to show line below x-axis for ticks.
                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = categories,
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // add event on chart click , to load medium type summary of selected medium chart. 
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // add event on chart points click , to load medium type summary of selected medium chart. 
                // also disble marker on chart
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        point = new PlotPoint()
                        {
                            events = new PlotEvents()
                            {
                                click = "ChangeMediumTypeOnPointClick"
                            }
                        },
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };

                Int64 totNumOfHits = listOfSummaryReportData.Where(smr => smr.DefaultMediaType
                                        && CheckSubMediaTypeAccess(p_MediaTypeList, smr.SubMediaType)).Sum(s => s.Number_Docs);

                lstSummaryReportMulti.TotalNumOfHits = totNumOfHits.ToString("N0");

                // start to set series of data for  multi line medium chart
                List<Series> lstSeriesSubMediaType = new List<Series>();

                string searchRequestIds = string.Empty;
                string searchRequestNames = string.Empty;

                if (p_SearchRequests != null && p_SearchRequests.Count > 0)
                {
                    searchRequestIds = Newtonsoft.Json.JsonConvert.SerializeObject(listOfSummaryReportData.Select(a => a.SearchRequestID).Distinct());
                    searchRequestNames = Newtonsoft.Json.JsonConvert.SerializeObject(listOfSummaryReportData.Select(a => a.Query_Name).Distinct());
                }

                lstSummaryReportMulti.SummaryReportMedium = new List<SummaryReportMedium>();

                foreach (var media in p_MediaTypeList.Where(m => m.TypeLevel == 1 && m.HasAccess == true && p_MediaTypeList.Where(sm=>string.Compare(m.MediaType, sm.MediaType, true) == 0 && sm.TypeLevel == 2 && sm.HasAccess).Count() > 0))
                {

                    // set sereies name of multiline medium chart as media display name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = media.DisplayName;

                    // set sereies name of signle line spark medium chart  as medium description, will shown in legent and tooltip.
                    Series seriesSingleMedia = new Series();
                    seriesSingleMedia.data = new List<HighChartDatum>();
                    seriesSingleMedia.name = "";

                    // loop for each date to create list of data for selected medium type
                    foreach (var item in dateRange)
                    {
                        var daywiseSum = listOfSummaryReportData.Where(smr => String.Compare(smr.MediaType, media.MediaType.ToString(), true) == 0 && smr.GMT_DateTime.Equals(item) && CheckSubMediaTypeAccess(p_MediaTypeList,smr.SubMediaType)).Sum(s => s.Number_Docs);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current medium type at perticular date 
                            *  SearchTerm = medium description  , used in chart drill down click event
                            *  Value = medium tpye  , used in chart drill down click event
                            *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                        highChartDatum.SearchTerm = media.DisplayName;
                        highChartDatum.Value = media.MediaType;
                        highChartDatum.Type = "SubMedia";
                        series.data.Add(highChartDatum);

                        seriesSingleMedia.data.Add(highChartDatum);
                    }

                    lstSeriesSubMediaType.Add(series);

                    // set signle series for spark chart of current medium type, and assign list of data for that series.
                    List<Series> lstSeriesSingleMediaType = new List<Series>();
                    lstSeriesSingleMediaType.Add(seriesSingleMedia);
                    highLineChartSingleMediaChartOutput.series = lstSeriesSingleMediaType;


                    // set json chart for spark chart based on medium type                    

                    lstSummaryReportMulti.SummaryReportMedium.Add(new SummaryReportMedium()
                    {
                        MediaTypeModel = media,
                        PrevRecordsSum = (p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(a => a.SubMediaType != null && string.Compare(a.MediaType, media.MediaType, true) == 0 && CheckSubMediaTypeAccess(p_MediaTypeList, a.SubMediaType)).Sum(s => s.NoOfDocs) : 0),
                        Records = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput),
                        RecordsSum = (series.data.Sum(s => Convert.ToInt64(s.y)))

                    });
                }

                // Create series for third party data
                if (p_IsThirdPartyAccess)
                {
                    BuildThirdPartySeries(listOfSummaryReportData, p_SearchRequests, p_ThirdPartyDataTypes, dateRange, dictSeriesColors, highLineChartOutput.series, highLineChartOutput.yAxis, lstSeriesSubMediaType, highLineChartSubMediaOutput.yAxis, false);
                }

                // assign set of series data to multi line medium type chart
                highLineChartSubMediaOutput.series = lstSeriesSubMediaType;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSubMediaOutput);
                lstSummaryReportMulti.SubMediaRecords = jsonResult;

                jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
                lstSummaryReportMulti.MediaRecords = jsonResult;

                // create spark chart for audience and media value
                if (p_MediaTypeList.Where(m => m.TypeLevel == 2 && m.HasAccess == true && (m.UseAudience == true || m.UseMediaValue == true)).Count() > 0)
                {
                    //Single Media Chart 
                    HighLineChartOutput highLineChartAudienceMediaValue = new HighLineChartOutput();
                    highLineChartAudienceMediaValue.title = new Title() { text = "", x = -20 };
                    highLineChartAudienceMediaValue.subtitle = new Subtitle() { text = "", x = -20 };
                    //highLineChartAudienceMediaValue.Colors = new List<string>();

                    highLineChartAudienceMediaValue.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                    highLineChartAudienceMediaValue.xAxis = new XAxis()
                    {
                        tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                        tickmarkPlacement = "off",
                        categories = categories,
                        labels = new labels()
                        {
                            enabled = false
                        }
                    };


                    highLineChartAudienceMediaValue.hChart = new HChart() { height = 100, width = 120, type = "spline" };
                    highLineChartAudienceMediaValue.plotOption = new PlotOptions()
                    {
                        spline = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = false,
                                lineWidth = 0
                            }
                        }
                    };
                    highLineChartAudienceMediaValue.tooltip = new Tooltip() { valueSuffix = "" };
                    highLineChartAudienceMediaValue.legend = new Legend() { enabled = false };

                    if (p_MediaTypeList.Where(m => m.TypeLevel == 2 && m.HasAccess == true && m.UseMediaValue == true).Count() > 0)
                    {
                        List<Series> lstSeriesMediaValue = new List<Series>();

                        Series series = new Series();
                        series.data = new List<HighChartDatum>();
                        series.name = "Media Value";

                        foreach (var item in dateRange)
                        {

                            var singleIQMediaValueRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Equals(item)
                                && smr.DefaultMediaType
                                && (
                                        p_MediaTypeList.Where(m => CheckSubMediaTypeAccess(m, smr.SubMediaType) 
                                            && CommonFunctions.CheckNielsenCompeteAccess(m.UseMediaValue, m.RequireNielsenAccess, p_NielsenAccess, m.RequireCompeteAccess, p_CompeteDataAccess)).Count() > 0

                                   )
                              ).Sum(s => s.IQMediaValue);// SingleOrDefault();

                            HighChartDatum highChartDatum = new HighChartDatum();
                            highChartDatum.y = Convert.ToDecimal(singleIQMediaValueRec != null ? singleIQMediaValueRec : 0);
                            series.data.Add(highChartDatum);
                        }

                        lstSeriesMediaValue.Add(series);

                        highLineChartAudienceMediaValue.series = lstSeriesMediaValue;

                        lstSummaryReportMulti.IQMediaValueRecords = CommonFunctions.SearializeJson(highLineChartAudienceMediaValue);


                        //lstSummaryReportMulti.IQMediaValueRecords = temp;
                        lstSummaryReportMulti.IQMediaValueRecordsSum = (series.data.Sum(s => Convert.ToDecimal(s.y)));
                        lstSummaryReportMulti.IQMediaValuePrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(smr =>
                                            p_MediaTypeList.Where(m => CheckSubMediaTypeAccess(m, smr.SubMediaType) 
                                            && CommonFunctions.CheckNielsenCompeteAccess(m.UseMediaValue, m.RequireNielsenAccess, p_NielsenAccess, m.RequireCompeteAccess, p_CompeteDataAccess)).Count() > 0
                                          ).Sum(a => a.IQMediaValue) : 0;
                    }



                    if (p_MediaTypeList.Where(m => m.TypeLevel == 2 && m.HasAccess == true && m.UseAudience == true).Count() > 0)
                    {
                        List<Series> lstSeriesAudience = new List<Series>();
                        Series seriesAudience = new Series();
                        seriesAudience.name = "Audience";
                        seriesAudience.data = new List<HighChartDatum>();

                        foreach (var item in dateRange)
                        {

                            var singleAudienceRec = listOfSummaryReportData.Where(smr => smr.GMT_DateTime.Equals(item)
                                && smr.DefaultMediaType
                                && (
                                            p_MediaTypeList.Where(m => CheckSubMediaTypeAccess(m, smr.SubMediaType) 
                                            && CommonFunctions.CheckNielsenCompeteAccess(m.UseAudience, m.RequireNielsenAccess, p_NielsenAccess, m.RequireCompeteAccess, p_CompeteDataAccess)).Count() > 0
                                    )
                            ).Sum(s => s.Audience);// SingleOrDefault();

                            HighChartDatum highChartDatum = new HighChartDatum();
                            highChartDatum.y = singleAudienceRec;
                            seriesAudience.data.Add(highChartDatum);

                        }

                        lstSeriesAudience.Add(seriesAudience);

                        highLineChartAudienceMediaValue.series = lstSeriesAudience;

                        lstSummaryReportMulti.AudienceRecords = CommonFunctions.SearializeJson(highLineChartAudienceMediaValue);

                        lstSummaryReportMulti.AudienceRecordsSum = (seriesAudience.data.Sum(s => Convert.ToInt64(s.y)));
                        lstSummaryReportMulti.AudiencePrevRecordsSum = p_ListOfIQAgent_ComparisionValues != null ? p_ListOfIQAgent_ComparisionValues.Where(smr =>

                            p_MediaTypeList.Where(m => CheckSubMediaTypeAccess(m, smr.SubMediaType) 
                                            && CommonFunctions.CheckNielsenCompeteAccess(m.UseAudience, m.RequireNielsenAccess, p_NielsenAccess, m.RequireCompeteAccess, p_CompeteDataAccess)).Count() > 0

                                            ).Sum(a => a.Audience) : 0;
                    }
                }

                return lstSummaryReportMulti;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        private bool CheckMainMediaTypeAccess(List<IQ_MediaTypeModel> p_MediaTypeList, string p_RecordMediaType)
        {
            return (p_MediaTypeList.Where(m => string.Compare(p_RecordMediaType, m.MediaType, true) == 0 && m.TypeLevel == 1).Single().HasAccess);
        }

        private bool CheckSubMediaTypeAccess(List<IQ_MediaTypeModel> p_MediaTypeList, string p_RecordSubMediaType)
        {
            return (p_MediaTypeList.Where(m => string.Compare(p_RecordSubMediaType, m.SubMediaType, true) == 0 && m.TypeLevel == 2).Single().HasAccess);
        }

        private bool CheckSubMediaTypeAccess(IQ_MediaTypeModel p_MediaType, string p_RecordSubMediaType)
        {
            return (string.Compare(p_RecordSubMediaType, p_MediaType.SubMediaType, true) == 0 && p_MediaType.TypeLevel == 2 && p_MediaType.HasAccess);
        }

        #endregion

        private void BuildThirdPartySeries(List<SummaryReportModel> p_SummaryData, Dictionary<long, string> p_SearchRequests, List<ThirdPartyDataTypeModel> p_ThirdPartyDataTypes, List<DateTime> p_DateRange, Dictionary<long, string> p_AgentSeriesColors,
                                            List<Series> p_AgentSeries, List<YAxis> p_AgentYAxes, List<Series> p_SubMediaTypeSeries, List<YAxis> p_SubMediaTypeYAxes, bool isMonthData)
        {
            bool isAgentChart = p_SearchRequests != null && p_SearchRequests.Count > 0;
            List<SummaryReportModel> lstThirdPartySummaries = p_SummaryData.Where(w => w.ThirdPartyDataTypeID.HasValue).ToList();
            int numNonAgentSeries = 0;
            List<string> lstColors = new HighLineChartOutput().colors;

            if (lstThirdPartySummaries.Count > 0)
            {
                // Create the necessary y-axes for the selected series
                List<Tuple<int, string>> lstYAxes = p_ThirdPartyDataTypes.Where(w => lstThirdPartySummaries.Select(s => s.ThirdPartyDataTypeID.Value).Contains(w.ID))
                                                                    .Select(s => new Tuple<int, string>(s.YAxisID, s.YAxisName)).Distinct().ToList();
                Dictionary<int, int> dictYAxes = new Dictionary<int, int>();
                Dictionary<int, int> dictAgentYAxes = new Dictionary<int, int>();
                foreach (Tuple<int, string> yAxis in lstYAxes)
                {
                    p_SubMediaTypeYAxes.Add(new YAxis() { title = new Title2() { text = yAxis.Item2, rotation = 90 }, opposite = true });
                    dictYAxes.Add(yAxis.Item1, p_SubMediaTypeYAxes.Count - 1);

                    if (isAgentChart)
                    {
                        p_AgentYAxes.Add(new YAxis() { title = new Title2() { text = yAxis.Item2, rotation = 90 }, opposite = true });
                        dictAgentYAxes.Add(yAxis.Item1, p_AgentYAxes.Count - 1);
                    }
                }

                foreach (int dataTypeID in lstThirdPartySummaries.Select(s => s.ThirdPartyDataTypeID.Value).Distinct())
                {
                    ThirdPartyDataTypeModel dataTypeModel = p_ThirdPartyDataTypes.First(w => w.ID == dataTypeID);

                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = dataTypeModel.DisplayName;
                    series.yAxis = dictYAxes[dataTypeModel.YAxisID];
                    series.dashStyle = dataTypeModel.SeriesLineType;

                    foreach (var item in p_DateRange)
                    {
                        var daywiseSum = lstThirdPartySummaries.Where(smr => smr.ThirdPartyDataTypeID == dataTypeID && ((!isMonthData && smr.GMT_DateTime.Equals(item)) || (isMonthData && smr.GMT_DateTime.Month.Equals(item.Month) && smr.GMT_DateTime.Year.Equals(item.Year)))).Sum(s => s.Number_Docs);

                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum;
                        series.data.Add(highChartDatum);
                    }

                    p_SubMediaTypeSeries.Add(series);

                    // Create series for Agent chart
                    if (isAgentChart)
                    {
                        if (!dataTypeModel.IsAgentSpecific)
                        {
                            // If the data type isn't agent specific, use the same series that were created for the Medium chart
                            Series nonAgentSeries = new Series();
                            nonAgentSeries.data = series.data;
                            nonAgentSeries.name = series.name;
                            nonAgentSeries.yAxis = series.yAxis;
                            nonAgentSeries.dashStyle = series.dashStyle;
                            nonAgentSeries.color = lstColors[(p_AgentSeriesColors.Count + numNonAgentSeries) % lstColors.Count]; // Use the next color in the list

                            p_AgentSeries.Add(nonAgentSeries);
                            numNonAgentSeries++;
                        }
                        else
                        {
                            // If the data type is agent specific, create a series for each selected agent
                            foreach (KeyValuePair<long, string> searchRequest in p_SearchRequests)
                            {
                                Series agentSeries = new Series();
                                agentSeries.data = new List<HighChartDatum>();
                                agentSeries.name = searchRequest.Value + " (" + dataTypeModel.DisplayName + ")";
                                agentSeries.yAxis = dictAgentYAxes[dataTypeModel.YAxisID];
                                agentSeries.dashStyle = dataTypeModel.SeriesLineType;
                                agentSeries.color = p_AgentSeriesColors[searchRequest.Key]; // Match the series color to the color of the associated agent series

                                foreach (var item in p_DateRange)
                                {
                                    HighChartDatum highChartDatum = new HighChartDatum();
                                    highChartDatum.y = lstThirdPartySummaries.Where(smr => smr.SearchRequestID == searchRequest.Key && smr.ThirdPartyDataTypeID == dataTypeID && ((!isMonthData && smr.GMT_DateTime.Equals(item)) || (isMonthData && smr.GMT_DateTime.Month.Equals(item.Month) && smr.GMT_DateTime.Year.Equals(item.Year)))).Sum(s => s.Number_Docs);
                                    agentSeries.data.Add(highChartDatum);
                                }

                                p_AgentSeries.Add(agentSeries);
                            }
                        }
                    }
                }
            }
        }

        #endregion


        #region Media

        public List<IQAgent_DaySummaryModel> GetDaySummaryMediumWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium)
        {
            DashboardDA dashboardDA = new DashboardDA();

            List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = dashboardDA.GetDaySummaryMediumWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium);

            return lstIQAgent_DaySummaryModel;
        }

        #region Default Chart
        /*
        public string GetLineChartForDocs(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, List<string> p_SearchRequestIDs, out Int64 p_TotalAirSeconds)
        {

            try
            {
                var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                }


                p_TotalAirSeconds = ((p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfDocs) : 0) * 8;

                Chart chart = GetChartObject();
                chart.showLabels = "1";
                //var distinctDate = p_lstIQAgent_DaySummaryModel.Select(s => s.DayDate.ToShortDateString()).Distinct().ToList();

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();


                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();

                seriesData.seriesname = p_Medium;
                seriesData.color = "";
                //seriesData.anchorBorderColor = "";
                //seriesData.anchorBgColor = "";

                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    foreach (DateTime rDate in distinctDate)
                    {
                        Category2 category2 = new Category2();
                        category2.label = rDate.ToShortDateString();
                        allCategory.category.Add(category2);
                    }
                    lstallCategory.Add(allCategory);

                    SparkChart sparkChartMediaWise = new SparkChart();
                    SparkChartOutput sparkChartOutputMediaWise = new SparkChartOutput();
                    foreach (var searchRequest in p_SearchRequestIDs)
                    {
                        sparkChartOutputMediaWise.chart = sparkChartMediaWise;
                        List<SparkSeriesData> lstSparkSeriesDataMediaWise = new List<SparkSeriesData>();
                        SparkSeriesData sparkSeriesMediaWise = new SparkSeriesData();
                        sparkSeriesMediaWise.data = new List<SparkDatum>();
                        //sparkChartMediaWise.caption = CommonFunctions.GetEnumDescription(subMedia);

                        seriesData = new SeriesData();
                        seriesData.data = new List<Datum>();
                        var SearchRequest = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.SearchRequestID.ToString(), searchRequest) == 0).FirstOrDefault();
                        if (SearchRequest != null)
                        {
                            seriesData.seriesname = SearchRequest.Query_Name;
                            seriesData.color = "";
                            //multiSeriesData.anchorBorderColor = "";
                            //multiSeriesData.anchorBgColor = "";

                            foreach (DateTime rDate in distinctDate)
                            {
                                var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.SearchRequestID.ToString(), searchRequest, true) == 0 && smr.DayDate.Equals(rDate)).Sum(s => s.NoOfDocs);

                                Datum datum = new Datum();
                                datum.value = Convert.ToString(daywiseSum != null ? daywiseSum : 0);
                                datum.link = "javascript:OpenFeed('" + rDate.ToShortDateString() + "','" + CommonFunctions.GetValueFromDescription<IQMedia.Shared.Utility.CommonFunctions.CategoryType>(p_Medium) + "','" + p_Medium + "','[" + searchRequest + "]','[\"" + System.Web.HttpUtility.UrlEncode(SearchRequest.Query_Name.Replace("\"", "\\\"")) + "\"]')";
                                seriesData.data.Add(datum);


                                SparkDatum sparkDatum = new SparkDatum();
                                sparkDatum.value = Convert.ToString(daywiseSum != null ? daywiseSum : 0);
                                sparkSeriesMediaWise.data.Add(sparkDatum);
                            }



                            lstSparkSeriesDataMediaWise.Add(sparkSeriesMediaWise);
                            sparkChartOutputMediaWise.dataset = lstSparkSeriesDataMediaWise;
                            //Multi Line Charts
                            lstSeriesData.Add(seriesData);
                        }
                    }
                }
                else
                {
                    var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                    {
                        recordDate = s.Key,
                        recordCount = s.Sum(sm => sm.NoOfDocs)
                    });



                    foreach (DateTime rDate in distinctDate)
                    {

                        Category2 category2 = new Category2();
                        category2.label = rDate.ToShortDateString();
                        allCategory.category.Add(category2);

                        var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Equals(rDate)).Select(s => s.recordCount).FirstOrDefault();

                        Datum datum = new Datum();
                        datum.value = daywiseCount.ToString();
                        datum.link = "javascript:OpenFeed('" + rDate.ToShortDateString() + "','" + CommonFunctions.GetValueFromDescription<CommonFunctions.CategoryType>(p_Medium) + "','" + p_Medium + "','','')";


                        seriesData.data.Add(datum);

                    }

                    lstallCategory.Add(allCategory);


                    lstSeriesData.Add(seriesData);
                }


                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }*/

        public string GetLineChartForHits(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Int64 p_HitsCount)
        {

            try
            {
                p_HitsCount = 0;
                SparkChart sparkChart = new SparkChart();
                sparkChart.caption = "";

                SparkChartOutput sparkChartOutput = new SparkChartOutput();
                sparkChartOutput.chart = sparkChart;
                List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                SparkSeriesData sparkSeries = new SparkSeriesData();
                sparkSeries.data = new List<SparkDatum>();

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);

                }

                /*var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => sm.NoOfHits)
                });*/

                p_HitsCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfHits) : 0;

                foreach (var rDate in distinctDate)
                {

                    var daywiseCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.NoOfHits);// Select(s => s.recordCount).FirstOrDefault();

                    SparkDatum datum = new SparkDatum();
                    datum.value = Convert.ToString(daywiseCount != null ? daywiseCount : 0);
                    sparkSeries.data.Add(datum);
                }
                lstSparkSeriesData.Add(sparkSeries);
                sparkChartOutput.dataset = lstSparkSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(sparkChartOutput);
                return jsonResult;

                #region Commented
                /*Chart chart = GetChartObject();
                chart.showlegend = "0";
                chart.caption = "Number of Mention";
                chart.showYAxisValues = "0";
                chart.showvalues = "0";

                var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                }

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();


                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();

                seriesData.seriesname = "Number of Mention";
                seriesData.color = "";
                seriesData.anchorbordercolor = "";
                seriesData.anchorbgcolor = "";

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => sm.NoOfHits)
                });
                p_HitsCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfHits) : 0;
                foreach (DateTime rDate in distinctDate)
                {

                    Category2 category2 = new Category2();
                    category2.label = rDate.ToShortDateString();
                    allCategory.category.Add(category2);

                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Equals(rDate)).Select(s => s.recordCount).FirstOrDefault();

                    Datum datum = new Datum();
                    datum.value = daywiseCount.ToString();
                    seriesData.data.Add(datum);

                }

                lstallCategory.Add(allCategory);


                lstSeriesData.Add(seriesData);
                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;*/

                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetLineChartForViews(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, out Int64 p_ViewsCount)
        {

            try
            {
                p_ViewsCount = 0;

                SparkChart sparkChart = new SparkChart();
                /*if (p_Medium == CommonFunctions.CategoryType.TW.ToString())
                {
                    sparkChart.caption = "Reach";
                }
                else
                {
                    sparkChart.caption = "Views";
                }*/

                SparkChartOutput sparkChartOutput = new SparkChartOutput();
                sparkChartOutput.chart = sparkChart;
                List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                SparkSeriesData sparkSeries = new SparkSeriesData();
                sparkSeries.data = new List<SparkDatum>();

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                }

                /*var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => sm.Audience)
                });*/

                p_ViewsCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.Audience) : 0;

                foreach (var rDate in distinctDate)
                {

                    var daywiseCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.Audience);

                    SparkDatum datum = new SparkDatum();
                    datum.value = Convert.ToString(daywiseCount != null ? daywiseCount : 0);
                    sparkSeries.data.Add(datum);
                }
                lstSparkSeriesData.Add(sparkSeries);
                sparkChartOutput.dataset = lstSparkSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(sparkChartOutput);
                return jsonResult;

                #region Commented
                /*Chart chart = GetChartObject();
                chart.showlegend = "0";
                if (p_Medium == CommonFunctions.CategoryType.TW.ToString())
                {
                    chart.caption = "Reach";
                }
                else
                {
                    chart.caption = "Views";
                }
                chart.showYAxisValues = "0";
                chart.showvalues = "0";

                var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                }

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();


                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();

                seriesData.seriesname = "Views";
                seriesData.color = "";
                seriesData.anchorbordercolor = "";
                seriesData.anchorbgcolor = "";

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => sm.Audience)
                });
                p_ViewsCount = (dayWiseTotalRecord != null && dayWiseTotalRecord.ToList().Count > 0) ? dayWiseTotalRecord.Sum(s => s.recordCount) : 0;

                foreach (DateTime rDate in distinctDate)
                {

                    Category2 category2 = new Category2();
                    category2.label = rDate.ToShortDateString();
                    allCategory.category.Add(category2);

                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Equals(rDate)).Select(s => s.recordCount).FirstOrDefault();

                    Datum datum = new Datum();
                    datum.value = daywiseCount.ToString();
                    seriesData.data.Add(datum);

                }

                lstallCategory.Add(allCategory);


                lstSeriesData.Add(seriesData);
                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;*/

                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetLineChartForMinutesOfAiring(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Decimal p_MinsOfAiringCount)
        {

            try
            {
                p_MinsOfAiringCount = 0;

                SparkChart sparkChart = new SparkChart();
                //sparkChart.caption = "Minutes of Airing";

                SparkChartOutput sparkChartOutput = new SparkChartOutput();
                sparkChartOutput.chart = sparkChart;
                List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                SparkSeriesData sparkSeries = new SparkSeriesData();
                sparkSeries.data = new List<SparkDatum>();

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                }

                /*var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = Math.Round(Convert.ToDouble(s.Sum(sm => (sm.NoOfDocs)) * 8) / 60, 2)
                });*/

                p_MinsOfAiringCount = Convert.ToDecimal((p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfDocs) : 0);
                p_MinsOfAiringCount = Math.Round((p_MinsOfAiringCount * 8) / 60, 2);
                foreach (var rDate in distinctDate)
                {

                    var daywiseCount = Convert.ToDouble(p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.NoOfDocs));
                    daywiseCount = Math.Round(Convert.ToDouble(daywiseCount * 8) / 60, 2);
                    SparkDatum datum = new SparkDatum();
                    datum.value = Convert.ToString(daywiseCount != null ? daywiseCount : 0);
                    sparkSeries.data.Add(datum);
                }
                lstSparkSeriesData.Add(sparkSeries);
                sparkChartOutput.dataset = lstSparkSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(sparkChartOutput);
                return jsonResult;


                #region Commented
                /*Chart chart = GetChartObject();
                chart.showlegend = "0";
                chart.caption = "Minutes of Airing";
                chart.showYAxisValues = "0";
                chart.showvalues = "0";

                var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                }
                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();


                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();

                seriesData.seriesname = "Minutes Of Airing";
                seriesData.color = "";
                seriesData.anchorbordercolor = "";
                seriesData.anchorbgcolor = "";

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = Math.Round(Convert.ToDouble(s.Sum(sm => (sm.NoOfDocs)) * 8) / 60, 2)
                });
                p_MinsOfAiringCount = Convert.ToDecimal((dayWiseTotalRecord != null && dayWiseTotalRecord.ToList().Count > 0) ? dayWiseTotalRecord.Sum(s => s.recordCount) : 0);

                foreach (DateTime rDate in distinctDate)
                {

                    Category2 category2 = new Category2();
                    category2.label = rDate.ToShortDateString();
                    allCategory.category.Add(category2);

                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Equals(rDate)).Select(s => s.recordCount).FirstOrDefault();

                    Datum datum = new Datum();
                    datum.value = daywiseCount.ToString();
                    seriesData.data.Add(datum);

                }

                lstallCategory.Add(allCategory);


                lstSeriesData.Add(seriesData);
                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;*/

                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetLineChartForAd(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Decimal p_AdCount)
        {

            try
            {
                p_AdCount = 0;

                SparkChart sparkChart = new SparkChart();
                //sparkChart.caption = "Ad Equivalency";

                SparkChartOutput sparkChartOutput = new SparkChartOutput();
                sparkChartOutput.chart = sparkChart;
                List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                SparkSeriesData sparkSeries = new SparkSeriesData();
                sparkSeries.data = new List<SparkDatum>();

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                }

                /*var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => Convert.ToDecimal(sm.IQMediaValue))
                });*/

                p_AdCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => Convert.ToDecimal(s.IQMediaValue)) : 0;

                foreach (var rDate in distinctDate)
                {

                    var daywiseCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.IQMediaValue);

                    SparkDatum datum = new SparkDatum();
                    datum.value = Convert.ToString(daywiseCount != null ? daywiseCount : 0);
                    sparkSeries.data.Add(datum);
                }
                lstSparkSeriesData.Add(sparkSeries);
                sparkChartOutput.dataset = lstSparkSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(sparkChartOutput);
                return jsonResult;

                #region Commented
                /*Chart chart = GetChartObject();
                chart.showlegend = "0";
                chart.caption = "Ad Equivalency";
                chart.showYAxisValues = "0";
                chart.showvalues = "0";

                var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                }

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();


                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();

                seriesData.seriesname = "Ad Equivalency";
                seriesData.color = "";
                seriesData.anchorbordercolor = "";
                seriesData.anchorbgcolor = "";

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => Convert.ToDecimal(sm.IQMediaValue))
                });

                p_AdCount = (dayWiseTotalRecord != null && dayWiseTotalRecord.ToList().Count > 0) ? dayWiseTotalRecord.Sum(s => s.recordCount) : 0;

                foreach (DateTime rDate in distinctDate)
                {

                    Category2 category2 = new Category2();
                    category2.label = rDate.ToShortDateString();
                    allCategory.category.Add(category2);

                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Equals(rDate)).Select(s => s.recordCount).FirstOrDefault();

                    Datum datum = new Datum();
                    datum.value = daywiseCount.ToString();
                    seriesData.data.Add(datum);

                }

                lstallCategory.Add(allCategory);


                lstSeriesData.Add(seriesData);
                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;*/

                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetLineChartForSentiment(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate,
                                            out Int64 positiveSentiment,
                                            out Int64 negativeSentiment)
        {

            try
            {
                negativeSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.NegativeSentiment);
                positiveSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.PositiveSentiment);

                Chart chart = new Chart();
                chart.subcaption = "";
                chart.caption = "";
                chart.linethickness = "1";
                chart.showLabels = "0";
                chart.showvalues = "0";
                chart.showYAxisValues = "0";
                chart.formatnumberscale = "0";
                chart.anchorRadius = "0";
                chart.divlinealpha = "FFFFFF";
                chart.divlinecolor = "FFFFFF";
                chart.divlineisdashed = "1";
                chart.showalternatehgridcolor = "1";
                chart.alternatehgridcolor = "FFFFFF";
                chart.shadowalpha = "40";
                chart.labelstep = "1";
                chart.numvdivlines = "5";
                chart.chartrightmargin = "10";
                chart.bgcolor = "FFFFFF";
                chart.bgangle = "270";
                chart.bgalpha = "10,10";
                chart.alternatehgridalpha = "5";
                chart.legendposition = "BOTTOM";
                chart.drawAnchors = "1";
                chart.showBorder = "0";
                chart.canvasBorderAlpha = "0";
                chart.showlegend = "0";

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;
                List<SeriesData> lstSeriesData = new List<SeriesData>();


                var distinctDate = new List<DateTime>();

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                    Category2 category2 = new Category2();

                    category2.label = dt.ToShortDateString();

                    allCategory.category.Add(category2);
                }

                lstallCategory.Add(allCategory);

                SeriesData positiveSeries = new SeriesData();
                positiveSeries.data = new List<Datum>();
                positiveSeries.seriesname = "Positive Sentiment";
                positiveSeries.color = "#c7d36a";


                SeriesData negativeSeries = new SeriesData();
                negativeSeries.data = new List<Datum>();
                negativeSeries.seriesname = "Negative Sentiment";
                negativeSeries.color = "#d8635d";

                foreach (var rDate in distinctDate)
                {
                    var daywisePositiveSentimentCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.PositiveSentiment);
                    var daywiseNegativeSentimentCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.NegativeSentiment);


                    Datum datumPositive = new Datum();
                    datumPositive.value = Convert.ToString(daywisePositiveSentimentCount != null ? daywisePositiveSentimentCount : 0);
                    datumPositive.link = "#";

                    Datum datumNegative = new Datum();
                    datumNegative.value = Convert.ToString(daywiseNegativeSentimentCount != null ? daywiseNegativeSentimentCount : 0);
                    datumNegative.link = "#";

                    positiveSeries.data.Add(datumPositive);
                    negativeSeries.data.Add(datumNegative);
                }

                lstSeriesData.Add(positiveSeries);
                lstSeriesData.Add(negativeSeries);

                lineChartOutput.dataset = lstSeriesData;
                lineChartOutput.categories = lstallCategory;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetFusionUsaDmaMap(Dictionary<string, Int64> p_DmaMentionMapList)
        {
            try
            {
                FusionMapOutput fusionMapOutput = new FusionMapOutput();

                //List<string> colors = new List<string> { "F5F5FF", "E1E1FF", "#828CFF", "6464FF", "3232FF", "0101FF" };
                List<string> colors = new List<string> { "A0D6FC", "83C9FC", "5DBBFE", "3FAEFD", "0395FE" };

                // set all map display properties
                FusionMap fusionMap = new FusionMap();
                fusionMap.animation = "0";
                fusionMap.showbevel = "1";
                fusionMap.usehovercolor = "1";
                fusionMap.canvasbordercolor = "FFFFFF";
                fusionMap.bordercolor = "B7B7B7";
                fusionMap.showlegend = "1";
                fusionMap.showshadow = "0";
                fusionMap.legendposition = "BOTTOM";
                fusionMap.legendborderalpha = 1;
                fusionMap.legendbordercolor = "ffffff";
                fusionMap.legendallowdrag = "0";
                fusionMap.legendshadow = "1";
                //fusionMap.caption = "Website Visits for the month of Jan 2014";
                fusionMap.connectorcolor = "000000";
                fusionMap.fillalpha = "80";
                fusionMap.hovercolor = "CCCCCC";
                fusionMap.showEntityToolTip = "1";
                fusionMap.showToolTip = "0";

                // set legend color ranges 
                FusionMapColorRange fusionMapColorRange = new FusionMapColorRange();
                fusionMapColorRange.color = new List<FusionMapColor>();

                long minValue = 1000000;
                long maxValue = 0;

                // set map data 
                List<FusionMapData> lstFusionMapData = new List<FusionMapData>();
                foreach (KeyValuePair<string, short> keyval in IQDmaToFusionIDMapModel.IQDmaToFusionIDMap)
                {
                    FusionMapData fusionMapData = new FusionMapData();
                    long mention = 0;
                    p_DmaMentionMapList.TryGetValue(keyval.Key, out mention);

                    fusionMapData.id = keyval.Value.ToString();
                    fusionMapData.value = mention.ToString();
                    fusionMapData.tooltext = "DMA Area : " + keyval.Key + "{br}Mention:" + mention.ToString("N0");
                    fusionMapData.showEntityToolTip = "1";
                    if (string.Compare("Honolulu", keyval.Key, true) == 0 || string.Compare("Anchorage", keyval.Key, true) == 0 || string.Compare("Juneau", keyval.Key, true) == 0 || string.Compare("Fairbanks", keyval.Key, true) == 0)
                    {
                        fusionMapData.showlabel = "1";
                    }
                    else
                    {
                        fusionMapData.showlabel = "0";
                    }

                    if (mention > maxValue)
                    {
                        maxValue = mention;
                    }

                    if (mention < minValue)
                    {
                        minValue = mention;
                    }

                    lstFusionMapData.Add(fusionMapData);
                }

                long colorStep = (maxValue - minValue) / 5;
                if (colorStep > 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        FusionMapColor fusionMapColor = new FusionMapColor();
                        if (i == 0)
                        {
                            fusionMapColor.minvalue = (minValue).ToString();
                            fusionMapColor.maxvalue = ((colorStep * (i + 1)) - 1).ToString();
                        }
                        else if (i == 4)
                        {
                            fusionMapColor.minvalue = (colorStep * i).ToString();
                            fusionMapColor.maxvalue = maxValue.ToString();
                        }
                        else
                        {
                            fusionMapColor.minvalue = (colorStep * i).ToString();
                            fusionMapColor.maxvalue = ((colorStep * (i + 1)) - 1).ToString();
                        }

                        fusionMapColor.code = colors[i];
                        fusionMapColor.displayvalue = fusionMapColor.maxvalue;
                        fusionMapColorRange.color.Add(fusionMapColor);
                    }
                }
                else
                {
                    if (maxValue == 0)
                    {
                        maxValue = 1;
                    }

                    if (minValue == maxValue)
                    {
                        minValue = minValue - 1;
                    }

                    FusionMapColor fusionMapColor = new FusionMapColor();
                    fusionMapColor.minvalue = minValue.ToString();
                    fusionMapColor.maxvalue = maxValue.ToString();
                    fusionMapColor.code = "C3EBFD";
                    fusionMapColor.displayvalue = fusionMapColor.maxvalue;
                    fusionMapColorRange.color.Add(fusionMapColor);
                }

                fusionMapOutput.map = fusionMap;
                fusionMapOutput.colorrange = fusionMapColorRange;
                fusionMapOutput.data = lstFusionMapData;

                string result = CommonFunctions.SearializeJson(fusionMapOutput);
                return result;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetFusionCanadaProvinceMap(Dictionary<string, Int64> p_ProvinceMentionMapList)
        {
            try
            {
                FusionMapOutput fusionMapOutput = new FusionMapOutput();

                List<string> colors = new List<string> { "A0D6FC", "83C9FC", "5DBBFE", "3FAEFD", "0395FE" };

                // set all map display properties
                FusionMap fusionMap = new FusionMap();
                fusionMap.animation = "0";
                fusionMap.showbevel = "1";
                fusionMap.usehovercolor = "1";
                fusionMap.canvasbordercolor = "FFFFFF";
                fusionMap.bordercolor = "B7B7B7";
                fusionMap.showlegend = "1";
                fusionMap.showshadow = "0";
                fusionMap.legendposition = "BOTTOM";
                fusionMap.legendborderalpha = 1;
                fusionMap.legendbordercolor = "ffffff";
                fusionMap.legendallowdrag = "0";
                fusionMap.legendshadow = "1";
                fusionMap.connectorcolor = "000000";
                fusionMap.fillalpha = "80";
                fusionMap.hovercolor = "CCCCCC";
                fusionMap.showEntityToolTip = "1";
                fusionMap.showToolTip = "0";

                // set legend color ranges 
                FusionMapColorRange fusionMapColorRange = new FusionMapColorRange();
                fusionMapColorRange.color = new List<FusionMapColor>();

                long minValue = 1000000;
                long maxValue = 0;

                // set map data 
                List<FusionMapData> lstFusionMapData = new List<FusionMapData>();
                foreach (KeyValuePair<string, string> keyval in IQProvinceToFusionIDMapModel.IQProvinceToFusionIDMap)
                {
                    FusionMapData fusionMapData = new FusionMapData();
                    long mention = 0;
                    p_ProvinceMentionMapList.TryGetValue(keyval.Key, out mention);

                    fusionMapData.id = keyval.Value.ToString();
                    fusionMapData.value = mention.ToString();
                    fusionMapData.tooltext = "Province : " + keyval.Key + "{br}Mention:" + mention.ToString("N0");
                    fusionMapData.showEntityToolTip = "1";
                    fusionMapData.showlabel = "0";

                    if (mention > maxValue)
                    {
                        maxValue = mention;
                    }

                    if (mention < minValue)
                    {
                        minValue = mention;
                    }

                    lstFusionMapData.Add(fusionMapData);
                }

                long colorStep = (maxValue - minValue) / 5;
                if (colorStep > 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        FusionMapColor fusionMapColor = new FusionMapColor();
                        if (i == 0)
                        {
                            fusionMapColor.minvalue = (minValue).ToString();
                            fusionMapColor.maxvalue = ((colorStep * (i + 1)) - 1).ToString();
                        }
                        else if (i == 4)
                        {
                            fusionMapColor.minvalue = (colorStep * i).ToString();
                            fusionMapColor.maxvalue = maxValue.ToString();
                        }
                        else
                        {
                            fusionMapColor.minvalue = (colorStep * i).ToString();
                            fusionMapColor.maxvalue = ((colorStep * (i + 1)) - 1).ToString();
                        }

                        fusionMapColor.code = colors[i];
                        fusionMapColor.displayvalue = fusionMapColor.maxvalue;
                        fusionMapColorRange.color.Add(fusionMapColor);
                    }
                }
                else
                {
                    if (maxValue == 0)
                    {
                        maxValue = 1;
                    }

                    if (minValue == maxValue)
                    {
                        minValue = minValue - 1;
                    }

                    FusionMapColor fusionMapColor = new FusionMapColor();
                    fusionMapColor.minvalue = minValue.ToString();
                    fusionMapColor.maxvalue = maxValue.ToString();
                    fusionMapColor.code = "C3EBFD";
                    fusionMapColor.displayvalue = fusionMapColor.maxvalue;
                    fusionMapColorRange.color.Add(fusionMapColor);
                }

                fusionMapOutput.map = fusionMap;
                fusionMapOutput.colorrange = fusionMapColorRange;
                fusionMapOutput.data = lstFusionMapData;

                string result = CommonFunctions.SearializeJson(fusionMapOutput);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetHighChartForDocs(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, IQ_MediaTypeModel p_MediaType, List<string> p_SearchRequestIDs, out Int64 p_TotalAirSeconds)
        {

            try
            {
                var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                }


                List<string> categories = new List<string>();
                foreach (DateTime rDate in distinctDate)
                {
                    categories.Add(rDate.ToShortDateString());
                }

                p_TotalAirSeconds = ((p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfDocs) : 0) * 8;

                // this signle line medium chart, for selected medium type 
                // if one or more request request applid, then it will show multi line chart, one for each request request. 
                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
                highLineChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories,
                    labels = new labels()
                };

                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { borderWidth = "0", width = 750 };

                // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
                highLineChartOutput.hChart = new HChart() { height = 300, type = "spline" };

                // start to set series of data for medium chart (or multi line search request chart)
                List<Series> lstSeries = new List<Series>();
                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {

                    // set plot options and click event for series points (which will again assigned in JS as this is string value)
                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        spline = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };

                    // set list of data for each series 
                    foreach (var searchRequest in p_SearchRequestIDs)
                    {
                        var SearchRequest = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.SearchRequestID.ToString(), searchRequest) == 0).FirstOrDefault();

                        if (SearchRequest != null)
                        {
                            // set sereies name as search request query name, will shown in legent and tooltip.
                            Series series = new Series();
                            series.data = new List<HighChartDatum>();
                            series.name = SearchRequest.Query_Name;

                            foreach (DateTime rDate in distinctDate)
                            {
                                var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.SearchRequestID.ToString(), searchRequest, true) == 0 && smr.DayDate.Equals(rDate)).Sum(s => s.NoOfDocs);


                                // set data point of current series 
                                /*
                                    *  y = y series value of current point === total no. of records for current search request at perticular date 
                                    *  SearchTerm = query name  , used in chart drill down click event
                                    *  Value = Search Request ID  , used in chart drill down click event
                                    *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                                */
                                HighChartDatum highChartDatum = new HighChartDatum();
                                highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                                highChartDatum.SearchTerm = SearchRequest.Query_Name;
                                highChartDatum.Value = SearchRequest.SearchRequestID.ToString();
                                highChartDatum.Type = "Media";
                                series.data.Add(highChartDatum);
                            }

                            lstSeries.Add(series);
                        }

                    }
                }
                else
                {
                    // as its single media chart, we will show it as area chart, by setting chart type to "area"
                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        spline = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };

                    var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                    {
                        recordDate = s.Key,
                        recordCount = s.Sum(sm => sm.NoOfDocs)
                    });

                    // set sereies name as "Media" , will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = p_MediaType.DisplayName;

                    foreach (DateTime rDate in distinctDate)
                    {

                        var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Equals(rDate)).Select(s => s.recordCount).FirstOrDefault();

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                            *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                            *  we will not set SearchTerm and Value properies of data, as this is signle medium chart , without any search request
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                        highChartDatum.Type = "SubMedia";
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

                // assign set of series data to medium chart (or multi line searchrequest chart)
                highLineChartOutput.series = lstSeries;
                string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForHits(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Int64 p_HitsCount)
        {

            try
            {
                p_HitsCount = 0;

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);

                }

                p_HitsCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfHits) : 0;

                // this signle line spark chart, for selected medium type for no. of hits 
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                List<PlotLine> plotlinesSingleMedia = new List<PlotLine>();

                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = distinctDate.Select(a => a.ToShortDateString()).ToList(),
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type of chart
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false, 
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };

                // series of data for medium chart
                List<Series> lstSeries = new List<Series>();

                // set series name
                Series series = new Series();
                series.name = "Mentions";
                series.data = new List<HighChartDatum>();

                // set series data
                foreach (var rDate in distinctDate)
                {

                    var daywiseCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.NoOfHits);// Select(s => s.recordCount).FirstOrDefault();

                    HighChartDatum highChartDatum = new Model.HighChartDatum();
                    highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                    series.data.Add(highChartDatum);
                }

                lstSeries.Add(series);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForViews(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Int64 p_ViewsCount)
        {

            try
            {
                p_ViewsCount = 0;

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                }

                p_ViewsCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.Audience) : 0;

                // signle line spark chart, for selected medium type for no. of views 
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
                //highLineChartSingleMediaChartOutput.Colors = new List<string>();

                List<PlotLine> plotlinesSingleMedia = new List<PlotLine>();

                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = distinctDate.Select(a => a.ToShortDateString()).ToList(),
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type 
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false, 
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };


                // series of data for medium chart 
                List<Series> lstSeries = new List<Series>();

                // set series name
                Series series = new Series();
                series.name = "Audience";
                series.data = new List<HighChartDatum>();

                // set series data
                foreach (var rDate in distinctDate)
                {
                    var daywiseCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(sm => sm.Audience);

                    HighChartDatum highChartDatum = new Model.HighChartDatum();
                    highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                    series.data.Add(highChartDatum);
                }

                lstSeries.Add(series);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForMinutesOfAiring(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Decimal p_MinsOfAiringCount)
        {

            try
            {
                p_MinsOfAiringCount = 0;

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                }

                p_MinsOfAiringCount = Convert.ToDecimal((p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfDocs) : 0);
                p_MinsOfAiringCount = Math.Round((p_MinsOfAiringCount * 8) / 60, 2);

                // this signle line spark chart, for selected medium type for no. of air minute
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                List<PlotLine> plotlinesSingleMedia = new List<PlotLine>();

                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = distinctDate.Select(a => a.ToShortDateString()).ToList(),
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type 
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false, 
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };


                // series of data for medium chart 
                List<Series> lstSeries = new List<Series>();

                // set series name
                Series series = new Series();
                series.name = "Air Time";
                series.data = new List<HighChartDatum>();

                // set list of data for series 
                foreach (var rDate in distinctDate)
                {
                    var daywiseCount = Convert.ToDouble(p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.NoOfDocs));
                    daywiseCount = Math.Round(Convert.ToDouble(daywiseCount * 8) / 60, 2);

                    HighChartDatum highChartDatum = new Model.HighChartDatum();
                    highChartDatum.y = Convert.ToDecimal(daywiseCount != null ? daywiseCount : 0);
                    series.data.Add(highChartDatum);
                }

                lstSeries.Add(series);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForAd(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Decimal p_AdCount)
        {

            try
            {
                p_AdCount = 0;

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                }

                p_AdCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => Convert.ToDecimal(s.IQMediaValue)) : 0;


                // this signle line spark chart, for selected medium type for iq media value 
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
                //highLineChartSingleMediaChartOutput.Colors = new List<string>();

                List<PlotLine> plotlinesSingleMedia = new List<PlotLine>();

                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = distinctDate.Select(a => a.ToShortDateString()).ToList(),
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type 
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false, 
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };


                // series of data for medium chart 
                List<Series> lstSeries = new List<Series>();

                // set series name
                Series series = new Series();
                series.name = "Media Value";
                series.data = new List<HighChartDatum>();

                // set list of data for series 
                foreach (var rDate in distinctDate)
                {

                    var daywiseCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.IQMediaValue);

                    HighChartDatum highChartDatum = new Model.HighChartDatum();
                    highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                    series.data.Add(highChartDatum);
                }

                lstSeries.Add(series);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForSentiment(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate,
                                            out Int64 positiveSentiment,
                                            out Int64 negativeSentiment)
        {

            try
            {
                negativeSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.NegativeSentiment);
                positiveSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.PositiveSentiment);


                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    distinctDate.Add(dt);
                }

                // this signle line spark chart, for selected medium type for sentiment
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                List<PlotLine> plotlinesSingleMedia = new List<PlotLine>();

                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = distinctDate.Select(a => a.ToShortDateString()).ToList(),
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type 
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false, 
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };

                // series of data for medium chart 
                List<Series> lstSeries = new List<Series>();

                // set positive series data
                Series positiveSeries = new Series();
                positiveSeries.name = "Positive Sentiment";
                positiveSeries.data = new List<HighChartDatum>();
                positiveSeries.color = "#c7d36a";

                // set negative series data
                Series negativeSeries = new Series();
                negativeSeries.data = new List<HighChartDatum>();
                negativeSeries.name = "Negative Sentiment";
                negativeSeries.color = "#d8635d";

                // set list of data for positive and negative series 
                foreach (var rDate in distinctDate)
                {
                    var daywisePositiveSentimentCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.PositiveSentiment);
                    var daywiseNegativeSentimentCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.NegativeSentiment);


                    HighChartDatum datumPositive = new Model.HighChartDatum();
                    datumPositive.y = daywisePositiveSentimentCount != null ? daywisePositiveSentimentCount : 0;

                    HighChartDatum datumNegative = new HighChartDatum();
                    datumNegative.y = daywiseNegativeSentimentCount != null ? daywiseNegativeSentimentCount : 0;

                    positiveSeries.data.Add(datumPositive);
                    negativeSeries.data.Add(datumNegative);
                }

                lstSeries.Add(positiveSeries);
                lstSeries.Add(negativeSeries);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;


            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public string GetHighChartsForDocsForDma(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, List<DashboardDMAChartSelectionModel> p_Dmas)
        {
            var distinctDate = new List<DateTime>();

            for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
            {
                distinctDate.Add(dt);
            }


            List<string> categories = new List<string>();
            foreach (DateTime rDate in distinctDate)
            {
                categories.Add(rDate.ToShortDateString());
            }

            HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
            highLineChartOutput.title = new Title() { text = "", x = -20 };
            highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
            highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };
            /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
            /* 
                if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
            */
            /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
            highLineChartOutput.xAxis = new XAxis()
            {
                tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                tickmarkPlacement = "off",
                categories = categories,
                labels = new labels()
                {
                    enabled = false
                }
            };

            // set plot options and disable marker
            highLineChartOutput.plotOption = new PlotOptions()
            {
                spline = new PlotSeries()
                {
                    events = new PlotEvents()
                    {
                        mouseOver = "HandleChartMouseHover",
                        mouseOut = "HandleChartMouseOut"
                    },
                    marker = new PlotMarker()
                    {
                        enabled = false,
                        lineWidth = 0
                    }
                }
            };

            // show default tooltip format x / y values
            highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "", shared = true };

            // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
            highLineChartOutput.legend = new Legend() { enabled = false };

            // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
            highLineChartOutput.hChart = new HChart() { height = 100, width = 140, type = "spline" };

            // start to set series of data for medium chart (or multi line search request chart)
            List<Series> lstSeries = new List<Series>();


            // set list of data for each series 
            foreach (var dma in p_Dmas)
            {
                var reqDma = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.Query_Name, dma.id, true) == 0).FirstOrDefault();

                if (reqDma != null)
                {
                    // set sereies name as search request query name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = reqDma.Query_Name;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.Query_Name.ToString(), dma.id, true) == 0 && smr.DayDate.Equals(rDate)).Sum(s => s.NoOfDocs);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }
                else
                {
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = dma.id;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

            }

            // assign set of series data to medium chart (or multi line searchrequest chart)
            highLineChartOutput.series = lstSeries;
            string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
            return jsonResult;
        }

        public string GetHighChartsForHitsForDma(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, List<DashboardDMAChartSelectionModel> p_Dmas)
        {
            var distinctDate = new List<DateTime>();

            for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
            {
                distinctDate.Add(dt);
            }


            List<string> categories = new List<string>();
            foreach (DateTime rDate in distinctDate)
            {
                categories.Add(rDate.ToShortDateString());
            }


            HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
            highLineChartOutput.title = new Title() { text = "", x = -20 };
            highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
            highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };
            /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
            /* 
                if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
            */
            /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
            highLineChartOutput.xAxis = new XAxis()
            {
                tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                tickmarkPlacement = "off",
                categories = categories,
                labels = new labels()
                {
                    enabled = false
                }
            };

            // set plot options and disable marker
            highLineChartOutput.plotOption = new PlotOptions()
            {
                spline = new PlotSeries()
                {
                    events = new PlotEvents()
                    {
                        mouseOver = "HandleChartMouseHover",
                        mouseOut = "HandleChartMouseOut"
                    },
                    marker = new PlotMarker()
                    {
                        enabled = false,
                        lineWidth = 0
                    }
                }
            };

            // show default tooltip format x / y values
            highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "", shared = true };

            // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
            highLineChartOutput.legend = new Legend() { enabled = false };

            // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
            highLineChartOutput.hChart = new HChart() { height = 100, width = 140, type = "spline" };

            // start to set series of data for medium chart (or multi line search request chart)
            List<Series> lstSeries = new List<Series>();


            // set list of data for each series 
            foreach (var dma in p_Dmas)
            {
                var reqDma = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.Query_Name, dma.id, true) == 0).FirstOrDefault();

                if (reqDma != null)
                {
                    // set sereies name as search request query name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = reqDma.Query_Name;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.Query_Name.ToString(), dma.id, true) == 0 && smr.DayDate.Equals(rDate)).Sum(s => s.NoOfHits);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }
                else
                {
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = dma.id;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

            }

            // assign set of series data to medium chart (or multi line searchrequest chart)
            highLineChartOutput.series = lstSeries;
            string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
            return jsonResult;
        }

        public string GetHighChartsForMinutesOfAiringForDma(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, List<DashboardDMAChartSelectionModel> p_Dmas)
        {
            var distinctDate = new List<DateTime>();

            for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
            {
                distinctDate.Add(dt);
            }


            List<string> categories = new List<string>();
            foreach (DateTime rDate in distinctDate)
            {
                categories.Add(rDate.ToShortDateString());
            }


            HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
            highLineChartOutput.title = new Title() { text = "", x = -20 };
            highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
            highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };
            /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
            /* 
                if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
            */
            /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
            highLineChartOutput.xAxis = new XAxis()
            {
                tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                tickmarkPlacement = "off",
                categories = categories,
                labels = new labels()
                {
                    enabled = false
                }
            };

            // set plot options and disable marker
            highLineChartOutput.plotOption = new PlotOptions()
            {
                spline = new PlotSeries()
                {
                    events = new PlotEvents()
                    {
                        mouseOver = "HandleChartMouseHover",
                        mouseOut = "HandleChartMouseOut"
                    },
                    marker = new PlotMarker()
                    {
                        enabled = false,
                        lineWidth = 0
                    }
                }
            };

            // show default tooltip format x / y values
            highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "", shared = true };

            // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
            highLineChartOutput.legend = new Legend() { enabled = false };

            // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
            highLineChartOutput.hChart = new HChart() { height = 100, width = 140, type = "spline" };

            // start to set series of data for medium chart (or multi line search request chart)
            List<Series> lstSeries = new List<Series>();


            // set list of data for each series 
            foreach (var dma in p_Dmas)
            {
                var reqDma = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.Query_Name, dma.id, true) == 0).FirstOrDefault();

                if (reqDma != null)
                {
                    // set sereies name as search request query name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = reqDma.Query_Name;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        var daywiseCount = Convert.ToDecimal(p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.Query_Name.ToString(), dma.id, true) == 0 && smr.DayDate.Equals(rDate)).Sum(s => s.NoOfDocs));
                        daywiseCount = Math.Round(Convert.ToDecimal(daywiseCount * 8) / 60, 2);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }
                else
                {
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = dma.id;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

            }

            // assign set of series data to medium chart (or multi line searchrequest chart)
            highLineChartOutput.series = lstSeries;
            string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
            return jsonResult;
        }

        public string GetHighChartsForViewsForDma(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, List<DashboardDMAChartSelectionModel> p_Dmas)
        {
            var distinctDate = new List<DateTime>();

            for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
            {
                distinctDate.Add(dt);
            }


            List<string> categories = new List<string>();
            foreach (DateTime rDate in distinctDate)
            {
                categories.Add(rDate.ToShortDateString());
            }


            HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
            highLineChartOutput.title = new Title() { text = "", x = -20 };
            highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
            highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };
            /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
            /* 
                if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
            */
            /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
            highLineChartOutput.xAxis = new XAxis()
            {
                tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                tickmarkPlacement = "off",
                categories = categories,
                labels = new labels()
                {
                    enabled = false
                }
            };

            // set plot options and disable marker
            highLineChartOutput.plotOption = new PlotOptions()
            {
                spline = new PlotSeries()
                {
                    events = new PlotEvents()
                    {
                        mouseOver = "HandleChartMouseHover",
                        mouseOut = "HandleChartMouseOut"
                    },
                    marker = new PlotMarker()
                    {
                        enabled = false,
                        lineWidth = 0
                    }
                }
            };

            // show default tooltip format x / y values
            highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "", shared = true };

            // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
            highLineChartOutput.legend = new Legend() { enabled = false };

            // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
            highLineChartOutput.hChart = new HChart() { height = 100, width = 140, type = "spline" };

            // start to set series of data for medium chart (or multi line search request chart)
            List<Series> lstSeries = new List<Series>();


            // set list of data for each series 
            foreach (var dma in p_Dmas)
            {
                var reqDma = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.Query_Name, dma.id, true) == 0).FirstOrDefault();

                if (reqDma != null)
                {
                    // set sereies name as search request query name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = reqDma.Query_Name;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.Query_Name.ToString(), dma.id, true) == 0 && smr.DayDate.Equals(rDate)).Sum(s => s.Audience);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }
                else
                {
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = dma.id;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

            }

            // assign set of series data to medium chart (or multi line searchrequest chart)
            highLineChartOutput.series = lstSeries;
            string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
            return jsonResult;
        }

        #endregion

        #region Hourly Chart
        /*
        public string GetLineChartForDocsHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, decimal p_ClientGmtOffset, decimal p_ClientDstOffset, List<string> p_SearchRequestIDs, out Int64 p_TotalAirSeconds)
        {

            try
            {

                var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }

                p_TotalAirSeconds = ((p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfDocs) : 0) * 8;

                Chart chart = GetChartObject();
                chart.showLabels = "1";
                //var distinctDate = p_lstIQAgent_DaySummaryModel.Select(s => s.DayDate.ToShortDateString()).Distinct().ToList();

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();


                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();

                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    //foreach (DateTime rDate in distinctDate)
                    //{
                    //    Category2 category2 = new Category2();
                    //    category2.label = rDate.ToShortDateString();
                    //    allCategory.category.Add(category2);
                    //}

                    foreach (DateTime rDate in distinctDate)
                    {
                        Category2 category2 = new Category2();

                        if (rDate.IsDaylightSavingTime())
                        {

                            category2.label = rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString();
                        }
                        else
                        {
                            category2.label = rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString();
                        }


                        allCategory.category.Add(category2);
                    }


                    lstallCategory.Add(allCategory);

                    SparkChart sparkChartMediaWise = new SparkChart();
                    SparkChartOutput sparkChartOutputMediaWise = new SparkChartOutput();
                    foreach (var searchRequest in p_SearchRequestIDs)
                    {
                        sparkChartOutputMediaWise.chart = sparkChartMediaWise;
                        List<SparkSeriesData> lstSparkSeriesDataMediaWise = new List<SparkSeriesData>();
                        SparkSeriesData sparkSeriesMediaWise = new SparkSeriesData();
                        sparkSeriesMediaWise.data = new List<SparkDatum>();
                        //sparkChartMediaWise.caption = CommonFunctions.GetEnumDescription(subMedia);

                        seriesData = new SeriesData();
                        seriesData.data = new List<Datum>();
                        var SearchRequest = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.SearchRequestID.ToString(), searchRequest) == 0).FirstOrDefault();
                        if (SearchRequest != null)
                        {
                            seriesData.seriesname = SearchRequest.Query_Name;
                            seriesData.color = "";
                            //multiSeriesData.anchorBorderColor = "";
                            //multiSeriesData.anchorBgColor = "";

                            foreach (DateTime rDate in distinctDate)
                            {

                                var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.SearchRequestID.ToString(), searchRequest, true) == 0 && smr.DayDate.Equals(rDate)).Sum(s => s.NoOfDocs);

                                Datum datum = new Datum();
                                datum.value = Convert.ToString(daywiseSum != null ? daywiseSum : 0);
                                datum.link = "javascript:OpenFeed('" + rDate.ToShortDateString() + "','" + CommonFunctions.GetValueFromDescription<IQMedia.Shared.Utility.CommonFunctions.CategoryType>(p_Medium) + "','" + p_Medium + "','[" + searchRequest + "]','[\"" + System.Web.HttpUtility.UrlEncode(SearchRequest.Query_Name.Replace("\"", "\\\"")) + "\"]')";

                                seriesData.data.Add(datum);

                                SparkDatum sparkDatum = new SparkDatum();
                                sparkDatum.value = Convert.ToString(daywiseSum != null ? daywiseSum : 0);
                                sparkSeriesMediaWise.data.Add(sparkDatum);
                            }



                            lstSparkSeriesDataMediaWise.Add(sparkSeriesMediaWise);
                            sparkChartOutputMediaWise.dataset = lstSparkSeriesDataMediaWise;

                            //Multi Line Charts
                            lstSeriesData.Add(seriesData);
                        }
                    }
                }
                else
                {
                    seriesData.seriesname = p_Medium;
                    seriesData.color = "";
                    //seriesData.anchorBorderColor = "";
                    //seriesData.anchorBgColor = "";

                    var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                    {
                        recordDate = s.Key,
                        recordCount = s.Sum(sm => sm.NoOfDocs)
                    });

                    foreach (DateTime rDate in distinctDate)
                    {
                        Category2 category2 = new Category2();

                        if (rDate.IsDaylightSavingTime())
                        {

                            category2.label = rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString();
                        }
                        else
                        {
                            category2.label = rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString();
                        }


                        allCategory.category.Add(category2);

                        var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Equals(rDate)).Select(s => s.recordCount).FirstOrDefault();

                        Datum datum = new Datum();
                        datum.value = daywiseCount.ToString();
                        datum.link = "javascript:OpenFeed('" + rDate.ToShortDateString() + "','" + CommonFunctions.GetValueFromDescription<CommonFunctions.CategoryType>(p_Medium) + "','" + p_Medium + "','','')";

                        seriesData.data.Add(datum);

                    }

                    lstallCategory.Add(allCategory);
                    lstSeriesData.Add(seriesData);
                }
                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }*/

        public string GetLineChartForHitsHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Int64 p_HitsCount)
        {

            try
            {
                p_HitsCount = 0;

                SparkChart sparkChart = new SparkChart();
                //sparkChart.caption = "Number of Mention";

                SparkChartOutput sparkChartOutput = new SparkChartOutput();
                sparkChartOutput.chart = sparkChart;
                List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                SparkSeriesData sparkSeries = new SparkSeriesData();
                sparkSeries.data = new List<SparkDatum>();

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }

                /*var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => sm.NoOfHits)
                });*/

                p_HitsCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfHits) : 0;

                foreach (var rDate in distinctDate)
                {

                    var daywiseCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(sm => sm.NoOfHits);// Select(s => s.recordCount).FirstOrDefault();

                    SparkDatum datum = new SparkDatum();
                    datum.value = Convert.ToString(daywiseCount != null ? daywiseCount : 0);
                    sparkSeries.data.Add(datum);
                }
                lstSparkSeriesData.Add(sparkSeries);
                sparkChartOutput.dataset = lstSparkSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(sparkChartOutput);
                return jsonResult;

                #region Commented
                /*
                var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }

                Chart chart = GetChartObject();
                chart.showLabels = "0";
                chart.showlegend = "0";
                chart.caption = "Number of Mention";
                chart.showYAxisValues = "0";
                chart.showvalues = "0";
                //var distinctDate = p_lstIQAgent_DaySummaryModel.Select(s => s.DayDate.ToShortDateString()).Distinct().ToList();

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();


                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();

                seriesData.seriesname = "Number of Hits";
                seriesData.color = "";
                seriesData.anchorbordercolor = "";
                seriesData.anchorbgcolor = "";

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => sm.NoOfHits)
                });

                p_HitsCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfHits) : 0;

                foreach (DateTime rDate in distinctDate)
                {
                    Category2 category2 = new Category2();
                    category2.label = rDate.ToString();
                    allCategory.category.Add(category2);

                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Equals(rDate)).Select(s => s.recordCount).FirstOrDefault();

                    Datum datum = new Datum();
                    datum.value = daywiseCount.ToString();
                    seriesData.data.Add(datum);

                }

                lstallCategory.Add(allCategory);


                lstSeriesData.Add(seriesData);
                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;*/

                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetLineChartForViewsHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, out Int64 p_ViewsCount)
        {

            try
            {
                p_ViewsCount = 0;

                SparkChart sparkChart = new SparkChart();
                /*if (p_Medium == CommonFunctions.CategoryType.TW.ToString())
                {
                    sparkChart.caption = "Reach";
                }
                else
                {
                    sparkChart.caption = "Views";
                }*/

                SparkChartOutput sparkChartOutput = new SparkChartOutput();
                sparkChartOutput.chart = sparkChart;
                List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                SparkSeriesData sparkSeries = new SparkSeriesData();
                sparkSeries.data = new List<SparkDatum>();

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }

                /*var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => sm.Audience)
                });*/

                p_ViewsCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.Audience) : 0;

                foreach (var rDate in distinctDate)
                {

                    var daywiseCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(sm => sm.Audience); // Select(s => s.recordCount).FirstOrDefault();

                    SparkDatum datum = new SparkDatum();
                    datum.value = Convert.ToString(daywiseCount != null ? daywiseCount : 0);
                    sparkSeries.data.Add(datum);
                }
                lstSparkSeriesData.Add(sparkSeries);
                sparkChartOutput.dataset = lstSparkSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(sparkChartOutput);
                return jsonResult;

                #region Commented
                /*var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }

                Chart chart = GetChartObject();
                chart.showLabels = "0";
                chart.showlegend = "0";
                if (p_Medium == CommonFunctions.CategoryType.TW.ToString())
                {
                    chart.caption = "Reach";
                }
                else
                {
                    chart.caption = "Views";
                }
                chart.showYAxisValues = "0";
                chart.showvalues = "0";
                //var distinctDate = p_lstIQAgent_DaySummaryModel.Select(s => s.DayDate.ToShortDateString()).Distinct().ToList();

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();


                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();

                seriesData.seriesname = "Views";
                seriesData.color = "";
                seriesData.anchorbordercolor = "";
                seriesData.anchorbgcolor = "";

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => sm.Audience)
                });

                p_ViewsCount = (dayWiseTotalRecord != null && dayWiseTotalRecord.ToList().Count > 0) ? dayWiseTotalRecord.Sum(s => s.recordCount) : 0;
                foreach (DateTime rDate in distinctDate)
                {
                    Category2 category2 = new Category2();
                    category2.label = rDate.ToString();
                    allCategory.category.Add(category2);

                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Equals(rDate)).Select(s => s.recordCount).FirstOrDefault();

                    Datum datum = new Datum();
                    datum.value = daywiseCount.ToString();
                    seriesData.data.Add(datum);

                }

                lstallCategory.Add(allCategory);


                lstSeriesData.Add(seriesData);
                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;*/
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetLineChartForMinutesOfAiringHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out decimal p_MinsOfAiringCount)
        {

            try
            {
                p_MinsOfAiringCount = 0;

                SparkChart sparkChart = new SparkChart();
                //sparkChart.caption = "Minutes of Airing";

                SparkChartOutput sparkChartOutput = new SparkChartOutput();
                sparkChartOutput.chart = sparkChart;
                List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                SparkSeriesData sparkSeries = new SparkSeriesData();
                sparkSeries.data = new List<SparkDatum>();

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }

                /*var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = Math.Round(Convert.ToDouble(s.Sum(sm => (sm.NoOfDocs)) * 8) / 60, 2)
                });*/

                p_MinsOfAiringCount = Convert.ToDecimal((p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => Convert.ToDouble(s.NoOfDocs)) : 0);

                p_MinsOfAiringCount = Math.Round((p_MinsOfAiringCount * 8) / 60, 2);
                foreach (var rDate in distinctDate)
                {
                    var daywiseCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(sm => Math.Round(Convert.ToDouble((sm.NoOfDocs) * 8) / 60, 2));// Select(s => s.recordCount).FirstOrDefault();

                    SparkDatum datum = new SparkDatum();
                    datum.value = Convert.ToString(daywiseCount != null ? daywiseCount : 0);
                    sparkSeries.data.Add(datum);
                }
                lstSparkSeriesData.Add(sparkSeries);
                sparkChartOutput.dataset = lstSparkSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(sparkChartOutput);
                return jsonResult;

                #region Commented
                /*var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }

                Chart chart = GetChartObject();
                chart.showLabels = "0";
                chart.showlegend = "0";
                chart.caption = "Minutes of Airing";
                chart.showYAxisValues = "0";
                chart.showvalues = "0";
                //var distinctDate = p_lstIQAgent_DaySummaryModel.Select(s => s.DayDate.ToShortDateString()).Distinct().ToList();

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();


                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();

                seriesData.seriesname = "Minutes Of Airing";
                seriesData.color = "";
                seriesData.anchorbordercolor = "";
                seriesData.anchorbgcolor = "";

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = Math.Round(Convert.ToDouble(s.Sum(sm => (sm.NoOfDocs)) * 8) / 60, 2)
                });

                p_MinsOfAiringCount = Convert.ToDecimal((dayWiseTotalRecord != null && dayWiseTotalRecord.ToList().Count > 0) ? dayWiseTotalRecord.Sum(s => s.recordCount) : 0);
                foreach (DateTime rDate in distinctDate)
                {
                    Category2 category2 = new Category2();
                    category2.label = rDate.ToString();
                    allCategory.category.Add(category2);

                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Equals(rDate)).Select(s => s.recordCount).FirstOrDefault();

                    Datum datum = new Datum();
                    datum.value = daywiseCount.ToString();
                    seriesData.data.Add(datum);

                }

                lstallCategory.Add(allCategory);


                lstSeriesData.Add(seriesData);
                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;*/

                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetLineChartForAdHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Decimal p_AdCount)
        {

            try
            {
                p_AdCount = 0;

                SparkChart sparkChart = new SparkChart();
                //sparkChart.caption = "Ad Equivalency";

                SparkChartOutput sparkChartOutput = new SparkChartOutput();
                sparkChartOutput.chart = sparkChart;
                List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                SparkSeriesData sparkSeries = new SparkSeriesData();
                sparkSeries.data = new List<SparkDatum>();

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }

                /*var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => Convert.ToDecimal(sm.IQMediaValue))
                });*/

                p_AdCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.IQMediaValue) : 0;

                foreach (var rDate in distinctDate)
                {
                    var daywiseCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.IQMediaValue);//.FirstOrDefault();

                    SparkDatum datum = new SparkDatum();
                    datum.value = Convert.ToString(daywiseCount != null ? daywiseCount : 0);
                    sparkSeries.data.Add(datum);
                }
                lstSparkSeriesData.Add(sparkSeries);
                sparkChartOutput.dataset = lstSparkSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(sparkChartOutput);
                return jsonResult;

                #region Commented

                /*var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }

                Chart chart = GetChartObject();
                chart.showLabels = "0";
                chart.showlegend = "0";
                chart.caption = "Ad Equivalency";
                chart.showYAxisValues = "0";
                chart.showvalues = "0";
                //var distinctDate = p_lstIQAgent_DaySummaryModel.Select(s => s.DayDate.ToShortDateString()).Distinct().ToList();

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();


                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();

                seriesData.seriesname = "Ad Equivalency";
                seriesData.color = "";
                seriesData.anchorbordercolor = "";
                seriesData.anchorbgcolor = "";

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => Convert.ToDecimal(sm.IQMediaValue))
                });

                p_AdCount = (dayWiseTotalRecord != null && dayWiseTotalRecord.ToList().Count > 0) ? dayWiseTotalRecord.Sum(s => s.recordCount) : 0;

                foreach (DateTime rDate in distinctDate)
                {
                    Category2 category2 = new Category2();
                    category2.label = rDate.ToString();
                    allCategory.category.Add(category2);

                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Equals(rDate)).Select(s => s.recordCount).FirstOrDefault();

                    Datum datum = new Datum();
                    datum.value = daywiseCount.ToString();
                    seriesData.data.Add(datum);

                }

                lstallCategory.Add(allCategory);


                lstSeriesData.Add(seriesData);
                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;*/

                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetLineChartForSentimentHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate,
                                            out Int64 positiveSentiment,
                                            out Int64 negativeSentiment)
        {

            try
            {
                negativeSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.NegativeSentiment);
                positiveSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.PositiveSentiment);

                Chart chart = new Chart();
                chart.subcaption = "";
                chart.caption = "";
                chart.linethickness = "1";
                chart.showLabels = "0";
                chart.showvalues = "0";
                chart.showYAxisValues = "0";
                chart.formatnumberscale = "0";
                chart.anchorRadius = "0";
                chart.divlinealpha = "FFFFFF";
                chart.divlinecolor = "FFFFFF";
                chart.divlineisdashed = "1";
                chart.showalternatehgridcolor = "1";
                chart.alternatehgridcolor = "FFFFFF";
                chart.shadowalpha = "40";
                chart.labelstep = "1";
                chart.numvdivlines = "5";
                chart.chartrightmargin = "10";
                chart.bgcolor = "FFFFFF";
                chart.bgangle = "270";
                chart.bgalpha = "10,10";
                chart.alternatehgridalpha = "5";
                chart.legendposition = "BOTTOM";
                chart.drawAnchors = "1";
                chart.showBorder = "0";
                chart.canvasBorderAlpha = "0";
                chart.showlegend = "0";

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;
                List<SeriesData> lstSeriesData = new List<SeriesData>();


                var distinctDate = new List<DateTime>();

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                    Category2 category2 = new Category2();

                    category2.label = dt.ToString();

                    allCategory.category.Add(category2);
                }

                lstallCategory.Add(allCategory);

                SeriesData positiveSeries = new SeriesData();
                positiveSeries.data = new List<Datum>();
                positiveSeries.seriesname = "Positive Sentiment";
                positiveSeries.color = "#c7d36a";


                SeriesData negativeSeries = new SeriesData();
                negativeSeries.data = new List<Datum>();
                negativeSeries.seriesname = "Negative Sentiment";
                negativeSeries.color = "#d8635d";

                foreach (var rDate in distinctDate)
                {
                    var daywisePositiveSentimentCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.PositiveSentiment);
                    var daywiseNegativeSentimentCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.NegativeSentiment);


                    Datum datumPositive = new Datum();
                    datumPositive.value = Convert.ToString(daywisePositiveSentimentCount != null ? daywisePositiveSentimentCount : 0);
                    datumPositive.link = "#";

                    Datum datumNegative = new Datum();
                    datumNegative.value = Convert.ToString(daywiseNegativeSentimentCount != null ? daywiseNegativeSentimentCount : 0);
                    datumNegative.link = "#";

                    positiveSeries.data.Add(datumPositive);
                    negativeSeries.data.Add(datumNegative);
                }

                lstSeriesData.Add(positiveSeries);
                lstSeriesData.Add(negativeSeries);

                lineChartOutput.dataset = lstSeriesData;
                lineChartOutput.categories = lstallCategory;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;


            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public string GetHighChartForDocsHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, IQ_MediaTypeModel p_MediaType, decimal p_ClientGmtOffset, decimal p_ClientDstOffset, List<string> p_SearchRequestIDs, out Int64 p_TotalAirSeconds)
        {

            try
            {

                var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }


                List<string> categories = new List<string>();
                foreach (DateTime rDate in distinctDate)
                {
                    if (rDate.IsDaylightSavingTime())
                    {

                        categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString());
                    }
                    else
                    {
                        categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString());
                    }
                }

                p_TotalAirSeconds = ((p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfDocs) : 0) * 8;

                // this signle line medium chart, for selected medium type 
                // if one or more request request applid, then it will show multi line chart, one for each request request. 
                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
                highLineChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories,
                    labels = new labels() { staggerLines = 2 }
                };

                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { borderWidth = "0", width = 750 };

                // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
                highLineChartOutput.hChart = new HChart() { height = 300, type = "spline" };

                // start to set series of data for medium chart (or multi line search request chart)
                List<Series> lstSeries = new List<Series>();
                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    // set plot options and click event for series points (which will again assigned in JS as this is string value)
                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        spline = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };

                    // set list of data for each series 
                    foreach (var searchRequest in p_SearchRequestIDs)
                    {
                        var SearchRequest = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.SearchRequestID.ToString(), searchRequest) == 0).FirstOrDefault();

                        if (SearchRequest != null)
                        {
                            // set sereies name as search request query name, will shown in legent and tooltip.
                            Series series = new Series();
                            series.data = new List<HighChartDatum>();
                            series.name = SearchRequest.Query_Name;

                            foreach (DateTime rDate in distinctDate)
                            {
                                var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.SearchRequestID.ToString(), searchRequest, true) == 0 && smr.DayDate.Equals(rDate)).Sum(s => s.NoOfDocs);

                                // set data point of current series 
                                /*
                                    *  y = y series value of current point === total no. of records for current search request at perticular date 
                                    *  SearchTerm = query name  , used in chart drill down click event
                                    *  Value = Search Request ID  , used in chart drill down click event
                                    *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                                */
                                HighChartDatum highChartDatum = new HighChartDatum();
                                highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                                highChartDatum.SearchTerm = SearchRequest.Query_Name;
                                highChartDatum.Value = SearchRequest.SearchRequestID.ToString();
                                highChartDatum.Type = "Media";
                                series.data.Add(highChartDatum);
                            }

                            lstSeries.Add(series);
                        }

                    }
                }
                else
                {
                    // as its single media chart, we will show it as area chart, by setting chart type to "area"
                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        spline = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };

                    var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                    {
                        recordDate = s.Key,
                        recordCount = s.Sum(sm => sm.NoOfDocs)
                    });

                    // set sereies name as "Media" , will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = p_MediaType.DisplayName;

                    foreach (DateTime rDate in distinctDate)
                    {

                        var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Equals(rDate)).Select(s => s.recordCount).FirstOrDefault();

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                            *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                            *  we will not set SearchTerm and Value properies of data, as this is signle medium chart , without any search request
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                        highChartDatum.Type = "SubMedia";
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

                // assign set of series data to medium chart (or multi line searchrequest chart)
                highLineChartOutput.series = lstSeries;
                string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForHitsHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Int64 p_HitsCount, decimal p_ClientGmtOffset, decimal p_ClientDstOffset)
        {

            try
            {
                p_HitsCount = 0;

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);

                }

                List<string> categories = new List<string>();
                foreach (DateTime rDate in distinctDate)
                {
                    if (rDate.IsDaylightSavingTime())
                    {

                        categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString());
                    }
                    else
                    {
                        categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString());
                    }
                }

                p_HitsCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfHits) : 0;

                // this signle line spark chart, for selected medium type for no. of hits 
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
                //highLineChartSingleMediaChartOutput.Colors = new List<string>();

                List<PlotLine> plotlinesSingleMedia = new List<PlotLine>();

                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = categories,
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type of chart
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false, 
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };

                // series of data for medium chart
                List<Series> lstSeries = new List<Series>();

                // set series name
                Series series = new Series();
                series.name = "Mentions";
                series.data = new List<HighChartDatum>();

                // set series data
                foreach (var rDate in distinctDate)
                {

                    var daywiseCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(sm => sm.NoOfHits);// Select(s => s.recordCount).FirstOrDefault();

                    HighChartDatum highChartDatum = new Model.HighChartDatum();
                    highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                    series.data.Add(highChartDatum);
                }

                lstSeries.Add(series);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForViewsHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Int64 p_ViewsCount, decimal p_ClientGmtOffset, decimal p_ClientDstOffset)
        {

            try
            {
                p_ViewsCount = 0;

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }

                List<string> categories = new List<string>();
                foreach (DateTime rDate in distinctDate)
                {
                    if (rDate.IsDaylightSavingTime())
                    {

                        categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString());
                    }
                    else
                    {
                        categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString());
                    }
                }

                p_ViewsCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.Audience) : 0;

                // signle line spark chart, for selected medium type for audience 
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = categories,
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type of chart
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };


                // series of data for medium chart
                List<Series> lstSeries = new List<Series>();

                // set series name
                Series series = new Series();
                series.name = "Audience";
                series.data = new List<HighChartDatum>();

                // set series data
                foreach (var rDate in distinctDate)
                {
                    var daywiseCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(sm => sm.Audience);

                    HighChartDatum highChartDatum = new Model.HighChartDatum();
                    highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                    series.data.Add(highChartDatum);
                }

                lstSeries.Add(series);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForMinutesOfAiringHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out decimal p_MinsOfAiringCount, decimal p_ClientGmtOffset, decimal p_ClientDstOffset)
        {

            try
            {
                p_MinsOfAiringCount = 0;

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }

                List<string> categories = new List<string>();
                foreach (DateTime rDate in distinctDate)
                {
                    if (rDate.IsDaylightSavingTime())
                    {

                        categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString());
                    }
                    else
                    {
                        categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString());
                    }
                }

                p_MinsOfAiringCount = Convert.ToDecimal((p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfDocs) : 0);
                p_MinsOfAiringCount = Math.Round((p_MinsOfAiringCount * 8) / 60, 2);

                // signle line spark chart, for selected medium type for no. of air minute
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                List<PlotLine> plotlinesSingleMedia = new List<PlotLine>();

                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = categories,
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type of chart
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };

                // series of data for medium chart
                List<Series> lstSeries = new List<Series>();

                // set series name
                Series series = new Series();
                series.name = "Air Time";
                series.data = new List<HighChartDatum>();

                // set series data
                foreach (var rDate in distinctDate)
                {
                    var daywiseCount = Convert.ToDouble(p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.NoOfDocs));
                    daywiseCount = Math.Round(Convert.ToDouble(daywiseCount * 8) / 60, 2);

                    HighChartDatum highChartDatum = new Model.HighChartDatum();
                    highChartDatum.y = Convert.ToDecimal(daywiseCount != null ? daywiseCount : 0);
                    series.data.Add(highChartDatum);
                }

                lstSeries.Add(series);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForAdHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Decimal p_AdCount, decimal p_ClientGmtOffset, decimal p_ClientDstOffset)
        {

            try
            {

                p_AdCount = 0;

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }

                List<string> categories = new List<string>();
                foreach (DateTime rDate in distinctDate)
                {
                    if (rDate.IsDaylightSavingTime())
                    {

                        categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString());
                    }
                    else
                    {
                        categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString());
                    }
                }

                p_AdCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.IQMediaValue) : 0;

                // signle line spark chart, for selected medium type for media value 
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = categories,
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type of chart
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };

                // series of data for medium chart
                List<Series> lstSeries = new List<Series>();

                // set series name
                Series series = new Series();
                series.name = "Media Value";
                series.data = new List<HighChartDatum>();

                // set series data
                foreach (var rDate in distinctDate)
                {

                    var daywiseCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.IQMediaValue);//.FirstOrDefault();

                    HighChartDatum highChartDatum = new Model.HighChartDatum();
                    highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                    series.data.Add(highChartDatum);
                }

                lstSeries.Add(series);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForSentimentHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate,
                                            out Int64 positiveSentiment,
                                            out Int64 negativeSentiment, decimal p_ClientGmtOffset, decimal p_ClientDstOffset)
        {

            try
            {

                negativeSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.NegativeSentiment);
                positiveSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.PositiveSentiment);


                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
                {
                    distinctDate.Add(dt);
                }

                List<string> categories = new List<string>();
                foreach (DateTime rDate in distinctDate)
                {
                    if (rDate.IsDaylightSavingTime())
                    {

                        categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString());
                    }
                    else
                    {
                        categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString());
                    }
                }

                // signle line spark chart, for selected medium type for sentiment
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };


                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = categories,
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type 
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false, 
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };

                // series of data for medium chart 
                List<Series> lstSeries = new List<Series>();

                // set positive series data
                Series positiveSeries = new Series();
                positiveSeries.name = "Positive Sentiment";
                positiveSeries.data = new List<HighChartDatum>();
                positiveSeries.color = "#c7d36a";

                // set negative series data
                Series negativeSeries = new Series();
                negativeSeries.data = new List<HighChartDatum>();
                negativeSeries.name = "Negative Sentiment";
                negativeSeries.color = "#d8635d";

                // set list of data for positive and negative series 
                foreach (var rDate in distinctDate)
                {
                    var daywisePositiveSentimentCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.PositiveSentiment);
                    var daywiseNegativeSentimentCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Equals(rDate)).Sum(s => s.NegativeSentiment);


                    HighChartDatum datumPositive = new Model.HighChartDatum();
                    datumPositive.y = daywisePositiveSentimentCount != null ? daywisePositiveSentimentCount : 0;

                    HighChartDatum datumNegative = new HighChartDatum();
                    datumNegative.y = daywiseNegativeSentimentCount != null ? daywiseNegativeSentimentCount : 0;

                    positiveSeries.data.Add(datumPositive);
                    negativeSeries.data.Add(datumNegative);
                }

                lstSeries.Add(positiveSeries);
                lstSeries.Add(negativeSeries);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public string GetHighChartsForDocsForDmaHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, List<DashboardDMAChartSelectionModel> p_Dmas, decimal p_ClientGmtOffset, decimal p_ClientDstOffset)
        {
            var distinctDate = new List<DateTime>();

            for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
            {
                distinctDate.Add(dt);
            }


            List<string> categories = new List<string>();
            foreach (DateTime rDate in distinctDate)
            {
                if (rDate.IsDaylightSavingTime())
                {

                    categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString());
                }
                else
                {
                    categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString());
                }
            }


            HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
            highLineChartOutput.title = new Title() { text = "", x = -20 };
            highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
            highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };
            /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
            /* 
                if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
            */
            /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
            highLineChartOutput.xAxis = new XAxis()
            {
                tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                tickmarkPlacement = "off",
                categories = categories,
                labels = new labels()
                {
                    enabled = false
                }
            };

            // set plot options and disable marker
            highLineChartOutput.plotOption = new PlotOptions()
            {
                spline = new PlotSeries()
                {
                    events = new PlotEvents()
                    {
                        mouseOver = "HandleChartMouseHover",
                        mouseOut = "HandleChartMouseOut"
                    },
                    marker = new PlotMarker()
                    {
                        enabled = false,
                        lineWidth = 0
                    }
                }
            };

            // show default tooltip format x / y values
            highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "", shared = true };

            // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
            highLineChartOutput.legend = new Legend() { enabled = false };

            // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
            highLineChartOutput.hChart = new HChart() { height = 100, width = 140, type = "spline" };

            // start to set series of data for medium chart (or multi line search request chart)
            List<Series> lstSeries = new List<Series>();


            // set list of data for each series 
            foreach (var dma in p_Dmas)
            {
                var reqDma = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.Query_Name, dma.id, true) == 0).FirstOrDefault();

                if (reqDma != null)
                {
                    // set sereies name as search request query name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = reqDma.Query_Name;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.Query_Name.ToString(), dma.id, true) == 0 && smr.DayDate.Equals(rDate)).Sum(s => s.NoOfDocs);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }
                else
                {
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = dma.id;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

            }

            // assign set of series data to medium chart (or multi line searchrequest chart)
            highLineChartOutput.series = lstSeries;
            string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
            return jsonResult;
        }

        public string GetHighChartsForHitsForDmaHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, List<DashboardDMAChartSelectionModel> p_Dmas, decimal p_ClientGmtOffset, decimal p_ClientDstOffset)
        {
            var distinctDate = new List<DateTime>();

            for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
            {
                distinctDate.Add(dt);
            }


            List<string> categories = new List<string>();
            foreach (DateTime rDate in distinctDate)
            {
                if (rDate.IsDaylightSavingTime())
                {

                    categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString());
                }
                else
                {
                    categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString());
                }
            }


            HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
            highLineChartOutput.title = new Title() { text = "", x = -20 };
            highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
            highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };
            /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
            /* 
                if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
            */
            /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
            highLineChartOutput.xAxis = new XAxis()
            {
                tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                tickmarkPlacement = "off",
                categories = categories,
                labels = new labels()
                {
                    enabled = false
                }
            };

            // set plot options and disable marker
            highLineChartOutput.plotOption = new PlotOptions()
            {
                spline = new PlotSeries()
                {
                    events = new PlotEvents()
                    {
                        mouseOver = "HandleChartMouseHover",
                        mouseOut = "HandleChartMouseOut"
                    },
                    marker = new PlotMarker()
                    {
                        enabled = false,
                        lineWidth = 0
                    }
                }
            };

            // show default tooltip format x / y values
            highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "", shared = true };

            // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
            highLineChartOutput.legend = new Legend() { enabled = false };

            // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
            highLineChartOutput.hChart = new HChart() { height = 100, width = 140, type = "spline" };

            // start to set series of data for medium chart (or multi line search request chart)
            List<Series> lstSeries = new List<Series>();


            // set list of data for each series 
            foreach (var dma in p_Dmas)
            {
                var reqDma = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.Query_Name, dma.id, true) == 0).FirstOrDefault();

                if (reqDma != null)
                {
                    // set sereies name as search request query name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = reqDma.Query_Name;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.Query_Name.ToString(), dma.id, true) == 0 && smr.DayDate.Equals(rDate)).Sum(s => s.NoOfHits);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }
                else
                {
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = dma.id;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

            }

            // assign set of series data to medium chart (or multi line searchrequest chart)
            highLineChartOutput.series = lstSeries;
            string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
            return jsonResult;
        }

        public string GetHighChartsForMinutesOfAiringForDmaHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, List<DashboardDMAChartSelectionModel> p_Dmas, decimal p_ClientGmtOffset, decimal p_ClientDstOffset)
        {
            var distinctDate = new List<DateTime>();

            for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
            {
                distinctDate.Add(dt);
            }


            List<string> categories = new List<string>();
            foreach (DateTime rDate in distinctDate)
            {
                if (rDate.IsDaylightSavingTime())
                {

                    categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString());
                }
                else
                {
                    categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString());
                }
            }


            HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
            highLineChartOutput.title = new Title() { text = "", x = -20 };
            highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
            highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };
            /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
            /* 
                if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
            */
            /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
            highLineChartOutput.xAxis = new XAxis()
            {
                tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                tickmarkPlacement = "off",
                categories = categories,
                labels = new labels()
                {
                    enabled = false
                }
            };

            // set plot options and disable marker
            highLineChartOutput.plotOption = new PlotOptions()
            {
                spline = new PlotSeries()
                {
                    events = new PlotEvents()
                    {
                        mouseOver = "HandleChartMouseHover",
                        mouseOut = "HandleChartMouseOut"
                    },
                    marker = new PlotMarker()
                    {
                        enabled = false,
                        lineWidth = 0
                    }
                }
            };

            // show default tooltip format x / y values
            highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "", shared = true };

            // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
            highLineChartOutput.legend = new Legend() { enabled = false };

            // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
            highLineChartOutput.hChart = new HChart() { height = 100, width = 140, type = "spline" };

            // start to set series of data for medium chart (or multi line search request chart)
            List<Series> lstSeries = new List<Series>();


            // set list of data for each series 
            foreach (var dma in p_Dmas)
            {
                var reqDma = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.Query_Name, dma.id, true) == 0).FirstOrDefault();

                if (reqDma != null)
                {
                    // set sereies name as search request query name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = reqDma.Query_Name;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        var daywiseCount = Convert.ToDecimal(p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.Query_Name.ToString(), dma.id, true) == 0 && smr.DayDate.Equals(rDate)).Sum(s => s.NoOfDocs));
                        daywiseCount = Math.Round(Convert.ToDecimal(daywiseCount * 8) / 60, 2);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }
                else
                {
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = dma.id;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

            }

            // assign set of series data to medium chart (or multi line searchrequest chart)
            highLineChartOutput.series = lstSeries;
            string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
            return jsonResult;
        }

        public string GetHighChartsForViewsForDmaHourly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, List<DashboardDMAChartSelectionModel> p_Dmas, decimal p_ClientGmtOffset, decimal p_ClientDstOffset)
        {
            var distinctDate = new List<DateTime>();

            for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddHours(1))
            {
                distinctDate.Add(dt);
            }


            List<string> categories = new List<string>();
            foreach (DateTime rDate in distinctDate)
            {
                if (rDate.IsDaylightSavingTime())
                {

                    categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset)) + Convert.ToDouble(p_ClientDstOffset)).ToString());
                }
                else
                {
                    categories.Add(rDate.AddHours((Convert.ToDouble(p_ClientGmtOffset))).ToString());
                }
            }


            HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
            highLineChartOutput.title = new Title() { text = "", x = -20 };
            highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
            highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };
            /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
            /* 
                if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
            */
            /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
            highLineChartOutput.xAxis = new XAxis()
            {
                tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                tickmarkPlacement = "off",
                categories = categories,
                labels = new labels()
                {
                    enabled = false
                }
            };

            // set plot options and disable marker
            highLineChartOutput.plotOption = new PlotOptions()
            {
                spline = new PlotSeries()
                {
                    events = new PlotEvents()
                    {
                        mouseOver = "HandleChartMouseHover",
                        mouseOut = "HandleChartMouseOut"
                    },
                    marker = new PlotMarker()
                    {
                        enabled = false,
                        lineWidth = 0
                    }
                }
            };

            // show default tooltip format x / y values
            highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "", shared = true };

            // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
            highLineChartOutput.legend = new Legend() { enabled = false };

            // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
            highLineChartOutput.hChart = new HChart() { height = 100, width = 140, type = "spline" };

            // start to set series of data for medium chart (or multi line search request chart)
            List<Series> lstSeries = new List<Series>();


            // set list of data for each series 
            foreach (var dma in p_Dmas)
            {
                var reqDma = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.Query_Name, dma.id, true) == 0).FirstOrDefault();

                if (reqDma != null)
                {
                    // set sereies name as search request query name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = reqDma.Query_Name;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.Query_Name.ToString(), dma.id, true) == 0 && smr.DayDate.Equals(rDate)).Sum(s => s.Audience);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }
                else
                {
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = dma.id;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

            }

            // assign set of series data to medium chart (or multi line searchrequest chart)
            highLineChartOutput.series = lstSeries;
            string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
            return jsonResult;
        }
        #endregion

        #region Monthly Chart

        public string GetHighChartForDocsMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, IQ_MediaTypeModel p_MediaType, List<string> p_SearchRequestIDs, out Int64 p_TotalAirSeconds)
        {

            try
            {
                var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    distinctDate.Add(dt);
                }


                List<string> categories = new List<string>();
                foreach (DateTime rDate in distinctDate)
                {
                    categories.Add(rDate.ToShortDateString());
                }

                p_TotalAirSeconds = ((p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfDocs) : 0) * 8;

                // this signle line medium chart, for selected medium type 
                // if one or more request request applid, then it will show multi line chart, one for each request request. 
                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
                highLineChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };


                highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories,
                    labels = new labels()
                    {
                        formatter = "GetMonth"
                    }
                };

                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { borderWidth = "0", width = 750 };

                // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
                highLineChartOutput.hChart = new HChart() { height = 300, type = "spline" };

                // start to set series of data for medium chart (or multi line search request chart)
                List<Series> lstSeries = new List<Series>();
                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    // set plot options and click event for series points (which will again assigned in JS as this is string value)
                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        spline = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };

                    // set list of data for each series 
                    foreach (var searchRequest in p_SearchRequestIDs)
                    {
                        var SearchRequest = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.SearchRequestID.ToString(), searchRequest) == 0).FirstOrDefault();

                        if (SearchRequest != null)
                        {
                            // set sereies name as search request query name, will shown in legent and tooltip.
                            Series series = new Series();
                            series.data = new List<HighChartDatum>();
                            series.name = SearchRequest.Query_Name;

                            foreach (DateTime rDate in distinctDate)
                            {
                                var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.SearchRequestID.ToString(), searchRequest, true) == 0 && smr.DayDate.Month.Equals(rDate.Month) && smr.DayDate.Year.Equals(rDate.Year)).Sum(s => s.NoOfDocs);

                                // set data point of current series 
                                /*
                                    *  y = y series value of current point === total no. of records for current search request at perticular date 
                                    *  SearchTerm = query name  , used in chart drill down click event
                                    *  Value = Search Request ID  , used in chart drill down click event
                                    *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                                */
                                HighChartDatum highChartDatum = new HighChartDatum();
                                highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                                highChartDatum.SearchTerm = SearchRequest.Query_Name;
                                highChartDatum.Value = SearchRequest.SearchRequestID.ToString();
                                highChartDatum.Type = "Media";
                                series.data.Add(highChartDatum);
                            }

                            lstSeries.Add(series);
                        }

                    }
                }
                else
                {
                    // as its single media chart, we will show it as area chart, by setting chart type to "area"
                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        spline = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };

                    var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => new { g.DayDate.Month, g.DayDate.Year }).Select(s => new
                    {
                        recordDate = s.Key,
                        recordCount = s.Sum(sm => sm.NoOfDocs)
                    });

                    // set sereies name as "Media" , will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = p_MediaType.DisplayName;

                    foreach (DateTime rDate in distinctDate)
                    {

                        var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Month.Equals(rDate.Month) && d.recordDate.Year.Equals(rDate.Year)).Select(s => s.recordCount).FirstOrDefault();

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                            *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                            *  we will not set SearchTerm and Value properies of data, as this is signle medium chart , without any search request
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                        highChartDatum.Type = "SubMedia";
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

                // assign set of series data to medium chart (or multi line searchrequest chart)
                highLineChartOutput.series = lstSeries;
                string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForHitsMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Int64 p_HitsCount)
        {

            try
            {
                p_HitsCount = 0;

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    distinctDate.Add(dt);

                }

                p_HitsCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfHits) : 0;

                // signle line spark chart, for selected medium type for no. of hits 
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = distinctDate.Select(a => a.ToShortDateString()).ToList(),
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type of chart
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false, 
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => sm.NoOfHits)
                });

                // series of data for medium chart
                List<Series> lstSeries = new List<Series>();

                // set series name
                Series series = new Series();
                series.name = "Mentions";
                series.data = new List<HighChartDatum>();

                // set series data
                foreach (var rDate in distinctDate)
                {

                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Month.Equals(rDate.Month) && d.recordDate.Year.Equals(rDate.Year)).Select(s => s.recordCount).FirstOrDefault();

                    HighChartDatum highChartDatum = new Model.HighChartDatum();
                    highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                    series.data.Add(highChartDatum);
                }

                lstSeries.Add(series);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForViewsMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Int64 p_ViewsCount)
        {

            try
            {
                p_ViewsCount = 0;

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    distinctDate.Add(dt);
                }

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => sm.Audience)
                });

                p_ViewsCount = (dayWiseTotalRecord != null && dayWiseTotalRecord.ToList().Count > 0) ? dayWiseTotalRecord.Sum(s => s.recordCount) : 0;

                // single line spark chart, for selected medium type for audience
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };


                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = distinctDate.Select(a => a.ToShortDateString()).ToList(),
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type of chart
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false, 
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };


                // series of data for medium chart
                List<Series> lstSeries = new List<Series>();

                // set series name
                Series series = new Series();
                series.name = "Audience";
                series.data = new List<HighChartDatum>();

                // set series data
                foreach (var rDate in distinctDate)
                {

                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Month.Equals(rDate.Month) && d.recordDate.Year.Equals(rDate.Year)).Select(s => s.recordCount).FirstOrDefault();

                    HighChartDatum highChartDatum = new Model.HighChartDatum();
                    highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                    series.data.Add(highChartDatum);
                }

                lstSeries.Add(series);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForMinutesOfAiringMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Decimal p_MinsOfAiring)
        {

            try
            {

                p_MinsOfAiring = 0;

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    distinctDate.Add(dt);
                }

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = Math.Round(Convert.ToDouble(s.Sum(sm => (sm.NoOfDocs)) * 8) / 60, 2)
                });

                p_MinsOfAiring = Convert.ToDecimal((dayWiseTotalRecord != null && dayWiseTotalRecord.ToList().Count > 0) ? dayWiseTotalRecord.Sum(s => s.recordCount) : 0);

                // signle line spark chart, for selected medium type for  minute of airing
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = distinctDate.Select(a => a.ToShortDateString()).ToList(),
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type of chart
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false, 
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };


                // series of data for medium chart
                List<Series> lstSeries = new List<Series>();

                // set series name
                Series series = new Series();
                series.name = "Air Time";
                series.data = new List<HighChartDatum>();

                // set series data
                foreach (var rDate in distinctDate)
                {
                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Month.Equals(rDate.Month) && d.recordDate.Year.Equals(rDate.Year)).Select(s => s.recordCount).FirstOrDefault();

                    HighChartDatum highChartDatum = new Model.HighChartDatum();
                    highChartDatum.y = Convert.ToDecimal(daywiseCount != null ? daywiseCount : 0);
                    series.data.Add(highChartDatum);
                }

                lstSeries.Add(series);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForAdMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Decimal p_AdCount)
        {

            try
            {
                p_AdCount = 0;

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    distinctDate.Add(dt);
                }

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => Convert.ToDecimal(sm.IQMediaValue))
                });

                p_AdCount = (dayWiseTotalRecord != null && dayWiseTotalRecord.ToList().Count > 0) ? dayWiseTotalRecord.Sum(s => s.recordCount) : 0;

                // signle line spark chart, for selected medium type for no. of hits 
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = distinctDate.Select(a => a.ToShortDateString()).ToList(),
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type of chart
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };


                // series of data for medium chart
                List<Series> lstSeries = new List<Series>();

                // set series name
                Series series = new Series();
                series.name = "Media Value";
                series.data = new List<HighChartDatum>();

                // set series data
                foreach (var rDate in distinctDate)
                {

                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Month.Equals(rDate.Month) && d.recordDate.Year.Equals(rDate.Year)).Select(s => s.recordCount).FirstOrDefault();

                    HighChartDatum highChartDatum = new Model.HighChartDatum();
                    highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                    series.data.Add(highChartDatum);
                }

                lstSeries.Add(series);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetHighChartForSentimentMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate,
                                            out Int64 positiveSentiment,
                                            out Int64 negativeSentiment)
        {

            try
            {
                negativeSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.NegativeSentiment);
                positiveSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.PositiveSentiment);


                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    distinctDate.Add(dt);
                }

                // signle line spark chart, for selected medium type for sentiment
                HighLineChartOutput highLineChartSingleMediaChartOutput = new HighLineChartOutput();
                highLineChartSingleMediaChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartSingleMediaChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSingleMediaChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                // set chart with height , width and type 
                highLineChartSingleMediaChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    tickmarkPlacement = "off",
                    categories = distinctDate.Select(a => a.ToShortDateString()).ToList(),
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                // set chart with height , width and type 
                highLineChartSingleMediaChartOutput.hChart = new HChart() { height = 100, width = 120, type = "spline" };

                // show default tooltip format x / y values
                highLineChartSingleMediaChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // set plot options and disable marker
                highLineChartSingleMediaChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                // hide legend by setting legend with enabled = false
                highLineChartSingleMediaChartOutput.legend = new Legend() { enabled = false };


                // set series of data for medium chart 
                List<Series> lstSeries = new List<Series>();


                // set positive series data
                Series positiveSeries = new Series();
                positiveSeries.name = "Positive Sentiment";
                positiveSeries.data = new List<HighChartDatum>();
                positiveSeries.color = "#c7d36a";

                // set negative series data
                Series negativeSeries = new Series();
                negativeSeries.data = new List<HighChartDatum>();
                negativeSeries.name = "Negative Sentiment";
                negativeSeries.color = "#d8635d";

                // set list of data for positive and negative series 
                foreach (var rDate in distinctDate)
                {
                    var daywisePositiveSentimentCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Month.Equals(rDate.Month) && d.DayDate.Year.Equals(rDate.Year)).Sum(s => s.PositiveSentiment);
                    var daywiseNegativeSentimentCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Month.Equals(rDate.Month) && d.DayDate.Year.Equals(rDate.Year)).Sum(s => s.NegativeSentiment);

                    HighChartDatum datumPositive = new Model.HighChartDatum();
                    datumPositive.y = daywisePositiveSentimentCount != null ? daywisePositiveSentimentCount : 0;

                    HighChartDatum datumNegative = new HighChartDatum();
                    datumNegative.y = daywiseNegativeSentimentCount != null ? daywiseNegativeSentimentCount : 0;

                    positiveSeries.data.Add(datumPositive);
                    negativeSeries.data.Add(datumNegative);
                }

                lstSeries.Add(positiveSeries);
                lstSeries.Add(negativeSeries);

                highLineChartSingleMediaChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartSingleMediaChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /*
        public string GetLineChartForDocsMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, List<string> p_SearchRequestIDs, out Int64 p_TotalAirSeconds)
        {

            try
            {
                var distinctDate = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    distinctDate.Add(dt);
                }

                p_TotalAirSeconds = ((p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.ToList().Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfDocs) : 0) * 8;

                Chart chart = GetChartObject();
                chart.showLabels = "1";
                chart.canvasRightMargin = "20";
                //var distinctDate = p_lstIQAgent_DaySummaryModel.Select(s => s.DayDate.ToShortDateString()).Distinct().ToList();

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();


                List<SeriesData> lstSeriesData = new List<SeriesData>();
                SeriesData seriesData = new SeriesData();
                seriesData.data = new List<Datum>();

                seriesData.seriesname = p_Medium;
                seriesData.color = "";
                //seriesData.anchorBorderColor = "";
                //seriesData.anchorBgColor = "";

                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {

                    foreach (DateTime rDate in distinctDate)
                    {
                        Category2 category2 = new Category2();
                        category2.label = rDate.ToShortDateString();
                        allCategory.category.Add(category2);
                    }
                    lstallCategory.Add(allCategory);

                    SparkChart sparkChartMediaWise = new SparkChart();
                    SparkChartOutput sparkChartOutputMediaWise = new SparkChartOutput();
                    foreach (var searchRequest in p_SearchRequestIDs)
                    {
                        sparkChartOutputMediaWise.chart = sparkChartMediaWise;
                        List<SparkSeriesData> lstSparkSeriesDataMediaWise = new List<SparkSeriesData>();
                        SparkSeriesData sparkSeriesMediaWise = new SparkSeriesData();
                        sparkSeriesMediaWise.data = new List<SparkDatum>();
                        //sparkChartMediaWise.caption = CommonFunctions.GetEnumDescription(subMedia);

                        seriesData = new SeriesData();
                        seriesData.data = new List<Datum>();
                        var SearchRequest = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.SearchRequestID.ToString(), searchRequest) == 0).FirstOrDefault();
                        if (SearchRequest != null)
                        {
                            seriesData.seriesname = SearchRequest.Query_Name;
                            seriesData.color = "";
                            //multiSeriesData.anchorBorderColor = "";
                            //multiSeriesData.anchorBgColor = "";

                            foreach (DateTime rDate in distinctDate)
                            {
                                var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.SearchRequestID.ToString(), searchRequest, true) == 0 && smr.DayDate.Month.Equals(rDate.Month) && smr.DayDate.Year.Equals(rDate.Year)).Sum(s => s.NoOfDocs);

                                Datum datum = new Datum();
                                datum.value = Convert.ToString(daywiseSum != null ? daywiseSum : 0);
                                datum.link = "javascript:OpenFeed('" + rDate.ToShortDateString() + "','" + CommonFunctions.GetValueFromDescription<IQMedia.Shared.Utility.CommonFunctions.CategoryType>(p_Medium) + "','" + p_Medium + "','[" + searchRequest + "]','[\"" + System.Web.HttpUtility.UrlEncode(SearchRequest.Query_Name.Replace("\"", "\\\"")) + "\"]')";

                                seriesData.data.Add(datum);


                                SparkDatum sparkDatum = new SparkDatum();
                                sparkDatum.value = Convert.ToString(daywiseSum != null ? daywiseSum : 0);
                                sparkSeriesMediaWise.data.Add(sparkDatum);
                            }



                            lstSparkSeriesDataMediaWise.Add(sparkSeriesMediaWise);
                            sparkChartOutputMediaWise.dataset = lstSparkSeriesDataMediaWise;

                            //Multi Line Charts
                            lstSeriesData.Add(seriesData);
                        }
                    }
                }
                else
                {

                    var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => new { g.DayDate.Month, g.DayDate.Year }).Select(s => new
                    {
                        recordDate = s.Key,
                        recordCount = s.Sum(sm => sm.NoOfDocs)
                    });

                    foreach (DateTime rDate in distinctDate)
                    {
                        Category2 category2 = new Category2();
                        category2.label = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(rDate.Month) + " - " + rDate.Year;
                        allCategory.category.Add(category2);

                        var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Month.Equals(rDate.Month) && d.recordDate.Year.Equals(rDate.Year)).Select(s => s.recordCount).FirstOrDefault();

                        Datum datum = new Datum();
                        datum.value = daywiseCount.ToString();
                        datum.link = "javascript:OpenFeed('" + rDate.ToShortDateString() + "','" + CommonFunctions.GetValueFromDescription<CommonFunctions.CategoryType>(p_Medium) + "','" + p_Medium + "','','')";


                        seriesData.data.Add(datum);

                    }

                    lstallCategory.Add(allCategory);
                    lstSeriesData.Add(seriesData);
                }
                lineChartOutput.categories = lstallCategory;
                lineChartOutput.dataset = lstSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }*/

        public string GetLineChartForHitsMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Int64 p_HitsCount)
        {

            try
            {
                p_HitsCount = 0;

                SparkChart sparkChart = new SparkChart();
                //sparkChart.caption = "Number Of Mention";

                SparkChartOutput sparkChartOutput = new SparkChartOutput();
                sparkChartOutput.chart = sparkChart;
                List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                SparkSeriesData sparkSeries = new SparkSeriesData();
                sparkSeries.data = new List<SparkDatum>();

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    distinctDate.Add(dt);
                }

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => sm.NoOfHits)
                });

                p_HitsCount = (p_lstIQAgent_DaySummaryModel != null && p_lstIQAgent_DaySummaryModel.Count > 0) ? p_lstIQAgent_DaySummaryModel.Sum(s => s.NoOfHits) : 0;

                foreach (var rDate in distinctDate)
                {
                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Month.Equals(rDate.Month) && d.recordDate.Year.Equals(rDate.Year)).Select(s => s.recordCount).FirstOrDefault();

                    SparkDatum datum = new SparkDatum();
                    datum.value = Convert.ToString(daywiseCount != null ? daywiseCount : 0);
                    sparkSeries.data.Add(datum);
                }
                lstSparkSeriesData.Add(sparkSeries);
                sparkChartOutput.dataset = lstSparkSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(sparkChartOutput);
                return jsonResult;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetLineChartForViewsMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, out Int64 p_ViewsCount)
        {

            try
            {
                p_ViewsCount = 0;

                SparkChart sparkChart = new SparkChart();
                /*if (p_Medium == CommonFunctions.CategoryType.TW.ToString())
                {
                    sparkChart.caption = "Reach";
                }
                else
                {
                    sparkChart.caption = "Views";
                }*/

                SparkChartOutput sparkChartOutput = new SparkChartOutput();
                sparkChartOutput.chart = sparkChart;
                List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                SparkSeriesData sparkSeries = new SparkSeriesData();
                sparkSeries.data = new List<SparkDatum>();

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    distinctDate.Add(dt);
                }

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => sm.Audience)
                });

                p_ViewsCount = (dayWiseTotalRecord != null && dayWiseTotalRecord.ToList().Count > 0) ? dayWiseTotalRecord.Sum(s => s.recordCount) : 0;

                foreach (var rDate in distinctDate)
                {
                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Month.Equals(rDate.Month) && d.recordDate.Year.Equals(rDate.Year)).Select(s => s.recordCount).FirstOrDefault();

                    SparkDatum datum = new SparkDatum();
                    datum.value = Convert.ToString(daywiseCount != null ? daywiseCount : 0);
                    sparkSeries.data.Add(datum);
                }
                lstSparkSeriesData.Add(sparkSeries);
                sparkChartOutput.dataset = lstSparkSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(sparkChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetLineChartForMinutesOfAiringMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Decimal p_MinsOfAiring)
        {

            try
            {
                p_MinsOfAiring = 0;

                SparkChart sparkChart = new SparkChart();
                //sparkChart.caption = "Minutes of Airing";

                SparkChartOutput sparkChartOutput = new SparkChartOutput();
                sparkChartOutput.chart = sparkChart;
                List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                SparkSeriesData sparkSeries = new SparkSeriesData();
                sparkSeries.data = new List<SparkDatum>();

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    distinctDate.Add(dt);
                }

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = Math.Round(Convert.ToDouble(s.Sum(sm => (sm.NoOfDocs)) * 8) / 60, 2)
                });

                p_MinsOfAiring = Convert.ToDecimal((dayWiseTotalRecord != null && dayWiseTotalRecord.ToList().Count > 0) ? dayWiseTotalRecord.Sum(s => s.recordCount) : 0);

                foreach (var rDate in distinctDate)
                {
                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Month.Equals(rDate.Month) && d.recordDate.Year.Equals(rDate.Year)).Select(s => s.recordCount).FirstOrDefault();

                    SparkDatum datum = new SparkDatum();
                    datum.value = Convert.ToString(daywiseCount != null ? daywiseCount : 0);
                    sparkSeries.data.Add(datum);
                }
                lstSparkSeriesData.Add(sparkSeries);
                sparkChartOutput.dataset = lstSparkSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(sparkChartOutput);
                return jsonResult;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetLineChartForAdMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, out Decimal p_AdCount)
        {

            try
            {
                p_AdCount = 0;

                SparkChart sparkChart = new SparkChart();
                //sparkChart.caption = "Ad Equivalency";

                SparkChartOutput sparkChartOutput = new SparkChartOutput();
                sparkChartOutput.chart = sparkChart;
                List<SparkSeriesData> lstSparkSeriesData = new List<SparkSeriesData>();
                SparkSeriesData sparkSeries = new SparkSeriesData();
                sparkSeries.data = new List<SparkDatum>();

                var distinctDate = new List<DateTime>();
                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    distinctDate.Add(dt);
                }

                var dayWiseTotalRecord = p_lstIQAgent_DaySummaryModel.GroupBy(g => g.DayDate).Select(s => new
                {
                    recordDate = s.Key,
                    recordCount = s.Sum(sm => Convert.ToDecimal(sm.IQMediaValue))
                });

                p_AdCount = (dayWiseTotalRecord != null && dayWiseTotalRecord.ToList().Count > 0) ? dayWiseTotalRecord.Sum(s => s.recordCount) : 0;

                foreach (var rDate in distinctDate)
                {
                    var daywiseCount = dayWiseTotalRecord.Where(d => d.recordDate.Month.Equals(rDate.Month) && d.recordDate.Year.Equals(rDate.Year)).Select(s => s.recordCount).FirstOrDefault();

                    SparkDatum datum = new SparkDatum();
                    datum.value = Convert.ToString(daywiseCount != null ? daywiseCount : 0);
                    sparkSeries.data.Add(datum);
                }
                lstSparkSeriesData.Add(sparkSeries);
                sparkChartOutput.dataset = lstSparkSeriesData;

                string jsonResult = CommonFunctions.SearializeJson(sparkChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetLineChartForSentimentMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate,
                                            out Int64 positiveSentiment,
                                            out Int64 negativeSentiment)
        {

            try
            {
                negativeSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.NegativeSentiment);
                positiveSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.PositiveSentiment);

                Chart chart = new Chart();
                chart.subcaption = "";
                chart.caption = "";
                chart.linethickness = "1";
                chart.showLabels = "0";
                chart.showvalues = "0";
                chart.showYAxisValues = "0";
                chart.formatnumberscale = "0";
                chart.anchorRadius = "0";
                chart.divlinealpha = "FFFFFF";
                chart.divlinecolor = "FFFFFF";
                chart.divlineisdashed = "1";
                chart.showalternatehgridcolor = "1";
                chart.alternatehgridcolor = "FFFFFF";
                chart.shadowalpha = "40";
                chart.labelstep = "1";
                chart.numvdivlines = "5";
                chart.chartrightmargin = "10";
                chart.bgcolor = "FFFFFF";
                chart.bgangle = "270";
                chart.bgalpha = "10,10";
                chart.alternatehgridalpha = "5";
                chart.legendposition = "BOTTOM";
                chart.drawAnchors = "1";
                chart.showBorder = "0";
                chart.canvasBorderAlpha = "0";
                chart.showlegend = "0";

                LineChartOutput lineChartOutput = new LineChartOutput();
                lineChartOutput.chart = chart;
                List<SeriesData> lstSeriesData = new List<SeriesData>();


                var distinctDate = new List<DateTime>();

                List<AllCategory> lstallCategory = new List<AllCategory>();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                {
                    distinctDate.Add(dt);
                    Category2 category2 = new Category2();

                    category2.label = dt.ToShortDateString();

                    allCategory.category.Add(category2);
                }

                lstallCategory.Add(allCategory);

                SeriesData positiveSeries = new SeriesData();
                positiveSeries.data = new List<Datum>();
                positiveSeries.seriesname = "Positive Sentiment";
                positiveSeries.color = "#c7d36a";


                SeriesData negativeSeries = new SeriesData();
                negativeSeries.data = new List<Datum>();
                negativeSeries.seriesname = "Negative Sentiment";
                negativeSeries.color = "#d8635d";

                foreach (var rDate in distinctDate)
                {
                    var daywisePositiveSentimentCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Month.Equals(rDate.Month) && d.DayDate.Year.Equals(rDate.Year)).Sum(s => s.PositiveSentiment);
                    var daywiseNegativeSentimentCount = p_lstIQAgent_DaySummaryModel.Where(d => d.DayDate.Month.Equals(rDate.Month) && d.DayDate.Year.Equals(rDate.Year)).Sum(s => s.NegativeSentiment);


                    Datum datumPositive = new Datum();
                    datumPositive.value = Convert.ToString(daywisePositiveSentimentCount != null ? daywisePositiveSentimentCount : 0);
                    datumPositive.link = "#";

                    Datum datumNegative = new Datum();
                    datumNegative.value = Convert.ToString(daywiseNegativeSentimentCount != null ? daywiseNegativeSentimentCount : 0);
                    datumNegative.link = "#";

                    positiveSeries.data.Add(datumPositive);
                    negativeSeries.data.Add(datumNegative);
                }

                lstSeriesData.Add(positiveSeries);
                lstSeriesData.Add(negativeSeries);

                lineChartOutput.dataset = lstSeriesData;
                lineChartOutput.categories = lstallCategory;

                string jsonResult = CommonFunctions.SearializeJson(lineChartOutput);
                return jsonResult;


            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public string GetHighChartsForDocsForDmaMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, List<DashboardDMAChartSelectionModel> p_Dmas)
        {
            var distinctDate = new List<DateTime>();


            for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
            {
                distinctDate.Add(dt);
            }


            List<string> categories = new List<string>();
            foreach (DateTime rDate in distinctDate)
            {
                categories.Add(rDate.ToShortDateString());
            }


            HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
            highLineChartOutput.title = new Title() { text = "", x = -20 };
            highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
            highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };
            /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
            /* 
                if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
            */
            /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
            highLineChartOutput.xAxis = new XAxis()
            {
                tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                tickmarkPlacement = "off",
                categories = categories,
                labels = new labels()
                {
                    enabled = false
                }
            };

            // set plot options and disable marker
            highLineChartOutput.plotOption = new PlotOptions()
            {
                spline = new PlotSeries()
                {
                    events = new PlotEvents()
                    {
                        mouseOver = "HandleChartMouseHover",
                        mouseOut = "HandleChartMouseOut"
                    },
                    marker = new PlotMarker()
                    {
                        enabled = false,
                        lineWidth = 0
                    }
                }
            };

            // show default tooltip format x / y values
            highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "", shared = true };

            // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
            highLineChartOutput.legend = new Legend() { enabled = false };

            // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
            highLineChartOutput.hChart = new HChart() { height = 100, width = 140, type = "spline" };

            // start to set series of data for medium chart (or multi line search request chart)
            List<Series> lstSeries = new List<Series>();


            // set list of data for each series 
            foreach (var dma in p_Dmas)
            {
                var reqDma = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.Query_Name, dma.id, true) == 0).FirstOrDefault();

                if (reqDma != null)
                {
                    // set sereies name as search request query name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = reqDma.Query_Name;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.Query_Name.ToString(), dma.id, true) == 0 && smr.DayDate.Month.Equals(rDate.Month) && smr.DayDate.Year.Equals(rDate.Year)).Sum(s => s.NoOfDocs);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }
                else
                {
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = dma.id;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

            }

            // assign set of series data to medium chart (or multi line searchrequest chart)
            highLineChartOutput.series = lstSeries;
            string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
            return jsonResult;
        }

        public string GetHighChartsForHitsForDmaMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, List<DashboardDMAChartSelectionModel> p_Dmas)
        {
            var distinctDate = new List<DateTime>();


            for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
            {
                distinctDate.Add(dt);
            }


            List<string> categories = new List<string>();
            foreach (DateTime rDate in distinctDate)
            {
                categories.Add(rDate.ToShortDateString());
            }


            HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
            highLineChartOutput.title = new Title() { text = "", x = -20 };
            highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
            highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };
            /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
            /* 
                if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
            */
            /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
            highLineChartOutput.xAxis = new XAxis()
            {
                tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                tickmarkPlacement = "off",
                categories = categories,
                labels = new labels()
                {
                    enabled = false
                }
            };

            // set plot options and disable marker
            highLineChartOutput.plotOption = new PlotOptions()
            {
                spline = new PlotSeries()
                {
                    events = new PlotEvents()
                    {
                        mouseOver = "HandleChartMouseHover",
                        mouseOut = "HandleChartMouseOut"
                    },
                    marker = new PlotMarker()
                    {
                        enabled = false,
                        lineWidth = 0
                    }
                }
            };

            // show default tooltip format x / y values
            highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "", shared = true };

            // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
            highLineChartOutput.legend = new Legend() { enabled = false };

            // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
            highLineChartOutput.hChart = new HChart() { height = 100, width = 140, type = "spline" };

            // start to set series of data for medium chart (or multi line search request chart)
            List<Series> lstSeries = new List<Series>();


            // set list of data for each series 
            foreach (var dma in p_Dmas)
            {
                var reqDma = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.Query_Name, dma.id, true) == 0).FirstOrDefault();

                if (reqDma != null)
                {
                    // set sereies name as search request query name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = reqDma.Query_Name;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.Query_Name.ToString(), dma.id, true) == 0 && smr.DayDate.Month.Equals(rDate.Month) && smr.DayDate.Year.Equals(rDate.Year)).Sum(s => s.NoOfHits);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }
                else
                {
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = dma.id;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

            }

            // assign set of series data to medium chart (or multi line searchrequest chart)
            highLineChartOutput.series = lstSeries;
            string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
            return jsonResult;
        }

        public string GetHighChartsForMinutesOfAiringForDmaMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, List<DashboardDMAChartSelectionModel> p_Dmas)
        {
            var distinctDate = new List<DateTime>();


            for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
            {
                distinctDate.Add(dt);
            }


            List<string> categories = new List<string>();
            foreach (DateTime rDate in distinctDate)
            {
                categories.Add(rDate.ToShortDateString());
            }


            HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
            highLineChartOutput.title = new Title() { text = "", x = -20 };
            highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
            highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };
            /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
            /* 
                if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
            */
            /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
            highLineChartOutput.xAxis = new XAxis()
            {
                tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                tickmarkPlacement = "off",
                categories = categories,
                labels = new labels()
                {
                    enabled = false
                }
            };

            // set plot options and disable marker
            highLineChartOutput.plotOption = new PlotOptions()
            {
                spline = new PlotSeries()
                {
                    events = new PlotEvents()
                    {
                        mouseOver = "HandleChartMouseHover",
                        mouseOut = "HandleChartMouseOut"
                    },
                    marker = new PlotMarker()
                    {
                        enabled = false,
                        lineWidth = 0
                    }
                }
            };

            // show default tooltip format x / y values
            highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "", shared = true };

            // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
            highLineChartOutput.legend = new Legend() { enabled = false };

            // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
            highLineChartOutput.hChart = new HChart() { height = 100, width = 140, type = "spline" };

            // start to set series of data for medium chart (or multi line search request chart)
            List<Series> lstSeries = new List<Series>();


            // set list of data for each series 
            foreach (var dma in p_Dmas)
            {
                var reqDma = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.Query_Name, dma.id, true) == 0).FirstOrDefault();

                if (reqDma != null)
                {
                    // set sereies name as search request query name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = reqDma.Query_Name;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {

                        var daywiseCount = Convert.ToDecimal(p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.Query_Name.ToString(), dma.id, true) == 0 && smr.DayDate.Month.Equals(rDate.Month) && smr.DayDate.Year.Equals(rDate.Year)).Sum(s => s.NoOfDocs));
                        daywiseCount = Math.Round(Convert.ToDecimal(daywiseCount * 8) / 60, 2);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseCount != null ? daywiseCount : 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }
                else
                {
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = dma.id;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

            }

            // assign set of series data to medium chart (or multi line searchrequest chart)
            highLineChartOutput.series = lstSeries;
            string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
            return jsonResult;
        }

        public string GetHighChartsForViewsForDmaMonthly(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, DateTime p_FromDate, DateTime p_ToDate, List<DashboardDMAChartSelectionModel> p_Dmas)
        {
            var distinctDate = new List<DateTime>();


            for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
            {
                distinctDate.Add(dt);
            }


            List<string> categories = new List<string>();
            foreach (DateTime rDate in distinctDate)
            {
                categories.Add(rDate.ToShortDateString());
            }


            HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
            highLineChartOutput.title = new Title() { text = "", x = -20 };
            highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };
            highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };
            /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
            /* 
                if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
            */
            /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
            highLineChartOutput.xAxis = new XAxis()
            {
                tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                tickmarkPlacement = "off",
                categories = categories,
                labels = new labels()
                {
                    enabled = false
                }
            };

            // set plot options and disable marker
            highLineChartOutput.plotOption = new PlotOptions()
            {
                spline = new PlotSeries()
                {
                    events = new PlotEvents()
                    {
                        mouseOver = "HandleChartMouseHover",
                        mouseOut = "HandleChartMouseOut"
                    },
                    marker = new PlotMarker()
                    {
                        enabled = false,
                        lineWidth = 0
                    }
                }
            };

            // show default tooltip format x / y values
            highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "", shared = true };

            // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
            highLineChartOutput.legend = new Legend() { enabled = false };

            // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
            highLineChartOutput.hChart = new HChart() { height = 100, width = 140, type = "spline" };

            // start to set series of data for medium chart (or multi line search request chart)
            List<Series> lstSeries = new List<Series>();


            // set list of data for each series 
            foreach (var dma in p_Dmas)
            {
                var reqDma = p_lstIQAgent_DaySummaryModel.Where(a => string.Compare(a.Query_Name, dma.id, true) == 0).FirstOrDefault();

                if (reqDma != null)
                {
                    // set sereies name as search request query name, will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = reqDma.Query_Name;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        var daywiseSum = p_lstIQAgent_DaySummaryModel.Where(smr => String.Compare(smr.Query_Name.ToString(), dma.id, true) == 0 && smr.DayDate.Month.Equals(rDate.Month) && smr.DayDate.Year.Equals(rDate.Year)).Sum(s => s.Audience);
                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum != null ? daywiseSum : 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }
                else
                {
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = dma.id;
                    series.color = dma.clickColor;

                    foreach (DateTime rDate in distinctDate)
                    {
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = 0;
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

            }

            // assign set of series data to medium chart (or multi line searchrequest chart)
            highLineChartOutput.series = lstSeries;
            string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
            return jsonResult;
        }

        #endregion


        public void GetSentiment(List<IQAgent_DaySummaryModel> p_lstIQAgent_DaySummaryModel, out Int64 negativeSentiment,
                            out Int64 positiveSentiment)
        {

            try
            {
                negativeSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.NegativeSentiment);
                positiveSentiment = p_lstIQAgent_DaySummaryModel.Sum(s => s.PositiveSentiment);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        public Chart GetChartObject()
        {
            try
            {
                Chart chart = new Chart();
                chart.subcaption = "";
                chart.caption = string.Empty;// "Number of Hits";
                chart.linethickness = "1";
                chart.showvalues = "0";
                chart.formatnumberscale = "0";
                chart.anchorRadius = "3";
                chart.divlinealpha = "20";
                chart.divlinecolor = "000000";
                chart.divlineisdashed = "0";
                chart.showalternatehgridcolor = "1";
                chart.alternatehgridcolor = "FFFFFF";
                chart.shadowalpha = "40";
                chart.labelstep = "1";
                chart.numvdivlines = "0";
                chart.chartrightmargin = "10";
                chart.bgcolor = "FFFFFF";
                chart.bgangle = "270";
                chart.bgalpha = "10,10";
                chart.alternatehgridalpha = "5";
                chart.legendposition = "BOTTOM";
                chart.drawAnchors = "1";
                chart.showBorder = "0";
                chart.canvasBorderAlpha = "0";
                chart.showLabels = "0";
                //chart.lineColor = "#4493D6";

                return chart;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private string GetXmlOfMediaType(List<IQCommon.Model.IQ_MediaTypeModel> p_MediaTypeList)
        {
            XDocument doc = new XDocument(new XElement("list", from mt in p_MediaTypeList select new XElement("item", new XAttribute("SubMediaType", mt.SubMediaType), new XAttribute("HasAccess", mt.HasAccess), new XAttribute("MediaType", mt.MediaType), new XAttribute("TypeLevel", mt.TypeLevel))));

            return doc.ToString();
        }

        private string GetListSPNameOfMediaType(List<IQ_MediaTypeModel> p_MediaTypeList, string p_Medium)
        {
            var listSPName = "";

            if (!string.IsNullOrEmpty(p_Medium))
            {
                listSPName = p_MediaTypeList.Where(mt => string.Compare(mt.MediaType, p_Medium, true) == 0 && mt.TypeLevel == 1).Single().DashboardData.ListSPName;
            }

            return listSPName;
        }

        //public class SearchTermTotalRecords
        //{

        //    public DateTime GMT_DateTime { get; set; }
        //    public Int64 TotalRecords { get; set; }

        //}
    }
}
