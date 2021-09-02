using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using SP_SanHtar.CustomModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP_SanHtar.Helpers
{
  public  class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static UserModel LoginData
        {
            get
            {
                string value = AppSettings.GetValueOrDefault(App.loginModel, string.Empty);
                UserModel userData;
                if (string.IsNullOrEmpty(value))
                    userData = new UserModel();
                else
                    userData = JsonConvert.DeserializeObject<UserModel>(value);
                return userData;
            }
            set
            {
                string userDataValue = JsonConvert.SerializeObject(value);
                AppSettings.AddOrUpdateValue(App.loginModel, userDataValue);
            }
        }

        //public static string CkPassword
        //{
        //    get => AppSettings.GetValueOrDefault(App.ckPassword, string.Empty);
        //    set => AppSettings.AddOrUpdateValue(App.ckPassword, value);
        //}
    }
}
