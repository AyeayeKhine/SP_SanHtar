using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtarWebPage.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class ChangePasswordModel
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class UserModel
    {
        //[PrimaryKey]
        public Guid? ID { get; set; }
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
        public string PartID { get; set; }
        public string ChemistryID { get; set; }
        public string ChemistryString { get; set; }
        public string Token { get; set; }
    }
}
