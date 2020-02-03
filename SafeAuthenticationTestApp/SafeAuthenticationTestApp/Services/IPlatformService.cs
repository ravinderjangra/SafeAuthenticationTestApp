using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeAuthenticationTestApp.Services
{
    public interface IPlatformService
    {
        string ConfigFilesPath { get; }

        Task TransferAssetsAsync(List<(string, string)> fileList);
    }
}
