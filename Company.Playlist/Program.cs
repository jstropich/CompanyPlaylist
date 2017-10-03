using System;
using System.Collections.Generic;
using System.IO;

namespace Company.Playlist
{
    class Program
    {
        private const string fileName = "PlaylistInfo.txt";
        static void Main(string[] args)
        {
            IEnumerable<string> lines = File.ReadAllLines(fileName);
            var parser = new PlaylistParser();
            var availability = parser.GetPersonAvailability(lines);
            var requests = parser.GetSongRequests(lines);

            var builder = new PlaylistBuilder(requests, availability);
            var outputFirstPeriod = builder.GetPlaylist(1);
            var outputSecondPeriod = builder.GetPlaylist(2);

            Console.WriteLine("The first 30 minutes will be:");
            DisplayPlaylist(outputFirstPeriod);
            Console.WriteLine("The final 30 minutes will be:");
            DisplayPlaylist(outputSecondPeriod);
        }

        private static void DisplayPlaylist(List<Models.SongRequest> outputFirstPeriod)
        {
            for (int i = 0; i < outputFirstPeriod.Count; i++)
                Console.WriteLine("\t{0}. {1}", i + 1, outputFirstPeriod[i].TrackTitle);
        }
    }
}
