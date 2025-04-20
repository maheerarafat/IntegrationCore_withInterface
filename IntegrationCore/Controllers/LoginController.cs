using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using IntegrationCore.Models;
using System.Text;
using IntegrationCore.IntegrationLibrary;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;

namespace IntegrationCore.Controllers
{
    
    public class LoginController : Controller
    {
        private IConfiguration _config;
        private static IMemoryCache _cache;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public LoginController(IConfiguration config, IMemoryCache memoryCache, IWebHostEnvironment _hostingE)
        {
            _config = config;
            _cache = memoryCache;
            _hostingEnvironment = _hostingE;
            if (_cache.Get("IsSettingsLoaded") == null || (bool)_cache.Get("IsSettingsLoaded") == false)
            {
                Cache cache = new Cache(_cache);
                IntegrationSettings.LoadSettings(_hostingEnvironment);
            } 
                
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult JWTLogin([FromHeader]string UserName, [FromHeader]string Password)
        {
            Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true);
            IActionResult response = Unauthorized();
            UserModel userModel = new UserModel
            {
                Username = UserName,
                Password = Password
            };
            var user = AuthenticateUser(userModel);
            Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Username=" + UserName /*+ "-Password=" + Password*/, TrackInterface: Logger.EnableLoggingInfo,PrintInterface:Logger.EnableLoggingInfo);
            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }
            else
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Username="+UserName /*+ "-Password="+ Password*/, TrackInterface: true);
            }
            Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
            return response;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //Claim claim = new Claim("name", userInfo.Username);
            IEnumerable<Claim> claims = new Claim[] { new Claim("name", userInfo.Username) };
            JwtSecurityToken token;
            if (!string.IsNullOrEmpty(IntegrationSettings.GetSetting("TokenPeriod").ToString()))
            {
               token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(Convert.ToDouble(IntegrationSettings.GetSetting("TokenPeriod"))), // comment this row to make Token unexpired
              signingCredentials: credentials);
            }
            else
            {
               token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              signingCredentials: credentials);
            }
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            //Validate the User Credentials  
            if (login.Username == (string)IntegrationSettings.GetSetting("UserName") && login.Password == Encryption.DecryptString( (string)IntegrationSettings.GetSetting("Password")))
            {
                user = new UserModel { Username = login.Username };
            }
            else if (login.Username == (string)IntegrationSettings.GetSetting("DMSUser") && login.Password == Encryption.DecryptString((string)IntegrationSettings.GetSetting("DMSPass")))
            {
                user = new UserModel { Username = login.Username };
            }
            return user;
        }
    }
}