using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.Helper
{
    public class Enum
    {
        public enum enumExchangStatus
        {
            Folder = 1,
            Email,
            Fax
        }

        public enum enumExchangAddressToType
        {
            Internal,
            External,
        }

        public enum enumEmailDistributor
        {
            Department,
            Individual,
        }

        public enum enumConnType
        {
            Oracle=1,
            Sql,
        }
    }
}
