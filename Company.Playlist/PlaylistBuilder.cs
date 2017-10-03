using Company.Playlist.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Company.Playlist
{
    public class PlaylistBuilder
    {
        private List<SongRequest> Requests = new List<SongRequest>();
        private List<PersonAvailability> Availability = new List<PersonAvailability>();

        public PlaylistBuilder(List<SongRequest> requests, List<PersonAvailability> availability)
        {
            Requests = new List<SongRequest>(requests);
            Availability = new List<PersonAvailability>(availability);
        }

        public List<SongRequest> GetPlaylist(int period)
        {
            period--;
            var playlist = new List<SongRequest>();
            var timeLimit = new TimeSpan(0, 30, 00);
            var timeTotal = new TimeSpan();

            Dictionary<string, bool> requesterHasAvailableSongs = GetRequestersSongAvailability(period);
            
            timeTotal = SinglePassBuildList(playlist, timeLimit, timeTotal, requesterHasAvailableSongs);

            while (Requests.Exists(p => p.Length <= timeLimit.Subtract(timeTotal) && requesterHasAvailableSongs.ContainsKey(p.Requester)))
            {
                var songsThatAreShortEnough = Requests.Where(r => r.Length <= timeLimit.Subtract(timeTotal) && requesterHasAvailableSongs.ContainsKey(r.Requester));
                var song = songsThatAreShortEnough.OrderBy(s => s.TimesPlayed).FirstOrDefault();
                if (song != null)
                    timeTotal = AddSongToPlaylist(playlist, timeTotal, song);
            }
            
            return playlist;
        }

        private TimeSpan SinglePassBuildList(List<SongRequest> playlist, TimeSpan timeLimit, TimeSpan timeTotal, Dictionary<string, bool> requesterHasAvailableSongs)
        {
            var keyList = new List<string>(requesterHasAvailableSongs.Keys);
            while (requesterHasAvailableSongs.Any(r => r.Value == false))
            {
                foreach (var requester in keyList)
                {
                    var song = Requests.FirstOrDefault(r => r.Requester == requester && r.TimesPlayed == 0);
                    if (song != null && timeTotal.Add(song.Length) <= timeLimit)
                        timeTotal = AddSongToPlaylist(playlist, timeTotal, song);
                    else
                        requesterHasAvailableSongs[requester] = true;
                }
            }

            return timeTotal;
        }

        private Dictionary<string, bool> GetRequestersSongAvailability(int period)
        {
            Dictionary<string, bool> requesterHasAvailableSongs = new Dictionary<string, bool>();
            Requests.ForEach(o =>
            {
                if (!requesterHasAvailableSongs.ContainsKey(o.Requester)
                     && Availability.Exists(a => a.Name == o.Requester && a.Available[period]))
                    requesterHasAvailableSongs.Add(o.Requester, false);
            });
            return requesterHasAvailableSongs;
        }

        private static TimeSpan AddSongToPlaylist(List<SongRequest> playlist, TimeSpan timeTotal, SongRequest song)
        {
            playlist.Add(song);
            timeTotal = timeTotal.Add(song.Length);
            song.TimesPlayed++;
            return timeTotal;
        }
    }
}
