using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class ImagiQLogoModel
    {
        public Int64 ID { get; set; }
        public string CompanyName { get; set; }
        public string ThumbnailPath { get; set; }
        public string HitLogoPath { get; set; }
        public int Offset { get; set; }
    }
}
