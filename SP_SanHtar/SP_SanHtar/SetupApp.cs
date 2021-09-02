using Autofac;
using GalaSoft.MvvmLight.Ioc;
using SP_SanHtar.Interfaces;
using SP_SanHtar.CustomModels;
using SP_SanHtar.Services;
using SP_SanHtar.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using SP_SanHtar.Models;

namespace SP_SanHtar
{
   public class SetupApp
    {
        private static SetupApp instance;
        /// <summary>
        /// This is a singleton instance for bootstraping the application.
        /// </summary>
        /// <value>The instance.</value>
        public static SetupApp Instance
        {
            get
            {
                if (instance == null)
                    instance = new SetupApp();

                return instance;
            }
        }

        public IContainer CreateContainer()
        {
            ContainerBuilder cb = new ContainerBuilder();

            Setup();

            return cb.Build();
        }

        /// <summary>
        /// Setup all injections
        /// </summary>
        public void Setup()
        {
            SimpleIoc.Default.Register<IDownloadService, DownloadService>();
            SimpleIoc.Default.Register<DownloadViewModel>();
            SimpleIoc.Default.Register<IRepository<UserModel>, Repository<UserModel>>();
            SimpleIoc.Default.Register<IRepository<MyanmarModel>, Repository<MyanmarModel>>();
            SimpleIoc.Default.Register<IRepository<ResponseDetailModel>, Repository<ResponseDetailModel>>();
        }

       
    }
}
