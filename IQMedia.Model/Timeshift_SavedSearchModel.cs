using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMedia.Model
{
    [Serializable]
    public class Timeshift_SavedSearchModel
    {
        public Int64 ID { get; set; }

        public string Title { get; set; }

        public Guid CustomerGuid { get; set; }

        public Guid ClientGuid { get; set; }

        public TimeShiftSearchTerm SearchTerm { get; set; }
    }

    [Serializable]
    public class TimeShiftSearchTerm
    {
        public string SearchTerm
        {
            get { return _SearchTerm; }
            set { _SearchTerm = value; }
        } string _SearchTerm = string.Empty;

        public string Title120
        {
            get { return _Title120; }
            set { _Title120 = value; }
        } string _Title120 = string.Empty;

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        } string _Category = string.Empty;

        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        } string _CategoryName = string.Empty;

        public int? Country
        {
            get { return _Country; }
            set { _Country = value; }
        } int? _Country = null;

        public string CountryName
        {
            get { return _CountryName; }
            set { _CountryName = value; }
        } string _CountryName = string.Empty;

        public int? Region
        {
            get { return _Region; }
            set { _Region = value; }
        } int? _Region = null;

        public string RegionName
        {
            get { return _RegionName; }
            set { _RegionName = value; }
        } string _RegionName = string.Empty;

        [XmlArray("DmaList")]
        [XmlArrayItem("Dma")]
        public List<string> Dma { get; set; }

        [XmlArray("StationList")]
        [XmlArrayItem("Station")]
        public List<string> Station { get; set; }

        [XmlArray("IQStationIDList")]
        [XmlArrayItem("IQStation")]
        public List<IQ_Station> IQStationID { get; set; }
    }
}
