using IntegrationCore.Helper;
using IntegrationCore.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace IntegrationCore.IntegrationLibrary
{
    public class Correspondence
    {
        public const String NameMainFilePDF = "___Main___PDF___T04File";
        public const String PDFExtenstion = ".pdf";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tarasolDocument"></param>
        /// <returns></returns>
        public static object ImportToFolder(TarasolDocument tarasolDocument)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            string msgResponse = string.Empty;
            string GUID = Guid.NewGuid().ToString();

            try
            {
                Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, $"GUID : {GUID} ", TrackInterface: true);
                //Jobs.AddLogJobESB("ImportToFolder", "Started", (int)ICALibrary.Helper.Enum.responseCode.Success, startDate, endDate, 0, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                //Check Required fields
                if (string.IsNullOrEmpty(tarasolDocument.Subject) || string.IsNullOrEmpty(tarasolDocument.CorrespondenceNo) || tarasolDocument.Date == null)
                {
                    Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage 1  : DataException");
                    if (string.IsNullOrEmpty(tarasolDocument.Subject))
                    {
                        msgResponse = "stage 1  Subject:" + string.IsNullOrEmpty(tarasolDocument.Subject);
                    }
                    else if (string.IsNullOrEmpty(tarasolDocument.CorrespondenceNo))
                    {
                        msgResponse = "stage 1  CorrespondenceNo:" + string.IsNullOrEmpty(tarasolDocument.CorrespondenceNo);
                    }
                    else if (tarasolDocument.Date == null)
                    {
                        msgResponse = "stage 1  Date:" + (tarasolDocument.Date == null);
                    }
                    Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, msgResponse);
                    endDate = DateTime.Now;
                    //Jobs.AddLogJobESB("ImportToFolder", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Failed, startDate, endDate, (endDate - startDate).TotalSeconds, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);

                    throw new System.Data.DataException();
                }
                //if(string.IsNullOrEmpty(tarasolDocument.Classification) || tarasolDocument.Sender == null || tarasolDocument.Receiver == null || tarasolDocument.Files == null)
                //{
                //    throw new System.Data.DataException();
                //}
                if (tarasolDocument.Sender == null || tarasolDocument.Receiver == null || tarasolDocument.Files == null) // TODO
                {
                    Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage 2  : DataException");
                    if (tarasolDocument.Sender == null)
                    {
                        msgResponse = "stage 2  Sender:" + (tarasolDocument.Sender == null);
                    }
                    else if (tarasolDocument.Receiver == null)
                    {
                        //Post Req: http://ar126appv02/TarasolAPI412/Config/DepartmentPositionsListGet?CurrentPage=1&MaxNumberItems=20&SessionId=bce251df-ae69-4632-ba27-0d459df7fd2e
                        //Post data{"SearchList":[{"FieldName":"GlobalId","Opertation":"contains","Value":"40000","LogicOP":""}],"OrderList":[]}
                        //Response when found: {"Data":[{"ActivateLink":false,"CompleteLink":false,"DisableWorkflow":false,"WorkflowCount":0,"LinkDate":null,"Flags":"","IsImport":false,"RoleDistributorEName":"","RoleDistributorLName":"","DepTypeEName":"General administration","DepTypeLName":"إدارة عامة","GlobalId":"40000","DepType":1,"IsMain":true,"MainParentEName":"M : New Vision System","MainParentLName":"M : نيو فيجن سيستم","MainParentId":"7E8F6196-DD64-481E-A68F-465EA005D0C6","PrintCode":"","IsActive":true,"UID":"7E8F6196-DD64-481E-A68F-465EA005D0C6","EName":"M : New Vision System","Fax":"","LName":"M : نيو فيجن سيستم","MyParentCode":"","MyParentEName":"","MyParentLName":"","OldDepartment":null,"Code":"M","ParentID":"","RoleDistributor":"","Notes":""}],"CountofRecords":1,"NumberOfPages":1}
                        //Empty Response: {"Data":[],"CountofRecords":0,"NumberOfPages":0}
                        msgResponse = "stage 2  Receiver:" + (tarasolDocument.Receiver == null);
                    }
                    else if (tarasolDocument.Files == null)
                    {
                        msgResponse = "stage 2  Files:" + (tarasolDocument.Files == null);
                    }
                    Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, msgResponse);
                    endDate = DateTime.Now;
                    //Jobs.AddLogJobESB("ImportToFolder", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Failed, startDate, endDate, (endDate - startDate).TotalSeconds, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);

                    throw new System.Data.DataException();
                }
                //Get import folder path
                string path = (string)IntegrationSettings.GetSetting("IntegrationDataFolder") + "/In/Draft/"
                    + GUID;
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "stage 4  : pass");

                path += "/" + GUID;
                DirectoryInfo di = Directory.CreateDirectory(path);
                List<Attachment> attachments = tarasolDocument.Files;
                foreach (var data in attachments)
                {
                    var base64string = data.FileContent;
                    var base64array = Convert.FromBase64String(base64string);
                    //string filePath = path + "/" + data.FileName + "." + data.FileExtension;
                    string filePath = path + "/" + ((data.FileName.Contains(NameMainFilePDF)) ? GUID + ".pdf" : data.FileName) + "." + data.FileExtension;
                    if (filePath.Length >= ICALibrary.Helper.Constant.MaxFilePathLength)
                    {
                        string GUIDFile = Guid.NewGuid().ToString();
                        filePath = $"{path}/{GUIDFile}.{data.FileExtension}";
                    }

                    System.IO.File.WriteAllBytes(filePath, base64array);
                }

                Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "stage 5  : pass");
                DocumentObject documentObject = Mapping.MapImportedFile(tarasolDocument);
                string jsondata = JsonConvert.SerializeObject(documentObject);

                if (documentObject.AddressTo.Count == 0)
                {
                    return IntResultLib.GetResult(2);
                }

                if (documentObject.PostFromID.GlobalIDs == null)
                {
                    Exception ex = new Exception("The sender global id is not define in system");
                    Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message + $" Correspondence number {documentObject.ExternalRefNumber.Value}, GUID {GUID}", TrackInterface: true);
                    return IntResultLib.GetResult(ex);
                }


                // Write that JSON to  file,  
                System.IO.File.WriteAllText(path + "/" + GUID + ".json", jsondata, encoding: Encoding.UTF8);
                // Create zip file for correspondence
                ZipFile.CreateFromDirectory(path, path + ".zip");
                File.Move(path + ".zip", Path.ChangeExtension(path + ".zip", ".tarasol"));
                Directory.Delete(path, true);

                Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "stage 6  : pass");

                endDate = DateTime.Now;
                //Jobs.AddLogJobESB("ImportToFolder", "End - result : Success", (int)ICALibrary.Helper.Enum.responseCode.Success, startDate, endDate, (endDate - startDate).TotalSeconds, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                ///////////////////////
                string corrImportResult = CallCorrespondecesImport();
                ///
                Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, corrImportResult);

                return IntResultLib.GetResult(1); // if operation succeeded

            }
            catch (Exception ex)
            {
                Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, msgResponse);
                //Jobs.AddLogJobESB("ImportToFolder", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Failed, startDate, endDate, (endDate - startDate).TotalSeconds, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                throw ex;
            }
        }


        public object WriteAck(DeliveryRequest acknowledegment)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            string msgResponse = string.Empty;
            string objStr = JsonConvert.SerializeObject(acknowledegment);

            try
            {
                //Jobs.AddLogJobESB("WriteAck", "Started", (int)ICALibrary.Helper.Enum.responseCode.Success, startDate, endDate, 0, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                if (acknowledegment is null)
                {
                    return IntResultLib.GetResult(2);
                }
                string DocPathName = acknowledegment.acknowledgementDate.ToString("yyyy") + "." + acknowledegment.TransactionID + "." + acknowledegment.EntityCode;
                //Get delivery reports folder path
                string path = (string)IntegrationSettings.GetSetting("IntegrationDataFolder") + "/In/Draft/" + DocPathName;
                //string path = (string)IntegrationSettings.GetSetting("IntegrationDataFolder", _hostingEnvironment) + "/In/Draft/" + DocPathName;
                if (!Directory.Exists(path))
                {
                    TarasolAck tarasolAck = new TarasolAck()
                    {
                        AckDate = Mapping.ConvertToDateString(acknowledegment.acknowledgementDate),
                        ExternalExchangeNo = acknowledegment.TransactionID,
                        LastParentGlobalID = acknowledegment.EntityCode,
                        ReadBy = acknowledegment.ReadBy,
                        ExchangeStatus = ((int)Helper.Enum.enumExchangStatus.Folder).ToString()
                        //ReferenceNumber = acknowledegment.CorrespondenceNo,
                        //ReciveDept = acknowledegment.GlobalID,
                        //SenderParentGlobalID = acknowledegment.GlobalID.DeptCode
                    };
                    if (tarasolAck.ExternalExchangeNo is null || tarasolAck.LastParentGlobalID == 0 || tarasolAck.AckDate is null)
                    {
                        return IntResultLib.GetResult(2);
                    }
                    Directory.CreateDirectory(path + "/" + DocPathName);
                    //acknowledegment.GlobalID.DeptLName = acknowledegment.ReadBy;

                    string JsonAck = JsonConvert.SerializeObject(tarasolAck);
                    File.WriteAllText(path + "/" + DocPathName + "/" + DocPathName + ".json", JsonAck, encoding: Encoding.UTF8);
                    ZipFile.CreateFromDirectory(path + "/" + DocPathName,
                        path + "/" + DocPathName + ".zip");
                    Directory.Delete(path + "/" + DocPathName, true);
                    File.Move(path + "/" + DocPathName + ".zip", Path.ChangeExtension(path + "/" + DocPathName + ".zip", ".ack"));
                }

                endDate = DateTime.Now;
                //Jobs.AddLogJobESB("WriteAck", "End - result : Success", (int)ICALibrary.Helper.Enum.responseCode.Success, startDate, endDate, (endDate - startDate).TotalSeconds, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                AcknowledgmentReportLoad();
                return IntResultLib.GetResult(1);
            }
            catch (Exception ex)
            {
                //Jobs.AddLogJobESB("WriteAck", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Failed, startDate, endDate, (endDate - startDate).TotalSeconds, objStr, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                throw ex;
            }
        }
        public static object ExportFromFolderTask(IHostingEnvironment _hostingEnvironment, ExportCorr[] scanRequest)
        {
            //Task.Delay(0).Wait();
            string NotificationURL = IntegrationSettings.GetSetting("APIURL").ToString() + "/Corr/corrExportStatus?ExchangeNo=&status=1&Docuid=&RequestNo=0";
            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "NotificationURL : " + NotificationURL);
            string Tarasol_Token = Cache.GetSession().Token;
            //CallExternalAPI.CallGetAPI(NotificationURL, Tarasol_Token);
            return CallExternalAPI.CallGetAPI(NotificationURL, Tarasol_Token);
        }


        public static ICALibrary.Models.IntResult ExportFromFolderICAEmail(string entity, DocumentObject document, ICALibrary.Models.DocumentRequest.Attachments ECSAttachmentList, IHostingEnvironment _hostingEnvironment, ConfigAttribute configAttribute)
        {
            string TransactionRefNo = string.Empty;
            string msgResponse = string.Empty;
            ICALibrary.Models.EmailObject.EmailAttachment attachments = new ICALibrary.Models.EmailObject.EmailAttachment();
            
            if (ECSAttachmentList.attachmentItems.Count > 0)
            {
                // this solution of naming attachment temporary because we have problem with name arabic in exchange server
                foreach (ICALibrary.Models.DocumentRequest.Attachment item in ECSAttachmentList.attachmentItems)
                {
                    ICALibrary.Models.EmailObject.AttachmentItem elem = new ICALibrary.Models.EmailObject.AttachmentItem();
                    if (item.DocumentName.Contains(NameMainFilePDF))
                    {
                        elem.name = document.Subject.Value + Path.GetExtension(item.DocumentName);
                    }
                    else
                    {
                        elem.name = Path.GetFileNameWithoutExtension(item.DocumentName) + Path.GetExtension(item.DocumentName);
                    }
                    elem.name = ICALibrary.Helper.Common.RemoveSpecialChar(elem.name);
                    elem.contentType = item.MimeType; // item.FileType; // "application/octet-stream"; //item.attachmentItem.contentType;
                    elem.data = item.Stream; //ICALibrary.Helper.Common.Base64Encode(item.attachmentItem.data);
                    attachments.attachmentItem.Add(elem);
                }
            }

            List<string> _Emailcc = new List<string>();
            List<string> _EmailTo = new List<string>();
            foreach (var item in document.AddressTo)
            {
                if (item.ExchangeStatus == ((int)Helper.Enum.enumExchangStatus.Email).ToString())
                {
                    _EmailTo.Add(item.Email);
                }
            }

            ICALibrary.Models.EmailObject.Notification notification = new ICALibrary.Models.EmailObject.Notification
            {
                notificationType = ICALibrary.Helper.Constant.notificationTypeEmail,
                toAddress = String.Join(";", _EmailTo.ToArray()),// email.Notification.toAddress,
                subject = ICALibrary.Helper.Common.RemoveSpecialChar(document.Subject.Value),
                fromAddress = configAttribute.senderMail,
                messageBody = Mail.GetBodyEmail(_hostingEnvironment.ContentRootPath, document.Subject.Value, document.ReferenceNo.Value), // must be base64 only for tarasol channel
                                                                                                                                          // importance = string.Empty, // email.Notification.importance,
                                                                                                                                          //ccAddress = String.Join(";", _Emailcc.ToArray()),
                                                                                                                                          // bccAddress = email.Notification.ccAddress,
                                                                                                                                          // attachments = attachments
            };

            if (_Emailcc.Count > 0)
            {
                notification.ccAddress = String.Join(";", _Emailcc.ToArray());
            }
            if (attachments.attachmentItem.Count > 0)
            {
                notification.attachments = attachments;
            }

            ICALibrary.Models.EmailObject.Body bodyRequest = new ICALibrary.Models.EmailObject.Body
            {
                Notification = notification,
                transactionRefNo = ICALibrary.Business.SMS.GenerateTransactionRefNo(configAttribute.consumerID),
                timestamp = DateTime.Now
            };

            string XMLstringBody = XMLProcessing.Serialize(bodyRequest);
            XMLstringBody = ICALibrary.Helper.Common.ReplaceXMLStr(XMLstringBody);
            TransactionRefNo = bodyRequest.transactionRefNo;

            // Unicode XML Formatted
            //String UnicodeXMLFormatted = XMLstringBody.Replace("<not1:Body>", "").Replace("</not1:Body>", "");
            //UnicodeXMLFormatted = ICALibrary.Helper.Common.UnicodeXMLFormatted(UnicodeXMLFormatted);

            // Header Request
            String DevelopSecKey = IntegrationSettings.GetSetting("DevelopSecKey").ToString();
            ICALibrary.Models.SMSRequest.Header headerRequest = new ICALibrary.Models.SMSRequest.Header
            {
                hash = String.Format("{0}", ICALibrary.Helper.MessageIntegrityEncrypter.SignMessageICA(XMLstringBody, DevelopSecKey)),
                password = ICALibrary.Helper.MessageIntegrityEncrypter.EncryptDataICA(Encryption.DecryptString(IntegrationSettings.GetSetting("Password").ToString()), DevelopSecKey), //ICALibrary.Helper.Constant.Password,
                userName = ICALibrary.Helper.MessageIntegrityEncrypter.EncryptDataICA(IntegrationSettings.GetSetting("UserName").ToString(), DevelopSecKey), //ICALibrary.Helper.Constant.Username,
                serviceLanguage = ICALibrary.Business.Email.GetLanguageMessage(ICALibrary.Helper.Constant.ENGLISH_NUM),
                serviceName = ICALibrary.Helper.Constant.serviceNameSMS,
                serviceVersion = ICALibrary.Helper.Constant.serviceVersionSMS,
                sourceChannel = ICALibrary.Helper.Constant.sourceChannelSMS
            };


            string XMLstringHeader = XMLProcessing.Serialize(headerRequest);
            ICALibrary.Models.IntResult Response = ICALibrary.Business.Email.CallEmailWebService(XMLstringHeader, XMLstringBody, configAttribute.urlServiceNotify);

            if (Response.Code == (int)ICALibrary.Helper.Enum.responseCode.Success)
            {
                msgResponse = $"End - result : {Response.EMessage}, TransactionRefNo: { TransactionRefNo} ";
                //Jobs.AddLogJobESB("ExportFromFolderICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Success, startDate, endDate, (endDate - startDate).TotalSeconds, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, msgResponse, TrackInterface: true);
                Response.EMessage = "Success";
                //Response.Entity = entity;
                //endDate = DateTime.Now;
                //msgResponse = "End - result : " + Response.EMessage;
                //Jobs.AddLogJobESB("SendSMSICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Success, startDate, endDate, (endDate - startDate).TotalSeconds, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);
            }
            else if (Response.Code == (int)ICALibrary.Helper.Enum.responseCode.Failed)
            {
                msgResponse = $"End - Details : {Response.Details}, TransactionRefNo: { TransactionRefNo}";
                //Jobs.AddLogJobESB("ExportFromFolderICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Failed, startDate, endDate, (endDate - startDate).TotalSeconds, objStr, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, msgResponse, TrackInterface: true);
                Response.EMessage = "Unable to send Email";
            }
            Response.Entity = entity;

            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_hostingEnvironment"></param>
        /// <param name="scanRequest"></param>
        /// <param name="consumerID"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static object ExportFromFolderICA(IHostingEnvironment _hostingEnvironment, ExportCorr[] scanRequest, ConfigAttribute configAttribute)
        {
            try
            {
                string path = (string)IntegrationSettings.GetSetting("IntegrationDataFolder", _hostingEnvironment) + "/Out";

                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now;
                string msgResponse = string.Empty;
                string objStr = JsonConvert.SerializeObject(scanRequest);
                string TransactionRefNo = string.Empty;
                //Jobs.AddLogJobESB("ExportFromFolderICA", "Started", (int)ICALibrary.Helper.Enum.responseCode.Success, startDate, endDate, 0, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);


                if (Directory.Exists(path))
                {

                    string[] subDirectories = Directory.GetDirectories(path);

                    foreach (var subDir in subDirectories)
                    {
                        if (subDir.EndsWith("Draft") || subDir.EndsWith("Draft-fax") || subDir.EndsWith("Draft-email"))
                        {

                            string[] DraftSubDirectories = Directory.GetDirectories(subDir);

                            foreach (var subDirectory in DraftSubDirectories)
                            {


                                foreach (var CorrItem in scanRequest)
                                {
                                    if (subDirectory.EndsWith(CorrItem.CorrUID))
                                    {
                                        string[] FilePaths = Directory.GetFiles(subDirectory, "*.tarasol");
                                        string[] FilePathsZip = Directory.GetFiles(subDirectory, "*.zip");
                                        if (FilePaths.Length == 0 && FilePathsZip.Length == 0)
                                        {
                                            continue;
                                        }

                                        String FilePath = string.Empty;
                                        if (FilePathsZip.Length > 0)
                                        {
                                            FilePath = Directory.GetFiles(subDirectory, "*.zip")[0];
                                            FilePath = FilePath.Replace(".zip", "");

                                        }
                                        else
                                        {
                                            FilePath = Directory.GetFiles(subDirectory, "*.tarasol")[0];
                                            File.Move(FilePath, Path.ChangeExtension(FilePath, ".zip"));
                                            FilePath = FilePath.Replace(".tarasol", "");
                                            ZipFile.ExtractToDirectory(FilePath + ".zip", subDirectory, true);
                                        }


                                        StreamReader reader;
                                        string strJSONdata = "";
                                        if (File.Exists(FilePath + ".json"))
                                        {
                                            string CorrNameUID = Path.GetFileName(FilePath + ".json").Replace(".json", "");
                                            reader = new StreamReader(FilePath + ".json");
                                            strJSONdata = reader.ReadToEnd();
                                            reader.Dispose();
                                            DocumentObject document = JsonConvert.DeserializeObject<DocumentObject>(strJSONdata);
                                            List<string> EntityCodes = new List<string>();
                                            string[] Files = Directory.GetFiles(subDirectory, "*.*");
                                            ICALibrary.Models.DocumentRequest.Attachments ECSAttachmentList = new ICALibrary.Models.DocumentRequest.Attachments();
                                            ECSAttachmentList.attachmentItems = new List<ICALibrary.Models.DocumentRequest.Attachment>();
                                            foreach (string item in Files)
                                            {
                                                if (!item.EndsWith(CorrNameUID + ".tarasol") && !item.EndsWith(CorrNameUID + ".zip") && !item.EndsWith(CorrNameUID + ".json"))
                                                {
                                                    string mimeType = Path.GetExtension(item);
                                                    byte[] bytes = File.ReadAllBytes(item);
                                                    string fileName = Path.GetFileName(item);

                                                    FileInfo fileInfo = new FileInfo(item);
                                                    // add this condition because there is limitation in size send throw ESB for that we make copmress PDF only which has big size
                                                    if (fileInfo.Length > configAttribute.maxSizeFileCompress && mimeType.ToLower() == PDFExtenstion)
                                                    {
                                                        string GUIDTemp = Guid.NewGuid().ToString();
                                                        string GUIDFild = Guid.NewGuid().ToString();
                                                        string copressItem = item.Replace(fileName, "cps_" + GUIDFild + PDFExtenstion);
                                                        string tempItem = item.Replace(fileName, GUIDTemp + PDFExtenstion);
                                                        try
                                                        {
                                                            File.Copy(item, tempItem, true);
                                                            var compress = new CompressPdf(tempItem.Replace('\"', ' ').Trim(), copressItem.Replace('\"', ' ').Trim());
                                                            compress.ProcessFiles();
                                                            bytes = File.ReadAllBytes(copressItem);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Logger.AddToLogFile("CompressPdf." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "CompressPdf : " + ex.Message);
                                                        }

                                                        if (File.Exists(tempItem))
                                                        {
                                                            File.Delete(tempItem);
                                                        }
                                                        if (File.Exists(copressItem))
                                                        {
                                                            File.Delete(copressItem);
                                                        }
                                                    }
                                                    string stream = Convert.ToBase64String(bytes);
                                                    //ICALibrary.Models.DocumentRequest.AttachmentItem eCSAttachment = new ICALibrary.Models.DocumentRequest.AttachmentItem();

                                                    if (!string.IsNullOrWhiteSpace(stream))
                                                    {
                                                        ICALibrary.Models.DocumentRequest.Attachment eCSAttachmentItem = new ICALibrary.Models.DocumentRequest.Attachment()
                                                        {
                                                            Stream = stream,
                                                            DocumentName = (fileName == CorrNameUID + ".pdf") ? NameMainFilePDF + ".pdf" : ICALibrary.Helper.Common.RemoveSpecialChar(fileName),
                                                            AttDescription = ICALibrary.Helper.Common.RemoveSpecialChar(fileName),
                                                            MimeType = mimeType
                                                        };
                                                        //eCSAttachment.attachmentItem = eCSAttachmentItem;
                                                        ECSAttachmentList.attachmentItems.Add(eCSAttachmentItem);
                                                        //File.Delete(item);
                                                    }
                                                    else
                                                    {
                                                        Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, $"{fileName} Attachment hasn't data ", TrackInterface: true);
                                                    }

                                                }
                                            }


                                            Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, $"CorrUID  : {CorrItem.CorrUID} ", TrackInterface: true);

                                            // if the action is resend-to we need change address to
                                            if (CorrItem.ResendTo != null)
                                            {
                                                document.AddressTo = CorrItem.ResendTo;
                                            }

                                            // TODO Ghaith
                                            if (CorrItem.IsFirstTry == false)
                                            {
                                                List<TarasolTargetItem> newAddressTo = new List<TarasolTargetItem>();
                                                foreach (var item in document.AddressTo)
                                                {
                                                    if (CorrItem.ExchangeNo.Contains(item.ExchangeNo))
                                                    {

                                                        if (item != null && item.GlobalIDs != null)
                                                        {
                                                            item.GlobalIDs.MainParentGlobalID = Helper.Common.RemovePrefixEntity(item.GlobalIDs.MainParentGlobalID);
                                                        }
                                                        newAddressTo.Add(item);
                                                    }
                                                }
                                                document.AddressTo = newAddressTo;
                                            }

                                            foreach (var item in document.AddressTo)
                                            {
                                                if (item != null && item.GlobalIDs != null)
                                                {
                                                    item.GlobalIDs.MainParentGlobalID = Helper.Common.RemovePrefixEntity(item.GlobalIDs.MainParentGlobalID);
                                                    String element = string.IsNullOrWhiteSpace(item.GlobalIDs.MainParentGlobalID) ? item.GlobalIDs.GlobalID : item.GlobalIDs.MainParentGlobalID;
                                                    if (item.ExchangeStatus == ((int)Helper.Enum.enumExchangStatus.Email).ToString())
                                                    {
                                                       
                                                        if (!EntityCodes.Contains(item.Email)) EntityCodes.Add(item.Email);
                                                    }
                                                    else
                                                    {
                                                        if (!EntityCodes.Contains(element)) EntityCodes.Add(element);
                                                    }
                                                }
                                            }

                                            // Check list empty
                                            if (EntityCodes.Count == 0)
                                            {
                                                Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "stage 1  : EntityCodes Empty;" + CorrItem.CorrUID);
                                            }

                                            List<ICALibrary.Models.IntResult> ResponseList = new List<ICALibrary.Models.IntResult>();

                                            // Type Folder corr send
                                            if (subDir.EndsWith("Draft"))
                                            {

                                                foreach (var entity in EntityCodes)
                                                {
                                                    ICALibrary.Models.DocumentRequest.Destination destinationList = new ICALibrary.Models.DocumentRequest.Destination();
                                                    List<string> transactionIdList = new List<string>();
                                                    ICALibrary.Models.DocumentRequest.TransactionIds transactionIdElementList = new ICALibrary.Models.DocumentRequest.TransactionIds();
                                                    List<ICALibrary.Models.DocumentRequest.ReceiverDetail> destination = new List<ICALibrary.Models.DocumentRequest.ReceiverDetail>();
                                                    bool sendByEmail = false;

                                                    foreach (var item in document.AddressTo)
                                                    {
                                                        if (entity == item.GlobalIDs.MainParentGlobalID && item.ExchangeStatus == "1") //Folder type
                                                        {
                                                            ICALibrary.Models.DocumentRequest.ReceiverDetail element = new ICALibrary.Models.DocumentRequest.ReceiverDetail();
                                                            //destination.receiverDetail = new ICALibrary.Models.DocumentRequest.ReceiverDetail();
                                                            element.DestinationValue = item.GlobalIDs.GlobalID;
                                                            destination.Add(element);
                                                            string elem = item.ExchangeNo + "@" + item.GlobalIDs.GlobalID + "@" + (string)IntegrationSettings.GetSetting("SiteEndPoint");
                                                            transactionIdList.Add(elem);
                                                        }
                                                        else if (entity == item.Email && item.ExchangeStatus == ((int)Helper.Enum.enumExchangStatus.Email).ToString())
                                                        {
                                                            // Send by Email
                                                            sendByEmail = true;
                                                            Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, $"Email Send to {entity} , " + CorrItem.CorrUID);
                                                            ICALibrary.Models.IntResult intResult = ExportFromFolderICAEmail(entity, document, ECSAttachmentList, _hostingEnvironment, configAttribute);
                                                            ResponseList.Add(intResult);
                                                            // End
                                                        }
                                                    }

                                                    destinationList.receiverDetail = destination;
                                                    transactionIdElementList.TransactionIdValue = transactionIdList;

                                                    // Check list empty
                                                    if (destinationList.receiverDetail.Count == 0)
                                                    {
                                                        Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "stage 2  : Destination List Empty;" + CorrItem.CorrUID);
                                                    }
                                                    if (destinationList.receiverDetail.Count == 0)
                                                    {
                                                        Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "stage 3  : Transaction Id List List Empty;" + CorrItem.CorrUID);
                                                    }

                                                    // Expose ESB service
                                                    if (destinationList.receiverDetail.Count > 0 && transactionIdList.Count > 0)
                                                    {

                                                        int secrecyLevel = Convert.ToInt16(document.SecrecyID.Value);
                                                        int priorityLeve = Convert.ToInt16(document.PriorityID.Value);

                                                        ICALibrary.Models.DocumentRequest.Sender sender = new ICALibrary.Models.DocumentRequest.Sender(); // TODO Must rollback comment after test add Global ID
                                                        sender.SourceValue = document.PostFromID.GlobalIDs.GlobalID;
                                                        if (string.IsNullOrWhiteSpace(document.PostFromID.GlobalIDs.GlobalID))
                                                        {
                                                            sender.SourceValue = document.PostFromID.GlobalIDs.DirectParentGlobalID;
                                                        }

                                                        foreach (var trackElem in destinationList.receiverDetail)
                                                        {
                                                            if (sender != null && trackElem != null)
                                                            {
                                                                Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, $"{sender?.SourceValue} => {entity}@{trackElem?.DestinationValue}", TrackInterface: true);
                                                            }
                                                        }
                                                        //--
                                                        if (!string.IsNullOrWhiteSpace(sender.SourceValue))
                                                        {
                                                            ICALibrary.Models.DocumentRequest.Body bodyRequest = new ICALibrary.Models.DocumentRequest.Body()
                                                            {
                                                                //EntityCode = entity, // last parent TODO Must rollback comment after test
                                                                orgId = entity,
                                                                SourceUnitId = sender,//Tarasol Global ID 
                                                                DestinationList = destinationList,
                                                                OutboxNumber = document.ExternalRefNumber.Value.Trim(),
                                                                DocumentSubject = ICALibrary.Helper.Common.RemoveSpecialChar(document.Subject.Value),
                                                                OutboxDate = Mapping.ConvertToDateTime(document.CreationDate.Value).ToString("yyyy-MM-ddTHH:mm:ss"),
                                                                Priority = Convert.ToInt16(document.PriorityID.Value),
                                                                Attachments = ECSAttachmentList,
                                                                SecrecyLevel = (secrecyLevel - 1),
                                                                TransactionIds = transactionIdElementList,//transactionIdList,
                                                                HtmlContent = ICALibrary.Helper.Common.RemoveRegexBodyHTML(document.Body.Value), //document.Body.Value, // "Testing service" //TODO change Must rollback comment after test add real body document html
                                                                DocumentType = 4,
                                                                transactionRefNo = ICALibrary.Business.Document.GenerateTransactionRefNo(configAttribute.consumerID),
                                                                timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") // format requiered from side ESB 
                                                            };

                                                            string XMLstringBody = XMLProcessing.Serialize(bodyRequest);
                                                            XMLstringBody = ICALibrary.Helper.Common.ReplaceXMLStr(XMLstringBody);
                                                            TransactionRefNo = bodyRequest.transactionRefNo;

                                                            // Header Request
                                                            String DevelopSecKey = IntegrationSettings.GetSetting("DevelopSecKey").ToString();
                                                            ICALibrary.Models.DocumentRequest.Header headerRequest = new ICALibrary.Models.DocumentRequest.Header
                                                            {
                                                                hash = String.Format("{0}", ICALibrary.Helper.MessageIntegrityEncrypter.SignMessageICA(XMLstringBody, DevelopSecKey)),
                                                                password = ICALibrary.Helper.MessageIntegrityEncrypter.EncryptDataICA(Encryption.DecryptString(IntegrationSettings.GetSetting("Password").ToString()), DevelopSecKey), //ICALibrary.Helper.Constant.Password,
                                                                userName = ICALibrary.Helper.MessageIntegrityEncrypter.EncryptDataICA(IntegrationSettings.GetSetting("UserName").ToString(), DevelopSecKey), //ICALibrary.Helper.Constant.Username,
                                                                serviceLanguage = ICALibrary.Business.SMS.GetLanguageMessage(ICALibrary.Helper.Constant.ARABIC_NUM),
                                                                serviceName = ICALibrary.Helper.Constant.serviceNameAddDocument,
                                                                serviceVersion = ICALibrary.Helper.Constant.serviceVersionSMS,
                                                                sourceChannel = ICALibrary.Helper.Constant.sourceChannelSMS
                                                            };
                                                            string XMLstringHeader = XMLProcessing.Serialize(headerRequest);
                                                            // End Header Request

                                                            try
                                                            {
                                                                Logger.AddToLogFile("Correspondence." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, $"Document CallAddNewDocumentWebService ESB TransactionRefNo : {TransactionRefNo}");
                                                                //SoapRequest.CallECSWebService(XMLstring);
                                                                // IntResult Response = SoapRequest.CallECSAddWebService(XMLstring);
                                                                // ResponseCodesESB
                                                                //
                                                                var cacheObject = Cache.GetFromCache("ResponseCodesESB");
                                                                ICALibrary.Models.ResponseCodesESB responseCodesESB = (cacheObject is null) ? new ICALibrary.Models.ResponseCodesESB() : (ICALibrary.Models.ResponseCodesESB)cacheObject;

                                                                ICALibrary.Models.IntResult Response = ICALibrary.Business.Document.CallAddNewDocumentWebService(XMLstringHeader, XMLstringBody, configAttribute.urlServiceESB, responseCodesESB);
                                                                // Response need to be manipulated and send to tarasol through notification API

                                                                if (Response.Code == (int)ICALibrary.Helper.Enum.responseCode.Success)
                                                                {
                                                                    endDate = DateTime.Now;
                                                                    msgResponse = $"End - result : {Response.EMessage}, TransactionRefNo: { TransactionRefNo} ";
                                                                    //Jobs.AddLogJobESB("ExportFromFolderICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Success, startDate, endDate, (endDate - startDate).TotalSeconds, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                                                                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, msgResponse, TrackInterface: true);
                                                                    Response.EMessage = entity;
                                                                    Response.Entity = entity;
                                                                    ResponseList.Add(Response);
                                                                }
                                                                else if (Response.Code == (int)ICALibrary.Helper.Enum.responseCode.Failed)
                                                                {
                                                                    endDate = DateTime.Now;
                                                                    msgResponse = $"End - Details : {Response.Details}, TransactionRefNo: { TransactionRefNo}";
                                                                    //Jobs.AddLogJobESB("ExportFromFolderICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Failed, startDate, endDate, (endDate - startDate).TotalSeconds, objStr, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                                                                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, msgResponse, TrackInterface: true);
                                                                    Response.EMessage = entity;
                                                                    Response.Entity = entity;
                                                                    ResponseList.Add(Response);
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                endDate = DateTime.Now;
                                                                msgResponse = "End - Details : " + ex.Message;
                                                                //Jobs.AddLogJobESB("ExportFromFolderICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Failed, startDate, endDate, (endDate - startDate).TotalSeconds, objStr, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                                                                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, $"error occured in  : {msgResponse} TransactionRefNo : {TransactionRefNo} ");
                                                                foreach (TarasolTargetItem NotiAdd in document.AddressTo)
                                                                {
                                                                    if (entity == (string.IsNullOrEmpty(NotiAdd.GlobalIDs.MainParentGlobalID) ? NotiAdd.GlobalIDs.GlobalID : NotiAdd.GlobalIDs.MainParentGlobalID))
                                                                    {
                                                                        var result = SendNotificationTarasol(NotiAdd.ExchangeNo, CorrItem.CorrUID, 0, 0, msgResponse, msgResponse);
                                                                    }
                                                                }
                                                                foreach (string itr in Files)
                                                                {
                                                                    if (!itr.EndsWith(CorrNameUID + ".tarasol") && !itr.EndsWith(CorrNameUID + ".zip"))
                                                                    {
                                                                        if (File.Exists(itr))
                                                                        {
                                                                            File.Delete(itr);
                                                                        }
                                                                    }
                                                                }
                                                                //File.Delete(FilePath + ".json");
                                                                if (File.Exists(FilePath + ".zip"))
                                                                    File.Move(FilePath + ".zip", Path.ChangeExtension(FilePath + ".zip", ".tarasol"));
                                                                //throw ex;
                                                            }
                                                        }
                                                        //--



                                                    }
                                                    else
                                                    {
                                                        if (!sendByEmail)
                                                        {
                                                            ICALibrary.Models.IntResult Response = ICALibrary.Helper.IntResultLib.GetResult(2, " Transaction Id List List Empty;");
                                                            endDate = DateTime.Now;
                                                            msgResponse = $"End - Details : {Response.Details}, TransactionRefNo: { TransactionRefNo}";
                                                            //Jobs.AddLogJobESB("ExportFromFolderICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Failed, startDate, endDate, (endDate - startDate).TotalSeconds, objStr, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                                                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, msgResponse);
                                                            Response.EMessage = entity;
                                                            Response.Entity = entity;
                                                            ResponseList.Add(Response);
                                                        }
                                                    }
                                                }
                                            }

                                            // after sending succeeds
                                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Sending Notification : ");
                                            List<String> sentNotifyBefore = new List<string>();
                                            foreach (var Response in ResponseList)
                                            {
                                                foreach (TarasolTargetItem NotiAdd in document.AddressTo)
                                                {
                                                    if (Response.EMessage == (string.IsNullOrEmpty(NotiAdd.GlobalIDs.MainParentGlobalID) ? NotiAdd.GlobalIDs.GlobalID : NotiAdd.GlobalIDs.MainParentGlobalID)) // TODO GHAITH
                                                    {
                                                        if (!sentNotifyBefore.Contains(NotiAdd.ExchangeNo))
                                                        {
                                                            sentNotifyBefore.Add(NotiAdd.ExchangeNo);
                                                            if (Response.Code == (int)ICALibrary.Helper.Enum.responseCode.Success)
                                                            {

                                                                var result = SendNotificationTarasol(NotiAdd.ExchangeNo, CorrItem.CorrUID, Response.Code, 0, String.Empty, String.Empty);
                                                            }
                                                            else if (Response.Code == (int)ICALibrary.Helper.Enum.responseCode.Failed)
                                                            {
                                                                msgResponse = Response.Details;
                                                                endDate = DateTime.Now;
                                                                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, msgResponse);

                                                                var result = SendNotificationTarasol(NotiAdd.ExchangeNo, CorrItem.CorrUID, (int)Response.statusNotification, 0, msgResponse, msgResponse);
                                                            }
                                                        }
                                                    }
                                                    else if (NotiAdd.ExchangeStatus == ((int)Helper.Enum.enumExchangStatus.Email).ToString() && Response.Entity == NotiAdd.Email)
                                                    {
                                                        var result = SendNotificationTarasol(NotiAdd.ExchangeNo, CorrItem.CorrUID, Response.Code, 0, String.Empty, String.Empty);
                                                    }
                                                }
                                            }
                                            // after sending succeeds
                                            foreach (string item in Files)
                                            {
                                                if (!item.EndsWith(CorrNameUID + ".tarasol") && !item.EndsWith(CorrNameUID + ".zip"))
                                                {
                                                    if (File.Exists(item))
                                                    {
                                                        File.Delete(item);
                                                    }
                                                }
                                            }
                                            //File.Delete(FilePath + ".json");
                                            if (File.Exists(FilePath + ".zip"))
                                            {
                                                File.Move(FilePath + ".zip", Path.ChangeExtension(FilePath + ".zip", ".tarasol"));
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }

                }
                else
                {
                    throw new System.IO.DirectoryNotFoundException();
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw ex;
            }

        }






        /// <summary>
        /// Expose the service notification to ESB ICA
        /// </summary>
        /// <param name="_hostingEnvironment"></param>
        /// <param name="scanRequest"></param>
        /// <returns></returns>
        public static object SendAckICA(IHostingEnvironment _hostingEnvironment, ExportCorr[] scanRequest, ConfigAttribute configAttribute)
        {
            try
            {
                string path = (string)IntegrationSettings.GetSetting("IntegrationDataFolder", _hostingEnvironment) + "/Out/Draft";
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now;
                string msgResponse = string.Empty;
                string objStr = JsonConvert.SerializeObject(scanRequest);
                string TransactionRefNo = string.Empty;
                //Jobs.AddLogJobESB("SendAckICA", "Started", (int)ICALibrary.Helper.Enum.responseCode.Success, startDate, endDate, 0, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);


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
                                string FilePathZIP = FilePath.Replace(".ack", "");
                                if (!File.Exists(FilePathZIP + ".zip"))
                                {
                                    File.Move(FilePath, Path.ChangeExtension(FilePath, ".zip"));
                                }
                                FilePath = FilePath.Replace(".ack", "");
                                ZipFile.ExtractToDirectory(FilePath + ".zip", subDirectory, true);
                                StreamReader reader;
                                string strJSONdata = "";
                                if (File.Exists(FilePath + ".json"))
                                {
                                    string CorrNameUID = Path.GetFileName(FilePath + ".json").Replace(".json", "");
                                    reader = new StreamReader(FilePath + ".json");
                                    strJSONdata = reader.ReadToEnd();
                                    reader.Dispose();
                                    TarasolAck tarasolAck = JsonConvert.DeserializeObject<TarasolAck>(strJSONdata);

                                    ICALibrary.Models.DeliveryRequest.Body bodyRequest = new ICALibrary.Models.DeliveryRequest.Body()
                                    {
                                        orgId = tarasolAck.LastParentGlobalID,
                                        EntityCode = new ICALibrary.Models.DeliveryRequest.GlobalID { Code = tarasolAck.LastParentGlobalID }, // last parent : SenderParentGlobalID
                                        TransactionId = tarasolAck.ExternalExchangeNo,
                                        ReadBy = ICALibrary.Helper.Common.RemoveSpecialChar(tarasolAck.ReadBy),
                                        transactionRefNo = ICALibrary.Business.Document.GenerateTransactionRefNo(configAttribute.consumerID),
                                        DateOfUpdate = Mapping.ConvertToDateTime(tarasolAck.AckDate).ToString("yyyy-MM-ddTHH:mm:ss"),
                                        timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") // format requiered from side ESB 
                                    };

                                    string XMLstringBody = XMLProcessing.Serialize(bodyRequest);
                                    XMLstringBody = ICALibrary.Helper.Common.ReplaceXMLStr(XMLstringBody);
                                    TransactionRefNo = bodyRequest.transactionRefNo;

                                    // Header Request
                                    String DevelopSecKey = IntegrationSettings.GetSetting("DevelopSecKey").ToString();
                                    ICALibrary.Models.DocumentRequest.Header headerRequest = new ICALibrary.Models.DocumentRequest.Header
                                    {
                                        hash = String.Format("{0}", ICALibrary.Helper.MessageIntegrityEncrypter.SignMessageICA(XMLstringBody, DevelopSecKey)),
                                        password = ICALibrary.Helper.MessageIntegrityEncrypter.EncryptDataICA(Encryption.DecryptString(IntegrationSettings.GetSetting("Password").ToString()), DevelopSecKey), //ICALibrary.Helper.Constant.Password,
                                        userName = ICALibrary.Helper.MessageIntegrityEncrypter.EncryptDataICA(IntegrationSettings.GetSetting("UserName").ToString(), DevelopSecKey), //ICALibrary.Helper.Constant.Username,
                                        serviceLanguage = ICALibrary.Business.SMS.GetLanguageMessage(ICALibrary.Helper.Constant.ARABIC_NUM),
                                        serviceName = ICALibrary.Helper.Constant.serviceNameDeliveryReport,
                                        serviceVersion = ICALibrary.Helper.Constant.serviceVersionSMS,
                                        sourceChannel = ICALibrary.Helper.Constant.sourceChannelSMS
                                    };
                                    string XMLstringHeader = XMLProcessing.Serialize(headerRequest);
                                    // End Header Request


                                    try
                                    {
                                        ICALibrary.Models.IntResult Response = ICALibrary.Business.Document.CallDeliveryReportWebService(XMLstringHeader, XMLstringBody, configAttribute.urlServiceESB);
                                        //IntResult Response = SoapRequest.CallECSDeliveryWebService(XMLstring);
                                        if (Response.Code == (int)ICALibrary.Helper.Enum.responseCode.Success)
                                        {
                                            endDate = DateTime.Now;
                                            msgResponse = $"End - result : {Response.EMessage}, TransactionRefNo : {TransactionRefNo}";
                                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, msgResponse, TrackInterface: true);
                                            //Jobs.AddLogJob("SendAckICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Success, startDate, endDate, (endDate - startDate).TotalSeconds, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                                        }
                                        else
                                        {
                                            endDate = DateTime.Now;
                                            msgResponse = $"End - Details : {Response.Details}, TransactionRefNo : {TransactionRefNo}";
                                            //Jobs.AddLogJobESB("SendAckICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Failed, startDate, endDate, (endDate - startDate).TotalSeconds, objStr, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, msgResponse, TrackInterface: true);

                                            // Make local exception  MUST change not good to add TODO
                                            Exception exception = new Exception(msgResponse);
                                            throw exception;
                                        }

                                        return Response;
                                    }
                                    catch (Exception ex)
                                    {
                                        File.Delete(FilePath + ".json");
                                        if (!File.Exists(FilePathZIP + ".ack"))
                                        {
                                            File.Move(FilePath + ".zip", Path.ChangeExtension(FilePath + ".zip", ".ack"));
                                        }
                                        else
                                        {
                                            if (File.Exists(FilePath + ".zip"))
                                            {
                                                File.Delete((FilePath + ".zip"));
                                            }
                                        }

                                        endDate = DateTime.Now;
                                        msgResponse = "End - result : " + ex.Message;
                                        //Jobs.AddLogJobESB("SendAckICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Failed, startDate, endDate, (endDate - startDate).TotalSeconds, objStr, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                                        throw ex;
                                    }
                                    //Remove Comment
                                    //// after sending succeeds
                                    //File.Delete(FilePath + ".json");
                                    //File.Move(FilePath + ".zip", Path.ChangeExtension(FilePath + ".zip", ".ack"));

                                }
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message);
                throw ex;

            }

        }

        public static object ListGet(string MethodName)
        {
            try
            {
                string Tarasol_SessionID = Cache.GetSession().SessionID;
                string Tarasol_Token = Cache.GetSession().Token;
                object jsonResult = CallExternalAPI.CallGetAPI(Cache.GetFromCache("APIURL") + "/Corr/ListGet" +
                    "?MethodName=" + MethodName + "&SessionID=" + Tarasol_SessionID, Tarasol_Token);
                object jsonResponse = JsonConvert.DeserializeObject<object>(jsonResult.ToString()); //need to serialize to relative object
                return jsonResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static object SendNotificationTarasol(String exchangeNo, String corrUID, int status, int RequestNo, String lMessage, String eMessage)
        {
            try
            {
                string NotificationURL = IntegrationSettings.GetSetting("APIURL").ToString() + "/Corr/corrExportStatus";
                string json = @"{
                    ""docUid"":""" + corrUID + @""",
	                ""status"":" + status + @",
	                ""ExchangeNo"":" + exchangeNo + @",
	                ""RequestNo"":" + RequestNo + @",
	                ""FailReasonEmsg"":""" + eMessage + @""",
                    ""FailReasonLmsg"":""" + lMessage + @""",
                    }";

                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, string.Format("NotificationURL : {0},Statsus: {1}", NotificationURL, status));
                string Tarasol_Token = Cache.GetSession().Token;
                object jsonResponse = CallExternalAPI.CallPostAuthAPI(NotificationURL, json, Tarasol_Token);
                return jsonResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object AcknowledgmentReportLoad()
        {
            try
            {
                string Tarasol_Token = Cache.GetSession().Token;
                string SessionID = Cache.GetSession().SessionID; ;
                if (Tarasol_Token == "")
                {
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Tarasol_Token : Unauthorized user");
                    return null;
                }
                string apiurl = IntegrationSettings.GetSetting("APIURL").ToString() + "/Corr/AcknowledgmentTransmittalReportLoad?CurrentPage=1&MaxNumberItems=15&SessionId=" + SessionID;
                string json = @"{""SearchList"":[],""OrderList"":[]}";

                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "apiurl : " + apiurl);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Tarasol_Token : " + Tarasol_Token.Length);
                CallExternalAPI.CallPostAPI(apiurl, json, Tarasol_Token);
                return null;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                throw ex;
            }
        }

        public static string CallCorrespondecesImport()
        {
            try
            {
                /// add it to email import also
                string URL = IntegrationSettings.GetSetting("APIURL").ToString() + "/Corr/CorrespondencesImport?SessionId=";


                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "calling API: CorrespondencesImport");
                string Tarasol_Token = Cache.GetSession().Token;
                URL += Cache.GetSession().SessionID;
                object jsonResponse = CallExternalAPI.CallGetAPI(URL, Tarasol_Token: Tarasol_Token);

                if (!jsonResponse.ToString().Contains(ICALibrary.Helper.Constant.ResponseSuccessTarasolNotification))
                {
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, jsonResponse.ToString(), TrackInterface: true);
                }
                return jsonResponse.ToString();
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, ex.Message, TrackInterface: true);
                throw ex;
            }
        }
    }
}
