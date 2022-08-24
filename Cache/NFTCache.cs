using EdcentralizedNet.Helpers;
using EdcentralizedNet.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EdcentralizedNet.Cache
{
    public class NFTCache : INFTCache
    {
        private readonly ILogger<NFTCache> _logger;
        private readonly IApplicationCache _cache;

        static string nftAssetPageKey = "NFTAssetPage|{0}{1}";
        static string portfolioInformationKey = "PortfolioInformation|{0}";

        public NFTCache(ILogger<NFTCache> logger, IApplicationCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        #region NFT Asset List
        public Task<CursorPagedList<NFTAsset>> GetNFTAssetPage(string accountAddress, int pageNumber)
        {
            string key = string.Format(nftAssetPageKey, accountAddress, pageNumber);
            return _cache.Get<CursorPagedList<NFTAsset>>(key);
        }

        public Task SetNFTAssetPage(string accountAddress, int pageNumber, CursorPagedList<NFTAsset> assetList)
        {
            string key = string.Format(nftAssetPageKey, accountAddress, pageNumber);

            //Hold pages for a sliding 10 minutes
            return _cache.Set(key, assetList, expireSliding: new TimeSpan(0, 10, 0));
        }

        public Task RemoveNFTAssetPage(string accountAddress, int pageNumber)
        {
            string key = string.Format(nftAssetPageKey, accountAddress, pageNumber);
            return _cache.Remove(key);
        }
        #endregion

        #region Portfolio Information
        public Task<AccountSummary> GetPortfolioInformation(string accountAddress)
        {
            string key = string.Format(portfolioInformationKey, accountAddress);
            return _cache.Get<AccountSummary>(key);
        }

        public Task SetPortfolioInformation(string accountAddress, AccountSummary portfolioInformation)
        {
            string key = string.Format(portfolioInformationKey, accountAddress);

            //Hold portfolio for a sliding 30 minutes
            return _cache.Set(key, portfolioInformation, expireSliding: new TimeSpan(0, 30, 0));
        }

        public Task RemovePortfolioInformation(string accountAddress)
        {
            string key = string.Format(portfolioInformationKey, accountAddress);
            return _cache.Remove(key);
        }
        #endregion
    }
}
