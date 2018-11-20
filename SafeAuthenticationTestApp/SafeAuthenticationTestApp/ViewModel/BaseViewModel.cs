using SafeAuthenticationTestApp.Services;
using Xamarin.Forms;
using Constants = SafeAuthenticationTestApp.Helpers.AppConstants;

namespace SafeAuthenticationTestApp.ViewModel
{
    public class BaseViewModel : MvvmHelpers.BaseViewModel
    {
        public ISafeService RequestService { get; }
        public SafeAppService AppService { get; }
        public BaseViewModel()
        {
            RequestService = DependencyService.Get<ISafeService>();
            AppService = DependencyService.Get<SafeAppService>();
        }

        public string AppId { get; } = Constants.AppId;
        public string AppName { get; } = Constants.AppName;
        public string AppVendor { get; } = Constants.AppVendor;
    }
}
