using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtarWebPage.Controllers
{
    public class BaseController : Controller
    {
        public string CurrentBearToken
        {
            get
            {
                string token = "";
                if (User.Identity.IsAuthenticated)
                {
                    token = User.FindFirst("token").Value;
                }
                return token;
            }
        }
    }
}
