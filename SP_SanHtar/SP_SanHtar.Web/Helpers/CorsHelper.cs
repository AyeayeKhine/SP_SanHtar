using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.Helpers
{
    public class CorsHelper
    {
        public static void ConfigureService(IServiceCollection service)
        {
            service.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization, "x-custom-header");
            }));
        }
    }
}
