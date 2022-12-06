using System;
using System.Collections.Generic;
using System.Text;

namespace ICALibrary.Models
{
    public class ManualRequest
    {
        public String Id { get; set; }

        public String MethodName { get; set; }

        public String BodyRequest { get; set; }
    }
}
