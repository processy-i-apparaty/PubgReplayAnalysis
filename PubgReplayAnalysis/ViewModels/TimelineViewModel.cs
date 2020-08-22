using System;
using System.Collections.ObjectModel;
using Analysis;
using PubgReplayAnalysis.Infrastructure;
using PubgReplayAnalysis.Models;

namespace PubgReplayAnalysis.ViewModels
{
    internal class TimelineViewModel : Notifier
    {
        private PubgMatch _pubgMatch;


        public TimelineViewModel()
        {
            foreach (var dtc in Enum.GetNames(typeof(Enums.DamageTypeCategory)))
                CheckDamageTypeCategory.Add(new CheckModel(dtc, true, ActionCheckModel));
        }

        public void Initialize(PubgMatch pubgMatch)
        {
            _pubgMatch = pubgMatch;
            DataHelper.DisplayTimeLine(pubgMatch, ReplayEvents, CheckDamageTypeCategory);
        }

        private void ActionCheckModel(string name, bool isChecked)
        {
            if (_pubgMatch != null)
                DataHelper.DisplayTimeLine(_pubgMatch, ReplayEvents, CheckDamageTypeCategory);
        }

        #region bindings

        public ObservableCollection<ReplayEvent> ReplayEvents { get; set; }
            = new ObservableCollection<ReplayEvent>();


        public ObservableCollection<CheckModel> CheckDamageTypeCategory { get; set; } =
            new ObservableCollection<CheckModel>();

        #endregion
    }
}