using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.Models
{
    public class DownloadUploadRequest
    {
        public string SessionID { get; set; }
        public string DocUID { get; set; }
        public string PageGUID { get; set; }
    }
}
