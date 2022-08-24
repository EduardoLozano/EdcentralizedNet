using EdcentralizedNet.Cache;
using EdcentralizedNet.EtherscanModels;
using EdcentralizedNet.Helpers;
using EdcentralizedNet.Models;
using EdcentralizedNet.OpenseaModels;
using EdcentralizedNet.Repositories;
using EdcentralizedNet.ViewModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdcentralizedNet.Business
{
    public class NFTManager : INFTManager
    {
        private readonly ILogger<NFTManager> _logger;
        private readonly IEtherscanManager _etherscanManager;
        private readonly IOpenseaManager _openseaManager;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IAccountTokenRepository _accountTokenRepository;
        private readonly ICollectionStatsRepository _collectionStatsRepository;
        private readonly INFTCache _nftCache;

        public NFTManager(ILogger<NFTManager> logger, IEtherscanManager etherscanManager, IOpenseaManager openseaManager,
                          IUserAccountRepository userAccountRepository, IAccountTokenRepository accountTokenRepository,
                          ICollectionStatsRepository collectionStatsRepository, INFTCache nftCache)
        {
            _logger = logger;
            _etherscanManager = etherscanManager;
            _openseaManager = openseaManager;
            _userAccountRepository = userAccountRepository;
            _accountTokenRepository = accountTokenRepository;
            _collectionStatsRepository = collectionStatsRepository;
            _nftCache = nftCache;
        }

        public async Task<AccountStatusVM> GetAccountStatus(string accountAddress)
        {
            AccountStatusVM response = new AccountStatusVM(accountAddress);
            UserAccount account = await _userAccountRepository.GetByIdAsync(accountAddress);

            if (account == null)
            {
                //If account does not exist yet then lets assume this is a new user
                //Lets create the account and fire off their initial load
                account = new UserAccount() { WalletAddress = accountAddress };
                await _userAccountRepository.AddAsync(account);

                //Let this keep running in the background
                InitialAccountLoad(accountAddress);

                response.IsLoaded = false;
                response.Message = "Welcome! It seems you are new here. Please be patient with us while we do an initial load for your account.";
            }
            else if (account.IsLoaded == false)
            {
                //If we did find an account but it still is not fully loaded
                //Lets let our user know we are still loading all of their data
                response.IsLoaded = false;
                response.Message = "Still loading and calculating your portfolio! Please be patient with us while we do an initial load for your account.";
            }
            else
            {
                response.IsLoaded = true;
            }

            return response;
        }

        public async Task<CursorPagedList<NFTAsset>> GetNFTAssetPage(string accountAddress, int pageNumber, string pageCursor = null)
        {
            //Check if we have this page cached already
            //Chached with page number because cursors vary and cause duplicate storage
            //All pages held for a short amount of time so that our results dont get too stale
            CursorPagedList<NFTAsset> assetList = await _nftCache.GetNFTAssetPage(accountAddress, pageNumber);

            if (assetList == null)
            {
                //Not found in cache so lets start fresh
                assetList = new CursorPagedList<NFTAsset>();

                //Get all assets from OpenSea
                OSAssetList assets = await _openseaManager.GetAssetPageForAccount(accountAddress, pageCursor);

                if (assets != null)
                {
                    //Map into GUI model
                    foreach (OSAsset asset in assets.assets)
                    {
                        //If the last sale does not have a total price, then we assume it is a mint event
                        //Attempt to get the transaction from etherscan for the mint price
                        if (asset.last_sale != null && asset.last_sale.transaction != null && asset.last_sale.total_price == null)
                        {
                            EtherscanTransaction trx = await _etherscanManager.GetEthTransaction(asset.last_sale.transaction.transaction_hash);

                            if (trx != null)
                            {
                                asset.last_sale.total_price = trx.value.ToString();
                            }
                        }

                        assetList.DataList.Add(new NFTAsset(asset));
                    }

                    assetList.NextPageCursor = assets.next;
                    assetList.PrevPageCursor = assets.previous;

                    //Store this page in cache for future use
                    await _nftCache.SetNFTAssetPage(accountAddress, pageNumber, assetList);
                }
            }

            return assetList;
        }

        public async Task<AccountSummary> GetPortfolioInformation(string accountAddress)
        {
            //Try and retrieve from cache first as this a very time consuming function
            AccountSummary portfolio = await _nftCache.GetPortfolioInformation(accountAddress);

            if (portfolio == null)
            {
                //If we did not find information in cache, retrieve all assets so that we can calculate values
                System.Collections.Generic.List<NFTAsset> assets = await GetAllNFTAssets(accountAddress);

                //Temporary
                portfolio = new AccountSummary();

                //Store in cache so we dont have to keep making this expensive calculation
                await _nftCache.SetPortfolioInformation(accountAddress, portfolio);
            }

            return portfolio;
        }

        private async Task InitialAccountLoad(string accountAddress)
        {
            //Load all assets for account from Opensea
            System.Collections.Generic.List<OSAsset> assets = await _openseaManager.GetAllAssetsForAccount(accountAddress);

            if (assets != null)
            {
                //Maintain a list of all collection stats that need saving
                //Doing this so that we do not update the same collection over and over again
                Dictionary<string, OSStats> collectionStats = new Dictionary<string, OSStats>();

                foreach (OSAsset asset in assets)
                {
                    //If the last sale does not have a total price, then we assume it is a mint event
                    //Attempt to get the transaction from etherscan for the mint price
                    if (asset.last_sale != null && asset.last_sale.transaction != null && asset.last_sale.total_price == null)
                    {
                        EtherscanTransaction trx = await _etherscanManager.GetEthTransaction(asset.last_sale.transaction.transaction_hash);

                        if (trx != null)
                        {
                            asset.last_sale.total_price = trx.value.ToString();
                        }
                    }

                    //Map into DB model and save
                    AccountToken entity = new AccountToken(accountAddress, asset);
                    bool wasAdded = await _accountTokenRepository.AddAsync(entity);

                    //Include token's collection for saving if we haven't already
                    if(asset.collection != null && asset.collection.stats != null && !collectionStats.ContainsKey(asset.collection.slug))
                    {
                        collectionStats.Add(asset.collection.slug, asset.collection.stats);
                    }

                    if (!wasAdded)
                    {
                        _logger.LogError("Unable to save token. Wallet: {0}, Contract: {1}, TokenId: {2}", accountAddress, entity.ContractAddress, entity.TokenId);
                    }
                }

                //Save any collection stats that we found
                foreach(var stat in collectionStats)
                {
                    //Save collection stats to DB
                    CollectionStats stats = new CollectionStats(stat.Key, stat.Value);
                    bool statsAdded = await _collectionStatsRepository.AddAsync(stats);

                    if (!statsAdded)
                    {
                        _logger.LogError("Unable to save collection stats. Slug: {0}", stat.Key);
                    }
                }

                //Update account as loaded
                UserAccount account = new UserAccount() { WalletAddress = accountAddress, IsLoaded = true };
                await _userAccountRepository.UpdateAsync(account);
            }
        }

        private async Task<System.Collections.Generic.List<NFTAsset>> GetAllNFTAssets(string accountAddress)
        {
            int pageNumber = 1;
            System.Collections.Generic.List<NFTAsset> allAssets = new System.Collections.Generic.List<NFTAsset>();
            CursorPagedList<NFTAsset> assetPage = new CursorPagedList<NFTAsset>();

            do
            {
                assetPage = await GetNFTAssetPage(accountAddress, pageNumber, assetPage.NextPageCursor);

                if (assetPage != null)
                {
                    allAssets.AddRange(assetPage.DataList);
                    pageNumber++;
                }
            }
            while (assetPage != null && !string.IsNullOrEmpty(assetPage.NextPageCursor));

            return allAssets;
        }

        #region NOT USED | Getting NFT Assets using Etherscan
        //public async Task<List<NFTAsset>> GetAllNFTForAccount2(string accountAddress)
        //{
        //    List<NFTAsset> nfi = new List<NFTAsset>();

        //    //First try and get all NFT collection info from OpenSea
        //    var osCollections = await _openseaManager.GetCollectionsForAccount(accountAddress);

        //    if (osCollections != null && osCollections.Any())
        //    {
        //        //Now lets get all the NFTs owned by address from etherscan
        //        var tokensOwned = await _etherscanManager.GetERC721OwnedByAccount(accountAddress);

        //        if (tokensOwned != null && tokensOwned.Any())
        //        {
        //            //Lets manipulate data as needed
        //            nfi = JoinNFTData(tokensOwned, osCollections);
        //        }
        //    }

        //    return nfi;
        //}

        //private List<NFTAsset> JoinNFTData(IEnumerable<ERC721Transfer> tokensOwned, IEnumerable<OSCollection> osCollections)
        //{
        //    List<NFTAsset> nfi = new List<NFTAsset>();

        //    foreach (var o in tokensOwned)
        //    {
        //        NFTAsset nft = new NFTAsset();
        //        var col = osCollections.FirstOrDefault(oc => oc.primary_asset_contracts.Any(a => a.address.Equals(o.contractAddress)));

        //        if (col != null && !col.name.Equals("ENS: Ethereum Name Service"))
        //        {
        //            nft.TransactionHash = o.hash;
        //            nft.CollectionName = col.name;
        //            nft.TokenID = o.tokenID;
        //            nft.PurchaseDate = DateTimeOffset.FromUnixTimeSeconds(o.timeStamp).DateTime;
        //            nft.PurchasePrice = (decimal)o.transaction.value / 1000000000000000000;
        //            nft.FloorPrice = col.stats != null && col.stats.floor_price.HasValue ? col.stats.floor_price.Value : 0;
        //            nft.ProfitLossAmount = nft.FloorPrice - nft.PurchasePrice;
        //            nft.ProfitLossPercent = Math.Round((nft.ProfitLossAmount / (nft.PurchasePrice.Equals(0) ? 1 : nft.PurchasePrice)) * 100, 4);

        //            nfi.Add(nft);
        //        }
        //    }

        //    return nfi;
        //}
        #endregion
    }
}
