using System;
using Microsoft.AspNetCore.Mvc;
using IntegrationCore.IntegrationLibrary;
using IntegrationCore.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace IntegrationCore.Controllers
{
    public class CorrController : Controller
    {
        private IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;


        public CorrController(IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache, IConfiguration config)
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


        // Import correspondence to middle folder
        [HttpPost]
        [Authorize]
        public JsonResult CorrespondenceImport([FromBody]TarasolDocument tarasolDocument)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true);
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Payload :" + JsonConvert.SerializeObject(tarasolDocument));
                    TarasolDocument eSBDocument = tarasolDocument;
                    //read post data correspondence and files
                    object jsonResult = Correspondence.ImportToFolder(eSBDocument);
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                    return Json(jsonResult);
                }
                else
                {
                    return Json(IntResultLib.GetResult(5));
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Code Error:" + ex.HResult + ", Messgae" + ex.Message, TrackInterface: true);
                return Json(IntResultLib.GetResult(ex));
            }

        }

        // Push Notification From ESB to Tarasol.
        // Core will write Acknowledgment : Correspondence No , ReadBy, GlobalID , date  
        [HttpPost]
        [Authorize]
        public JsonResult DeliveryReport([FromBody]DeliveryRequest acknowledegment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true);
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Payload :" + JsonConvert.SerializeObject(acknowledegment), TrackInterface: true);
                    Correspondence correspondence = new Correspondence();
                    object jsonResult = correspondence.WriteAck(acknowledegment);
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                    return Json(jsonResult);
                }
                else
                {
                    return Json(IntResultLib.GetResult(5));
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message, TrackInterface: true);
                return Json(IntResultLib.GetResult(ex));
            }

        }


        //Tarasol will call this API to trigger export scanning in the itegration layer
        [HttpPost]
        [Authorize]
        public JsonResult ScanForExport([FromBody]ExportCorr[] scanRequest)
        {
            string objStr = Newtonsoft.Json.JsonConvert.SerializeObject(scanRequest);
            try
            {
                object result;
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Obj : " + objStr, TrackInterface: true);
                if (this._hostingEnvironment.IsEnvironment("ICA"))
                {
                    ConfigAttribute configAttribute = new ConfigAttribute
                    {
                        consumerID = _config["ICA:tarasol_service_contract:consumer_id"],
                        urlServiceNotify = _config["ICA:notification_service:url"],
                        urlServiceESB =_config["ICA:tarasol_service_contract:url"],
                        senderMail = _config["ICA:SenderMail"],
                        maxSizeFileCompress = long.Parse(_config["ICA:MaxSizeFileCompress"])
                    };
                    var task = new[]
                    {
                        Task.Run(()=>  Correspondence.ExportFromFolderICA(_hostingEnvironment, scanRequest, configAttribute))
                    };
                }
               
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                return Json(IntResultLib.GetResult(200));
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, objStr, TrackInterface: true);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message, TrackInterface: true);
                return Json(IntResultLib.GetResult(ex));
            }
        }



        [HttpPost]
        [Authorize]
        public JsonResult ScanForAck([FromBody]ExportCorr[] scanRequest)
        {
            string objStr = Newtonsoft.Json.JsonConvert.SerializeObject(scanRequest);
            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Obj : " + objStr, TrackInterface: true);

                //scan ack folder to push read notification to esb
                // will expose ESB service for push notification
                if (this._hostingEnvironment.IsEnvironment("ICA"))
                {
                    ConfigAttribute configAttribute = new ConfigAttribute
                    {
                        consumerID = _config["ICA:tarasol_service_contract:consumer_id"],
                        urlServiceESB = _config["ICA:tarasol_service_contract:url"]
                    };
                    var task = new[]
                    {
                        Task.Run(()=>   Correspondence.SendAckICA(_hostingEnvironment, scanRequest, configAttribute))
                    };
                }
               
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                return Json(IntResultLib.GetResult(1));
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, objStr, TrackInterface: true);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message, TrackInterface: true);
                return Json(IntResultLib.GetResult(ex));
            }
        }


    }

}