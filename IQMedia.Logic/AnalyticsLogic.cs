using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using IQCommon.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;
using IQMedia.Shared.Utility;
using AnalyticsSearch;

namespace IQMedia.Web.Logic
{
    public class AnalyticsLogic: IQMedia.Web.Logic.Base.ILogic
    {
        #region DataAccess

        public List<AnalyticsSecondaryTable> GetSecondaryTables(SecondaryTabID tab, string pageType)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            List<AnalyticsSecondaryTable> listSecondaryTables = analyticsDA.GetSecondaryTables(tab, pageType);

            return listSecondaryTables;
        }

        public List<AnalyticsActiveElement> GetActiveElements()
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            List<AnalyticsActiveElement> listActiveElements = analyticsDA.GetActiveElements();

            return listActiveElements;
        }

        public AnalyticsDataModel GetHourSummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, bool inDefaultRange, bool loadEverything = true)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();

            AnalyticsDataModel dataModel = analyticsDA.GetHourSummaryData(clientGUID, searchRequestXml, subMediaType, gmtAdjustment, dstAdjustment, inDefaultRange, loadEverything);
            return dataModel;
        }

        public AnalyticsDataModel GetDaySummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, bool inDefaultRange, bool loadEverything = true)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();

            AnalyticsDataModel dataModel = analyticsDA.GetDaySummaryData(clientGUID, searchRequestXml, subMediaType, gmtAdjustment, dstAdjustment, loadEverything, inDefaultRange);
            return dataModel;
        }

        public AnalyticsDataModel GetMonthSummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();

            AnalyticsDataModel dataModel = analyticsDA.GetMonthSummaryData(clientGUID, searchRequestXml, subMediaType, gmtAdjustment, dstAdjustment);
            return dataModel;
        }

        public AnalyticsDataModel GetCampaignHourSummaryData(string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, bool loadEverything = true, string GroupByHeader = "")
        {
            AnalyticsDA analyticsDA = (AnalyticsDA)DataAccessFactory.GetDataAccess(DataAccessType.Analytics);
            return analyticsDA.GetHourSummaryDataForCampaign(searchRequestXml, subMediaType, gmtAdjustment, dstAdjustment, loadEverything, GroupByHeader);
        }

        public AnalyticsDataModel GetCampaignDaySummaryData(string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, bool loadEverything = true)
        {
            AnalyticsDA analyticsDA = (AnalyticsDA)DataAccessFactory.GetDataAccess(DataAccessType.Analytics);
            return analyticsDA.GetDaySummaryDataForCampaign(searchRequestXml, subMediaType, gmtAdjustment, dstAdjustment, loadEverything);
        }

        public AnalyticsDataModel GetNetworkShowSummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, List<string> lstTopTen, SecondaryTabID tab, string dateInterval)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();

            AnalyticsDataModel dataModel = analyticsDA.GetNetworkShowSummaryData(clientGUID, searchRequestXml, subMediaType, gmtAdjustment, dstAdjustment, lstTopTen, tab, dateInterval);
            return dataModel;
        }

        public List<string> GetTopNetworkShows(Guid clientGUID, string searchRequestXml, SecondaryTabID tab)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();

            List<string> topTen = analyticsDA.GetTopNetworkShows(clientGUID, searchRequestXml, tab);
            return topTen;
        }

        public List<AnalyticsCampaign> GetCampaigns(Guid clientGUID)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            List<AnalyticsCampaign> lstCampaigns = analyticsDA.GetCampaigns(clientGUID);

            return lstCampaigns;
        }

        public AnalyticsCampaign GetCampaignByID(Int64 campaignID)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            return analyticsDA.GetCampaignByID(campaignID);
        }

        public DateTime EditCampaign(Int64 campaignID, string campaignName, Int64? agentSRID, DateTime? startDate, DateTime? endDate, DateTime? startDateGMT, DateTime? endDateGMT)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            DateTime modifiedDate = analyticsDA.EditCampaign(campaignID, campaignName, agentSRID, startDate, endDate, startDateGMT, endDateGMT);
            return modifiedDate;
        }

        public Int64 CreateCampaign(string campaignName, int agentSRID, DateTime startDate, DateTime endDate, DateTime startDateGMT, DateTime endDateGMT)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            return analyticsDA.CreateCampaign(campaignName, agentSRID, startDate, endDate, startDateGMT, endDateGMT);
        }

        public void DeleteCampaign(Int64 campaignID)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            analyticsDA.DeleteCampaign(campaignID);
        }

        public Dictionary<string, string> GetAllDMAs()
        {
            AnalyticsDA analyticsDA = (AnalyticsDA)DataAccessFactory.GetDataAccess(DataAccessType.Analytics);
            return analyticsDA.GetAllDMAs();
        }

        public List<DayPartDataItem> GetDayPartData(string AffiliateCode)
        {
            AnalyticsDA analyticsDA = (AnalyticsDA)DataAccessFactory.GetDataAccess(DataAccessType.Analytics);
            return analyticsDA.GetDayPartData(AffiliateCode);
        }

        #endregion

        #region Utility

        private List<string> GetChartXAxisValues(AnalyticsRequest graphRequest, out List<DateTime> dateRange)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //Log4NetLogger.Debug("GetChartXAxisValues");
            Boolean isCampaign = graphRequest.PageType == "campaign";

            List<string> xAxisValues = new List<string>();
            dateRange = new List<DateTime>();
            DateTime startDate;
            DateTime endDate;
            KeyValuePair<long, TimeSpan> maxSpan;

            if (isCampaign)
            {
                // Only include campaigns requested, should always have requested campaigns or a specific request for a blank chart
                var requestedCampaigns = graphRequest.Campaigns.Where(w => graphRequest.RequestIDs.Contains(w.CampaignID)).ToList();

                Dictionary<long, TimeSpan> timeSpanList = requestedCampaigns.Select(campaign => new {
                    campaign.CampaignID,
                    timeSpan = campaign.EndDate.Subtract(campaign.StartDate)
                }).ToDictionary(
                    c => c.CampaignID,
                    c => c.timeSpan
                );

                maxSpan = timeSpanList.First(span => span.Value == timeSpanList.Values.Max());
                startDate = graphRequest.Campaigns.First(campaign => campaign.CampaignID == maxSpan.Key).StartDate;
                endDate = graphRequest.Campaigns.First(campaign => campaign.CampaignID == maxSpan.Key).EndDate;
            }
            else
            {
                // If on amplification, max span is taken from overall date range specified in request
                maxSpan = new KeyValuePair<long, TimeSpan>(-1, Convert.ToDateTime(graphRequest.DateTo).Subtract(Convert.ToDateTime(graphRequest.DateFrom)));
                startDate = Convert.ToDateTime(graphRequest.DateFrom);
                endDate = Convert.ToDateTime(graphRequest.DateTo);
            }

            switch (graphRequest.DateInterval)
            {
                case "hour":
                    for (int i = 0; i <= maxSpan.Value.TotalHours; i++)
                    {
                        dateRange.Add(startDate.AddHours(i));
                    }

                    if (!isCampaign)
                    {
                        xAxisValues = dateRange.Select(s => s.ToString()).ToList();
                    }
                    else
                    {
                        for (int i = 0; i < dateRange.Count; i++)
                        {
                            xAxisValues.Add(i.ToString());
                        }
                    }
                    break;
                case "day":
                    for (int i = 0; i <= maxSpan.Value.Days; i++)
                    {
                        dateRange.Add(startDate.AddDays(i));
                    }

                    if (!isCampaign)
                    {
                        xAxisValues = dateRange.Select(date => date.ToShortDateString()).ToList();
                    }
                    else
                    {
                        for (int i = 0; i < dateRange.Count; i++)
                        {
                            xAxisValues.Add(i.ToString());
                        }
                    }
                    break;
                case "month":
                    // Get number of months between start and end
                    var months = ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month;

                    for (int i = 0; i <= months; i++)
                    {
                        dateRange.Add(startDate.AddMonths(i));
                    }

                    if (!isCampaign)
                    {
                        xAxisValues = dateRange.Select(date => date.ToString("MMM - yyyy")).ToList();
                    }
                    else
                    {
                        for (int i = 0; i <= months; i++)
                        {
                            xAxisValues.Add(i.ToString());
                        }
                    }
                    break;
            }

            sw.Stop();
            //Log4NetLogger.Debug(string.Format("GetChartXAxisValues: {0} ms", sw.ElapsedMilliseconds));
            return xAxisValues;
        }

        private AnalyticsPESHFilters GetPESHFilters(List<string> PESHTypes, List<string> sourceGroups)
        {
            // Earned + Paid = All
            // Seen + Heard + Read = All

            AnalyticsPESHFilters filters = new AnalyticsPESHFilters();
            filters.isFiltering = (PESHTypes != null && PESHTypes.Count > 0) || (sourceGroups != null && sourceGroups.Count > 0);
            filters.isOnAir = false;
            filters.isOnline = false;
            filters.isPrint = false;

            filters.isSeenEarned = false;
            filters.isHeardEarned = false;

            filters.isSeenPaid = false;
            filters.isHeardPaid = false;

            if (PESHTypes != null && PESHTypes.Count > 0)
            {
                // Read is a unique filter. Within the Seen/Heard/Read group, if it is the only one selected then OnAir results should be excluded. 
                // But if Seen or Heard is also selected then OnAir results should be included. 
                // This is accomplished by filtering out the SeenPaid, SeenEarned, HeardPaid, and HeardEarned values when necessary, since those are 
                bool isRead = PESHTypes.Contains("Read");
                bool isSeen = PESHTypes.Contains("Seen");
                bool isHeard = PESHTypes.Contains("Heard");
                bool isPaid = PESHTypes.Contains("Paid");
                bool isEarned = PESHTypes.Contains("Earned");

                filters.isReadEarned = (isRead && isEarned) || (isRead && !isPaid) || (isEarned && !isHeard && !isSeen);

                // Seen Earned must be isSeen or earned and not heard
                filters.isSeenEarned = (isSeen && isEarned) || (isSeen && !isPaid) || (isEarned && !isHeard && !isRead);

                // heard earned must be isHeard or earned and not seen
                filters.isHeardEarned = (isHeard && isEarned) || (isHeard && !isPaid) || (isEarned && !isSeen && !isRead);

                // seen paid must be isSeen or paid and not heard
                filters.isSeenPaid = (isSeen && isPaid) || (isSeen && !isEarned) || (isPaid && !isHeard && !isRead);

                // heard paid must be isHeard or paid and not seen
                filters.isHeardPaid = (isHeard && isPaid) || (isHeard && !isEarned) || (isPaid && !isSeen && !isRead);

                /*
                filters.isRead = isRead;
                // isEarned if only picked earned PESH type - if isEarned & notRead & notSeen & notHeard => isEarned & isSeenEarned
                filters.isEarned = isEarned && !isSeen && !isHeard; // E = E & NOT S & NOT H -> NOT EARNED IF SEEN OR HEARD

                // Only fails when isEarned & isRead & notSeen & notHeard
                if (!(isEarned && isRead && !isSeen && !isHeard)) // => (NOT E & NOT R) OR (S OR H)
                {
                    // Seen Earned must be isSeen or earned and not heard
                    filters.isSeenEarned = (isSeen && isEarned) || (isSeen && !isPaid) || (isEarned && !isHeard);
                    // heard earned must be isHeard or earned and not seen
                    filters.isHeardEarned = (isHeard && isEarned) || (isHeard && !isPaid) || (isEarned && !isSeen);
                }

                if (!(isPaid && isRead && !isSeen && !isHeard)) // => (NOT P & NOT R) OR (S 0R H)
                {
                    // seen paid must be isSeen or paid and not heard
                    filters.isSeenPaid = (isSeen && isPaid) || (isSeen && !isEarned) || (isPaid && !isHeard);
                    // heard paid must be isHeard or paid and not seen
                    filters.isHeardPaid = (isHeard && isPaid) || (isHeard && !isEarned) || (isPaid && !isSeen);
                }*/
            }
            if (sourceGroups != null && sourceGroups.Count > 0)
            {
                filters.isOnAir = sourceGroups.Contains("OnAir");
                filters.isOnline = sourceGroups.Contains("Online");
                filters.isPrint = sourceGroups.Contains("Print");
            }

            return filters;
        }

        private decimal GetSumsFromSummaries(AnalyticsRequest request, List<IQ_MediaTypeModel> lstSubMediaTypes, AnalyticsPESHFilters peshFilters, IEnumerable<AnalyticsSummaryModel> summaries)
        {
            List<string> onAirSubMediaTypes = lstSubMediaTypes.Where(w => w.AnalyticsDataType.Equals("OnAir")).Select(s => s.SubMediaType).ToList();
            List<string> onlineSubMediaTypes = lstSubMediaTypes.Where(w => w.AnalyticsDataType.Equals("Online")).Select(s => s.SubMediaType).ToList();
            List<string> printSubMediaTypes = lstSubMediaTypes.Where(w => w.AnalyticsDataType.Equals("Print")).Select(s => s.SubMediaType).ToList();

            decimal returnSum = 0;

            if (request.Tab != SecondaryTabID.Overview)
            {
                // Neither a PESH type nor a source group was selected
                if (!peshFilters.isFiltering)
                {
                    returnSum = summaries.Sum(summary => summary.Number_Of_Hits);
                }
                else
                {
                    if (peshFilters.isOnAir || peshFilters.isOnline || peshFilters.isPrint)
                    {
                        var partialSummaries = new List<AnalyticsSummaryModel>();
                        if (peshFilters.isOnAir)
                        {
                            partialSummaries.AddRange(summaries.Where(w => onAirSubMediaTypes.Contains(w.SubMediaType)));
                        }
                        if (peshFilters.isOnline)
                        {
                            partialSummaries.AddRange(summaries.Where(w => onlineSubMediaTypes.Contains(w.SubMediaType)));
                        }
                        if (peshFilters.isPrint)
                        {
                            partialSummaries.AddRange(summaries.Where(w => printSubMediaTypes.Contains(w.SubMediaType)));
                        }

                        summaries = partialSummaries;
                    }

                    // If filtering to source groups but NOT to PESH types
                    if (request.PESHTypes == null || request.PESHTypes.Count == 0)
                    {
                        returnSum = summaries.Sum(s => s.Number_Of_Hits);
                    }

                    if (peshFilters.isSeenEarned)
                    {
                        returnSum += summaries.Sum(s => s.SeenEarned);
                    }
                    if (peshFilters.isSeenPaid)
                    {
                        returnSum += summaries.Sum(s => s.SeenPaid);
                    }
                    if (peshFilters.isHeardEarned)
                    {
                        returnSum += summaries.Sum(s => s.HeardEarned);
                    }
                    if (peshFilters.isHeardPaid)
                    {
                        returnSum += summaries.Sum(s => s.HeardPaid);
                    }
                    if (peshFilters.isReadEarned)
                    {
                        returnSum += summaries.Sum(s => s.ReadEarned);
                    }
                }
            }
            else
            {
                switch (request.SummationProperty)
                {
                    case SummationProperty.IQMediaValue:
                        returnSum = summaries.Sum(s => s.IQMediaValue);
                        break;
                    case SummationProperty.Docs:
                        returnSum = summaries.Sum(s => s.Number_Docs);
                        break;
                    case SummationProperty.Hits:
                        returnSum = summaries.Sum(s => s.Number_Of_Hits);
                        break;
                    case SummationProperty.AdSpend:
                        returnSum = summaries.Sum(s => s.IQMediaValue);
                        //returnSum = summaries.Sum(s => (s.IQMediaValue * (decimal)0.85) + ((s.SeenPaid / 30) * s.IQMediaValue * (decimal)1.15));
                        break;
                    default:
                        returnSum = summaries.Sum(s => s.Number_Of_Hits);
                        break;
                }
            }

            return returnSum;
        }

        private List<AnalyticsSummaryModel> GetSummariesForSources(List<IQ_MediaTypeModel> subMediaTypes, AnalyticsPESHFilters peshFilters, IEnumerable<AnalyticsSummaryModel> summaries)
        {
            try
            {
                List<string> onAirSubMediaTypes = subMediaTypes.Where(w => w.AnalyticsDataType.Equals("OnAir")).Select(s => s.SubMediaType).ToList();
                List<string> onlineSubMediaTypes = subMediaTypes.Where(w => w.AnalyticsDataType.Equals("Online")).Select(s => s.SubMediaType).ToList();
                List<string> printSubMediaTypes = subMediaTypes.Where(w => w.AnalyticsDataType.Equals("Print")).Select(s => s.SubMediaType).ToList();
                var partialSummaries = new List<AnalyticsSummaryModel>();
                if (peshFilters.isFiltering)
                {
                    if (peshFilters.isOnAir || peshFilters.isOnline || peshFilters.isPrint)
                    {
                        if (peshFilters.isOnAir)
                        {
                            partialSummaries.AddRange(summaries.Where(w => onAirSubMediaTypes.Contains(w.SubMediaType)));
                        }
                        if (peshFilters.isOnline)
                        {
                            partialSummaries.AddRange(summaries.Where(w => onlineSubMediaTypes.Contains(w.SubMediaType)));
                        }
                        if (peshFilters.isPrint)
                        {
                            partialSummaries.AddRange(summaries.Where(w => printSubMediaTypes.Contains(w.SubMediaType)));
                        }
                    }
                    else
                    {
                        partialSummaries.AddRange(summaries);
                    }
                }
                else
                {
                    partialSummaries.AddRange(summaries);
                }

                return partialSummaries;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new List<AnalyticsSummaryModel>();
            }
        }

        private List<AnalyticsSummaryModel> GetSummariesForPESHR(AnalyticsPESHFilters PESHR, IEnumerable<AnalyticsSummaryModel> summaries)
        {
            try
            {
                var partialSummaries = new List<AnalyticsSummaryModel>();

                foreach (var summary in summaries)
                {
                    if (PESHR.isFiltering)
                    {
                        bool seenEarned = PESHR.isSeenEarned && summary.SeenEarned > 0;
                        bool seenPaid = PESHR.isSeenPaid && summary.SeenPaid > 0;
                        bool heardEarned = PESHR.isHeardEarned && summary.HeardEarned > 0;
                        bool heardPaid = PESHR.isHeardPaid && summary.HeardPaid > 0;
                        bool readEarned = PESHR.isReadEarned && summary.ReadEarned > 0;

                        if (seenEarned || seenPaid || heardEarned || heardPaid || readEarned)
                        {
                            partialSummaries.Add(summary);
                        }
                        else if (!PESHR.isSeenEarned && !PESHR.isSeenPaid && !PESHR.isHeardEarned && !PESHR.isHeardPaid && !PESHR.isReadEarned)
                        {
                            partialSummaries.Add(summary);
                        }
                    }
                    else
                    {
                        partialSummaries.Add(summary);
                    }
                }

                return partialSummaries;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new List<AnalyticsSummaryModel>();
            }
        }

        #endregion

        #region ChartCreation

        public Dictionary<string, object> GetChart(AnalyticsRequest graphRequest, AnalyticsSecondaryTable table, AnalyticsDataModel analyticsData, List<AnalyticsGrouping> groupings, List<IQ_MediaTypeModel> subMediaTypes)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Dictionary<string, object> seriesAndChart = new Dictionary<string, object>();
                if (groupings.Count > 0)
                {
                    //Log4NetLogger.Debug(string.Format("Logic.GetChart"));
                    //Log4NetLogger.Debug(string.Format("graphParams - HCChartType: {0}, chartType: {1}", graphRequest.HCChartType, graphRequest.ChartType));

                    AnalyticsPESHFilters PESHFilters = GetPESHFilters(graphRequest.PESHTypes, graphRequest.SourceGroups);
                    switch (graphRequest.HCChartType)
                    {
                        case HCChartTypes.bar:
                        case HCChartTypes.column:
                            seriesAndChart = CreateBarOrColumnChart(groupings, graphRequest, subMediaTypes, PESHFilters);
                            break;
                        case HCChartTypes.spline:
                            seriesAndChart = CreateLineChart(groupings, graphRequest, subMediaTypes, PESHFilters);
                            break;
                        case HCChartTypes.pie:
                            seriesAndChart = CreatePieChart(groupings, graphRequest, subMediaTypes, PESHFilters);
                            break;
                        case HCChartTypes.heatmap:
                            seriesAndChart = CreateHeatMap(groupings, graphRequest, subMediaTypes, PESHFilters);
                            break;
                        case HCChartTypes.fusionMap:
                            seriesAndChart.Add("chart", CreateFusionMap(graphRequest, analyticsData, groupings, subMediaTypes, PESHFilters));
                            seriesAndChart.Add("series", null);
                            break;
                    }
                }
                else
                {
                    seriesAndChart.Add("chart", null);
                    seriesAndChart.Add("series", null);
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("Logic.GetChart: {0} ms", sw.ElapsedMilliseconds));
                return seriesAndChart;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new Dictionary<string, object>();
            }
        }

        private Dictionary<string, object> CreateBarOrColumnChart(List<AnalyticsGrouping> groupings, AnalyticsRequest request, List<IQ_MediaTypeModel> subMediaTypes, AnalyticsPESHFilters PESHFilters)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug("CreateBarOrColumnChart");
                //Log4NetLogger.Debug(request.ToString());
                List<Series> seriesList = new List<Series>();
                List<string> categoryList = new List<string>();
                // Flag - currently only active for demo tab (hence check)
                bool multiFilter = request.IsCompareMode && !request.Tab.Equals(SecondaryTabID.OverTime);
                bool isCamp = request.PageType.Equals("campaign");

                // IF filtering to multiple agents on demographics tab - need categories
                if (multiFilter)
                {
                    foreach (var req in request.RequestIDs)
                    {
                        if (isCamp)
                        {
                            categoryList.Add(request.Campaigns.First(f => f.CampaignID.Equals(req)).CampaignName);
                        }
                        else
                        {
                            categoryList.Add(request.Agents.First(f => f.Key.Equals(req)).Value);
                        }
                    }
                }

                HighColumnChartModel barOrColumn = new HighColumnChartModel() {
                    chart = new HChart() {
                        type = request.HCChartType.ToString()
                    },
                    title = new Title() {
                        text = string.Empty,
                        x = -20
                    },
                    subtitle = new Subtitle() {
                        text = string.Empty,
                        x = -20
                    },
                    legend = new Legend() {
                        align = "center",
                        borderWidth = "0",
                        width = 950,
                        enabled = false
                    },
                    xAxis = new XAxis() {
                        categories = categoryList,
                        tickWidth = 2,
                        tickInterval = 0,
                        tickmarkPlacement = "off",
                        labels = new labels() {
                            enabled = multiFilter
                        }
                    },
                    yAxis = new YAxis() {
                        title = new Title2() {
                            text = request.Tab == SecondaryTabID.Demographic ? "Audience" : "Occurrences",
                            rotation = request.HCChartType == HCChartTypes.bar ? 0 : 270
                        },
                        min = 0
                    },
                    tooltip = new Tooltip() {
                        formatter = "FormatBarColumnTooltip"
                    },
                    plotOptions = new PlotOptions() {
                        column = new Column() {
                            borderWidth = 0,
                            pointPadding = 0.2
                        },
                        series = new PlotSeries() {
                            cursor = "pointer",
                            point = new PlotPoint() {
                                events = new PlotEvents() {
                                    click = ""
                                }
                            }
                        }
                    },
                    series = new List<Series>()
                };

                if (request.Tab == SecondaryTabID.Demographic)
                {
                    List<AnalyticsSummaryModel> allSummaries = new List<AnalyticsSummaryModel>();
                    groupings.ForEach(e => {
                        allSummaries.AddRange(GetSummariesForSources(subMediaTypes, PESHFilters, e.Summaries));
                    });

                    if (request.SubTab == "gender")
                    {
                        Series maleSeries = new Series() {
                            data = new List<HighChartDatum>(),
                            name = "male"
                        };

                        Series femaleSeries = new Series() {
                            data = new List<HighChartDatum>(),
                            name = "female"
                        };

                        if (multiFilter)
                        {
                            foreach (var reqID in request.RequestIDs)
                            {
                                var agSumms = allSummaries.Where(w => Convert.ToInt64(reqID).Equals(request.PageType.Equals("campaign") ? w.CampaignID : w.SearchRequestID));
                                agSumms = GetSummariesForPESHR(PESHFilters, agSumms);

                                HighChartDatum maleHCD = new HighChartDatum() {
                                    y = agSumms.Any() ? agSumms.Sum(s => s.MaleAudience) : 0,
                                    Value = "male",
                                    SearchTerm = reqID.ToString()
                                };
                                maleSeries.data.Add(maleHCD);

                                HighChartDatum femaleHCD = new HighChartDatum() {
                                    y = agSumms.Any() ? agSumms.Sum(s => s.FemaleAudience) : 0,
                                    Value = "female",
                                    SearchTerm = reqID.ToString()
                                };
                                femaleSeries.data.Add(femaleHCD);
                            }
                        }
                        else
                        {
                            var agSumms = GetSummariesForPESHR(PESHFilters, allSummaries);

                            HighChartDatum maleDatum = new HighChartDatum() {
                                y = agSumms.Sum(s => s.MaleAudience),
                                Value = "male"
                            };
                            maleSeries.data.Add(maleDatum);

                            HighChartDatum femaleDatum = new HighChartDatum() {
                                y = agSumms.Sum(s => s.FemaleAudience),
                                Value = "female"
                            };
                            femaleSeries.data.Add(femaleDatum);
                        }

                        seriesList.Add(maleSeries);
                        barOrColumn.series.Add(maleSeries);

                        seriesList.Add(femaleSeries);
                        barOrColumn.series.Add(femaleSeries);
                    }
                    else
                    {
                        Series ar18_24 = new Series() {
                            name = "18-24",
                            data = new List<HighChartDatum>()
                        };
                        Series ar25_34 = new Series() {
                            name = "25-34",
                            data = new List<HighChartDatum>()
                        };
                        Series ar35_49 = new Series() {
                            name = "35-49",
                            data = new List<HighChartDatum>()
                        };
                        Series ar50_54 = new Series() {
                            name = "50-54",
                            data = new List<HighChartDatum>()
                        };
                        Series ar55_64 = new Series() {
                            name = "55-64",
                            data = new List<HighChartDatum>()
                        };
                        Series ar65_Plus = new Series() {
                            name = "65+",
                            data = new List<HighChartDatum>()
                        };

                        if (multiFilter)
                        {
                            foreach (var reqID in request.RequestIDs)
                            {
                                var agSumms = allSummaries.Where(w => Convert.ToInt64(reqID).Equals(request.PageType.Equals("campaign") ? w.CampaignID : w.SearchRequestID));
                                agSumms = GetSummariesForPESHR(PESHFilters, agSumms);

                                ar18_24.data.Add(new HighChartDatum() {
                                    Value = "18-24",
                                    y = agSumms.Any() ? agSumms.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24) : 0,
                                    SearchTerm = reqID.ToString()
                                });
                                ar25_34.data.Add(new HighChartDatum() {
                                    Value = "25-34",
                                    y = agSumms.Any() ? agSumms.Sum(s => s.AM25_34 + s.AF25_34) : 0,
                                    SearchTerm = reqID.ToString()
                                });
                                ar35_49.data.Add(new HighChartDatum() {
                                    Value = "35-49",
                                    y = agSumms.Any() ? agSumms.Sum(s => s.AM35_49 + s.AF35_49) : 0,
                                    SearchTerm = reqID.ToString()
                                });
                                ar50_54.data.Add(new HighChartDatum() {
                                    Value = "50-54",
                                    y = agSumms.Any() ? agSumms.Sum(s => s.AM50_54 + s.AF50_54) : 0,
                                    SearchTerm = reqID.ToString()
                                });
                                ar55_64.data.Add(new HighChartDatum() {
                                    Value = "55-64",
                                    y = agSumms.Any() ? agSumms.Sum(s => s.AM55_64 + s.AF55_64) : 0,
                                    SearchTerm = reqID.ToString()
                                });
                                ar65_Plus.data.Add(new HighChartDatum() {
                                    Value = "65+",
                                    y = agSumms.Any() ? agSumms.Sum(s => s.AM65_Plus + s.AF65_Plus) : 0,
                                    SearchTerm = reqID.ToString()
                                });
                            }
                        }
                        else
                        {
                            var agSumms = GetSummariesForPESHR(PESHFilters, allSummaries);

                            ar18_24.data.Add(new HighChartDatum() {
                                Value = "18-24",
                                y = agSumms.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24)
                            });
                            ar25_34.data.Add(new HighChartDatum() {
                                Value = "25-34",
                                y = agSumms.Sum(s => s.AM25_34 + s.AF25_34)
                            });
                            ar35_49.data.Add(new HighChartDatum() {
                                Value = "35-49",
                                y = agSumms.Sum(s => s.AM35_49 + s.AF35_49)
                            });
                            ar50_54.data.Add(new HighChartDatum() {
                                Value = "50-54",
                                y = agSumms.Sum(s => s.AM50_54 + s.AF50_54)
                            });
                            ar55_64.data.Add(new HighChartDatum() {
                                Value = "55-64",
                                y = agSumms.Sum(s => s.AM55_64 + s.AF55_64)
                            });
                            ar65_Plus.data.Add(new HighChartDatum() {
                                Value = "65+",
                                y = agSumms.Sum(s => s.AM65_Plus + s.AF65_Plus)
                            });
                        }

                        seriesList.Add(ar18_24);
                        seriesList.Add(ar25_34);
                        seriesList.Add(ar35_49);
                        seriesList.Add(ar50_54);
                        seriesList.Add(ar55_64);
                        seriesList.Add(ar65_Plus);

                        seriesList.ForEach(e => {
                            barOrColumn.series.Add(e);
                        });
                    }
                }
                else
                {
                    int count = 0;
                    foreach (var group in groupings.OrderByDescending(g => g.Summaries.Sum(s => s.Number_Of_Hits)))
                    {
                        Series groupSeries = new Series() {
                            data = new List<HighChartDatum>(),
                            name = group.Name
                        };

                        // A list of groupings to loop through and create data points from - if multiFilter want to create data points from agent
                        // sub-group summaries, if not then use main group summaries
                        List<AnalyticsGrouping> dataGroupings = multiFilter ? group.AgentSubGroupings : new List<AnalyticsGrouping>() { group };
                        foreach (var dg in dataGroupings)
                        {
                            HighChartDatum groupDatum = new HighChartDatum() {
                                y = dg.Summaries.Any() ? GetSumsFromSummaries(request, subMediaTypes, PESHFilters, dg.Summaries) : 0,
                                SearchTerm = group.Name,
                                Value = dg.ID
                            };

                            groupSeries.data.Add(groupDatum);
                        }
                        seriesList.Add(groupSeries);

                        if (request.Tab == SecondaryTabID.OverTime)
                        {
                            if (request.RequestIDs.Any(id => id.ToString().Equals(group.ID)))
                            {
                                barOrColumn.series.Add(groupSeries);
                            }
                        }
                        else
                        {
                            if (count < 10)
                            {
                                barOrColumn.series.Add(groupSeries);
                            }
                            count += 1;
                        }
                    }
                }

                Dictionary<string, object> seriesAndChart = new Dictionary<string, object>();
                seriesAndChart.Add("series", seriesList);
                seriesAndChart.Add("chart", CommonFunctions.SearializeJson(barOrColumn));

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateBarOrColumnChart: {0} ms", sw.ElapsedMilliseconds));

                return seriesAndChart;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new Dictionary<string, object>();
            }
        }

        private Dictionary<string, object> CreateLineChart(List<AnalyticsGrouping> groupings, AnalyticsRequest request, List<IQ_MediaTypeModel> subMediaTypes, AnalyticsPESHFilters PESHFilters)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //Log4NetLogger.Debug("CreateLineChart");
                List<DateTime> distinctDates = new List<DateTime>();
                List<string> xAxisValues = GetChartXAxisValues(request, out distinctDates);
                List<Series> seriesList = new List<Series>();
                bool multiFilter = request.IsCompareMode && !request.Tab.Equals(SecondaryTabID.OverTime);
                List<string> dashStyles = new List<string>() {
                    "Solid", "ShortDash", "ShortDot", "ShortDashDot", "ShortDashDotDot", "Dot", "Dash", "LongDash", "DashDot", "LongDashDot", "LongDashDotDot"
                };
                List<string> colors = new List<string>() {
                    "#598ea2", "#f3b350", "#c7d36a", "#b4b4da", "#d8635d", "#f3da72", "#9ad1dc", "#e1cba4", "#ff9bb8", "#808285", "#da3ab3", "#6ecdb2", "#e2cc00", "#ff6c36", "#3b5cad", "#9778d3", "#00bfd6", "#5b2c3f", "#4c8b2b", "#d9a460"
                };

                string chartTitle;
                if (request.ChartType == ChartType.Growth)
                {
                    chartTitle = "% Growth";
                }
                else
                {
                    if (request.Tab == SecondaryTabID.Demographic)
                    {
                        chartTitle = "Audience";
                    }
                    else if (request.Tab == SecondaryTabID.Overview)
                    {
                        //chartTitle = request.SummationProperty.Equals(SummationProperty.AdSpend) ? "Ad Spend" : "Airings";
                        chartTitle = "";
                    }
                    else
                    {
                        chartTitle = "Occurrences";
                    }
                }

                HighLineChartOutput lineChart = new HighLineChartOutput() {
                    hChart = new HChart() {
                        type = "spline"
                    },
                    title = new Title() {
                        text = string.Empty,
                        x = -20
                    },
                    subtitle = new Subtitle() {
                        text = string.Empty,
                        x = -20
                    },
                    legend = new Legend() {
                        align = "right",
                        borderWidth = "0",
                        enabled = true,
                        verticalAlign = "top",
                        layout = "vertical",
                        x = -90
                    },
                    xAxis = new XAxis() {
                        tickmarkPlacement = "off",
                        tickWidth = 2,
                        labels = new labels() {
                            staggerLines = request.DateInterval == "hour" ? 2 : 0
                        },
                        title = new Title2() {
                            rotation = 0,
                            text = request.PageType == "campaign" ? "Campaign Day" : (request.Tab == SecondaryTabID.Overview ? "" : "Date")
                        },
                        categories = xAxisValues,
                        tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(xAxisValues.Count()) / 7))
                    },
                    yAxis = new List<YAxis>() {
                        new YAxis() {
                            title = new Title2() {
                                text = chartTitle
                            }
                        }
                    },
                    tooltip = new Tooltip() {
                        formatter = "FormatSplineTooltip"
                    },
                    plotOption = new PlotOptions() {
                        series = new PlotSeries() {
                            cursor = "pointer",
                            point = new PlotPoint() {
                                events = new PlotEvents() {
                                    click = ""
                                }
                            }
                        },
                        spline = new PlotSeries() {
                            marker = new PlotMarker() {
                                enabled = true
                            }
                        }
                    },
                    series = new List<Series>()
                };

                if (request.Tab == SecondaryTabID.Demographic)
                {
                    int count = 0;
                    foreach (var group in groupings)
                    {
                        if (request.SubTab == "gender")
                        {
                            Series maleSeries = new Series() {
                                data = new List<HighChartDatum>(),
                                name = string.Format("{0}{1}male", group.Name, string.IsNullOrEmpty(group.Name) ? "" : " "),
                                color = colors[0],
                                dashStyle = dashStyles[count % dashStyles.Count]
                            };
                            Series femaleSeries = new Series() {
                                data = new List<HighChartDatum>(),
                                name = string.Format("{0}{1}female", group.Name, string.IsNullOrEmpty(group.Name) ? "" : " "),
                                color = colors[1],
                                dashStyle = dashStyles[count % dashStyles.Count]
                            };

                            long malePrevSum = 0;
                            long femalePrevSum = 0;

                            if (request.PageType == "campaign")
                            {
                                foreach (var step in xAxisValues)
                                {
                                    var summariesForOffset = group.Summaries.Where(w => w.CampaignOffset.Equals(Convert.ToInt64(step))).ToList();
                                    summariesForOffset = GetSummariesForSources(subMediaTypes, PESHFilters, summariesForOffset);
                                    summariesForOffset = GetSummariesForPESHR(PESHFilters, summariesForOffset);

                                    long maleSum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.MaleAudience) : 0;
                                    long femaleSum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.FemaleAudience) : 0;
                                    decimal malePctChange = 0;
                                    decimal femalePctChange = 0;

                                    if (malePrevSum != 0)
                                    {
                                        malePctChange = ((maleSum - malePrevSum) / (decimal)malePrevSum) * 100;
                                    }
                                    malePrevSum = maleSum;
                                    if (femalePrevSum != 0)
                                    {
                                        femalePctChange = ((femaleSum - femalePrevSum) / (decimal)femalePrevSum) * 100;
                                    }
                                    femalePrevSum = femaleSum;

                                    HighChartDatum maleDatum = new HighChartDatum() {
                                        Value = "male",
                                        SearchName = "male",
                                        y = request.ChartType == ChartType.Growth ? malePctChange : maleSum
                                    };
                                    HighChartDatum femaleDatum = new HighChartDatum() {
                                        Value = "female",
                                        SearchName = "female",
                                        y = request.ChartType == ChartType.Growth ? femalePctChange : femaleSum
                                    };

                                    if (multiFilter)
                                    {
                                        var campStart = request.Campaigns.Where(w => group.ID.Equals(w.CampaignID.ToString())).DefaultIfEmpty(null).FirstOrDefault().StartDate;
                                        var currentDate = campStart;
                                        string dateStr = "";
                                        switch(request.DateInterval)
                                        {
                                            case "hour":
                                                currentDate = currentDate.AddHours(Convert.ToInt32(step));
                                                dateStr = currentDate.ToString("M/d/yyyy hh:00 tt");
                                                break;
                                            case "day":
                                                currentDate = currentDate.AddDays(Convert.ToInt32(step));
                                                dateStr = currentDate.ToShortDateString();
                                                break;
                                            case "month":
                                                currentDate = currentDate.AddMonths(Convert.ToInt32(step));
                                                dateStr = currentDate.ToShortDateString();
                                                break;
                                        }

                                        maleDatum.Date = femaleDatum.Date = dateStr;
                                    }

                                    maleSeries.data.Add(maleDatum);
                                    femaleSeries.data.Add(femaleDatum);
                                }
                            }
                            else
                            {
                                foreach (var date in distinctDates)
                                {
                                    var summariesForDate = group.Summaries.Where(w => w.SummaryDateTime.Equals(date));
                                    summariesForDate = GetSummariesForSources(subMediaTypes, PESHFilters, summariesForDate);
                                    summariesForDate = GetSummariesForPESHR(PESHFilters, summariesForDate);

                                    long maleSum = summariesForDate.Any() ? summariesForDate.Sum(s => s.MaleAudience) : 0;
                                    long femaleSum = summariesForDate.Any() ? summariesForDate.Sum(s => s.FemaleAudience) : 0;
                                    decimal malePctChange = 0;
                                    decimal femalePctChange = 0;

                                    if (malePrevSum != 0)
                                    {
                                        malePctChange = ((maleSum - malePrevSum) / (decimal)malePrevSum) * 100;
                                    }
                                    malePrevSum = maleSum;
                                    if (femalePrevSum != 0)
                                    {
                                        femalePctChange = ((femaleSum - femalePrevSum) / (decimal)femalePrevSum) * 100;
                                    }
                                    femalePrevSum = femaleSum;

                                    string dateStr = "";
                                    switch(request.DateInterval)
                                    {
                                        case "hour":
                                            dateStr = date.ToString("M/d/yyyy hh:00 tt");
                                            break;
                                        case "day":
                                            dateStr = date.ToShortDateString();
                                            break;
                                        case "month":
                                            dateStr = date.ToShortDateString();
                                            break;
                                    }

                                    HighChartDatum maleDatum = new HighChartDatum() {
                                        Date = dateStr,
                                        Value = string.Format("male{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID),
                                        SearchName = "male",
                                        y = request.ChartType == ChartType.Growth ? malePctChange : maleSum
                                    };
                                    maleSeries.data.Add(maleDatum);
                                    HighChartDatum femaleDatum = new HighChartDatum() {
                                        Date = dateStr,
                                        Value = string.Format("female{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID),
                                        SearchName = "female",
                                        y = request.ChartType == ChartType.Growth ? femalePctChange : femaleSum
                                    };
                                    femaleSeries.data.Add(femaleDatum);
                                }
                            }

                            seriesList.Add(maleSeries);
                            seriesList.Add(femaleSeries);
                            lineChart.series.Add(maleSeries);
                            lineChart.series.Add(femaleSeries);
                        }
                        else
                        {
                            Series s18_24 = new Series() {
                                data = new List<HighChartDatum>(),
                                name = string.Format("18-24{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                color = colors[0],
                                dashStyle = dashStyles[count % dashStyles.Count]
                            };
                            long s18_24PrevSum = 0;

                            Series s25_34 = new Series() {
                                data = new List<HighChartDatum>(),
                                name = string.Format("25-34{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                color = colors[1],
                                dashStyle = dashStyles[count % dashStyles.Count]
                            };
                            long s25_34PrevSum = 0;

                            Series s35_49 = new Series() {
                                data = new List<HighChartDatum>(),
                                name = string.Format("35-49{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                color = colors[2],
                                dashStyle = dashStyles[count % dashStyles.Count]
                            };
                            long s35_49PrevSum = 0;

                            Series s50_54 = new Series() {
                                data = new List<HighChartDatum>(),
                                name = string.Format("50-54{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                color = colors[3],
                                dashStyle = dashStyles[count % dashStyles.Count]
                            };
                            long s50_54PrevSum = 0;

                            Series s55_64 = new Series() {
                                data = new List<HighChartDatum>(),
                                name = string.Format("55-64{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                color = colors[4],
                                dashStyle = dashStyles[count % dashStyles.Count]
                            };
                            long s55_64PrevSum = 0;

                            Series s65_Plus = new Series() {
                                data = new List<HighChartDatum>(),
                                name = string.Format("65+{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                color = colors[5],
                                dashStyle = dashStyles[count % dashStyles.Count]
                            };
                            long s65_PlusPrevSum = 0;

                            if (request.PageType == "campaign")
                            {
                                foreach (var step in xAxisValues)
                                {
                                    var summariesForOffset = group.Summaries.Where(w => w.CampaignOffset.Equals(Convert.ToInt64(step))).ToList();
                                    summariesForOffset = GetSummariesForSources(subMediaTypes, PESHFilters, summariesForOffset);
                                    summariesForOffset = GetSummariesForPESHR(PESHFilters, summariesForOffset);

                                    long s18_24Sum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24) : 0;
                                    decimal s18_24PctChange = 0;
                                    long s25_34Sum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.AM25_34 + s.AF25_34) : 0;
                                    decimal s25_34PctChange = 0;
                                    long s35_49Sum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.AM35_49 + s.AF35_49) : 0;
                                    decimal s35_49PctChange = 0;
                                    long s50_54Sum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.AM50_54 + s.AF50_54) : 0;
                                    decimal s50_54PctChange = 0;
                                    long s55_64Sum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.AM55_64 + s.AF55_64) : 0;
                                    decimal s55_64PctChange = 0;
                                    long s65_PlusSum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.AM65_Plus + s.AF65_Plus) : 0;
                                    decimal s65_PlusPctChange = 0;

                                    if (s18_24PrevSum != 0)
                                    {
                                        s18_24PctChange = ((s18_24Sum - s18_24PrevSum) / (decimal)s18_24PrevSum) * 100;
                                    }
                                    s18_24PrevSum = s18_24Sum;

                                    if (s25_34PrevSum != 0)
                                    {
                                        s25_34PctChange = ((s25_34Sum - s25_34PrevSum) / (decimal)s25_34PrevSum) * 100;
                                    }
                                    s25_34PrevSum = s25_34Sum;

                                    if (s35_49PrevSum != 0)
                                    {
                                        s35_49PctChange = ((s35_49Sum - s35_49PrevSum) / (decimal)s35_49PrevSum) * 100;
                                    }
                                    s35_49PrevSum = s35_49Sum;

                                    if (s50_54PrevSum != 0)
                                    {
                                        s50_54PctChange = ((s50_54Sum - s50_54PrevSum) / (decimal)s50_54PrevSum) * 100;
                                    }
                                    s50_54PrevSum = s50_54Sum;

                                    if (s55_64PrevSum != 0)
                                    {
                                        s55_64PctChange = ((s55_64Sum - s55_64PrevSum) / (decimal)s55_64PrevSum) * 100;
                                    }
                                    s55_64PrevSum = s55_64Sum;

                                    if (s65_PlusPrevSum != 0)
                                    {
                                        s65_PlusPctChange = ((s65_PlusSum - s65_PlusPrevSum) / (decimal)s65_PlusPrevSum) * 100;
                                    }
                                    s65_PlusPrevSum = s65_PlusSum;

                                    HighChartDatum s18_24HCD = new HighChartDatum() {
                                        Value = "18-24",
                                        SearchName = s18_24.name,
                                        y = request.ChartType == ChartType.Growth ? s18_24PctChange : s18_24Sum
                                    };

                                    HighChartDatum s25_34HCD = new HighChartDatum() {
                                        Value = "25-34",
                                        SearchName = s25_34.name,
                                        y = request.ChartType == ChartType.Growth ? s25_34PctChange : s25_34Sum
                                    };

                                    HighChartDatum s35_49HCD = new HighChartDatum() {
                                        Value = "35-49",
                                        SearchName = s35_49.name,
                                        y = request.ChartType == ChartType.Growth ? s35_49PctChange : s35_49Sum
                                    };

                                    HighChartDatum s50_54HCD = new HighChartDatum() {
                                        Value = "50-54",
                                        SearchName = s50_54.name,
                                        y = request.ChartType == ChartType.Growth ? s50_54PctChange : s50_54Sum
                                    };

                                    HighChartDatum s55_64HCD = new HighChartDatum() {
                                        Value = "55-64",
                                        SearchName = s55_64.name,
                                        y = request.ChartType == ChartType.Growth ? s55_64PctChange : s55_64Sum
                                    };

                                    HighChartDatum s65_PlusHCD = new HighChartDatum() {
                                        Value = "65+",
                                        SearchName = s65_Plus.name,
                                        y = request.ChartType == ChartType.Growth ? s65_PlusPctChange : s65_PlusSum
                                    };

                                    if (multiFilter)
                                    {
                                        var campStart = request.Campaigns.Where(w => group.ID.Equals(w.CampaignID.ToString())).DefaultIfEmpty(null).FirstOrDefault().StartDate;
                                        var currentDate = campStart;
                                        string dateStr = "";
                                        switch(request.DateInterval)
                                        {
                                            case "hour":
                                                currentDate = currentDate.AddHours(Convert.ToInt32(step));
                                                dateStr = currentDate.ToString("M/d/yyyy hh:00 tt");
                                                break;
                                            case "day":
                                                currentDate = currentDate.AddDays(Convert.ToInt32(step));
                                                dateStr = currentDate.ToShortDateString();
                                                break;
                                            case "month":
                                                currentDate = currentDate.AddMonths(Convert.ToInt32(step));
                                                dateStr = currentDate.ToShortDateString();
                                                break;
                                        }

                                        s18_24HCD.Date = s25_34HCD.Date = s35_49HCD.Date = s50_54HCD.Date = s55_64HCD.Date = s65_PlusHCD.Date = dateStr;
                                    }

                                    s18_24.data.Add(s18_24HCD);
                                    s25_34.data.Add(s25_34HCD);
                                    s35_49.data.Add(s35_49HCD);
                                    s50_54.data.Add(s50_54HCD);
                                    s55_64.data.Add(s55_64HCD);
                                    s65_Plus.data.Add(s65_PlusHCD);
                                }
                            }
                            else
                            {
                                foreach (var date in distinctDates)
                                {
                                    var summariesForDate = group.Summaries.Where(w => w.SummaryDateTime.Equals(date));
                                    summariesForDate = GetSummariesForSources(subMediaTypes, PESHFilters, summariesForDate);
                                    summariesForDate = GetSummariesForPESHR(PESHFilters, summariesForDate);

                                    long s18_24Sum = summariesForDate.Any() ? summariesForDate.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24) : 0;
                                    decimal s18_24PctChange = 0;
                                    long s25_34Sum = summariesForDate.Any() ? summariesForDate.Sum(s => s.AM25_34 + s.AF25_34) : 0;
                                    decimal s25_34PctChange = 0;
                                    long s35_49Sum = summariesForDate.Any() ? summariesForDate.Sum(s => s.AM35_49 + s.AF35_49) : 0;
                                    decimal s35_49PctChange = 0;
                                    long s50_54Sum = summariesForDate.Any() ? summariesForDate.Sum(s => s.AM50_54 + s.AF50_54) : 0;
                                    decimal s50_54PctChange = 0;
                                    long s55_64Sum = summariesForDate.Any() ? summariesForDate.Sum(s => s.AM55_64 + s.AF55_64) : 0;
                                    decimal s55_64PctChange = 0;
                                    long s65_PlusSum = summariesForDate.Any() ? summariesForDate.Sum(s => s.AM65_Plus + s.AF65_Plus) : 0;
                                    decimal s65_PlusPctChange = 0;

                                    if (s18_24PrevSum != 0)
                                    {
                                        s18_24PctChange = ((s18_24Sum - s18_24PrevSum) / (decimal)s18_24PrevSum) * 100;
                                    }
                                    s18_24PrevSum = s18_24Sum;

                                    if (s25_34PrevSum != 0)
                                    {
                                        s25_34PctChange = ((s25_34Sum - s25_34PrevSum) / (decimal)s25_34PrevSum) * 100;
                                    }
                                    s25_34PrevSum = s25_34Sum;

                                    if (s35_49PrevSum != 0)
                                    {
                                        s35_49PctChange = ((s35_49Sum - s35_49PrevSum) / (decimal)s35_49PrevSum) * 100;
                                    }
                                    s35_49PrevSum = s35_49Sum;

                                    if (s50_54PrevSum != 0)
                                    {
                                        s50_54PctChange = ((s50_54Sum - s50_54PrevSum) / (decimal)s50_54PrevSum) * 100;
                                    }
                                    s50_54PrevSum = s50_54Sum;

                                    if (s55_64PrevSum != 0)
                                    {
                                        s55_64PctChange = ((s55_64Sum - s55_64PrevSum) / (decimal)s55_64PrevSum) * 100;
                                    }
                                    s55_64PrevSum = s55_64Sum;

                                    if (s65_PlusPrevSum != 0)
                                    {
                                        s65_PlusPctChange = ((s65_PlusSum - s65_PlusPrevSum) / (decimal)s65_PlusPrevSum) * 100;
                                    }
                                    s65_PlusPrevSum = s65_PlusSum;

                                    string dateStr = "";
                                    switch (request.DateInterval)
                                    {
                                        case "hour":
                                            dateStr = date.ToString("M/d/yyyy hh:00 tt");
                                            break;
                                        case "day":
                                            dateStr = date.ToShortDateString();
                                            break;
                                        case "month":
                                            dateStr = date.ToShortDateString();
                                            break;
                                    }

                                    HighChartDatum s18_24HCD = new HighChartDatum() {
                                        Date = dateStr,
                                        Value = string.Format("18-24{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID),
                                        SearchName = s18_24.name,
                                        y = request.ChartType == ChartType.Growth ? s18_24PctChange : s18_24Sum
                                    };
                                    s18_24.data.Add(s18_24HCD);

                                    HighChartDatum s25_34HCD = new HighChartDatum() {
                                        Date = dateStr,
                                        Value = string.Format("25-34{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID),
                                        SearchName = s25_34.name,
                                        y = request.ChartType == ChartType.Growth ? s25_34PctChange : s25_34Sum
                                    };
                                    s25_34.data.Add(s25_34HCD);

                                    HighChartDatum s35_49HCD = new HighChartDatum() {
                                        Date = dateStr,
                                        Value = string.Format("35-49{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID),
                                        SearchName = s35_49.name,
                                        y = request.ChartType == ChartType.Growth ? s35_49PctChange : s35_49Sum
                                    };
                                    s35_49.data.Add(s35_49HCD);

                                    HighChartDatum s50_54HCD = new HighChartDatum() {
                                        Date = dateStr,
                                        Value = string.Format("50-54{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID),
                                        SearchName = s50_54.name,
                                        y = request.ChartType == ChartType.Growth ? s50_54PctChange : s50_54Sum
                                    };
                                    s50_54.data.Add(s50_54HCD);

                                    HighChartDatum s55_64HCD = new HighChartDatum() {
                                        Date = dateStr,
                                        Value = string.Format("55-64{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID),
                                        SearchName = s55_64.name,
                                        y = request.ChartType == ChartType.Growth ? s55_64PctChange : s55_64Sum
                                    };
                                    s55_64.data.Add(s55_64HCD);

                                    HighChartDatum s65_PlusHCD = new HighChartDatum() {
                                        Date = dateStr,
                                        Value = string.Format("65+{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID),
                                        SearchName = s65_Plus.name,
                                        y = request.ChartType == ChartType.Growth ? s65_PlusPctChange : s65_PlusSum
                                    };
                                    s65_Plus.data.Add(s65_PlusHCD);
                                }
                            }

                            seriesList.Add(s18_24);
                            lineChart.series.Add(s18_24);

                            seriesList.Add(s25_34);
                            lineChart.series.Add(s25_34);

                            seriesList.Add(s35_49);
                            lineChart.series.Add(s35_49);

                            seriesList.Add(s50_54);
                            lineChart.series.Add(s50_54);

                            seriesList.Add(s55_64);
                            lineChart.series.Add(s55_64);

                            seriesList.Add(s65_Plus);
                            lineChart.series.Add(s65_Plus);
                        }

                        count++;
                    }
                }
                else
                {
                    int count = 0;
                    List<AnalyticsGrouping> seriesGroupings = new List<AnalyticsGrouping>();
                    Dictionary<string, string> colorGroups = new Dictionary<string, string>();
                    Dictionary<string, string> dashGroups = new Dictionary<string, string>();

                    for (int i = 0; i < request.RequestIDs.Count; i++)
                    {
                        dashGroups.Add(request.RequestIDs[i].ToString(), dashStyles[i % dashStyles.Count]);
                    }

                    if (multiFilter)
                    {
                        groupings = groupings.OrderByDescending(ob => ob.Summaries.Sum(s => s.Number_Of_Hits)).ToList();
                        // Make a single list of subgroups from all major groupings
                        // add color to dictionary that maps main groups
                        for (int i = 0; i < groupings.Count; i++)
                        {
                            seriesGroupings.AddRange(groupings[i].AgentSubGroupings);
                            colorGroups.Add(groupings[i].ID, colors[i % colors.Count]);
                        }
                    }
                    else
                    {
                        seriesGroupings = groupings;
                    }

                    foreach (var group in seriesGroupings.OrderByDescending(ob => ob.Summaries.Sum(s => s.Number_Of_Hits)))
                    {
                        Series groupSeries = new Series() {
                            data = new List<HighChartDatum>(),
                            name = group.Name,
                            tooltip = new Tooltip() {
                                formatter = ""
                            }
                        };

                        // If multi-filter change color to be same across groupings of same "main" (i.e. all groups of the same source)
                        // Change dash style to be the same across agent sub-groups
                        if (multiFilter)
                        {
                            // Split group ID mainGroupID_agentID into constituent parts - color off of mainGroup and dash off of agent
                            string[] splitIDs = group.ID.Split('_');
                            groupSeries.color = colorGroups.First(w => splitIDs[0].Equals(w.Key)).Value;
                            groupSeries.dashStyle = dashGroups.First(w => splitIDs[1].Equals(w.Key)).Value;
                        }

                        decimal prevSum = 0;

                        if (request.PageType == "campaign")
                        {
                            DateTime campaignStart = new DateTime();
                            DateTime campaignEnd = new DateTime();
                            if (request.Tab == SecondaryTabID.OverTime || multiFilter)
                            {
                                string gID = group.ID;
                                if (multiFilter)
                                {
                                    gID = group.ID.Split('_')[1];
                                }
                                var campaign = request.Campaigns.Where(w => gID.Equals(w.CampaignID.ToString())).DefaultIfEmpty(null).FirstOrDefault();
                                campaignStart = campaign == null ? new DateTime() : campaign.StartDate;
                                campaignEnd = campaign == null ? new DateTime() : campaign.EndDate;
                            }
                            foreach (var step in xAxisValues)
                            {
                                var summariesForOffset = group.Summaries.Where(w => w.CampaignOffset.Equals(Convert.ToInt64(step))).ToList();
                                decimal? offsetSum = summariesForOffset.Any() ? GetSumsFromSummaries(request, subMediaTypes, PESHFilters, summariesForOffset) : 0;
                                decimal pctChange = 0;
                                if (prevSum != 0)
                                {
                                    pctChange = (((long)offsetSum - prevSum) / (decimal)prevSum) * 100;
                                }
                                prevSum = (long)offsetSum;
                                var offset = Convert.ToInt32(step);

                                var dateString = string.Empty;
                                if (request.Tab == SecondaryTabID.OverTime || multiFilter)
                                {
                                    switch(request.DateInterval)
                                    {
                                        case "hour":
                                            dateString = campaignStart.AddHours(offset).ToString("d/MM/yyyy hh:00 tt");
                                            if (campaignStart.AddHours(offset).CompareTo(campaignEnd) > 0)
                                            {
                                                offsetSum = null;
                                            }
                                            break;
                                        case "day":
                                            dateString = campaignStart.AddDays(offset).ToShortDateString();
                                            if (campaignStart.AddDays(offset).CompareTo(campaignEnd) > 0)
                                            {
                                                offsetSum = null;
                                            }
                                            break;
                                        case "month":
                                            dateString = campaignStart.AddMonths(offset).ToShortDateString();
                                            if (campaignStart.AddMonths(offset).CompareTo(campaignEnd) > 0)
                                            {
                                                offsetSum = null;
                                            }
                                            break;
                                    }
                                }

                                // Date is only applicable to add into tooltip when on OverTime tab or MultiFilter
                                HighChartDatum hcd = new HighChartDatum() {
                                    Date = dateString,
                                    Value = group.ID,
                                    SearchName = group.Name,
                                    SearchTerm = group.Name,
                                    y = request.ChartType == ChartType.Growth ? pctChange : offsetSum
                                };

                                groupSeries.data.Add(hcd);
                            }
                        }
                        else
                        {
                            foreach (var date in distinctDates)
                            {
                                var summariesForDate = group.Summaries.Where(w => w.SummaryDateTime.Equals(date));

                                decimal daySum = summariesForDate.Any() ? GetSumsFromSummaries(request, subMediaTypes, PESHFilters, summariesForDate) : 0;
                                decimal pctChange = 0;
                                if (prevSum != 0)
                                {
                                    pctChange = ((daySum - prevSum) / (decimal)prevSum) * 100;
                                }
                                prevSum = daySum;

                                string dateStr = "";
                                switch (request.DateInterval)
                                {
                                    case "hour":
                                        dateStr = date.ToString("M/d/yyyy hh:00 tt");
                                        break;
                                    case "day":
                                        dateStr = date.ToShortDateString();
                                        break;
                                    case "month":
                                        dateStr = date.ToShortDateString();
                                        break;
                                }

                                HighChartDatum hcd = new HighChartDatum() {
                                    Date = dateStr,
                                    Type = "Media",
                                    Value = group.ID,
                                    SearchName = group.Name,
                                    SearchTerm = group.Name,
                                    y = request.ChartType == ChartType.Growth ? pctChange : daySum
                                };

                                groupSeries.data.Add(hcd);
                            }
                        }

                        seriesList.Add(groupSeries);
                        if (request.Tab == SecondaryTabID.OverTime)
                        {
                            // Add only series from agentList
                            if (request.RequestIDs.Any(ID => ID.ToString().Equals(group.ID)))
                            {
                                lineChart.series.Add(groupSeries);
                            }
                        }
                        else if (multiFilter)
                        {
                            lineChart.series.Add(groupSeries);
                            // Check how many distinct "main" series in line chart series
                            var lcMainSeriesIDs = lineChart.series.Select(s => s.data.First().Value.Split('_')[0]).ToList();
                            if (lcMainSeriesIDs.Distinct().Count() > 10)
                            {
                                // If adding series to chart will add 
                                lineChart.series.Remove(groupSeries);
                            }
                        }
                        else
                        {
                            if (request.Tab == SecondaryTabID.Overview)
                            {
                                lineChart.series.Add(groupSeries);
                            }
                            else if (count < 10)
                            {
                                lineChart.series.Add(groupSeries);
                            }
                            count += 1;
                        }
                    }
                }

                Dictionary<string, object> seriesAndChart = new Dictionary<string,object>();
                seriesAndChart.Add("series", seriesList);
                seriesAndChart.Add("chart", CommonFunctions.SearializeJson(lineChart));
                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateLineChart: {0} ms", sw.ElapsedMilliseconds));
                return seriesAndChart;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new Dictionary<string, object>();
            }
        }

        private Dictionary<string, object> CreatePieChart(List<AnalyticsGrouping> groupings, AnalyticsRequest request, List<IQ_MediaTypeModel> subMediaTypes, AnalyticsPESHFilters PESHFilters)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //Log4NetLogger.Debug("CreatePieChart");
                //Log4NetLogger.Debug(string.Format("groupings.Count: {0}", groupings.Count));
                // Series list holds all series - used to pass back series
                List<object> seriesList = new List<object>();
                // Slice list holds all pie "series" each of which is a single pie slice
                List<object> sliceList = new List<object>();
                List<object> secondarySlices = new List<object>();
                bool isCamp = request.PageType.Equals("campaign");

                bool multiFilter = request.IsCompareMode && !request.Tab.Equals(SecondaryTabID.OverTime);

                // Pie Slices have properties in this order
                // Name
                // Value
                // ID

                string chartTitle;
                if (request.Tab.Equals(SecondaryTabID.Demographic))
                {
                    chartTitle = "Audience";
                }
                else if (request.Tab.Equals(SecondaryTabID.Overview))
                {
                    chartTitle = "";
                }
                else
                {
                    chartTitle = "Occurrences";
                }

                HighPieChartModel pieChart = new HighPieChartModel() {
                    chart = new PChart(),
                    title = new PTitle() {
                        text =  chartTitle,
                        style = new HStyle() {
                            color = "null",
                            fontFamily = "Open Sans",
                            fontSize = "null",
                            fontWeight = "null"
                        }
                    },
                    plotOptions = new PPlotOptions() {
                        pie = new Pie() {
                            allowPointSelect = true
                            ,cursor = "pointer"
                            ,showInLegend = request.PageType.Equals("dashboard") ? true : false
                            ,innerSize = "60%"
                            ,dataLabels = new DataLabels() {
                                enabled = request.PageType.Equals("dashboard") ? false : false
                            }
                        }
                    },
                    legend = new Legend() {
                        align = request.Tab.Equals(SecondaryTabID.Overview) ? "left" : "center",
                        borderWidth = "0",
                        width = 500,
                        enabled = request.Tab.Equals(SecondaryTabID.Overview) ? true : false
                    },
                    tooltip = new PTooltip(),
                    series = new List<PSeries>()
                };

                if (request.Tab == SecondaryTabID.Demographic)
                {
                    // Will make a series for each group, used in compare to have each agent be a "series"
                    foreach (var group in groupings)
                    {
                        PSeries groupSeries = new PSeries() {
                            type = "pie",
                            name = group.Name,
                            data = new List<object>()
                        };

                        List<AnalyticsSummaryModel> agSumms = new List<AnalyticsSummaryModel>();
                        if (!request.PageType.Equals("dashboard"))
                        {
                            agSumms = GetSummariesForPESHR(PESHFilters, group.Summaries);
                        }
                        else
                        {
                            agSumms = group.Summaries;
                        }

                        if (request.SubTab == "gender")
                        {
                            var maleSlice = new object[] {
                                string.Format("male{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                Convert.ToInt64(agSumms.Sum(s => s.MaleAudience)),
                                string.Format("male{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID)
                            };
                            groupSeries.data.Add(maleSlice);
                            seriesList.Add(maleSlice);

                            var femaleSlice = new object[] {
                                string.Format("female{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                Convert.ToInt64(agSumms.Sum(s => s.FemaleAudience)),
                                string.Format("female{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID)
                            };
                            groupSeries.data.Add(femaleSlice);
                            seriesList.Add(femaleSlice);
                        }
                        else
                        {
                            var s18_24 = new object[] {
                                string.Format("18-24{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                Convert.ToInt64(agSumms.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24)),
                                string.Format("18-24{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID)
                            };
                            groupSeries.data.Add(s18_24);
                            seriesList.Add(s18_24);

                            var s25_34 = new object[] {
                                string.Format("25-34{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                Convert.ToInt64(agSumms.Sum(s => s.AM25_34 + s.AF25_34)),
                                string.Format("25-34{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID)
                            };
                            groupSeries.data.Add(s25_34);
                            seriesList.Add(s25_34);

                            var s35_49 = new object[] {
                                string.Format("35-49{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                Convert.ToInt64(agSumms.Sum(s => s.AM35_49 + s.AF35_49)),
                                string.Format("35-49{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID)
                            };
                            groupSeries.data.Add(s35_49);
                            seriesList.Add(s35_49);

                            var s50_54 = new object[] {
                                string.Format("50-45{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                Convert.ToInt64(agSumms.Sum(s => s.AM50_54 + s.AF50_54)),
                                string.Format("50-54{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID)
                            };
                            groupSeries.data.Add(s50_54);
                            seriesList.Add(s50_54);

                            var s55_64 = new object[] {
                                string.Format("55-64{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                Convert.ToInt64(agSumms.Sum(s => s.AM55_64 + s.AF55_64)),
                                string.Format("55-64{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID)
                            };
                            groupSeries.data.Add(s55_64);
                            seriesList.Add(s55_64);

                            var s65_Plus = new object[] {
                                string.Format("65+{0}{1}", string.IsNullOrEmpty(group.Name) ? "" : " ", group.Name),
                                Convert.ToInt64(agSumms.Sum(s => s.AM65_Plus + s.AF65_Plus)),
                                string.Format("65+{0}{1}", string.IsNullOrEmpty(group.ID) ? "" : "_", group.ID)
                            };
                            groupSeries.data.Add(s65_Plus);
                            seriesList.Add(s65_Plus);
                        }
                        pieChart.series.Add(groupSeries);
                    }

                    // Code to construct pie chart in different fashion
                    //foreach (var group in groupings)
                    //{
                    //    if (request.SubTab == "gender")
                    //    {
                    //        var maleSeries = new object[] {
                    //            "male",
                    //            Convert.ToInt64(group.Summaries.Sum(s => s.MaleAudience)),
                    //            "male"
                    //        };
                    //        seriesList.Add(maleSeries);
                    //        sliceList.Add(maleSeries);

                    //            var femaleSeries = new object[] {
                    //            "female",
                    //            Convert.ToInt64(group.Summaries.Sum(s => s.FemaleAudience)),
                    //            "female"
                    //        };
                    //        seriesList.Add(femaleSeries);
                    //        sliceList.Add(femaleSeries);

                    //        if (multiFilter)
                    //        {
                    //            List<object> maleList = new List<object>();
                    //            List<object> femaleList = new List<object>();
                    //            foreach (var subGroup in group.AgentSubGroupings)
                    //            {
                    //                maleList.Add(new object[] {
                    //                    string.Format("male {0}", subGroup.Name),
                    //                    Convert.ToInt64(subGroup.Summaries.Sum(s => s.MaleAudience)),
                    //                    string.Format("male_{0}", subGroup.ID)
                    //                });
                    //                femaleList.Add(new object[]{
                    //                    string.Format("female {0}", subGroup.Name),
                    //                    Convert.ToInt64(subGroup.Summaries.Sum(s => s.FemaleAudience)),
                    //                    string.Format("female_{0}", subGroup.ID)
                    //                });
                    //            }
                    //            seriesList.AddRange(maleList);
                    //            seriesList.AddRange(femaleList);
                    //            secondarySlices.AddRange(maleList);
                    //            secondarySlices.AddRange(femaleList);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        var s18_24 = new object[] {
                    //            "18-24",
                    //            Convert.ToInt64(group.Summaries.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24)),
                    //            "18-24"
                    //        };
                    //        seriesList.Add(s18_24);
                    //        sliceList.Add(s18_24);

                    //        var s25_34 = new object[] {
                    //            "25-34",
                    //            Convert.ToInt64(group.Summaries.Sum(s => s.AM25_34 + s.AF25_34)),
                    //            "25-34"
                    //        };
                    //        seriesList.Add(s25_34);
                    //        sliceList.Add(s25_34);

                    //        var s35_49 = new object[] {
                    //            "35-49",
                    //            Convert.ToInt64(group.Summaries.Sum(s => s.AM35_49 + s.AF35_49)),
                    //            "35-49"
                    //        };
                    //        seriesList.Add(s35_49);
                    //        sliceList.Add(s35_49);

                    //        var s50_54 = new object[] {
                    //            "50-54",
                    //            Convert.ToInt64(group.Summaries.Sum(s => s.AM50_54 + s.AF50_54)),
                    //            "50-54"
                    //        };
                    //        seriesList.Add(s50_54);
                    //        sliceList.Add(s50_54);

                    //        var s55_64 = new object[] {
                    //            "55-64",
                    //            Convert.ToInt64(group.Summaries.Sum(s => s.AM55_64 + s.AF55_64)),
                    //            "55-64"
                    //        };
                    //        seriesList.Add(s55_64);
                    //        sliceList.Add(s55_64);

                    //        var s65_Plus = new object[] {
                    //            "65+",
                    //            Convert.ToInt64(group.Summaries.Sum(s => s.AM65_Plus + s.AF65_Plus)),
                    //            "65+"
                    //        };
                    //        seriesList.Add(s65_Plus);
                    //        sliceList.Add(s65_Plus);

                    //        if (multiFilter)
                    //        {
                    //            List<object> s18_24List = new List<object>();
                    //            List<object> s25_34List = new List<object>();
                    //            List<object> s35_49List = new List<object>();
                    //            List<object> s50_54List = new List<object>();
                    //            List<object> s55_64List = new List<object>();
                    //            List<object> s65_PlusList = new List<object>();
                    //            foreach (var subGroup in group.AgentSubGroupings)
                    //            {
                    //                s18_24List.Add(new object[] {
                    //                    string.Format("18-24 {0}", subGroup.Name),
                    //                    Convert.ToInt64(subGroup.Summaries.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24)),
                    //                    string.Format("18-24_{0}", subGroup.ID)
                    //                });
                    //                s25_34List.Add(new object[] {
                    //                    string.Format("25-34 {0}", subGroup.Name),
                    //                    Convert.ToInt64(subGroup.Summaries.Sum(s => s.AM25_34 + s.AF25_34)),
                    //                    string.Format("25-34_{0}", subGroup.ID)
                    //                });
                    //                s35_49List.Add(new object[] {
                    //                    string.Format("35-49 {0}", subGroup.Name),
                    //                    Convert.ToInt64(subGroup.Summaries.Sum(s => s.AM35_49 + s.AF35_49)),
                    //                    string.Format("35-49_{0}", subGroup.ID)
                    //                });
                    //                s50_54List.Add(new object[] {
                    //                    string.Format("50-54 {0}", subGroup.Name),
                    //                    Convert.ToInt64(subGroup.Summaries.Sum(s => s.AM50_54 + s.AF50_54)),
                    //                    string.Format("50-54_{0}", subGroup.ID)
                    //                });
                    //                s55_64List.Add(new object[] {
                    //                    string.Format("55-64 {0}", subGroup.Name),
                    //                    Convert.ToInt64(subGroup.Summaries.Sum(s => s.AM55_64 + s.AF55_64)),
                    //                    string.Format("55-64_{0}", subGroup.ID)
                    //                });
                    //                s65_PlusList.Add(new object[] {
                    //                    string.Format("65+ {0}", subGroup.Name),
                    //                    Convert.ToInt64(subGroup.Summaries.Sum(s => s.AM65_Plus + s.AF65_Plus)),
                    //                    string.Format("65+_{0}", subGroup.ID)
                    //                });
                    //            }

                    //            seriesList.AddRange(s18_24List);
                    //            secondarySlices.AddRange(s18_24List);
                    //            seriesList.AddRange(s25_34List);
                    //            secondarySlices.AddRange(s25_34List);
                    //            seriesList.AddRange(s35_49List);
                    //            secondarySlices.AddRange(s35_49List);
                    //            seriesList.AddRange(s50_54List);
                    //            secondarySlices.AddRange(s50_54List);
                    //            seriesList.AddRange(s55_64List);
                    //            secondarySlices.AddRange(s55_64List);
                    //            seriesList.AddRange(s65_PlusList);
                    //            secondarySlices.AddRange(s65_PlusList);
                    //        }
                    //    }
                    //}

                }
                else
                {
                    if (multiFilter)
                    {
                        Dictionary<long, PSeries> psList = new Dictionary<long, PSeries>();
                        foreach (var id in request.RequestIDs)
                        {
                            psList.Add(id, new PSeries() {
                                type = "pie",
                                name = id.ToString(),
                                data = new List<object>()
                            });
                        }

                        int count = 0;
                        foreach (var group in groupings.OrderByDescending(ob => ob.Summaries.Sum(s => s.Number_Of_Hits)))
                        {
                            foreach (var subGroup in group.AgentSubGroupings)
                            {
                                long agID = Convert.ToInt64(subGroup.ID.Split('_')[1]);
                                if (request.Tab.Equals(SecondaryTabID.Daytime))
                                {
                                    agID = Convert.ToInt64(subGroup.ID.Split('_')[2]);
                                }
                                PSeries ps = psList[agID];
                                var subSlice = new object[] {
                                    subGroup.Name,
                                    GetSumsFromSummaries(request, subMediaTypes, PESHFilters, subGroup.Summaries),
                                    subGroup.ID
                                };

                                if (count < 10)
                                {
                                    ps.data.Add(subSlice);
                                }
                                seriesList.Add(subSlice);
                            }
                            count++;
                        }

                        foreach (var series in psList)
                        {
                            pieChart.series.Add(series.Value);
                        }
                    }
                    else
                    {
                        int count = 0;
                        foreach (var group in groupings.OrderByDescending(ob => ob.Summaries.Sum(s => s.Number_Of_Hits)))
                        {
                            var slice = new object[] {
                                group.Name,
                                group.Summaries.Any() ? GetSumsFromSummaries(request, subMediaTypes, PESHFilters, group.Summaries) : 0,
                                group.ID
                            };

                            seriesList.Add(slice);

                            if (multiFilter)
                            {
                                foreach (var subGroup in group.AgentSubGroupings)
                                {
                                    var secondarySlice = new object[] {
                                        subGroup.Name,
                                        subGroup.Summaries.Any() ? GetSumsFromSummaries(request, subMediaTypes, PESHFilters, subGroup.Summaries) : 0,
                                        string.Format("{0}_{1}", group.ID, subGroup.ID)
                                    };
                                    seriesList.Add(secondarySlice);

                                    if (count < 10)
                                    {
                                        secondarySlices.Add(secondarySlice);
                                    }
                                }
                            }

                            if (request.Tab == SecondaryTabID.OverTime)
                            {
                                if (request.RequestIDs.Any(id => id.ToString().Equals(group.ID)))
                                {
                                    sliceList.Add(slice);
                                }
                            }
                            else
                            {
                                if (request.Tab == SecondaryTabID.Overview)
                                {
                                    sliceList.Add(slice);
                                }
                                else if (count < 10)
                                {
                                    sliceList.Add(slice);
                                }
                                count += 1;
                            }
                        }
                        pieChart.series.Add(new PSeries() {
                            type = "pie",
                            name = request.Tab == SecondaryTabID.Demographic ? "Audience" : "Occurrences",
                            data = sliceList
                        });
                    }
                }

                //pieChart.series.Add(new PSeries() {
                //    type = "pie",
                //    name = request.Tab == SecondaryTabID.Demographic ? "Audience" : "Occurrences",
                //    data = sliceList
                //});

                //if (multiFilter)
                //{
                //    pieChart.series.Add(new PSeries() {
                //        type = "pie",
                //        name = "SubGroups",
                //        data = secondarySlices
                //    });
                //}

                Dictionary<string, object> seriesAndChart = new Dictionary<string, object>();
                seriesAndChart.Add("series", seriesList);
                seriesAndChart.Add("chart", CommonFunctions.SearializeJson(pieChart));

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreatePieChart: {0} ms", sw.ElapsedMilliseconds));

                return seriesAndChart;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new Dictionary<string, object>();
            }
        }

        private Dictionary<string, object> CreateHeatMap(List<AnalyticsGrouping> groupings, AnalyticsRequest request, List<IQ_MediaTypeModel> subMediaTypes, AnalyticsPESHFilters PESHFilters)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //Log4NetLogger.Debug("CreateHeatMap");

                List<DayOfWeek> daysUnordered = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();
                var days = daysUnordered.OrderBy(d => d == DayOfWeek.Sunday).ThenBy(d => d).Reverse().ToList();
                List<int> hours = Enumerable.Range(0, 24).ToList();
                List<string> axisLabels = new List<string>();
                for (int x = 0; x <= 7; x++)
                {
                    if (x < 2)
                    {
                        axisLabels.Add(days[x].ToString());
                    }
                    else if (x == 2)
                    {
                        axisLabels.Add("");
                    }
                    else
                    {
                        axisLabels.Add(days[x-1].ToString());
                    }
                }

                HighHeatMapModel heatMap = new HighHeatMapModel() {
                    hChart = new HChart() {
                        type = "heatmap"
                    },
                    title = new Title() {
                        text = "Occurrences"
                    },
                    legend = new HeatMapLegend() {
                        align = "right",
                        verticalAlign = "top",
                        layout = "vertical",
                        y = 25,
                        symbolHeight = 300
                    },
                    xAxis = new XAxis() {
                        categories = hours.Select(s => (s > 12 ? s - 12 : (s == 0 ? 12 : s)).ToString("D2") + ":00 " + (s >= 12 ? "PM" : "AM")).ToList(),
                        tickmarkPlacement = "between",
                        tickWidth = 1,
                        labels = new labels() {
                            rotation = 315
                        }
                    },
                    yAxis = new HeatMapYAxis() {
                        title = new Title2() {
                            text = null
                        },
                        categories = request.Tab == SecondaryTabID.Daytime ? days.Select(s => s.ToString()).ToList() : axisLabels
                    },
                    tooltip = new Tooltip() {
                        formatter = request.Tab == SecondaryTabID.Daytime ? "FormatDaytimeTooltip" : "FormatDaypartTooltip"
                    },
                    colorAxis = new ColorAxis() {
                        min = 0,
                        minColor = "#ffffff",
                        maxColor = "#598ea2"
                    }
                };

                HeatMapSeries series = new HeatMapSeries() {
                    name = "Occurrences",
                    data = new List<HeatMapDatum>()
                };

                if (request.Tab == SecondaryTabID.Daytime)
                {
                    foreach (var group in groupings.OrderByDescending(ob => ob.Summaries.Sum(s => s.Number_Of_Hits)))
                    {
                        // Daytime will have both day and hour in its ID
                        var yIndex = days.FindIndex(i => group.ID.Split('_').ElementAt(0).Equals(i.ToString()));
                        var xIndex = hours.FindIndex(i => group.ID.Split('_').ElementAt(1).Equals(i.ToString()));

                        HeatMapDatum hcd = new HeatMapDatum() {
                            value = group.Summaries.Any() ? (long)GetSumsFromSummaries(request, subMediaTypes, PESHFilters, group.Summaries) : 0,
                            //value = group.Summaries.Any() ? group.Summaries.Sum(s => s.Number_Docs) : 0,
                            borderColor = "#cccccc",
                            borderWidth = 1,
                            x = xIndex,
                            y = yIndex,
                            name = group.Name,
                            code = group.ID
                        };

                        series.data.Add(hcd);
                    }
                }
                else
                {
                    series.dataLabels = new HeatMapDataLabels() {
                        enabled = true,
                        color = "#636F72",
                        formatter = "FormatDaypartDataLabel",
                        style = new HeatStyle() {
                            fontSize = "12px",
                            fontWeight = "normal",
                            textShadow = "0 0 10px white"
                        }
                    };

                    //Get Day Part Data
                    List<DayPartDataItem> dayPartData = GetDayPartData("A");

                    List<AnalyticsSummaryModel> allSummaries = new List<AnalyticsSummaryModel>();
                    groupings.ForEach(e => {
                        allSummaries.AddRange(e.Summaries);
                    });

                    foreach (var hour in hours)
                    {
                        int count = 0;

                        foreach (var day in days)
                        {
                            if (count == 2)
                            {
                                count += 1;
                            }

                            //var summsForPart = allSummaries.Where(w => request.Tab.Equals(SecondaryTabID.Daytime) ? (w.SummaryDateTime.ToUniversalTime().DayOfWeek.Equals(day) && w.SummaryDateTime.ToUniversalTime().Hour.Equals(hour)) : (w.SummaryDateTime.DayOfWeek.Equals(day) && w.SummaryDateTime.Hour.Equals(hour)));
                            // Daypart based on Local Times
                            var summsForPart = allSummaries.Where(w => w.LocalDateTime.DayOfWeek.Equals(day) && w.LocalDateTime.Hour.Equals(hour));
                            
                            DayPartDataItem dayPart = dayPartData.Any() ? dayPartData.First(dp => dp.DayOfWeek.Equals(day) && dp.HourOfDay.Equals(hour)) : new DayPartDataItem();

                            HeatMapDatum hcd = new HeatMapDatum() {
                                value = summsForPart.Any() ? (long)GetSumsFromSummaries(request, subMediaTypes, PESHFilters, summsForPart) : 0,
                                //value = summsForPart.Any() ? summsForPart.Sum(s => s.Number_Docs) : 0,
                                x = hours.IndexOf(hour),
                                y = count,
                                name = dayPart.DayPartName,
                                code = dayPart.DayPartCode
                            };

                            series.data.Add(hcd);
                            count += 1;
                        }

                        series.data.Insert(2, new HeatMapDatum() {
                            x = hours.IndexOf(hour),
                            y = 2,
                            value = 0,
                            name = string.Empty,
                            code = string.Empty
                        });
                    }
                }

                //Log4NetLogger.Debug(string.Format(" {0,-12} | {1,12} ", "ID", "Sum"));
                //Log4NetLogger.Debug(string.Format("--------------+--------------"));
                // Heat maps do not have toggle-able series so no need to limit # of "series"


                heatMap.series = new List<HeatMapSeries>() { series };
                Dictionary<string, object> seriesAndChart = new Dictionary<string, object>();
                seriesAndChart.Add("series", series.data);
                seriesAndChart.Add("chart", CommonFunctions.SearializeJson(heatMap));

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateHeatMap: {0} ms", sw.ElapsedMilliseconds));

                return seriesAndChart;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new Dictionary<string, object>();
            }
        }

        public string CreateGoogleOverlay(List<GoogleSummaryModel> googleSummaries, AnalyticsRequest request)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug("CreateGoogleOverlay");

                List<DateTime> distinctDates = new List<DateTime>();
                List<string> xAxisValues = GetChartXAxisValues(request, out distinctDates);

                HighLineChartOutput chartOutput = new HighLineChartOutput() {
                    xAxis = new XAxis() {
                        tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDates.Count()) / 7)),
                        categories = xAxisValues,
                        labels = new labels() {
                            enabled = false
                        }
                    },
                    hChart = new HChart() {
                        height = 100,
                        width = 120,
                        type = "spline"
                    },
                    tooltip = new Tooltip() {
                        formatter = ""
                    },
                    plotOption = new PlotOptions() {
                        spline = new PlotSeries() {
                            marker = new PlotMarker() {
                                enabled = false,
                                lineWidth = 0
                            }
                        }
                    },
                    series = new List<Series>()
                };

                List<string> dataTypes = googleSummaries.Select(s => s.DataType).Distinct().ToList();

                foreach (var dataType in dataTypes)
                {
                    Series dataTypeSeries = new Series() {
                        name = dataType,
                        data = new List<HighChartDatum>()
                    };

                    decimal prevDaySum = 0;
                    foreach (var date in distinctDates)
                    {
                        var summariesForDate = googleSummaries.Where(w => w.DataType.Equals(dataType) && w.DayDate.Equals(date));
                        decimal daySum = summariesForDate.Any() ? summariesForDate.Sum(s => s.NoOfDocs) : 0;
                        decimal pctChange = 0;

                        if (prevDaySum != 0)
                        {
                            pctChange = ((daySum - prevDaySum) / prevDaySum) * 100;
                        }
                        prevDaySum = daySum;

                        HighChartDatum hcd = new HighChartDatum() {
                            Date = date.ToShortDateString(),
                            SearchName = dataType,
                            y = request.ChartType == ChartType.Growth ? pctChange : daySum
                        };

                        dataTypeSeries.data.Add(hcd);
                    }

                    chartOutput.series.Add(dataTypeSeries);
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateGoogleOverlay: {0}", sw.ElapsedMilliseconds));

                return CommonFunctions.SearializeJson(chartOutput);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return string.Empty;
            }
        }

        public string CreateOverlay(List<AnalyticsGrouping> groupings, AnalyticsRequest request, int overlayType, List<IQ_MediaTypeModel> subMediaTypes)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug(string.Format("CreateOverlay"));

                List<DateTime> distinctDates = new List<DateTime>();
                List<string> xAxisValues = GetChartXAxisValues(request, out distinctDates);
                AnalyticsPESHFilters PESHFilters = GetPESHFilters(request.PESHTypes, request.SourceGroups);

                HighLineChartOutput chartOutput = new HighLineChartOutput() {
                    xAxis = new XAxis() {
                        tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDates.Count()) / 7)),
                        categories = xAxisValues,
                        labels = new labels() {
                            enabled = false
                        }
                    },
                    hChart = new HChart() {
                        height = 100,
                        width = 120,
                        type = "spline"
                    },
                    tooltip = new Tooltip() {
                        formatter = "FormatSplineTooltip"
                    },
                    plotOption = new PlotOptions() {
                        spline = new PlotSeries() {
                            marker = new PlotMarker() {
                                enabled = false,
                                lineWidth = 0
                            }
                        }
                    },
                    series = new List<Series>()
                };

                foreach (var group in groupings.OrderByDescending(ob => ob.Summaries.Sum(s => s.Number_Of_Hits)))
                {
                    Series groupSeries = new Series() {
                        name = group.Name + (overlayType == 2 ? " (Audience)" : " (Ad Value)"),
                        data = new List<HighChartDatum>()
                    };

                    decimal prevSum = 0;

                    if (request.PageType == "campaign")
                    {
                        DateTime campaignStart = new DateTime();
                        DateTime campaignEnd = new DateTime();
                        if (request.Tab == SecondaryTabID.OverTime)
                        {
                            campaignStart = request.Campaigns.First(c => group.ID.Equals(c.CampaignID.ToString())).StartDate;
                            campaignEnd = request.Campaigns.First(c => group.ID.Equals(c.CampaignID.ToString())).EndDate;
                        }
                        foreach (var step in xAxisValues)
                        {
                            var summariesForOffset = group.Summaries.Where(w => w.CampaignOffset.Equals(Convert.ToInt64(step))).ToList();
                            if (overlayType == 2)
                            {
                                summariesForOffset = GetSummariesForSources(subMediaTypes, PESHFilters, summariesForOffset);
                            }
                            decimal? offsetSum = summariesForOffset.Any() ? (overlayType == 2 ? summariesForOffset.Sum(s => s.Audience) : summariesForOffset.Sum(s => s.IQMediaValue)) : 0;

                            decimal pctChange = 0;
                            if (prevSum != 0)
                            {
                                pctChange = (((long)offsetSum - prevSum) / (decimal)prevSum) * 100;
                            }
                            prevSum = (long)offsetSum;
                            var offset = Convert.ToInt32(step);

                            var dateString = string.Empty;
                            if (request.Tab == SecondaryTabID.OverTime)
                            {
                                switch(request.DateInterval)
                                {
                                    case "hour":
                                        dateString = campaignStart.AddHours(offset).ToShortDateString();
                                        if (campaignStart.AddHours(offset).CompareTo(campaignEnd) > 0)
                                        {
                                            offsetSum = null;
                                        }
                                        break;
                                    case "day":
                                        dateString = campaignStart.AddDays(offset).ToShortDateString();
                                        if (campaignStart.AddDays(offset).CompareTo(campaignEnd) > 0)
                                        {
                                            offsetSum = null;
                                        }
                                        break;
                                    case "month":
                                        dateString = campaignStart.AddMonths(offset).ToShortDateString();
                                        if (campaignStart.AddMonths(offset).CompareTo(campaignEnd) > 0)
                                        {
                                            offsetSum = null;
                                        }
                                        break;
                                }
                            }

                            HighChartDatum hcd = new HighChartDatum() {
                                Date = dateString,
                                Value = group.ID,
                                SearchName = group.Name,
                                y = offsetSum
                            };

                            groupSeries.data.Add(hcd);
                        }
                    }
                    else
                    {
                        foreach (var date in distinctDates)
                        {
                            var summariesForDate = group.Summaries.Where(w => w.SummaryDateTime.Equals(date));
                            if (overlayType == 2)
                            {
                                summariesForDate = GetSummariesForSources(subMediaTypes, PESHFilters, summariesForDate);
                            }
                            decimal daySum = summariesForDate.Any() ? (overlayType == 2 ? summariesForDate.Sum(s => s.Audience) : summariesForDate.Sum(s => s.IQMediaValue)) : 0;
                            decimal pctChange = 0;

                            if (prevSum != 0)
                            {
                                pctChange = ((daySum - prevSum) / prevSum) * 100;
                            }
                            prevSum = daySum;

                            HighChartDatum hcd = new HighChartDatum() {
                                Date = date.ToShortDateString(),
                                Value = group.ID,
                                SearchName = group.Name,
                                //y = request.ChartType == ChartType.Growth ? pctChange : daySum    // Keep for if want overlay on growth to show growth
                                y = daySum
                            };

                            groupSeries.data.Add(hcd);
                        }
                    }

                    chartOutput.series.Add(groupSeries);
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateOverlay: {0}", sw.ElapsedMilliseconds));

                return CommonFunctions.SearializeJson(chartOutput);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return string.Empty;
            }
        }

        public List<string> CreateFusionMap(AnalyticsRequest request, AnalyticsDataModel analyticsData, List<AnalyticsGrouping> groupings, List<IQ_MediaTypeModel> subMediaTypes, AnalyticsPESHFilters PESHFilters)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug(string.Format("CreateFusionMap"));
                List<string> mapList = new List<string>();
                bool multiFilter = request.IsCompareMode;
                Dictionary<string, List<AnalyticsMapSummaryModel>> groups = new Dictionary<string, List<AnalyticsMapSummaryModel>>();
                // FOR WHEN SWITCHED TO NEW ANALYTICS TABLE - USE SUMMARIES INSTEAD OF RESULT TABLES
                //List<AnalyticsSummaryModel> allSummaries = new List<AnalyticsSummaryModel>();
                //groupings.ForEach(e => {
                //    allSummaries.AddRange(GetSummariesForSources(subMediaTypes, PESHFilters, e.Summaries));
                //});

                if (multiFilter)
                {
                    foreach(var id in request.RequestIDs)
                    {
                        var agentName = groupings.Where(w => w.ID.Equals(id.ToString())).DefaultIfEmpty(new AnalyticsGrouping { Name = "" }).FirstOrDefault().Name;
                        var dmaList = request.ChartType.Equals(ChartType.US) ? 
                            analyticsData.DmaMentionMapList.Where(w => w.SearchRequestID.Equals(id)).ToList() :
                            analyticsData.CanadaMentionMapList.Where(w => w.SearchRequestID.Equals(id)).ToList();
                        groups.Add(agentName, dmaList);
                    }
                }
                else
                {
                    groups.Add("", request.ChartType.Equals(ChartType.US) ? analyticsData.DmaMentionMapList : analyticsData.CanadaMentionMapList);
                }

                List<string> colors = new List<string>() {
                    "A0D6FC",
                    "83C9FC",
                    "5DBBFE",
                    "3FAEFD",
                    "0395FE"
                };

                foreach (var group in groups)
                {
                    FusionMapOutput fusionMap = new FusionMapOutput() {
                        map = new FusionMap() {
                            animation = "0",
                            showbevel = "1",
                            usehovercolor = "1",
                            canvasbordercolor = "FFFFFF",
                            bordercolor = "B7B7B7",
                            showlegend = "1",
                            showshadow = "0",
                            legendposition = "BOTTOM",
                            legendborderalpha = 1,
                            legendbordercolor = "FFFFFF",
                            legendallowdrag = "0",
                            legendshadow = "1",
                            connectorcolor = "000000",
                            fillalpha = "80",
                            hovercolor = "CCCCCC",
                            showEntityToolTip = "1",
                            showToolTip = "0",
                            caption = group.Key
                        }
                    };

                    // Set legend color ranges
                    FusionMapColorRange mapColorRange = new FusionMapColorRange() {
                        color = new List<FusionMapColor>()
                    };

                    long minValue = 1000000;
                    long maxValue = 0;

                    // Set map data
                    List<FusionMapData> mapData = new List<FusionMapData>();
                    if (request.ChartType == ChartType.US)
                    {
                        foreach (var keyVal in IQDmaToFusionIDMapModel.IQDmaToFusionIDMap)
                        {
                            // keyVal values ARE NOT the IDs of those markets
                            // FOR WHEN SWITCHING TO SUMMARY TABLES INSTEAD OF RESULTS TABLES
                            //var dmaSummaries = allSummaries.Where(w => w.SubMediaType.Equals("TV") && w.Market.Equals(keyVal.Key)).ToList();

                            //long mention = dmaSummaries.Any() ? GetSumsFromSummaries(request.PESHTypes, subMediaTypes, PESHFilters, dmaSummaries) : 0;

                            long mention = group.Value.Where(w => w.DMAName == keyVal.Key).Sum(s => s.NumberOfHits);


                            FusionMapData mapDatum = new FusionMapData() {
                                id = keyVal.Value.ToString(),
                                value = mention.ToString(),
                                tooltext = string.Format("DMA Area : {0}{1}Mention: {2:N0}", keyVal.Key, "{br}", mention),
                                showEntityToolTip = "1"
                            };

                            bool isHonolulu = string.Compare("Honolulu", keyVal.Key, true) == 0;
                            bool isAnchorage = string.Compare("Anchorage", keyVal.Key, true) == 0;
                            bool isJuneau = string.Compare("Juneau", keyVal.Key, true) == 0;
                            bool isFairbanks = string.Compare("Fairbanks", keyVal.Key, true) == 0;

                            if (isHonolulu || isAnchorage || isJuneau || isFairbanks)
                            {
                                mapDatum.showlabel = "1";
                            }
                            else
                            {
                                mapDatum.showlabel = "0";
                            }

                            if (mention > maxValue)
                            {
                                maxValue = mention;
                            }

                            if (mention < minValue)
                            {
                                minValue = mention;
                            }

                            mapData.Add(mapDatum);
                        }
                    }
                    else
                    {
                        foreach (var keyVal in IQProvinceToFusionIDMapModel.IQProvinceToFusionIDMap)
                        {
                            long mention = group.Value.Where(w => w.DMAName == keyVal.Key).Sum(s => s.NumberOfHits);
                            FusionMapData mapDatum = new FusionMapData() {
                                id = keyVal.Value,
                                value = mention.ToString(),
                                tooltext = string.Format("Province : {0}{1}Mention: {2:N0}", keyVal.Key, "{br}", mention),
                                showEntityToolTip = "1",
                                showlabel = "0"
                            };

                            if (mention > maxValue)
                            {
                                maxValue = mention;
                            }

                            if (mention < minValue)
                            {
                                minValue = mention;
                            }

                            mapData.Add(mapDatum);
                        }
                    }

                    long colorStep = (maxValue - minValue) / 5;
                    if (colorStep > 0)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            FusionMapColor mapColor = new FusionMapColor();
                            if (i == 0)
                            {
                                mapColor.minvalue = minValue.ToString();
                                mapColor.maxvalue = ((colorStep * (i + 1)) - 1).ToString();
                            }
                            else if (i == 4)
                            {
                                mapColor.minvalue = (colorStep * i).ToString();
                                mapColor.maxvalue = maxValue.ToString();
                            }
                            else
                            {
                                mapColor.minvalue = (colorStep * i).ToString();
                                mapColor.maxvalue = ((colorStep * (i + 1)) - 1).ToString();
                            }

                            mapColor.code = colors[i];
                            mapColor.displayvalue = mapColor.maxvalue;
                            mapColorRange.color.Add(mapColor);
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

                        FusionMapColor mapColor = new FusionMapColor() {
                            minvalue = minValue.ToString(),
                            maxvalue = maxValue.ToString(),
                            code = "C3EBFD",
                            displayvalue = maxValue.ToString()
                        };
                        mapColorRange.color.Add(mapColor);
                    }

                    fusionMap.colorrange = mapColorRange;
                    fusionMap.data = mapData;

                    mapList.Add(CommonFunctions.SearializeJson(fusionMap));
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateFusionMap: {0} ms", sw.ElapsedMilliseconds));
                return mapList;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new List<string>();
            }
        }

        #endregion

        #region Solr

        public List<FacetResponse> Search(List<string> SRIDs, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                
                System.Uri searchRequestUrl = new Uri("http://10.100.1.41:8080/solr/cfe-2016-1/select?");
                SearchEngine searchEngine = new SearchEngine(searchRequestUrl);
                SearchRequest request = new SearchRequest() {
                    SearchRequestIDs = SRIDs,
                    FromDate = dateFrom,
                    ToDate = dateTo
                };

                //Dictionary<string, string> searchResult = searchEngine.Search(request);

                return searchEngine.Search(request);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new List<FacetResponse>();
            }
        }

        public List<FacetResponse> AnalyticsSearch(AnalyticsRequest request)
        {
            try
            {
                string url = string.Format("http://10.100.1.161:8080:/solr/{0}/select?", request.DateInterval.Equals("day") ? "AnalyticsHourSummary" : "AnalyticsDaySummary");
                Uri requestURL = new Uri(url);

                SearchEngine engine = new SearchEngine(requestURL);
                SearchRequest sRequest = new SearchRequest() {
                    SearchRequestIDs = request.RequestIDs.Select(s => s.ToString()).ToList(),
                    FromDate = Convert.ToDateTime(request.DateFrom),
                    ToDate = Convert.ToDateTime(request.DateTo),
                    DateInterval = request.DateInterval,
                    Tab = request.Tab.ToString()
                };

                return engine.Search(sRequest);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new List<FacetResponse>();
            }
        }

        #endregion

    }
}

