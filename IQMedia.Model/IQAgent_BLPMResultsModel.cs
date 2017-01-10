using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQAgent_BLPMResultsModel
    {
        public Int64 ID { get; set; }
        public string FileLocation { get; set; }
        public int Circulation { get; set; }
        public string Pub_Name { get; set; }
        public string HighlightingText { get; set; }
        public string Title { get; set; }
    }
}
