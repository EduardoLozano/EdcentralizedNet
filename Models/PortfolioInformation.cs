using System;
using System.Collections.Generic;
using System.Linq;

namespace EdcentralizedNet.Models
{
    public class PortfolioInformation
    {
        public decimal InvestedValue { get; set; }
        public decimal ProfitLossAmount { get; set; }
        public decimal ProfitLossPercent { get; set; }
        public decimal AccountValue { get; set; }

        public PortfolioInformation() { }

        public PortfolioInformation(List<NFTAsset> assets)
        {
            if (assets != null && assets.Any())
            {
                InvestedValue = Math.Round(assets.Sum(t => t.PurchasePrice), 4);
                ProfitLossAmount = Math.Round(assets.Sum(t => t.ProfitLossAmount), 4);
                ProfitLossPercent = Math.Round((ProfitLossAmount / (InvestedValue.Equals(0) ? 1 : InvestedValue)) * 100, 2);
                AccountValue = InvestedValue + ProfitLossAmount;
            }
        }
    }
}
