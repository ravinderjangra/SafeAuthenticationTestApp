
using SafeAuthenticationTestApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeAuthenticationTestApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShareMDataRequestPage : ContentPage
    {
        ShareMDataRequestPageViewModel _viewModel;
        public ShareMDataRequestPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel == null)
                _viewModel = new ShareMDataRequestPageViewModel(Navigation);

            BindingContext = _viewModel;
        }

        private void UnSelect_ListView(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            ((ListView)sender).SelectedItem = null;
        }
    }
}