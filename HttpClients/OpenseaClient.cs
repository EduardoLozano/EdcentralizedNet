using EdcentralizedNet.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace EdcentralizedNet.HttpClients
{
    public class OpenseaClient
    {
        private static string _apiKey;
        private readonly HttpClient _httpClient;
        JsonSerializerOptions _jsonSerializerOptions;

        public OpenseaClient(HttpClient httpClient, IConfiguration configuration)
        {
            _apiKey = configuration.GetSection("Opensea")["ApiKey"];
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Opensea")["ApiBaseAddress"]);
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            _jsonSerializerOptions = new JsonSerializerOptions();
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

                return await _httpClient.GetFromJsonAsync<OSAssetList>(builder.Uri);
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<OSEventList> GetAssetEvents(string accountAddress, string contractAddress, string tokenId, string eventType = null, string cursor = null)
        {
            try
            {
                UriBuilder builder = new UriBuilder(_httpClient.BaseAddress);
                var query = HttpUtility.ParseQueryString(builder.Query);

                query["account_address"] = accountAddress;
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

                return await _httpClient.GetFromJsonAsync<OSEventList>(builder.Uri);
            }
            catch (Exception ex)
            {

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

                return await _httpClient.GetFromJsonAsync<List<OSCollection>>(builder.Uri);
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<OSStats> GetStatsForCollection(string collectionSlug)
        {
            try
            {
                UriBuilder builder = new UriBuilder(_httpClient.BaseAddress);
                builder.Path = $"collection/{collectionSlug}/stats";

                var collectionObj = await _httpClient.GetFromJsonAsync<OSCollection>(builder.Uri);

                if (collectionObj != null)
                {
                    return collectionObj.stats;
                }
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<OSCollection> GetCollection(string collectionSlug)
        {
            try
            {
                UriBuilder builder = new UriBuilder(_httpClient.BaseAddress);
                builder.Path = $"collection/{collectionSlug}";

                return await _httpClient.GetFromJsonAsync<OSCollection>(builder.Uri);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
