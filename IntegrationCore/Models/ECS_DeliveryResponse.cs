using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntegrationCore.Models
{
    
    [XmlRoot("FollowStatus")]
    public class ECS_DeliveryResponse
    {
        [XmlElement("FollowRespponseStatus")]
        public int FollowRespponseStatus { get; set; }
        [XmlElement("ResponseMessage")]
        public string ResponseMessage { get; set; }

       
    }
}
