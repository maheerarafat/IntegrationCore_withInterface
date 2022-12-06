using System;
using System.Collections.Generic;
using System.Text;

namespace ICALibrary.Models
{
    public class IntUser
    {
        public String UAE_ID { get; set; }
        public String MAIL { get; set; }
        public String MOBILE_NO { get; set; }
        public String EMP_NAME_A { get; set; }
        public String EMP_NAME_E { get; set; }
        public String RANK_ID { get; set; }
        public String EMP_STATUS { get; set; }
        public String LAST_DEPT_ID { get; set; }
        public String RANK_NAME_A { get; set; }
        public String DEPT_NAME_A { get; set; }
        public String OLD_ECS_NUM { get; set; }
        public String EMP_NO { get; set; }
        public String SECURITYKEY { get; set; }
    }

    public class IntUserRequest
    {
        public String code { get; set; }
        public String ecs { get; set; }
        public String empno { get; set; }

        public String mobile { get; set; }
        public String email { get; set; }
    }

    public class UserTarasol
    {
        public String flduid { get; set; }

        public String fldnickname { get; set; }
    }
}
