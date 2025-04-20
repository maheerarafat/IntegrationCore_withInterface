using IntegrationCore.IntegrationLibrary;
using IntegrationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Security.Claims;

namespace IntegrationCore.Controllers
{
    public class TarasolFilesController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;
        public TarasolFilesController(IWebHostEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {

            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
            if (_cache.Get("IsSettingsLoaded") == null || (bool)_cache.Get("IsSettingsLoaded") == false)
            {
                Cache cache = new Cache(_cache);
                IntegrationSettings.LoadSettings(_hostingEnvironment);
            }
        }
        
        [HttpPost]
        [Authorize] //////////////////Used Currently
        public FileStreamResult GetCorrFileFromTarasol(string InterfaceID)
        {
            //Expose file upload api from tarasol to upload files to ESB
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst("name")?.Value;
                if(userId.ToString() == IntegrationSettings.GetSetting("DMSUser").ToString())
                {
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Interface:" + InterfaceID, TrackInterface: true,PrintInterface:true);
                    MemoryStream jsonResult = TarasolFiles.GetStreamFileFromTarasol(InterfaceID, _cache);
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                    System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = InterfaceID,
                        Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
                    };
                    Response.Headers.Add("Content-Disposition", cd.ToString());
                    return File(jsonResult, "application/zip",InterfaceID +".zip");
                }
                else
                {
                    throw new NullReferenceException("Invalid or Expired Session");
                }
                
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        [Authorize] //////////////////Used Currently???
        public FileStreamResult GetCorFileFromTarasol(string InterfaceID)
        {
            //Expose file upload api from tarasol to upload files to ESB
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst("name")?.Value;
                if (userId.ToString() == IntegrationSettings.GetSetting("DMSUser").ToString())
                {
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Interface:" + InterfaceID, TrackInterface: true, PrintInterface: true);
                    MemoryStream jsonResult = TarasolFiles.GetStreamFileFromTarasol(InterfaceID, _cache);
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                    System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = InterfaceID,
                        Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
                    };
                    Response.Headers.Add("Content-Disposition", cd.ToString());
                    return File(jsonResult, "application/zip", InterfaceID + ".zip");
                }
                else
                {
                    throw new NullReferenceException("Invalid or Expired Session");
                }

            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw ex;
            }
        }

        //[HttpPost]
        //[Authorize] //////////////////To Maher - fix Correspondence Approve Issue
        //public FileContentResult GetCorrFileFromTarasolByUID(string CorrUID)
        //{
        //    //Expose file upload api from tarasol to upload files to ESB
        //    try
        //    {
        //        var claimsIdentity = this.User.Identity as ClaimsIdentity;
        //        var userId = claimsIdentity.FindFirst("name")?.Value;
        //        if (userId.ToString() == IntegrationSettings.GetSetting("DMSUser").ToString())
        //        {
        //            Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - CorrUID:" + CorrUID, TrackInterface: true,PrintInterface:true);
        //            MemoryStream jsonResult = TarasolFiles.GetStreamFileFromTarasolByUID(CorrUID, _cache);
        //            Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
        //            return File(jsonResult.GetBuffer(), "application/octet-stream");
        //        }
        //        else
        //        {
        //            throw new NullReferenceException("Invalid or Expired Session");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
        //        throw ex;
        //    }
        //}

        [HttpPost, DisableRequestSizeLimit]
        [Authorize]  //////////////////Used Currently
        public async Task<JsonResult> SendFilesToTarasolAsync(string InterfaceID)
        {
            //Expose file upload api from tarasol to upload files to ESB
            try
            {

                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true);
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst("name")?.Value;
                if (userId.ToString() == IntegrationSettings.GetSetting("DMSUser").ToString())
                {
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "file count :" + Request.Form.Files.Count, TrackInterface: true,PrintInterface: true);
                    List<SendFileToTarasolReturnedData> jsonResult = await TarasolFiles.SendFilesStreamToTarasolAsync(Request.Form.Files, InterfaceID);

                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                    return Json(jsonResult);
                }
                else
                {
                    throw new NullReferenceException("Invalid or Expired Session");
                }
                
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return Json(IntResultLib.GetResult(ex));
            }
        }
        
        //[HttpPost]
        //[Authorize]
        //public JsonResult GetMultiFilesFromTarasol([FromBody] FileRequest getFileRequest)
        //{
        //    //Expose file upload api from tarasol to upload files to ESB
        //    try
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
        //        List<Attachment> list = TarasolFiles.GetMultiFilesFromTarasol(getFileRequest);
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
        //        return Json(list);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
        //        throw ex;
        //    }
        //}

        //[HttpPost, DisableRequestSizeLimit]
        //[Authorize]
        //public async Task<JsonResult> SendMultiFilesToTarasolAsync([FromBody]List<Attachment> attachments, string InterfaceID)
        //{
        //    //Expose file upload api from tarasol to upload files to ESB
        //    try
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
        //        IntResult jsonResult = await TarasolFiles.SendFilesToTarasolAsync(attachments, InterfaceID);
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
        //        return Json(jsonResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
        //        return Json(IntResultLib.GetResult(ex));
        //    }
        //}

        [HttpGet]
        public RedirectResult callDMSInteface(string InterfaceUID,string Direction)
        {
            Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "InterfaceUID:" + InterfaceUID, TrackInterface: true);
            return RedirectPermanent(IntegrationSettings.GetSetting("DMSInterfaceURL").ToString() + "/#/login/" + ((Direction == "1")? "import/" : "download/") + InterfaceUID);
        }
        
        [HttpGet]
        public ActionResult DMSInterface(string SessionID, string DocUID,string Direction,string PageGUID)
        {
            
            Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "session:" + SessionID + " - Docuid:" + DocUID + " - Direction:" + Direction + " - PageGUID:" + PageGUID, TrackInterface: true);
            DownloadUploadRequest downloadUploadRequest = new DownloadUploadRequest() { SessionID = SessionID, DocUID = DocUID,PageGUID = PageGUID };
            string InterfaceUID = Guid.NewGuid().ToString();
            ViewBag.InterfaceUID = InterfaceUID;
            ViewBag.Direction = Direction;
            ViewBag.DMS_URL = IntegrationSettings.GetSetting("DMSInterfaceURL").ToString();
            TarasolFiles.setSession(downloadUploadRequest,InterfaceUID);
            return View();
        }
    }
}
