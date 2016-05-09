using ChronoTalk.Views;
using Xamarin.Forms;

namespace ChronoTalk
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            MainPage = new NavigationPage(new MeetingView());
            GalaSoft.MvvmLight.Ioc.SimpleIoc.Default.Register<INavigation>(() => MainPage.Navigation);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
