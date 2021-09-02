using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SP_SanHtarWebPage.cls
{
    public class ApiException : Exception
    {
        public ApiException()
        {

        }
        public ApiException(HttpStatusCode statusCode, string jsonData)
        {
            StatusCode = statusCode;
            JsonData = jsonData;
        }

        public HttpStatusCode StatusCode { get; private set; }
        public string JsonData { get; private set; }
    }
}
