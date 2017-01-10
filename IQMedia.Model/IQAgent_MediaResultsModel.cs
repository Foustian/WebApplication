using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQAgent_MediaResultsModel : Object
    {

        public Int64 ID { get; set; }
        public DateTime MediaDateTime { get; set; }
        public string ArticleID { get; set; }
        public string MediaType { get; set; }
        public string CategoryType { get; set; }
        public Int16 PositiveSentiment { get; set; }
        public Int16 NegativeSentiment { get; set; }
        public dynamic MediaData { get; set; }
        public decimal IQProminence { get; set; }
        public decimal IQProminenceMultiplier { get; set; }
        public bool HasChildren { get; set; }
        public bool IsRead { get; set; }
        public string timeDifference { get; set; }
        public string SearchAgentName { get; set; }
        public long NumberOfHits { get; set; }
        public List<string> ExcludedIDs { get; set; }

        public IQAgent_MediaResultsModel ShallowCopy()
        {
            return (IQAgent_MediaResultsModel)this.MemberwiseClone();
        }
    }

    
}
