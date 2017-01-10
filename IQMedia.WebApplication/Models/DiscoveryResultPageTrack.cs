using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQMedia.WebApplication.Models
{
    [Serializable]
    public class DiscoveryResultRecordTrack
    {
        public string SearchName { get; set; }
        public string SearchTerm { get; set; }
        public Int64? TotalRecords { get; set; }
        public List<RecordTrackSubMediaType> RecordTrackSubMediaTypes { get; set; }

        // TODO: DELETE
        public Int64? TVRecordTotal { get; set; }
        public Int64? NMRecordTotal { get; set; }
        public Int64? SMRecordTotal { get; set; }
        public Int64? PQRecordTotal { get; set; }

        public bool IsTVValid { get; set; }
        public bool IsNMValid { get; set; }
        public bool IsSMValid { get; set; }
        public bool IsPQValid { get; set; }
    }

    [Serializable]
    public class RecordTrackSubMediaType
    {
        public string SubMediaType { get; set; }
        public Int64? RecordTotal { get; set; }
        public bool IsValid { get; set; }
    }
}
