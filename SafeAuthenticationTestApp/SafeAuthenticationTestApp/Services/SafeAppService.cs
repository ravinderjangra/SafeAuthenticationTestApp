using MvvmHelpers;
using SafeApp;
using SafeApp.Utilities;
using SafeAuthenticationTestApp.Helpers;
using SafeAuthenticationTestApp.Model;
using SafeAuthenticationTestApp.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(SafeAppService))]
namespace SafeAuthenticationTestApp.Services
{
    public class SafeAppService : ObservableObject
    {
        private Session _session;

        private bool _isUnregistered;

        private bool _isSessionAvailable;
        public bool IsSessionAvailable
        {
            get { return _isSessionAvailable; }
            set
            {
                _isSessionAvailable = value;
                OnPropertyChanged();
            }
        }
        public void InitialiseSession(Session session, bool isUnregistered)
        {
            _session = session;
            _isUnregistered = isUnregistered;
            IsSessionAvailable = !isUnregistered;
        }

        public async Task RefreshContainersAsync()
        {
            await _session.AccessContainer.RefreshAccessInfoAsync();
        }

        public void ResetSession()
        {
            _session.Dispose();
            _session = null;
            IsSessionAvailable = false;
        }

        public async Task<ShareMDataModel> AddRandomPrivateMDataAsync()
        {

            var typeTag = 150001;
            var mdInfo = await _session.MDataInfoActions.RandomPrivateAsync((ulong)typeTag);
            var metadata = new MetadataResponse
            {
                Name = "Random Private Mdata",
                Description = "Random Description",
                TypeTag = mdInfo.TypeTag,
                XorName = mdInfo.Name
            };

            var actKey = Utilities.GetRandomString(10).ToUtfBytes();
            var actValue = Utilities.GetRandomString(10).ToUtfBytes();
            using (var userSignKeyHandle = await _session.Crypto.AppPubSignKeyAsync())
            using (var permissionsHandle = await _session.MDataPermissions.NewAsync())
            {
                var permissionSet = new PermissionSet { Read = true, Insert = true, Delete = true, Update = true, ManagePermissions = true };
                await _session.MDataPermissions.InsertAsync(permissionsHandle, userSignKeyHandle, permissionSet);
                using (var entriesHandle = await _session.MDataEntries.NewAsync())
                {
                    var encMetaData = await _session.MData.EncodeMetadata(metadata);
                    await _session.MDataEntries.InsertAsync(entriesHandle, Encoding.UTF8.GetBytes(SafeApp.Utilities.AppConstants.MDataMetaDataKey).ToList(), encMetaData);

                    var key = await _session.MDataInfoActions.EncryptEntryKeyAsync(mdInfo, actKey);
                    var value = await _session.MDataInfoActions.EncryptEntryKeyAsync(mdInfo, actValue);
                    await _session.MDataEntries.InsertAsync(entriesHandle, key, value);
                    await _session.MData.PutAsync(mdInfo, permissionsHandle, entriesHandle);
                }
            }
            return new ShareMDataModel((ulong)typeTag, mdInfo.Name);
        }
    }
}
