using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Analysis.JsonModels;
using Newtonsoft.Json;

namespace Analysis
{
    public static class Reader
    {
        public static string GetReplays(out List<ReplayInfo> replayInfos, out List<PubgMatch> pubgMatches)
        {
            replayInfos = new List<ReplayInfo>();
            pubgMatches = new List<PubgMatch>();

            try
            {

                if (!Directory.Exists(Values.ReplaysDirectory)) return "DEMOS DIRECTORY NOT EXIST!";

                foreach (var directory in Directory.GetDirectories(Values.ReplaysDirectory))
                    if (directory.Contains("match."))
                    {
                        GetReplay(directory, out var pubgMatch, out var replayInfo);
                        replayInfos.Add(replayInfo);
                        pubgMatches.Add(pubgMatch);
                    }

                if (replayInfos.Count > 0 && pubgMatches.Count > 0) return "OK";
                return "SOMETHING WRONG. DEMOS DIRECTORY MAY BE EMPTY";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private static void GetReplay(string directory, out PubgMatch match, out ReplayInfo replayInfo)
        {
            var jsonReplayInfo = Ue4StringSerializer(directory + "\\PUBG.replayinfo");
            match = ReadSummary(directory);
            replayInfo = JsonConvert.DeserializeObject<ReplayInfo>(jsonReplayInfo);
        }

        private static void GetData(string path, ref PubgMatch pubgMatch)
        {
            var data = path + "\\data";
            var files = Directory.GetFiles(data);
            foreach (var fileName in files)
            {
                var info = new FileInfo(fileName);
                if (info.Length >= 100000) continue;
                var json = Ue4StringSerializer(fileName, 1);
                // Console.WriteLine($"{info.Name} : {info.Length} bytes\n{json}\n");
                var kill = JsonConvert.DeserializeObject<Kill>(json);
                if (kill.DamageCauseClassName != null)
                {
                    kill.KillId = fileName.Substring(fileName.LastIndexOf('\\')).Replace("\\", "");
                    pubgMatch.Kills.Add(kill);
                    continue;
                }

                var pubgData = JsonConvert.DeserializeObject<PubgData>(json);
                if (pubgData.MatchId != null)
                {
                    pubgMatch.PubgData = pubgData;
                    continue;
                }

                var weather = JsonConvert.DeserializeObject<Weather>(json);
                if (weather.WeatherId != null) pubgMatch.Weather = weather;
            }
        }

        private static void GetEvents(string path, ref PubgMatch pubgMatch)
        {
            var events = path + "\\events";
            var files = Directory.GetFiles(events);
            foreach (var file in files)
            {
                var json = Ue4StringSerializer(file);
                var gameEvent = JsonConvert.DeserializeObject<GameEvent>(json);
                pubgMatch.GameEvents.Add(gameEvent);
            }
        }

        private static PubgMatch ReadSummary(string path)
        {
            var pubgMatch = new PubgMatch();
            GetData(path, ref pubgMatch);
            GetEvents(path, ref pubgMatch);
            GetShortIds(ref pubgMatch);
            return pubgMatch;
        }

        private static void GetShortIds(ref PubgMatch pubgMatch)
        {
            foreach (var kill in pubgMatch.Kills)
            {
                SetShortId(kill.KillerNetId, kill.KillerPlayerId, pubgMatch.PubgData.PlayerStateSummaries);
                SetShortId(kill.VictimNetId, kill.VictimPlayerId, pubgMatch.PubgData.PlayerStateSummaries);
            }
        }

        private static void SetShortId(string netId, string playerId, IEnumerable<PlayerStateSummary> stateSummaries)
        {
            if (netId == null || playerId == null) return;
            var player = stateSummaries.FirstOrDefault(x => x.UniqueId == netId);
            if (player == null) return;
            if (player.ShortId != null) return;
            player.ShortId = playerId;
        }


        private static string Ue4StringSerializer(string path, int encodedOffset = 0)
        {
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var lengthBytes = new byte[4];
            fs.Read(lengthBytes, 0, lengthBytes.Length);
            var bytesToRead = BitConverter.ToInt32(lengthBytes, 0);

            var bytesUnencoded = new byte[bytesToRead];
            for (var i = 0; i < bytesToRead; i++)
            {
                var byteEncoded = fs.ReadByte();

                if (byteEncoded > 0
                ) //if the byte is zero (technically should only be handled at the end per numInit (https://github.com/numinit)'s specifications but I'm lazy
                    bytesUnencoded[i] = (byte) (byteEncoded + encodedOffset);
            }

            fs.Close();

            var stringBytesLength = bytesUnencoded[bytesUnencoded.Length - 1] == 0
                ? bytesUnencoded.Length - 1
                : bytesUnencoded.Length; // Skip last byte if its zero
            return
                Encoding.ASCII.GetString(bytesUnencoded, 0,
                    stringBytesLength); // take all the bytes, put the array into UTF8 encoding and return it
        }
    }
}