using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQAgent_TVEyesResultsModel
    {
        public Int64 ID { get; set; }
        public string Title { get; set; }
        public DateTime? UTCDateTime { get; set; }
        public DateTime LocalDateTime { get; set; }
        public string HighlightingText { get; set; }
        public string StationID { get; set; }
        public int StationIDNum { get; set; }
        public string DMARank { get; set; }
        public string Market { get; set; }
        public int PositiveSentiment { get; set; }
        public string TranscriptUrl { get; set; }

        public int NegativeSentiment { get; set; }
        public string TimeZone { get; set; }
        public int Duration { get; set; }
    }
}
