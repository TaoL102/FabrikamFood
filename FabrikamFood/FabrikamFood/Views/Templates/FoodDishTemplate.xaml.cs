using Android;
using Android.Content;
using Android.Graphics;
using FabrikamFood.APIManagers;
using FabrikamFood.Helpers;
using FabrikamFood.Views.CustomControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FabrikamFood.Views.Templates
{
    public partial class FoodDishTemplate : ContentView
    {
        public FoodDishTemplate()
        {
            InitializeComponent();
        }

        private async void Btn_Share_Clicked(object sender, EventArgs args)
        {

            var b = (ImageTextButton)sender;
            string id = b.CommandParameter as string;

            //Reference: https://developer.xamarin.com/recipes/cross-platform/xamarin-forms/maps/map-navigation/
            switch (Device.OS)
            {
                //case TargetPlatform.iOS:
                //    Device.OpenUri(
                //      new Uri(string.Format("http://maps.apple.com/?q={0}", WebUtility.UrlEncode(dropoffPos))));
                //    break;
                case TargetPlatform.Android:

                    var sharer = DependencyService.Get<IShareable>();
                    // Get dishname
                   var dish=await AzureMobileServiceManager.Instance.GetFoodDishByIdAsync(id);
                    var content = dish.Name + ",$" + dish.Price;
                    var picName = System.IO.Path.GetFileNameWithoutExtension("PIC" + dish.PicUrl);
                    sharer.Share("Share", content, picName);

                    break;
                    //case TargetPlatform.Windows:
                    //case TargetPlatform.WinPhone:
                    //    Device.OpenUri(
                    //      new Uri(string.Format("bingmaps:?where={0}", Uri.EscapeDataString(dropoffPos))));
                    //    break;
                   
            }
            Debug.WriteLine("clicked");

        }


    }
}
