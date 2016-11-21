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

        private AzureEasyTableManager()
        {
            this.client = new MobileServiceClient("http://msafabrikamfood.azurewebsites.net");
            this.restaurantTable = this.client.GetTable<Restaurant>();
            this.couponTable = this.client.GetTable<Coupon>();
            this.foodDishTable = this.client.GetTable<FoodDish>();
            this.categoryTable= this.client.GetTable<Category>();
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
            List<Coupon> coupons = await GetCouponsAsync();
            coupons= coupons.Where(c=>c.ApplicableRestaurantId.Equals(applicableRestaurantId)).ToList();
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
            List<FoodDish> foodDishes = await GetFoodDishesAsync();
            foodDishes = foodDishes.Where(c => c.CategoryId.Equals(categoryId)).ToList();
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
    }
}
