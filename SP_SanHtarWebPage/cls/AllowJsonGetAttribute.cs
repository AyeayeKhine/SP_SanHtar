using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SP_SanHtarWebPage.cls
{
    public class AllowJsonGetAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var jsonResult = filterContext.Result as JsonResult;

            if (jsonResult == null)
                throw new ArgumentException("Action does not return a JsonResult, attribute AllowJsonGet is not allowed");


            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            base.OnResultExecuting(filterContext);
        }
    }
}
