using Microsoft.AspNetCore.Http;
using System;

namespace SP_SanHtarWebPage.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class ImportClass
    {
        public string receive { get; set; }   // import
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
