using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class ArticleStatsModel
    {
        public int Likes { get; set; }
        public int Shares { get; set; }
        public int Comments { get; set; }
        public bool IsVerified { get; set; }
    }
}
