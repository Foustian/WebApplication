using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IQMedia.Model
{
    public class JobStatusModel
    {
        public Int64 ID { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? RequestedDateTime { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public string DownloadPath { get; set; }
        public string Status { get; set; }
        public Int64 RequestID { get; set; }
        public bool CanReset { get; set; }
        public string ResetProcedureName { get; set; }
    }

    public class JobType
    {
        public int ID { get; set; }

        public string Description { get; set; }
    }
}