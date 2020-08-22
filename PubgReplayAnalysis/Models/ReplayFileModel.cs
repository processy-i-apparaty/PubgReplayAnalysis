using System;

namespace PubgReplayAnalysis.Models
{
    internal class ReplayFileModel
    {
        private string _name;
        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                var spl = value.Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries);
                _name = $"{spl[2]} {spl[3]} {spl[4]}";
            }
        }

        public DateTime TimeStamp { get; set; }
        public string TimeStampString => $"{TimeStamp:yyMMdd ddd} {TimeStamp:HH:mm:ss}";
        public string DurationString => Duration.ToString("mm\\:ss");
        public TimeSpan Duration { get; set; }
        public string MapName { get; set; }
    }
}