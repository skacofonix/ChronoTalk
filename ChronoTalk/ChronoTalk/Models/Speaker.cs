using Xamarin.Forms;

namespace ChronoTalk.Models
{
    public class Speaker
    {
        private static int SpeakerCounter;

        public Speaker()
        {
            this.Name = "Speaker #" + ++SpeakerCounter;
            this.Image = ImageSource.FromFile("avatar.png");
        }

        public string Id { get; private set; }
        public string Name { get; set; }
        public ImageSource Image { get; set; }
    }
}