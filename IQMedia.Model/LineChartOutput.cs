using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IQMedia.Model
{

    [DataContract]
    public class LineChartOutput
    {
        [DataMember(Name = "chart")]
        public Chart chart { get; set; }

        [DataMember(Name = "categories")]
        public List<AllCategory> categories { get; set; }

        [DataMember(Name = "dataset")]
        public List<SeriesData> dataset { get; set; }

        public Styles styles { get; set; }
    }

    public class Chart
    {
        public string caption { get; set; }
        public string subcaption { get; set; }
        public string linethickness { get; set; }
        public string showvalues { get; set; }
        public string formatnumberscale { get; set; }
        public string anchorRadius { get; set; }
        public string divlinealpha { get; set; }
        public string divlinecolor { get; set; }
        public string divlineisdashed { get; set; }
        public string showalternatehgridcolor { get; set; }
        public string alternatehgridcolor { get; set; }
        public string shadowalpha { get; set; }
        public string labelstep { get; set; }
        public string numvdivlines { get; set; }
        public string chartrightmargin { get; set; }
        public string bgcolor { get; set; }
        public string bgangle { get; set; }
        public string bgalpha { get; set; }
        public string alternatehgridalpha { get; set; }
        public string legendposition { get; set; }
        public string drawAnchors { get; set; }
        public string showBorder { get; set; }
        public string canvasBorderAlpha { get; set; }
        public string palettecolors 
        {
            get { return _paletteColors; }
            set { _paletteColors = value; }
        } private string _paletteColors = "";
             
        public string showlegend
        {
            get { return _showlegend; }
            set { _showlegend = value; }
        }
        private string _showlegend = "1";

        public string lineColor
        {
            get { return _lineColor; }
            set { _lineColor = value; }
        }
        private string _lineColor = "";

        public string showLabels
        {
            get { return _showLabels; }
            set { _showLabels = value; }
        }
        private string _showLabels = "1";

        public string showYAxisValues
        {
            get { return _showYAxisValues; }
            set { _showYAxisValues = value; }
        }
        private string _showYAxisValues = "1";

        public string canvasRightMargin
        {
            get { return _canvasRightMargin; }
            set { _canvasRightMargin = value; }
        }
        private string _canvasRightMargin = "0";


        public string legendBorderThickness
        {
            get { return _legendBorderThickness; }
            set { _legendBorderThickness = value; }
        }
        private string _legendBorderThickness = "0";


        public string legendShadow
        {
            get { return _legendShadow; }
            set { _legendShadow = value; }
        }
        private string _legendShadow = "0";

        public string numVisibleLabels
        {
            get { return _numVisibleLabels; }
            set { _numVisibleLabels = value; }
        }
        private string _numVisibleLabels = "10";


    }

    public class Category2
    {
        public string label { get; set; }
    }

    public class AllCategory
    {
        public List<Category2> category { get; set; }
    }

    public class Datum
    {
        public string value { get; set; }
        public string link { get; set; }
    }

    public class SeriesData
    {
        public string seriesname { get; set; }
        public string color { get; set; }
        /*public string anchorBorderColor
        {
            get { return _anchorBorderColor; }
            set { _anchorBorderColor = value; }
        }*/
        private string _anchorBorderColor = "#D1E9FB";

        public string anchorAlpha
        {
            get { return _anchorAlpha; }
            set { _anchorAlpha = value; }
        }
        private string _anchorAlpha = "100";

        /*public string anchorBgColor
        {
            get { return _anchorBgColor; }
            set { _anchorBgColor = value; }
        }*/
        private string _anchorBgColor = "#D1E9FB";

        public List<Datum> data { get; set; }
    }

    public class Definition
    {
        public string name { get; set; }
        public string type { get; set; }
        public string size { get; set; }
    }

    public class Application
    {
        public string toobject { get; set; }
        public string styles { get; set; }
    }

    public class Styles
    {
        public List<Definition> definition { get; set; }
        public List<Application> application { get; set; }
    }

}
