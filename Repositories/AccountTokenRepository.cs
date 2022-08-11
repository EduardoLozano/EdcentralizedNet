using EdcentralizedNet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace EdcentralizedNet.Repositories
{
    public class AccountTokenRepository : IAccountTokenRepository
    {
        private const string _tableName = "AccountTokens";
        private readonly ILogger<UserAccountRepository> _logger;
        private readonly MySqlConnection _connection;

        public AccountTokenRepository(ILogger<UserAccountRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connection = new MySqlConnection(configuration.GetSection("MySql")["ConnectionString"]);
        }

        public async Task<bool> AddAsync(AccountToken entity)
        {
            bool isAdded = false;

            entity.WalletAddress = entity.WalletAddress.ToLower();
            entity.DateCreated = DateTime.Now;

            string sql = $@"INSERT INTO {_tableName}(DateCreated,
                                                     WalletAddress,
                                                     ContractAddress,
                                                     TokenId,
                                                     TokenName,
                                                     CollectionName,
                                                     CollectionDesc,
                                                     CollectionSlug,
                                                     PurchaseTransactionHash,
                                                     PurchaseDate,
                                                     PurchasePrice,
                                                     PurchasePriceDecimals,
                                                     PurchaseUsdPrice,
                                                     ImageUrl)
                           VALUES(@DateCreated,
                                  @WalletAddress,
                                  @ContractAddress,
                                  @TokenId,
                                  @TokenName,
                                  @CollectionName,
                                  @CollectionDesc,
                                  @CollectionSlug,
                                  @PurchaseTransactionHash,
                                  @PurchaseDate,
                                  @PurchasePrice,
                                  @PurchasePriceDecimals,
                                  @PurchaseUsdPrice,
                                  @ImageUrl)";

            try
            {
                using (_connection)
                {
                    _connection.Open();

                    var result = await _connection.ExecuteAsync(sql, entity);

                    isAdded = result > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
            }

            return isAdded;
        }

        public async Task<bool> DeleteAsync(object id)
        {
            string sql = $@"DELETE FROM {_tableName}
                            WHERE AccountTokenId = @id";

            try
            {
                using (_connection)
                {
                    _connection.Open();

                    var result = await _connection.ExecuteAsync(sql, new { id });

                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
            }

            return false;
        }

        public async Task<IEnumerable<AccountToken>> GetAllAsync()
        {
            string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using (_connection)
                {
                    _connection.Open();

                    var result = await _connection.QueryAsync<AccountToken>(sql);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
            }

            return null;
        }

        public async Task<AccountToken> GetByIdAsync(object id)
        {
            string sql = $@"SELECT * FROM {_tableName} WHERE AccountTokenId = @id ";

            try
            {
                using (_connection)
                {
                    _connection.Open();

                    var result = await _connection.QuerySingleOrDefaultAsync<AccountToken>(sql, new { id });

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
            }

            return null;
        }

        public async Task<bool> UpdateAsync(AccountToken entity)
        {
            //Should not be updating the token records for now
            //Expecting to delete when token is sold
            //Expecting to insert when token is purchased
            throw new System.NotImplementedException();
        }
    }
}
