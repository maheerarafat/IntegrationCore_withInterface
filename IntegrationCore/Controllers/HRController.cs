using System;
using Microsoft.AspNetCore.Mvc;
using IntegrationCore.IntegrationLibrary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using IntegrationCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace IntegrationCore.Controllers
{

    public class HRController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;
        public HRController(IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {
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
        //public JsonResult GetDepartments()
        //{
        //    return Json(Departments.GetDepartments());
        //}

        // Tarasol will call this API to query user information based on military number or Identity number
        [HttpPost]
        [Authorize]
        public JsonResult GetUserInfo([FromBody] SearchObject searchObject)
        {
            try
            {
                string objStr = Newtonsoft.Json.JsonConvert.SerializeObject(searchObject);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Obj : " + objStr);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Obj : " + objStr);
                object jsonResult = IntegrationLibrary.User.GetUserInfo(searchObject);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
                return Json(jsonResult);
                
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return Json(IntResultLib.GetResult(ex));
            }
            
        }
        
        // Tarasol will call this API to query department information based on department code
        [HttpPost]
        [Authorize]
        public JsonResult GetDepartmentInfo([FromBody]SearchObject searchObject)
        {
            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
                object jsonResult = Departments.GetDepartmentInfo(searchObject);
                string objStr = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End : result : " + objStr);
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return Json(IntResultLib.GetResult(ex));
            }
        }

        //[HttpPost]
        //public IActionResult GetUserMobileLookup([FromBody] UserModel num)
        //{
        //    try
        //    {

        //        var data = SyncHR.GetUserMobileLookup(num.Username);

        //        //Returning Json Data
        //        return Ok(Json(data));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
        //        return NotFound(Json(IntResultLib.GetResult(ex)));
        //    }
        //}

    }
}
