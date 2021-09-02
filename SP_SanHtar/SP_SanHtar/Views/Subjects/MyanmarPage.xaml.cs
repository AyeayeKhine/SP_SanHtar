using Acr.UserDialogs;
using GalaSoft.MvvmLight.Ioc;
using Plugin.Connectivity;
using SP_SanHtar.cls;
using SP_SanHtar.CustomModels;
using SP_SanHtar.Interfaces;
using SP_SanHtar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SP_SanHtar.Views.Subjects
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyanmarPage : ContentPage
    {
        protected readonly IRepository<MyanmarModel> _repository;
        public MyanmarPage()
        {
            InitializeComponent();
            _repository = SimpleIoc.Default.GetInstance<IRepository<MyanmarModel>>();
            UserDialogs.Instance.ShowLoading();
        }

        protected async override void OnAppearing()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var result = await WebApiClient.Instance.GetListAsync<ResponseModel>("/api/Myanmar/GetAll",null);

                    myanmarList.ItemsSource = (result.Data ?? new List<CommomModels>()).ToList();
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Alert("Please Check Your Connection !!!", "Alert", "OK");
                }

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message, "Alert", "OK");
            }

        }
        async void OnTap(object sender, ItemTappedEventArgs e)
        {
            try
            {
                CommomModels model = e.Item as CommomModels;
                await Navigation.PushAsync(new Views.Subjects.MyanDetailPage(model));              
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message, "Alert", "OK");
            }


        }
        async  void OnRefresh(object sender, EventArgs e)
        {
            var list = (Xamarin.Forms.ListView)sender;
            var result = await WebApiClient.Instance.GetListAsync<ResponseModel>("/api/Myanmar/GetAll", null);
            myanmarList.ItemsSource = (result.Data ?? new List<CommomModels>()).ToList();
            list.IsRefreshing = false;
        }

    }
}