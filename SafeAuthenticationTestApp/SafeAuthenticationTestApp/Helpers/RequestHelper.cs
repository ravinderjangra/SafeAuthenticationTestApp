using SafeApp;
using SafeApp.Utilities;
using SafeAuthenticationTestApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Constants = SafeAuthenticationTestApp.Helpers.AppConstants;

namespace SafeAuthenticationTestApp.Helpers
{
    internal class RequestHelper
    {
        public static async Task<(uint, string)> GenerateEncodedContainerRequest(List<ContainerPermissionsModel> containers)
        {
            var containerReq = new ContainersReq
            {
                App = Utilities.GetAppExchangeInfo(),
                Containers = new List<ContainerPermissions>()
            };

            foreach (var item in containers)
            {
                if (item.IsRequested)
                {
                    containerReq.Containers.Add(new ContainerPermissions
                    {
                        ContName = item.ContName,
                        Access = new PermissionSet
                        {
                            Read = item.Access.Read,
                            Insert = item.Access.Insert,
                            Delete = item.Access.Delete,
                            Update = item.Access.Update,
                            ManagePermissions = item.Access.ManagePermissions,
                        }
                    });
                }
            }
            var encodedContainerRequest = await Session.EncodeContainerRequestAsync(containerReq);
            return encodedContainerRequest;
        }

        public static async Task<(uint, string)> GenerateEncodedAuthRequest(bool appContainerPermission, List<ContainerPermissionsModel> containers)
        {
            var authReq = new AuthReq
            {
                App = Utilities.GetAppExchangeInfo(),
                Containers = new List<ContainerPermissions>()
            };

            authReq.AppContainer = appContainerPermission;
            foreach (var item in containers)
            {
                if (item.IsRequested)
                {
                    authReq.Containers.Add(new ContainerPermissions
                    {
                        ContName = item.ContName,
                        Access = new PermissionSet
                        {
                            Read = item.Access.Read,
                            Insert = item.Access.Insert,
                            Delete = item.Access.Delete,
                            Update = item.Access.Update,
                            ManagePermissions = item.Access.ManagePermissions,
                        }
                    });
                }
            }
            var encodedAuthRequest = await Session.EncodeAuthReqAsync(authReq);
            return encodedAuthRequest;
        }

        public static async Task<(uint, string)> GenerateEncodedShareMDataRequest(List<ShareMDataModel> mDataList)
        {
            var shareMDataReq = new ShareMDataReq
            {
                App = Utilities.GetAppExchangeInfo(),
                MData = new List<ShareMData>()
            };

            foreach (var item in mDataList)
            {
                shareMDataReq.MData.Add(new ShareMData()
                {
                    Name = item.Name,
                    TypeTag = item.TypeTag,
                    Perms = new PermissionSet
                    {
                        Read = item.Access.Read,
                        Insert = item.Access.Insert,
                        Delete = item.Access.Delete,
                        Update = item.Access.Update,
                        ManagePermissions = item.Access.ManagePermissions,
                    }
                });
            }

            var encodedShareMDataRequest = await Session.EncodeShareMDataRequestAsync(shareMDataReq);
            return encodedShareMDataRequest;
        }

        public static async Task<(uint, string)> GenerateEncodedUnregisteredRequest()
        {
            var (reqId, req) = await Session.EncodeUnregisteredRequestAsync(Constants.AppId);
            return (reqId, req);
        }
    }
}
