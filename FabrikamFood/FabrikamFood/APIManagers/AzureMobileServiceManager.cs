#define OFFLINE_SYNC_ENABLED

using FabrikamFood.DataModels;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using FabrikamFood.Authentication;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using FabrikamFood.Helpers;
#endif

namespace FabrikamFood.APIManagers
{
    public class AzureMobileServiceManager
    {
        private static AzureMobileServiceManager instance = new AzureMobileServiceManager();
        private MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        private IMobileServiceSyncTable<Restaurant> restaurantTable;
        private IMobileServiceSyncTable<Coupon> couponTable;
        private IMobileServiceSyncTable<FoodDish> foodDishTable;
        private IMobileServiceSyncTable<Category> categoryTable;
        private IMobileServiceSyncTable<Reservation> reservationTable;
#else
        private IMobileServiceTable<Restaurant> restaurantTable;
        private IMobileServiceTable<Coupon> couponTable;
        private IMobileServiceTable<FoodDish> foodDishTable;
        private IMobileServiceTable<Category> categoryTable;
        private IMobileServiceTable<Reservation> reservationTable;
#endif


        private AzureMobileServiceManager()
        {
            

            var handler = new AuthHandler();
            //Create our client and pass in Authentication handler
            this.client = new MobileServiceClient(Constants.APPLICATION_URL, handler);

            //assign mobile client to handler
            handler.Client = client;

            client.CurrentUser = new MobileServiceUser(Settings.UserId);
            client.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;



#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<Restaurant>();
            store.DefineTable<Coupon>();
            store.DefineTable<FoodDish>();
            store.DefineTable<Category>();
            store.DefineTable<Reservation>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            this.restaurantTable = this.client.GetSyncTable<Restaurant>();
            this.couponTable = this.client.GetSyncTable<Coupon>();
            this.foodDishTable = this.client.GetSyncTable<FoodDish>();
            this.categoryTable = this.client.GetSyncTable<Category>();
            this.reservationTable = this.client.GetSyncTable<Reservation>();

#else
			this.restaurantTable = this.client.GetTable<Restaurant>();
            this.couponTable = this.client.GetTable<Coupon>();
            this.foodDishTable = this.client.GetTable<FoodDish>();
            this.categoryTable= this.client.GetTable<Category>();
            this.reservationTable = this.client.GetTable<Reservation>();
#endif

        }

        public static AzureMobileServiceManager Instance
        {
            get
            {
                return instance;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get
            {
                return client;
            }
        }


        public async Task InsertTableRestaurant(Restaurant restaurant)
        {

            await this.restaurantTable.InsertAsync(restaurant);
        }

        public async Task<List<Restaurant>> GetRestaurantsAsync()
        {
            try
            {
                return await this.restaurantTable.ToListAsync();

            }
            catch (MobileServiceInvalidOperationException msioe)
            {

                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task<List<Coupon>> GetCouponsByApplicableRestaurantIdAsync(string applicableRestaurantId)
        {

            try
            {
                List<Coupon> coupons = await this.couponTable
                                .Where(c => c.ApplicableRestaurantId == applicableRestaurantId)
                                .ToListAsync();

                return coupons;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task<List<Coupon>> GetCouponsAsync()
        {
            try
            {
                List<Coupon> coupons = await this.couponTable.ToListAsync();
                return coupons;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task InsertTableCoupon(Coupon coupon)
        {
            await this.couponTable.InsertAsync(coupon);
        }

        public async Task<List<FoodDish>> GetFoodDishByCategoryIdAsync(string categoryId)
        {
            try
            {
                List<FoodDish> foodDishes = await this.foodDishTable
                                .Where(c => c.CategoryId == categoryId)
                                .ToListAsync();

                return foodDishes;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task<List<FoodDish>> GetFoodDishesAsync()
        {
            try
            {
                List<FoodDish> foodDishes = await this.foodDishTable.ToListAsync();
                return foodDishes;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task<FoodDish> GetFoodDishByIdAsync(string id)
        {
            try
            {
                FoodDish foodDish= await this.foodDishTable.LookupAsync(id);
                return foodDish;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task InsertTableFoodDish(FoodDish foodDish)
        {
            await this.foodDishTable.InsertAsync(foodDish);
        }

        public async Task InsertTableCategory(Category category)
        {
            await this.categoryTable.InsertAsync(category);
        }

        public async Task InsertTableReservation(Reservation o)
        {
            await this.reservationTable.InsertAsync(o);
        }

        public async Task<List<Reservation>> GetReservationByUserIdAsync(string userId)
        {
            try
            {
                List<Reservation> list = await this.reservationTable.Where(c => c.UserID == userId).ToListAsync();
                list = list.Where(o => o.Date.Date >= DateTime.Today.Date).ToList();
                return list;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task<List<Reservation>> GetReservationForTodayByUserIdAsync(string userId)
        {
            try
            {
                List<Reservation> list = await GetReservationByUserIdAsync(userId);
                list = list.Where(o => o.Date.Date == DateTime.Today.Date).ToList();
                return list;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async void DeleteReservationByIdAsync(string id)
        {
            try
            {
                // get instance
                var instance=await this.reservationTable.LookupAsync(id);
                await this.reservationTable.DeleteAsync(instance);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
           
        }


#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.restaurantTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "restaurantTable",
                    this.restaurantTable.CreateQuery());

                await this.couponTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "couponTable",
                    this.couponTable.CreateQuery());

                await this.foodDishTable.PullAsync(
                   //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                   //Use a different query name for each unique query in your program
                   "foodDishTable",
                   this.foodDishTable.CreateQuery());

                await this.categoryTable.PullAsync(
                   //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                   //Use a different query name for each unique query in your program
                   "categoryTable",
                   this.categoryTable.CreateQuery());

                await this.reservationTable.PullAsync(
   //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
   //Use a different query name for each unique query in your program
   "reservationTable",
   this.reservationTable.CreateQuery());


            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
#endif

        public async Task<SocialLoginResult> GetUserData()
        {
            return await client.InvokeApiAsync<SocialLoginResult>(
                    "getextrauserinfo",
                    HttpMethod.Get, null);
        }
    }
}
