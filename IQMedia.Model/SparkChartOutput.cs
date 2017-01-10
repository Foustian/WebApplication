using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace IQMedia.Model
{
    //class SparkChartOutput
    //{
    //}
    [DataContract]
    public class SparkChartOutput
    {
        [DataMember(Name = "chart")]
        public SparkChart chart { get; set; }

        //[DataMember(Name = "categories")]
        //public List<AllCategory> categories { get; set; }

        [DataMember(Name = "dataset")]
        public List<SparkSeriesData> dataset { get; set; }

        //public Styles styles { get; set; }
    }

    public class SparkChart
    {

        public string caption { get; set; }

        [DefaultValue("0")]
        public string showOpenAnchor { get; set; }

        [DefaultValue("0")]
        public string showCloseAnchor { get; set; }

        [DefaultValue("0")]
        public string showHighAnchor { get; set; }

        [DefaultValue("0")]
        public string showLowAnchor { get; set; }

        [DefaultValue("0")]
        public string showOpenValue { get; set; }

        [DefaultValue("0")]
        public string showCloseValue { get; set; }

        [DefaultValue("0")]
        public string showHighLowValue { get; set; }

        [DefaultValue("1")]
        public string showToolTip { get; set; }
        //public string formatNumber { get; set; }
        //public string thousandSeparator { get; set; }

        [DefaultValue("0")]
        public string thousandSeparatorPosition { get; set; }


        [DefaultValue("0")]
        public string formatNumberScale { get; set; }

        [DefaultValue("#FFFFFF")]
        public string bgColor { get; set; }


        [DefaultValue("#4493D6")]
        public string lineColor { get; set; }

        [DefaultValue("5")]
        public string palette
        {
            get;
            set;
        }


        [DefaultValue("0")]
        public string setAdaptiveYMin
        {
            get;
            set;
        }

        public string captionPadding { get; set; }

        public string baseFontColor { get; set; }

        public SparkChart()
        {
            this.showOpenAnchor = "0";
            this.showCloseAnchor = "0";
            this.showHighAnchor = "0";
            this.showLowAnchor = "0";
            this.showOpenValue = "0";
            this.showCloseValue = "0";
            this.showHighLowValue = "0";
            this.showToolTip = "0";
            this.showToolTip = "1";
            this.thousandSeparatorPosition = "0";
            this.formatNumberScale = "0";
            this.bgColor = "#FFFFFF";
            this.lineColor = "#4493D6";
            this.palette = "5";
            this.setAdaptiveYMin = "1";
            this.captionPadding = "0";
            this.baseFontColor = "#555555";
            this.caption = "";
        }

    }
    public class SparkSeriesData
    {
        public List<SparkDatum> data { get; set; }
    }

    public class SparkDatum
    {
        public string value { get; set; }
    }


}
