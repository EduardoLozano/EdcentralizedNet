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

        public EtherscanDA(ILogger<EtherscanDA> logger, EtherscanClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<IEnumerable<ERC721Transfer>> GetERC721TransfersForAccount(string accountAddress)
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
