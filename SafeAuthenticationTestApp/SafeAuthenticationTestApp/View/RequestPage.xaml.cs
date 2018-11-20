using SafeAuthenticationTestApp.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeAuthenticationTestApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestPage : ContentPage
    {
        private RequestPageViewModel _viewModel;
        public RequestPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel == null)
            {
                _viewModel = new RequestPageViewModel(Navigation);
            }

            BindingContext = _viewModel;
        }
    }
}