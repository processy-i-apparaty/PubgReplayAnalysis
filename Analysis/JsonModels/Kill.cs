using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Analysis.JsonModels
{
    public class Kill
    {
        [JsonProperty("killerNetId")] public string KillerNetId { get; set; }

        [JsonProperty("killerName")] public string KillerName { get; set; }

        [JsonProperty("victimNetId")] public string VictimNetId { get; set; }

        [JsonProperty("victimName")] public string VictimName { get; set; }

        [JsonProperty("damageCauseClassName")] public string DamageCauseClassName { get; set; }

        [JsonProperty("damageTypeCategory")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.DamageTypeCategory DamageTypeCategory { get; set; }

        [JsonProperty("damageReason")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.DamageReason DamageReason { get; set; }

        [JsonProperty("bGroggy")] public bool BGroggy { get; set; }

        [JsonProperty("killerPlayerId")] public string KillerPlayerId { get; set; }

        [JsonProperty("victimPlayerId")] public string VictimPlayerId { get; set; }
        public string KillId { get; set; }

        public override string ToString()
        {
            return $"[{DamageTypeCategory}]\t {KillerName} X {VictimName}";
        }
    }
}