using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    [Serializable]
    public class FBPageModel
    {
        public long ID { get; set; }
        public long FBPageID { get; set; }
        public string FBPageName { get; set; }
        public string FBPageUrl { get; set; }
        public long Likes { get; set; }
        public string Category { get; set; }
        public bool IsVerified { get; set; }
        public string PictureUrl { get; set; }
    }

    public class FBPageModelComparer : IEqualityComparer<FBPageModel>
    {
        public bool Equals(FBPageModel x, FBPageModel y)
        {
            return x.FBPageID == y.FBPageID;
        }

        public int GetHashCode(FBPageModel obj)
        {
            return obj.FBPageID.GetHashCode();
        }
    }
}
