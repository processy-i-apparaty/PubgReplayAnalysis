using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Analysis;
using Analysis.JsonModels;
using PubgReplayAnalysis.Models;

namespace PubgReplayAnalysis
{
    internal class EventHelper
    {
        public static List<ReplayEvent> FindActiveEvents<T>(Dictionary<T, List<TimelineEvent>> allEvents,
            TimeSpan maxTimeSpan, int minEvents)
        {
            var activeEvents = new List<ReplayEvent>();
            var eventsDictionary = new Dictionary<T, Tuple<TimeSpan, TimeSpan, int, string>>();

            var type = 0;
            if (typeof(T) == typeof(string)) type = 1;
            if (typeof(T) == typeof(int)) type = 2;


            foreach (var keyValuePair in allEvents)
            {
                var key = keyValuePair.Key;
                foreach (var timelineEvent in keyValuePair.Value)
                {
                    switch (type)
                    {
                        case 1:
                        {
                            var p = (string) (object) key;
                            if (p == null || p != timelineEvent.Killer) continue;
                            break;
                        }
                        case 2:
                        {
                            var p = (int) (object) key;
                            if (!int.TryParse(timelineEvent.KillerTeam, out var killerTeam)) continue;
                            if (p != killerTeam) continue;
                            break;
                        }
                    }


                    var currentTime = TimeSpan.ParseExact(timelineEvent.TimeString, "mm':'ss", null);

                    if (eventsDictionary.ContainsKey(key))
                    {
                        Debug.WriteLine($"playerEventsDictionary contains {key}");

                        var (lastEventTime, firstEventTime, eventsCount, team) = eventsDictionary[key];
                        var timeDiff = currentTime - lastEventTime;
                        if (timeDiff <= maxTimeSpan)
                        {
                            eventsDictionary[key] =
                                new Tuple<TimeSpan, TimeSpan, int, string>(currentTime, firstEventTime, eventsCount + 1,
                                    team);

                            Debug.WriteLine(
                                $"timeDiff {timeDiff} <= maxTimeSpan {maxTimeSpan}. currentTime: {currentTime}, firstEventTime: {firstEventTime}, eventsCount: {eventsCount + 1}");
                        }
                        else
                        {
                            if (eventsCount >= minEvents)
                            {
                                Debug.WriteLine(
                                    $"eventsCount {eventsCount} >= minEvents {minEvents}. adding event: {timelineEvent.TimeString}, {eventsCount} {firstEventTime}");

                                var p = (key as object).ToString();
                                if (type == 2) p = $"Team {p}";

                                activeEvents.Add(new ReplayEvent
                                {
                                    Player = p,
                                    TimeString = $"{lastEventTime.Minutes:00}:{lastEventTime.Seconds:00}",
                                    Team = timelineEvent.KillerTeam,
                                    EventType = $"Active play. {eventsCount} events. Started at {firstEventTime}"
                                });
                            }

                            eventsDictionary[key] =
                                new Tuple<TimeSpan, TimeSpan, int, string>(currentTime, currentTime, 1, team);

                            Debug.WriteLine(
                                $"timeDiff {timeDiff} > maxTimeSpan {maxTimeSpan}. currentTime: {currentTime}, firstEventTime: {firstEventTime}, eventsCount: 1");
                        }
                    }
                    else
                    {
                        eventsDictionary.Add(key,
                            new Tuple<TimeSpan, TimeSpan, int, string>(currentTime, currentTime, 1,
                                timelineEvent.KillerTeam));
                        Debug.WriteLine($"playerEventsDictionary NOT contains {key}, adding time: {currentTime}");
                    }
                }
            }


            foreach (var keyValuePair in eventsDictionary)
            {
                var player = keyValuePair.Key;
                var (lastTime, firstTime, eventsCount, team) = keyValuePair.Value;

                var p = (player as object).ToString();
                if (type == 2) p = $"Team {p}";

                if (eventsCount >= minEvents)
                    activeEvents.Add(new ReplayEvent
                    {
                        Player = p,
                        TimeString = $"{lastTime.Minutes:00}:{lastTime.Seconds:00}",
                        Team = team,
                        EventType = $"Active play. {eventsCount} events. started at {firstTime}."
                    });
            }


            return activeEvents;
        }

