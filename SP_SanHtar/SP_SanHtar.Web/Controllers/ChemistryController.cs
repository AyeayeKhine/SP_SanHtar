using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SP_SanHtar.Web.cls;
using SP_SanHtar.Web.ContextDB;
using SP_SanHtar.Web.Models;

namespace SP_SanHtar.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChemistryController : ControllerBase
    {
        clsUtility utility = new clsUtility();
        private readonly IHostingEnvironment _hostingEnvironment;
        private byte[] videoBytes;
        private byte[] imageBytes;

        public ChemistryController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> Create(CommonModel item)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                   
                    if (item.Photo_Path != null && item.Photo_Path != "string")
                    {
                        imageBytes = await utility.FileToByteArray(item.Photo_Path);
                    }
                    var chemistryitem = db.Tb_Chemistrys.Where(i => i.Chapter == item.Chapter && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if (chemistryitem != null)
                    {
                        return Ok(new ResponseModel { Message = "Already have Chapter" + item.Chapter, Status = APIStatus.Warning });
                    }
                    else
                    {
                        var dataitem = new Tb_Chemistry
                        {
                            Chapter = item.Chapter,
                            Title = item.Title,
                            Teachear_Name = item.Teachear_Name,
                            Photo_Path = item.Photo_Path != "string" ? utility.SavePathFile("Chem", "UploadPhotoChemistry", Convert.ToBase64String(imageBytes), _hostingEnvironment,".jpg") : null,
                        };
                        db.Tb_Chemistrys.Add(dataitem);
                        db.SaveChanges();
                    }
                   
                }
                catch (Exception ex)
                {
                    return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.Error });
                }

            }
            return Ok(new ResponseModel { Message = "Save Successful", Status = APIStatus.Successfull, Data = null });
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            OnlineContext db = new OnlineContext();
            try
            {
                string query = "";
                query = "Select * from [SPSH_Database].[dbo].[Tb_Myanmar]";
                var itemList = await db.Tb_Chemistrys.OrderBy(i => i.Chapter).ToListAsync();
                var dataList = new List<CommomModels>();
                foreach (var item in itemList)
                {
                    dataList.Add(new CommomModels { ID = item.ID,Description="Chapter "+item.Chapter, Title = item.Title, PhotoUrl = item.Photo_Path, Teacher_Name = item.Teachear_Name });
                }
                return Ok(new ResponseModel { Message = "Save Successful", Status = APIStatus.Successfull, Data = dataList });

            }
            catch (Exception ex)
            {
                return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.Error, Data = null });
            }
        }

    }
}
