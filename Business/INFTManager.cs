using EdcentralizedNet.Models;
using System.Threading.Tasks;

namespace EdcentralizedNet.Business
{
    public interface INFTManager
    {
        Task<NFTInformationList> GetAllNFTForAccount(string accountAddress, string pageCursor);
    }
}
