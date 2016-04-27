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

namespace ChronoTalk.ViewModels
{
    public class MeetingViewModel : BaseViewModel
    {
        private const int RefreshDelayMillisecond = 20;
        private const int BlinkDelayMillisecond = 400;

        private ObservableCollection<SpeakerViewModel> speakers = new ObservableCollection<SpeakerViewModel>();
        private RelayCommand addSpeakerCommand;
        private RelayCommand toggleMeetingStopwatch;
        private RelayCommand startStopwatchCommand;
        private RelayCommand stopStopwatchCommmand;
        private RelayCommand resetStopwatchCommand;
        private RelayCommand<SpeakerViewModel> showSpeakerCommand;

        private SpeakerViewModel currentSpeaker;
        private bool displayStopwatch = true;
        private StopwatchState stopwatchState;
        private ICommand showSettingsCommand;
        private Timer blinkStopWatchDisplayTimer;
        private Timer refreshStopwatchRenderTimer;


        public MeetingViewModel()
        {
            Messenger.Default.Register<ToggleSpeakerChangeMessage>(this, OnReceiveToogleSpeakerChangeMessage);

            this.Initialize();
        }
        
        private void Initialize(Meeting meeting = null)
        {
            this.StopwatchState = StopwatchState.Zero;

            if (this.Meeting != null)
            {
                this.Meeting.MeetingStatusChanged -= MeetingOnMeetingStatusChanged;
                this.Meeting.SpeakerAdded -= MeetingOnSpeakerAdded;
            }

            this.Meeting = meeting ?? new Meeting();
            this.Speakers.Clear();

            this.RaiseAllCanExecute();
            this.OnPropertyChanged("ElapsedTime");
            this.OnPropertyChanged("ElapsedMilliseconds");

            this.Meeting.MeetingStatusChanged += MeetingOnMeetingStatusChanged;
            this.Meeting.SpeakerAdded += MeetingOnSpeakerAdded;

            this.blinkStopWatchDisplayTimer?.Dispose();
            this.displayStopwatch = true;
        }

        public Meeting Meeting { get; private set; }

        public ObservableCollection<SpeakerViewModel> Speakers
        {
            get { return speakers; }
            set
            {
                speakers = value;
                this.OnPropertyChanged();
            }
        }

