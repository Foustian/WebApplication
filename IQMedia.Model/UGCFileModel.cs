using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class UGCFileModel
    {
        public string Name { get; set; }

        public bool IsDirectory { get; set; }

        public string Path { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
