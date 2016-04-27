using System.Threading.Tasks;
using ChronoTalk.ViewModels;
using Xamarin.Forms;

namespace ChronoTalk.Views
{
    public partial class MeetingPage
    {
        private MeetingViewModel vm;

        public MeetingPage()
        {
            this.InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            vm = this.BindingContext as MeetingViewModel;
            vm.Navigation = this.Navigation;
        }

        private async void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var speaker = e.Item as SpeakerViewModel;

            var listview = (ListView)sender;
            listview.SelectedItem = null;

            speaker.ToggleSpeakerCommand.Execute(speaker);
        }

        private async Task NavigateToSpeakerPage(SpeakerViewModel speaker)
        {
            var speakerPage = new SpeakerPage();
            speakerPage.BindingContext = speaker;

            await this.Navigation.PushAsync(speakerPage);
        }
    }
}