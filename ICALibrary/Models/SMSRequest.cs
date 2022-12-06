using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ICALibrary.Models
{
    public class SMSRequest
    {


        [XmlRoot("com:Header")]
        public class Header
        {
            [XmlElement("com:serviceName")]
            public string serviceName { get; set; }

            [XmlElement("com:sourceChannel")]
            public string sourceChannel { get; set; }

            [XmlElement("com:serviceVersion")]
            public int serviceVersion { get; set; }

            [XmlElement("com:serviceLanguage")]
            public string serviceLanguage { get; set; }

            [XmlElement("com:userName")]
            public string userName { get; set; }

            [XmlElement("com:password")]
            public string password { get; set; }

            [XmlElement("com:hash")]
            public string hash { get; set; }
        }

        [XmlRoot("not1:Body")]
        public class Body
        {
            [XmlElement("not1:transactionRefNo")]
            public string transactionRefNo { get; set; }

            [XmlElement("not1:notification")]
            public Notification Notification { get; set; }

            [XmlElement("not1:timestamp")]
            public DateTime timestamp { get; set; }
        }

        [XmlRoot("not1:notification")]
        public class Notification
        {
            [XmlElement("not1:notificationType")]
            public string notificationType { get; set; }

            [XmlElement("not1:toAddress")]
            public string toAddress { get; set; }

            [XmlElement("not1:messageBody")]
            public string messageBody { get; set; }
        }

    }

    public class SMSResponse
    {
        [XmlRoot("Body")]
        public class Body
        {
            [XmlElement("transactionRefNo")]
            public string transactionRefNo { get; set; }

            [XmlElement("responseCode")]
            public int responseCode { get; set; }

            [XmlElement("responseDescription")]
            public string responseDescription { get; set; }

            [XmlElement("timestamp")]
            public string timestamp { get; set; }
        }
     }
}
