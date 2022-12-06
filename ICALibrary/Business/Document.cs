using ICALibrary.Helper;
using ICALibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace ICALibrary.Business
{
    public class Document : SoapRequest
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
        /// Call service AddNewDocument from ESB service
        /// </summary>
        /// <param name="requestHeader"></param>
        /// <param name="requestBody"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IntResult CallAddNewDocumentWebService(String requestHeader, String requestBody, String url, ResponseCodesESB responseCodesESB)
        {
            try
            {
                string _HRurl = url;
                XmlDocument xmlDocument = CreateDocumentSoapEnvelope(requestHeader, requestBody);
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
                        string ResponseNode = string.Empty;
                        if (xml.InnerXml.Contains("NS2:Body"))
                        {
                            ResponseNode = xml.GetElementsByTagName("NS2:Body")[0].InnerXml.Replace(@"xmlns:NS2=""http://id.gov.ae/ws/tarasolservice""", "").Replace("NS2:", "");
                        }
                        else if (xml.InnerXml.Contains("NS3:Body"))
                        {
                            ResponseNode = xml.GetElementsByTagName("NS3:Body")[0].InnerXml.Replace(@"xmlns:NS3=""http://id.gov.ae/schema/tarasolservice""", "").Replace("NS3:", "");
                        }
                        else
                        {
                            ResponseNode = xml.GetElementsByTagName("out2:Body")[0].InnerXml.Replace("out2:", "");
                        }

                        DocumentResponse.Body XMLObjBody = XMLProcessing.Deserializer<DocumentResponse.Body>("<Body>" + ResponseNode + "</Body>");
                        Helper.Enum.responseStatusNotification statusNotification = Helper.Enum.responseStatusNotification.NotTryAgain;

                        if (XMLObjBody != null)
                        {
                            if (XMLObjBody.responseCode == (int)Helper.Enum.responseCode.SuccessNotify)
                            {
                                return IntResultLib.GetResult(1);
                            }
                            else
                            {
                                
                                // detect response code  old way static 
                                //if (Constant.responseCodes.Find( e => e.Code== XMLObjBody.responseCode) != null)
                                //{
                                //    statusNotification = Helper.Enum.responseStatusNotification.TryAgain;
                                //}
                                // New way
                                if (!(responseCodesESB is null))
                                {
                                    statusNotification = (responseCodesESB.ResponseCodeESB.Find(e => e.Code == XMLObjBody.responseCode) != null) ? Helper.Enum.responseStatusNotification.TryAgain : Helper.Enum.responseStatusNotification.NotTryAgain;
                                }
                                //
                                string resultMSG = $"Code : {XMLObjBody.Code} ,transactionRefNo : {XMLObjBody.transactionRefNo} , " +
                                                   $"Message: {XMLObjBody.Message} , responseCode: {XMLObjBody.responseCode} , " +
                                                   $"Description: {XMLObjBody.responseDescription}, Time : {XMLObjBody.timestamp}";
                                //string resultMSG = string.Format("Code : {0} ,transactionRefNo : {4} , Message: {1} , Description: {2}, Time : {3}", XMLObjBody.Code, XMLObjBody.Message, XMLObjBody.responseDescription, XMLObjBody.timestamp, XMLObjBody.transactionRefNo);
                                return IntResultLib.GetResult(2, resultMSG, statusNotification);
                            }
                        }
                        else
                        {
                            statusNotification = Helper.Enum.responseStatusNotification.TryAgain;
                            string resultMSG = "Response XML Error";
                            return IntResultLib.GetResult(2, resultMSG, statusNotification);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string resultMSG = $"CallAddNewDocumentWebService::catch, message: {e.Message}" ;
                return IntResultLib.GetResult(2, resultMSG, Helper.Enum.responseStatusNotification.TryAgain);
                //throw e;
            }
        }


        /// <summary>
        /// Call service DeliveryRepor from ESB service
        /// </summary>
        /// <param name="requestHeader"></param>
        /// <param name="requestBody"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IntResult CallDeliveryReportWebService(String requestHeader, String requestBody, String url)
        {
            try
            {
                string _HRurl = url;
                XmlDocument xmlDocument = CreateDelieverDocumentSoapEnvelope(requestHeader, requestBody);
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
                        string ResponseNode = string.Empty;
                        if (xml.InnerXml.Contains("NS2:Body"))
                        {
                            ResponseNode = xml.GetElementsByTagName("NS2:Body")[0].InnerXml.Replace(@"xmlns:NS2=""http://id.gov.ae/ws/tarasolservice""", "").Replace("NS2:", "");
                        }
                        else if (xml.InnerXml.Contains("NS3:Body"))
                        {
                            ResponseNode = xml.GetElementsByTagName("NS3:Body")[0].InnerXml.Replace(@"xmlns:NS3=""http://id.gov.ae/schema/tarasolservice""", "").Replace("NS3:", "");
                        }
                        else
                        {
                            ResponseNode = xml.GetElementsByTagName("Body")[0].InnerXml.Replace("out2:", "");
                        }

                        DocumentResponse.Body XMLObjBody = XMLProcessing.Deserializer<DocumentResponse.Body>("<Body>" + ResponseNode + "</Body>");

                        if (XMLObjBody.responseCode == (int)Helper.Enum.responseCode.SuccessNotify)
                        {
                            return IntResultLib.GetResult(1);
                        }
                        else
                        {
                            string resultMSG = string.Format("Code : {0} , transactionRefNo: {1} , Description: {2}, Time : {3}", XMLObjBody.responseCode, XMLObjBody.transactionRefNo, XMLObjBody.responseDescription, XMLObjBody.timestamp);
                            return IntResultLib.GetResult(2, resultMSG);
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
        private static XmlDocument CreateDocumentSoapEnvelope(String requestHeader, String requestBody)
        {
            try
            {
                // this must change according to XML that will provide us
                XmlDocument xmlDocument = new XmlDocument();
                string envelope = string.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tar=""http://id.gov.ae/ws/tarasolservice"" xmlns:com=""http://id.gov.ae/schema/commontypes"" xmlns:tar1=""http://id.gov.ae/schema/tarasolservice"">
                <soapenv:Header/> <soapenv:Body> 
                <tar:addNewDocument>{0}{1}</tar:addNewDocument> 
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
        ///    
        /// </summary>
        /// <param name="requestHeader"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private static XmlDocument CreateDelieverDocumentSoapEnvelope(String requestHeader, String requestBody)
        {
            try
            {
                // this must change according to XML that will provide us
                XmlDocument xmlDocument = new XmlDocument();
                string envelope = string.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tar=""http://id.gov.ae/ws/tarasolservice"" xmlns:com=""http://id.gov.ae/schema/commontypes"" xmlns:tar1=""http://id.gov.ae/schema/tarasolservice"">
                <soapenv:Header/> <soapenv:Body> 
                <tar:deliveryReport>{0}{1}</tar:deliveryReport>
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
    }
}
