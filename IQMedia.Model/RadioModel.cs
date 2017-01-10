using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class RadioModel
    {
        public long RL_GUIDSKey { get; set; }
      
        public string IQ_Station_ID { get; set; }

        public string Market { get; set; }

        public DateTime? RL_StationDateTime { get; set; }

        public string RL_GUID { get; set; }

        public string TimeZone { get; set; }

        public int Mentions { get; set; }

        public TadsFilterModel Facets { get; set; }
    }

    public class RadioStation
    {
        public string StationID { get; set; }

        public string DMA { get; set; }
    }
}
