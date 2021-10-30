using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IntegrationCore.Models
{
    [XmlRoot("")]
    public class HRUpdatedDepartments
    {
        [XmlElement("Content")]
        public List<HRUpdatedDepartment> HRUpdatedDepartmentsList { get; set; }
    }
    public class HRUpdatedDepartment
    {
        [XmlElement("DepartmentID")]
        public int DepartmentID { get; set; }
        [XmlElement("SerialNumber")]
        public int SerialNumber { get; set; }
        [XmlElement("TransactionType")]
        public string TransactionType { get; set; }
        [XmlElement("ChangedColumn")]
        public string ColumnName { get; set; }
        [XmlElement("OldValue")]
        public string OldValue { get; set; }
        [XmlElement("NewValue")]
        public string NewValue { get; set; }
        [XmlElement("TransactionDate")]
        public DateTime TransactionDate { get; set; }
    }
}
