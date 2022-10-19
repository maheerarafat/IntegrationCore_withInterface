using System;
using Microsoft.AspNetCore.Mvc;
using IntegrationCore.IntegrationLibrary;
using IntegrationCore.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft;
using System.Xml;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Diagnostics;

namespace IntegrationCore.Controllers
{
    public class CorrController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;
        public CorrController(IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {
           
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
            string objStr = Newtonsoft.Json.JsonConvert.SerializeObject(tarasolDocument);
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst("name")?.Value;
                //bool EnableLoggingInfo = (bool)_cache.Get("EnableLoggingInfo");
                if (userId.ToString() == IntegrationSettings.GetSetting("UserName").ToString())
                {
                    
                    if(string.IsNullOrEmpty(objStr) || objStr == "null") throw new NullReferenceException("Invalid Input format");
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Obj : " + objStr,TrackInterface: Logger.EnableLoggingInfo);
                    TarasolDocument eSBDocument;
                    try
                    {
                        eSBDocument = tarasolDocument;
                    }
                    catch (Exception)
                    {

                        throw new NullReferenceException("Invalid Input format");
                    }
                    
                    //read post data correspondence and files
                    object jsonResult = Correspondence.ImportToFolder(eSBDocument);
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface:true, PrintInterface: true);
                    return Json(jsonResult);
                }
                else
                {
                    throw new NullReferenceException("Invalid or Expired Session");
                }
                
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message, PrintInterface: true);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Object: " + objStr, PrintInterface: true);
                return Json(IntResultLib.GetResult(ex));
            }
          
        }
        

        // Push Notification From ESB to Tarasol.
        // Core will write Acknowledgment : Correspondence No , ReadBy, GlobalID , date  
        [HttpPost]
        [Authorize]
        public JsonResult DeliveryReport([FromBody]DeliveryRequest acknowledegment)
        {
            string objStr = Newtonsoft.Json.JsonConvert.SerializeObject(acknowledegment);
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst("name")?.Value;
                //bool EnableLoggingInfo = (bool)_cache.Get("EnableLoggingInfo");

                if (userId.ToString() == IntegrationSettings.GetSetting("UserName").ToString())
                {
                    
                    if (string.IsNullOrEmpty(objStr)) throw new NullReferenceException("Invalid or Input");
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true, PrintInterface: true);
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Obj : " + objStr,TrackInterface: Logger.EnableLoggingInfo);
                    Correspondence correspondence = new Correspondence();
                    object jsonResult = correspondence.WriteAck(acknowledegment);
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true, PrintInterface: true);
                    return Json(jsonResult);
                }
                else
                {
                    throw new NullReferenceException("Invalid or Expired Session");
                }
                
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message, PrintInterface: true);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Object: " + objStr, PrintInterface: true);
                return Json(IntResultLib.GetResult(ex));
            }

        }
       
        
        //Recieve Fax correspondences from fax server to middle folder
        [HttpPost]
        [Authorize]
        public JsonResult FaxImport([FromBody]TarasolDocument tarasolDocument)
        {
            string objStr = Newtonsoft.Json.JsonConvert.SerializeObject(tarasolDocument);
            try
            {
                
                if (string.IsNullOrEmpty(objStr)) throw new NullReferenceException("Invalid or Input");
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true, PrintInterface: true);
                TarasolDocument eSBDocument = tarasolDocument;
                //read post data correspondence and files
                object jsonResult = Correspondence.ImportFaxToFolder(eSBDocument);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true, PrintInterface: true);
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message, PrintInterface: true);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Object: " + objStr, PrintInterface: true);
                return Json(IntResultLib.GetResult(ex));
            }

        }
                

        //Tarasol will call this API to trigger export scanning in the itegration layer
        [HttpPost]
        [Authorize]
        public JsonResult ScanForExport([FromBody]ExportCorr[] scanRequest)
        {
            try
            {
                Debugger.Launch();
                string objStr = Newtonsoft.Json.JsonConvert.SerializeObject(scanRequest);
                if (string.IsNullOrEmpty(objStr)) throw new NullReferenceException("Invalid or Input");

                //bool EnableLoggingInfo = (bool)_cache.Get("EnableLoggingInfo");
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true, PrintInterface: true);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Obj : " + objStr,TrackInterface: Logger.EnableLoggingInfo);
                bool IsFirstTry = true;
                foreach (var item in scanRequest)
                {
                    if(item.IsFirstTry == false)
                    {
                        IsFirstTry = item.IsFirstTry;
                    }
                }
                if (IsFirstTry)
                {
                    var task = new[]
                {
                   Task.Run(()=>Correspondence.ExportFromFolder(_hostingEnvironment, scanRequest))
                };
                    Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true, PrintInterface: true);
                    return Json(IntResultLib.GetResult(200));
                }
                else
                {
                    Correspondence.ExportFromFolder(_hostingEnvironment, scanRequest);
                    return Json(IntResultLib.GetResult(200));
                }
                
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message, PrintInterface: true);
                return Json(IntResultLib.GetResult(ex));
            }
        }
        

        //Tarasol will call this API to trigger pulling exported fax status from fax server -- not used yet
        [HttpPost]
        [Authorize]
        public JsonResult FaxInquiry([FromBody]FaxInquiry[] faxInquiries)
        {
            try
            {
                string objStr = Newtonsoft.Json.JsonConvert.SerializeObject(faxInquiries);
                if (string.IsNullOrEmpty(objStr)) throw new NullReferenceException("Invalid or Input");
                //bool EnableLoggingInfo = (bool)_cache.Get("EnableLoggingInfo");
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true, PrintInterface: true);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Obj : " + objStr,TrackInterface: Logger.EnableLoggingInfo);
                Correspondence.FaxInquiry(ref faxInquiries);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true, PrintInterface: true);
                return Json(IntResultLib.GetResult(200));
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message, PrintInterface: true);
                return Json(IntResultLib.GetResult(ex));
            }
        }


        ///Tarasol will call this API to trigger pulling exported Correspondences status from ESB
        [HttpPost]
        [Authorize]
        public JsonResult ScanForAck([FromBody]ExportCorr[] scanRequest)
        {
            try
            {
                string objStr = Newtonsoft.Json.JsonConvert.SerializeObject(scanRequest);
                if (string.IsNullOrEmpty(objStr)) throw new NullReferenceException("Invalid or Input");
                //bool EnableLoggingInfo = (bool)_cache.Get("EnableLoggingInfo");
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true, PrintInterface: true);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Obj : " + objStr,TrackInterface: Logger.EnableLoggingInfo);

                //scan ack folder to push read notification to esb
                // will expose ESB service for push notification
                
                var task = new[]
                {
                   Task.Run(()=>Correspondence.SendAck(_hostingEnvironment, scanRequest))
                };
                
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true, PrintInterface: true);
                return Json(IntResultLib.GetResult(1));
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message, PrintInterface: true);
                return Json(IntResultLib.GetResult(ex));
            }
        }

        [HttpPost]
        [Authorize]
        public JsonResult SendReject([FromBody] ExportCorr[] scanRequest)
        {
            try
            {
                string objStr = Newtonsoft.Json.JsonConvert.SerializeObject(scanRequest);
                if (string.IsNullOrEmpty(objStr)) throw new NullReferenceException("Invalid or Input");
               
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true, PrintInterface: true);
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - Obj : " + objStr, TrackInterface: Logger.EnableLoggingInfo);

                
                //var task = new[]
                //{
                //   Task.Run(()=>Correspondence.SendAck(_hostingEnvironment, scanRequest))
                //};

                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End", TrackInterface: true, PrintInterface: true);
                return Json(IntResultLib.GetResult(1));
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message, PrintInterface: true);
                return Json(IntResultLib.GetResult(ex));
            }
        }
        //[HttpPost]
        ////[Authorize]
        //public JsonResult TestAPI([FromBody]string result)
        //{
        //    try
        //    {
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Soap - Response : " + result);
        //        XmlDocument xml = new XmlDocument();
        //        xml.LoadXml(result);
        //        string ResponseNode = xml.GetElementsByTagName("tns:AddNewDocumentResponse")[0].InnerXml.Replace("xmlns:tns=\"http://www.moi.gov.ae/2018/MoiIntegration/MOICorrespondenceServiceSchema\"", "").Replace("tns:", "");
        //        Ecs_DocumentAddResponse Objectresult = XMLProcessing.Deserializer<Ecs_DocumentAddResponse>(ResponseNode);


        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
        //        return Json(IntResultLib.GetResult(1));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
        //        return Json(IntResultLib.GetResult(ex));
        //    }
        //}

        //[HttpPost]
        ////[Authorize]
        //public JsonResult Testnot([FromBody] string result)
        //{
        //    try
        //    {
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Soap - Response : " + result);
        //        string NotificationURL = IntegrationSettings.GetSetting("APIURL").ToString() + "/Corr/corrExportStatus?ExchangeNo=132"
        //                                                                            + "&status=0";
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "NotificationURL : " + NotificationURL);
        //        string Tarasol_Token = Cache.GetSession().Token;
        //        CallExternalAPI.CallGetAPI(NotificationURL, Tarasol_Token);

        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End");
        //        return Json(IntResultLib.GetResult(1));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
        //        return Json(IntResultLib.GetResult(ex));
        //    }
        //}
    }

}