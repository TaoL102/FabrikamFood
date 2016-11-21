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

namespace FabrikamFood.Views
{
    public partial class ForYou : ContentPage
    {
        // Fields
        private GoogleMapsManager googleMapsManager;
        private WeatherManager weatherManager;
        private AzureEasyTableManager azureEasyTableManager;
        private List<Restaurant> restaurantList;
        private Restaurant nearestRestaurant;
        private ViewModels.GoogleMapsDistance.Element nearestMapElement;
        private Position currentPosition;

        public ForYou()
        {
            InitializeComponent();

            Init();
        }

        private async Task Init()
        {
            // Initialize fileds
            googleMapsManager = GoogleMapsManager.Instance;
            weatherManager = WeatherManager.Instance;
            azureEasyTableManager = AzureEasyTableManager.Instance;

            // Get restaurant list
            restaurantList=await azureEasyTableManager.GetTableRestaurantAsync();

            // Get nearest restaurant and pin to map
            nearestRestaurant = await googleMapsManager.GetNearestRestaurantAsync(restaurantList);
            PinToMap(nearestRestaurant);

            // Get nearest distance element
            nearestMapElement = googleMapsManager.GetMinDistanceElement();

            // Get current position
            currentPosition = await googleMapsManager.GetCurrentPositionAsync();

            // Set map text
            SetMapText(nearestRestaurant, nearestMapElement);

            // Set weather text
            SetWeatherText(currentPosition);
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
            Dictionary<string, string>  dic=await weatherManager.GetCurrentWeather(currentPosition);

            // set text
            Lbl_Weather.Text = dic["CityAndTemp"];
            Img_Weather.Source = dic["IconURL"];
        }
    }
}
