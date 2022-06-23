using EdcentralizedNet.Cache;
using EdcentralizedNet.DataAccess;
using EdcentralizedNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdcentralizedNet.Business
{
    public class NFTManager : INFTManager
    {
        private readonly IEtherscanDA _etherscanDA;
        private readonly IOpenseaDA _openseaDA;
        private readonly INFTCache _nftCache;

        public NFTManager(IEtherscanDA etherscanDA, IOpenseaDA openseaDA, INFTCache nftCache)
        {
            _etherscanDA = etherscanDA;
            _openseaDA = openseaDA;
            _nftCache = nftCache;
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
                OSAssetList assets = await _openseaDA.GetAssetsForAccount(accountAddress, pageCursor);

                if (assets != null)
                {
                    //Map into GUI model
                    foreach (OSAsset asset in assets.assets)
                    {
                        //If the last sale does not have a total price, then we assume it is a mint event
                        //Attempt to get the transaction from etherscan for the mint price
                        if (asset.last_sale != null && asset.last_sale.transaction != null && asset.last_sale.total_price == null)
                        {
                            EthTransaction trx = await _etherscanDA.GetEthTransaction(asset.last_sale.transaction.transaction_hash);

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
                    //TODO: Currently storing first page with null cursor on initial load and another cursor on going previous
                    await _nftCache.SetNFTAssetPage(accountAddress, pageNumber, assetList);
                }
            }

            return assetList;
        }

        public async Task<PortfolioInformation> GetPortfolioInformation(string accountAddress)
        {
            //Try and retrieve from cache first as this a very time consuming function
            PortfolioInformation portfolio = await _nftCache.GetPortfolioInformation(accountAddress);

            if(portfolio == null)
            {
                //If we did not find information in cache, retrieve all assets so that we can calculate values
                List<NFTAsset> assets = await GetAllNFTAssets(accountAddress);

                portfolio = new PortfolioInformation(assets);

                //Store in cache so we dont have to keep making this expensive calculation
                await _nftCache.SetPortfolioInformation(accountAddress, portfolio);
            }

            return portfolio;
        }

        private async Task<List<NFTAsset>> GetAllNFTAssets(string accountAddress)
        {
            int pageNumber = 1;
            List<NFTAsset> allAssets = new List<NFTAsset>();
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
        public async Task<List<NFTAsset>> GetAllNFTForAccount2(string accountAddress)
        {
            List<NFTAsset> nfi = new List<NFTAsset>();

            //First try and get all NFT collection info from OpenSea
            var osCollections = await _openseaDA.GetCollectionsForAccount(accountAddress);

            if (osCollections != null && osCollections.Any())
            {
                //Now lets get all the NFTs owned by address from etherscan
                var tokensOwned = await _etherscanDA.GetERC721OwnedByAccount(accountAddress);

                if (tokensOwned != null && tokensOwned.Any())
                {
                    //Lets manipulate data as needed
                    nfi = JoinNFTData(tokensOwned, osCollections);
                }
            }

            return nfi;
        }

        private List<NFTAsset> JoinNFTData(IEnumerable<ERC721Transfer> tokensOwned, IEnumerable<OSCollection> osCollections)
        {
            List<NFTAsset> nfi = new List<NFTAsset>();

            foreach (var o in tokensOwned)
            {
                NFTAsset nft = new NFTAsset();
                var col = osCollections.FirstOrDefault(oc => oc.primary_asset_contracts.Any(a => a.address.Equals(o.contractAddress)));

                if (col != null && !col.name.Equals("ENS: Ethereum Name Service"))
                {
                    nft.TransactionHash = o.hash;
                    nft.CollectionName = col.name;
                    nft.TokenID = o.tokenID;
                    nft.PurchaseDate = DateTimeOffset.FromUnixTimeSeconds(o.timeStamp).DateTime;
                    nft.PurchasePrice = (decimal)o.transaction.value / 1000000000000000000;
                    nft.FloorPrice = col.stats != null ? col.stats.floor_price : 0;
                    nft.ProfitLossAmount = nft.FloorPrice - nft.PurchasePrice;
                    nft.ProfitLossPercent = Math.Round((nft.ProfitLossAmount / (nft.PurchasePrice.Equals(0) ? 1 : nft.PurchasePrice)) * 100, 4);

                    nfi.Add(nft);
                }
            }

            return nfi;
        }
        #endregion
    }
}
