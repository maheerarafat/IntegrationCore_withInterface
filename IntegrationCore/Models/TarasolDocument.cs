using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntegrationCore.Models
{
    public class TarasolDocument
    {
        public string faxAuth { get; set; }
        public string faxurl { get; set; }
        public Target Sender { get; set; }
        public List<Target> Receiver { get; set; }
        
        [Required]
        public List<string> TransactionIDs { get; set; }
        //public string Global_ID  { get; set; }
        public string Subject { get; set; }
        public string CorrespondenceNo { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        //public string DueDate { get; set; }
        public string Body  { get; set; }
        //public string CreationUsername { get; set; }
        public int Priority { get; set; }
        public int Importance { get; set; }
        public int Secrecy { get; set; }
        public string ClassificationID { get; set; }
        
        public string SenderNo { get; set; }
        public string ReceiverNo { get; set; }
        
        public List<Attachment> Files { get; set; }
       
    }
     public class ScanFileResponse
    {
        public string Value { get; set; }
    }
    public class SendFileToTarasolReturnedData
    {
        public string Name { get; set; }
        public string FileID { get; set; }
    }
}
