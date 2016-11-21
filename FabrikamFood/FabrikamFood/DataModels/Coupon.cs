using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FabrikamFood.DataModels
{
    class Coupon
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "ApplicableRestaurantId")]
        public string ApplicableRestaurantId { get; set; }

        [JsonProperty(PropertyName = "CategoryName")]
        public string CategoryName { get; set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Discount")]
        public int Discount { get; set; }

        [JsonProperty(PropertyName = "ExpireDate")]
        public DateTime ExpireDate { get; set; }
    }
}
