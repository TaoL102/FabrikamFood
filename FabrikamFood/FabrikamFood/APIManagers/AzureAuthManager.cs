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
        private static AzureAuthManager instance;
        private MobileServiceClient client;

        private AzureAuthManager()
        {
            this.client = new MobileServiceClient("http://msafabrikamfood.azurewebsites.net");
        }

        public static AzureAuthManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureAuthManager();
                }
                return instance;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }
    }
}
