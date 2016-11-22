using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabrikamFood.APIManagers
{
   public partial class AzureAuthManager
    {
        private static readonly AzureAuthManager instance= new AzureAuthManager();
        private MobileServiceClient client;

        private AzureAuthManager()
        {
            this.client = new MobileServiceClient("https://msafabrikamfood.azurewebsites.net");
        }

        public static AzureAuthManager Instance
        {
            get
            {
                return instance;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public async void Authenticate()
        {
            if (App.Authenticator != null)
                 await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);
        }
    }
}
