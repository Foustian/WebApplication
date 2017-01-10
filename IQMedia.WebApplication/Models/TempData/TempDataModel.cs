using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMedia.Model;
using IQCommon.Model;

namespace IQMedia.WebApplication.Models.TempData
{
    [Serializable]
    public class FeedsTempData
    {
        public Int64? TotalResults { get; set; }
        public Int64? TotalResultsDisplay { get; set; }
        public Int64? SinceID { get; set; }
        public Int64? FromRecordID { get; set; }
        public Int32? MaxFeedsReportLimit { get; set; }
        public Int32? MaxFeedsExportCSVLimit { get; set; }
        public Int64? FromRecordIDDisplay { get; set; }
        public Dictionary<string, string> ChildCounts { get; set; }
        public Dictionary<string, List<string>> ChildIDs { get; set; }
        public Int32? RawMediaExpiration { get; set; }
        public Int32 DefaultFeedsPageSize { get; set; }
        public bool? UseProminence { get; set; }
        public bool? UseProminenceMediaValue { get; set; }
    }

    [Serializable]
    public class DiscoveryTempData
    {
        public int? SavedSearchPage { get; set; }
        public Discovery_SavedSearchModel ActiveSearch { get; set; }
        public bool IsAllDmaAllowed { get; set; }
        public bool IsAllClassAllowed { get; set; }
        public bool IsAllStationAllowed { get; set; }
        public Int32 CurrentPageSize { get; set; }
        public List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack { get; set; }
        public Int32? MaxDiscoveryReportLimit { get; set; }
        public Int32? MaxDiscoveryExportLimit { get; set; }
        public Int32? MaxDiscoveryHistory { get; set; }
        public IQClient_ThresholdValueModel iQClient_ThresholdValueModel { get; set; }
        public List<Int16> lstIQLicense { get; set; }
        public List<int> IQTVRegion { get; set; }
        public List<int> IQTVCountry { get; set; }
        public List<IQ_Dma> DmaList { get; set; }
        public List<IQ_Station> StationList { get; set; }
        public List<IQ_Class> ClassList { get; set; }
        public List<Station_Affil> AffiliateList { get; set; }
        public List<IQ_Region> RegionList { get; set; }
        public List<IQ_Country> CountryList { get; set; }
        public Int32 DefaultDiscoveryPageSize { get; set; }
    }

    [Serializable]
    public class DiscoveryLiteTempData
    {


        public bool IsAllDmaAllowed { get; set; }
        public bool IsAllClassAllowed { get; set; }
        public bool IsAllStationAllowed { get; set; }
        public List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack { get; set; }
        public List<int> IQTVRegion { get; set; }
        public List<int> IQTVCountry { get; set; }
    }

    [Serializable]
    public class TimeShiftTempData
    {
        public Int64? SinceID { get; set; }
        public Int64? TotalResults { get; set; }
        public int? CurrentPage { get; set; }
        public bool p_IsAllDmaAllowed { get; set; }
        public bool p_IsAllClassAllowed { get; set; }
        public bool p_IsAllStationAllowed { get; set; }
        public int PageNumber { get; set; }
        public int ResultCount { get; set; }
        public bool HasMoreResultPage { get; set; }
        public bool HasPreviousResultPage { get; set; }
        public string RecordNumber { get; set; }
        public List<string> SelectedDma { get; set; }
        public List<string> SelectedStation { get; set; }
        public int CurrentSavedSearchPageNumner { get; set; }
        public Timeshift_SavedSearchModel ActiveSearch { get; set; }
        public List<int> IQTVRegion { get; set; }
        public List<int> IQTVCountry { get; set; }
    }

    [Serializable]
    public class TAdsTempData
    {
        public Int64? SinceID { get; set; }
        public Int64? TotalResults { get; set; }
        public int? CurrentPage { get; set; }
        public bool p_IsAllDmaAllowed { get; set; }
        public bool p_IsAllClassAllowed { get; set; }
        public bool p_IsAllStationAllowed { get; set; }
        public int PageNumber { get; set; }
        public int ResultCount { get; set; }
        public bool HasMoreResultPage { get; set; }
        public bool HasPreviousResultPage { get; set; }
        public string RecordNumber { get; set; }
        public List<string> SelectedDma { get; set; }
        public List<string> SelectedStation { get; set; }
        public List<int> IQTVRegion { get; set; }
        public List<int> IQTVCountry { get; set; }
        public List<string> IQTVStations { get; set; }
        public List<string> VisibleLRIndustries { get; set; }
        public List<string> VisibleLRBrands { get; set; }
        public Dictionary<string, object> SSPData { get; set; }
        public int CurrentSavedSearchPageNumber { get; set; }
        public Tads_SavedSearchModel ActiveSearch { get; set; }
    }

