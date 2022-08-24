using EdcentralizedNet.Models;
using System;

namespace EdcentralizedNet.ViewModels
{
    public class AccountSummaryVM
    {

        public decimal InvestedValue { get; set; }
        public decimal ProfitLossAmount { get; set; }
        public decimal ProfitLossPercent { get; set; }
        public decimal AccountValue { get; set; }

        public AccountSummaryVM() { }

        public AccountSummaryVM(AccountSummary entity)
        {
            if (entity != null)
            {
                InvestedValue = Math.Round(entity.InvestedValue, 4);
                AccountValue = Math.Round(entity.AccountValue, 4);
                ProfitLossAmount = Math.Round(entity.AccountValue - entity.InvestedValue, 4);
                ProfitLossPercent = Math.Round((ProfitLossAmount / (InvestedValue.Equals(0) ? 1 : InvestedValue)) * 100, 2);
            }
        }
    }
}
