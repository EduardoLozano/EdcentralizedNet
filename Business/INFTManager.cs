using EdcentralizedNet.Models;
using System.Threading.Tasks;

namespace EdcentralizedNet.Business
{
    public interface INFTManager
    {
        Task<CursorPagedList<NFTAsset>> GetNFTAssetPage(string accountAddress, int pageNumber, string pageCursor);
        Task<PortfolioInformation> GetPortfolioInformation(string accountAddress);
        Task<AccountStatusResponse> GetAccountStatus(string accountAddress);
    }
}
