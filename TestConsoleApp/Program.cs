using System;
using System.Linq;
using Analysis;
using Analysis.JsonModels;

namespace TestConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Reader.GetReplays(out var replayInfos, out var pubgMatches);

            var info = replayInfos[0];
            var match = pubgMatches[0];

            Console.WriteLine(info+"\n");
            
            match.GameEvents.Sort((x, y) => x.Time1.CompareTo(y.Time1));
            foreach (var matchGameEvent in match.GameEvents)
            {
                if (matchGameEvent.Group < Enums.EventGroup.Groggy) continue;
                var id = matchGameEvent.Id;
                var kill = match.Kills.FirstOrDefault(x => x.KillId == id);
                if (kill == null) continue;
                var killer = GetName(kill.KillerNetId, kill.KillerPlayerId, match.PubgData.PlayerStateSummaries);
                var victim = GetName(kill.VictimNetId, kill.VictimPlayerId, match.PubgData.PlayerStateSummaries);

                var state = kill.BGroggy ? "knocked" : "killed";
                Console.WriteLine(
                    $"[{matchGameEvent.Time1:mm\\:ss}] {killer,-20} {state,-8} {victim,-20} {kill.DamageReason,-15} {kill.DamageTypeCategory,-10}");
                //match.PubgData.PlayerStateSummaries[0].UniqueId
            }


            // var replays = Reader.GetReplays();
            // foreach (var replay in replays) Console.WriteLine($"{replay[0]}\n{replay[1]}\n");

            Console.WriteLine("end.");
            Console.ReadKey();
        }

        private static string GetName(string id, string shortId, PlayerStateSummary[] playerStates)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var playerState = playerStates.FirstOrDefault(x => x.UniqueId == id);
                if (playerState != null) return $"[{playerState.TeamNumber:00}]{playerState.PlayerName}";
            }

            if (!string.IsNullOrWhiteSpace(shortId))
            {
                var playerState = playerStates.FirstOrDefault(x => x.ShortId == shortId);
                if (playerState != null) return $"[{playerState.TeamNumber:00}]{playerState.PlayerName}";
            }

            return "?";
        }
    }
}