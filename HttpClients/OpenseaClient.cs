using EdcentralizedNet.Cache;
using EdcentralizedNet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EdcentralizedNet.HttpClients
{
    public class OpenseaClient
    {
        private readonly ILogger<OpenseaClient> _logger;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly IRateLimitCache _rateLimitCache;
        JsonSerializerOptions _jsonSerializerOptions;

        public OpenseaClient(HttpClient httpClient, IConfiguration configuration, IRateLimitCache rateLimitCache, ILogger<OpenseaClient> logger)
        {
            _apiKey = configuration.GetSection("Opensea")["ApiKey"];
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Opensea")["ApiBaseAddress"]);
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            _jsonSerializerOptions = new JsonSerializerOptions();
            _logger = logger;
            _rateLimitCache = rateLimitCache;
        }

        public async Task<OSAssetList> GetAssetsForAccount(string accountAddress, string cursor = null)
        {
            try
            {
                UriBuilder builder = new UriBuilder(_httpClient.BaseAddress);
                var query = HttpUtility.ParseQueryString(builder.Query);

                query["owner"] = accountAddress;
                query["order_direction"] = "asc";
                query["limit"] = "20"; //Currently defaults to 20, capped at 50

                //If requesting next or previous page, cursor should be passed
                if(!string.IsNullOrWhiteSpace(cursor))
                {
                    query["cursor"] = cursor;
                }

                builder.Path = "assets";
                builder.Query = query.ToString();

                OSAssetList result = new OSAssetList();

                if (CanRequestOpenSea())
                {
                    result = await _httpClient.GetFromJsonAsync<OSAssetList>(builder.Uri);
                }

                //When in asc order, opensea returns the pages in asc order, instead of all items in asc order
                //Since the default is desc order, the most recent item ends up on the first page but last on the page
                //In addition, the next/prev cursors are also in reverse order. The first page contains a prev cursor instead of next
                var tempCursor = result.next;
                result.next = result.previous;
                result.previous = tempCursor;

                if(result.assets != null && result.assets.Any())
                {
                    result.assets.Reverse();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        public async Task<OSEventList> GetAssetEvents(string accountAddress, string contractAddress, string tokenId, string eventType = null, string cursor = null)
        {
            try
            {
                UriBuilder builder = new UriBuilder(_httpClient.BaseAddress);
                var query = HttpUtility.ParseQueryString(builder.Query);

                //Do not filter by account address anymore, it seems Opensea filters using the "from address" only
                //query["account_address"] = accountAddress;
                query["asset_contract_address"] = contractAddress;
                query["token_id"] = tokenId;

                //If requesting only a certain type of event then eventType should be passed
                if(!string.IsNullOrWhiteSpace(eventType))
                {
                    query["event_type"] = eventType;
                }

                //If requesting next or previous page, cursor should be passed
                if (!string.IsNullOrWhiteSpace(cursor))
                {
                    query["cursor"] = cursor;
                }

                builder.Path += "/events";
                builder.Query = query.ToString();

                OSEventList result = new OSEventList();

                if(CanRequestOpenSea())
                {
                    result = await _httpClient.GetFromJsonAsync<OSEventList>(builder.Uri);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        public async Task<List<OSCollection>> GetCollectionsForAccount(string accountAddress)
        {
            try
            {
                UriBuilder builder = new UriBuilder(_httpClient.BaseAddress);
                var query = HttpUtility.ParseQueryString(builder.Query);

                query["asset_owner"] = accountAddress;
                query["offset"] = "0";
                query["limit"] = "300";

                builder.Path = "collections";
                builder.Query = query.ToString();

                List<OSCollection> result = new List<OSCollection>();

                if(CanRequestOpenSea())
                {
                    result = await _httpClient.GetFromJsonAsync<List<OSCollection>>(builder.Uri);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        public async Task<OSStats> GetStatsForCollection(string collectionSlug)
        {
            try
            {
                UriBuilder builder = new UriBuilder(_httpClient.BaseAddress);
                builder.Path = $"collection/{collectionSlug}/stats";

                OSCollection result = new OSCollection();

                if(CanRequestOpenSea())
                {
                    result = await _httpClient.GetFromJsonAsync<OSCollection>(builder.Uri);
                }

                if (result != null)
                {
                    return result.stats;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CollectionSlug : {collectionSlug} | {ex.Message}");
            }

            return null;
        }

        public async Task<OSCollection> GetCollection(string collectionSlug)
        {
            try
            {
                UriBuilder builder = new UriBuilder(_httpClient.BaseAddress);
                builder.Path = $"collection/{collectionSlug}";

                OSCollection result = new OSCollection();

                if(CanRequestOpenSea())
                {
                    result = await _httpClient.GetFromJsonAsync<OSCollection>(builder.Uri);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        private bool CanRequestOpenSea()
        {
            int retryCounter = 5;
            bool isAllowed = _rateLimitCache.CanRequestOpensea();

            while(!isAllowed && retryCounter > 0)
            {
                Thread.Sleep(50);
                isAllowed = _rateLimitCache.CanRequestOpensea();
                retryCounter--;
            }

            return isAllowed;
        }
    }
}
