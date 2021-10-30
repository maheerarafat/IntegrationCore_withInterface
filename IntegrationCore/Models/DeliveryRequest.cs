

using System;

namespace IntegrationCore.Models
{
    public class DeliveryRequest
    {
        
        public string ReadBy { get; set; }

        public string TransactionID { get; set; }

        public int EntityCode { get; set; }

        public DateTime acknowledgementDate { get; set; }

    }
}
