using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQ_QVCData
    {
        public string IQ_CC_Key { get; set; }

        public string Station_ID { get; set; }

        public DateTime GMT_air_datetime { get; set; }

        public int? audience { get; set; }
    }
}
