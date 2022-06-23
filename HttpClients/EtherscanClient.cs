using EdcentralizedNet.Cache;
using EdcentralizedNet.Helpers;
using EdcentralizedNet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EdcentralizedNet.HttpClients
{
    public class EtherscanClient
    {
        private readonly ILogger<EtherscanClient> _logger;
        private static string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly IRateLimitCache _rateLimitCache;
        JsonSerializerOptions _jsonSerializerOptions;

        public EtherscanClient(HttpClient httpClient, IConfiguration configuration, IRateLimitCache rateLimitCache, ILogger<EtherscanClient> logger)
        {
            _apiKey = configuration.GetSection("Etherscan")["ApiKey"];
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Etherscan")["ApiBaseAddress"]);
            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.Converters.Add(new HexToLongConverter());
            _rateLimitCache = rateLimitCache;
            _logger = logger;
        }

        public async Task<EtherscanResponse<ERC721Transfer>> GetERC721TransfersForAccount(string accountAddress)
        {
            try
            {
                UriBuilder builder = new UriBuilder(_httpClient.BaseAddress);
                var query = HttpUtility.ParseQueryString(builder.Query);

                query["module"] = "account";
                query["action"] = "tokennfttx";
                query["address"] = accountAddress;
                query["page"] = "1";
                query["offset"] = "100";
                query["startblock"] = "0";
                query["endblock"] = "27025780";
                query["sort"] = "asc";
                query["apikey"] = _apiKey;

                builder.Query = query.ToString();

                EtherscanResponse<ERC721Transfer> result = new EtherscanResponse<ERC721Transfer>();

                if (CanRequestEtherscan())
                {
                    result = await _httpClient.GetFromJsonAsync<EtherscanResponse<ERC721Transfer>>(builder.Uri);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        public async Task<ParityResponse<EthTransaction>> GetEthTransaction(string transactionHash)
        {
            try
            {
                UriBuilder builder = new UriBuilder(_httpClient.BaseAddress);
                var query = HttpUtility.ParseQueryString(builder.Query);

                query["module"] = "proxy";
                query["action"] = "eth_getTransactionByHash";
                query["txhash"] = transactionHash;
                query["apikey"] = _apiKey;

                builder.Query = query.ToString();

                ParityResponse<EthTransaction> result = new ParityResponse<EthTransaction>();

                if (CanRequestEtherscan())
                {
                    result = await _httpClient.GetFromJsonAsync<ParityResponse<EthTransaction>>(builder.Uri, _jsonSerializerOptions);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"TransactionHash: {transactionHash} | {ex.Message}");
            }

            return null;
        }

        private bool CanRequestEtherscan()
        {
            int retryCounter = 5;
            bool isAllowed = _rateLimitCache.CanRequestEtherscan();

            while (!isAllowed && retryCounter > 0)
            {
                Thread.Sleep(40);
                isAllowed = _rateLimitCache.CanRequestEtherscan();
                retryCounter--;
            }

            return isAllowed;
        }
    }
}
