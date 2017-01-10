using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class DiscoverySearchResponse
    {
        public string MediumType { get; set; }
        public List<RecordData> ListRecordData { get; set; }
        public string SearchName { get; set; }
        public string SearchTerm { get; set; }
        public string SearchTermParent { get; set; }
        public bool isFacet { get; set; }
        public string FromRecordID { get; set; }
        public Int64 TotalResult { get; set; }
        public bool IsValid { get; set; }
        public List<TopResults> ListTopResults { get; set; }
    }

    public class RecordData
    {
        public String Date { get; set; }
        public string TotalRecord { get; set; }
        public string FeedClass { get; set; }
    }

    // TODO: DELETE IF ABLE
    public class SocialMediaFacet
    {
        public DiscoverySearchResponse DateData { get; set; }
        public DiscoverySearchResponse FeedClassData { get; set; }
    }

    public class TopResults
    {
        public string Title { get; set; }
        public String Logo { get; set; }
        public string Publisher { get; set; }
    }
}
