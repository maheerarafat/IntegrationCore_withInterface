using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.Models
{
    public class Report
    {
    }

    public class ReportFilter
    {
        public string Date { get; set; }
    }

    public class ReportResult
    {
        public string Date { get; set; }

        public List<ICALibrary.Models.IntJobEsb> Resources { get; set; }

        public List<ICALibrary.Models.IntJobEsb> ServicesCount { get; set; }

        public int Incident { get; set; }

        public int CountFolderInDraft { get; set; }
    }
}
