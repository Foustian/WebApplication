using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;
using IQMedia.Shared.Utility;

namespace IQMedia.Web.Logic
{
    public class IQ_QVCDataLogic : ILogic
    {
        public List<string> GetQVCStations(Int16 p_DataType)
        {
            IQ_QVCDataDA iQ_KantorDataDA = (IQ_QVCDataDA)DataAccessFactory.GetDataAccess(DataAccessType.QVCData);
            return iQ_KantorDataDA.GetQVCStations(p_DataType);
        }

        public List<string> GetQVCIQ_CC_Keys(string p_StationID, DateTime p_DataTime, Int16 p_DataType)
        {
            IQ_QVCDataDA iQ_KantorDataDA = (IQ_QVCDataDA)DataAccessFactory.GetDataAccess(DataAccessType.QVCData);
            return iQ_KantorDataDA.GetQVCIQ_CC_Keys(p_StationID, p_DataTime, p_DataType);
        }

        public string GetQVCAudienceDataByIQ_CC_Key(string p_IQ_CC_Key, Int16 p_DataType)
        {
            IQ_QVCDataDA iQ_KantorDataDA = (IQ_QVCDataDA)DataAccessFactory.GetDataAccess(DataAccessType.QVCData);
            return iQ_KantorDataDA.GetQVCAudienceDataByIQ_CC_Key(p_IQ_CC_Key, p_DataType);
        }

