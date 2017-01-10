using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class ArticleNMDownload
    {
        public long ID { get; set; }

        public string ArticleID { get; set; }

        public string ArticleTitle { get; set; }
        
        public string CustomerGuid { get; set; }
        
        public int DownloadStatus { get; set; }
        
        public string DownloadLocation { get; set; }
        
        public DateTime? DLRequestDateTime { get; set; }
        
        public DateTime? DownLoadedDateTime { get; set; }
        
        public bool IsActive { get; set; }
    }
}
