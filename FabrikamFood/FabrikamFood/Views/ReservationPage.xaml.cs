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
    public partial class ReservationPage : ContentPage
    {

        public ReservationPage()
        {
            InitializeComponent();
            TimePicker.Time = DateTime.Now.TimeOfDay;
            SetPickerRestaurantDataSource();

        }

        private void SetPickerRestaurantDataSource()
        {

            foreach (Restaurant r in App.RestaurantList)
            {
                Picker_Restaurant.Items.Add(r.Name);
            }

        }

        private async void Btn_Confirm_Clicked(object sender, EventArgs e)
        {
            // Get current user id
            var user = AzureAuthManager.Instance.CurrentClient.CurrentUser;

            if (user == null)
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
                UserID = user.UserId,
                Date = date,
                Time = time
            };

            await AzureEasyTableManager.Instance.InsertTableReservation(reservation);

            await DisplayAlert("Success", $"You have successfully reserved a book at {restaurantName} on {date}, {time}", "OK");
        }

    }
}
