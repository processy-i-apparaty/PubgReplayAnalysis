using System;
using Analysis.JsonConverters;
using Newtonsoft.Json;

namespace Analysis.JsonModels
{
    public class ReplayInfo
    {
        [JsonProperty("LengthInMS")] [JsonConverter(typeof(MillisecondsTimeSpanConverter))] public TimeSpan LengthInMs { get; set; }

        [JsonProperty("NetworkVersion")] public long NetworkVersion { get; set; }

        [JsonProperty("Changelist")] public long Changelist { get; set; }

        [JsonProperty("FriendlyName")] public string FriendlyName { get; set; }

        [JsonProperty("DemoFileLastOffset")] public long DemoFileLastOffset { get; set; }

        [JsonProperty("Timestamp")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("SizeInBytes")] public long SizeInBytes { get; set; }

        [JsonProperty("bShouldKeep")] public bool BShouldKeep { get; set; }

        [JsonProperty("bIncomplete")] public bool BIncomplete { get; set; }

        [JsonProperty("bIsServerRecording")] public bool BIsServerRecording { get; set; }

        [JsonProperty("Mode")] public string Mode { get; set; }

        [JsonProperty("RecordUserId")] public string RecordUserId { get; set; }

        [JsonProperty("RecordUserNickName")] public string RecordUserNickName { get; set; }

        [JsonProperty("MapName")] public string MapName { get; set; }

        [JsonProperty("GameVersion")] public string GameVersion { get; set; }

        [JsonProperty("MK3DReplayVersion")] public long Mk3DReplayVersion { get; set; }

        public override string ToString()
        {
            var str = string.Empty;
            str += $"Friendly Name:\n{FriendlyName}\n";
            str += $"Timestamp: {Timestamp}\n";
            str += $"Length: {LengthInMs}\n";
            str += $"Network Version: {NetworkVersion}\n";
            str += $"Mode: {Mode}\n";
            str += $"RecordUserId: {RecordUserId}\n";
            str += $"RecordUserId: {RecordUserNickName}\n";
            str += $"Map Name: {MapName}\n";
            str += $"Game Version: {GameVersion}";
            return str;
        }
    }
}