﻿using SafeApp.Utilities;
using SafeAuthenticationTestApp.Model;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SafeAuthenticationTestApp.Services
{
    public interface ISafeService
    {
        Task<string> CreateAuthRequestAsync(
            bool isAppContainerRequested, List<ContainerPermissionsModel> containers,
            [Optional] AppExchangeInfo appExchangeInfo);

        Task<string> CreateContainerRequestAsync(
            List<ContainerPermissionsModel> containers,
            [Optional] AppExchangeInfo appExchangeInfo);

        Task<string> CreateMDataShareRequestAsync(List<ShareMDataModel> shareMDataList);

        Task<string> CreateUnregisteredRequestAsync();

        void SendRequest(string encodedRequest, bool isUnregistered);

        Task ProcessResponseAsync(string encodedResponse);
    }
}
