using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQClient_CustomImageModel
    {
        public Int64 ID { get; set; }

        public string Location { get; set; }

        public Guid ClientGuid { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsDefault { get; set; }

        public bool IsDefaultEmail { get; set; }
    }
}
