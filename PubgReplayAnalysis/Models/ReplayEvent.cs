using System;

namespace PubgReplayAnalysis.Models
{
    public class ReplayEvent
    {
        public string TimeString
        {
            get => $"{Time.Minutes:00}:{Time.Seconds:00}";
            set => Time = TimeSpan.ParseExact(value, "mm':'ss", null);
        }

        public string Player { get; set; }
        public string Team { get; set; }
        public string EventType { get; set; }
        public TimeSpan Time;
    }
}