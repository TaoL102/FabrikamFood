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
        private IMobileServiceTable<Coupon> couponTable;
        private IMobileServiceTable<FoodDish> foodDishTable;
        private IMobileServiceTable<Category> categoryTable;
        private IMobileServiceTable<Reservation> reservationTable;

        private AzureEasyTableManager()
        {
            this.client = new MobileServiceClient("https://msafabrikamfood.azurewebsites.net");
            this.restaurantTable = this.client.GetTable<Restaurant>();
            this.couponTable = this.client.GetTable<Coupon>();
            this.foodDishTable = this.client.GetTable<FoodDish>();
            this.categoryTable= this.client.GetTable<Category>();
            this.reservationTable = this.client.GetTable<Reservation>();
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

        public async Task<List<Restaurant>> GetRestaurantsAsync()
        {
            return await this.restaurantTable.ToListAsync();
        }

        public async Task<List<Coupon>> GetCouponsByApplicableRestaurantIdAsync(string applicableRestaurantId)
        {
            List<Coupon> coupons = await this.couponTable
                .Where(c=>c.ApplicableRestaurantId==applicableRestaurantId)
                .ToListAsync();

            return coupons;
        }

        public async Task<List<Coupon>> GetCouponsAsync()
        {
            List<Coupon> coupons = await this.couponTable.ToListAsync();
            return coupons;
        }

        public async Task InsertTableCoupon(Coupon coupon)
        {
            await this.couponTable.InsertAsync(coupon);
        }

        public async Task<List<FoodDish>> GetFoodDishByCategoryIdAsync(string categoryId)
        {
            List<FoodDish> foodDishes = await this.foodDishTable
                .Where(c => c.CategoryId==categoryId)
                .ToListAsync();
            
            return foodDishes;
        }

        public async Task<List<FoodDish>> GetFoodDishesAsync()
        {
            List<FoodDish> foodDishes = await this.foodDishTable.ToListAsync();
            return foodDishes;
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
            List<Reservation> list = await this.reservationTable.Where(c => c.UserID==userId).ToListAsync();
            list = list.Where(o => o.Date.Date >= DateTime.Today.Date).ToList();
            return list;
        }

        public async Task<List<Reservation>> GetReservationForTodayByUserIdAsync(string userId)
        {
            List<Reservation> list = await GetReservationForTodayByUserIdAsync(userId);
            list = list.Where(o => o.Date.Date == DateTime.Today.Date).ToList();
            return list;
        }
    }
}
