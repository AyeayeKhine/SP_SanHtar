using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SP_SanHtar.Web.cls;
using SP_SanHtar.Web.ContextDB;
using SP_SanHtar.Web.Models;

namespace SP_SanHtar.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        clsUtility utility = new clsUtility();
        private readonly IHostingEnvironment _hostingEnvironment;
        private byte[] imageBytes;
        private readonly AuthMessageSenderOptions _emailSender;

        public UserController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
            _emailSender = new AuthMessageSenderOptions();
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> Create(RegisterUser registerUser)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                    if (registerUser.ID ==null)
                    {
                        var userItem = db.Tb_Users.Where(i => i.UserName == registerUser.UserName && i.Active == true && i.Enabled == true).FirstOrDefault();
                        if (userItem != null)
                        {
                            return Ok(new ResponseModel { Message = "UserName Already Have.....", Status = APIStatus.Error, Data = null });
                        }
                        else
                        {
                            CryptographyClass tempCryptography = new CryptographyClass()
                            {
                                Text = string.Format("{0}{1}", registerUser.UserName.ToLower().Trim(), registerUser.Password.Trim()),
                                SALT = Encoding.UTF8.GetBytes(registerUser.Password.Trim()),
                                PlainText = Encoding.UTF8.GetBytes(registerUser.Password.Trim())

                            };
                            var encryptedPassword = await Hash.Encrypt(tempCryptography);
                            if (registerUser.PhotoUrl != null && registerUser.PhotoUrl != "")
                            {
                                imageBytes = await utility.FileToByteArray(registerUser.PhotoUrl);
                            }
                            var item = new tbl_User
                            {
                                PhotoUrl = imageBytes != null ? utility.SavePathFile("User", "UploadUser", Convert.ToBase64String(imageBytes), _hostingEnvironment, ".jpg") : null,
                                Password = encryptedPassword,
                                SaltAes = tempCryptography.PlainText,
                                SaltHash = tempCryptography.SALT,
                                UserName = registerUser.UserName,
                                PersonalContactNumber = registerUser.PersonalContactNumber,
                                OtherContactNumber = registerUser.OtherContactNumber,
                                Sex = registerUser.Sex,
                                FirstName = registerUser.FirstName,
                                LastName = registerUser.LastName,
                                Email = registerUser.Email,
                                UserType = registerUser.UserType,
                                PartID = registerUser.PartID,
                                ChemistryID = registerUser.ChemistryID
                            };
                            db.Tb_Users.Add(item);
                            db.SaveChanges();
                            if (registerUser.ChemistryID != null)
                            {
                                string[] subs = registerUser.ChemistryID.Split(',');
                                foreach (var id in subs)
                                {
                                    Guid guidID = Guid.Parse(id);
                                    var chemItem = db.Tb_Chemistrys.Where(i => i.ID == guidID && i.Active == true && i.Enabled == true).FirstOrDefault();
                                    var assignItem = new tbl_Assign
                                    {
                                        Teachear_Name = chemItem.Teachear_Name,
                                        Title = chemItem.Title,
                                        Photo_Path = chemItem.Photo_Path,
                                        UserID = item.ID,
                                        ChapterID = guidID,
                                        Chapter = chemItem.Chapter
                                    };
                                    db.tbl_Assigns.Add(assignItem);
                                }
                            }

                            db.SaveChanges();
                            return Ok(new SaveResponseModel { Message = "Save Successful", Status = APIStatus.Successfull, Data = item });
                        }
                    }
                    else
                    {
                        var userItem = new UserModel();
                        if (registerUser.PhotoUrl != null && registerUser.PhotoUrl != "")
                        {
                            imageBytes = await utility.FileToByteArray(registerUser.PhotoUrl);
                        }
                        var itemUser = db.Tb_Users.Where(u => u.ID == registerUser.ID && u.Enabled==true && u.Active ==true).FirstOrDefault();
                        if(itemUser != null)
                        {
                            if (registerUser.isUpdatePhoto)
                            {
                                itemUser.PhotoUrl = imageBytes != null ? utility.SavePathFile("User", "UploadUser", Convert.ToBase64String(imageBytes), _hostingEnvironment, ".jpg") : null;
                            }
                            itemUser.UserName = registerUser.UserName;
                            itemUser.PersonalContactNumber = registerUser.PersonalContactNumber;
                            itemUser.OtherContactNumber = registerUser.OtherContactNumber;
                            itemUser.Sex = registerUser.Sex;
                            itemUser.FirstName = registerUser.FirstName;
                            itemUser.LastName = registerUser.LastName;
                            itemUser.Email = registerUser.Email;
                            itemUser.UserType = registerUser.UserType;
                            itemUser.PartID = registerUser.PartID;
                            itemUser.ChemistryID = registerUser.ChemistryID;
                            db.Tb_Users.Update(itemUser);
                            db.SaveChanges();
                            var itemAssigns = db.tbl_Assigns.Where(ass => ass.UserID == registerUser.ID && ass.Enabled == true && ass.Active == true).ToList();
                            foreach (var itemAssign in itemAssigns)
                            {
                                itemAssign.Active = false;
                                itemAssign.Enabled = false;
                                db.tbl_Assigns.Update(itemAssign);
                            }
                            db.SaveChanges();
                            string ChemistryString = "";

                            if (registerUser.ChemistryID != null)
                            {
                                string[] subs = registerUser.ChemistryID.Split(',');

                                foreach (var id in subs)
                                {
                                    Guid guidID = Guid.Parse(id);
                                    var chemItem = db.Tb_Chemistrys.Where(i => i.ID == guidID && i.Active == true && i.Enabled == true).FirstOrDefault();

                                    var assignItem = new tbl_Assign
                                    {
                                        Teachear_Name = chemItem.Teachear_Name,
                                        Title = chemItem.Title,
                                        Photo_Path = chemItem.Photo_Path,
                                        UserID = itemUser.ID,
                                        ChapterID = guidID,
                                        Chapter = chemItem.Chapter
                                    };
                                    db.tbl_Assigns.Add(assignItem);
                                    if (ChemistryString == "")
                                    {
                                        ChemistryString = "Chapter " + chemItem.Chapter.ToString() + " (" + chemItem.Title + ")";
                                    }
                                    else
                                    {
                                        ChemistryString += ", Chapter " + assignItem.Chapter.ToString() + " (" + assignItem.Title + ")";
                                    }
                                }
                            }

                            db.SaveChanges();

                            //itemUser.ChemistryID = registerUser.ChemistryID;
                            //itemUser.ChemistryString = ChemistryString;
                            userItem = new UserModel
                            {
                                ID = itemUser.ID,
                                UserName = itemUser.UserName,
                                PersonalContactNumber = itemUser.PersonalContactNumber,
                                OtherContactNumber = itemUser.OtherContactNumber,
                                Sex = itemUser.Sex,
                                FirstName = itemUser.FirstName,
                                LastName = itemUser.LastName,
                                Email = itemUser.Email,
                                UserType = itemUser.UserType,
                                PartID = itemUser.PartID,
                                ChemistryID = registerUser.ChemistryID,
                                UserID = itemUser.UserID,
                                ChemistryString = ChemistryString
                            };
                            
                        }
                        return Ok(new SaveResponseModel { Message = "Save Successful", Status = APIStatus.Successfull, Data = userItem });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new SaveResponseModel { Message = ex.Message, Status = APIStatus.Error });
                }

            }

        }

        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(UserModel userModel)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                  
                    var userItem = db.Tb_Users.Where(i => i.ID == userModel.ID && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if (userItem != null)
                    {
                        userItem.Enabled = false;
                        db.Tb_Users.Update(userItem);
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
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> Update(RegisterUser registerUser)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                    CryptographyClass tempCryptography = new CryptographyClass()
                    {
                        Text = string.Format("{0}{1}", registerUser.UserName.ToLower().Trim(), registerUser.Password.Trim()),
                        SALT = Encoding.UTF8.GetBytes(registerUser.Password.Trim()),
                        PlainText = Encoding.UTF8.GetBytes(registerUser.Password.Trim())

                    };
                    var encryptedPassword = await Hash.Encrypt(tempCryptography);
                    var item = new tbl_User
                    {

                        Password = encryptedPassword,
                        SaltAes = tempCryptography.PlainText,
                        SaltHash = tempCryptography.SALT,
                        UserName = registerUser.UserName,
                        PersonalContactNumber = registerUser.PersonalContactNumber,
                        OtherContactNumber = registerUser.OtherContactNumber,
                        Sex = registerUser.Sex,
                        FirstName = registerUser.FirstName,
                        LastName = registerUser.LastName,
                        Email = registerUser.Email,
                        UserType = registerUser.UserType,
                        //PartID = registerUser.PartID,
                        //ChemistryID = registerUser.ChemistryID
                    };
                    if (registerUser.isUpdatePhoto)
                    {
                        if (registerUser.PhotoUrl != null || registerUser.PhotoUrl != "String")
                        {
                            imageBytes = await utility.FileToByteArray(registerUser.PhotoUrl);
                        }
                        item.PhotoUrl = utility.SavePathFile("User", "UploadUser", Convert.ToBase64String(imageBytes), _hostingEnvironment, ".jpg");
                    }
                    db.Tb_Users.Add(item);
                    db.SaveChanges();

                    return Ok(new SaveResponseModel { Message = "Save Successful", Status = APIStatus.Successfull, Data = item });

                }
                catch (Exception ex)
                {
                    return Ok(new SaveResponseModel { Message = ex.Message, Status = APIStatus.Error });
                }

            }

        }

        [HttpPost("UpdateUserAssign")]
        public async Task<IActionResult> UpdateAssign(AssignModel assignModel)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                    string[] subs = assignModel.ChemistryID.Split(',');
                    foreach (var id in subs)
                    {
                        Guid guidID = Guid.Parse(id);
                        var chemItem = db.Tb_Chemistrys.Where(i => i.ID == guidID && i.Active == true && i.Enabled == true).FirstOrDefault();
                        var assignItem = new tbl_Assign
                        {
                            Teachear_Name = chemItem.Teachear_Name,
                            Title = chemItem.Title,
                            Photo_Path = chemItem.Photo_Path,
                            UserID = assignModel.ID,
                            ChapterID = guidID,
                            Chapter = chemItem.Chapter
                        };
                        db.tbl_Assigns.Add(assignItem);
                    }
                    db.SaveChanges();
                    return Ok(new SaveResponseModel { Message = "Save Successful", Status = APIStatus.Successfull, Data = null });
                }
                catch (Exception ex)
                {
                    return Ok(new SaveResponseModel { Message = ex.Message, Status = APIStatus.Error });
                }

            }

        }

        [HttpGet]
        [Route("Login/{UserName}/{Password}")]
        public async Task<IActionResult> GetUser(string UserName, string Password)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                    var userModel = new UserModel();
                    var userItem = db.Tb_Users.Where(i => i.UserName == UserName && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if (userItem != null)
                    {
                        var OldHASHValue = userItem.Password;
                        CryptographyClass tempCryptography = new CryptographyClass()
                        {
                            Text = string.Format("{0}{1}", UserName.ToLower().Trim(), Password.Trim()),
                            SALT = userItem.SaltHash
                        };
                        userModel = new UserModel
                        {
                            ID = userItem.ID,
                            UserID = userItem.UserID,
                            UserName = userItem.UserName,
                            FirstName = userItem.FirstName,
                            LastName = userItem.LastName,
                            Email = userItem.Email,
                            Password = Password,
                            UserType = userItem.UserType,
                            Sex = userItem.Sex,
                            PersonalContactNumber = userItem.PersonalContactNumber,
                            OtherContactNumber = userItem.OtherContactNumber,
                            PhotoUrl = userItem.PhotoUrl
                        };
                        bool isLogin = await Hash.CompareHashValue(tempCryptography, OldHASHValue);
                        if (isLogin)
                        {
                            userItem.Password = Password;
                            //return Ok(userItem);
                        }
                        else
                        {
                            return Ok(new Response { Message = "Wrong UserName and Password", Status = APIStatus.Error, Data = null });
                        }
                        return Ok(new Response { Message = "Login Successful", Status = APIStatus.Successfull, Data = userModel });
                    }
                    else
                    {
                        return Ok(new Response { Message = "Wrong UserName and Password", Status = APIStatus.Error, Data = null });
                    }
                    
                }
                catch (Exception ex)
                {
                    return Ok(new Response { Message = ex.Message, Status = APIStatus.SystemError, Data = null });
                }

            }

        }


        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            int companyId = 0;
            int userId = 0;
            GeneratePassword generate = new GeneratePassword();
            string newPasswrod = "";
            clsUtility utility = new clsUtility();
            try
            {
                using (OnlineContext db = new OnlineContext())
                {
                    if (ModelState.IsValid)
                    {
                        var userItem = db.Tb_Users.Where(i => i.UserName == changePasswordModel.UserName && i.Active == true && i.Enabled == true).FirstOrDefault();
                        if(userItem != null)
                        {
                            var OldHASHValue = userItem.Password;
                            CryptographyClass tempCryptography = new CryptographyClass()
                            {
                                Text = string.Format("{0}{1}", changePasswordModel.UserName.ToLower().Trim(), changePasswordModel.OldPassword.Trim()),
                                SALT = userItem.SaltHash
                            };

                            bool isLogin = await Hash.CompareHashValue(tempCryptography, OldHASHValue);
                            if (isLogin)
                            {
                                newPasswrod = changePasswordModel.NewPassword;

                                //User Entry
                                CryptographyClass NewtempCryptography = new CryptographyClass()
                                {
                                    Text = string.Format("{0}{1}", changePasswordModel.UserName.ToLower().Trim(), newPasswrod.Trim()),
                                    SALT = Encoding.UTF8.GetBytes(newPasswrod.Trim()),
                                    PlainText = Encoding.UTF8.GetBytes(newPasswrod.Trim())

                                };

                                var encryptedPassword = await Hash.Encrypt(NewtempCryptography);
                                //var encryptedPassword = await UPlace_WebAPI.PublicClassUsed.Hash.Encrypt(newtempCryptography);


                                userItem.Password = encryptedPassword;
                                userItem.SaltAes = NewtempCryptography.PlainText;
                                userItem.SaltHash = NewtempCryptography.SALT;
                                db.SaveChanges();

                                var userModel = new UserModel
                                {
                                    ID = userItem.ID,
                                    UserID = userItem.UserID,
                                    UserName = userItem.UserName,
                                    FirstName = userItem.FirstName,
                                    LastName = userItem.LastName,
                                    Email = userItem.Email,
                                    Password = newPasswrod,
                                    UserType = userItem.UserType,
                                    Sex = userItem.Sex,
                                    PersonalContactNumber = userItem.PersonalContactNumber,
                                    OtherContactNumber = userItem.OtherContactNumber,
                                    PhotoUrl = userItem.PhotoUrl
                                };

                                return Ok(new Response { Message = "Change Password Detected.", Status = APIStatus.Successfull, Data = userModel });
                            }
                            else
                            {
                                return Ok(new Response { Message = "Change Password Fail.", Status = APIStatus.Error });
                            }
                        }
                        else
                        {
                            return Ok(new Response { Message = "Wrong Data .", Status = APIStatus.Error });
                        }
                    }
                    return Ok(new Response { Message = "Wrong Data .", Status = APIStatus.Error });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.Error });
            }

        }


        [HttpGet]
        [Route("ForgetPassword/{Email}")]
        public async Task<IActionResult> ForgetPassword(string Email)
        {            
            GeneratePassword generate = new GeneratePassword();
            string newPasswrod = "";
            clsUtility utility = new clsUtility();
            try
            {
                using (OnlineContext db = new OnlineContext())
                {
                    if (ModelState.IsValid)
                    {
                        var userData = db.Tb_Users.Where(us => (us.Email == Email || us.UserName == Email) /*&& us.UserId == model.UserId*/ && us.Active.Equals(true) && us.Enabled.Equals(true)).FirstOrDefault();

                        if(userData != null)
                        {
                            newPasswrod = generate.RandomString(8);
                            //User Entry
                            CryptographyClass newtempCryptography = new CryptographyClass()
                            {
                                Text = string.Format("{0}{1}", userData.UserName.ToLower().Trim(), newPasswrod.Trim()),
                                SALT = Encoding.UTF8.GetBytes(newPasswrod.Trim()),
                                PlainText = Encoding.UTF8.GetBytes(newPasswrod.Trim())

                            };
                            var encryptedPassword = await Hash.Encrypt(newtempCryptography);
                            userData.Active = true;
                            userData.CreatedDate = DateTime.Now;
                            userData.Password = encryptedPassword;
                            userData.SaltAes = newtempCryptography.PlainText;
                            userData.SaltHash = newtempCryptography.SALT;
                            db.SaveChanges();
                            await _emailSender.SendEmailAsync(userData.Email, "SPSH New Password ", "Please use this code to login password for User account <br/> " +
                                "Here is your code :  " + newPasswrod);
                            return Ok(new Response { Message = "ForgetPassword Detected.", Status = APIStatus.Successfull });
                        }
                        else
                        {
                            return Ok(new Response { Message = "Your Email is not correct", Status = APIStatus.Error });
                        }
                        
                    }
                    return Ok(new Response { Message = "Wrong Data .", Status = APIStatus.Error });
                }
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = ex.Message, Status = APIStatus.Error });
            }

        }

        [HttpPost]
        [Route("GetAllUser")]
        public async Task<IActionResult> GetAllUser(SearchClass tempData)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                    var userModel = new UserModel();
                    var resultTemp = db.Tb_Users.AsEnumerable().Where(u => u.Enabled ==true && u.Active ==true)
                        .Select((item, index) => new
                        {
                            No = (index + 1),
                            item.UserID,
                            item.UserName,
                            item.FirstName,
                            item.LastName,
                            item.Email,
                            item.ID,
                            item.Sex,
                            item.UserType,
                            item.PersonalContactNumber,
                            item.OtherContactNumber,

                        }).ToList();
                    if (resultTemp.Any())
                    {
                        if (!string.IsNullOrEmpty(tempData.filter))
                        {
                            resultTemp = resultTemp.Where(item => (string.IsNullOrEmpty(item.UserName) ? false : (item.UserName.ToLower()).IndexOf(tempData.filter.ToLower()) != -1 ? true : (item.UserName.ToLower()).Contains(tempData.filter.ToLower()))).ToList();

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
                        var list = resultTemp.Select(x => new UserModel
                        {
                            ID = x.ID,
                            UserID = x.UserID,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Email = x.Email,
                            UserName = x.UserName,
                            Sex = x.Sex,
                            UserType = x.UserType,
                            PersonalContactNumber = x.PersonalContactNumber,
                            OtherContactNumber = x.OtherContactNumber
                        }).ToList();
                        var temp = new
                        {
                            total = total != null ? total : null,
                            per_page = tempData.per_page != null ? tempData.per_page : null,
                            current_page = tempData.page != null ? tempData.page : null,
                            last_page = last_page != null ? last_page : null,
                            //from = tempData.page >= 1 ? ((tempData.per_page * (tempData.page - 1)) + 1) : null,
                            //to = tempData.page >= 1 ? ((tempData.page - 1) * tempData.per_page) + resultTemp.Count() : null,
                            from = tempData.page != null ? tempData.page : null,
                            to = last_page != null ? last_page : null,
                            data = list,
                        };
                        return Ok(temp);
                    }
                    else
                    {
                        return Ok(new ResponseList { Message = "List Error", Status = APIStatus.Successfull });
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
        public async Task<IActionResult> GetbyID(UserModel userModel)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                    string ChemistryId = "";
                    string ChemistryString = "";
                    var userItem = db.Tb_Users.Where(i => i.ID == userModel.ID && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if (userItem != null)
                    {
                        var userAssigns = db.tbl_Assigns.Where(i => i.UserID == userModel.ID && i.Active == true && i.Enabled == true).ToList();
                        if (userAssigns.Count() > 0)
                        {
                            ChemistryId = userAssigns[0].ChapterID.ToString();
                            ChemistryString = "Chapter " + userAssigns[0].Chapter.ToString() + " (" + userAssigns[0].Title + ")";
                            for (int i = 1; i < userAssigns.Count(); i++)
                            {
                                ChemistryId += "," + userAssigns[i].ChapterID.ToString();
                                ChemistryString += ", Chapter " + userAssigns[i].Chapter.ToString() + " (" + userAssigns[i].Title + ")";
                            }
                        }

                        userModel = new UserModel
                        {
                            ID = userItem.ID,
                            UserID = userItem.UserID,
                            UserName = userItem.UserName,
                            FirstName = userItem.FirstName,
                            LastName = userItem.LastName,
                            Email = userItem.Email,
                            Password = userItem.Password,
                            UserType = userItem.UserType,
                            Sex = userItem.Sex,
                            PersonalContactNumber = userItem.PersonalContactNumber,
                            OtherContactNumber = userItem.OtherContactNumber,
                            PhotoUrl = userItem.PhotoUrl,
                            ChemistryID = ChemistryId,
                            ChemistryString = ChemistryString

                        };

                    }
                    return Ok(new Response { Message = "Successful", Status = APIStatus.Successfull, Data = userModel });
                }
                catch (Exception ex)
                {
                    return Ok(new Response { Message = ex.Message, Status = APIStatus.SystemError, Data = null });
                }

            }

        }

        [HttpPost]
        [Route("RecoverUser")]
        public async Task<IActionResult> RecoverUser(SearchClass tempData)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                    var userModel = new UserModel();
                    var resultTemp = db.Tb_Users.AsEnumerable().Where(u => u.Enabled == false && u.Active == true)
                        .Select((item, index) => new
                        {
                            No = (index + 1),
                            item.UserID,
                            item.UserName,
                            item.FirstName,
                            item.LastName,
                            item.Email,
                            item.ID,
                            item.Sex,
                            item.UserType,
                            item.PersonalContactNumber,
                            item.OtherContactNumber,

                        }).ToList();
                    if (resultTemp.Any())
                    {
                        if (!string.IsNullOrEmpty(tempData.filter))
                        {
                            resultTemp = resultTemp.Where(item => (string.IsNullOrEmpty(item.UserName) ? false : (item.UserName.ToLower()).IndexOf(tempData.filter.ToLower()) != -1 ? true : (item.UserName.ToLower()).Contains(tempData.filter.ToLower()))).ToList();

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
                        var list = resultTemp.Select(x => new UserModel
                        {
                            ID = x.ID,
                            UserID = x.UserID,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Email = x.Email,
                            UserName = x.UserName,
                            Sex = x.Sex,
                            UserType = x.UserType,
                            PersonalContactNumber = x.PersonalContactNumber,
                            OtherContactNumber = x.OtherContactNumber
                        }).ToList();
                        var temp = new
                        {
                            total = total != null ? total : null,
                            per_page = tempData.per_page != null ? tempData.per_page : null,
                            current_page = tempData.page != null ? tempData.page : null,
                            last_page = last_page != null ? last_page : null,
                            //from = tempData.page >= 1 ? ((tempData.per_page * (tempData.page - 1)) + 1) : null,
                            //to = tempData.page >= 1 ? ((tempData.page - 1) * tempData.per_page) + resultTemp.Count() : null,
                            from = tempData.page != null ? tempData.page : null,
                            to = last_page != null ? last_page : null,
                            data = list,
                        };
                        return Ok(temp);
                    }
                    else
                    {
                        return Ok(new ResponseList { Message = "List Error", Status = APIStatus.Successfull });
                    }

                }
                catch (Exception ex)
                {
                    return Ok(new Response { Message = ex.Message, Status = APIStatus.SystemError, Data = null });
                }

            }

        }

        [HttpPost]
        [Route("RestoreUser")]
        public async Task<IActionResult> RestoreUser(UserModel userModel)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {

                    var userItem = db.Tb_Users.Where(i => i.UserName ==userModel.UserName && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if (clsUtility.CheckNull(userItem))
                    {
                        var resItem = db.Tb_Users.Where(i => i.ID == userModel.ID && i.Active == true && i.Enabled == false).FirstOrDefault();
                        resItem.Enabled = true;
                        db.Tb_Users.Update(resItem);
                        await db.SaveChangesAsync();
                        return Ok(new Response { Message = "Restore Successful !", Status = APIStatus.Successfull, Data = null });
                    }
                    else
                    {
                        return Ok(new Response { Message = "Can't restore your UserName is " + userModel.UserName + " Already Have !", Status = APIStatus.Error, Data = null });
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
