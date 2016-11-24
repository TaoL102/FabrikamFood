using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FabrikamFood.Helpers
{
    static class UIHelper
    {
        public static async Task<bool> PopUpYesOrNoDialogue(string title,string message)
        {
            var page = new ContentPage();
            return  await page.DisplayAlert(title, message, "YES", "NO");

        }

        public static async void PopUpYesDialogue(string title, string message)
        {
            var page = new ContentPage();
            await page.DisplayAlert(title, message, "YES");

        }



    }
}
