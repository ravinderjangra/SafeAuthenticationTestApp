using SafeAuthenticationTestApp.Services;
using SafeAuthenticationTestApp.View;
using System.Windows.Input;
using Xamarin.Forms;

namespace SafeAuthenticationTestApp.ViewModel
{
    public class RequestPageViewModel : BaseViewModel
    {
        private INavigation _navigation;
        private bool _isSessionAvailable;
        public ICommand NavigateAuthPage { get; private set; }
        public ICommand NavigateContainerPage { get; private set; }
        public ICommand NavigateCustomRequestPage { get; private set; }
        public ICommand ResetSessionCommand { get; private set; }
        public ICommand UnregisteredRequest { get; private set; }

        public bool IsSessionAvailable
        {
            get { return _isSessionAvailable; }
            set
            {
                _isSessionAvailable = value;
                OnPropertyChanged();
            }
        }

        public RequestPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            SetNaviationCommands();

            MessagingCenter.Subscribe<SafeRequestService>(this, "UpdateUI", (sender) =>
            {
                IsSessionAvailable = AppService.IsSessionAvailable;
            });
        }

        ~RequestPageViewModel()
        {
            MessagingCenter.Unsubscribe<SafeRequestService>(this, "UpdateUI");
        }

        private void SetNaviationCommands()
        {
            NavigateAuthPage = new Command(() =>
            {
                _navigation.PushAsync(new AuthRequestPage());
            });

            NavigateContainerPage = new Command(() =>
            {
                _navigation.PushAsync(new AuthRequestPage(true));
            });

            NavigateCustomRequestPage = new Command(() =>
            {
                _navigation.PushAsync(new CustomRequestPage());
            });

            UnregisteredRequest = new Command(async () =>
            {
                var encodedRequest = await RequestService.CreateUnregisteredRequestAsync();
                RequestService.SendRequest(encodedRequest, true);
            });

            ResetSessionCommand = new Command(() =>
            {
                AppService.ResetSession();
                IsSessionAvailable = false;
            });
        }
    }
}
