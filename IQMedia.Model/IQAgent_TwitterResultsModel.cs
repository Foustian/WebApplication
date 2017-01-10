using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQAgent_TwitterResultsModel
    {
        public Int64 ID { get; set; }
        public string Actor_PreferredName { get; set; }
        public string Actor_DisplayName { get; set; }
        public Int64 KlOutScore { get; set; }
        public Int64 Actor_FollowersCount { get; set; }
        public Int64 Actor_FriendsCount { get; set; }
        public string Summary { get; set; }
        public DateTime? Tweet_DateTime { get; set; }
        public string TweetID { get; set; }
        public string Actor_Link { get; set; }
        public string Actor_Image { get; set; }
        public decimal IQProminence { get; set; }
        public decimal IQProminenceMultiplier { get; set; }

        public HighlightedTWOutput HighlightedOutput { get; set; }

        public int PositiveSentiment { get; set; }

        public int NegativeSentiment { get; set; }

        public string SearchTerm { get; set; }

        public int Number_Hits { get; set; }
    }
}
