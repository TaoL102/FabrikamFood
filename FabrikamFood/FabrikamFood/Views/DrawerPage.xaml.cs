using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FabrikamFood.ViewModels;

using Xamarin.Forms;

namespace FabrikamFood.Views
{
    public partial class DrawerPage : ContentPage
    {
        public DrawerPage()
        {
            BindingContext = new MenuPageViewModel();
            Title = "Menu";
            Icon = Device.OS == TargetPlatform.iOS ? "menu.png" : null;
            InitializeComponent();
        }
    }
}
