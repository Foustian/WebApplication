using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Model;
using System.Xml.Linq;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Shared.Utility;

namespace IQMedia.Web.Logic
{
    public class ImagiQLogic : ILogic
    {
        public List<ImagiQLogoModel> GetLRResultsByGuid(Guid recordfileGuid)
        {
            ImagiQDA ImagiQDA = (ImagiQDA)DataAccessFactory.GetDataAccess(DataAccessType.ImagiQ);
            return ImagiQDA.GetLRResultsByGuid(recordfileGuid);
        }

        public Dictionary<string, object> GetLRResults(Guid clientGuid, DateTime? fromDate, DateTime? toDate, List<string> logoIDList, List<string> dmaList, List<string> stationAffilList, List<string> stationIDList, string classNum, int? regionNum, int? countryNum, bool isAsc, bool isMarketSort, long? fromRecordID, int pageSize, List<string> industryList, List<string> brandList, ref long? sinceID, out int totalResults)
        {
            string strLogoIDList = null;
            if (logoIDList != null && logoIDList.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in logoIDList
                                             select new XElement("item", new XAttribute("id", ele))
                                                     ));
                strLogoIDList = xdoc.ToString();
            }

            string strDmaList = null;
            if (dmaList != null && dmaList.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in dmaList
                                             select new XElement("item", new XAttribute("id", ele))
                                                     ));
                strDmaList = xdoc.ToString();
            }

            string strStationAffilList = null;
            if (stationAffilList != null && stationAffilList.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in stationAffilList
                                             select new XElement("item", new XAttribute("id", ele))
                                                     ));
                strStationAffilList = xdoc.ToString();
            }

            string strStationIDList = null;
            if (stationIDList != null && stationIDList.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in stationIDList
                                             select new XElement("item", new XAttribute("id", ele))
                                                     ));
                strStationIDList = xdoc.ToString();
            }

            string strIndustryList = null;
            if (industryList != null && industryList.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in industryList
                                             select new XElement("item", new XAttribute("id", ele))
                                                     ));
                strIndustryList = xdoc.ToString();
            }

            string strBrandList = null;
            if (brandList != null && brandList.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in brandList
                                             select new XElement("item", new XAttribute("id", ele))
                                                     ));
                strBrandList = xdoc.ToString();
            }

            ImagiQDA ImagiQDA = (ImagiQDA)DataAccessFactory.GetDataAccess(DataAccessType.ImagiQ);
            return ImagiQDA.GetLRResults(clientGuid, fromDate, toDate, strLogoIDList, strDmaList, strStationAffilList, strStationIDList, classNum, regionNum, countryNum, isAsc, isMarketSort, fromRecordID, pageSize, strIndustryList, strBrandList, ref sinceID, out totalResults);
        }

        public string LRHighLineChart(List<ImagiQLogoModel> lstLogoHits, out List<string> yAxisCompanies, out List<string> yAxisLogoPaths)
        {
            try
            {
                // start to set series of data for multiline search term chart 
                List<Series> lstSeries = new List<Series>();

                yAxisCompanies = new List<string>();
                yAxisLogoPaths = new List<string>();
                var yAxisData = new List<HighChartDatum>();
                var xAxisData = new List<string>();

                double numOfTicks = (60 * 60) + 1;
                int yAxis = 1;
                int numOfYAxisEntres = 0;

                if (lstLogoHits != null && lstLogoHits.Count() > 0)
                {
                    foreach (var tuple in lstLogoHits.Select(x => new Tuple<string,string>(x.CompanyName, x.ThumbnailPath)).Distinct())
                    {
                        var chartDatum = new HighChartDatum();
                        int counter = 0;
                        numOfYAxisEntres++;
                        yAxisData = new List<HighChartDatum>();

                        foreach (var result in lstLogoHits.Where(x => x.CompanyName == tuple.Item1))
                        {
                            chartDatum = new HighChartDatum()
                            {
                                y = yAxis,
                                SearchName = result.CompanyName, //Brand Name
                                SearchTerm = result.ThumbnailPath, //Path of Logo of Brand
                                Medium = result.HitLogoPath //Path of Logo Used to Match Image
                            };

                            bool moreThan10Away = (result.Offset - counter) > 10;

                            if (counter == 0 || moreThan10Away)
                            {
                                if (moreThan10Away && (yAxisData.Count > 1 && yAxisData[yAxisData.Count - 2].y == null) || yAxisData.Count == 1)
                                {
                                    yAxisData.Add(chartDatum);
                                    counter++;
                                }

                                while (counter < result.Offset)
                                {
                                    yAxisData.Add(new HighChartDatum()
                                    {
                                        y = null,
                                        SearchName = "",
                                        SearchTerm = "",
                                        Medium = ""
                                    });

                                    counter++;
                                }
                            }

                            while (counter <= result.Offset)
                            {
                                yAxisData.Add(chartDatum);
                                counter++;
                            }
                        }

                        if ((yAxisData.Count > 1 && yAxisData[yAxisData.Count - 2].y == null) || yAxisData.Count == 1)
                        {
                            yAxisData.Add(chartDatum);

                            counter++;
                        }
                        while (counter <= numOfTicks)
                        {
                            var datum = new HighChartDatum()
                            {
                                y = null,
                                SearchName = "",
                                SearchTerm = "",
                                Medium = ""
                            };
                            yAxisData.Add(datum);
                                
                            counter++;
                        }

                        yAxisCompanies.Add(tuple.Item1.ToString());
                        yAxisLogoPaths.Add(tuple.Item2.ToString());

                        Series series = new Series();
                        series.name = tuple.Item1;
                        series.data = yAxisData;

                        lstSeries.Add(series);

                        if (yAxis == 1)
                        {
                            for (int x = 0; x < counter; x++)
                            {
                                xAxisData.Add(x.ToString());
                            }
                        }
                        yAxis++;
                    }
                }


                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();

                // set chart title and title style
                highLineChartOutput.title = new Title()
                {
                    text = "LR Data",
                    x = 0,
                    y = 10,
                    align = "left",
                    style = new HStyle
                    {
                        color = "#555555",
                        fontWeight = "bold",
                        fontFamily = "Verdana",
                        fontSize = "13px"
                    }
                };
                //highLineChartOutput.subtitle = new Subtitle() { text = p_GraphStructureModel.ChartSubTitle, x = 80, y = 20, align = "left", };

                highLineChartOutput.yAxis = new List<YAxis>() { new YAxis ()
                {
                    title = new Title2() { text = "" },
                    labels = new labels()
                    {
                        formatter = "FormatLRBrandLabel",
                        useHTML = true,
                        enabled = true
                    },
                    allowDecimals = false,
                    min = 0,
                    gridLineWidth = 1,
                    minorGridLineWidth = 0,
                    tickInterval = 1
                }};

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 labels will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(xAxisData.Count()) / 20)),
                    tickmarkPlacement = "off",
                    categories = xAxisData,
                    labels = new labels()
                    {
                        rotation = 0,
                        formatter = "FormatTime",
                        enabled = true
                    },
                    tickWidth = 1,
                    gridLineWidth = 1,
                    minorGridLineWidth = 0
                };

                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { formatter = "tooltipFormat", useHTML = true };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { enabled = true };

                // set chart with height and width
                highLineChartOutput.hChart = new HChart() { height = (numOfYAxisEntres * 120) + (numOfYAxisEntres < 3 ? 80 : 0), zoomType = "x", width = 980 };

                // set plot options and click event for series points (which will again assigned in JS as this is string value)
                // legendItemClick event to show / hide column chart series on line legend click
                highLineChartOutput.plotOption = new PlotOptions()
                {
                    column = null,
                    series = new PlotSeries()
                    {
                        events = new PlotEvents()
                        {
                            hide = "HandleSeriesHide",
                            show = "HandleSeriesShow"
                        },
                        lineWidth = 8,
                        marker = new PlotMarker
                        {
                            enabled = false,
                            radius = 6
                        },
                        states = new PlotSeriesStates()
                        {
                            hover = new SeriesState()
                            {
                                halo = new PlotStateHalo()
                                {
                                    size = 2
                                }
                            }
                        },
                        cursor = "pointer",
                        turboThreshold = 40000,
                        point = new PlotPoint()
                        {
                            events = new PlotEvents()
                            {
                                click = "ChartClick",
                                mouseOver = "ChartHoverManage",
                                mouseOut = "ChartHoverOutManage"
                            }
                        }
                    }
                };

                highLineChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
                return jsonResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<long> GetSearchImageIDs(long BrandID)
        {
            ImagiQDA ImagiQDA = (ImagiQDA)DataAccessFactory.GetDataAccess(DataAccessType.ImagiQ);
            return ImagiQDA.GetSearchImageIDs(BrandID);
        }

        public List<long> GetSearchImagesById(List<long> LRSearchIDs, out List<string> lstSearchImages)
        {
            ImagiQDA ImagiQDA = (ImagiQDA)DataAccessFactory.GetDataAccess(DataAccessType.ImagiQ);
            return ImagiQDA.GetSearchImagesById(LRSearchIDs, out lstSearchImages);
        }
    }
}
