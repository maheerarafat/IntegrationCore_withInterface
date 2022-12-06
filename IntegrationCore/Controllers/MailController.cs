
using System;
using Microsoft.AspNetCore.Mvc;
using IntegrationCore.IntegrationLibrary;
using IntegrationCore.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft;
using System.Xml;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;

namespace IntegrationCore.Controllers
{
    public class MailController : Controller
    {
        private IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;
        public MailController(IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache, IConfiguration config)
        {

            //Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
            this._config = config;
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
                if (ModelState.IsValid)
                {
                    IntResult jsonResult = new IntResult();
                    // string objStr = Newtonsoft.Json.JsonConvert.SerializeObject(mail);
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: false);

                    var task = new[]
                        {
                        Task.Run(
                            ()=>
                            {
                                bool EnableSendEmail = CheckTimeEnableSendEmail(_cache.Get("TimeStartSendEmail").ToString());                                

                                if (this._hostingEnvironment.IsEnvironment("ICA"))
                                {
                                    ConfigAttribute configAttribute = new ConfigAttribute
                                    {
                                        consumerID = _config["ICA:notification_service:consumer_id"],
                                        urlServiceNotify = _config["ICA:notification_service:url"],
                                        senderMail = _config["ICA:SenderMail"]
                                    };
                                    if (EnableSendEmail)
                                        jsonResult = IntegrationLibrary.Mail.SendEmailICA(mail, configAttribute);
                                }
                                else
                                {
                                    jsonResult = IntegrationLibrary.Mail.SendEmail(mail);
                                }

                                if (EnableSendEmail)
                                {
                                    if (jsonResult.Code == (int)ICALibrary.Helper.Enum.responseCode.Success)
                                    {
                                        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: false);
                                    }
                                    else
                                    {
                                        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, jsonResult.EMessage , TrackInterface: false);
                                    }
                                }
                            }
                       )
                    };
                    return Json(IntResultLib.GetResult(1));
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

        /// <summary>
        ///  Import correspondence to middle folder
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
        public JsonResult ReceiveEmail([FromBody]ICALibrary.Models.Email email)
        {
            try
            {
                if (ModelState.IsValid)
                {                    
                    string EmailDistributorStr = _cache.Get("EmailDistributors").ToString();
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true);
                    //Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Payload :" + JsonConvert.SerializeObject(email));
                    //IntResult res = IntegrationLibrary.Mail.ImportEmailToFolder(email, EmailDistributorStr);
                    IntResult res = IntResultLib.GetResult(1);
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true);
                    return Json(res);
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

        public bool CheckTimeEnableSendEmail(string startTime)
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                string settingFormattime = currentTime.ToString("dd/MM/yyyy") + " " + startTime;
                string format = "dd/MM/yyyy HH:mm";
                CultureInfo culture = new CultureInfo("ar-AE");
                DateTime settingTime = DateTime.ParseExact(settingFormattime, format, culture);

                return ((currentTime > settingTime) ? true : false);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
