using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMedia.Model
{
    public class HighlightedCCOutput
    {        
        public List<ClosedCaption> CC { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
