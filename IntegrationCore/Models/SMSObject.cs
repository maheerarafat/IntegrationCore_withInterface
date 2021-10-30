using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.Models
{
    public class SMSObject
    {
        public string MobileNumber { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
    }
}
