using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class GoogleSummaryModel
    {
        public DateTime DayDate { get; set; }
        public Int64 NoOfDocs { get; set; }
        public string MediaType { get; set; }
        public string DataType { get; set; }
    }
}
