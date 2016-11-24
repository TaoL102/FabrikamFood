﻿using System;
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
using FabrikamFood.Helpers;

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
       
            // Offline data sync
            await AzureMobileServiceManager.Instance.SyncAsync();

            // Get restaurant list
            if(DataHelper.GetFromPropertyDictionary("RestaurantList")==null)
            {
restaurantList =await AzureMobileServiceManager.Instance.GetRestaurantsAsync();
                DataHelper.SaveToPropertyDictionary("RestaurantList", restaurantList);
            }
            else
            {
                restaurantList = await AzureMobileServiceManager.Instance.GetRestaurantsAsync();
                DataHelper.SaveToPropertyDictionary("RestaurantList", restaurantList);
                restaurantList = DataHelper.GetFromPropertyDictionary("RestaurantList") as List<Restaurant>;
            }
            

            // Set listview_restaurants datasource
            var reservationList = await GetReservationsForToday();
            ListView_Reservations.ItemsSource = reservationList;
            ListView_Reservations.HeightRequest = reservationList.Count() * (Constants.LISTVIEW_CELL_HEIGHT_RESERVATION + Constants.LISTVIEW_CELL_SPACING);

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
             var couponList= await AzureMobileServiceManager.Instance.GetCouponsByApplicableRestaurantIdAsync(nearestRestaurant.ID);
            ListView_Coupons.ItemsSource = couponList;
            ListView_Coupons.HeightRequest = couponList.Count * (Constants.LISTVIEW_CELL_HEIGHT_COUPON + Constants.LISTVIEW_CELL_SPACING);


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

       

        private async void SetMapText(Restaurant restaurant, ViewModels.GoogleMapsDistance.Element minDistanceElement)
        {
            // Get distance and duration
            string distance = minDistanceElement.distance.text;
            string duration = minDistanceElement.duration.text;
            string uberPrice = await UberManager.Instance.GetEstimatedPrice(currentPosition,new Position(restaurant.Latitude,restaurant.Longitude));
            string uberTime = await UberManager.Instance.GetEstimatedTime(currentPosition, new Position(restaurant.Latitude, restaurant.Longitude));

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

                if (!Settings.IsLoggedIn)
                {

                    return new List<ReservationViewModel>();
                }

                // Get reservations
                List<Reservation> list = await AzureMobileServiceManager.Instance.GetReservationForTodayByUserIdAsync(Settings.UserId);

                if( list == null) return new List<ReservationViewModel>();
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
            catch (Exception )
            {

                return new List<ReservationViewModel>();
            }
           



            
        }
    }
}
