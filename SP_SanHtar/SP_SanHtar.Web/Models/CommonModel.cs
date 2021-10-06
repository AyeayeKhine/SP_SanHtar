using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.Models
{
    public class CommonModel
    {
        public System.Guid? ID { get; set; }
        public long CommonID { get; set; }
        public System.Guid ParentID { get; set; }
        public int Chapter { get; set; }
        public string Title { get; set; }
        public string Photo_Path { get; set; }
        public string Teachear_Name { get; set; }
        public bool isPhotoUpdate { get; set; }
    }
    public class DetailModel
    {
        public System.Guid? ID { get; set; }
        public int Part { get; set; }
        public System.Guid ParentID { get; set; }
        public long CommonID { get; set; }
        public string Title { get; set; }
        public string Teachear_Name { get; set; }
        public string Video_Path { get; set; }
        public bool isPhotoUpdate { get; set; }
    }
    public class SearchClass
    {
        public string sort { get; set; }
        public int? page { get; set; }
        public int? per_page { get; set; }
        public string filter { get; set; }
    }
    public class AssignModel
    {
        public Guid ID { get; set; }
        public string ChemistryID { get; set; }
    }
}
