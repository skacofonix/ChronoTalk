using ChronoTalk.ViewModels;

namespace ChronoTalk.Messages
{
    public class ToggleSpeakerChangeMessage
    {
        public ToggleSpeakerChangeMessage(SpeakerViewModel speakerViewModel)
        {
            this.SpeakerViewModel = speakerViewModel;
        }

        public SpeakerViewModel SpeakerViewModel { get; private set; }
    }
}