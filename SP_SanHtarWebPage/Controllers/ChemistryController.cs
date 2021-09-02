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
    public class ChemistryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost, ActionName("InsertChem")]
        public async Task<JsonResult> PostUser([FromBody] ImportClass tempData)
        {
            try
            {
                dynamic jsData = JsonConvert.DeserializeObject(tempData.receive);
                var commonData = new CommonModel
                {
                    Chapter = jsData.Chapter,
                    Title = jsData.Title,
                    Main_Title = "Chapter " + jsData.Chapter,
                    Teachar_Name = jsData.Teachar_Name,
                    Photo_Data = jsData.fileName,
                    Data = "string",
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
            //chemistyList.Add(new DropdownList { ID = new Guid(), Text = "Selected" });
            foreach (var item in result.Data)
            {
                chemistyList.Add(new DropdownList { ID = item.ID, Text = item.Description });
            }
            
            return Json(chemistyList);
        }
    }
}
