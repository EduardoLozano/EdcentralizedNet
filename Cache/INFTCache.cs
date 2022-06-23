using EdcentralizedNet.Models;
using System.Threading.Tasks;

namespace EdcentralizedNet.Cache
{
    public interface INFTCache
    {
        Task<CursorPagedList<NFTAsset>> GetNFTAssetPage(string accountAddress, string pageCursor);
        Task SetNFTAssetPage(string accountAddress, string pageCursor, CursorPagedList<NFTAsset> assetList);
        Task RemoveNFTAssetPage(string accountAddress, string pageCursor);

        Task<PortfolioInformation> GetPortfolioInformation(string accountAddress);
        Task SetPortfolioInformation(string accountAddress, PortfolioInformation portfolioInformation);
        Task RemovePortfolioInformation(string accountAddress);
    }
}
