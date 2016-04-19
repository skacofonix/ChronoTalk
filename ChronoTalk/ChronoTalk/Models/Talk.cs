using System;

namespace ChronoTalk.Models
{
    public class Talk
    {
        private readonly object locker = new object();

        private Talk(Speaker speaker)
        {
            this.Speaker = speaker;
        }

        public Speaker Speaker { get; private set; }
        public DateTime? StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }
        public TimeSpan Duration => this.ComputeDuration();
        public SpeakerStatus State { get; private set; }

        public static Talk StartNew(Speaker speaker)
        {
            var talk = new Talk(speaker);

            talk.Start();

            return talk;
        }

        private void Start()
        {
            lock (locker)
            {
                this.StartTalk();
            }
        }

        public void Stop()
        {
            lock (locker)
            {
                this.StopTalk();
            }
        }

        private void StartTalk()
        {
            this.StartTime = DateTime.Now;
            this.State = SpeakerStatus.Speaking;
        }

        private void StopTalk()
        {
            if (this.State == SpeakerStatus.Ended)
                return;

            this.EndTime = DateTime.Now;
            this.State = SpeakerStatus.Ended;
        }

        private TimeSpan ComputeDuration()
        {
            if(!this.StartTime.HasValue)
                return TimeSpan.Zero;

            return this.EndTime.GetValueOrDefault(DateTime.Now).Subtract(this.StartTime.Value);
        }
    }
}