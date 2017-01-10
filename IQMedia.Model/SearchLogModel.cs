using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class SearchLogModel
    {
        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public Int64 SearchLogKey { get; set; }

        /// <summary>
        /// Represents CustomerID
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// Represents Search Type
        /// </summary>
        public string SearchType { get; set; }

        /// <summary>
        /// Represents RequestXML
        /// </summary>
        public string RequestXML { get; set; }

        /// <summary>
        /// Represents ResponseXML
        /// </summary>
        public string ErrorResponseXML { get; set; }
    }
}
