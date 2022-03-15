using System;

namespace EdcentralizedNet.Models
{
    public class NFTInformation
    {
        public string TransactionHash { get; set; }
        public string CollectionName { get; set; }
        public string TokenID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal FloorPrice { get; set; }
        public decimal ProfitLossAmount { get; set; }
        public decimal ProfitLossPercent { get; set; }
    }
}
