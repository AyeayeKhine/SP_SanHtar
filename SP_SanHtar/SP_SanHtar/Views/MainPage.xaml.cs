using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SP_SanHtar.Models;

namespace SP_SanHtar.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

           // MenuPages.Add((int)MenuItemType.HomePage, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.HomePage:
                        MenuPages.Add(id, new NavigationPage(new Views.HomeViews.HomePage()));
                        break;
                    case (int)MenuItemType.Subject:
                        MenuPages.Add(id, new NavigationPage(new Views.Subjects.ChemPage()));
                        break;
                    case (int)MenuItemType.Setting:
                        MenuPages.Add(id, new NavigationPage(new Views.HomeViews.HomePage()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                    case (int)MenuItemType.Password:
                        MenuPages.Add(id, new NavigationPage(new Views.LoginViews.ChangePasswordPage()));
                        break;
                    case (int)MenuItemType.Logout:
                        MenuPages.Add(id, new NavigationPage(new Views.LoginViews.LoginPage()));
                        break;
                }
            }
           

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}