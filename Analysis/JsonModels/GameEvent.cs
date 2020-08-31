using System;
using Analysis.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Analysis.JsonModels
{
    [Serializable]
    public class GameEvent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("group")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.EventGroup Group { get; set; }

        [JsonProperty("meta")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public string Meta { get; set; }

        [JsonProperty("time1")]
        [JsonConverter(typeof(MillisecondsTimeSpanConverter))]
        public TimeSpan Time1 { get; set; }

        [JsonProperty("time2")]
        [JsonConverter(typeof(MillisecondsTimeSpanConverter))]
        public TimeSpan Time2 { get; set; }

        public override string ToString()
        {
            return $"[{Group}] {Meta} {Time1} {Time2} {Id}";
        }
    }
}