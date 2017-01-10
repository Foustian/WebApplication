using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IQMedia.Model
{

    [DataContract]
    public class HighLineChartOutput
    {
        [DataMember(Name = "title")]
        public Title title { get; set; }

        [DataMember(Name = "subtitle")]
        public Subtitle subtitle { get; set; }

        [DataMember(Name = "xAxis")]
        public XAxis xAxis { get; set; }

        [DataMember(Name = "yAxis")]
        public List<YAxis> yAxis { get; set; }

        [DataMember(Name = "tooltip")]
        public Tooltip tooltip { get; set; }

        [DataMember(Name = "legend",EmitDefaultValue=false)]
        public Legend legend { get; set; }

        [DataMember(Name = "series")]
        public List<Series> series { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "chart")]
        public HChart hChart { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "colors")]
        public List<string> colors
        {
            get { return _colors; }
            set { _colors = value; }
        } List<string> _colors = new List<string>() { "#598ea2", "#f3b350", "#c7d36a", "#b4b4da", "#d8635d", "#f3da72", "#9ad1dc", "#e1cba4", "#ff9bb8", "#808285", "#da3ab3", "#6ecdb2", "#e2cc00", "#ff6c36", "#3b5cad", "#9778d3", "#00bfd6", "#5b2c3f", "#4c8b2b", "#d9a460" };

        [DataMember(Name = "plotOptions")]
        public PlotOptions plotOption { get; set; }

        [DataMember()]
        public Credits credits
        {
            get { return _credits; }
            set { _credits = value; }
        } Credits _credits = new Credits();
    }

    [DataContract]
    public class HChart
    {
        [DataMember(EmitDefaultValue = false)]
        public string type { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? height { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? width { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public PlotEvents events { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int borderWidth { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string borderColor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string backgroundColor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string zoomType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int marginRight { get; set; }
    }

    [DataContract]
    public class Title
    {
        [DataMember(EmitDefaultValue=false)]
        public string text { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int x { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public HStyle style { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? y { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string align { get; set; }

    }

    public class HStyle
    {
        public string color { get; set; }
        public string fontWeight { get; set; }
        public string fontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; }
        } string _fontFamily = "Open Sans";
        public string fontSize { get; set; }
        public string width { get; set; }
        public string minWidth { get; set; }
        public string textAlign { get; set; }
    }

    [DataContract]
    public class Subtitle
    {
        [DataMember(EmitDefaultValue = false)]
        public string text { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int x { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? y { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string align { get; set; }
    }

    public class PlotBand
    {
        public int from { get; set; }
        public int to { get; set; }
        public string color { get; set; }
    }

    public class XAxis
    {
        [DataMember(EmitDefaultValue = false)]
        public Title2 title { get; set; }

        public List<string> categories { get; set; }

        [DataMember(Name = "labels", EmitDefaultValue = false)]
        public labels labels { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int tickInterval { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string tickmarkPlacement { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string type { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateLabelFormat dateTimeLabelFormats { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? tickWidth 
        {
            get { return _tickWidth; }
            set { _tickWidth = value; }
        }int? _tickWidth = 0;

        [DataMember(EmitDefaultValue = false)]
        public int? minorGridLineWidth { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? gridLineWidth { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string gridLineColor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string gridLineDashStyle { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? minorTickInterval { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<PlotBand> plotBands { get; set; }
    }

    public class labels
    {
        public int? rotation 
        {
            get { return _rotation; }
            set { _rotation = value; } 
        } int? _rotation = 0;
        public bool enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }bool _enabled = true;

        public string formatter { get; set; }

        public bool useHTML { get; set; }

        public int staggerLines { get; set; }

        public HStyle style
        {
            get { return _style; }
            set { _style = value; }
        } HStyle _style = new HStyle();
    }

    public class Title2
    {

        public string text { get; set; }
        //{
        //    get { return _text; }
        //    set { _text = value; }
        //}string _text = " ";

        public int rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        } int _rotation = 270;
    }

    public class PlotLine
    {
        public string value { get; set; }
        public string width { get; set; }
        public string color { get; set; }
    }

    [DataContract]
    public class YAxis
    {
        [DataMember(EmitDefaultValue = false)]
        public Title2 title { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<PlotLine> plotLines { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool opposite { get; set; }

        public List<string> categories { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? min 
        {
            get { return _min; }
            set { _min = value; }
        }int? _min = null;

        [DataMember(EmitDefaultValue = false)]
        public int? max
        {
            get { return _max; }
            set { _max = value; }
        }int? _max = null;

        [DataMember(EmitDefaultValue = false)]
        public double? minRange 
        {
            get { return _minRange; }
            set { _minRange = value; }
        } double? _minRange = 0.1;

        [DataMember(EmitDefaultValue = false)]
        public int? lineWidth { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? minorGridLineWidth { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? tickInterval { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string lineColor
        {
            get { return _lineColor; }
            set { _lineColor = value; }
        }string _lineColor = null;

        [DataMember(EmitDefaultValue = false)]
        public int? gridLineWidth { get; set; }

        [DataMember(Name = "labels", EmitDefaultValue = false)]
        public labels labels { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool allowDecimals
        {
            get { return _allowDecimals; }
            set { _allowDecimals = value; }
        }bool _allowDecimals = true;
    }

    [DataContract]
    public class Tooltip
    {
        [DataMember(EmitDefaultValue = false)]
        public string valueSuffix { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string pointFormat { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool shared { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool crosshairs { get; set; }

        [DataMember(EmitDefaultValue=false)]
        public bool? useHTML { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string formatter { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string positioner { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string valuePrefix { get; set; }
    }

    [DataContract]
    public class Legend
    {
        [DataMember(EmitDefaultValue = false)]
        public string layout { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string align { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string verticalAlign { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string borderWidth { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? width { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool? enabled { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int x { get; set; }

    }

    [DataContract]
    public class Series
    {
        [DataMember(EmitDefaultValue = false)]
        public string name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<HighChartDatum> data { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string color { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int yAxis { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Tooltip tooltip { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string dashStyle { get; set; }
    }

    public class HighChartDatum
    {
        public decimal? y { get; set; }
        
        public string SearchTerm { get; set; }

        public string SearchName { get; set; }

        public string Medium { get; set; }

        public string MediumName { get; set; }
        
        public string Value { get; set; }

        public string Type { get; set; }

        public string Date { get; set; }
    }

    [DataContract]
    public class PlotEvents
    {
        [DataMember(Name = "click",EmitDefaultValue=false)]
        public string click { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string legendItemClick { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string load { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string redraw { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string mouseOver { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string mouseOut { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string hide { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string show { get; set; }
    }

    [DataContract]
    public class PlotPoint
    {
        [DataMember(Name = "events")]
        public PlotEvents events { get; set; }
    }

    [DataContract]
    public class PlotSeries
    {
        [DataMember(EmitDefaultValue = false, Name = "cursor")]
        public string cursor { get; set; }

        [DataMember(EmitDefaultValue=false,Name = "point")]
        public PlotPoint point { get; set; }

        [DataMember(EmitDefaultValue=false)]
        public PlotMarker marker 
        {
            get { return _marker; }
            set { _marker = value; }
        } PlotMarker _marker = new PlotMarker();

        [DataMember(EmitDefaultValue = false)]
        public int turboThreshold
        {
            get { return _turboThreshold; }
            set { _turboThreshold = value; }
        }int _turboThreshold = 50000;

        [DataMember(EmitDefaultValue = false)]
        public PlotEvents events { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public PlotSeriesStates states { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int pointWidth { get; set; }

        [DataMember(Name = "lineWidth")]
        public int lineWidth 
        {
            get { return _lineWidth; }
            set { _lineWidth = value; }
        } int _lineWidth = 2;
    }

    public class PlotMarker
    {
        public int lineWidth
        {
            get { return _linewidth; }
            set { _linewidth = value; }
        } int _linewidth = 1;

        public bool enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }bool _enabled = false;

        public string symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }string _symbol = "circle";

        public int? radius
        {
            get { return _radius; }
            set { _radius = value; }
        }int? _radius = 3;
    }

    [DataContract]
    public class DateLabelFormat
    {
        [DataMember(EmitDefaultValue = false)]
        public string millisecond { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string second { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string minute { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string hour { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string day { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string week { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string month { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string year { get; set; }
    }


    [DataContract]
    public class Credits
    {
        [DataMember]
        public bool enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }bool _enabled = false;
    }
}
