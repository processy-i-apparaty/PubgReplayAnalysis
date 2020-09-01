using System;
using Analysis.JsonConverters;
using Newtonsoft.Json;

namespace Analysis.JsonModels
{
    public class Camera
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("group")] public string Group { get; set; }

        [JsonProperty("meta")] public string Meta { get; set; }

        [JsonProperty("time1")]
        [JsonConverter(typeof(MillisecondsTimeSpanConverter))]
        public TimeSpan Time1 { get; set; }


        [JsonProperty("time2")]
        [JsonConverter(typeof(MillisecondsTimeSpanConverter))]
        public TimeSpan Time2 { get; set; }

        [JsonProperty("data")] public string Data { get; set; }
    }
}