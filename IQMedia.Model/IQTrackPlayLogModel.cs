using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQTrackPlayLogModel
    {
        public Guid AssetGuid { get; set; }

        public DateTime PlayDate { get; set; }

        public Int64 Count { get; set; }

        public string IPAddress { get; set; }

        public string ClipTitle { get; set; }

        public Int64 LifeTimeCount { get; set; }

        public Dictionary<string, long> RegionPlayMapList { get; set; }

        public List<PlayLogTopReferrersModel> TopReferrersList { get; set; }
    }

    public class PlayLogTopReferrersModel
    {
        public string Url { get; set; }

        public Int64 ViewsCount { get; set; }

        public Int64 ViewsPercent { get; set; }
    }
}
