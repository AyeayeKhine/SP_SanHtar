using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SP_SanHtar.Web.cls;
using SP_SanHtar.Web.ContextDB;
using SP_SanHtar.Web.Models;

namespace SP_SanHtar.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyanmarController : ControllerBase
    {
        clsUtility utility = new clsUtility();
        private readonly IHostingEnvironment _hostingEnvironment;
        private byte[] videoBytes;
        private byte[] imageBytes;

        public MyanmarController(IHostingEnvironment environment)
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
                    if(item.Data !=null && item.Data != "string")
                    {
                         //videoBytes = System.Text.Encoding.UTF8.GetBytes(item.Data);
                        videoBytes = await utility.FileToByteArray(item.Data);
                    }
                    if (item.Photo_Data != null && item.Photo_Data != "string")
                    {
                        //imageBytes = System.Text.Encoding.UTF8.GetBytes(item.Photo_Data);
                        imageBytes = await utility.FileToByteArray(item.Photo_Data);
                    }

                    var dataitem = new Tb_Myanamr
                    {
                        Main_Title = item.Main_Title,
                        Sub_Title = item.Sub_Title,
                        Title = item.Title,
                        Teachar_Name = item.Teachar_Name,
                        Name = item.Data != "string" ? utility.SavePathFile("Myan.mp4","UploadMyanmar", Convert.ToBase64String(videoBytes), _hostingEnvironment):null,
                        //Data = videoBytes,
                        Photo_Name = item.Photo_Data != "string" ? utility.SavePathFile("Myan.jpg", "UploadPhotoMyanamr", Convert.ToBase64String(imageBytes), _hostingEnvironment): null,
                    };
                    db.Tb_Myanamrs.Add(dataitem);
                    db.SaveChanges();
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
                var itemList = await db.Tb_Myanamrs.ToListAsync();
                var dataList = new List<CommomModels>();
                foreach(var item in itemList){
                    dataList.Add(new CommomModels { ID = item.ID, Title = item.Title, Description = item.Main_Title, PhotoUrl = item.Photo_Name,Teacher_Name=item.Teachar_Name});
                }
                //List<Tb_Myanamr> item = db.Tb_Myanamrs.FromSql(query).OrderBy(b => b.CreatedDate).ToList();
                return Ok(new ResponseModel { Message = "Save Successful", Status = APIStatus.Successfull, Data = dataList });
                //return Ok(item);

            }
            catch(Exception ex)
            {
                return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.Error, Data = null });
            }


        }

        
    }
}
