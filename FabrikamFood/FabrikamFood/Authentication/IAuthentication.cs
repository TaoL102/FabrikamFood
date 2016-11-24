#define AZURE

#if AZURE
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FabrikamFood.Authentication
{
    public interface IAuthentication
    {
        Task<MobileServiceUser> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider, JObject paramameters = null);
        Task<bool> RefreshUser(IMobileServiceClient client);
        void ClearCookies();
    }
}
#endif
