using Acr.UserDialogs;
using GalaSoft.MvvmLight.Ioc;
using Plugin.Connectivity;
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

namespace SP_SanHtar.Views.Subjects
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChemDetailPage : ContentPage
    {
        protected readonly IRepository<ResponseDetailModel> _repository;
        public System.Guid ParentID { get; set; }
        string url;
        string Teacher_Name;
        public ChemDetailPage(CommomModels commom)
        {
            InitializeComponent();
            var nav = new NavigationPage(new ContentPage { Title = "Details434" });
            nav.BarBackgroundColor = Color.DarkBlue;
            _repository = SimpleIoc.Default.GetInstance<IRepository<ResponseDetailModel>>();
            ParentID = commom.ID;
            url = WebApiClient.UrlApi + commom.PhotoUrl;
            Photourl.Source = ImageSource.FromUri(new Uri(url));
            lblTeacher.Text = commom.Teacher_Name;
            Teacher_Name = commom.Teacher_Name;
        }

        protected async override void OnAppearing()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var result = await WebApiClient.Instance.GetListAsyncDetail<ResponseDetailModel>("/api/ChemistryDetail/GetAllByChemistryID/" + ParentID);
                    //chapterList.ItemsSource = (result.Data ?? new List<CommonDetails>()).ToList();
                    chapterList.FlowItemsSource = null;
                    chapterList.FlowItemsSource = (result.Data ?? new List<CommonDetails>()).ToList();
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
                // Console
                Console.WriteLine(string.Format("Exception: {0}", ex.Message));
                Console.WriteLine(string.Format("StackTrace: {0}", ex.StackTrace));

            }

        }

        async void OnTap(object sender, ItemTappedEventArgs e)
        {
            try
            {
                CommomModels model = e.Item as CommomModels;

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message, "Alert", "OK");
            }


        }
        async void OnRefresh(object sender, EventArgs e)
        {
            var list = (Xamarin.Forms.ListView)sender;
            var result = await WebApiClient.Instance.GetListAsync<ResponseModel>("/api/Myanmar/GetAll", null);
            //myanmarList.ItemsSource = (result.Data ?? new List<CommomModels>()).ToList();
            list.IsRefreshing = false;
        }

        private void Video_Tapped(object sender, EventArgs e)
        {
            var responseDetailModel = (sender as Grid).BindingContext as CommonDetails;
            responseDetailModel.Photo_Name = url;
            responseDetailModel.Teachar_Name = Teacher_Name;
            Navigation.PushAsync(new Views.Subjects.ChemVideoPage(responseDetailModel));

        }
    }
}