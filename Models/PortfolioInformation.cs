using System;
using System.Linq;

namespace EdcentralizedNet.Models
{
    public class PortfolioInformation
    {
        public string NextPageCursor { get; set; }
        public string PrevPageCursor { get; set; }
        public decimal InvestedValue { get; set; }
        public decimal ProfitLossAmount { get; set; }
        public decimal ProfitLossPercent { get; set; }
        public NFTInformationList Tokens { get; set; }

        public PortfolioInformation() { }

        public PortfolioInformation(NFTInformationList tokens)
        {
            if (tokens != null && tokens.Any())
            {
                NextPageCursor = tokens.NextPageCursor;
                PrevPageCursor = tokens.PrevPageCursor;
                Tokens = tokens;
                InvestedValue = Math.Round(tokens.Sum(t => t.PurchasePrice), 4);
                ProfitLossAmount = Math.Round(tokens.Sum(t => t.ProfitLossAmount), 4);
                ProfitLossPercent = Math.Round((ProfitLossAmount / (InvestedValue.Equals(0) ? 1 : InvestedValue)) * 100, 4);
            }
            else
            {
                tokens = new NFTInformationList();
            }
        }
    }
}
