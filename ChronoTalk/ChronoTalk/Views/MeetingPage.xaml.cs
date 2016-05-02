using ChronoTalk.ViewModels;
using Xamarin.Forms;

namespace ChronoTalk.Views
{
    public partial class MeetingPage
    {
        private INavigation navigation;
        private MeetingViewModel vm;

        public MeetingPage()
        {
            this.InitializeComponent();
            navigation = this.Navigation;
        }

        protected override void OnBindingContextChanged()
        {
            vm = this.BindingContext as MeetingViewModel;
            vm.Navigation = navigation;
        }

        private void ListViewOnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var listview = (ListView)sender;
            listview.SelectedItem = null;
        }
    }
}