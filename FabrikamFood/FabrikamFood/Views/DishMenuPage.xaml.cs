using FabrikamFood.APIManagers;
using FabrikamFood.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FabrikamFood.Views
{
    public partial class DishMenuPage : ContentPage
    {
        public DishMenuPage()
        {
            InitializeComponent();
            Init();

        }

        private async void Init()
        {
            List<FoodDish> list = new List<FoodDish>();
            await Task.Run(async () => {
                 list = await AzureMobileServiceManager.Instance.GetFoodDishesAsync();
            });
            if (list.Count <= 0) return;
            // Set listview_restaurants datasource
           
            foreach (var item in list)
            {
                item.PicUrl = "PIC" + item.PicUrl;
            }
            ListView_FoodDishes.ItemsSource = list;
            ListView_FoodDishes.HeightRequest = list.Count() * (Constants.LISTVIEW_CELL_HEIGHT_FOODDISH );
            Frame_Dishes.IsVisible = true;
        }
    }
}
