using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQService_DiscoveryExportModel
    {
        public string ArticleXml { get; set; }

        public string SearchCriteria { get; set; }

        public bool IsSelectAll { get; set; }

        public Int64 ID { get; set; }

        public string Status { get; set; }

        public string DownloadPath { get; set; }

        public Guid CustomerGuid { get; set; }

        public DateTime CreatedDate { get; set; }

        public string SearchTerm { get; set; }
    }
}
