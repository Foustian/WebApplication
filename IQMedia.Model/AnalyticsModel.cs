using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    [Serializable]
    public class AnalyticsRequest
    {
        public SecondaryTabID Tab { get; set; }
        public string SubTab { get; set; }  // For use in demographics, determines if to show chart by gender or age
        public string DateInterval { get; set; }
        public List<Int64> RequestIDs { get; set; }
        public ChartType ChartType { get; set; }
        public string PageType { get; set; }
        public HCChartTypes HCChartType { get; set; }
        public string DateFrom { get; set; }    // Using string instead of DateTime because server will auto-convert between TZs on de-/serialization
        public string DateTo { get; set; }
        public List<string> PESHTypes { get; set; }
        public List<string> SourceGroups { get; set; }
        public Boolean IsFilter { get; set; }
        public string SubMediaType { get; set; }
        public List<AnalyticsCampaign> Campaigns { get; set; }  // Needed to get xAxis values for line chart - caused by different date ranges
        public Dictionary<long, string> Agents { get; set; }
        public Boolean IsCompareMode { get; set; }
        public SummationProperty SummationProperty { get; set; }

        public override string ToString()
        {
            string retStr = string.Format("\nGraphRequest\n------------\nTab: {0}\n", this.Tab);
            if (this.SubTab != null)
            {
                retStr = string.Format("{0}SubTab: {1}\n", retStr, this.SubTab);
            }
            retStr = string.Format("{0}Interval: {1}\n", retStr, this.DateInterval);

            if (this.RequestIDs != null && this.RequestIDs.Count > 0)
            {
                string idStr = string.Format("IDs: {0}", this.RequestIDs[0]);
                for (int i = 1; i < this.RequestIDs.Count; i++)
                {
                    idStr += string.Format(", {0}", this.RequestIDs[i]);
                }
                retStr = string.Format("{0}{1}\n", retStr, idStr);
            }
            else
            {
                retStr = string.Format("{0}IDs: None\n", retStr);
            }

            retStr = string.Format("{0}ChartType: {1}\nPageType: {2}\nHCChartType: {3}\nDate: {4} to {5}\n", retStr, this.ChartType, this.PageType, this.HCChartType, this.DateFrom, this.DateTo);

            if (this.PESHTypes != null && this.PESHTypes.Count > 0)
            {
                string PESHStr = string.Format("PESHTypes: {0}", this.PESHTypes[0]);
                for (int i = 1; i < this.PESHTypes.Count; i++)
                {
                    PESHStr += string.Format(", {0}", this.PESHTypes[i]);
                }
                retStr = string.Format("{0}{1}\n", retStr, PESHStr);
            }
            else
            {
                retStr = string.Format("{0}PESHTypes: None\n", retStr);
            }

            if (this.SourceGroups != null && this.SourceGroups.Count > 0)
            {
                string sourceStr = string.Format("SourceGroups: {0}", this.SourceGroups[0]);
                for (int i = 1; i < this.SourceGroups.Count; i++)
                {
                    sourceStr += string.Format(", {0}", this.SourceGroups[i]);
                }
                retStr = string.Format("{0}{1}\n", retStr, sourceStr);
            }
            else
            {
                retStr = string.Format("{0}SourceGroups: None\n", retStr);
            }

            retStr = string.Format("{0}IsFilter: {1}\n", retStr, this.IsFilter);
            if (this.SubMediaType != null)
            {
                retStr = string.Format("{0}SubMediaType: {1}\n", retStr, this.SubMediaType);
            }
            else
            {
                retStr = string.Format("{0}SubMediaType: None\n", retStr);
            }

            return retStr;
        }
    }

    public class AnalyticsPESHFilters
    {
        public bool isFiltering { get; set; }
        public bool isOnAir { get; set; }
        public bool isOnline { get; set; }
        public bool isPrint { get; set; }
        public bool isReadEarned { get; set; }
        public bool isSeenEarned { get; set; }
        public bool isSeenPaid { get; set; }
        public bool isHeardEarned { get; set; }
        public bool isHeardPaid { get; set; }
    }

    [Serializable]
    public class AnalyticsSecondaryTable
    {
        public string GroupByHeader { get; set; }
        public List<string> ColumnHeaders { get; set; }
        public List<string> ColumnHeadersAds { get; set; }
        public List<string> ColumnHeadersLR { get; set; }
        public List<string> ColumnHeadersAdsLR { get; set; }
        public string GroupBy { get; set; }
        public string GroupByDisplay { get; set; }
        public string TabDisplay { get; set; }
        public string PageType { get; set; }
    }

    public class AnalyticsAgeRange
    {
        public string AgeRange { get; set; }
        public Int64 MaleAudience { get; set; }
        public Int64 FemaleAudience { get; set; }
        public Int64 TotalAudience { get; set; }
    }

    /// <summary>
    /// Used to organize summaries into groups and associate these summaries with a name and ID
    /// </summary>
    public class AnalyticsGrouping
    {
        string _name = "";
        string _id = "";
        List<AnalyticsSummaryModel> _summaries = new List<AnalyticsSummaryModel>();
        List<AnalyticsGrouping> _subGroups = new List<AnalyticsGrouping>();

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public List<AnalyticsSummaryModel> Summaries
        {
            get { return _summaries; }
            set { _summaries = value; }
        }
        public List<AnalyticsGrouping> AgentSubGroupings
        {
            get { return _subGroups; }
            set { _subGroups = value; }
        }
    }

    [Serializable]
    public class AnalyticsCampaign
    {
        public Int64 CampaignID { get; set; }
        public Int64 SearchRequestID { get; set; }
        public string CampaignName { get; set; }
        public string QueryName { get; set; }
        public string QueryVersion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AnalyticsSummaryModel CampaignSummary { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Int64 IsActive { get; set; }
    }

    [Serializable]
    public class AnalyticsActiveElement
    {
        public string ActivePage { get; set; }
        public string ElementSelector { get; set; }
        public string ElementSelectorID { get; set; }
        public List<string> ActiveTabs { get; set; }
        public bool IsActiveWithPESH { get; set; }
        public bool IsActiveWithMaps { get; set; }
        public bool IsActiveWithLineCharts { get; set; }
        public bool IsActiveWithOtherCharts { get; set; }
        public List<string> HiddenTabs { get; set; }
    }

    public enum ChartType
    {
        Bar,
        Column,
        Line,
        Pie,
        Daytime,
        Growth,
        Daypart,
        US,
        Canada
    }

    public enum SecondaryTabID
    {
        OverTime,
        Demographic,
        Daytime,
        Sources,
        Daypart,
        Market,
        Networks,
        Shows,
        Overview,
        Stations,
        DemoOverview
    }

    public enum HCChartTypes
    {
        area,
        arearange,
        areaspline,
        areasplinerange,
        bar,
        boxplot,
        bubble,
        column,
        columnrange,
        errorbar,
        funnel,
        gauge,
        heatmap,
        line,
        pie,
        polygon,
        pyramid,
        scatter,
        solidgauge,
        spline,
        treemap,
        waterfall,
        fusionMap
    }

    public enum SummationProperty
    {
        IQMediaValue,
        Docs,
        Hits,
        AdSpend
    }

    // Cohorts Models
    [Serializable]
    public class CohortSolrFacet
    {
        public string Name { get; set; }
        public Int64 Count { get; set; }
        public decimal AdValue { get; set; }
        public Int64 PaidHits { get; set; }
    }

    public class CohortSolrFacetRequest
    {
        List<string> _SRIDs = null;
        DateTime? _startDate = null;
        DateTime? _endDate = null;
        string _facet = null;
        List<KeyValuePair<string, string>> _extraParmaeters = null;

        public List<string> SearchRequestIDs
        {
            get { return _SRIDs; }
            set { _SRIDs = value; }
        }

        public DateTime? StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public string Facet
        {
            get { return _facet; }
            set { _facet = value; }
        }

        public List<KeyValuePair<string, string>> ExtraParameters
        {
            get { return _extraParmaeters; }
            set { _extraParmaeters = value; }
        }
    }

    public class RawCohortFacet
    {
        public string val { get; set; }
        public Int64 count { get; set; }
        public decimal totaladvalue { get; set; }
        public Int64 totalpaidhits { get; set; }
    }
}
