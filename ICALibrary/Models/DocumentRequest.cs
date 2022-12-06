using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ICALibrary.Models
{

    public class DocumentRequest
    {

        [XmlRoot("com:Header")]
        public class Header
        {
            [XmlElement("com:serviceName")]
            public string serviceName { get; set; }

            [XmlElement("com:sourceChannel")]
            public string sourceChannel { get; set; }

            [XmlElement("com:serviceVersion")]
            public int serviceVersion { get; set; }

            [XmlElement("com:serviceLanguage")]
            public string serviceLanguage { get; set; }

            [XmlElement("com:userName")]
            public string userName { get; set; }

            [XmlElement("com:password")]
            public string password { get; set; }

            [XmlElement("com:hash")]
            public string hash { get; set; }
        }

        [XmlRoot("Body")]
        public class Body
        {
            [XmlElement("tar1:transactionRefNo")]
            public string transactionRefNo { get; set; }

            [XmlElement("tar1:OrgId")]
            public string orgId { get; set; }

            [XmlElement("tar1:EntityCode")] // Site ID
            public String EntityCode { get; set; } // TODO Must rollback comment after test


            [XmlElement("tar1:Sender")]  // Global ID
            public Sender SourceUnitId { get; set; } //OK

            [XmlElement("tar1:Receivers")]  // Global ID
            public Destination DestinationList { get; set; } //OK

            [XmlElement("tar1:CorrespondenceNo")]
            public string OutboxNumber { get; set; }



            [XmlElement("tar1:Date")]
            public string OutboxDate { get; set; } // ok

            [XmlElement("tar1:DocumentSubject")]
            public string DocumentSubject { get; set; } // ok

            [XmlElement("tar1:Priority")]
            public int Priority { get; set; } //ok

            [XmlElement("tar1:ForeignOutboxNo")]
            public string ForeignOutboxNo { get; set; } // ok

            [XmlElement("tar1:DocumentType")]
            public int DocumentType { get; set; } // ok

            [XmlElement("tar1:DocumentBody")]
            public string HtmlContent { get; set; } // ok

            [XmlElement("tar1:Secrecy")]
            public int SecrecyLevel { get; set; } // ok

            [XmlElement("tar1:TransactionIds")]  // Global ID
            public TransactionIds TransactionIds { get; set; } //ok

            [XmlElement("tar1:Attachments")]
            public Attachments Attachments { get; set; } // ok


            [XmlElement("tar1:timestamp")]
            public string timestamp { get; set; }
        }


        [XmlRoot("tar1:Attachments")]
        public class Attachments
        {
            [XmlElement("tar1:Attachment")]
            public List<Attachment> attachmentItems { get; set; }
        }

        [XmlRoot("tar1:Attachment")]
        public class Attachment
        {
            [XmlElement("tar1:DocumentContent")]
            public string Stream { get; set; }

            [XmlElement("tar1:DocumentName")]
            public string DocumentName { get; set; } // ok

            [XmlElement("tar1:Description")]
            public string AttDescription { get; set; } // ok

            [XmlElement("tar1:documentNumber")]
            public int DocumentNumber { get; set; } // ok
                                                    //MimeType
            [XmlElement("tar1:MimeType")]
            public string MimeType { get; set; } // ok

        }

        [XmlRoot("tar1:Sender")]
        public class Sender
        {
            [XmlElement("tar1:Code")]
            public string SourceValue { get; set; } // ok
        }

        [XmlRoot("tar1:Receivers")]
        public class Destination
        {
            [XmlElement("tar1:ReceiverDetail")]
            public List<ReceiverDetail> receiverDetail { get; set; } // ok
        }

        [XmlRoot("tar1:ReceiverDetail")]
        public class ReceiverDetail
        {
            [XmlElement("tar1:Code")]
            public string DestinationValue { get; set; } // ok
        }


        [XmlRoot("tar1:TransactionIds")]
        public class TransactionIds
        {
            [XmlElement("tar1:TransactionId")]
            public List<string> TransactionIdValue { get; set; }
        }

        //public class TransactionId
        //{
        //    [XmlElement("tar1:TransactionId")]
        //    public string TransactionIdValue { get; set; } // ok
        //}

    }


    public class DocumentResponse
    {
        [XmlRoot("Body")]
        public class Body
        {
            [XmlElement("Code")]
            public string Code { get; set; }

            [XmlElement("Message")]
            public string Message { get; set; }

            [XmlElement("RequestId")]
            public string RequestId { get; set; }

            [XmlElement("transactionRefNo")]
            public string transactionRefNo { get; set; } //ok 

            [XmlElement("responseCode")]
            public int responseCode { get; set; } //ok 

            [XmlElement("responseDescription")]
            public string responseDescription { get; set; } //ok

            [XmlElement("timestamp")]
            public string timestamp { get; set; } // ok
        }
    }


   

    public class DeliveryRequest
    {
        [XmlRoot("Body")]
        public class Body
        {
            [XmlElement("tar1:transactionRefNo")]
            public string transactionRefNo { get; set; }

            [XmlElement("tar1:OrgId")]
            public int orgId { get; set; }

            [XmlElement("tar1:GlobalID")]
            public GlobalID EntityCode { get; set; }

            [XmlElement("tar1:CorrespondenceNo")]
            public string TransactionId { get; set; }

            [XmlElement("tar1:ReadBy")]
            public string ReadBy { get; set; } // OK

            [XmlElement("tar1:DateOfUpdate")]
            public string DateOfUpdate { get; set; } 

            [XmlElement("tar1:timestamp")]
            public string timestamp { get; set; }
        }


        [XmlRoot("tar1:GlobalID")]
        public class GlobalID
        {
            [XmlElement("tar1:Code")]
            public int Code { get; set; }

            [XmlElement("tar1:LName")]
            public string LName { get; set; }

            [XmlElement("tar1:EName")]
            public string EName { get; set; }
        }
    }

    public class DeliveryResponse
    {
        [XmlRoot("Body")]
        public class Body
        {
            [XmlElement("StatusCode")]
            public string statusCode { get; set; }

            [XmlElement("StatusDescription")]
            public string statusDescription { get; set; }


            [XmlElement("transactionRefNo")]
            public string transactionRefNo { get; set; } //ok 

            [XmlElement("responseCode")]
            public int responseCode { get; set; } //ok 

            [XmlElement("responseDescription")]
            public string responseDescription { get; set; } //ok

            [XmlElement("timestamp")]
            public string timestamp { get; set; } // ok
        }
    }
}
