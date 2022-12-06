using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.Models
{
    public class ConfigAttribute
    {

        public string consumerID { get; set; }

        public string urlServiceESB { get; set; }

        public string urlServiceNotify { get; set; }

        public string senderMail { get; set; }

        public long maxSizeFileCompress { get; set; }

        public bool enableSendNotify { get; set; }
    }
}
