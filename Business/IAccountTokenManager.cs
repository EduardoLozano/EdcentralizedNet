using EdcentralizedNet.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdcentralizedNet.Business
{
    public interface IAccountTokenManager
    {
        Task<IEnumerable<AccountTokenVM>> GetAccountTokenPageAsync(string walletAddress, int pageNumber, int pageSize = 20);
    }
}
