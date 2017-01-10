using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQAgent_NewsResultsModel
    {
        public Int64 ID { get; set; }
        public string Title { get; set; }
        public DateTime? Harvest_Time { get; set; }
        public string Url { get; set; }
        public string HighlightingText { get; set; }
        public HighlightedNewsOutput HighlightedNewsOutput { get; set; }
        public Int64? SinceID { get; set; }
        public string ArticleID { get; set; }
        public Boolean IsUrlFound { get; set; }
        public string ArticleUri { get; set; }
        public string Publication { get; set; }
        public string CompeteUrl { get; set; }
        public decimal IQProminence { get; set; }
        public decimal IQProminenceMultiplier { get; set; }

        public string Market { get; set; }
        public int? Compete_Audience { get; set; }

        public decimal? IQAdShareValue { get; set; }

        public string Compete_Result { get; set; }

        public int PositiveSentiment { get; set; }

        public int NegativeSentiment { get; set; }

        public Int16 IQLicense { get; set; }

        public Int32 _ParentID { get; set; }

        public List<IQAgent_MediaResultsModel> ChildResults { get; set; }

        public string SearchTerm { get; set; }

        public int Number_Hits { get; set; }

        public string FeedClass { get; set; }
    }
}
