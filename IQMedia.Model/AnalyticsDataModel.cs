using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    [Serializable]
    public class AnalyticsDataModel
    {
        public List<AnalyticsSummaryModel> SummaryDataList { get; set; }

        public List<AnalyticsMapSummaryModel> DmaMentionMapList { get; set; }

        public List<AnalyticsMapSummaryModel> CanadaMentionMapList { get; set; }
    }

    [Serializable]
    public class DayPartDataItem
    {
        public string AffiliateCode { get; set; }
        public string DayPartName { get; set; }
        public string DayPartCode { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public int HourOfDay { get; set; }       
    }
}
