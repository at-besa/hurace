﻿using Microsoft.Extensions.Configuration;

namespace Hurace.Core.DAL.Common
{
  public static class ConfigurationUtil
  {
    private static IConfiguration configuration;

    public static IConfiguration GetConfiguration() =>
      configuration ??= new ConfigurationBuilder()
        .AddJsonFile("Client/appsettings.json", false)
        .Build();

    public static (string connectionString, string providerName) GetConnectionParameters(string configName)
    {
      var connectionConfig = GetConfiguration().GetSection("ConnectionStrings").GetSection(configName);
      return (connectionConfig["ConnectionString"], connectionConfig["ProviderName"]);
    }
  }
}
