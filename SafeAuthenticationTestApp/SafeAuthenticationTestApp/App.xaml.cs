using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SafeAuthenticationTestApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
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
