using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQAgent_PQResultsModel
    {
        public Int64 ID { get; set; }
        public string Title { get; set; }
        public DateTime? AvailableDate { get; set; }
        public DateTime? MediaDate { get; set; }
        public HighlightedPQOutput HighlightedPQOutput { get; set; }
        public string HighlightingText { get; set; }
        public Int64? SinceID { get; set; }
        public string ArticleID { get; set; }
        public string Publication { get; set; }
        public string Abstract { get; set; }
        public List<string> Authors { get; set; }
        public string Content { get; set; }
        public string ContentHTML { get; set; }
        public Int16 LanguageNum { get; set; }
        public int PositiveSentiment { get; set; }
        public int NegativeSentiment { get; set; }
        public string SearchTerm { get; set; }
        public int Number_Hits { get; set; }
        public string Copyright { get; set; }
        public string MediaCategory { get; set; }
        public string EncryptedID { get; set; }
        public decimal IQProminence { get; set; }
        public decimal IQProminenceMultiplier { get; set; }
    }
}
