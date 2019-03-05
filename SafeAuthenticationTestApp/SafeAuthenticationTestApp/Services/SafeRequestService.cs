using SafeAuthenticationTestApp.Services;
using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using SafeAuthenticationTestApp.Helpers;
using System.Diagnostics;
using SafeApp;
using SafeApp.Utilities;
using Constants = SafeAuthenticationTestApp.Helpers.AppConstants;
using SafeAuthenticationTestApp.Model;
using System.Collections.Generic;

[assembly: Dependency(typeof(SafeRequestService))]
namespace SafeAuthenticationTestApp.Services
{
    public class SafeRequestService : ISafeService
    {
        public async Task<string> CreateAuthRequestAsync(bool isAppContainerRequested, List<ContainerPermissionsModel> containers)
        {
            var (reqId, encodedReq) = await RequestHelper.GenerateEncodedAuthRequest(isAppContainerRequested, containers);
            return encodedReq;
        }

        public async Task<string> CreateContainerRequestAsync(List<ContainerPermissionsModel> containers)
        {
            var (reqId, encodedReq) = await RequestHelper.GenerateEncodedContainerRequest(containers);
            return encodedReq;
        }

        public async Task<string> CreateMDataShareRequestAsync(List<ShareMDataModel> shareMDataList)
        {
            var (reqId, encodedReq) = await RequestHelper.GenerateEncodedShareMDataRequest(shareMDataList);
            return encodedReq;
        }

        public async Task<string> CreateUnregisteredRequestAsync()
        {
            var (reqId, encodedReq) = await RequestHelper.GenerateEncodedUnregisteredRequest();
            return encodedReq;
        }

        public void SendRequest(string encodedRequest, bool isUnregistered)
        {
            var formattedReqUrl = UrlFormat.Format(Constants.AppId, encodedRequest, true);
            Debug.WriteLine($"Encoded Req : {formattedReqUrl}");

            Device.BeginInvokeOnMainThread(() => { Device.OpenUri(new Uri(formattedReqUrl)); });
        }

        public async Task ProcessResponseAsync(string encodedResponse)
        {
            try
            {
                var encodedRequest = UrlFormat.GetRequestData(encodedResponse);
                var decodeResult = await Session.DecodeIpcMessageAsync(encodedRequest);
                var decodeResultType = decodeResult.GetType();
                if (decodeResultType == typeof(AuthIpcMsg))
                {
                    Debug.WriteLine("Received Auth Granted from Authenticator");
                    // Update auth progress message
                    var ipcMsg = decodeResult as AuthIpcMsg;
                    await Application.Current.MainPage.DisplayAlert("Auth Request", $"Request granted", "OK");
                    var session = await Session.AppRegisteredAsync(Constants.AppId, ipcMsg.AuthGranted);
                    DependencyService.Get<SafeAppService>().InitialiseSession(session, false);
                    MessagingCenter.Send(this, "UpdateUI");
                }
                else if (decodeResultType == typeof(ContainersIpcMsg))
                {
                    // Get container response and refresh container
                    var ipcMsg = decodeResult as ContainersIpcMsg;
                    await Application.Current.MainPage.DisplayAlert("Container Request", $"Request granted", "OK");
                    await DependencyService.Get<SafeAppService>().RefreshContainersAsync();
                }
                else if (decodeResultType == typeof(ShareMDataIpcMsg))
                {
                    // Get Share MData response
                    var ipcMsg = decodeResult as ShareMDataIpcMsg;
                    await Application.Current.MainPage.DisplayAlert("ShareMData Request", $"Request granted", "OK");
                }
                else if (decodeResultType == typeof(UnregisteredIpcMsg))
                {
                    //Create session object
                    var ipcMsg = decodeResult as UnregisteredIpcMsg;
                    await Application.Current.MainPage.DisplayAlert("Unregistred Request", $"Request granted", "OK");
                    var session = await Session.AppUnregisteredAsync(ipcMsg.SerialisedCfg);
                    DependencyService.Get<SafeAppService>().InitialiseSession(session, true);
                    MessagingCenter.Send(this, "UpdateUI");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Request not granted", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Description: {ex.Message}", "OK");
            }
        }
    }
}
