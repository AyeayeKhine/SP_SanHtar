using Acr.UserDialogs;
using GalaSoft.MvvmLight.Ioc;
using SP_SanHtar.Interfaces;
using SP_SanHtar.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SP_SanHtar.Views.HomeViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        MasterDetailPage masterDetailPage = new MasterDetailPage();
        List<MyanmarModel> InProgressList = new List<MyanmarModel>();
        protected readonly IRepository<MyanmarModel> _repository;
        List<clsSubject> typeofSubjecList = new List<clsSubject>();
        public HomePage()
        {
            InitializeComponent();
            _repository = SimpleIoc.Default.GetInstance<IRepository<MyanmarModel>>();
            typeofSubjecList.Add(new clsSubject { typeOfSubject = 7, nameofSubject = "ALL" });
            typeofSubjecList.Add(new clsSubject { typeOfSubject = (int)TypeOfSubject.Myanamr,nameofSubject= TypeOfSubject.Myanamr.ToString() });
            typeofSubjecList.Add(new clsSubject { typeOfSubject = (int)TypeOfSubject.English, nameofSubject = TypeOfSubject.English.ToString() });
            typeofSubjecList.Add(new clsSubject { typeOfSubject = (int)TypeOfSubject.Mathematics, nameofSubject = TypeOfSubject.Mathematics.ToString() });
            typeofSubjecList.Add(new clsSubject { typeOfSubject = (int)TypeOfSubject.Physics, nameofSubject = TypeOfSubject.Physics.ToString() });
            typeofSubjecList.Add(new clsSubject { typeOfSubject = (int)TypeOfSubject.Chemistry, nameofSubject = TypeOfSubject.Chemistry.ToString() });
            typeofSubjecList.Add(new clsSubject { typeOfSubject = (int)TypeOfSubject.Biology, nameofSubject = TypeOfSubject.Biology.ToString() });
            typeofSubjecList.Add(new clsSubject { typeOfSubject = (int)TypeOfSubject.Economics, nameofSubject = TypeOfSubject.Economics.ToString() });
            subjectList.ItemsSource = null;
            subjectList.ItemsSource = typeofSubjecList;
            BindingData();
        }

        private async void BindingData()
        {
            var result = await _repository.GetAllMyanmar();
            //foreach (var item in result)
            //{
            //    await _repository.DeleteById(item.ID);
            //}
            //foreach (var item in result)
            //{
            //    item.Source = Xamarin.Forms.ImageSource.FromStream(
            //        () => new MemoryStream(Convert.FromBase64String(Helpers.Settings.TLoginData.UserImage)));
            //}
            InProgressList = result;
           
            DataList.ItemsSource = InProgressList;
        }

        private void Menu_Tapped(object sender, EventArgs e)
        {
            //Navigation.PopAsync(true);
            // masterDetailPage.IsPresented = true;
            if (App.Current.MainPage is MasterDetailPage mdp)
            {
                mdp.IsPresented = true;
            }
        }

        private void CheckIN_Tapped(object sender, EventArgs e)
        {
            var myanmarModel = (sender as Grid).BindingContext as MyanmarModel;
            Navigation.PushAsync(new Views.HomeViews.DetailVideoPage(myanmarModel));
        }
        private async void OnTap(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var myanmarModel = e.Item  as MyanmarModel;
                Navigation.PushAsync(new Views.HomeViews.DetailVideoPage(myanmarModel));
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message, "Alert", "OK");
            }

        }

        private void Download_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.HomeViews.DownloadPage());
        }

        private async void Subject_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Xamarin.Forms.Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                lblleave.Text = typeofSubjecList[selectedIndex].nameofSubject.ToString();
                int subjecttype = typeofSubjecList[selectedIndex].typeOfSubject;
                subjectList.SelectedItem = "";
                if(subjecttype == 7)
                {
                    var result = await _repository.GetAllMyanmar();
                    InProgressList = result;
                }
                else
                {
                    var result = await _repository.GetTypeOfSubject(subjecttype);
                    InProgressList = result;
                }
                
                DataList.ItemsSource = InProgressList;
            }
        }

        async void OnRefresh(object sender, EventArgs e)
        {
            var list = (Xamarin.Forms.ListView)sender;
            var result = await _repository.GetAllMyanmar();
            InProgressList = result;
            DataList.ItemsSource = InProgressList;
            list.IsRefreshing = false;
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            Guid Id =Guid.Parse(mi.CommandParameter.ToString());
            await _repository.DeleteById(Id);
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            var normalizedQuery = searchBar.Text?.ToLower() ?? "";
            var result= await _repository.GetSearchResults(normalizedQuery);
            DataList.ItemsSource = result; 
        }
    }
}