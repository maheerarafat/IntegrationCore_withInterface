using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ICALibrary.Models
{
    public class Email
    {

        [Required]
        public string ExchangeNo { get; set; } // Trace Number

        [Required]
        public string Body { get; set; } // Full HTML

        public List<AttachmentEmail> AttachmentEmails;

        [Required]
        public string From { get; set; } // EMail string 

        [Required]
        public string Subject { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Priority { get; set; }

        public List<EmailDistributor> EmailDistributors { get; set; }
    }


    public class AttachmentEmail
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileContent { get; set; } // base64 format
    }

}
