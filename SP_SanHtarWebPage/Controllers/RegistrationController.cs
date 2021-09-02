using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SP_SanHtarWebPage.cls;
using SP_SanHtarWebPage.Models;

namespace SP_SanHtarWebPage.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //[AllowJsonGet]
        [HttpPost, ActionName("InsertUser")]
        public async Task<JsonResult> PostUser([FromBody]ImportClass tempData)
        {
            try
            {
                dynamic jsData = JsonConvert.DeserializeObject(tempData.receive);
                return Json(new
                {
                    status = "2" //PASS
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "3" //FAIL
                });
            }
        }


        [HttpGet, ActionName("GetAllUser")]
        public async Task<JsonResult> GetAllUser(TableSearchClass tempData)
        {

            try
            {
                //==============================================Query============================================================
                var result = await WebApiClient.Instance.PostAsync<ResponseList>("/api/User/GetAllUser", tempData);
                return Json(result.Data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetChapter()
        {
            try
            {
                var result = await WebApiClient.Instance.GetListAsync<ResponseModel>("/api/Chemistry/GetAll", null);
                //var resultTemp1 = result.Data.li
                //       .Select((item, index) => new
                //       {
                //           Text = item.ProductName,
                //           Value = item.Price

                //       });

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
           
        }
    }
}
