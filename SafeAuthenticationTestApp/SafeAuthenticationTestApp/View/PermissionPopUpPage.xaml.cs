using Rg.Plugins.Popup.Pages;
using SafeAuthenticationTestApp.Model;
using SafeAuthenticationTestApp.ViewModel;
using Xamarin.Forms.Xaml;

namespace SafeAuthenticationTestApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PermissionPopUpPage : PopupPage
    {
        private PermissionPopUpPageViewModel _viewModel;
        private PermissionSetModel _permissionSet;
        public PermissionPopUpPage(ref PermissionSetModel permissionSet)
        {
            InitializeComponent();
            _permissionSet = permissionSet;
            CloseWhenBackgroundIsClicked = false;
            IsAnimationEnabled = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel == null)
            {
                _viewModel = new PermissionPopUpPageViewModel(ref _permissionSet);
            }

            BindingContext = _viewModel;
        }
    }
}