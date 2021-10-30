using System.Collections.Generic;
using System.Xml.Serialization;

namespace IntegrationCore.Models
{
    
    [XmlRoot("")]
    public class HRUpdatedOccupations
    {
        [XmlElement("Content")]
        public List<HRUpdatedOccupation> HRUpdatedOccupationsList { get; set; }
    }
    public class HRUpdatedOccupation
    {
        [XmlElement("OccupationID")]
        public string OccupationID { get; set; }
        [XmlElement("OccupationGroupID")]
        public string OccupationGroupID { get; set; }
        [XmlElement("OccupationArabicName")]
        public string OccupationArabicName { get; set; }
        [XmlElement("OccupationEnglishName")]
        public string OccupationEnglishName { get; set; }
        [XmlElement("TransactionType")]
        public string TransactionType { get; set; }
        [XmlElement("TransactionDate")]
        public string TransactionDate { get; set; }
        [XmlElement("LastDepartmentID")]
        public string LastDepartmentID { get; set; }
    }
}
