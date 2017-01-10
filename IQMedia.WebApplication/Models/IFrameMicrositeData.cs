using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMedia.Model;

namespace IQMedia.WebApplication.Models
{
    public class IFrameMicrositeData
    {
        public int MaxCols { get; set; }
        public int MaxRows { get; set; }
        public List<IFrameMicrositeModel> lstIFrameMicrositeModel { get; set; }
        public string Result { get; set; }
        public bool HasMSDownloadRight { get; set; }
    }
}