using Xamarin.Forms;

namespace ChronoTalk.Views
{
    public partial class SpeakerPage
    {
        public SpeakerPage()
        {
            this.InitializeComponent();
        }

        private void ListViewOnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listview = (ListView)sender;
            listview.SelectedItem = null;
        }
    }
}