using FabrikamFood.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace FabrikamFood.APIManagers
{
    class WeatherManager
    {
        private static readonly WeatherManager instance = new WeatherManager();

        private WeatherManager()
        { }

        public static WeatherManager Instance
        {
            get
            {
                return instance;
            }
        }

        public async Task<Dictionary<string, string>> GetCurrentWeather(Position currentPosition)
        {
            
            // API call parameters
            string apiKey = "appid="+Constants.WEATHER_API_KEY;
            string url = "http://api.openweathermap.org/data/2.5/weather?";
            string pos = "lat=" + currentPosition.Latitude + "&lon=" + currentPosition.Longitude;
            string unit = "units=metric";

            // Httpclient
            HttpClient client = new HttpClient();

            string test = String.Format("{0}{1}&{2}&{3}", url, pos, unit, apiKey);

            // Get Json
            string x = await client.GetStringAsync(new Uri(String.Format("{0}{1}&{2}&{3}", url, pos, unit, apiKey)));

            // Deserialize
            CurrentWeather.RootObject rootObject = JsonConvert.DeserializeObject<CurrentWeather.RootObject>(x);

            // Analyze all elements in Json data
            string cityName = rootObject.name;
            string temp = rootObject.main.temp + "°C";
            string icon = rootObject.weather[0].icon;

            // text
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "CityAndTemp",$"{cityName}  {temp}"},
                { "IconURL","http://openweathermap.org/img/w/" + icon + ".png"}
            };

            return dic;
        }
    }
}
