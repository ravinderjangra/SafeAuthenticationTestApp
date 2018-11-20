using Rg.Plugins.Popup.Extensions;
using SafeAuthenticationTestApp.Model;
using SafeAuthenticationTestApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SafeAuthenticationTestApp.ViewModel
{
    public class ShareMDataRequestPageViewModel : BaseViewModel
    {
        private INavigation _navigation;
        public ICommand AddNewMDataCommand { get; private set; }
        public ICommand SendRequestCommand { get; private set; }
        public ICommand SetMDataPermission { get; private set; }
        public ObservableCollection<ShareMDataModel> ShareMDataList { get; set; }

        public ShareMDataRequestPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            ShareMDataList = new ObservableCollection<ShareMDataModel>();
            InitialiseCommands();
        }

        private void InitialiseCommands()
        {
            AddNewMDataCommand = new Command(async () => await OnAddNewDataCommandAsync());
            SendRequestCommand = new Command(async () => await OnSendRequestCommand());
            SetMDataPermission = new Command<PermissionSetModel>((permission) =>
            {
                _navigation.PushPopupAsync(new PermissionPopUpPage(ref permission));
            });
        }

        private async Task OnAddNewDataCommandAsync()
        {
            try
            {
                var shareMDataInfo = await AppService.AddRandomPrivateMDataAsync();
                ShareMDataList.Add(shareMDataInfo);
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
            }
        }

        private async Task OnSendRequestCommand()
        {
            try
            {
                var shareMDataList = new List<ShareMDataModel>(ShareMDataList);
                var shareMDataRequest = await RequestService.CreateMDataShareRequestAsync(shareMDataList);
                RequestService.SendRequest(shareMDataRequest, false);
                await _navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
            }
        }
    }
}
