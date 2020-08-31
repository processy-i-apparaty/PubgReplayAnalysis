using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Analysis;
using Analysis.JsonModels;
using PubgReplayAnalysis.Models;

namespace PubgReplayAnalysis
{
    public static class DataHelper
    {
        public static void DisplayEvents(PubgMatch pubgMatch, ObservableCollection<ReplayEvent> replayEvents,
            CheckReplayEventsModel checkReplayEvents)
        {
            replayEvents.Clear();
            EventHelper.GetEventsFromMatch(pubgMatch, out var playerEvents, out var teamEvents);

            var maxTime = checkReplayEvents.MaxTimeSpan;
            var minEvents = checkReplayEvents.MinEvents;

            var allEvents = new List<ReplayEvent>();

            if (checkReplayEvents.TeamDeaths) allEvents.AddRange(EventHelper.FindTeamDeathsEvents(teamEvents));
            if (checkReplayEvents.PlayerEvents)
                allEvents.AddRange(EventHelper.FindActiveEvents(playerEvents, maxTime, minEvents));
            if (checkReplayEvents.TeamEvents)
                allEvents.AddRange(EventHelper.FindActiveEvents(teamEvents, maxTime, minEvents));

            allEvents.Sort((x, y) => x.Time.CompareTo(y.Time));

            foreach (var replayEvent in allEvents) replayEvents.Add(replayEvent);
        }


        public static void DisplayTimeLine(PubgMatch pubgMatch, ObservableCollection<TimelineEvent> timelineEvents,
            ObservableCollection<CheckModel> checkDamageTypeCategory)
        {
            timelineEvents.Clear();
            var match = pubgMatch.Copy();
            match.GameEvents.Sort((x, y) => x.Time1.CompareTo(y.Time1));
            foreach (var matchGameEvent in match.GameEvents)
            {
                if (matchGameEvent.Group < Enums.EventGroup.Groggy) continue;
                var id = matchGameEvent.Id;
                var kill = match.Kills.FirstOrDefault(x => x.KillId == id);
                if (kill == null) continue;


                var killDamageTypeCategory = kill.DamageTypeCategory;
                var check = checkDamageTypeCategory
                    .Where(checkModel => checkModel.CheckName == killDamageTypeCategory.ToString())
                    .Any(checkModel => !checkModel.IsChecked);
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

                var state = kill.BGroggy ? Enums.LifeState.Knocked : Enums.LifeState.Killed;
                var re = new TimelineEvent
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
                timelineEvents.Add(re);
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