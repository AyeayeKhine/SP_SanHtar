using Acr.UserDialogs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using SP_SanHtar.Interfaces;
using SP_SanHtar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SP_SanHtar.ViewModels
{
    public class DownloadViewModel : ViewModelBase
    {
        public MyanmarModel itemData;
        protected readonly IRepository<MyanmarModel> _repository;
       
        private double _progressValue;
        /// <summary>
        /// Gets or sets the progress value.
        /// </summary>
        /// <value>The progress value.</value>
        public double ProgressValue
        {
            get { return _progressValue; }
            set { Set(ref _progressValue, value); }
        }

        private bool _isDownloading;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:XFDownloadProject.ViewModels.DownloadViewModel"/>
        /// is downloading.
        /// </summary>
        /// <value><c>true</c> if is downloading; otherwise, <c>false</c>.</value>
        public bool IsDownloading
        {
            get { return _isDownloading; }
            set { Set(ref _isDownloading, value); }
        }

        /// <summary>
        /// The download service.
        /// </summary>
        private readonly IDownloadService _downloadService;

        /// <summary>
        /// Gets the start download command.
        /// </summary>
        /// <value>The start download command.</value>
        public ICommand StartDownloadCommand { get; }

        public DownloadViewModel(IDownloadService downloadService)
        {
            _downloadService = downloadService;
            _repository = SimpleIoc.Default.GetInstance<IRepository<MyanmarModel>>();
            StartDownloadCommand = new RelayCommand(async () => await StartDownloadAsync());
        }

        /// <summary>
        /// Starts the download async.
        /// </summary>
        /// <returns>The download async.</returns>
        public async Task StartDownloadAsync()
        {
            var progressIndicator = new Progress<double>(ReportProgress);
            var cts = new CancellationTokenSource();
            try
            {
                IsDownloading = true;
                itemData = Helpers.Constants.CommonData;

                //var url = "https://archive.org/download/BigBuckBunny_328/BigBuckBunny_512kb.mp4";
                var downloadData = await _repository.GetMyanmar(itemData.ID);
                if (downloadData != null)
                {
                    IsDownloading = false;
                    UserDialogs.Instance.Alert("Already Download Data", "Alert", "OK");
                }
                else
                {
                    var url = Helpers.Constants.videoUrl;
                    await _downloadService.DownloadFileAsync(url, progressIndicator, cts.Token);
                    SaveFile(itemData);
                }

            }
            catch (OperationCanceledException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                //Manage cancellation here
            }
            finally
            {
                IsDownloading = false;
            }
        }

        private async void SaveFile(MyanmarModel myanmarmodel)
        {
            myanmarmodel.VideoUrl = Helpers.Constants.videoUrl;
           await _repository.Insert(myanmarmodel);
        }

        /// <summary>
        /// Reports the progress status for the downlaod.
        /// </summary>
        /// <param name="value">Value.</param>
        internal void ReportProgress(double value)
        {
            ProgressValue = value;
        }
    }
}
