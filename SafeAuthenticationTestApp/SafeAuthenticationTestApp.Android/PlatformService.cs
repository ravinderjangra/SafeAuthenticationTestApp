using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SafeAuthenticationTestApp.Droid;
using SafeAuthenticationTestApp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformService))]
namespace SafeAuthenticationTestApp.Droid
{
    public class PlatformService : IPlatformService
    {
        public string ConfigFilesPath
        {
            get
            {
                // Personal -> /data/data/@PACKAGE_NAME@/files
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                return path;
            }
        }

        public async Task TransferAssetsAsync(List<(string, string)> fileList)
        {
            foreach (var tuple in fileList)
            {
                using (var reader = new StreamReader(Android.App.Application.Context.Assets.Open(tuple.Item1)))
                {
                    using (var writer = new StreamWriter(Path.Combine(ConfigFilesPath, tuple.Item2)))
                    {
                        await writer.WriteAsync(await reader.ReadToEndAsync());
                        writer.Close();
                    }

                    reader.Close();
                }
            }
        }
    }
}
