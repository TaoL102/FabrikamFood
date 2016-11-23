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
    public partial class AddUpdateReservationPage : ContentPage
    {
        public AddUpdateReservationPage()
        {
            InitializeComponent();

            TimePicker.Time = DateTime.Now.TimeOfDay;

            SetPickerRestaurantDataSource();

            // Add a save button to toolbar
            // Reference: https://forums.xamarin.com/discussion/17808/add-a-nav-bar-right-button
            ToolbarItem btn_Add_Reservation = new ToolbarItem();
            btn_Add_Reservation.Icon = "ic_add_white_48dp.png";
            btn_Add_Reservation.Clicked += Btn_Save_Reservation_Clicked;
            this.ToolbarItems.Add(btn_Add_Reservation);
        }

        private void SetPickerRestaurantDataSource()
        {

            foreach (Restaurant r in App.RestaurantList)
            {
                Picker_Restaurant.Items.Add(r.Name);
            }

        }

        private void Btn_Save_Reservation_Clicked(object sender, EventArgs e)
        {
            // Open a dialogue

        }


        private async void Btn_Confirm_Clicked(object sender, EventArgs e)
        {
            // Get current user id
            var userId = App.GetUserId();

            if (userId == null)
            {
                await DisplayAlert("Fail", $"Please log in.", "OK");
                return;
            }

            // Get user input
            string restaurantName = Picker_Restaurant.Items[Picker_Restaurant.SelectedIndex];
            Restaurant restaurant = App.RestaurantList.Where(r => r.Name.Equals(restaurantName)).FirstOrDefault();
            string restaurantId = restaurant.ID;
            DateTime date = DatePicker.Date.Date;
            TimeSpan time = TimePicker.Time;

            Reservation reservation = new Reservation()
            {
                ID = Guid.NewGuid().ToString(),
                RestaurantID = restaurantId,
                UserID = userId,
                Date = date,
                Time = time
            };

            await AzureMobileServiceManager.Instance.InsertTableReservation(reservation);

            await DisplayAlert("Success", $"You have successfully reserved a book at {restaurantName} on {date}, {time}", "OK");
        }


    }
}
