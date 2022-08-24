using Dapper;
using EdcentralizedNet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace EdcentralizedNet.Repositories
{
    public class AccountSummaryRepository : IAccountSummaryRepository
    {
        private readonly ILogger<AccountSummaryRepository> _logger;
        private readonly MySqlConnection _connection;

        public AccountSummaryRepository(ILogger<AccountSummaryRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connection = new MySqlConnection(configuration.GetSection("MySql")["ConnectionString"]);
        }

        public async Task<AccountSummary> GetByIdAsync(object id)
        {
            string sql = $@"SELECT SUM(a.PurchasePrice/POW(10,a.PurchasePriceDecimals)) AS InvestedValue,
	                               SUM(c.FloorPrice) AS AccountValue
                            FROM accounttokens a
                            LEFT JOIN collectionstats c ON a.collectionSlug = c.collectionSlug
                            WHERE a.WalletAddress = @WalletAddress";

            try
            {
                using (_connection)
                {
                    _connection.Open();

                    var result = await _connection.QuerySingleOrDefaultAsync<AccountSummary>(sql, new
                    {
                        WalletAddress = id.ToString(),
                    });

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
            }

            return null;
        }
    }
}
