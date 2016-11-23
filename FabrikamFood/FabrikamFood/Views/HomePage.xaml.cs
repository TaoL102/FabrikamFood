using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Xamarin.Forms;
using System.Net.Http;
using static FabrikamFood.ViewModels.GoogleMapsDistance;
using Xamarin.Forms.Maps;
using Android.Locations;
using Plugin.Geolocator;
using FabrikamFood.DataModels;
using FabrikamFood.APIManagers;
using System.Globalization;
using FabrikamFood.ViewModels;

namespace FabrikamFood.Views
{
    public partial class HomePage : ContentPage
    {
        // Fields
        private List<Restaurant> restaurantList;
        private Restaurant nearestRestaurant;
        private ViewModels.GoogleMapsDistance.Element nearestMapElement;
        private Position currentPosition;

        public HomePage()
        {
            InitializeComponent();

            Init();
        }

        private async void Init()
        {
            await AzureMobileServiceManager.Instance.SyncAsync();

            // Set listview_restaurants datasource
            ListView_Reservations.ItemsSource = await GetReservationsForToday();

            // Get restaurant list
            restaurantList = App.RestaurantList;

            // Get nearest restaurant and pin to map
            nearestRestaurant = await GoogleMapsManager.Instance.GetNearestRestaurantAsync(restaurantList);
            PinToMap(nearestRestaurant);

            // Get nearest distance element
            nearestMapElement = GoogleMapsManager.Instance.GetMinDistanceElement();

            // Get current position
            currentPosition = await GoogleMapsManager.Instance.GetCurrentPositionAsync();

            // Set map text
            SetMapText(nearestRestaurant, nearestMapElement);

            // Set weather text
            SetWeatherText(currentPosition);

            // Set listview_coupons datasource
            ListView_Coupons.ItemsSource = await AzureMobileServiceManager.Instance.GetCouponsByApplicableRestaurantIdAsync(nearestRestaurant.ID);


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

       

        private void SetMapText(Restaurant restaurant, ViewModels.GoogleMapsDistance.Element minDistanceElement)
        {
            // Get distance and duration
            string distance = minDistanceElement.distance.text;
            string duration = minDistanceElement.duration.text;

            MapRestaurantName.Text = restaurant.Name;
            MapRestaurantNav.Text = String.Format("Drive: {0}({1})", distance, duration);
        }

        private void NavBtn_Clicked(object sender, EventArgs e)
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

        private async void SetWeatherText(Position currentPosition)
        {
            // Get weather info
            Dictionary<string, string>  dic=await WeatherManager.Instance.GetCurrentWeather(currentPosition);

            // set text
            Lbl_Weather.Text = dic["CityAndTemp"];
            Img_Weather.Source = dic["IconURL"];
        }

        private async Task<List<ReservationViewModel>> GetReservationsForToday()
        {
            try
            {
 // Get current user id
            

                if (App.GetUserId() == null)
                {

                    return null;
                }

                // Get reservations
                List<Reservation> list = await AzureMobileServiceManager.Instance.GetReservationForTodayByUserIdAsync(App.GetUserId());

                if( list == null) return null;
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
