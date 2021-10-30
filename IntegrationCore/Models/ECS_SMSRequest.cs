using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntegrationCore.Models
{

    [XmlRoot("sen:SendSMS")]
    public class ECS_SMSRequest
    {

        [XmlElement("sen:MobileNumber")]
        public string MobileNumber { get; set; }
        [XmlElement("sen:Message")]
        public string Message { get; set; }
        [XmlElement("sen:Language")]
        public string Language { get; set; }
    }
}
