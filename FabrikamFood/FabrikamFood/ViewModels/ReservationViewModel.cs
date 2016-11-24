using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace FabrikamFood.ViewModels
{
    class ReservationViewModel
    {
        public string ID { get; set; }
        public string RestaurantID { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }
        public Position RestaurantPosition { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string RestaurantPhone { get; set; }


    }
}
