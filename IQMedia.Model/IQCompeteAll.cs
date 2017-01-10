using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQCompeteAll
    {
        public string CompeteURL { get; set; }

        public decimal? IQ_AdShare_Value { get; set; }

        public int? c_uniq_visitor { get; set; }

        public Boolean IsCompeteAll { get; set; }

        public Boolean IsUrlFound { get; set; }
    }
}
