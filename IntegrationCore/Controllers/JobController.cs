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
                var data = Jobs.GetLogJobESBSuccuss(pageNumber, pageSize, searchValue, ref recordsTotal);

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
                var data = Jobs.GetLogJobESBFaild(pageNumber, pageSize, searchValue, ref recordsTotal);

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

        [HttpPost]
        [Authorize]
        public ActionResult ExcuteManualRequest(ICALibrary.Models.ManualRequest manualRequest)
        {
            try
            {
                object jsonResult;
                if (manualRequest.MethodName.StartsWith(ICALibrary.Helper.Constant.MethodSendAckICA))
                {
                    ExportCorr[] SendAckICA = Newtonsoft.Json.JsonConvert.DeserializeObject<ExportCorr[]>(manualRequest.BodyRequest);
                    if (this._hostingEnvironment.IsEnvironment("ICA"))
                    {
                        ConfigAttribute configAttribute = new ConfigAttribute
                        {
                            consumerID = _config["ICA:tarasol_service_contract:consumer_id"],
                            urlServiceESB = _config["ICA:tarasol_service_contract:url"]
                        };
                        Correspondence.SendAckICA(_hostingEnvironment, SendAckICA, configAttribute);
                    }
                }
                else if (manualRequest.MethodName.StartsWith(ICALibrary.Helper.Constant.MethodExportFromFolderICA))
                {
                    ExportCorr[] ExportFromFolderICA = Newtonsoft.Json.JsonConvert.DeserializeObject<ExportCorr[]>(manualRequest.BodyRequest);
                    ConfigAttribute configAttribute = new ConfigAttribute
                    {
                        consumerID = _config["ICA:tarasol_service_contract:consumer_id"],
                        urlServiceNotify = _config["ICA:tarasol_service_contract:url"],
                        urlServiceESB = _config["ICA:notification_service:url"],
                        senderMail = _config["ICA:SenderMail"]
                    };
                    if (this._hostingEnvironment.IsEnvironment("ICA"))
                    {
                        Correspondence.ExportFromFolderICA(_hostingEnvironment, ExportFromFolderICA, configAttribute);
                    }
                }
                else if (manualRequest.MethodName.StartsWith(ICALibrary.Helper.Constant.MethodWriteAck))
                {
                    Correspondence correspondence = new Correspondence();
                    DeliveryRequest acknowledegment = Newtonsoft.Json.JsonConvert.DeserializeObject<DeliveryRequest>(manualRequest.BodyRequest);
                    jsonResult = correspondence.WriteAck(acknowledegment);
                }
                else if (manualRequest.MethodName.StartsWith(ICALibrary.Helper.Constant.MethodSendSMSICA))
                {
                    String strDecode = String.Empty;
                    strDecode = System.Net.WebUtility.HtmlDecode(manualRequest.BodyRequest);
                    strDecode = strDecode.Replace("%20", " ");
                    List<SMSObject> sMSObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SMSObject>>(strDecode);
                    string transactionRefNo = string.Empty;
                    ConfigAttribute configAttribute = new ConfigAttribute
                    {
                        consumerID = _config["ICA:notification_service:consumer_id"],
                        urlServiceNotify = _config["ICA:notification_service:url"]
                    };
                    IntResult jsonObj = SMS.SendSMSICA(sMSObject, configAttribute, out transactionRefNo);
                    if (jsonObj.Code == (int)ICALibrary.Helper.Enum.responseCode.Success)
                    {
                        Jobs.UpdateProccessScheduleStatus(manualRequest.Id, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                    }
                }

                //Returning Json Data
                return Json(IntResultLib.GetResult(200));
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(ControllerContext.ActionDescriptor.ControllerName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}
