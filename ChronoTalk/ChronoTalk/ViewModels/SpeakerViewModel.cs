using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ChronoTalk.Messages;
using ChronoTalk.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Xamarin.Forms;

namespace ChronoTalk.ViewModels
{
    public class SpeakerViewModel : BaseViewModel
    {
        private readonly Speaker speaker;
        private readonly Meeting meeting;
        private RelayCommand toggleSpeaker;
        private ObservableCollection<TalkViewModel> talks = new ObservableCollection<TalkViewModel>(); 

        private SpeakerViewModel()
        {
            Messenger.Default.Register<RefreshStopwatchRenderMessage>(this, OnReceiveRefreshStopwatchRenderMessage);
        }

        public SpeakerViewModel(Speaker speaker, Meeting meeting) : this()
        {
            this.speaker = speaker;
            this.meeting = meeting;

            this.meeting.TalkChanged += MeetingOnTalkChanged;
            this.meeting.MeetingStatusChanged += MeetingOnMeetingStatusChanged;
        }

        public string Name
        {
            get { return this.speaker.Name; }
            set
            {
                this.speaker.Name = value;
                OnPropertyChanged();
            }
        }

        public ImageSource Image
        {
            get { return this.speaker.Image; }
            set
            {
                this.speaker.Image = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TalkViewModel> Talks
        {
            get { return this.talks; }
            set
            {
                this.talks = value;
                OnPropertyChanged();
            }
        }

        public bool IsSpeaking
            =>
                this.meeting?.CurrentTalk?.Speaker == this.speaker &&
                this.meeting?.CurrentTalk?.State == SpeakerStatus.Speaking;

        public TimeSpan SpeakTime => this.meeting.TotalSpeakTime(this.speaker);

        public string SpeakTimeMilliseconds => this.SpeakTime.Milliseconds.ToString("000").Substring(0, 2);

        public ICommand ToggleSpeakerCommand
        {
            get
            {
                if (this.toggleSpeaker == null)
                {
                    this.toggleSpeaker = new RelayCommand(() => this.meeting.ToggleSpeaker(this.speaker));
                }

                return toggleSpeaker;
            }
        }

        private void MeetingOnTalkChanged(object sender, Talk talk)
        {
            OnPropertyChanged("IsSpeaking");

            if (talk?.Speaker == speaker)
            {
                var vm = new TalkViewModel(talk);
                this.Talks.Insert(0, vm);
            }
        }

        private void MeetingOnMeetingStatusChanged(object sender, MeetingStatus meetingStatus)
        {
            OnPropertyChanged("IsSpeaking");
        }

        private void OnReceiveRefreshStopwatchRenderMessage(RefreshStopwatchRenderMessage message)
        {
            OnPropertyChanged("IsSpeaking");

            if (this.IsSpeaking)
            {
                this.OnPropertyChanged("SpeakTime");
                this.OnPropertyChanged("SpeakTimeMilliseconds");
            }
        }
    }
}