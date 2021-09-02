using Acr.UserDialogs;
using GalaSoft.MvvmLight.Ioc;
using SP_SanHtar.cls;
using SP_SanHtar.CustomModels;
using SP_SanHtar.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SP_SanHtar.Views.LoginViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        protected readonly IRepository<UserModel> _repository;
        MainPage masterDetail = new MainPage();
        public ChangePasswordPage()
        {
            InitializeComponent();
            _repository = SimpleIoc.Default.GetInstance<IRepository<UserModel>>();
            
        }

        //When User Click On Current Password viewimage
        private void CurrentPassword_Tapped(object sender, EventArgs e)
        {

            if (txtpassword.IsPassword == true)
            {
                txtpassword.IsPassword = false;
                currentimage.Source = "showPassword";


            }
            else
            {
                txtpassword.IsPassword = true;
                currentimage.Source = "loginShowPassword";

            }



        }

        //When User Click On New Password viewimage
        private void NewPassword_Tapped(object sender, EventArgs e)
        {

            if (txtnewpassword.IsPassword == true)
            {
                txtnewpassword.IsPassword = false;
                newPasswordHide.Source = "showPassword";
            }
            else
            {
                txtnewpassword.IsPassword = true;
                newPasswordHide.Source = "loginShowPassword";
            }
        }

        //When User Click On Comfirm Password viewimage
        private void ComfirmPassword_Tapped(object sender, EventArgs e)
        {

            if (txtcomfirmpassword.IsPassword == true)
            {
                txtcomfirmpassword.IsPassword = false;
                confirmPasswordHide.Source = "showPassword";
            }
            else
            {
                txtcomfirmpassword.IsPassword = true;
                confirmPasswordHide.Source = "loginShowPassword";
            }
        }

        private async void Save_Tapped(object sender, EventArgs e)
        {

            if (txtcomfirmpassword.Text == txtnewpassword.Text)
            {
                if (Helpers.Settings.LoginData.Password == txtpassword.Text)
                {
                    UserDialogs.Instance.ShowLoading();
                    var requestchangepassword = new ChangePasswordModel()
                    {
                        UserName = Helpers.Settings.LoginData.UserName,
                        OldPassword = txtpassword.Text,
                        NewPassword = txtnewpassword.Text
                    };
                    var result = await WebApiClient.Instance.PostAsync<Response>("/api/User/ChangePassword/", requestchangepassword);
                    if(result.Status == 0)
                    {
                        await _repository.Update(result.Data);
                        UserDialogs.Instance.HideLoading();
                        var success = await DisplayAlert("Success", "ChangePassword Successful", "Ok", "Cancel"); // since we are using async, we should specify the DisplayAlert as awaiting.
                        if (success == true) // if it's equal to Ok
                        {
                            masterDetail.MasterBehavior = MasterBehavior.Popover;
                            masterDetail.IsPresented = false;
                            masterDetail.Detail = new Xamarin.Forms.NavigationPage(new Views.HomeViews.HomePage());
                            App.Current.MainPage = masterDetail;
                        }
                       

                    }
                }
                else
                {

                      UserDialogs.Instance.Alert("Your current password is wrong. ", "Alert", "Ok");
                }

            }
            else
            {
                UserDialogs.Instance.Alert("New Password and Confirm  Password are not match", "Alert", "Ok");
            }

        }

    }
}