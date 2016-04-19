using System.Threading.Tasks;
using ChronoTalk.ViewModels;
using Xamarin.Forms;

namespace ChronoTalk.Views
{
    public partial class MeetingPage
    {
        public MeetingPage()
        {
            this.InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            var vm = this.BindingContext as BaseViewModel;
            vm.Navigation = this.Navigation;
        }

        private async void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var speaker = e.Item as SpeakerViewModel;

            if (speaker != null)
            {
                await NavigateToSpeakerPage(speaker);
            }

            var listview = (ListView)sender;
            listview.SelectedItem = null;
        }

        private async Task NavigateToSpeakerPage(SpeakerViewModel speaker)
        {
            var speakerPage = new SpeakerPage();
            speakerPage.BindingContext = speaker;

            await this.Navigation.PushAsync(speakerPage);
        }
    }
}