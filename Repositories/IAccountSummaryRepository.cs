using EdcentralizedNet.Models;
using System.Threading.Tasks;

namespace EdcentralizedNet.Repositories
{
    public interface IAccountSummaryRepository
    {
        Task<AccountSummary> GetByIdAsync(object id);
    }
}
