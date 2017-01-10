using System;
namespace IQMedia.Model
{
    public class PlayerObjectsModel
    {
        public string VideoHtml { get; set; }

        public string CCHtml { get; set; }

        public string HighlightHtml { get; set; }

        public string KeyValues { get; set; }

        public Guid RawMediaGUID { get; set; }
    }
}