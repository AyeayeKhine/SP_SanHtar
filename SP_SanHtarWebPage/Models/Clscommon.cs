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
        public int Part { get; set; }
        public string Title { get; set; }
        public string Teachear_Name { get; set; }
        public int Chapter { get; set; }
        public string Photo_Path { get; set; }
        public string Video_Path { get; set; }
    }

    public class DropdownList
    {
        public Guid ID { get; set; }
        public string Text { get; set; }
    }
}
