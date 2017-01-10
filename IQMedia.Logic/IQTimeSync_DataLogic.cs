using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Shared.Utility;

namespace IQMedia.Web.Logic
{
    public class IQTimeSync_DataLogic : ILogic
    {
        public List<IQTimeSync_DataModel> GetTimeSyncDataByIQCCKeyAndCustomerGuid(string IQ_CC_Key, Guid p_CustomerGuid)
        {
            IQTimeSync_DataDA iQTimeSync_DataDA = (IQTimeSync_DataDA)DataAccessFactory.GetDataAccess(DataAccessType.IQTimeSync_Data);
            return iQTimeSync_DataDA.GetTimeSyncDataByIQCCKeyAndCustomerGuid(IQ_CC_Key, p_CustomerGuid);
        }

        public List<IQTimeSync_DataModel> GetClipTimeSyncDataByClipGuidAndCustomerGuid(Guid p_ClipGuid, Guid p_CustomerGuid)
        {
            IQTimeSync_DataDA iQTimeSync_DataDA = (IQTimeSync_DataDA)DataAccessFactory.GetDataAccess(DataAccessType.IQTimeSync_Data);
            return iQTimeSync_DataDA.GetClipTimeSyncDataByClipGuidAndCustomerGuid(p_ClipGuid, p_CustomerGuid);
        }

        public string TimeSyncHighLineChart(string p_TimeSyncData, GraphStructureModel p_GraphStructureModel)
        {
            try
            {
                if (p_GraphStructureModel == null)
                {
                    p_GraphStructureModel = new GraphStructureModel();
                    p_GraphStructureModel.AudienceTooltipPrefix = "Kantar (second by second)";
                    p_GraphStructureModel.MediaValueXTooltipPrefix = "Nielsen (minute by minute)";
                    p_GraphStructureModel.AudienceXAxisLabel = null;
                    p_GraphStructureModel.MediaValueXAxisLabel = null;
                    p_GraphStructureModel.DefaultYAxisLabel = "Audience";
                    p_GraphStructureModel.SecondYAxisLabel = null;
                    p_GraphStructureModel.ChartHeaderTitle = null;
                    p_GraphStructureModel.ChartSubTitle = null;
                    p_GraphStructureModel.IsMultiAxis = false;

                }


                IQ_KantorAudienceDataModel iQ_KantorAudienceDataModel = new IQ_KantorAudienceDataModel();
                iQ_KantorAudienceDataModel = (IQ_KantorAudienceDataModel)Newtonsoft.Json.JsonConvert.DeserializeObject(p_TimeSyncData, iQ_KantorAudienceDataModel.GetType());

                List<string> categories = new List<string>();

                Series series = new Series();
                series.data = new List<HighChartDatum>();
                series.name = !string.IsNullOrEmpty(p_GraphStructureModel.AudienceXAxisLabel) ? p_GraphStructureModel.AudienceXAxisLabel : " ";
                series.tooltip = new Tooltip
                {
                    valuePrefix = !string.IsNullOrEmpty(p_GraphStructureModel.AudienceTooltipPrefix) ? p_GraphStructureModel.AudienceTooltipPrefix : "Kantar (second by second)"
                };

                Series series2 = new Series();
                series2.data = new List<HighChartDatum>();
                series2.name = !string.IsNullOrEmpty(p_GraphStructureModel.MediaValueXAxisLabel) ? p_GraphStructureModel.MediaValueXAxisLabel : " ";
                series2.tooltip = new Tooltip
                {
                    valuePrefix = !string.IsNullOrEmpty(p_GraphStructureModel.MediaValueXTooltipPrefix) ? p_GraphStructureModel.MediaValueXTooltipPrefix : "Nielsen (minute by minute)"
                };
                if (p_GraphStructureModel.IsMultiAxis)
                {
                    series2.yAxis = 1;
                }

                if (iQ_KantorAudienceDataModel != null && iQ_KantorAudienceDataModel.data != null)
                {
                    categories = iQ_KantorAudienceDataModel.data.Select(a => a.S.ToString()).ToList();

                    series.data = iQ_KantorAudienceDataModel.data.Select(a => new HighChartDatum() { y = Convert.ToDecimal(a.A) }).ToList();

                    series2.data = iQ_KantorAudienceDataModel.data.Select(a => new HighChartDatum() { y = Convert.ToDecimal(a.V) }).ToList();
                }

                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();

                // set chart title and title style
                highLineChartOutput.title = new Title()
                {
                    text = p_GraphStructureModel.ChartHeaderTitle,
                    x = 80,
                    y = 3,
                    align ="left",
                    style = new HStyle
                    {
                        color = "#555555",
                        fontWeight = "bold",
                        fontFamily = "Verdana",
                        fontSize = "13px"
                    }
                };
                highLineChartOutput.subtitle = new Subtitle() { text = p_GraphStructureModel.ChartSubTitle, x = 80, y = 20, align = "left", };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 12)),
                    tickmarkPlacement = "off",
                    //tickPixelInterval = 50,
                    categories = categories,
                    labels = new labels()
                    {
                        rotation = 0,
                        formatter = "FormatTime"
                    },
                    tickWidth = 1
                };

                if (!p_GraphStructureModel.IsMultiAxis)
                {
                    highLineChartOutput.yAxis = new List<YAxis>() { new YAxis{
                            title = new Title2(){ text=p_GraphStructureModel.DefaultYAxisLabel }
                        }
                    };
                }
                else
                {
                    highLineChartOutput.yAxis = new List<YAxis>() { new YAxis{
                        title = new Title2(){ text=p_GraphStructureModel.SecondYAxisLabel }
                    },
                    new YAxis{
                        title = new Title2(){text =p_GraphStructureModel.DefaultYAxisLabel }                        
                    },
                    };
                }

                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { formatter = "tooltipFormat", shared = true, crosshairs = true /*, positioner ="TooltipSetY"*/ };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { borderWidth = "0", x = -400 };

                // set chart with height and width
                highLineChartOutput.hChart = new HChart() { height = 300, zoomType = "x", width = 980 };

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
                        marker = new PlotMarker
                        {
                            enabled = false,
                            radius = 2,
                        },
                        states = new PlotSeriesStates()
                        {
                            hover = new SeriesState()
                            {
                                halo = new PlotStateHalo()
                                {
                                    size = 1
                                }
                            }
                        },
                        cursor = "pointer",
                        turboThreshold = 4000,
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

                // start to set series of data for multiline search term chart 
                List<Series> lstSeries = new List<Series>();

                lstSeries.Add(series);
                lstSeries.Add(series2);

                highLineChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
                return jsonResult;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
