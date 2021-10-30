using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntegrationCore.Models
{
    [XmlRoot("moic:DeliveryReportRequest")]
    public class ECS_DeliveryRequest
    {
        [XmlElement("moic:EntityCode")]
        public string EntityCode { get; set; }
        [XmlElement("moic:TransactionId")]
        public string TransactionId { get; set; }
        [XmlElement("moic:ReadBy")]
        public string ReadBy { get; set; }
    }
}
