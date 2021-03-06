using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP_SanHtar.CustomModels
{
    public class UserModel
    {
        [PrimaryKey]
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
    }
    public class Response
    {
        public string Message { get; set; }
        public APIStatus Status { get; set; }
        public UserModel Data { get; set; }
    }
    public class ResponseModel
    {
        public string Message { get; set; }
        public APIStatus Status { get; set; }
        public List<CommomModels> Data { get; set; }
    }
    public class ResponseDetailModel
    {
        public string Message { get; set; }
        public APIStatus Status { get; set; }
        public List<CommonDetails> Data { get; set; }
    }
    public class CommomModels
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public string Teacher_Name { get; set; }
    }
    public class CommonDetails
    {
        public System.Guid ID { get; set; }
        public int Chapter { get; set; }
        public System.Guid ParentID { get; set; }
        public long CommonID { get; set; }
        public string Main_Title { get; set; }
        public string Title { get; set; }
        public string Sub_Title { get; set; }
        public string Teachar_Name { get; set; }
        public string VideoUrl { get; set; }
        public string ContentType { get; set; }
        public string Data { get; set; }
        public string Photo_Name { get; set; }
        public string Photo_ContentType { get; set; }
        public string Photo_Data { get; set; }
    }
    public enum APIStatus
    {
        Successfull = 0,
        Error = 1,
        SystemError = 2,
        Warning = 3
    }

    public class ChangePasswordModel
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }


}
