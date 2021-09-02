using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SP_SanHtarWebPage.Models;

namespace SP_SanHtarWebPage.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            //LoginModel newLoginModel = new LoginModel();
            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                ViewData["ReturnUrl"] = returnUrl;
            }
            else
            {
                ViewData["ReturnUrl"] = "/Registration/Index";
            }
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                ViewData["ReturnUrl"] = returnUrl;
            }
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                //await HttpContext.SignOutAsync(Startup.ApplicationAuthenticationSchema);
                try
                {
                    //var result = await WebApiClient.Instance.SignInAsync<Models.SignInResult>(model.Email, model.Password);

                    //if (!string.IsNullOrEmpty(result.AccessToken))
                    //{

                    //}
                    return await RedirectToLocal(returnUrl,true);

                }
                catch (Exception ex)
                {        
                    throw;
                }
            }
            return View(model);
        }

        private async Task<ActionResult> RedirectToLocal(string returnURL = "", bool IsLogin = false)
        {
            try
            {
                if (IsLogin)
                {

                    if (!string.IsNullOrWhiteSpace(returnURL) && Url.IsLocalUrl(string.Format("/{0}/", returnURL)))
                        return RedirectToRoute("Default", new { controller = returnURL, action = "Index" });
                    else
                        return RedirectToRoute("Default", new { controller = "Registration", action = "Index" });
                }
                return RedirectToRoute("Default", new { controller = "Login", action = "Index" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else if (ViewData["ReturnUrl"] != null)
            {
                return Redirect(ViewData["ReturnUrl"].ToString());
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });
            }
        }

    }
}
