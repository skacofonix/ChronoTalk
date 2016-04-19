using System.Windows.Input;
using ChronoTalk.Messages;
using ChronoTalk.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace ChronoTalk.ViewModels
{
    public class ChronoTalkSettingsViewModel : BaseViewModel
    {
        private readonly StopwatchSettings settings = new StopwatchSettings();
        private RelayCommand resetCommand;

        public int RefreshDelay
        {
            get { return settings.StopwatchRefreshDelayMillisecond; }
            set
            {
                settings.StopwatchRefreshDelayMillisecond = value;
                OnPropertyChanged();
                NotifySettingChanged();
            }
        }

        public bool AllowStopwatchMeetingWithoutTalk
        {
            get { return settings.AllowStopwatchMeetingWithoutTalk; }
            set
            {
                settings.AllowStopwatchMeetingWithoutTalk = value;
                OnPropertyChanged();
                NotifySettingChanged();
            }
        }

        public bool ReStartLastTalkAfterReStartMeetingStopwatch
        {
            get { return settings.ReStartLastTalkAfterReStartMeetingStopwatch; }
            set
            {
                settings.ReStartLastTalkAfterReStartMeetingStopwatch = value;
                OnPropertyChanged();
                NotifySettingChanged();
            }
        }

        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new RelayCommand(() => this.settings.Reset());
                }

                return resetCommand;
            }
        }

        private void NotifySettingChanged()
        {
            Messenger.Default.Send(new ChronoTalkSettingMessage());
        }
    }
}