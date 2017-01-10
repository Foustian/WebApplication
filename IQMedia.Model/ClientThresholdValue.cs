using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    [Serializable]
    public class IQClient_ThresholdValueModel
    {
        public float? TVLowThreshold { get; set; }
        public float? TVHighThreshold { get; set; }

        public float? NMLowThreshold { get; set; }
        public float? NMHighThreshold { get; set; }

        public float? SMLowThreshold { get; set; }
        public float? SMHighThreshold { get; set; }

        public float? TwitterLowThreshold { get; set; }
        public float? TwitterHighThreshold { get; set; }

        public float? PQLowThreshold { get; set; }
        public float? PQHighThreshold { get; set; }

        public IQClient_ThresholdValueModel Copy()
        {
            return (IQClient_ThresholdValueModel)this.MemberwiseClone();
        }
    }
}
