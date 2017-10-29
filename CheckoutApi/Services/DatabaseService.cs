using CheckoutApi.Abstract;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace CheckoutApi.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IConfigurationRoot _configuration;

        public DatabaseService(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public DbConnection GetDbConnection()
        {
            return new SqliteConnection(_configuration.GetConnectionString("Database"));
        }
    }
}
