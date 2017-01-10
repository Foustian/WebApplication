using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Shared.Utility;

namespace IQMedia.Model
{
    public class DiscoveryTopicsResponse
    {
        public List<Topics> ListTopics { get; set; }
        public bool IsValid { get; set; }
    }

    public class Topics
    {
        public string Topic { get; set; }
        public int Frequency { get; set; }
    }
}
