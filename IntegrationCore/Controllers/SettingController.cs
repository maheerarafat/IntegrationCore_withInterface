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
    public class SettingController : Controller
    {
        private IConfiguration _config;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;
        
        public SettingController(IConfiguration config, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache)
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
            return View();
        }

        
        [Authorize]
        public IActionResult GetSettings()
        {
            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
                Setting settingModel = new Setting();
                settingModel.LoginUser = _cache.Get("LoginUser").ToString();
                settingModel.LoginPassword = Encryption.DecryptString(_cache.Get("LoginPassword").ToString()) ;
                settingModel.Password = Encryption.DecryptString(_cache.Get("Password").ToString());
                settingModel.UserName = _cache.Get("UserName").ToString();
                //settingModel.ConfigPath = _cache.Get("ConfigPath").ToString();
                settingModel.DBType = _cache.Get("DBType").ToString();
                settingModel.ConnectionString = _cache.Get("ConnectionString").ToString();
                settingModel.DBPassword = Encryption.DecryptString(_cache.Get("DBPassword").ToString());
                settingModel.APIURL = _cache.Get("APIURL").ToString();
                settingModel.DataFolder = _cache.Get("DataFolder").ToString();
                settingModel.EnableLogging = Convert.ToBoolean( _cache.Get("EnableLogging").ToString());
                //settingModel.Logs = _cache.Get("Logs").ToString();
                settingModel.IntegrationDataFolder = _cache.Get("IntegrationDataFolder").ToString();
                settingModel.TimeStartSendEmail = _cache.Get("TimeStartSendEmail").ToString();
                settingModel.ClassificationId = _cache.Get("ClassificationId").ToString();
                settingModel.EntityPrefix = _cache.Get("EntityPrefix").ToString();

                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
                return Json(settingModel);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw ex;
            }
        }

        [Authorize]
        public IActionResult ManualSyncImporting()
        {
            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");

                string corrImportResult = Correspondence.CallCorrespondecesImport();
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, $"End : {corrImportResult}");
                return Json(corrImportResult);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw ex;
            }
        }


        [HttpPost]
        [Authorize]
        public IActionResult EditSettings([FromBody]Models.Setting setting)
        {
            try
            {
                if (setting != null && ModelState.IsValid)
                {
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
                    Models.Setting UpdateModel = new Models.Setting();
                    UpdateModel.Password = Encryption.EncryptString(setting.Password);
                    UpdateModel.UserName = setting.UserName;
                    //UpdateModel.ConfigPath = settingModel.ConfigPath;
                    UpdateModel.DBType = setting.DBType;
                    UpdateModel.ConnectionString = setting.ConnectionString;
                    UpdateModel.DBPassword = Encryption.EncryptString(setting.DBPassword);
                    UpdateModel.APIURL = setting.APIURL;
                    UpdateModel.DataFolder = setting.DataFolder;
                    UpdateModel.EnableLogging = setting.EnableLogging;
                    UpdateModel.IntegrationDataFolder = setting.IntegrationDataFolder;
                    UpdateModel.TimeStartSendEmail = setting.TimeStartSendEmail;
                    UpdateModel.ClassificationId = setting.ClassificationId;
                    UpdateModel.EntityPrefix = setting.EntityPrefix;
                    IntegrationSettings.SetSetting(UpdateModel);
                    Cache cache = new Cache(_cache);
                    IntegrationSettings.LoadSettings(_hostingEnvironment);
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
                }
                return Json(IntResultLib.GetResult(200));
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw ex;
            }
        }
    }
}
