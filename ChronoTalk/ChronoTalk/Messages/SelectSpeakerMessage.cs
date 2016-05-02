using ChronoTalk.ViewModels;

namespace ChronoTalk.Messages
{
    public class SelectSpeakerMessage
    {
        public SelectSpeakerMessage(SpeakerViewModel speakerViewModel)
        {
            SpeakerViewModel = speakerViewModel;
        }

        public SpeakerViewModel SpeakerViewModel { get; private set; }
    }
}