        public static List<ReplayEvent> FindTeamDeathsEvents(Dictionary<int, List<TimelineEvent>> teamEvents)
        {
            var teamDeathEvents = new List<ReplayEvent>();
            var teamDeaths = new Dictionary<int, int>();
            foreach (var teamEvent in teamEvents)
            {
                var teamNumber = teamEvent.Key;
                foreach (var timelineEvent in teamEvent.Value)
                {
                    var lifeState = timelineEvent.State;
                    if (lifeState != Enums.LifeState.Killed) continue;

                    if (!int.TryParse(timelineEvent.VictimTeam, out var victimTeam)) continue;
                    if (victimTeam != teamNumber) continue;

                    if (teamDeaths.ContainsKey(teamNumber)) teamDeaths[teamNumber]++;
                    else teamDeaths.Add(teamNumber, 1);

                    if (teamDeaths[teamNumber] == 4)
                        teamDeathEvents.Add(new ReplayEvent
                        {
                            EventType =
                                $"team wiped by {timelineEvent.Killer} team #{timelineEvent.KillerTeam}",
                            Player = timelineEvent.Victim,
                            Team = timelineEvent.VictimTeam,
                            TimeString = timelineEvent.TimeString
                        });
                }
            }

            return teamDeathEvents;
        }

        public static void GetEventsFromMatch(PubgMatch pubgMatch,
            out Dictionary<string, List<TimelineEvent>> playerEvents,
            out Dictionary<int, List<TimelineEvent>> teamEvents)
        {
            GetTimelineEventsSorted(pubgMatch, out var timelineEvents);

            teamEvents = new Dictionary<int, List<TimelineEvent>>();
            playerEvents = new Dictionary<string, List<TimelineEvent>>();
            var playerStateSummaries = pubgMatch.PubgData.PlayerStateSummaries;

            foreach (var timelineEvent in timelineEvents)
            {
                var killer = timelineEvent.Killer;
                var victim = timelineEvent.Victim;

                AddPlayerEvent(killer, timelineEvent, playerStateSummaries, playerEvents);
                AddPlayerEvent(victim, timelineEvent, playerStateSummaries, playerEvents);

                AddTeamEvent(killer, timelineEvent, playerStateSummaries, teamEvents);
                AddTeamEvent(victim, timelineEvent, playerStateSummaries, teamEvents);
            }
        }

        private static void AddPlayerEvent(string player, TimelineEvent timelineEvent,
            IEnumerable<PlayerStateSummary> playerStateSummaries, IDictionary<string, List<TimelineEvent>> playerEvents)
        {
            var playerState = playerStateSummaries.FirstOrDefault(s => s.PlayerName == player);
            if (playerState == null) return;

            if (playerEvents.ContainsKey(player))
                playerEvents[player].Add(timelineEvent);
            else
                playerEvents.Add(player, new List<TimelineEvent> {timelineEvent});
        }

        private static void AddTeamEvent(string player, TimelineEvent timelineEvent,
            IEnumerable<PlayerStateSummary> playerStateSummaries, IDictionary<int, List<TimelineEvent>> teamEvents)
        {
            var playerState = playerStateSummaries.FirstOrDefault(s => s.PlayerName == player);
            if (playerState == null) return;

            var teamNumber = playerState.TeamNumber;
            if (teamEvents.ContainsKey(teamNumber))
                teamEvents[teamNumber].Add(timelineEvent);
            else
                teamEvents.Add(teamNumber, new List<TimelineEvent> {timelineEvent});
        }

        private static void GetTimelineEventsSorted(PubgMatch pubgMatch, out List<TimelineEvent> timelineEvents)
        {
            var match = pubgMatch.Copy();
            match.GameEvents.Sort((x, y) => x.Time1.CompareTo(y.Time1));
            timelineEvents = (from matchGameEvent in match.GameEvents
                where matchGameEvent.Group >= Enums.EventGroup.Groggy
                let id = matchGameEvent.Id
                let kill = match.Kills.FirstOrDefault(x => x.KillId == id)
                where kill != null
                let killer = GetPlayer(kill.KillerNetId, kill.KillerPlayerId, match.PubgData.PlayerStateSummaries)
                let victim = GetPlayer(kill.VictimNetId, kill.VictimPlayerId, match.PubgData.PlayerStateSummaries)
                let state = kill.BGroggy ? Enums.LifeState.Knocked : Enums.LifeState.Killed
                select new TimelineEvent
                {
                    TimeString = $"{matchGameEvent.Time1:mm\\:ss}",
                    Killer = killer == null ? "" : killer.PlayerName,
                    Victim = victim == null ? "" : victim.PlayerName,
                    KillerTeam = killer?.TeamNumber.ToString() ?? "",
                    VictimTeam = victim?.TeamNumber.ToString() ?? "",
                    State = state,
                    DamageArea = kill.DamageReason,
                    DamageCategory = kill.DamageTypeCategory
                }).ToList();
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