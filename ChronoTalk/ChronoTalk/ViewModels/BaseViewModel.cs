using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace ChronoTalk.ViewModels
{
    public abstract class BaseViewModel : ViewModelBase
    {
        protected BaseViewModel(INavigation navigation = null)
        {
            Navigation = navigation;
        }

        public INavigation Navigation { get; set; }
    }
}