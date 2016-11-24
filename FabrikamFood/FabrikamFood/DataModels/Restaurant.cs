using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabrikamFood.DataModels
{
    public class Restaurant
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "Latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "Longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "Phone")]
        public string Phone { get; set; }
    }
}
