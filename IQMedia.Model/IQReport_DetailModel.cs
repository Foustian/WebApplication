using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQReport_DetailModel
    {
        public bool IsFeedsReport { get; set; }
        public int MaxFeedsReportItems { get; set; }

        public bool IsDiscoveryReport { get; set; }
        public int MaxDiscoveryReportItems { get; set; }

        public int CurrentReportTotal { get; set; }
    }
}
