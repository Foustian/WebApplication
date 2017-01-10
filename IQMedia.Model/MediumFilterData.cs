using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class MediaTypeFilterData
    {
        public string MediaType { get; set; }
        public string MediaTypeName { get; set; }
        public short SortOrder { get; set; }
        public List<SubMediaTypeFilterData> SubMediaTypeFilterData { get; set; }
    }

    public class SubMediaTypeFilterData
    {
        public string SubMediaType { get; set; }
        public string SubMediaTypeName { get; set; }
        public short SortOrder { get; set; }
    }
}
