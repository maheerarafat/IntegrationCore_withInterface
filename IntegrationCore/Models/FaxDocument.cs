using System.Collections.Generic;

namespace IntegrationCore.Models
{
    public class FaxDocument
    {
        public string LoginUserName { get; set; }
        public string LoginPassword { get; set; }
        public string SenderNumber { get; set; }
        public string ReceiverNumber { get; set; }
        public string Body { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
