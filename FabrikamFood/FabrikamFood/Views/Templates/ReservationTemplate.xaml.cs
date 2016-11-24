using FabrikamFood.APIManagers;
using FabrikamFood.Helpers;
using FabrikamFood.Views.CustomControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FabrikamFood.Views.Template
{
    public partial class ReservationTemplate : ContentView
    {


        public ReservationTemplate()
        {
            InitializeComponent();


        }


        private void Btn_Call_Clicked(object sender, EventArgs args)
        {

            var b = (ImageTextButton)sender;
            Position t = (b.CommandParameter == null) ? new Position(0, 0) : ((Position)b.CommandParameter);

            //Reference: https://developer.xamarin.com/recipes/cross-platform/xamarin-forms/maps/map-navigation/
            switch (Device.OS)
            {
                //case TargetPlatform.iOS:
                //    Device.OpenUri(
                //      new Uri(string.Format("http://maps.apple.com/?q={0}", WebUtility.UrlEncode(dropoffPos))));
                //    break;
                case TargetPlatform.Android:
                    Device.OpenUri(
                      new Uri(string.Format("tel:1112223333")));
                    break;
                    //case TargetPlatform.Windows:
                    //case TargetPlatform.WinPhone:
                    //    Device.OpenUri(
                    //      new Uri(string.Format("bingmaps:?where={0}", Uri.EscapeDataString(dropoffPos))));
                    //    break;
            }
            Debug.WriteLine("clicked");

        }

        private void Btn_Uber_Clicked(object sender, EventArgs e)
        {
            var b = (ImageTextButton)sender;
            Position t = (b.CommandParameter == null) ? new Position(0, 0) : ((Position)b.CommandParameter);

            // Get dropoffPos string
            string dropoffPos = "dropoff[latitude] =" + t.Latitude + "dropoff[longitude]=" + t.Longitude;
            string clientId = Constants.UBER_CLIENT_ID;

            //Reference: https://developer.xamarin.com/recipes/cross-platform/xamarin-forms/maps/map-navigation/
            switch (Device.OS)
            {
                //case TargetPlatform.iOS:
                //    Device.OpenUri(
                //      new Uri(string.Format("http://maps.apple.com/?q={0}", WebUtility.UrlEncode(dropoffPos))));
                //    break;
                case TargetPlatform.Android:
                    Device.OpenUri(
                      new Uri(string.Format("uber://?action=setPickup&pickup=my_location&{0}&client_id={1}", WebUtility.UrlEncode(dropoffPos), WebUtility.UrlEncode(clientId))));
                    break;
                    //case TargetPlatform.Windows:
                    //case TargetPlatform.WinPhone:
                    //    Device.OpenUri(
                    //      new Uri(string.Format("bingmaps:?where={0}", Uri.EscapeDataString(dropoffPos))));
                    //    break;
            }
            Debug.WriteLine("clicked" + t);
        }
        private void Btn_Map_Clicked(object sender, EventArgs e)
        {
            var b = (ImageTextButton)sender;
            Position t = (b.CommandParameter == null) ? new Position(0, 0) : ((Position)b.CommandParameter);

            // Get dropoffPos string
            string pos = t.Latitude + "," + t.Longitude;

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
            Debug.WriteLine("clicked" + t);
        }
        private async void Btn_Cancel_Clicked(object sender, EventArgs e)
        {
            var b = (ImageTextButton)sender;
            string t = b.CommandParameter as string ;

            bool result=await UIHelper.PopUpYesOrNoDialogue("CANCEL RESERVATION", "Confirm to cancel this reservation?");

            if (!result) return;

            AzureMobileServiceManager.Instance.DeleteReservationByIdAsync(t);
        }
    }
}
