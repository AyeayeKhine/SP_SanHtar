using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace SP_SanHtar.Web.Helpers
{
    public class AuthenticationHelper
    {
        public static void ConfigureService(IServiceCollection service, string Issuer, string Audience, string Key)
        {
            service
               .AddAuthentication(o => {
                   o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                   o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
               })
                .AddJwtBearer(o =>
                {
                    //o.RequireHttpsMetadata = false;
                    //o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,

                        ValidIssuer = Issuer,
                        ValidAudience = Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key))
                    };
                });
        }
    }
}
