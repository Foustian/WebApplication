using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using IQMedia.Model;

namespace IQMedia.WebApplication.Models
{
    public class MediaChartJsonResponse
    {
        public string ColumnChartData { get; set; }
        public string LineChartData { get; set; }
        public List<PieChartResponse> LineChartMediumData { get; set; }
        public List<PieChartResponse> PieChartMediumData { get; set; }
        public Dictionary<string, object> PieChartSearchTermData { get; set; }
        public string DataNotAvailableList { get; set; }
        public string DataAvailableList { get; set; }
        public IEnumerable DateFilter { get; set; }
        public IEnumerable MediumFilter { get; set; }
        public IEnumerable TVMarket { get; set; }
        public bool IsSearchTermValid { get; set; }
        public bool IsSuccess { get; set; }
        public string SearchTerm { get; set; }
    }
}