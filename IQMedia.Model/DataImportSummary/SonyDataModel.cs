using System;

namespace IQMedia.Model
{
    public class SonySummaryModel
    {
        public DateTime GMTDateTime { get; set; }
        public long NoOfDocs { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Track { get; set; }
        public string MediaType { get; set; }
        public string SubMediaType { get; set; }
        public string SeriesType { get; set; }
        public long SearchRequestID { get; set; }
    }
}
