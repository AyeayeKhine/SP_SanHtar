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


        [HttpPost, ActionName("InsertChemDetail")]
        public async Task<JsonResult> PostChemDetail(string receive)
        {
            try
            {
                //file upload process
                var files = Request.Form.Files[0];
                dynamic jsData = JsonConvert.DeserializeObject(receive.ToString());
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
                    Part = jsData.Part,
                    Title = jsData.Title,
                    ParentID = Guid.Parse(Id),
                    Teachear_Name = jsData.Teachar_Name,
                    Video_Path = path + @"\" + fileName,
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
            if(result.Data !=null && result.Data.Count() > 0)
            {
                foreach (var item in result.Data)
                {
                    chemistyList.Add(new DropdownList { ID = item.ID, Text = "Part "+item.Part });
                }
            }
            return Json(chemistyList);
        }
    }
}
