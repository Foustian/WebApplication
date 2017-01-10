using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQCommon.Model;
using System.ComponentModel;

namespace IQMedia.Model
{
    public class SummaryReportModel
    {
        public Int64 Number_Docs { get; set; }
        public DateTime GMT_DateTime { get; set; }
        public string MediaType { get; set; }
        public Int64 Number_Of_Hits { get; set; }
        public Int64 Audience { get; set; }
        public decimal IQMediaValue { get; set; }
        public string SubMediaType { get; set; }
        public string Query_Name { get; set; }
        public Int64 SearchRequestID { get; set; }
        public int? ThirdPartyDataTypeID { get; set; }
        [DefaultValue(true)]
        public bool DefaultMediaType { get; set; }

    }
    public class SummaryReportMulti
    {
        public string MediaRecords { get; set; }
        public string SubMediaRecords { get; set; }
        
        public string AudienceRecords { get; set; }
        public Int64 AudienceRecordsSum { get; set; }
        public Int64 AudiencePrevRecordsSum { get; set; }        
        
        public string IQMediaValueRecords { get; set; }
        public decimal IQMediaValueRecordsSum { get; set; }
        public decimal IQMediaValuePrevRecordsSum { get; set; }

        public string TotalNumOfHits { get; set; }

        public List<SummaryReportMedium> SummaryReportMedium { get; set; }
    }

    public class SummaryReportMedium
    {
        public string Records { get; set; }
        public Int64 RecordsSum { get; set; }
        public Int64 PrevRecordsSum { get; set; }

        public IQ_MediaTypeModel MediaTypeModel { get; set; }
    }
}
