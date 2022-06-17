﻿using System;

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
        public string ImageUrl { get; set; }

        public NFTInformation() { }

        public NFTInformation(OSAsset asset)
        {
            CollectionName = asset.collection.name;
            TokenID = asset.token_id;
            ImageUrl = asset.image_thumbnail_url;

            if (asset.last_sale != null)
            {
                double totalPrice;

                if (double.TryParse(asset.last_sale.total_price, out totalPrice))
                {
                    PurchasePrice = (decimal)((totalPrice) / Math.Pow(10, asset.last_sale.payment_token.decimals));
                }

                DateTime purchaseDate;
                if(DateTime.TryParse(asset.last_sale.event_timestamp, out purchaseDate))
                {
                    PurchaseDate = purchaseDate;
                }

                if(asset.last_sale.transaction != null)
                {
                    TransactionHash = asset.last_sale.transaction.transaction_hash;
                }
            }

            FloorPrice = asset.collection.stats != null ? asset.collection.stats.floor_price : 0;
            ProfitLossAmount = FloorPrice - PurchasePrice;
            ProfitLossPercent = Math.Round((ProfitLossAmount / (PurchasePrice.Equals(0) ? 1 : PurchasePrice)) * 100, 4);
        }
    }
}
