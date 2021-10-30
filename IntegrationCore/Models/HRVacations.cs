using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IntegrationCore.Models
{
    [XmlRoot("GetListOfVacationsResponse")]
    public class HRVacations
    {
        [XmlElement("Content")]
        public List<Vacation> VacationsList { get; set; }
    }
    public class Vacation
    {

        [XmlElement("TransactionSerial")]
        public int FLDTRANSACTIONSERIAL { get; set; }

        [XmlElement("TransactionDate")]
        public DateTime FLDTRANSACTIONDATE { get; set; }

        [XmlElement("MilitaryNumber")]
        public string FLDMILITARYNUMBER { get; set; }
        //[XmlElement("TransactionSubType")]
        //public string TransactionSubType { get; set; }
        //[XmlElement("TransactionSubTypeArabicDescription")]
        //public string TransactionSubTypeArabicDescription { get; set; }
        //[XmlElement("TransactionSubTypeEnglishDescription")]
        //public string TransactionSubTypeEnglishDescription { get; set; }
        [XmlElement("StartDate")]
        public DateTime FLDSTARTDATE { get; set; }

        [XmlElement("EndDate")]
        public DateTime FLDENDDATE { get; set; }
        //[XmlElement("NumberOfDays")]
        //public string NumberOfDays { get; set; }
        public string FLDNOTES { get; set; }
        public int FLDISDONE { get; set; }

        [XmlElement("EmployeeID")]
        public int FLDEMPLOYEEID { get; set; }
    }
}
