using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class fliQ_CustomerApplicationModel
    {
        public Int64 ID { get; set; }

        public Int64 FliqApplicationID { get; set; }

        public Int64 ClientID { get; set; }

        public Int64 CustomerID { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public string Application { get; set; }

        public bool? IsActive { get; set; }
    }

    public class fliQ_CustomerApplicationPostModel
    {
        public fliQ_CustomerApplicationModel customerApplication { get; set; }
        public CustomerApplication_DropDown CustomerApplication_DropDown { get; set; }

    }

    public class CustomerApplication_DropDown
    {
        public List<ClientModel> ClientList { get; set; }
        public List<fliq_CustomerModel> CustomerList { get; set; }
        public List<fliQ_ApplicationModel> ApplicationList { get; set; }
    }
}
