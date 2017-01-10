using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class PieChartResponse
    {
        public string SearchName { get; set; }
        public string SearchTerm { get; set; }
        public string JsonResult { get; set; }
        public string TopResultHtml { get; set; }
    }
}
