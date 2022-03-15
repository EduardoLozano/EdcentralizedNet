using EdcentralizedNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdcentralizedNet.DataAccess
{
    public interface IEtherscanDA
    {
        Task<IEnumerable<ERC721Transfer>> GetERC721TransfersForAccount(string accountAddress);
    }
}
