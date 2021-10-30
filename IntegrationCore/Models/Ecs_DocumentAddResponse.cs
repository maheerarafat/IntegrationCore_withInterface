using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntegrationCore.Models
{
    [XmlRoot("Status")]
    public class Ecs_DocumentAddResponse
    {
        [XmlElement("RequestStatus")] 
        public int RequestStatus { get; set; }
        [XmlElement("StatusMessage")] 
        public string StatusMessage { get; set; }

        [XmlElement("RequestId")]  
        public string RequestId { get; set; }
    }
}
