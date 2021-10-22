
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SP_SanHtar.Web.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            //HttpContext context=context1.Current
            //var id = User.FindFirst("token").Value;
            //var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (context.Request.Headers.ContainsKey("Authorization") && context.Request.Headers["Authorization"][0].StartsWith("Bearer "))
            {
                var token = context.Request.Headers["Authorization"][0]
                    .Substring("Bearer ".Length);
                if (token != null)
                    attachUserToContext(context, token);
            }
            else if (context.Request.Headers.ContainsKey("Cookie") && context.Request.Headers["Cookie"][0].StartsWith(""))
            {
                var token = context.Request.Headers["Cookie"][0];
                if (token != null)
                    attachUserToContext(context, token);
            }

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Key);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer=_appSettings.Issuer,
                    ValidAudience=_appSettings.Audience,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = new TimeSpan(0,5,0)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // attach user to context on successful jwt validation
                context.Items["User"] = userId;
            }
            catch(Exception ex)
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
