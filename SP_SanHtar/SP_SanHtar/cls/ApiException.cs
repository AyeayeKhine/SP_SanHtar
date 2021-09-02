using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SP_SanHtar.cls
{
  public  class ApiException : Exception
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
