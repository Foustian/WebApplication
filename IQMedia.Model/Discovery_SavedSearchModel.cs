using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    [Serializable]
    public class Discovery_SavedSearchModel
    {
        public Int32 ID { get; set; }
        public string Title { get; set; }
        public string SearchID { get; set; }
        public string SearchTerm { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<string> Mediums { get; set; }
        public string TVMarket { get; set; }
        public string[] SearchTermArray { get; set; }
        public string[] SearchIDArray { get; set; }
        public bool IsCurrent { get; set; }
        public Guid ClientGuid { get; set; }
        public Guid CustomerGuid { get; set; }

        public List<DiscoveryAdvanceSearchModel> AdvanceSearchSettingsList { get; set; }
        public List<string> AdvanceSearchSettingIDsList { get; set; }
    }
}
