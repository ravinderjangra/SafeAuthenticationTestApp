using MvvmHelpers;
using Rg.Plugins.Popup.Extensions;
using SafeAuthenticationTestApp.Model;
using System.Windows.Input;
using Xamarin.Forms;

namespace SafeAuthenticationTestApp.ViewModel
{
    public class PermissionPopUpPageViewModel : ObservableObject
    {
        public PermissionSetModel PermissionSet { get; private set; }

        public ICommand BackCommand { get;}

        public PermissionPopUpPageViewModel(ref PermissionSetModel permissionSet)
        {
            PermissionSet = permissionSet;

            BackCommand = new Command(() =>
            {
                App.Current.MainPage.Navigation.PopPopupAsync();
            });
        }
    }
}
