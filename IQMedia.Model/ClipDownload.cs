using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class ClipDownload
    {
        public long ID { get; set; }

        public string ClipGUID { get; set; }

        public string ClipTitle { get; set; }

        public string ClipFormat { get; set; }

        public string ClipFileLocation { get; set; }

        public int? ClipDownloadStatus { get; set; }

        public DateTime? ClipDLRequestDateTime { get; set; }

        public DateTime? ClipDownLoadedDateTime { get; set; }

        public bool IsActive { get; set; }
    }
}
