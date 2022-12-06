using IntegrationCore.IntegrationLibrary;
using IntegrationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IntegrationCore.Controllers
{
    public class TarasolFilesController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;
        public TarasolFilesController(IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {

            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
            if (_cache.Get("IsSettingsLoaded") == null || (bool)_cache.Get("IsSettingsLoaded") == false)
            {
                Cache cache = new Cache(_cache);
                IntegrationSettings.LoadSettings(_hostingEnvironment);
            }
        }
        /// <summary>
        /// Streaming
        /// </summary>
        /// <param name="getFileRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public FileContentResult GetCorrFileFromTarasol(string InterfaceID)
        {
            //Expose file upload api from tarasol to upload files to ESB
            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Interface:" + InterfaceID);
                MemoryStream jsonResult = TarasolFiles.GetStreamFileFromTarasol(InterfaceID, _cache);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
                return File(jsonResult.GetBuffer(), "application/octet-stream");
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw ex;
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Authorize]  //////////////////Used Currently
        public async Task<JsonResult> SendFilesToTarasolAsync(string InterfaceID)
        {
            //Expose file upload api from tarasol to upload files to ESB
            try
            {

                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");

                List<SendFileToTarasolReturnedData> jsonResult = await TarasolFiles.SendFilesStreamToTarasolAsync(Request.Form.Files, InterfaceID);

                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return Json(IntResultLib.GetResult(ex));
            }
        }
        /// <summary>
        /// Base64
        /// </summary>
        /// <param name="getFileRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult GetMultiFilesFromTarasol([FromBody] FileRequest getFileRequest)
        {
            //Expose file upload api from tarasol to upload files to ESB
            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
                List<Attachment> list = TarasolFiles.GetMultiFilesFromTarasol(getFileRequest);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
                return Json(list);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw ex;
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Authorize]
        public async Task<JsonResult> SendMultiFilesToTarasolAsync([FromBody]List<Attachment> attachments, string InterfaceID)
        {
            //Expose file upload api from tarasol to upload files to ESB
            try
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
                IntResult jsonResult = await TarasolFiles.SendFilesToTarasolAsync(attachments, InterfaceID);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return Json(IntResultLib.GetResult(ex));
            }
        }

        [HttpGet]
        public RedirectResult callDMSInteface(string InterfaceUID, string Direction)
        {
            Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "InterfaceUID:" + InterfaceUID);
            return RedirectPermanent(IntegrationSettings.GetSetting("DMSInterfaceURL").ToString() + "/#/login/" + ((Direction == "1") ? "import/" : "download/") + InterfaceUID);
        }
        [HttpGet]
        public ActionResult DMSInterface(string SessionID, string DocUID, string Direction, string PageGUID)
        {
            Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "session:" + SessionID + " - Docuid:" + DocUID + " - Direction:" + Direction + " - PageGUID:" + PageGUID);
            DownloadUploadRequest downloadUploadRequest = new DownloadUploadRequest() { SessionID = SessionID, DocUID = DocUID, PageGUID = PageGUID };
            string InterfaceUID = Guid.NewGuid().ToString();
            ViewBag.InterfaceUID = InterfaceUID;
            ViewBag.Direction = Direction;
            ViewBag.DMS_URL = IntegrationSettings.GetSetting("DMSInterfaceURL").ToString();
            Cache.SetCache(InterfaceUID, JsonConvert.SerializeObject(downloadUploadRequest));
            return View();
        }
    }
}
