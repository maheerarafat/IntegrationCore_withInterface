
namespace IntegrationCore.Models
{
    public class Target
    {
      
        public string GlobalID { get; set; }
        public string FaxNo { get; set; }
        //public int Type { get; set; }
        public string Name { get; set; }
        //public int SendingType { get; set; }
    }
    public class DeptQAckTarget
    {
        public string UserFullName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }

    }
    
}
