using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace IQMedia.Model
{
    [XmlRoot("SearchSettings")]
    public class MCMediaSearchModel
    {
        [XmlElement("FromDate")]
        public DateTime? FromDate { get; set; }

        [XmlElement("ToDate")]
        public DateTime? ToDate { get; set; }

        [XmlElement("ClientGuid")]
        public Guid? ClientGuid
        {
            get { return _ClientGuid; }
            set { _ClientGuid = value; }
        } Guid? _ClientGuid = null;

        [XmlElement("SearchTerm")]
        public string SearchTerm
        {
            get { return _SearchTerm; }
            set { _SearchTerm = value; } 
        } string _SearchTerm = null;

        [XmlElement("SubMediaType")]
        public string SubMediaType
        {
            get { return _SubMediaType; }
            set { _SubMediaType = value; }
        } string _SubMediaType = null;

        [XmlElement("CategorySet")]
        public CategorySet CategorySet { get; set; }

        [XmlElement("SelectionType")]
        public string SelectionType
        {
            get { return _SelectionType; }
            set { _SelectionType = value; }
        } string _SelectionType = null;

        [XmlElement("SentimentFlag")]
        public int? SentimentFlag
        {
            get { return _SentimentFlag; }
            set { _SentimentFlag = value; }
        } int? _SentimentFlag = null;
    }

    public class CategorySet
    {
        public CategorySet()
        {
            CategoryList = new List<string>();
        }

        [XmlAttribute]
        public bool IsAllowAll
        {
            get { return _IsAllowAll; }
            set { _IsAllowAll = value; }
        } bool _IsAllowAll = true;

        [XmlElement("Category")]
        public List<string> CategoryList { get; set; }
    }
}
