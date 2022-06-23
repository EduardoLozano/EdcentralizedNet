using EdcentralizedNet.Models;
using System.Threading.Tasks;

namespace EdcentralizedNet.Cache
{
    public interface INFTCache
    {
        Task<CursorPagedList<NFTAsset>> GetNFTAssetPage(string accountAddress, int pageNumber);
        Task SetNFTAssetPage(string accountAddress, int pageNumber, CursorPagedList<NFTAsset> assetList);
        Task RemoveNFTAssetPage(string accountAddress, int pageNumber);

        Task<PortfolioInformation> GetPortfolioInformation(string accountAddress);
        Task SetPortfolioInformation(string accountAddress, PortfolioInformation portfolioInformation);
        Task RemovePortfolioInformation(string accountAddress);
    }
}
