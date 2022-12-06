using System;
using System.Collections.Generic;
using System.Text;

namespace ICALibrary.Models
{
    public class IntJobEsb
    {
        public int fldid { get; set; }

        public String fldname { get; set; }

        public String flddescription { get; set; }

        public int fldtype { get; set; }

        public DateTime flddateStart { get; set; }

        public DateTime flddateEnd { get; set; }

        public float fldduration { get; set; }

        public String fldbodyrequest { get; set; }

        public int fldnumbertimesendschudle { get; set; }

        public int cnt { get; set; }
    }
}
