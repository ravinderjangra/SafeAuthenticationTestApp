using MvvmHelpers;
using SafeApp;
using SafeAuthenticationTestApp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(SafeAppService))]
namespace SafeAuthenticationTestApp.Services
{
    public class SafeAppService : ObservableObject
    {
        private Session _session;

        private bool _isUnregistered;

        private bool _isSessionAvailable;
        public bool IsSessionAvailable
        {
            get { return _isSessionAvailable; }
            set
            {
                _isSessionAvailable = value;
                OnPropertyChanged();
            }
        }
        public void InitialiseSession(Session session, bool isUnregistered)
        {
            _session = session;
            _isUnregistered = isUnregistered;
            IsSessionAvailable = true;
        }

        public void ResetSession()
        {
            _session?.Dispose();
            _session = null;
            IsSessionAvailable = false;
        }
    }
}
