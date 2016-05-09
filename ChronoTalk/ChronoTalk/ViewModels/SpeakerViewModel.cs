using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ChronoTalk.Messages;
using ChronoTalk.Models;
using ChronoTalk.Tools;
using ChronoTalk.Views;
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
        private double speakTimeRatio;
        private Timer timer;
        private ICommand deleteCommand;

        public SpeakerViewModel()
        {
            Messenger.Default.Register<RefreshStopwatchRenderMessage>(this, OnReceiveRefreshStopwatchRenderMessage);
        }

        public SpeakerViewModel(MeetingViewModel meetingViewModel, Speaker speaker) : this()
        {
            this.MeetingViewModel = meetingViewModel;
            this.meeting = meetingViewModel.Meeting;
            this.speaker = speaker;

            this.meeting.TalkChanged += MeetingOnTalkChanged;
            this.meeting.MeetingStatusChanged += MeetingOnMeetingStatusChanged;

            timer = new Timer(state => RefreshSpeakTimeRatio(), null, 0, 1000);
        }

        public Speaker Speaker => this.speaker;

        public Meeting Meeting => this.meeting;

        public MeetingViewModel MeetingViewModel { get; }

        public string Name
        {
            get { return this.speaker.Name; }
            set
            {
                this.speaker.Name = value;
                RaisePropertyChanged();
            }
        }

        public ImageSource Image
        {
            get { return this.speaker.Image; }
            set
            {
                this.speaker.Image = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<TalkViewModel> Talks
        {
            get { return this.talks; }
            set
            {
                this.talks = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSpeaking
            =>
                this.meeting?.CurrentTalk?.Speaker == this.speaker &&
                this.meeting?.CurrentTalk?.State == SpeakerStatus.Speaking;

        public TimeSpan SpeakTime => this.meeting.TotalSpeakTime(this.speaker);

        public double SpeakTimeRatio
        {
            get { return speakTimeRatio; }
            set
            {
                speakTimeRatio = value;
                RaisePropertyChanged();
            }
        }

        public string SpeakTimeMilliseconds => this.SpeakTime.Milliseconds.ToString("000").Substring(0, 2);

        private void RefreshSpeakTimeRatio()
        {
            if (this.SpeakTime.TotalMilliseconds > 0.0)
                SpeakTimeRatio = this.meeting.ComputeRatioSpeakTime(this.speaker);
        }

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
            RaisePropertyChanged(() => this.IsSpeaking);

            if (talk?.Speaker == speaker)
            {
                var vm = new TalkViewModel(talk);
                this.Talks.Insert(0, vm);
            }
        }

        private void MeetingOnMeetingStatusChanged(object sender, MeetingStatus meetingStatus)
        {
            RaisePropertyChanged(() => this.IsSpeaking);
        }

        private void OnReceiveRefreshStopwatchRenderMessage(RefreshStopwatchRenderMessage message)
        {
            RaisePropertyChanged(() => this.IsSpeaking);

            if (this.IsSpeaking)
            {
                RaisePropertyChanged(() => SpeakTime);
                RaisePropertyChanged(() => SpeakTimeMilliseconds);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new Command<SpeakerViewModel>(async speakerViewModel => await ExecuteDeleteComand(speakerViewModel));
                }

                return deleteCommand;
            }
        }

        private async Task ExecuteDeleteComand(SpeakerViewModel speakerViewModel)
        {
            this.meeting.DeleteSpeaker(this.speaker);
        }
    }
}