using System;
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

        private async void ListViewOnItemTapped(object sender, ItemTappedEventArgs e)
        {
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