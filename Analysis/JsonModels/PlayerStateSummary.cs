using System;
using Newtonsoft.Json;

namespace Analysis.JsonModels
{
    [Serializable]
    public class PlayerStateSummary
    {
        [JsonProperty("uniqueId")] public string UniqueId { get; set; }
        public string ShortId { get; set; }

        [JsonProperty("playerName")] public string PlayerName { get; set; }

        [JsonProperty("teamNumber")] public int TeamNumber { get; set; }

        [JsonProperty("ranking")] public long Ranking { get; set; }

        [JsonProperty("headShots")] public int HeadShots { get; set; }

        [JsonProperty("numKills")] public int NumKills { get; set; }

        [JsonProperty("totalGivenDamages")] public double TotalGivenDamages { get; set; }

        [JsonProperty("longestDistanceKill")] public double LongestDistanceKill { get; set; }

        [JsonProperty("totalMovedDistanceMeter")]
        public double TotalMovedDistanceMeter { get; set; }
    }
}