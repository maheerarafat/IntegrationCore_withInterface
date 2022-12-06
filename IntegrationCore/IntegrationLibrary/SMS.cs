using IntegrationCore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.IntegrationLibrary
{
    public class SMS
    {
        public static IntResult SendSMS(List<SMSObject> sMSObjects)
        {
            try
            {

                foreach (var sMSObject in sMSObjects)
                {
                    ECS_SMSRequest eCS_SMSRequest = new ECS_SMSRequest()
                    {
                        MobileNumber = sMSObject.MobileNumber,
                        Message = sMSObject.Message,
                        Language = sMSObject.Language
                    };
                    string XMLstring = XMLProcessing.Serialize(eCS_SMSRequest);
                    SoapRequest.CallSMSWebService(XMLstring);
                }

                return IntResultLib.GetResult(1);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        /// This function to call send SMS in ICA portal
        /// </summary>
        /// <param name="sMSObject">object contains all data from Tarasol  </param>
        /// <returns>string successful</returns>
        public static IntResult SendSMSICA(List<SMSObject> sMSObjects, ConfigAttribute configAttribute, out string transactionRefNo)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            string msgResponse = string.Empty;
            string objStr = JsonConvert.SerializeObject(sMSObjects);
            transactionRefNo = string.Empty;
            //Jobs.AddLogJobESB("SendSMSICA", "Started", (int)ICALibrary.Helper.Enum.responseCode.Success, startDate, endDate, 0, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);
            try
            {

                foreach (var sMSObject in sMSObjects)
                {
                    ICALibrary.Models.SMSRequest.Notification notification = new ICALibrary.Models.SMSRequest.Notification
                    {
                        notificationType = ICALibrary.Helper.Constant.notificationTypeSMS,
                        //subject = ICALibrary.Helper.Constant.subjectSMS,
                        messageBody = ICALibrary.Helper.Common.RemoveSpecialChar(sMSObject.Message),
                        toAddress = Helper.Common.RemoveZeroLeadingPhonenumber(sMSObject.MobileNumber.ToString()),
                        //fromAddress = ICALibrary.Helper.Constant.fromAddresSMS
                    };

                    ICALibrary.Models.SMSRequest.Body bodyRequest = new ICALibrary.Models.SMSRequest.Body
                    {
                        Notification = notification,
                        transactionRefNo = ICALibrary.Business.SMS.GenerateTransactionRefNo(configAttribute.consumerID),
                        timestamp = DateTime.Now
                    };

                    string XMLstringBody = XMLProcessing.Serialize(bodyRequest);
                    XMLstringBody = ICALibrary.Helper.Common.ReplaceXMLStr(XMLstringBody);
                    transactionRefNo = bodyRequest.transactionRefNo;

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
                        serviceLanguage = ICALibrary.Business.SMS.GetLanguageMessage(sMSObject.Language),
                        serviceName = ICALibrary.Helper.Constant.serviceNameSMS,
                        serviceVersion = ICALibrary.Helper.Constant.serviceVersionSMS,
                        sourceChannel = ICALibrary.Helper.Constant.sourceChannelSMS
                    };


                    string XMLstringHeader = XMLProcessing.Serialize(headerRequest);
                    ICALibrary.Models.IntResult Response = ICALibrary.Business.SMS.CallSMSWebService(XMLstringHeader, XMLstringBody, configAttribute.urlServiceNotify);

                    if (Response.Code == (int)ICALibrary.Helper.Enum.responseCode.Success)
                    {
                        endDate = DateTime.Now;
                        msgResponse = $"End - result : {Response.EMessage}, {transactionRefNo}";
                        //Jobs.AddLogJobESB("SendSMSICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Success, startDate, endDate, (endDate - startDate).TotalSeconds, String.Empty, (int)ICALibrary.Helper.Enum.scheduleType.Done);
                    }
                    else
                    {
                        endDate = DateTime.Now;
                        msgResponse = $"End - error : {Response.Details}, {transactionRefNo}";
                        //if (writeLog)
                        //{
                        //    Jobs.AddLogJobESB("SendSMSICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Failed, startDate, endDate, (endDate - startDate).TotalSeconds, objStr, (int)ICALibrary.Helper.Enum.scheduleType.NotStarted);
                        //}
                        return IntResultLib.GetResult(2);
                    }

                }

                return IntResultLib.GetResult(1);
            }
            catch (Exception ex)
            {
                endDate = DateTime.Now;
                msgResponse = $"End - error : {ex.Message}" ;
                //if (writeLog)
                //{
                //    Jobs.AddLogJobESB("SendSMSICA", msgResponse, (int)ICALibrary.Helper.Enum.responseCode.Failed, startDate, endDate, (endDate - startDate).TotalSeconds, objStr, (int)ICALibrary.Helper.Enum.scheduleType.NotStarted);
                //}
                throw ex;
            }
        }

        public static IntResult SendOTP(string mobile, string userNumber, ConfigAttribute configAttribute)
        {
            try
            {
                // Generate TOTP and security-key
                string otp = String.Empty;
                string securityKey =  ICALibrary.Business.Totp.GenerateTOTP(out otp);
                string transactionRefNo = string.Empty;

                // Save the security-key in table the lookup
                SyncHR.UpdateSecurityKeyUser(securityKey, userNumber);

                // Send OTP to mobile number    
                List<SMSObject> sMSObjects = new List<SMSObject>();
                SMSObject sms = new SMSObject();
                sms.MobileNumber = mobile.Substring(1);
                sms.Message = string.Format(ICALibrary.Helper.ConstantMessage.OTPMessage, otp);
                sms.Language = ICALibrary.Helper.Constant.ARABIC_NUM;
                sMSObjects.Add(sms);

                IntResult result = SendSMSICA(sMSObjects, configAttribute, out transactionRefNo);
                //IntResult result = IntResultLib.GetResult(1); // TODO remove 
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static ICALibrary.Models.IntResult checkOTP(string code, string userNumber)
        {
            try
            {

                // get the security-key in table the lookup
                string securityKey = SyncHR.GetUserSecuirtyLookup(userNumber);

                // check TOTP and security-key
                string otp = code;
                ICALibrary.Models.IntResult result = ICALibrary.Business.Totp.CompareTOTP(otp, securityKey);
              
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
