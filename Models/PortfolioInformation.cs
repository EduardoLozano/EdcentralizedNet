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
        public List<NFTInformation> Tokens { get; set; }

        public PortfolioInformation() { }

        public PortfolioInformation(List<NFTInformation> tokens)
        {
            if (tokens != null && tokens.Any())
            {
                Tokens = tokens;
                InvestedValue = Math.Round(tokens.Sum(t => t.PurchasePrice), 4);
                ProfitLossAmount = Math.Round(tokens.Sum(t => t.ProfitLossAmount), 4);
                ProfitLossPercent = Math.Round((ProfitLossAmount / (InvestedValue.Equals(0) ? 1 : InvestedValue)) * 100, 4);
            }
        }
    }
}
