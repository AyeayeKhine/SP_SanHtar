using SP_SanHtar.Web.ContextDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.Models
{

    public class Tb_Chemistry_Detail : Entity
    {
        public System.Guid ID { get; set; }
        public long ChemistryDetailID { get; set; }
        public System.Guid ChemistryID { get; set; }
        public int Part { get; set; }
        public string Title { get; set; }
        public string Teachear_Name { get; set; }
        public string Video_Path { get; set; }

        public Tb_Chemistry_Detail()
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
