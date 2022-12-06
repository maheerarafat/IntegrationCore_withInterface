using IntegrationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;

namespace IntegrationCore.IntegrationLibrary
{
    public class Mail
    {
        private static string _pathSendEmail = @"\Templates\SendEmail.html";
        static string subjectTag = "#1#";
        static string refernecTag = "#2#";
        static string dateTag = "#3#";

        public static IntResult SendEmail(Models.Mail mail)
        {
            try
            {
                List<SoapEMialAttachment> emailAttachments = new List<SoapEMialAttachment>();
                foreach (EmailAttachment item in mail.Attachments)
                {
                    emailAttachments.Add(new SoapEMialAttachment()
                    {
                        Body = item.FileContent,
                        ContentType = "application/octet-stream",
                        FileName = item.FileName + "." + item.FileType.Replace(".", "")
                    });
                }
                List<SoapEmail> _Emailcc = new List<SoapEmail>();
                List<SoapEmail> _EmailTo = new List<SoapEmail>();
                foreach (var email in mail.Receivers)
                {
                    if (email.ReceiverType == Models.ReceiverType.to)
                        _EmailTo.Add(new SoapEmail() { email = email.ReceiverEmail });
                    else
                        _Emailcc.Add(new SoapEmail() { email = email.ReceiverEmail });
                };
                SoapEmailObject emailObject = new SoapEmailObject()
                {
                    Attachments = emailAttachments,
                    EmailCC = _Emailcc,
                    EmailTo = _EmailTo,
                    EmailSubject = mail.Subject,
                    EmailContent = mail.Body
                };
                string XMLstring = XMLProcessing.Serialize(emailObject);
                return SoapRequest.CallEmailWebService(XMLstring);
            }
            catch (Exception ex)
            {

                Exception exception = new Exception("Unable to send Email");
                throw exception;
            }
        }

