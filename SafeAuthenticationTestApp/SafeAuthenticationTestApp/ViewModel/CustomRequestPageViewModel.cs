using Rg.Plugins.Popup.Extensions;
using SafeAuthenticationTestApp.Model;
using SafeAuthenticationTestApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SafeAuthenticationTestApp.ViewModel
{
    public class CustomRequestPageViewModel : BaseViewModel
    {
        private string appId;

        public new string AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        private string appVendor;

        public new string AppVendor
        {
            get { return appVendor; }
            set { appVendor = value; }
        }

        private string appName;

        public new string AppName
        {
            get { return appName; }
            set { appName = value; }
        }

        private INavigation _navigation;
        public bool IsContainerRequest { get; set; }
        public bool IsAppContainerRequested { get; set; }
        public ICommand AddContainerPermissioncommand { get; private set; }
        public ICommand DeleteContainerPermissionCommand { get; private set; }
        public ICommand SendRequest { get; private set; }
        public ICommand AddContainerCommand { get; private set; }
        public ObservableCollection<ContainerPermissionsModel> Containers { get; set; }

        public CustomRequestPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            Title = $"Custom Request";
            InitialiseCommands();

            if (Containers == null)
            {
                Containers = new ObservableCollection<ContainerPermissionsModel>();
            }
        }

        private void InitialiseCommands()
        {
            AddContainerPermissioncommand = new Command<ContainerPermissionsModel>((container) =>
            {
                _navigation.PushPopupAsync(new PermissionPopUpPage(ref container, isCustomRequest: true));
            });

            DeleteContainerPermissionCommand = new Command<ContainerPermissionsModel>((container) =>
            {
                Containers.Remove(container);
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
                        encodedRequest = await RequestService.CreateAuthRequestAsync(IsAppContainerRequested, containers);
                    }
                    RequestService.SendRequest(encodedRequest, isUnregistered: false);
                    await _navigation.PopToRootAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error : {ex.Message}");
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                }
            });

            AddContainerCommand = new Command(() =>
            {
                var newContainer = new ContainerPermissionsModel(string.Empty);
                Containers.Add(newContainer);
                _navigation.PushPopupAsync(new PermissionPopUpPage(ref newContainer, isCustomRequest: true));
            });
        }
    }
}
