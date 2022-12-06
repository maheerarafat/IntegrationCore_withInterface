using System;
using System.Collections.Generic;
using System.Text;

namespace ICALibrary.Models
{
    public class TarasolDocument
    {
        public Target Sender { get; set; }
        public List<Target> Receiver { get; set; }
        public List<TransactionId> TransactionIDs { get; set; }
        public string Subject { get; set; }
        public string CorrespondenceNo { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public int Priority { get; set; }
        public int Importance { get; set; }
        public int Secrecy { get; set; }
        public string Classification { get; set; }
        public string SenderNo { get; set; }
        public string ReceiverNo { get; set; }
        public List<Attachment> Files { get; set; }
    }

    public class TransactionId
    {
        public string TransactionIDs { get; set; }
    }

    public class Target
    {
        public string GlobalID { get; set; }
    }

    public class Attachment
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileContent { get; set; } // base64 format
    }
}
