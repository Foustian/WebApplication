using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IQMedia.Model
{
    public class PChart
    {
        public object plotBackgroundColor { get; set; }
        public object plotBorderWidth { get; set; }
        public bool plotShadow { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class PTitle
    {
        public string text { get; set; }
        public HStyle style { get; set; }
    }

    public class PTooltip
    {
        public string pointFormat { get; set; }
    }

    public class DataLabels
    {
        public bool enabled { get; set; }
    }

    public class Pie
    {
        public bool allowPointSelect { get; set; }
        public string cursor { get; set; }
        public DataLabels dataLabels { get; set; }
        public bool showInLegend { get; set; }
        public string innerSize { get; set; }
        public string size { get; set; } 
    }

    public class PPlotOptions
    {
        public Pie pie { get; set; }
    }

    public class PSeries
    {
        public string type { get; set; }
        public string name { get; set; }
        public List<object> data { get; set; }
    }

    public class HighPieChartModel
    {
        public PChart chart { get; set; }
        public PTitle title { get; set; }
        public PTooltip tooltip { get; set; }
        public PPlotOptions plotOptions { get; set; }
        public List<PSeries> series { get; set; }

        public List<string> colors
        {
            get { return _colors; }
            set { _colors = value; }
        } List<string> _colors = new List<string>() { "#598ea2", "#f3b350", "#c7d36a", "#b4b4da", "#d8635d", "#f3da72", "#9ad1dc", "#e1cba4", "#ff9bb8", "#808285", "#da3ab3", "#6ecdb2", "#e2cc00", "#ff6c36", "#3b5cad", "#9778d3", "#00bfd6", "#5b2c3f", "#4c8b2b", "#d9a460" };

        public Legend legend { get; set; }

        public Credits credits
        {
            get { return _credits; }
            set { _credits = value; }
        } Credits _credits = new Credits();
    }

    public class PSeriesData
    {
        public string SeriesName { get; set; }
        public double SeriesValue { get; set; }
    }
}
