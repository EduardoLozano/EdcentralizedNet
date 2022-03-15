﻿using EdcentralizedNet.Helpers;
using EdcentralizedNet.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace EdcentralizedNet.HttpClients
{
    public class EtherscanClient
    {
        private static string apiKey = "F793PNEVJ81SFSFT3TGMWYEK65JHNSU7NH";
        private readonly HttpClient _httpClient;
        JsonSerializerOptions _jsonSerializerOptions;

        public EtherscanClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.etherscan.io/api");
            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.Converters.Add(new HexToLongConverter());
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
                query["apikey"] = apiKey;

                builder.Query = query.ToString();

                return await _httpClient.GetFromJsonAsync<EtherscanResponse<ERC721Transfer>>(builder.Uri);
            }
            catch(Exception ex)
            {

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
                query["apikey"] = apiKey;

                builder.Query = query.ToString();

                return await _httpClient.GetFromJsonAsync<ParityResponse<EthTransaction>>(builder.Uri, _jsonSerializerOptions);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}