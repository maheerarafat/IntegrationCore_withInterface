using IntegrationCore.IntegrationLibrary;
using IntegrationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IntegrationCore.Controllers
{
    public class SMSController : Controller
    {
        private IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;
        public SMSController(IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache, IConfiguration config)
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

        // Send SMS message from Tarasol
        [HttpPost]
        [Authorize]
        public JsonResult SendSMS([FromBody] List<SMSObject> sMSObject)
        {
            string Objstring = Newtonsoft.Json.JsonConvert.SerializeObject(sMSObject);
            string transactionRefNo = string.Empty;
            try
            {
                IntResult jsonResult = new IntResult();
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Objstring : " + Objstring, TrackInterface: false);
                //Expose SMS Service
                if (sMSObject != null)
                {
                    if (this._hostingEnvironment.IsEnvironment("ICA"))
                    {
                        //bool EnableSend = CheckTimeEnableSendEmail(_cache.Get("TimeStartSendEmail").ToString());

                        ConfigAttribute configAttribute = new ConfigAttribute
                        {
                            consumerID = _config["ICA:notification_service:consumer_id"],
                            urlServiceNotify = _config["ICA:notification_service:url"]
                        };
                        jsonResult = SMS.SendSMSICA(sMSObject, configAttribute, out transactionRefNo);
                    }
                    else
                    {
                        jsonResult = SMS.SendSMS(sMSObject);
                    }
                }
                else
                {
                    jsonResult = IntResultLib.GetResult(2);
                }

                if (jsonResult.Code == (int)ICALibrary.Helper.Enum.responseCode.Success)
                {
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, $"End, {transactionRefNo}", TrackInterface: false);
                }
                else
                {
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, jsonResult.EMessage + $", {transactionRefNo}", BodyRequest: Objstring, ScheduleType: (int)ICALibrary.Helper.Enum.scheduleType.NotStarted, TrackInterface: false);
                }
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message + $", {transactionRefNo}", BodyRequest: Objstring, ScheduleType: (int)ICALibrary.Helper.Enum.scheduleType.NotStarted, TrackInterface: true);
                return Json(IntResultLib.GetResult(ex));
            }
        }

        /// <summary>
        /// send OTP for change password
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public IActionResult SendOTP([FromBody]ICALibrary.Models.IntUserRequest user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IntResult jsonResult = new IntResult();
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start ", TrackInterface: true);
                    ConfigAttribute configAttribute = new ConfigAttribute
                    {
                        consumerID = _config["ICA:notification_service:consumer_id"],
                        urlServiceNotify = _config["ICA:notification_service:url"]
                    };
                    jsonResult = SMS.SendOTP(user.mobile, user.ecs, configAttribute);

                    if (jsonResult.Code == (int)ICALibrary.Helper.Enum.responseCode.Success)
                    {
                        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                    }
                    else
                    {
                        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, jsonResult.EMessage, BodyRequest: user.mobile, ScheduleType: (int)ICALibrary.Helper.Enum.scheduleType.NotStarted, TrackInterface: true);
                    }
                    return Ok(Json(jsonResult));
                }
                else
                {
                    return Json(IntResultLib.GetResult(5));
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message, BodyRequest: user.mobile, ScheduleType: (int)ICALibrary.Helper.Enum.scheduleType.NotStarted, TrackInterface: true);
                return NotFound(Json(IntResultLib.GetResult(ex)));
            }
        }
    }
}
