using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SP_SanHtar.Services;
using SP_SanHtar.Views;
using SP_SanHtar.Interfaces;
using SP_SanHtar.CustomModels;
using System.ComponentModel;

namespace SP_SanHtar
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        //To debug on Android emulators run the web backend against .NET Core not IIS
        //If using other emulators besides stock Google images you may need to adjust the IP address
        public static string AzureBackendUrl =
            DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
        public static bool UseMockDataStore = true;
        protected readonly IRepository<UserModel> _businessCode;
        private static IContainer _container;
        public static string loginModel = "loginModel";
        public App()
        {
            InitializeComponent();

            if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<AzureDataStore>();

            //MainPage = new MainPage();
            //SetupApp appSetup = new SetupApp();
            //_container = appSetup.CreateContainer();
            
        }

        protected override void OnStart()
        {
            SetupApp.Instance.Setup();
            MainPage = new NavigationPage(new Views.LoginViews.LoginPage());
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
