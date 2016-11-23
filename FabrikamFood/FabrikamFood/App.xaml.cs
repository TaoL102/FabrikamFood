
using FabrikamFood.APIManagers;
using FabrikamFood.DataModels;
using FabrikamFood.Views;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FabrikamFood
{
    public static class Constants
    {
        #region API key and gateway URLs
        public static string APPLICATION_URL = "https://msafabrikamfood.azurewebsites.net";
        public static string WEATHER_API_KEY = "6a9c4821930e1eeb29ab3511220b6102";
        public static string GOOGLE_MAPS_API_KEY = "AIzaSyAGDpbFjrhzH7NPQ7Omb7u9uFbFVkneXcQ";
        public static string UBER_SERVER_TOKEN = "t-Fz6_aiHMhOvlc73xCN1Ft93-BL8OgHzq_abh-P";
        public static string UBER_CLIENT_ID = "blAhtOJ_kNwjrPjSL7KC-rZanl5T-TV2";
        #endregion

        #region MISC
        public static string MISC_UBERX_GUID = "a0eb34b7-c910-4fc7-8582-8180b9378349";
        #endregion

        #region DimentionValues

        public static int  LISTVIEW_CELL_HEIGHT_RESERVATION = 160;
        public static int LISTVIEW_CELL_HEIGHT_COUPON = 130;
        public static int LISTVIEW_CELL_SPACING = 10;

        #endregion
    }

    public interface IAuthenticate
        {
            Task<bool> Authenticate(MobileServiceAuthenticationProvider authProvider);
        Task<bool> LogoutAsync();

    }

    public partial class App : Application
    {

        public static NavigationPage NavigationPage { get; private set; }
        public static RootPage RootPage;
        public static IAuthenticate Authenticator { get; private set; }
        public static List<Restaurant> RestaurantList { get; private set; }
        public static string UserID { get; set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }      

        public static bool MenuIsPresented
        {
            get
            {
                return RootPage.IsPresented;
            }
            set
            {
                RootPage.IsPresented = value;
            }
        }

        public App()
        {
 var menuPage = new DrawerPage();
            RootPage = new RootPage();
            RootPage.Master = menuPage;

            if (App.GetUserId()==null)
            {
                NavigationPage = new NavigationPage(new LoginPage());
                RootPage.Master.IsVisible = false;
            }
            else
            {
                NavigationPage = new NavigationPage(new HomePage());
            }
           
            RootPage.Detail = NavigationPage;
            MainPage = RootPage;

            // Initialize 
            InitRestaurantList();
        }


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private async void InitRestaurantList()
        {
            RestaurantList= await AzureMobileServiceManager.Instance.GetRestaurantsAsync();
        }

        public async static void SaveUserId(string id)
        {
                Application.Current.Properties["userid"] = id;
            await Application.Current.SavePropertiesAsync();
        }

        public static string GetUserId()
        {
            if (Application.Current.Properties.ContainsKey("userid"))
            {
return Application.Current.Properties["userid"] as string;
            }
            return null;
        }
    }
}
