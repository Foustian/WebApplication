using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMedia.Model;

namespace IQMedia.WebApplication.Models
{
    [Serializable]
    public class GlobalAdminTempData
    {
        public bool ClientHasMoreRecords { get; set; }
        public int ClientPageNumber { get; set; }

        public bool CustomerHasMoreRecords { get; set; }
        public int CustomerPageNumber { get; set; }

        public bool fliq_CustomerHasMoreRecords { get; set; }
        public int fliq_CustomerPageNumber { get; set; }

        public bool fliq_ApplicationHasMoreRecords { get; set; }
        public int fliq_ApplicationPageNumber { get; set; }

        public bool fliq_ClientApplicationHasMoreRecords { get; set; }
        public int fliq_ClientApplicationPageNumber { get; set; }

        public bool fliq_CustomerApplicationHasMoreRecords { get; set; }
        public int fliq_CustomerApplicationPageNumber { get; set; }

        public bool ClientUGCMapHasMoreRecords { get; set; }
        public int ClientUGCMapPageNumber { get; set; }


        public Client_DropDown Client_DropDowns { get; set; }
    }
}