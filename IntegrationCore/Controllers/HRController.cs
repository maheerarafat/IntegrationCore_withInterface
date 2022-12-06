using System;
using Microsoft.AspNetCore.Mvc;
using IntegrationCore.IntegrationLibrary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using IntegrationCore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

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

        public IActionResult Add()
        {
            return View();
        }
        public JsonResult GetDepartments()
        {
            return Json(Departments.GetDepartments());
        }

        // Tarasol will call this API to query user information based on military number or Identity number
        //[HttpPost]
        //[Authorize]
        //public JsonResult GetUserInfo([FromBody] SearchObject searchObject)
        //{
        //    try
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
        //        object jsonResult = IntegrationLibrary.User.GetUserInfo(searchObject);
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
        //        return Json(jsonResult);

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
        //        return Json(IntResultLib.GetResult(ex));
        //    }

        //}

        // Tarasol will call this API to query department information based on department code
        //[HttpPost]
        //[Authorize]
        //public JsonResult GetDepartmentInfo([FromBody]SearchObject searchObject)
        //{
        //    try
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
        //        object jsonResult = IntegrationLibrary.Departments.GetDepartmentInfo(searchObject);
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
        //        return Json(jsonResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
        //        return Json(IntResultLib.GetResult(ex));
        //    }
        //}


        [HttpPost]
        public ActionResult GetHistoryUpdateHR()
        {
            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int pageNumber = start != null ? Convert.ToInt32(start) : 0;
                if (pageNumber != 0)
                {
                    pageNumber = pageNumber / pageSize;
                }

                //total number of rows count 
                int recordsTotal = 0;

                //Paging 
                var data = SyncHR.GetHistoryUpdateHR(pageNumber, pageSize, ref recordsTotal);


                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");

                //Returning Json Data
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }


        [HttpPost]
        public ActionResult RunProcedureSyncManual(string id)
        {
            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");

                var data =  Json(SyncHR.RunProcedureSync(id));

                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
                return data;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }



        [HttpPost]
        public ActionResult GetActivityLogHR()
        {

            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int pageNumber = start != null ? Convert.ToInt32(start) : 0;

                if (pageNumber != 0)
                {
                    pageNumber = pageNumber / pageSize;
                }


                //total number of rows count 
                int recordsTotal = 0;

                //Paging 
                var data = SyncHR.GetActivityLogHR(pageNumber, pageSize, ref recordsTotal);

                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
                //Returning Json Data
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }


        [HttpPost]
        public ActionResult GetActivityLogErrorHR()
        {

            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int pageNumber = start != null ? Convert.ToInt32(start) : 0;

                if (pageNumber != 0)
                {
                    pageNumber = pageNumber / pageSize;
                }


                //total number of rows count 
                int recordsTotal = 0;

                //Paging 
                var data = SyncHR.GetActivityLogErrorHR(pageNumber, pageSize, ref recordsTotal);


                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
                //Returning Json Data
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }


        [HttpGet]
        public IActionResult GetUserDetails(string num, int type)
        {
            try
            {

                var data = SyncHR.GetUserDetails(num, type);

                //Returning Json Data
                return Ok(Json(data));
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }


        [HttpPost]
        public IActionResult GetUserMobileLookup([FromBody] UserModel user)
        {
            try
            {

                var data = SyncHR.GetUserMobileLookup(user.Username);

                //Returning Json Data
                return Ok(Json(data));
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }


        [HttpPost]
        public IActionResult UpdateOldECS([FromBody]ICALibrary.Models.IntUserRequest user)
        {
            try
            {
                
                var data = SyncHR.UpdateUser(user);

                //Returning Json Data
                return Ok(Json(data));
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }


        [HttpPost]
        public IActionResult UpdateLokupTable([FromBody]ICALibrary.Models.IntUserRequest user)
        {
            try
            {

                //Returning Json Data
                return Ok(Json(""));
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }

    }
}
