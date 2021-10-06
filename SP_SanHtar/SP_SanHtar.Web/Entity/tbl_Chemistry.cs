using SP_SanHtar.Web.ContextDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.Models
{
   
    public class tbl_Chemistry : Entity
    {
        public System.Guid ID { get; set; }
        public long ChemistryID { get; set; }
        public int Chapter { get; set; }
        public string Title { get; set; }
        public string Teachear_Name { get; set; }
        public string Photo_Path { get; set; }
        public tbl_Chemistry()
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
