using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP_SanHtar.Models
{
  public  class MyanmarModel
    {
        [PrimaryKey]
        public System.Guid ID { get; set; }
        public int Chapter { get; set; }
        public System.Guid ParentID { get; set; }
        public int typeOfSubject { get; set; }
        public long CommonID { get; set; }
        public string Main_Title { get; set; }
        public string Title { get; set; }
        public string Sub_Title { get; set; }
        public string Teachar_Name { get; set; }
        public string VideoUrl { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string DataString { get { return Data == null ? "" : System.Text.Encoding.UTF8.GetString(Data); } }
        public string Photo_Name { get; set; }
        public string Photo_ContentType { get; set; }
        public byte[] Photo_Data { get; set; }
        public string Photo_DataString { get { return Photo_Data == null ? "" : System.Text.Encoding.UTF8.GetString(Photo_Data); } }
       
    }
}
