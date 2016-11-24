using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FabrikamFood.Helpers
{
    class DataHelper
    {
        public static async void SaveToPropertyDictionary(string propertyName, object value)
        {
            Application.Current.Properties[propertyName] = value;
            await Application.Current.SavePropertiesAsync();
        }


        public static object GetFromPropertyDictionary(string propertyName)
        {
            if (Application.Current.Properties.ContainsKey(propertyName))
            {
                return Application.Current.Properties[propertyName];
            }
            return null;
        }
    }
}
