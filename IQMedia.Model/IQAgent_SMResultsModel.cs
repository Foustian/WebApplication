using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQAgent_SMResultsModel
    {
        public Int64 ID { get; set; }

        private string _Description = "";

        public string Description { get { return _Description; } set { _Description = value; } }
        public DateTime? ItemHarvestDate { get; set; }
        public string HighlightingText { get; set; }
        public HighlightedSMOutput HighlightedSMOutput { get; set; }
        public string Link { get; set; }

        //public decimal? IQ_AdShare_Value { get; set; }
        //public int? c_uniq_visitor { get; set; }
        //public Boolean IsUrlFound { get; set; }
        //public Boolean IsCompeteAll { get; set; }
        
        public string ArticleID { get; set; }
        public string ArticleUri { get; set; }
        public string HomeLink { get; set; }
        public string CompeteURL { get; set; }
        public string SourceCategory { get; set; }
        public string ThumbUrl { get; set; }
        public ArticleStatsModel ArticleStats { get; set; }
        public decimal IQProminence { get; set; }
        public decimal IQProminenceMultiplier { get; set; }

        public int? Compete_Audience { get; set; }

        public decimal? IQAdShareValue { get; set; }

        public string Compete_Result { get; set; }

        public int PositiveSentiment { get; set; }

        public int NegativeSentiment { get; set; }

        public string SearchTerm { get; set; }

        public int Number_Hits { get; set; }
    }
}
