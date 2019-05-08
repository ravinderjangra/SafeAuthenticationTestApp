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
        private ContainerPermissionsModel _ContainerPermissionSet;
        private PermissionSetModel _permissionSet;
        private bool? _isCustomRequest;
        public PermissionPopUpPage(ref ContainerPermissionsModel containerPermissionSet, bool isCustomRequest)
        {
            InitializeComponent();
            _isCustomRequest = isCustomRequest;
            _ContainerPermissionSet = containerPermissionSet;
            _permissionSet = _ContainerPermissionSet.Access;
            CloseWhenBackgroundIsClicked = false;
            IsAnimationEnabled = true;
        }

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
                if (_isCustomRequest.HasValue && _isCustomRequest.Value == true)
                    _viewModel = new PermissionPopUpPageViewModel(ref _ContainerPermissionSet, _isCustomRequest.Value);
                else
                    _viewModel = new PermissionPopUpPageViewModel(ref _permissionSet);
            }

            BindingContext = _viewModel;
        }
    }
}