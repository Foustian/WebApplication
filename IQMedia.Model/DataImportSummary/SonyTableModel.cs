using System;

namespace IQMedia.Model
{
    public class SonyTableModel
    {
        public int RowID { get; set; }
        public DateTime DayDate { get; set; }
        public string MediaType { get; set; }
        public string AgentName { get; set; }
        public long ArtistID { get; set; }
        public string Artist { get; set; }
        public long AlbumID { get; set; }
        public string Album { get; set; }
        public long TrackID { get; set; }
        public string Track { get; set; }
        public long TotalCount { get; set; }
        public long SpotifyCount { get; set; }
        public long ITunesAlbumCount { get; set; }
        public long ITunesTrackCount { get; set; }
        public long AppleMusicCount { get; set; }
        public long NumberOfDocs { get; set; }
        public long NumberOfHits { get; set; }
    }
}
