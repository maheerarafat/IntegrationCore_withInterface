using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntegrationCore.Models
{
    [XmlRoot("AddNewDocumentRequest")]
    public class ECS_Document
    {
        [XmlElement("moic:EntityCode")] // Site ID
        public string EntityCode { get; set; }
        [XmlElement("moic:SourceUnitId")] // Global ID
        public string SourceUnitId { get; set; }

        [XmlElement("moic:Destinations")]  // Global ID
        public List<Destination> DestinationList { get; set; }

        [XmlElement("moic:OutboxNumber")]
        public string OutboxNumber { get; set; }

        [XmlElement("moic:OutboxDate")]
        public string OutboxDate { get; set; }

        [XmlElement("moic:DocumentSubject")]
        public string DocumentSubject { get; set; }

        [XmlElement("moic:Priority")]
        public int Priority { get; set; }

        [XmlElement("moic:SecrecyLevel")]
        public int SecrecyLevel { get; set; }
        [XmlElement("moic:Alignment")]
        public int Alignment { get; set; }
        [XmlElement("moic:DocumentType")]
        public int DocumentType { get; set; }

        [XmlElement("moic:HtmlContent")]
        public string HtmlContent { get; set; }

        [XmlArray("moic:Attachments")]
        public List<ECSAttachment> ECSAttachmentList { get; set; }

        [XmlArray("moic:TransactionIds")]  // Global ID
        public List<TransactionId> TransactionIdList { get; set; }
    }
    [XmlRoot("moic:Attachment")]
    public class ECSAttachment
    {

        [XmlElement("moic:Stream")]
        public string Stream { get; set; }

        [XmlElement("moic:DocumentName")]
        public string DocumentName { get; set; }

        [XmlElement("moic:AttDescription")]
        public string AttDescription { get; set; }

        [XmlElement("moic:DocumentNumber")]
        public int DocumentNumber { get; set; }
        //MimeType
        [XmlElement("moic:MimeType")]
        public string MimeType { get; set; }

    }
    public class Destination
    {

        [XmlElement("moic:Destination")]
        public string DestinationValue { get; set; }
    }
    [XmlRoot("moic:TransactionId")]
    public class TransactionId
    {

        [XmlElement("moic:TransactionId")]
        public string TransactionIdValue { get; set; }
    }
}
