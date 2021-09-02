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
        public string Main_Title { get; set; }
        public string Title { get; set; }
        public string Sub_Title { get; set; }
        public string Teachar_Name { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public string DataString { get { return Data == null ? "" : System.Text.Encoding.UTF8.GetString(Data); } }
        public string Photo_Name { get; set; }
        public string Photo_ContentType { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public byte[] Photo_Data { get; set; }
        public string Photo_DataString { get { return Photo_Data == null ? "" : System.Text.Encoding.UTF8.GetString(Photo_Data); } }

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
