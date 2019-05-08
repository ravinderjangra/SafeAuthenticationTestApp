using Rg.Plugins.Popup.Extensions;
using SafeApp.Utilities;
using SafeAuthenticationTestApp.Model;
using SafeAuthenticationTestApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using AConstants = SafeAuthenticationTestApp.Helpers.AppConstants;

namespace SafeAuthenticationTestApp.ViewModel
{
    public class CustomRequestPageViewModel : BaseViewModel
    {
        private string appId;

        public new string AppId
        {
            get { return appId; }
            set { appId = value; OnPropertyChanged(); }
        }

        private string appVendor;

        public new string AppVendor
        {
            get { return appVendor; }
            set { appVendor = value; OnPropertyChanged(); }
        }

        private string appName;

        public new string AppName
        {
            get { return appName; }
            set { appName = value; OnPropertyChanged(); }
        }

        private bool _useStaticAppInfo;

        public bool UseStaticAppInfo
        {
            get { return _useStaticAppInfo; }
            set
            {
                if (value)
                {
                    AppName = AConstants.AppName;
                    AppId = AConstants.AppId;
                    AppVendor = AConstants.AppVendor;
                }
                _useStaticAppInfo = value;
                OnPropertyChanged();
            }
        }

        private INavigation _navigation;
        public bool IsContainerRequest { get; set; }
        public bool IsAppContainerRequested { get; set; }
        public ICommand AddContainerPermissioncommand { get; private set; }
        public ICommand DeleteContainerPermissionCommand { get; private set; }
        public ICommand SendRequestcommand { get; private set; }
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

            SendRequestcommand = new Command(async () => await SendRequest());

            AddContainerCommand = new Command(() =>
            {
                var newContainer = new ContainerPermissionsModel(string.Empty);
                Containers.Add(newContainer);
                _navigation.PushPopupAsync(new PermissionPopUpPage(ref newContainer, isCustomRequest: true));
            });
        }

        private async Task SendRequest()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AppName) || string.IsNullOrWhiteSpace(AppVendor) || string.IsNullOrWhiteSpace(AppId))
                    throw new Exception("Please enter app info");

                var encodedRequest = string.Empty;
                var containers = new List<ContainerPermissionsModel>(Containers);
                var appExchangeInfo = new AppExchangeInfo { Id = AppId, Name = AppName, Vendor = AppVendor, Scope = string.Empty };

                if (IsContainerRequest)
                {
                    if (containers.Count == 0)
                        throw new Exception("Add one or more containers");

                    encodedRequest = await RequestService.CreateContainerRequestAsync(containers, appExchangeInfo);
                }
                else
                {
                    encodedRequest = await RequestService.CreateAuthRequestAsync(IsAppContainerRequested, containers, appExchangeInfo);
                }

                RequestService.SendRequest(encodedRequest, isUnregistered: false);
                await _navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error : {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
