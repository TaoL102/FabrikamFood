using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Droid;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using FabrikamFood.APIManagers;
using Android.Webkit;
using FabrikamFood.Helpers;

namespace FabrikamFood.Droid
{

    [Activity(Label = "FabrikamFood", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        // Define a authenticated user.
        private MobileServiceUser user;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);

            CachedImageRenderer.Init();

            // Initialize the authenticator before loading the app.
            App.Init((IAuthenticate)this);

            LoadApplication(new App());
        }

        private async void InitAuth()
        {
            if (!await Authenticate(MobileServiceAuthenticationProvider.Google))
            {
              await  Authenticate(MobileServiceAuthenticationProvider.Facebook);
            }
        }

        public async Task<bool> Authenticate(MobileServiceAuthenticationProvider authProvider)
        {
            var success = false;
            var message = string.Empty;
            try
            {
                if (authProvider.Equals(MobileServiceAuthenticationProvider.Google))
                {
                    // Sign in with Google login using a server-managed flow.
                user = await AzureMobileServiceManager.Instance.CurrentClient.LoginAsync(this,
                    MobileServiceAuthenticationProvider.Google);

                }
                else
                {
                    // Sign in with Facebook login using a server-managed flow.
                    user = await AzureMobileServiceManager.Instance.CurrentClient.LoginAsync(this,
                        MobileServiceAuthenticationProvider.Facebook);
                }

                if (user != null)
                {
                                        Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                    Settings.UserId = user?.UserId ?? string.Empty;

                    message = string.Format("you are now signed-in as {0}.",
                        user.UserId);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
         //   CreateAndShowDialog(message, "Sign-in result");

            return success;
        }


        public async Task<bool> LogoutAsync()
        {
            bool success = false;
            try
            {
                if (user != null)
                {
                    CookieManager.Instance.RemoveAllCookie();
                    await AzureMobileServiceManager.Instance.CurrentClient.LogoutAsync();
                    CreateAndShowDialog(string.Format("You are now logged out - {0}", user.UserId), "Logged out!");
                }
                user = null;
                success = true;
            }
            catch (Exception ex)
            {
                CreateAndShowDialog(ex.Message, "Logout failed");
            }

            return success;
        }

        void CreateAndShowDialog(string message, string title)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.SetNeutralButton("OK", (sender, args) => {
            });
            builder.Create().Show();
        }
    }
}

