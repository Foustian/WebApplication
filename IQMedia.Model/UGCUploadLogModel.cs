using System;
using System.Data.SqlTypes;
namespace IQMedia.Model
{
    public class UGCUploadLogModel
    {
        /// <summary>
        /// Represents UGC_Upload_LogKey
        /// </summary>
        public Int64 UGC_Upload_LogKey { get; set; }

        /// <summary>
        /// Represents CustomerGUID
        /// </summary>
        public Guid CustomerGUID { get; set; }

        /// <summary>
        /// Represents UGCXml
        /// </summary>
        public SqlXml UGCXml { get; set; }

        /// <summary>
        /// Represents FileName
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Represents UploadedDateTime
        /// </summary>
        public DateTime UploadedDateTime { get; set; }
    }
}
