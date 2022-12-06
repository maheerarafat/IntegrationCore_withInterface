using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    public class ReportController : Controller
    {
        private IConfiguration _config;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;

        public ReportController(IConfiguration config, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache)
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

        [HttpPost]
        [Authorize]
        public ActionResult GetReportJobESB([FromBody]ReportFilter report)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(report.Date))
                {
                    report.Date = DateTime.Now.ToString("yyyy-MM-dd");
                }

                // Report 
                var data = Jobs.GetReportJobESB(report);

                //Returning Json Data
                return Json( data );
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }
    }
}
