using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SP_SanHtarWebPage.cls;
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
                    var result = await WebApiClient.Instance.SignInAsync<UserModel>("/api/User/Login/" + model.UserName+"/"+ model.Password);
                    var json = JsonConvert.SerializeObject(result.Data);
                    var userResult = JsonConvert.DeserializeObject<UserModel>(json);
                    var claims = new List<Claim>
                            {
                                //new Claim("userid", result.UserID),
                                //new Claim("name", model.Email),
                                //new Claim("fullname", result.UserName),
                                //new Claim("group", result.UserGroup),
                                //new Claim("memberid", result.MemberID),
                                //new Claim("membername", result.MemberName),
                                new Claim("ID", userResult.ID.ToString()),
                                new Claim("token",userResult.Token)
                            };
                    var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id), new AuthenticationProperties
                    {
                        //IsPersistent = model.RememberMe,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    }) ;
                    HttpContext.Session.SetString("ID", userResult.ID.ToString());
                    return await RedirectToLocal(returnUrl,true);

                }
                catch (Exception ex)
                {        
                    throw;
                }
            }
            return View(model);
        }

       
        public async Task<IActionResult> SignedOut()
        {
            var userName = User.Identity.Name;
            ViewBag.UserName = userName;
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        private async Task<ActionResult> RedirectToLocal(string returnURL = "", bool IsLogin = false)
        {
            try
            {
                if (IsLogin)
                {

                    if (!string.IsNullOrWhiteSpace(returnURL) && Url.IsLocalUrl(string.Format("{0}/", returnURL)))
                        return RedirectToRoute("Default", new { controller = returnURL, action = "Index" });
                    else
                        return  RedirectToRoute("Default", new { controller = "Registration", action = "Index" });
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
