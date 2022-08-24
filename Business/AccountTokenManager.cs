using EdcentralizedNet.Repositories;
using EdcentralizedNet.ViewModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdcentralizedNet.Business
{
    public class AccountTokenManager : IAccountTokenManager
    {
        private readonly ILogger<AccountTokenManager> _logger;
        private readonly IAccountTokenRepository _accountTokenRepository;

        public AccountTokenManager(ILogger<AccountTokenManager> logger, IAccountTokenRepository accountTokenRepository)
        {
            _logger = logger;
            _accountTokenRepository = accountTokenRepository;
        }

        public async Task<IEnumerable<AccountTokenVM>> GetAccountTokenPageAsync(string walletAddress, int pageNumber, int pageSize = 20)
        {
            var entities = await _accountTokenRepository.GetPageAsync(walletAddress, pageNumber, pageSize);

            var viewModels = entities.Select(e => new AccountTokenVM(e));

            return viewModels;
        }
    }
}
