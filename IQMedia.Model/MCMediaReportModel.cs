using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class MCMediaReportModel
    {
        public Guid? ReportGuid { get; set; }

        public string CustomHeader { get; set; }

        public Guid? MasterClientGuid { get; set; }

        public bool HasResults { get; set; }

        public List<MCMediaReport_GroupTier1Model> GroupTier1Results { get; set; }

        public MCMediaReport_FilterModel FilterResults { get; set; }
    }

    public class MCMediaReport_GroupTier1Model
    {
        public string GroupName { get; set; }

        public bool IsEnabled { get; set; }

        public int GroupRank { get; set; }

        public List<MCMediaReport_GroupTier2Model> GroupTier2Results { get; set; }
    }

    public class MCMediaReport_GroupTier2Model
    {
        public string GroupName { get; set; }

        public bool IsEnabled { get; set; }

        public int GroupRank { get; set; }

        public List<MCMediaReport_GroupTier3Model> GroupTier3Results { get; set; }
    }

    public class MCMediaReport_GroupTier3Model
    {
        public string GroupName { get; set; }

        public bool IsEnabled { get; set; }

        public int GroupRank { get; set; }

        public List<IQArchive_MediaModel> MediaResults { get; set; }
    }

    public class MCMediaReport_FilterModel
    {
        public List<MCMediaReport_MediaTypeFilter> MediaTypes { get; set; }

        public List<MCMediaReport_Filter> Clients { get; set; }

        public List<MCMediaReport_Filter> Categories { get; set; }

        // store date into MM/dd/yyyy format for Javascript parsing
        public List<string> Dates { get; set; }

        public long PositiveSentiment { get; set; }
        public long NegativeSentiment { get; set; }
        public long NullSentiment { get; set; }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string PositiveSentimentFormatted
        {
            set { }
            get
            {
                return string.Format("{0:n0}", PositiveSentiment);
            }
        }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string NegativeSentimentFormatted
        {
            set { }
            get
            {
                return string.Format("{0:n0}", NegativeSentiment);
            }
        }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string NullSentimentFormatted
        {
            set { }
            get
            {
                return string.Format("{0:n0}", NullSentiment);
            }
        }
    }

    public class MCMediaReport_Filter
    {
        public string ClientName { get; set; }

        public string ClientGuid { get; set; }

        public string CategoryName { get; set; }

        public long RecordCount { get; set; }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string RecordCountFormatted
        {
            get
            {
                return string.Format("{0:n0}", RecordCount);
            }
            set { }
        }
    }

    public class MCMediaReport_MediaTypeFilter
    {
        public string MediaType { get; set; }

        public string MediaTypeDesc { get; set; }

        public List<MCMediaReport_SubMediaTypeFilter> SubMediaTypes { get; set; }

        public long RecordCount { get; set; }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string RecordCountFormatted
        {
            set { RecordCountFormatted = value; }
            get
            {
                return string.Format("{0:n0}", RecordCount);
            }
        }
    }

    public class MCMediaReport_SubMediaTypeFilter
    {
        public string SubMediaType { get; set; }

        public string SubMediaTypeDesc { get; set; }

        public long RecordCount { get; set; }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string RecordCountFormatted
        {
            set { RecordCountFormatted = value; }
            get
            {
                return string.Format("{0:n0}", RecordCount);
            }
        }
    }
}
