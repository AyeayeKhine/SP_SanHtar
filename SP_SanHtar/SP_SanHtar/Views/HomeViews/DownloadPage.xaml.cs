using GalaSoft.MvvmLight.Ioc;
using SP_SanHtar.ViewModels;
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
    public partial class DownloadPage : ContentPage
    {
        public DownloadPage()
        {
            InitializeComponent();
            this.BindingContext = SimpleIoc.Default.GetInstance<DownloadViewModel>();
        }

        private void Menu_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync(true);
        }
    }
}