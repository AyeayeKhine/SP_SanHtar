using SP_SanHtar.FormsVideoLibrary;
using SP_SanHtar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SP_SanHtar.Views.HomeViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailVideoPage : ContentPage
    {
        public DetailVideoPage(MyanmarModel myanmarModel)
        {
            InitializeComponent();
            videoUrl.Source = VideoSource.FromUri(myanmarModel.VideoUrl);
            lblTeacherName.Text = myanmarModel.Teachar_Name;
            lbltitle.Text = myanmarModel.Sub_Title;
        }

        private void Menu_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync(true);
        }
    }
}