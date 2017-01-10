using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Shared.Utility;
using System.Runtime.Serialization;

namespace IQMedia.Model
{
    [Serializable]
    public class IQArchive_MediaModel
    {

        public IQArchive_MediaModel ShallowCopy()
        {
            return (IQArchive_MediaModel)this.MemberwiseClone();
        }

        public long ID { get; set; }

        public long ArchiveMediaID { get; set; }

        public string MediaType { get; set; }

        public DateTime MediaDate { get; set; }

        public Guid CategoryGUID { get; set; }

        public string CategoryName { get; set; }

        public Guid SubCategory1GUID { get; set; }

        public string SubCategory1Name { get; set; }

        public Guid SubCategory2GUID { get; set; }

        public string SubCategory2Name { get; set; }

        public Guid SubCategory3GUID { get; set; }

        public string SubCategory3Name { get; set; }

        public int CategoryRanking { get; set; }

        public Guid ClientGUID { get; set; }

        public string ClientName { get; set; }

        public Guid CustomerGUID { get; set; }

        public string Title { get; set; }

        public CommonFunctions.CategoryType SubMediaType { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public dynamic MediaData { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }

        public bool DisplayDescription { get; set; }

        public string DataModelType { get; set; }

        // Holds the formatted text that is displayed for the item. Can be Description, Content, or HighlightingText.
        public string DisplayText { get; set; }

        // USed to group items by submedia type
        public string SubMediaTypeDesc { get; set; }

        // Used to group items by search agent
        public string AgentName { get; set; }

        // Used for custom sorting in reports
        public long AgentID { get; set; }
        public string GroupTier1Value { get; set; }
        public string GroupTier2Value { get; set; }
        public int Position
        {
            get { return _Position; }
            set { _Position = value; }
        } int _Position = 99999; // Place unsorted items at the bottom of the group
        
        public bool IsPublished { get; set; }

        public decimal? MediaValue { get; set; }

        // Default to -1 for sorting purposes
        public long Audience 
        {
            get { return _Audience; }
            set { _Audience = value; }
        } long _Audience = -1;

        // Store these at this level so they can be accessed in a generic manner
        public Int64? National_Nielsen_Audience { get; set; }
        public decimal? National_IQAdShareValue { get; set; }

        // Sorting reports by audience requires a secondary field
        public string SecondarySortField { get; set; }

        public string timeDifference { get; set; }
    }

    [Serializable]
    public class IQArchive_ArchiveBLPMModel
    {
        public string FileLocation { get; set; }
        public int Circulation { get; set; }
        public string Pub_Name { get; set; }
    }

    [Serializable]
    public class IQArchive_ArchiveClipModel
    {
        public IQArchive_ArchiveClipModel ShallowCopy()
        {
            return (IQArchive_ArchiveClipModel)this.MemberwiseClone();
        }

        public string ClipID { get; set; }
        public string ClosedCaption { get; set; }
        public int? Nielsen_Audience { get; set; }
        public DateTime LocalDateTime { get; set; }
        public decimal? IQAdShareValue { get; set; }
        public string Nielsen_Result { get; set; }
        public string Market { get; set; }
        public string StationLogo { get; set; }
        public Int16 PositiveSentiment { get; set; }
        public Int16 NegativeSentiment { get; set; }
        public string TimeZone { get; set; }
        public string Station_Call_Sign { get; set; }
        public string ClipTitle { get; set; }
        public long? _ParentID { get; set; }
        public List<IQArchive_MediaModel> ChildResults { get; set; }
        public string Dma_Num { get; set; }
        public HighlightedCCOutput HighlightedOutput { get; set; }

        public Int64? National_Nielsen_Audience { get; set; }
        public decimal? National_IQAdShareValue { get; set; }
        public string National_Nielsen_Result { get; set; }

    }

    [Serializable]
    public class IQArchive_ArchiveNMModel
    {
        public string Url { get; set; }
        public int? Compete_Audience { get; set; }
        public decimal? IQAdShareValue { get; set; }
        public string Compete_Result { get; set; }
        public string Publication { get; set; }
        public Int16 PositiveSentiment { get; set; }
        public Int16 NegativeSentiment { get; set; }
        public Int16 IQLicense { get; set; }
        public long? _ParentID { get; set; }
        public HighlightedNewsOutput HighlightedOutput { get; set; }
        public List<IQArchive_MediaModel> ChildResults { get; set; }
    }

    [Serializable]
    public class IQArchive_ArchiveSMModel
    {
        public string Url { get; set; }
        public int? Compete_Audience { get; set; }
        public decimal? IQAdShareValue { get; set; }
        public string Compete_Result { get; set; }
        public string Publication { get; set; }
        public Int16 PositiveSentiment { get; set; }
        public Int16 NegativeSentiment { get; set; }
        public HighlightedSMOutput HighlightedOutput { get; set; }
        public string ThumbUrl { get; set; }
        public ArticleStatsModel ArticleStats { get; set; }
    }

    [Serializable]
    public class IQArchive_ArchiveTweetsModel
    {
        public string ActorDisplayname { get; set; }
        public string PreferredUserName { get; set; }
        public int FollowersCount { get; set; }
        public int FreiendsCount { get; set; }
        public string ActorImage { get; set; }
        public string KloutScore { get; set; }
        public Int16 PositiveSentiment { get; set; }
        public Int16 NegativeSentiment { get; set; }
        public string ActorLink { get; set; }
        public long TweetID { get; set;  }
        public HighlightedTWOutput HighlightedOutput { get; set; }
    }

