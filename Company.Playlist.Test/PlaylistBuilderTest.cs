using Company.Playlist.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Company.Playlist.Test
{
    [TestClass]
    public class PlaylistBuilderTest
    {
        
        private static List<SongRequest> GetReturnPlaylise(List<SongRequest> requests, List<PersonAvailability> availability)
        {
            var sut = new PlaylistBuilder(requests, availability);
            
            return sut.GetPlaylist(1);
        }

        [TestMethod]
        public void GetPlaylist_EmptyRequestsAndAvailability_NothingReturned()
        {
            var requests = new List<SongRequest>();
            var availability = new List<PersonAvailability>();

            List<SongRequest> playlist = GetReturnPlaylise(requests, availability);

            Assert.AreEqual(0, playlist.Count);
        }
        
        [TestMethod]
        public void GetPlaylist_PersonAvailableWithRequest_RequestAdded()
        {
            var requests = new List<SongRequest>();
            requests.Add(new SongRequest()
                            {
                                Band = "Nirvana",
                                SongTitle = "Come as you are",
                                Length = new TimeSpan(0, 5, 3),
                                Requester = "Lisa"
                            });
            var availability = new List<PersonAvailability>();
            availability.Add(new PersonAvailability()
                                {
                                    Name = "Lisa",
                                    Available = new bool[2] { true, true }
                                });

            List<SongRequest> playlist = GetReturnPlaylise(requests, availability);

            Assert.AreEqual("Come as you are - Nirvana", playlist.First().TrackTitle);
        }

        [TestMethod]
        public void GetPlaylist_PersonNotAvailableWithRequest_RequestNotAdded()
        {
            var requests = new List<SongRequest>();
            requests.Add(new SongRequest()
            {
                Band = "Nirvana",
                SongTitle = "Come as you are",
                Length = new TimeSpan(0, 5, 3),
                Requester = "Lisa"
            });
            var availability = new List<PersonAvailability>();
            availability.Add(new PersonAvailability()
            {
                Name = "Lisa",
                Available = new bool[2] { false, false }
            });

            List<SongRequest> playlist = GetReturnPlaylise(requests, availability);

            Assert.AreEqual(0, playlist.Count);
        }

        [TestMethod]
        public void GetPlaylist_SongOver30Minutes_RequestNotAdded()
        {
            var requests = new List<SongRequest>();
            requests.Add(new SongRequest()
            {
                Band = "Nirvana",
                SongTitle = "Come as you are",
                Length = new TimeSpan(0, 31, 3),
                Requester = "Lisa"
            });
            var availability = new List<PersonAvailability>();
            availability.Add(new PersonAvailability()
            {
                Name = "Lisa",
                Available = new bool[2] { true, true }
            });

            List<SongRequest> playlist = GetReturnPlaylise(requests, availability);

            Assert.AreEqual(0, playlist.Count);
        }

        [TestMethod]
        public void GetPlaylist_SecondRequestOverLimit_RequestNotAdded()
        {
            var requests = new List<SongRequest>();
            requests.Add(new SongRequest()
            {
                Band = "Nirvana",
                SongTitle = "Come as you are",
                Length = new TimeSpan(0, 29, 3),
                Requester = "Lisa"
            });
            requests.Add(new SongRequest()
            {
                Band = "Deftones",
                SongTitle = "Bored",
                Length = new TimeSpan(0, 29, 3),
                Requester = "Lisa"
            });
            var availability = new List<PersonAvailability>();
            availability.Add(new PersonAvailability()
            {
                Name = "Lisa",
                Available = new bool[2] { true, true }
            });
            List<SongRequest> playlist = GetReturnPlaylise(requests, availability);

            Assert.AreEqual(1, playlist.Count);
        }
        
        [TestMethod]
        public void GetPlaylist_PersonOneSongTooLong_SkipPersonOne()
        {
            var requests = new List<SongRequest>();
            requests.Add(new SongRequest()
            {
                Band = "Nirvana",
                SongTitle = "Come as you are",
                Length = new TimeSpan(0, 41, 3),
                Requester = "Lisa"
            });
            requests.Add(new SongRequest()
            {
                Band = "Deftones",
                SongTitle = "Bored",
                Length = new TimeSpan(0, 29, 3),
                Requester = "Frank"
            });
            var availability = new List<PersonAvailability>();
            availability.Add(new PersonAvailability()
            {
                Name = "Lisa",
                Available = new bool[2] { true, true }
            });
            availability.Add(new PersonAvailability()
            {
                Name = "Frank",
                Available = new bool[2] { true, true }
            });
            List<SongRequest> playlist = GetReturnPlaylise(requests, availability);

            Assert.AreEqual(1, playlist.Count);
            Assert.AreEqual("Frank", playlist[0].Requester);
        }

        [TestMethod]
        public void GetPlaylist_SongAddedDuringFirstPeriod_NotAddedDuringSecondPeriod()
        {
            var requests = new List<SongRequest>();
            requests.Add(new SongRequest()
            {
                Band = "Nirvana",
                SongTitle = "Come as you are",
                Length = new TimeSpan(0, 29, 3),
                Requester = "Lisa"
            });
            requests.Add(new SongRequest()
            {
                Band = "Nirvana",
                SongTitle = "The man who sold the world",
                Length = new TimeSpan(0, 29, 3),
                Requester = "Lisa"
            });
            var availability = new List<PersonAvailability>();
            availability.Add(new PersonAvailability()
            {
                Name = "Lisa",
                Available = new bool[2] { true, true }
            });
            var sut = new PlaylistBuilder(requests, availability);

            var playlistOne = sut.GetPlaylist(1);
            var playlistTwo = sut.GetPlaylist(2);

            Assert.AreEqual("Come as you are - Nirvana", playlistOne.First().TrackTitle);
            Assert.AreEqual("The man who sold the world - Nirvana", playlistTwo.First().TrackTitle);
        }

        [TestMethod]
        public void GetPlaylist_RoomAtEndOfPlaylistToPlayRepeatSong_AddRepeatSong()
        {
            var requests = new List<SongRequest>();
            requests.Add(new SongRequest()
            {
                Band = "Nirvana",
                SongTitle = "Come as you are",
                Length = new TimeSpan(0, 28, 3),
                Requester = "Lisa"
            });
            requests.Add(new SongRequest()
            {
                Band = "Nirvana",
                SongTitle = "The man who sold the world",
                Length = new TimeSpan(0, 28, 3),
                Requester = "Lisa"
            });
            requests.Add(new SongRequest()
            {
                Band = "Deftones",
                SongTitle = "Change",
                Length = new TimeSpan(0, 1, 3),
                Requester = "Lisa"
            });
            var availability = new List<PersonAvailability>();
            availability.Add(new PersonAvailability()
            {
                Name = "Lisa",
                Available = new bool[2] { true, true }
            });
            var sut = new PlaylistBuilder(requests, availability);

            var playlistOne = sut.GetPlaylist(1);
            var playlistTwo = sut.GetPlaylist(2);

            Assert.AreEqual("Change - Deftones", playlistOne[1].TrackTitle);
            Assert.AreEqual("Change - Deftones", playlistTwo[1].TrackTitle);
        }
    }
}
