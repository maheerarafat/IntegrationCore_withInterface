using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntegrationCore.Models
{
    [XmlRoot("ema:EmailNotificationRequest")]
    public class SoapEmailObject
    {

        [XmlElement("ema:EmailTo")]
        public List<SoapEmail> EmailTo { get; set; }
        [XmlElement("ema:EmailCC")]
        public List<SoapEmail> EmailCC { get; set; }
        [XmlElement("ema:EmailSubject")]
        public string EmailSubject { get; set; }
        [XmlElement("ema:EmailContent")]
        public string EmailContent { get; set; }
        [XmlArray("ema:Attachments")]
        public List<SoapEMialAttachment> Attachments { get; set; }
    }

    [XmlRoot("ema:Attachment")]
    public class SoapEMialAttachment
    {

        [XmlElement("ema:ContentType")]
        public string ContentType { get; set; }

        [XmlElement("ema:FileName")]
        public string FileName { get; set; }

        [XmlElement("ema:Body")]
        public string Body { get; set; }

        

    }
   
    public class SoapEmail
    {

        [XmlElement("ema:email")]
        public string email { get; set; }

        



    }
}
