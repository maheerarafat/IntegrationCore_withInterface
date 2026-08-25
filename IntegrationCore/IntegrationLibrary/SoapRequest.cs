using IntegrationCore.IntegrationLibrary;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace IntegrationCore.Models
{
    public class SoapRequest
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(5)
        };

        private static async Task<string> PostSoapAsync(
         string url,
         string soapXml,
         string contentType = "text/xml;charset=\"utf-8\"",
         string accept = "text/xml",
         CancellationToken ct = default)
        {
            try
            {
                Logger.AddToLogFile(
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    Logger.Enum_LogType._Info,
                    "Soap - PostSoapAsync url :" + url,
                    TrackInterface: true);

                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(soapXml, Encoding.UTF8);

                // Keep exact content-type used by legacy code
                request.Content.Headers.Clear();
                request.Content.Headers.TryAddWithoutValidation("Content-Type", contentType);

                request.Headers.Accept.Clear();
                request.Headers.TryAddWithoutValidation("Accept", accept);

                using HttpResponseMessage response = await _httpClient.SendAsync(
                    request,
                    HttpCompletionOption.ResponseContentRead,
                    ct).ConfigureAwait(false);

                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    Logger.AddToLogFile(
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        Logger.Enum_LogType._Error,
                        $"HTTP Status Code: {(int)response.StatusCode} ({response.StatusCode})",
                        TrackInterface: true,
                        PrintInterface: true);

                    Logger.AddToLogFile(
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        Logger.Enum_LogType._Error,
                        "Server Response: " + responseBody,
                        TrackInterface: true,
                        PrintInterface: true);

                    response.EnsureSuccessStatusCode();
                }

                return responseBody;
            }
            catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
            {
                Logger.AddToLogFile(
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    Logger.Enum_LogType._Error,
                    "Request timed out: " + ex.Message,
                    TrackInterface: true,
                    PrintInterface: true);
                throw;
            }
        }

        ///////////////// HR 
        public static object CallHRWebService(String Request, List<string[]> Parameters = null)
        {
            try
            {
                string _HRurl = IntegrationSettings.GetSetting("ESBURL").ToString() + "/HRMurasalat";
                XmlDocument xmlDocument = CreateHRSoapEnvelope(Request, Parameters);

                string result = PostSoapAsync(_HRurl, xmlDocument.OuterXml).GetAwaiter().GetResult();
                result = RemoveAllNamespaces(result);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);
                string ResponseNode = xml.GetElementsByTagName("Body")[0].InnerXml;

                Logger.AddToLogFile(
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    Logger.Enum_LogType._Info,
                    " response string :" + ResponseNode,
                    TrackInterface: true);

                return ResponseNode;
            }
            catch (HttpRequestException httpEx)
            {
                Logger.AddToLogFile(
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    Logger.Enum_LogType._Error,
                    "HttpRequestException occurred: " + httpEx.Message,
                    TrackInterface: true,
                    PrintInterface: true);
                throw;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    Logger.Enum_LogType._Error,
                    "General error occurred: " + ex.Message,
                    TrackInterface: true,
                    PrintInterface: true);
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

                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Soap - sending request :", TrackInterface: true);
                string result = PostSoapAsync(_HRurl, xmlDocument.OuterXml).GetAwaiter().GetResult();
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Soap - Response : " + result, TrackInterface: true);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);

                string ResponseNode = xml.GetElementsByTagName("tns:AddNewDocumentResponse")[0]
                    .InnerXml
                    .Replace("xmlns:tns=\"http://www.moi.gov.ae/2018/MoiIntegration/MOICorrespondenceServiceSchema\"", "")
                    .Replace("tns:", "");

                Ecs_DocumentAddResponse Objectresult = XMLProcessing.Deserializer<Ecs_DocumentAddResponse>(ResponseNode);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Objectresult.RequestStatus :" + Objectresult.RequestStatus, TrackInterface: true);

                if (Objectresult.RequestStatus == 1 || Objectresult.StatusMessage.ToLower() == "success")
                    return IntResultLib.GetResult(1);

                if (Objectresult.RequestStatus == 9998 || Objectresult.RequestStatus == 9999 || Objectresult.RequestStatus == 9997
                    || Objectresult.RequestStatus == 1015 || Objectresult.RequestStatus == 1014 || Objectresult.RequestStatus == 1013
                    || Objectresult.RequestStatus == 1012 || Objectresult.RequestStatus == 1011 || Objectresult.RequestStatus == 1010
                    || Objectresult.RequestStatus == 1009 || Objectresult.RequestStatus == 1008 || Objectresult.RequestStatus == 1004
                    || Objectresult.RequestStatus == 1003 || Objectresult.RequestStatus == 1002 || Objectresult.RequestStatus == 1001)
                {
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                    return IntResultLib.GetResult(2);
                }

                if (Objectresult.RequestStatus == 1007 || Objectresult.RequestStatus == 1006 || Objectresult.RequestStatus == 1005 || Objectresult.RequestStatus == 2
                    || Objectresult.RequestStatus == 3)
                {
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                    return IntResultLib.GetResult(0);
                }

                if (Objectresult.StatusMessage.ToLower().Contains("object ref")
                    || Objectresult.StatusMessage.ToLower().Contains("communication request is nul")
                    || Objectresult.StatusMessage.ToLower().Contains("one or more errors"))
                {
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                    return IntResultLib.GetResult(2);
                }

                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                return IntResultLib.GetResult(0);
            }
            catch (HttpRequestException httpEx)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "HttpRequestException occurred: " + httpEx.Message, TrackInterface: true, PrintInterface: true);
                throw;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "General error occurred: " + ex.Message, TrackInterface: true, PrintInterface: true);
                throw;
            }
        }

        public static IntResult CallECSDeliveryWebService(String Request)
        {
            try
            {
                string _HRurl = IntegrationSettings.GetSetting("ESBURL").ToString() + "/MOICorrespondenceService";
                XmlDocument xmlDocument = CreateECSSoapEnvelope(Request);

                string result = PostSoapAsync(_HRurl, xmlDocument.OuterXml).GetAwaiter().GetResult();
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "result  : " + result, TrackInterface: Logger.EnableLoggingInfo);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);

                string ResponseNode = xml.GetElementsByTagName("tns:DeliveryReportResponse")[0]
                    .InnerXml
                    .Replace("xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:tns=\"http://www.moi.gov.ae/2018/MoiIntegration/MOICorrespondenceServiceSchema\"", "")
                    .Replace("tns:", "");

                if (ResponseNode.ToLower().Contains("ResponseMessage>Success<".ToLower())
                    || ResponseNode.ToLower().Contains("FollowRespponseStatus>1<".ToLower())
                    || ResponseNode.ToLower().Contains("ResponseMessage>Succeded<".ToLower())
                    || ResponseNode.ToLower().Contains("ResponseMessage>Data updated successfully<".ToLower()))
                {
                    return IntResultLib.GetResult(1);
                }

                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                return IntResultLib.GetResult(2);
            }
            catch (HttpRequestException httpEx)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "HttpRequestException occurred: " + httpEx.Message, TrackInterface: true, PrintInterface: true);
                throw;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "General error occurred: " + ex.Message, TrackInterface: true, PrintInterface: true);
                throw;
            }
        }

        private static XmlDocument CreateECSSoapEnvelope(String Request)
        {
            try
            {
                string ESBECSPass = Encryption.DecryptString(IntegrationSettings.GetSetting("ESBECSPass").ToString());
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

                string result = PostSoapAsync(_HRurl, xmlDocument.OuterXml).GetAwaiter().GetResult();

                string ResponseNode = result
                    .Replace("tns:", "")
                    .Replace(@"xmlns:ns1=""http://adpolice.gov.ae/"" xmlns:tns=""http://moi.gov.ae/2019/MoiIntegration/SendPushNotificationSchema""", "");

                if (ResponseNode.IndexOf("<SMSSerailNumber>1</SMSSerailNumber>") != -1)
                    return IntResultLib.GetResult(1);

                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                return IntResultLib.GetResult(2);
            }
            catch (HttpRequestException httpEx)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "HttpRequestException occurred: " + httpEx.Message, TrackInterface: true, PrintInterface: true);
                throw;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "General error occurred: " + ex.Message, TrackInterface: true, PrintInterface: true);
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

                string result = PostSoapAsync(_HRurl, xmlDocument.OuterXml).GetAwaiter().GetResult();

                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: Logger.EnableLoggingInfo, PrintInterface: Logger.EnableLoggingInfo);

                string ResponseNode = result.Replace("ema:", "").Replace(@"xmlns:ns1=""http://adpolice.gov.ae/"" xmlns:tns=""http://moi.gov.ae/2019/MoiIntegration/SendPushNotificationSchema""", "");
                if (ResponseNode.IndexOf("<Status>1</Status>") != -1)
                    return IntResultLib.GetResult(1);

                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error - Response : " + result, TrackInterface: true, PrintInterface: true);
                return IntResultLib.GetResult(2);
            }
            catch (HttpRequestException httpEx)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "HttpRequestException occurred: " + httpEx.Message, TrackInterface: true, PrintInterface: true);
                throw;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "General error occurred: " + ex.Message, TrackInterface: true, PrintInterface: true);
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
