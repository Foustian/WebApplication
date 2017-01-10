using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQTAdsResultModel
    {
        public Int64 ID { get; set; }

        public string IQ_CC_Key { get; set; }

        public string StationID { get; set; }

        public List<IQTAdsHit> Hits { get; set; }

        public int HitCount { get; set; }

        public DateTime CreatedDate { get; set; }

        public int IsActive { get; set; }
    }
    public class IQTAdsHit
    {
        public int startOffset { get; set; }
        public int endOffset { get; set; }
    }
}
