using SP_SanHtar.Web.ContextDB;
using System;

namespace SP_SanHtar.Web.Models
{
    public class Tb_User : Entity
    {
        public Guid ID { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
        public bool Sex { get; set; }
        public string PersonalContactNumber { get; set; }
        public string OtherContactNumber { get; set; }
        public string PhotoUrl { get; set; }
        public byte[] SaltAes { get; set; }
        public byte[] SaltHash { get; set; }

        public Tb_User()
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
