using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace IQMedia.Model
{
    public class IQFeeds_ReportModel
    {
        public Int64 ID { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public Guid CategoryGuid { get; set; }
        public Guid? SubCategory1Guid { get; set; }
        public Guid? SubCategory2Guid { get; set; }
        public Guid? SubCategory3Guid { get; set; } 
        public XDocument MediaID { get; set; }
        public String Status { get; set; }
        public Guid CustomerGuid { get; set; }
        public Guid ClientGuid { get; set; }
        public Int64? ReportImageID { get; set; }
        public Int64? FolderID { get; set; }
    }

    public class IQDiscovery_ReportModel
    {
        public Int64 ID { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public Guid CategoryGuid { get; set; }
        public XDocument MediaID { get; set; }
        public String Status { get; set; }
        public Guid CustomerGuid { get; set; }
        public Guid ClientGuid { get; set; }
        public Int64? ReportImageID { get; set; }
        public Int64? FolderID { get; set; }
    }
}