    [Serializable]
    public class IQArchive_ArchiveTVEyesModel
    {
        public string StationID { get; set; }
        public string Market { get; set; }
        public string DMARank { get; set; }
        public Int16 PositiveSentiment { get; set; }
        public Int16 NegativeSentiment { get; set; }
        public DateTime LocalDateTime { get; set; }
        public string TimeZone { get; set; }
    }

    [Serializable]
    public class IQArchive_ArchivePQModel
    {
        public string Content { get; set; }
        public string ContentHTML { get; set; }
        public string Publication { get; set; }
        public List<string> Authors { get; set; }
        public Int16 PositiveSentiment { get; set; }
        public Int16 NegativeSentiment { get; set; }
        public string Copyright { get; set; }
        public HighlightedPQOutput HighlightedOutput { get; set; }
    }

    [Serializable]
    public class IQArchive_ArchiveMiscModel
    {
        public DateTime CreateDT { get; set; }
        public string TimeZone { get; set; }
        public CommonFunctions.IQUGCMediaTypes FileType { get; set; }
        public string FileTypeExt { get; set; }
        public string MediaUrl { get; set; }
    }

    [Serializable]
    public class IQArchive_ArchiveRadioModel
    {
        public string ClipGuid { get; set; }
        public DateTime LocalDateTime { get; set; }
        public string TimeZone { get; set; }
        public string StationID { get; set; }
        public string StationLogo { get; set; }
        public HighlightedCCOutput HighlightedOutput { get; set; }
        public string Market { get; set; }
        public Int16 DMARank { get; set; }
        public Int16 PositiveSentiment { get; set; }
        public Int16 NegativeSentiment { get; set; }
    }

    [Serializable, DataContract]
    public class IQArchive_Filter
    {
        [DataMember]
        public string CategoryGUID { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public string CustomerKey { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public long RecordCount { get; set; }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        [DataMember]
        public string RecordCountFormatted
        {
            get
            {
                return string.Format("{0:n0}", RecordCount);
            }
            set { }
        }
    }

    [Serializable, DataContract]
    public class IQArchive_MediaTypeFilter
    {
        [DataMember]
        public string MediaType { get; set; }

        [DataMember]
        public string MediaTypeDesc { get; set; }

        [DataMember]
        public List<IQArchive_SubMediaTypeFilter> SubMediaTypes { get; set; }

        [DataMember]
        public long RecordCount { get; set; }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        [DataMember]
        public string RecordCountFormatted
        {
            set { RecordCountFormatted = value; }
            get
            {
                return string.Format("{0:n0}", RecordCount);
            }
        }
    }

    [Serializable, DataContract]
    public class IQArchive_SubMediaTypeFilter
    {
        [DataMember]
        public string SubMediaType { get; set; }

        [DataMember]
        public string SubMediaTypeDesc { get; set; }

        [DataMember]
        public long RecordCount { get; set; }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        [DataMember]
        public string RecordCountFormatted
        {
            set { RecordCountFormatted = value; }
            get
            {
                return string.Format("{0:n0}", RecordCount);
            }
        }
    }

    [Serializable, DataContract]
    public class IQArchive_FilterModel
    {
        [DataMember]
        public List<IQArchive_MediaTypeFilter> MediaTypes { get; set; }

        [DataMember]
        public List<IQArchive_Filter> Customers { get; set; }

        [DataMember]
        public List<IQArchive_Filter> Categories { get; set; }

        // store date into MM/dd/yyyy format for Javascript parsing
        [DataMember]
        public List<string> Dates { get; set; }
    }



    public class IQArchive_EditModel
    {
        public long ID { get; set; }

        public string MediaType { get; set; }

        public CommonFunctions.CategoryType SubMediaType { get; set; }

        public long ArchiveMediaKey { get; set; }

        public string Title { get; set; }

        public string CategoryGuid { get; set; }

        public string SubCategory1Guid { get; set; }

        public string SubCategory2Guid { get; set; }

        public string SubCategory3Guid { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public bool DisplayDescription { get; set; }

        public bool UseDisplayDescription { get; set; }

        public List<CustomCategoryModel> Categories { get; set; }

        public short PositiveSentiment { get; set; }

        public short NegativeSentiment { get; set; }
    }

    public class IQArchive_RefreshResultsForTV
    {
        public long ArchiveClipKey { get; set; }

        public string ClipGuid { get; set; }
    }

    public class IQArchive_DisplayLibraryReport
    {
        // Used for things that don't need grouped results, such as CSV export
        public List<IQArchive_MediaModel> ArchiveResults { get; set; }

        // Used to display grouped results
        public List<IQArchive_GroupTier1Model> GroupTier1Results { get; set; }

        // Holds the count of items in each top-level group
        public Dictionary<string, int> GroupTier1Counts { get; set; }

        public IQ_ReportModel ReportDetails { get; set; }
    }

    public class IQArchive_GroupTier1Model
    {
        // Used when saving item positions, to identify the item's group 
        public string GroupValue { get; set; }

        // The name to be displayed in the web
        public string GroupName { get; set; }

        public bool IsEnabled { get; set; }

        public long TotalAudience { get; set; }

        public decimal TotalMediaValue { get; set; }

        public List<IQArchive_GroupTier2Model> GroupTier2Results { get; set; }

        // Holds the count of items in each second-level group
        public Dictionary<string, int> GroupTier2Counts { get; set; }
    }

    public class IQArchive_GroupTier2Model
    {
        // Used when saving item positions, to identify the item's group 
        public string GroupValue { get; set; }

        // The name to be displayed in the web
        public string GroupName { get; set; }

        public bool IsEnabled { get; set; }

        public List<IQArchive_MediaModel> ArchiveResults { get; set; }
    }
}
