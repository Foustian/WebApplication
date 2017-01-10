using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQTimeSync_DataModel
    {
        public Int64 ID { get; set; }

        public string Type { get; set; }

        public int _TypeID { get; set; }

        public string IQ_CC_Key { get; set; }

        public string Data { get; set; }

        public GraphStructureModel GraphStructure { get; set; }
    }

    public class GraphStructureModel
    {
        public string AudienceXAxisLabel { get; set; }

        public string MediaValueXAxisLabel { get; set; }

        public string DefaultYAxisLabel { get; set; }

        public string SecondYAxisLabel { get; set; }

        public string ChartHeaderTitle { get; set; }

        public string ChartSubTitle { get; set; }

        public string AudienceTooltipPrefix { get; set; }

        public string MediaValueXTooltipPrefix { get; set; }

        public bool IsMultiAxis { get; set; }
    }
}
