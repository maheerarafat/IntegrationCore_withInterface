using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ICALibrary.Models
{
    [XmlRoot("ResponseCodesESB")]
    public class ResponseCodesESB
    {
        [XmlElement("ResponseCodeESB")]
        public List<ResponseCodeESB> ResponseCodeESB { get; set; } = new List<ResponseCodeESB>();
    }

    public class ResponseCodeESB
    {
        [XmlElement("Description")]
        public string Description { get; set; }
        [XmlElement("Code")]
        public int Code { get; set; }
    }
}
