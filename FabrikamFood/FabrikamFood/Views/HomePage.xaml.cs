using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using FabrikamFood.DataModels;
using FabrikamFood.APIManagers;
using FabrikamFood.ViewModels;
using FabrikamFood.Helpers;
using System.Diagnostics;

namespace FabrikamFood.Views
{
    public partial class HomePage : ContentPage
    {
        // Fields
        private List<Restaurant> restaurantList;
        private Restaurant nearestRestaurant;

        public HomePage()
        {
            InitializeComponent();
            Init();
        }

        private async void Init()
        {

            // Offline data sync
            Task.Run(async () => { await AzureMobileServiceManager.Instance.SyncAsync(); });

            // Set weather text
            SetWeatherText();

            // Get restaurant list
            if (DataHelper.GetFromPropertyDictionary("RestaurantList") == null)
            {
                restaurantList = await AzureMobileServiceManager.Instance.GetRestaurantsAsync();
                DataHelper.SaveToPropertyDictionary("RestaurantList", restaurantList);
            }
            else
            {
                restaurantList = DataHelper.GetFromPropertyDictionary("RestaurantList") as List<Restaurant>;
            }

            // Get nearest restaurant and pin to map
            nearestRestaurant = await GoogleMapsManager.Instance.GetNearestRestaurantAsync(restaurantList);
            PinToMap(nearestRestaurant);

            // Set map text
            SetMapText(nearestRestaurant);

            // Set listview_restaurants datasource
            SetListView_Reservations();

            // Set listview_coupons datasource
            SetListView_Coupons();

        }

        private async void SetListView_Reservations()
        {
            var reservationList=new List<ReservationViewModel>();
            await Task.Run(async () => {
                reservationList= await GetReservationsForToday();
            });
            if (reservationList.Count <= 0) return;
            ListView_Reservations.ItemsSource = reservationList;
            ListView_Reservations.HeightRequest = reservationList.Count() * (Constants.LISTVIEW_CELL_HEIGHT_RESERVATION + Constants.LISTVIEW_CELL_SPACING);
            Frame_Reservation_Today.IsVisible = true;
        }

        private async void SetListView_Coupons()
        {
            var couponList = new List<Coupon>();
            await Task.Run(async () => {
                 couponList = await AzureMobileServiceManager.Instance.GetCouponsByApplicableRestaurantIdAsync(nearestRestaurant.ID);
            });
            if (couponList.Count <= 0) return;

ListView_Coupons.ItemsSource = couponList;
            ListView_Coupons.HeightRequest = couponList.Count * (Constants.LISTVIEW_CELL_HEIGHT_COUPON + Constants.LISTVIEW_CELL_SPACING);
            Frame_Coupon.IsVisible = true;
        }

        private void PinToMap(Restaurant restaurant)
        {
            Position position = new Position(restaurant.Latitude, restaurant.Longitude);

            // Pin 
            var pin = new Pin
            {
                Type = PinType.SearchResult,
                Position = position,
                Label = restaurant.Name,
                Address = restaurant.Address
            };

            // Move map to this position
            NearestRestaurantMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    position, Xamarin.Forms.Maps.Distance.FromMiles(1)));

