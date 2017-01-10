using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class DiscoveryLiteSearchResponse
    {
        public string MediumType { get; set; }
        public List<LiteRecordData> ListRecordData { get; set; }
        public string SearchTerm { get; set; }
        public bool isFacet { get; set; }
        public string FromRecordID { get; set; }
        public Int64 TotalResult { get; set; }
        public bool IsValid { get; set; }
        public List<LiteTopResults> ListTopResults { get; set; }
    }

    public class LiteRecordData
    {
        public String Date { get; set; }
        public string TotalRecord { get; set; }
        public string FeedClass { get; set; }
    }

    public class LiteSocialMediaFacet
    {
        public DiscoveryLiteSearchResponse DateData { get; set; }
        public DiscoveryLiteSearchResponse FeedClassData { get; set; }
    }

    public class LiteTopResults
    {
        public string Title { get; set; }
        public String Logo { get; set; }
        public string Publisher { get; set; }
    }
}
