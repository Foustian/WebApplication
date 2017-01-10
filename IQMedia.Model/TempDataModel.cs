using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    [Serializable]
    public class TempDataModel
    {
        public long UGCTotalResults { get; set; }

        public long UGCSinceID { get; set; }

        public long UGCFromRecordID { get; set; }

        public IQClient_UGCMapModel UGC_Client_MapModel { get; set; }
    }
}
