using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    [Serializable]
    public class IQClient_UGCMapModel
    {
        public bool AutoClip_Status { get; set; }

        public string SourceID { get; set; }

        public Int64 IQClient_UGCMapKey { get; set; }

        public Guid? SourceGUID { get; set; }

        public string BroadcastLocation { get; set; }

        public string BroadcastType { get; set; }

        public string Logo { get; set; }

        public int RetentionDays { get; set; }        

        public string Title { get; set; }

        public string URL { get; set; }

        public Guid ClientGuid { get; set; }

        public string ClientName { get; set; }

        public bool IsActive { get; set; }

        public int TimeZoneID { get; set; }
    }

    public class IQClient_UGCMapPostModel
    {
        public IQClient_UGCMapModel IQClient_UGCMapModel { get; set; }

        public IQClient_UGCMapDropDowns IQClient_UGCMapDropDowns { get; set; }


    }

    public class IQClient_UGCMapDropDowns
    {
        public List<ClientModel> Client_DropDown { get; set; }

        public List<IQTimeZone> TimeZone_DropDown { get; set; }
    }
    public class IQTimeZone
    {
        public int ID { get; set; }
        public string Code {get; set;}
        public string Name { get; set; }
    }
}
