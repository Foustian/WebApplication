using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using System.Globalization;
using IQMedia.Shared.Utility;
using System.Xml.Linq;

namespace IQMedia.Web.Logic
{
    public class ClipLogic : IQMedia.Web.Logic.Base.ILogic
    {

        #region Get Data

        public List<IQTrackPlayLogModel> GetPlayLogNSummary(string p_AssetGuid, DateTime p_FromDate, DateTime p_ToDate)
        {
            ClipDA clipDA = (ClipDA)DataAccessFactory.GetDataAccess(DataAccessType.Clip);
            return clipDA.GetPlayLogNSummary(p_AssetGuid, p_FromDate, p_ToDate);
        }

        #region TV Views Spark Chart
        public string GetTVViewsSparkChart(List<IQTrackPlayLogModel> listOfIQTrackPlayLogNSummary, DateTime p_FromDate, DateTime p_ToDate, out Int64 p_TotalViews)
        {
            try
            {
                var dateRange = new List<DateTime>();

                for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddDays(1))
                {
                    dateRange.Add(dt);
                }

                Series seriesTVViews = new Series();
                seriesTVViews.data = new List<HighChartDatum>();
                p_TotalViews = 0;

                foreach (var item in dateRange)
                {
                    Int64 Count = 0;
                    var assetGuid = listOfIQTrackPlayLogNSummary.Where(cp => cp.PlayDate.Equals(item.Date));
                    if (assetGuid != null && assetGuid.Count() > 0)
                    {
                        Count = assetGuid.FirstOrDefault().Count;
                        p_TotalViews += Count;
                    }

                    Series series = new Series();
                    series.data = new List<HighChartDatum>();

                    HighChartDatum highChartDatum = new HighChartDatum();
                    highChartDatum.y = Count;
                    series.data.Add(highChartDatum);

                    seriesTVViews.data.Add(highChartDatum);
                }

                List<string> categories = new List<string>();

                foreach (var date in dateRange)
                {
                    categories.Add(date.ToShortDateString());
                }

                // this is signle line spark chart , different for each medium type (with all commom properties set here)
                HighLineChartOutput highLineChartTVViewsChartOutput = new HighLineChartOutput();
                highLineChartTVViewsChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartTVViewsChartOutput.subtitle = new Subtitle() { text = "", x = -20 };


                // set min = 0 , to force chart to start from 0 , and show line in bottom, 
                // gridLineWidth = 0 , to hide grid lines on y axis. 
                highLineChartTVViewsChartOutput.yAxis = new List<YAxis>() { new YAxis() { min = 0, gridLineWidth = 0, title = new Title2() } };

                // not to show x axis labels for spark charts 
                // we have set default value for TickWidth to 0 in XAxis class defination, to not to show line below x-axis for ticks.
                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartTVViewsChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 45)),
                    tickmarkPlacement = "off",
                    categories = categories,
                    labels = new labels()
                    {
                        rotation = 270,
                        enabled = false
                    }
                };

                // add event on chart click , to load medium type summary of selected medium chart. 
                highLineChartTVViewsChartOutput.hChart = new HChart()
                {
                    events = new PlotEvents()
                    {
                        click = ""
                    },
                    height = 100,
                    width = 250,
                    type = "spline"
                };
                highLineChartTVViewsChartOutput.tooltip = new Tooltip() { valueSuffix = "" };


                // add event on chart points click , to load medium type summary of selected medium chart. 
                // also disble marker on chart
                highLineChartTVViewsChartOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        point = new PlotPoint()
                        {
                            events = new PlotEvents()
                            {
                                click = ""
                            }
                        },
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };
                highLineChartTVViewsChartOutput.legend = new Legend() { enabled = false };

                List<Series> lstSeriesTVViews = new List<Series>();
                lstSeriesTVViews.Add(seriesTVViews);
                highLineChartTVViewsChartOutput.series = lstSeriesTVViews;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartTVViewsChartOutput);

                return jsonResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Demographics Fusion Map
        public string GetDemographicsFusionMap(Dictionary<string, long> regionPlayMapList)
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
                fusionMap.showlegend = "0";
                fusionMap.showshadow = "0";
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
                foreach (KeyValuePair<string, string> keyval in IQStateToFusionIDMapModel.IQStateToFusionIDMap)
                {
                    FusionMapData fusionMapData = new FusionMapData();
                    long plays = 0;
                    regionPlayMapList.TryGetValue(keyval.Key, out plays);

                    fusionMapData.id = keyval.Value;
                    fusionMapData.value = plays.ToString();
                    fusionMapData.showEntityToolTip = "1";
                    fusionMapData.showlabel = "0";

                    if (plays  > maxValue)
                    {
                        maxValue = plays;
                    }

                    if (plays < minValue)
                    {
                        minValue = plays;
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

        #endregion

        public Dictionary<Guid, string> GetClipCCLocation(Guid p_ClientGUID, IEnumerable<Guid> p_ClipGUIDList)
        {
            ClipDA clipDA = (ClipDA)DataAccessFactory.GetDataAccess(DataAccessType.Clip);

            XDocument doc = new XDocument(new XElement("list", from i in p_ClipGUIDList select new XElement("item", new XAttribute("ClipGUID", i))));

            return clipDA.GetClipCCLocation(p_ClientGUID, doc.ToString());
        }

        #endregion

        public int UpdateClipDownloadCCStatus(Int64 p_ClipDownloadKey, Guid p_ClipGUID)
        {
            ClipDA clipDA = (ClipDA)DataAccessFactory.GetDataAccess(DataAccessType.Clip);
            return clipDA.UpdateClipDownloadCCStatus(p_ClipDownloadKey, p_ClipGUID);
        }

        public int UpdateRadioClipDownloadCCStatus(Int64 p_ClipDownloadKey, Guid p_ClipGUID)
        {
            ClipDA clipDA = (ClipDA)DataAccessFactory.GetDataAccess(DataAccessType.Clip);
            return clipDA.UpdateRadioClipDownloadCCStatus(p_ClipDownloadKey, p_ClipGUID);
        }
    }
}
