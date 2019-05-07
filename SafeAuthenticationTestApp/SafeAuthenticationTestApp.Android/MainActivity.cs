using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SafeAuthenticationTestApp.Helpers;
using Xamarin.Forms;
using SafeAuthenticationTestApp.Services;

namespace SafeAuthenticationTestApp.Droid
{
    [Activity(Label = "SafeAuthenticationTestApp", 
        Icon = "@mipmap/icon", 
        Theme = "@style/MainTheme", 
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleTask,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation),
        IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = AppConstants.AppId)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private SafeRequestService RequestService => DependencyService.Get<SafeRequestService>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);

            LoadApplication(new App());
            
            if (Intent?.Data != null)
            {
                HandleAppLaunch(Intent.Data.ToString());
            }
        }

        private void HandleAppLaunch(string uri)
        {
            System.Diagnostics.Debug.WriteLine($"Launched via: {uri}");
            Device.BeginInvokeOnMainThread(
              async () => {
                  try
                  {
                      await RequestService.ProcessResponseAsync(uri);
                      System.Diagnostics.Debug.WriteLine("IPC Msg Handling Completed");
                  }
                  catch (Exception ex)
                  {
                      System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                  }
              });
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            if (intent?.Data != null)
            {
                HandleAppLaunch(intent.Data.ToString());
            }
        }
    }
}