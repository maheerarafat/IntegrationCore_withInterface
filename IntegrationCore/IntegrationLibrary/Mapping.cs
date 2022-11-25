using IntegrationCore.Models;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace IntegrationCore.IntegrationLibrary
{
    public class Mapping
    {
        public static DocumentObject MapImportedFile(TarasolDocument tarasolDocument, List<TarasolTargetItem> AddressTo)
        {
            try
            {
                //List<TarasolTargetItem> AddressTo = new List<TarasolTargetItem>();
                //foreach (var item in tarasolDocument.TransactionIDs)
                //{
                //    if (string.IsNullOrEmpty(item)) continue;
                //        GlobalIDs globalIDs = new GlobalIDs { GlobalID = item.Split('@')[1],MainParentGlobalID = IntegrationSettings.GetSetting("EntityPrefix").ToString() + item.Split('@')[2] };//MainParentGlobalID shouldn't be filled
                //    if (ChechDepartmentGlobalID(item.Split('@')[1], "GlobalId") && !string.IsNullOrEmpty(item.Split('@')[0])) AddressTo.Add(new TarasolTargetItem() { GlobalIDs = globalIDs ,ExchangeNo = item.Split('@')[0] ,ExchangeStatus = "1"});
                //}
                
                DocumentObject documentObject = new DocumentObject
                {
                    //GlobalID = tarasolDocument.Global_ID,
                    ImportanceID = new TarasolItem { Value = (tarasolDocument.Importance>3?3: tarasolDocument.Importance).ToString() },
                    Notes = new TarasolItem() { Value = tarasolDocument.Notes },
                    Subject = new TarasolItem() { Value = (tarasolDocument.Subject.Trim() == null) ? "empty" : Regex.Replace((string.IsNullOrEmpty(tarasolDocument.Subject.Trim())) ? "empty" : tarasolDocument.Subject.Trim(), "<.*?>", String.Empty).Replace("\n", " ").Replace("&nbsp", " ").Replace(";", "").Replace("PrintingDateTime", "") },
                    ExternalRefNumber = new TarasolItem() { Value = tarasolDocument.CorrespondenceNo.ToString().Trim() },
                    SecrecyID = new TarasolItem() { Value = (tarasolDocument.Secrecy>3?4:tarasolDocument.Secrecy + 1).ToString() },
                    PriorityID = new TarasolItem() { Value = (tarasolDocument.Priority>3?3:tarasolDocument.Priority).ToString() },
                    ClassificationID = new Classification() { Code = IntegrationSettings.GetSetting("DefaultReference").ToString()/*tarasolDocument.ClassificationID*/ },
                    Body = new TarasolItem() { Value = (tarasolDocument.Body?.Trim() == null) ? "empty" : Regex.Replace((string.IsNullOrEmpty(tarasolDocument.Body.Trim())) ? "empty" : tarasolDocument.Body.Trim(), "<.*?>", String.Empty).Replace("\n", " ").Replace("&nbsp", " ").Replace(";", "").Replace("PrintingDateTime", "") } ,
                    DBCreationDate = new TarasolItem() { Value = ConvertToDateString(tarasolDocument.Date) },
                    CreationDate = new TarasolItem() { Value = ConvertToDateString(tarasolDocument.Date)  },
                    PostFromID = new TarasolTargetItem() { GlobalIDs = new GlobalIDs { GlobalID = tarasolDocument.Sender.GlobalID/*, MainParentGlobalID = IntegrationSettings.GetSetting("EntityPrefix").ToString() + tarasolDocument.TransactionIDs[0].Split('@')[2] */}, ExchangeStatus = "1" },
                    PostFromType = new TarasolItem() { Value = "1" },
                    PostToType = new TarasolItem() { Value = "0" },
                    AddressTo = AddressTo
                };
                //documentObject.Variables = new List<TarasolItem>();
                //documentObject.Variables.Add(new TarasolItem() { Name = IntegrationSettings.GetSetting("ImportDate").ToString(), Value = tarasolDocument.Date.ToString("DD-MM-yyyy HH:mm:ss") });
                foreach (var item in AddressTo)
                {
                    if (item?.GlobalIDs != null && tarasolDocument != null && tarasolDocument?.Sender != null)
                    {
                        Logger.AddToLogFile("CorrespondenceImport From => to", Logger.Enum_LogType._Info, $"{item?.GlobalIDs?.MainParentGlobalID}@{tarasolDocument?.Sender.GlobalID} =>  {item?.GlobalIDs?.GlobalID}", TrackInterface: true);
                    }
                }

                return documentObject;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                throw ex;
            }
            
        }
        public static bool ChechDepartmentGlobalID(string Globalid,string FieldName)
        {
            try
            {
                ////bool EnableLoggingInfo = (bool)IntegrationSettings.GetSetting("EnableLoggingInfo");
                if (string.IsNullOrEmpty(Globalid)) return false;

                string Tarasol_Token = Cache.GetSession().Token;
                string Session_ID = Cache.GetSession().SessionID;
                string ApiUrl = IntegrationSettings.GetSetting("APIURL").ToString() + "/Config/DepartmentPositionsListGet";
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ApiUrl : " + ApiUrl, TrackInterface: true);

                if (string.IsNullOrEmpty(Tarasol_Token))
                {
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Tarasol_Token : Unauthorized user");
                    return false;
                }
                List<SearchCls> searchList = new List<SearchCls>();
                searchList.Add(new SearchCls()
                {
                    FieldName = FieldName,
                    Opertation = "eq",
                    Value = Globalid
                });
                DepartmentPositionsReqCls departmentPositionsReqCls = new DepartmentPositionsReqCls()
                {
                    SearchList = searchList
                };
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Tarasol_Token : " + Tarasol_Token.Length, TrackInterface: true);
                string jsondata = JsonConvert.SerializeObject(departmentPositionsReqCls);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ApiUrl post data : " + jsondata,TrackInterface: Logger.EnableLoggingInfo);
                try
                {
                    DepartmentPositionList jsonResponse = JsonConvert.DeserializeObject<DepartmentPositionList>(CallExternalAPI.CallPostAPI(ApiUrl, jsondata, Tarasol_Token).ToString());
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ApiUrl post data : " + JsonConvert.SerializeObject(jsonResponse), TrackInterface: true);
                    return jsonResponse.CountofRecords != "0";
                }
                catch (Exception)
                {
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, " error occured in  : Calling Tarasol API ");
                    throw;
                }
                
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, " error occured in  :  " + ex.Message);
                throw ex;
            }

        }
        public static DocumentObject MapImportedFax(TarasolDocument tarasolDocument)
        {
            try
            {
                List<TarasolTargetItem> AddressTo = new List<TarasolTargetItem>();
                foreach (var item in tarasolDocument.Receiver)
                {
                    if (ChechDepartmentGlobalID(item.FaxNo, "Fax")) AddressTo.Add(new TarasolTargetItem() { Fax = item.FaxNo ,Exchange_no_fax = tarasolDocument.TransactionIDs[0], ExchangeStatus = "4",GlobalIDs = new GlobalIDs()});
                }
                List<TarasolItem> variabels = new List<TarasolItem>();
                variabels.Add(new TarasolItem() { Name = (string)IntegrationSettings.GetSetting("FromFaxField"), Value = tarasolDocument.Sender.Name });
                DocumentObject documentObject = new DocumentObject
                {
                    ImportanceID = new TarasolItem { Value = "1" },
                    Notes = new TarasolItem() { Value = "" },
                    Subject = new TarasolItem() { Value = tarasolDocument.Subject },
                    ExternalRefNumber = new TarasolItem() { Value = "" },
                    SecrecyID = new TarasolItem() { Value = "1" },
                    PriorityID = new TarasolItem() { Value = "1" },
                    ClassificationID = new Classification() { Code = "" },
                    Body = new TarasolItem() { Value = "" },
                    DBCreationDate = new TarasolItem() { Value = ConvertToDateString(tarasolDocument.Date) },
                    PostFromID = new TarasolTargetItem() { Fax = tarasolDocument.Sender.FaxNo, ExchangeStatus = "3", GlobalIDs = new GlobalIDs() },
                    PostFromType = new TarasolItem() { Value = "1" },
                    PostToType = new TarasolItem() { Value = "1" },
                    AddressTo = AddressTo,
                    Variables = variabels
                };
                return documentObject;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                throw ex;
            }

        }

        public static DateTime ConvertToDateTime(string TarasolDateTime)
        {
            //20181004015324
            DateTime dateTime = new DateTime();
            if (TarasolDateTime.Length == 14)
            {
                int Year = Convert.ToInt16(TarasolDateTime.Substring(0, 4));
                int Month = Convert.ToInt16(TarasolDateTime.Substring(4, 2));
                int Day = Convert.ToInt16(TarasolDateTime.Substring(6, 2));
                int Hour = Convert.ToInt16(TarasolDateTime.Substring(8, 2));
                int Minutes = Convert.ToInt16(TarasolDateTime.Substring(10, 2));
                int Seconds = Convert.ToInt16(TarasolDateTime.Substring(12, 2));
                dateTime = new DateTime(year: Year, month: Month, day: Day, hour: Hour, minute: Minutes, second: Seconds);
            }
            return dateTime;
        }
        public static string ConvertToDateString(DateTime dateTime)
        {
            return dateTime.Year.ToString() + dateTime.Month.ToString("00") + dateTime.Day.ToString("00") + dateTime.Hour.ToString("00") + dateTime.Minute.ToString("00") + dateTime.Second.ToString("00");
        }

    }
}
