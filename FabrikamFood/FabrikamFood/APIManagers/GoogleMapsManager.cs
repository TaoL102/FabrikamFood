using FabrikamFood.DataModels;
using Newtonsoft.Json;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using static FabrikamFood.ViewModels.GoogleMapsDistance;

namespace FabrikamFood.APIManagers
{
    class GoogleMapsManager
    {
        private static readonly GoogleMapsManager instance=new GoogleMapsManager();
        private Element minDistanceElement;


        private GoogleMapsManager()
        {

        }

        public static GoogleMapsManager Instance
        {
            get
            {
                return instance;
            }
        }


        /// <summary>
        /// 
        /// Reference:https://www.nuget.org/packages/Xam.Plugin.Geolocator/
        /// </summary>
        /// <returns></returns>
        public async Task<Position> GetCurrentPositionAsync()
        {
            
            var locator = CrossGeolocator.Current;

            locator.DesiredAccuracy = 100; //100 is new default

            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

            Position p = new Position(position.Latitude, position.Longitude);

            return p;
        }

        /// <summary>
        /// 
        /// Reference: https://developers.google.com/maps/documentation/distance-matrix/start
        /// </summary>
        /// <returns></returns>
        public async Task<Restaurant> GetNearestRestaurantAsync(List<Restaurant> restaurantList)
        {
         
            // Get current position
            Position curPos = await GetCurrentPositionAsync();

             String[] originsArray = new string[restaurantList.Count];
            for (int i = 0; i < restaurantList.Count; i++)
            {
                originsArray[i] = restaurantList[i].Latitude + "," + restaurantList[i].Longitude;
            }

            // API call parameters
            string apiKey = "key=AIzaSyAGDpbFjrhzH7NPQ7Omb7u9uFbFVkneXcQ";
            string url = "https://maps.googleapis.com/maps/api/distancematrix/json?";
            string unit = "units=metric";
            string destinations = String.Format("destinations={0}", string.Join("|", originsArray));
            string origins = String.Format("origins={0},{1}", curPos.Latitude, curPos.Longitude);

            // Httpclient
            HttpClient client = new HttpClient();

            // Get Json
            string x = await client.GetStringAsync(
                new Uri(String.Format("{0}{1}&{2}&{3}&{4}", url, unit, origins, destinations, apiKey)));

            // Deserialize
            RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(x);

            // Analyze all elements in Json data
            List<ViewModels.GoogleMapsDistance.Element> elements = rootObject.rows.FirstOrDefault().elements;
            minDistanceElement = elements.OrderBy(e => Convert.ToInt32(e.distance.value)).First();
            int n = elements.IndexOf(minDistanceElement);

            return restaurantList[n];

        }

        public Element GetMinDistanceElement()
        {
            return minDistanceElement;
        }
    }
}
