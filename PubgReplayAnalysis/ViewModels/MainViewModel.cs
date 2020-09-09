using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Analysis;
using Analysis.JsonModels;
using PubgReplayAnalysis.Infrastructure;
using PubgReplayAnalysis.Models;
using PubgReplayAnalysis.Views;

namespace PubgReplayAnalysis.ViewModels
{
    internal class MainViewModel : Notifier
    {
        private readonly EventsView _eventsView = new EventsView();
        private readonly TimelineView _timelineView = new TimelineView();
        private bool _checkedCheckedEventsMode;
        private bool _checkedTimelineMode;
        private Control _contentRightPanel;
        private DisplayMode _displayMode;
        private List<PubgMatch> _pubgMatches;
        private int _replayFileSelectedIndex;
        private string _replayInfo;
        private List<ReplayInfo> _replayInfos;
        private Control _upperContent;

        public MainViewModel()
        {
            Command = new RelayCommand(CommandHandler);
            var culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;


            CheckedTimelineMode = true;
        }


        #region methods

        private void DisplayReplayInfo(int selectedIndex)
        {
            if (selectedIndex < 0 || selectedIndex >= ReplayFiles.Count) return;
            if (_pubgMatches.Count == 0) return;
            var index = ReplayFiles[selectedIndex].Id;
            var pubgMatch = _pubgMatches[index];

            ReplayInfo = _replayInfos[index].ToString();

            switch (_displayMode)
            {
                case DisplayMode.Timeline:
                    if (ContentRightPanel is TimelineView)
                    {
                        var timeLine = (TimelineViewModel) ContentRightPanel.DataContext;
                        timeLine.Initialize(pubgMatch);
                    }

                    break;
                case DisplayMode.Events:
                    if (ContentRightPanel is EventsView)
                    {
                        var eventsViewModel = (EventsViewModel) ContentRightPanel.DataContext;
                        eventsViewModel.Initialize(pubgMatch);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void SetMode(DisplayMode mode)
        {
            _displayMode = mode;

            switch (mode)
            {
                case DisplayMode.Timeline:
                    ContentRightPanel = _timelineView;
                    break;
                case DisplayMode.Events:
                    ContentRightPanel = _eventsView;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            if (ReplayFiles.Count > 0) DisplayReplayInfo(ReplayFileSelectedIndex);
        }

        #endregion

        #region commands

        public ICommand Command { get; }

        private void CommandHandler(object obj)
        {
            switch ((string) obj)

            {
                case "update":
                    Update();
                    break;
            }
        }

        private async void Update()
        {
            UpperContent = new Spinner();
            await Task.Run(() =>
            {
                var result = Reader.GetReplays(out _replayInfos, out _pubgMatches);
                if (result != "OK")
                {
                    MessageBox.Show(result, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var i = 0;
                var list = _replayInfos.Select(replayInfo => new ReplayFileModel
                    {
                        Duration = replayInfo.LengthInMs,
                        Id = i++,
                        MapName = replayInfo.MapName,
                        Name = replayInfo.FriendlyName,
                        TimeStamp = replayInfo.Timestamp
                    })
                    .ToList();
                list.Sort((x, y) => x.TimeStamp.CompareTo(y.TimeStamp));

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ReplayFiles.Clear();
                    foreach (var replayFileModel in list) ReplayFiles.Add(replayFileModel);
                    if (list.Count > 0) ReplayFileSelectedIndex = 0;
                });
            });

            UpperContent = null;
        }

        #endregion

        #region bindings

        public Control UpperContent
        {
            get => _upperContent;
            set
            {
                _upperContent = value;
                Notify();
            }
        }

        public bool CheckedTimelineMode
        {
            get => _checkedTimelineMode;
            set
            {
                _checkedTimelineMode = value;
                Notify();

                if (value) SetMode(DisplayMode.Timeline);
            }
        }

        public bool CheckedEventsMode
        {
            get => _checkedCheckedEventsMode;
            set
            {
                _checkedCheckedEventsMode = value;
                Notify();
                if (value)
                    SetMode(DisplayMode.Events);
            }
        }

        public int ReplayFileSelectedIndex
        {
            get => _replayFileSelectedIndex;
            set
            {
                _replayFileSelectedIndex = value;
                Notify();
                DisplayReplayInfo(value);
            }
        }

        public string ReplayInfo
        {
            get => _replayInfo;
            set
            {
                _replayInfo = value;
                Notify();
            }
        }

        public ObservableCollection<ReplayFileModel> ReplayFiles { get; set; } =
            new ObservableCollection<ReplayFileModel>();


        public Control ContentRightPanel
        {
            get => _contentRightPanel;
            set
            {
                _contentRightPanel = value;
                Notify();
            }
        }

        #endregion
    }
}