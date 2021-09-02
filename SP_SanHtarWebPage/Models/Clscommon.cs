using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtarWebPage.Models
{
    public class Clscommon
    {
    }

    public class CommonModel
    {
        public System.Guid ID { get; set; }
        public long CommonID { get; set; }
        public System.Guid ParentID { get; set; }
        public int Chapter { get; set; }
        public string Main_Title { get; set; }
        public string Title { get; set; }
        public string Sub_Title { get; set; }
        public string Teachar_Name { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Data { get; set; }
        public string Photo_Name { get; set; }
        public string Photo_ContentType { get; set; }
        public string Photo_Data { get; set; }
    }

    public class DropdownList
    {
        public Guid ID { get; set; }
        public string Text { get; set; }
    }
}
