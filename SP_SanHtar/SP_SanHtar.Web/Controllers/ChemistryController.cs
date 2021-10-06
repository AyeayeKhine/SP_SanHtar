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
                    if(item.ID == null)
                    {
                        if (item.Photo_Path != null && item.Photo_Path != "")
                        {
                            imageBytes = await utility.FileToByteArray(item.Photo_Path);
                        }
                        var chemistryitem = db.Tb_Chemistrys.Where(i => i.Chapter == item.Chapter && i.Active == true && i.Enabled == true).FirstOrDefault();
                        if (chemistryitem != null)
                        {
                            return Ok(new Response { Message = "Already have Chapter" + item.Chapter, Status = APIStatus.Warning });
                        }
                        else
                        {
                            var dataitem = new tbl_Chemistry
                            {
                                Chapter = item.Chapter,
                                Title = item.Title,
                                Teachear_Name = item.Teachear_Name,
                                Photo_Path = imageBytes != null ? utility.SavePathFile("Chem", "UploadPhotoChemistry", Convert.ToBase64String(imageBytes), _hostingEnvironment, ".jpg") : null,
                            };
                            db.Tb_Chemistrys.Add(dataitem);
                            db.SaveChanges();
                        }
                        return Ok(new SaveResponseModel { Message = "Save Successful", Status = APIStatus.Successfull, Data = null });
                    }
                    else
                    {
                        var chemistryitem = db.Tb_Chemistrys.Where(i => i.ID == item.ID && i.Active == true && i.Enabled == true).FirstOrDefault();
                        if (clsUtility.CheckNotNull(chemistryitem))
                        {
                            if (item.isPhotoUpdate)
                            {
                                if (item.Photo_Path != null && item.Photo_Path != "")
                                {
                                    imageBytes = await utility.FileToByteArray(item.Photo_Path);
                                }
                                chemistryitem.Photo_Path = imageBytes != null ? utility.SavePathFile("Chem", "UploadPhotoChemistry", Convert.ToBase64String(imageBytes), _hostingEnvironment, ".jpg") : null;
                            }
                            var checkChem = db.Tb_Chemistrys.Where(i => i.Chapter == item.Chapter && i.ID!=item.ID && i.Active == true && i.Enabled == true).FirstOrDefault();
                            if (clsUtility.CheckNull(checkChem))
                            {
                                chemistryitem.Chapter = item.Chapter;
                                chemistryitem.Title = item.Title;
                                chemistryitem.Teachear_Name = item.Teachear_Name;
                                db.Tb_Chemistrys.Update(chemistryitem);
                                db.SaveChanges();
                                
                            }
                            else
                            {
                                return Ok(new SaveResponseModel { Message = "Already have Chapter" + item.Chapter, Status = APIStatus.Warning });
                            }
                               
                        }
                        return Ok(new SaveResponseModel { Message = "Update Successful", Status = APIStatus.Successfull, Data = chemistryitem });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new SaveResponseModel { Message = ex.Message, Status = APIStatus.Error });
                }

            }
           
        }

        [HttpPost]
        [Route("DeleteChemistry")]
        public async Task<IActionResult> DeleteChemistry(CommonModel item)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {

                    var chemItem = db.Tb_Chemistrys.Where(i => i.ID == item.ID && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if (chemItem != null)
                    {
                        chemItem.Enabled = false;
                        db.Tb_Chemistrys.Update(chemItem);
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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            OnlineContext db = new OnlineContext();
            try
            {
               
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

        [HttpGet("GetAll/{ID}")]
        public async Task<IActionResult> GetAll(Guid ID)
        {
            OnlineContext db = new OnlineContext();
            try
            {
                var itemList = await db.tbl_Assigns.OrderBy(i => i.Chapter).Where(j => j.UserID == ID && j.Active ==true && j.Enabled ==true).ToListAsync();
                var dataList = new List<CommomModels>();
                foreach (var item in itemList)
                {
                    dataList.Add(new CommomModels { ID = item.ID, Description = "Chapter " + item.Chapter, Title = item.Title, PhotoUrl = item.Photo_Path, Teacher_Name = item.Teachear_Name });
                }
                return Ok(new ResponseModel { Message = "Show All Data", Status = APIStatus.Successfull, Data = dataList });

            }
            catch (Exception ex)
            {
                return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.Error, Data = null });
            }
        }

        [HttpPost]
        [Route("GetChemistry")]
        public async Task<IActionResult> GetChemistry(SearchClass tempData)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                    //var userModel = new UserModel();
                    var resultTemp = db.Tb_Chemistrys.AsEnumerable().Where(u => u.Enabled == true && u.Active == true)
                        .Select((item, index) => new
                        {
                            No = (index + 1),
                            item.ChemistryID,
                            item.Chapter,
                            item.Title,
                            item.Teachear_Name,
                            item.Photo_Path,
                            item.ID,

                        }).ToList();
                    if (resultTemp.Any())
                    {
                        if (!string.IsNullOrEmpty(tempData.filter))
                        {
                            resultTemp = resultTemp.Where(item => (string.IsNullOrEmpty(item.Chapter.ToString()) ? false : (item.Chapter.ToString()).IndexOf(tempData.filter) != -1 ? true : (item.Chapter.ToString()).Contains(tempData.filter.ToLower()))).ToList();

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
                        var list = resultTemp.Select(x => new ChemistryModel
                        {
                            ID = x.ID,
                            Chapter = x.Chapter,
                            ChemistryID = x.ChemistryID,
                            Title = x.Title,
                            Teachear_Name = x.Teachear_Name,
                            Photo_Path = x.Photo_Path,
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
                        return Ok(new ResponseList { Message = "List Error", Status = APIStatus.Error, Data = null });
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
        public async Task<IActionResult> GetbyID(CommonModel common)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                   
                    var chemItem = db.Tb_Chemistrys.Where(i => i.ID == common.ID && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if (clsUtility.CheckNotNull(chemItem))
                    {

                        common = new CommonModel
                        {
                            ID = chemItem.ID,
                           Chapter=chemItem.Chapter,
                           Title=chemItem.Title,
                           Teachear_Name=chemItem.Teachear_Name

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
        [Route("RecoverChemistry")]
        public async Task<IActionResult> RecoverChemistry(SearchClass tempData)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                    //var userModel = new UserModel();
                    var resultTemp = db.Tb_Chemistrys.AsEnumerable().Where(u => u.Enabled == false && u.Active == true)
                        .Select((item, index) => new
                        {
                            No = (index + 1),
                            item.ChemistryID,
                            item.Chapter,
                            item.Title,
                            item.Teachear_Name,
                            item.Photo_Path,
                            item.ID,

                        }).ToList();
                    if (resultTemp.Any())
                    {
                        if (!string.IsNullOrEmpty(tempData.filter))
                        {
                            resultTemp = resultTemp.Where(item => (string.IsNullOrEmpty(item.Chapter.ToString()) ? false : (item.Chapter.ToString()).IndexOf(tempData.filter) != -1 ? true : (item.Chapter.ToString()).Contains(tempData.filter.ToLower()))).ToList();

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
                        var list = resultTemp.Select(x => new ChemistryModel
                        {
                            ID = x.ID,
                            Chapter = x.Chapter,
                            ChemistryID = x.ChemistryID,
                            Title = x.Title,
                            Teachear_Name = x.Teachear_Name,
                            Photo_Path = x.Photo_Path,
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
                        return Ok(new ResponseList { Message = "List Error", Status = APIStatus.Error, Data = null });
                    }

                }
                catch (Exception ex)
                {
                    return Ok(new Response { Message = ex.Message, Status = APIStatus.SystemError, Data = null });
                }

            }

        }

        [HttpPost]
        [Route("RestoreChemistry")]
        public async Task<IActionResult> RestoreChemistry(CommonModel item)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {

                    var chemItem = db.Tb_Chemistrys.Where(i => i.Chapter == item.Chapter && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if (clsUtility.CheckNull(chemItem))
                    {
                        var resItem = db.Tb_Chemistrys.Where(i => i.Chapter == item.Chapter && i.ID==item.ID && i.Active == true && i.Enabled == false).FirstOrDefault();
                        resItem.Enabled = true;
                        db.Tb_Chemistrys.Update(resItem);
                        await db.SaveChangesAsync();
                        return Ok(new SaveResponseModel { Message = "Restore Successful !", Status = APIStatus.Successfull, Data = null });
                    }
                    else
                    {
                        return Ok(new SaveResponseModel { Message = "Can't restore your chapter is "+item.Chapter +" Already Have !", Status = APIStatus.Error, Data = null });
                    }

                }
                catch (Exception ex)
                {
                    return Ok(new SaveResponseModel { Message = ex.Message, Status = APIStatus.SystemError, Data = null });
                }

            }

        }

    }
}
