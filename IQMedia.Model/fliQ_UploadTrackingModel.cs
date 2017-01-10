using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class fliQ_UploadTrackingModel
    {
        public Int64 ID { get; set; }

        public Guid _FliqCustomerGuid { get; set; }

        public Int64 _FliqApplicationID { get; set; }

        public string  FileName { get; set; }

        public Guid CategoryGuid { get; set; }

        public string Tags { get; set; }

        public string Status { get; set; }

        public DateTime? UploadedDateTime { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public string CategoryName { get; set; }

    }
}
