using System.Collections.Generic;
using System.Xml.Serialization;

namespace IntegrationCore.Models
{
  
    [XmlRoot("GetEmployeeDetailsResponse")]
    public class HREmployeeDetails
    {
        [XmlElement("Content")]
        public List<HREmployeeDetail> HREmployeeDetailsList { get; set; }
    }
    public class HREmployeeDetail
    {
        [XmlElement("EmployeeID")]
        public string EmployeeID { get; set; }
        [XmlElement("MilitaryNumber")]
        public string MilitaryNumber { get; set; }
        [XmlElement("ArabicEmployeeName")]
        public string ArabicEmployeeName { get; set; }
        [XmlElement("EnglishEmployeeName")]
        public string EnglishEmployeeName { get; set; }
        [XmlElement("RankID")]
        public string RankID { get; set; }
        [XmlElement("RankName")]
        public string RankName { get; set; }
        [XmlElement("OccupationGroupID")]
        public string OccupationGroupID { get; set; }
        [XmlElement("OccupationGroupName")]
        public string OccupationGroupName { get; set; }
        [XmlElement("OccupationJobID")]
        public string OccupationJobID { get; set; }
        [XmlElement("OccupationJobName")]
        public string OccupationJobName { get; set; }
        [XmlElement("JobTileGroupID")]
        public string JobTileGroupID { get; set; }
        [XmlElement("JobTileGroupName")]
        public string JobTileGroupName { get; set; }
        [XmlElement("JobTitleID")]
        public string JobTitleID { get; set; }
        [XmlElement("JobTitleName")]
        public string JobTitleName { get; set; }
        [XmlElement("Gender")]
        public string Gender { get; set; }
        [XmlElement("DateOfBirth")]
        public string DateOfBirth { get; set; }
        [XmlElement("EmiratesID")]
        public string EmiratesID { get; set; }
        [XmlElement("CategoryID")]
        public string CategoryID { get; set; }
        [XmlElement("CategoryName")]
        public string CategoryName { get; set; }
        [XmlElement("CountryName")]
        public string CountryName { get; set; }
        [XmlElement("LastDepartmentID")]
        public string LastDepartmentID { get; set; }
        [XmlElement("LastDepartmentName")]
        public string LastDepartmentName { get; set; }
        [XmlElement("MainDepartmentID")]
        public string MainDepartmentID { get; set; }
        [XmlElement("MainDepartmentName")]
        public string MainDepartmentName { get; set; }
        [XmlElement("SubDepartmentID")]
        public string SubDepartmentID { get; set; }
        [XmlElement("SubDepartmentName")]
        public string SubDepartmentName { get; set; }
        [XmlElement("DepartmentID")]
        public string DepartmentID { get; set; }
        [XmlElement("DepartmentName")]
        public string DepartmentName { get; set; }
        [XmlElement("SectionID")]
        public string SectionID { get; set; }
        [XmlElement("SectionName")]
        public string SectionName { get; set; }
        [XmlElement("BranchID")]
        public string BranchID { get; set; }
        [XmlElement("BranchName")]
        public string BranchName { get; set; }
        [XmlElement("EmployeeStatus")]
        public string EmployeeStatus { get; set; }
    }
}
