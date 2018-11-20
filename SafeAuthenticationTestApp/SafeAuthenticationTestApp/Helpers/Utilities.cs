using SafeApp.Utilities;
using System;
using System.Linq;

namespace SafeAuthenticationTestApp.Helpers
{
    public class Utilities
    {
        private static readonly Random Random = new Random();

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static AppExchangeInfo GetAppExchangeInfo()
        {
            return new AppExchangeInfo { Id = AppConstants.AppId, Name = AppConstants.AppName, Scope = null, Vendor = AppConstants.AppVendor };
        }
    }
}
