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

        public IActionResult CustomIndex()
        {
            return View();
        }

        [HttpPost, ActionName("InsertUser")]
        public async Task<JsonResult> PostUser(string receive)
        {
            try
            {
                string path = "";
                string fileName = "";
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());
                if (Request.ContentType != null && Request.Form.Files.Count > 0)
                {
                    var files = Request.Form.Files[0];
                    string wwwPath = this._hostingEnvironment.WebRootPath;
                    string contentPath = this._hostingEnvironment.ContentRootPath;

                    path = Path.Combine(this._hostingEnvironment.WebRootPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    fileName = Path.GetFileName(files.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        files.CopyTo(stream);
                    }
                    path = path + @"\" + fileName;
                }
                var commonData = new UserModel
                {
                    ID=jsData.ID,
                    FirstName = jsData.FirstName,
                    LastName = jsData.LastName,
                    UserType = jsData.UserType,
                    UserName = jsData.UserName,
                    Password = jsData.Password,
                    Email = jsData.Email,
                    PersonalContactNumber = jsData.PersonalContactNumber,
                    OtherContactNumber = jsData.OtherContactNumber,
                    Sex = jsData.Sex,
                    PartID = jsData.PartID,
                    ChemistryID = jsData.ChemistryID,
                    PhotoUrl = path,
                };
                var result = await WebApiClient.Instance.PostAsync<Response, UserModel>("/api/User/AddUser", commonData);
                if (result.Status == APIStatus.Successfull)
                {
                    return Json(new
                    {
                        status = "2", //PASS
                        result.Data
                    }) ;
                }
                else
                {
                    throw new Exception(result.Message);
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "3", //FAIL
                    message = ex.Message
                });
            }
        }


        [HttpGet, ActionName("GetAllUser")]
        public async Task<JsonResult> GetAllUser(TableSearchClass tempData)
        {

            try
            {
                //==============================================Query============================================================
                var result = await WebApiClient.Instance.PostAsyncForList<ResponseList<UserModel>,UserModel>("/api/User/GetAllUser", tempData);
               return Json(result);
            }
            catch (Exception ex)
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

        [HttpPost, ActionName("GetbyId")]
        public async Task<JsonResult> GetbyId(string receive)
        {
            try
            {
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());


                var commonData = new UserModel
                {
                    ID = jsData.ID
                };
                var result = await WebApiClient.Instance.PostAsync<Response, UserModel>("/api/User/GetbyID", commonData);
                if (result.Status == APIStatus.Successfull)
                {
                    return Json(new
                    {
                        status = "2",//Pass
                        result.Data
                    }) ;
                }
                else
                {
                    throw new Exception(result.Message);
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "3", //FAIL
                    message = ex.Message
                });
            }
        }

        [HttpPost, ActionName("DeleteUser")]
        public async Task<JsonResult> DeleteUser(string receive)
        {
            try
            {
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());
                var commonData = new UserModel
                {
                    ID = jsData.ID
                };
                var result = await WebApiClient.Instance.PostAsync<Response, UserModel>("/api/User/DeleteUser", commonData);
                if (result.Status == APIStatus.Successfull)
                {
                    return Json(new
                    {
                        status = "2",//Pass
                    });
                }
                else
                {
                    throw new Exception(result.Message);
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "3", //FAIL
                    message = ex.Message
                });
            }
        }

        [HttpGet, ActionName("GetAllRecoverUser")]
        public async Task<JsonResult> GetAllRecoverUser(TableSearchClass tempData)
        {

            try
            {
                //==============================================Query============================================================
                var result = await WebApiClient.Instance.PostAsyncForList<ResponseList<UserModel>, UserModel>("/api/User/RecoverUser", tempData);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost, ActionName("RestoreUser")]
        public async Task<JsonResult> RestoreUser(string receive)
        {
            try
            {
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());
                var commonData = new UserModel
                {
                    ID = jsData.ID,
                    UserName=jsData.UserName
                };
                var result = await WebApiClient.Instance.PostAsync<Response, UserModel>("/api/User/RestoreUser", commonData);
                if (result.Status == APIStatus.Successfull)
                {
                    return Json(new
                    {
                        status = "2",//Pass
                    });
                }
                else
                {
                    throw new Exception(result.Message);
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "3", //FAIL
                    message = ex.Message
                });
            }
        }
    }
}
