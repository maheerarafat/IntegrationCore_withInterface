using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.Models
{
    public class IntJobEsb
    {
        public int flduid { get; set; }
        public String fldname { get; set; }
        public String flddescription { get; set; }
        public int fldtype { get; set; }
        public DateTime flddateStart { get; set; }
        public DateTime flddateEnd { get; set; }
        public float fldduration { get; set; }
        public String fldbodyrequest { get; set; }
        public int fldnumbertimesendschudle { get; set; }
    }
}
