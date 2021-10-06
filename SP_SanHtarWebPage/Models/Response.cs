using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtarWebPage.Models
{
    //public class ResponseList<T>
    //{
    //    public string Message { get; set; }
    //    public APIStatus Status { get; set; }
    //    public Temp<T> Data { get; set; }
    //}
    public class ResponseList<T>
    {
        public string Message { get; set; }
        public APIStatus Status { get; set; }
        public int? total { get; set; }
        public int? per_page { get; set; }
        public int? current_page { get; set; }
        public int? last_page { get; set; }
        public int? from { get; set; }
        public int? to { get; set; }
        public List<T> data { get; set; }
    }    
    public class Response
    {
        public string Message { get; set; }
        public APIStatus Status { get; set; }
        public object Data { get; set; }
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
        public int Chapter { get; set; }
        public int Part { get; set; }
    }
    public class CommonDetails
    {
        public System.Guid? ID { get; set; }
        public int Part { get; set; }
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
        public Guid ChemistryID { get; set; }
        public string Teachear_Name { get; set; }
        public int Chapter { get; set; }
        public string Photo_Path { get; set; }
        public string Video_Path { get; set; }
        public bool isPhotoUpdate { get; set; }
    }
    public enum APIStatus
    {
        Successfull = 0,
        Error = 1,
        SystemError = 2,
        Warning = 3
    }   
}
