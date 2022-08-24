using EdcentralizedNet.Controllers;
using EdcentralizedNet.Helpers;
using EdcentralizedNet.Models;
using EdcentralizedNet.ViewModels;
using System.Threading.Tasks;

namespace EdcentralizedNet.Business
{
    public interface INFTManager
    {
        Task<CursorPagedList<NFTAsset>> GetNFTAssetPage(string accountAddress, int pageNumber, string pageCursor);
        Task<AccountSummary> GetPortfolioInformation(string accountAddress);
        Task<AccountStatusVM> GetAccountStatus(string accountAddress);
    }
}
