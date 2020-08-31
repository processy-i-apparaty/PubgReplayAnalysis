using System;
using System.Collections.ObjectModel;
using Analysis;
using PubgReplayAnalysis.Infrastructure;
using PubgReplayAnalysis.Models;

namespace PubgReplayAnalysis.ViewModels
{
    internal class EventsViewModel : Notifier
    {
        private readonly CheckReplayEventsModel _checkReplayEvents = new CheckReplayEventsModel();
        private PubgMatch _pubgMatch;

        public EventsViewModel()
        {
            ReplayEvents = new ObservableCollection<ReplayEvent>();
        }

        public ObservableCollection<ReplayEvent> ReplayEvents { get; set; }

        public bool CheckDeaths
        {
            get => _checkReplayEvents.TeamDeaths;
            set
            {
                _checkReplayEvents.TeamDeaths = value;
                Notify();
                Initialize(_pubgMatch);
            }
        }

        public bool TeamEvents
        {
            get => _checkReplayEvents.TeamEvents;
            set
            {
                _checkReplayEvents.TeamEvents = value;
                Notify();
                Initialize(_pubgMatch);
            }
        }

        public bool PlayerEvents
        {
            get => _checkReplayEvents.PlayerEvents;
            set
            {
                _checkReplayEvents.PlayerEvents = value;
                Notify();
                Initialize(_pubgMatch);
            }
        }

        public int MaxTime
        {
            get => (int) _checkReplayEvents.MaxTimeSpan.TotalSeconds;
            set
            {
                var seconds = value;
                if (seconds < 1) seconds = 1;
                if (seconds > 30) seconds = 30;
                _checkReplayEvents.MaxTimeSpan = TimeSpan.FromSeconds(seconds);
                Notify();
                Initialize(_pubgMatch);
            }
        }

        public int MinEvents
        {
            get => _checkReplayEvents.MinEvents;
            set
            {
                var minEvents = value;
                if (minEvents < 2) minEvents = 2;
                if (minEvents > 20) minEvents = 20;
                _checkReplayEvents.MinEvents = minEvents;
                Notify();
                Initialize(_pubgMatch);
            }
        }

        public void Initialize(PubgMatch pubgMatch)
        {
            if (pubgMatch == null) return;
            _pubgMatch = pubgMatch;
            DataHelper.DisplayEvents(pubgMatch, ReplayEvents, _checkReplayEvents);
        }
    }
}