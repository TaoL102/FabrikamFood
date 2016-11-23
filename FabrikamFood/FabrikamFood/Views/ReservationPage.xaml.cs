using FabrikamFood.APIManagers;
using FabrikamFood.DataModels;
using FabrikamFood.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
            // Add a button to toolbar
            // Reference: https://forums.xamarin.com/discussion/17808/add-a-nav-bar-right-button
            ToolbarItem btn_Add_Reservation = new ToolbarItem();
            btn_Add_Reservation.Icon = "ic_add_white_48dp.png";
            btn_Add_Reservation.Clicked += Btn_Add_Reservation_Clicked;
            this.ToolbarItems.Add(btn_Add_Reservation);


            // Set listview_restaurants datasource
            var reservationList = await GetReservations();
            ListView_Reservations.ItemsSource = reservationList;
            ListView_Reservations.HeightRequest = reservationList.Count() * (Constants.LISTVIEW_CELL_HEIGHT_RESERVATION + Constants.LISTVIEW_CELL_SPACING);

        }

        private void Btn_Add_Reservation_Clicked(object sender, EventArgs e)
        {
            App.RootPage.Detail = new NavigationPage(new AddUpdateReservationPage());
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
