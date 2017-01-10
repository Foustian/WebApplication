using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQGalleryModel
    {
        public long ID { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public List<IQGallery> Json { get; set; }

        public string xml { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class IQGalleryResult
    {
        public List<IQGalleryModel> IQGalleryList { get; set; }
    }

    public class IQGallery
    {
        public long ID { get; set; }

        public string Type { get; set; }

        public string ClipID { get; set; }

        public int Col { get; set; }

        public int Row { get; set; }

        public int size_x { get; set; }

        public int size_y { get; set; }

        public string TVThumbUrl { get; set; }

        public string MetaData { get; set; }
    }

    public class IQGalleryItemType
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
