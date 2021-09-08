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


        [HttpPost, ActionName("InsertChem")]
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
                var commonData = new CommonModel
                {
                    Chapter = jsData.Chapter,
                    Title = jsData.Title,
                    Teachear_Name = jsData.Teachar_Name,
                    Photo_Path = path + @"\" + fileName,
                };
                var result = await WebApiClient.Instance.PostAsyncForC<ResponseList>("/api/Chemistry/AddItem", commonData);
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
    }
}
