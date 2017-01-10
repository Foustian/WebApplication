using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQMedia.WebApplication.Config.Sections
{
    public class SolrUrlSettings
    {
        public List<SolrCore> SolrCores { get; set; }
    }

    public class SolrCore
    {
        public string Type { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Url { get; set; }
    }
}