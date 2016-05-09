using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ChronoTalk.Properties;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace ChronoTalk.ViewModels
{
    public abstract class BaseViewModel : ViewModelBase
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        protected BaseViewModel(INavigation navigation = null)
        {
            Navigation = navigation;
        }

        public INavigation Navigation { get; set; }

        //[NotifyPropertyChangedInvocator]
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}