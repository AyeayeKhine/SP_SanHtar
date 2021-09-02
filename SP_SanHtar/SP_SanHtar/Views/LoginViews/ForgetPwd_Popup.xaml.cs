using Acr.UserDialogs;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using SP_SanHtar.cls;
using SP_SanHtar.CustomModels;
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
    public partial class ForgetPwd_Popup : PopupPage
    {
        public ForgetPwd_Popup()
        {
            InitializeComponent();
            FrameContainer.WidthRequest = Xamarin.Forms.Application.Current.MainPage.Width - 40;
        }

        //When User Click On Cancel 
        private void Cancel_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAllPopupAsync(true);
        }
        //When User Click On Confirm 
        private async void Confirm_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    
                    var result = await WebApiClient.Instance.SignInAsync<Response>("/api/User/ForgetPassword/" + txtUserName.Text );
                    if(result.Status == 0)
                    {
                       await Navigation.PopAllPopupAsync(true);
                    }
                    else
                    {
                        UserDialogs.Instance.Alert(result.Message, "Alert", "OK");
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Alert("Please Check Your Connection!!!", "Alert", "OK");
                }
            }
            catch (Exception ex)
            {
                //await Navigation.PushPopupAsync(new CheckInternetPage());
            }

        }
    }
}