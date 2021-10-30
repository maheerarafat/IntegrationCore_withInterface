
namespace IntegrationCore.Models
{
    public class IntResult
    {
        public int Code { get; set; }
        public string LMessage { get; set; }
        public string EMessage { get; set; }
    }
    public class ExportStatus
    {
        public string FailReaesonEmsg { get; set; }
        public string FailReaesonLmsg { get; set; }
        public string ExchangeNo { get; set; }
        public string RequestNo { get; set; }
        public string Docuid { get; set; }
        public string status { get; set; }
    }
}
