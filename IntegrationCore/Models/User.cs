
namespace IntegrationCore.Models
{
    public class User
    {
        public bool FldBlocked { get; set; }
        public string FldCellPhoneNumber { get; set; }
        public string FldDescription { get; set; }
        public string FldEmail { get; set; }
        public string FldExpiryDate { get; set; }
        public string FldFamilyName { get; set; }
        public string FldFirstName { get; set; }
        public string FldName { get; set; }
        public string FldNickName { get; set; }
        public string FldPassword { get; set; }
        public string FldUID { get; set; }
        public string FldLocalFirstName { get; set; }
        public string FldLocalFamilyName { get; set; }
        public string FldTitle { get; set; }
        public string FldLocalTitle { get; set; }
        public string FldUserCode { get; set; }
        public string FldDigitalSignatureImage { get; set; }
        public string FldBlockChainKey { get; set; }
        public string FldBlockChainAddress { get; set; }
        public string FldDisplayName { get; set; }
        //public int FldUserStatus { get; set; }
        //public string FldWebCPanelPermissions { get; set; }
        //public string FldNotes { get; set; }
        //public string FldIP { get; set; }
        //public string FldSyncUID { get; set; }
        //public string FldDepartment { get; set; }
        //public string FldModifiedFields { get; set; }
        //public string FldFlags { get; set; }
        //public string FldLASTLOGIN { get; set; }
        public string HREMPID { get; set; }
        public string HRDEPTCODE { get; set; }
        public string HRPOSCODE { get; set; }
        public string HRPOSNAME { get; set; }
        public string ResponseCode { get; set; }
        public string EMessage { get; set; }
        public string LMessage { get; set; }
        

    }
    public class IntUserRequest
    {
        public string code { get; set; }
        public string ecs { get; set; }
        public string empno { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        
    }

    public class IntUser
    {
        public string UAE_ID { get; set; }
        public string MAIL { get; set; }
        public string MOBILE_NO { get; set; }
        public string EMP_NAME_A { get; set; }
        public string EMP_NAME_E { get; set; }
        public string RANK_ID { get; set; }
        public string EMP_STATUS { get; set; }
        public string LAST_DEPT_ID { get; set; }
        public string RANK_NAME_A { get; set; }
        public string DEPT_NAME_A { get; set; }
        public string OLD_ECS_NUM { get; set; }
        public string EMP_NO { get; set; }
        public string SECURITYKEY { get; set; }
    }
}
