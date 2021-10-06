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
                    if (clsUtility.CheckNull(item.ID))
                    {
                        if (item.Video_Path != null && item.Video_Path != "")
                        {
                            videoBytes = await utility.FileToByteArray(item.Video_Path);
                        }
                        //var chemistryitem = db.Tb_Chemistrys.Where(i => i.ID == item.ParentID && i.Active == true && i.Enabled == true).FirstOrDefault();
                        var detailitem = db.Tb_Chemistry_Details.Where(i => i.ChemistryID == item.ParentID && i.Part == item.Part && i.Active == true && i.Enabled == true).FirstOrDefault();
                        if (detailitem != null)
                        {
                            return Ok(new ResponseModel { Message = "Already have Part" + item.Part, Status = APIStatus.Warning });
                        }
                        else
                        {
                            var dataitem = new tbl_Chemistry_Detail
                            {
                                Part = item.Part,
                                ChemistryID = item.ParentID,
                                Title = item.Title,
                                Teachear_Name = item.Teachear_Name,
                                Video_Path = item.Video_Path != "" ? utility.SavePathFile("Chem", "UploadChemistry", Convert.ToBase64String(videoBytes), _hostingEnvironment, ".mp4") : null
                            };
                            db.Tb_Chemistry_Details.Add(dataitem);
                            db.SaveChanges();
                        }
                        return Ok(new SaveResponseModel { Message = "Save Successful", Status = APIStatus.Successfull, Data = null });
                    }
                    else
                    {
                        var detailitem = db.Tb_Chemistry_Details.Where(i => i.ID == item.ID && i.Active == true && i.Enabled == true).FirstOrDefault();
                        if (clsUtility.CheckNotNull(detailitem))
                        {
                            var checkPart = db.Tb_Chemistry_Details.Where(i => i.ChemistryID == item.ParentID && i.Part == item.Part && i.ID != item.ID && i.Active == true && i.Enabled == true).FirstOrDefault();
                            if (clsUtility.CheckNull(checkPart))
                            {
                                if (item.isPhotoUpdate)
                                {
                                    if (item.Video_Path != null && item.Video_Path != "")
                                    {
                                        videoBytes = await utility.FileToByteArray(item.Video_Path);
                                    }
                                    detailitem.Video_Path = item.Video_Path != "" ? utility.SavePathFile("Chem", "UploadChemistry", Convert.ToBase64String(videoBytes), _hostingEnvironment, ".mp4") : null;
                                }
                                detailitem.Part = item.Part;
                                detailitem.ChemistryID = item.ParentID;
                                detailitem.Title = item.Title;
                                detailitem.Teachear_Name = item.Teachear_Name;
                                db.Tb_Chemistry_Details.Update(detailitem);
                                db.SaveChanges();
                            }
                            else
                            {
                                return Ok(new SaveResponseModel { Message = "Already have Part" + item.Part, Status = APIStatus.Warning });
                            }
                        }
                        return Ok(new SaveResponseModel { Message = "Update Successful", Status = APIStatus.Successfull, Data = detailitem });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new SaveResponseModel { Message = ex.Message, Status = APIStatus.Error });
                }

            }

        }

        [HttpPost]
        [Route("DeleteChemistryDetail")]
        public async Task<IActionResult> DeleteChemistryDetail(CommonModel item)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {

                    var chemItem = db.Tb_Chemistry_Details.Where(i => i.ID == item.ID && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if (chemItem != null)
                    {
                        chemItem.Enabled = false;
                        db.Tb_Chemistry_Details.Update(chemItem);
                        await db.SaveChangesAsync();
                        return Ok(new Response { Message = "Delete Successful !", Status = APIStatus.Successfull, Data = null });
                    }
                    else
                    {
                        return Ok(new Response { Message = "System Wrong", Status = APIStatus.Error, Data = null });
                    }

                }
                catch (Exception ex)
                {
                    return Ok(new Response { Message = ex.Message, Status = APIStatus.SystemError, Data = null });
                }

            }

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
                return Ok(new ResponseDetailModel { Message = "Get Data form Id", Status = APIStatus.Successfull, Data = dataList });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseDetailModel { Message = ex.Message, Status = APIStatus.Error, Data = null });
            }


        }

        [HttpPost]
        [Route("GetChemistryDetail")]
        public async Task<IActionResult> GetChemistryDetail(SearchClass tempData)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                    var userModel = new UserModel();
                    var resultTemp = db.Tb_Chemistry_Details.AsEnumerable().Where(u => u.Enabled == true && u.Active == true)
                         .Join(db.Tb_Chemistrys.AsEnumerable().Where(c => c.Enabled == true && c.Active == true),
                                           tmpJoin => tmpJoin.ChemistryID, tmpDetailJoin => tmpDetailJoin.ID,
                                           (tmpJoin, tmpDetailJoin) => new { tmpJoin, tmpDetailJoin })
                        .Select((items, index) => new
                        {
                            No = (index + 1),
                            items.tmpJoin.Part,
                            items.tmpJoin.ChemistryID,
                            items.tmpJoin.ChemistryDetailID,
                            items.tmpJoin.Title,
                            items.tmpJoin.Teachear_Name,
                            items.tmpJoin.ID,
                            items.tmpJoin.Video_Path,
                            items.tmpDetailJoin.Chapter

                        }).ToList();
                    if (resultTemp.Any())
                    {
                        if (!string.IsNullOrEmpty(tempData.filter))
                        {
                            resultTemp = resultTemp.Where(item => (string.IsNullOrEmpty(item.Part.ToString()) ? false : (item.Part.ToString()).IndexOf(tempData.filter.ToLower()) != -1 ? true : (item.Part.ToString()).Contains(tempData.filter.ToLower())) ||
                            string.IsNullOrEmpty(item.Teachear_Name) ? false : (item.Teachear_Name.ToLower()).IndexOf(tempData.filter.ToLower()) != -1 ? true : (item.Teachear_Name.ToLower()).Contains(tempData.filter.ToLower())
                            ).ToList();

                        }
                    }
                    if (resultTemp.Any())
                    {
                        bool sortType = tempData.sort.Split('|')[1].Equals("asc");
                        string sortName = tempData.sort.Split('|')[0];
                        int? total = resultTemp.Count();
                        int? last_page = (int)Math.Ceiling((double)total / (double)tempData.per_page);
                        if (sortType)
                        {
                            resultTemp = resultTemp
                                        .OrderBy(item => item.GetType().GetProperty(sortName).GetValue(item, null))
                                        .Skip(((int)tempData.page - 1) * (int)tempData.per_page)
                                        .Take((int)tempData.per_page).ToList();
                        }
                        else
                        {
                            resultTemp = resultTemp
                                        .OrderByDescending(item => item.GetType().GetProperty(sortName).GetValue(item, null))
                                        .Skip(((int)tempData.page - 1) * (int)tempData.per_page)
                                        .Take((int)tempData.per_page).ToList();
                        }
                        var list = resultTemp.Select(x => new ChemistryDetailModel
                        {
                            ID = x.ID,
                            Title = x.Title,
                            Part = x.Part,
                            Teachear_Name = x.Teachear_Name,
                            Video_Path = x.Video_Path,
                            ChemistryID = x.ChemistryID,
                            Chapter=x.Chapter,
                            
                            
                        }).ToList();
                        var temp = new
                        {
                            total = total != null ? total : null,
                            per_page = tempData.per_page != null ? tempData.per_page : null,
                            current_page = tempData.page != null ? tempData.page : null,
                            last_page = last_page != null ? last_page : null,
                            from = tempData.page >= 1 ? ((tempData.per_page * (tempData.page - 1)) + 1) : null,
                            to = tempData.page >= 1 ? ((tempData.page - 1) * tempData.per_page) + resultTemp.Count() : null,
                            data = list,
                        };
                        return Ok(temp);
                    }
                    else
                    {
                        return Ok(new ResponseList { Message = "List Error", Status = APIStatus.Successfull, Data = null });
                    }

                }
                catch (Exception ex)
                {
                    return Ok(new Response { Message = ex.Message, Status = APIStatus.SystemError, Data = null });
                }

            }

        }

        [HttpPost]
        [Route("GetbyID")]
        public async Task<IActionResult> GetbyID(CommonDetails common)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {

                    var chemItem = db.Tb_Chemistry_Details.Where(i => i.ID == common.ID && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if (clsUtility.CheckNotNull(chemItem))
                    {

                        common = new CommonDetails
                        {
                            ID = chemItem.ID,
                            Part = chemItem.Part,
                            Title = chemItem.Title,
                            ParentID = chemItem.ChemistryID,
                            Teachar_Name = chemItem.Teachear_Name,
                        };

                    }
                    return Ok(new SaveResponseModel { Message = "Successful", Status = APIStatus.Successfull, Data = common });
                }
                catch (Exception ex)
                {
                    return Ok(new SaveResponseModel { Message = ex.Message, Status = APIStatus.SystemError, Data = null });
                }

            }

        }

        [HttpPost]
        [Route("RecoverChemistryDetail")]
        public async Task<IActionResult> RecoverChemistryDetail(SearchClass tempData)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                    var userModel = new UserModel();
                    var resultTemp = db.Tb_Chemistry_Details.AsEnumerable().Where(u => u.Enabled == false && u.Active == true)
                         .Join(db.Tb_Chemistrys.AsEnumerable().Where(c => c.Enabled == true && c.Active == true),
                                           tmpJoin => tmpJoin.ChemistryID, tmpDetailJoin => tmpDetailJoin.ID,
                                           (tmpJoin, tmpDetailJoin) => new { tmpJoin, tmpDetailJoin })
                        .Select((items, index) => new
                        {
                            No = (index + 1),
                            items.tmpJoin.Part,
                            items.tmpJoin.ChemistryID,
                            items.tmpJoin.ChemistryDetailID,
                            items.tmpJoin.Title,
                            items.tmpJoin.Teachear_Name,
                            items.tmpJoin.ID,
                            items.tmpJoin.Video_Path,
                            items.tmpDetailJoin.Chapter

                        }).ToList();
                    if (resultTemp.Any())
                    {
                        if (!string.IsNullOrEmpty(tempData.filter))
                        {
                            resultTemp = resultTemp.Where(item => (string.IsNullOrEmpty(item.Part.ToString()) ? false : (item.Part.ToString()).IndexOf(tempData.filter.ToLower()) != -1 ? true : (item.Part.ToString()).Contains(tempData.filter.ToLower())) ||
                            string.IsNullOrEmpty(item.Teachear_Name) ? false : (item.Teachear_Name.ToLower()).IndexOf(tempData.filter.ToLower()) != -1 ? true : (item.Teachear_Name.ToLower()).Contains(tempData.filter.ToLower())
                            ).ToList();

                        }
                    }
                    if (resultTemp.Any())
                    {
                        bool sortType = tempData.sort.Split('|')[1].Equals("asc");
                        string sortName = tempData.sort.Split('|')[0];
                        int? total = resultTemp.Count();
                        int? last_page = (int)Math.Ceiling((double)total / (double)tempData.per_page);
                        if (sortType)
                        {
                            resultTemp = resultTemp
                                        .OrderBy(item => item.GetType().GetProperty(sortName).GetValue(item, null))
                                        .Skip(((int)tempData.page - 1) * (int)tempData.per_page)
                                        .Take((int)tempData.per_page).ToList();
                        }
                        else
                        {
                            resultTemp = resultTemp
                                        .OrderByDescending(item => item.GetType().GetProperty(sortName).GetValue(item, null))
                                        .Skip(((int)tempData.page - 1) * (int)tempData.per_page)
                                        .Take((int)tempData.per_page).ToList();
                        }
                        var list = resultTemp.Select(x => new ChemistryDetailModel
                        {
                            ID = x.ID,
                            Title = x.Title,
                            Part = x.Part,
                            Teachear_Name = x.Teachear_Name,
                            Video_Path = x.Video_Path,
                            ChemistryID = x.ChemistryID,
                            Chapter = x.Chapter

                        }).ToList();
                        var temp = new
                        {
                            total = total != null ? total : null,
                            per_page = tempData.per_page != null ? tempData.per_page : null,
                            current_page = tempData.page != null ? tempData.page : null,
                            last_page = last_page != null ? last_page : null,
                            from = tempData.page >= 1 ? ((tempData.per_page * (tempData.page - 1)) + 1) : null,
                            to = tempData.page >= 1 ? ((tempData.page - 1) * tempData.per_page) + resultTemp.Count() : null,
                            data = list,
                        };
                        return Ok(temp);
                    }
                    else
                    {
                        return Ok(new ResponseList { Message = "List Error", Status = APIStatus.Successfull, Data = null });
                    }

                }
                catch (Exception ex)
                {
                    return Ok(new Response { Message = ex.Message, Status = APIStatus.SystemError, Data = null });
                }

            }

        }

        [HttpPost]
        [Route("RestoreChemistryDetail")]
        public async Task<IActionResult> RestoreChemistryDetail(DetailModel item)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {

                    var chemItem = db.Tb_Chemistry_Details.Where(i => i.ChemistryID== item.ParentID && i.Part ==item.Part && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if (clsUtility.CheckNull(chemItem))
                    {
                        var resItem = db.Tb_Chemistry_Details.Where(i => i.ID == item.ID && i.Active == true && i.Enabled == false).FirstOrDefault();
                        resItem.Enabled = true;
                        db.Tb_Chemistry_Details.Update(resItem);
                        await db.SaveChangesAsync();
                        return Ok(new Response { Message = "Restore Successful !", Status = APIStatus.Successfull, Data = null });
                    }
                    else
                    {
                        return Ok(new Response { Message = "Can't restore your chapter is " + item.Part + " Already Have !", Status = APIStatus.Error, Data = null });
                    }

                }
                catch (Exception ex)
                {
                    return Ok(new Response { Message = ex.Message, Status = APIStatus.SystemError, Data = null });
                }

            }

        }
    }
}
