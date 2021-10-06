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
    public class ChemistryController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ChemistryController(IHostingEnvironment environment)
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

        [HttpPost, ActionName("InsertChem")]
        public async Task<JsonResult> PostUser(string receive)
        {
            try
            {
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());
                string path = "";
                string fileName = "";
                bool isPhotoupdate = false;
                if (Request.ContentType !=null && Request.Form.Files.Count > 0)
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
                    isPhotoupdate = true;
                }

                var commonData = new CommonModel
                {
                    ID = jsData.ID,
                    Chapter = jsData.Chapter,
                    Title = jsData.Title,
                    Teachear_Name = jsData.Teachar_Name,
                    Photo_Path = path,
                    isPhotoUpdate= isPhotoupdate
                };
                var result = await WebApiClient.Instance.PostAsync<Response, CommonModel>("/api/Chemistry/AddItem", commonData);
                if (result.Status == APIStatus.Successfull)
                {
                    return Json(new
                    {
                        status = "2", //PASS
                        result.Data
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

        [HttpGet,ActionName("GetChemistryDropdown")]
        public async Task<JsonResult> GetChemistryDropdown()
        {

            var result = await WebApiClient.Instance.GetChemistryAsync<ResponseModel>("/api/Chemistry/GetAll", null);
            List<DropdownList> chemistyList = new List<DropdownList>();
            if(result.Data !=null && result.Data.Count() > 0)
            {
                foreach (var item in result.Data)
                {
                    chemistyList.Add(new DropdownList { ID = item.ID, Text = item.Description });
                }
            }
            return Json(chemistyList);
        }

        [HttpGet, ActionName("GetAllChemistry")]
        public async Task<JsonResult> GetAllChemistry(TableSearchClass tempData)
        {

            try
            {
                //==============================================Query============================================================
                var result = await WebApiClient.Instance.PostAsyncForList<ResponseList<CommonModel>,CommonModel>("/api/Chemistry/GetChemistry", tempData);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet, ActionName("GetAllRecoverChemistry")]
        public async Task<JsonResult> GetAllRecoverChemistry(TableSearchClass tempData)
        {

            try
            {
                //==============================================Query============================================================
                var result = await WebApiClient.Instance.PostAsyncForList<ResponseList<CommonModel>, CommonModel>("/api/Chemistry/RecoverChemistry", tempData);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost, ActionName("GetbyId")]
        public async Task<JsonResult> GetbyId(string receive)
        {
            try
            {
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());


                var commonData = new CommonModel
                {
                    ID = jsData.ID
                };
                var result = await WebApiClient.Instance.PostAsync<Response, CommonModel>("/api/Chemistry/GetbyID", commonData);
                if (result.Status == APIStatus.Successfull)
                {
                    return Json(new
                    {
                        status = "2",//Pass
                        result.Data
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

        [HttpPost, ActionName("DeleteChemistry")]
        public async Task<JsonResult> DeleteUser(string receive)
        {
            try
            {
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());
                var commonData = new CommomModels
                {
                    ID = jsData.ID
                };
                var result = await WebApiClient.Instance.PostAsync<Response, CommomModels>("/api/Chemistry/DeleteChemistry", commonData);
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

        [HttpPost, ActionName("RestoreChemistry")]
        public async Task<JsonResult> RestoreChemistry(string receive)
        {
            try
            {
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());
                var commonData = new CommomModels
                {
                    ID = jsData.ID,
                    Chapter=jsData.Chapter
                    
                };
                var result = await WebApiClient.Instance.PostAsync<Response, CommomModels>("/api/Chemistry/RestoreChemistry", commonData);
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
