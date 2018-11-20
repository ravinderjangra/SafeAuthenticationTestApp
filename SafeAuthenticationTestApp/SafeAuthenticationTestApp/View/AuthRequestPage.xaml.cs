using SafeAuthenticationTestApp.ViewModel;
using System.Runtime.InteropServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeAuthenticationTestApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthRequestPage : ContentPage
    {
        public bool IsContainerRequest { get; private set; } = false;

        private AuthRequestPageViewModel _viewModel;

        public AuthRequestPage([Optional]bool isConainerRequest)
        {
            InitializeComponent();
            IsContainerRequest = isConainerRequest;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel == null)
            {
                _viewModel = new AuthRequestPageViewModel(Navigation, IsContainerRequest);
            }

            BindingContext = _viewModel;
        }

        private void UnSelect_ListView(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            ((ListView)sender).SelectedItem = null;
        }
    }
}