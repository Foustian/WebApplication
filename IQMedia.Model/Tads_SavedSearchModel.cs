using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMedia.Model
{
    [Serializable]
    public class Tads_SavedSearchModel
    {
        public Int64 ID { get; set; }

        public string Title { get; set; }

        public Guid CustomerGuid { get; set; }

        public Guid ClientGuid { get; set; }

        public TadsSearchTerm SearchTerm { get; set; }
    }

    [Serializable]
    public class TadsSearchTerm
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

        [XmlArray("AffiliateList")]
        [XmlArrayItem("Affiliate")]
        public List<string> Affiliate { get; set; }

        [XmlArray("IQStationIDList")]
        [XmlArrayItem("IQStation")]
        public List<string> IQStationID { get; set; }

        [XmlArray("IndustryList")]
        [XmlArrayItem("Industry")]
        public List<string> IndustryID { get; set; }

        [XmlArray("IndustryNameList")]
        [XmlArrayItem("IndustryName")]
        public List<string> IndustryName { get; set; }

        [XmlArray("BrandList")]
        [XmlArrayItem("Brand")]
        public List<string> BrandID { get; set; }

        [XmlArray("BrandNameList")]
        [XmlArrayItem("BrandName")]
        public List<string> BrandName { get; set; }

        public int? Logo
        {
            get { return _Logo; }
            set { _Logo = value; }
        } int? _Logo = null;
        public string PaidEarned
        {
            get { return _PaidEarned; }
            set { _PaidEarned= value; }
        } string _PaidEarned = string.Empty;
    }
}
