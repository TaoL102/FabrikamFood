using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabrikamFood.DataModels
{
    public class Category
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }


        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
    }
}
