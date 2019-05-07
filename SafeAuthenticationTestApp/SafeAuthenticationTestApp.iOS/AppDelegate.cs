using System;
using System.Diagnostics;
using Foundation;
using SafeAuthenticationTestApp.Services;
using UIKit;
using Xamarin.Forms;

namespace SafeAuthenticationTestApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private SafeRequestService RequestService => DependencyService.Get<SafeRequestService>();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            Rg.Plugins.Popup.Popup.Init();
            Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            FormsMaterial.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            Device.BeginInvokeOnMainThread(
                async () =>
                {
                    try
                    {
                        Debug.WriteLine($"IPC Response: {url.ToString()}");
                        await RequestService.ProcessResponseAsync(url.ToString());
                        Debug.WriteLine("IPC Msg Handling Completed");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error: {ex.Message}");
                    }
                });
            return true;
        }
    }
}
