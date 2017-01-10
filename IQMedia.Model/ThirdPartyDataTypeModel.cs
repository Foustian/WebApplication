using System;

namespace IQMedia.Model
{
    [Serializable]
    public class ThirdPartyDataTypeModel
    {
        public int ID { get; set; }
        public string DataType { get; set; }
        public string DisplayName { get; set; }
        public int YAxisID { get; set; }
        public string YAxisName { get; set; }
        public string SPName { get; set; }
        public bool IsAgentSpecific { get; set; }
        public bool UseHourData { get; set; }
        public bool UseIDParam { get; set; }
        public string SeriesLineType { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public bool IsSelected { get; set; }
        public bool HasAccess { get; set; }
    }
}
