using FabrikamFood.DataModels;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabrikamFood.APIManagers
{
    class AzureEasyTableManager
    {
        private static AzureEasyTableManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<Restaurant> restaurantTable;

        private AzureEasyTableManager()
        {
            this.client = new MobileServiceClient("http://msafabrikamfood.azurewebsites.net");
            this.restaurantTable = this.client.GetTable<Restaurant>();
            
        }

        public static AzureEasyTableManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureEasyTableManager();
                }
                return instance;
            }
        }

        public async Task InsertTableRestaurant(Restaurant restaurant)
        {
            await this.restaurantTable.InsertAsync(restaurant);
        }

        public async Task<List<Restaurant>> GetTableRestaurantAsync()
        {
            return await this.restaurantTable.ToListAsync();
        }

    }
}
