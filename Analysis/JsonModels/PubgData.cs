using Newtonsoft.Json;

namespace Analysis.JsonModels
{
    public class PubgData
    {
        [JsonProperty("matchId")]
        public string MatchId { get; set; }

        [JsonProperty("bIsServerRecording")]
        public bool BIsServerRecording { get; set; }

        [JsonProperty("recordUserId")]
        public string RecordUserId { get; set; }

        [JsonProperty("recordAccountId")]
        public string RecordAccountId { get; set; }

        [JsonProperty("recordUserNickName")]
        public string RecordUserNickName { get; set; }

        [JsonProperty("mapName")]
        public string MapName { get; set; }

        [JsonProperty("weatherName")]
        public string WeatherName { get; set; }

        [JsonProperty("regionName")]
        public string RegionName { get; set; }

        [JsonProperty("numPlayers")]
        public long NumPlayers { get; set; }

        [JsonProperty("numTeams")]
        public long NumTeams { get; set; }

        [JsonProperty("playerStateSummaries")]
        public PlayerStateSummary[] PlayerStateSummaries { get; set; }
    }
}