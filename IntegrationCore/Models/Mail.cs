using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.Models
{
    public class Mail
    {
        public string Sender { get; set; }

        public List<Receiver>Receivers { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<EmailAttachment> Attachments { get; set; }
    }
    public class Receiver
    {
        public string ReceiverEmail { get; set; }
        public ReceiverType ReceiverType { get; set; }
    }
    public enum ReceiverType
    {
        to = 1,
        cc = 2,
        bcc = 3
    }
    public class EmailAttachment
    {
        public string FileName { get; set; }
        public string FileContent { get; set; }
        public string FileType { get; set; }
    }
}
