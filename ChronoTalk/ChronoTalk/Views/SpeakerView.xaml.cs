using Xamarin.Forms;

namespace ChronoTalk.Views
{
    public partial class SpeakerView
    {
        public SpeakerView()
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