using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IQMedia.Model
{
    public class FeedsFilterModel
    {
        public List<DmaFilter> DMAFilter { get; set; }
        public List<SearchRequestFilter> ListOfSearchRequestFilter { get; set; }
        public List<MediaTypeFilter> ListOfMediaTypeFilter { get; set; }
        public List<string> FilterMediaDate { get; set; }

        public long PositiveSentiment { get; set; }
        public long NegativeSentiment { get; set; }
        public long NullSentiment { get; set; }
        public long Read { get; set; }
        public long Unread { get; set; }
        public long Seen { get; set; }
        public long Heard { get; set; }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string PositiveSentimentFormatted
        {
            set { PositiveSentimentFormatted = value; }
            get
            {
                return string.Format("{0:n0}", PositiveSentiment);
            }
        }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string NegativeSentimentFormatted
        {
            set { NegativeSentimentFormatted = value; }
            get
            {
                return string.Format("{0:n0}", NegativeSentiment);
            }
        }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string NullSentimentFormatted
        {
            set { NullSentimentFormatted = value; }
            get
            {
                return string.Format("{0:n0}", NullSentiment);
            }
        }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string ReadFormatted
        {
            set { ReadFormatted = value; }
            get
            {
                return string.Format("{0:n0}", Read);
            }
        }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string UnreadFormatted
        {
            set { UnreadFormatted = value; }
            get
            {
                return string.Format("{0:n0}", Unread);
            }
        }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string SeenFormatted
        {
            set { SeenFormatted = value; }
            get
            {
                return string.Format("{0:n0}", Seen);
            }
        }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string HeardFormatted
        {
            set { HeardFormatted = value; }
            get
            {
                return string.Format("{0:n0}", Heard);
            }
        }
    }

    public class DmaFilter
    {
        public int Count { get; set; }
        public string DmaName { get; set; }
        public int ID { get; set; }

        public string CountFormatted
        {
            set { CountFormatted = value; }
            get
            {
                return string.Format("{0:n0}", Count);
            }
        }
    }

    public class SearchRequestFilter
    {
        public string QueryName { get; set; }
        public long ID { get; set; }
        public int Count { get; set; }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string CountFormatted
        {
            set { CountFormatted = value; }
            get
            {
                return string.Format("{0:n0}", Count);
            }
        }
    }

    public class MediaTypeFilter
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public List<SubMediaTypeFilter> SubMediaTypes { get; set; }
        public int Count { get; set; }
        public int SortOrder { get; set; }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string CountFormatted
        {
            set { CountFormatted = value; }
            get
            {
                return string.Format("{0:n0}", Count);
            }
        }
    }

    public class SubMediaTypeFilter
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int Count { get; set; }
        public int SortOrder { get; set; }

        /// <summary>
        /// Just returned formatted record count. i.e. 12,345 instead of 12345.
        /// </summary>
        public string CountFormatted
        {
            set { CountFormatted = value; }
            get
            {
                return string.Format("{0:n0}", Count);
            }
        }
    }
}
