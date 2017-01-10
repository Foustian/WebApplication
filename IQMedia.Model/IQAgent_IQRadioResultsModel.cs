using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQAgent_IQRadioResultsModel
    {
        public Int64 ID { get; set; }
        public string Title { get; set; }
        public DateTime GMTDateTime { get; set; }
        public DateTime LocalDateTime { get; set; }
        public string TimeZone { get; set; }
        public string StationID { get; set; }
        public string StationLogo { get; set; }
        public string HighlightingText { get; set; }
        public HighlightedCCOutput HighlightedCCOutput { get; set; }
        public Guid Guid { get; set; }
        public string Market { get; set; }
        public string DMARank { get; set; }
        public int Hits { get; set; }
        public IQAgentXML.SearchRequest SearchRequestModel { get; set; } // Used to get search term for the player

        public IQAgent_IQRadioResultsModel ShallowCopy()
        {
            return (IQAgent_IQRadioResultsModel)this.MemberwiseClone();
        }
    }
}
