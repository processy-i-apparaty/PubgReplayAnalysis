using System;

namespace PubgReplayAnalysis.Models
{
    public class CheckReplayEventsModel
    {
        public bool TeamDeaths { get; set; } = true;
        public bool PlayerEvents { get; set; } = true;
        public bool TeamEvents { get; set; } = true;
        public TimeSpan MaxTimeSpan { get; set; } = TimeSpan.FromSeconds(10.0);
        public int MinEvents { get; set; } = 3;
    }
}