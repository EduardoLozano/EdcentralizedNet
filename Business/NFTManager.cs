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

        public async Task<List<NFTInformation>> GetAllNFTForAccount(string accountAddress)
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
