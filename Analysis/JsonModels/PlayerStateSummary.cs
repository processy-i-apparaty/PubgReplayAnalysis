using Newtonsoft.Json;

namespace Analysis.JsonModels
{
    public class PlayerStateSummary
    {
        [JsonProperty("uniqueId")] public string UniqueId { get; set; }
        public string ShortId { get; set; }

        [JsonProperty("playerName")] public string PlayerName { get; set; }

        [JsonProperty("teamNumber")] public long TeamNumber { get; set; }

        [JsonProperty("ranking")] public long Ranking { get; set; }

        [JsonProperty("headShots")] public long HeadShots { get; set; }

        [JsonProperty("numKills")] public long NumKills { get; set; }

        [JsonProperty("totalGivenDamages")] public double TotalGivenDamages { get; set; }

        [JsonProperty("longestDistanceKill")] public double LongestDistanceKill { get; set; }

        [JsonProperty("totalMovedDistanceMeter")]
        public double TotalMovedDistanceMeter { get; set; }
    }
}