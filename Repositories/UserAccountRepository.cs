using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using Dapper;
using System.Threading.Tasks;
using EdcentralizedNet.Models;
using System;
using System.Data;

namespace EdcentralizedNet.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly ILogger<UserAccountRepository> _logger;
        private readonly MySqlConnection _connection;

        public UserAccountRepository(ILogger<UserAccountRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connection = new MySqlConnection(configuration.GetSection("MySql")["ConnectionString"]);
        }

        public async Task<bool> AddAsync(UserAccount entity)
        {
            bool isAdded = false;

            //Only add account if it does not already exist
            bool exists = await Exists(entity.WalletAddress);

            if (!exists)
            {

                entity.DateCreated = DateTime.Now;

                string sql = @"INSERT INTO UserAccounts(WalletAddress,DateCreated)
                           VALUES(@WalletAddress,@DateCreated)";

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
            }
            else
            {
                isAdded = true;
            }

            return isAdded;
        }

        public async Task<bool> DeleteAsync(object id)
        {
            string sql = @"DELETE FROM UserAccounts
                           WHERE WalletAddress = @id";

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

        public async Task<bool> Exists(object id)
        {
            string sql = @"SELECT EXISTS(SELECT 1 FROM UserAccounts
                           WHERE WalletAddress = @id)";

            try
            {
                using (_connection)
                {
                    _connection.Open();

                    var result = await _connection.ExecuteScalarAsync<bool>(sql, new { id });

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
            }

            return false;
        }

        public async Task<IEnumerable<UserAccount>> GetAllAsync()
        {
            string sql = @"SELECT * FROM UserAccounts";

            try
            {
                using (_connection)
                {
                    _connection.Open();

                    var result = await _connection.QueryAsync<UserAccount>(sql);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
            }

            return null;
        }

        public async Task<UserAccount> GetByIdAsync(object id)
        {
            string sql = @"SELECT * FROM UserAccounts WHERE WalletAddress = @id ";

            try
            {
                using (_connection)
                {
                    _connection.Open();

                    var result = await _connection.QuerySingleOrDefaultAsync<UserAccount>(sql, new { id });

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
            }

            return null;
        }

        public async Task<bool> UpdateAsync(UserAccount entity)
        {
            //We currently should not be updating User Accounts
            //Wallet Address and Date created should only be set on Add
            return true;
        }
    }
}