        /// <summary>
        /// This function to call send SMS in ICA portal
        /// </summary>
        /// <param name="mail">object contains all data from Tarasol  </param>
        /// <returns>string successful</returns>
        public static IntResult SendEmailICA(Models.Mail mail, ConfigAttribute configAttribute)
        {
            try
            {

                ICALibrary.Models.EmailObject.EmailAttachment attachments = new ICALibrary.Models.EmailObject.EmailAttachment();
                if (mail.Attachments != null)
                {
                    // this solution of naming attachment temporary because we have problem with name arabic in exchange server
                    foreach (EmailAttachment item in mail.Attachments)
                    {
                        ICALibrary.Models.EmailObject.AttachmentItem elem = new ICALibrary.Models.EmailObject.AttachmentItem();
                        elem.name = ICALibrary.Helper.Common.RemoveSpecialChar(string.Format("attachment{0}.", attachments.attachmentItem.Count() + 1) + item.FileType.Replace(".", "")); //item.FileName; //+ "." + item.FileType.Replace(".", ""); //item.attachmentItem.name; //TODO temporary after fix ESB issue name arabic
                        elem.contentType = item.FileType; // "application/octet-stream"; //item.attachmentItem.contentType;
                        elem.data = item.FileContent; //ICALibrary.Helper.Common.Base64Encode(item.attachmentItem.data);
                        attachments.attachmentItem.Add(elem);
                    }
                }


                List<string> _Emailcc = new List<string>();
                List<string> _EmailTo = new List<string>();
                foreach (var email in mail.Receivers)
                {
                    if (email.ReceiverType == Models.ReceiverType.to)
                        _EmailTo.Add(email.ReceiverEmail);
                    else
                        _Emailcc.Add(email.ReceiverEmail);
                };

                ICALibrary.Models.EmailObject.Notification notification = new ICALibrary.Models.EmailObject.Notification
                {
                    notificationType = ICALibrary.Helper.Constant.notificationTypeEmail,
                    toAddress = String.Join(";", _EmailTo.ToArray()),// email.Notification.toAddress,
                    subject = mail.Subject,
                    fromAddress = configAttribute.senderMail,
                    messageBody = ICALibrary.Helper.Common.Base64Encode(mail.Body), // must be base64 only for tarasol channel
                    // importance = string.Empty, // email.Notification.importance,
                    // ccAddress = String.Join(";", _Emailcc.ToArray()),
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
                    //endDate = DateTime.Now;
                    //msgResponse = "End - result : " + Response.EMessage;
                    //Jobs.AddLogJobESB("SendSMSICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Success, startDate, endDate, (endDate - startDate).TotalSeconds, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                }
                else
                {
                    //endDate = DateTime.Now;
                    //msgResponse = "End - error : " + Response.Details;
                    return IntResultLib.GetResult(2);
                }


                return IntResultLib.GetResult(1);
            }
            catch (Exception ex)
            {
                //endDate = DateTime.Now;
                //msgResponse = "End - error : " + ex.Message;
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tarasolDocument"></param>
        /// <returns></returns>
        public static IntResult ImportEmailToFolder(ICALibrary.Models.Email email, string EmailDistributorStr)
        {
            try
            {
                if (email != null)
                {
                    List<ICALibrary.Models.EmailDistributor> EmailDistributors = new List<ICALibrary.Models.EmailDistributor>();
                    string nameAttachment = "body_email.HTML";
                    string filePath = string.Empty;

                    //Get import folder path
                    string GUID = Guid.NewGuid().ToString();
                    string path = (string)IntegrationSettings.GetSetting("IntegrationDataFolder") + "/In/Draft/" // here change to email folder
                        + GUID;

                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                    path += "/" + GUID;
                    DirectoryInfo di = Directory.CreateDirectory(path);
                    List<ICALibrary.Models.AttachmentEmail> attachments = email?.AttachmentEmails;

                    EmailDistributors = JsonConvert.DeserializeObject<List<ICALibrary.Models.EmailDistributor>>(EmailDistributorStr);
                    email.EmailDistributors = EmailDistributors;


                    if (email?.AttachmentEmails != null)
                    {
                        foreach (var data in attachments)
                        {
                            var base64string = data.FileContent;
                            var base64array = Convert.FromBase64String(base64string);
                            filePath = path + "/" + data.FileName + "." + data.FileExtension;
                            System.IO.File.WriteAllBytes(filePath, base64array);
                        }
                    }

                    // write body of email in attachment corr
                    byte[] bytesBodyEmail = Encoding.UTF8.GetBytes(email?.Body);
                    filePath = path + "/" + nameAttachment;
                    System.IO.File.WriteAllBytes(filePath, bytesBodyEmail);


                    DocumentObject documentObject = Mapping.MapImportedEmail(email);
                    string jsondata = JsonConvert.SerializeObject(documentObject);

                    // Write that JSON to  file,  
                    System.IO.File.WriteAllText(path + "/" + GUID + ".json", jsondata, encoding: Encoding.UTF8);
                    // Create zip file for correspondence
                    ZipFile.CreateFromDirectory(path, path + ".zip");
                    File.Move(path + ".zip", Path.ChangeExtension(path + ".zip", ".tarasol"));
                    Directory.Delete(path, true);

                    return IntResultLib.GetResult(200); // if operation succeeded
                }
                else
                {
                    return IntResultLib.GetResult(5);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetBodyEmail(string basicPath, string subject, string refNumber)
        {
            // string locationTemplateEmail = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _pathSendEmail);
            string locationTemplateEmail = basicPath + _pathSendEmail;
            string bodyContent = System.IO.File.ReadAllText(locationTemplateEmail);
            bodyContent = bodyContent.Replace(subjectTag, subject);
            bodyContent = bodyContent.Replace(refernecTag, refNumber);
            bodyContent = bodyContent.Replace(dateTag, DateTime.Now.ToString("yyyy-MM-dd"));


            return ICALibrary.Helper.Common.Base64Encode(bodyContent);
        }

    }
}
