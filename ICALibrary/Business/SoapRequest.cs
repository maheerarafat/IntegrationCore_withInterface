using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace ICALibrary.Business
{
    public class SoapRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Method"></param>
        /// <param name="ContentType"></param>
        /// <param name="Accept"></param>
        /// <returns></returns>
        protected static HttpWebRequest CreateWebRequest(string url, string Method = "Post", string ContentType = "text/xml;charset=\"utf-8\"", string Accept = "text/xml")
        {
            try
            {
                ServicePointManager.Expect100Continue = false;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = ContentType;
                httpWebRequest.Accept = Accept;
                httpWebRequest.Method = Method;
                // config 
                //httpWebRequest.KeepAlive = false;
                //httpWebRequest.ProtocolVersion = HttpVersion.Version10;
                //httpWebRequest.ServicePoint.ConnectionLimit = 1;

                return httpWebRequest;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="soapEnvelpoeXML"></param>
        /// <param name="httpWebRequest"></param>
        protected static void InsertEnvelopeIntoWebRequest(XmlDocument soapEnvelpoeXML, HttpWebRequest httpWebRequest)
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
    }
}
