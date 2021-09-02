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
    public partial class SubjectPage : ContentPage
    {
        public SubjectPage()
        {
            InitializeComponent();
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

        private void Myanamr_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Subjects.MyanmarPage());
        }
        private void English_Tapped(object sender, EventArgs e)
        {

        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
        private void Math_Tapped(object sender, EventArgs e)
        {

        }
        private void Physics_Tapped(object sender, EventArgs e)
        {

        }
        private void Chemistry_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Subjects.ChemPage());
        }
        private void Bio_Tapped(object sender, EventArgs e)
        {

        }

        private void Economics_Tapped(object sender, EventArgs e)
        {

        }
    }
}