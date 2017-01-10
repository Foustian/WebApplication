using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class MultiLoginActiveUser
    {
        public string EmailAddress { get; set; }

        public string SessionID { get; set; }

        public DateTime SessionTimeOut { get; set; }

        public DateTime LastAccessTime { get; set; }
    }
}
