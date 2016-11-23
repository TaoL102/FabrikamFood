using FabrikamFood.APIManagers;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FabrikamFood.Views
{
    /// <summary>
    /// Authentication
    /// Reference: https://github.com/NZMSA/2016-Phase-2/tree/master/Training/Day%203/3.5%20Facebook%20Authentication 
    ///            https://docs.microsoft.com/en-us/azure/app-service-mobile/app-service-mobile-xamarin-forms-get-started-users
    /// </summary>
    public partial class LoginPage : ContentPage
    {
        bool authenticated = false;

        public LoginPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (authenticated == true)
            {
                // Save to property dictionary 
                App.SaveUserId(AzureMobileServiceManager.Instance.CurrentClient.CurrentUser.UserId);

                Navigation.InsertPageBefore(new HomePage(), this);
                App.RootPage.Master.IsVisible = true;
                await Navigation.PopAsync();

            }
        }

        async void Btn_Google_Login_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);



        }

        async void Btn_Facebook_Login_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Facebook);



        }
    }
}
