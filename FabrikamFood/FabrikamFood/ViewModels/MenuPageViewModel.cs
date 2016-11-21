using FabrikamFood.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FabrikamFood.ViewModels
{
    class MenuPageViewModel
    {
        public ICommand GoHomeCommand { get; set; }
        public ICommand GoSecondCommand { get; set; }

        public MenuPageViewModel()
        {
            GoHomeCommand = new Command(GoHome);
            GoSecondCommand = new Command(GoSecond);
        }

        void GoHome(object obj)
        {
            App.RootPage.Detail = new NavigationPage(new MainPage());
            App.MenuIsPresented = false;
        }

        void GoSecond(object obj)
        {
           // App.RootPage.Detail = new NavigationPage(new SecondPage());
            App.MenuIsPresented = false;
        }
    }
}
