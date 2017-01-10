using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMedia.Model
{
    public class IQ_ReportModel
    {
        public long ID { get; set; }

        public string ReportGUID { get; set; }

        public string Title { get; set; }

        public long _ReportTypeID { get; set; }

        public string ReportRule { get; set; }

        public DateTime ReportDate { get; set; }

        public string ClientGuid { get; set; }

        public bool IsActive { get; set; }

        public string _ReportImage { get; set; }

        public long? _ReportImageID { get; set; }

        public IQ_ReportSettingsModel Settings { get; set; }

        public long RecordCount { get; set; }

        public bool HasCustomSort { get; set; }
    }

    public class IQAgentReport_WithoutAuthentication
    {
        public string ReportTitle { get; set; }

        public string ReportID { get; set; }

        public List<IQAgent_MediaResultsModel> MediaResults { get; set; }

        public string CustomHeader { get; set; }

        public Guid ClientGuid { get; set; }

        public IQ_ReportSettingsModel Settings { get; set; }
    }

    public class IQAgentReport
    {
        public string ReportTitle { get; set; }

        public string ReportID { get; set; }

        public List<IQAgentReport_SearchRequestModel> Results { get; set; }

        public string CustomHeader { get; set; }

        public Guid ClientGuid { get; set; }

        public IQ_ReportSettingsModel Settings { get; set; }

        public bool UseRollup { get; set; }
    }

    public class IQAgentReport_SearchRequestModel
    {
        public Int64 SearchRequestID { get; set; }

        public string QueryName { get; set; }

        public List<IQAgent_MediaResultsModel> MediaResults { get; set; }

    }

    public class IQAgentReport_RawMediaPlayer
    {
        public string RawMediaGuid { get; set; }

        public string CC_Highlight { get; set; }

        public string ClosedCaption { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string RawMediaPlayerHTML { get; set; }

        public string SearchTerm { get; set; }

        public bool IsMobileDevice { get; set; }

        public int PlayOffset { get; set; }

        public string DataModelType { get; set; }
    }

    [XmlRoot("Settings")]
    public class IQ_ReportSettingsModel
    {
        public bool ShowAudience 
        {
            get { return _ShowAudience; }
            set { _ShowAudience = value; }
        } bool _ShowAudience = true;

        public bool ShowMediaValue
        {
            get { return _ShowMediaValue; }
            set { _ShowMediaValue = value; }
        } bool _ShowMediaValue = true;

        public bool ShowSentiment
        {
            get { return _ShowSentiment; }
            set { _ShowSentiment = value; }
        } bool _ShowSentiment = true;

        public bool ShowTotalAudience
        {
            get { return _ShowTotalAudience; }
            set { _ShowTotalAudience = value; }
        } bool _ShowTotalAudience = true;

        public bool ShowTotalMediaValue
        {
            get { return _ShowTotalMediaValue; }
            set { _ShowTotalMediaValue = value; }
        } bool _ShowTotalMediaValue = true;

        public bool ShowNationalValues
        {
            get { return _ShowNationalValues; }
            set { _ShowNationalValues = value; }
        }bool _ShowNationalValues = true;

        public bool ShowTotalNationalAudience
        {
            get { return _ShowTotalNationalAudience; }
            set { _ShowTotalNationalAudience = value; }
        }bool _ShowTotalNationalAudience = true;

        public bool ShowTotalNationalMediaValue
        {
            get { return _ShowTotalNationalMediaValue; }
            set { _ShowTotalNationalMediaValue = value; }
        }bool _ShowTotalNationalMediaValue = true;

        public bool ShowCoverageSources
        {
            get { return _ShowCoverageSources; }
            set { _ShowCoverageSources = value; }
        }bool _ShowCoverageSources = true;

        [XmlArrayItem(ElementName = "ChartMediaType")]
        public List<string> ChartMediaTypes { get; set; }

        public string Sort { get; set; }

        public string PrimaryGroup { get; set; }

        public string SecondaryGroup { get; set; }
    }

    public enum ReportGroupType
    {
        SubMediaType,
        CategoryName,
        AgentName,
        None
    }
}
