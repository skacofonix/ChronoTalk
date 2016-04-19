using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace ChronoTalk.Droid
{
    [Activity(Label = "MainActivity", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //ToolbarResource = Resource.Layout.toolbar;
            //TabLayoutResource = Resource.Layout.tabs;
            Xamarin.Forms.Forms.Init(this, bundle);

            base.OnCreate(bundle);

            LoadApplication(new App());
        }
    }
}