using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabrikamFood.DataModels
{
    class UberEstimate
    {
        public class UberEstimatedPrice
        {
            public string localized_display_name { get; set; }
            public double distance { get; set; }
            public string display_name { get; set; }
            public string product_id { get; set; }
            public double? high_estimate { get; set; }
            public double? low_estimate { get; set; }
            public int duration { get; set; }
            public string estimate { get; set; }
            public string currency_code { get; set; }
        }

        public class UberEstimatedPriceRootObject
        {
            public List<UberEstimatedPrice> prices { get; set; }
        }

        public class UberEstimatedTime
        {
            public string localized_display_name { get; set; }
            public int estimate { get; set; }
            public string display_name { get; set; }
            public string product_id { get; set; }
        }

        public class UberEstimatedTimeRootObject
        {
            public List<UberEstimatedTime> times { get; set; }
        }
    }
}