            // Pin to map
            NearestRestaurantMap.Pins.Add(pin);

        }



        private async void SetMapText(Restaurant restaurant)
        {
            string distance = "";
            string duration = "";
            string uberPrice = "";
            string uberTime = "";

            await Task.Run(async () =>
            {
                // Get current pos
                var currentPosition = await GoogleMapsManager.Instance.GetCurrentPositionAsync();
                // Get nearest distance element
                var nearestMapElement = GoogleMapsManager.Instance.GetMinDistanceElement();

                // Get distance and duration
                distance = nearestMapElement.distance.text;
                duration = nearestMapElement.duration.text;
                uberPrice = await UberManager.Instance.GetEstimatedPrice(currentPosition, new Position(restaurant.Latitude, restaurant.Longitude));
                uberTime = await UberManager.Instance.GetEstimatedTime(currentPosition, new Position(restaurant.Latitude, restaurant.Longitude));

            });


            MapRestaurantName.Text = restaurant.Name;
            Btn_Uber_Price.Text = uberPrice;
            Btn_Uber_Time.Text = uberTime;
            Btn_Nav_Distance.Text = distance;
            Btn_Nav_Time.Text = duration;
        }

        private void Btn_Nav_Clicked(object sender, EventArgs e)
        {
            // Get pos string
            string pos = nearestRestaurant.Latitude + "," + nearestRestaurant.Longitude;

            //Reference: https://developer.xamarin.com/recipes/cross-platform/xamarin-forms/maps/map-navigation/
            switch (Device.OS)
            {
                case TargetPlatform.iOS:
                    Device.OpenUri(
                      new Uri(string.Format("http://maps.apple.com/?q={0}", WebUtility.UrlEncode(pos))));
                    break;
                case TargetPlatform.Android:
                    Device.OpenUri(
                      new Uri(string.Format("geo:0,0?q={0}", WebUtility.UrlEncode(pos))));
                    break;
                case TargetPlatform.Windows:
                case TargetPlatform.WinPhone:
                    Device.OpenUri(
                      new Uri(string.Format("bingmaps:?where={0}", Uri.EscapeDataString(pos))));
                    break;
            }
        }

        private void Btn_Uber_Clicked(object sender, EventArgs e)
        {


            // Get dropoffPos string
            string dropoffPos = "dropoff[latitude] =" + nearestRestaurant.Latitude + "dropoff[longitude]=" + nearestRestaurant.Longitude;
            string clientId = Constants.UBER_CLIENT_ID;

            //Reference: https://developer.xamarin.com/recipes/cross-platform/xamarin-forms/maps/map-navigation/
            switch (Device.OS)
            {
                //case TargetPlatform.iOS:
                //    Device.OpenUri(
                //      new Uri(string.Format("http://maps.apple.com/?q={0}", WebUtility.UrlEncode(dropoffPos))));
                //    break;
                case TargetPlatform.Android:
                    Device.OpenUri(
                      new Uri(string.Format("uber://?action=setPickup&pickup=my_location&{0}&client_id={1}", WebUtility.UrlEncode(dropoffPos), WebUtility.UrlEncode(clientId))));
                    break;
                    //case TargetPlatform.Windows:
                    //case TargetPlatform.WinPhone:
                    //    Device.OpenUri(
                    //      new Uri(string.Format("bingmaps:?where={0}", Uri.EscapeDataString(dropoffPos))));
                    //    break;
            }
           
        }

        private async void SetWeatherText()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            await Task.Run(async () =>
            {
                // Get current pos
                var currentPosition = await GoogleMapsManager.Instance.GetCurrentPositionAsync();

                // Get weather info
                dic = await WeatherManager.Instance.GetCurrentWeather(currentPosition);
            });



            // set text
            if (DateTime.Now.Hour < 12)
            {
                Lbl_Greeting.Text = "Good Morning";
            
            }
            else if (DateTime.Now.Hour < 17)
            {
                Lbl_Greeting.Text = "Good Afternoon";
          
            }
            else
            {
                Lbl_Greeting.Text = "Good Evening";
               
            }
            Lbl_Weather.Text = dic["CityAndTemp"];
            Img_Weather.Source = dic["IconURL"];
        }

        private async Task<List<ReservationViewModel>> GetReservationsForToday()
        {
            try
            {

                if (!Settings.IsLoggedIn)
                {

                    return new List<ReservationViewModel>();
                }

                // Get reservations
                List<Reservation> list = await AzureMobileServiceManager.Instance.GetReservationForTodayByUserIdAsync(Settings.UserId);

                if (list == null) return new List<ReservationViewModel>();
                List<ReservationViewModel> listModel = new List<ReservationViewModel>();

                foreach (var r in list)
                {
                    Restaurant restaurant = restaurantList.Where(o => o.ID.Equals(r.RestaurantID)).FirstOrDefault();
                    ReservationViewModel model = new ReservationViewModel()
                    {
                        ID = r.ID,
                        RestaurantID = restaurant.ID,
                        RestaurantAddress = restaurant.Address,
                        RestaurantName = restaurant.Name,
                        RestaurantPosition = new Position(restaurant.Latitude, restaurant.Longitude),
                        Date = r.Date.ToString("dd/MM/yyyy"),
                        Time = r.Time.ToString(@"hh\:mm"),
                        RestaurantPhone = restaurant.Phone,
                    };

                    listModel.Add(model);
                }

                return listModel;


            }
            catch (Exception)
            {

                return new List<ReservationViewModel>();
            }





        }
    }
}