    [Serializable]
    public class ImagiQTempData
    {
        public Int64? SinceID { get; set; }
        public Int64? TotalResults { get; set; }
        public int PageNumber { get; set; }
        public bool HasMoreResultPage { get; set; }
        public bool HasPreviousResultPage { get; set; }
        public string RecordNumber { get; set; }
    }

    [Serializable]
    public class IFrameMicrositeTempData
    {
        public bool IsMicrositeDownload { get; set; }
        /* public string IFrameMicroSiteSearchText { get; set; }
         public bool IsSortDirecitonAsc { get; set; }
         public string ClipSortExpression { get; set; }
         public int? CurrentClipPage { get; set; }
         public string ClientGuid { get; set; }
         public string Title { get; set; }
         public string CustID { get; set; }
         public string CatID { get; set; }
         public string SubCat1 { get; set; }
         public string SubCat2 { get; set; }
         public string SubCat3 { get; set; }
         public int Rows { get; set; }
         public int Cols { get; set; }
         public string Sort { get; set; }*/
    }

    [Serializable]
    public class KantorMediaTempData
    {
        public Int64? TotalResults { get; set; }
        public int PageNumber { get; set; }
        public int ResultCount { get; set; }
        public bool HasMoreResultPage { get; set; }
        public bool HasPreviousResultPage { get; set; }
        public string RecordNumber { get; set; }
        public List<int> IQTVRegion { get; set; }
        public List<int> IQTVCountry { get; set; }
        public List<string> IQ_CC_Key { get; set; }
        public int ManualClipDuration { get; set; }
    }

    [Serializable]
    public class SetupTempData
    {
        public List<IQAgent_SearchRequestModel> lstIQAgent_SearchRequestModel { get; set; }

        public List<FBPageModel> lstFBPageModel { get; set; }

        public bool fliq_ClientApplicationHasMoreRecords { get; set; }
        public int fliq_ClientApplicationPageNumber { get; set; }

        public bool fliq_CustomerApplicationHasMoreRecords { get; set; }
        public int fliq_CustomerApplicationPageNumber { get; set; }

        public bool fliq_UGCUploadHasMoreRecords { get; set; }
        public int fliq_UGCUploadPageNumber { get; set; }

        public bool IQNotificationHasMoreRecords { get; set; }
        public int IQNotificationPageNumber { get; set; }

        public bool JobStatusHasMoreRecords { get; set; }
        public int JobStatusPageNumber { get; set; }

        public string DownloadLocation { get; set; }

        public List<AnalyticsCampaign> ClientCampaigns { get; set; }
        public List<IQAgent_SearchRequestModel> ClientAgents { get; set; }
    }

    [Serializable]
    public class RsetPwdTempData
    {
        public bool IsLinkExpired { get; set; }
    }

    [Serializable]
    public class AnalyticsTempData
    {
        public AnalyticsDataModel AnalyticsDataModel { get; set; }
        public AnalyticsDataModel DaytimeDataModel { get; set; } // Daytime data is different from everything else, so store it separately
        public AnalyticsDataModel DaypartDataModel { get; set; } // Daypart data is different from everything else, so store it separately
        public List<IQAgent_CampaignModel> CampaignModelList { get; set; }
        public List<string> OnlineSubMediaTypes { get; set; }
        public List<string> PrintSubMediaTypes { get; set; }
        public List<string> OnAirSubMediaTypes { get; set; }
        public Dictionary<string, Dictionary<string, string>> Groups { get; set; }
        public List<IQ_MediaTypeModel> SubMediaTypes { get; set; }
        public Dictionary<long, string> ClientAgents { get; set; }
        public List<AnalyticsSecondaryTable> PrevSecondaryTables { get; set; }
        public List<AnalyticsCampaign> ClientAnalyticsCampaigns { get; set; }
        public List<AnalyticsActiveElement> ActiveElements { get; set; }
        public AnalyticsDataModel PrevMSTData { get; set; }
        public AnalyticsDataModel PrevTSSTData { get; set; }
    }

    [Serializable]
    public class DownloadTVTempData
    {
        public Dictionary<Guid, string> ClipCCLocationList { get; set; }
    }
}
