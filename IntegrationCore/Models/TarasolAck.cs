using IntegrationCore.Models;

namespace IntegrationCore.Models
{
    public class TarasolAck
    {
        public string ExternalExchangeNo { get; set; }
        public string LastParentGlobalID { get; set; }
        public string AckDate { get; set; }
        public string ReadBy { get; set; }
        public string SenderGlobalID { get; set; }
       
        public string ExchangeStatus { get; set; }
    }
}
