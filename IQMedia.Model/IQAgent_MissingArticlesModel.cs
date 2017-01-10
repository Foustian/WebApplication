using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQAgent_MissingArticlesModel
    {
        public Int64 ID { get; set; }

        public string Title { get; set; }

        public Int64 _SearchRequestID { get; set; }

        public string Content { get; set; }

        public string Url { get; set; }

        public DateTime harvest_time { get; set; }

        public string Category { get; set; }

        public bool AddToLibrary { get; set; }

        public Guid LibraryCategory { get; set; }
    }
}
