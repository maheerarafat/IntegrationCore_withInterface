using IntegrationCore.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntegrationCore.IntegrationLibrary
{
    public class Correspondence
    {
        private static string _filesNotSend = "_headerfooter";
        public static object ImportToFolder(TarasolDocument tarasolDocument)
        {
            try
            {
                ////bool EnableLoggingInfo = (bool)IntegrationSettings.GetSetting("EnableLoggingInfo");
                //Check Required fields
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start - tarasolDocument.Subject : " + tarasolDocument.Subject,TrackInterface: Logger.EnableLoggingInfo);
                if (string.IsNullOrEmpty(tarasolDocument.Subject) || string.IsNullOrEmpty(tarasolDocument.CorrespondenceNo) || tarasolDocument.Date == null)
                {
                    throw new System.Data.DataException();
                }
                if(/*string.IsNullOrEmpty(tarasolDocument.ClassificationID) || */tarasolDocument.Sender == null || tarasolDocument.Receiver == null || tarasolDocument.Files == null)
                {
                    throw new System.Data.DataException();
                }
                //Get import folder path
                string GUID = Guid.NewGuid().ToString();
                string path = (string)IntegrationSettings.GetSetting("IntegrationDataFolder") + "/In/Draft/" 
                    + GUID;
                if (Directory.Exists(path))
                {
                    Directory.Delete(path,true);
                }
                path += "/" + GUID;
                try
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                    List<Attachment> attachments = tarasolDocument.Files;
                    if (tarasolDocument.Files != null)
                    {
                        foreach (var data in attachments)
                        {
                            var base64string = data.FileContent;
                            var base64array = Convert.FromBase64String(base64string);
                            string filePath = path + "/" + ((data.FileName.Contains("___Main___PDF___T04File")) ? GUID + ".pdf" : data.FileName) + "." + data.FileExtension;
                            System.IO.File.WriteAllBytes(filePath, base64array);
                        }
                    }


                    DocumentObject documentObject = Mapping.MapImportedFile(tarasolDocument);
                    if (documentObject.AddressTo.Count == 0)
                    {
                        Directory.Delete(path, true);
                        return IntResultLib.GetResult(2);
                    }
                    string jsondata = JsonConvert.SerializeObject(documentObject);

                    Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, $"GUID : {GUID} ", TrackInterface: true, PrintInterface: true);
                    // Write that JSON to  file,  
                    File.WriteAllText(path + "/" + GUID + ".json", jsondata, encoding: Encoding.UTF8);
                    // Create zip file for correspondence
                    ZipFile.CreateFromDirectory(path, path + ".zip");
                    File.Move(path + ".zip", Path.ChangeExtension(path + ".zip", ".tarasol"));
                    Directory.Delete(path, true);
                    // Call Correspondence Import API from Tarasol
                    string corrImportResult = TarasolCorrespondencesImport();
                    //
                    Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, corrImportResult);

                    return IntResultLib.GetResult(1); // if operation succeeded
                }
                catch (Exception)
                {
                    Directory.Delete(path, true);
                    throw;
                }
                
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message, PrintInterface: true);
                throw ex;
            }
           
        }
        public static string TarasolCorrespondencesImport()
        {
            try
            {
                string Tarasol_Token = Cache.GetSession().Token;
                string Session_ID = Cache.GetSession().SessionID;
                string ImportURL = IntegrationSettings.GetSetting("APIURL").ToString() +"/Corr/CorrespondencesImport?SessionId="+ Session_ID;
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ImportURL : " + ImportURL, TrackInterface: true);
                /*b8a862d1-ed8a-487c-bd44-0ebe1b0f6cf8&CurrentPosition=016b10b9-7930-44b4-96d7-07b6f58cb357*/
                if (Tarasol_Token == "")
                {
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Tarasol_Token : Unauthorized user");
                    return null;
                }
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Tarasol_Token : " + Tarasol_Token.Length, TrackInterface: true);
                object jsonResponse = CallExternalAPI.CallGetAPI(ImportURL, Tarasol_Token);
                return jsonResponse.ToString();
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message, PrintInterface: true);
                throw ex;
            }
            
        }
        
        public static object ImportFaxToFolder(TarasolDocument tarasolDocument)
        {
            try
            {
                //Check Required fields
                if (tarasolDocument.Date == null)
                {
                    throw new System.Data.DataException();
                }
                if (tarasolDocument.Sender == null || tarasolDocument.Receiver == null)
                {
                    throw new System.Data.DataException();
                }
                //Get import folder path
                string GUID = Guid.NewGuid().ToString();
                string path = (string)IntegrationSettings.GetSetting("IntegrationDataFolder") + "/In/Draft/"
                    + GUID;
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                path += "/" + GUID;
                DirectoryInfo di = Directory.CreateDirectory(path);
                //List<Attachment> attachments = tarasolDocument.Files;
                //foreach (var data in attachments)
                //{
                //    var base64string = data.FileContent;
                //    var base64array = Convert.FromBase64String(base64string);
                //    string filePath = path + "/" + data.FileName + "." + data.FileExtension;
                //    System.IO.File.WriteAllBytes(filePath, base64array);
                //}
                try
                {
                    string filePath = path + "/" + GUID + ".pdf";
                    CallExternalAPI.CallGetFaxFile(tarasolDocument.faxurl, tarasolDocument.faxAuth, filePath);

                    DocumentObject documentObject = Mapping.MapImportedFax(tarasolDocument);
                    if (documentObject.AddressTo.Count == 0)
                    {
                        Directory.Delete(path, true);
                        return IntResultLib.GetResult(2);
                    }
                    string jsondata = JsonConvert.SerializeObject(documentObject);

                    // Write that JSON to  file,  
                    File.WriteAllText(path + "/" + GUID + ".json", jsondata, encoding: Encoding.UTF8);
                    // Create zip file for correspondence
                    ZipFile.CreateFromDirectory(path, path + ".zip");
                    File.Move(path + ".zip", Path.ChangeExtension(path + ".zip", ".tarasol"));
                    Directory.Delete(path, true);
                    // Call Correspondence Import API from Tarasol
                    TarasolCorrespondencesImport();
                    //
                    return IntResultLib.GetResult(1); // if operation succeeded
                }
                catch (Exception)
                {
                    Directory.Delete(path, true);
                    throw;
                }
                
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message, PrintInterface: true);
                throw ex;
            }
        }

        public object WriteAck(DeliveryRequest acknowledegment)
        {
            try
            {
                if(acknowledegment is null)
                {
                    return IntResultLib.GetResult(2);
                }
                string DocPathName = acknowledegment.acknowledgementDate.ToString("yyyy") + "." + acknowledegment.TransactionID.Trim() + "." + acknowledegment.EntityCode;
                //Get delivery reports folder path
                string path = (string)IntegrationSettings.GetSetting("IntegrationDataFolder") + "/In/Draft/" + DocPathName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path + "/" + DocPathName);

                    TarasolAck tarasolAck = new TarasolAck()
                    {
                        AckDate = Mapping.ConvertToDateString(acknowledegment.acknowledgementDate),
                        ExternalExchangeNo = acknowledegment.TransactionID.Trim(),
                        LastParentGlobalID = IntegrationSettings.GetSetting("EntityPrefix").ToString() + acknowledegment.EntityCode.ToString(),
                        ReadBy = acknowledegment.ReadBy.Trim(),
                        ExchangeStatus = "1"
                    };
                    if(tarasolAck.ExternalExchangeNo is null || string.IsNullOrEmpty(tarasolAck.LastParentGlobalID) || tarasolAck.AckDate is null)
                    {
                        Directory.Delete(path, true);
                        return IntResultLib.GetResult(2);
                    }
                    string JsonAck = JsonConvert.SerializeObject(tarasolAck);
                    File.WriteAllText(path + "/" + DocPathName+ "/" + DocPathName + ".json", JsonAck,encoding:Encoding.UTF8);
                    ZipFile.CreateFromDirectory(path + "/" + DocPathName,
                        path + "/" + DocPathName + ".zip");
                    Directory.Delete(path + "/" + DocPathName,true);
                    File.Move(path + "/" + DocPathName + ".zip", Path.ChangeExtension(path + "/" + DocPathName + ".zip", ".ack"));
                }
                AcknowledgmentReportLoad();
                return IntResultLib.GetResult(1);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message, PrintInterface: true);
                throw ex;
            }
        }
        
        public static object ExportFromFolder(IHostingEnvironment _hostingEnvironment,ExportCorr[] scanRequest)
        {
            try
            {
                string path = (string)IntegrationSettings.GetSetting("IntegrationDataFolder", _hostingEnvironment) + "/Out";
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Data Path : " + path, TrackInterface: true);
                if (Directory.Exists(path))
                {
                    List<string> sentDocuments = new List<string>();
                    string[] subDirectories = Directory.GetDirectories(path);
                    foreach (var subDir in subDirectories)
                    {
                        if(subDir.EndsWith("Draft")|| subDir.EndsWith("Draft-fax")|| subDir.EndsWith("Draft-email"))
                        {
                            string[] DraftsubDirectories = Directory.GetDirectories(subDir);
                            foreach (var subDirectory in DraftsubDirectories)
                            {
                                foreach (var CorrItem in scanRequest)
                                {
                                    if (subDirectory.EndsWith(CorrItem.CorrUID))
                                    {
                                        try
                                        {
                                            string[] FilePaths = Directory.GetFiles(subDirectory, "*.tarasol");
                                            if (FilePaths.Length == 0)
                                            {
                                                continue;
                                            }
                                            string FilePath = Directory.GetFiles(subDirectory, "*.tarasol")[0];
                                            if(sentDocuments.Contains(CorrItem.CorrUID))
                                            {
                                                continue;
                                            }
                                            File.Move(FilePath, Path.ChangeExtension(FilePath, ".zip"));
                                            FilePath = FilePath.Replace(".tarasol", "");
                                            ZipFile.ExtractToDirectory(FilePath + ".zip", subDirectory);
                                            StreamReader reader;
                                            string strJSONdata = "";
                                            if (File.Exists(FilePath + ".json") )
                                            {

                                                string CorrNameUID = Path.GetFileName(FilePath + ".json").Replace(".json", "");
                                                reader = new StreamReader(FilePath + ".json");
                                                strJSONdata = reader.ReadToEnd();
                                                reader.Dispose();
                                                DocumentObject document = JsonConvert.DeserializeObject<DocumentObject>(strJSONdata);
                                                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Document - Obj : " + strJSONdata, TrackInterface: Logger.EnableLoggingInfo);
                                                List<string> EntityCodes = new List<string>();

                                                string[] Files = Directory.GetFiles(subDirectory, "*.*");
                                                List<ECSAttachment> ECSAttachmentList = new List<ECSAttachment>();
                                                List<MultipartFormDataContent> contentAttachmentFax = new List<MultipartFormDataContent>();
                                                List<Models.EmailAttachment> emailAttachmentList = new List<EmailAttachment>();
                                                foreach (string item in Files)
                                                {
                                                    if (!item.EndsWith(CorrNameUID + ".tarasol") && !item.EndsWith(CorrNameUID + ".zip") && !item.EndsWith(CorrNameUID + ".json"))
                                                    {
                                                        byte[] bytes = File.ReadAllBytes(item);
                                                        string stream = Convert.ToBase64String(bytes);
                                                        string fileName = Path.GetFileName(item);
                                                        if (!fileName.ToLower().EndsWith(_filesNotSend))
                                                        {
                                                            ECSAttachment eCSAttachment = new ECSAttachment()
                                                            {
                                                                Stream = stream,
                                                                DocumentName = (fileName == CorrNameUID + ".pdf") ? "___Main___PDF___T04File.pdf" : fileName
                                                            };
                                                            ECSAttachmentList.Add(eCSAttachment);

                                                            //////Fax Attachment
                                                            var content = new MultipartFormDataContent();
                                                            Stream streamFile = new MemoryStream(bytes);
                                                            var streamContent = new StreamContent(streamFile, 16384);
                                                            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                                                            content.Add(streamContent, fileName, fileName);
                                                            contentAttachmentFax.Add(content);

                                                            ////////////Email Attachment
                                                            emailAttachmentList.Add(new EmailAttachment()
                                                            {
                                                                FileContent = stream,
                                                                FileName = fileName,
                                                                FileType = Path.GetExtension(item)
                                                            });
                                                        }
                                                    }

                                                }

                                                Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, $"CorrUID  : {CorrItem.CorrUID} ", TrackInterface: true, PrintInterface: true);

                                                foreach (string item in Files)
                                                {
                                                    if (!item.EndsWith(CorrNameUID + ".tarasol") && !item.EndsWith(CorrNameUID + ".zip"))
                                                    {
                                                        File.Delete(item);
                                                    }
                                                }
                                                File.Move(FilePath + ".zip", Path.ChangeExtension(FilePath + ".zip", ".tarasol"));
                                                
                                                if(CorrItem.ResendTo != null)
                                                {
                                                    document.AddressTo = CorrItem.ResendTo;
                                                }
                                                if (CorrItem.IsFirstTry == false)
                                                {
                                                    List<TarasolTargetItem> newAddressTo = new List<TarasolTargetItem>();
                                                    foreach (var item in document.AddressTo)
                                                    {
                                                        if (CorrItem.ExchangeNo.Contains(item.ExchangeNo))
                                                        {
                                                            newAddressTo.Add(item);
                                                        }
                                                    }
                                                    document.AddressTo = newAddressTo;
                                                }
                                                foreach (var addressTo in document.AddressTo)
                                                {
                                                    if ((Convert.ToInt32(addressTo.ExchangeStatus) & 4) == 4) // fax type
                                                    {
                                                        try
                                                        {
                                                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Satrt - Fax Operation " + document.ExternalRefNumber.Value, TrackInterface: true);
                                                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, $"Calling fax service Outgoing Fax number {addressTo.Fax}", TrackInterface: true);
                                                            SendFax.Send(addressTo.Exchange_no_fax, CorrItem.CorrUID, contentAttachmentFax, addressTo.Fax, IntegrationSettings.GetSetting("FaxServer").ToString(),
                                                                IntegrationSettings.GetSetting("FaxUser").ToString(),
                                                                (IntegrationSettings.GetSetting("FaxPass").ToString() == "") ? "" : Encryption.DecryptString(IntegrationSettings.GetSetting("FaxPass").ToString()),
                                                                nameRecipient: document.ExternalRefNumber.Value);
                                                            //Send Notification to Tarasol
                                                            
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            SendNotificationToTarasol(addressTo.Exchange_no_fax, CorrItem.CorrUID, 0, 0, ex.Message, ex.Message);
                                                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message, PrintInterface: true);
                                                            //throw ex;
                                                        }
                                                    }
                                                    if ((Convert.ToInt32(addressTo.ExchangeStatus) & 2) == 2) // Email type
                                                    {
                                                        try
                                                        {                                                           
                                                            List<Receiver> receivers = new List<Receiver>();
                                                            receivers.Add(new Receiver() { ReceiverEmail = addressTo.Email, ReceiverType = ReceiverType.to });
                                                            Models.Mail mail = new Models.Mail()
                                                            {
                                                                Attachments = emailAttachmentList,
                                                                Subject = document.Subject.Value,
                                                                Receivers = receivers,
                                                                Sender = document.PostFromID.Email
                                                            };
                                                            string objStr = JsonConvert.SerializeObject(mail);
                                                            //Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Email : " + objStr);
                                                            IntResult mailResult = (IntResult)Mail.SendEmail(mail);
                                                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Result: " + JsonConvert.SerializeObject(mailResult), TrackInterface: true);
                                                            //Send Notification to Tarasol
                                                            if(mailResult.EMessage == "Unable to send Email")
                                                            {
                                                                SendNotificationToTarasol(addressTo.Exchange_no_email, CorrItem.CorrUID, 2, 0, "Unable to send Email", "");
                                                            }
                                                            else
                                                            {
                                                                SendNotificationToTarasol(addressTo.Exchange_no_email, CorrItem.CorrUID, 1, 0, "", "");
                                                            }
                                                            
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            SendNotificationToTarasol(addressTo.Exchange_no_email, CorrItem.CorrUID, 0, 0, ex.Message, ex.Message);
                                                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message, PrintInterface: true);
                                                            //throw ex;
                                                        }
                                                    }
                                                }
                                                foreach (var item in document.AddressTo)
                                                {
                                                    if ((Convert.ToInt32(item.ExchangeStatus) & 1) == 1)
                                                    {
                                                        string TempEntity = string.IsNullOrEmpty(item.GlobalIDs.MainParentGlobalID) ? item.GlobalIDs.GlobalID : item.GlobalIDs.MainParentGlobalID;
                                                        if (!EntityCodes.Contains(TempEntity)) EntityCodes.Add(TempEntity);
                                                    }
                                                        
                                                }
                                                List<IntResult> ResponseList = new List<IntResult>();
                                                foreach (var entity in EntityCodes)
                                                {
                                                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "entity : " + entity, TrackInterface: true);
                                                    List<Destination> destinationList = new List<Destination>();
                                                    List<TransactionId> transactionIdList = new List<TransactionId>();
                                                    foreach (var addressTo in document.AddressTo)
                                                    {
                                                        if (entity == addressTo.GlobalIDs.MainParentGlobalID && (Convert.ToInt32(addressTo.ExchangeStatus) & 1) == 1) //Folder type
                                                        {
                                                            Destination destination = new Destination();
                                                            destination.DestinationValue = addressTo.GlobalIDs.GlobalID;
                                                            destinationList.Add(destination);
                                                            transactionIdList.Add(new TransactionId { TransactionIdValue = addressTo.ExchangeNo + "@" + addressTo.GlobalIDs.GlobalID + "@" + addressTo.GlobalIDs.MainParentGlobalID.Replace(IntegrationSettings.GetSetting("EntityPrefix").ToString(), "")/*(string)IntegrationSettings.GetSetting("SiteEndPoint")*/ });
                                                        }
                                                    }
                                                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "transactionIdList : " + transactionIdList.Count, TrackInterface: true);
                                                    foreach (var trackElem in destinationList)
                                                    {
                                                        if (document.PostFromID != null && trackElem != null)
                                                        {
                                                            Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, $"{document?.PostFromID?.GlobalIDs} => {entity}@{trackElem?.DestinationValue}", TrackInterface: true, PrintInterface: true);
                                                        }
                                                    }

                                                    //Expose ESB service
                                                    if (destinationList.Count > 0 && transactionIdList.Count > 0)
                                                    {
                                                        int secrecyLevel = Convert.ToInt16(document.SecrecyID.Value);
                                                        int priorityLeve = Convert.ToInt16(document.PriorityID.Value);
                                                        ECS_Document eCS_Document = new ECS_Document()
                                                        {
                                                            EntityCode = entity.Replace(IntegrationSettings.GetSetting("EntityPrefix").ToString(), ""), // last parent
                                                            //shouldn't use SiteEndPoint here
                                                            SourceUnitId = (string.IsNullOrEmpty(document.PostFromID.GlobalIDs.GlobalID)) ? IntegrationSettings.GetSetting("SiteEndPoint").ToString() : document.PostFromID.GlobalIDs.GlobalID,//Tarasol Global ID 
                                                            DestinationList = destinationList,
                                                            OutboxNumber = document.ExternalRefNumber.Value,
                                                            DocumentSubject = document.Subject.Value,
                                                            OutboxDate = Mapping.ConvertToDateTime(document.CreationDate.Value).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                                                            Priority = Convert.ToInt16(document.PriorityID.Value),
                                                            ECSAttachmentList = ECSAttachmentList,
                                                            SecrecyLevel = (secrecyLevel - 1),
                                                            Alignment = 1,
                                                            TransactionIdList = transactionIdList,
                                                            HtmlContent = (document.Body == null)?"empty":Regex.Replace((string.IsNullOrEmpty(document.Body.Value))? "empty" : document.Body.Value, "<.*?>", String.Empty).Replace("\n"," ").Replace("&nbsp"," ").Replace(";","").Replace("PrintingDateTime",""),
                                                            DocumentType = 4
                                                        };
                                                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "eCS_Document xml : ", TrackInterface: true);
                                                        string XMLstring = XMLProcessing.Serialize(eCS_Document);
                                                        XMLstring = XMLstring.Replace("<TransactionId>", "<moic:TransactionId>").Replace("</TransactionId>", "</moic:TransactionId>");
                                                        XMLstring = XMLstring.Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "").Replace(@"<AddNewDocumentRequest xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">", "<moic:AddNewDocumentRequest> <moic:DocumentContext>").Replace("</AddNewDocumentRequest>", "</moic:DocumentContext></moic:AddNewDocumentRequest>");
                                                        try
                                                        {
                                                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "CallECSAddWebService : ", TrackInterface: true);
                                                            IntResult Response = SoapRequest.CallECSAddWebService(XMLstring);
                                                            Response.EMessage = entity;
                                                            ResponseList.Add(Response);
                                                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End -  entity: " + Response.EMessage, TrackInterface: true);
                                                            //Response is manipulated and sent to tarasol through notification API
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                                                            foreach (TarasolTargetItem NotiAdd in document.AddressTo)
                                                            {

                                                                if (entity == (string.IsNullOrEmpty(NotiAdd.GlobalIDs.MainParentGlobalID) ? NotiAdd.GlobalIDs.GlobalID : NotiAdd.GlobalIDs.MainParentGlobalID))
                                                                {
                                                                    SendNotificationToTarasol(NotiAdd.ExchangeNo, CorrItem.CorrUID, 2, 0, "", ex.Message);
                                                                }
                                                            }

                                                            //foreach (string item in Files)
                                                            //{
                                                            //    if (!item.EndsWith(CorrNameUID + ".tarasol") && !item.EndsWith(CorrNameUID + ".zip"))
                                                            //    {
                                                            //        File.Delete(item);
                                                            //    }
                                                            //}
                                                            //File.Move(FilePath + ".zip", Path.ChangeExtension(FilePath + ".zip", ".tarasol"));
                                                        }
                                                    }


                                                }

                                                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Sending Notification : ", TrackInterface: true);
                                                foreach (TarasolTargetItem NotiAdd in document.AddressTo)
                                                {
                                                    if(NotiAdd.GlobalIDs != null && NotiAdd.GlobalIDs.MainParentGlobalID == null && NotiAdd.ExchangeStatus == "1")
                                                    {
                                                        SendNotificationToTarasol(NotiAdd.ExchangeNo, CorrItem.CorrUID, 0, 0, "«·—„“ «·⁄«„ €Ì— „ÊÃÊœ ÷„‰ ÃÂ… „— »ÿ…", "Global ID is not under Integrated Entity");
                                                    }
                                                    else
                                                    {
                                                        foreach (var Response in ResponseList)
                                                        {
                                                            if (Response.EMessage == (string.IsNullOrEmpty(NotiAdd.GlobalIDs.MainParentGlobalID) ? NotiAdd.GlobalIDs.GlobalID : NotiAdd.GlobalIDs.MainParentGlobalID))
                                                            {
                                                                SendNotificationToTarasol(NotiAdd.ExchangeNo, CorrItem.CorrUID, Response.Code, 0, "", "");
                                                            }
                                                        }
                                                    }
                                                    

                                                }
                                                // after sending succeeds
                                                //foreach (string item in Files)
                                                //{
                                                //    if (!item.EndsWith(CorrNameUID + ".tarasol") && !item.EndsWith(CorrNameUID + ".zip"))
                                                //    {
                                                //        File.Delete(item);
                                                //    }
                                                //}
                                                //if (File.Exists(FilePath + ".zip"))
                                                //    File.Move(FilePath + ".zip", Path.ChangeExtension(FilePath + ".zip", ".tarasol"));
                                                //// Prevent sending same correspondence again
                                                sentDocuments.Add(CorrItem.CorrUID);
                                                    
                                            }

                                        }
                                        catch (Exception ex)
                                        {

                                            SendNotificationToTarasol("", CorrItem.CorrUID, 0, 0, "", "");
                                            throw ex;
                                        }

                                    }
                                }


                            }
                        }
                    }
                    
                }
                return null; 
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message, PrintInterface: true);
                throw ex;
            }

        }
        
        public static void FaxInquiry(ref FaxInquiry[] faxInquiries)
        {
            try
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start", TrackInterface: true);
                //FaxOutgoingRef.ServiceSoapClient serviceSoapClient = new FaxOutgoingRef.ServiceSoapClient(FaxOutgoingRef.ServiceSoapClient.EndpointConfiguration.ServiceSoap);
                foreach (var request in faxInquiries)
                {
                    //Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "FaxStatusAsync - no :" + request.RequestNo);
                    /*request.status = "0";
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "FaxStatusAsync - request.status :" + request.status);
                    if (request.status.ToLower() == "success" || request.status == "1")
                    {
                        DeliveryRequest FaxDeliveryRequest = new DeliveryRequest()
                        {
                            acknowledgementDate = DateTime.Now,
                            TransactionID = request.ExchangeNo
                        };
                        Correspondence correspondence = new Correspondence();
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Write Ack - ExchangeNo:" + request.ExchangeNo);
                        correspondence.WriteAck(FaxDeliveryRequest);
                    }*/
                    
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message, PrintInterface: true);
                throw ex;
            }

        }
        
        public static object SendAck(IHostingEnvironment _hostingEnvironment, ExportCorr[] scanRequest)
        {
            try
            {
                string path = (string)IntegrationSettings.GetSetting("IntegrationDataFolder", _hostingEnvironment) + "/Out/Draft";
                if (Directory.Exists(path))
                {
                    string[] subDirectories = Directory.GetDirectories(path);

                    foreach (var subDirectory in subDirectories)
                    {
                        foreach (var CorrItem in scanRequest)
                        {
                            if (subDirectory.EndsWith(CorrItem.CorrUID))
                            {
                                string[] FilePaths = Directory.GetFiles(subDirectory, "*.ack");
                                if (FilePaths.Length == 0)
                                {
                                    continue;
                                }
                                string FilePath = Directory.GetFiles(subDirectory, "*.ack")[0];
                                File.Move(FilePath, Path.ChangeExtension(FilePath, ".zip"));
                                FilePath = FilePath.Replace(".ack", "");
                                ZipFile.ExtractToDirectory(FilePath + ".zip", subDirectory);
                                StreamReader reader;
                                string strJSONdata = "";
                                if (File.Exists(FilePath + ".json"))
                                {
                                    string CorrNameUID = Path.GetFileName(FilePath + ".json").Replace(".json", "");
                                    reader = new StreamReader(FilePath + ".json");
                                    strJSONdata = reader.ReadToEnd();
                                    reader.Dispose();
                                    TarasolAck tarasolAck = JsonConvert.DeserializeObject<TarasolAck>(strJSONdata);
                                    string EntityCode = string.IsNullOrEmpty(tarasolAck.LastParentGlobalID) ? tarasolAck.SenderGlobalID : tarasolAck.LastParentGlobalID.Replace(IntegrationSettings.GetSetting("EntityPrefix").ToString(), "");
                                    if (tarasolAck.ExternalExchangeNo is null || string.IsNullOrEmpty(EntityCode) || tarasolAck.AckDate is null)
                                    {

                                        return IntResultLib.GetResult(2);
                                    }

                                    ECS_DeliveryRequest eCS_Document = new ECS_DeliveryRequest()
                                    {
                                        EntityCode = EntityCode,
                                        TransactionId = tarasolAck.ExternalExchangeNo,
                                        ReadBy = tarasolAck.ReadBy
                                    };
                                    string XMLstring = XMLProcessing.Serialize(eCS_Document);
                                    XMLstring = XMLstring.Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "").Replace(@"<DeliveryReportRequest xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">", "<moic:DeliveryReportRequest> ").Replace("</DeliveryReportRequest>", "</moic:DeliveryReportRequest>");
                                    try
                                    {
                                        IntResult Response = SoapRequest.CallECSDeliveryWebService(XMLstring);
                                    }
                                    catch (Exception ex)
                                    {
                                        File.Delete(FilePath + ".json");
                                        File.Move(FilePath + ".zip", Path.ChangeExtension(FilePath + ".zip", ".ack"));
                                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                                        throw ex;
                                    }
                                    
                                    // after sending succeeds
                                    File.Delete(FilePath + ".json");
                                    File.Move(FilePath + ".zip", Path.ChangeExtension(FilePath + ".zip", ".ack"));

                                }
                            }
                        }


                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message, PrintInterface: true);
                throw ex;
            }

        }
        
        public static object SendNotificationToTarasol(String exchangeNo, String corrUID, int status, int RequestNo, string lMessage, string eMessage)
        {
            try
            {
                Logger.Enum_LogType t = Logger.Enum_LogType._Info;
                ////bool EnableLoggingInfo = (bool)IntegrationSettings.GetSetting("EnableLoggingInfo");
                if (status == 0)
                {
                    t = Logger.Enum_LogType._Error;
                    lMessage = "›‘·  ’œÌ— «·„—«”·… »”»» „‘ﬂ·… ›Ì »Ì«‰« Â«";
                    eMessage = "correspondence export failed due to metadata error";
                }
                else if (status == 2)
                {
                    t = Logger.Enum_LogType._Error;
                    lMessage = "›‘·  ’œÌ— «·„—«”·… »”»» „‘ﬂ·… »«·« ’«· ⁄»— ESB";
                    eMessage = "correspondence export failed due to ESB connection error";
                }
                else if (status == 1)
                {
                    lMessage = " „  ’œÌ—«·„—«”·… »‰Ã«Õ";
                    eMessage = "correspondence export exported successfully";
                }
                string NotificationURL = IntegrationSettings.GetSetting("APIURL").ToString() + "/Corr/corrExportStatus";
                string json = @"{
                    ""docUid"":""" + corrUID + @""",
	                ""status"":" + status + @",
	                ""ExchangeNo"":" + exchangeNo + @",
	                ""RequestNo"":" + RequestNo + @",
	                ""FailReasonEmsg"":""" + eMessage + @""",
                    ""FailReasonLmsg"":""" + lMessage + @""",
                    }";

                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, t, "NotificationURL : " + NotificationURL, TrackInterface: true);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, t, "Notification data : " + json,TrackInterface: Logger.EnableLoggingInfo, PrintInterface: true);
                string Tarasol_Token = Cache.GetSession().Token;
                if (string.IsNullOrEmpty(Tarasol_Token))
                {
                    //Cache.SetCache("TarasolSession", null);
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Tarasol_Token : Unauthorized user");
                    return null;
                }
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Tarasol_Token : " + Tarasol_Token.Length, TrackInterface: true);
                object jsonResponse = CallExternalAPI.CallPostAPI(NotificationURL, json, Tarasol_Token);
                return jsonResponse;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message, PrintInterface: true);
                return null;
                //throw ex;
            }
        }

        public static object AcknowledgmentReportLoad()
        {
            try
            {
                string Tarasol_Token = Cache.GetSession().Token;
                string SessionID = Cache.GetSession().SessionID;;
                if (Tarasol_Token == "")
                {
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Tarasol_Token : Unauthorized user");
                    return null;
                }
                string apiurl = IntegrationSettings.GetSetting("APIURL").ToString() + "/Corr/AcknowledgmentTransmittalReportLoad?CurrentPage=1&MaxNumberItems=15&SessionId="+SessionID;
                string json = @"{""SearchList"":[],""OrderList"":[]}";

                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "apiurl : " + apiurl, TrackInterface: true);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Tarasol_Token : " + Tarasol_Token.Length, TrackInterface: true);
                CallExternalAPI.CallPostAPI(apiurl, json, Tarasol_Token);
                return null;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message, PrintInterface: true);
                throw ex;
            }
        }
        //public static object GetTargetByGlobalID(string Globalid)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(Globalid)) return false;

        //        string Tarasol_Token = Cache.GetSession().Token;
        //        string Session_ID = Cache.GetSession().SessionID;
        //        string ApiUrl = IntegrationSettings.GetSetting("APIURL").ToString() + "/Config/ContactsItemsGet?ContactType=TargetExternal&SessionID=" + Session_ID;
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ApiUrl : " + ApiUrl);

        //        if (string.IsNullOrEmpty(Tarasol_Token))
        //        {
        //            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Tarasol_Token : Unauthorized user");
        //            return false;
        //        }
        //        List<SearchCls> searchList = new List<SearchCls>();
        //        searchList.Add(new SearchCls()
        //        {
        //            FieldName = "GlobalID",
        //            Opertation = "eq",
        //            Value = Globalid
        //        });
        //        DepartmentPositionsReqCls departmentPositionsReqCls = new DepartmentPositionsReqCls()
        //        {
        //            SearchList = searchList
        //        };
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Tarasol_Token : " + Tarasol_Token.Length);
        //        string jsondata = JsonConvert.SerializeObject(departmentPositionsReqCls);
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ApiUrl post data : " + jsondata);

        //        DepartmentPositionList jsonResponse = JsonConvert.DeserializeObject<DepartmentPositionList>(CallExternalAPI.CallPostAPI(ApiUrl, jsondata, Tarasol_Token).ToString());
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ApiUrl post data : " + JsonConvert.SerializeObject(jsonResponse));
        //        return jsonResponse.CountofRecords != "0";
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
        //        throw ex;
        //    }
        //}
    }
}
