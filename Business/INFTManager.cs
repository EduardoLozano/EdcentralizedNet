using EdcentralizedNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdcentralizedNet.Business
{
    public interface INFTManager
    {
        Task<List<NFTInformation>> GetAllNFTForAccount(string accountAddress);
    }
}
