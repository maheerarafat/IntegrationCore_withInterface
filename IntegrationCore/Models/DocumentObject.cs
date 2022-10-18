
using System.Collections.Generic;


namespace IntegrationCore.Models
{
    public class DocumentObject
    {
       
        public TarasolItem Subject { get; set; }
        public TarasolItem ExternalRefNumber { get; set; }
        public TarasolItem Notes { get; set; }
        public TarasolItem CreationDate { get; set; }
        public TarasolItem DBCreationDate { get; set; }

        public TarasolItem Body { get; set; }
        
        public TarasolItem PriorityID { get; set; }
        public TarasolItem ImportanceID { get; set; }
        public TarasolItem SecrecyID { get; set; }
        public Classification ClassificationID { get; set; }
        public TarasolItem ReferenceNo { get; set; }
        public List<TarasolTargetItem> AddressTo { get; set; }
       // public TarasolItem FromDepID { get; set; }
        public TarasolTargetItem PostFromID { get; set; }
        public TarasolItem PostFromType { get; set; }
        public TarasolItem PostToType { get; set; }
        public List<TarasolItem> Variables { get; set; }
       public TarasolItem MessageTypeID { get; set; }
        //public List<Attachment> Files { get; set; }
    }
}
