using EdcentralizedNet.Models;
using System.Threading.Tasks;

namespace EdcentralizedNet.Repositories
{
    public interface IUserAccountRepository : IBaseRepository<UserAccount>
    {
        Task<bool> Exists(object id);
    }
}
