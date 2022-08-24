using EdcentralizedNet.ViewModels;
using System.Threading.Tasks;

namespace EdcentralizedNet.Business
{
    public interface IAccountSummaryManager
    {
        Task<AccountSummaryVM> GetAccountSummaryForAccount(string walletAddress);
    }
}
