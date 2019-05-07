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
        public ContainerPermissionsModel ContainerPermissionSet { get; private set; }


        public string ContainerName
        {
            get { return ContainerPermissionSet?.ContName; }
            set
            {
                if (ContainerPermissionSet?.ContName != value)
                    ContainerPermissionSet.ContName = value;
            }
        }

        public bool IsCustomRequest { get; private set; }

        public ICommand BackCommand { get; }

        public PermissionPopUpPageViewModel(ref ContainerPermissionsModel permissionSet, bool isCustomRequest)
        {
            IsCustomRequest = isCustomRequest;
            ContainerPermissionSet = permissionSet;
            PermissionSet = permissionSet.Access;

            BackCommand = new Command(() =>
            {
                App.Current.MainPage.Navigation.PopPopupAsync();
            });
        }

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
