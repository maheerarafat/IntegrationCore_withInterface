using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.Helper
{
    public class Enum
    {
        /// <summary>
        /// 
        /// </summary>
        public enum Lang
        {
            English,
            Arabic
        }

        /// <summary>
        /// 
        /// </summary>
        public enum responseCode
        {
            SuccessNotify,
            Success,
            Failed
        }


        public enum scheduleType
        {
            Done,
            Progress,
            NotStarted
        }
    }
}
