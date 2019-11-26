﻿using SafeApp;
using SafeApp.Core;
using SafeAuthenticationTestApp.Model;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Constants = SafeAuthenticationTestApp.Helpers.AppConstants;

namespace SafeAuthenticationTestApp.Helpers
{
    internal class RequestHelper
    {
        public static async Task<(uint, string)> GenerateEncodedContainerRequest(
            List<ContainerPermissionsModel> containers,
            [Optional] AppExchangeInfo appExchangeInfo)
        {
            var containerReq = new ContainersReq
            {
                App = string.IsNullOrWhiteSpace(appExchangeInfo.Name) ? Utilities.GetAppExchangeInfo() : appExchangeInfo,
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

        public static async Task<(uint, string)> GenerateEncodedAuthRequest(
            bool appContainerPermission,
            List<ContainerPermissionsModel> containers,
            [Optional] AppExchangeInfo appExchangeInfo)
        {
            var authReq = new AuthReq
            {
                App = string.IsNullOrWhiteSpace(appExchangeInfo.Name) ? Utilities.GetAppExchangeInfo() : appExchangeInfo,
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

        public static async Task<(uint, string)> GenerateEncodedUnregisteredRequest()
        {
            var (reqId, req) = await Session.EncodeUnregisteredRequestAsync(Constants.AppId);
            return (reqId, req);
        }
    }
}
