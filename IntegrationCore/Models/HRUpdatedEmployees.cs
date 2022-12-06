using System.Collections.Generic;
using System.Xml.Serialization;

namespace IntegrationCore.Models
{

    [XmlRoot("")]
    public class HRUpdatedEmployees
    {
        [XmlElement("Content")]
        public List<HRUpdatedEmployee> HRUpdatedEmployeessList { get; set; }
    }
    public class HRUpdatedEmployee
    {
        [XmlElement("EmployeeID")]
        public string EmployeeID { get; set; }
        [XmlElement("SerialNumber")]
        public string SerialNumber { get; set; }
        [XmlElement("TransactionType")]
        public string TransactionType { get; set; }
        [XmlElement("ColumnName")]
        public string ColumnName { get; set; }
        [XmlElement("OldValue")]
        public string OldValue { get; set; }
        [XmlElement("NewValue")]
        public string NewValue { get; set; }
        [XmlElement("TransactionDate")]
        public string TransactionDate { get; set; }
    }
}
