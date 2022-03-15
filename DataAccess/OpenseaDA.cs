using EdcentralizedNet.HttpClients;
using EdcentralizedNet.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EdcentralizedNet.DataAccess
{
    public class OpenseaDA : IOpenseaDA
    {
        private readonly ILogger<EtherscanDA> _logger;
        private readonly OpenseaClient _client;

        public OpenseaDA(ILogger<EtherscanDA> logger, OpenseaClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<IEnumerable<OSCollection>> GetCollectionsForAccount(string accountAddress)
        {
            List<OSCollection> collections = new List<OSCollection>();

            collections = await _client.GetCollectionsForAccount(accountAddress);

            if (collections != null)
            {
                foreach (var collection in collections)
                {
                    if (!string.IsNullOrWhiteSpace(collection.slug))
                    {
                        collection.stats = await _client.GetStatsForCollection(collection.slug);

                        //Throttle calls to the API
                        Thread.Sleep(200);
                    }
                }
            }

            return collections;
        }
    }
}
