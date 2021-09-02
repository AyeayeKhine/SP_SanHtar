using SP_SanHtar.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace SP_SanHtar.ViewModels
{
    public class TutorialViewModel : BaseViewModel
    {
        public ObservableCollection<TutorialModel> appmodel = new ObservableCollection<TutorialModel>()
        {
            new TutorialModel(){tuotrial_Image ="icon.png"},
           //new TutorialModel(){tuotrial_Image="Adverise2.png"},
           //new TutorialModel(){tuotrial_Image="Adverise3.png"},
           //new TutorialModel(){tuotrial_Image="Adverise1.png"},
           //new TutorialModel(){tuotrial_Image="aboutLearn.png"},
            new TutorialModel(){tuotrial_Image ="sicon.png"},
            new TutorialModel(){tuotrial_Image="aboutLearn.png"},

        };
        public TutorialViewModel()
        {
            NewsList = appmodel;
        }

        private ObservableCollection<TutorialModel> _NewsList;
        public ObservableCollection<TutorialModel> NewsList
        {
            get { return _NewsList; }
            set
            {
                if (_NewsList != value)
                {
                    _NewsList = value;
                    OnPropertyChanged("NewsList");
                }
            }
        }

        public async Task GetNewsList()
        {
            NewsList = appmodel;
        }
    }
}
