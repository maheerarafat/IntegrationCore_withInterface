

using System.Collections.Generic;

namespace IntegrationCore.Models
{
    public class ExportCorr
    {
        public string CorrUID { get; set; }
        public char Type { get; set; }
        public List<string> ExchangeNo { get; set; }
        public bool IsFirstTry { get; set; }
        
        public List<TarasolTargetItem> ResendTo { get; set; }
       
    }

    public class RejectCorr
    {
        public string CorrUID { get; set; }        
        public List<Transaction> Transactions { get; set; }
        public bool IsFirstTry { get; set; }

    }
    public class Transaction
    {
        public string ExchangeNo { get; set; }
        public string GlobalID { get; set; }
    }
    public class FaxInquiry
    {
        public string ExchangeNo { get; set; }
        public string RequestNo { get; set; }
        public string status { get; set; }
        
    }

}
