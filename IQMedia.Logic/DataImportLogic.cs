using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;
using IQMedia.Web.Logic.Base;
using IQMedia.Shared.Utility;
using IQCommon.Model;

namespace IQMedia.Web.Logic
{
    public class DataImportLogic : ILogic
    {
        public DataImportClientModel GetDataImportClient(Guid clientGuid)
        {
            DataImportDA dataImportDA = (DataImportDA)DataAccessFactory.GetDataAccess(DataAccessType.DataImport);
            return dataImportDA.GetDataImportClient(clientGuid);
        }

        #region Sony

        public List<SonySummaryModel> GetSonySummaryData(Guid clientGuid, DateTime fromDate, DateTime toDate, int dateIntervalType, List<string> searchRequestIDs, List<string> artists, List<string> albums, List<string> tracks, string tableType, List<IQCommon.Model.IQ_MediaTypeModel> mediaTypes)
        {
            string searchRequestIDXml = null;
            if (searchRequestIDs != null && searchRequestIDs.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in searchRequestIDs
                    select new XElement(
                        "item",
                        new XAttribute("id", i)
                    )
                ));
                searchRequestIDXml = doc.ToString();
            }

            string mediaTypeAccessXml = null;
            if (mediaTypes != null && mediaTypes.Count > 0)
            {    
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from mt in mediaTypes
                    select new XElement(
                        "item",
                        new XAttribute("SubMediaType", mt.SubMediaType),
                        new XAttribute("HasAccess", mt.HasAccess), 
                        new XAttribute("MediaType", mt.MediaType), 
                        new XAttribute("TypeLevel", mt.TypeLevel)                        
                    )
                ));   
                mediaTypeAccessXml = doc.ToString();         
            }

            string filterXml = null;
            if (artists != null && artists.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in artists
                    select new XElement(
                        "item",
                        new XAttribute("artist", i),
                        new XAttribute("album", ""),
                        new XAttribute("track", "")
                    )
                ));
                filterXml = doc.ToString();
            }
            else if (albums != null && albums.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in albums
                    select new XElement(
                        "item",
                        new XAttribute("artist", ""),
                        new XAttribute("album", i),
                        new XAttribute("track", "")
                    )
                ));
                filterXml = doc.ToString();
            }
            else if (tracks != null && tracks.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in tracks
                    select new XElement(
                        "item",
                        new XAttribute("artist", ""),
                        new XAttribute("album", ""),
                        new XAttribute("track", i)
                    )
                ));
                filterXml = doc.ToString();
            }
            else
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    new XElement(
                        "item",
                        new XAttribute("artist", ""),
                        new XAttribute("album", ""),
                        new XAttribute("track", "")
                    )
                ));
                filterXml = doc.ToString();
            }

            DataImportDA dataImportDA = (DataImportDA)DataAccessFactory.GetDataAccess(DataAccessType.DataImport);
            return dataImportDA.GetSonySummaryData(clientGuid, fromDate, toDate, dateIntervalType, searchRequestIDXml, filterXml, tableType, mediaTypeAccessXml);
        }

        public List<SonyTableModel> GetSonyTableData(Guid clientGuid, DateTime fromDate, DateTime toDate, List<string> searchRequestIDs, int pageSize, int startIndex, string tableType, out int numTotalRecords)
        {
            string searchRequestIDXml = null;
            if (searchRequestIDs != null && searchRequestIDs.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in searchRequestIDs
                    select new XElement(
                        "item",
                        new XAttribute("id", i)
                    )
                ));
                searchRequestIDXml = doc.ToString();
            }

            DataImportDA dataImportDA = (DataImportDA)DataAccessFactory.GetDataAccess(DataAccessType.DataImport);
            return dataImportDA.GetSonyTableData(clientGuid, fromDate, toDate, searchRequestIDXml, pageSize, startIndex, tableType, out numTotalRecords);
        }

        public List<SonyTableModel> GetSonyExportData(Guid clientGuid, DateTime fromDate, DateTime toDate, List<string> searchRequestIDs, string tableType, List<IQCommon.Model.IQ_MediaTypeModel> mediaTypes)
        {
            string searchRequestIDXml = null;
            if (searchRequestIDs != null && searchRequestIDs.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in searchRequestIDs
                    select new XElement(
                        "item",
                        new XAttribute("id", i)
                    )
                ));
                searchRequestIDXml = doc.ToString();
            }

            string mediaTypeAccessXml = null;
            if (mediaTypes != null && mediaTypes.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from mt in mediaTypes
                    select new XElement(
                        "item",
                        new XAttribute("SubMediaType", mt.SubMediaType),
                        new XAttribute("HasAccess", mt.HasAccess),
                        new XAttribute("MediaType", mt.MediaType),
                        new XAttribute("TypeLevel", mt.TypeLevel)
                    )
                ));
                mediaTypeAccessXml = doc.ToString();
            }

            DataImportDA dataImportDA = (DataImportDA)DataAccessFactory.GetDataAccess(DataAccessType.DataImport);
            return dataImportDA.GetSonyExportData(clientGuid, fromDate, toDate, searchRequestIDXml, tableType, mediaTypeAccessXml);
        }

        public SummaryReportMulti SonyLineChart(List<SonySummaryModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, int dateIntervalType, int? chartWidth, Dictionary<long, string> p_SearchRequests,
                                                    List<string> p_Artists, List<string> p_Albums, List<string> p_Tracks, List<IQ_MediaTypeModel> p_SubMediaTypes)
        {
            try
            {
                List<DateTime> dateRange = new List<DateTime>();
                if (dateIntervalType == 1) // Day
                {
                    TimeSpan ts = p_ToDate.Subtract(p_FromDate);
                    for (int i = 0; i <= ts.Days; i++)
                    {
                        dateRange.Add(p_FromDate.AddDays(i));
                    }
                }
                else // Month
                {
                    for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                    {
                        dateRange.Add(dt);
                    }
                }

                SummaryReportMulti lstSummaryReportMulti = new SummaryReportMulti();


                List<string> categories = new List<string>();

                foreach (var date in dateRange)
                {
                    categories.Add(date.ToShortDateString());
                }

                #region Media Chart

                // Create a chart to display a single series for the aggregate of all submedia types if no search requests are selected.
                // If search requests are selected, this chart will display a series for each one.
                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
                highLineChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { title = new Title2() }, 
                                                                new YAxis() { title = new Title2() { text = "iTunes", rotation = 90 }, opposite = true },
                                                                new YAxis() { title = new Title2() { text = "Spotify", rotation = 90 }, opposite = true },
                                                                new YAxis() { title = new Title2() { text = "Apple Music", rotation = 90 }, opposite = true }};


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
                    {
                        formatter = dateIntervalType == 1 ? null : "GetMonth"
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

                // if one or more search requests are selected, then create a series for each one
                // with total no. of records for that search request on a particular date
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
                    int colorIndex = 0;
                    foreach (var searchRequest in p_SearchRequests)
                    {
                        // Search Request Series
                        Series series = new Series();
                        series.data = new List<HighChartDatum>();
                        series.name = searchRequest.Value;
                        series.color = highLineChartOutput.colors[colorIndex % highLineChartOutput.colors.Count];
                        series.yAxis = 0;

                        // ITunes Series
                        Series iTunesAgentSeries = new Series();
                        iTunesAgentSeries.dashStyle = "shortdash";
                        iTunesAgentSeries.data = new List<HighChartDatum>();
                        iTunesAgentSeries.name = searchRequest.Value + " (iTunes)";
                        iTunesAgentSeries.color = series.color;
                        iTunesAgentSeries.yAxis = 1;

                        // Spotify Series
                        Series spotifyAgentSeries = new Series();
                        spotifyAgentSeries.dashStyle = "shortdot";
                        spotifyAgentSeries.data = new List<HighChartDatum>();
                        spotifyAgentSeries.name = searchRequest.Value + " (Spotify)";
                        spotifyAgentSeries.color = series.color;
                        spotifyAgentSeries.yAxis = 2;

                        // Apple Music Series
                        Series appleAgentSeries = new Series();
                        appleAgentSeries.dashStyle = "shortdashdot";
                        appleAgentSeries.data = new List<HighChartDatum>();
                        appleAgentSeries.name = searchRequest.Value + " (Apple Music)";
                        appleAgentSeries.color = series.color;
                        appleAgentSeries.yAxis = 3;

                        // loop for each date to create list of data for selected search request series. 
                        foreach (var item in dateRange)
                        {
                            List<SonySummaryModel> lstSummaries = listOfSummaryReportData.Where(smr => smr.SearchRequestID == searchRequest.Key
                                    && ((dateIntervalType == 1 && smr.GMTDateTime.Equals(item)) || (dateIntervalType == 3 && smr.GMTDateTime.Month.Equals(item.Month) && smr.GMTDateTime.Year.Equals(item.Year)))).ToList();

                            // Search Request Data
                            var daywiseSum = lstSummaries.Where(smr => smr.SeriesType == "SubMedia"
                                    && CheckSubMediaTypeAccess(p_SubMediaTypes, smr.SubMediaType)
                                ).Sum(s => s.NoOfDocs);

                            // set data point of current series 
                            /*
                                *  y = y series value of current point === total no. of records for current search request at particular date 
                                *  SearchTerm = query name  , used in chart drill down click event
                                *  Value = Search Request ID  , used in chart drill down click event
                                *  Type = "Media" / "SubMedia" ,used in chart drill down click event 
                            */
                            HighChartDatum highChartDatum = new HighChartDatum();
                            highChartDatum.y = daywiseSum;
                            highChartDatum.SearchTerm = searchRequest.Value;
                            highChartDatum.Value = searchRequest.Key.ToString();
                            highChartDatum.Type = "Media";
                            series.data.Add(highChartDatum);

                            // ITunes Data
                            HighChartDatum iTunesAgentDatum = new HighChartDatum();
                            iTunesAgentDatum.y = lstSummaries.Where(smr => smr.SubMediaType == "iTunes").Sum(s => s.NoOfDocs);
                            iTunesAgentDatum.Type = "Client";
                            iTunesAgentSeries.data.Add(iTunesAgentDatum);

                            // Spotify Data
                            HighChartDatum spotifyAgentDatum = new HighChartDatum();
                            spotifyAgentDatum.y = lstSummaries.Where(smr => smr.SubMediaType == "Spotify").Sum(s => s.NoOfDocs);
                            spotifyAgentDatum.Type = "Client";
                            spotifyAgentSeries.data.Add(spotifyAgentDatum);

                            // Apple Music Data
                            HighChartDatum appleAgentDatum = new HighChartDatum();
                            appleAgentDatum.y = lstSummaries.Where(smr => smr.SubMediaType == "Apple").Sum(s => s.NoOfDocs);
                            appleAgentDatum.Type = "Client";
                            appleAgentSeries.data.Add(appleAgentDatum);
                        }

                        colorIndex++;
                        lstSeries.Add(series);
                        lstSeries.Add(iTunesAgentSeries);
                        lstSeries.Add(spotifyAgentSeries);
                        lstSeries.Add(appleAgentSeries);
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
                        var sumOfDocs = listOfSummaryReportData.Where(smr => ((dateIntervalType == 1 && smr.GMTDateTime.Equals(item)) || (dateIntervalType == 3 && smr.GMTDateTime.Month.Equals(item.Month) && smr.GMTDateTime.Year.Equals(item.Year)))
                                && CheckSubMediaTypeAccess(p_SubMediaTypes, smr.SubMediaType)
                            ).Sum(s => s.NoOfDocs);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                            *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                            *  we will not set SearchTerm and Value properies of data, as this is signle medium chart , without any search request
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = sumOfDocs;
                        highChartDatum.Type = "Media";
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

                // assign set of series data to medium chart (or multi line searchrequest chart)
                highLineChartOutput.series = lstSeries;

                lstSummaryReportMulti.MediaRecords = CommonFunctions.SearializeJson(highLineChartOutput);

                #endregion

                #region SubMedia Chart

                // This chart will include a series for each submedia type, plus ITunes and Spotify
                HighLineChartOutput highLineChartSubMediaOutput = new HighLineChartOutput();
                highLineChartSubMediaOutput.title = new Title() { text = "", x = -20 };
                highLineChartSubMediaOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartSubMediaOutput.yAxis = new List<YAxis>() { new YAxis() { title = new Title2() }, 
                                                                        new YAxis() { title = new Title2() { text = "iTunes", rotation = 90 }, opposite = true },
                                                                        new YAxis() { title = new Title2() { text = "Spotify", rotation = 90 }, opposite = true },
                                                                        new YAxis() { title = new Title2() { text = "Apple Music", rotation = 90 }, opposite = true }};

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
                        formatter = dateIntervalType == 1 ? null : "GetMonth"
                    }
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

                Int64 totNumOfHits = listOfSummaryReportData.Where(smr => CheckSubMediaTypeAccess(p_SubMediaTypes, smr.SubMediaType)).Sum(s => s.NoOfDocs);
                lstSummaryReportMulti.TotalNumOfHits = totNumOfHits.ToString("N0");

                // start to set series of data for  multi line medium chart
                List<Series> lstSeriesSubMediaType = new List<Series>();

                // SubMedia Data
                foreach (var mediaType in p_SubMediaTypes.Where(m => m.TypeLevel == 1 && m.HasAccess && p_SubMediaTypes.Where(sm => string.Compare(m.MediaType, sm.MediaType, true) == 0 && sm.TypeLevel == 2 && sm.HasAccess).Count() > 0))
                {
                    // set series name of multiline medium chart as medium description, will be shown in legend and tooltip.
                    string enumDesc = mediaType.DisplayName;
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = enumDesc;
                    series.yAxis = 0;

                    // loop for each date to create list of data for selected medium type
                    foreach (var item in dateRange)
                    {
                        var daywiseSum = listOfSummaryReportData.Where(smr => String.Compare(smr.MediaType, mediaType.MediaType, true) == 0
                                && ((dateIntervalType == 1 && smr.GMTDateTime.Equals(item)) || (dateIntervalType == 3 && smr.GMTDateTime.Month.Equals(item.Month) && smr.GMTDateTime.Year.Equals(item.Year)))).Sum(s => s.NoOfDocs);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current medium type at perticular date 
                            *  SearchTerm = medium description  , used in chart drill down click event
                            *  Value = medium tpye  , used in chart drill down click event
                            *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = daywiseSum;
                        highChartDatum.SearchTerm = enumDesc;
                        highChartDatum.Value = mediaType.MediaType;
                        highChartDatum.Type = "SubMedia";
                        series.data.Add(highChartDatum);
                    }

                    lstSeriesSubMediaType.Add(series);
                }

                // Sony Data
                int seriesColorIndex = lstSeriesSubMediaType.Count;
                List<string> lstSelections = new List<string>();
                string selectionType = String.Empty;
                if (p_Artists != null && p_Artists.Count > 0)
                {
                    lstSelections = p_Artists;
                    selectionType = "Artist";
                }
                else if (p_Albums != null && p_Albums.Count > 0)
                {
                    lstSelections = p_Albums;
                    selectionType = "Album";
                }
                else if (p_Tracks != null && p_Tracks.Count > 0)
                {
                    lstSelections = p_Tracks;
                    selectionType = "Track";
                }

                if (!String.IsNullOrEmpty(selectionType))
                {
                    foreach (string selection in lstSelections)
                    {
                        Series iTunesSeries = new Series();
                        iTunesSeries.dashStyle = "shortdash";
                        iTunesSeries.data = new List<HighChartDatum>();
                        iTunesSeries.name = selection + " (iTunes)";
                        iTunesSeries.yAxis = 1;
                        iTunesSeries.color = highLineChartOutput.colors[seriesColorIndex % highLineChartOutput.colors.Count];
                        lstSeriesSubMediaType.Add(iTunesSeries);

                        Series spotifySeries = new Series();
                        spotifySeries.dashStyle = "shortdot";
                        spotifySeries.data = new List<HighChartDatum>();
                        spotifySeries.name = selection + " (Spotify)";
                        spotifySeries.yAxis = 2;
                        spotifySeries.color = iTunesSeries.color;
                        lstSeriesSubMediaType.Add(spotifySeries);

                        Series appleSeries = new Series();
                        appleSeries.dashStyle = "shortdashdot";
                        appleSeries.data = new List<HighChartDatum>();
                        appleSeries.name = selection + " (Apple Music)";
                        appleSeries.yAxis = 3;
                        appleSeries.color = iTunesSeries.color;
                        lstSeriesSubMediaType.Add(appleSeries);

                        foreach (var item in dateRange)
                        {
                            List<SonySummaryModel> lstSummaries = listOfSummaryReportData.Where(smr => ((dateIntervalType == 1 && smr.GMTDateTime.Equals(item)) || (dateIntervalType == 3 && smr.GMTDateTime.Month.Equals(item.Month) && smr.GMTDateTime.Year.Equals(item.Year)))).ToList();
                            switch (selectionType)
                            {
                                case "Artist":
                                    lstSummaries = lstSummaries.Where(smr => String.Compare(smr.Artist, selection, true) == 0).ToList();
                                    break;
                                case "Album":
                                    lstSummaries = lstSummaries.Where(smr => String.Compare(smr.Album, selection, true) == 0).ToList();
                                    break;
                                case "Track":
                                    lstSummaries = lstSummaries.Where(smr => String.Compare(smr.Track, selection, true) == 0).ToList();
                                    break;
                            }

                            HighChartDatum iTunesDatum = new HighChartDatum();
                            iTunesDatum.y = lstSummaries.Where(smr => String.Compare(smr.SubMediaType, "iTunes", true) == 0).Sum(s => s.NoOfDocs);
                            iTunesDatum.Type = "Client";
                            iTunesSeries.data.Add(iTunesDatum);

                            HighChartDatum spotifyDatum = new HighChartDatum();
                            spotifyDatum.y = lstSummaries.Where(smr => String.Compare(smr.SubMediaType, "Spotify", true) == 0).Sum(s => s.NoOfDocs);
                            spotifyDatum.Type = "Client";
                            spotifySeries.data.Add(spotifyDatum);

                            HighChartDatum appleDatum = new HighChartDatum();
                            appleDatum.y = lstSummaries.Where(smr => String.Compare(smr.SubMediaType, "Apple", true) == 0).Sum(s => s.NoOfDocs);
                            appleDatum.Type = "Client";
                            appleSeries.data.Add(appleDatum);
                        }

                        seriesColorIndex++;
                    }
                }
                else
                {
                    Series iTunesSeries = new Series();
                    iTunesSeries.dashStyle = "shortdash";
                    iTunesSeries.data = new List<HighChartDatum>();
                    iTunesSeries.name = "iTunes";
                    iTunesSeries.yAxis = 1;
                    lstSeriesSubMediaType.Add(iTunesSeries);

                    Series spotifySeries = new Series();
                    spotifySeries.dashStyle = "shortdash";
                    spotifySeries.data = new List<HighChartDatum>();
                    spotifySeries.name = "Spotify";
                    spotifySeries.yAxis = 2;
                    lstSeriesSubMediaType.Add(spotifySeries);

                    Series appleSeries = new Series();
                    appleSeries.dashStyle = "shortdash";
                    appleSeries.data = new List<HighChartDatum>();
                    appleSeries.name = "Apple Music";
                    appleSeries.yAxis = 3;
                    lstSeriesSubMediaType.Add(appleSeries);

                    foreach (var item in dateRange)
                    {
                        List<SonySummaryModel> lstSummaries = listOfSummaryReportData.Where(smr => ((dateIntervalType == 1 && smr.GMTDateTime.Equals(item)) ||
                                                                        (dateIntervalType == 3 && smr.GMTDateTime.Month.Equals(item.Month) && smr.GMTDateTime.Year.Equals(item.Year)))).ToList();

                        HighChartDatum iTunesDatum = new HighChartDatum();
                        iTunesDatum.y = lstSummaries.Where(smr => String.Compare(smr.SubMediaType, "iTunes", true) == 0).Sum(s => s.NoOfDocs);
                        iTunesDatum.Type = "Client";
                        iTunesSeries.data.Add(iTunesDatum);

                        HighChartDatum spotifyDatum = new HighChartDatum();
                        spotifyDatum.y = lstSummaries.Where(smr => String.Compare(smr.SubMediaType, "Spotify", true) == 0).Sum(s => s.NoOfDocs);
                        spotifyDatum.Type = "Client";
                        spotifySeries.data.Add(spotifyDatum);

                        HighChartDatum appleDatum = new HighChartDatum();
                        appleDatum.y = lstSummaries.Where(smr => String.Compare(smr.SubMediaType, "Apple", true) == 0).Sum(s => s.NoOfDocs);
                        appleDatum.Type = "Client";
                        appleSeries.data.Add(appleDatum);
                    }
                }

                // assign set of series data to multi line medium type chart
                highLineChartSubMediaOutput.series = lstSeriesSubMediaType;

                lstSummaryReportMulti.SubMediaRecords = CommonFunctions.SearializeJson(highLineChartSubMediaOutput);

                #endregion

                return lstSummaryReportMulti;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        private bool CheckSubMediaTypeAccess(List<IQ_MediaTypeModel> p_MediaTypeList, string p_RecordSubMediaType)
        {
            List<IQ_MediaTypeModel> lstSubMediaTypes = p_MediaTypeList.Where(m => string.Compare(p_RecordSubMediaType, m.SubMediaType, true) == 0 && m.TypeLevel == 2).ToList();

            if (lstSubMediaTypes.Count > 0)
            {
                return lstSubMediaTypes.Single().HasAccess;
            }
            return false;
        }
    }
}
