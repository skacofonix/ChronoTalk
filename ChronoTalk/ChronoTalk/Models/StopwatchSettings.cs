namespace ChronoTalk.Models
{
    public class StopwatchSettings : Settings
    {
        private const int DefaultStopwatchRefreshDelayMillisecond = 50;
        private const bool DefaultAllowStopwatchMeetingWithoutTalk = true;
        private const bool DefaultReStartLastTalkAfterReStartMeetingStopwatch = false;

        public int StopwatchRefreshDelayMillisecond
        {
            get
            {
                return GetOrDefault(DefaultStopwatchRefreshDelayMillisecond);
            }
            set
            {
                Set(value);
            }
        }

        public bool AllowStopwatchMeetingWithoutTalk
        {
            get
            {
                return GetOrDefault(DefaultAllowStopwatchMeetingWithoutTalk);
            }
            set
            {
                this.Set(value);
            }
        }

        public bool ReStartLastTalkAfterReStartMeetingStopwatch
        {
            get
            {
                return GetOrDefault(DefaultReStartLastTalkAfterReStartMeetingStopwatch);
            }
            set
            {
                Set(value);
            }
        }

        public override void Reset()
        {
            this.StopwatchRefreshDelayMillisecond = DefaultStopwatchRefreshDelayMillisecond;
            this.AllowStopwatchMeetingWithoutTalk = DefaultAllowStopwatchMeetingWithoutTalk;
            this.ReStartLastTalkAfterReStartMeetingStopwatch = DefaultReStartLastTalkAfterReStartMeetingStopwatch;
        }
    }
}