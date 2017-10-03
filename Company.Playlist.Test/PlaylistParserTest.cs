using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Company.Playlist.Test
{
    [TestClass]
    public class PlaylistParserTest
    {

        [TestMethod]
        public void GetPersonAvailability_BlankList_EmptyListReturned()
        {
            var list = new List<string>();
            var sut = new PlaylistParser();

            var returnList = sut.GetPersonAvailability(list);

            Assert.AreEqual(0, returnList.Count);
        }

        [TestMethod]
        public void GetPersonAvailability_SingleLineDoesntMatch_DoesntAdd()
        {
            var list = new List<string>();
            list.Add("Johnny section 1");
            var sut = new PlaylistParser();

            var returnList = sut.GetPersonAvailability(list);

            Assert.AreEqual(0, returnList.Count);
        }

        [TestMethod]
        public void GetPersonAvailability_SingleLine_GetName()
        {
            var list = new List<string>();
            list.Add("Johnny is available for section 1");
            var sut = new PlaylistParser();

            var returnList = sut.GetPersonAvailability(list);

            Assert.AreEqual(1, returnList.Count);
            Assert.AreEqual("Johnny", returnList[0].Name);
        }

        [TestMethod]
        public void GetPersonAvailability_TwoSameName_OnlyAddOnce()
        {
            var list = new List<string>();
            list.Add("Johnny is available for section 1");
            list.Add("Johnny is available for section 2");
            var sut = new PlaylistParser();

            var returnList = sut.GetPersonAvailability(list);

            Assert.AreEqual(1, returnList.Count);
            Assert.AreEqual("Johnny", returnList[0].Name);
        }

        [TestMethod]
        public void GetPersonAvailability_SingleOpenSection_SetsOpenSection()
        {
            var list = new List<string>();
            list.Add("Johnny is available for section 1");
            var sut = new PlaylistParser();

            var returnList = sut.GetPersonAvailability(list);

            Assert.AreEqual(1, returnList.Count);
            Assert.IsTrue(returnList[0].Available[0]);
            Assert.IsFalse(returnList[0].Available[1]);
        }

        [TestMethod]
        public void GetPersonAvailability_NameMultipleWords_GetsEntireName()
        {
            var list = new List<string>();
            list.Add("Johnny Five is available for section 1");
            var sut = new PlaylistParser();

            var returnList = sut.GetPersonAvailability(list);

            Assert.AreEqual("Johnny Five", returnList[0].Name);
        }

        [TestMethod]
        public void GetPersonAvailability_SingleOpenSection_SetsOpenSectionToSecont()
        {
            var list = new List<string>();
            list.Add("Johnny is available for section 2");
            var sut = new PlaylistParser();

            var returnList = sut.GetPersonAvailability(list);

            Assert.AreEqual(1, returnList.Count);
            Assert.IsFalse(returnList[0].Available[0]);
            Assert.IsTrue(returnList[0].Available[1]);
        }
        [TestMethod]
        public void GetSongRequests_BlankList_EmptyListReturned()
        {
            var list = new List<string>();
            var sut = new PlaylistParser();

            var returnList = sut.GetSongRequests(list);

            Assert.AreEqual(0, returnList.Count);
        }
        
        [TestMethod]
        public void GetSongRequests_SingleLineMatches_Added()
        {
            var list = new List<string>();
            list.Add("Bleed American - Jimmy Eat World 3:32 added by Lisa");
            var sut = new PlaylistParser();

            var returnList = sut.GetSongRequests(list);

            Assert.AreEqual(1, returnList.Count);
        }

        [TestMethod]
        public void GetSongRequests_SingleLineDoesntMatch_NotAdded()
        {
            var list = new List<string>();
            list.Add("Bleed American - Jimmy Eat World added by Lisa");
            var sut = new PlaylistParser();

            var returnList = sut.GetSongRequests(list);

            Assert.AreEqual(0, returnList.Count);
        }

        [TestMethod]
        public void GetSongRequests_SingleLineMatch_ParsesRegex()
        {
            var list = new List<string>();
            list.Add("Bleed American - Jimmy Eat World 3:32 added by Lisa");
            var sut = new PlaylistParser();

            var returnList = sut.GetSongRequests(list);

            Assert.AreEqual(1, returnList.Count);
            Assert.AreEqual("Bleed American", returnList[0].SongTitle);
            Assert.AreEqual("Jimmy Eat World", returnList[0].Band);
            Assert.AreEqual(new TimeSpan(0, 3, 32), returnList[0].Length);
            Assert.AreEqual("Lisa", returnList[0].Requester);
        }

        [TestMethod]
        public void GetSongRequests_NameMultipleWords_GetsEntireName()
        {
            var list = new List<string>();
            list.Add("American - Jimmy 3:32 added by Lisa Loeb");
            var sut = new PlaylistParser();

            var returnList = sut.GetSongRequests(list);

            Assert.AreEqual("Lisa Loeb", returnList[0].Requester);
        }

        [TestMethod]
        public void GetSongRequests_NumberInNameAndSong_AddsEntry()
        {
            var list = new List<string>();
            list.Add("System 11:11 - Powerman 5000 3:32 added by Lisa Loeb");
            var sut = new PlaylistParser();

            var returnList = sut.GetSongRequests(list);

            Assert.AreEqual("Lisa Loeb", returnList[0].Requester);
        }

    }
}
