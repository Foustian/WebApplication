using System;

namespace IQMedia.Model
{
    public class PlayerParamModel
    {
        public Guid RawMediaGUID { get; set; }

        public Guid CustomerGUID { get; set; }

        public Guid ClientGUID { get; set; }

        public string ServiceBaseURL { get; set; }

        public bool IsActivePlayerLogo { get; set; }

        public string PlayerLogoImage { get; set; }

        public string BrowserType { get; set; }

        public string SearchTerm { get; set; }

        public int? Offset { get; set; }

        public bool EnableCC { get; set; }

        public int PlayerDefaultOffset { get; set; }

        public string KeyValues { get; set; }

        public bool AutoPlayback { get; set; }

        /// <summary>
        /// This property will only effect if autoplayback is false.
        /// Expected values are:
        /// "A" - Auto : Display thumbnail of first frame of video
        /// "N" - None : Display thumbnail by calling thumbnail svc for RawMediaID
        /// "C" - Custom : Display thumbnail image from url provided in PreviewImageUrl property
        /// </summary>
        public char PreviewImageOption { get; set; }

        /// <summary>
        /// This property will only effect if autoplayback is false and PreviewImageOption is "C".
        /// </summary>
        public string PreviewImageURL { get; set; }
    }
}