using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using IntegrationCore.IntegrationLibrary;
using IntegrationCore.Models;

namespace IntegrationCore.Controllers
{
    public class JobController : Controller
    {

        private IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;

        public JobController(IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache, IConfiguration config)
        {
            this._config = config;
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetLogJobESBSuccuss()
        {
            try
            {
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
                var data = Jobs.GetLogJobESBSuccuss(pageNumber, pageSize, ref recordsTotal);

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
        [Authorize]
        public ActionResult GetLogJobESBFaild()
        {
            try
            {
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
                var data = Jobs.GetLogJobESBFaild(pageNumber, pageSize, ref recordsTotal);

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
        [Authorize]
        public ActionResult GetLogJobESBWait()
        {
            try
            {
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
                var data = Jobs.GetLogJobESBWait(pageNumber, pageSize, ref recordsTotal);

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
        [Authorize]
        public ActionResult GetLogJobESBInProgress()
        {
            try
            {
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
                var data = Jobs.GetLogJobESBInProgress(pageNumber, pageSize, ref recordsTotal);

                //Returning Json Data
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }


    }
}
