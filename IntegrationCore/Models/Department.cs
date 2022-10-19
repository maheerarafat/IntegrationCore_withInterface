

namespace IntegrationCore.Models
{
    public class Department
    {
        public string FLDUID { get; set; }
        public string FLDENAME { get; set; }
        public string FLDLNAME { get; set; }
        public string FLDCODE { get; set; }
        public string FLDPARENTID { get; set; }
        public int FLDLEVEL { get; set; }
        public string FLDMAINPARENTID { get; set; }
        public string FLDROLEDISTRIBUTOR { get; set; }
        public int FLDTYPE { get; set; }
        
        public string FLDACTIVE { get; set; }
        public string FLDOLDDEPTID { get; set; }
        public string FLDOLDDEPTACTIVE { get; set; }
       public string fldDepartmentClassCode { get; set; }
        //public string FLDNOTES { get; set; }
        //public string FLDCOUNTER { get; set; }
        //public string FLDFLAGS { get; set; }
        //public string FLDISMAIN { get; set; }
    }
}
