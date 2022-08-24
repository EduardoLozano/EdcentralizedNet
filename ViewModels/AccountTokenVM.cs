using EdcentralizedNet.Models;
using EdcentralizedNet.OpenseaModels;
using System;

namespace EdcentralizedNet.ViewModels
{
    public class AccountTokenVM
    {
        public string TransactionHash { get; set; }
        public string CollectionName { get; set; }
        public string TokenID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal FloorPrice { get; set; }
        public decimal ProfitLossAmount { get; set; }
        public decimal ProfitLossPercent { get; set; }
        public string ImageUrl { get; set; }
        public string OpenseaUrl { get; set; }

        public AccountTokenVM() { }

        public AccountTokenVM(AccountToken entity)
        {
            CollectionName = entity.CollectionName;
            TokenID = entity.TokenId;
            ImageUrl = entity.ImageUrl;
            OpenseaUrl = $"https://opensea.io/assets/ethereum/{entity.ContractAddress}/{entity.TokenId}";
            PurchasePrice = (decimal)(entity.PurchasePrice / Math.Pow(10, entity.PurchasePriceDecimals));
            PurchaseDate = entity.PurchaseDate;
            TransactionHash = entity.PurchaseTransactionHash;
            FloorPrice = entity.FloorPrice;
            ProfitLossAmount = FloorPrice - PurchasePrice;
            ProfitLossPercent = Math.Round((ProfitLossAmount / (PurchasePrice.Equals(0) ? 1 : PurchasePrice)) * 100, 2);
        }
    }
}
