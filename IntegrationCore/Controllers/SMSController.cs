using IntegrationCore.IntegrationLibrary;
using IntegrationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.Controllers
{
    public class SMSController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;
        public SMSController(IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {

            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
            if (_cache.Get("IsSettingsLoaded") == null || (bool)_cache.Get("IsSettingsLoaded") == false)
            {
                Cache cache = new Cache(_cache);
                IntegrationSettings.LoadSettings(_hostingEnvironment);
            }
        }
       
        // Send SMS message from Tarasol
        [HttpPost]
        [Authorize]
        public JsonResult SendSMS([FromBody]List<SMSObject> sMSObject)
        {
            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true);
                string Objstring = Newtonsoft.Json.JsonConvert.SerializeObject(sMSObject);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Objstring : " + Objstring, TrackInterface: Logger.EnableLoggingInfo);
                //Expose SMS Service
                object jsonResult = SMS.SendSMS(sMSObject);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return Json(IntResultLib.GetResult(ex));
            }
        }
        //[HttpPost]
        ////[Authorize]
        //public IActionResult SendOTP([FromBody] IntUserRequest user)
        //{
        //    try
        //    {
        //        string Objstring = Newtonsoft.Json.JsonConvert.SerializeObject(user);
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Objstring : " + Objstring);
        //        object jsonResult = SMS.SendOTP(user.mobile, user.ecs);

        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
        //        return Ok(Json(jsonResult));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
        //        throw ex;
        //    }
        //}
    }
}
