using ICALibrary.Helper;
using ICALibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Net;

namespace ICALibrary.Business
{
    public class Email : SoapRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumerID">static ID according to application number</param>
        /// <returns></returns>
        public static string GenerateTransactionRefNo(string consumerID)
        {
            return string.Format("{0}{1}{2}", consumerID, Helper.Common.RandomNumberStr(10), DateTime.Now.ToString("ddMMyyHHMMss"));
        }


        /// <summary>
        /// SMS Email
        /// </summary>
        /// <param name="requestHeader"></param>
        /// <param name="requestBody"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IntResult CallEmailWebService(String requestHeader, String requestBody, String url)
        {
            try
            {
                string _HRurl = url;
                XmlDocument xmlDocument = CreateSMSSoapEnvelope(requestHeader, requestBody);
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                HttpWebRequest httpWebRequest = CreateWebRequest(_HRurl);
                InsertEnvelopeIntoWebRequest(xmlDocument, httpWebRequest);
                IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();
                string result;
                using (WebResponse webResponse = httpWebRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        result = rd.ReadToEnd();
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(result);

                        // TODO if receiv error message integraty please check the body of base64 contains new line or not I try without in producation and it is working fine
                        string ResponseNode = string.Empty;
                        if (xml.InnerXml.Contains("NS2:Body"))
                        {
                            ResponseNode = xml.GetElementsByTagName("NS2:Body")[0].InnerXml.Replace(@"xmlns:NS2=""http://id.gov.ae/schema/notification""", "").Replace("NS2:", "");
                        }
                        else
                        {
                            ResponseNode = xml.GetElementsByTagName("NS3:Body")[0].InnerXml.Replace(@"xmlns:NS3=""http://id.gov.ae/schema/notification""", "").Replace("NS3:", "");
                        }

                        SMSResponse.Body XMLObjBody = XMLProcessing.Deserializer<SMSResponse.Body>("<Body>" + ResponseNode + "</Body>");

                        if (XMLObjBody.responseCode == (int)Helper.Enum.responseCode.SuccessNotify)
                        {
                            return IntResultLib.GetResult(1);
                        }
                        else
                        {
                            return IntResultLib.GetResult(2, XMLObjBody.responseDescription);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        private static XmlDocument CreateSMSSoapEnvelope(String requestHeader, String requestBody)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                string envelope = string.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:not=""http://id.gov.ae/ws/notification"" xmlns:com=""http://id.gov.ae/schema/commontypes"" xmlns:not1=""http://id.gov.ae/schema/notification"">
                <soapenv:Header/> <soapenv:Body> 
                <not:sendNotification>{0}{1}</not:sendNotification> 
                </soapenv:Body> </soapenv:Envelope>",
                requestHeader, requestBody);

                envelope = Common.ReplaceXMLStr(envelope);

                xmlDocument.LoadXml(envelope);
                return xmlDocument;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get language type 
        /// </summary>
        /// <param name="Lang"></param>
        /// <returns></returns>
        public static string GetLanguageMessage(string Lang)
        {
            if (Lang == Constant.ENGLISH_STR || Lang == Constant.ENGLISH_NUM)
            {
                return "EN";
            }
            else if (Lang == Constant.ARABIC_STR || Lang == Constant.ARABIC_NUM)
            {
                return "AR";
            }

            return "EN";
        }
    }
}
