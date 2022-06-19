using EdcentralizedNet.Cache;
using EdcentralizedNet.HttpClients;
using EdcentralizedNet.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdcentralizedNet.DataAccess
{
    public class OpenseaDA : IOpenseaDA
    {
        private readonly ILogger<OpenseaDA> _logger;
        private readonly OpenseaClient _client;
        private readonly IOpenseaCache _cache;

        public OpenseaDA(ILogger<OpenseaDA> logger, OpenseaClient client, IOpenseaCache cache)
        {
            _logger = logger;
            _client = client;
            _cache = cache;
        }

        public async Task<OSAssetList> GetAssetsForAccount(string accountAddress, string cursor = null)
        {
            OSAssetList assets = await _client.GetAssetsForAccount(accountAddress, cursor);

            if (assets != null)
            {
                foreach (var asset in assets.assets)
                {
                    if (asset.collection.stats == null && !string.IsNullOrWhiteSpace(asset.collection.slug))
                    {
                        //Try and retrieve from cache first
                        OSStats stats = await _cache.GetStatsForCollection(asset.collection.slug);

                        //If we could not retrieve from cache then lets request from client
                        if (stats == null)
                        {
                            //Get from client
                            stats = await _client.GetStatsForCollection(asset.collection.slug);

                            //Update cache for next time around
                            await _cache.SetStatsForCollection(asset.collection.slug, stats);

                            //Throttle calls to the API
                            Thread.Sleep(50);
                        }

                        asset.collection.stats = stats;
                    }

                    //If there is not a last sale, lets assume it was minted and never sold
                    //Attempt to get the mint event from opensea so that we have a transaction hash
                    if (asset.last_sale == null)
                    {
                        //Try and retrieve from cache first
                        OSEvent aEvent = await _cache.GetAssetMintEvent(asset.asset_contract.address, asset.token_id);

                        if (aEvent == null)
                        {
                            OSEventList events = await _client.GetAssetEvents(accountAddress, asset.asset_contract.address, asset.token_id, "transfer");

                            //If this is a mint event, there should only be one transfer returned
                            if (events != null && events.asset_events != null && events.asset_events.Count.Equals(1))
                            {
                                aEvent = events.asset_events.First();

                                //Specify that the payment token was ETH
                                aEvent.payment_token = new OSPaymentToken()
                                {
                                    symbol = "ETH",
                                    address = "0x0000000000000000000000000000000000000000",
                                    name = "Ether",
                                    decimals = 18
                                };

                                //Update cache for next time around
                                await _cache.SetAssetMintEvent(asset.asset_contract.address, asset.token_id, aEvent);
                            }

                            //Throttle calls to the API
                            Thread.Sleep(50);
                        }

                        asset.last_sale = aEvent;
                    }
                }
            }

            return assets;
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
