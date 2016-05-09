using ChronoTalk.ViewModels;
using Xamarin.Forms;

namespace ChronoTalk.Views
{
    public partial class MeetingView
    {
        private INavigation navigation;
        private MeetingViewModel vm;

        public MeetingView()
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