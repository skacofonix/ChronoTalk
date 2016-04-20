using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace ChronoTalk.Droid
{
    [Activity(Label = "ChronoTalk", MainLauncher = true, Icon= "@drawable/miniBlitzWhite", NoHistory = true, Theme = "@style/Theme.Splash", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}