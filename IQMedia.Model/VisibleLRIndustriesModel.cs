using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMedia.Model
{
    [Serializable]
    public class VisibleLRIndustries
    {       
            public List<IQ_Industry> Industries { get; set; }       
    }
}
