
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace SP_SanHtarWebPage.cls
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
      
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string redirectUrl = "/Login/Index";

            if (context.HttpContext.User.Identity.IsAuthenticated == false)
            {
                if (context.HttpContext.Session.GetString("ID") == null)
                {
                    context.HttpContext.Response.StatusCode = 401;

                    //result is returned to AJAX call and user is redirected to sign in page
                    JsonResult jsonResult = new JsonResult(new { message = "Unauthorized", redirectUrl = redirectUrl });
                    context.Result = new ForbidResult();
                    return;
                }
                else
                {
                    context.Result = new RedirectResult(redirectUrl);
                }
            }
        }
    }
}