        public string KantorHighLineChart(string p_KantorAudience,bool isMultiYAxes = false)
        {
            try
            {
                IQ_KantorAudienceDataModel iQ_KantorAudienceDataModel = new IQ_KantorAudienceDataModel();
                iQ_KantorAudienceDataModel = (IQ_KantorAudienceDataModel)Newtonsoft.Json.JsonConvert.DeserializeObject(p_KantorAudience, iQ_KantorAudienceDataModel.GetType());

                List<string> categories = new List<string>();

                Series series = new Series();
                series.data = new List<HighChartDatum>();
                series.name = " ";

                Series series2 = new Series();
                series2.data = new List<HighChartDatum>();
                series2.name = " ";
                if (isMultiYAxes)
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
                    text = "",
                    x = -20,
                    style = new HStyle
                    {
                        color = "#555555",
                        fontWeight = "bold",
                        fontFamily = "Verdana",
                        fontSize = "13px"
                    }
                };
                highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 120)),
                    tickmarkPlacement = "off",
                    //tickPixelInterval = 50,
                    categories = categories,
                    labels = new labels()
                    {
                        rotation = 0,
                        formatter  ="FormatTime"
                    },
                    tickWidth =1
                };

                if (!isMultiYAxes)
                {
                    highLineChartOutput.yAxis = new List<YAxis>() { new YAxis{
                            title = new Title2(){ text="Audience" }
                        }
                    };
                }
                else
                {
                    highLineChartOutput.yAxis = new List<YAxis>() { new YAxis{
                        title = new Title2(){ text="Kantar Ratings" }
                    },
                    new YAxis{
                        title = new Title2(){text ="Kantar Audience" }                        
                    },
                    };
                }

                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { formatter = "tooltipFormat", shared = true, crosshairs = true /*, positioner ="TooltipSetY"*/ };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { borderWidth = "0", x = - (CommonFunctions.KantorChartWidth / 2) + 100 };

                // set chart with height and width
                highLineChartOutput.hChart = new HChart() { height = 300, width = CommonFunctions.KantorChartWidth, zoomType = "x" };

                // set plot options and click event for series points (which will again assigned in JS as this is string value)
                // legendItemClick event to show / hide column chart series on line legend click
                highLineChartOutput.plotOption = new PlotOptions()
                {
                    column = null,
                    series = new PlotSeries()
                    {
                        events = new PlotEvents(){
                            hide ="HandleSeriesHide",
                            show = "HandleSeriesShow"
                        },
                        marker = new PlotMarker{
                            enabled = false,
                            radius = 2,
                        },
                        states = new PlotSeriesStates()
                        {
                            hover = new SeriesState()
                            {
                                halo = new PlotStateHalo(){
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

        public string QVCHighLineChart(string p_QVCAudience)
        {
            try
            {
                IQ_QVCDataModel iQ_QVCDataModel = new IQ_QVCDataModel();
                iQ_QVCDataModel = (IQ_QVCDataModel)Newtonsoft.Json.JsonConvert.DeserializeObject(p_QVCAudience, iQ_QVCDataModel.GetType());

                List<string> categories = new List<string>();

                Series seriesAudience = new Series();
                seriesAudience.data = new List<HighChartDatum>();

                Series seriesCallLogs = new Series();
                seriesCallLogs.data = new List<HighChartDatum>();

                Series seriesTweet = new Series();
                seriesTweet.data = new List<HighChartDatum>();

                List<Series> lstSeries = new List<Series>();

                if (iQ_QVCDataModel != null && iQ_QVCDataModel.Data != null)
                {
                    categories = iQ_QVCDataModel.Data.Select(a => a.M.ToString()).ToList();

                    seriesAudience.data = iQ_QVCDataModel.Data.Select(a => new HighChartDatum() { y = Convert.ToDecimal(a.A), Value = Convert.ToString(a.A) }).ToList();
                    seriesAudience.yAxis = 0;
                    seriesAudience.name = "Audience";
                    lstSeries.Add(seriesAudience);

                    seriesCallLogs.data = iQ_QVCDataModel.Data.Select(a => new HighChartDatum() { y = Convert.ToDecimal(a.CL), Value = Convert.ToString(a.CL) }).ToList();
                    seriesCallLogs.yAxis = 0;
                    seriesCallLogs.name = "Call Logs";
                    lstSeries.Add(seriesCallLogs);

                    seriesTweet.data = iQ_QVCDataModel.Data.Select(a => new HighChartDatum() { y = Convert.ToDecimal(a.T), Value = Convert.ToString(a.T) }).ToList();
                    seriesTweet.yAxis = 0;
                    seriesTweet.name = "Tweets";
                    lstSeries.Add(seriesTweet);

                    List<string> colors = new List<string> { "#0088CC", "#D70000", "#FB4801", "#FECB00", "#CFFD00", "#87FE00", "#1EFD01", "#00FDD7", "#001EFE", "#6750FE", "#7E00FD", "#E939FD", "#FE3790", "#86B549", "#D3CE36", "#3F7AA4", "#60635E", "#4E5A63", "#007DD7", "#E23D31", "#FD7945", "#FDD73D", "#E4FC7A", "#B0FB5A", "#70FC5D", "#79FDE9", "#4359FE", "#8A78FD", "#A54CFD", "#EF6CFE", "#FE74B2", "#94B566", "#D2CF6E", "#5985A5", "#7F857C", "#62727D", "#ff0026","#50d07d","#d02552","#b2cecf","#DC3D24","#232B2B","#FFFFFF","#E3AE57","#221E1D","#ECEAE0","#63AA9C","#E9633B","#222930","#4EB1BA","#E9E9E9","#E4E4E4","#F1684E","#1B1B1B", "#763A7A","#0288AD","#F6F3EC","#AC2832","#000000","#DFD297","#F2EFE4","#CB8C1D","#4C3327","#BD3632","#DCD8CF","#E25D33","#282827","#B94629","#E3DEC1","#E89F65","#E89F65","#47AFAF" };

                    for (int i = 0; i <iQ_QVCDataModel.Data[0].labelData.Count; i++)
                    {
                        Series lblSeriesSA = new Series() { color = colors[i] };
                        lblSeriesSA.data = new List<HighChartDatum>();
                        lblSeriesSA.name = iQ_QVCDataModel.Appendix[i].Label;

/*                        Series lblSeriesSQ = new Series() { color = colors[i + 1] };
                        lblSeriesSQ.data = new List<HighChartDatum>();
                        lblSeriesSQ.name = iQ_QVCDataModel.Appendix[i].Label;*/


                        for (int j = 0; j < categories.Count; j++)
                        {
                            lblSeriesSA.data.Add(new HighChartDatum() { y = Convert.ToDecimal(iQ_QVCDataModel.Data[j].labelData.Where(l => l.I == i).FirstOrDefault().SA), Value = "Amt = " + iQ_QVCDataModel.Data[j].labelData.Where(l => l.I == i).FirstOrDefault().SA + ", Qty = " + iQ_QVCDataModel.Data[j].labelData.Where(l => l.I == i).FirstOrDefault().SQ});
                            //lblSeriesSQ.data.Add(new HighChartDatum() { y = Convert.ToDecimal(iQ_QVCDataModel.Data[j].labelData.Where(l => l.I == i).FirstOrDefault().SQ) });
                        }
                        
                        lblSeriesSA.yAxis = 1;
                        //lblSeriesSQ.yAxis = 1;
                        lstSeries.Add(lblSeriesSA);
                        //lstSeries.Add(lblSeriesSQ);
                    }
                }

                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();

                // set chart title and title style
                highLineChartOutput.title = new Title()
                {
                    text = "",
                    x = -20,
                    style = new HStyle
                    {
                        color = "#555555",
                        fontWeight = "bold",
                        fontFamily = "Verdana",
                        fontSize = "13px"
                    }
                };
                highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 120)),
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

                
                    highLineChartOutput.yAxis = new List<YAxis>() { new YAxis{
                        title = new Title2(){text ="Audience/#Call/Tweets" }                        
                    },
                    new YAxis{
                        title = new Title2(){text ="Amount" }                        
                    }
                    };

                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { formatter = "tooltipFormat", shared = true, crosshairs = true /*, positioner ="TooltipSetY"*/ };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { borderWidth = "0", x = 0, verticalAlign="bottom", align="left" };

                // set chart with height and width
                highLineChartOutput.hChart = new HChart() { height = 500, width = CommonFunctions.KantorChartWidth, zoomType = "x" };

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
