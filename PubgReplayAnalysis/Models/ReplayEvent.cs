using Analysis;

namespace PubgReplayAnalysis.Models
{
    public class ReplayEvent
    {
        public string TimeString { get; set; }
        public string Killer { get; set; }
        public string KillerTeam { get; set; }

        public string Victim { get; set; }
        public string VictimTeam { get; set; }
        public string State { get; set; }
        public Enums.DamageReason DamageArea { get; set; }
        public Enums.DamageTypeCategory DamageCategory { get; set; }
    }
}