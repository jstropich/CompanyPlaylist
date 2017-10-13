using Company.Playlist.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Company.Playlist
{
    public class PlaylistParser
    {
        private const int RequestSongTitleGroup = 1;
        private const int RequestBandNameGroup = 2;
        private const int RequestMinutesGroup = 3;
        private const int RequestSecondsGroup = 4;
        private const int RequestPersonGroup = 5;
        private const int AvailablePersonGroup = 1;
        private const int AvailableSectionGroup = 2;

        public List<SongRequest> GetSongRequests(IEnumerable<string> fileContents)
        {
            var requests = new List<SongRequest>();
            foreach(var line in fileContents)
            {
                MatchCollection matches = Regex.Matches(line, @"([\w\s]+) - ([\w\s]+) (\d+):(\b\d+\b) added by ([\w\s]+)");
                if (matches.Count == 1)
                {
                    requests.Add(new SongRequest()
                    {
                        SongTitle = matches[0].Groups[RequestSongTitleGroup].Value,
                        Band = matches[0].Groups[RequestBandNameGroup].Value,
                        Length = new TimeSpan(0, int.Parse(matches[0].Groups[RequestMinutesGroup].Value), int.Parse(matches[0].Groups[RequestSecondsGroup].Value)),
                        Requester = matches[0].Groups[RequestPersonGroup].Value.Trim()
                    });
                }
            }
            return requests;
        }

        public List<PersonAvailability> GetPersonAvailability(IEnumerable<string> fileContents)
        {
            var availability = new List<PersonAvailability>();
            foreach(var line in fileContents)
            {
                MatchCollection matches = Regex.Matches(line, @"([\w\s]+) is available for section (.*)");
                if ((matches.Count == 1) && (!availability.Exists(a => a.Name == matches[0].Groups[AvailablePersonGroup].Value)))
                {
                    bool[] isAvailable = new bool[2];
                    isAvailable[0] = matches[0].Groups[AvailableSectionGroup].Value.Contains("1");
                    isAvailable[1] = matches[0].Groups[AvailableSectionGroup].Value.Contains("2");
                    availability.Add(new PersonAvailability()
                                            {
                                                Name = matches[0].Groups[AvailablePersonGroup].Value.Trim(),
                                                Available = isAvailable
                                            });
                }
            }
            return availability;
        }
    }
}
