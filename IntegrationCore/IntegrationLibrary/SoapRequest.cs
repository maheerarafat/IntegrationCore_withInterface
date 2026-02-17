using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.IO;
using IntegrationCore.IntegrationLibrary;
using System.IdentityModel.Tokens.Jwt;
using System.Xml.Linq;
using System.Linq;

namespace IntegrationCore.Models
{
    public class SoapRequest
    {

        public static string HREnvelopeBegining = @"<soapenv:Envelope xmlns:hrm=""http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">";

        private static HttpWebRequest CreateWebRequest(string url, string Method = "Post", string ContentType = "text/xml;charset=\"utf-8\"", string Accept = "text/xml")
        {
            try
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Soap - CreateWebRequest url :" + url, TrackInterface: true);
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; }; 
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Timeout = 300000;
                httpWebRequest.ContentType = ContentType;
                httpWebRequest.Accept = Accept;
                httpWebRequest.Method = Method;
                return httpWebRequest;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static void InsertEnvelopeIntoWebRequest(XmlDocument soapEnvelpoeXML, HttpWebRequest httpWebRequest)
        {
            try
            {
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    soapEnvelpoeXML.Save(stream);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        ///////////////// HR 
        public static string HREnvelopeHeader = @"<soapenv:Header>
                          <wsse:Security soapenv:mustUnderstand=""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
                             <wsse:UsernameToken>
                                <wsse:Username>stshr-testuser</wsse:Username>
                                <wsse:Password>P@ssw0rd</wsse:Password>
                             </wsse:UsernameToken>
                          </wsse:Security>
                       </soapenv:Header>";
        public static object CallHRWebService(String Request, List<string[]> Parameters = null)
        {
            try
            {
                string _HRurl = IntegrationSettings.GetSetting("ESBURL").ToString() + "/HRMurasalat";
                XmlDocument xmlDocument = CreateHRSoapEnvelope(Request, Parameters);
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; }; 
                HttpWebRequest httpWebRequest = CreateWebRequest(_HRurl);
                httpWebRequest.Timeout = 300000;
                InsertEnvelopeIntoWebRequest(xmlDocument, httpWebRequest);
                IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();
                string result;
                using (WebResponse webResponse = httpWebRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        result = RemoveAllNamespaces(rd.ReadToEnd());

                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(result);
                        string ResponseNode = xml.GetElementsByTagName("Body")[0].InnerXml;
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, " response string :" + ResponseNode, TrackInterface: true);
                        return ResponseNode;
                        //Vacations vacation111 = Serializer.Desrializer<Vacations>(ResponseNode);
                        //string JsonResult = JsonConvert.SerializeXmlNode(xml);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        private static XmlDocument CreateHRSoapEnvelope(String Request, List<string[]> Parameters = null)
        {
            try
            {
                string ESBHRPass = Encryption.DecryptString(IntegrationSettings.GetSetting("ESBHRPass").ToString());
                ////bool EnableLoggingInfo = (bool)IntegrationSettings.GetSetting("EnableLoggingInfo");
                XmlDocument xmlDocument = new XmlDocument();
                string ParameterString = "";
                if (Parameters != null)
                {
                    foreach (string[] item in Parameters)
                    {
                        ParameterString += @"<hrm:" + item[0] + @">" + item[1] + @"</hrm:" + item[0] + @">";
                    }
                }
                string str = @"<soapenv:Envelope xmlns:hrm=""http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""><soapenv:Header><wsse:Security soapenv:mustUnderstand=""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""><wsse:UsernameToken wsu:Id =""UsernameToken-118F53D6777338BA5C15713123557251""><wsse:Username>" +
                   IntegrationSettings.GetSetting("ESBHRUser").ToString() + @"</wsse:Username><wsse:Password Type =""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">" +
                   ESBHRPass + @"</wsse:Password><wsse:Nonce EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"">rWYoctMstXil8ckAYlvARw==</wsse:Nonce><wsu:Created>2019-10-17T11:39:15.723Z</wsu:Created></wsse:UsernameToken></wsse:Security></soapenv:Header><soapenv:Body><hrm:" + Request + @">" + ParameterString + "</hrm:" + Request + @"></soapenv:Body></soapenv:Envelope>";
                xmlDocument.LoadXml(str);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, " request string :" + str, TrackInterface: Logger.EnableLoggingInfo);
                return xmlDocument;
            }
            catch (Exception)
            {

                throw;
            }

        }


        ///////////////// ECS 
        public static IntResult CallECSAddWebService(String Request)
        {
            try
            {
                string _HRurl = IntegrationSettings.GetSetting("ESBURL").ToString() + "/MOICorrespondenceService";
                XmlDocument xmlDocument = CreateECSSoapEnvelope(Request);
                ///ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                HttpWebRequest httpWebRequest = CreateWebRequest(_HRurl);
                httpWebRequest.Timeout = 300000;
                httpWebRequest.ReadWriteTimeout = 300000; // for large attachments
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Soap - InsertEnvelopeIntoWebRequest :", TrackInterface: true);
                InsertEnvelopeIntoWebRequest(xmlDocument, httpWebRequest);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Soap - end InsertEnvelopeIntoWebRequest :", TrackInterface: true);
                IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(null, null);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Soap - asyncResult finish :", TrackInterface: true);
                asyncResult.AsyncWaitHandle.WaitOne();
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Soap - asyncResult wait finish :" + asyncResult.IsCompleted.ToString(), TrackInterface: true);
                string result;
                using (WebResponse webResponse = httpWebRequest.EndGetResponse(asyncResult))
                {
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Soap - reading Response :", TrackInterface: true);
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Soap - start reading Response :", TrackInterface: true);
                        result = rd.ReadToEnd();
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Soap - Response : " + result, TrackInterface: true);
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(result);
                        string ResponseNode = xml.GetElementsByTagName("tns:AddNewDocumentResponse")[0].InnerXml.Replace("xmlns:tns=\"http://www.moi.gov.ae/2018/MoiIntegration/MOICorrespondenceServiceSchema\"", "").Replace("tns:", "");
                        Ecs_DocumentAddResponse Objectresult = XMLProcessing.Deserializer<Ecs_DocumentAddResponse>(ResponseNode);
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Objectresult.RequestStatus :" + Objectresult.RequestStatus, TrackInterface: true);
                        if (Objectresult.RequestStatus == 1 || Objectresult.StatusMessage.ToLower() == "Success".ToLower())
                        {
                            return IntResultLib.GetResult(1);
                        }
                        else if (Objectresult.RequestStatus == 9998 || Objectresult.RequestStatus == 9999 || Objectresult.RequestStatus == 9997
                                || Objectresult.RequestStatus == 1015 || Objectresult.RequestStatus == 1014 || Objectresult.RequestStatus == 1013
                                || Objectresult.RequestStatus == 1012 || Objectresult.RequestStatus == 1011 || Objectresult.RequestStatus == 1010
                                || Objectresult.RequestStatus == 1009 || Objectresult.RequestStatus == 1008 || Objectresult.RequestStatus == 1004
                                || Objectresult.RequestStatus == 1003 || Objectresult.RequestStatus == 1002 || Objectresult.RequestStatus == 1001)
                        {
                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                            return IntResultLib.GetResult(2);
                        }
                        else if (Objectresult.RequestStatus == 1007 || Objectresult.RequestStatus == 1006 || Objectresult.RequestStatus == 1005 || Objectresult.RequestStatus == 2
                            || Objectresult.RequestStatus == 3)
                        {
                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                            return IntResultLib.GetResult(0);
                        }
                        else if (Objectresult.StatusMessage.ToLower().Contains("object ref") || Objectresult.StatusMessage.ToLower().Contains("communication request is nul") || Objectresult.StatusMessage.ToLower().Contains("one or more errors"))
                        {
                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                            return IntResultLib.GetResult(2);
                        }
                        else
                        {
                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                            return IntResultLib.GetResult(0);
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "WebException occurred: " + webEx.Message, TrackInterface: true, PrintInterface: true);

                // exception type
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Status: " + webEx.Status.ToString(), TrackInterface: true, PrintInterface: true);

                // try to get more details from the response if available
                if (webEx.Response != null)
                {
                    using (Stream responseStream = webEx.Response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                string errorResponse = reader.ReadToEnd();
                                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Server Response: " + errorResponse, TrackInterface: true, PrintInterface: true);
                            }
                        }
                    }

                    // print HTTP status code if available
                    if (webEx.Response is HttpWebResponse httpResponse)
                    {
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, $"HTTP Status Code: {(int)httpResponse.StatusCode} ({httpResponse.StatusCode})", TrackInterface: true, PrintInterface: true);

                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Status Description: " + httpResponse.StatusDescription, TrackInterface: true, PrintInterface: true);
                    }
                }
                else
                {
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Response is null - No response received from server", TrackInterface: true, PrintInterface: true);
                }
                throw;
            }
            catch (Exception ex)
            {
                // print general exception details
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "General error occurred: " + ex.Message, TrackInterface: true, PrintInterface: true);

                throw;
            }


        }
        public static IntResult CallECSDeliveryWebService(String Request)
        {
            try
            {
                string _HRurl = IntegrationSettings.GetSetting("ESBURL").ToString() + "/MOICorrespondenceService";
                ////bool EnableLoggingInfo = (bool)IntegrationSettings.GetSetting("EnableLoggingInfo");
                XmlDocument xmlDocument = CreateECSSoapEnvelope(Request);
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; }; 
                HttpWebRequest httpWebRequest = CreateWebRequest(_HRurl);
                httpWebRequest.Timeout = 300000;
                InsertEnvelopeIntoWebRequest(xmlDocument, httpWebRequest);
                IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();
                string result;
                using (WebResponse webResponse = httpWebRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        result = rd.ReadToEnd();
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "result  : " + result, TrackInterface: Logger.EnableLoggingInfo);
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(result);
                        string ResponseNode = xml.GetElementsByTagName("tns:DeliveryReportResponse")[0].InnerXml.Replace("xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:tns=\"http://www.moi.gov.ae/2018/MoiIntegration/MOICorrespondenceServiceSchema\"", "").Replace("tns:", "");
                        if (ResponseNode.ToLower().Contains("ResponseMessage>Success<".ToLower()) || ResponseNode.ToLower().Contains("ResponseMessage>1<".ToLower()) || ResponseNode.ToLower().Contains("ResponseMessage>Succeded<".ToLower()) || ResponseNode.ToLower().Contains("ResponseMessage>Data updated successfully<".ToLower()))
                        {
                            return IntResultLib.GetResult(1);
                        }
                        else
                        {
                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                            return IntResultLib.GetResult(2);
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        private static XmlDocument CreateECSSoapEnvelope(String Request)
        {
            try
            {
                //Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Request : " + Request + " /Request");
                string ESBECSPass = Encryption.DecryptString(IntegrationSettings.GetSetting("ESBECSPass").ToString());
                //Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "ESBECSPass : " + ESBECSPass + " /ESBECSPass");
                XmlDocument xmlDocument = new XmlDocument();
                string envelope = @"<soapenv:Envelope xmlns:moic=""http://www.moi.gov.ae/2018/MoiIntegration/MOICorrespondenceServiceSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""><soapenv:Header><wsse:Security soapenv:mustUnderstand = ""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu =""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""><wsse:UsernameToken wsu:Id =""UsernameToken-4CA7F3FE3551917FC815764787625025""><wsse:Username>"
                                + IntegrationSettings.GetSetting("ESBECSUser").ToString() + @"</wsse:Username><wsse:Password Type =""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">"
                                + ESBECSPass + @"</wsse:Password><wsse:Nonce EncodingType = ""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"">eFACcvctRERFHgmsuxGrGw==</wsse:Nonce><wsu:Created>2019-12-16T06:46:02.502Z</wsu:Created></wsse:UsernameToken></wsse:Security></soapenv:Header><soapenv:Body>" + Request + @" </soapenv:Body></soapenv:Envelope>";
                envelope = envelope.Replace("moic_x003A_", "moic:").Replace("ECSAttachment>", "moic:Attachment>");
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End - Logenvelope : " + envelope + " /Logenvelope", TrackInterface: Logger.EnableLoggingInfo, PrintInterface: Logger.EnableLoggingInfo);
                xmlDocument.LoadXml(envelope);
                return xmlDocument;
            }
            catch (Exception)
            {

                throw;
            }

        }


        ///////SMS
        public static object CallSMSWebService(String Request)
        {
            try
            {
                string _HRurl = IntegrationSettings.GetSetting("ESBURL").ToString() + "/SendPushNotification";
                XmlDocument xmlDocument = CreateSMSSoapEnvelope(Request);
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; }; 
                HttpWebRequest httpWebRequest = CreateWebRequest(_HRurl);
                httpWebRequest.Timeout = 300000;
                InsertEnvelopeIntoWebRequest(xmlDocument, httpWebRequest);
                IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();
                string result;
                using (WebResponse webResponse = httpWebRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        result = rd.ReadToEnd();
                        //XmlDocument xml = new XmlDocument();
                        //xml.LoadXml(result);
                        //string ResponseNode = xml.GetElementsByTagName("S:Body")[0].InnerXml.Replace("tns:", "").Replace(@"xmlns:ns1=""http://adpolice.gov.ae/"" xmlns:tns=""http://moi.gov.ae/2019/MoiIntegration/SendPushNotificationSchema""", "");
                        string ResponseNode = result.Replace("tns:", "").Replace(@"xmlns:ns1=""http://adpolice.gov.ae/"" xmlns:tns=""http://moi.gov.ae/2019/MoiIntegration/SendPushNotificationSchema""", "");
                        if (ResponseNode.IndexOf("<SMSSerailNumber>1</SMSSerailNumber>") != -1)
                        {
                            return IntResultLib.GetResult(1);
                        }
                        else
                        {
                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                            return IntResultLib.GetResult(2);
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        private static XmlDocument CreateSMSSoapEnvelope(String Request)
        {
            try
            {
                AM_EncryptCore.clsEncrypt clsEncrypt = new AM_EncryptCore.clsEncrypt();

                string ESBSMSPass = Encryption.DecryptString(IntegrationSettings.GetSetting("ESBSMSPass").ToString());
                string MOISMSPass = Encryption.DecryptString(IntegrationSettings.GetSetting("MOISMSPass").ToString());
                XmlDocument xmlDocument = new XmlDocument();
                string envelope = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" "
                                + @"xmlns:sen=""http://moi.gov.ae/2019/MoiIntegration/SendPushNotificationSchema""><soapenv:Header><sen:MOISecurityHeader><sen:username>"
                                + IntegrationSettings.GetSetting("MOISMSUser").ToString() + @"</sen:username><sen:password>"
                                + MOISMSPass + @"</sen:password><sen:channel>"
                                + IntegrationSettings.GetSetting("MOISMSChannel").ToString() + @"</sen:channel></sen:MOISecurityHeader>"
                                + @"<wsse:Security soapenv:mustUnderstand = ""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu =""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""><wsse:UsernameToken wsu:Id =""UsernameToken-4CA7F3FE3551917FC815764787625025""><wsse:Username>"
                                + IntegrationSettings.GetSetting("ESBSMSUser").ToString()
                                + @"</wsse:Username><wsse:Password Type =""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">"
                                + ESBSMSPass + @"</wsse:Password><wsse:Nonce EncodingType = ""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"">eFACcvctRERFHgmsuxGrGw==</wsse:Nonce><wsu:Created>2019-12-16T06:46:02.502Z</wsu:Created></wsse:UsernameToken></wsse:Security></soapenv:Header><soapenv:Body>"
                 + Request + @" </soapenv:Body></soapenv:Envelope>";
                envelope = envelope.Replace("sen_x003A_", "sen:").Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "");
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End - Logenvelope : " + envelope + " /Logenvelope", TrackInterface: Logger.EnableLoggingInfo);
                xmlDocument.LoadXml(envelope);
                return xmlDocument;
            }
            catch (Exception)
            {

                throw;
            }

        }


        ////////////////Email Service
        public static object CallEmailWebService(String Request)
        {
            try
            {
                string _HRurl = IntegrationSettings.GetSetting("ESBEmail").ToString();
                XmlDocument xmlDocument = CreateEmailSoapEnvelope(Request);
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; }; 
                HttpWebRequest httpWebRequest = CreateWebRequest(_HRurl);
                httpWebRequest.Timeout = 300000;
                InsertEnvelopeIntoWebRequest(xmlDocument, httpWebRequest);
                IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();
                string result;
                using (WebResponse webResponse = httpWebRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        result = rd.ReadToEnd();
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: Logger.EnableLoggingInfo, PrintInterface: Logger.EnableLoggingInfo);
                        string ResponseNode = result.Replace("ema:", "").Replace(@"xmlns:ns1=""http://adpolice.gov.ae/"" xmlns:tns=""http://moi.gov.ae/2019/MoiIntegration/SendPushNotificationSchema""", "");
                        if (ResponseNode.IndexOf("<Status>1</Status>") != -1)
                        {
                            return IntResultLib.GetResult(1);
                        }
                        else
                        {
                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                            return IntResultLib.GetResult(2);
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        private static XmlDocument CreateEmailSoapEnvelope(String Request)
        {
            try
            {
                string ESBECSPass = Encryption.DecryptString(IntegrationSettings.GetSetting("ESBEmailPassword").ToString());
                XmlDocument xmlDocument = new XmlDocument();
                string envelope = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" "
                                    + @" xmlns:ema=""http://www.moi.gov.ae/2020/MoiIntegration/EmailNotificationSchema""><soapenv:Header><wsse:Security soapenv:mustUnderstand=""1"""
                                    + @" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"""
                                    + @" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">"
                                    + @"<wsse:UsernameToken wsu:Id=""UsernameToken-4CA7F3FE3551917FC815764787625025""><wsse:Username>"
                                + IntegrationSettings.GetSetting("ESBEmailUser").ToString()
                                + @"</wsse:Username><wsse:Password  Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">"
                + ESBECSPass + @"</wsse:Password><wsse:Nonce EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"""
                                    + @">eFACcvctRERFHgmsuxGrGw==</wsse:Nonce><wsu:Created>2019-12-16T06:46:02.502Z</wsu:Created></wsse:UsernameToken></wsse:Security></soapenv:Header><soapenv:Body>"
                + Request + @"</soapenv:Body></soapenv:Envelope>";

                //string ESBECSPass = Encryption.DecryptString(IntegrationSettings.GetSetting("ESBECSPass").ToString());
                //XmlDocument xmlDocument = new XmlDocument();
                //string envelope = @"<soapenv:Envelope xmlns:moic=""http://www.moi.gov.ae/2018/MoiIntegration/MOICorrespondenceServiceSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""><soapenv:Header><wsse:Security soapenv:mustUnderstand = ""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu =""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""><wsse:UsernameToken wsu:Id =""UsernameToken-4CA7F3FE3551917FC815764787625025""><wsse:Username>"
                //                + IntegrationSettings.GetSetting("ESBECSUser").ToString() + @"</wsse:Username><wsse:Password Type =""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">"
                //                + ESBECSPass + @"</wsse:Password><wsse:Nonce EncodingType = ""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"">eFACcvctRERFHgmsuxGrGw==</wsse:Nonce><wsu:Created>2019-12-16T06:46:02.502Z</wsu:Created></wsse:UsernameToken></wsse:Security></soapenv:Header><soapenv:Body>" + Request + @" </soapenv:Body></soapenv:Envelope>";

                envelope = envelope.Replace("ema_x003A_", "ema:").Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "").Replace("SoapEMialAttachment>", "ema:Attachment>");
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "End - Logenvelope : " + envelope + " /Logenvelope", TrackInterface: Logger.EnableLoggingInfo, PrintInterface: Logger.EnableLoggingInfo);
                xmlDocument.LoadXml(envelope);
                return xmlDocument;
            }
            catch (Exception)
            {
                throw;
            }

        }


        //Implemented based on interface, not part of algorithm
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }
        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
    }




}
