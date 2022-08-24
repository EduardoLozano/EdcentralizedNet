using EdcentralizedNet.Repositories;
using EdcentralizedNet.ViewModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EdcentralizedNet.Business
{
    public class AccountSummaryManager : IAccountSummaryManager
    {
        private readonly ILogger<AccountSummaryManager> _logger;
        private readonly IAccountSummaryRepository _accountSummaryRepository;

        public AccountSummaryManager(ILogger<AccountSummaryManager> logger, IAccountSummaryRepository accountSummaryRepository)
        {
            _logger = logger;
            _accountSummaryRepository = accountSummaryRepository;
        }

        public async Task<AccountSummaryVM> GetAccountSummaryForAccount(string walletAddress)
        {
            var entity = await _accountSummaryRepository.GetByIdAsync(walletAddress);
            var viewModel = new AccountSummaryVM(entity);
            return viewModel;
        }
    }
}
