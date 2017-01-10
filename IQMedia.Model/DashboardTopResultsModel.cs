using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class DashboardTopResultsModel
    {
        public string Outlet_Name { get; set; }

        public string DMA_Name { get; set; }

        public string DMA_Num { get; set; }

        public long Mentions { get; set; }

        public int NoOfDocs { get; set; }

        public decimal MediaValue { get; set; }

        public long Audience { get; set; }

        public int PositiveSentiment { get; set; }

        public int NegativeSentiment { get; set; }

        public Int64 FriendsCount { get; set; }

        public int _IQDmaIDs { get; set; }

        public string Country { get; set; }

        public string Country_Num { get; set; }

        public string SubMediaType { get; set; }
    }
}
