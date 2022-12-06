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
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace IntegrationCore.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _config;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;
        
        public HomeController(IConfiguration config, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache)
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
            string UserName = IntegrationSettings.GetSetting("LoginUser").ToString();
            string Password = IntegrationSettings.GetSetting("LoginPassword").ToString();
            return View();
        }

        [HttpGet]
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
        public JsonResult GetToken(string LoginUser,string LoginPassword)
        {
            //AM_EncryptCore.clsEncrypt clsEncrypt = new AM_EncryptCore.clsEncrypt();
            //string errorMsg = string.Empty;
            //string encodePassword = clsEncrypt.EncryptString("aa@112233", "NVS", AM_EncryptCore.clsEncrypt.OperationKind.oEncrypt, ref errorMsg);

            Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
            string UserName = IntegrationSettings.GetSetting("LoginUser").ToString();
            string Password = IntegrationSettings.GetSetting("LoginPassword").ToString();
            
            
            if (UserName == LoginUser && Encryption.DecryptString(Password) == LoginPassword)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
                UserModel userModel = new UserModel()
                {
                    Username = UserName,
                    Password = Password
                };
               string Token =  GenerateJSONWebToken(userModel);
               return Json(Token);
            }
            Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Invalid user name or password");
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
        

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult ForgetPassowrd()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Check the OTP enter by user is correct using OTP and secuirty key stored in DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CheckOTP([FromBody]ICALibrary.Models.IntUserRequest user)
        {
            try
            {
                var res = SMS.checkOTP(user.code, user.ecs);
                return Ok(Json(res));
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }

        /// <summary>
        /// Change user password after successful check OTP
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ChangePassword([FromBody] UserModel user)
        {
            try
            {
                ICALibrary.Models.IntResult res = SMS.checkOTP(user.OTP, user.Username);
                if (res.Code == 1)
                {
                    var res1 = IntegrationLibrary.User.ChangePassword(user);
                    return Ok(Json(res1));
                }
                return Ok(Json(IntResultLib.GetResult(4)));
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }

        [HttpGet]
        public IActionResult DownloadLatestLog()
        {
            try
            {
                
                string fileName = Logger.getLogFileName();
                string contentType = ICALibrary.Helper.Constant.FileContentType;
                var content = Logger.DownloadLatestLog(_hostingEnvironment, fileName);
                
                return File(content, contentType, fileName);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }


    }
}
