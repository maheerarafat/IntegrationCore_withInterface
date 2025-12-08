
namespace IntegrationCore.Models
{
    public class TarasolItem
    {
        public string Value { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

    }

    public class TarasolTargetItem
    {
        public GlobalIDs GlobalIDs { get; set; }
        public int? ExchangeNo { get; set; }
        public int? Exchange_no_fax { get; set; }
        public int? Exchange_no_email { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int? ExchangeStatus { get; set; }
    }

    public class GlobalIDs
    {
        public string GlobalID { get; set; }
        public string DirectParentGlobalID { get; set; }
        public string MainParentGlobalID { get; set; }
    }
}
