using System;

namespace EdcentralizedNet.Models
{
    public class AccountToken
    {
        public int AccountTokenId { get; set; }
        public DateTime DateCreated { get; set; }
        public string WalletAddress { get; set; }
        public string ContractAddress { get; set; }
        public string TokenId { get; set; }
        public string TokenName { get; set; }
        public string CollectionName { get; set; }
        public string CollectionDesc { get; set; }
        public string CollectionSlug { get; set; }
        public string PurchaseTransactionHash { get; set; }
        public DateTime PurchaseDate { get; set; }
        public long PurchasePrice { get; set; }
        public int PurchasePriceDecimals { get; set; }
        public decimal PurchaseUsdPrice { get; set; }
        public string ImageUrl { get; set; }

        public AccountToken() { }

        public AccountToken(string walletAddress, OSAsset asset)
        {
            WalletAddress = walletAddress;
            ContractAddress = asset.asset_contract.address;
            TokenId = asset.token_id;
            CollectionName = asset.collection.name;
            CollectionDesc = asset.collection.description;
            CollectionSlug = asset.collection.slug;
            TokenId = asset.token_id;
            ImageUrl = asset.image_thumbnail_url;
            TokenName = asset.name ?? $"{CollectionName} #{TokenId}";

            if (asset.last_sale != null)
            {
                long totalPrice;

                if (long.TryParse(asset.last_sale.total_price, out totalPrice))
                {
                    PurchasePrice = totalPrice;
                    PurchasePriceDecimals = asset.last_sale.payment_token.decimals;
                }

                DateTime purchaseDate;
                if (DateTime.TryParse(asset.last_sale.event_timestamp, out purchaseDate))
                {
                    PurchaseDate = purchaseDate;
                }

                if (asset.last_sale.transaction != null)
                {
                    PurchaseTransactionHash = asset.last_sale.transaction.transaction_hash;
                }
            }
        }
    }
}
