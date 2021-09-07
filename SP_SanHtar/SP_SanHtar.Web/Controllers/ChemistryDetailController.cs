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
    public class ChemistryDetailController : ControllerBase
    {
        clsUtility utility = new clsUtility();
        private readonly IHostingEnvironment _hostingEnvironment;
        private byte[] videoBytes;
        private byte[] imageBytes;

        public ChemistryDetailController(IHostingEnvironment environment)
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
                    if (item.Data != null && item.Data != "string")
                    {
                        //videoBytes = System.Text.Encoding.UTF8.GetBytes(item.Data);
                        videoBytes =await utility.FileToByteArray(item.Data);
                    }
                    if (item.Photo_Data != null && item.Photo_Data != "string")
                    {
                        imageBytes = System.Text.Encoding.UTF8.GetBytes(item.Photo_Data);
                        //imageBytes = utility.FileToByteArray(item.Photo_Data);
                    }
                    var chemistryitem = db.Tb_Chemistrys.Where(i => i.ID == item.ParentID && i.Active == true && i.Enabled == true).FirstOrDefault();
                    var detailitem = db.Tb_Chemistry_Details.Where(i => i.ChemistryID == item.ParentID && i.Part == item.Chapter && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if (detailitem != null)
                    {
                        return Ok(new ResponseModel { Message = "Already have Part" + item.Chapter, Status = APIStatus.Warning });
                    }
                    else
                    {
                        var dataitem = new Tb_Chemistry_Detail
                        {
                            Part = item.Chapter,
                            ChemistryID = item.ParentID,
                            Main_Title = chemistryitem.Main_Title,
                            Sub_Title = item.Sub_Title,
                            Title = item.Title,
                            Teachar_Name = item.Teachar_Name,
                            Name = item.Data != "string" ? utility.SavePathFile("Myan.mp4", "UploadChemistry", Convert.ToBase64String(videoBytes), _hostingEnvironment) : null,
                            //Data = videoBytes,
                            //Photo_Name = item.Photo_Data != "string" ? utility.SavePathFile("Myan.jpg", "UploadPhotoChemistry", Convert.ToBase64String(imageBytes), _hostingEnvironment) : null,
                        };
                        db.Tb_Chemistry_Details.Add(dataitem);
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

        [HttpGet("GetAllByChemistryID/{ID}")]
        public async Task<IActionResult> GetAllByMyanmmarID(Guid ID)
        {
            OnlineContext db = new OnlineContext();
            try
            {
                string query = "";
                query = "Select * from [SPSH_Database].[dbo].[Tb_Myanmar]";
                var itemList = await db.Tb_Chemistry_Details.Where(i => i.ChemistryID == ID).ToListAsync();
                var dataList = new List<CommonDetails>();
                foreach (var item in itemList)
                {
                    dataList.Add(new CommonDetails { ID = item.ID, Title = item.Title, Sub_Title = item.Sub_Title, VideoUrl = item.Name, ParentID = item.ChemistryID, Chapter = item.Part });
                }
                //List<Tb_Myanamr> item = db.Tb_Myanamrs.FromSql(query).OrderBy(b => b.CreatedDate).ToList();
                return Ok(new ResponseDetailModel { Message = "Save Successful", Status = APIStatus.Successfull, Data = dataList });
                //return Ok(item);

            }
            catch (Exception ex)
            {
                return Ok(new ResponseDetailModel { Message = ex.Message, Status = APIStatus.Error, Data = null });
            }


        }
    }
}
