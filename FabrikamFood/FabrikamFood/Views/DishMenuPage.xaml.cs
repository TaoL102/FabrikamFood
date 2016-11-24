using FabrikamFood.APIManagers;
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
            // Set listview_restaurants datasource
            var list = await AzureMobileServiceManager.Instance.GetFoodDishesAsync();
            foreach (var item in list)
            {
                item.PicUrl = "PIC" + item.PicUrl;
            }
            ListView_FoodDishes.ItemsSource = list;
            ListView_FoodDishes.HeightRequest = list.Count() * (Constants.LISTVIEW_CELL_HEIGHT_FOODDISH );

        }
    }
}
