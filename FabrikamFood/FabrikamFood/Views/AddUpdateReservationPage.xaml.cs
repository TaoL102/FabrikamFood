using FabrikamFood.APIManagers;
using FabrikamFood.DataModels;
using FabrikamFood.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FabrikamFood.Views
{
    public partial class AddUpdateReservationPage : ContentPage
    {
        private List<Restaurant> restaurantList;
        public AddUpdateReservationPage()
        {
            InitializeComponent();

            Init();

        }

        private void Init()
        {
            // Add a save button to toolbar
            // Reference: https://forums.xamarin.com/discussion/17808/add-a-nav-bar-right-button
            ToolbarItem btn_Add_Reservation = new ToolbarItem();
            btn_Add_Reservation.Icon = "ic_save_white_48dp.png";
            btn_Add_Reservation.Clicked += Btn_Save_Reservation_Clicked;
            this.ToolbarItems.Add(btn_Add_Reservation);

            // Get restaurants
            restaurantList = DataHelper.GetFromPropertyDictionary("RestaurantList") as List<Restaurant>;

            // Set restaurant picker datasource and default value
            foreach (Restaurant r in restaurantList)
            {
                Picker_Restaurant.Items.Add(r.Name);
            }
            Picker_Restaurant.SelectedIndex = 0;

            // Set timepicker default time
            TimePicker.Time = DateTime.Now.TimeOfDay;

        }

        private async void Btn_Save_Reservation_Clicked(object sender, EventArgs e)
        {
            // Get user input
            string restaurantName = Picker_Restaurant.Items[Picker_Restaurant.SelectedIndex];
            Restaurant restaurant = restaurantList.Where(r => r.Name.Equals(restaurantName)).FirstOrDefault();
            string restaurantId = restaurant.ID;
            DateTime date = DatePicker.Date.Date;
            TimeSpan time = TimePicker.Time;

            // Open a dialogue
           var result=await UIHelper.PopUpYesOrNoDialogue("Confirm", $"Restaurant: {restaurantName}\nDate: {date.ToString("dd/MM/yyyy")}\nTime: {time.ToString(@"hh\:mm")}");

            if (result)
            {

                // Get current user id
                var userId = Settings.UserId;

                if (userId == null)
                {
                    UIHelper.PopUpYesDialogue("Fail", $"Please log in.");
                    return;
                }




                Reservation reservation = new Reservation()
                {
                    ID = Guid.NewGuid().ToString(),
                    RestaurantID = restaurantId,
                    UserID = userId,
                    Date = date,
                    Time = time
                };

                try
                {
await AzureMobileServiceManager.Instance.InsertTableReservation(reservation);
                }
                catch (Exception ex)
                {
                    UIHelper.PopUpYesDialogue("Fail", $"Error:" + ex.Message);
                    return;
                }

                App.RootPage.Detail = new NavigationPage(new ReservationPage());

            }


        }
    }
}
