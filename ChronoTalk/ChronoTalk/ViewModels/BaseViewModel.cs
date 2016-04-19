using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ChronoTalk.Properties;
using Xamarin.Forms;

namespace ChronoTalk.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        protected BaseViewModel(INavigation navigation = null)
        {
            Navigation = navigation;
        }

        public INavigation Navigation { get; set; }

        public async Task PushModalAsync(Page page)
        {
            if (Navigation != null)
            {
                await Navigation.PushModalAsync(page);
            }
        }

        public async Task PopModalAsync()
        {
            if (Navigation != null)
            {
                await Navigation.PopModalAsync();
            }
        }

        public async Task PushAsync(Page page)
        {
            if (Navigation != null)
            {
                await Navigation.PushAsync(page);
            }
        }

        public async Task PopAsync()
        {
            if (Navigation != null)
            {
                await Navigation.PopAsync();
            }
        }

        protected void SetProperty<TU>(
            ref TU backingStore, TU value,
            string propertyName,
            Action onChanged = null,
            Action<TU> onChanging = null)
        {
            if (EqualityComparer<TU>.Default.Equals(backingStore, value))
                return;

            onChanging?.Invoke(value);

            OnPropertyChanging(propertyName);

            backingStore = value;

            onChanged?.Invoke();

            if (propertyName != null) OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}