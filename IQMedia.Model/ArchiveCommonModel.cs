using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class ArchiveCommonModel
    {
        public int ArchiveKey { get; set; }

        public string Title { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public Guid CustomerGuid { get; set; }       

        public Guid ClientGuid { get; set; }

        public Guid CategoryGuid { get; set; }

        public Guid? SubCategory1Guid { get; set; }

        public Guid? SubCategory2Guid { get; set; }

        public Guid? SubCategory3Guid { get; set; }

        public Int32 Rating { get; set; }

        public Int64 MediaResultID { get; set; }

        public Int64 ParentID { get; set; }
    }
}
