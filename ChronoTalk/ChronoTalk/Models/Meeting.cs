using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ChronoTalk.Models
{
    public class Meeting
    {
        private readonly object locker = new object();
        private MeetingStatus state = MeetingStatus.PauseOrEnded;
        private List<Speaker> speakers = new List<Speaker>();
        private List<Talk> talks = new List<Talk>();
        private Talk currentTalk;
        private Stopwatch stopwatch = new Stopwatch();

        public event EventHandler<Speaker> SpeakerAdded;
        public event EventHandler<MeetingStatus> MeetingStatusChanged;
        public event EventHandler<Talk> TalkChanged;

        public List<Speaker> Speakers
        {
            get { return speakers; }
            private set { speakers = value; }
        }

        public List<Talk> Talks
        {
            get { return talks; }
            private set { talks = value; }
        }

        public MeetingStatus State
        {
            get { return this.state; }
            private set
            {
                this.state = value;
                this.OnMeetingStatusChanged(this.state);
            }
        }
        public DateTime? StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }
        public TimeSpan Duration => this.stopwatch.Elapsed;
        public Talk CurrentTalk
        {
            get { return this.currentTalk; }
            private set
            {
                this.currentTalk = value;
                this.OnTalkChanged(this.currentTalk);
            }
        }
        public Speaker CurrenSpeaker => this.CurrentTalk?.Speaker;

        public void AddSpeaker(Speaker speaker)
        {
            this.Speakers.Add(speaker);
            this.OnSpeakerAdded(speaker);
        }

        public void ToggleSpeaker(Speaker speaker)
        {
            lock (this.locker)
            {
                if (this.State == MeetingStatus.PauseOrEnded)
                {
                    // Start or restart meeting
                    this.StartMeeting();
                }

                if (this.CurrentTalk == null)
                {
                    // First talk
                    this.StartNewTalk(speaker);
                }
                else if (this.CurrentTalk.Speaker == speaker)
                {
                    // Same speaker
                    if (this.CurrentTalk.State == SpeakerStatus.Speaking)
                    {
                        // Break of pause talk
                        this.CurrentTalk.Stop();
                    }
                    else
                    {
                        // Restart talk
                        this.StartNewTalk(speaker);
                    }
                }
                else
                {
                    // Change speaker
                    this.StartNewTalk(speaker);
                }
            }
        }

        public void Start()
        {
            lock (this.locker)
            {
                this.StartMeeting();
            }
        }

        public void Stop()
        {
            lock (this.locker)
            {
                this.StopMeeting();
            }
        }

        public void ToggleMeeting()
        {
            lock (this.locker)
            {
                if (this.State == MeetingStatus.PauseOrEnded)
                {
                    this.StartMeeting();
                }
                else
                {
                    this.StopMeeting();
                }
            }
        }

        public TimeSpan TotalSpeakTime()
        {
            var ticks = this.Talks.Sum(x => x.Duration.Ticks);
            return TimeSpan.FromTicks(ticks);
        }

        public TimeSpan TotalSpeakTime(Speaker speaker)
        {
            var ticks = this.Talks.Where(x => x.Speaker == speaker)
                                  .Sum(x => x.Duration.Ticks);
            return TimeSpan.FromTicks(ticks);
        }

        public double ComputeRatioSpeakTime(Speaker speaker)
        {
            var speakerSpeakTime = this.TotalSpeakTime(speaker).TotalMilliseconds;
            var meetingSpeakTime = this.TotalSpeakTime().TotalMilliseconds;

            return speakerSpeakTime / meetingSpeakTime;
        }

        private void StartMeeting()
        {
            stopwatch.Start();

            if (!this.StartTime.HasValue)
                this.StartTime = DateTime.Now;

            this.EndTime = null;
            this.State = MeetingStatus.IsRunning;
        }

        private void StopMeeting()
        {
            this.stopwatch.Stop();
            this.CurrentTalk?.Stop();
            this.EndTime = DateTime.Now;
            this.State = MeetingStatus.PauseOrEnded;
        }

        private void StartNewTalk(Speaker speaker)
        {
            this.CurrentTalk?.Stop();

            var newTalk = Talk.StartNew(speaker);
            this.CurrentTalk = newTalk;
            this.Talks.Add(newTalk);
        }
        
        protected virtual void OnMeetingStatusChanged(MeetingStatus e)
        {
            MeetingStatusChanged?.Invoke(this, e);
        }

        protected virtual void OnTalkChanged(Talk e)
        {
            TalkChanged?.Invoke(this, e);
        }

        protected virtual void OnSpeakerAdded(Speaker e)
        {
            SpeakerAdded?.Invoke(this, e);
        }
    }
}