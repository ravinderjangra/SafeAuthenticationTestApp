using SafeAuthenticationTestApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeAuthenticationTestApp.Services
{
    public interface ISafeService
    {
        Task<string> CreateAuthRequestAsync(bool isAppContainerRequested, List<ContainerPermissionsModel> containers);
        Task<string> CreateContainerRequestAsync(List<ContainerPermissionsModel> containers);
        Task<string> CreateMDataShareRequestAsync(List<ShareMDataModel> shareMDataList);
        Task<string> CreateUnregisteredRequestAsync();
        void SendRequest(string encodedRequest, bool isUnregistered);
        Task ProcessResponseAsync(string encodedResponse);
    }
}
