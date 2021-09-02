using System;
using System.Collections.Generic;
using System.Text;

namespace SP_SanHtar.Models
{
   public class CommonModel
    {
        public System.Guid ID { get; set; }
        public long CommonID { get; set; }
        public string Main_Title { get; set; }
        public string Title { get; set; }
        public string Sub_Title { get; set; }
        public string Teachar_Name { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Data { get; set; }
        public string Photo_Name { get; set; }
        public string Photo_ContentType { get; set; }
        public string Photo_Data { get; set; }
    }

    public enum TypeOfSubject
    {
        Myanamr = 0,
        English = 1,
        Mathematics = 2,
        Physics = 3,
        Chemistry = 4,
        Biology = 5,
        Economics = 6
    }

    public class clsSubject
    {
        public int typeOfSubject { get; set; }
        public string nameofSubject { get; set; }
    }
}
