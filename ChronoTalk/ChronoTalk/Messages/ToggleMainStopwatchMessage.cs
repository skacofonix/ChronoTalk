using ChronoTalk.ViewModels;

namespace ChronoTalk.Messages
{
    public class ToggleMainStopwatchMessage
    {
        public ToggleMainStopwatchMessage(bool isRunning, SpeakerViewModel speaker)
        {
            this.IsRunning = isRunning;
        }

        public bool IsRunning { get; private set; }

        public SpeakerViewModel Speaker { get; private set; }
    }
}