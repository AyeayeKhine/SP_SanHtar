using GalaSoft.MvvmLight.Ioc;
using SP_SanHtar.cls;
using SP_SanHtar.CustomModels;
using SP_SanHtar.FormsVideoLibrary;
using SP_SanHtar.Models;
using SP_SanHtar.ViewModels;
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
    public partial class ChemVideoPage : ContentPage
    {
        clsUtility utility = new clsUtility();
        public ChemVideoPage(CommonDetails responseDetailModel)
        {
            InitializeComponent();
            string url = WebApiClient.UrlApi + responseDetailModel.VideoUrl;
            videoUrl.Source = VideoSource.FromUri(url);
            Helpers.Constants.videoUrl = url;
            var myanmarModel = new MyanmarModel
            {
                ID= responseDetailModel.ID,
                CommonID=responseDetailModel.CommonID,
                ParentID= responseDetailModel.ParentID,
                Main_Title= responseDetailModel.Main_Title,
                Sub_Title= responseDetailModel.Sub_Title,
                Title= responseDetailModel.Title,
                Chapter= responseDetailModel.Chapter,
                typeOfSubject=(int)TypeOfSubject.Chemistry,
                VideoUrl= responseDetailModel.VideoUrl,
                Teachar_Name= responseDetailModel.Teachar_Name,
                Photo_Name=responseDetailModel.Photo_Name,
                Photo_Data= responseDetailModel.Photo_Name != "string" ? System.Text.Encoding.UTF8.GetBytes(responseDetailModel.Photo_Name) : null,

            };
            //DownloadViewModel downloadViewModel = new DownloadViewModel(myanmarModel);
            //this.BindingContext = downloadViewModel;
            Helpers.Constants.CommonData = myanmarModel;
            this.BindingContext = SimpleIoc.Default.GetInstance<DownloadViewModel>();
        }
    }
}