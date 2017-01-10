using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace IQMedia.Model
{
    
    [XmlRoot(ElementName="SessionModelList")]
    public class SessionModel
    {
        public string SessionID { get; set; }

        public string LoginID { get; set; }

        public DateTime SessionTimeOut { get; set; }

        public DateTime LastAccessTime { get; set; }

        public static DateTime? NextDBRefreshDateTime { get; set; }

        public string Server { get; set; }
    }

    public class SessionSearchSortModel
    {
        public string SessionID { get; set; }

        public string LoginID { get; set; }

        public DateTime SessionTimeOut { get; set; }

        public DateTime LastAccessTime { get; set; }

        public static DateTime? NextDBRefreshDateTime { get; set; }      

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Server { get; set; }
    }
}
