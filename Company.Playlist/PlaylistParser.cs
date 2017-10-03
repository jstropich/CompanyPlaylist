using Company.Playlist.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Company.Playlist
{
    public class PlaylistParser
    {
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
                        SongTitle = matches[0].Groups[1].Value,
                        Band = matches[0].Groups[2].Value,
                        Length = new TimeSpan(0, int.Parse(matches[0].Groups[3].Value), int.Parse(matches[0].Groups[4].Value)),
                        Requester = matches[0].Groups[5].Value.Trim()
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
                if (matches.Count == 1)
                {
                    if (!availability.Exists(a => a.Name == matches[0].Groups[1].Value))
                    {
                        bool[] isAvailable = new bool[2];
                        isAvailable[0] = matches[0].Groups[2].Value.Contains("1");
                        isAvailable[1] = matches[0].Groups[2].Value.Contains("2");
                        availability.Add(new PersonAvailability()
                                                {
                                                    Name = matches[0].Groups[1].Value.Trim(),
                                                    Available = isAvailable
                                                });
                    }
                }
            }
            return availability;
        }
    }
}
