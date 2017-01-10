using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IFrameMicrositeModel
    {

        public int ArchiveClipKey { get; set; }
        
        public Guid ClipID { get; set; }
        
        public int ClientID { get; set; }
        
        public DateTime? ClipDate { get; set; }
        
        public string ClipTitle { get; set; }
        
        public string ClipLogo { get; set; }
        
        public string FirstName { get; set; }
        
        public int CustomerID { get; set; }

        public string Category { get; set; }

        public string CategoryName { get; set; }

        public string SubCategory1Name { get; set; }

        public string SubCategory2Name { get; set; }

        public string SubCategory3Name { get; set; }

        public DateTime? ClipCreationDate { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public string SearchText { get; set; }
        public string ThumbnailImagePath { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsActive { get; set; }

        public Guid? CategoryGUID { get; set; }

        public Guid? ClientGUID { get; set; }
        public Guid? CustomerGUID { get; set; }
        

    }
}
