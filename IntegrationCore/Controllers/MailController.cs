using System;
using Microsoft.AspNetCore.Mvc;
using IntegrationCore.IntegrationLibrary;
using IntegrationCore.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft;
using System.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;

namespace IntegrationCore.Controllers
{
    public class MailController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;
        public MailController(IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {

            //Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
            if (_cache.Get("IsSettingsLoaded") == null || (bool)_cache.Get("IsSettingsLoaded") == false)
            {
                Cache cache = new Cache(_cache);
                IntegrationSettings.LoadSettings(_hostingEnvironment);
            }
            //Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
        }

        [HttpPost]
        [Authorize]
        public JsonResult SendEmail([FromBody] Models.Mail mail)
        {
            try
            {
                string objStr = Newtonsoft.Json.JsonConvert.SerializeObject(mail);
                bool EnableLoggingInfo = (bool)_cache.Get("EnableLoggingInfo");
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Obj : " + objStr, TrackInterface: EnableLoggingInfo);
                object jsonResult = IntegrationLibrary.Mail.SendEmail(mail);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return Json(IntResultLib.GetResult(ex));
            }
        }

        

    }
}
