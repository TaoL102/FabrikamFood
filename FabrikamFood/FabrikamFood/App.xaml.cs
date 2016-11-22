
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

            if (AzureAuthManager.Instance.CurrentClient.CurrentUser == null)
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
            RestaurantList= await AzureEasyTableManager.Instance.GetRestaurantsAsync();
        }
    }
}
