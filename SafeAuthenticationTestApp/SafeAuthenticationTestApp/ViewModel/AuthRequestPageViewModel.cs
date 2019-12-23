using Rg.Plugins.Popup.Extensions;
using SafeAuthenticationTestApp.Model;
using SafeAuthenticationTestApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace SafeAuthenticationTestApp.ViewModel
{
    public class AuthRequestPageViewModel : BaseViewModel
    {
        private INavigation _navigation;
        public bool IsContainerRequest { get; }
        public bool IsAppContainerRequested { get; set; }

        public bool IsAppPerformMutationsRequested { get; set; }

        public bool IsAppGetBalanceRequested { get; set; }

        public bool IsAppTransferCoinsRequested { get; set; }

        public ICommand SetContainerPermission { get; private set; }
        public ICommand SendRequest { get; private set; }

        public ObservableCollection<ContainerPermissionsModel> Containers { get; set; }

        public AuthRequestPageViewModel(INavigation navigation, bool isContainerRequest)
        {
            _navigation = navigation;
            IsContainerRequest = isContainerRequest;
            Title = $"{ (isContainerRequest ? "Container" : "Authentication")} Request";
            AddDefaultContainers();
            InitialiseCommands();
        }

        private void InitialiseCommands()
        {
            SetContainerPermission = new Command<ContainerPermissionsModel>((container) =>
            {
                _navigation.PushPopupAsync(new PermissionPopUpPage(ref container, isCustomRequest: false));
            });

            SendRequest = new Command(async () =>
            {
                try
                {
                    var encodedRequest = string.Empty;
                    var containers = new List<ContainerPermissionsModel>(Containers);
                    if (IsContainerRequest)
                    {
                        encodedRequest = await RequestService.CreateContainerRequestAsync(containers);
                    }
                    else
                    {
                        encodedRequest = await RequestService.CreateAuthRequestAsync(
                            IsAppContainerRequested,
                            IsAppGetBalanceRequested,
                            IsAppTransferCoinsRequested,
                            IsAppPerformMutationsRequested,
                            containers);
                    }
                    RequestService.SendRequest(encodedRequest, isUnregistered: false);
                    await _navigation.PopToRootAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error : {ex.Message}");
                }
            });
        }

        private void AddDefaultContainers()
        {
            if (Containers == null)
            {
                Containers = new ObservableCollection<ContainerPermissionsModel>();
            }
        }
    }
}
