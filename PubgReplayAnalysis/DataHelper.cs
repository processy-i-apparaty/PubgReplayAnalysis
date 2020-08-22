using System.Collections.ObjectModel;
using System.Linq;
using Analysis;
using Analysis.JsonModels;
using PubgReplayAnalysis.Models;

namespace PubgReplayAnalysis
{
    public static class DataHelper
    {
        public static void DisplayTimeLine(PubgMatch match, ObservableCollection<ReplayEvent> replayEvents, ObservableCollection<CheckModel> checkDamageTypeCategory)
        {
            replayEvents.Clear();
            match.GameEvents.Sort((x, y) => x.Time1.CompareTo(y.Time1));
            foreach (var matchGameEvent in match.GameEvents)
            {
                if (matchGameEvent.Group < Enums.EventGroup.Groggy) continue;
                var id = matchGameEvent.Id;
                var kill = match.Kills.FirstOrDefault(x => x.KillId == id);
                if (kill == null) continue;

                
                var killDamageTypeCategory = kill.DamageTypeCategory;
                var check = checkDamageTypeCategory.Where(checkModel => checkModel.CheckName == killDamageTypeCategory.ToString()).Any(checkModel => !checkModel.IsChecked);
                
                // var check = false;
                // foreach (var checkModel in CheckDamageTypeCategory)
                //     if (checkModel.CheckName == killDamageTypeCategory.ToString())
                //         if (!checkModel.IsChecked)
                //         {
                //             check = true;
                //             break;
                //         }

                if (check) continue;


                var killer = GetPlayer(kill.KillerNetId, kill.KillerPlayerId, match.PubgData.PlayerStateSummaries);
                var victim = GetPlayer(kill.VictimNetId, kill.VictimPlayerId, match.PubgData.PlayerStateSummaries);

                var state = kill.BGroggy ? "knocked" : "killed";
                var re = new ReplayEvent
                {
                    TimeString = $"{matchGameEvent.Time1:mm\\:ss}",
                    Killer = killer == null ? "" : killer.PlayerName,
                    Victim = victim == null ? "" : victim.PlayerName,
                    KillerTeam = killer?.TeamNumber.ToString() ?? "",
                    VictimTeam = victim?.TeamNumber.ToString() ?? "",
                    State = state,
                    DamageArea = kill.DamageReason,
                    DamageCategory = kill.DamageTypeCategory
                };
                replayEvents.Add(re);
            }
        }

        private static PlayerStateSummary GetPlayer(string id, string shortId, PlayerStateSummary[] playerStates)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var playerState = playerStates.FirstOrDefault(x => x.UniqueId == id);
                if (playerState != null) return playerState;
            }

            if (string.IsNullOrWhiteSpace(shortId)) return null;
            {
                var playerState = playerStates.FirstOrDefault(x => x.ShortId == shortId);
                if (playerState != null) return playerState;
            }

            return null;
        }

    }
}