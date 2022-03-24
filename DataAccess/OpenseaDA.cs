using EdcentralizedNet.Cache;
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
        private readonly IOpenseaCache _cache;

        public OpenseaDA(ILogger<EtherscanDA> logger, OpenseaClient client, IOpenseaCache cache)
        {
            _logger = logger;
            _client = client;
            _cache = cache;
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
                        //Try and retrieve from cache first
                        OSStats stats = await _cache.GetStatsForCollection(collection.slug);

                        //If we could not retrieve from cache then lets request from client
                        if (stats == null)
                        {
                            //Get from client
                            stats = await _client.GetStatsForCollection(collection.slug);

                            //Update cache for next time around
                            if (stats != null)
                            {
                                await _cache.SetStatsForCollection(collection.slug, stats);
                            }

                            //Throttle calls to the API
                            Thread.Sleep(100);
                        }

                        collection.stats = stats;
                    }
                }
            }

            return collections;
        }

        public async Task<OSCollection> GetCollection(string collectionSlug)
        {
            OSCollection collection = await _client.GetCollection(collectionSlug);

            return collection;
        }
    }
}
