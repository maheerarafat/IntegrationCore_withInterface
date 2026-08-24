using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IntegrationCore.Models;
using IntegrationCore.IntegrationLibrary;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;

namespace IntegrationCore.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;
        
        public HomeController(IConfiguration config, IWebHostEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
            
            if (_cache.Get("IsSettingsLoaded") == null || (bool)_cache.Get("IsSettingsLoaded") == false)
            {
                Cache cache = new Cache(_cache);
                IntegrationSettings.LoadSettings(_hostingEnvironment);
            }
        }

        public IActionResult Index()

        {
            try
            {

            string UserName = IntegrationSettings.GetSetting("LoginUser").ToString();
            string Password = IntegrationSettings.GetSetting("LoginPassword").ToString();
            return View();
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message, PrintInterface: true); 
                return View();
                
            }
        }
        [HttpGet]
        //[Authorize]
        public JsonResult Encrypt(string value)
        {
            try
            {
                return Json(Encryption.EncryptString(value));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
        [HttpGet]
        [Authorize]
        public JsonResult Decrypt(string value)
        {
            try
            {
                return Json(Encryption.DecryptString(value));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }

        [HttpGet]
        public JsonResult GetSession()
        {
            try
            {
                return Json(Cache.GetSession(isNew:true));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }

        [HttpGet]
        public JsonResult GetToken(string LoginUser,string LoginPassword)
        {
            Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true);
            string UserName = IntegrationSettings.GetSetting("LoginUser").ToString();
            string Password = IntegrationSettings.GetSetting("LoginPassword").ToString();


            if (UserName == LoginUser && Encryption.DecryptString(Password) == LoginPassword)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                UserModel userModel = new UserModel()
                {
                    Username = UserName,
                    Password = Password
                };
               string Token = GenerateJSONWebToken(userModel);
               return Json(Token);
            }
            Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Invalid user name or password");
            return Json("invalid user name or password");
        }
        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        [Authorize]
        public IActionResult EditSettings()
        {
            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true);
                ViewData["Message"] = "Your Settings page.";
                SettingModel settingModel = new SettingModel();
                settingModel.LoginUser = IntegrationSettings.GetSetting("LoginUser").ToString();
                settingModel.LoginPassword = Encryption.DecryptString(IntegrationSettings.GetSetting("LoginPassword").ToString()) ;
                settingModel.Password = Encryption.DecryptString(IntegrationSettings.GetSetting("Password").ToString());
                settingModel.UserName = IntegrationSettings.GetSetting("UserName").ToString();
                //settingModel.ConfigPath = IntegrationSettings.GetSetting("ConfigPath").ToString();
                settingModel.DBType = IntegrationSettings.GetSetting("DBType").ToString();
                settingModel.ConnectionString = IntegrationSettings.GetSetting("ConnectionString").ToString();
                settingModel.DBPassword = Encryption.DecryptString(IntegrationSettings.GetSetting("DBPassword").ToString());
                settingModel.APIURL = IntegrationSettings.GetSetting("APIURL").ToString();
                settingModel.DataFolder = IntegrationSettings.GetSetting("DataFolder").ToString();
                settingModel.EnableLogging = Convert.ToBoolean( IntegrationSettings.GetSetting("EnableLogging").ToString());
                settingModel.Logs = IntegrationSettings.GetSetting("Logs").ToString();
                settingModel.IntegrationDataFolder = IntegrationSettings.GetSetting("IntegrationDataFolder").ToString();

                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                return View(settingModel);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw;
            }
            
        }
        [HttpPost]
        [Authorize]
        public IActionResult EditSettings(SettingModel settingModel)
        {
            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true);
                ViewData["Message"] = "Your Settings page.";
                SettingModel UpdateModel = new SettingModel();
                UpdateModel.LoginUser = settingModel.LoginUser;
                UpdateModel.LoginPassword = Encryption.EncryptString(settingModel.LoginPassword);
                UpdateModel.Password = Encryption.EncryptString(settingModel.Password);
                UpdateModel.UserName = settingModel.UserName;
                //UpdateModel.ConfigPath = settingModel.ConfigPath;
                UpdateModel.DBType = settingModel.DBType;
                UpdateModel.ConnectionString = settingModel.ConnectionString;
                UpdateModel.DBPassword = Encryption.EncryptString(settingModel.DBPassword);
                UpdateModel.APIURL = settingModel.APIURL;
                UpdateModel.DataFolder = settingModel.DataFolder;
                UpdateModel.EnableLogging = settingModel.EnableLogging;
                UpdateModel.Logs = settingModel.Logs;
                UpdateModel.IntegrationDataFolder = settingModel.IntegrationDataFolder;
                IntegrationSettings.SetSetting(settingModel);
                ViewData["EditMessage"] = "Saved Successfully";
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                return View(UpdateModel);
            }
            catch (Exception ex)
            {

                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw;
            }
            
            
        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult ForgetPassowrd()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Check the OTP enter by user is correct using OTP and secuirty key stored in DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        //[HttpPost]
        //public IActionResult CheckOTP([FromBody] IntUserRequest user)
        //{
        //    try
        //    {
        //        var res = SMS.checkOTP(user.code, user.ecs);
        //        return Ok(Json(res));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
        //        return NotFound(Json(IntResultLib.GetResult(ex)));
        //    }
        //}

        /// <summary>
        /// Change user password after successful check OTP
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        //[HttpPost]
        //public IActionResult ChangePassword([FromBody] UserModel user)
        //{
        //    try
        //    {
        //        IntResult res = SMS.checkOTP(user.OTP, user.Username);
        //        if(res.Code == 1)
        //        {
        //            var res1 = IntegrationLibrary.User.ChangePassword(user);
        //            return Ok(Json(res1));
        //        }
        //        return Ok(Json(IntResultLib.GetResult(4))); 
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
        //        return NotFound(Json(IntResultLib.GetResult(ex)));
        //    }
        //}
        //public static void RedirectWithData([FromBody] GetFileRequest getFileRequest , string url)
        //{
        //    HttpResponse response = HttpContext.Current.Response;
        //    response.Clear();

        //    StringBuilder s = new StringBuilder();
        //    s.Append("<html>");
        //    s.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
        //    s.AppendFormat("<form name='form' action='{0}' method='post'>", url);

        //    s.AppendFormat("<input type='hidden' name='{0}' value='{1}' />", "DocUID", getFileRequest.DocUID);

        //    s.Append("</form></body></html>");
        //    response.Write(s.ToString());
        //    response.End();
        //}
    }
}
