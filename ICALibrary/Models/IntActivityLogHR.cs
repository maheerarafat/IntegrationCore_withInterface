using System;
using System.Collections.Generic;
using System.Text;

namespace ICALibrary.Models
{
    public class IntActivityLogHR
    {
        public string FLDID { get; set; }

        public string FLDSYNCNAME { get; set; }

        public string FLDDESCRIPTION { get; set; }

        public int FLDTYPE { get; set; }

        public DateTime FLDDATE { get; set; }

        public int FLDTYPEINSERT { get; set; }
    }
}
