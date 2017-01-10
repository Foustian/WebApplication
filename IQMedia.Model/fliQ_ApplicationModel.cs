using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class fliQ_ApplicationModel
    {
        public Int64 ID { get; set; }

        public string Application { get; set; }

        public string Version { get; set; }

        public string Path { get; set; }

        public string Description { get; set; }

        public bool? IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
