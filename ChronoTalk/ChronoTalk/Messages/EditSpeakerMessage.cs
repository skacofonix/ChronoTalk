using ChronoTalk.ViewModels;

namespace ChronoTalk.Messages
{
    internal class EditSpeakerMessage
    {
        public EditSpeakerMessage(SpeakerViewModel speakerViewModel)
        {
            SpeakerViewModel = speakerViewModel;
        }

        public SpeakerViewModel SpeakerViewModel { get; private set; }
    }
}