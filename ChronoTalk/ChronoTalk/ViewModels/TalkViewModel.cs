using System;
using ChronoTalk.Messages;
using ChronoTalk.Models;
using GalaSoft.MvvmLight.Messaging;

namespace ChronoTalk.ViewModels
{
    public class TalkViewModel : BaseViewModel
    {
        private Talk talk;

        private TalkViewModel()
        {
            Messenger.Default.Register<RefreshStopwatchRenderMessage>(this, OnReceiveRefreshStopwatchRenderMessage);
        }

        public TalkViewModel(Talk talk) : this()
        {
            this.talk = talk;
        }

        public TimeSpan Duration => this.talk.Duration;

        private void OnReceiveRefreshStopwatchRenderMessage(RefreshStopwatchRenderMessage message)
        {
            if(this.talk.State == SpeakerStatus.Speaking)
                OnPropertyChanged("Duration");
        }
    }
}