using System.Collections.Generic;
using System.Xml.Serialization;

namespace IntegrationCore.Models
{
    [XmlRoot("GetListOfAllDepartmentsResponse")]
    public class HRDepartments
    {
        [XmlElement("Content")]
        public List<HRDepartment> HRDepartmentsList { get; set; }
    }
    public class HRDepartment
    {
        [XmlElement("DepartmentID")]
        public string DepartmentID { get; set; }
        [XmlElement("DepartmentArabicName")]
        public string DepartmentArabicName { get; set; }
        [XmlElement("DepartmentEnglishName")]
        public string DepartmentEnglishName { get; set; }
        [XmlElement("DepartmentStatus")]
        public string DepartmentStatus { get; set; }
        [XmlElement("DepartmentParentID")]
        public string DepartmentParentID { get; set; }
        [XmlElement("DepartmentClassCode")]
        public string DepartmentClassCode { get; set; }
        [XmlElement("OccupationGroupID")]
        public string OccupationGroupID { get; set; }
        [XmlElement("OldDepartmentID")]
        public string OldDepartmentID { get; set; }
    }
}
