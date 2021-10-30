
namespace IntegrationCore.Models
{
    public class LoginResponse
    {
        //"ResultMessage":"Success","PrivilegeNumber":0,"PrivilegeAttributeNumber":0}
        public int Modules { get; set; }
        public string ExpiredDuration { get; set; }
        public string Permission { get; set; } //need to change to biggest number
        public string UserFullEName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsHasDigitalSign { get; set; }
        public string SessionID { get; set; }
        public bool IsLoggedIN { get; set; }
        public string ResultMessage { get; set; }
        public string PrivilegeNumber { get; set; } //need to change to biggest number
        public string PrivilegeAttributeNumber { get; set; }//need to change to biggest number
        public string Token { get; set; }
    }
}
