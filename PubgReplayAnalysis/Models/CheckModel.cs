using System;

namespace PubgReplayAnalysis.Models
{
    public class CheckModel
    {
        private bool _isChecked;

        public CheckModel(string checkName, bool isChecked, Action<string, bool> act)
        {
            CheckName = checkName;
            IsChecked = isChecked;
            Act = act;
        }

        public Action<string, bool> Act { get; set; }
        public string CheckName { get; set; }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                Act?.Invoke(CheckName, value);
            }
        }
    }
}