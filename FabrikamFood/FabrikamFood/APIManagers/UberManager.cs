using FabrikamFood.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using static FabrikamFood.DataModels.UberEstimate;

namespace FabrikamFood.APIManagers
{
    class UberManager
    {
        private static readonly UberManager instance = new UberManager();

        private UberManager()
        { }

        public static UberManager Instance
        {
            get
            {
                return instance;
            }
        }

        public async Task<string> GetEstimatedPrice(Position startPos,Position endPos)
        {

            // API call parameters
            string apiKey = "server_token="+ Constants.UBER_SERVER_TOKEN;
            string url = "https://api.uber.com/v1.2/estimates/price?";
            string startPosStr = $"start_latitude={startPos.Latitude}&start_longitude={startPos.Longitude}";
            string endPosStr = $"end_latitude={endPos.Latitude}&end_longitude={endPos.Longitude}";

            // Httpclient
            HttpClient client = new HttpClient();

            //test
            string test = String.Format("{0}{1}&{2}&{3}", url, startPosStr, endPosStr, apiKey);

            // Get Json

            string x = await client.GetStringAsync(new Uri(String.Format("{0}{1}&{2}&{3}", url, startPosStr, endPosStr, apiKey)));

            // Deserialize
            UberEstimate.UberEstimatedPriceRootObject rootObject = JsonConvert.DeserializeObject<UberEstimate.UberEstimatedPriceRootObject>(x);

            // Get UBER_X data
            var priceObject = rootObject.prices.Where(o => o.product_id.Equals(Constants.MISC_UBERX_GUID)).FirstOrDefault();
            if (priceObject == null) return null;
            return priceObject.estimate+" for UberX";
        }

        public async Task<string> GetEstimatedTime(Position startPos, Position endPos)
        {

            // API call parameters
            string apiKey = "server_token=" + Constants.UBER_SERVER_TOKEN;
            string url = "https://api.uber.com/v1.2/estimates/time?";
            string startPosStr = $"start_latitude={startPos.Latitude}&start_longitude={startPos.Longitude}";
            string endPosStr = $"end_latitude={endPos.Latitude}&end_longitude={endPos.Longitude}";

            // Httpclient
            HttpClient client = new HttpClient();

            // Get Json
            string x = await client.GetStringAsync(new Uri(String.Format("{0}{1}&{2}&{3}", url, startPosStr, endPosStr, apiKey)));

            // Deserialize
            UberEstimate.UberEstimatedTimeRootObject rootObject = JsonConvert.DeserializeObject<UberEstimate.UberEstimatedTimeRootObject>(x);

            // Get UBER_X data
            var timeObject = rootObject.times.Where(o => o.product_id.Equals(Constants.MISC_UBERX_GUID)).FirstOrDefault();
            if (timeObject == null) return null;
            return Convert.ToInt32(timeObject.estimate/60)+" MIN AWAY";
        }
    }

}
