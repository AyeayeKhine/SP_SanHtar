using Microsoft.AspNetCore.Http;
using System;

namespace SP_SanHtarWebPage.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    //public class ImportClass
    //{
    //    public string receive { get; set; }   // import
    //    public IFormFile videofileName { get; set; }   // import
    //}

    public class ImportClass
    {
        public string receive { get; set; }   // import
        public string Chapter { get; set; }   // import
        public string Part { get; set; }   // import
        public string Sub_title { get; set; }   // import
        public string Teachar_Name { get; set; }   // import
        public IFormFile fileName { get; set; }   // import
        public IFormFile videofileName { get; set; }   // import
    }


    public class TableSearchClass
    {
        public string sort { get; set; }
        public int? page { get; set; }
        public int? per_page { get; set; }
        public string filter { get; set; }
    }
}
