using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    [Serializable]
    public class IQAgent_CampaignModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public long SearchRequestID { get; set; }
        public string QueryName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int? QueryVersion { get; set; }
    }
}
