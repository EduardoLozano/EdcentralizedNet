using EdcentralizedNet.Helpers;
using EdcentralizedNet.Models;
using System.Threading.Tasks;

namespace EdcentralizedNet.Cache
{
    public interface INFTCache
    {
        Task<CursorPagedList<NFTAsset>> GetNFTAssetPage(string accountAddress, int pageNumber);
        Task SetNFTAssetPage(string accountAddress, int pageNumber, CursorPagedList<NFTAsset> assetList);
        Task RemoveNFTAssetPage(string accountAddress, int pageNumber);

        Task<AccountSummary> GetPortfolioInformation(string accountAddress);
        Task SetPortfolioInformation(string accountAddress, AccountSummary portfolioInformation);
        Task RemovePortfolioInformation(string accountAddress);
    }
}
