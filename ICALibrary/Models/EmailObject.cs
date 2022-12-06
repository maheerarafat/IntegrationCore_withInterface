using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ICALibrary.Models
{
    public class EmailObject
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
            public String toAddress { get; set; }

            [XmlElement("not1:fromAddress")]
            public String fromAddress { get; set; }

            [XmlElement("not1:subject")]
            public String subject { get; set; }

            [XmlElement("not1:messageBody")]
            public String messageBody { get; set; }

            [XmlElement("not1:ccAddress")]
            public String ccAddress { get; set; }

            [XmlElement("not1:bccAddress")]
            public String bccAddress { get; set; }

            [XmlElement("not1:importance")]
            public String importance { get; set; }

            [XmlElement("not1:attachments")]
            public EmailAttachment attachments { get; set; }
        }


        [XmlRoot("not1:attachments")]
        public class EmailAttachment
        {
            [XmlElement("not1:attachment")]
            public List<AttachmentItem> attachmentItem { get; set; } = new List<AttachmentItem>();
        }

        [XmlRoot("not1:attachment")]
        public class AttachmentItem
        {
            [XmlElement("not1:name")]
            public String name { get; set; }

            [XmlElement("not1:contentType")]
            public String contentType { get; set; }

            [XmlElement("not1:data")]
            public String data { get; set; }

            [XmlElement("not1:dataLocation")]
            public String dataLocation;
        }
    }


    public class ICA_EmailResponse
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
