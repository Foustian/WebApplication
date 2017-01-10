using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    [Serializable]
    public class AnalyticsMapSummaryModel
    {
        public long SearchRequestID { get; set; }
        public string DMAName { get; set; }
        public long NumberOfHits { get; set; }
    }
}
