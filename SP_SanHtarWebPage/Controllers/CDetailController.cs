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
    public class CDetailController : Controller
    {
        clsUtility utility = new clsUtility();
        private readonly IHostingEnvironment _hostingEnvironment;

        public CDetailController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }

       
        [HttpPost, ActionName("InsertChemDetail")]
        public async Task<JsonResult> PostChemDetail(string receive,IFormFile videofile)
        {
            try
            {
                //file upload process
                var files = Request.Form.Files[0];
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());
               
                //string path = Path.GetFullPath(jsData.fileName);
                //string photodata = jsData.fileName.ToString().Split(',')[1];
                //string videodata= tempData.videofileName.ToString().Split(',')[1];
                //IFormFile customer = receive["videofileName"].ToObject<IFormFile>();
                //IFormFile postedFile =jsData.videofileName.File;
                string Id = jsData.Chapter;
               
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

                var commonData = new CommonModel
                {
                    Chapter = jsData.Part,
                    Title = jsData.Title,
                    ParentID = Guid.Parse(Id),
                    Main_Title = "Part " + jsData.Part,
                    Teachar_Name = jsData.Teachar_Name,
                    //Photo_Data = photodata != "string" ? utility.SavePathFile("Myan.jpg", "UploadPhotoChemistry", photodata, _hostingEnvironment) : null,
                    Data = path +@"\"+ fileName,
                };
                var result = await WebApiClient.Instance.PostAsyncForC<ResponseList>("/api/ChemistryDetail/AddItem", commonData);
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

        [HttpGet, ActionName("GetDetailChemistryDropdown")]
        public async Task<JsonResult> GetChemistryDropdown(Guid Id)
        {

            var result = await WebApiClient.Instance.GetChemistryDetailAsync<ResponseDetailModel>("/api/ChemistryDetail/GetAllByChemistryID/" + Id, null);
            List<DropdownList> chemistyList = new List<DropdownList>();
            //chemistyList.Add(new DropdownList { ID = new Guid(), Text = "Selected" });
            foreach (var item in result.Data)
            {
                chemistyList.Add(new DropdownList { ID = item.ID, Text = item.Main_Title + "[" + item.Title +"]" });
            }

            return Json(chemistyList);
        }
    }
}
