using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SP_SanHtarWebPage.cls;
using SP_SanHtarWebPage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtarWebPage.Controllers
{
    public class ChemistryDetailController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ChemistryDetailController(IHostingEnvironment environment)
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

        [HttpPost, ActionName("InsertChemDetail")]
        public async Task<JsonResult> PostChemDetail(string receive)
        {
            try
            {
                //file upload process
                //var files = Request.Form.Files[0];
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());
                string Id = jsData.Chapter;

                string path = "";
                string fileName = "";
                bool isVideoUpdate = false;
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
                    isVideoUpdate = true;
                }
                var commonData = new CommonModel
                {
                    ID = jsData.ID,
                    Part = jsData.Part,
                    Title = jsData.Title,
                    ParentID = Guid.Parse(Id),
                    Teachear_Name = jsData.Teachar_Name,
                    Video_Path = path ,
                    isPhotoUpdate = isVideoUpdate
                };
                var result = await WebApiClient.Instance.PostAsync<Response,CommonModel>("/api/ChemistryDetail/AddItem", commonData);
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

        [HttpGet, ActionName("GetDetailChemistryDropdown")]
        public async Task<JsonResult> GetChemistryDropdown(Guid Id)
        {

            var result = await WebApiClient.Instance.GetChemistryDetailAsync<ResponseDetailModel>("/api/ChemistryDetail/GetAllByChemistryID/" + Id, null);
            List<DropdownList> chemistyList = new List<DropdownList>();
            if(result.Data !=null && result.Data.Count() > 0)
            {
                foreach (var item in result.Data)
                {
                    chemistyList.Add(new DropdownList { ID =Guid.Parse(item.ID.ToString()), Text = "Part "+item.Part });
                }
            }
            return Json(chemistyList);
        }

        [HttpGet, ActionName("GetAllChemistryDetail")]
        public async Task<JsonResult> GetAllChemistryDetail(TableSearchClass tempData)
        {

            try
            {
                //==============================================Query============================================================
                var result = await WebApiClient.Instance.PostAsyncForList<ResponseList<CommonDetails>, CommonDetails>("/api/ChemistryDetail/GetChemistryDetail", tempData);
                return Json(result);
            }
            catch (Exception)
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


                var commonData = new CommonDetails
                {
                    ID = jsData.ID
                };
                var result = await WebApiClient.Instance.PostAsync<Response, CommonDetails>("/api/ChemistryDetail/GetbyID", commonData);
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

        [HttpPost, ActionName("DeleteChemistryDetail")]
        public async Task<JsonResult> DeleteChemistryDetail(string receive)
        {
            try
            {
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());
                var commonData = new CommonDetails
                {
                    ID = jsData.ID
                };
                var result = await WebApiClient.Instance.PostAsync<Response, CommonDetails>("/api/ChemistryDetail/DeleteChemistryDetail", commonData);
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

        [HttpPost, ActionName("RestoreChemistryDetail")]
        public async Task<JsonResult> RestoreChemistryDetail(string receive)
        {
            try
            {
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());
                var commonData = new CommonDetails
                {
                    ID = jsData.ID,
                    ParentID=jsData.ChemistryID,
                    Part=jsData.Part
                };
                var result = await WebApiClient.Instance.PostAsync<Response, CommonDetails>("/api/ChemistryDetail/RestoreChemistryDetail", commonData);
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

        [HttpGet, ActionName("GetAllRecoverChemistryDetail")]
        public async Task<JsonResult> GetAllRecoverChemistryDetail(TableSearchClass tempData)
        {

            try
            {
                //==============================================Query============================================================
                var result = await WebApiClient.Instance.PostAsyncForList<ResponseList<CommonDetails>, CommonDetails>("/api/ChemistryDetail/RecoverChemistryDetail", tempData);
                return Json(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
