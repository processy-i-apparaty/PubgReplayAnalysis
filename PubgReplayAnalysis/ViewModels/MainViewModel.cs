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
        private bool _isGridUpdating;
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
            foreach (var dtc in Enum.GetNames(typeof(Enums.DamageTypeCategory)))
                CheckDamageTypeCategory.Add(new CheckModel(dtc, true, ActionCheckModel));
        }

        private void ActionCheckModel(string name, bool isChecked)
        {
            DisplayReplayInfo(ReplayFileSelectedIndex);
        }

        private void DisplayReplayInfo(int selectedIndex)
        {
            if (_isGridUpdating || selectedIndex < 0 || selectedIndex >= ReplayFiles.Count) return;
            var index = ReplayFiles[selectedIndex].Id;
            ReplayInfo = _replayInfos[index].ToString();
            DataHelper.DisplayTimeLine(_pubgMatches[selectedIndex], ReplayEvents, CheckDamageTypeCategory);
        }



        // private void DisplayTimeLine(int index)
        // {
        //     ReplayEvents.Clear();
        //     var match = _pubgMatches[index];
        //     match.GameEvents.Sort((x, y) => x.Time1.CompareTo(y.Time1));
        //     foreach (var matchGameEvent in match.GameEvents)
        //     {
        //         if (matchGameEvent.Group < Enums.EventGroup.Groggy) continue;
        //         var id = matchGameEvent.Id;
        //         var kill = match.Kills.FirstOrDefault(x => x.KillId == id);
        //         if (kill == null) continue;
        //
        //         var check = false;
        //         var kdtc = kill.DamageTypeCategory;
        //         foreach (var checkModel in CheckDamageTypeCategory)
        //             if (checkModel.CheckName == kdtc.ToString())
        //                 if (!checkModel.IsChecked)
        //                 {
        //                     check = true;
        //                     break;
        //                 }
        //
        //         if (check) continue;
        //
        //
        //         var killer = GetPlayer(kill.KillerNetId, kill.KillerPlayerId, match.PubgData.PlayerStateSummaries);
        //         var victim = GetPlayer(kill.VictimNetId, kill.VictimPlayerId, match.PubgData.PlayerStateSummaries);
        //
        //         var state = kill.BGroggy ? "knocked" : "killed";
        //         var re = new ReplayEvent
        //         {
        //             TimeString = $"{matchGameEvent.Time1:mm\\:ss}",
        //             Killer = killer == null ? "" : killer.PlayerName,
        //             Victim = victim == null ? "" : victim.PlayerName,
        //             KillerTeam = killer?.TeamNumber.ToString() ?? "",
        //             VictimTeam = victim?.TeamNumber.ToString() ?? "",
        //             State = state,
        //             DamageArea = kill.DamageReason,
        //             DamageCategory = kill.DamageTypeCategory
        //         };
        //         ReplayEvents.Add(re);
        //         //match.PubgData.PlayerStateSummaries[0].UniqueId
        //     }
        // }

        // private static PlayerStateSummary GetPlayer(string id, string shortId, PlayerStateSummary[] playerStates)
        // {
        //     if (!string.IsNullOrWhiteSpace(id))
        //     {
        //         var playerState = playerStates.FirstOrDefault(x => x.UniqueId == id);
        //         if (playerState != null) return playerState;
        //     }
        //
        //     if (!string.IsNullOrWhiteSpace(shortId))
        //     {
        //         var playerState = playerStates.FirstOrDefault(x => x.ShortId == shortId);
        //         if (playerState != null) return playerState;
        //     }
        //
        //     return null;
        // }


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
            _isGridUpdating = true;
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
                    foreach (var fileModel in list) ReplayFiles.Add(fileModel);
                    list.Clear();
                    list = null;
                });
            });
            UpperContent = null;
            _isGridUpdating = false;
            if (ReplayFiles.Count > 0) ReplayFileSelectedIndex = 0;
        }

        #endregion

        #region bindings

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

        public ObservableCollection<ReplayFileModel> ReplayFiles { get; set; } =
            new ObservableCollection<ReplayFileModel>();

        public string ReplayInfo
        {
            get => _replayInfo;
            set
            {
                _replayInfo = value;
                Notify();
            }
        }

        public ObservableCollection<ReplayEvent> ReplayEvents { get; set; }
            = new ObservableCollection<ReplayEvent>();

        public ObservableCollection<CheckModel> CheckDamageTypeCategory { get; set; } =
            new ObservableCollection<CheckModel>();

        public Control UpperContent
        {
            get => _upperContent;
            set
            {
                _upperContent = value;
                Notify();
            }
        }

        #endregion
    }
}