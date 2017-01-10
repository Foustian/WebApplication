using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQUGCArchiveModel
    {
        public long IQUGCArchiveKey { get; set; }

        public string Title { get; set; }

        public string UGCGuid { get; set; }

        public DateTime? CreateDT { get; set; }

        public IQMedia.Shared.Utility.CommonFunctions.Timezone CreateDTTimeZone { get; set; }

        public DateTime? AirDate { get; set; }

        public string CategoryGUID { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public string ThumbnailImage { get; set; }

        public string UGCFileLocation { get; set; }

        public string UGCFileName { get; set; }

        public DateTime? DateUploaded { get; set; }

        public Shared.Utility.CommonFunctions.IQUGCMediaTypes FileType { get; set; }

        public string MediaUrl { get; set; }
    }

    public class IQUGCArchiveResult
    {
        public List<IQUGCArchiveModel> IQUGCArchiveList { get; set; }

        public long TotalResults { get; set; }

        public long SinceID { get; set; }

        public IQUGCArchiveResult_FilterModel Filter { get; set; }
    }

    public class IQUGCArchiveResult_FilterModel
    {
        public List<string> CreateDTList { get; set; }

        public List<IQUGCArchiveResult_Filter> Customers { get; set; }

        public List<IQUGCArchiveResult_Filter> Categories { get; set; }

        public List<IQUGCArchiveResult_Filter> MediaTypes { get; set; }
    }

    public class IQUGCArchiveResult_Filter
    {
        public string CategoryGUID { get; set; }

        public string CategoryName { get; set; }

        public string CustomerKey { get; set; }

        public string CustomerName { get; set; }

        public string MediaType { get; set; }

        public string MediaTypeDesc { get; set; }

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
        }
    }

    public class IQUGCArchiveEditModel
    {
        public long IQUGCArchiveKey { get; set; }

        public string Title { get; set; }

        public string CategoryGuid { get; set; }

        public string CustomerGuid { get; set; }

        public string SubCategory1Guid { get; set; }

        public string SubCategory2Guid { get; set; }

        public string SubCategory3Guid { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public List<CustomCategoryModel> CustomCategories { get; set; }

        public List<CustomerModel> Customers { get; set; }
    }

}
