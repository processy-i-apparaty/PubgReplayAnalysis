using System.Collections.Generic;
using Analysis.JsonModels;

namespace Analysis
{
    public class PubgMatch
    {
        public PubgData PubgData { get; set; }
        public Weather Weather { get; set; }
        public List<Kill> Kills { get; } = new List<Kill>();
        public List<GameEvent> GameEvents { get; } = new List<GameEvent>();
    }
}