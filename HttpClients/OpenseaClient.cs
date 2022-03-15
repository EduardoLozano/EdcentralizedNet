using EdcentralizedNet.Models;
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
        private static string apiKey = "";
        private readonly HttpClient _httpClient;
        JsonSerializerOptions _jsonSerializerOptions;

        public OpenseaClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.opensea.io/api/v1");
            _jsonSerializerOptions = new JsonSerializerOptions();
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
    }
}
