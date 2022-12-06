using System;
using System.Collections.Generic;
using System.Text;

namespace ICALibrary.Models
{
    public class EmailDistributor
    {
        public string GlobalID { get; set; }
        public string DirectParentGlobalID { get; set; }
        public string MainParentGlobalID { get; set; }
        public string Email { get; set; }
        public int TypeDistributor { get; set; }
    }
}
