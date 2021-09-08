using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SP_SanHtarWebPage.cls;
using SP_SanHtarWebPage.Models;

namespace SP_SanHtarWebPage.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public RegistrationController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("InsertUser")]
        public async Task<JsonResult> PostUser(string receive)
        {
            try
            {
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());
                var files = Request.Form.Files[0];
                string wwwPath = this._hostingEnvironment.WebRootPath;
                string contentPath = this._hostingEnvironment.ContentRootPath;

                string path = Path.Combine(this._hostingEnvironment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(files.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    files.CopyTo(stream);
                }
                var commonData = new UserModel
                {
                    FirstName = jsData.FirstName,
                    LastName = jsData.LastName,
                    UserType = jsData.UserType,
                    UserName = jsData.UserName,
                    Password = jsData.Password,
                    Email = jsData.Email,
                    PersonalContactNumber = jsData.PersonalContactNumber,
                    OtherContactNumber = jsData.OtherContactNumber,
                    Sex = jsData.Sex == "1" ? false : true,
                    PartID = jsData.PartID,
                    ChemistryID = jsData.ChemistryID,
                    PhotoUrl = path + @"\" + fileName,
                };
                var result = await WebApiClient.Instance.PostAsyncForR<ResponseList>("/api/User/AddUser", commonData);
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
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
           
        }
    }
}
