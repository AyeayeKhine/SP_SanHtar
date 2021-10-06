using SP_SanHtar.Web.ContextDB;
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SP_SanHtar.Web.Models
{
    public class tbl_User : Entity
    {
        public Guid ID { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
        public string Sex { get; set; }
        public string PersonalContactNumber { get; set; }
        public string OtherContactNumber { get; set; }
        public string PhotoUrl { get; set; }
        public byte[] SaltAes { get; set; }
        public byte[] SaltHash { get; set; }
        public string PartID { get; set; }
        public string ChemistryID { get; set; }   

        public tbl_User()
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
