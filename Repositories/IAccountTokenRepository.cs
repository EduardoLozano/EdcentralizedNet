using EdcentralizedNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdcentralizedNet.Repositories
{
    public interface IAccountTokenRepository : IBaseRepository<AccountToken>
    {
        Task<IEnumerable<AccountToken>> GetPageAsync(string walletAddress, int pageNumber, int pageSize = 20);
    }
}
