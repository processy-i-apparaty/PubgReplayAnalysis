using System;
using Newtonsoft.Json;

namespace Analysis.JsonModels
{
    [Serializable]
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
        public int NumPlayers { get; set; }

        [JsonProperty("numTeams")]
        public int NumTeams { get; set; }

        [JsonProperty("playerStateSummaries")]
        public PlayerStateSummary[] PlayerStateSummaries { get; set; }

        // public PubgData Copy()
        // {
        //     var data = new PubgData
        //     {
        //         MatchId = $"{MatchId}",
        //         BIsServerRecording = BIsServerRecording,
        //         RecordUserId = $"{RecordUserId}",
        //         RecordAccountId = $"{RecordAccountId}",
        //         RecordUserNickName = $"{RecordUserNickName}",
        //         MapName = $"{MapName}",
        //         NumPlayers = NumPlayers,
        //         NumTeams = NumTeams,
        //         RegionName = $"{RegionName}",
        //         PlayerStateSummaries = new PlayerStateSummary[PlayerStateSummaries.Length],
        //
        //     };
        //     for (int i = 0; i < PlayerStateSummaries.Length; i++)
        //     {
        //         data.PlayerStateSummaries[i] = PlayerStateSummaries[i].Copy();
        //     }
        // }
    }
}