using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQLog_UserActionsModel
    {
        public Int64? CustomerID { get; set; }
        public string SessionID { get; set; }
        public string ActionName { get; set; }
        public string PageName { get; set; }
        public string RequestParameters { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string IPAddress { get; set; }
    }
}
