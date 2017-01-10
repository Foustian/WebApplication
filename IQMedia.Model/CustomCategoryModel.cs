using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class CustomCategoryModel
    {
        public Int64 CategoryKey { get; set; }
        
        public Guid ClientGUID { get; set; }
        
        public Guid CategoryGUID { get; set; }
        
        public String CategoryName { get; set; }

        public String DefaultCategory { get; set; }
        
        public String CategoryDescription { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        
        public DateTime? ModifiedDate { get; set; }
        
        public bool IsActive { get; set; }

        public Int32 CategoryRanking { get; set; }
    }
}
