using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IQMedia.Model
{
    [DataContract]
    public class PieChartOutput
    {
        [DataMember(Name = "chart")]
        public Piechart pieChart { get; set; }

        [DataMember(Name = "data")]
        public List<PieChartdata> lstPieChartData { get; set; }
    }

    public class Piechart
    {
        public string caption { get; set; }
        public string showpercentageinlabel { get; set; }
        public string showvalues { get; set; }
        public string showlabels { get; set; }
        public string showlegend { get; set; }
        public string bgcolor { get; set; }
        public string showBorder { get; set; }
        public string pieRadius { get; set; }
        public string paletteColors { get; set; }
        public string plotFillAlpha { get; set; }
        public string legendPosition
        {
            get { return _legendPosition; }
            set { _legendPosition = value; }
        }
        private string _legendPosition = "BOTTOM";
        public string legendBorderThickness
        {
            get { return _legendBorderThickness; }
            set { _legendBorderThickness = value; }
        }
        private string _legendBorderThickness = "1";

        public string legendShadow
        {
            get { return _legendShadow; }
            set { _legendShadow = value; }
        }
        private string _legendShadow = "1";

    }

    public class PieChartdata
    {
        public Int64 value { get; set; }
        public string label { get; set; }
    }

    public class PieChartTotal
    {
        public string date { get; set; }
        public string searchName { get; set; }
        public string searchTerm { get; set; }
        public string medium { get; set; }
        public Int64 totalResult { get; set; }
    }
}
