using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeApp;
using SafeAuthenticationTestApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SafeAuthenticationTestApp
{
    public partial class App : Application
    {
        private static bool _isInitialized;

        public App()
        {
            InitializeComponent();
            if (!_isInitialized)
            {
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    var fileList = new List<(string, string)>
                    {
                        ("log.toml", "log.toml")
                    };
                    var fileOps = DependencyService.Get<IPlatformService>();
                    await fileOps.TransferAssetsAsync(fileList);
                    await Session.SetAppConfigurationDirectoryPathAsync(fileOps.ConfigFilesPath);
                    await Session.InitLoggingAsync($"{DateTime.Now.ToShortTimeString()}.log");
                });
                _isInitialized = true;
            }
            //System.Diagnostics.Debug.WriteLine("IsMock : " + SafeApp.Session.IsMockBuild());
            MainPage = new NavigationPage(new View.RequestPage());
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
