using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IQMedia.Model
{
    [DataContract]
    public class ColumnChartOutput
    {
        [DataMember(Name = "chart")]
        public chart chartdata { get; set; }

        [DataMember(Name = "data")]
        public List<data> lstdata { get; set; }

        [DataMember()]
        public Credits credits
        {
            get { return _credits; }
            set { _credits = value; }
        } Credits _credits = new Credits();
    }

    [DataContract]
    public class chart
    {
        [DataMember(Name = "yaxisname")]
        public string yaxisname { get; set; }

        [DataMember(Name = "caption")]
        public string caption { get; set; }

        [DataMember(Name = "bgcolor")]
        public string bgcolor { get; set; }

        [DataMember(Name = "alternatehgridcolor")]
        public string alternatehgridcolor { get; set; }

        [DataMember(Name = "divLineThickness")]
        public string divLineThickness { get; set; }

        [DataMember(Name = "showBorder")]
        public string showBorder { get; set; }

        [DataMember(Name = "canvasBorderAlpha")]
        public string canvasBorderAlpha { get; set; }

        [DataMember(Name = "showYAxisValues")]
        public string showYAxisValues { get; set; }

        [DataMember(Name = "showLegend")]
        public string showLegend { get; set; }

        [DataMember(Name = "legendPosition")]
        public string legendPosition { get; set; }

        [DataMember(Name = "rotateValues")]
        public string rotateValues
        {
            get { return _rotateValues; }
            set { _rotateValues = value; }
        }
        private string _rotateValues = "0";



    }

    public class data
    {
        public string label { get; set; }
        public string value { get; set; }
    }
}
