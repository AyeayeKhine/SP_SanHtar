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
        public async Task<IActionResult> Create(DetailModel item)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                    if (item.Video_Path != null && item.Video_Path != "string")
                    {
                        videoBytes =await utility.FileToByteArray(item.Video_Path);
                    }
                   
                    var chemistryitem = db.Tb_Chemistrys.Where(i => i.ID == item.ParentID && i.Active == true && i.Enabled == true).FirstOrDefault();
                    var detailitem = db.Tb_Chemistry_Details.Where(i => i.ChemistryID == item.ParentID && i.Part == item.Part && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if (detailitem != null)
                    {
                        return Ok(new ResponseModel { Message = "Already have Part" + item.Part, Status = APIStatus.Warning });
                    }
                    else
                    {
                        var dataitem = new Tb_Chemistry_Detail
                        {
                            Part = item.Part,
                            ChemistryID = item.ParentID,
                            Title = item.Title,
                            Teachear_Name = item.Teachear_Name,
                            Video_Path = item.Video_Path != "string" ? utility.SavePathFile("Chem", "UploadChemistry", Convert.ToBase64String(videoBytes), _hostingEnvironment,".mp4") : null
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
        public async Task<IActionResult> GetAllByChemistryID(Guid ID)
        {
            OnlineContext db = new OnlineContext();
            try
            {
                var itemList = await db.Tb_Chemistry_Details.Where(i => i.ChemistryID == ID).ToListAsync();
                var dataList = new List<CommonDetails>();
                foreach (var item in itemList)
                {
                    dataList.Add(new CommonDetails { ID = item.ID, Title = item.Title, VideoUrl = item.Video_Path, ParentID = item.ChemistryID, Part = item.Part });
                }
                return Ok(new ResponseDetailModel { Message = "Get Data Form Id", Status = APIStatus.Successfull, Data = dataList });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseDetailModel { Message = ex.Message, Status = APIStatus.Error, Data = null });
            }


        }
    }
}
