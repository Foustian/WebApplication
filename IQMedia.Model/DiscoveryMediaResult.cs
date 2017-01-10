using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMedia.Shared.Utility;

namespace IQMedia.Model
{
    public class DiscoveryMediaResult
    {
        public DateTime? Date { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string StationLogo { get; set; }
        public string Market { get; set; }
        public int? PositiveSentiment { get; set; }
        public int? NegativeSentiment { get; set; }
        public string ArticleID { get; set; }
        public string ArticleURL { get; set; }
        public int? Audience { get; set; }
        public decimal? IQAdsharevalue { get; set; }
        public string Nielsen_Result { get; set; }
        public string SourceCategory { get; set; }
        public string Publication { get; set; }
       // public CommonFunctions.CategoryType MediumType { get; set; }  TODO: DELETE
        public string MediumType { get; set; }
        public string SearchTerm { get; set; }
        public string SearchName { get; set; }
        public Guid? VideoGuid { get; set; }
        public string IQ_CC_Key { get; set; }
        public string CompeteImage { get; set; }
        public Int64 TotalRecords { get; set; }
        public Boolean IsValid { get; set; }
        public Boolean IncludeInResult { get; set; }
        public string TimeZone { get; set; }
        public DateTime? LocalDateTime { get; set; }
        public Int16 IQLicense { get; set; }
        public string ProQuestAuthors { get; set; }
        public string ProQuestCopyright { get; set; }
        public string timeDifference { get; set; }
    }
}