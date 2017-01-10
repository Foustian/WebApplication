using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IQMedia.Model
{
    [DataContract]
    public class HighHeatMapModel
    {
        [DataMember(EmitDefaultValue = false, Name = "chart")]
        public HChart hChart { get; set; }

        [DataMember(Name = "title")]
        public Title title { get; set; }

        [DataMember(Name = "xAxis")]
        public XAxis xAxis { get; set; }

        [DataMember(Name = "yAxis")]
        public HeatMapYAxis yAxis { get; set; }

        [DataMember(Name = "colorAxis")]
        public ColorAxis colorAxis { get; set; }

        [DataMember(Name = "legend", EmitDefaultValue = false)]
        public HeatMapLegend legend { get; set; }

        [DataMember(Name = "tooltip")]
        public Tooltip tooltip { get; set; }

        [DataMember(Name = "series")]
        public List<HeatMapSeries> series { get; set; }

        // Disables Highcharts.com link
        [DataMember()]
        public Credits credits
        {
            get { return _credits; }
            set { _credits = value; }
        } Credits _credits = new Credits();
    }

    public class HeatMapYAxis
    {
        public Title2 title { get; set; }

        public List<string> categories { get; set; }
    }

    public class ColorAxis
    {
        public int min { get; set; }

        public string minColor { get; set; }

        public string maxColor { get; set; }
    }

    public class HeatMapLegend : Legend
    {
        public int y { get; set; }

        public int symbolHeight { get; set; }
    }

    public class HeatMapSeries
    {
        public string name { get; set; }

        public int borderWidth { get; set; }

        public string borderColor { get; set; }

        public List<HeatMapDatum> data { get; set; }

        public HeatMapDataLabels dataLabels
        {
            get { return _dataLabels; }
            set { _dataLabels = value; }
        } HeatMapDataLabels _dataLabels = new HeatMapDataLabels();
    }

    public class HeatMapDatum
    {
        public int y { get; set; }

        public int x { get; set; }

        public long value { get; set; }

        public int borderWidth { get; set; }

        public string borderColor { get; set; }

        public string name { get; set; }

        public string code { get; set; }

        public string colorcode { get; set; }
    }

    public class HeatMapDataLabels
    {
        public bool enabled { get; set; }

        public string color { get; set; }

        public string formatter { get; set; }

        public HeatStyle style
        {
            get { return _style; }
            set { _style = value; }
        } HeatStyle _style = new HeatStyle();
    }

    public class HeatStyle
    {
        public string color { get; set; }
        public string fontWeight { get; set; }
        public string fontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; }
        } string _fontFamily = "Open Sans";
        public string fontSize { get; set; }
        public string textShadow { get; set; }
    }
}
