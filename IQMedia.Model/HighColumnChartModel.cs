using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IQMedia.Model
{
    public class Column
    {
        public double pointPadding { get; set; }
        public int borderWidth { get; set; }
    }

    public class PlotOptions
    {
        public Column column { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string cursor { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "Series")]
        public PlotSeries series { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "Area")]
        public PlotSeries area { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "spline")]
        public PlotSeries spline { get; set; }
    }

    [DataContract]
    public class PlotSeriesStates
    {
        [DataMember(EmitDefaultValue = false)]
        public SeriesState hover { get; set; }
    }

    [DataContract]
    public class SeriesState {
        [DataMember(EmitDefaultValue = false)]
        public PlotStateHalo halo { get; set; }
    }

    [DataContract]
    public class PlotStateHalo
    {
        [DataMember(EmitDefaultValue = false)]
        public int size{get;set;}
    }

    [DataContract]

    public class HighColumnChartModel
    {
        [DataMember(Name = "chart")]
        public HChart chart { get; set; }

        [DataMember(Name = "title")]
        public Title title { get; set; }

        [DataMember(Name = "legend")]
        public Legend legend { get; set; }

        [DataMember(Name = "subtitle")]
        public Subtitle subtitle { get; set; }

        [DataMember(Name = "xAxis")]
        public XAxis xAxis { get; set; }

        [DataMember(Name = "yAxis")]
        public YAxis yAxis { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "tooltip")]
        public Tooltip tooltip { get; set; }

        [DataMember(Name = "plotOptions")]
        public PlotOptions plotOptions { get; set; }

        [DataMember(Name = "series")]
        public List<Series> series { get; set; }

        [DataMember()]
        public Credits credits
        {
            get { return _credits; }
            set { _credits = value; }
        } Credits _credits = new Credits();

        [DataMember(Name = "colors")]
        public List<string> colors
        {
            get { return _colors; }
            set { _colors = value; }
        } List<string> _colors = new List<string>() { "#598ea2", "#f3b350", "#c7d36a", "#b4b4da", "#d8635d", "#f3da72", "#9ad1dc", "#e1cba4", "#ff9bb8", "#808285", "#da3ab3", "#6ecdb2", "#e2cc00", "#ff6c36", "#3b5cad", "#9778d3", "#00bfd6", "#5b2c3f", "#4c8b2b", "#d9a460" };
    }

}
