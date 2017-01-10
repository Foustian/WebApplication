using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class FeedsSearchResponse
    {
        public List<IQAgent_MediaResultsModel> MediaResults { get; set; }
        public FeedsFilterModel Filter { get; set; }
        public Dictionary<string, string> ChildCounts { get; set; }
        public bool IsValid { get; set; }
        public bool IsReadLimitExceeded { get; set; }
        public List<string> ExcludedIDs { get; set; }
    }

    public class FeedsChildSearchResponse
    {
        public IQAgent_MediaResultsModel MediaResult { get; set; }
        public Int64 OrigParentID { get; set; }
        public bool IsValid { get; set; }
        public List<string> ExcludedIDs { get; set; }
    }
}
