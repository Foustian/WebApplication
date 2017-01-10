using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IQMedia.Model
{
    public class IQ_KantorAudienceDataModel
    {
        public List<KantorAudience> data { get; set; }
    }

    public class KantorAudience
    {
        public int S { get; set; }
        public double A { get; set; }
        public double V { get; set; }
    }
}
