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

        private void ListViewOnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var listview = (ListView)sender;
            listview.SelectedItem = null;
        }
    }
}