﻿using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Threading.Tasks;

namespace Hurace.Dal.Common
{
    public class DefaultConnectionFactory : IConnectionFactory
    {
        private DbProviderFactory dbProviderFactory;

        public static IConnectionFactory FromConfiguration(IConfiguration config, string connectionStringConfigName)
        {
            var connectionConfig = config.GetSection("ConnectionStrings").GetSection(connectionStringConfigName);
            string connectionString = connectionConfig["ConnectionString"];
            string providerName = connectionConfig["ProviderName"];

            return new DefaultConnectionFactory(connectionString, providerName);
        }

        public DefaultConnectionFactory(string connectionString, string providerName)
        {
            this.ConnectionString = connectionString;
            this.ProviderName = providerName;


#if NET_STANDARD_2_1
      DbUtil.RegisterAdoProviders();
      this.dbProviderFactory = DbProviderFactories.GetFactory(providerName);
#else
            this.dbProviderFactory = DbUtil.GetDbProviderFactory(providerName);
#endif
        }

        public string ConnectionString { get; }

        public string ProviderName { get; }

        public DbConnection CreateConnection()
        {
            var connection = dbProviderFactory.CreateConnection();
            connection.ConnectionString = this.ConnectionString;
            connection.Open();
            return connection;
        }

        public async Task<DbConnection> CreateConnectionAsync()
        {
            var connection = dbProviderFactory.CreateConnection();
            connection.ConnectionString = this.ConnectionString;
            await connection.OpenAsync();
            return connection;
        }
    }
}
