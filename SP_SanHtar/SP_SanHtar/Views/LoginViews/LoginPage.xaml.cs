using GalaSoft.MvvmLight.Ioc;
using Plugin.Connectivity;
using SP_SanHtar.cls;
using SP_SanHtar.Interfaces;
using SP_SanHtar.CustomModels;
using SP_SanHtar.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Extensions;

namespace SP_SanHtar.Views.LoginViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        TutorialViewModel viewModel = new TutorialViewModel();
        bool Continue = true;
        bool checkConnection = false;
        protected readonly IRepository<UserModel> _repository;
        MainPage masterDetail = new MainPage();
        public LoginPage()
        {
            InitializeComponent();
            
            viewModel = new TutorialViewModel();
            this.BindingContext = viewModel;
            BindingContext = this;
            _repository = SimpleIoc.Default.GetInstance<IRepository<UserModel>>();
            masterDetail.Master = new Views.MenuPage();
            if (CrossConnectivity.Current.IsConnected)
            {
                checkConnection = true;
            }
            else
            {
                checkConnection = false;
            }
        }

        protected async override void OnAppearing()
        {
            try
            {
                await viewModel.GetNewsList();
                NewsList.ItemsSource = viewModel.NewsList;
                NewsListIndi.ItemsSource = viewModel.NewsList;
                //Device.StartTimer(TimeSpan.FromSeconds(2),() =>
                //{

                //    NewsList.Position = (imgSlider.Position + 1) % imageSources.Count;

                //    return true;
                //});
                Device.StartTimer(new TimeSpan(0, 0, 10), () =>
                {

                    NewsList.Position = (NewsList.Position + 1) % viewModel.NewsList.Count;
                    return true; //true to continuous, false to single use
                });
                Connectivity.ConnectivityChanged += OnConnectivityChanged;

            }
            catch (Exception ex)
            {
                // Console
                Console.WriteLine(string.Format("Exception: {0}", ex.Message));
                Console.WriteLine(string.Format("StackTrace: {0}", ex.StackTrace));

            }

        }

        protected async override void OnDisappearing()
        {
            Connectivity.ConnectivityChanged -= OnConnectivityChanged;

            base.OnDisappearing();
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if(e.NetworkAccess.ToString() == "Internet")
            {
                checkConnection = true;
            }
            else
            {
                checkConnection = false;
                DisplayAlert("Netwrok", "No Inernet Access", "OK");
            }
             
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            UserModel userModel = await _repository.GetUser(Username.Text, Password.Text);
            if (userModel != null)
            {
                // await Navigation.PushAsync(new Views.HomeViews.HomePage());
                Helpers.Settings.LoginData = userModel;
                masterDetail.MasterBehavior = MasterBehavior.Popover;
                masterDetail.IsPresented = false;
                masterDetail.Detail = new Xamarin.Forms.NavigationPage(new Views.HomeViews.HomePage());
                App.Current.MainPage = masterDetail;
            }
            else
            {
                if (checkConnection)
                {
                    var result = await WebApiClient.Instance.SignInAsync<ResponseModel>("/api/User/Login/" + Username.Text + "/"+ Password.Text);
                    if(result.Status == 0)
                    {
                        userModel = result.Data;
                        _repository.Insert(userModel);
                        Helpers.Settings.LoginData = userModel;
                        masterDetail.MasterBehavior = MasterBehavior.Popover;
                        masterDetail.IsPresented = false;
                        masterDetail.Detail = new Xamarin.Forms.NavigationPage(new Views.HomeViews.HomePage());
                        App.Current.MainPage = masterDetail;
                    }
                    else
                    {
                        DisplayAlert(result.Message, "Error", "OK");
                    }
                  
                }
                else
                {
                    DisplayAlert("Netwrok", "No Inernet Access", "OK");
                }
            }
            
        }
        private async void FingerPrint_Tapped(object sender, EventArgs e)
        {

        }
        private async void FaceId_Tapped(object sender, EventArgs e)
        {

        }
        private void Password_Tapped(object sender, EventArgs e)
        {
            if (Password.IsPassword == true)
            {
                Password.IsPassword = false;
                passwordimage.Source = "showPassword";//Show

            }
            else
            {
                Password.IsPassword = true;
                passwordimage.Source = "loginShowPassword";//Hide
            }
        }
        private async void ForgetPassword_Tapped(object sender, EventArgs e)
        {
           await Navigation.PushPopupAsync(new Views.LoginViews.ForgetPwd_Popup());
        }

        private int _position;
        public int Position { get { return _position; } set { _position = value; OnPropertyChanged(); } }

        //Disable BackButton
        protected override bool OnBackButtonPressed()
        {
            //return true;
            if (Navigation.NavigationStack.Count > 0)
            {
                var NavPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 1];
                if (NavPage.IsBusy == false)
                    return true;
                else
                    return base.OnBackButtonPressed();
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }
    }
}