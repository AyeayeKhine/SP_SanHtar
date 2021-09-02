using System;
using System.Collections.Generic;
using System.Text;

namespace SP_SanHtar.Models
{
    public enum MenuItemType
    {
        HomePage,
        Subject,
        Setting,
        About,
        Password,
        Logout
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }
        public string Title { get; set; }
        public string ImgSource { get; set; }
    }
}
