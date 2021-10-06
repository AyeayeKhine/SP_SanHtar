using SP_SanHtar.Web.ContextDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.Models
{
    public class tbl_Assign : Entity
    {
        public Guid ID { get; set; }
        public long AssignID { get; set; }
        public Guid UserID { get; set; }
        public Guid ChapterID { get; set; }
        public string Title { get; set; }
        public string Teachear_Name { get; set; }
        public string Photo_Path { get; set; }
        public int Chapter { get; set; }
        public tbl_Assign()
        {
            this.ID = GenerateNewIdentity();
            CreatedBy = "admin";
            CreatedDate = DateTime.Now;
            UpdatedBy = "admin";
            UpdatedDate = DateTime.Now;
            Active = true;
            Enabled = true;
        }
    }
}
