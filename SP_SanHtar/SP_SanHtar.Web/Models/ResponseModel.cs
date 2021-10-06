using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.Models
{
    public class Response
    {
        public string Message { get; set; }
        public APIStatus Status { get; set; }
        public UserModel Data { get; set; }
    }
    public class ResponseList
    {
        public string Message { get; set; }
        public APIStatus Status { get; set; }

        public Temp Data { get; set; }
    }
    public class Temp
    {
        public int? total { get; set; }
        public int? per_page { get; set; }
        public int? current_page { get; set; }
        public int? last_page { get; set; }
        public int? from { get; set; }
        public int? to { get; set; }
        public List<UserModel> data { get; set; }
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
    public class SaveResponseModel
    {
        public string Message { get; set; }
        public APIStatus Status { get; set; }
        public object Data { get; set; }
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
        public int Part { get; set; }
        public System.Guid ParentID { get; set; }
        public long CommonID { get; set; }
        public string Main_Title { get; set; }
        public string Title { get; set; }
        public string Teachar_Name { get; set; }
        public string VideoUrl { get; set; }
    }
    public enum APIStatus
    {
        Successfull = 0,
        Error = 1,
        SystemError = 2,
        Warning =3
    }
    public class UserModel
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
        public string PartID { get; set; }
        public string ChemistryID { get; set; }
        public string ChemistryString { get; set; }
    }
}
