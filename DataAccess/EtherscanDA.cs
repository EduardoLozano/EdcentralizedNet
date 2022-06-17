using EdcentralizedNet.Cache;
using EdcentralizedNet.HttpClients;
using EdcentralizedNet.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EdcentralizedNet.DataAccess
{
    public class EtherscanDA : IEtherscanDA
    {
        private readonly ILogger<EtherscanDA> _logger;
        private readonly EtherscanClient _client;
        private readonly IEtherscanCache _cache;

        public EtherscanDA(ILogger<EtherscanDA> logger, EtherscanClient client, IEtherscanCache cache)
        {
            _logger = logger;
            _client = client;
            _cache = cache;
        }

        public async Task<EthTransaction> GetEthTransaction(string transactionHash)
        {
            //Try and find transaction in cache first
            EthTransaction trx = await _cache.GetEthTransaction(transactionHash);

            if (trx == null)
            {
                //If we did not find the transaction in cache, lets hit the API
                ParityResponse<EthTransaction>  trxResponse = await _client.GetEthTransaction(transactionHash);

                if (trxResponse != null)
                {
                    trx = trxResponse.result;

                    //Update cache for next time around
                    await _cache.SetEthTransaction(transactionHash, trx);
                }
            }

            return trx;
        }

        public async Task<IEnumerable<ERC721Transfer>> GetERC721OwnedByAccount(string accountAddress)
        {
            IEnumerable<ERC721Transfer> transfers = new List<ERC721Transfer>();
            EtherscanResponse<ERC721Transfer> response = await _client.GetERC721TransfersForAccount(accountAddress);

            if(response.status.Equals("1"))
            {
                transfers = FilterTransfers(response.result, accountAddress);

                foreach(ERC721Transfer t in transfers)
                {
                    ParityResponse<EthTransaction> trxResponse = await _client.GetEthTransaction(t.hash);

                    if (trxResponse != null)
                    {
                        t.transaction = trxResponse.result;
                    }

                    //Throttle calls to the API
                    Thread.Sleep(500);
                }
            }

            return transfers;
        }

        private IEnumerable<ERC721Transfer> FilterTransfers(IEnumerable<ERC721Transfer> transfers, string accountAddress)
        {
            List<ERC721Transfer> finalList = new List<ERC721Transfer>();

            //Need to handle this way more efficiently
            foreach(ERC721Transfer t in transfers)
            {
                if(t.to.Equals(accountAddress, StringComparison.InvariantCultureIgnoreCase))
                {
                    finalList.Add(t);
                }
                else
                {
                    finalList.RemoveAll(f => f.blockNumber < t.blockNumber &&
                                             f.contractAddress.Equals(t.contractAddress) &&
                                             f.tokenID.Equals(t.tokenID));
                }
            }

            return finalList;
        }
    }
}
