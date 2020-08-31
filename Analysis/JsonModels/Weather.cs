using System;
using Newtonsoft.Json;

namespace Analysis.JsonModels
{
    [Serializable]
    public class Weather
    {
        [JsonProperty("weatherId")]
        public string WeatherId { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }

        [JsonProperty("weatherLevel")]
        public string WeatherLevel { get; set; }
    }
}