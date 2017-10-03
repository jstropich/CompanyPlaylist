using System;

namespace Company.Playlist.Models
{
    public class SongRequest
    {
        public string Band;
        public string SongTitle;
        public string TrackTitle
        {
            get { return string.Format("{0} - {1}", SongTitle, Band); }
        }
        public TimeSpan Length = new TimeSpan();
        public string Requester;
        public int TimesPlayed;
    }
}