        public SpeakerViewModel CurrentSpeaker
        {
            get { return currentSpeaker; }
            set
            {
                currentSpeaker = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan ElapsedTime => this.Meeting.Duration;

        public string ElapsedMilliseconds => this.Meeting.Duration.Milliseconds.ToString("000").Substring(0, 2);

        public bool IsRunning => this.Meeting.State == MeetingStatus.IsRunning;

        public bool DisplayStopwatch
        {
            get { return displayStopwatch; }
            set
            {
                displayStopwatch = value;
                OnPropertyChanged();
            }
        }

        public StopwatchState StopwatchState
        {
            get { return stopwatchState; }
            set
            {
                stopwatchState = value;
                OnPropertyChanged();
                OnPropertyChanged("IsRunning");
            }
        }

        public ICommand StartStopwatchCommand
        {
            get
            {
                if (this.startStopwatchCommand == null)
                {
                    this.startStopwatchCommand = new RelayCommand(StartStopwatch, () => !this.IsRunning);
                }

                return this.startStopwatchCommand;
            }
        }

        public ICommand StopStopwatchCommmand
        {
            get
            {
                if (this.stopStopwatchCommmand == null)
                {
                    this.stopStopwatchCommmand = new RelayCommand(StopStopwatch, () => this.IsRunning);
                }

                return this.stopStopwatchCommmand;
            }
        }

        public ICommand ResetStopwatchCommand
        {
            get
            {
                if (this.resetStopwatchCommand == null)
                {
                    this.resetStopwatchCommand = new RelayCommand(() => Initialize(), () => this.StopwatchState == StopwatchState.Pause);
                }

                return resetStopwatchCommand;
            }
        }

        public ICommand ToggleStopwatchCommand
        {
            get
            {
                if (this.toggleMeetingStopwatch == null)
                {
                    this.toggleMeetingStopwatch = new RelayCommand(() => this.Meeting.ToggleMeeting());
                }

                return toggleMeetingStopwatch;
            }
        }

        #region AddSpeaker Command

        public ICommand AddSpeakerCommand
        {
            get
            {
                if (this.addSpeakerCommand == null)
                {
                    this.addSpeakerCommand = new RelayCommand(ExecuteAddSpeaker);
                }

                return addSpeakerCommand;
            }
        }

        private void ExecuteAddSpeaker()
        {
            var newSpeaker = new Speaker();
            this.Meeting.AddSpeaker(newSpeaker);
        }

        #endregion

        #region ShowSpeaker Command

        public ICommand ShowSpeakerCommand
        {
            get
            {
                if (this.showSpeakerCommand == null)
                {
                    this.showSpeakerCommand = new RelayCommand<SpeakerViewModel>(async (s) => await NavigateToSpeaker(s), (s) => true);
                }
                return showSpeakerCommand;
            }
        }

        private async Task NavigateToSpeaker(SpeakerViewModel speaker)
        {
            var speakerPage = new SpeakerPage {BindingContext = speaker};

            await this.Navigation.PushAsync(speakerPage);
        }

        #endregion

        #region ShowSettings Command

        public ICommand ShowSettingsCommand
        {
            get
            {
                if (showSettingsCommand == null)
                {
                    showSettingsCommand = new RelayCommand(async () => await NavigateToSetting());
                }
                return showSettingsCommand;
            }
        }

        private async Task NavigateToSetting()
        {
            var settingsPage = new ChonoTalkSettingsPage();

            try
            {
                await this.Navigation.PushAsync(settingsPage);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        private void MeetingOnSpeakerAdded(object sender, Speaker speaker)
        {
            var vm = new SpeakerViewModel(speaker, this.Meeting);
            this.Speakers.Add(vm);
        }

        private void MeetingOnMeetingStatusChanged(object sender, MeetingStatus status)
        {
            if (status == MeetingStatus.IsRunning)
            {
                this.StopwatchState = StopwatchState.Running;

                refreshStopwatchRenderTimer = new Timer(state => RefreshStopwatchRender(), null, 0, RefreshDelayMillisecond);
                blinkStopWatchDisplayTimer?.Dispose();
                DisplayStopwatch = true;

                Messenger.Default.Send(new ToggleMainStopwatchMessage(true, this.CurrentSpeaker));
            }
            else if (status == MeetingStatus.PauseOrEnded)
            {
                if (this.ElapsedTime == TimeSpan.Zero)
                {
                    this.StopwatchState = StopwatchState.Zero;
                }
                else
                {
                    this.StopwatchState = StopwatchState.Pause;
                }

                blinkStopWatchDisplayTimer = new Timer(state => BlinkStopwatchDisplay(), null, 0, BlinkDelayMillisecond);
                refreshStopwatchRenderTimer?.Dispose();

                Messenger.Default.Send(new ToggleMainStopwatchMessage(false, this.CurrentSpeaker));
            }

            this.RaiseAllCanExecute();
        }

        private void StopStopwatch()
        {
            this.Meeting.Stop();
        }

        private void StartStopwatch()
        {
            this.Meeting.Start();
        }

        private void RefreshStopwatchRender()
        {
            this.OnPropertyChanged("ElapsedTime");
            this.OnPropertyChanged("ElapsedDays");
            this.OnPropertyChanged("ElapsedHours");
            this.OnPropertyChanged("ElapsedMinutes");
            this.OnPropertyChanged("ElapsedSeconds");
            this.OnPropertyChanged("ElapsedMilliseconds");
            Messenger.Default.Send(new RefreshStopwatchRenderMessage());
        }

        private void BlinkStopwatchDisplay()
        {
            DisplayStopwatch = !DisplayStopwatch;
        }

        private void OnReceiveToogleSpeakerChangeMessage(ToggleSpeakerChangeMessage message)
        {
            this.CurrentSpeaker = message.SpeakerViewModel;

            if(!this.IsRunning)
                this.StartStopwatch();
        }

        private void RaiseAllCanExecute()
        {
            this.startStopwatchCommand?.RaiseCanExecuteChanged();
            this.stopStopwatchCommmand?.RaiseCanExecuteChanged();
            this.resetStopwatchCommand?.RaiseCanExecuteChanged();
        }
    }
}