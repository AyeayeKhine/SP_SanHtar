﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.cls
{
  public  interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
