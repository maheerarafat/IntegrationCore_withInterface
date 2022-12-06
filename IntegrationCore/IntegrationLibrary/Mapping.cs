using IntegrationCore.Helper;
using IntegrationCore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IntegrationCore.IntegrationLibrary
{
    public class Mapping
    {
        public static DocumentObject MapImportedFile(TarasolDocument tarasolDocument)
        {
            try
            {
                List<TarasolTargetItem> AddressTo = new List<TarasolTargetItem>();
                TarasolTargetItem PostFromID = new TarasolTargetItem() ;

                foreach (var item in tarasolDocument.TransactionIDs)
                {
                    GlobalIDs globalIDs = new GlobalIDs { GlobalID = item.Split('@')[1], MainParentGlobalID = item.Split('@')[2] };

                    globalIDs.MainParentGlobalID = Common.AddPrefixEntity(globalIDs.MainParentGlobalID);
                    if (CheckDepartmentGlobalID(item.Split('@')[1], "GlobalId"))
                        AddressTo.Add(new TarasolTargetItem() { GlobalIDs = globalIDs, ExchangeNo = item.Split('@')[0], ExchangeStatus = ((int)Helper.Enum.enumExchangStatus.Folder).ToString() }); // ExchangStatus = "1"
                }

                if (CheckTategetGlobalID(tarasolDocument.Sender.GlobalID, "GlobalId"))
                {
                    PostFromID = new TarasolTargetItem() { GlobalIDs = new GlobalIDs { GlobalID = tarasolDocument.Sender.GlobalID }, ExchangeStatus = ((int)Helper.Enum.enumExchangStatus.Folder).ToString() };
                }

                DocumentObject documentObject = new DocumentObject
                {
                    //GlobalID = tarasolDocument.Global_ID,
                    ImportanceID = new TarasolItem { Value = tarasolDocument.Importance.ToString() },
                    Notes = new TarasolItem() { Value = Common.CheckEmptyString(tarasolDocument.Notes) },
                    Subject = new TarasolItem() { Value = tarasolDocument.Subject },
                    ExternalRefNumber = new TarasolItem() { Value = tarasolDocument.CorrespondenceNo.ToString() },
                    SecrecyID = new TarasolItem() { Value = (tarasolDocument.Secrecy + 1).ToString() },
                    PriorityID = new TarasolItem() { Value = tarasolDocument.Priority.ToString() },
                    ClassificationID = new Classification() { Code = Common.CheckEmptyString((string)IntegrationSettings.GetSetting("ClassificationId")) },
                    Body = new TarasolItem() { Value = tarasolDocument.Body },
                    CreationDate = new TarasolItem() { Value = ConvertToDateString(tarasolDocument.Date) },
                    PostFromID = PostFromID,
                    PostFromType = new TarasolItem() { Value = ((int)Helper.Enum.enumExchangAddressToType.External).ToString() },
                    PostToType = new TarasolItem() { Value = ((int)Helper.Enum.enumExchangAddressToType.External).ToString() },
                    AddressTo = AddressTo
                };

                foreach (var item in AddressTo)
                {
                    if (item?.GlobalIDs != null && tarasolDocument != null && tarasolDocument?.Sender != null)
                    {
                        Logger.AddToLogFile("CorrespondenceImport From => to", Logger.Enum_LogType._Info, $"{item?.GlobalIDs?.MainParentGlobalID}@{tarasolDocument?.Sender.GlobalID} =>  {item?.GlobalIDs?.GlobalID}"   , TrackInterface: true);
                    }
                }

                return documentObject;
            }
            catch (IndexOutOfRangeException ex)
            {
                string message = "TransactionIDs there are missing element " + ex.Message;
                Exception e = new Exception(message);
                throw e;
            }
            catch (Exception ex)
            {

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
                    AddressTo.Add(new TarasolTargetItem() { Fax = item.FaxNo, ExchangeStatus = ((int)Helper.Enum.enumExchangStatus.Fax).ToString() });
                }

                DocumentObject documentObject = new DocumentObject
                {
                    //ImportanceID = new TarasolItem { Value = tarasolDocument.Importance.ToString() },
                    Subject = new TarasolItem() { Value = tarasolDocument.Subject },
                    //PriorityID = new TarasolItem() { Value = tarasolDocument.Priority.ToString() },
                    //Body = new TarasolItem() { Value = tarasolDocument.Body },
                    CreationDate = new TarasolItem() { Value = ConvertToDateString(tarasolDocument.Date) },
                    PostFromID = new TarasolTargetItem() { Fax = tarasolDocument.Sender.FaxNo, ExchangeStatus = ((int)Helper.Enum.enumExchangStatus.Fax).ToString() },
                    PostFromType = new TarasolItem() { Value = "3" },
                    PostToType = new TarasolItem() { Value = "3" },
                    AddressTo = AddressTo
                };
                return documentObject;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tarasolDocument"></param>
        /// <returns></returns>
        public static DocumentObject MapImportedEmail(ICALibrary.Models.Email emailDocument)
        {
            try
            {
                List<TarasolTargetItem> AddressTo = new List<TarasolTargetItem>();
                foreach (var item in emailDocument.EmailDistributors)
                {
                    if (item.TypeDistributor == (int)Helper.Enum.enumEmailDistributor.Department)
                    {
                        GlobalIDs globalIDs = new GlobalIDs { GlobalID = item.GlobalID, MainParentGlobalID = item.MainParentGlobalID };
                        AddressTo.Add(new TarasolTargetItem() { GlobalIDs = globalIDs, Email = "tarasol@ica.gov.ae", ExchangeStatus = ((int)Helper.Enum.enumExchangStatus.Email).ToString() }); //TODO how will receive it whom make import here trace number needed 
                    }
                }

                DocumentObject documentObject = new DocumentObject
                {
                    ExternalRefNumber = new TarasolItem() { Value = emailDocument.ExchangeNo },
                    Subject = new TarasolItem() { Value = emailDocument.Subject },
                    CreationDate = new TarasolItem() { Value = ConvertToDateString(emailDocument.Date) },
                    PostFromID = new TarasolTargetItem() { Email = emailDocument.From, ExchangeStatus = ((int)Helper.Enum.enumExchangStatus.Email).ToString(), GlobalIDs = new GlobalIDs() }, // here number needed same 
                    PostFromType = new TarasolItem() { Value = ((int)Helper.Enum.enumExchangAddressToType.External).ToString() },
                    PostToType = new TarasolItem() { Value = ((int)Helper.Enum.enumExchangAddressToType.External).ToString() },
                    SecrecyID = new TarasolItem() { Value = emailDocument.Priority.ToString() },
                    AddressTo = AddressTo
                };
                return documentObject;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static DateTime ConvertToDateTime(string TarasolDateTime)
        {
            //20181004015324
            DateTime dateTime = DateTime.Now;
            if (!String.IsNullOrWhiteSpace(TarasolDateTime))
            {
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

                if (TarasolDateTime.Length == 8)
                {
                    int Year = Convert.ToInt16(TarasolDateTime.Substring(0, 4));
                    int Month = Convert.ToInt16(TarasolDateTime.Substring(4, 2));
                    int Day = Convert.ToInt16(TarasolDateTime.Substring(6, 2));
                    dateTime = new DateTime(year: Year, month: Month, day: Day);
                }
            }
            return dateTime;
        }

        public static string ConvertToDateString(DateTime dateTime)
        {
            return dateTime.Year.ToString("00") + dateTime.Month.ToString("00") + dateTime.Day.ToString("00") + dateTime.Hour.ToString("00") + dateTime.Minute.ToString("00") + dateTime.Second.ToString("00");
        }

        public static bool CheckDepartmentGlobalID(string Globalid, string FieldName)
        {
            try
            {
                if (string.IsNullOrEmpty(Globalid)) return false;

                string Tarasol_Token = Cache.GetSession().Token;
                string Session_ID = Cache.GetSession().SessionID;
                string ApiUrl = IntegrationSettings.GetSetting("APIURL").ToString() + "/Config/DepartmentPositionsListGet";
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ApiUrl : " + ApiUrl);

                if (Tarasol_Token == "")
                {
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Tarasol_Token : Unauthorized user");
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
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Tarasol_Token : " + Tarasol_Token.Length);
                string jsondata = JsonConvert.SerializeObject(departmentPositionsReqCls);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ApiUrl post data : " + jsondata);

                DepartmentPositionList jsonResponse = JsonConvert.DeserializeObject<DepartmentPositionList>(CallExternalAPI.CallPostAPI(ApiUrl, jsondata, Tarasol_Token).ToString());
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ApiUrl post data : " + JsonConvert.SerializeObject(jsonResponse));
                return jsonResponse.CountofRecords != "0";
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                throw ex;
            }

        }

        public static bool CheckTategetGlobalID(string Globalid, string FieldName)
        {
            try
            {
                if (string.IsNullOrEmpty(Globalid)) return false;

                string Tarasol_Token = Cache.GetSession().Token;
                string Session_ID = Cache.GetSession().SessionID;
                string ApiUrl = IntegrationSettings.GetSetting("APIURL").ToString() + $"/Corr/ContactsItemsGet?SessionID={Session_ID}&ContactType=TargetExternal";
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ApiUrl : " + ApiUrl);

                if (Tarasol_Token == "")
                {
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Tarasol_Token : Unauthorized user");
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
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Tarasol_Token : " + Tarasol_Token.Length);
                string jsondata = JsonConvert.SerializeObject(departmentPositionsReqCls);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ApiUrl post data : " + jsondata);

                DepartmentPositionList jsonResponse = JsonConvert.DeserializeObject<DepartmentPositionList>(CallExternalAPI.CallPostAPI(ApiUrl, jsondata, Tarasol_Token).ToString());
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ApiUrl post data : " + JsonConvert.SerializeObject(jsonResponse));
                return jsonResponse.CountofRecords != "0";
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                throw ex;
            }

        }

    }
}
