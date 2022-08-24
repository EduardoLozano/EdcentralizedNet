using Dapper;
using EdcentralizedNet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdcentralizedNet.Repositories
{
    public class CollectionStatsRepository : ICollectionStatsRepository
    {
        private const string _tableName = "CollectionStats";
        private readonly ILogger<CollectionStatsRepository> _logger;
        private readonly MySqlConnection _connection;

        public CollectionStatsRepository(ILogger<CollectionStatsRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connection = new MySqlConnection(configuration.GetSection("MySql")["ConnectionString"]);
        }

        public async Task<bool> AddAsync(CollectionStats entity)
        {
            bool isAdded = false;

            //Only add collection stats if they do not already exist
            bool exists = await Exists(entity.CollectionSlug);

            if (!exists)
            {
                entity.DateCreated = DateTime.Now;

                string sql = $@"INSERT INTO {_tableName}(CollectionSlug,FloorPrice,DateCreated)
                                VALUES(@CollectionSlug,@FloorPrice,@DateCreated)";

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
                //If collection stats already exist, lets run an update
                isAdded = await UpdateAsync(entity);
            }

            return isAdded;
        }

        public async Task<bool> DeleteAsync(object id)
        {
            string sql = $@"DELETE FROM {_tableName}
                            WHERE CollectionSlug = @id";

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
            string sql = $@"SELECT EXISTS(SELECT 1 FROM {_tableName}
                            WHERE CollectionSlug = @id)";

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

        public async Task<IEnumerable<CollectionStats>> GetAllAsync()
        {
            string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using (_connection)
                {
                    _connection.Open();

                    var result = await _connection.QueryAsync<CollectionStats>(sql);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
            }

            return null;
        }

        public async Task<CollectionStats> GetByIdAsync(object id)
        {
            string sql = $@"SELECT * FROM {_tableName} WHERE CollectionSlug = @id ";

            try
            {
                using (_connection)
                {
                    _connection.Open();

                    var result = await _connection.QuerySingleOrDefaultAsync<CollectionStats>(sql, new { id });

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
            }

            return null;
        }

        public async Task<bool> UpdateAsync(CollectionStats entity)
        {
            bool isUpdated = false;
            entity.DateUpdated = DateTime.Now;

            string sql = $@"UPDATE {_tableName}
                            SET FloorPrice = @FloorPrice, 
                                DateUpdated = @DateUpdated
                            WHERE CollectionSlug = @CollectionSlug";

            try
            {
                using (_connection)
                {
                    _connection.Open();

                    var result = await _connection.ExecuteAsync(sql, entity);

                    isUpdated = result > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
            }

            return isUpdated;
        }
    }
}
