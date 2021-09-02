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
                    var userItem = db.Tb_Users.Where(i => i.UserName == registerUser.UserName && i.Active == true && i.Enabled == true).FirstOrDefault();
                    if(userItem != null)
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
                        if (registerUser.PhotoUrl != null || registerUser.PhotoUrl != "String")
                        {
                            imageBytes = await utility.FileToByteArray(registerUser.PhotoUrl);                           
                        }
                        var item = new Tb_User
                        {
                            PhotoUrl = utility.SavePathFile("User.jpg", "UploadUser", Convert.ToBase64String(imageBytes), _hostingEnvironment),
                            Password = encryptedPassword,
                            SaltAes = tempCryptography.PlainText,
                            SaltHash = tempCryptography.SALT,
                            UserName=registerUser.UserName,
                            PersonalContactNumber=registerUser.PersonalContactNumber,
                            OtherContactNumber=registerUser.OtherContactNumber,
                            Sex=registerUser.Sex,
                            FirstName=registerUser.FirstName,
                            LastName=registerUser.LastName,
                            Email=registerUser.Email,
                            UserType=registerUser.UserType
                        };
                        db.Tb_Users.Add(item);
                        db.SaveChanges();
                        return Ok(new SaveResponseModel { Message = "Save Successful", Status = APIStatus.Successfull, Data = item });
                    }
                    
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
                    var OldHASHValue = userItem.Password;
                    CryptographyClass tempCryptography = new CryptographyClass()
                    {
                        Text = string.Format("{0}{1}", UserName.ToLower().Trim(), Password.Trim()),
                        SALT = userItem.SaltHash
                    };
                    if(userItem != null)
                    {
                        userModel = new UserModel
                        {
                            ID = userItem.ID,
                            UserID=userItem.UserID,
                            UserName=userItem.UserName,
                            FirstName=userItem.FirstName,
                            LastName=userItem.LastName,
                            Email=userItem.Email,
                            Password= Password,
                            UserType=userItem.UserType,
                            Sex=userItem.Sex,
                            PersonalContactNumber=userItem.PersonalContactNumber,
                            OtherContactNumber=userItem.OtherContactNumber,
                            PhotoUrl=userItem.PhotoUrl
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
                    }
                    return Ok(new Response { Message = "Login Successful", Status = APIStatus.Successfull, Data = userModel });
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

                            return Ok(new Response { Message = "Change Password Detected.", Status = APIStatus.Successfull,Data= userModel });
                        }
                        else
                        {
                            return Ok(new Response { Message = "Change Password Fail.", Status = APIStatus.Error });
                        }


                    }
                    //return BadRequest("Posted invalid data.");
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
                        var userData = db.Tb_Users.Where(us => ( us.Email == Email || us.UserName == Email ) /*&& us.UserId == model.UserId*/ && us.Active.Equals(true)).FirstOrDefault();
                       

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
                        _emailSender.SendEmailAsync(userData.Email, "SPSH New Password ", "Please use this code to login password for User account <br/> " +
                            "Here is your code :  " + newPasswrod);
                        return Ok(new Response { Message = "ForgetPassword Detected.", Status = APIStatus.Successfull });
                    }
                    //return BadRequest("Posted invalid data.");
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
        public async Task<IActionResult> GetAllUser(TableSearchClass tempData)
        {
            using (OnlineContext db = new OnlineContext())
            {
                try
                {
                    var userModel = new UserModel();
                    var resultTemp = db.Tb_Users.AsEnumerable()
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
                            resultTemp = resultTemp.Where(item => (string.IsNullOrEmpty(item.FirstName) ? false : item.FirstName.IndexOf(tempData.filter) != -1 ? true : item.FirstName.Contains(tempData.filter))).ToList();

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
                           ID=x.ID,
                           UserID=x.UserID,
                           FirstName=x.FirstName,
                           LastName=x.LastName,
                           Email=x.Email,
                           UserName=x.UserName,
                           Sex=x.Sex,
                           UserType=x.UserType,
                           PersonalContactNumber=x.PersonalContactNumber,
                           OtherContactNumber=x.OtherContactNumber
                        }).ToList();
                        var temp = new Temp
                        {
                            total = total != null ? total : null,
                            per_page = tempData.per_page != null ? tempData.per_page : null,
                            current_page = tempData.page != null ? tempData.page : null,
                            last_page = last_page != null ? last_page : null,
                            //next_page_url = tempData.page < last_page ? string.Format("{0}?page={1}", Get.BaseURL(), tempData.page + 1) : null,
                            //prev_page_url = tempData.page > 1 ? string.Format("{0}?page={1}", Get.BaseURL(), tempData.page - 1) : null,
                            from = tempData.page >= 1 ? ((tempData.per_page * (tempData.page - 1)) + 1) : null,
                            to = tempData.page >= 1 ? ((tempData.page - 1) * tempData.per_page) + resultTemp.Count() : null,
                            data = list,
                        };

                        //return Json(temp);
                        //  return Json(temp.data);
                        return Ok(new ResponseList { Message = "List", Status = APIStatus.Successfull, Data = temp });
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


    }
}
