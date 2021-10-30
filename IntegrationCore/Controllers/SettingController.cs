using System;

using Microsoft.AspNetCore.Mvc;
using IntegrationCore.Models;
using IntegrationCore.IntegrationLibrary;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;


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
                SettingModel settingModel = new SettingModel();
                settingModel.LoginUser = _cache.Get("LoginUser").ToString();
                settingModel.UserName = _cache.Get("UserName").ToString();
                settingModel.DBType = _cache.Get("DBType").ToString();
                settingModel.ConnectionString = _cache.Get("ConnectionString").ToString();
                settingModel.APIURL = _cache.Get("APIURL").ToString();
                settingModel.DataFolder = _cache.Get("DataFolder").ToString();
                settingModel.EnableLogging = Convert.ToBoolean( _cache.Get("EnableLogging").ToString());
                //settingModel.Logs = _cache.Get("Logs").ToString();
                settingModel.IntegrationDataFolder = _cache.Get("IntegrationDataFolder").ToString();
                settingModel.ESBECSUser =  _cache.Get("ESBECSUser").ToString();
                settingModel.ESBHRUser = _cache.Get("ESBHRUser").ToString();
                settingModel.ESBSMSUser = _cache.Get("ESBSMSUser").ToString();
                settingModel.MOISMSChannel = _cache.Get("MOISMSChannel").ToString();
                settingModel.MOISMSUser = _cache.Get("MOISMSUser").ToString();
                settingModel.ESBURL = _cache.Get("ESBURL").ToString();

                settingModel.ESBEmail = _cache.Get("ESBEmail").ToString();
                settingModel.ESBEmailUser = _cache.Get("ESBEmailUser").ToString();
                //settingModel.ESBEmailPassword = _cache.Get("ESBEmailPassword").ToString();

                settingModel.SiteEndPoint = _cache.Get("SiteEndPoint").ToString();

                settingModel.DefaultReference = _cache.Get("DefaultReference").ToString();

                settingModel.EntityPrefix = _cache.Get("EntityPrefix").ToString();
                settingModel.DMSInterfaceURL = _cache.Get("DMSInterfaceURL").ToString();
                settingModel.DMSUser = _cache.Get("DMSUser").ToString();
                settingModel.TokenPeriod = _cache.Get("TokenPeriod").ToString();
                settingModel.DMSInterfaceURL = _cache.Get("DMSInterfaceURL").ToString();

                settingModel.FaxServer = _cache.Get("FaxServer").ToString();
                settingModel.FaxUser = _cache.Get("FaxUser").ToString();
                settingModel.FromFaxField = _cache.Get("FromFaxField").ToString();
                settingModel.FaxPass = _cache.Get("FaxPass").ToString();

                settingModel.TarasolINTUser = _cache.Get("TarasolINTUser").ToString();
                settingModel.TarasolINTPassword = Encryption.DecryptString(_cache.Get("TarasolINTPassword").ToString());
                //settingModel.LoginPassword = Encryption.DecryptString(_cache.Get("LoginPassword").ToString()) ;
                //settingModel.Password = Encryption.DecryptString(_cache.Get("Password").ToString());
                //settingModel.ConfigPath = _cache.Get("ConfigPath").ToString();
                //settingModel.DBPassword = Encryption.DecryptString(_cache.Get("DBPassword").ToString());
                //settingModel.ESBECSPass = Encryption.DecryptString(_cache.Get("ESBECSPass").ToString());
                //settingModel.ESBHRPass = Encryption.DecryptString(_cache.Get("ESBHRPass").ToString());
                //settingModel.ESBSMSPass = Encryption.DecryptString(_cache.Get("ESBSMSPass").ToString());
                //settingModel.MOISMSPass = Encryption.DecryptString(_cache.Get("MOISMSPass").ToString());
                //settingModel.TimeStartSendEmail = _cache.Get("TimeStartSendEmail").ToString();

                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
                return Json(settingModel);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw ex;
            }
        }


        [HttpPost]
        [Authorize]
        public IActionResult EditSettings([FromBody]Models.SettingModel setting)
        {
            try
            {
                if (setting != null && ModelState.IsValid)
                {
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
                    SettingModel UpdateModel = new SettingModel();
                    if(setting.Password != "") UpdateModel.Password = Encryption.EncryptString(setting.Password);
                    UpdateModel.UserName = setting.UserName;
                    UpdateModel.DBType = setting.DBType;
                    UpdateModel.ConnectionString = setting.ConnectionString;
                    if (setting.DBPassword != "") UpdateModel.DBPassword = Encryption.EncryptString(setting.DBPassword);
                    UpdateModel.APIURL = setting.APIURL;
                    //UpdateModel.DataFolder = setting.DataFolder;
                    UpdateModel.EnableLogging = setting.EnableLogging;
                    UpdateModel.IntegrationDataFolder = setting.IntegrationDataFolder;
                    UpdateModel.ESBECSUser = setting.ESBECSUser;
                    if (setting.ESBECSPass != "") UpdateModel.ESBECSPass = Encryption.EncryptString(setting.ESBECSPass);
                    UpdateModel.ESBHRUser = setting.ESBHRUser;
                    if (setting.ESBHRPass != "") UpdateModel.ESBHRPass = Encryption.EncryptString(setting.ESBHRPass);
                    if (setting.ESBSMSPass != "") UpdateModel.ESBSMSPass = Encryption.EncryptString(setting.ESBSMSPass);
                    UpdateModel.ESBSMSUser = setting.ESBSMSUser;
                    UpdateModel.MOISMSChannel = setting.MOISMSChannel;
                    UpdateModel.MOISMSUser = setting.MOISMSUser;
                    if (setting.MOISMSPass != "") UpdateModel.MOISMSPass = Encryption.EncryptString(setting.MOISMSPass);
                    UpdateModel.ESBURL = setting.ESBURL;

                    UpdateModel.ESBEmail = setting.ESBEmail;
                    UpdateModel.ESBEmailUser = setting.ESBEmailUser;
                    if (setting.ESBEmailPassword != "") UpdateModel.ESBEmailPassword = Encryption.EncryptString(setting.ESBEmailPassword);

                    UpdateModel.SiteEndPoint = setting.SiteEndPoint;

                    UpdateModel.DefaultReference = setting.DefaultReference;

                    UpdateModel.EntityPrefix = setting.EntityPrefix;
                    UpdateModel.TokenPeriod = setting.TokenPeriod;
                    UpdateModel.DMSInterfaceURL = setting.DMSInterfaceURL;
                    UpdateModel.DMSUser = setting.DMSUser;
                    if (setting.DMSPass != "") UpdateModel.DMSPass = Encryption.EncryptString(setting.DMSPass);
                    UpdateModel.FaxUser = setting.FaxUser;
                    if (setting.FaxPass != "") UpdateModel.FaxPass = Encryption.EncryptString(setting.FaxPass);
                    UpdateModel.FaxServer = setting.FaxServer;
                    UpdateModel.FromFaxField = setting.FromFaxField;
                    
                    UpdateModel.TarasolINTUser = setting.TarasolINTUser;
                    if (setting.TarasolINTPassword != "") UpdateModel.TarasolINTPassword = Encryption.EncryptString(setting.TarasolINTPassword);
                    //UpdateModel.TimeStartSendEmail = setting.TimeStartSendEmail;
                    //UpdateModel.ConfigPath = settingModel.ConfigPath;
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
        [HttpPost]
        [Authorize]
        public IActionResult test_Connection([FromBody]SettingModel settingModel)
        {
            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
                ViewData["Message"] = "Your Settings page.";
                //"ConnectionString":"data source=localhost:1521/orcl;user id=T4_2;password=","DBPassword":"T4_2"
                IntegrationSettings.testConnection(settingModel.ConnectionString, settingModel.DBPassword);
                ViewData["EditMessage"] = "Saved Successfully";
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
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
