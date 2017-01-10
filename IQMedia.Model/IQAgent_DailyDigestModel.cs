using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMedia.Model
{
    public class IQAgent_DailyDigestModel
    {
        public Int64 ID { get; set; }

        public DateTime TimeOfDay { get; set; }

        public string EmailAddress { get; set; }

        public Int64? ReportImageID { get; set; }

        public Int64? IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public List<Int64> IQAgentList { get; set; }

        public string IQAgentNames { get; set; }
    }

    public class IQAgentList
    {
        public IQAgentList()
        {
            IQAgent = new List<Int64>();
        }

        [XmlElement("IQAgent")]
        public List<Int64> IQAgent { get; set; }
    }
}
