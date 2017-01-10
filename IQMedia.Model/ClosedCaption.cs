using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace IQMedia.Model
{

    public class ClosedCaption
    {           
        public String Text { get; set; }
        public int Offset { get; set; }
    }
}
