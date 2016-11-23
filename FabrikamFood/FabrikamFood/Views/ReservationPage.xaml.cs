using FabrikamFood.APIManagers;
using FabrikamFood.DataModels;
using FabrikamFood.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FabrikamFood.Views
{
    public partial class ReservationPage : ContentPage
    {

        public ReservationPage()
        {
            InitializeComponent();
            Init();
        }

        private async void Init()
        {
            TimePicker.Time = DateTime.Now.TimeOfDay;
            SetPickerRestaurantDataSource();

            // Set listview_restaurants datasource
            var items=await GetReservations();
            ListView_Reservations.ItemsSource = items;
            ListView_Reservations.HeightRequest = Constants.LISTVIEW_CELL_HEIGHT_RESERVATION * items.Count();
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

        private async Task<List<ReservationViewModel>> GetReservations()
        {
            try
            {
                // Get current user id


                if (App.GetUserId() == null)
                {

                    return null;
                }

                // Get reservations
                List<Reservation> list = await AzureMobileServiceManager.Instance.GetReservationByUserIdAsync(App.GetUserId());

                if (list == null) return null;
                List<ReservationViewModel> listModel = new List<ReservationViewModel>();

                foreach (var r in list)
                {
                    Restaurant restaurant = App.RestaurantList.Where(o => o.ID.Equals(r.RestaurantID)).FirstOrDefault();
                    ReservationViewModel model = new ReservationViewModel()
                    {
                        ID = r.ID,
                        RestaurantID = restaurant.ID,
                        RestaurantAddress = restaurant.Address,
                        RestaurantName = restaurant.Name,
                        RestaurantPosition = new Position(restaurant.Latitude, restaurant.Longitude),
                        Date = r.Date.ToString("dd/MM/yyyy"),
                        Time = r.Time.ToString(@"hh\:mm")
                    };

                    listModel.Add(model);
                }

                return listModel;


            }
            catch (Exception ex)
            {

                throw ex;
            }





        }
    }
}
