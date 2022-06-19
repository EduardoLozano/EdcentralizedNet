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

        public NFTManager(IEtherscanDA etherscanDA, IOpenseaDA openseaDA)
        {
            _etherscanDA = etherscanDA;
            _openseaDA = openseaDA;
        }

        public async Task<NFTInformationList> GetAllNFTForAccount(string accountAddress, string pageCursor)
        {
            NFTInformationList nfi = new NFTInformationList();

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

                    nfi.Add(new NFTInformation(asset));
                }

                //Set cursors for next and previous pages
                //Setting the backwards because when in asc order, opensea returns previous cursor as the next page
                nfi.NextPageCursor = assets.previous;
                nfi.PrevPageCursor = assets.next;

                //Reverse list because opensea simply returns the pages in reverse order when requested in asc order
                nfi.Reverse();
            }

            return nfi;
        }

        public async Task<List<NFTInformation>> GetAllNFTForAccount2(string accountAddress)
        {
            List<NFTInformation> nfi = new List<NFTInformation>();

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

        private List<NFTInformation> JoinNFTData(IEnumerable<ERC721Transfer> tokensOwned, IEnumerable<OSCollection> osCollections)
        {
            List<NFTInformation> nfi = new List<NFTInformation>();

            foreach (var o in tokensOwned)
            {
                NFTInformation nft = new NFTInformation();
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
    }
}
